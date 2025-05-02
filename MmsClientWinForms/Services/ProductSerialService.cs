using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MmsClientWinForms.Models;

namespace MmsClientWinForms.Services
{
   public class ProductSerialService
   {
      private readonly HttpClient _client;

      public ProductSerialService(IHttpClientFactory factory)
      {
         _client = factory.CreateClient("ProductSerial");
      }

      public async Task<ProductSerialResponse?> IssueSerialAsync(ProductSerialRequest request)
      {
         var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
         {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
         });

         var content = new StringContent(json, Encoding.UTF8, "application/json");
         var response = await _client.PostAsync("/api/ProductSerial/issueProductSerial", content);

         if (!response.IsSuccessStatusCode)
            return new ProductSerialResponse { Status = "FAIL", Message = $"HTTP 오류: {response.StatusCode}" };

         var resultJson = await response.Content.ReadAsStringAsync();
         return JsonSerializer.Deserialize<ProductSerialResponse>(resultJson, new JsonSerializerOptions
         {
            PropertyNameCaseInsensitive = true
         });
      }
   }
}
