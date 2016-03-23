using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRSales
  {
    #region GetSalesByPR
    /// <summary>
    /// Obtiene Guests por PR
    /// </summary>
    /// <param name="dateFrom">FechaInicio</param>
    /// <param name="dateTo">FechaFin</param>
    /// <param name="leadSources">Lead Source</param>
    /// <param name="PR">PR</param>
    /// <returns>List<SaleByPR></returns>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    public static List<SaleByPR> GetSalesByPR(DateTime dateFrom, DateTime dateTo, string leadSources, string PR)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetSalesByPR(dateFrom, dateTo, leadSources, PR).ToList();
        
      }
    }

    #endregion
  }
}
