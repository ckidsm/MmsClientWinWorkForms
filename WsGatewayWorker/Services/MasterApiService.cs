
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WsGatewayWorker.Services
{
    public class MasterApiService
    {
        private readonly HttpClient _httpClient;

        public MasterApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> PostDeviceResultAsync(string unitId, Dictionary<string, object> deviceData)
        {
            var payload = new
            {
                unit_id = unitId,
                device = deviceData
            };

            var response = await _httpClient.PostAsJsonAsync("/api/acmqa/master/device_updated", payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UploadZipFileAsync(string deviceId, string zipFilePath)
        {
            if (!File.Exists(zipFilePath)) return false;

            var content = new MultipartFormDataContent();
            var fileBytes = await File.ReadAllBytesAsync(zipFilePath);
            var fileContent = new ByteArrayContent(fileBytes);
            content.Add(fileContent, "zipped_img", Path.GetFileName(zipFilePath));
            content.Add(new StringContent(deviceId), "device_id");

            var response = await _httpClient.PostAsync("/api/DeviceCheck/upload-zip", content);
            return response.IsSuccessStatusCode;
        }
    }
}
