using System.Collections.Generic;
using System.Net.WebSockets;
using MmsClientWinForms.Enums;

namespace WsGatewayWorker.Models
{
   public class Device
   {
      public string? DeviceId { get; set; }
      public required string BoardNo { get; set; }
      public required string MacAddress { get; set; }
      public required string LineNo { get; set; }
      public required string Position { get; set; }
      public required string Status { get; set; }

      public int SheetCount { get; set; } = 0;
      public int JamCount { get; set; } = 0;
      public int RcgFailCount { get; set; } = 0;

      public bool Passed { get; set; } = false;
      public Dictionary<string, object> Steps { get; set; } = new();

      public ClientWebSocket? WebSocket { get; set; }
      public DeviceState State { get; set; } = DeviceState.DS_IDLE;

      public bool IsConnected => !string.IsNullOrEmpty(BoardNo) && WebSocket != null;
      public bool IsRetired => State == DeviceState.DS_RETIRED;
      public bool IsIdle => State == DeviceState.DS_IDLE;
      public bool IsFeeding => State == DeviceState.DS_FEEDING_NORMAL;
      public bool IsPaused => State == DeviceState.DS_FEEDING_PAUSED;
      public bool IsStopped => State == DeviceState.DS_FEEDING_STOPPED;
      public bool IsCompleted => State == DeviceState.DS_COMPLETED;


      public Dictionary<string, object> ToDictionary()
      {
         return new Dictionary<string, object>
         {
            { "DeviceId", this.DeviceId ?? string.Empty },
            { "MacAddress", this.MacAddress ?? string.Empty },
            { "BoardNo", this.BoardNo ?? string.Empty },
            { "LineNo", this.LineNo ?? string.Empty },
            { "Position", this.Position ?? string.Empty },
            { "Status", this.Status ?? string.Empty },
            { "State", this.State.ToString() },
            { "JamCount", this.JamCount }
          };
      }


   }
}

