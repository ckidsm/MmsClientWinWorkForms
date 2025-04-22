using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MmsClientWinForms.Services
{
   public class WebSocketClient
   {
      private ClientWebSocket? _client;
      private CancellationTokenSource? _cts;
      private readonly string _uri;

      public event Action<JsonElement>? OnMessageReceived;

      public WebSocketClient(string uri)
      {
         _uri = uri;
      }

      public void Connect()
      {
         _ = Task.Run(async () => await RunClientAsync());
      }

      private async Task RunClientAsync()
      {
         while (true)
         {
            try
            {
               _client = new ClientWebSocket();
               _cts = new CancellationTokenSource();
               await _client.ConnectAsync(new Uri(_uri), _cts.Token);

               Console.WriteLine($"[WS] Connected to {_uri}");

               var buffer = new byte[4096];
               while (_client.State == WebSocketState.Open)
               {
                  var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                  var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                  try
                  {
                     var doc = JsonDocument.Parse(json);
                     OnMessageReceived?.Invoke(doc.RootElement);
                  }
                  catch (Exception ex)
                  {
                     Console.WriteLine($"[WS] Invalid JSON: {ex.Message}");
                  }
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine($"[WS] Connection failed: {ex.Message}");
               await Task.Delay(5000);
            }
         }
      }

      public async Task SendAsync(object payload)
      {
         if (_client?.State == WebSocketState.Open && _cts != null)
         {
            string json = JsonSerializer.Serialize(payload);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            await _client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, _cts.Token);
         }
      }

      public void Close()
      {
         try
         {
            _client?.Abort();
            _cts?.Cancel();
         }
         catch { }
      }
   }
}