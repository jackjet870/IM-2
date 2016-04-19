using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using System.IO;
using IM.Base.Helpers;
using System.Data;

namespace IM.ProcessorOuthouse.Classes
{
  public class clsReports
  {
    #region ExportRptDepositsPaymentTypes
    /// <summary>
    ///  Obtiene los datos para exportar a excel el reporte DepositsPaymentTypesByPR
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptDepositsPaymentByPR">Lista de los depositos y pagos por PR</param>
    /// <history>
    ///   [vku] 07/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDepositsPaymentByPR(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptDepositsPaymentByPR)
    {
      var lstDepPyPR = lstRptDepositsPaymentByPR[0] as List<RptDepositsPaymentByPR>;
      var lstGuests = lstRptDepositsPaymentByPR[1] as List<Guest>;
      var lstBookDep = lstRptDepositsPaymentByPR[3] as List<BookingDeposit>; 
      var depPayPRwhitGuestBookDep = (from deppyPR in lstDepPyPR
                               join gu in lstGuests.Where(c => c.guInvit) on deppyPR.PR equals gu.guPRInvit1
                               join bookDep in lstBookDep on gu.guID equals bookDep.bdgu
                               select new
                               {
                                 deppyPR.PR,
                                 deppyPR.PRN,
                                 deppyPR.Books,
                                 deppyPR.InOuts,
                                 deppyPR.GrossBooks,
                                 deppyPR.ShowsFactor,
                                 deppyPR.GrossShows,
                                 gu.guID,
                                 guName = string.Format("{0}, {1}", gu.guLastName1, gu.guFirstName1),
                                 gu.guBookD,
                                 gu.guOutInvitNum,
                                 gu.guls,
                                 gu.gusr,
                                 gu.guHotel,
                                 bookDep.bdpc,
                                 deppyPR.SalesAmount,
                                 deppyPR.Efficiency,
                                 bookDep.bdAmount,
                                 bookDep.bdReceived,          
                              })
                              .ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptDepositsPaymentByPR);
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptDepositsPaymentByPR());
    }
    #endregion

    #region ExportRptGiftsReceivedBySR
    /// <summary>
    ///  Obtiene los datos para el reporte GiftsReceivedBySR
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptGiftsReceivedBySR">Lista de regalos recibidos por SR</param>
    /// <history>
    ///   [vku] 11/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsReceivedBySR(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptGiftsReceivedBySR)
     {

      var lstGiftsReceivedBySR = lstRptGiftsReceivedBySR[0] as List<RptGiftsReceivedBySR>;
      var curriencies = lstRptGiftsReceivedBySR[1] as List<Currency>;

      var lstGifRecBySRWithCu = (from giftRecBySR in lstGiftsReceivedBySR
                                 join cu in curriencies on giftRecBySR.Currency equals cu.cuID
                                 select new
                                 {
                                   giftRecBySR.SalesRoom,
                                   giftRecBySR.Gift,
                                   giftRecBySR.GiftN,
                                   giftRecBySR.Quantity,
                                   giftRecBySR.Couples,
                                   giftRecBySR.Adults,
                                   giftRecBySR.Minors,
                                   cu.cuN,
                                   giftRecBySR.Amount,
                                 }).OrderBy(c => c.cuN).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstGifRecBySRWithCu);
      return EpplusHelper.createExcelCustomPivot(dtData, filters, strReport, dateRangeFileName, clsFormatReport.rptGiftsRecivedBySR(), blnShowSubtotal: true);



      // return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptGiftsRecivedBySR(), showRowGrandTotal: true);
    }
     #endregion

    #region ExportRptGuestsShowNoPresentedInvitation
    /// <summary>
    ///  Obtiene los datos para el reporte GuestsShowNoPresentedInvitation
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptGuestsShowNoPresentedInvitation">Lista de los huespedes que no presentaron invitacion</param>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGuestsShowNoPresentedInvitation(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<GuestShowNoPresentedInvitation> lstRptGuestsShowNoPresentedInvitation)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptGuestsShowNoPresentedInvitation);
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptGuestsShowNoPresentedInvitation());
    }
    #endregion

    #region ExportRptProductionByPROuthouse
    /// <summary>
    ///  Obtiene los datos  para el reporte ProductionByPROuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByPROuthouse">Lista de produccion por PR</param>
    /// <history>
    ///  [vku] 14/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByPROuthouse(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByPROuthouse> lstRptProductionByPROuthouse)
    {
      var lstRptProductionByPROuthouseAux = lstRptProductionByPROuthouse.Select(c => new
      {
        c.Status,
        c.PR,
        c.PRN,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.GrossShows,
        c.Directs,
        c.WalkOuts,
        c.CourtesyTours,
        c.SaveTours,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByPROuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptProductionByPR(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptProductionByAge
    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByAge
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgeOuthouse">Lista de produccion por edad</param>
    /// <history>
    ///   [vku] 13/abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByAge(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByAgeOuthouse> lstRptProductionByAgeOuthouse)
    {    
      var lstRptProductionByAgeOuthouseAux = lstRptProductionByAgeOuthouse.Select(c => new
      {
        c.Age,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByAgeOuthouseAux);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptProductionByAge(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptProductionByAgeSalesRoomOuthouse
    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByAgeSalesRoomOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgeSalesRoomOuthouse">Lista de produccion por edad y sales room</param>
    /// <history>
    ///   [vku] 13/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByAgeSalesRoomOuthouse(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByAgeSalesRoomOuthouse> lstRptProductionByAgeSalesRoomOuthouse)
    {
      var lstRptProductionByAgeSalesRoomOuthouseAux = lstRptProductionByAgeSalesRoomOuthouse.Select(c => new
      {
        c.SalesRoomN,
        c.Age,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByAgeSalesRoomOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptProductionByAgeSalesRoomOuthouse(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptProductionByAgencyOuthouse
    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByAgencyOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgeSalesRoomOuthouse">Lista de produccion por edad y sales room</param>
    /// <history>
    ///   [vku] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByAgencyOuhouse(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByAgencyOuthouse> lstRptProductionByAgencyOuthouse)
    {
     var lstRptProductionByAgencyOuthouseAux = lstRptProductionByAgencyOuthouse.Select(c => new
                          {
                            c.Agency,
                            c.AgencyN,
                            c.Books,
                            c.InOuts,
                            c.GrossBooks,
                            c.Shows,
                            c.WalkOuts,
                            c.Tours,
                            c.CourtesyTours,
                            c.SaveTours,
                            c.TotalTours,
                            c.UPS,
                            c.Sales_PROC,
                            c.SalesAmount_PROC,
                            c.Sales_OOP,
                            c.SalesAmount_OOP,
                            c.Sales_CANCEL,
                            c.SalesAmount_CANCEL,
                            c.Sales_TOTAL,
                            c.SalesAmount_TOTAL,
                            c.ShowsFactor,
                            c.CancelFactor,
                            c.Efficiency,
                            c.ClosingFactor,
                            c.AverageSale
                          }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByAgencyOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptProductionByAgencyOuthouse(), showRowGrandTotal: true);

    }
    #endregion

    #region ExportRptProductionByAgencySalesRoomOuthouse
    #endregion

    #region ExportRptProductionByAgencyMarketHotelOuthouse
    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByAgencyMarketHotelOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgencyMarketHotelOuthouse">Lista de produccion por agencia, mercado y hotel</param>
    /// <history>
    ///   [vku] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByAgencyMarketHotelOuthouse(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByAgencyMarketHotelOuthouse> lstRptProductionByAgencyMarketHotelOuthouse)
    {
      var lstRptProductionByAgencyMarketHotelOuthouseAux = lstRptProductionByAgencyMarketHotelOuthouse.Select(c => new
      {
        c.Hotel,
        c.Market,
        c.Agency,
        c.AgencyN,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByAgencyMarketHotelOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptProductionByAgencyMarketHotelOuthouse(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptProductionByCoupleTypeOuthouse
    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByCoupleTypeOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgencyMarketHotelOuthouse">Lista de produccion por agencia, mercado y hotel</param>
    /// <history>
    ///   [vku] 18/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByCoupleTypeOuthouse(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByCoupleTypeOuthouse> lstRptProductionByCoupleTypeOuthouse)
    {
      var lstRptProductionByCoupleTypeOuthouseAux = lstRptProductionByCoupleTypeOuthouse.Select(c => new
      {
        c.CoupleType,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByCoupleTypeOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptProductionByCoupleTypeOuthouse(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptProductionByCoupleTypeSalesRoomOuthouse
    /// <summary>
    ///  Obtiene los datos para el report ProductionByCoupleTypeSalesRoomOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByCoupleTypeSalesRoomOuthouse">Lista de production por tipo de pareja yt sala</param>
    /// <history>
    ///   [vku] 18/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByCoupleTypeSalesRoomOuthouse(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByCoupleTypeSalesRoomOuthouse> lstRptProductionByCoupleTypeSalesRoomOuthouse)
    {
      var lstRptProductionByCoupleTypeSalesRoomOuthouseAux = lstRptProductionByCoupleTypeSalesRoomOuthouse.Select(c => new
      {
        c.SalesRoomN,
        c.CoupleType,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByCoupleTypeSalesRoomOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptProductionByCoupleTypeSalesRoomOuthouse(), showRowGrandTotal: true);
    }
    #endregion
  }
}
