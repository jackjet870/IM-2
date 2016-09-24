using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using System.IO;
using IM.Base.Helpers;
using System.Data;
using System.Collections;
using IM.Base.Reports;
using System.Threading.Tasks;
using IM.Model.Classes;
using IM.Model.Helpers;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Enums;
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
    public static async Task<FileInfo> ExportRptBookingsBySalesRoomProgramTime(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptBookingsBySalesRoomProgramTime> lstRptBbSalesRoom)
    {

      lstRptBbSalesRoom = lstRptBbSalesRoom.orderListBy("SalesRoom asc,Program asc, BookType asc, Time asc");
      var dtData = TableHelper.GetDataTableFromList(lstRptBbSalesRoom);
      return await ReportBuilder.CreateCustomExcel(dtData, filters, strReport, string.Empty, clsFormatReport.RptBookingsBySalesRoomProgramTime(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptBookingsBySalesRoomProgramLeadSourceTime(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptBookingsBySalesRoomProgramLeadSourceTime> lstRptBOokingsSalesRoomLeadSource)
    {
      var dtData = TableHelper.GetDataTableFromList(lstRptBOokingsSalesRoomLeadSource);
      return await ReportBuilder.CreateCustomExcel(dtData, filters, strReport, string.Empty, clsFormatReport.RptBookingsBySalesRoomProgramLeadSourceTime(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
    }
    #endregion

    #endregion

    #region CxC

    #region ExportRptCxc
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
    public static async Task<FileInfo> ExportRptCxc(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptCxC> lstRptCxC)
    {
      var dtData = TableHelper.GetDataTableFromList(lstRptCxC, true, false, true);
      return await ReportBuilder.CreateCustomExcel(dtData, filters, strReport, string.Empty, clsFormatReport.RptCxc(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
    }
    #endregion

    #region ExportRptCxcByType

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptCxC
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="fileFullPath"></param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptCxC">Lista de datos para el reporte.</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 17/May/2016 Created
    /// </history>
    public static async Task<FileInfo> ExportRptCxcByType(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptCxCExcel> lstRptCxC)
    {
      var dtData = TableHelper.GetDataTableFromList(lstRptCxC, true);
      return await ReportBuilder.CreateCustomExcel(dtData, filters, strReport, string.Empty, clsFormatReport.RptCxcByType(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath);
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
    public static async Task<FileInfo> ExportRptCxCDeposits(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptCxCDeposits)
    {
      var lstCxCDeposits = lstRptCxCDeposits[0] as List<RptCxCDeposits>;
      var currencies = lstRptCxCDeposits[1] as List<Currency>;

      var dtData = TableHelper.GetDataTableFromList((from cxcDep in lstCxCDeposits
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

      return await ReportBuilder.CreateCustomExcel(dtData, filters, strReport, string.Empty, clsFormatReport.RptCxcDeposits(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptCxCGift(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptCxCGift)
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

      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(receiptsWithGift.OrderBy(c => c.giN).ToList()), filters, strReport, string.Empty, clsFormatReport.RptCxcGifts(), blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true);
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
    public static async Task<FileInfo> ExportRptCxCNotAuthorized(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptCxCNotAuthorized> lstRptCxCNotAuthorized)
    {
      var dtData = TableHelper.GetDataTableFromList(lstRptCxCNotAuthorized, true, false, true);
      return await ReportBuilder.CreateCustomExcel(dtData, filters, strReport, string.Empty, clsFormatReport.RptCxcNotAuthorized(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptCxCPayments(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptCxCPayments> lstRptCxCPayments)
    {
      var dtData = TableHelper.GetDataTableFromList(lstRptCxCPayments, true, false, true);
      return await ReportBuilder.CreateCustomExcel(dtData, filters, strReport, string.Empty, clsFormatReport.RptCxcPayments(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptDeposits(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptDeposits)
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

      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptDeps, true), filters, strReport, string.Empty, clsFormatReport.RptDeposits(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptBurnedDeposits(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptBurnedDeposits, DateTime dtmStart, DateTime dtmEnd)
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
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptDeps, true, false), filters, strReport, string.Empty, clsFormatReport.RptBurnedDeposits(), blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
    }
    #endregion 

    #region ExportRptBurnedDepositsByResorts

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Burned Deposits by Resort.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="fileFullPath"></param>
    /// <param name="filters"></param>
    /// <param name="lstRptBurnedDeposits">Lista de BurnedDepositsByResorts,Currency, PaymentType y Sales</param>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public static async Task<FileInfo> ExportRptBurnedDepositsByResorts(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptBurnedDeposits, DateTime dtmStart, DateTime dtmEnd)
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
                        ).OrderBy(c=> c.guHotelB)
                        .ThenBy(c => c.grD)
                        .ThenBy(c => c.grID)
                        .ToList();

      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptDeps, true, false), filters, strReport, string.Empty, clsFormatReport.RptBurnedDepositsByResorts(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptPaidDeposits(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptPaidDeposits, bool byPr = false)
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
                        )
                        .OrderBy(c=>c.grpe)
                        .ThenBy(c => c.grD)
                        .ThenBy(c => c.grID)
                        .ToList();

      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptDeps, true, false), filters, strReport, string.Empty, (byPr) ? clsFormatReport.RptPaidDepositsByPr() : clsFormatReport.RptPaidDeposits(), blnRowGrandTotal: true, blnShowSubtotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);

    }
    #endregion

    #endregion

    #region Gifts

    #region ExportRptCancelledGiftsManifest

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Cancelled Gifts Manifest.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="filters"></param>
    /// <param name="lstRptCancelledGifts">Lista de RptDailyGiftSimple</param>
    /// <param name="dateRangeFileName"></param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 06/Jun/2016 Created
    /// </history>
    public static async Task<FileInfo> ExportRptCancelledGiftsManifest(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<IEnumerable> lstRptCancelledGifts)
    {
      var cancelledGifts = lstRptCancelledGifts[0] as List<RptGiftsManifestCancel>;
      var gifts = lstRptCancelledGifts[1] as List<RptGiftsManifestCancel_Gifts>;

      if (cancelledGifts != null && cancelledGifts.Any(c => c.grDeposit > 0 || c.grDepositTwisted > 0 || c.grTaxiOut > 0))
      {
        cancelledGifts.ForEach(c =>
        {
          c.grcu = (lstRptCancelledGifts[2] as List<CurrencyShort>).FirstOrDefault(cu => cu.cuID == c.grcu)?.cuN;
          c.grpt = (lstRptCancelledGifts[3] as List<PaymentTypeShort>).FirstOrDefault(pt => pt.ptID == c.grpt)?.ptN;
        });
      }
      var rptGiftsManifest = (from gr in cancelledGifts
                              join g in gifts on gr.gegi equals g.giID into giftM
                              from gm in giftM.DefaultIfEmpty()
                              select new
                              {
                                gr.grgu,
                                gr.grGuest,
                                gr.grsr,
                                gr.grlo,
                                gr.grHost,
                                gr.peN,
                                gr.grID,
                                gr.grD,
                                gr.grDeposit,
                                gr.grDepositTwisted,
                                gr.grcu,
                                gr.grpt,
                                gr.grTaxiOut,
                                gr.geQty,
                                gr.geFolios,
                                Cost = (gm != null) ? gr.gePriceA + gr.gePriceM : 0,
                                gr.grComments,
                                Gift = gm?.giID,
                                GiftN = gm?.giN,
                              }).ToList();

      //Convertimos la lista a datatable.
      var dt = TableHelper.GetDataTableFromList(rptGiftsManifest, true, false);
      //Obtenemos el formato del reporte.
      var format = clsFormatReport.RptCancelledGiftsManifest();
      var pivot1 = new List<string> { "grDeposit", "grDepositTwisted", "grTaxiOut", "grcu", "grpt" };
      var pivot2 = new List<string>();
      //Asignamos el tipo de eje que tendran algunas columnas.
      format.ForEach(c =>
      {
        if (!pivot1.Contains(c.PropertyName)) return;

        if (cancelledGifts.Any(g => g.grDeposit > 0) && c.PropertyName == "grDeposit")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
          pivot2.Add("grDeposit");
        }
        else if (cancelledGifts.Any(g => g.grDepositTwisted > 0) && c.PropertyName == "grDepositTwisted")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
          pivot2.Add("grDepositTwisted");
        }
        else if (cancelledGifts.Any(g => g.grTaxiOut > 0) && c.PropertyName == "grTaxiOut")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Row;
          pivot2.Add("grTaxiOut");
        }
        else if ((cancelledGifts.Any(g => g.grDeposit > 0) || cancelledGifts.Any(g => g.grDepositTwisted > 0)) && c.PropertyName == "grcu")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          //c.Order = 1;
          pivot2.Add("grcu");
        }
        else if ((cancelledGifts.Any(g => g.grDeposit > 0) || cancelledGifts.Any(g => g.grDepositTwisted > 0)) && c.PropertyName == "grpt")
        {
          c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
          //c.Order = 2;
          pivot2.Add("grpt");
        }
        else
        {
          c.IsVisible = false;
        }
      });

      var firstPivotdt = new DataTable();
      var formatPivot2 = new ExcelFormatItemsList();
      if (cancelledGifts.Any(g => g.grDeposit > 0) || cancelledGifts.Any(g => g.grDepositTwisted > 0))
      {
        //Obtenemos el primer pivot.
        firstPivotdt = ReportBuilder.GetPivotTable(format, dt);

        var order = format.Where(c => c.Axis == OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values).Min(c => format.IndexOf(c));

        //Obtenemos la nueva lista excluyendo las columnas que ya sirvieron como pivote.
        formatPivot2.AddRange(format.Where(c => !pivot2.Contains(c.PropertyName)).ToList());
        //var afterPivot = format.Where(c => format.IndexOf(c) > order).ToList();
        //Obtenemos las columnas que ya se les aplico el pivot.Y las agregamos a la lista de formatos.
        firstPivotdt.Columns.Cast<DataColumn>().ToList().ForEach(col =>
        {
          if (formatPivot2.Exists(c => c.PropertyName == col.ColumnName)) return;
          formatPivot2.Insert(order, new ExcelFormatTable { PropertyName = col.ColumnName });
          order++;
        });       
      }
      else
      {
        firstPivotdt = dt;
        formatPivot2 = format;
      }
      //Cambiamos las propiedades de las columnas para el nuevo pivote.
      formatPivot2.ForEach(c =>
      {
        switch (c.PropertyName)
        {
          case "geQty":
          case "Cost":
          case "geFolios":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
            break;
          case "Gift":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 1;
            break;
          case "GiftN":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 2;
            break;
        }
      });

      //Obtenemos el segudo pivot.
      var secondPivotdt = ReportBuilder.GetPivotTable(formatPivot2, firstPivotdt);
      return await ReportBuilder.CreateCustomExcel(null, filters, strReport, string.Empty, clsFormatReport.RptCancelledGiftsManifest(), blnRowGrandTotal: true, pivotedTable: secondPivotdt, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
    }
    #endregion

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
    public static FileInfo ExportRptDailyGiftSimple(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptDailyGiftSimple> lstRptDailyGiftSimples)
    {
      return ReportBuilder.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptDailyGiftSimples, true, false),strReport, string.Empty, clsFormatReport.RptDailyGiftSimple(), true, isRptQueue:true ,filePath:fileFullPath);
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
    public static async Task<FileInfo> ExportRptGiftsByCategory(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptGiftsByCategory> lstRptGiftsByCat)
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



      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstGifts, true, false), filters, strReport, string.Empty, clsFormatReport.RptGiftsByCategory(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptGiftsByCategoryProgram(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptGiftsByCategoryProgram> lstRptGiftsByCatP)
    {
      var lstGifts = lstRptGiftsByCatP.Select(c => new
      {
        Gift = c.gegi,
        TotalQty = lstRptGiftsByCatP.Where(g => g.gegi == c.gegi && g.Program==c.Program).Sum(g => g.Quantity),
        c.UnitCost,
        TotalCost = lstRptGiftsByCatP.Where(g => g.gegi == c.gegi && g.Program == c.Program).Sum(g => g.TotalCost),
        c.Quantity,
        c.Day,
        Category = c.gcN,
        c.Program
      }).ToList();

      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstGifts, true, false), filters, strReport, string.Empty, clsFormatReport.RptGiftsByCategoryProgram(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptGiftsCertificates(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptGiftsCerts)
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
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(certificates, true, false), filters, strReport, string.Empty, clsFormatReport.RptGiftsCertificates(), blnShowSubtotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptGiftsManifest(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<IEnumerable> lstRptGiftsManifest)
    {
      var lstGiftManifest = lstRptGiftsManifest[0] as List<RptGiftsManifest>;
      var lstGifts = lstRptGiftsManifest[1] as List<GiftForReceipts>;

      lstGiftManifest.ForEach(c =>
      {
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
                                Gift = gm?.giID,
                                GiftN = gm?.giN,
                                Quantity = (gm != null && gm.giWPax) ? (gr.Adults + gr.Minors) : gr.Quantity,
                                gr.Adults,
                                gr.Minors,
                                gr.Folios,
                                Cost = (gm != null) ? (gr.PriceAdults + gr.PriceMinors) : 0,
                                TotalCost = lstGiftManifest.Where(c => c.ReceiptID == gr.ReceiptID).Sum(c => c.PriceAdults + c.PriceMinors),
                                gr.Comments
                              }).ToList();

      //Convertimos la lista a datatable.
      var dt = TableHelper.GetDataTableFromList(rptGiftsManifest, true);
      //Obtenemos el formato del reporte.
      var format = clsFormatReport.RptGiftsManifest();
      //Asignamos el tipo de eje que tendran algunas columnas.
      format.ForEach(c =>
      {
        switch (c.PropertyName)
        {
          case "Deposited":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
            break;
          case "Currency":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 1;
            break;
          case "PaymentType":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 2;
            break;
        }
      });

      //Obtenemos el primer pivot.
      var firstPivotdt = ReportBuilder.GetPivotTable(format, dt);
      var order = format.Where(c => c.Axis == OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values).Min(c => format.IndexOf(c));

      //Creamos la lista de columnas que se excluiran.
      var pivot2 = new List<string>() { "Deposited", "Currency", "PaymentType" };

      //Obtenemos la nueva lista excluyendo las columnas que ya sirvieron como pivote.
      var formatPivot2 = new ExcelFormatItemsList();
      formatPivot2.AddRange(format.Where(c => !pivot2.Contains(c.PropertyName)));
      //var afterPivot = format.Where(c => c.Order > order).OrderBy(c=>c.Order).ToList();
      //Obtenemos las columnas que ya se les aplico el pivot.Y las agregamos a la lista de formatos.
      firstPivotdt.Columns.Cast<DataColumn>().ToList().ForEach(col =>
      {
        if (formatPivot2.Exists(c => c.PropertyName == col.ColumnName)) return;
        formatPivot2.Insert(order, (new ExcelFormatTable { PropertyName = col.ColumnName }));
        order++;
      });

      //Cambiamos las propiedades de las columnas para el nuevo pivote.
      formatPivot2.ForEach(c =>
      {
        switch (c.PropertyName)
        {
          case "Quantity":
          case "Cost":
          case "Folios":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
            break;
          case "Gift":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 1;
            break;
          case "GiftN":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 2;
            break;
        }
      });

      //Obtenemos el segudo pivot.
      var secondPivotdt = ReportBuilder.GetPivotTable(formatPivot2, firstPivotdt);

      return await ReportBuilder.CreateCustomExcel(null, filters, strReport, string.Empty, format, pivotedTable: secondPivotdt, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true);
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
    public static async Task<FileInfo> ExportRptGiftsReceipts(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<IEnumerable> lstRptGiftsReceipts)
    {
      var lstGiftReceipt = lstRptGiftsReceipts[0] as List<RptGiftsReceipts>;
      var lstGifts = lstRptGiftsReceipts[1] as List<GiftForReceipts>;

      lstGiftReceipt.ForEach(c =>
      {
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
                                Gift = gm?.giID,
                                GiftN = gm?.giN,
                                Quantity = (gm != null && gm.giWPax) ? (gr.Adults + gr.Minors) : gr.Quantity,
                                Cost = (gm != null) ? (gr.PriceAdults + gr.PriceMinors) : 0,
                                TotalCost = lstGiftReceipt.Where(c => c.ReceiptID == gr.ReceiptID).Sum(c => c.PriceAdults + c.PriceMinors),
                                gr.WithCost,
                                gr.PublicPrice,
                                gr.Adults,
                                gr.Minors,
                                gr.Folios,
                                gr.Comments
                              }).ToList();
      //Convertimos la lista a datatable.
      var dt = TableHelper.GetDataTableFromList(rptGiftsReceipts, true);
      //Obtenemos el formato del reporte.
      var format = clsFormatReport.RptGiftsReceipts();
      //Asignamos el tipo de eje que tendran algunas columnas.
      format.ForEach(c =>
      {
        switch (c.PropertyName)
        {
          case "Deposited":
          case "Burned":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
            break;
          case "Currency":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 1;
            break;
          case "PaymentType":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 2;
            break;
        }
      });

      //Obtenemos el primer pivot.
      
      var firstPivotdt = ReportBuilder.GetPivotTable(format, dt);

      var order = format.Where(c => c.Axis == OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values).Min(c => format.IndexOf(c));

      //Creamos la lista de columnas que se excluiran.
      var pivot2 = new List<string>() { "Deposited", "Burned", "Currency", "PaymentType" };

      //Obtenemos la nueva lista excluyendo las columnas que ya sirvieron como pivote.
      var formatPivot2 = new ExcelFormatItemsList();
      formatPivot2.AddRange(format.Where(c => !pivot2.Contains(c.PropertyName)));

      var afterPivot = format.Where(c => c.Order > order).OrderBy(c => c.Order).ToList();
      //Obtenemos las columnas que ya se les aplico el pivot.Y las agregamos a la lista de formatos.
      firstPivotdt.Columns.Cast<DataColumn>().ToList().ForEach(col =>
      {
        if (formatPivot2.Exists(c => c.PropertyName == col.ColumnName)) return;
        formatPivot2.Insert(order, (new ExcelFormatTable { PropertyName = col.ColumnName, Order = order }));
        order++;

      });
      //Cambiamos las propiedades de las columnas para el nuevo pivote.
      formatPivot2.ForEach(c =>
      {
        switch (c.PropertyName)
        {
          case "Quantity":
          case "Cost":
          case "WithCost":
          case "PublicPrice":
          case "Folios":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Values;
            break;
          case "Gift":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 1;
            break;
          case "GiftN":
            c.Axis = OfficeOpenXml.Table.PivotTable.ePivotFieldAxis.Column;
            //c.Order = 2;
            break;
        }
      });

      //Obtenemos el segudo pivot.
      var secondPivotdt = ReportBuilder.GetPivotTable(formatPivot2, firstPivotdt);
      return await ReportBuilder.CreateCustomExcel(null, filters, strReport, string.Empty, format, blnRowGrandTotal: true, pivotedTable: secondPivotdt, isRptQueue:true ,filePath:fileFullPath, isPivot: true);
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
    public static async Task<FileInfo> ExportRptGiftsReceiptsPayments(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptGiftsRcptPaym)
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

      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(receipts, true, false), filters, strReport, string.Empty, clsFormatReport.RptGiftsReceiptsPayments(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptGiftsSale(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<IEnumerable> lstRptGiftsSale)
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
      var lstSalesPayments = new List<Tuple<int, string, RptGiftsSale_Payment>>();
      //Obtenemos los folios de recibo de la lista de pagos.
      var receipts = lstGiftPayments.Select(c => c.Receipt).Distinct().ToList();

      //Recorremos los recibos.
      foreach (var receipt in receipts)
      {
        //Obtenemos los pagos del recibo.
        var payments = lstGiftPayments.Where(c => c.Receipt == receipt).ToList();
        //Obtenemos los Regalos del recibo.
        var sales = lstGiftReceipts.Where(c => c.Receipt == receipt).ToList();

        if (payments.Count <= 0) continue;

        decimal curPayment = 0;//Pago Actual
        decimal curPaymentUs = 0;//Pago Actual Dlls
        decimal curBalance = 0;
        decimal curBalanceUs = 0;
        //Recorremos los pagos.
        foreach (var payment in payments)
        {
          //Asignamos el monto del pago al balance actual.
          curBalance = payment.Amount ?? 0;//Balance Actual
          //Asignamos el monto del pago en Dlls al balance actual.
          curBalanceUs = payment.AmountUS ?? 0;//Balance Actual Dlls
          
          do
          {
            int index = 0;
            var sale = sales.FirstOrDefault(c => c.Receipt == payment.Receipt && c.Difference != 0);
            if (sale != null)
            {
              index = sales.IndexOf(sale);

              if (Math.Abs(sales[index].Difference ?? 0) > Math.Abs(curBalanceUs))
              {
                curPayment = curBalance;
                curPaymentUs = curBalanceUs;
                curBalance = 0;
                curBalanceUs = 0;
              }
              else //if (Math.Abs(sales[i].Difference ?? 0) <= Math.Abs(curBalanceUs))
              {
                curPayment = (sales[index].Difference ?? 0) / payment.ExchangeRate;
                curPaymentUs = sales[index].Difference ?? 0;
                curBalance -= curPayment;
                curBalanceUs -= curPaymentUs;
              }
              lstSalesPayments.Add(Tuple.Create(sales[index].Receipt, sales[index].Gift,
            new RptGiftsSale_Payment
            {
              Amount = curPayment,
              User = payment.User,
              UserN = payment.UserN,
              Source = payment.Source,
              Currency = payment.Currency,
              PaymentType = payment.PaymentType,
              Receipt = payment.Receipt,
              AmountUS = curPaymentUs,
              ExchangeRate = payment.ExchangeRate
            }));
            }
            else
            {
              curPayment = curBalance;
              curPaymentUs = curBalanceUs;
              curBalance = 0;
              curBalanceUs = 0;
              lstSalesPayments.Add(Tuple.Create(sales[index].Receipt, sales[index].Gift,
             new RptGiftsSale_Payment
             {
               Amount = 0,
               User = payment.User,
               UserN = payment.UserN,
               Source = payment.Source,
               Currency = payment.Currency,
               PaymentType = payment.PaymentType,
               Receipt = payment.Receipt,
               AmountUS = 0,
               ExchangeRate = payment.ExchangeRate
             }));
              lstSalesPayments[lstSalesPayments.Count - 1].Item3.Amount += curPayment;
              lstSalesPayments[lstSalesPayments.Count - 1].Item3.AmountUS += curPaymentUs;
              
            }            

            sales[index].Difference -= curPaymentUs;
          } while (Math.Abs(curBalanceUs) > 0);         
        }
      }     

      var source = (from gr in lstGiftReceipts
                    join gp in lstSalesPayments on new { gr.Receipt, gr.Gift } equals new { Receipt = gp.Item1, Gift = gp.Item2 } into pays
                    from py in pays.DefaultIfEmpty()
                    select new
                    {
                      gr.Program,
                      gr.Receipt,
                      gr.Date,
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
                      gr.ExchangeRate,
                      gr.Comments,
                      PaymentTotal = (py != null) ? lstSalesPayments.Where(c => c.Item1 == gr.Receipt && c.Item2 == gr.Gift).Sum(c => c.Item3.AmountUS) : 0,
                      Difference = (py != null) ? gr.TotalToPay - (lstSalesPayments.Where(c => c.Item1 == gr.Receipt && c.Item2 == gr.Gift).Sum(c => c.Item3.AmountUS)) : 0,
                      User = (py != null) ? string.Join(",", lstSalesPayments.Where(c => c.Item1 == gr.Receipt && c.Item2 == gr.Gift).Select(c => c.Item3.User).Distinct().ToList()) : "",
                      UserN = (py != null) ? string.Join(",", lstSalesPayments.Where(c => c.Item1 == gr.Receipt && c.Item2 == gr.Gift).Select(c => c.Item3.UserN).Distinct().ToList()) : "",
                      Amount = (py != null) ? py.Item3.Amount : 0,
                      Source = (py != null) ? py.Item3.Source : "",
                      Currency = (py != null) ? py.Item3.Currency : "",
                      PaymentType = (py != null) ? py.Item3.PaymentType : ""
                    }
        ).ToList();
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(source, true), filters, strReport, string.Empty, clsFormatReport.RptGiftsSale(), isPivot: true, blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptGiftsUsedBySistur(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptGiftsUsedBySistur> lstRptGiftsSistur)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptGiftsSistur, true), filters, strReport, string.Empty, clsFormatReport.RptGiftsUsedBySistur(), blnShowSubtotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static FileInfo ExportRptWeeklyGiftSimple(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptWeeklyGiftsItemsSimple> lstRptWeeklyGiftsSimple)
    {
      return ReportBuilder.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptWeeklyGiftsSimple, true, false),strReport, string.Empty, clsFormatReport.RptWeeklyGiftSimple(), true, isRptQueue:true ,filePath:fileFullPath);
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
    public static FileInfo ExportRptGuestCeco(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptGuestCeco> lstRptGuestCeco)
    {
      return ReportBuilder.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptGuestCeco, true, false),strReport, string.Empty, clsFormatReport.RptGuestCeco(), isRptQueue:true ,filePath:fileFullPath);
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
    public static FileInfo ExportRptGuestNoBuyers(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptGuestsNoBuyers> lstRptGuestNoBuyers)
    {
      lstRptGuestNoBuyers = lstRptGuestNoBuyers
        .OrderBy(c => c.Program)
        .ThenBy(c => c.LeadSource)
        .ThenBy(c => c.GuestID)
        .ToList();
      return ReportBuilder.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptGuestNoBuyers, true, false),strReport, string.Empty, clsFormatReport.RptGuestNoBuyers(), isRptQueue:true ,filePath:fileFullPath);
    }
    #endregion

    #region ExportRptInOut

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de In & Out.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptInOut">Lista de RptInOut</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 09/Abr/2016 Created
    /// </history>
    public static async Task<FileInfo> ExportRptInOut(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptInOut> lstRptInOut)
    {

      var lstRptInOutNew = lstRptInOut
        .OrderBy(c => c.SalesRoom)
        .ThenBy(c => c.Location)
        .ThenBy(c => c.GUID)
        .ThenBy(c => c.ShowDate)
        .ToList();
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptInOutNew, true), filters, strReport, string.Empty, clsFormatReport.RptInOut(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
    }
    #endregion

    #region ExportRptManifestRange

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Guest Manifest Range.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptManifestRange">Lista de RptManifestRange</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 06/Jun/2016 Created
    /// </history>
    public static async Task<FileInfo> ExportRptManifestRange(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptManifestRange> lstRptManifestRange)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptManifestRange, true, false), filters, strReport, string.Empty, clsFormatReport.RptManifestRange(), blnRowGrandTotal: true, blnShowSubtotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
    }
    #endregion

    #region ExportRptManifestRangeByLs

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Guest Manifest Range.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptManifestRange">Lista de RptManifestRange</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 13/Jun/2016 Created
    /// </history>
    public static async Task<FileInfo> ExportRptManifestRangeByLs(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<IEnumerable> lstRptManifestRange)
    {
      var lstRptManifest = lstRptManifestRange[0] as List<RptManifestByLSRange>;
      var lstBookings = lstRptManifestRange[1] as List<RptManifestByLSRange_Bookings>;

      var dtRptManifest = TableHelper.GetDataTableFromList(lstRptManifest, true);

      var dtBookings = TableHelper.GetDataTableFromList(lstBookings.Select(c => new
      {
        c.guloInvit,
        c.LocationN,
        guBookTime = c.guBookT,
        c.guBookT,
        c.Bookings
      }).ToList(), true, false);

      return await EpplusHelper.ExportRptManifestRangeByLs(new List<Tuple<DataTable, ExcelFormatItemsList>> {
        Tuple.Create(dtRptManifest, clsFormatReports.RptManifestRangeByLs()),
        Tuple.Create(dtBookings, clsFormatReports.RptManifestRangeByLs_Bookings())
      }, filters, strReport, string.Empty, blnRowGrandTotal: true, blnShowSubtotal: true, fileFullPath: fileFullPath);
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
    public static async Task<FileInfo> ExportRptGuestNoShows(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptGuestsNoShows> lstRptGuestNoShows)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptGuestNoShows, true), filters, strReport, string.Empty, clsFormatReport.RptGuestNoShow(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static FileInfo ExportRptMealTickets(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptMealTickets> lstRptMealTickets, bool groupByHost = false)
    {
      return ReportBuilder.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptMealTickets, true, true,true),strReport, string.Empty, groupByHost ? clsFormatReport.RptMealTicketsByHost() : clsFormatReport.RptMealTickets(), true, isRptQueue:true ,filePath:fileFullPath);
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
    public static FileInfo ExportRptMealTicketsCost(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptMealTicketsCost> lstRptMealTicketsCost)
    {
      return ReportBuilder.CreatePivotRptExcel(false, filters, TableHelper.GetDataTableFromList(lstRptMealTicketsCost, true, false,true),strReport, string.Empty, clsFormatReport.RptMealTicketsCost(), true, true, isRptQueue:true ,filePath:fileFullPath);
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
    public static async Task<FileInfo> ExportRptMemberships(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptMemberships> lstRptMemberships)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptMemberships, true, false), filters, strReport, string.Empty, clsFormatReport.RptMemberships(), blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptMembershipsByAgencyMarket(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptMembershipsByAgencyMarket> lstRptMembershipsAgencyM)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptMembershipsAgencyM, true, false), filters, strReport, string.Empty, clsFormatReport.RptMembershipsByAgencyMarket(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptMembershipsByHost(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptMembershipsByHost> lstRptMembershipsHost)
    {
      lstRptMembershipsHost.ForEach(c => c.guEntryHost = $"{c.guEntryHost} {c.guEntryHostN}");
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptMembershipsHost, true, false), filters, strReport, string.Empty, clsFormatReport.RptMembershipsByHost(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptProductionBySalesRoom(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionBySalesRoom> lstRptProductionBySr)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptProductionBySr, true, false), filters, strReport, string.Empty, clsFormatReport.RptProductionBySalesRoom(), blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptProductionBySalesRoomMarket(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionBySalesRoomMarket> lstRptProductionBySrm)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptProductionBySrm, true, false), filters, strReport, string.Empty, clsFormatReport.RptProductionBySalesRoomMarket(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptProductionBySalesRoomMarketSubMarket(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionBySalesRoomProgramMarketSubmarket> lstRptProductionBySrmSm)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptProductionBySrmSm, true, false), filters, strReport, string.Empty, clsFormatReport.RptProductionBySalesRoomMarketSubMarket(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptProductionByShowProgram(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByShowProgram> lstRptProductionByShowProgram)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptProductionByShowProgram, true, false), filters, strReport, string.Empty, clsFormatReport.RptProductionByShowProgram(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptProductionByShowProgramProgram(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByShowProgramProgram> lstRptProductionByShowProgramProgram)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptProductionByShowProgramProgram, true, false), filters, strReport, string.Empty, clsFormatReport.RptProductionByShowProgramProgram(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptCloserStatistics(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptCloserStatistics> lstRptCloserStatistic)
    {
      lstRptCloserStatistic=lstRptCloserStatistic.OrderBy(c => c.saCloserps).ThenByDescending(c => c.TotalProcAmount).ThenByDescending(c => c.Efficiency).ToList();
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptCloserStatistic, true, false), filters, strReport, string.Empty, clsFormatReport.RptCloserStatistic(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptLinerStatistics(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptLinerStatistics> lstRptCloserStatistic)
    {
      lstRptCloserStatistic = lstRptCloserStatistic.OrderBy(c => c.saLinerps).ThenByDescending(c => c.TotalProcAmount).ThenByDescending(c => c.Efficiency).ToList();
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptCloserStatistic, true, false), filters, strReport, string.Empty, clsFormatReport.RptLinerStatistic(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
    }
    #endregion

    #region ExportRptWeeklyMonthlyHostess

    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte de Weekly & Monthly Hostess.
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptWeeklyMonthly">Lista de RptWeeklyMonthlyHostess</param>
    /// <returns> FileInfo </returns>
    ///  <history>
    /// [edgrodriguez] 09/Jun/2016 Created
    /// </history>
    public static FileInfo ExportRptWeeklyMonthlyHostess(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<IEnumerable> lstRptWeeklyMonthly)
    {
      var weeklyMonthly = TableHelper.GetDataTableFromList(lstRptWeeklyMonthly[0] as List<RptWeeklyMonthlyHostess_ByPR>);
      var hostessTime = lstRptWeeklyMonthly[1] as List<RptWeeklyMonthlyHostess_ByTourTime>;
      var dates = hostessTime.Select(c => c.guD).Distinct().ToList();
      var leadsources = hostessTime.Select(c => c.guls).Distinct().ToList();
      dates.ForEach(d =>
      {
        leadsources.ForEach(l =>
        {
          if (!hostessTime.Exists(h => h.guD == d && h.guls == l))
          {
            hostessTime.Add(new RptWeeklyMonthlyHostess_ByTourTime
            {
              guls = l,
              guD = d
            });
          }
        });
      });

      hostessTime = hostessTime
        .OrderBy(c => c.guls)
        .ThenBy(c => c.guD).ToList();

      var dtHostessTime = TableHelper.GetDataTableFromList(hostessTime);

      return EpplusHelper.ExportRptWeeklyMonthlyHostess(new List<Tuple<DataTable, ExcelFormatItemsList, string>> {
        Tuple.Create(weeklyMonthly, clsFormatReport.RptWeeklyMonthlyHostessByPr(),strReport+" By PR"),
        Tuple.Create(dtHostessTime,clsFormatReport.RptWeeklyMonthlyHostessByTourTime(),strReport+" By Tour Time")
      }, filters, strReport, string.Empty, blnShowSubtotal: true, blnRowGrandTotal:true, fileFullPath:fileFullPath);
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
    public static async Task<FileInfo> ExportRptTaxiIn(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptTaxisIn> lstRptTaxiIn)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptTaxiIn, true, false), filters, strReport, string.Empty, clsFormatReport.RptTaxisIn(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptTaxiOut(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptTaxisOut> lstRptTaxiOut)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptTaxiOut, true, false), filters, strReport, string.Empty, clsFormatReport.RptTaxisOut(), blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptBurnedDepositsGuests(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptBurnedDepGuest)
    {
      var lstGiftsR = lstRptBurnedDepGuest[0] as List<RptDepositsBurnedGuests>;
      var currencies = lstRptBurnedDepGuest[1] as List<Currency>;
      var payType = lstRptBurnedDepGuest[2] as List<PaymentType>;

      lstGiftsR.ForEach(c =>
      {
        c.gucu = currencies.First(cu => cu.cuID == c.gucu).cuN ?? "";
        c.gupt = payType.First(pt => pt.ptID == c.gupt).ptN ?? "";
      });

      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstGiftsR, true, false), filters, strReport, string.Empty, clsFormatReport.RptDepositsBurnedGuests(), blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptDepositRefunds(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptDepositRefund> lstRptDepRef)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptDepRef, true, false), filters, strReport, string.Empty, clsFormatReport.RptDepositRefunds(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptDepositByPr(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptDepPr)
    {
      var lstDepPr = lstRptDepPr[0] as List<RptDepositsByPR>;
      var currencies = lstRptDepPr[1] as List<Currency>;
      var payType = lstRptDepPr[2] as List<PaymentType>;

      lstDepPr.ForEach(c =>
      {
        c.guPRInvit1 = $"{c.guPRInvit1} {c.peN}";
        c.gucu = currencies.First(cu => cu.cuID == c.gucu).cuN ?? "";
        c.gupt = payType.First(pt => pt.ptID == c.gupt).ptN ?? "";
      });

      lstDepPr = lstDepPr.OrderBy(c=>c.guPRInvit1).ThenBy(c => c.guID).ToList();
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstDepPr, true, false), filters, strReport, string.Empty, clsFormatReports.RptDepositByPr(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptDepositsNoShow(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<object> lstRptDepNoShow)
    {
      var lstDepPrNoShow = lstRptDepNoShow[0] as List<RptDepositsNoShow>;
      var currencies = lstRptDepNoShow[1] as List<Currency>;
      var payType = lstRptDepNoShow[2] as List<PaymentType>;

      lstDepPrNoShow.ForEach(c =>
      {
        c.guPRInvit1 = $"{c.guPRInvit1} {c.peN}";
        c.gucu = currencies.First(cu => cu.cuID == c.gucu).cuN ?? "";
        c.gupt = payType.First(pt => pt.ptID == c.gupt).ptN ?? "";
      });

      lstDepPrNoShow = lstDepPrNoShow.OrderBy(c => c.guPRInvit1).ThenBy(c => c.guID).ToList();

      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstDepPrNoShow, true, false), filters, strReport, string.Empty, clsFormatReports.RptDepositByPr(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptInOutByPr(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptInOutByPR> lstRptInOutPr)
    {
      lstRptInOutPr = lstRptInOutPr.OrderBy(c => c.PR1).ToList();
      lstRptInOutPr.ForEach(c =>
      {
        c.PR1 = $"{c.PR1} {c.PR1N}";
      });

      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptInOutPr, true), filters, strReport, string.Empty, clsFormatReport.RptInOutByPr(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptPersonnelAccess(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptPersonnelAccess> lstRptPersonnelAccess)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptPersonnelAccess, true), filters, strReport, string.Empty, clsFormatReport.RptPersonnelAccess(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptSelfGen(string strReport, string fileFullPath, List<Tuple<string, string>> filters, Tuple<List<RptSelfGen>, List<Sale>, List<Personnel>> lstRptselfGenTuple, DateTime dtmStart, DateTime dtmEnd)
    {
      var lstRptSelfGen = (from pe in lstRptselfGenTuple.Item3
                           select new
                           {
                             PRID = pe.peID,
                             PRN = pe.peN,
                             PRps = pe.peps,
                             Shows = lstRptselfGenTuple.Item1.Where(c => c.guPRInvit1 == pe.peID).Count(c => c.guShowD != null && (c.guShowD >= dtmStart && c.guShowD <= dtmEnd)),
                             IO = lstRptselfGenTuple.Item1.Where(c => c.guPRInvit1 == pe.peID).Count(c => c.guInOut),
                             WO = lstRptselfGenTuple.Item1.Where(c => c.guPRInvit1 == pe.peID).Count(c => c.guWalkOut),

                             Proc = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID).Count(c => c.saProcD != null && (c.saD == c.saProcD && (c.saD >= dtmStart && c.saD <= dtmEnd)) && c.sast != "DG"),
                             ProcAmount = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD == c.saProcD && (c.saD >= dtmStart && c.saD <= dtmEnd))).Sum(c => c.saGrossAmount),

                             OOP = lstRptselfGenTuple.Item2.Count(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD != c.saProcD && (c.saProcD >= dtmStart && c.saProcD <= dtmEnd)) && c.sast != "DG"),
                             OOPAmount = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD != c.saProcD && (c.saProcD >= dtmStart && c.saProcD <= dtmEnd))).Sum(c => c.saGrossAmount),

                             Cancel = lstRptselfGenTuple.Item2.Count(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saCancelD >= dtmStart && c.saCancelD <= dtmEnd) && c.sast != "DG"),
                             CancelAmount = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saCancelD >= dtmStart && c.saCancelD <= dtmEnd)).Sum(c => c.saGrossAmount),

                             Subtotal = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD == c.saProcD && (c.saD >= dtmStart && c.saD <= dtmEnd))).Sum(c => c.saGrossAmount) +
                             lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD != c.saProcD && (c.saProcD >= dtmStart && c.saProcD <= dtmEnd))).Sum(c => c.saGrossAmount),

                             TotalProc = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID).Count(c => c.saProcD != null && (c.saD == c.saProcD && (c.saD >= dtmStart && c.saD <= dtmEnd)) && c.sast != "DG") +
                             lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD != c.saProcD && (c.saProcD >= dtmStart && c.saProcD <= dtmEnd))).Sum(c => c.saGrossAmount) -
                             lstRptselfGenTuple.Item2.Count(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saCancelD >= dtmStart && c.saCancelD <= dtmEnd) && c.sast != "DG"),

                             TotalProcAmount = (lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD == c.saProcD && (c.saD >= dtmStart && c.saD <= dtmEnd))).Sum(c => c.saGrossAmount) +
                             lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saD != c.saProcD && (c.saProcD >= dtmStart && c.saProcD <= dtmEnd))).Sum(c => c.saGrossAmount)) -
                             lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && c.saProcD != null && (c.saCancelD >= dtmStart && c.saCancelD <= dtmEnd)).Sum(c => c.saGrossAmount),

                             Pending = lstRptselfGenTuple.Item2.Count(c => c.saPR1 == pe.peID && (c.saProcD == null || c.saProcD > dtmEnd) && c.saD >= dtmStart && c.saD <= dtmEnd),
                             PendingAmount = lstRptselfGenTuple.Item2.Where(c => c.saPR1 == pe.peID && (c.saProcD == null || c.saProcD > dtmEnd) && c.saD >= dtmStart && c.saD <= dtmEnd).Sum(c => c.saGrossAmount)
                           }).Distinct()
                           .Select(c => new
                           {
                             c.PRID,
                             c.PRN,
                             c.PRps,
                             c.Shows,
                             c.IO,
                             c.WO,
                             c.Proc,
                             c.ProcAmount,
                             c.OOP,
                             c.OOPAmount,
                             c.Cancel,
                             c.CancelAmount,
                             c.Subtotal,
                             c.TotalProc,
                             c.TotalProcAmount,
                             c.Pending,
                             c.PendingAmount,
                             CancelF = c.Subtotal > 0 ? c.CancelAmount / c.Subtotal : c.CancelAmount == 0 ? 0 : 1,
                             Eff = c.Shows == 0 ? 0 : c.TotalProcAmount / c.Shows,
                             CI = c.Shows == 0 ? 0 : c.TotalProc / c.Shows,
                             AvgSale = c.TotalProc == 0 ? 0 : c.TotalProcAmount / c.TotalProc,
                           }).ToList();

      lstRptSelfGen = lstRptSelfGen.Distinct().OrderBy(c => c.PRps)
        .ThenByDescending(c => c.TotalProcAmount).ToList();


      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptSelfGen.Distinct().ToList(), true, false), filters, strReport, string.Empty, clsFormatReport.RptSelfGen(), blnRowGrandTotal: true, blnShowSubtotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public async static Task<FileInfo> ExportRptAgencies(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptAgencies> lstRptAgencies)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptAgencies, true), filters, strReport, string.Empty, clsFormatReport.RptAgencies(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public async static Task<FileInfo> ExportRptPersonnel(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptPersonnel> lstRptPersonnel)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptPersonnel, true), filters, strReport, string.Empty, clsFormatReport.RptPersonnel(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public async static Task<FileInfo> ExportRptGifts(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptGifts> lstRptGifts)
    {
      
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptGifts, true), filters, strReport, string.Empty, clsFormatReport.RptGifts(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
    }
    #endregion

    #region ExportRptGiftsKardex
    /// <summary>
    /// Obtiene los datos para Exportar a Excel el reporte Gifts Kardex
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="dateRangeFileName">Rango de Fechas</param>
    /// <param name="filters"></param>
    /// <param name="lstRptGiftsKardex">Lista de RptGifts</param>
    /// <returns> FileInfo </returns>
    /// <history>
    /// [edgrodriguez] 07/Jun/2016 Created
    /// </history>
    public static async Task<FileInfo> ExportRptGiftsKardex(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptGiftsKardex> lstRptGiftsKardex)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptGiftsKardex, true), filters, strReport, string.Empty, clsFormatReport.RptGiftsKardex(), blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, isPivot: true);
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
    public static async Task<FileInfo> ExportRptProductionByLeadSourceMarketMonthly(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByLeadSourceMarketMonthly> lstRptProdLsmMonthly)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptProdLsmMonthly, true), filters, strReport, string.Empty, clsFormatReport.RptProductionByLeadSourceMarketMonthly(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptProductionReferral(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionReferral> lstRptProductionReferrals)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptProductionReferrals, true), filters, strReport, string.Empty, clsFormatReport.RptProductionReferral(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath);
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
    public async static Task<FileInfo> ExportRptReps(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<Rep> lstRptReps)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptReps, true), filters, strReport, string.Empty, clsFormatReport.RptReps(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
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
    public static async Task<FileInfo> ExportRptSalesByProgramLeadSourceMarket(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptSalesByProgramLeadSourceMarket> lstRptSalesByProgramLeadSourceMarkets)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptSalesByProgramLeadSourceMarkets, true), filters, strReport, string.Empty, clsFormatReport.RptSalesByProgramLeadSourceMarket(), blnShowSubtotal: true, blnRowGrandTotal: true, isRptQueue:true ,filePath:fileFullPath);
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
    public async static Task<FileInfo> ExportRptWarehouseMovements(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptWarehouseMovements> lstRptWarehouseMovements)
    {
      return await ReportBuilder.CreateCustomExcel(TableHelper.GetDataTableFromList(lstRptWarehouseMovements), filters, strReport, string.Empty, clsFormatReport.RptWarehouseMovements(), isRptQueue:true ,filePath:fileFullPath, addEnumeration: true);
      
    }
    #endregion

    #endregion
  }
}