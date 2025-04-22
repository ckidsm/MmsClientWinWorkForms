
namespace WsGatewayWorker.Models
{
   using System.Net.WebSockets;
   using System.Text.Json;

   public class IncomingWsMessage
   {
      public string Source { get; set; } = "ws_client";
      public string DeviceId { get; set; } = string.Empty;
      public JsonElement Content { get; set; }
      public WebSocket Client { get; set; } = default!;
   }
}
