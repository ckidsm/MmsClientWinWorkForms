
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WsGatewayWorker.Services
{
   public class WsConnectionManager
   {
      private readonly ConcurrentDictionary<string, WebSocket> _clients = new();

      public bool Add(string deviceId, WebSocket socket)
      {
         if (_clients.TryGetValue(deviceId, out var oldSocket) && oldSocket != socket)
         {
            oldSocket.Abort(); // or CloseAsync if graceful
         }
         _clients[deviceId] = socket;
         return true;
      }

      public bool TryGet(string deviceId, out WebSocket? socket) => _clients.TryGetValue(deviceId, out socket);

      public bool Remove(string deviceId)
      {
         return _clients.TryRemove(deviceId, out _);
      }
   }
}
