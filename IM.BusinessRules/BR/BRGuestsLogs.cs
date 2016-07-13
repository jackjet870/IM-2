using IM.Model;
using IM.Model.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGuestsLogs
  {
    #region SetLogGuest
    /// <summary>
    /// Guarda el log de un guest
    /// </summary>
    /// <param name="Guest">Id del Guest</param>
    /// <param name="HoursDif"></param>
    /// <param name="ChangedBy"></param>
    /// <history>
    /// [jorcanche] 09/03/2016
    /// </history>
    public static async Task<int> SetLogGuest(int Guest, short HoursDif, string ChangedBy)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
         return  dbContext.USP_OR_SaveGuestLog(Guest, HoursDif, ChangedBy);
        }
      });
    } 
    #endregion

    #region GetGuestLog

    /// <summary>
    /// Obtiene log del Guest Ingresado
    /// </summary>
    /// <param name="idGuest">Id del Guest</param>
    /// <history>
    /// [jorcanche] 09/03/2016
    /// </history>
    public static async Task<List<GuestLogData>> GetGuestLog(int idGuest)
    {  
     return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_GetGuestLog(idGuest).ToList();
        }
      });
    }
    #endregion

    #region SaveGuestLog

    ///  <summary>
    ///  Guarda los cambios del log del Guest
    ///  </summary>
    /// <param name="idGuest"></param>
    /// <param name="lsHoursDif"></param>
    /// <param name="changedBy"></param>
    /// <history>
    ///  [jorcanche] 11/03/2016
    ///  </history>
    public static async Task<int> SaveGuestLog(int idGuest, short lsHoursDif, string changedBy)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
         return dbContext.USP_OR_SaveGuestLog(idGuest, lsHoursDif, changedBy);
        }
      });
    }
    #endregion
  }

}
