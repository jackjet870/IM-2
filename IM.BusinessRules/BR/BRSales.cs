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
    /// <param name="searchBySalePR">True SalePR - False ContactsPR</param>
    /// <returns>List<SaleByPR></returns>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    public static List<SaleByPR> GetSalesByPR(DateTime dateFrom, DateTime dateTo, string leadSources, string PR, bool searchBySalePR)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetSalesByPR(dateFrom, dateTo, leadSources, PR, searchBySalePR).ToList();
      }
    }

    #endregion

    #region GetSalesByLiner
    /// <summary>
    /// Obtiene Guests por PR
    /// </summary>
    /// <param name="dateFrom">FechaInicio</param>
    /// <param name="dateTo">FechaFin</param>
    /// <param name="salesRooms">Sales Room</param>
    /// <param name="Liner">liner</param>
    /// <returns>List<SaleByLiner></returns>
    /// <history>
    /// [erosado] 23/Mar/2016 Created
    /// </history>
    public static List<SaleByLiner> GetSalesByLiner(DateTime dateFrom, DateTime dateTo, string salesRooms, string Liner)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetSalesByLiner(dateFrom, dateTo, salesRooms, Liner).ToList();

      }
    }

    #endregion

    #region GetSalesByCloser
    /// <summary>
    /// Obtiene Guests por PR
    /// </summary>
    /// <param name="dateFrom">FechaInicio</param>
    /// <param name="dateTo">FechaFin</param>
    /// <param name="salesRooms">Sales Room</param>
    /// <param name="Closer">closer</param>
    /// <returns>List<SaleByCloser></returns>
    /// <history>
    /// [erosado] 24/Mar/2016 Created
    /// </history>
    public static List<SaleByCloser> GetSalesByCloser(DateTime dateFrom, DateTime dateTo, string salesRooms, string closer)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetSalesByCloser(dateFrom, dateTo, salesRooms, closer).ToList();

      }
    }

    #endregion

  }
}
