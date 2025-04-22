using System.Text.Json.Serialization;

namespace WsGatewayWorker.Services
{
   public class Settings
   {
      [JsonPropertyName("master_server")]
      public string MasterServer { get; set; } = "http://localhost:5000";

      [JsonPropertyName("local_server")]
      public string ServerUrl { get; set; } = "http://localhost:8080";

      [JsonPropertyName("line_number")]
      public string LineNo { get; set; } = "1";

      [JsonPropertyName("device_count_max")]
      public int DeviceCountMax { get; set; } = 40;

      [JsonPropertyName("device_type")]
      public string DeviceType { get; set; } = "PCOS";

      [JsonPropertyName("external_ws_uri")]
      public string ExternalWsUri { get; set; } = "ws://localhost:5000/ws";

      [JsonPropertyName("serial_port")]
      public string SerialPort { get; set; } = "COM1";

      //이하는 내부에서만 사용하는 경우 JSON에 저장하지 않아도 무방
      public int SerialBaudRate { get; set; } = 9600;
      public string DeviceId { get; set; } = "DEVICE001";
      public string AndroidBoardNo { get; set; } = "ANDROID001";
      public string MacAddress { get; set; } = "00:00:00:00:00:00";
      public string Position { get; set; } = "1";
      public string Status { get; set; } = "Idle";
      public int SheetCount { get; set; } = 0;
      public int JamCount { get; set; } = 0;
      public int RcgFailCount { get; set; } = 0;
   }
}
