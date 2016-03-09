using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGuestsLogs
  {
    public static void SetLogGuest(int Guest, short HoursDif, string ChangedBy)
    {
      using (var dbContext = new Model.IMEntities())
      {
         dbContext.USP_OR_SaveGuestLog(Guest, HoursDif, ChangedBy);  
      }
    }
  }
}
