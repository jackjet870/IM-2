using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRConfiguration
  {
    #region GetCloseDate

    /// <summary>
    /// Obtiene la fecha de cierre
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// </history>
    public static DateTime? GetCloseDate()
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.Configurations.Single().ocInvitationsCloseD;
      }
    }
    #endregion

    #region GetConfigurations
    /// <summary>
    ///  Obtiene el unico registro de configuracion
    /// </summary>
    /// <history>
    ///   [vku] 13/Jun/2016 Created 
    /// </history>
    public async static Task<List<Configuration>> GetConfigurations()
    {
      List<Configuration> lstConfigurations = new List<Configuration>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from configs in dbContext.Configurations
                      select configs;
          lstConfigurations = query.ToList();
        }
      });
      return lstConfigurations;
    }
    #endregion

    #region GetTourTimesSchema
    /// <summary>
    ///   Obtiene el esquema de horarios de tour
    /// </summary>
    /// <history>
    ///   [vku] 23/Jun/2016 Created
    /// </history>
    public async static Task<int> GetTourTimesSchema()
    {
      int tt = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          tt = dbContext.Configurations.Single().ocTourTimesSchema;
        }
      });
      return tt;
    }
    #endregion

    #region SaveCloseDate
    /// <summary>
    ///   Guarda la fecha de cierre de invitaciones
    /// </summary>
    /// <param name="dtmCloseDate">Fecha de cierre de invitaciones</param>
    /// <history>
    ///   [vku] 16/Jun/2016 Created
    /// </history>
    public async static Task<int> SaveCloseDate(DateTime dtmCloseDate)
    {
      int res = 0;
      res = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var invitationClose = dbContext.Configurations;
          invitationClose.Single().ocInvitationsCloseD = dtmCloseDate;
          return dbContext.SaveChanges();
        }
      });
      return res;
    }
    #endregion
  }
}
