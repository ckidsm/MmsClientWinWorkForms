using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Collections.Concurrent;
using MmsClientWinForms.Services;
using MmsClientWinForms.Enums;
using MmsClientWinForms.Models;
using MmsClientWinForms.Utils;
using MmsClientWinForms.State;
using Microsoft.AspNetCore.Http;

namespace MmsClientWinForms.Services
{
   public class WebSocketMiddleware
   {
      private readonly RequestDelegate _next;
      private static readonly ConcurrentDictionary<string, WebSocket> _clients = new(); // deviceId → WebSocket 매핑
      private readonly UnitController _unitController;

      public WebSocketMiddleware(RequestDelegate next, UnitController unitController)
      {
         _next = next;
         _unitController = unitController;
      }

      /// <summary>
      /// WebSocket 요청이 들어오면 처리하고, 아니면 다음 미들웨어로 전달
      /// </summary>
      public async Task InvokeAsync(HttpContext context)
      {
         if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
         {
            using WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
            await HandleWebSocket(ws); // WebSocket 연결 처리
         }
         else
         {
            await _next(context); // WebSocket이 아니면 다음 미들웨어로 이동
         }
      }

      /// <summary>
      /// WebSocket 연결을 통해 메시지를 수신하고 처리
      /// </summary>
      private async Task HandleWebSocket(WebSocket ws)
      {
         string? deviceId = null;
         var buffer = new byte[4096];

         try
         {
            while (ws.State == WebSocketState.Open)
            {
               var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
               string jsonText = Encoding.UTF8.GetString(buffer, 0, result.Count);
               Console.WriteLine($"[WS] Received: {jsonText}");

               JsonDocument doc;
               JsonElement root;

               try
               {
                  doc = JsonDocument.Parse(jsonText);
                  root = doc.RootElement;
               }
               catch (JsonException)
               {
                  await SendJson(ws, new { error = "Invalid JSON format." });
                  continue;
               }

               string cmd = root.GetProperty("cmd").GetString() ?? "";
               string? cmdData = root.TryGetProperty("cmdData", out var cmdVal) ? cmdVal.GetString() : null;

               // identify 처리
               if (cmd == "identify")
               {
                  deviceId = cmdData;
                  if (string.IsNullOrWhiteSpace(deviceId))
                  {
                     await SendJson(ws, new { status = "error", message = "Missing device_id." });
                     continue;
                  }

                  var device = _unitController.GetDevice(deviceId);
                  if (device == null)
                  {
                     await SendJson(ws, new { status = "error", message = $"Unregistered device: {deviceId}" });
                     await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Unregistered", CancellationToken.None);
                     break;
                  }

                  if (_clients.TryGetValue(deviceId, out var existingWs) && existingWs != ws)
                  {
                     await existingWs.CloseAsync(WebSocketCloseStatus.NormalClosure, "Replaced", CancellationToken.None);
                  }

                  _clients[deviceId] = ws;
                  device.WebSocket = (ClientWebSocket?)ws;

                  await SendJson(ws, new { status = "ok", cmd = "identified", device_id = deviceId });

                  await _unitController.EnqueueMessageAsync(new
                  {
                     source = "identify",
                     device_id = deviceId,
                     client = ws
                  });

                  var pause = (_unitController.State == UnitState.PAUSED && device.State == DeviceState.DS_FEEDING);
                  await SendJson(ws, new { cmd = pause ? "pause" : "commence" });
               }
               // jam 처리
               else if (cmd == "jam")
               {
                  if (string.IsNullOrWhiteSpace(deviceId))
                  {
                     await SendJson(ws, new { error = "Unknown device_id for jam event." });
                     continue;
                  }

                  var device = _unitController.GetDevice(deviceId);
                  if (device == null)
                  {
                     await SendJson(ws, new { error = $"Device {deviceId} not found for jam event." });
                     continue;
                  }

                  device.State = DeviceState.DS_PAUSED;
                  device.JamCount++;

                  var resultData = device.ToDictionary();
                  resultData["status"] = "jam_detected";

                  await ApiService.ReportDeviceResult(_unitController.MasterServerUrl, _unitController.UnitId, resultData);
                  await SendJson(ws, new { status = "ok", cmd = "jam_reported", device_id = deviceId });
               }
               // 기타 명령
               else
               {
                  if (string.IsNullOrWhiteSpace(deviceId))
                  {
                     await SendJson(ws, new { error = "device_id not set. Please identify first." });
                     continue;
                  }

                  await _unitController.EnqueueMessageAsync(new
                  {
                     source = "ws_client",
                     device_id = deviceId,
                     content = root,
                     client = ws
                  });

                  await SendJson(ws, new { status = "received", cmd });
               }
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine($"[WS] Error: {ex.Message}");
         }
         finally
         {
            if (!string.IsNullOrWhiteSpace(deviceId) && _clients.TryRemove(deviceId, out _))
            {
               var device = _unitController.GetDevice(deviceId);
               if (device != null && device.WebSocket == ws)
               {
                  device.WebSocket = null;
                  device.State = DeviceState.DS_DISCONNECTED;

                  await ApiService.ReportDeviceResult(_unitController.MasterServerUrl, _unitController.UnitId, device.ToDictionary());

                  await _unitController.EnqueueMessageAsync(new
                  {
                     source = "ws_client",
                     device_id = deviceId,
                     content = new { cmd = "disconnected" },
                     client = ws
                  });
               }
            }
         }
      }


      /// <summary>
      /// WebSocket으로 JSON 데이터를 전송
      /// </summary>
      private static async Task SendJson(WebSocket ws, object data)
      {
         var json = JsonSerializer.Serialize(data);
         var bytes = Encoding.UTF8.GetBytes(json);
         await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
      }
   }
}
