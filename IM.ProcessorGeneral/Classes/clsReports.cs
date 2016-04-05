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
      DataTable dtData = GridHelper.GetDataTableFromGrid(lstRptBBSalesRoom);
      return EpplusHelper.CreatePivotRptExcel(true, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptBookingsBySalesRoomProgramTime());
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
      DataTable dtData = GridHelper.GetDataTableFromGrid(lstRptBBSalesRoomLS);
      return EpplusHelper.CreatePivotRptExcel(true, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptBookingsBySalesRoomProgramLeadSourceTime());
    }
    #endregion

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
      DataTable dtData = GridHelper.GetDataTableFromGrid(lstRptCxC, changeDataTypeBoolToString: true, showCheckMark: false);
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


      DataTable dtData = GridHelper.GetDataTableFromGrid(CxCDeCur, changeDataTypeBoolToString: true, showCheckMark: false);
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
                              join gft in lstGift on gRcpt.Gift equals gft.giID
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
                                gft.giN,//Obtenemos el ID del regalo.
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

      var pivot = receiptsWithGift.Where(c => c.Gift != null).OrderBy(c => c.giN)
                              .ToPivot(
        c => new { c.Gift, c.giN },
        c => new { c.grID, c.grNum, c.grpe, c.peN, c.grlo, c.grHost, c.HostN, c.grgu, c.grGuest, c.Adults, c.Minors, c.grD, c.CostUS, c.exExchRate, c.CostMX, c.grMemberNum, c.grCxCComments },
        c => new { Quantity = c.Distinct().Select(v => v.Quantity).FirstOrDefault(), Cost = c.Distinct().Select(v => v.Cost).FirstOrDefault(), Folios = c.Distinct().Select(v => v.Folios).FirstOrDefault() ?? string.Empty });

      lstReceipts.Where(c => c.Gift == null).
        Select(c => new { c.grID, c.grNum, c.grpe, c.peN, c.grlo, c.grHost, c.HostN, c.grgu, c.grGuest, c.Adults, c.Minors, c.grD, c.CostUS, c.exExchRate, c.CostMX, c.grMemberNum, c.grCxCComments }).ToList()
        .ForEach(c => pivot.Add(c.GetType().GetProperties().Select(v => v.GetValue(c)).ToArray()));

      //Ordenamos por grID
      pivot = pivot.OrderBy(c => Convert.ToSingle(c[0])).ToList();

      return EpplusHelper.createExcelCustom(GridHelper.GetDataTableFromGrid(receiptsWithGift.OrderBy(c => c.giN).ToList()), filters, strReport, dateRangeFileName, clsFormatReport.rptCxCGifts(), true, pivot, true, true);
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
      DataTable dtData = GridHelper.GetDataTableFromGrid(lstRptCxCNotAuthorized, changeDataTypeBoolToString: true, showCheckMark: false);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, dateRangeFileName, clsFormatReport.rptCxCNotAuthorized());
    }
    #endregion 

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

      return EpplusHelper.CreatePivotRptExcel(false, filters, GridHelper.GetDataTableFromGrid(lstRptDeps, true, false), strReport, dateRangeFileName, clsFormatReport.rptDeposits(), showRowGrandTotal: true);
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
    public static FileInfo ExportRptBurnedDeposits(string strReport, string dateRangeFileName, List<Tuple<string, string>> filters, List<object> lstRptBurnedDeposits, DateTime dtmStart,DateTime dtmEnd)
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
                            memberNum = (lstGiftSales.ContainsKey(gRcpt.grID)) ? string.Join(",", lstGiftSales[gRcpt.grID].Select(s => s.saMembershipNum).ToList()) : "-",
                            procAmount = (lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => s.saProc && (s.saProcD >= dtmStart && s.saProcD <= dtmEnd)).Sum(s => s.saGrossAmount) : 0,
                            pendAmount = (lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => !s.saProc || (s.saProc && !(s.saProcD >= dtmStart && s.saProcD <= dtmEnd))).Sum(s => s.saGrossAmount) : 0
                          }
                        ).OrderBy(c => c.grD)
                        .ThenBy(c => c.grID)
                        .ToList();

      var pivot = lstRptDeps.ToPivot(
        c => new { c.cuN, c.ptN },
        c => new { c.grID, c.grNum, c.grD, c.grgu, c.grGuest, c.grHotel, c.grlo, c.grsr, c.grpe, c.peN, c.grHost, c.grComments, c.memberNum, c.procAmount, c.pendAmount },
        c => new { grDepositTwisted = c.Distinct().Select(v => v.grDepositTwisted).FirstOrDefault() }
        );

      return EpplusHelper.createExcelCustom(GridHelper.GetDataTableFromGrid(lstRptDeps, true, false), filters, strReport, dateRangeFileName, clsFormatReport.rptBurnedDeposits(), true, pivot, true, true);
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
                          memberNum = (lstGiftSales.ContainsKey(gRcpt.grID)) ? string.Join(",", lstGiftSales[gRcpt.grID].Select(s => s.saMembershipNum).ToList()) : "-",
                          procAmount = (lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => s.saProc && (s.saProcD >= dtmStart && s.saProcD <= dtmEnd)).Sum(s => s.saGrossAmount) : 0,
                          pendAmount = (lstGiftSales.ContainsKey(gRcpt.grID)) ? lstGiftSales[gRcpt.grID].Where(s => !s.saProc || (s.saProc && !(s.saProcD >= dtmStart && s.saProcD <= dtmEnd))).Sum(s => s.saGrossAmount) : 0
                        }
                        ).OrderBy(c => c.grD)
                        .ThenBy(c => c.grID)
                        .ToList();

      var pivot = lstRptDeps.ToPivot(
        c => new { c.cuN, c.ptN },
        c => new { c.grID, c.grNum, c.grD, c.grgu, c.grGuest, c.grHotel, c.guHotelB, c.grlo, c.grsr, c.grpe, c.peN, c.grHost, c.memberNum, c.procAmount, c.pendAmount },
        c => new { grDepositTwisted = c.Distinct().Select(v => v.grDepositTwisted).FirstOrDefault() }
        );

      return EpplusHelper.createExcelCustom(GridHelper.GetDataTableFromGrid(lstRptDeps, true, false), filters, strReport, dateRangeFileName, clsFormatReport.rptBurnedDeposits(), true, pivot, true, true);
    }
    #endregion 

    #endregion


  }
}