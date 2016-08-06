using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
   public class BREfficiency
  {
    #region GetEfficiencyByWeeks
    /// <summary>
    /// Obtiene una lista de Efficiency 
    /// </summary>
    /// <param name="sr">Sales Room</param>
    /// <param name="dateFrom">Fecha de inicio </param>
    /// <param name="dateTo">Fecha final</param>
    /// <history>
    /// [ecanul] 30/07/2016 Created
    /// [ecanul] 05/08/2016 Modified. Ahora el return es directo
    /// </history>
    public async static Task<List<EfficiencyData>> GetEfficiencyByWeeks(string sr, DateTime dateFrom, DateTime dateTo)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_IM_GetEfficiencyByWeeks(sr, dateFrom, dateTo).ToList();
        }
      });
    }
    #endregion
  }
}
