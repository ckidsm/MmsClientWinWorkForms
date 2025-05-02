using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmsClientWinForms.Models
{
   public class ProductSerialRequest
   {
      public string Type { get; set; } = string.Empty;
      public string? Local { get; set; }
      public string BoardSerial { get; set; } = string.Empty;
      public string MacAddress { get; set; } = string.Empty;
   }
}
