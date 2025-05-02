namespace MmsClientWinForms.Models
{
   public class ProductSerialResponse
   {
      public string Status { get; set; } = string.Empty;
      public string Message { get; set; } = string.Empty;
      public ProductSerialData? Data { get; set; }
   }

   public class ProductSerialData
   {
      public string ProductSerial { get; set; } = string.Empty;
      public string QrContent { get; set; } = string.Empty;
      public string Type { get; set; } = string.Empty;
      public string? Local { get; set; }
      public string MacAddress { get; set; } = string.Empty;
      public string BoardSerial { get; set; } = string.Empty;
   }
}
