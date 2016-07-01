using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public static class BRGraph
  {
    #region GetGraphProductionByPR

    /// <summary>
    /// Devuelve los datos para la grafica de produccion de PR
    /// </summary>
    /// <param name="dateFrom">Fecha desde</param>
    /// <param name="dateTo">Fecha hasta</param>
    /// <param name="leadSource">Clave de Lead Source</param>
    /// <returns><list type="GraphProductionByPRData"></list></returns>
    /// <history>
    /// [aalcocer] 07/03/2016 Created
    /// [aalcocer] 07/06/2016 Modified. Se agregó asincronía
    /// </history>
    public static async Task<List<GraphProductionByPR>> GetGraphProductionByPR(DateTime dateFrom, DateTime dateTo, string leadSource)
    {
      var listGraphProductionByPr = new List<GraphProductionByPR>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GraphProductionByPR_Timeout;
          listGraphProductionByPr = dbContext.USP_OR_GraphProductionByPR(dateFrom, dateTo, leadSource).OrderBy(gp => gp.PR).ToList();
        }
      });
      return listGraphProductionByPr;
    }

    #endregion GetGraphProductionByPR
  }
}