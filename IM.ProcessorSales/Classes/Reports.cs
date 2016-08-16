using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using IM.Base.Helpers;
using IM.Model;
using IM.Model.Enums;
using System.Text;

namespace IM.ProcessorSales.Classes
{
  public static class Reports
  {
    #region Reports By Sales Room

    #region RptStatisticsByLocation
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte Statics by Location
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <history>
    /// [ecanul] 07/05/2016 Created
    /// </history>
    public static FileInfo RptStatisticsByLocation(string report, string fileFullPath,
      List<Tuple<string, string>> filters, List<RptStatisticsByLocation> lstReport)
    {
      var customList =
        lstReport.Select(c => new {c.Location, c.SalesAmount, c.Shows, c.SalesVIP, c.SalesRegular, c.SalesExit, c.Sales, c.SalesAmountOOP}).ToList();
      var dtData = TableHelper.GetDataTableFromList(customList, true, false);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, string.Empty, FormatReport.RptStatisticsByLocation(), true, fileFullPath:fileFullPath);
    }

    #endregion

    #region RptStatisticsBySalesRoomLocation
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte de Statics by Sales Room Location
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <history>
    /// [ecanul] 06/05/2016 Created
    /// [ecanul] 09/05/2016 Modified Corregido error del calculo de las columnas  C%, EFF, AV/S
    /// </history>
    public static FileInfo RptStatisticsBySalesRoomLocation(string report, string fileFullPath,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, string.Empty, FormatReport.RptStatisticsBySalesRoomLocation(), true, showRowHeaders: true, fileFullPath: fileFullPath);
    }

    #endregion

    #region RptStaticsByLocationMonthly
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte Statics by Location Monthly
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// </history>
    public static FileInfo RptStaticsByLocationMonthly(string report, string fileFullPath,
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
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, string.Empty, FormatReport.RptStatisticsByLocationMonthly(), true, fileFullPath: fileFullPath);
    }

    #endregion

    #region RptSalesByLocationMonthly
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte Sales by Location Monthly
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <history>
    /// [ecanul] 10/05/2016 Created
    /// </history>
    public static FileInfo RptSalesByLocationMonthly(string report, string fileFullPath,
      List<Tuple<string, string>> filters, List<RptSalesByLocationMonthly> lstReport)
    {
      var customList =
        lstReport.Select(
          c =>
            new {c.Location, c.Year, c.MonthN, c.Shows, c.Sales, c.SalesAmountTotal, c.SalesAmountCancel, c.SalesAmount})
          .ToList();
      var dtData = TableHelper.GetDataTableFromList(customList, true, false);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, string.Empty,
        FormatReport.RptSalesByLocationMonthly(), true, fileFullPath: fileFullPath);
    }
    #endregion

    #region RptConcentrateDailySales
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptConcentrateDailySales
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="goals">Listado de Goals para comparar</param>
    /// <history>
    /// [ecanul] 13/05/2016 Created
    /// </history>
    public static FileInfo RptConcentrateDailySales(string report, string fileFullPath, DateTime date,
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

      #region BodyReport
      var customList = goals.Select(c => new
      {
        c.SalesRoom,
        Goal = c.Goals,
        Difference = (c.Goals - c.SalesAmount),
        UPS = c.UPS,
        Sales = c.Sales,
        Proc = c.SalesAmount - c.SalesAmountOPP,
        OPP =  c.SalesAmountOPP,
        Fall = c.SalesAmountFall,
        Cancel = c.SalesAmountCancel,
        TotalProc =  c.SalesAmount,
        Pact =  c.DownPact,
        Collect = c.DownColl
      }).ToList();
      var dtData = TableHelper.GetDataTableFromList(customList, true, false);
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
      List<Tuple<string, dynamic, EnumFormatTypeExcel>> extraHeader = new List<Tuple<string, dynamic, EnumFormatTypeExcel>>
      {
        new Tuple<string, dynamic,EnumFormatTypeExcel>("Goal", goal.ToString(),EnumFormatTypeExcel.DecimalNumberWithCero),
        new Tuple<string, dynamic,EnumFormatTypeExcel>("Forecast", forecast.ToString(),EnumFormatTypeExcel.DecimalNumberWithCero),
        new Tuple<string, dynamic,EnumFormatTypeExcel>("Difference", diference.ToString(),EnumFormatTypeExcel.DecimalNumberWithCero)
      }; 
      #endregion
      
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, string.Empty,
        FormatReport.RptConcentrateDailySales(), true, extraFieldHeader: extraHeader, numRows: 3, fileFullPath: fileFullPath);
    }
    #endregion

    #region RptDailySales

    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptDailySales
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="dateRangeFileName">Rango de fechas</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="reportHeader">Listado de report Header</param>
    /// <param name="dtStart">Fecha inicial del reporte</param>
    /// <param name="dtEnd">Fecha final del reporte</param>
    /// <param name="goal"></param>
    /// <history>
    /// [ecanul] 16/05/2016 Created
    /// [ecanul] 17/05/2016 Modified Ahora tiene Header
    /// </history>
    public static FileInfo RptDailySales(string report, string dateRangeFileName, string fileFullPath, List<Tuple<string, string>> filters, List<RptDailySalesDetail> lstReport, List<RptDailySalesHeader> reportHeader, DateTime dtStart, DateTime dtEnd, decimal goal)
    {
      #region BodyReport
      var customList = lstReport.Select(c => new
      {
        c.Date, //0
        UPS = c.Shows, //0
        Sale = c.SalesRegular, //1
        Exit = c.SalesExit, //2
        VIP = c.SalesVIP, //3
        TotalSales = c.SalesRegular + c.SalesExit + c.SalesVIP, //Sales 4
        Proc = c.SalesAmount - c.SalesAmountOOP, //5
        OOP = c.SalesAmountOOP, //6
        Fall = c.CnxSalesAmount, //7
        Cxld = c.SalesAmountCancel, //8
        TotalProc = c.SalesAmount, //9
        Pact = c.DownPact,//13
        Collect = c.DownColl//14
      }).ToList();
      var dtData = TableHelper.GetDataTableFromList(customList, true, false);
      #endregion

      #region ReportExtraHeader
      //Actual
      var dateRangeCurrent= dateRangeFileName;
      var shows = Convert.ToInt32(reportHeader[0].Shows);
      var salesAount = Convert.ToDecimal(reportHeader[0].SalesAmount);
      var eff = salesAount / shows;
      //Prev
      var dateRangePrev = DateHelper.DateRange(dtStart.AddYears(-1), dtEnd.AddYears(-1));
      var shPrev = Convert.ToInt32(reportHeader[0].ShowsPrevious);
      var SaPrev = Convert.ToDecimal(reportHeader[0].SalesAmountPrevious);
      var effPrev = SaPrev / shPrev;
      //Info
      var forecast = goal / DateTime.DaysInMonth(dtEnd.Year, dtEnd.Month) * dtEnd.Day;
      var diference = salesAount - forecast;
      
      List<Tuple<string, dynamic, EnumFormatTypeExcel>> extraHeader = new List<Tuple<string, dynamic, EnumFormatTypeExcel>>
      {
        //Prev
        new Tuple<string, dynamic, EnumFormatTypeExcel>(dateRangePrev, string.Empty,EnumFormatTypeExcel.General),
        new Tuple<string, dynamic, EnumFormatTypeExcel>("Volumen", decimal.Round(SaPrev,2),EnumFormatTypeExcel.Currency),
        new Tuple<string, dynamic, EnumFormatTypeExcel>("Efficiency", decimal.Round(effPrev,2) ,EnumFormatTypeExcel.DecimalNumberWithCero),
        new Tuple<string, dynamic, EnumFormatTypeExcel>("Toured", shPrev ,EnumFormatTypeExcel.NumberWithCero),
        //Current
        new Tuple<string, dynamic, EnumFormatTypeExcel>(dateRangeCurrent, string.Empty,EnumFormatTypeExcel.General),
        new Tuple<string, dynamic, EnumFormatTypeExcel>("Volumen", decimal.Round(salesAount,2),EnumFormatTypeExcel.Currency),
        new Tuple<string, dynamic, EnumFormatTypeExcel>("Efficiency", decimal.Round(eff,2) ,EnumFormatTypeExcel.DecimalNumberWithCero),
        new Tuple<string, dynamic, EnumFormatTypeExcel>("Toured", shows ,EnumFormatTypeExcel.NumberWithCero),
        //Info
        new Tuple<string, dynamic, EnumFormatTypeExcel>(string.Empty, string.Empty,EnumFormatTypeExcel.General),
        new Tuple<string, dynamic, EnumFormatTypeExcel>("Goal", goal,EnumFormatTypeExcel.DecimalNumberWithCero),
        new Tuple<string, dynamic, EnumFormatTypeExcel>("Forecast", forecast,EnumFormatTypeExcel.DecimalNumberWithCero),
        new Tuple<string, dynamic, EnumFormatTypeExcel>("Difference", diference,EnumFormatTypeExcel.DecimalNumberWithCero)
      };

      #endregion

      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, string.Empty,
       FormatReport.RptDailySales(), true, extraFieldHeader: extraHeader, numRows: 4, fileFullPath: fileFullPath);
    }
    #endregion

    #region RptManifest
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptManifest
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="dtStart">Fecha inicial del reporte</param>
    /// <param name="dtEnd">Fecha final del reporte</param>
    /// <history>
    ///  [ecanul] 07/06/2016 Created
    /// </history>
    public static FileInfo RptManifest(string report, string fileFullPath, List<Tuple<string, string>> filters,
      List<RptManifest> lstReport, DateTime dtStartm, DateTime dtEnd)
    {
      var dtData = TableHelper.GetDataTableFromList(lstReport, true, true);
      return EpplusHelper.CreateExcelCustom(dtData, filters, report, string.Empty, FormatReport.RptManifest(), blnShowSubtotal: true, blnRowGrandTotal: true, blnColumnGrandTotal: false, fileFullPath: fileFullPath);
    }
    #endregion

    #region RptFTMInOutHouse
    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptFTMIn&OutHouse
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="dtStart">Fecha inicial del reporte</param>
    /// <param name="dtEnd">Fecha final del reporte</param>
    /// <history>
    ///  [ecanul] 02/07/2016 Created
    /// </history>
    public static FileInfo RptFTMInOutHouse(string report, string fileFullPath, List<Tuple<string, string>> filters, List<RptFTMInOutHouse> lstReport,
       DateTime dtStartm, DateTime dtEnd)
    {
      var nlist = lstReport.Select(c =>
      new
      {
       // GR = true,
        c.Liner,
        c.peN,
        c.OOP,
        //Overflow
        c.OFSalesAmount,
        c.OFShows,
        c.OFSales,
        c.OFExit,
        c.OFTotal,
        //Regen
        c.RSalesAmount,
        c.RShows,
        c.RSales,
        c.RExit,
        c.RTotal,
        //Normal
        c.NSalesAmount,
        c.NShows,
        c.NSales,
        c.NExit,
        c.NTotal,
        //TOTAL
        c.TSalesAmount,
        c.TShows,
        c.TSales,
        c.TExit,
        c.TTotal,
        //Calculados
        EFFOver = c.NShows != 0 ? c.TSalesAmount / c.NShows : 0,
        EFF = c.TShows != 0 ? c.TSalesAmount / c.TShows : 0,
        CPer = c.TShows != 0 ? c.TTotal / c.TShows : 0,
        AVS = c.TTotal != 0 ? c.TSalesAmount / c.TTotal : 0
      }).ToList();
      var data = TableHelper.GetDataTableFromList(nlist, true, true);
      return EpplusHelper.CreateExcelCustom(data, filters, report, string.Empty, FormatReport.RptFTMInOutHouse(),  
        blnRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion

    #region RptStatisticsBySegments

    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptStatisticsBySegments
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="groupedByTeams">Agrupado por equipos</param>
    /// <history>
    ///  [aalcocer] 04/07/2016 Created
    /// </history>
    internal static FileInfo RptStatisticsBySegments(string report, string fileFullPath, List<Tuple<string, string>> filters, List<RptStatisticsBySegments> lstReport, bool groupedByTeams)
    {
      var lstReportAux = new List<dynamic>(); 
      if (groupedByTeams)
      {
        lstReportAux.AddRange(lstReport.Select(c => new
        {
          c.SegmentN,
          Team = c.TeamN + "   " + c.TeamLeaderN,
          c.Status,
          c.SalemanType,
          c.SalemanID,
          c.SalemanName,
          c.UPS,
          c.Sales,
          c.Amount
        }));
      }
      else
      {
        lstReportAux.AddRange(lstReport.Select(c => new
        {
          c.SegmentN,
          c.SalemanType,
          c.SalemanID,
          c.SalemanName,
          c.UPS,
          c.Sales,
          c.Amount
        }));
      }

      DataTable dtData = TableHelper.GetDataTableFromList(lstReportAux);
      return EpplusHelper.CreatePivotRptExcel(false, filters, dtData, report, string.Empty, groupedByTeams ? FormatReport.RptStatisticsBySegmentsGroupedByTeams() : FormatReport.RptStatisticsBySegments(), true, true, true, fileFullPath: fileFullPath);
    }
    #endregion

    #region RptStatisticsByCloser

    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptStatisticsByCloser
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="groupedByTeams">Agrupado por equipos</param>
    /// <history>
    ///  [aalcocer] 13/07/2016 Created
    ///  [ecanul]   16/08/2016 Modified, Se ha cambiado el formato Pivote a Excel Custom
    /// </history>
    internal static FileInfo RptStatisticsByCloser(string report, string fileFullPath, List<Tuple<string, string>> filters, List<RptStatisticsByCloser> lstReport, bool groupedByTeams)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstReport);
      return EpplusHelper.CreateExcelCustom(dtData, filters, report, string.Empty, FormatReport.RptStatisticsByCloser(groupedByTeams),
        blnRowGrandTotal: true, blnShowSubtotal: groupedByTeams, fileFullPath: fileFullPath);
    }
    #endregion RptStatisticsByCloser

    #region RptStatisticsByExitCloser

    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptStatisticsByExitCloser
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="groupedByTeams">Agrupado por equipos</param>
    /// <history>
    ///  [aalcocer] 18/07/2016 Created
    /// </history>
    internal static FileInfo RptStatisticsByExitCloser(string report, string fileFullPath, List<Tuple<string, string>> filters, List<RptStatisticsByExitCloser> lstReport, bool groupedByTeams)
    {
      var lstReportAux = new List<dynamic>(); 
      if (groupedByTeams)
      {
        lstReportAux.AddRange(lstReport.Select(c => new
        {
          Team = c.TeamN + "   " + c.TeamLeaderN,
          c.SalesmanStatus,
          c.SalemanType,
          c.SalemanID,
          c.SalemanName,
          c.SalesAmount,
          c.OPP,
          c.UPS,
          c.SalesAmountRange,
          c.Sales,
          c.SalesTotal,
          c.Efficiency,
          c.ClosingFactor,
          c.SaleAverage
        }));
      }
      else
      {
        lstReportAux.AddRange(lstReport.Select(c => new
        {
          c.SalemanType,
          c.SalemanID,
          c.SalemanName,
          c.SalesAmount,
          c.OPP,
          c.UPS,
          c.SalesAmountRange,
          c.Sales,
          c.SalesTotal,
          c.Efficiency,
          c.ClosingFactor,
          c.SaleAverage
        }));
      }

      DataTable dtData = TableHelper.GetDataTableFromList(lstReportAux);
      return EpplusHelper.CreateExcelCustomPivot(dtData, filters, report, string.Empty, groupedByTeams ? FormatReport.RptStatisticsByExitCloserGroupedByTeams() : FormatReport.RptStatisticsByExitCloser(),blnShowSubtotal:true, blnRowGrandTotal:true, fileFullPath: fileFullPath);
    }
    #endregion RptStatisticsByExitCloser

    #region RptSelfGenAndSelfGenTeam
    /// <summary>
    /// Obtiene un archivocon el reporte RptSelfGen&SelfGenTeam
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta de archivo </param>
    /// <param name="filters">Filtros</param>
    /// <param name="lstReport">Datos del reporte</param>
    /// <param name="dtStart">Fecha Inicio</param>
    /// <param name="dtEnd">Fecha Fin</param>
    /// <history>
    /// [ecanul] 26/07/2016 Created
    /// </history>
    public static FileInfo RptSelfGenAndSelfGenTeam(string report, string fileFullPath, List<Tuple<string,string>> filters, List<RptSelfGenTeam> lstReport, DateTime dtStart, DateTime dtEnd)
    {
      var data = lstReport.Select(c => new
      {
        c.Liner,
        c.SalesmanName,
        c.OOP,
        c.OFVol,
        c.OFUPS,
        c.OFSales,
        c.RGVol,
        c.RGUPS,
        c.RGSales,
        c.NVol,
        c.NUPS,
        c.NSales,
        c.TVol,
        c.TUPS,
        c.TSales,
        c.SelfGenType,
        EFF = c.TUPS != 0 ? c.TVol / c.TUPS : 0,
        CPer = c.TUPS != 0 ?  c.TSales / c.TUPS : 0,
        AVS = c.TSales != 0 ? c.TVol / c.TSales : 0
      }).ToList();
      var dtData = TableHelper.GetDataTableFromList(data, true, true);
      return EpplusHelper.CreateExcelCustom(dtData, filters, report, string.Empty, FormatReport.RptSelfGenAndSelfGenTeam(), blnShowSubtotal: true, blnRowGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion

    #region RptStatisticsByFTB

    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptStatisticsByFTB
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="groupedByTeams">Agrupado por equipos</param>
    /// <history>
    ///  [michan] 21/07/2016 Created
    /// </history>
    internal static FileInfo RptStatisticsByFTB(string report, string fileFullPath, List<Tuple<string, string>> filters, List<RptStatisticsByFTB> lstReport, bool groupedByTeams)
    {
      //var listTemp = lstReport.ToList();
      var dtData = TableHelper.GetDataTableFromList(lstReport, true, true);
      return EpplusHelper.CreateExcelCustom(dtData, filters, report, string.Empty, groupedByTeams ? FormatReport.RptStatisticsByFTBGroupedByTeams() : FormatReport.RptStatisticsByFTB(), blnShowSubtotal: true, blnRowGrandTotal: true, fileFullPath: fileFullPath);
      
    }
    #endregion RptStatisticsByFTB

    #region RptStatisticsByFTBByLocations

    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptStatisticsByFTB ByLocations
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="groupedByTeams">Agrupado por equipos</param>
    /// <history>
    ///  [michan] 22/07/2016 Created
    /// </history>
    internal static FileInfo RptStatisticsByFTBByLocations(string report, string fileFullPath, List<Tuple<string, string>> filters, List<RptStatisticsByFTBLocations> lstReport, bool groupedByTeams)
    {
      var dtData = TableHelper.GetDataTableFromList(lstReport, true, true);
      return EpplusHelper.CreateExcelCustom(dtData, filters, report, string.Empty, groupedByTeams ? FormatReport.RptStatisticsByFTBByLocationsGroupedByTeams() : FormatReport.RptStatisticsByFTBByLocations(), blnShowSubtotal: true, blnColumnGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion RptStatisticsByFTBByLocations

    #region RptStatisticsByFTBByCategories

    /// <summary>
    /// Obtiene los datos para exportar a excel el reporte RptStatisticsByFTB ByLocations
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta completa del archivo</param>
    /// <param name="filters">Listado de filtros</param>
    /// <param name="lstReport">Contenido del reporte</param>
    /// <param name="groupedByTeams">Agrupado por equipos</param>
    /// <history>
    ///  [michan] 22/07/2016 Created
    /// </history>
    internal static FileInfo RptStatisticsByFTBByCategories(string report, string fileFullPath, List<Tuple<string, string>> filters, List<RptStatisticsByFTBCategories> lstReport, bool groupedByTeams)
    {
      var dtData = TableHelper.GetDataTableFromList(lstReport, true, true);
      return EpplusHelper.CreateExcelCustom(dtData, filters, report, string.Empty, groupedByTeams ? FormatReport.RptStatisticsByFTBByCategoriesGroupedByTeams() : FormatReport.RptStatisticsByFTBByCategories(), blnShowSubtotal: true, blnColumnGrandTotal: true, fileFullPath: fileFullPath);
    }
    #endregion RptStatisticsByFTB

    #region RptEfficiencyWeekly
    /// <summary>
    /// Devuele el reporte Efficiency Weekly
    /// </summary>
    /// <param name="report">Nombre del reporte</param>
    /// <param name="fileFullPath">Ruta a guardar</param>
    /// <param name="filters">Filtros Aplicados</param>
    /// <param name="lstReport">Listado con la informacion del reporte</param>
    /// <history>
    /// [ecanul] 16/08/2016 Created
    /// </history>
    public static FileInfo RptEfficiencyWeekly(string report, string fileFullPath, List<Tuple<string, string>> filters, List<RptEfficiencyWeekly> lstReport)
    {
      DataTable dtData = TableHelper.GetDataTableFromList(lstReport);
      var unAssistance = lstReport.Where(x => x.EfficiencyType == "Undefined Assistance").ToList();

      StringBuilder str = new StringBuilder();
      if (unAssistance.Any())
      {
        str.AppendLine("The following salesmen are not have defined their assistance:");
        unAssistance.ForEach(x =>
        {
          str.AppendLine($"{x.SalemanID} - {x.SalemanName}");
        });
      }
      UIHelper.ShowMessage(str.ToString(), System.Windows.MessageBoxImage.Exclamation, "Front To Backs without Assistance");
      return EpplusHelper.CreateExcelCustom(dtData, filters, report, string.Empty, FormatReport.RptEfficiencyWeekly(), fileFullPath: fileFullPath);
    } 
    #endregion

    #endregion
  }
}
