using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IM.Base.Helpers;
using IM.Model;

namespace IM.ProcessorSales.Classes
{
  public class Reports
  {
    #region Reports By Sales Room

    #region RptStatisticsByLocation
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte Statics by Location
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <history>
    /// [ecanul] 07/05/2016 Created
    /// </history>
    public static FileInfo RptStatisticsByLocation(string report, string dateRangeFileName,
      List<Tuple<string, string>> filters, List<RptStatisticsByLocation> lstReport)
    {
      var customList =
        lstReport.Select(c => new {c.Location, c.SalesAmount, c.Shows, c.SalesVIP, c.SalesRegular, c.SalesExit, c.Sales, c.SalesAmountOOP}).ToList();
      var dtData = TableHelper.GetDataTableFromList(customList, true, false);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, dateRangeFileName, FormatReport.RptStatisticsByLocation(), true);
    }

    #endregion

    #region RptStatisticsBySalesRoomLocation
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte de Statics by Sales Room Location
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <history>
    /// [ecanul] 06/05/2016 Created
    /// [ecanul] 09/05/2016 Modified Corregido error del calculo de las columnas  C%, EFF, AV/S
    /// </history>
    public static FileInfo RptStatisticsBySalesRoomLocation(string report, string dateRangeFileName,
      List<Tuple<string, string>> filters, List<RptStatisticsBySalesRoomLocation> lstReport)
    {
      var customList =
        lstReport.Select(
          c =>
            new
            {
              c.Zona,
              c.SalesRoom,
              c.Program,
              c.SalesRoomId,
              c.LocationId,
              c.Location,
              c.SalesAmount,
              c.Shows,
              c.SalesVIP,
              c.SalesRegular,
              c.SalesExit,
              c.Sales,
              c.SalesAmountOOP
            }).ToList();
      var dtData = TableHelper.GetDataTableFromList(customList, true, false);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, dateRangeFileName, FormatReport.RptStatisticsBySalesRoomLocation(), true, showRowHeaders: true);
    }

    #endregion

    #region RptStaticsByLocationMonthly
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte Statics by Location Monthly
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// </history>
    public static FileInfo RptStaticsByLocationMonthly(string report, string dateRangeFileName,
      List<Tuple<string, string>> filters, List<RptStatisticsByLocationMonthly> lstReport)
    {
      var customList =
        lstReport.Select(
          c => new
          {
            c.Program,
            c.Location,
            c.SalesAmountPrevious,
            c.UPSPrevious,
            c.Goal,
            c.Books,
            c.GrossUPS,
            c.Directs,
            c.Shows,
            c.SalesAmount,
            c.Sales
          }).ToList();
      var dtData = TableHelper.GetDataTableFromList(customList, true, false);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, dateRangeFileName, FormatReport.RptStatisticsByLocationMonthly(), true);
    }

    #endregion

    #region RptSalesByLocationMonthly
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte Sales by Location Monthly
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="dateRangefileName">Rango de fechas</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// </history>
    public static FileInfo RptSalesByLocationMonthly(string report, string dateRangefileName,
      List<Tuple<string, string>> filters, List<RptSalesByLocationMonthly> lstReport)
    {
      var customList =
        lstReport.Select(
          c =>
            new {c.Location, c.Year, c.MonthN, c.Shows, c.Sales, c.SalesAmountTotal, c.SalesAmountCancel, c.SalesAmount})
          .ToList();
      var dtData = TableHelper.GetDataTableFromList(customList, true, false);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, dateRangefileName,
        FormatReport.RptSalesByLocationMonthly(), true);
    }
    #endregion

    #region RptConcentrateDailySales

    //public static FileInfo RptConcentrateDailySales(string report, string dateRangeFileName,
    //  List<Tuple<string, string>> filters, List<RptDailySalesDetail> lstReport, List<GoalsHelpper> goals)
    //{
    //  foreach (var goal in goals)
    //  {
    //    var customList =
    //      lstReport.Select(
    //        c =>
    //          new
    //          {
    //            SRoom = goal.salesRoom.srN,
    //            goal.goal,
    //            c.Shows /*UPS*/,
    //            c.SalesRegular,
    //            c.SalesExit,
    //            c.SalesVIP,
    //            sales = (c.SalesRegular + c.SalesExit + c.SalesVIP)

    //          })
    //        .ToList();
    //  }
    //}
    #endregion

    #endregion
  }
}
