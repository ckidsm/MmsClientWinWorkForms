using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmsClientWinForms.Enums
{

   public enum DeviceState
   {
      DS_IDLE,
      DS_FEEDING_NORMAL,
      DS_FEEDING_PAUSED,
      DS_FEEDING_STOPPED,
      DS_COMPLETED,
      DS_RETIRED,
      DS_DISCONNECTED, //추가
      DS_FEEDING,
      DS_PAUSED
   }


}
