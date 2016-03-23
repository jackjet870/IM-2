using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRGuestsLogs
  {
    public static void SetLogGuest(int Guest, short HoursDif, string ChangedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
         dbContext.USP_OR_SaveGuestLog(Guest, HoursDif, ChangedBy);  
      }
    }
  }
}
