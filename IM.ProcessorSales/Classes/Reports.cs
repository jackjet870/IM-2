using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IM.Base.Helpers;
using IM.Model;
using IM.Model.Enums;

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
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptConcentrateDailySales
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="goals">Listado de Goals para comparar</param>
    /// <history>
    /// [ecanul] 13/05/2016 Created
    /// </history>
    public static FileInfo RptConcentrateDailySales(string report, string dateRangeFileName, DateTime date,
      List<Tuple<string, string>> filters, List<RptConcentrateDailySales> lstReport, List<GoalsHelpper> goals)
    {
      #region Llena Datos
      for (var i = 0; i < lstReport.Count; i++)
      {
        goals[i].SalesRoom = lstReport[i].SalesRoom;
        goals[i].UPS = lstReport[i].UPS;
        goals[i].Sales = lstReport[i].Sales;
        goals[i].SalesAmountOPP = lstReport[i].SalesAmountOPP;
        goals[i].SalesAmountFall = lstReport[i].SalesAmountFall;
        goals[i].SalesAmountCancel = lstReport[i].SalesAmountCancel;
        goals[i].SalesAmount = lstReport[i].SalesAmount;
        goals[i].DownPact = lstReport[i].DownPact;
        goals[i].DownColl = lstReport[i].DownColl;
      }
      #endregion

      #region Seleccion de datos necesarios
      var customList = goals.Select(c => new
      {
        c.SalesRoom,
        Goal = c.goal == 0 ? null : c.goal,
        Difference = (c.goal - c.SalesAmount),
        UPS = c.UPS == 0 ? null : c.UPS,
        Sales = c.Sales == 0 ? null : c.Sales,
        Proc = (c.SalesAmount - c.SalesAmountOPP) == 0 ? null : (c.SalesAmount - c.SalesAmountOPP),
        OPP = c.SalesAmountOPP == 0 ? null : c.SalesAmountOPP,
        Fall = c.SalesAmountFall == 0 ? null : c.SalesAmountFall,
        Cancel = c.SalesAmountCancel == 0 ? null : c.SalesAmountCancel,
        TotalProc = c.SalesAmount == 0 ? null : c.SalesAmount,
        Pact = c.DownPact == 0 ? null : c.DownPact,
        Collect = c.DownColl == 0 ? null : c.DownColl
      }).ToList();
      #endregion

      #region CreateExtraFieldHeader
      //Se obtiene el total de las metas (Goals)
      decimal goal = Convert.ToDecimal(customList.Select(c => c.Goal).Sum());

      //Se obtiene el total de dias del mes
      var totDays = DateTime.DaysInMonth(date.Year, date.Month);
      //Se obtiene el pronostocp
      decimal forecast = goal / totDays * date.Day;
      // Se obtiene la diferencia
      var diference = Convert.ToDecimal(customList.Select(c => c.TotalProc).Sum() - forecast);
      //Redondeamos los resultados a 2 decimales
      forecast = decimal.Round(forecast, 2);
      diference = decimal.Round(diference, 2);
      //Creamos los ExtraHeader
      List<Tuple<string, string, EnumFormatTypeExcel>> prueba = new List<Tuple<string, string, EnumFormatTypeExcel>>
      {
        new Tuple<string, string,EnumFormatTypeExcel>("Goal", goal.ToString(),EnumFormatTypeExcel.DecimalNumberWithCero),
        new Tuple<string, string,EnumFormatTypeExcel>("Forecast", forecast.ToString(),EnumFormatTypeExcel.DecimalNumberWithCero),
        new Tuple<string, string,EnumFormatTypeExcel>("Difference", diference.ToString(),EnumFormatTypeExcel.DecimalNumberWithCero)
      }; 
      #endregion

      var dtData = TableHelper.GetDataTableFromList(customList, true, false);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, dateRangeFileName,
        FormatReport.RptConcentrateDailySales(), true, extraFieldHeader: prueba, numRows: 3);
    }
    #endregion

    #endregion
  }
}
