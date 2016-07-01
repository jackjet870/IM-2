﻿using IM.Model;
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
    public static void SetLogGuest(int Guest, short HoursDif, string ChangedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
    /// <history>
    /// [jorcanche] 09/03/2016
    /// </history>
    public async static Task<List<GuestLogData>> GetGuestLog(int IdGuest)
    {
      List<GuestLogData> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          result = dbContext.USP_OR_GetGuestLog(IdGuest).ToList();
        }
      });
      return result;
    }
    #endregion

    #region SaveGuestLog

    /// <summary>
    /// Guarda los cambios del log del Guest
    /// </summary>
    ///<param name="IdGuest"></param>
    /// <param name="changedBy"></param>
    /// <param name="changedBy"></param>
    /// <history>
    /// [jorcanche] 11/03/2016
    /// </history>
    public static void SaveGuestLog(int IdGuest, short lsHoursDif, string changedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.USP_OR_SaveGuestLog(IdGuest, lsHoursDif, changedBy);
      }
    }
    #endregion

  }

}
