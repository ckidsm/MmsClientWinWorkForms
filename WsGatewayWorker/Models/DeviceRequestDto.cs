
namespace WsGatewayWorker.Models
{
    public class DeviceRequestDto
    {
        public string DeviceId { get; set; } = "";
        public string? Status { get; set; }
        public string? Passed { get; set; }
        public string? Name { get; set; }
        public int ScanCnt { get; set; }
        public int ErrorCnt { get; set; }
        public string? MonitorId { get; set; }
        public string? Location { get; set; }
    }
}
