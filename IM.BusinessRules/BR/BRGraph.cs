using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRGraph
  {
    #region GetGraphProductionByPR

    /// <summary>
    /// Devuelve los datos para la grafica de produccion de PR
    /// </summary>
    /// <param name="dateFrom">Fecha desde</param>
    /// <param name="dateTo">Fecha hasta</param>
    /// <param name="leadSource">Clave de Lead Source</param>
    /// <returns>List<GraphProductionByPRData></returns>
    /// <history>
    /// [aalcocer] 07/03/2016 Created
    /// </history>
    public static List<GraphProductionByPR> GetGraphProductionByPR(DateTime dateFrom, DateTime dateTo, string leadSource)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GraphProductionByPR(dateFrom, dateTo, leadSource).OrderBy(gp => gp.PR).ToList();
      }
    }

    #endregion GetGraphProductionByPR
  }
}