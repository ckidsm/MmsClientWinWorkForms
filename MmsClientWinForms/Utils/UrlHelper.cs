using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmsClientWinForms.Utils
{
   // Utils/UrlHelper.cs
   using System;
   using System.Collections.Generic;
   using System.Linq;

   namespace MmsClientWinForms.Utils
   {
      public static class UrlHelper
      {
         public static string AddQueryString(string baseUrl, Dictionary<string, string> parameters)
         {
            if (string.IsNullOrWhiteSpace(baseUrl)) return string.Empty;
            if (parameters == null || parameters.Count == 0) return baseUrl;

            var query = string.Join("&", parameters.Select(kvp =>
                $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

            return baseUrl.Contains("?") ? $"{baseUrl}&{query}" : $"{baseUrl}?{query}";
         }
      }
   }

}
