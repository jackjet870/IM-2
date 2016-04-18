using IM.Base.Helpers;
using IM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace IM.ProcessorInhouse.Classes
{
  internal static class clsReports
  {
    #region ExportRptCostByPR

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptCostByPR
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptCostByPrs">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptCostByPR(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptCostByPR> listRptCostByPrs)
    {
      var listRptCostByPrsAux = listRptCostByPrs.Select(c => new { c.PR, c.PRN, c.Shows, c.TotalCost }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptCostByPrsAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptCostByPRFormat(), true);
    }

    #endregion ExportRptCostByPR

    #region ExportRptCostByPRWithDetailGifts

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptCostByPRWithDetailGifts
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptCostByPRWithDetailGifts">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptCostByPRWithDetailGifts(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptCostByPRWithDetailGifts> listRptCostByPRWithDetailGifts)
    {
      var listRptCostByPRWithDetailGiftsAux = listRptCostByPRWithDetailGifts.Select(c => new
      {
        PR = c.PR + "  " + c.PRN,
        c.Qty,
        c.GiftsID,
        c.GiftsName,
        c.Shows,
        c.TotalCost
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptCostByPRWithDetailGiftsAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptCostByPRWithDetailGiftsFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptCostByPRWithDetailGifts

    #region ExportRptFollowUpByAgencies

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptFollowUpByAgency
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptFollowUpByAgencies">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptFollowUpByAgencies(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptFollowUpByAgency> listRptFollowUpByAgencies)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptFollowUpByAgencyFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptFollowUpByAgencies

    #region ExportRptFollowUpByPRs

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptFollowUpByPR
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptFollowUpByPR">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptFollowUpByPRs(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptFollowUpByPR> listRptFollowUpByPR)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptFollowUpByPRFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptFollowUpByPRs

    #region ExportProductionByAgeInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByAgeInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByAgeInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportProductionByAgeInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
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

    #endregion ExportProductionByAgeInhouses

    #region ExportProductionByAgeMarketOriginallyAvailableInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByAgeMarketOriginallyAvailableInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listProductionByAgeMarketOriginallyAvailableInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportProductionByAgeMarketOriginallyAvailableInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetProductionByAgeMarketOriginallyAvailableInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportProductionByAgeMarketOriginallyAvailableInhouses

    #region ExportRptProductionByContractAgencyInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByContractAgencyInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByContractAgencyInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByContractAgencyInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptProductionByContractAgencyInhouse> listRptProductionByContractAgencyInhouses)
    {
      var listRptProductionByContractAgencyInhousesAux = listRptProductionByContractAgencyInhouses.Select(c => new
      {
        c.Contract,
        c.ContractN,
        Agency = c.Agency + "  " + c.AgencyN,
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
      }).OrderBy(c => c.Agency).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByContractAgencyInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByContractAgencyInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptProductionByContractAgencyInhouses

    #region ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByContractAgencyMarketOriginallyAvailableInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByContractAgencyMarketOriginallyAvailableInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptProductionByContractAgencyMarketOriginallyAvailableInhouse> listRptProductionByContractAgencyMarketOriginallyAvailableInhouses)
    {
      var listRptProductionByContractAgencyInhousesAux = listRptProductionByContractAgencyMarketOriginallyAvailableInhouses.Select(c => new
      {
        c.OriginallyAvailable,
        c.Market,
        c.Contract,
        c.ContractN,
        Agency = c.Agency + "  " + c.AgencyN,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses

    #region ExportRptProductionByCoupleTypeInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByCoupleTypeInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByCoupleTypeInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByCoupleTypeInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters, List<RptProductionByCoupleTypeInhouse> listRptProductionByCoupleTypeInhouses)
    {
      var listRptProductionByCoupleTypeInhousesAux = listRptProductionByCoupleTypeInhouses.Select(c => new
      {
        c.CoupleType,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByCoupleTypeInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByCoupleTypeInhouseFormat(), true);
    }

    #endregion ExportRptProductionByCoupleTypeInhouses

    #region ExportRptProductionByNationalityInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByNationalityInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByNationalityInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByNationalityInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByNationalityInhouse> listRptProductionByNationalityInhouses)
    {
      var listRptProductionByNationalityInhousesAux = listRptProductionByNationalityInhouses.Select(c => new
      {
        c.Nationality,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByNationalityInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByNationalityInhouseFormat(), true);
    }

    #endregion ExportRptProductionByNationalityInhouses

    #region ExportRptProductionByNationalityMarketOriginallyAvailableInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByNationalityMarketOriginallyAvailableInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByNationalityMarketOriginallyAvailableInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByNationalityMarketOriginallyAvailableInhouses(string reportname, string dateRangeFileNameRep,
      List<Tuple<string, string>> filters, List<RptProductionByNationalityMarketOriginallyAvailableInhouse> listRptProductionByNationalityMarketOriginallyAvailableInhouses)
    {
      var listRptProductionByNationalityMarketOriginallyAvailableInhousesAux = listRptProductionByNationalityMarketOriginallyAvailableInhouses.Select(c => new
      {
        c.OriginallyAvailable,
        c.Market,
        c.Nationality,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByNationalityMarketOriginallyAvailableInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetProductionByNationalityMarketOriginallyAvailableInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptProductionByNationalityMarketOriginallyAvailableInhouses

    #region ExportRptProductionByDeskInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByDeskInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByDeskInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByDeskInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByDeskInhouse> listRptProductionByDeskInhouses)
    {
      var listRptProductionByDeskInhousesAux = listRptProductionByDeskInhouses.Select(c => new
      {
        c.Desk,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByDeskInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByDeskInhouseFormat(), true);
    }

    #endregion ExportRptProductionByDeskInhouses

    #region ExportRptProductionByGroupInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByGroupInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByGroupInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByGroupInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByGroupInhouse> listRptProductionByGroupInhouses)
    {
      var listRptProductionByGroupInhousesAux = listRptProductionByGroupInhouses.Select(c => new
      {
        c.Groups,
        c.Integrants,
        c.TotalIntegrants,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByGroupInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByGroupInhouseFormat(), true);
    }

    #endregion ExportRptProductionByGroupInhouses

    #region ExportRptProductionByPRGroupInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByPRGroupInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByPRGroupInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByPRGroupInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByPRGroupInhouse> listRptProductionByPRGroupInhouses)
    {
      var listRptProductionByPRGroupInhousesAux = listRptProductionByPRGroupInhouses.Where(c => c.Type != "TOTAL").Select(c => new
      {
        PR = c.PR + "  " + c.PRN,
        c.Type,
        c.Groups,
        c.Integrants,
        c.TotalIntegrants,
        c.Assigns,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByPRGroupInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByPRGroupInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptProductionByPRGroupInhouses

    #region ExportRptProductionByGuestStatusInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByGuestStatusInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByGuestStatusInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByGuestStatusInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByGuestStatusInhouse> listRptProductionByGuestStatusInhouses)
    {
      var listRptProductionByGuestStatusInhousesAux = listRptProductionByGuestStatusInhouses.Select(c => new
      {
        c.GuestStatus,
        c.LeadSource,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByGuestStatusInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByGuestStatusInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptProductionByGuestStatusInhouses

    #region ExportRptProductionByMemberTypeAgencyInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByMemberTypeAgencyInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByMemberTypeAgencyInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByMemberTypeAgencyInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByMemberTypeAgencyInhouse> listRptProductionByMemberTypeAgencyInhouses)
    {
      var listRptProductionByMemberTypeAgencyInhousesAux = listRptProductionByMemberTypeAgencyInhouses.Select(c => new
      {
        Agency = c.Agency + "  " + c.AgencyN,
        c.MemberType,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByMemberTypeAgencyInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByMemberTypeAgencyInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptProductionByMemberTypeAgencyInhouses

    #region ExportRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses(string reportname, string dateRangeFileNameRep,
      List<Tuple<string, string>> filters, List<RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse> listRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses)
    {
      var listRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhousesAux = listRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses.Select(c => new
      {
        c.OriginallyAvailable,
        c.Market,
        Agency = c.Agency + "  " + c.AgencyN,
        c.MemberType,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses

    #region ExportRptProductionByPRInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByPRInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByPRInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByPRInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByPRInhouse> listRptProductionByPRInhouses)
    {
      var listRptProductionByPRInhousesAux = listRptProductionByPRInhouses.Select(c => new
      {
        c.Status,
        c.PRID,
        c.PRN,
        c.Assigns,
        c.Contacts,
        c.Availables,
        c.BooksNoDirects,
        c.Directs,
        TBooks = c.Books,
        c.ShowsNoDirects,
        c.Shows,
        c.InOuts,
        c.WalkOuts,
        c.Tours,
        c.CourtesyTours,
        c.SaveTours,
        c.TotalTours,
        c.UPS,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_OOP,
        c.SalesAmount_OOP
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByPRInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByPRInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptProductionByPRInhouses

    #region ExportRptProductionByPRSalesRoomInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByPRSalesRoomInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByPRSalesRoomInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByPRSalesRoomInhouses(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptProductionByPRSalesRoomInhouse> listRptProductionByPRSalesRoomInhouses)
    {
      var listRptProductionByPRSalesRoomInhousesAux = listRptProductionByPRSalesRoomInhouses.Select(c => new
      {
        c.SalesRoom,
        c.Status,
        c.PRID,
        c.PRN,
        c.Assigns,
        c.Contacts,
        c.Availables,
        c.BooksNoDirects,
        c.Directs,
        TBooks = c.Books,
        c.ShowsNoDirects,
        c.Shows,
        c.InOuts,
        c.WalkOuts,
        c.Tours,
        c.CourtesyTours,
        c.SaveTours,
        c.TotalTours,
        c.UPS,
        c.Sales_PROC,
        c.SalesAmount_PROC,
        c.Sales_PEND,
        c.SalesAmount_PEND,
        c.Sales_OOP,
        c.SalesAmount_OOP
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByPRSalesRoomInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptProductionByPRSalesRoomInhouseFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptProductionByPRSalesRoomInhouses

    #region ExportRptRepsPayments

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptRepsPayment
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptRepsPayments">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptRepsPayments(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptRepsPayment> listRptRepsPayments)
    {
      var listRptRepsPaymentsAux = listRptRepsPayments.Select(c => new
      {
        c.guag,
        c.guBookD,
        c.GuestName,
        Q = c.Q == "ü" ? "✓" : "",
        c.agShowPay,
        c.agSale,
        c.agTotalSalePay,
        c.agTotalPay,
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptRepsPaymentsAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptRepsPaymentFormat(), true, showRowHeaders: true);
    }

    #endregion ExportRptRepsPayments

    #region ExportRptRepsPaymentSummaries

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptRepsPaymentSummary
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="dateRangeFileNameRep">Rango de fechas</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptRepsPaymentSummaries">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptRepsPaymentSummaries(string reportname, string dateRangeFileNameRep, List<Tuple<string, string>> filters,
      List<RptRepsPaymentSummary> listRptRepsPaymentSummaries)
    {
      var listRptRepsPaymentSummariesAux = listRptRepsPaymentSummaries.Select(c => new
      {
        c.agrp,
        c.TotalShow,
        c.agShowPay,
        c.SumagShowPay,
        c.TotalSales,
        c.agSalePay,
        c.SumSalesPay,
        c.TotalPay
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptRepsPaymentSummariesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, reportname, dateRangeFileNameRep, clsFormatReport.GetRptRepsPaymentSummaryFormat());
    }

    #endregion ExportRptRepsPaymentSummaries
  }
}