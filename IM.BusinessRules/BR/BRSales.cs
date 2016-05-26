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

    #region GetSalesShort
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guest"></param>
    /// <param name="sale"></param>
    /// <param name="membership"></param>
    /// <param name="name"></param>
    /// <param name="leadSource"></param>
    /// <param name="salesRoom"></param>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <returns></returns>
    public static List<SaleShort> GetSalesShort(int guest = 0, int sale = 0, string membership = "ALL", string name = "ALL", string leadSource = "ALL", string salesRoom = "ALL", DateTime? dateFrom = null, DateTime? dateTo = null)
    {      
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetSales_Timeout;
        return dbContext.USP_OR_GetSales(guest, sale, membership, name, leadSource, salesRoom, dateFrom, dateTo).ToList();
      }
    }
    #endregion

    #region DeleteSale
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sale"></param>
    public static void DeleteSale(int sale)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_DeleteSale(sale);
      }
    }
    #endregion

    #region ValidateSale
    /// <summary>
    /// 
    /// </summary>
    /// <param name="changedBy"></param>
    /// <param name="password"></param>
    /// <param name="sale"></param>
    /// <param name="membershipNumber"></param>
    /// <param name="guest"></param>
    /// <param name="saleType"></param>
    /// <param name="salesRoom"></param>
    /// <param name="location"></param>
    /// <param name="pR1"></param>
    /// <param name="pR2"></param>
    /// <param name="pR3"></param>
    /// <param name="pRCaptain1"></param>
    /// <param name="pRCaptain2"></param>
    /// <param name="pRCaptain3"></param>
    /// <param name="liner1"></param>
    /// <param name="liner2"></param>
    /// <param name="linerCaptain"></param>
    /// <param name="closer1"></param>
    /// <param name="closer2"></param>
    /// <param name="closer3"></param>
    /// <param name="closerCaptain"></param>
    /// <param name="exit1"></param>
    /// <param name="exit2"></param>
    /// <param name="podium"></param>
    /// <param name="vLO"></param>
    /// <returns></returns>
    public static List<ValidationData> ValidateSale(string changedBy, string password, Nullable<int> sale, string membershipNumber, Nullable<int> guest, string saleType, string salesRoom, string location, string pR1, string pR2, string pR3, string pRCaptain1, string pRCaptain2, string pRCaptain3, string liner1, string liner2, string linerCaptain, string closer1, string closer2, string closer3, string closerCaptain, string exit1, string exit2, string podium, string vLO)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_ValidateSale(changedBy, password, sale, membershipNumber, guest, saleType, salesRoom, location, pR1, pR2, pR3, pRCaptain1, pRCaptain2, pRCaptain3, liner1, liner2, linerCaptain, closer1, closer2, closer3, closerCaptain, exit1, exit2, podium, vLO).ToList();
      }
    }
    #endregion

    #region UpdateGuestSalesmen
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guId"></param>
    /// <param name="saleID"></param>
    public static void UpdateGuestSalesmen(int guId, int saleID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_UpdateGuestSalesmen(guId, saleID);
      }
    }
    #endregion

    #region UpdateSaleUpdated
    /// <summary>
    /// 
    /// </summary>
    /// <param name="saleId"></param>
    /// <param name="updated"></param>
    public static void UpdateSaleUpdated(int saleId, bool updated)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_UpdateSaleUpdated(saleId, updated);
      }
    }
    #endregion

    #region UpdateGuestSale
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guestId"></param>
    /// <param name="sale"></param>
    public static void UpdateGuestSale(int guestId, bool sale)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_UpdateGuestSale(guestId, sale);
      }
    }
    #endregion

    #region SaveSalesmenChanges
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guestId"></param>
    /// <param name="sale"></param>
    public static void SaveSalesmenChanges(int guestId, bool sale)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_UpdateGuestSale(guestId, sale);
      }
    }
    #endregion

    #region SaveSaleLog
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sale"></param>
    /// <param name="hoursDif"></param>
    /// <param name="changedBy"></param>
    public static void SaveSaleLog(int sale, short hoursDif, string changedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_SaveSaleLog(sale, hoursDif, changedBy);
      }
    }
    #endregion

    #region GetSalesCloseD
    /// <summary>
    /// Obtiene la fecha de cierre de las ventas
    /// </summary>
    /// <param name="srId">ID Sales Room</param>
    /// <returns>La fecha dse cierre DateTime</returns>
    /// <hitory>
    /// [jorcanche] 20/05/2016
    /// </hitory>
    public static DateTime GetSalesCloseD(string srId)
    {
      using (var dbCOntext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from gu in dbCOntext.SalesRooms where gu.srID == srId select gu.srSalesCloseD).FirstOrDefault();
      }
    }
    #endregion

    #region GetSalesbyID
    /// <summary>
    /// Obtiene la venta por ID
    /// </summary>
    /// <param name="srId">ID Sales Room</param>
    /// <returns>La fecha dse cierre DateTime</returns>
    /// <hitory>
    /// [jorcanche] 20/05/2016
    /// </hitory>
    public static Sale GetSalesbyID(int saId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from gu in dbContext.Sales where gu.saID == saId select gu).FirstOrDefault();
      }
    }
    #endregion

    #region GetSalesTypes
    /// <summary>
    /// Obtiene los Tipos de ventas activos por ID
    /// </summary>
    /// <param name="stId"></param>
    /// <returns>Una lista de Tipo de ventas</returns>
    /// <hitory>
    /// [jorcanche] 20/05/2016
    /// </hitory>
    public static List<SaleType> GetSalesTypes(string stID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from gu in dbContext.SaleTypes
                where gu.stA == true && gu.stID == stID
                select gu).ToList();
      }
    } 
    #endregion
  }
}
