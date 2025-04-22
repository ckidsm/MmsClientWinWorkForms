
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using WsGatewayWorker.Models;
using WsGatewayWorker.Services;

namespace WsGatewayWorker
{
   public class WebSocketMiddleware
   {
      private readonly RequestDelegate _next;
      private readonly WebSocketRouter _router;
      private readonly WsConnectionManager _connectionManager;

      public WebSocketMiddleware(RequestDelegate next, WebSocketRouter router, WsConnectionManager connectionManager)
      {
         _next = next;
         _router = router;
         _connectionManager = connectionManager;
      }

      public async Task InvokeAsync(HttpContext context)
      {
         if (!context.WebSockets.IsWebSocketRequest)
         {
            await _next(context);
            return;
         }

         using var socket = await context.WebSockets.AcceptWebSocketAsync();
         Console.WriteLine("[WS] WebSocket 연결 수락됨");

         string? deviceId = null;
         var buffer = new byte[4096];

         try
         {
            while (socket.State == WebSocketState.Open)
            {
               var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
               if (result.MessageType == WebSocketMessageType.Close)
               {
                  Console.WriteLine($"[WS] 클라이언트가 연결 종료 요청함");
                  break;
               }

               var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
               Console.WriteLine($"[WS] 수신 데이터: {json}");

               try
               {
                  var doc = JsonDocument.Parse(json);
                  var root = doc.RootElement;

                  deviceId = root.TryGetProperty("cmdData", out var d) && d.ValueKind == JsonValueKind.String
                      ? d.GetString()
                      : root.TryGetProperty("deviceId", out var dev) ? dev.GetString() : null;

                  if (!string.IsNullOrWhiteSpace(deviceId))
                  {
                     _connectionManager.Add(deviceId, socket);
                  }

                  var message = new IncomingWsMessage
                  {
                     Client = socket,
                     DeviceId = deviceId ?? "",
                     Content = root,
                     Source = "ws_client"
                  };

                  await _router.RouteAsync(message);
               }
               catch (Exception ex)
               {
                  Console.WriteLine($"[WS] JSON 처리 오류: {ex.Message}");
                  await SendJson(socket, new { error = "Invalid JSON" });
               }
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine($"[WS] 연결 중 예외 발생: {ex.Message}");
         }
         finally
         {
            if (!string.IsNullOrWhiteSpace(deviceId))
            {
               _connectionManager.Remove(deviceId);
               Console.WriteLine($"[WS] 연결 제거됨: {deviceId}");
            }
         }
      }

      private static async Task SendJson(WebSocket socket, object data)
      {
         var json = JsonSerializer.Serialize(data);
         var bytes = Encoding.UTF8.GetBytes(json);
         await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
      }
   }
}
