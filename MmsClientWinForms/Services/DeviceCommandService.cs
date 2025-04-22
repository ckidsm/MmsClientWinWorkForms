using MmsClientWinForms.Models;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MmsClientWinForms.Services
{
   public class DeviceCommandService
   {
      private readonly ClientWebSocket? _webSocket;
      private Device? device;

      public DeviceCommandService(ClientWebSocket webSocket)
      {
         _webSocket = webSocket ?? throw new ArgumentNullException(nameof(webSocket));
      }

      public DeviceCommandService(Device device)
      {
         this.device = device ?? throw new ArgumentNullException(nameof(device));
      }

      private async Task SendCommandAsync(string command)
      {
         if (_webSocket?.State == WebSocketState.Open) // Added null check for _webSocket
         {
            var json = JsonSerializer.Serialize(new { cmd = command });
            var buffer = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(buffer);

            await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
         }
         else
         {
            Console.WriteLine("[WARN] WebSocket이 열려 있지 않습니다. 명령을 전송할 수 없습니다.");
         }
      }

      public Task CommenceFeedingAsync() => SendCommandAsync("commence");

      public Task PauseFeedingAsync() => SendCommandAsync("pause");

      public Task ResumeFeedingAsync() => SendCommandAsync("resume");

      public Task ConcludeFeedingAsync() => SendCommandAsync("conclude");
   }
}
