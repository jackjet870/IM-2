using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using System.IO;
using IM.Base.Helpers;
using System.Text.RegularExpressions;
using System.Data;

namespace IM.ProcessorGeneral.Classes
{
  public static class clsReports
  {

    #region Reports By SalesRoom

    #region Bookings

    #region ExportRptBookingsBySalesRoomProgramTime
    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptBookingsBySalesRoom,Program & Time
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptBBSalesRoom">Lista de datos para el reporte.</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptBookingsBySalesRoomProgramTime(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptBookingsBySalesRoomProgramTime> lstRptBBSalesRoom)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptBBSalesRoom);

      return EpplusHelper.CreatePivotRptExcel(true, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptBookingsBySalesRoomProgramTime(), true, true);
    }
    #endregion

    #region ExportRptBookingsBySalesRoomProgramLeadSourceTime
    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptBookingsBySalesRoom,Program, LeadSource & Time
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptBBSalesRoomLS">Lista de datos para el reporte.</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptBookingsBySalesRoomProgramLeadSourceTime(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptBookingsBySalesRoomProgramLeadSourceTime> lstRptBBSalesRoomLS)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptBBSalesRoomLS);
      return EpplusHelper.CreatePivotRptExcel(true, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptBookingsBySalesRoomProgramLeadSourceTime(), true, true);
    }
    #endregion

    #endregion

    #region CxC

    #region ExportRptCxC
    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptCxC
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptCxC">Lista de datos para el reporte.</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptCxC(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptCxC> lstRptCxC)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptCxC, changeDataTypeBoolToString: true, showCheckMark: false, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptCxC(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptCxCDeposits
    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptCxCDeposits
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptCxCDeposits">Lista de datos para el reporte.</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptCxCDeposits(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptCxCDeposits)
    {
      var lstCxCDeposits = lstRptCxCDeposits[0] as List<RptCxCDeposits>;
      var Currencies = lstRptCxCDeposits[1] as List<Currency>;

      var CxCDeCur = (from cxcDep in lstCxCDeposits
                      join curr in Currencies on cxcDep.grcuCxCPRDeposit equals curr.cuID
                      select new
                      {
                        cxcDep.grID,
                        cxcDep.grNum,
                        cxcDep.grD,
                        cxcDep.grls,
                        cxcDep.grgu,
                        cxcDep.grGuest,
                        cxcDep.grHotel,
                        cxcDep.grpe,
                        cxcDep.peN,
                        cxcDep.grHost,
                        cxcDep.HostN,
                        cxcDep.grCxCPRDeposit,
                        cxcDep.grcuCxCPRDeposit,
                        curr.cuN
                      }).ToList();


      DataTable dtData = TableHelper.GetDataTableFromList(CxCDeCur, changeDataTypeBoolToString: true, showCheckMark: false);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptCxCDeposits(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptCxCGift
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de RptCxCGifts.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptCxCGift">Lista de CxCGift y de Gifts</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptCxCGift(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptCxCGift)
    {
      //Lista de RecibosConRegalos.
      var lstReceipts = lstRptCxCGift[0] as List<RptCxCGifts>;
      //Lista de Regalos.
      var lstGift = lstRptCxCGift[1] as List<Gift>;

      //Obtenemos los datos.
      var receiptsWithGift = (from gRcpt in lstReceipts
                              select new
                              {
                                gRcpt.Adults,
                                gRcpt.Cost,
                                //Sumamos la costo en pesos mexicanos de todos los regalos.
                                CostMX = lstReceipts.Where(c => c.grID == gRcpt.grID).Sum(c => c.CostMX),
                                //Sumamos la costo en dolares de todos los regalos.
                                CostUS = lstReceipts.Where(c => c.grID == gRcpt.grID).Sum(c => c.CostUS),
                                gRcpt.exExchRate,
                                gRcpt.Folios,
                                giN = gRcpt.Gift != null ? lstGift.FirstOrDefault(g => g.giID == gRcpt.Gift).giN : null,//Obtenemos el ID del regalo.
                                Gift = gRcpt.Gift,
                                gRcpt.grCxCComments,
                                gRcpt.grD,
                                gRcpt.grgu,
                                gRcpt.grGuest,
                                gRcpt.grHost,
                                gRcpt.grID,
                                gRcpt.grlo,
                                gRcpt.grMemberNum,
                                gRcpt.grNum,
                                gRcpt.grpe,
                                gRcpt.HostN,
                                gRcpt.Minors,
                                gRcpt.peN,
                                //La Cantidad se obtienen checando si el regalo maneja Pax
                                //Si maneja se suma la cantidad de adultos y los menores.
                                Quantity = (lstGift.FirstOrDefault(c => gRcpt.Gift == c.giID) != null && lstGift.FirstOrDefault(c => gRcpt.Gift == c.giID).giWPax) ? (gRcpt.Adults + gRcpt.Minors) : gRcpt.Quantity
                              })
                              .ToList();

      return EpplusHelper.createExcelCustomPivot(TableHelper.GetDataTableFromList(receiptsWithGift.OrderBy(c => c.giN).ToList()), filters, strReport, dateRangeFileName, clsFormatReport.rptCxCGifts(), blnRowGrandTotal: true);
    }
    #endregion

    #region ExportRptCxCNotAuthorized
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de RptCxCNotAuthorized.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptCxCNotAuthorized">Lista de RptCxCNotAuthorized</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptCxCNotAuthorized(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptCxCNotAuthorized> lstRptCxCNotAuthorized)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptCxCNotAuthorized, changeDataTypeBoolToString: true, showCheckMark: false, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.createExcelCustom(dtData, filters, strReport, dateRangeFileName, clsFormatReport.rptCxCNotAuthorized());
    }
    #endregion

    #endregion

    #region Deposits

    #region ExportRptDeposits
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de RptCxCGifts.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptCxCGift">Lista de CxCGift y de Gifts</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 01/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDeposits(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptDeposits)
    {
      //Recibos de Regalo
      var lstGiftReceipts = lstRptDeposits[0] as List<RptDeposits>;

      //Monedas
      var lstCurrencies = lstRptDeposits[1] as List<Currency>;

      //Tipos de Pago
      var lstPaymentType = lstRptDeposits[2] as List<PaymentType>;

      var lstRptDeps = (from gRcpt in lstGiftReceipts
                        join curr in lstCurrencies on gRcpt.grcu equals curr.cuID
                        join payType in lstPaymentType on gRcpt.grpt equals payType.ptID
                        select new
                        {
                          gRcpt.grID,
                          gRcpt.grNum,
                          gRcpt.guOutInvitNum,
                          gRcpt.grgu,
                          gRcpt.grGuest,
                          gRcpt.grHotel,
                          gRcpt.grls,
                          gRcpt.grsr,
                          gRcpt.grpe,
                          gRcpt.peN,
                          gRcpt.grD,
                          gRcpt.guBookD,
                          gRcpt.guShow,
                          curr.cuN,
                          payType.ptN,
                          gRcpt.grDeposit,
                          gRcpt.guDepositReceived,
                          gRcpt.grDepositTwisted,
                          gRcpt.grDepositCxC
                        }
                      ).OrderBy(c => c.grD)
                      .ThenBy(c => c.grID)
                      .ToList();

      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptDeps, true, false, true), strReport, dateRangeFileName, clsFormatReport.rptDeposits(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptBurnedDeposits
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Burned Deposits.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptBurnedDeposits">Lista de BurnedDeposits,Currency, PaymentType y Sales</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptBurnedDeposits(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptBurnedDeposits, DateTime dtmStart, DateTime dtmEnd)
    {
      //Recibos de Regalo
      var lstGiftReceipts = lstRptBurnedDeposits[0] as List<RptDepositsBurned>;

      //Monedas
      var lstCurrencies = lstRptBurnedDeposits[1] as List<Currency>;

      //Tipos de Pago
      var lstPaymentType = lstRptBurnedDeposits[2] as List<PaymentType>;

      var lstGiftSales = lstRptBurnedDeposits[3] as Dictionary<int, List<Sale>>;

      var lstRptDeps = (from gRcpt in lstGiftReceipts
                        join curr in lstCurrencies on gRcpt.grcu equals curr.cuID
                        join payType in lstPaymentType on gRcpt.grpt equals payType.ptID
                        select new
                        {
                          gRcpt.grID,
                          gRcpt.grNum,
                          gRcpt.grD,
                          gRcpt.grgu,
                          gRcpt.grGuest,
                          gRcpt.grHotel,
                          gRcpt.grlo,
                          gRcpt.grsr,
                          gRcpt.grpe,
                          gRcpt.peN,
                          gRcpt.grHost,
                          gRcpt.grComments,
                          curr.cuN,
                          payType.ptN,
                          gRcpt.grDepositTwisted,
                          memberNum = ((lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) == 1 || (lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) > 1 && lstGiftReceipts.First(gr => gr.grgu == gRcpt.grgu).grID == gRcpt.grID)) ? ((lstGiftSales.ContainsKey(gRcpt.grID)) ? string.Join(",", lstGiftSales[gRcpt.grID].Select(s => s.saMembershipNum).ToList()) : "-") : "-"),
                          procAmount = ((lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) == 1 || (lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) > 1 && lstGiftReceipts.First(gr => gr.grgu == gRcpt.grgu).grID == gRcpt.grID)) ? ((lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => s.saProc && (s.saProcD >= dtmStart && s.saProcD <= dtmEnd)).Sum(s => s.saGrossAmount) : 0) : 0),
                          pendAmount = ((lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) == 1 || (lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) > 1 && lstGiftReceipts.First(gr => gr.grgu == gRcpt.grgu).grID == gRcpt.grID)) ? ((lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => !s.saProc || (s.saProc && !(s.saProcD >= dtmStart && s.saProcD <= dtmEnd))).Sum(s => s.saGrossAmount) : 0) : 0)
                        }
                        ).OrderBy(c => c.grD)
                        .ThenBy(c => c.grID)
                        .ToList();
      return EpplusHelper.createExcelCustomPivot(TableHelper.GetDataTableFromList(lstRptDeps, true, false), filters, strReport, dateRangeFileName, clsFormatReport.rptBurnedDeposits(), blnRowGrandTotal: true);
    }
    #endregion 

    #region ExportRptBurnedDepositsByResorts
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Burned Deposits by Resort.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptBurnedDeposits">Lista de BurnedDepositsByResorts,Currency, PaymentType y Sales</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptBurnedDepositsByResorts(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptBurnedDeposits, DateTime dtmStart, DateTime dtmEnd)
    {
      //Recibos de Regalo
      var lstGiftReceipts = lstRptBurnedDeposits[0] as List<RptDepositsBurnedByResort>;

      //Monedas
      var lstCurrencies = lstRptBurnedDeposits[1] as List<Currency>;

      //Tipos de Pago
      var lstPaymentType = lstRptBurnedDeposits[2] as List<PaymentType>;

      var lstGiftSales = lstRptBurnedDeposits[3] as Dictionary<int, List<Sale>>;

      var lstRptDeps = (from gRcpt in lstGiftReceipts
                        join curr in lstCurrencies on gRcpt.grcu equals curr.cuID
                        join payType in lstPaymentType on gRcpt.grpt equals payType.ptID
                        select new
                        {
                          grID = gRcpt.grID,
                          grNum = gRcpt.grNum,
                          grD = gRcpt.grD,
                          grgu = gRcpt.grgu,
                          grGuest = gRcpt.grGuest,
                          grHotel = gRcpt.grHotel,
                          guHotelB = gRcpt.guHotelB,
                          grlo = gRcpt.grlo,
                          grsr = gRcpt.grsr,
                          grpe = gRcpt.grpe,
                          peN = gRcpt.peN,
                          grHost = gRcpt.grHost,
                          cuN = curr.cuN,
                          ptN = payType.ptN,
                          grDepositTwisted = gRcpt.grDepositTwisted,
                          memberNum = ((lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) == 1 || (lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) > 1 && lstGiftReceipts.First(gr => gr.grgu == gRcpt.grgu).grID == gRcpt.grID)) ? ((lstGiftSales.ContainsKey(gRcpt.grID)) ? string.Join(",", lstGiftSales[gRcpt.grID].Select(s => s.saMembershipNum).ToList()) : "-") : "-"),
                          procAmount = ((lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) == 1 || (lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) > 1 && lstGiftReceipts.First(gr => gr.grgu == gRcpt.grgu).grID == gRcpt.grID)) ? ((lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => s.saProc && (s.saProcD >= dtmStart && s.saProcD <= dtmEnd)).Sum(s => s.saGrossAmount) : 0) : 0),
                          pendAmount = ((lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) == 1 || (lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) > 1 && lstGiftReceipts.First(gr => gr.grgu == gRcpt.grgu).grID == gRcpt.grID)) ? ((lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => !s.saProc || (s.saProc && !(s.saProcD >= dtmStart && s.saProcD <= dtmEnd))).Sum(s => s.saGrossAmount) : 0) : 0)
                        }
                        ).OrderBy(c => c.grD)
                        .ThenBy(c => c.grID)
                        .ToList();

      return EpplusHelper.createExcelCustomPivot(TableHelper.GetDataTableFromList(lstRptDeps, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptBurnedDepositsByResorts(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptPaidDeposits
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Paid Deposits.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptPaidDeposits">Lista de PaidDeposits,Currency, PaymentType y Sales</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptPaidDeposits(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptPaidDeposits, DateTime dtmStart, DateTime dtmEnd)
    {
      //Recibos de Regalo
      var lstGiftReceipts = lstRptPaidDeposits[0] as List<RptDepositsPaid>;
      //Monedas
      var lstCurrencies = lstRptPaidDeposits[1] as List<Currency>;
      //Tipos de Pago
      var lstPaymentType = lstRptPaidDeposits[2] as List<PaymentType>;

      var lstRptDeps = (from gRcpt in lstGiftReceipts
                        join curr in lstCurrencies on gRcpt.grcu equals curr.cuID
                        join payType in lstPaymentType on gRcpt.grpt equals payType.ptID
                        select new
                        {
                          gRcpt.grID,
                          gRcpt.grNum,
                          gRcpt.grD,
                          gRcpt.grgu,
                          gRcpt.guBookD, 
                          gRcpt.grGuest,
                          gRcpt.grHotel,
                          gRcpt.grls,
                          gRcpt.grsr,
                          gRcpt.grpe,
                          gRcpt.peN,
                          gRcpt.grHost,
                          curr.cuN,
                          payType.ptN,
                          gRcpt.grDeposit,
                          gRcpt.grDepositTwisted
                        }
                        ).OrderBy(c => c.grD)
                        .ThenBy(c => c.grID)
                        .ToList();

      return EpplusHelper.createExcelCustomPivot(TableHelper.GetDataTableFromList(lstRptDeps, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptPaidDeposits(), blnShowSubtotal: true);

    }
    #endregion

    #endregion

    #region Gifts

    #region ExportRptDailyGiftSimple
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Daily Gift Simple.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptDailyGS">Lista de RptDailyGiftSimple</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 07/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDailyGiftSimple(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptDailyGiftSimple> lstRptDailyGS)
    {
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptDailyGS, true, false), strReport, dateRangeFileName, clsFormatReport.RptDailyGiftSimple(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptWeeklyGiftSimple
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Weekly Gift Simple.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptWeeklyGS">Lista de RptWeeklyGiftSimple</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptWeeklyGiftSimple(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptWeeklyGiftsItemsSimple> lstRptWeeklyGS)
    {
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptWeeklyGS, true, false), strReport, dateRangeFileName, clsFormatReport.RptWeeklyGiftSimple(), showRowGrandTotal: true);
    }
    #endregion

    #endregion

    #region Guests

    #region ExportRptGuestCeco
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Guest CECO.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptGuestCeco">Lista de RptGuestCeco</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGuestCeco(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptGuestCeco> lstRptGuestCeco)
    {
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptGuestCeco, true, false), strReport, dateRangeFileName, clsFormatReport.RptGuestCeco());
    }
    #endregion 

    #region ExportRptGuestNoBuyers
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Guest No Buyers.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptGuestNoBuyers">Lista de RptGuestNoBuyers</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGuestNoBuyers(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptGuestsNoBuyers> lstRptGuestNoBuyers)
    {
      lstRptGuestNoBuyers = lstRptGuestNoBuyers
        .OrderBy(c => c.Program)
        .ThenBy(c => c.LeadSource)
        .ThenBy(c => c.GuestID)
        .ToList();
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptGuestNoBuyers, true, false), strReport, dateRangeFileName, clsFormatReport.RptGuestNoBuyers());
    }
    #endregion

    #region ExportRptInOut
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de In & Out.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptInOut">Lista de RptGuestNoBuyers</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptInOut(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptInOut> lstRptInOut)
    {

      var lstRptInOutNew = lstRptInOut
        .OrderBy(c => c.SalesRoom)
        .ThenBy(c => c.Location)
        .ThenBy(c => c.GUID)
        .ThenBy(c => c.ShowDate)
        .Select(c => new
        {
          c.SalesRoom,
          c.Location,
          c.GUID,
          c.Hotel,
          c.Room,
          c.Pax,
          c.LastName,
          c.FirstName,
          c.Agency,
          c.AgencyN,
          c.Country,
          c.CountryN,
          c.ShowDate,
          c.TimeIn,
          c.TimeOut,
          c.Direct,
          Tour = Convert.ToInt32(c.Tour),
          InOut = Convert.ToInt32(c.InOut),
          WalkOut = Convert.ToInt32(c.WalkOut),
          CourtesyTour = Convert.ToInt32(c.CourtesyTour),
          SaveProgram = Convert.ToInt32(c.SaveProgram),
          c.PR1,
          c.PR1N,
          c.PR2,
          c.PR2N,
          c.PR3,
          c.PR3N,
          c.Host,
          c.Comments
        })
        .ToList();
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptInOutNew, true, false), strReport, dateRangeFileName, clsFormatReport.RptInOut());
    }
    #endregion

    #region ExportRptGuestNoShows
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Guest No Shows.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptGuestNoShows">Lista de RptGuestNoShows</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGuestNoShows(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptGuestsNoShows> lstRptGuestNoShows)
    {
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptGuestNoShows, true, false), strReport, dateRangeFileName, clsFormatReport.RptGuestNoShow(), showRowGrandTotal: true);
    }
    #endregion

    #endregion

    #region Meal Tickets

    #region ExportRptMealTickets
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Meal Tickets.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptMealTickets">Lista de RptMealTickets</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptMealTickets(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptMealTickets> lstRptMealTickets, bool groupByHost = false)
    {
      FileInfo file = null;
      if (groupByHost)
        file = EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptMealTickets, true, false), strReport, dateRangeFileName, clsFormatReport.RptMealTicketsByHost(), showRowGrandTotal: true);
      else
        file = EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptMealTickets, true, false), strReport, dateRangeFileName, clsFormatReport.RptMealTickets(), showRowGrandTotal: true);


      return file;
    }
    #endregion

    #region ExportRptMealTicketsCost
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Meal Tickets With Cost.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRange">Rango de Fechas</param>
    /// <param name="lstRptMealTicketsCost">Lista de RptMealTicketsCost</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptMealTicketsCost(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptMealTicketsCost> lstRptMealTicketsCost)
    {
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptMealTicketsCost, true, false), strReport, dateRangeFileName, clsFormatReport.RptMealTicketsCost(), showRowGrandTotal: true, showColumnGrandTotal: true);
    }
    #endregion

    #endregion

    #region MemberShips

    #region ExportRptMemberships
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Memberships.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptMemberships">Lista de RptMemberships</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptMemberships(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptMemberships> lstRptMemberships)
    {
      return EpplusHelper.createExcelCustom(TableHelper.GetDataTableFromList(lstRptMemberships, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptMemberships(), blnRowGrandTotal: true);
    }
    #endregion

    #region ExportRptMembershipsByAgencyMarket
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Memberships By Agency & Market.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptMembershipsAgencyM">Lista de RptMembershipsByAgencyMarket</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptMembershipsByAgencyMarket(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptMembershipsByAgencyMarket> lstRptMembershipsAgencyM)
    {
      return EpplusHelper.createExcelCustom(TableHelper.GetDataTableFromList(lstRptMembershipsAgencyM, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptMembershipsByAgencyMarket(), blnShowSubtotal: true);//, blnRowGrandTotal: true);
    }
    #endregion

    #region ExportRptMembershipsByHost
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Memberships By Host.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptMembershipsHost">Lista de RptMembershipsByHost</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 14/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptMembershipsByHost(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptMembershipsByHost> lstRptMembershipsHost)
    {
      lstRptMembershipsHost.ForEach(c => c.guEntryHost = string.Format("{0} {1}", c.guEntryHost, c.guEntryHostN));
      return EpplusHelper.createExcelCustom(TableHelper.GetDataTableFromList(lstRptMembershipsHost, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptMembershipsByHost(), blnShowSubtotal: true);//, blnRowGrandTotal: true);
    }
    #endregion

    #endregion

    #endregion

  }
}