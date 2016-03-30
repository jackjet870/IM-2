using IM.Model;
using IM.Model.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRGuestsLogs
  {
    #region SetLogGuest
    public static void SetLogGuest(int Guest, short HoursDif, string ChangedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_SaveGuestLog(Guest, HoursDif, ChangedBy);
      }
    } 
    #endregion

    #region GetGuestLog

    /// <summary>
    /// Obtiene log del Guest Ingresado
    /// </summary>
    /// <param name="IdGuest">Id del Guest</param>
    /// <history>[jorcanche] 09/03/2016</history>
    public static List<GuestLogData> GetGuestLog(int IdGuest)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGuestLog(IdGuest).ToList();
      }
    }
    #endregion

    #region SaveGuestLog

    /// <summary>
    /// Guarda los cambios del log del Guest
    /// </summary>
    ///<param name="IdGuest"></param>
    /// <param name="changedBy"></param>
    /// <param name="changedBy"></param>
    /// <history>[jorcanche] 11/03/2016</history>
    public static void SaveGuestLog(int IdGuest, short lsHoursDif, string changedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_SaveGuestLog(IdGuest, lsHoursDif, changedBy);
      }
    }
    #endregion

  }

}
