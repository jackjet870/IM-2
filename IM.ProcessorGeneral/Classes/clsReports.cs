using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using System.IO;
using IM.Base.Helpers;
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
    /// <param name="lstRptBbSalesRoom">Lista de datos para el reporte.</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptBookingsBySalesRoomProgramTime(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptBookingsBySalesRoomProgramTime> lstRptBbSalesRoom)
    {
      var dtData = TableHelper.GetDataTableFromList(lstRptBbSalesRoom);

      return EpplusHelper.CreatePivotRptExcel(true, filters, dtData, strReport, dateRangeFileName, clsFormatReport.RptBookingsBySalesRoomProgramTime(), true, true);
    }
    #endregion

    #region ExportRptBookingsBySalesRoomProgramLeadSourceTime
    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptBookingsBySalesRoom,Program, LeadSource & Time
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptBOokingsSalesRoomLeadSource">Lista de datos para el reporte.</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptBookingsBySalesRoomProgramLeadSourceTime(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptBookingsBySalesRoomProgramLeadSourceTime> lstRptBOokingsSalesRoomLeadSource)
    {
      var dtData = TableHelper.GetDataTableFromList(lstRptBOokingsSalesRoomLeadSource);
      return EpplusHelper.CreatePivotRptExcel(true, filters, dtData, strReport, dateRangeFileName, clsFormatReport.RptBookingsBySalesRoomProgramLeadSourceTime(), true, true);
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.RptCxc(), showRowGrandTotal: true);
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
      var currencies = lstRptCxCDeposits[1] as List<Currency>;
      
      DataTable dtData = TableHelper.GetDataTableFromList((from cxcDep in lstCxCDeposits
                      join curr in currencies on cxcDep.grcuCxCPRDeposit equals curr.cuID
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
                      }).ToList(), true, false);

      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.RptCxcDeposits(), true);
    }
    #endregion

    #region ExportRptCxCGift

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de RptCxCGifts.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters">Filtros aplicados al los datos.</param>
    /// <param name="lstRptCxCGift">Lista de CxCGift y de Gifts</param>
    /// <param name="dateRangeFileName"></param>
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
                                giN = gRcpt.Gift != null ? lstGift.FirstOrDefault(g => g.giID == gRcpt.Gift)?.giN : null,//Obtenemos el ID del regalo.
                                gRcpt.Gift,
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

      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(receiptsWithGift.OrderBy(c => c.giN).ToList()), filters, strReport, dateRangeFileName, clsFormatReport.RptCxcGifts(), blnRowGrandTotal: true);
    }
    #endregion

    #region ExportRptCxCNotAuthorized

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de RptCxCNotAuthorized.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptCxCNotAuthorized">Lista de RptCxCNotAuthorized</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 04/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptCxCNotAuthorized(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptCxCNotAuthorized> lstRptCxCNotAuthorized)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptCxCNotAuthorized, true, false, true);
      return EpplusHelper.CreateExcelCustom(dtData, filters, strReport, dateRangeFileName, clsFormatReport.RptCxcNotAuthorized());
    }
    #endregion

    #region ExportRptCxCPayments

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de RptCxCNotAuthorized.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptCxCPayments">Lista de RptCxCPayments</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptCxCPayments(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptCxCPayments> lstRptCxCPayments)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptCxCPayments, true, false, true);
      return EpplusHelper.CreateExcelCustom(dtData, filters, strReport, dateRangeFileName, clsFormatReport.RptCxcPayments());
    }
    #endregion

    #endregion

    #region Deposits

    #region ExportRptDeposits

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de RptCxCGifts.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName"></param>
    /// <param name="filters"></param>
    /// <param name="lstRptDeposits"></param>
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

      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptDeps, true, false, true), strReport, dateRangeFileName, clsFormatReport.RptDeposits(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptBurnedDeposits

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Burned Deposits.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptBurnedDeposits">Lista de BurnedDeposits,Currency, PaymentType y Sales</param>
    /// <param name="dateRangeFileName"></param>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
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
      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstRptDeps, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptBurnedDeposits(), blnRowGrandTotal: true);
    }
    #endregion 

    #region ExportRptBurnedDepositsByResorts

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Burned Deposits by Resort.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptBurnedDeposits">Lista de BurnedDepositsByResorts,Currency, PaymentType y Sales</param>
    /// <param name="dateRangeFileName"></param>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
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
                          gRcpt.grID,
                          gRcpt.grNum,
                          gRcpt.grD,
                          gRcpt.grgu,
                          gRcpt.grGuest,
                          gRcpt.grHotel,
                          gRcpt.guHotelB,
                          gRcpt.grlo,
                          gRcpt.grsr,
                          gRcpt.grpe,
                          gRcpt.peN,
                          gRcpt.grHost,
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

      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstRptDeps, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptBurnedDepositsByResorts(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptPaidDeposits

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Paid Deposits.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptPaidDeposits">Lista de PaidDeposits,Currency, PaymentType y Sales</param>
    /// <param name="dateRangeFileName"></param>
    /// <param name="byPr">Filtro por PR</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptPaidDeposits(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptPaidDeposits, bool byPr = false)
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
                          grpe = (byPr) ? $"{gRcpt.grpe} {gRcpt.peN}" : gRcpt.grpe,
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

      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstRptDeps, true, false), filters, strReport, dateRangeFileName, (byPr) ? clsFormatReport.RptPaidDepositsByPr() : clsFormatReport.RptPaidDeposits(), blnRowGrandTotal: !byPr, blnShowSubtotal: byPr);

    }
    #endregion

    #endregion

    #region Gifts

    #region ExportRptDailyGiftSimple

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Daily Gift Simple.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptDailyGiftSimples">Lista de RptDailyGiftSimple</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 07/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDailyGiftSimple(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptDailyGiftSimple> lstRptDailyGiftSimples)
    {
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptDailyGiftSimples, true, false), strReport, dateRangeFileName, clsFormatReport.RptDailyGiftSimple(), showRowGrandTotal: true);
    }
    #endregion

    #region ExportRptGiftsByCategory

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Gift By Category.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptGiftsByCat">Lista de RptGiftsByCategory</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsByCategory(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptGiftsByCategory> lstRptGiftsByCat)
    {
      var lstGifts = lstRptGiftsByCat.Select(c => new
      {
        Gift = c.gegi,
        TotalQty = lstRptGiftsByCat.Where(g => g.gegi == c.gegi).Sum(g => g.Quantity),
        c.UnitCost,
        TotalCost = lstRptGiftsByCat.Where(g => g.gegi == c.gegi).Sum(g => g.TotalCost),
        c.Quantity,
        c.Day,
        Category = c.gcN
      }).ToList();



      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstGifts, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptGiftsByCategory(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptGiftsByCategoryProgram

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Gifts By Category & Program.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptGiftsByCatP">Lista de RptGiftsByCategoryProgram</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsByCategoryProgram(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptGiftsByCategoryProgram> lstRptGiftsByCatP)
    {
      var lstGifts = lstRptGiftsByCatP.Select(c => new
      {
        Gift = c.gegi,
        TotalQty = lstRptGiftsByCatP.Where(g => g.gegi == c.gegi).Sum(g => g.Quantity),
        c.UnitCost,
        TotalCost = lstRptGiftsByCatP.Where(g => g.gegi == c.gegi).Sum(g => g.TotalCost),
        c.Quantity,
        c.Day,
        Category = c.gcN,
        c.Program
      }).ToList();



      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstGifts, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptGiftsByCategoryProgram(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptGiftsCertificates

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Gifts Certificates.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptGiftsCerts">Lista de RptGiftsCertificates</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsCertificates(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptGiftsCerts)
    {
      var lstGifts = lstRptGiftsCerts[0] as List<RptGiftsCertificates>;
      var currencies = lstRptGiftsCerts[1] as List<Currency>;
      var payType = lstRptGiftsCerts[2] as List<PaymentType>;

      var certificates = lstGifts.Select(c => new
      {
        c.GiftID,
        c.GiftN,
        c.Receipt,
        c.Status,
        c.Folios,
        c.Date,
        c.Host,
        c.Guest,
        c.Location,
        c.Adults,
        c.Minors,
        c.ExtraAdults,
        c.Pax,
        c.CxC,
        PaymentType = c.PaymentType != null ? payType.First(p => p.ptID == c.PaymentType).ptN : null,
        Currency = c.Currency != null ? currencies.First(cu => cu.cuID == c.Currency).cuN : null,
        c.Paid,
        c.Refund,
        c.Comments
      }).ToList();
      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(certificates, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptGiftsCertificates(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptGiftsReceiptsPayments

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Gifts Receipts Payments.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptGiftsRcptPaym">Lista de object</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsReceiptsPayments(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptGiftsRcptPaym)
    {
      var lstGiftsR = lstRptGiftsRcptPaym[0] as List<RptGiftsReceiptsPayments>;
      var source = lstRptGiftsRcptPaym[1] as List<SourcePayment>;
      var currencies = lstRptGiftsRcptPaym[2] as List<Currency>;
      var payType = lstRptGiftsRcptPaym[3] as List<PaymentType>;

      var receipts = lstGiftsR.Select(c => new
      {
        Source = c.Source != null ? source.First(p => p.sbID == c.Source).sbN : null,
        c.Amount,
        AmountUS = lstGiftsR.Where(g => g.Source == c.Source).Sum(g => g.AmountUS),
        PaymentType = c.PaymentType != null ? payType.First(p => p.ptID == c.PaymentType).ptN : null,
        Currency = c.Currency != null ? currencies.First(cu => cu.cuID == c.Currency).cuN : null,
        GroupSource1 = (c.Source == "MK") ? "MARKETING" : "INCOME",
        GroupSource2 = (c.Source == "MK") ? "MARKETING" : (c.Source == "CXC") ? "CXC" : "SUBTOTAL INCOME"
      }).ToList();

      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(receipts, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptGiftsReceiptsPayments(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptGiftsSale

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Gifts Receipts Payments.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptGiftsSale">Tuple</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsSale(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, 
      List<RptGiftsSale> lstRptGiftsSale)
    {
       return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstRptGiftsSale, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptGiftsReceiptsPayments(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptGiftsUsedBySistur

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Gifts Used By Sistur.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptGiftsSistur">Lista de RptGiftsUsedBySistur</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsUsedBySistur(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptGiftsUsedBySistur> lstRptGiftsSistur)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptGiftsSistur, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptGiftsUsedBySistur(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptWeeklyGiftSimple

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Weekly Gift Simple.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptWeeklyGiftsSimple">Lista de RptWeeklyGiftSimple</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 08/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptWeeklyGiftSimple(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptWeeklyGiftsItemsSimple> lstRptWeeklyGiftsSimple)
    {
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptWeeklyGiftsSimple, true, false), strReport, dateRangeFileName, clsFormatReport.RptWeeklyGiftSimple(), showRowGrandTotal: true);
    }
    #endregion

    #endregion

    #region Guests

    #region ExportRptGuestCeco

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Guest CECO.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptGuestCeco">Lista de RptGuestCeco</param>
    /// <param name="dateRangeFileName"></param>
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
        file = EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptMealTickets, true, false,true), strReport, dateRangeFileName, clsFormatReport.RptMealTicketsByHost(), showRowGrandTotal: true);
      else
        file = EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptMealTickets, true, false,true), strReport, dateRangeFileName, clsFormatReport.RptMealTickets(), showRowGrandTotal: true);


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
      return EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptMealTicketsCost, true, false,true), strReport, dateRangeFileName, clsFormatReport.RptMealTicketsCost(), showRowGrandTotal: true, showColumnGrandTotal: true);
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
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptMemberships, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptMemberships(), blnRowGrandTotal: true);
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
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptMembershipsAgencyM, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptMembershipsByAgencyMarket(), blnShowSubtotal: true);//, blnRowGrandTotal: true);
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
      lstRptMembershipsHost.ForEach(c => c.guEntryHost = $"{c.guEntryHost} {c.guEntryHostN}");
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptMembershipsHost, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptMembershipsByHost(), blnShowSubtotal: true);//, blnRowGrandTotal: true);
    }
    #endregion

    #endregion

    #region Production

    #region ExportRptProductionBySalesRoom
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Production by SalesRoom.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptProductionBySR">Lista de RptProductionBySalesRoom</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionBySalesRoom(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionBySalesRoom> lstRptProductionBySR)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProductionBySR, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionBySalesRoom(), blnRowGrandTotal: true);
    }
    #endregion

    #region ExportRptProductionBySalesRoomMarket
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Production by SalesRoom & Market.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptProductionBySRM">Lista de RptProductionBySalesRoomMarket</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionBySalesRoomMarket(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionBySalesRoomMarket> lstRptProductionBySRM)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProductionBySRM, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionBySalesRoomMarket(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptProductionBySalesRoomMarketSubMarket
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Production by SalesRoom,Market & Submarket.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptProductionBySRMSm">Lista de RptProductionBySalesRoomMarket</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionBySalesRoomMarketSubMarket(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionBySalesRoomProgramMarketSubmarket> lstRptProductionBySRMSm)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProductionBySRMSm, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionBySalesRoomMarketSubMarket(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptProductionByShowProgram
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Production by Show & Program.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptProductionByShowProgram">Lista de RptProductionByShowProgram</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByShowProgram(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByShowProgram> lstRptProductionByShowProgram)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProductionByShowProgram, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionByShowProgram(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptProductionByShowProgramProgram
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Production by Show,Program & Program.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptProductionByShowProgram">Lista de RptProductionByShowProgram</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByShowProgramProgram(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByShowProgramProgram> lstRptProductionByShowProgramProgram)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProductionByShowProgramProgram, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionByShowProgramProgram(), blnShowSubtotal: true);
    }
    #endregion

    #endregion

    #region Taxi

    #region ExportRptTaxiIn
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Taxi In
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptProductionByShowProgram">Lista de RptTaxisIn</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptTaxiIn(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptTaxisIn> lstRptTaxiIn)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptTaxiIn, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptTaxisIn(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptTaxiOut
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Taxi Out
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptProductionByShowProgram">Lista de RptTaxisIn</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 16/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptTaxiOut(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptTaxisOut> lstRptTaxiOut)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptTaxiOut, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptTaxisOut(), blnRowGrandTotal: true);
    }
    #endregion

    #endregion

    #endregion

    #region Reports By Leadsources

    #region ExportRptBurnedDepositsGuests
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Burned Deposits Guests
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptBurnedDepGuest">Lista de object</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptBurnedDepositsGuests(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptBurnedDepGuest)
    {
      var lstGiftsR = lstRptBurnedDepGuest[0] as List<RptDepositsBurnedGuests>;
      var currencies = lstRptBurnedDepGuest[1] as List<Currency>;
      var payType = lstRptBurnedDepGuest[2] as List<PaymentType>;

      lstGiftsR.ForEach(c => {
        c.gucu = currencies.First(cu => cu.cuID == c.gucu).cuN ?? "";
        c.gupt = payType.First(pt => pt.ptID == c.gupt).ptN ?? "";
      });

      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstGiftsR, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptDepositsBurnedGuests(), blnRowGrandTotal: true);
    }
    #endregion

    #region ExportRptDepositRefunds
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Deposit Refunds
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptDepRef">Lista de RptDepositRefund</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDepositRefunds(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptDepositRefund> lstRptDepRef)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptDepRef, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptDepositRefunds(), blnRowGrandTotal: true);
    }
    #endregion

    #region ExportRptDepositByPR
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Deposit By PR
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptDepPR">Lista de object</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDepositByPR(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptDepPR)
    {
      var lstDepPR = lstRptDepPR[0] as List<RptDepositsByPR>;
      var currencies = lstRptDepPR[1] as List<Currency>;
      var payType = lstRptDepPR[2] as List<PaymentType>;

      lstDepPR.ForEach(c => {
        c.guPRInvit1 = string.Format("{0} {1}", c.guPRInvit1, c.peN);
        c.gucu = currencies.First(cu => cu.cuID == c.gucu).cuN ?? "";
        c.gupt = payType.First(pt => pt.ptID == c.gupt).ptN ?? "";
      });

      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstDepPR, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptDepositByPr(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptDepositsNoShow
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Deposits No Show
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptDepNoShow">Lista de object</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDepositsNoShow(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptDepNoShow)
    {
      var lstDepPRNoShow = lstRptDepNoShow[0] as List<RptDepositsNoShow>;
      var currencies = lstRptDepNoShow[1] as List<Currency>;
      var payType = lstRptDepNoShow[2] as List<PaymentType>;

      lstDepPRNoShow.ForEach(c => {
        c.guPRInvit1 = string.Format("{0} {1}", c.guPRInvit1, c.peN);
        c.gucu = currencies.First(cu => cu.cuID == c.gucu).cuN ?? "";
        c.gupt = payType.First(pt => pt.ptID == c.gupt).ptN ?? "";
      });

      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstDepPRNoShow, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptDepositByPr(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptInOutByPR
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte In & Out By PR
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptInOutPR">Lista de object</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptInOutByPR(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptInOutByPR> lstRptInOutPR)
    {

      lstRptInOutPR.ForEach(c => {
        c.PR1 = string.Format("{0} {1}", c.PR1, c.PR1N);
      });

      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptInOutPR, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptInOutByPr(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptPersonnelAccess
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Personnel Access
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptPersonnelAccess">Lista de RptPersonnelAccess</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptPersonnelAccess(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptPersonnelAccess> lstRptPersonnelAccess)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptPersonnelAccess, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptPersonnelAccess());
    }
    #endregion

    #region ExportRptSelfGen

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Personnel Access
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptselfGenTuple">Tuple de RptSelfGen, Sale, Personnel</param>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 25/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptSelfGen(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, Tuple<List<RptSelfGen>, List<Sale>, List<Personnel>> lstRptselfGenTuple, DateTime dtmStart, DateTime dtmEnd)
    {
      var lstRptSelfGen = (from pe in lstRptselfGenTuple.Item3
        select new
        {
          PRID = pe.peID,
          PRN = pe.peN,
          PRps = pe.peps,
          Shows = lstRptselfGenTuple.Item1.Where(c => c.guPRInvit1 == pe.peID).Count(c=> c.guShowD != null && (c.guShowD >= dtmStart && c.guShowD <= dtmEnd)),
          IO = lstRptselfGenTuple.Item1.Where(c => c.guPRInvit1 == pe.peID).Count(c => c.guInOut),
          WO = lstRptselfGenTuple.Item1.Where(c => c.guPRInvit1 == pe.peID).Count(c => c.guWalkOut),

          Proc = lstRptselfGenTuple.Item2.Where(c=> c.saPR1 == pe.peID).Count(c => c.saProcD != null && (c.saD == c.saProcD && (c.saD >= dtmStart && c.saD <= dtmEnd)) && c.sast != "DG"),
          ProcAmount = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD == c.saProcD && (c.saD >= dtmStart && c.saD <= dtmEnd))).Sum(c=> c.saGrossAmount),

          OOP = lstRptselfGenTuple.Item2.Count(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD != c.saProcD && (c.saProcD >= dtmStart && c.saProcD <= dtmEnd)) && c.sast != "DG"),
          OOPAmount = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD != c.saProcD && (c.saProcD >= dtmStart && c.saProcD <= dtmEnd))).Sum(c => c.saGrossAmount),

          Cancel = lstRptselfGenTuple.Item2.Count(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saCancelD >= dtmStart && c.saCancelD <= dtmEnd) && c.sast != "DG"),
          CancelAmount = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saCancelD >= dtmStart && c.saCancelD <= dtmEnd)).Sum(c=>c.saGrossAmount),

          CancelF = 0m,
          Subtotal = 0m,

          TotalProc = 0m,

          TotalProcAmount = (lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD == c.saProcD && (c.saD >= dtmStart && c.saD <= dtmEnd))).Sum(c => c.saGrossAmount)+
          lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD != c.saProcD && (c.saProcD >= dtmStart && c.saProcD <= dtmEnd))).Sum(c => c.saGrossAmount))-
          lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saCancelD >= dtmStart && c.saCancelD <= dtmEnd)).Sum(c => c.saGrossAmount),
          Eff = 0m,
          CI = 0m,
          AvgSale = 0m,
          Pending = lstRptselfGenTuple.Item2.Count(c => c.saPR1 == pe.peID && (c.saProcD == null || c.saProcD > dtmEnd) && c.saD >= dtmStart && c.saD <= dtmEnd),
          PendingAmount = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && (c.saProcD == null || c.saProcD > dtmEnd) && c.saD >= dtmStart && c.saD <= dtmEnd).Sum(c=>c.saGrossAmount)
        }).Distinct().ToList();

      lstRptSelfGen = lstRptSelfGen.Distinct().OrderBy(c => c.PRps)
        .ThenByDescending(c => c.TotalProcAmount).ToList();


      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptSelfGen.Distinct().ToList(), true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptSelfGen());
    }
    #endregion

    #endregion

    #region General Reports

    #region ExportRptAgencies
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Agencies
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptAgencies">Lista de RptAgencies</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 20/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptAgencies(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptAgencies> lstRptAgencies)
    {
      return EpplusHelper.CreateGeneralRptExcel(filters, TableHelper.GetDataTableFromList(lstRptAgencies, true), strReport, dateRangeFileName, clsFormatReport.RptAgencies());
    }
    #endregion

    #region ExportRptPersonnel
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Personnel
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptPersonnel">Lista de RptPersonnel</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 20/Mar/2016 Created
    /// </history>
    public static FileInfo ExportRptPersonnel(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptPersonnel> lstRptPersonnel)
    {
      return EpplusHelper.CreateGeneralRptExcel(filters, TableHelper.GetDataTableFromList(lstRptPersonnel, true, false), strReport, dateRangeFileName, clsFormatReport.RptPersonnel());
    }
    #endregion

    #region ExportRptGifts
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Gifts
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptGifts">Lista de RptGifts</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 17/Mar/2016 Created
    /// </history>
    public static FileInfo ExportRptGifts(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptGifts> lstRptGifts)
    {
      return EpplusHelper.CreateGeneralRptExcel(filters, TableHelper.GetDataTableFromList(lstRptGifts, true), strReport, dateRangeFileName, clsFormatReport.RptGifts());
    }
    #endregion

    #region ExportRptProductionByLeadSourceMarketMonthly
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Gifts
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="lstRptProdLSMMonthly">Lista de RptProductionByLeadSourceMarketMonthly</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByLeadSourceMarketMonthly(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByLeadSourceMarketMonthly> lstRptProdLSMMonthly)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProdLSMMonthly, true), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionByLeadSourceMarketMonthly(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptProductionReferral

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Gifts
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptProductionReferrals">Lista de RptProductionByLeadSourceMarketMonthly</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionReferral(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionReferral> lstRptProductionReferrals)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProductionReferrals, true), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionReferral());
    }
    #endregion

    #region ExportRptReps

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Reps
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptReps">Lista de Rep</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 23/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptReps(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<Rep> lstRptReps)
    {
      return EpplusHelper.CreateGeneralRptExcel( filters, TableHelper.GetDataTableFromList(lstRptReps, true), strReport, dateRangeFileName, clsFormatReport.RptReps());
    }
    #endregion

    #region ExportRptSalesByProgramLeadSourceMarket

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Sales By Program,LeadSources & Market
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptSalesByProgramLeadSourceMarkets">Lista de RptSalesByProgramLeadSourceMarket</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 23/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptSalesByProgramLeadSourceMarket(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptSalesByProgramLeadSourceMarket> lstRptSalesByProgramLeadSourceMarkets)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptSalesByProgramLeadSourceMarkets, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptSalesByProgramLeadSourceMarket(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptWarehouseMovements

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Warehouses Movements
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptWarehouseMovements">Lista de RptWarehouseMovements</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 23/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptWarehouseMovements(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptWarehouseMovements> lstRptWarehouseMovements)
    {
      return EpplusHelper.CreateGeneralRptExcel( filters, TableHelper.GetDataTableFromList(lstRptWarehouseMovements), strReport, dateRangeFileName, clsFormatReport.RptWarehouseMovements());
    }
    #endregion

    #endregion
  }
}