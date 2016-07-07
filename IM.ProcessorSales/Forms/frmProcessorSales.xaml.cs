using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using IM.Base.Helpers;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Windows.Controls;
using IM.Base.Forms;
using IM.Model;
using IM.ProcessorSales.Classes;

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
    private SystemCfg _systemConfig;
    //Formulario de filtros
    private frmFilterDateRange _frmFilter;
    //Archivo de configuracion
    private IniFileHelper _iniFieldHelper;
    //Detalles de los filtros
    private bool _multiDate, _onlyOnRegister, _allPrograms, _allSegments, _allSalesRoom;
    private string _salesman;
    //Listado de reportes
    private EnumRptRoomSales _rptRoomSales;
    private EnumRptSalesRoomAndSalesman _rptSalesman;

    #endregion

    #region Publicos

    //Listas para los reportes
    public List<PersonnelShort> lstPersonnel = new List<PersonnelShort>();

    //Filtros para los reportes
    public EnumBasedOnArrival basedOnArrival;
    public EnumQuinellas quinellas;
    public string salesRoom;

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
      dtgSalesRoom.ItemsSource = EnumToListHelper.GetList<EnumRptRoomSales>().OrderBy(x => x.Value);

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
      salesRoom = _iniFieldHelper.ReadText(FilterDate, "SalesRoom", string.Empty);
      if (!string.IsNullOrEmpty(salesRoom))
        _clsFilter.LstSalesRoom.Add(salesRoom);
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
      _salesman = _iniFieldHelper.ReadText(FilterDate, "Salesman", string.Empty);
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
      //segmentos y programas
      _allSegments = _allPrograms = true;
    }

    #endregion

    /// <summary>
    /// Muestra el reporte de Sales Room Seleccionado
    /// </summary>
    /// <param name="rptRoomSales"></param>
    /// <param name="clsFilter"></param>
    /// <history>
    /// [ecanul] 05/05/2016 Created
    /// </history>
    private async void ShowReportBySalesRoom(EnumRptRoomSales rptRoomSales, ClsFilter clsFilter)
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
      if (rptRoomSales != EnumRptRoomSales.ConcerntrateDailySales && rptRoomSales != EnumRptRoomSales.StatsBySegmentsCategoriesMultiDatesRanges)
      {
        //Si es de solo un registro El sales Room es unico, si no Se toma por Todos o por los seleccionados
        if (_onlyOnRegister)
          filters.Add(new Tuple<string, string>("Sales Room", clsFilter.LstSalesRoom.First()));
        else
          filters.Add(new Tuple<string, string>("Sales Room",
            _frmFilter.dtgSalesRoom.Items.Count == clsFilter.LstSalesRoom.Count ? "All" : string.Join(",", clsFilter.LstSalesRoom)));
      }
      #endregion

      List<dynamic> list = new List<dynamic>();
      string fileFullPath = EpplusHelper.CreateEmptyExcel(reporteName, dateRangeFileName);
      _frmReportQueue.AddReport(fileFullPath, reporteName);
      try
      {
        switch (rptRoomSales)
      {
        #region Manifest
        case EnumRptRoomSales.Manifest:
          list.AddRange(await BRReportsBySalesRoom.GetRptManiest(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptManifest(reporteName, fileFullPath, filters, list.Cast<RptManifest>().ToList(), clsFilter.DtmStart, clsFilter.DtmEnd);
          break;
        #endregion

        #region StatsByLocation
        case EnumRptRoomSales.StatsByLocation:
          list.AddRange(await BRReportsBySalesRoom.GetRptStatisticsByLocation(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptStatisticsByLocation(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByLocation>().ToList());
          break;
        #endregion

        #region StatsByLocationMonthly
        case EnumRptRoomSales.StatsByLocationMonthly:
          list.AddRange(await BRReportsBySalesRoom.GetRptStaticsByLocationMonthly(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptStaticsByLocationMonthly(reporteName, fileFullPath, filters, list.Cast<RptStatisticsByLocationMonthly>().ToList());
          break;
        #endregion

        #region SalesByLocationMonthly
        case EnumRptRoomSales.SalesByLocationMonthly:
          list.AddRange(await BRReportsBySalesRoom.GetRptSalesByLocationMonthly(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptSalesByLocationMonthly(reporteName, fileFullPath, filters, list.Cast<RptSalesByLocationMonthly>().ToList());
          break;
        #endregion

        #region StatsByLocationAndSalesRoom
        case EnumRptRoomSales.StatsByLocationAndSalesRoom:
          list.AddRange(await BRReportsBySalesRoom.GetRptStatisticsBySalesRoomLocation(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptStatisticsBySalesRoomLocation(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySalesRoomLocation>().ToList());
          break;
        #endregion

        #region ConcerntrateDailySales
        case EnumRptRoomSales.ConcerntrateDailySales:
          #region FiltroSalesRoomConcentrate
          clsFilter.LstSalesRoom.AddRange(clsFilter.LstGoals.Select(c => c.SalesRoomByUser.srID));
          filters.Add(new Tuple<string, string>("Sales Room", string.Join("/", clsFilter.LstGoals.Select(c => c.SalesRoomByUser.srID).ToList())));
          #endregion

          list.AddRange(await BRReportsBySalesRoom.GetRptConcentrateDailySales(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstGoals.Select(c => c.SalesRoomByUser.srID).ToList()));

          if (list.Count > 0)
            file = Reports.RptConcentrateDailySales(reporteName, fileFullPath, clsFilter.DtmEnd, filters,
              list.Cast<RptConcentrateDailySales>().ToList(), clsFilter.LstGoals);
          break;
        #endregion

        #region DailySales
        case EnumRptRoomSales.DailySales:
          list.AddRange(await BRReportsBySalesRoom.GetRptDailySalesDetail(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRoom));
          List<RptDailySalesHeader> lstHeader = await BRReportsBySalesRoom.GetRptDailySalesHeader(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRoom);
          if (list.Count > 0 && lstHeader.Count > 0)
            file = Reports.RptDailySales(reporteName, dateRange,fileFullPath, filters, list.Cast<RptDailySalesDetail>().ToList(),
              lstHeader, clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.Goal);
          break;
        #endregion

        #region FtmIn&OutHouse
        case EnumRptRoomSales.FtmInAndOutHouse:

          list.AddRange(await BRReportsBySalesRoom.GetRptFTMInOutHouse(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstSalesRoom));
          if (list.Count > 0)
          {
            file = Reports.RptFTMInOutHouse(reporteName, fileFullPath, filters, list.Cast<RptFTMInOutHouse>().ToList(), clsFilter.DtmStart, clsFilter.DtmEnd);
          }
          break;
        #endregion

        #region Stats by Segments

        case EnumRptRoomSales.StatsBySegments:
          list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRoom, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
          if (list.Any())
            file = Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
          break;
        #endregion Stats by Segments

        #region Stats by Segments (OWN)
        case EnumRptRoomSales.StatsBySegmentsOwn:
          list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRoom, own: true, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));

          if (list.Any())
            file = Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
          break;
        #endregion Stats by Segments (OWN)          

        #region Stats by Segments Categories
        case EnumRptRoomSales.StatsBySegmentsCategories:
          list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRoom, bySegmentsCategories: true,
            includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
          if (list.Any())
            file = Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
          break;
        #endregion Stats by Segments Categories

        #region Stats by Segments Categories (OWN)
        case EnumRptRoomSales.StatsBySegmentsCategoriesOwn:
          list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(new[] { clsFilter.DtmStart }, new[] { clsFilter.DtmEnd }, clsFilter.LstSalesRoom, bySegmentsCategories: true,
            own: true, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
          if (list.Any())
            file = Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
          break;
        #endregion Stats by Segments Categories (OWN)

        #region Stats by Segments Categories (Multi Date Ranges)
        case EnumRptRoomSales.StatsBySegmentsCategoriesMultiDatesRanges:
          list.AddRange(await BRReportsBySalesRoom.GetStatisticsBySegments(clsFilter.LstMultiDate.Select(x => x.DtStart), clsFilter.LstMultiDate.Select(x => x.DtEnd),
            clsFilter.LstMultiDate.Select(x => x.SalesRoom), bySegmentsCategories: true, includeAllSalesmen: clsFilter.BlnIncludeAllSalesmen));
          if (list.Any())
            file = Reports.RptStatisticsBySegments(reporteName, fileFullPath, filters, list.Cast<RptStatisticsBySegments>().ToList(), clsFilter.BlnGroupedByTeams);
          break;
          #endregion Stats by Segments Categories (Multi Date Ranges)

      }


        if (file == null)
        {
          file = EpplusHelper.CreateNoInfoRptExcel(filters, reporteName, fileFullPath);
        }
        _frmReportQueue.SetFileInfo(fileFullPath, file);
      }
      catch (Exception ex)
      {
        _frmReportQueue.SetFileInfoError(fileFullPath);
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error);
      }
    }


    #region ShowDateRangeSr
    /// <summary>
    /// Muestra el filtro por Sales Room
    /// </summary>
    /// <history>
    /// [ecanul] 03/05/2016 Created
    /// </history>
    private void ShowDateRangeSr()
    {
      _frmFilter = new frmFilterDateRange { frmPrs = this };
      StaStart("Loading Date Range Window...");
      switch (_rptRoomSales)
      {
        case EnumRptRoomSales.StatsBySegments:
        case EnumRptRoomSales.StatsBySegmentsCategories:
        case EnumRptRoomSales.StatsBySegmentsOwn:
        case EnumRptRoomSales.StatsBySegmentsCategoriesOwn:
        case EnumRptRoomSales.StatsByFtbAndLocatios:
        case EnumRptRoomSales.StatsByFtbAndLocatiosCategories:
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, shGroupsByTeams: true, groupByTeams: _clsFilter.BlnGroupedByTeams,
            shAllSalesmen: true, allSalesmen: _clsFilter.BlnIncludeAllSalesmen, multiDate: _multiDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom);
          break;
        case EnumRptRoomSales.StatsBySegmentsCategoriesMultiDatesRanges:
          //Se usa para indicar que no se mostrara el filtro de datos y que las fechas se usaran las que tenga el grid
          _multiDate = true;
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, multiDate: _multiDate, groupByTeams: _clsFilter.BlnGroupedByTeams,
            shGroupsByTeams: true, allSalesmen: _clsFilter.BlnIncludeAllSalesmen, shAllSalesmen: true, shSr: false,
            shMultiDateRanges: true);
          break;
        case EnumRptRoomSales.StatsByFtb:
        case EnumRptRoomSales.StatsByCloser:
        case EnumRptRoomSales.StatsByExitCloser:
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, shGroupsByTeams: true, groupByTeams: _clsFilter.BlnGroupedByTeams,
            shAllSalesmen: true, allSalesmen: _clsFilter.BlnIncludeAllSalesmen, multiDate: _multiDate, onlyOneRegister: _onlyOnRegister,
            shSegments: true, allSegments: _allSegments, shPrograms: true, allProgams: _allPrograms);
          break;
        case EnumRptRoomSales.DailySales:
          _onlyOnRegister = false;
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, multiDate: _multiDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, isGoal: true);
          break;
        case EnumRptRoomSales.ConcerntrateDailySales:
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, shSr: false, shConcentrate: true);
          break;
        case EnumRptRoomSales.EfficiencyWeekly:
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, period: EnumPeriod.Weekly, shWeeks: true, onePeriod: true);
          break;
        case EnumRptRoomSales.StatsByLocation:
        case EnumRptRoomSales.StatsByLocationAndSalesRoom:
          _onlyOnRegister = false;
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, multiDate: _multiDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom);
          break;
        case EnumRptRoomSales.StatsByLocationMonthly:
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, multiDate: _multiDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, period: EnumPeriod.Monthly, onePeriod: true);//queda pendiente blnoneperiod Obliga a que siempre sea de mes en mes
          break;
        case EnumRptRoomSales.SalesByLocationMonthly:
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, multiDate: _multiDate, onlyOneRegister: _onlyOnRegister,
           allSalesRoom: _allSalesRoom, period: EnumPeriod.Monthly);
          break;
        default:
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, multiDate: _multiDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom);
          break;
      }
      _frmFilter.Owner = GetWindow(this);
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
      _frmFilter = new frmFilterDateRange { frmPrs = this };
      StaStart("Loading Date Range Window...");
      switch (_rptSalesman)
      {
        case EnumRptSalesRoomAndSalesman.Manifest:
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, multiDate: _multiDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, shGroupsByTeams: true, groupByTeams: _clsFilter.BlnGroupedByTeams, salesman: _salesman,
            isBySalesman: true, shRoles: true);
          break;
        default:
          _frmFilter.ConfigForm(_clsFilter.DtmStart, _clsFilter.DtmEnd, salesRoom, multiDate: _multiDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, shGroupsByTeams: true, groupByTeams: _clsFilter.BlnGroupedByTeams, salesman: _salesman,
            isBySalesman: true);
          break;
      }
      _frmFilter.ShowDialog();
      StaEnd();
      if (!_frmFilter.ok) return;
      //muestraReportes
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
      if (dtgSalesRoom.SelectedIndex < 0)
        return;
      StaStart("Loading Date Range Window...");
      //obtener el nombre del reporte
      _rptRoomSales = ((KeyValuePair<EnumRptRoomSales, string>)dtgSalesRoom.SelectedItem).Key;
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
      if (ConfigRegistry.ExistReportsPath())
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
      else ShowSystemCfg();
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

    #region ShowSystemCfg

    /// <summary>
    ///Muestra la ventana  para configurar opciones de sistema
    /// </summary>
    /// <history>
    ///   [aalcocer] 07/07/2016 Created
    /// </history>
    private void ShowSystemCfg()
    {
      MessageBoxResult result = UIHelper.ShowMessage("It is not configured path yet. Do you want to configure path now?", MessageBoxImage.Question, Title);
      if (result != MessageBoxResult.Yes) return;
      _systemConfig = new SystemCfg(EnumConfiguration.ReportsPath);
      _systemConfig.Show();
    }

    #endregion ShowSystemCfg

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
      lblUserName.Content = App.User.User.peN;
      _frmReportQueue = new frmReportQueue();
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
      if (ConfigRegistry.ExistReportsPath())
      {
        _frmReportQueue.Show();
        if (_frmReportQueue.WindowState == WindowState.Minimized)
          _frmReportQueue.WindowState = WindowState.Normal;
        _frmReportQueue.Activate();
      }
      else ShowSystemCfg();
    }

    #endregion btnReportQueue_Click

    #endregion

  }

}
