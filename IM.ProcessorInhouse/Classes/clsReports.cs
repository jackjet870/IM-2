using IM.Base.Helpers;
using IM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace IM.ProcessorInhouse.Classes
{
  internal class clsReports
  {
    public static FileInfo ExportRptCostByPR(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptCostByPR> listRptCostByPrs)
    {
      var listRptCostByPrsAux = listRptCostByPrs.Select(c => new { c.PR, c.PRN, c.Shows, c.TotalCost }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptCostByPrsAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptCostByPRFormat(), true);
    }

    public static FileInfo ExportRptCostByPRWithDetailGifts(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptCostByPRWithDetailGifts> listRptCostByPRWithDetailGifts)
    {
      var listRptCostByPRWithDetailGiftsAux = listRptCostByPRWithDetailGifts.Select(c => new { c.PR, c.PRN, c.Qty, c.GiftsID, c.GiftsName, c.Shows, c.TotalCost }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptCostByPRWithDetailGiftsAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptCostByPRWithDetailGiftsFormat(), true);
    }

    public static FileInfo ExportRptFollowUpByAgencies(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptFollowUpByAgency> listRptFollowUpByAgencies)
    {
      var listRptFollowUpByAgenciesAux = listRptFollowUpByAgencies.Select(c => new
      {
        c.OriginallyAvailable,
        c.Market,
        c.Agency,
        c.AgencyN,
        c.Contacts,
        c.Availables,
        c.FollowUps,
        c.Books,
        c.Shows,
        c.Sales,
        c.SalesAmount
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptFollowUpByAgenciesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptFollowUpByAgencyFormat(), true);
    }

    public static FileInfo ExportRptFollowUpByPRs(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptFollowUpByPR> listRptFollowUpByPR)
    {
      var listRptFollowUpByPRAux = listRptFollowUpByPR.Select(c => new
      {
        c.Status,
        c.PRID,
        c.PRN,
        c.Contacts,
        c.Availables,
        c.FollowUps,
        c.Books,
        c.Shows,
        c.Sales,
        c.SalesAmount
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(listRptFollowUpByPRAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptFollowUpByPRFormat(), true);
    }

    public static FileInfo ExportProductionByAgeInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByAgeInhouse> listRptProductionByAgeInhouses)
    {
      var listRptProductionByAgeInhousesAux = listRptProductionByAgeInhouses.Select(c => new
      {
        c.Age,
        c.Arrivals,
        c.Contacts,
        c.Availables,
        c.GrossBooks,
        c.Directs,
        TBooks = c.Books,
        c.GrossShows,
        TShows = c.Shows,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Sales_PR,
        c.SalesAmount_PR,
        c.Sales_SELFGEN,
        c.SalesAmount_SELFGEN
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByAgeInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByAgeInhouseFormat(), true);
    }

    public static FileInfo ExportProductionByAgeMarketOriginallyAvailableInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByAgeMarketOriginallyAvailableInhouse> listProductionByAgeMarketOriginallyAvailableInhouses)
    {
      var listProductionByAgeMarketOriginallyAvailableInhousesAux = listProductionByAgeMarketOriginallyAvailableInhouses.Select(c => new
      {
        c.OriginallyAvailable,
        c.Market,
        c.Age,
        c.Arrivals,
        c.Contacts,
        c.Availables,
        c.GrossBooks,
        c.Directs,
        TBooks = c.Books,
        c.GrossShows,
        TShows = c.Shows,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Sales_PR,
        c.SalesAmount_PR,
        c.Sales_SELFGEN,
        c.SalesAmount_SELFGEN
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listProductionByAgeMarketOriginallyAvailableInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetProductionByAgeMarketOriginallyAvailableInhouseFormat(), true);
    }

    public static FileInfo ExportRptProductionByContractAgencyInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptProductionByContractAgencyInhouse> listRptProductionByContractAgencyInhouses)
    {
      var listRptProductionByContractAgencyInhousesAux = listRptProductionByContractAgencyInhouses.Select(c => new
      {
        c.Contract,
        c.ContractN,
        c.Agency,
        c.AgencyN,
        c.Arrivals,
        c.Contacts,
        c.Availables,
        c.GrossBooks,
        c.Directs,
        TBooks = c.Books,
        c.GrossShows,
        TShows = c.Shows,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Sales_PR,
        c.SalesAmount_PR,
        c.Sales_SELFGEN,
        c.SalesAmount_SELFGEN
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByContractAgencyInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByContractAgencyInhouseFormat(), true);
    }

    public static FileInfo ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptProductionByContractAgencyMarketOriginallyAvailableInhouse> listRptProductionByContractAgencyMarketOriginallyAvailableInhouses)
    {
      var listRptProductionByContractAgencyInhousesAux = listRptProductionByContractAgencyMarketOriginallyAvailableInhouses.Select(c => new
      {
        c.OriginallyAvailable,
        c.Market,
        c.Contract,
        c.ContractN,
        c.Agency,
        c.AgencyN,
        c.Arrivals,
        c.Contacts,
        c.Availables,
        c.GrossBooks,
        c.Directs,
        TBooks = c.Books,
        c.GrossShows,
        TShows = c.Shows,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Sales_PR,
        c.SalesAmount_PR,
        c.Sales_SELFGEN,
        c.SalesAmount_SELFGEN
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByContractAgencyInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat(), true);
    }
  }
}