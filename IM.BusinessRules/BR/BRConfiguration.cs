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
  }
}
