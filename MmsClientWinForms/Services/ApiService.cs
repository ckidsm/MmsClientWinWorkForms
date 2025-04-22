// Services/ApiService.cs
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MmsClientWinForms.Models;

namespace MmsClientWinForms.Services
{
   public class ApiService
   {
      private static readonly HttpClient _client = new();

      public async Task<bool> RegisterDevice(string mac, string line, string pos)
      {
         var payload = new RegisterRequest
         {
            MacAddress = mac,
            LineNo = line,
            Position = pos
         };

         var json = JsonSerializer.Serialize(payload);
         var content = new StringContent(json, Encoding.UTF8, "application/json");

         var response = await _client.PostAsync(SettingsService.Current.ServerUrl + "/api/product/add", content);
         return response.IsSuccessStatusCode;
      }

      public static async Task ReportDeviceResult(string serverUrl, string unitId, Dictionary<string, object> deviceData)
      {
         var payload = new
         {
            unit_id = unitId,
            device = deviceData
         };

         var json = JsonSerializer.Serialize(payload);
         var content = new StringContent(json, Encoding.UTF8, "application/json");
         var url = $"{serverUrl}/api/device/report";

         try
         {
            var response = await _client.PostAsync(url, content);
            Console.WriteLine($"[HTTP] Report status: {response.StatusCode}");
         }
         catch (Exception ex)
         {
            Console.WriteLine($"[HTTP] 보고 실패: {ex.Message}");
         }
      }
   }
}