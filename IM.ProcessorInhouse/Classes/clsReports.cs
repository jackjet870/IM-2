using IM.Base.Helpers;
using IM.Model;
using IM.Model.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IM.ProcessorInhouse.Classes
{
  internal static class clsReports
  {
    #region ExportRptCostByPR

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptCostByPR
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptCostByPrs">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal async static Task<FileInfo> ExportRptCostByPR(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptCostByPR> listRptCostByPrs)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(listRptCostByPrs, replaceStringNullOrWhiteSpace: true);
      return  await EpplusHelper.CreateCustomExcel(dtData, filters, reportname, string.Empty, clsFormatReport.GetRptCostByPRFormat(), blnRowGrandTotal:true, fileFullPath: fileFullPath, addEnumeration:true);
      //EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptCostByPRFormat(), true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptCostByPR

    #region ExportRptCostByPRWithDetailGifts

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptCostByPRWithDetailGifts
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptCostByPRWithDetailGifts">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptCostByPRWithDetailGifts(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptCostByPRWithDetailGiftsFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptCostByPRWithDetailGifts

    #region ExportRptFollowUpByAgencies

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptFollowUpByAgency
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptFollowUpByAgencies">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptFollowUpByAgencies(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptFollowUpByAgency> listRptFollowUpByAgencies)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptFollowUpByAgencyFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptFollowUpByAgencies

    #region ExportRptFollowUpByPRs

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptFollowUpByPR
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptFollowUpByPR">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptFollowUpByPRs(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptFollowUpByPR> listRptFollowUpByPR)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptFollowUpByPRFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptFollowUpByPRs

    #region ExportProductionByAgeInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByAgeInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByAgeInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportProductionByAgeInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByAgeInhouseFormat(), true, fileFullPath: fileFullPath);
    }

    #endregion ExportProductionByAgeInhouses

    #region ExportProductionByAgeMarketOriginallyAvailableInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByAgeMarketOriginallyAvailableInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listProductionByAgeMarketOriginallyAvailableInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportProductionByAgeMarketOriginallyAvailableInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetProductionByAgeMarketOriginallyAvailableInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportProductionByAgeMarketOriginallyAvailableInhouses

    #region ExportRptProductionByContractAgencyInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByContractAgencyInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByContractAgencyInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByContractAgencyInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByContractAgencyInhouse> listRptProductionByContractAgencyInhouses)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByContractAgencyInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByContractAgencyInhouses

    #region ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByContractAgencyMarketOriginallyAvailableInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByContractAgencyMarketOriginallyAvailableInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 11/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByContractAgencyMarketOriginallyAvailableInhouse> listRptProductionByContractAgencyMarketOriginallyAvailableInhouses)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByContractAgencyMarketOriginallyAvailableInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses

    #region ExportRptProductionByCoupleTypeInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByCoupleTypeInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByCoupleTypeInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByCoupleTypeInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByCoupleTypeInhouse> listRptProductionByCoupleTypeInhouses)
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByCoupleTypeInhouseFormat(), true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByCoupleTypeInhouses

    #region ExportRptProductionByNationalityInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByNationalityInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByNationalityInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByNationalityInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByNationalityInhouseFormat(), true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByNationalityInhouses

    #region ExportRptProductionByNationalityMarketOriginallyAvailableInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByNationalityMarketOriginallyAvailableInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByNationalityMarketOriginallyAvailableInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByNationalityMarketOriginallyAvailableInhouses(string reportname, string fileFullPath,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetProductionByNationalityMarketOriginallyAvailableInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByNationalityMarketOriginallyAvailableInhouses

    #region ExportRptProductionByDeskInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByDeskInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByDeskInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByDeskInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByDeskInhouseFormat(), true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByDeskInhouses

    #region ExportRptProductionByGiftQuantities

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByGiftQuantity
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByGiftQuantities">Lista de datos para el reporte.</param>
    /// <returns></returns>
    internal static FileInfo ExportRptProductionByGiftQuantities(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByGiftQuantity> listRptProductionByGiftQuantities)
    {
      var listRptProductionByDeskInhousesAux = listRptProductionByGiftQuantities.Select(c => new
      {
        Gift = c.GiftID + "  " + c.GiftN,
        c.PRID,
        c.PRN,
        c.Quantity,
        c.Amount,
        c.GrossBooks,
        c.Directs,
        TBooks = c.Books,
        c.Deposits,
        c.GrossShows,
        c.InOuts,
        TShows = c.Shows,
        c.SelfGens,
        c.Sales_TOTAL,
        c.SalesAmount_TOTAL,
        c.Sales_PR,
        c.SalesAmount_PR,
        c.Sales_SELFGEN,
        c.SalesAmount_SELFGEN
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByDeskInhousesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByGiftQuantityFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByGiftQuantities

    #region ExportRptProductionByGroupInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByGroupInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByGroupInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByGroupInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByGroupInhouseFormat(), true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByGroupInhouses

    #region ExportRptProductionByPRGroupInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByPRGroupInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByPRGroupInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByPRGroupInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByPRGroupInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByPRGroupInhouses

    #region ExportRptProductionByGuestStatusInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByGuestStatusInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByGuestStatusInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByGuestStatusInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByGuestStatusInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByGuestStatusInhouses

    #region ExportRptProductionByMemberTypeAgencyInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByMemberTypeAgencyInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByMemberTypeAgencyInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByMemberTypeAgencyInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByMemberTypeAgencyInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByMemberTypeAgencyInhouses

    #region ExportRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses(string reportname, string fileFullPath,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses

    #region ExportRptProductionByPRInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByPRInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByPRInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByPRInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByPRInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByPRInhouses

    #region ExportRptProductionByPRSalesRoomInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByPRSalesRoomInhouse
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByPRSalesRoomInhouses">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByPRSalesRoomInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByPRSalesRoomInhouseFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByPRSalesRoomInhouses

    #region ExportRptRepsPayments

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptRepsPayment
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptRepsPayments">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptRepsPayments(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptRepsPaymentFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptRepsPayments

    #region ExportRptRepsPaymentSummaries

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptRepsPaymentSummary
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptRepsPaymentSummaries">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 18/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptRepsPaymentSummaries(string reportname, string fileFullPath, List<Tuple<string, string>> filters,
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
      return EpplusHelper.CreateGeneralRptExcel(filters, dtData, reportname, string.Empty, clsFormatReport.GetRptRepsPaymentSummaryFormat(), fileFullPath: fileFullPath);
    }

    #endregion ExportRptRepsPaymentSummaries

    #region ExportRptGiftsReceivedBySR

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptGiftsReceivedBySR
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstRptGiftsReceivedBySR">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 22/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptGiftsReceivedBySR(string reportname, string fileFullPath, List<Tuple<string, string>> filters, GiftsReceivedBySRData lstRptGiftsReceivedBySR)
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
      return EpplusHelper.CreateExcelCustomPivot(dtData, filters, reportname, string.Empty, clsFormatReport.GetRptGiftsReceivedBySRFormat(), blnShowSubtotal: true, blnRowGrandTotal: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptGiftsReceivedBySR

    #region ExportRptUnavailableMotivesByAgencies

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptUnavailableMotivesByAgency
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptUnavailableMotivesByAgencies">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 22/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptUnavailableMotivesByAgencies(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptUnavailableMotivesByAgency> listRptUnavailableMotivesByAgencies)
    {
      var listRptUnavailableMotivesByAgenciesAux = listRptUnavailableMotivesByAgencies.Select(c => new
      {
        c.UnavailMot,
        c.Market,
        c.agID,
        c.Agency,
        c.Arrivals,
        c.ArrivalsPercentage,
        c.ByUser
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptUnavailableMotivesByAgenciesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptUnavailableMotivesByAgencyFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptUnavailableMotivesByAgencies

    #region ExportRptShowFactorByBookingDates

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptShowFactorByBookingDate
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptShowFactorByBookingDates">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 22/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptShowFactorByBookingDates(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptShowFactorByBookingDate> listRptShowFactorByBookingDates)
    {
      var listRptShowFactorByBookingDatesAux = listRptShowFactorByBookingDates.Select(c => new
      {
        c.Group,
        c.Agency,
        c.AgencyN,
        c.Difference,
        c.Books,
        c.Shows,
        c.InOuts,
        c.WalkOuts
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptShowFactorByBookingDatesAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptShowFactorByBookingDateFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptShowFactorByBookingDates

    #region ExportRptProductionByAgencyMonthly

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByAgencyMonthly
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByAgencyMonthly">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 25/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByAgencyMonthly(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByAgencyMonthly> listRptProductionByAgencyMonthly)
    {
      //Se pone el valor AgencyN a los que no tienen valor y que sean del mismo AgencyID
      listRptProductionByAgencyMonthly.
        Where(c => string.IsNullOrEmpty(c.AgencyN)).ToList().
        ForEach(c => c.AgencyN =
        listRptProductionByAgencyMonthly.FirstOrDefault(a => a.Agency == c.Agency && !string.IsNullOrEmpty(a.AgencyN))?.AgencyN);

      var listRptProductionByAgencyMonthlyAux = listRptProductionByAgencyMonthly.
        OrderBy(c => c.Year).ThenBy(c => c.AgencyTotal).ThenBy(c => c.Agency).
        ThenBy(c => c.LeadSourceTotal).ThenBy(c => c.LeadSource).ThenBy(c => c.Month).
        Select(c => new
        {
          c.Year,
          Agency = c.Agency + "  " + c.AgencyN,
          c.LeadSource,
          Month = new DateTime(Convert.ToInt32(c.Year), Convert.ToInt32(c.Month), 1),
          c.Arrivals,
          c.Contacts,
          c.Availables,
          Books = c.GrossBooks,
          TBooks = c.Books,
          Shows = c.GrossShows,
          TShows = c.Shows,
          c.Sales,
          c.SalesAmount
        }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByAgencyMonthlyAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByAgencyMonthlyFormat(), showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByAgencyMonthly

    #region ExportGraphNotBookingArrival

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de GraphNotBookingArrivals
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="dtmDate">Fecha de inicio de la semana</param>
    /// <param name="listGraphNotBookingArrivals">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 27/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportGraphNotBookingArrival(string reportname, string fileFullPath, List<Tuple<string, string>> filters, DateTime dtmDate, Tuple<GraphTotals, List<GraphNotBookingArrivals>, List<GraphNotBookingArrivals>> listGraphNotBookingArrivals)
    {
      var list1 = listGraphNotBookingArrivals.Item2.Select(c => new { c.NotBookingMotiveN, c.Percentage, c.Arrivals }).ToList();
      var list2 = listGraphNotBookingArrivals.Item3.Select(c => new { c.NotBookingMotiveN, c.Percentage, c.Arrivals }).ToList();

      DataTable dtData1 = TableHelper.GetDataTableFromList(list1, replaceStringNullOrWhiteSpace: true);
      DataTable dtData2 = TableHelper.GetDataTableFromList(list2, replaceStringNullOrWhiteSpace: true);

      // Establecemos las fechas de la grafica
      DateTime dtmFromWeek = dtmDate;
      DateTime dtmToWeek = dtmDate.AddDays(6);
      DateTime dtmFromMonth = new DateTime(dtmDate.Year, dtmDate.Month, 1);
      DateTime dtmToMonth = dtmFromWeek.Month == dtmToWeek.Month ? dtmToWeek : dtmFromMonth.AddMonths(1).AddDays(-1);

      string strWeek = DateHelper.DateRange(dtmFromWeek, dtmToWeek);
      string strMonth = DateHelper.DateRange(dtmFromMonth, dtmToMonth);

      return EpplusHelper.CreateGraphExcel(filters, listGraphNotBookingArrivals.Item1, reportname, string.Empty, Tuple.Create(strWeek, dtData1, clsFormatReport.GetGraphNotBookingArrivalsFormat()), Tuple.Create(strMonth, dtData2, clsFormatReport.GetGraphNotBookingArrivalsFormat()), fileFullPath: fileFullPath);
    }

    #endregion ExportGraphNotBookingArrival

    #region ExportGraphProduction

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de GraphProduction
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="dtmDate">Fecha de inicio de la semana</param>
    /// <param name="listGraphProduction">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 03/05/2016 Created
    /// </history>
    internal static FileInfo ExportGraphProduction(string reportname, string fileFullPath, List<Tuple<string, string>> filters, DateTime dtmDate, Tuple<GraphMaximum, List<GraphProduction_Weeks>, List<GraphProduction_Months>> listGraphProduction)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      Stream template = assembly.GetManifestResourceStream(assembly.GetName().Name + ".ExcelTemplate.GraphProductionTemplate.xlsx");
      int periods = listGraphProduction.Item2.Count;
      DateTime dtmFromWeek, dtmToWeek, dtmFromMonth, dtmToMonth;
      var listGraphProduction_Weeks = listGraphProduction.Item2.Select(c =>
      {
        dtmFromWeek = dtmDate.AddDays(Convert.ToInt32(periods - c.Period) * -7);
        dtmToWeek = dtmFromWeek.AddDays(6);
        return new
        {
          Period = DateHelper.DateRange(dtmFromWeek, dtmToWeek),
          c.Arrivals,
          c.Availables,
          c.AvailablesFactor,
          c.Contacts,
          c.ContactsFactor,
          c.Books,
          c.BooksFactor,
          c.Shows,
          c.ShowsFactor,
          c.Sales,
          c.ClosingFactor
        };
      }).ToList();

      periods = listGraphProduction.Item3.Count;
      var listGraphProduction_Months = listGraphProduction.Item3.Select(c =>
      {
        dtmFromMonth = dtmDate.AddMonths(Convert.ToInt32(periods - c.Period) * -1).AddDays(-dtmDate.Day + 1);
        dtmToMonth = dtmDate.AddDays(6).Month == dtmFromMonth.Month ? dtmDate.AddDays(6) : dtmFromMonth.AddMonths(1).AddDays(-1);
        return new
        {
          Period = DateHelper.DateRange(dtmFromMonth, dtmToMonth),
          c.Arrivals,
          c.Availables,
          c.AvailablesFactor,
          c.Contacts,
          c.ContactsFactor,
          c.Books,
          c.BooksFactor,
          c.Shows,
          c.ShowsFactor,
          c.Sales,
          c.ClosingFactor,
          c.SalesAmount,
          c.Efficiency
        };
      }).ToList();

      DataTable dtDataWeeks = TableHelper.GetDataTableFromList(listGraphProduction_Weeks, replaceStringNullOrWhiteSpace: true);
      dtDataWeeks.TableName = "GraphProduction_Weeks";
      DataTable dtDataMonths = TableHelper.GetDataTableFromList(listGraphProduction_Months, replaceStringNullOrWhiteSpace: true);
      dtDataMonths.TableName = "GraphProduction_Months";

      template = EpplusHelper.UpdateTableExcel(template, dtDataWeeks);
      template = EpplusHelper.UpdateTableExcel(template, dtDataMonths);

      return EpplusHelper.CreateExcelFromTemplate(filters, template, reportname, string.Empty, fileFullPath: fileFullPath);
    }

    #endregion ExportGraphProduction

    #region ExportGraphUnavailableArrivals

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de GraphUnavailableArrivals
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="dtmDate">Fecha de inicio de la semana</param>
    /// <param name="listGraphUnavailableArrivals">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 27/Abr/2016 Created
    /// </history>
    internal static FileInfo ExportGraphUnavailableArrivals(string reportname, string fileFullPath, List<Tuple<string, string>> filters, DateTime dtmDate, Tuple<GraphTotals, List<GraphUnavailableArrivals>, List<GraphUnavailableArrivals>> listGraphUnavailableArrivals)
    {
      var list1 = listGraphUnavailableArrivals.Item2.Select(c => new { c.UnavailableMotiveN, c.Percentage, c.ByUser, c.Arrivals }).ToList();
      var list2 = listGraphUnavailableArrivals.Item3.Select(c => new { c.UnavailableMotiveN, c.Percentage, c.ByUser, c.Arrivals }).ToList();

      DataTable dtData1 = TableHelper.GetDataTableFromList(list1, replaceStringNullOrWhiteSpace: true);
      DataTable dtData2 = TableHelper.GetDataTableFromList(list2, replaceStringNullOrWhiteSpace: true);

      // Establecemos las fechas de la grafica
      DateTime dtmFromWeek = dtmDate;
      DateTime dtmToWeek = dtmDate.AddDays(6);
      DateTime dtmFromMonth = new DateTime(dtmDate.Year, dtmDate.Month, 1);
      DateTime dtmToMonth = dtmFromWeek.Month == dtmToWeek.Month ? dtmToWeek : dtmFromMonth.AddMonths(1).AddDays(-1);

      string strWeek = DateHelper.DateRange(dtmFromWeek, dtmToWeek);
      string strMonth = DateHelper.DateRange(dtmFromMonth, dtmToMonth);

      return EpplusHelper.CreateGraphExcel(filters, listGraphUnavailableArrivals.Item1, reportname, string.Empty, Tuple.Create(strWeek, dtData1, clsFormatReport.GetGraphUnavailableArrivalsFormat()), Tuple.Create(strMonth, dtData2, clsFormatReport.GetGraphUnavailableArrivalsFormat()), fileFullPath: fileFullPath);
    }

    #endregion ExportGraphUnavailableArrivals

    #region ExportRptProductionByMembers

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByMember
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByMembers">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 04/05/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByMembers(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByMember> listRptProductionByMembers)
    {
      var listRptProductionByMembersAux = listRptProductionByMembers.Select(c => new
      {
        c.Wholesaler,
        c.Club,
        c.GuestType,
        c.Company,
        c.Application,
        c.Name,
        c.Date,
        c.Amount,
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
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByMembersAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByMemberFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByMembers

    #region ExportProductionByAgencyInhouses

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByAgencyInhouses
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="productionByAgencyInhouseData">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 1305/2016 Created
    /// </history>
    internal static FileInfo ExportProductionByAgencyInhouses(string reportname, string fileFullPath, List<Tuple<string, string>> filters, ProductionByAgencyInhouseData productionByAgencyInhouseData)
    {
      var productionByAgencyInhousesTotal = productionByAgencyInhouseData.ProductionByAgencyInhouses.Select(x =>
      new
      {
        prodByAgencyIh = x,
        mtN = "NULL",
        PartialSales = (int?)0,
        PartialSalesAmount = (decimal?)0
      }).ToList();

      var productionByAgencyInhousesPartial = (from prodByAgencyIh in productionByAgencyInhouseData.ProductionByAgencyInhouses
                                               join prodByAgencyIhSalesMemShip in productionByAgencyInhouseData.ProductionByAgencyInhouse_SalesByMembershipTypes on prodByAgencyIh.Agency equals prodByAgencyIhSalesMemShip.Agency
                                               join salesMemShip in productionByAgencyInhouseData.MembershipTypeShorts on prodByAgencyIhSalesMemShip.MembershipType equals salesMemShip.mtID
                                               select new
                                               {
                                                 prodByAgencyIh,
                                                 salesMemShip.mtN,
                                                 PartialSales = prodByAgencyIhSalesMemShip.Sales,
                                                 PartialSalesAmount = prodByAgencyIhSalesMemShip.SalesAmount
                                               }).ToList();

      var productionByAgencyInhouses = productionByAgencyInhousesPartial.Union(productionByAgencyInhousesTotal).
        Select(c => new
        {
          c.prodByAgencyIh.OriginallyAvailable,
          c.prodByAgencyIh.MarketN,
          c.prodByAgencyIh.Agency,
          c.prodByAgencyIh.AgencyN,
          c.prodByAgencyIh.Arrivals,
          c.prodByAgencyIh.Contacts,
          ContactsFactor = 0m,
          c.prodByAgencyIh.Availables,
          AvailablesFactor = 0m,
          c.prodByAgencyIh.GrossBooks,
          c.prodByAgencyIh.Directs,
          c.prodByAgencyIh.Books,
          BooksFactor = 0m,
          c.prodByAgencyIh.GrossShows,
          c.prodByAgencyIh.Shows,
          ShowsFactor = 0m,
          c.prodByAgencyIh.InOuts,
          c.prodByAgencyIh.WalkOuts,
          c.prodByAgencyIh.Tours,
          c.prodByAgencyIh.CourtesyTours,
          c.prodByAgencyIh.SaveTours,
          c.prodByAgencyIh.TotalTours,
          c.prodByAgencyIh.UPS,
          c.prodByAgencyIh.Sales,
          c.prodByAgencyIh.SalesAmount,
          Efficiency = 0m,
          ClosingFactor = 0m,
          AverageSale = 0m,
          c.mtN,
          c.PartialSales,
          c.PartialSalesAmount
        }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(productionByAgencyInhouses);
      return EpplusHelper.CreateExcelCustomPivot(dtData, filters, reportname, string.Empty, clsFormatReport.RptProductionByAgencyInhouse(), true, true, true, fileFullPath: fileFullPath);
    }

    #endregion ExportProductionByAgencyInhouses

    #region ExportRptProductionByMonths

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptProductionByMonth
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptProductionByMonths">Lista de datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 05/05/2016 Created
    /// </history>
    internal static FileInfo ExportRptProductionByMonths(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptProductionByMonth> listRptProductionByMonths)
    {
      var listRptProductionByMembersAux = listRptProductionByMonths.Select(c => new
      {
        c.Year,
        Month = new DateTime(Convert.ToInt32(c.Year), Convert.ToInt32(c.Month), 1),
        c.Contacts,
        c.Availables,
        c.GrossBooks,
        TBooks = c.Books,
        c.GrossShows,
        TShows = c.Shows,
        c.Sales,
        c.SalesAmount
      }).ToList();
      DataTable dtData = TableHelper.GetDataTableFromList(listRptProductionByMembersAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(true, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptProductionByMonthFormat(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptProductionByMonths

    #region ExportRptScoreByPrs

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptScoreByPR
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="scoreByPRData">Datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 16/05/2016 Created
    /// </history>
    internal static FileInfo ExportRptScoreByPrs(string reportname, string fileFullPath, List<Tuple<string, string>> filters, ScoreByPRData scoreByPRData)
    {
      var scoreByPRAux = (from sbpr in scoreByPRData.ScoreByPR
                          join srd in scoreByPRData.ScoreRuleDetail on sbpr.ScoreRule equals srd.sisu.ToString()
                          select new
                          {
                            sbpr.ScoreRuleTypeN,
                            ScoreRule = Convert.ToInt32(sbpr.ScoreRule),
                            sbpr.ScoreRuleN,
                            sbpr.ScoreRuleConcept,
                            ScoreRuleConceptN = scoreByPRData.ScoreRuleConcept.Single(x => x.spID == srd.sisp).spN,
                            SiScore = $"({srd.siScore.ToString("0.##")} {(srd.siScore > 1 ? "pts" : "pt")})",
                            sbpr.PR,
                            sbpr.PRN,
                            Shows = sbpr.ScoreRuleConcept == srd.sisp ? sbpr.Shows : 0,
                            Score = sbpr.ScoreRuleConcept == srd.sisp ? sbpr.Score : 0,
                            TShows = scoreByPRData.ScoreByPR.Where(x => x.PR == sbpr.PR).Sum(x => x.Shows),
                            TScore = scoreByPRData.ScoreByPR.Where(x => x.PR == sbpr.PR).Sum(x => x.Score)
                          }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(scoreByPRAux);
      return EpplusHelper.CreateExcelCustomPivot(dtData, filters, reportname, string.Empty, clsFormatReport.RptScoreByPR(), blnRowGrandTotal: true, fileFullPath:fileFullPath);
    }

    #endregion ExportRptScoreByPrs

    #region ExportRptContactBookShowQuinellas

    /// <summary>
    /// Obtiene los datos para exportar a Excel el reporte de RptContactBookShowQuinellas
    /// </summary>
    /// <param name="reportname">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Filtros</param>
    /// <param name="listRptContactBookShowQuinellas">Datos para el reporte.</param>
    /// <returns>FileInfo</returns>
    /// <history>
    /// [aalcocer] 16/05/2016 Created
    /// </history>
    internal static FileInfo ExportRptContactBookShowQuinellas(string reportname, string fileFullPath, List<Tuple<string, string>> filters, List<RptContactBookShowQuinellas> listRptContactBookShowQuinellas)
    {
      var total = listRptContactBookShowQuinellas.Where(c => c.Subgroup.ToUpper() == "TOTAL").ToList();
      var partial = listRptContactBookShowQuinellas.Where(c => !total.Contains(c)).ToList();

      var other = new List<RptContactBookShowQuinellas>();

      total.ForEach(c =>
      {
        if (partial.Any(p => p.Market == c.Market))
        {
          var otherAux = partial
          .SingleOrDefault(p => p.Market == c.Market && p.Year == c.Year && p.Month == c.Month && p.LeadSource == c.LeadSource);

          other.Add(new RptContactBookShowQuinellas
          {
            Group = c.Group,
            Market = c.Market,
            Subgroup = "OTROS",
            Year = c.Year,
            Month = c.Month,
            znN = c.znN,
            LeadSource = c.LeadSource,
            Arrivals = c.Arrivals - (otherAux == null ? 0 : otherAux.Arrivals),
            Contacts = c.Contacts - (otherAux == null ? 0 : otherAux.Contacts),
            Availables = c.Availables - (otherAux == null ? 0 : otherAux.Availables),
            GrossBooks = c.GrossBooks - (otherAux == null ? 0 : otherAux.GrossBooks),
            Books = c.Books - (otherAux == null ? 0 : otherAux.Books),
            GrossShows = c.GrossShows - (otherAux == null ? 0 : otherAux.GrossShows),
            Shows = c.Shows - (otherAux == null ? 0 : otherAux.Shows),
            Sales = c.Sales - (otherAux == null ? 0 : otherAux.Sales),
            SalesAmount = c.SalesAmount - (otherAux == null ? 0 : otherAux.SalesAmount),
          });
        }
      });

      total.RemoveAll(x => other.Select(o => o.Market).Contains(x.Market));

      var listRptContactBookShowQuinellasAux = partial.Union(other).Union(total).Select(c => new
      {
        c.Group,
        c.Market,
        c.Subgroup,
        c.Year,
        Month = new DateTime(Convert.ToInt32(c.Year), Convert.ToInt32(c.Month), 1),
        c.znN,
        c.LeadSource,
        c.Arrivals,
        c.Contacts,
        c.Availables,
        c.GrossBooks,
        TBooks = c.Books,
        c.GrossShows,
        TShows = c.Shows,
        c.Sales,
        c.SalesAmount,
        TotalFactor = c.Arrivals
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(listRptContactBookShowQuinellasAux, replaceStringNullOrWhiteSpace: true);
      return EpplusHelper.CreatePivotRptExcel(true, filters, dtData, reportname, string.Empty, clsFormatReport.GetRptContactBookShowQuinellasFormat(), true, fileFullPath: fileFullPath);
    }

    #endregion ExportRptContactBookShowQuinellas
  }
}