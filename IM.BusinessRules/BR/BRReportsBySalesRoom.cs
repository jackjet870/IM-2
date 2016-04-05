using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRReportsBySalesRoom
  {
    #region GetRptBookingsBySalesRoomProgramTime
    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de venta y Programa
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="SalesRooms">Salas de Venta</param>
    /// <returns> List<RptBookingsBySalesRoomProgramTime> </returns>
    /// <history>
    /// [edgrodriguez] 12/Mar/2016 Created
    /// </history>
    public static List<RptBookingsBySalesRoomProgramTime> GetRptBookingsBySalesRoomProgramTime(DateTime dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptBookingsBySalesRoomProgramTime(dtmStart, salesRooms).ToList();
      }
    } 
    #endregion

    #region GetRptBookingsBySalesRoomProgramLeadSourceTime
    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de Venta,Programa y Locaciones.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="SalesRooms">Salas de Venta</param>
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptBookingsBySalesRoomProgramLeadSourceTime> GetRptBookingsBySalesRoomProgramLeadSourceTime(DateTime dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptBookingsBySalesRoomProgramLeadSourceTime(dtmStart, salesRooms).ToList();
      }
    } 
    #endregion

    #region GetRptCxC
    /// <summary>
    /// Obtiene el reporte de CxC por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms">Sala de Venta</param>
    /// <returns> List<RptCxC> </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static List<RptCxC> GetRptCxC(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptCxC(dtmStart, dtmEnd, salesRooms).ToList();
      }
    } 
    #endregion

    #region GetRptCxCDeposits
    /// <summary>
    /// Obtiene el reporte de CxCDeposits por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptCxCDeposits> </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static List<object> GetRptCxCDeposits(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstDeposits= dbContext.USP_OR_RptCxCDeposits(dtmStart, dtmEnd, salesRooms).ToList();
        var lstCurrencies = (from cxcDep in lstDeposits
                             join curr in dbContext.Currencies on cxcDep.grcuCxCPRDeposit equals curr.cuID
                             select curr).Distinct().ToList();

        return new List<object> { lstDeposits, lstCurrencies };
      }
    }
    #endregion

    #region GetRptCxCGifts
    /// <summary>
    /// Obtiene el reporte de CxCGifts por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptCxCGifts> </returns>
    /// <history>
    /// [edgrodriguez] 28/Mar/2016 Created
    /// </history>
    public static List<object> GetRptCxCGifts(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        var lstGiftReceipt = dbContext.USP_OR_RptCxCGifts(dtmStart, dtmEnd, salesRooms).ToList();
        var gifts = (from gRcpt in lstGiftReceipt
                     join gift in dbContext.Gifts on gRcpt.Gift equals gift.giID
                     select gift).ToList();

        return new List<object> { lstGiftReceipt, gifts };
      }
    }
    #endregion

    #region GetRptCxCNotAuthorized
    /// <summary>
    /// Obtiene el reporte de CxCNotAuthorized por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptCxCNotAuthorized> </returns>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static List<RptCxCNotAuthorized> GetRptCxCNotAuthorized(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        return dbContext.USP_OR_RptCxCNotAuthorized(dtmStart, dtmEnd, salesRooms).ToList();
      }
    }
    #endregion

    #region GetRptDeposits
    /// <summary>
    /// Obtiene el reporte de Deposits por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<object> </returns>
    /// <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static List<object> GetRptDeposits(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Obtenemos los recibos de regalo.
        var giftReceipts = dbContext.USP_OR_RptDeposits(dtmStart, dtmEnd, salesRooms).ToList();
        var currencies = (from gRcpt in giftReceipts
                          join curr in dbContext.Currencies on gRcpt.grcu equals curr.cuID
                          select curr).Distinct().ToList();

        var paymentsType = (from gRcpt in giftReceipts
                            join payTyp in dbContext.PaymentTypes on gRcpt.grpt equals payTyp.ptID
                            select payTyp).Distinct().ToList();

        return new List<object> { giftReceipts, currencies, paymentsType };
      }
    }
    #endregion

    #region GetRptDepositsBurned
    /// <summary>
    /// Obtiene el reporte de Deposits Burned por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<object> </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static List<object> GetRptDepositsBurned(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      List<string> salesRoom = salesRooms.Split(',').ToList();
      try
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          //Obtenemos los recibos de regalo.
          var giftReceipts = dbContext.USP_OR_RptDepositsBurned(dtmStart, dtmEnd, salesRooms).ToList();

          var currencies = (from gRcpt in giftReceipts
                            join curr in dbContext.Currencies on gRcpt.grcu equals curr.cuID
                            select curr).Distinct().ToList();

          var paymentsType = (from gRcpt in giftReceipts
                              join payTyp in dbContext.PaymentTypes on gRcpt.grpt equals payTyp.ptID
                              select payTyp).Distinct().ToList();

          var saleReceipts = (from gSale in dbContext.Sales
                              join gu in dbContext.Guests on gSale.sagu equals gu.guID
                              join gRctp in dbContext.GiftsReceipts on gu.guID equals gRctp.grgu
                              join per in dbContext.Personnels on gRctp.grpe equals per.peID
                              where salesRoom.Contains(gSale.sasr) &&
                              gSale.saD >= dtmStart && gSale.saD <= dtmEnd &&
                              gRctp.grDepositTwisted > 0
                              select new
                              {
                                gRctp.grID,
                                gSale
                              })
                              .GroupBy(c => c.grID)
                              .ToDictionary(c => c.Key, c => c.Select(s=>s.gSale).ToList());

        return new List<object> { giftReceipts, currencies, paymentsType, saleReceipts };
        }
      }
      catch (Exception ex)
      {
        return null;
      }
      
    }
    #endregion

    #region GetRptDepositsBurnedByResort
    /// <summary>
    /// Obtiene el reporte de CxCNotAuthorized por Sala de ventas y un rango de fechas..
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<object> </returns>
    /// <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static List<object> GetRptDepositsBurnedByResort(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      List<string> salesRoom = salesRooms.Split(',').ToList();
      try
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          //Obtenemos los recibos de regalo.
          var giftReceipts = dbContext.USP_OR_RptDepositsBurnedByResort(dtmStart, dtmEnd, salesRooms).ToList();

          var currencies = (from gRcpt in giftReceipts
                            join curr in dbContext.Currencies on gRcpt.grcu equals curr.cuID
                            select curr).Distinct().ToList();

          var paymentsType = (from gRcpt in giftReceipts
                              join payTyp in dbContext.PaymentTypes on gRcpt.grpt equals payTyp.ptID
                              select payTyp).Distinct().ToList();

          var saleReceipts = (from gSale in dbContext.Sales
                              join gu in dbContext.Guests on gSale.sagu equals gu.guID
                              join gRctp in dbContext.GiftsReceipts on gu.guID equals gRctp.grgu
                              join per in dbContext.Personnels on gRctp.grpe equals per.peID
                              where salesRoom.Contains(gSale.sasr) &&
                              gSale.saD >= dtmStart && gSale.saD <= dtmEnd &&
                              (gRctp.grDepositTwisted > 0  ||  (gu.guHotelB!="" && gu.guHotelB != null)) 
                              select new
                              {
                                gRctp.grID,
                                gSale
                              })
                              .GroupBy(c => c.grID)
                              .ToDictionary(c => c.Key, c => c.Select(s => s.gSale).ToList());

          return new List<object> { giftReceipts, currencies, paymentsType, saleReceipts };
        }
      }
      catch (Exception ex)
      {
        return null;
      }

    }
    #endregion

  }
}
