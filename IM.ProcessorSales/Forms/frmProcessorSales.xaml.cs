﻿using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.ProcessorSales.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.ProcessorSales.Forms
{
  /// <summary>
  /// Interaction logic for frmProcessorSales.xaml
  /// </summary>
  public partial class frmProcessorSales
  {
    #region Atributos

    #region Constantes
    private const string FilterDate = "FilterDate";
    #endregion

    #region Privados
    private frmReportQueue _frmReportQueue;
    //Formulario de filtros
    private frmFilterDateRange _frmFilter;
    //Archivo de configuracion
    private IniFileHelper _iniFieldHelper;
    //Detalles de los filtros
    private bool _multiDate, _onlyOnRegister, _allSalesRoom;
    //Listado de reportes
    private EnumRptSalesRoom _rptRoomSales;
    private EnumRptSalesRoomAndSalesman _rptSalesman;

    #endregion

    #region Publicos
    //Filtros para los reportes
    public EnumBasedOnArrival basedOnArrival;
    public EnumQuinellas quinellas;
    public string _salesRoom;

    public ClsFilter _clsFilter;

    #endregion Publicos

    #endregion Atributos

    #region Metodos

    #region LoadGrids

    /// <summary>
    /// Carga los Grids del formulario
    /// </summary>
    /// <history>
    /// [ecanul] 22/04/2016 Created
    /// </history>
    private void LoadGrids()
    {
      //Reportes por Sales Room
      dtgSalesRoom.ItemsSource = EnumToListHelper.GetList<EnumRptSalesRoom>().OrderBy(x => x.Value);

      //Reportes por Sales Room y Salesman
      dtgSalesman.ItemsSource = EnumToListHelper.GetList<EnumRptSalesRoomAndSalesman>().OrderBy(x => x.Value);

      statusBarReg.Content = $"{dtgSalesRoom.Items.Count + dtgSalesman.Items.Count} Reports";
    }

    #endregion

    #region LoadIniField
    /// <summary>
    /// Carga un archivo Configuration.ini
    /// </summary>
    /// <history>
    /// [ecanul] 22/04/2016 Created
    /// </history>
    private bool LoadIniField()
    {
      string archivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(archivo))
        return false;
      _iniFieldHelper = new IniFileHelper(archivo);
      return true;
    }
    #endregion

    #region GetFirstDayValue
    /// <summary>
    /// Obtiene las fechas iniciales y finales de los reportes
    /// </summary>
    /// <history>
    /// [ecanul] 22/04/2016 Created
    /// </history>
    private void GetFirstDayValue()
    {
      //carga las fechas desde el archivo de configuracion
      _clsFilter.DtmStart = _iniFieldHelper.readDate(FilterDate, "DateStart", _clsFilter.DtmStart);
      _clsFilter.DtmEnd = _iniFieldHelper.readDate(FilterDate, "DateEnd", _clsFilter.DtmEnd);
      _salesRoom = _iniFieldHelper.ReadText(FilterDate, "SalesRoom", string.Empty);
      if (!string.IsNullOrEmpty(_salesRoom))
        _clsFilter.LstSalesRooms.Add(_salesRoom);
    }
    #endregion

    #region SetUpIniField

    /// <summary>
    /// Carga los valores de un archivo de configuracion
    /// </summary>
    /// <history>
    /// [ecanul] 04/05/2016 Created
    /// </history>
    private void SetUpIniField()
    {
      //si el archivo de configuracion existe configura los parametros
      if (!LoadIniField()) return;
      GetFirstDayValue();
      _clsFilter.Goal = Convert.ToDecimal(_iniFieldHelper.readDouble(FilterDate, "Goal", 0));
      _clsFilter.BlnGroupedByTeams = _iniFieldHelper.ReadBoolean(FilterDate, "GroupedByTeams", false);
      _clsFilter.BlnIncludeAllSalesmen = _iniFieldHelper.ReadBoolean(FilterDate, "IncludeAllSalesmen", false);
      _clsFilter.Salesman = new PersonnelShort
      {
        peID = _iniFieldHelper.ReadText(FilterDate, "Salesman", string.Empty)
      };
      //Se limpia el archivo de confiuguracion
      _iniFieldHelper = null;
    }

    #endregion

    #region SetupParameters

    /// <summary>
    /// Configura los parametros de los reportes
    /// </summary>
    /// <history>
    /// [ecanul] 22/04/2016 Created
    /// [ecanul] 04/05/2016 Modificated, Corregido error con archivo de configuracion 
    /// </history>
    private void SetupParameters()
    {
      _clsFilter = new ClsFilter();

      DateTime serverDate = BRHelpers.GetServerDate();
      // Fecha Inicial
      _clsFilter.DtmStart = new DateTime(serverDate.Year, serverDate.Month, 1);
      //Fecha final
      _clsFilter.DtmEnd = serverDate.Date;

      _allSalesRoom = false;

      // Obtenemos los valores de un archivo de configuracion
      SetUpIniField();
      //roles de vendedores
      _clsFilter.LstEnumRole.AddRange(new[] { EnumRole.PR, EnumRole.Liner, EnumRole.Closer, EnumRole.ExitCloser });
    }

    #endregion

    #region ShowReportBySalesRoom
    /// <summary>
    /// Muestra el reporte de Sales Room Seleccionado
    /// </summary>
    /// <param name="rptRoomSales"></param>
    /// <param name="clsFilter"></param>
    /// <history>
    /// [ecanul] 05/05/2016 Created
    /// </history>
    private async void ShowReportBySalesRoom(EnumRptSalesRoom rptRoomSales, ClsFilter clsFilter)
    {
      FileInfo file = null;
      //Deberia validarse con 
      #region Datos del reporte
      string dateRange = _multiDate
           ? string.Join("; ", clsFilter.LstMultiDate.Select(x => $"{x.SalesRoom}  {DateHelper.DateRange(x.DtStart, x.DtEnd)}"))
           : DateHelper.DateRange(clsFilter.DtmStart, clsFilter.DtmEnd);
      string dateRangeFileName = _multiDate
          ? string.Join("; ", clsFilter.LstMultiDate.Select(x => $"{x.SalesRoom}  {DateHelper.DateRange(x.DtStart, x.DtEnd)}"))
         : DateHelper.DateRangeFileName(clsFilter.DtmStart, clsFilter.DtmEnd);
      string reporteName = EnumToListHelper.GetEnumDescription(rptRoomSales);
      #endregion

      #region Filtro(s)
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>
      {
        new Tuple<string, string>("Date Range", dateRange)
      };
      //Si es cualquier reporte menos Concentrate Daily Sales o Multidate (porque sus grids son diferentes) se agrega de manera comun
      if (rptRoomSales != EnumRptSalesRoom.ConcerntrateDailySales && rptRoomSales != EnumRptSalesRoom.StatsBySegmentsCategoriesMultiDatesRanges)
      {
        //Si es de solo un registro El sales Room es unico, si no Se toma por Todos o por los seleccionados
        if (_onlyOnRegister)
          filters.Add(new Tuple<string, string>("Sales Room", clsFilter.LstSalesRooms.First()));
        else
          filters.Add(new Tuple<string, string>("Sales Room",
            _frmFilter.dtgSalesRoom.Items.Count == clsFilter.LstSalesRooms.Count ? "All" : string.Join(",", clsFilter.LstSalesRooms)));
      }
      if (rptRoomSales == EnumRptSalesRoom.StatsByCloser || rptRoomSales == EnumRptSalesRoom.StatsByExitCloser || rptRoomSales == EnumRptSalesRoom.StatsByFtb)
      {
        filters.Add(Tuple.Create("Programs", EnumToListHelper.GetEnumDescription(clsFilter.EnumProgram)));
        filters.Add(Tuple.Create("Segments", clsFilter.BlnAllSegments ? "ALL" : string.Join(",", clsFilter.LstSegments)));
      }
      
      #endregion

      List<dynamic> list = new List<dynamic>();
      string fileFullPath = ReportBuilder.CreateEmptyExcel(reporteName, dateRangeFileName);
      _frmReportQueue.AddReport(fileFullPath, reporteName);
      try
      {
        switch (rptRoomSales)
        {
          #region Manifest
          case EnumRptSalesRoom.Manifest:
            
            list.AddRange(await BRReportsBySalesRoom.GetRptManiest(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms));
            if (list.Count > 0)
              file = await Reports.RptManifest(reporteName, fileFullPath, filters, list.Cast<RptManifest>().ToList(), clsFilter.DtmStart, clsFilter.DtmEnd);
            break;
          #endregion

          #region StatsByLocation
          case EnumRptSalesRoom.StatsByLocation:
            list.AddRange(await BRReportsBySalesRoom.GetRptStatisticsByLocation(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms));
            if (list.Count > 0)
              file = await Reports.RptStatisticsByLocation(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByLocation>().ToList());
            break;
          #endregion

          #region StatsByLocationMonthly
          case EnumRptSalesRoom.StatsByLocationMonthly:
            list.AddRange(await BRReportsBySalesRoom.GetRptStaticsByLocationMonthly(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms));
            if (list.Count > 0)
              file = await Reports.RptStaticsByLocationMonthly(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByLocationMonthly>().ToList());
            break;
          #endregion

          #region SalesByLocationMonthly
          case EnumRptSalesRoom.SalesByLocationMonthly:
            list.AddRange(await BRReportsBySalesRoom.GetRptSalesByLocationMonthly(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms));
            if (list.Count > 0)
              file = await Reports.RptSalesByLocationMonthly(reporteName, fileFullPath, filters, list.Cast<RptSalesByLocationMonthly>().ToList());
            break;
          #endregion

          #region StatsByLocationAndSalesRoom
          case EnumRptSalesRoom.StatsByLocationAndSalesRoom:
            list.AddRange(await BRReportsBySalesRoom.GetRptStatisticsBySalesRoomLocation(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms));
            if (list.Count > 0)
              file = await Reports.RptStatisticsBySalesRoomLocation(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySalesRoomLocation>().ToList());
            break;
          #endregion

          #region ConcerntrateDailySales
          case EnumRptSalesRoom.ConcerntrateDailySales:
            #region FiltroSalesRoomConcentrate
            clsFilter.LstSalesRooms.AddRange(clsFilter.LstGoals.Select(c => c.SalesRoomByUser.srID));
            filters.Add(new Tuple<string, string>("Sales Room", string.Join("/", clsFilter.LstGoals.Select(c => c.SalesRoomByUser.srID).ToList())));
            #endregion

            list.AddRange(await BRReportsBySalesRoom.GetRptConcentrateDailySales(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstGoals.Select(c => c.SalesRoomByUser.srID).ToList()));

            if (list.Count > 0)
              file = await Reports.RptConcentrateDailySales(reporteName, fileFullPath, clsFilter.DtmEnd, filters,
                list.Cast<RptConcentrateDailySales>().ToList(), clsFilter.LstGoals);
            break;
          #endregion

          #region DailySales
          case EnumRptSalesRoom.DailySales:
            list.AddRange(await BRReportsBySalesRoom.GetRptDailySalesDetail(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms));
            List<RptDailySalesHeader> lstHeader = await BRReportsBySalesRoom.GetRptDailySalesHeader(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms);
            if (list.Count > 0 && lstHeader.Count > 0)
              file = await Reports.RptDailySales(reporteName, dateRange, fileFullPath, filters, list.Cast<RptDailySalesDetail>().ToList(),
                lstHeader, clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.Goal);
            break;
          #endregion

          #region FTM In & Out House
          case EnumRptSalesRoom.FtmInAndOutHouse:
            list.AddRange(await BRReportsBySalesRoom.GetRptFTMInOutHouse(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms));
            if (list.Count > 0)
            {
              file = await Reports.RptFTMInOutHouse(reporteName, fileFullPath, filters, list.Cast<RptFTMInOutHouse>().ToList(), clsFilter.DtmStart, clsFilter.DtmEnd);
            }
            break;
          #endregion

          #region Stats by Segments

          case EnumRptSalesRoom.StatsBySegments:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRooms, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion Stats by Segments

          #region Stats by Segments (OWN)
          case EnumRptSalesRoom.StatsBySegmentsOwn:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRooms, own: true, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));

            if (list.Any())
              file = await Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion Stats by Segments (OWN)          

          #region Stats by Segments Categories
          case EnumRptSalesRoom.StatsBySegmentsCategories:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRooms, bySegmentsCategories: true,
              includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion Stats by Segments Categories

          #region Stats by Segments Categories (OWN)
          case EnumRptSalesRoom.StatsBySegmentsCategoriesOwn:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRooms, bySegmentsCategories: true,
              own: true, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion Stats by Segments Categories (OWN)

          #region Stats by Segments Categories (Multi Date Ranges)
          case EnumRptSalesRoom.StatsBySegmentsCategoriesMultiDatesRanges:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(clsFilter.LstMultiDate.Select(x => x.DtStart), clsFilter.LstMultiDate.Select(x => x.DtEnd),
              clsFilter.LstMultiDate.Select(x => x.SalesRoom), bySegmentsCategories: true, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion Stats by Segments Categories (Multi Date Ranges)

          #region Stats by Closer
          case EnumRptSalesRoom.StatsByCloser:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByCloser(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(), 
              program: clsFilter.EnumProgram, segments: clsFilter.BlnAllSegments ? null : clsFilter.LstSegments, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen, groupByTeams:clsFilter.BlnGroupedByTeams));
            if (list.Any())
              file = await Reports.RptStatisticsByCloser(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByCloser>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion

          #region Stats by Exit Closer
          case EnumRptSalesRoom.StatsByExitCloser:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByExitCloser(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(),
              program: clsFilter.EnumProgram, segments: clsFilter.BlnAllSegments ? null : clsFilter.LstSegments, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsByExitCloser(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByExitCloser>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion

          #region Self Gen & Self Gen Team
          case EnumRptSalesRoom.SelfGenAndSelfGenTeam:
            list.AddRange(await BRReportsBySalesRoom.GetRptSelfGenAndSelfGenTeam(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms));
            if (list.Count > 0)
              file = await Reports.RptSelfGenAndSelfGenTeam(reporteName, fileFullPath, filters, list.Cast<RptSelfGenTeam>().ToList(), clsFilter.DtmStart, clsFilter.DtmEnd);
            break;
          #endregion

          #region Stats by F.T.B
          case EnumRptSalesRoom.StatsByFtb:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByFTB(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(),
              program: clsFilter.EnumProgram, segments: clsFilter.BlnAllSegments ? null : clsFilter.LstSegments, groupByTeam: clsFilter.BlnGroupedByTeams, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsByFTB(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByFTB>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion

          #region Stats by F.T.B. & Locations
          case EnumRptSalesRoom.StatsByFtbAndLocatios:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByFTBLocations(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(), groupByTeam: clsFilter.BlnGroupedByTeams,
              includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsByFTBByLocations(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByFTBLocations>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion

          #region Stats by F.T.B. & Locations Categories
          case EnumRptSalesRoom.StatsByFtbAndLocatiosCategories:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByFTBCategories(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(), groupByTeam: clsFilter.BlnGroupedByTeams,
               includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsByFTBByCategories(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByFTBCategories>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion

          #region Efficiency Weekly
          case EnumRptSalesRoom.EfficiencyWeekly:
            list.AddRange(await BRReportsBySalesRoom.GetRptEfficiencyWeekly(clsFilter.lstEfficiency.Select(x => x.efDateFrom), clsFilter.lstEfficiency.Select(x => x.efDateTo),
              clsFilter.LstSalesRooms.First().ToString()));
            if (list.Any())
              file = await Reports.RptEfficiencyWeekly(reporteName, fileFullPath, filters, list.Cast<RptEfficiencyWeekly>().ToList(), clsFilter);
            break; 
            #endregion
        }
        if (file == null)
        {
          file = ReportBuilder.CreateNoInfoRptExcel(filters, reporteName, fileFullPath);          
        }
        frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(file, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly));
        frmDocumentViewver.Show();
        _frmReportQueue.SetExist(file.FullName, file);
        _frmReportQueue.Activate();
      }
      catch (Exception ex)
      {        
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion ShowReportBySalesRoom
    
    #region ShowReportBySalesman
    /// <summary>
    /// Muestra el reporte de Salesman Seleccionado
    /// </summary>
    /// <param name="rptSalesman"></param>
    /// <param name="clsFilter"></param>
    /// <history>
    /// [aalcocer] 08/07/2016 Created
    /// </history>
    private async void ShowReportBySalesman(EnumRptSalesRoomAndSalesman rptSalesman, ClsFilter clsFilter)
    {
      FileInfo file = null;
      //Deberia validarse con 
      #region Datos del reporte
      string dateRange = _multiDate
           ? string.Join("; ", clsFilter.LstMultiDate.Select(x => $"{x.SalesRoom}  {DateHelper.DateRange(x.DtStart, x.DtEnd)}"))
           : DateHelper.DateRange(clsFilter.DtmStart, clsFilter.DtmEnd);
      string dateRangeFileName = _multiDate
          ? string.Join("; ", clsFilter.LstMultiDate.Select(x => $"{x.SalesRoom}  {DateHelper.DateRange(x.DtStart, x.DtEnd)}"))
         : DateHelper.DateRangeFileName(clsFilter.DtmStart, clsFilter.DtmEnd);
      string reporteName = EnumToListHelper.GetEnumDescription(rptSalesman);
      #endregion

      #region Filtro(s)
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>
      {
        new Tuple<string, string>("Date Range", dateRange)
      };

      //Si es de solo un registro El sales Room es unico, si no Se toma por Todos o por los seleccionados
      if (_onlyOnRegister)
        filters.Add(new Tuple<string, string>("Sales Room", clsFilter.LstSalesRooms.First()));
      else
        filters.Add(new Tuple<string, string>("Sales Room",
          _frmFilter.dtgSalesRoom.Items.Count == clsFilter.LstSalesRooms.Count ? "All" : string.Join(",", clsFilter.LstSalesRooms)));
      filters.Add(new Tuple<string, string>("Salesman", $"{clsFilter.Salesman.peID}  {clsFilter.Salesman.peN}"));

      #endregion

      List<dynamic> list = new List<dynamic>();
      string fileFullPath = ReportBuilder.CreateEmptyExcel(reporteName, dateRangeFileName);
      _frmReportQueue.AddReport(fileFullPath, reporteName);
      try
      {
        switch (rptSalesman)
        {
          #region FtmIn&OutHouse
          case EnumRptSalesRoomAndSalesman.FtmInAndOutHouse:
            list.AddRange(await BRReportsBySalesRoom.GetRptFTMInOutHouse(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms, clsFilter.Salesman.peID));
            if (list.Count > 0)
              file = await Reports.RptFTMInOutHouse(reporteName, fileFullPath, filters, list.Cast<RptFTMInOutHouse>().ToList(), clsFilter.DtmStart, clsFilter.DtmEnd);
            break; 
          #endregion
          #region Manifest
          case EnumRptSalesRoomAndSalesman.Manifest:
            // Roles
            filters.Add(new Tuple<string, string>("Roles", clsFilter.LstEnumRole.EnumListToString()));

            list.AddRange(await BRReportsBySalesRoom.GetRptManiest(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms,
              clsFilter.Salesman.peID, clsFilter.LstEnumRole));
            if (list.Any())
              file = await Reports.RptManifest(reporteName, fileFullPath, filters, list.Cast<RptManifest>().ToList(), clsFilter.DtmStart, clsFilter.DtmEnd);
            break;
          #endregion
          #region Self Gen & Self Gen Team
          case EnumRptSalesRoomAndSalesman.SelfGenAndSelfGenTeam:
            list.AddRange(await BRReportsBySalesRoom.GetRptSelfGenAndSelfGenTeam(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms,clsFilter.Salesman.peID));
            if (list.Count > 0)
              file = await Reports.RptSelfGenAndSelfGenTeam(reporteName, fileFullPath, filters, list.Cast<RptSelfGenTeam>().ToList(), clsFilter.DtmStart, clsFilter.DtmEnd);
            break;
          #endregion
          #region Stats by Closer
          case EnumRptSalesRoomAndSalesman.StatsByCloser:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByCloser(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(), clsFilter.Salesman.peID, groupByTeams: clsFilter.BlnGroupedByTeams));
            if (list.Any())
              file = await Reports.RptStatisticsByCloser(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByCloser>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion
          #region Stats by Exit Closer
          case EnumRptSalesRoomAndSalesman.StatsByExitCloser:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByExitCloser(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(), clsFilter.Salesman.peID));
            if (list.Any())
              file = await Reports.RptStatisticsByExitCloser(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByExitCloser>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion
          #region Stats by F.T.B
          case EnumRptSalesRoomAndSalesman.StatsByFtb:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByFTB(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(), clsFilter.Salesman.peID,
              program: clsFilter.EnumProgram, segments: clsFilter.BlnAllSegments ? null : clsFilter.LstSegments, groupByTeam: clsFilter.BlnGroupedByTeams, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsByFTB(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByFTB>().ToList(), clsFilter.BlnIncludeAllSalesmen);
            break;
          #endregion

          #region Stats by F.T.B. & Locations
          case EnumRptSalesRoomAndSalesman.StatsByFtbAndLocations:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByFTBLocations(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(), clsFilter.Salesman.peID, clsFilter.BlnGroupedByTeams, clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsByFTBByLocations(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByFTBLocations>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion

          #region Stats by F.T.B. & Locations Categories
          case EnumRptSalesRoomAndSalesman.StatsByFtbAndLocationsCategories:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsByFTBCategories(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRooms.First(), clsFilter.Salesman.peID, clsFilter.BlnGroupedByTeams, clsFilter.BlnIncludeAllSalesmen));
            if (list.Any())
              file = await Reports.RptStatisticsByFTBByCategories(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByFTBCategories>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion
          #region Statistics by Segments
          case EnumRptSalesRoomAndSalesman.StatsBySegments:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRooms, clsFilter.Salesman.peID));
            if (list.Any())
              file = await Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion Statistics by Segments Own
          #region Statistics by Segments
          case EnumRptSalesRoomAndSalesman.StatsBySegmentsOwn:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRooms, clsFilter.Salesman.peID, own: true));
            if (list.Any())
              file = await Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          #endregion Statistics by Segments Own
          case EnumRptSalesRoomAndSalesman.StatsBySegmentsCategories:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRooms, clsFilter.Salesman.peID, true));
            if (list.Any())
              file = await Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
          case EnumRptSalesRoomAndSalesman.StatsBySegmentsCategoriesOwn:
            list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRooms, clsFilter.Salesman.peID, true, true));
            if (list.Any())
              file = await Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
            break;
        }
        if (file == null)
        {
          file = ReportBuilder.CreateNoInfoRptExcel(filters, reporteName, fileFullPath);          
        }
        frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(file, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly));
        frmDocumentViewver.Show();
        _frmReportQueue.SetExist(file.FullName, file);
        _frmReportQueue.Activate();
      }
      catch (Exception ex)
      {        
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion ShowReportBySalesman

    #region ShowDateRangeSr

    /// <summary>
    /// Muestra el filtro por Sales Room
    /// </summary>
    /// <history>
    /// [ecanul] 03/05/2016 Created
    /// </history>
    private void ShowDateRangeSr()
    {
      _frmFilter = new frmFilterDateRange { frmPrs = this, Owner = this };
      StaStart("Loading Date Range Window...");
      switch (_rptRoomSales)
      {
        case EnumRptSalesRoom.StatsBySegments:
        case EnumRptSalesRoom.StatsBySegmentsCategories:
        case EnumRptSalesRoom.StatsBySegmentsOwn:
        case EnumRptSalesRoom.StatsBySegmentsCategoriesOwn:
        case EnumRptSalesRoom.StatsByFtbAndLocatios:
        case EnumRptSalesRoom.StatsByFtbAndLocatiosCategories:
          _frmFilter.ConfigurarFomulario(_salesRoom, shGroupsByTeams: true, groupByTeams: _clsFilter.BlnGroupedByTeams, shAllSalesmen: true, allSalesmen: _clsFilter.BlnIncludeAllSalesmen, multiDate: _multiDate, blnOnlyOneRegister: _onlyOnRegister, blnAllSalesRoom: _allSalesRoom);
          break;
        case EnumRptSalesRoom.StatsBySegmentsCategoriesMultiDatesRanges:
          //Se usa para indicar que no se mostrara el filtro de datos y que las fechas se usaran las que tenga el grid
          _multiDate = true;
          _frmFilter.ConfigurarFomulario(_salesRoom, multiDate: _multiDate, groupByTeams: _clsFilter.BlnGroupedByTeams, shGroupsByTeams: true, allSalesmen: _clsFilter.BlnIncludeAllSalesmen, shAllSalesmen: true, blnSalesRoom: false, shMultiDateRanges: true);
          break;
        case EnumRptSalesRoom.StatsByFtb:
        case EnumRptSalesRoom.StatsByCloser:
        case EnumRptSalesRoom.StatsByExitCloser:
          _frmFilter.ConfigurarFomulario(_salesRoom, shGroupsByTeams: true, groupByTeams: _clsFilter.BlnGroupedByTeams, shAllSalesmen: true, allSalesmen: _clsFilter.BlnIncludeAllSalesmen, multiDate: _multiDate, blnOnlyOneRegister: _onlyOnRegister, blnSegments: true, blnAllSegments: true, blnPrograms: true);
          break;
        case EnumRptSalesRoom.DailySales:
          _onlyOnRegister = false;
          _frmFilter.ConfigurarFomulario(_salesRoom, multiDate: _multiDate, blnOnlyOneRegister: _onlyOnRegister, blnAllSalesRoom: _allSalesRoom, isGoal: true);
          break;
        case EnumRptSalesRoom.ConcerntrateDailySales:
          _frmFilter.ConfigurarFomulario(_salesRoom, blnSalesRoom: false, shConcentrate: true);
          break;
        case EnumRptSalesRoom.EfficiencyWeekly:
          _frmFilter.ConfigurarFomulario(_salesRoom, blnOnlyOneRegister: _onlyOnRegister, blnAllSalesRoom: _allSalesRoom, period: EnumPeriod.Weekly, shWeeks: true, onePeriod: true);
          break;
        case EnumRptSalesRoom.StatsByLocation:
        case EnumRptSalesRoom.StatsByLocationAndSalesRoom:
          _onlyOnRegister = false;
          _frmFilter.ConfigurarFomulario(_salesRoom, multiDate: _multiDate, blnOnlyOneRegister: _onlyOnRegister, blnAllSalesRoom: _allSalesRoom);
          break;
        case EnumRptSalesRoom.StatsByLocationMonthly:
          _frmFilter.ConfigurarFomulario(_salesRoom, multiDate: _multiDate, blnOnlyOneRegister: _onlyOnRegister, blnAllSalesRoom: _allSalesRoom, period: EnumPeriod.Monthly, onePeriod: true); //queda pendiente blnoneperiod Obliga a que siempre sea de mes en mes
          break;
        case EnumRptSalesRoom.SalesByLocationMonthly:
          _frmFilter.ConfigurarFomulario(_salesRoom, multiDate: _multiDate, blnOnlyOneRegister: _onlyOnRegister, blnAllSalesRoom: _allSalesRoom, period: EnumPeriod.Monthly);
          break;
        default:
          _frmFilter.ConfigurarFomulario(_salesRoom, multiDate: _multiDate, blnOnlyOneRegister: _onlyOnRegister, blnAllSalesRoom: _allSalesRoom);
          break;
      }
      _frmFilter.ShowDialog();
      StaEnd();
      if (!_frmFilter.ok) return;
      ShowReportBySalesRoom(_rptRoomSales, _clsFilter);
      _frmFilter.Close();
    }

    #endregion

    #region ShowDateRangeSm

    /// <summary>
    /// Muestra el filtro por Sales Room & Salesman
    /// </summary>
    /// <history>
    /// [ecanul] 03/05/2016 Created
    /// </history>
    private void ShowDateRangeSm()
    {
      _frmFilter = new frmFilterDateRange { frmPrs = this, Owner = this };
      StaStart("Loading Date Range Window...");
      switch (_rptSalesman)
      {
        case EnumRptSalesRoomAndSalesman.Manifest:
          _frmFilter.ConfigurarFomulario(_salesRoom, multiDate: _multiDate, blnOnlyOneRegister: _onlyOnRegister, blnAllSalesRoom: _allSalesRoom, shGroupsByTeams: true, groupByTeams: _clsFilter.BlnGroupedByTeams, isBySalesman: true, shRoles: true);
          break;
        default:
          _frmFilter.ConfigurarFomulario(_salesRoom, multiDate: _multiDate, blnOnlyOneRegister: _onlyOnRegister, blnAllSalesRoom: _allSalesRoom, shGroupsByTeams: true, groupByTeams: _clsFilter.BlnGroupedByTeams, isBySalesman: true);
          break;
      }
      _frmFilter.ShowDialog();
      StaEnd();
      if (!_frmFilter.ok) return;
      ShowReportBySalesman(_rptSalesman, _clsFilter);
      _frmFilter.Close();
    }

    #endregion

    #region PrepareReportBySalesRoom

    /// <summary>
    /// Prepara un reporte por Sales Room
    /// </summary>
    /// <history>
    /// [ecanul] 23/04/2016 Created
    /// </history>
    private void PrepareReportBySalesRoom()
    {
      if (dtgSalesRoom.SelectedIndex < 0) return;
      StaStart("Loading Date Range Window...");
      //obtener el nombre del reporte
      _rptRoomSales = ((KeyValuePair<EnumRptSalesRoom, string>)dtgSalesRoom.SelectedItem).Key;
      //Reportes que solo necesitan una fecha 
      _multiDate = false;
      //Reportes que solo deben permitir seleccionar un registro 
      _onlyOnRegister = true;
      //desplegamos el filtro de fechas
      ShowDateRangeSr();
    }

    #endregion

    #region PrepareReportBySalesman

    /// <summary>
    /// Prepara un reporte por Sales Room & Salesman
    /// </summary>
    /// <history>
    /// [ecanul] 03/05/2016 Created
    /// </history>
    private void PrepareReportBySalesman()
    {
      if (dtgSalesman.SelectedIndex < 0) return;
      StaStart("Loading Date Range Window...");
      //Obtiene el nombre del reporte
      _rptSalesman = ((KeyValuePair<EnumRptSalesRoomAndSalesman, string>)dtgSalesman.SelectedItem).Key;
      //reportes que no solo necesitan una fecha
      _multiDate = false;
      //Reportes que solo deben permitir seleccionar un registro 
      _onlyOnRegister = true;
      //desplegamos el filtro de fechas
      ShowDateRangeSm();
    }

    #endregion

    #region LoadFilter 

    /// <summary>
    /// Método para abrir la ventana de filtros
    /// </summary>
    /// <param name="obj">objeto sender</param>
    /// <param name="type">0 grid | 1 boton</param>
    private void LoadFilter(object obj, int type)
    {
      if (type == 0) //de Grid
      {
        var dataGridRow = (DataGridRow)obj;
        if (dataGridRow.Item.Equals(dtgSalesRoom.CurrentItem)) PrepareReportBySalesRoom();
        else if (dataGridRow.Item.Equals(dtgSalesman.CurrentItem)) PrepareReportBySalesman();
      }
      else //de boton 
      {
        if (obj.Equals(btnPrintSr)) PrepareReportBySalesRoom();
        else if (obj.Equals(btnPrintSm)) PrepareReportBySalesman();
      }

    }

    #endregion

    #region StaStart

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje a mostrar</param>
    /// <history>
    /// [ecanul] 22/04/2016 Created 
    /// </history>
    private void StaStart(string message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      Cursor = Cursors.Wait;
    }

    #endregion

    #region StaEnd

    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    /// <history>
    /// [ecanul] 22/04/2016 Created 
    /// </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      Cursor = null;
    }

    #endregion

    #endregion

    #region Constructores y Destructores

    public frmProcessorSales()
    {
      InitializeComponent();
    }

    private void FrmProcessorSales_Closing(object sender, CancelEventArgs e)
    {
      _frmFilter?.Close();
      App.Current.Shutdown();
    }

    #endregion

    #region Eventos del formulario

    #region Window_KeyDown

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          KeyboardHelper.CkeckKeysPress(statusBarCap, Key.Capital);
          break;
        case Key.Insert:
          KeyboardHelper.CkeckKeysPress(statusBarIns, Key.Insert);
          break;
        case Key.NumLock:
          KeyboardHelper.CkeckKeysPress(statusBarNum, Key.NumLock);
          break;
      }
    }

    #endregion

    #region Window_Loaded

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      StaStart("Load Form...");
      LoadGrids();
      SetupParameters();
      lblUserName.Content = Context.User.User.peN;
      _frmReportQueue = new frmReportQueue(Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly));
      KeyboardHelper.CkeckKeysPress(statusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(statusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(statusBarNum, Key.NumLock);
      StaEnd();
    }

    #endregion

    #region btnExit_click

    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      App.Current.Shutdown();
    }

    #endregion

    #region BtnPrintSr_Click

    private void BtnPrintSr_Click(object sender, RoutedEventArgs e)
    {
      LoadFilter(sender, 1);
    }

    #endregion

    #region BtnPrintSm_Click

    private void BtnPrintSm_Click(object sender, RoutedEventArgs e)
    {
      LoadFilter(sender, 1);
    }

    #endregion

    #region grdrpt_MouseDoubleClick

    private void grdrpt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      LoadFilter(sender, 0);
    }

    #endregion

    #region grdrpt_PreviewKeyDown

    private void grdrpt_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter) return;
      LoadFilter(sender, 0);
    }

    #endregion

    #region btnReportQueue_Click

    /// <summary>
    ///   Método para abrir la ventana de cola de Reportes
    /// </summary>
    /// <history>
    ///   [aalcocer] 03/06/2016 Created
    /// </history>
    private void btnReportQueue_Click(object sender, RoutedEventArgs e)
    {
      _frmReportQueue.Show();
      if (_frmReportQueue.WindowState == WindowState.Minimized) _frmReportQueue.WindowState = WindowState.Normal;
      _frmReportQueue.Activate();
    }

    #endregion btnReportQueue_Click

    #endregion
  }
}
