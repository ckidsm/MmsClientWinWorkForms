
using MmsClientWinForms.Enums;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;
using WsGatewayWorker.Models;
using WsGatewayWorker.State;
using WsGatewayWorker.Utils;

namespace WsGatewayWorker.Services
{
   public class WebSocketRouter
   {
      private readonly UnitController _unitController;

      public WebSocketRouter(UnitController unitController)
      {
         _unitController = unitController;
      }

      public async Task RouteAsync(IncomingWsMessage message)
      {
         string cmd = message.Content.GetProperty("cmd").GetString() ?? "";
         var cmdData = message.Content.TryGetProperty("cmdData", out var d) ? d : default;

         switch (cmd)
         {
            case "identify":
               await HandleIdentifyAsync(message, cmdData);
               break;

            case "jam":
               await HandleJamAsync(message);
               break;

            default:
               await _unitController.EnqueueMessageAsync(message);
               await SendAsync(message.Client, new { status = "received", cmd });
               break;
         }
      }

      private async Task HandleIdentifyAsync(IncomingWsMessage msg, JsonElement cmdData)
      {
         var deviceId = cmdData.GetString();
         if (string.IsNullOrWhiteSpace(deviceId))
         {
            await SendAsync(msg.Client, new { status = "error", message = "Missing device_id." });
            return;
         }

         var device = _unitController.GetDevice(deviceId);
         if (device == null)
         {
            await SendAsync(msg.Client, new { status = "error", message = $"Unregistered device: {deviceId}" });
            return;
         }

         device.WebSocket = msg.Client;
         await SendAsync(msg.Client, new { status = "ok", cmd = "identified", device_id = deviceId });

         await _unitController.EnqueueMessageAsync(new IncomingWsMessage
         {
            Source = "identify",
            DeviceId = deviceId,
            Client = msg.Client,
            Content = msg.Content
         });
      }

      private async Task HandleJamAsync(IncomingWsMessage msg)
      {
         var device = _unitController.GetDevice(msg.DeviceId);
         if (device != null)
         {
            device.State = DeviceState.DS_PAUSED;
            device.JamCount++;
            await SendAsync(msg.Client, new { status = "ok", message = "jam reported" });
         }
      }

      private static async Task SendAsync(WebSocket socket, object obj)
      {
         string json = JsonSerializer.Serialize(obj);
         byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
         await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
      }
   }
}
