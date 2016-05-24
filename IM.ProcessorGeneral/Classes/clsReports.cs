using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using System.IO;
using IM.Base.Helpers;
using System.Data;
using System.Collections;

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
    /// [edgrodriguez] 17/May/2016 Created
    /// </history>
    public static FileInfo ExportRptCxC(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptCxCExcel> lstRptCxC)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptCxC, changeDataTypeBoolToString: true, showCheckMark: false, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreateExcelCustom(dtData, filters, strReport, dateRangeFileName, clsFormatReport.RptCxc(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptCxCByType
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
    public static FileInfo ExportRptCxCByType(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptCxC> lstRptCxC)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptCxC, changeDataTypeBoolToString: true, showCheckMark: false, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.RptCxcByType(), showRowGrandTotal: true);
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

      var lstGiftSales = lstRptBurnedDeposits[3] as Dictionary<int, List<Tuple<RptDepositsBurnedByResort, Sale>>>;

      var receiptsID = lstGiftReceipts.Select(c => c.grID).Distinct().ToList();

      lstGiftReceipts.AddRange(lstGiftSales.SelectMany(c => c.Value.Select(s => s.Item1)).Where(c => !receiptsID.Contains(c.grID)));

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
                          guHotelB = gRcpt.guHotelB ?? " ",
                          gRcpt.grlo,
                          gRcpt.grsr,
                          gRcpt.grpe,
                          gRcpt.peN,
                          gRcpt.grHost,
                          curr.cuN,
                          payType.ptN,
                          gRcpt.grDepositTwisted,
                          memberNum = ((lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) == 1 || (lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) > 1 && lstGiftReceipts.First(gr => gr.grgu == gRcpt.grgu).grID == gRcpt.grID)) ? ((lstGiftSales.ContainsKey(gRcpt.grID)) ? string.Join(",", lstGiftSales[gRcpt.grID].Select(s => s.Item2.saMembershipNum).ToList()) : "-") : "-"),
                          procAmount = ((lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) == 1 || (lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) > 1 && lstGiftReceipts.First(gr => gr.grgu == gRcpt.grgu).grID == gRcpt.grID)) ? ((lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => s.Item2.saProc && (s.Item2.saProcD >= dtmStart && s.Item2.saProcD <= dtmEnd)).Sum(s => s.Item2.saGrossAmount) : 0) : 0),
                          pendAmount = ((lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) == 1 || (lstGiftReceipts.Count(gr => gr.grgu == gRcpt.grgu) > 1 && lstGiftReceipts.First(gr => gr.grgu == gRcpt.grgu).grID == gRcpt.grID)) ? ((lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => !s.Item2.saProc || (s.Item2.saProc && !(s.Item2.saProcD >= dtmStart && s.Item2.saProcD <= dtmEnd))).Sum(s => s.Item2.saGrossAmount) : 0) : 0)
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

    #region ExportRptGiftsManifest

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Gifts Manifest.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptGiftsManifest">Lista de IEnumerable</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 10/May/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsManifest(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<IEnumerable> lstRptGiftsManifest)
    {
     var lstGiftManifest=lstRptGiftsManifest[0] as List<RptGiftsManifest>;
      var lstGifts = lstRptGiftsManifest[1] as List<GiftForReceipts>;

      lstGiftManifest.ForEach(c => {
        c.Currency = (lstRptGiftsManifest[2] as List<CurrencyShort>).First(cu => cu.cuID == c.Currency).cuN;
        c.PaymentType = (lstRptGiftsManifest[3] as List<PaymentTypeShort>).First(pt => pt.ptID == c.PaymentType).ptN;
      });

      var rptGiftsManifest = (from gr in lstGiftManifest
                              join g in lstGifts on gr.Gift equals g.giID into giftM
                              from gm in giftM.DefaultIfEmpty()
                              select new
                              {
                                gr.GuestID,
                                gr.ReservID,
                                gr.Date,
                                gr.Guest,
                                gr.Guest2,
                                gr.Pax,
                                gr.Agency,
                                gr.Membership,
                                gr.Location,
                                gr.PR,
                                gr.PRN,
                                gr.Host,
                                gr.HostN,
                                gr.ReceiptID,
                                gr.Show,
                                gr.HotelBurned,
                                gr.Cancel,
                                gr.CancelledDate,
                                gr.Deposited,
                                gr.Burned,
                                gr.Currency,
                                gr.PaymentType,
                                gr.TaxiOut,
                                gr.TaxiOutDiff,
                                Gift=(gm!=null) ? gm.giID:null,
                                GiftN=(gm!=null)?gm.giN:null,
                                Quantity = (gm != null && gm.giWPax) ? (gr.Adults + gr.Minors) : gr.Quantity,
                                gr.Adults,
                                gr.Minors,
                                gr.Folios,
                                Cost = (gm != null) ? (gr.PriceAdults + gr.PriceMinors) : 0,
                                gr.Comments
                              }).ToList();

      //Convertimos la lista a datatable.
      var dt = TableHelper.GetDataTableFromList(rptGiftsManifest, true, false);
      //Obtenemos el formato del reporte.
      var format = clsFormatReport.RptGiftsManifest();
      //Asignamos el tipo de eje que tendran algunas columnas.
      format.ForEach(c =>
      { 
        if (c.PropertyName == "Deposited")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
          c.Order = 19;
        }
        else if (c.PropertyName == "Currency")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          c.Order = 1;
        }
        else if (c.PropertyName == "PaymentType")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          c.Order = 2;
        }
      });
      
      //Obtenemos el primer pivot.
      var firstPivotdt = EpplusHelper.GetPivotTable(format, dt);
      int Order = format.Where(c => c.Axis == OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values).Min(c => c.Order);

      //Creamos la lista de columnas que se excluiran.
      List<string> pivot2 = new List<string>() { "Deposited", "Currency", "PaymentType" };
      
      //Obtenemos la nueva lista excluyendo las columnas que ya sirvieron como pivote.
      var formatPivot2 = format.Where(c => !pivot2.Contains(c.PropertyName)).ToList();
      var afterPivot = format.Where(c => c.Order > Order).OrderBy(c=>c.Order).ToList();
      //Obtenemos las columnas que ya se les aplico el pivot.Y las agregamos a la lista de formatos.
      firstPivotdt.Columns.Cast<DataColumn>().ToList().ForEach(col =>
      {
        if (!formatPivot2.Exists(c => c.PropertyName == col.ColumnName))
        {
          formatPivot2.Add(new Model.Classes.ExcelFormatTable { PropertyName = col.ColumnName, Order = Order });
          Order++;
        }
      });

      afterPivot.ForEach(c => {
        formatPivot2.ForEach(f =>
        {
          if (f.PropertyName == c.PropertyName)
          {
            f.Order = Order;
            Order++;
          }
        });
      });
      
      //Cambiamos las propiedades de las columnas para el nuevo pivote.
      formatPivot2.ForEach(c =>
      {
        if (c.PropertyName == "Quantity")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }
        else if (c.PropertyName == "Cost")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }
        else if (c.PropertyName == "Folios")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }
        else if (c.PropertyName == "Gift")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          c.Order = 1;
        }
        else if (c.PropertyName == "GiftN")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          c.Order = 2;
        }
      });           

      //Obtenemos el segudo pivot.
      var secondPivotdt = EpplusHelper.GetPivotTable(formatPivot2.OrderBy(c=>c.Order).ToList(), firstPivotdt);

      return EpplusHelper.CreateExcelCustomPivot(null, filters, strReport, dateRangeFileName, format, pivotedTable: secondPivotdt, blnRowGrandTotal: true);
    }
    #endregion

    #region ExportRptGiftsReceipts

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Gifts Manifest.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="dateRangeFileName"></param>
    /// <param name="lstRptGiftsReceipts"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 11/May/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsReceipts(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<IEnumerable> lstRptGiftsReceipts)
    {
      var lstGiftReceipt = lstRptGiftsReceipts[0] as List<RptGiftsReceipts>;
      var lstGifts = lstRptGiftsReceipts[1] as List<GiftForReceipts>;

      lstGiftReceipt.ForEach(c => {
        c.Currency = (lstRptGiftsReceipts[2] as List<CurrencyShort>).First(cu => cu.cuID == c.Currency).cuN;
        c.PaymentType = (lstRptGiftsReceipts[3] as List<PaymentTypeShort>).First(pt => pt.ptID == c.PaymentType).ptN;
      });

      var rptGiftsReceipts = (from gr in lstGiftReceipt
                              join g in lstGifts on gr.Gift equals g.giID into giftM
                              from gm in giftM.DefaultIfEmpty()
                              select new
                              {
                                gr.GuestID,
                                gr.ReservID,
                                gr.Date,
                                gr.Guest,
                                gr.Guest2,
                                gr.Pax,
                                gr.Agency,
                                gr.Membership,
                                gr.Location,
                                gr.PR,
                                gr.PRN,
                                gr.Host,
                                gr.HostN,
                                gr.ReceiptID,
                                gr.Show,
                                gr.HotelBurned,
                                gr.Cancel,
                                gr.CancelledDate,
                                gr.Deposited,
                                gr.Burned,
                                gr.Currency,
                                gr.PaymentType,
                                gr.TaxiOut,
                                gr.TaxiOutDiff,
                                Gift = (gm != null) ? gm.giID:null,
                                GiftN = (gm != null) ? gm.giN:null,
                                Quantity = (gm != null && gm.giWPax) ? (gr.Adults + gr.Minors) : gr.Quantity,
                                Cost = (gm != null) ? (gr.PriceAdults + gr.PriceMinors) : 0,
                                gr.WithCost,
                                gr.PublicPrice,
                                gr.Adults,
                                gr.Minors,
                                gr.Folios,                                
                                gr.Comments
                              }).ToList();
      //Convertimos la lista a datatable.
      var dt = TableHelper.GetDataTableFromList(rptGiftsReceipts, true, false);
      //Obtenemos el formato del reporte.
      var format = clsFormatReport.RptGiftsReceipts();
      //Asignamos el tipo de eje que tendran algunas columnas.
      format.ForEach(c =>
      {
        if (c.PropertyName == "Deposited")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }

        else if (c.PropertyName == "Burned")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }
        else if (c.PropertyName == "Currency")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          c.Order = 1;
        }
        else if (c.PropertyName == "PaymentType")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          c.Order = 2;
        }
      });

      //Obtenemos el primer pivot.
      var firstPivotdt = EpplusHelper.GetPivotTable(format, dt);

      int Order = format.Where(c => c.Axis == OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values).Min(c => c.Order);
      
      //Creamos la lista de columnas que se excluiran.
      List<string> pivot2 = new List<string>() { "Deposited", "Burned", "Currency", "PaymentType" };

      //Obtenemos la nueva lista excluyendo las columnas que ya sirvieron como pivote.
      var formatPivot2 = format.Where(c => !pivot2.Contains(c.PropertyName)).ToList();

      var afterPivot = format.Where(c => c.Order > Order).OrderBy(c => c.Order).ToList();
      //Obtenemos las columnas que ya se les aplico el pivot.Y las agregamos a la lista de formatos.
      firstPivotdt.Columns.Cast<DataColumn>().ToList().ForEach(col =>
      {
        if (!formatPivot2.Exists(c => c.PropertyName == col.ColumnName))
        {
          formatPivot2.Add(new Model.Classes.ExcelFormatTable { PropertyName = col.ColumnName, Order = Order });
          Order++;
        }
      });

      afterPivot.ForEach(c => {
        formatPivot2.ForEach(f =>
        {
          if (f.PropertyName == c.PropertyName)
          {
            f.Order = Order;
            Order++;
          }
        });
      });

      //Cambiamos las propiedades de las columnas para el nuevo pivote.
      formatPivot2.ForEach(c =>
      {
        if (c.PropertyName == "Quantity")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }
        else if (c.PropertyName == "Cost")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }
        else if (c.PropertyName == "WithCost")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }
        else if (c.PropertyName == "PublicPrice")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }
        else if (c.PropertyName == "Folios")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
        }
        else if (c.PropertyName == "Gift")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          c.Order = 1;
        }
        else if (c.PropertyName == "GiftN")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          c.Order = 2;
        }
      });


      //Obtenemos el segudo pivot.
      var secondPivotdt = EpplusHelper.GetPivotTable(formatPivot2.OrderBy(c => c.Order).ToList(), firstPivotdt);

      return EpplusHelper.CreateExcelCustomPivot(null, filters, strReport, dateRangeFileName, format, blnRowGrandTotal: true, pivotedTable: secondPivotdt);
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
    public static FileInfo ExportRptGiftsSale(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<IEnumerable> lstRptGiftsSale)
    {
      var lstGiftReceipts = lstRptGiftsSale[0] as List<RptGiftsSale>;
      var lstGiftPayments = lstRptGiftsSale[1] as List<RptGiftsSale_Payment>;
      lstGiftPayments.ForEach(gp =>
      {
        gp.Source = (lstRptGiftsSale[2] as List<SourcePaymentShort>).FirstOrDefault(c => c.sbID == gp.Source).sbN;
        gp.Currency = (lstRptGiftsSale[3] as List<CurrencyShort>).FirstOrDefault(c => c.cuID == gp.Currency).cuN;
        gp.PaymentType = (lstRptGiftsSale[4] as List<PaymentTypeShort>).FirstOrDefault(c => c.ptID == gp.PaymentType).ptN;
      });
      //Creamos una lista de Tuples, utilizando el id del recibo y el regalo como llaves principales.
      var lstSales_payments = new List<Tuple<int, string, RptGiftsSale_Payment>>();
      //Obtenemos los folios de recibo de la lista de pagos.
      var receipts = lstGiftPayments.Select(c => c.Receipt).Distinct().ToList();

      //Recorremos los recibos.
      foreach (var receipt in receipts)
      {
        //Obtenemos los pagos del recibo.
        List<RptGiftsSale_Payment> payments = lstGiftPayments.Where(c => c.Receipt == receipt).ToList();
        //Obtenemos los Regalos del recibo.
        List<RptGiftsSale> sales = lstGiftReceipts.Where(c => c.Receipt == receipt).ToList();

        if (payments.Count > 0)
        {
          decimal curPayment = 0;//Pago Actual
          decimal curPaymentUS = 0;//Pago Actual Dlls
          decimal curBalance = 0;//Balance Actual
          decimal curBalanceUS = 0;//Balance Actual Dlls

          //Recorremos los pagos.
          foreach (var payment in payments)
          {
            //Asignamos el monto del pago al balance actual.
            curBalance = payment.Amount ?? 0;
            //Asignamos el monto del pago en Dlls al balance actual.
            curBalanceUS = payment.AmountUS ?? 0;

            //Recorremos los recibos de regalo.
            for (int i = 0; i < sales.Count; i++)
            {
              
              if (sales[i].Difference == 0) continue;

              do
              {
                if (Math.Abs(sales[i].Difference ?? 0) > Math.Abs(curBalanceUS))
                {
                  curPayment = curBalance;
                  curPaymentUS = curBalanceUS;
                  //Se resta el pago.
                  sales[i].Difference -= curBalanceUS;
                  curBalance = 0;
                  curBalanceUS = 0;
                }
                else if (Math.Abs(sales[i].Difference ?? 0) <= Math.Abs(curBalanceUS))
                {
                  curPayment = sales[i].Difference ?? 0 / payment.ExchangeRate;
                  curPaymentUS = sales[i].Difference ?? 0;
                  curBalance = curBalance - curPayment;
                  curBalanceUS = curBalanceUS - curPaymentUS;

                  //Se resta el pago
                  sales[i].Difference -= curPaymentUS;
                }
                lstSales_payments.Add(Tuple.Create(sales[i].Receipt, sales[i].Gift,
                  new RptGiftsSale_Payment
                  {
                    Amount = curPayment,
                    User = payment.User,
                    UserN = payment.UserN,
                    Source = payment.Source,
                    Currency = payment.Currency,
                    PaymentType = payment.PaymentType,
                    Receipt = payment.Receipt,
                    AmountUS = curPaymentUS,
                    ExchangeRate = payment.ExchangeRate
                  }));

                if (sales[i].Difference == 0) break;
              } while (curBalanceUS > 0);

              if (i == sales.Count - 1 && curBalanceUS > 0 && sales[i].Difference == 0)
              {
                lstSales_payments.Add(Tuple.Create(sales[i].Receipt, sales[0].Gift,
                  new RptGiftsSale_Payment
                  {
                    Amount = curBalanceUS/payment.ExchangeRate,
                    User = payment.User,
                    UserN = payment.UserN,
                    Source = payment.Source,
                    Currency = payment.Currency,
                    PaymentType = payment.PaymentType,
                    Receipt = payment.Receipt,
                    AmountUS = curBalanceUS,
                    ExchangeRate = payment.ExchangeRate
                  }));
              }
              if (sales[i].Difference == 0) continue;
              if (sales[i].Difference < 0 || curBalanceUS == 0) break;
            }
          }
        }
      }

      var source = (from gr in lstGiftReceipts
                    join gp in lstSales_payments on new { Receipt = gr.Receipt, Gift = gr.Gift } equals new { Receipt = gp.Item1, Gift = gp.Item2 } into pays
                    from py in pays.DefaultIfEmpty()
                    select new
                    {
                      gr.Program,
                      gr.Receipt,
                      Date = $"{gr.Date.ToString("dd/MM/yyyy")} {gr.ExchangeRate}",
                      gr.Cancel,
                      CancelDate = (gr.CancelDate != null) ? gr.CancelDate?.ToString("dd/MM/yyy") : "",
                      gr.SalesRoom,
                      gr.LeadSource,
                      gr.PR,
                      gr.PRN,
                      gr.OutInvit,
                      gr.GuestID,
                      gr.LastName,
                      gr.FirstName,
                      gr.Gift,
                      gr.GiftN,
                      gr.GiftSale,
                      gr.Adults,
                      gr.Minors,
                      gr.ExtraAdults,
                      gr.PriceUS,
                      gr.PriceMX,
                      gr.PriceCAN,
                      gr.TotalToPay,
                      PaymentTotal = (py != null) ? lstSales_payments.Where(c => c.Item1 == gr.Receipt && c.Item2 == gr.Gift).Sum(c => c.Item3.AmountUS) : 0,
                      Difference = (py != null) ? gr.TotalToPay - (lstSales_payments.Where(c => c.Item1 == gr.Receipt && c.Item2 == gr.Gift).Sum(c => c.Item3.AmountUS)) : 0,
                      User = (py != null) ? string.Join(",", lstSales_payments.Where(c => c.Item1 == gr.Receipt && c.Item2 == gr.Gift).Select(c => c.Item3.User).Distinct().ToList()) : "",
                      UserN = (py != null) ? string.Join(",", lstSales_payments.Where(c => c.Item1 == gr.Receipt && c.Item2 == gr.Gift).Select(c => c.Item3.UserN).Distinct().ToList()) : "",
                      Amount = (py != null) ? py.Item3.Amount : 0,
                      Source = (py != null) ? py.Item3.Source : "",
                      Currency = (py != null) ? py.Item3.Currency : "",
                      PaymentType = (py != null) ? py.Item3.PaymentType : ""
                    }
        ).ToList();




      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(source,true,false), filters, strReport, dateRangeFileName, clsFormatReport.RptGiftsSale(), blnShowSubtotal: true);
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
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
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
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
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
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
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
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptMealTickets">Lista de RptMealTickets</param>
    /// <param name="groupByHost"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 11/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptMealTickets(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptMealTickets> lstRptMealTickets, bool groupByHost = false)
    {
      var file = EpplusHelper.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptMealTickets, true, false,true), strReport, dateRangeFileName, groupByHost ? clsFormatReport.RptMealTicketsByHost() : clsFormatReport.RptMealTickets(), true);
      return file;
    }

    #endregion

    #region ExportRptMealTicketsCost

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Meal Tickets With Cost.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
    /// <param name="lstRptProductionBySr">Lista de RptProductionBySalesRoom</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionBySalesRoom(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionBySalesRoom> lstRptProductionBySr)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProductionBySr, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionBySalesRoom(), blnRowGrandTotal: true);
    }
    #endregion

    #region ExportRptProductionBySalesRoomMarket
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Production by SalesRoom & Market.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptProductionBySrm">Lista de RptProductionBySalesRoomMarket</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionBySalesRoomMarket(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionBySalesRoomMarket> lstRptProductionBySrm)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProductionBySrm, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionBySalesRoomMarket(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptProductionBySalesRoomMarketSubMarket
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Production by SalesRoom,Market & Submarket.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptProductionBySrmSm">Lista de RptProductionBySalesRoomMarket</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionBySalesRoomMarketSubMarket(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionBySalesRoomProgramMarketSubmarket> lstRptProductionBySrmSm)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProductionBySrmSm, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionBySalesRoomMarketSubMarket(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptProductionByShowProgram
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Production by Show & Program.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
    /// <param name="lstRptProductionByShowProgramProgram"></param>
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

    #region Salesmen

    #region ExportRptCloserStatistics

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Closer Statistics.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptCloserStatistic">Lista de RptCloserStatistics</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 12/May/2016 Created
    /// </history>
    public static FileInfo ExportRptCloserStatistics(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptCloserStatistics> lstRptCloserStatistic)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptCloserStatistic, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptCloserStatistic(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptLinerStatistics

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Liner Statistics.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptCloserStatistic">Lista de RptCloserStatistics</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 12/May/2016 Created
    /// </history>
    public static FileInfo ExportRptLinerStatistics(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptLinerStatistics> lstRptCloserStatistic)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptCloserStatistic, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptLinerStatistic(), blnShowSubtotal: true);
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
    /// <param name="filters"></param>
    /// <param name="lstRptTaxiIn"></param>
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
    /// <param name="filters"></param>
    /// <param name="lstRptTaxiOut"></param>
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
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
    /// <param name="lstRptDepPr">Lista de object</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDepositByPr(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptDepPr)
    {
      var lstDepPr = lstRptDepPr[0] as List<RptDepositsByPR>;
      var currencies = lstRptDepPr[1] as List<Currency>;
      var payType = lstRptDepPr[2] as List<PaymentType>;

      lstDepPr.ForEach(c => {
        c.guPRInvit1 = $"{c.guPRInvit1} {c.peN}";
        c.gucu = currencies.First(cu => cu.cuID == c.gucu).cuN ?? "";
        c.gupt = payType.First(pt => pt.ptID == c.gupt).ptN ?? "";
      });

      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstDepPr, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptDepositByPr(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptDepositsNoShow
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Deposits No Show
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptDepNoShow">Lista de object</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDepositsNoShow(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptDepNoShow)
    {
      var lstDepPrNoShow = lstRptDepNoShow[0] as List<RptDepositsNoShow>;
      var currencies = lstRptDepNoShow[1] as List<Currency>;
      var payType = lstRptDepNoShow[2] as List<PaymentType>;

      lstDepPrNoShow.ForEach(c => {
        c.guPRInvit1 = $"{c.guPRInvit1} {c.peN}";
        c.gucu = currencies.First(cu => cu.cuID == c.gucu).cuN ?? "";
        c.gupt = payType.First(pt => pt.ptID == c.gupt).ptN ?? "";
      });

      return EpplusHelper.CreateExcelCustomPivot(TableHelper.GetDataTableFromList(lstDepPrNoShow, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptDepositByPr(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptInOutByPR
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte In & Out By PR
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptInOutPr">Lista de object</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 19/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptInOutByPr(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptInOutByPR> lstRptInOutPr)
    {

      lstRptInOutPr.ForEach(c => {
        c.PR1 = $"{c.PR1} {c.PR1N}";
      });

      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptInOutPr, true, false), filters, strReport, dateRangeFileName, clsFormatReport.RptInOutByPr(), blnShowSubtotal: true);
    }
    #endregion

    #region ExportRptPersonnelAccess
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Personnel Access
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
    /// <param name="lstRptPersonnel">Lista de RptPersonnel</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 20/Mar/2016 Created
    /// </history>
    public static FileInfo ExportRptPersonnel(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptPersonnel> lstRptPersonnel)
    {
      return EpplusHelper.CreateGeneralRptExcel(filters, TableHelper.GetDataTableFromList(lstRptPersonnel, true), strReport, dateRangeFileName, clsFormatReport.RptPersonnel());
    }
    #endregion

    #region ExportRptGifts
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Gifts
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
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
    /// <param name="filters"></param>
    /// <param name="lstRptProdLsmMonthly">Lista de RptProductionByLeadSourceMarketMonthly</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByLeadSourceMarketMonthly(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<RptProductionByLeadSourceMarketMonthly> lstRptProdLsmMonthly)
    {
      return EpplusHelper.CreateExcelCustom(TableHelper.GetDataTableFromList(lstRptProdLsmMonthly, true), filters, strReport, dateRangeFileName, clsFormatReport.RptProductionByLeadSourceMarketMonthly(), blnShowSubtotal: true, blnRowGrandTotal: true);
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