// Models/RegisterRequest.cs
namespace MmsClientWinForms.Models
{
   public class RegisterRequest
   {
      public string MacAddress { get; set; } = string.Empty;
      public string LineNo { get; set; } = string.Empty;
      public string Position { get; set; } = string.Empty;
   }
}
