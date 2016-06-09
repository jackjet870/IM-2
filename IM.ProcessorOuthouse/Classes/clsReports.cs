using IM.Base.Helpers;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace IM.ProcessorOuthouse.Classes
{
  public class clsReports
  {
    #region ExportRptDepositsPaymentByPR

    /// <summary>
    ///  Obtiene los datos para exportar a excel el reporte DepositsPaymentByPR
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptDepositsPaymentByPR">Lista de los depositos y pagos por PR</param>
    /// <history>
    ///   [vku] 07/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptDepositsPaymentByPR(string strReport, string fileFullPath, List<Tuple<string, string>> filters, DepositsPaymentByPRData lstRptDepositsPaymentByPR)
    {
      var lstDepositsPaymentByPR = lstRptDepositsPaymentByPR.DepositsPaymentByPR;
      var lstDepositsPaymentByPRDeposits = lstRptDepositsPaymentByPR.DepositsPaymentByPR_Deposit;
      var lstCurrencies = lstRptDepositsPaymentByPR.Currencies;
      var lstPaymentTypes = lstRptDepositsPaymentByPR.PaymentTypes;

      var lstDepositsPayments = (from paymentByPR in lstDepositsPaymentByPR
                                 join paymentByPRDep in lstDepositsPaymentByPRDeposits on paymentByPR.PR equals paymentByPRDep.PR
                                 join cu in lstCurrencies on paymentByPRDep.bdcu equals cu.cuID
                                 join payType in lstPaymentTypes on paymentByPRDep.bdpt equals payType.ptID
                                 select new
                                 {
                                   paymentByPR.Category,
                                   paymentByPRDep.pcN,
                                   paymentByPRDep.PaymentSchema,
                                   paymentByPR.PR,
                                   paymentByPR.PRN,

                                   paymentByPR.Books,
                                   paymentByPR.InOuts,
                                   paymentByPR.GrossBooks,
                                   paymentByPR.GrossShows,
                                   paymentByPR.SalesAmount,

                                   paymentByPRDep.guID,
                                   paymentByPRDep.guName,
                                   paymentByPRDep.guBookD,
                                   paymentByPRDep.guOutInvitNum,
                                   paymentByPRDep.guls,
                                   paymentByPRDep.gusr,
                                   paymentByPRDep.guHotel,

                                   paymentByPR.ShowsFactor,
                                   paymentByPR.Efficiency,

                                   cu.cuN,
                                   payType.ptN,
                                   paymentByPRDep.bdAmount,
                                   paymentByPRDep.bdReceived,
                                   paymentByPRDep.ToPay
                                 }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstDepositsPayments, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptDepositsPaymentByPR(), showRowGrandTotal: true, showColumnGrandTotal: true, fileFullPath: fileFullPath);
     
    }

    #endregion ExportRptDepositsPaymentByPR

    #region ExportRptGiftsReceivedBySR

    /// <summary>
    ///  Obtiene los datos para el reporte GiftsReceivedBySR
    /// </summary>
    /// <param name="strReport">Nombre del Reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptGiftsReceivedBySR">Lista de regalos recibidos por SR</param>
    /// <history>
    ///   [vku] 11/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGiftsReceivedBySR(string strReport, string fileFullPath, List<Tuple<string, string>> filters, GiftsReceivedBySRData lstRptGiftsReceivedBySR)
    {
      var lstGiftsReceivedBySR = lstRptGiftsReceivedBySR.GiftsReceivedBySR;
      var currencies = lstRptGiftsReceivedBySR.Currencies;

      var lstGifRecBySRWithCu = (from giftRecBySR in lstGiftsReceivedBySR
                                 join cu in currencies on giftRecBySR.Currency equals cu.cuID
                                 select new
                                 {
                                   giftRecBySR.SalesRoom,
                                   giftRecBySR.Gift,
                                   giftRecBySR.GiftN,
                                   giftRecBySR.Quantity,
                                   giftRecBySR.Couples,
                                   giftRecBySR.Adults,
                                   giftRecBySR.Minors,
                                   cuID = "A" + cu.cuID,
                                   cu.cuN,
                                   giftRecBySR.Amount,
                                 }).ToList();

      var lstGifRecBySRWithCuTotal = lstGiftsReceivedBySR.Select(giftRecBySR => new
      {
        giftRecBySR.SalesRoom,
        giftRecBySR.Gift,
        giftRecBySR.GiftN,
        giftRecBySR.Quantity,
        giftRecBySR.Couples,
        giftRecBySR.Adults,
        giftRecBySR.Minors,
        cuID = "B",
        cuN = "Total",
        Amount = lstGiftsReceivedBySR.Where(c => c.SalesRoom == giftRecBySR.SalesRoom && c.Gift == giftRecBySR.Gift).Sum(c => c.Amount)
      }).Distinct().ToList();

      lstGifRecBySRWithCu.AddRange(lstGifRecBySRWithCuTotal);

      DataTable dtData = TableHelper.GetDataTableFromList(lstGifRecBySRWithCu);
      return EpplusHelper.CreateExcelCustomPivot(dtData, filters, strReport, string.Empty, clsFormatReport.rptGiftsRecivedBySR(), blnShowSubtotal: true, blnRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion ExportRptGiftsReceivedBySR

    #region ExportRptGuestsShowNoPresentedInvitation

    /// <summary>
    ///  Obtiene los datos para el reporte GuestsShowNoPresentedInvitation
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptGuestsShowNoPresentedInvitation">Lista de los huespedes que no presentaron invitacion</param>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptGuestsShowNoPresentedInvitation(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<GuestShowNoPresentedInvitation> lstRptGuestsShowNoPresentedInvitation)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptGuestsShowNoPresentedInvitation);
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, strReport, string.Empty, clsFormatReport.rptGuestsShowNoPresentedInvitation(), fileFullPath: fileFullPath);
    }

    #endregion ExportRptGuestsShowNoPresentedInvitation

    #region ExportRptProductionByPROuthouse

    /// <summary>
    ///  Obtiene los datos  para el reporte ProductionByPROuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByPROuthouse">Lista de produccion por PR</param>
    /// <history>
    ///  [vku] 14/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByPROuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByPROuthouse> lstRptProductionByPROuthouse)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByPR(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByPROuthouse

    #region ExportRptProductionByAgeOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByAge
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de Fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgeOuthouse">Lista de produccion por edad</param>
    /// <history>
    ///   [vku] 13/abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByAgeOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByAgeOuthouse> lstRptProductionByAgeOuthouse)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByAge(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByAgeOuthouse

    #region ExportRptProductionByAgeSalesRoomOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByAgeSalesRoomOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgeSalesRoomOuthouse">Lista de produccion por edad y sales room</param>
    /// <history>
    ///   [vku] 13/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByAgeSalesRoomOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByAgeSalesRoomOuthouse> lstRptProductionByAgeSalesRoomOuthouse)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByAgeSalesRoomOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByAgeSalesRoomOuthouse

    #region ExportRptProductionByAgencyOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByAgencyOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgeSalesRoomOuthouse">Lista de produccion por edad y sales room</param>
    /// <history>
    ///   [vku] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByAgencyOuhouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, ProductionByAgencyOuthouseData lstRptProductionByAgencyOuthouse, EnumSalesByMemberShipType salesByMemberShipType = EnumSalesByMemberShipType.NoDetail)
    {
      DataTable dtData = null;
      var lstProductionByAgency = lstRptProductionByAgencyOuthouse.ProductionByAgencyOuthouse;
      if (!Convert.ToBoolean(salesByMemberShipType))
      {
        var lstProductionByAgencyAux = lstProductionByAgency.Select(c => new
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
        dtData = TableHelper.GetDataTableFromList(lstProductionByAgencyAux, replaceStringNullOrWhiteSpace: true);
        return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByAgencyOuthouse(), showRowGrandTotal: true, showColumnGrandTotal: true, fileFullPath: fileFullPath);
      }
      else
      {
        var lstProductionByAgencySalesMembershipType = lstRptProductionByAgencyOuthouse.ProductionByAgencyOuthouse_SalesByMembershipType;
        var lstMembershipType = lstRptProductionByAgencyOuthouse.MembershipTypes;

        var lstProductionByAgencySalesMembershipTypeAux = (from prodbyAgency in lstProductionByAgency
                                                           join prodByAgencyMemshipType in lstProductionByAgencySalesMembershipType on prodbyAgency.Agency equals prodByAgencyMemshipType.Agency
                                                           join mt in lstMembershipType on prodByAgencyMemshipType.MembershipType equals mt.mtID
                                                           select new
                                                           {
                                                             prodbyAgency.Agency,
                                                             prodbyAgency.AgencyN,
                                                             prodbyAgency.Books,
                                                             prodbyAgency.InOuts,
                                                             prodbyAgency.GrossBooks,
                                                             prodbyAgency.Shows,
                                                             prodbyAgency.WalkOuts,
                                                             prodbyAgency.Tours,
                                                             prodbyAgency.CourtesyTours,
                                                             prodbyAgency.SaveTours,
                                                             prodbyAgency.TotalTours,
                                                             prodbyAgency.UPS,
                                                             mt.mtN,
                                                             prodByAgencyMemshipType.Sales,
                                                             prodByAgencyMemshipType.SalesAmount,
                                                             SalesCancel = prodbyAgency.Sales_CANCEL,
                                                             SalesAmountCancel = prodbyAgency.SalesAmount_CANCEL,
                                                             SalesTotal = prodbyAgency.Sales_TOTAL,
                                                             SalesAmountTotal = prodbyAgency.SalesAmount_TOTAL,
                                                             prodbyAgency.ShowsFactor,
                                                             prodbyAgency.CancelFactor,
                                                             prodbyAgency.Efficiency,
                                                             prodbyAgency.ClosingFactor,
                                                             prodbyAgency.AverageSale
                                                           }).ToList();

        dtData = TableHelper.GetDataTableFromList(lstProductionByAgencySalesMembershipTypeAux, replaceStringNullOrWhiteSpace: true);
        return EpplusHelper.CreateExcelCustomPivot(dtData, filters, strReport, string.Empty, clsFormatReport.rptProductionByAgencySalesMembershipTypeOuthouse(), blnShowSubtotal: true, blnRowGrandTotal: true, blnColumnGrandTotal: true);
      }
    }
    #endregion ExportRptProductionByAgencyOuthouse

    #region ExportRptProductionByAgencySalesRoomOuthouse

    /// <summary>
    ///   Obtiene los datos para el reporte ProductionByAgencySalesRoomOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgencySalesRoomOuthouse">Lista de produccion por agencia y sala</param>
    /// <history>
    ///   [vku] 20/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByAgencySalesRoomOuhouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, ProductionByAgencySalesRoomOuthouseData lstRptProductionByAgencySalesRoomOuthouse)
    {
      var lstRptProductionByAgencySalesRoom = lstRptProductionByAgencySalesRoomOuthouse.ProductionByAgencySalesRoomOuthouse;
      var lstRptProductionByAgencySalesRoomOuthouseAux = lstRptProductionByAgencySalesRoom.Select(c => new
      {
        c.SalesRoomN,
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
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByAgencySalesRoomOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByAgencySalesRoomOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByAgencySalesRoomOuthouse

    #region ExportRptProductionByAgencyMarketHotelOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByAgencyMarketHotelOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgencyMarketHotelOuthouse">Lista de produccion por agencia, mercado y hotel</param>
    /// <history>
    ///   [vku] 15/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByAgencyMarketHotelOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByAgencyMarketHotelOuthouse> lstRptProductionByAgencyMarketHotelOuthouse)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByAgencyMarketHotelOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByAgencyMarketHotelOuthouse

    #region ExportRptProductionByCoupleTypeOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByCoupleTypeOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByAgencyMarketHotelOuthouse">Lista de produccion por agencia, mercado y hotel</param>
    /// <history>
    ///   [vku] 18/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByCoupleTypeOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByCoupleTypeOuthouse> lstRptProductionByCoupleTypeOuthouse)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByCoupleTypeOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByCoupleTypeOuthouse

    #region ExportRptProductionByCoupleTypeSalesRoomOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByCoupleTypeSalesRoomOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByCoupleTypeSalesRoomOuthouse">Lista de production por tipo de pareja y sala</param>
    /// <history>
    ///   [vku] 18/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByCoupleTypeSalesRoomOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByCoupleTypeSalesRoomOuthouse> lstRptProductionByCoupleTypeSalesRoomOuthouse)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByCoupleTypeSalesRoomOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #region ExportRptProductionByFlightSalesRoom
    /// <summary>
    ///  Obtiene los datos para el reporte productionByFlightSalesRoom
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByFlightSalesRoom">lista de produccion por vuelo y sala</param>
    /// <history>
    ///   [VKU] 17/May/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByFlightSalesRoom(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByFlightSalesRoom> lstRptProductionByFlightSalesRoom)
    {
      var lstRptProductionByFlightSalesRoomAux = lstRptProductionByFlightSalesRoom.Select(c => new
      {
        c.FlightID,
        c.SalesRoomID,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Subtotal,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByFlightSalesRoomAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByFlightSalesRoom(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion

    #endregion ExportRptProductionByCoupleTypeSalesRoomOuthouse

    #region ExportRptProductionByGiftInvitation

    /// <summary>
    ///   Obtiene los datos para el reporte ProductionByGiftInvitation
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByGiftInvitation">Lista de produccion por regalo de invitacion</param>
    /// <history>
    ///   [vku] 21/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByGiftInvitation(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByGiftInvitation> lstRptProductionByGiftInvitation)
    {
      var lstRptProductionByGiftInvitationAux = lstRptProductionByGiftInvitation.Select(c => new
      {
        c.GiftID,
        c.GiftN,
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

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByGiftInvitationAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByGiftInvitation(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByGiftInvitation

    #region ExportRptProductionByGiftInvitationSalesRoom

    /// <summary>
    ///   Obtiene los datos para el reporte ProductionByGiftInvitationSalesRoom
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByGiftInvitation">Lista de produccion por regalo de invitacion y sala</param>
    /// <history>
    ///   [vku] 21/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByGiftInvitationSalesRoom(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByGiftInvitationSalesRoom> lstRptProductionByGiftInvitationSalesRoom)
    {
      var lstRptProductionByGiftInvitationSalesRoomAux = lstRptProductionByGiftInvitationSalesRoom.Select(c => new
      {
        c.SalesRoomN,
        c.GiftID,
        c.GiftN,
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

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByGiftInvitationSalesRoomAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByGiftInvitationSalesRoom(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByGiftInvitationSalesRoom

    #region ExportRptProductionByGuestStatusOuthouse

    /// <summary>
    ///   Obtiene los datos para el reporte ProductionByGuestStatusOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByGuestStatusOuthouse">Lista de produccion por status de huesped</param>
    /// <history>
    ///   [vku] 22/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByGuestStatusOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByGuestStatusOuthouse> lstRptProductionByGuestStatusOuthouse)
    {
      var lstRptProductionByGuestStatusOuthouseAux = lstRptProductionByGuestStatusOuthouse.Select(c => new
      {
        c.LeadSource,
        c.GuestStatus,
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

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByGuestStatusOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByGuestStatusOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }
  #endregion

    #region ExportRptProductionByHotel
  /// <summary>
  ///   Obtiene los datos para el reporte productionbyhotel 
  /// </summary>
  /// <param name="strReport">Nombre del reporte</param>
  /// <param name="string.Empty">Rango de fechas</param>
  /// <param name="filters">Filtros</param>
  /// <param name="lstRptProductionByHotel">Lista de produccion por hotel</param>
  /// <history>
  ///   [vku] 17/May/2016 Created
  /// </history>
  public static FileInfo ExportRptProductionByHotel(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByHotel> lstRptProductionByHotel)
    {
      var lstRptProductionByHotelAux = lstRptProductionByHotel.Select(c => new
      {
        c.HotelID,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Subtotal,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByHotelAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByHotel(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion

    #region ExportRptProductionByHotelSalesRoom
    /// <summary>
    ///  Obtiene los datos para el reporte de produccion por hotel y sala
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByHotelSalesRoom">Lista de produccion por hotel y sala</param>
    /// <history>
    ///   [vku] 19/May/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByHotelSalesRoom(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByHotelSalesRoom> lstRptProductionByHotelSalesRoom)
    {
      var lstRptProductionByHotelSalesRoomAux = lstRptProductionByHotelSalesRoom.Select(c => new
      {
        c.HotelID,
        c.SalesRoomID,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Subtotal,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByHotelSalesRoomAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByHotelSalesRoom(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion

    #region ExportRptProductionByHotelGroup
    /// <summary>
    ///   Obtiene los datos para el reporte de produccion por grupo hotelero
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Flitros</param>
    /// <param name="lstRptProductionByHotelGroup">lista de produccion por grupo hotelero</param>
    /// <history>
    ///   [vku] 19/May/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByHotelGroup(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByHotelGroup> lstRptProductionByHotelGroup)
    {
      var lstRptProductionByHotelGroupAux = lstRptProductionByHotelGroup.Select(c => new
      {
        c.HotelID,
        c.hoGroup,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Subtotal,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByHotelGroupAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByHotelGroup(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion

    #region ExportRptProductionByHotelGroupSalesRoom
    /// <summary>
    ///  Obtiene los datos para el reporte de produccion por grupo hotelero y sala
    /// </summary>
    /// <param name="strReport"></param>
    /// <param name="string.Empty"></param>
    /// <param name="filters"></param>
    /// <param name="lstRptProductionByHotelGroupSalesRoom"></param>
    /// <history>
    ///   [vku] 19/May/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByHotelGroupSalesRoom(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByHotelGroupSalesRoom> lstRptProductionByHotelGroupSalesRoom)
    {
      var lstRptProductionByHotelGroupSalesRoomAux = lstRptProductionByHotelGroupSalesRoom.Select(c => new
      {
        c.hoGroup,
        c.SalesRoomID,
        c.HotelID,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Subtotal,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByHotelGroupSalesRoomAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByHotelGroupSalesRoom(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion

    #region ExportRptProductionByNationalityOuthouse

    /// <summary>
    ///  Obtienes los datos para el reporte ExportRptProductionByNationalityOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByNationalityOuthouse">Lista de produccion por nacionalidad</param>
    /// <history>
    ///   [vku] 22/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByNationalityOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByNationalityOuthouse> lstRptProductionByNationalityOuthouse)
    {
      var lstRptProductionByNationalityOuthouseAux = lstRptProductionByNationalityOuthouse.Select(c => new
      {
        c.Nationality,
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

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByNationalityOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByNationalityOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByNationalityOuthouse

    #region ExportRptProductionByNationalitySalesRoomOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByNationalitySalesRoomOuthouse
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">filtros</param>
    /// <param name="lstRptProductionByNationalitySalesRoomOuthouse">lista de produccion por nacionalidad y sala</param>
    /// <history>
    ///   [vku] 22/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByNationalitySalesRoomOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByNationalitySalesRoomOuthouse> lstRptProductionByNationalitySalesRoomOuthouse)
    {
      var lstRptProductionByNationalitySalesRoomOuthouseAux = lstRptProductionByNationalitySalesRoomOuthouse.Select(c => new
      {
        c.SalesRoomN,
        c.Nationality,
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

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByNationalitySalesRoomOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByNationalitySalesRoomOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByNationalitySalesRoomOuthouse

    #region ExportRptProductionByPRSalesRoomOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByPRSalesRoom
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByPRSalesRoomOuthouse">Lista de production por PR y sala</param>
    /// <history>
    ///   [vku] 25/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByPRSalesRoomOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomOuthouse)
    {
      var lstRptProductionByPRSalesRoomOuthouseAux = lstRptProductionByPRSalesRoomOuthouse.Select(c => new
      {
        c.SalesRoomN,
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
        c.Sales_OOP,
        c.SalesAmount_OOP,
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

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByPRSalesRoomOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByPRSalesRoomOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByPRSalesRoomOuthouse

    #region ExportRptProductionByPRContactOuthouse

    /// <summary>
    ///  Obtiene los datos para el reporte ProductionByPRContact
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de folios</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByPRContactOuthouse">Lista de produccion por PR de contacto</param>
    /// <history>
    ///   [vku] 25/Abr/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByPRContactOuthouse(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByPRContactOuthouse> lstRptProductionByPRContactOuthouse)
    {
      var lstRptProductionByPRContactOuthouseAux = lstRptProductionByPRContactOuthouse.Select(c => new
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

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByPRContactOuthouseAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByPRContactOuthouse(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }
  #endregion

    #region ExportRptProductionByWave
  /// <summary>
  ///   Obtiene los datos para el reporte de produccion por horario
  /// </summary>
  /// <param name="strReport">Nombre del reporte</param>
  /// <param name="string.Empty">Rango de folios</param>
  /// <param name="filters">Filtros</param>
  /// <param name="lstRptProductionByWave">Lista de produccion por horario</param>
  /// <history>
  ///   [vku] 20/May/2016 Created
  /// </history>
  public static FileInfo ExportRptProductionByWave(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByWave> lstRptProductionByWave)
    {

      var lstRptProductionByWaveAux = lstRptProductionByWave.Select(c => new
      {
        BookTime = DateTime.Parse(c.BookTime.ToString()).ToString("hh:mm:ss tt"),
        c.SalesRoomID,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Subtotal,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByWaveAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByWave(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion

    #region ExportRptProductionByWaveSalesRoom
    /// <summary>
    ///   Obtiene los datos para el reporte de produccion por horario y sala
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de folios</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptProductionByWave">Lista de produccion por horario</param>
    /// <history>
    ///   [vku] 20/May/2016 Created
    /// </history>
    public static FileInfo ExportRptProductionByWaveSalesRoom(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByWaveSalesRoom> lstRptProductionByWaveSalesRoom)
    {

      var lstRptProductionByWaveAux = lstRptProductionByWaveSalesRoom.Select(c => new
      {
        BookTime = DateTime.Parse(c.BookTime.ToString()).ToString("hh:mm:ss tt"),
        c.SalesRoomID,
        c.Books,
        c.InOuts,
        c.GrossBooks,
        c.Shows,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_OOP,
        c.SalesAmount_OOP,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_CANCEL,
        c.SalesAmount_CANCEL,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Subtotal,
        c.ShowsFactor,
        c.CancelFactor,
        c.Efficiency,
        c.ClosingFactor,
        c.AverageSale
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptProductionByWaveAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptProductionByWave(), showRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion

    #region ExportRptFoliosInvitationByDateFolio

    /// <summary>
    ///  Obtiene los datos para el reporte FoliosInvitationByDateFolio
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptFoliosInvitationByDateFolio">Lista de Folios Invitation</param>
    /// <history>
    ///   [vku] 03/May/2016 Created
    /// </history>
    public static FileInfo ExportRptFoliosInvitationByDateFolio(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptFoliosInvitationByDateFolio> lstRptFoliosInvitationByDateFolio)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstRptFoliosInvitationByDateFolio);
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, strReport, string.Empty, clsFormatReport.rptFoliosInvitationByDateFolio(), fileFullPath: fileFullPath);
    }

    #endregion ExportRptFoliosInvitationByDateFolio

    #region ExportRptFoliosInvitationOuthouseByPR
    /// <summary>
    ///  Obtiene los datos para el reporte FoliosInvitationOuthouseByPR
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptFoliosInvitationOuthouseByPR">Lista de Folios de invitationbyPR</param>
    /// <history>
    ///   [vku] 05/May/2016 Created
    /// </history>
    public static FileInfo ExportRptFoliosInvitationOuthouseByPR(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptFoliosInvitationsOuthouseByPR> lstRptFoliosInvitationOuthouseByPR)
    {
      var lstRptFoliosInvitationOuthouseByPRAux = lstRptFoliosInvitationOuthouseByPR.Select(c => new
      {
        c.peN,
        c.guStatus,
        c.guSerie,   
        c.guFrom,
        c.guTo,
        c.lsN,
        c.guLastName1,
        c.guFirstName1,
        c.guBookD,
      }).OrderBy(c=>c.peN).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptFoliosInvitationOuthouseByPRAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptFoliosInvitationOuthouseByPR(), fileFullPath: fileFullPath);
    }
    #endregion

    #region ExportRptFoliosCxCByPR
    /// <summary>
    ///   Obtiene los datos para el reporte FoliosCxCByPR
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptFoliosCxCByPR">Lista de Folios CxC by PR</param>
    /// <history>
    ///   [vku] 06/May/2016 Created
    /// </history>
    public static FileInfo ExportRptFoliosCxCByPR(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptFoliosCxCByPR> lstRptFoliosCxCByPR)
    {
      var lstRptFoliosCxCByPRAux = lstRptFoliosCxCByPR.Select(c => new
      {
        c.peN,
        c.guStatus,
        c.guFrom,
        c.guTo,
        c.lsN,
        c.guLastName1,
        c.guFirstName1,
        c.guBookD,
      }).OrderBy(c=>c.peN).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptFoliosCxCByPRAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, strReport, string.Empty, clsFormatReport.rptFoliosCxCByPR(), fileFullPath: fileFullPath);  
    }
    #endregion

    #region ExportRptFoliosCXC
    /// <summary>
    ///   Obtiene los datos para el reporte Folios CXC
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <param name="string.Empty">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptFoliosCxC">Lista de folios CXC</param>
    /// <history>
    ///   [vku] 07/May/2016 Created
    /// </history>
    public static FileInfo ExportRptFoliosCXC(string strReport, string fileFullPath, List<Tuple<string, string>> filters, List<RptFoliosCXC> lstRptFoliosCXC)
    {
      var lstRptFoliosCXCAux = lstRptFoliosCXC.Select(c => new
      {
        c.EsShow,
        c.Tipo,
        c.bdFolioCXC,
        c.PR,
        c.PRN,
        c.guOutInvitNum,
        c.lsN,
        c.guLastName1,
        c.guFirstName1,
        c.guBookD,
        c.bdUserCXC,
        c.peN,
        c.bdEntryDCXC
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(lstRptFoliosCXCAux);
      return EpplusHelper.CreateExcelCustom(dtData, filters, strReport, string.Empty, clsFormatReport.rptFoliosCXC(), fileFullPath: fileFullPath);
    }
    #endregion
  }
}