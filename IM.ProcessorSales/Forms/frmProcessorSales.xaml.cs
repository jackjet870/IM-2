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
    //Formulario de filtros
    private frmFilterDateRange _frmFilter;
    //Archivo de configuracion
    private IniFileHelper _iniFieldHelper;
    //Detalles de los filtros
    private bool _oneDate, _onlyOnRegister, _allPrograms, _allSegments, _allSalesRoom;
    private string _salesman;
    //Listado de reportes
    private EnumRptRoomSales _rptRoomSales;
    private EnumRptSalesRoomAndSalesman _rptSalesman;

    #endregion

    #region Publicos

    //Fechas Predefinidas
    public EnumPredefinedDate predefinedDate;

    //Listas para los reportes
    public List<string> lstSalesRoom = new List<string>();
    public List<string> lstPrograms = new List<string>();
    public List<string> lstSegments = new List<string>();
    public List<GoalsHelpper> lstGoals = new List<GoalsHelpper>();
    public List<MultiDateHelpper> lstMultiDate = new List<MultiDateHelpper>();
    public List<PersonnelShort> lstPersonnel = new List<PersonnelShort>();

    //Filtros para los reportes
    public EnumBasedOnArrival basedOnArrival;
    public EnumQuinellas quinellas;
    public DateTime dtmStart, dtmEnd;
    public string salesRoom;
    public decimal goal;
    public bool groupedByTeams, includeAllSalesmen, pr, liner, closer, exit;

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
      dtmStart = _iniFieldHelper.readDate(FilterDate, "DateStart", dtmStart);
      dtmEnd = _iniFieldHelper.readDate(FilterDate, "DateEnd", dtmEnd);
      salesRoom = _iniFieldHelper.ReadText(FilterDate, "SalesRoom", string.Empty);
      if (!string.IsNullOrEmpty(salesRoom))
        lstSalesRoom.Add(salesRoom);
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
      goal = Convert.ToDecimal(_iniFieldHelper.readDouble(FilterDate, "Goal", 0));
      groupedByTeams = _iniFieldHelper.ReadBoolean(FilterDate, "GroupedByTeams", false);
      includeAllSalesmen = _iniFieldHelper.ReadBoolean(FilterDate, "IncludeAllSalesmen", false);
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
      DateTime serverDate = BRHelpers.GetServerDate();
      // Fecha Inicial
      dtmStart = new DateTime(serverDate.Year, serverDate.Month, 1);
      //FechaFinal
      dtmEnd = serverDate;

      _allSalesRoom = false;

      // Obtenemos los valores de un archivo de configuracion
      SetUpIniField();
      //roles de vendedores
      pr = liner = closer = exit = true;
      //segmentos y programas
      _allSegments = _allPrograms = true;
    }

    #endregion

    /// <summary>
    /// Muestra el reporte de Sales Room Seleccionado
    /// </summary>
    /// <history>
    /// [ecanul] 05/05/2016 Created
    /// </history>
    private async void ShowReportBySalesRoom()
    {
      FileInfo file = null;
      //Deberia validarse con 
      #region Datos del reporte
      string dateRange = _oneDate
          ? DateHelper.DateRange(lstMultiDate[0].dtStart, lstMultiDate[0].dtEnd)
          : DateHelper.DateRange(dtmStart, dtmEnd);
      string dateRangeFileName = _oneDate
        ? DateHelper.DateRangeFileName(lstMultiDate[0].dtStart, lstMultiDate[0].dtEnd)
        : DateHelper.DateRangeFileName(dtmStart, dtmEnd);
      string reporteName = EnumToListHelper.GetEnumDescription(_rptRoomSales);
      #endregion

      #region Filtro(s)
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>
      {
        new Tuple<string, string>("Date Range", dateRange)
      }; 
      //Si es cualquier reporte menos Concentrate Daily Sales o Multidate (porque sus grids son diferentes) se agrega de manera comun
      if (_rptRoomSales != EnumRptRoomSales.ConcerntrateDailySales && _rptRoomSales != EnumRptRoomSales.StatsBySegmentsCategoriesMultiDatesRanges)
      {
        //Si es de solo un registro El sales Room es unico, si no Se toma por Todos o por los seleccionados
        if (_onlyOnRegister)
          filters.Add(new Tuple<string, string>("Sales Room", lstSalesRoom[0]));
        else
          filters.Add(new Tuple<string, string>("Sales Room",
            _frmFilter.dtgSalesRoom.Items.Count == lstSalesRoom.Count ? "All" : string.Join(",", lstSalesRoom)));
      }
      #endregion

      List<dynamic> list = new List<dynamic>();
      StaStart("Loading report...");
      switch (_rptRoomSales)
      {
        #region Manifest
        case EnumRptRoomSales.Manifest:
          list.AddRange(await BRReportsBySalesRoom.GetRptManiest(dtmStart, dtmEnd, lstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptManifest(reporteName, dateRangeFileName, filters, list.Cast<RptManifest>().ToList(), dtmStart, dtmEnd);
          break;
        #endregion

        #region StatsByLocation
        case EnumRptRoomSales.StatsByLocation:
          list.AddRange(await BRReportsBySalesRoom.GetRptStatisticsByLocation(dtmStart, dtmEnd, lstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptStatisticsByLocation(reporteName, dateRangeFileName, filters, list.Cast<RptStatisticsByLocation>().ToList());
          break;
        #endregion

        #region StatsByLocationMonthly
        case EnumRptRoomSales.StatsByLocationMonthly:
          list.AddRange(await BRReportsBySalesRoom.GetRptStaticsByLocationMonthly(dtmStart, dtmEnd, lstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptStaticsByLocationMonthly(reporteName, dateRangeFileName, filters, list.Cast<RptStatisticsByLocationMonthly>().ToList());
          break;
        #endregion

        #region SalesByLocationMonthly
        case EnumRptRoomSales.SalesByLocationMonthly:
          list.AddRange(await BRReportsBySalesRoom.GetRptSalesByLocationMonthly(dtmStart, dtmEnd, lstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptSalesByLocationMonthly(reporteName, dateRangeFileName, filters, list.Cast<RptSalesByLocationMonthly>().ToList());
          break;
        #endregion

        #region StatsByLocationAndSalesRoom
        case EnumRptRoomSales.StatsByLocationAndSalesRoom:
          list.AddRange(await BRReportsBySalesRoom.GetRptStatisticsBySalesRoomLocation(dtmStart, dtmEnd, lstSalesRoom));
          if (list.Count > 0)
            file = Reports.RptStatisticsBySalesRoomLocation(reporteName, dateRangeFileName, filters, list.Cast<RptStatisticsBySalesRoomLocation>().ToList());
          break;
        #endregion

        #region ConcerntrateDailySales
        case EnumRptRoomSales.ConcerntrateDailySales:
          #region FiltroSalesRoomConcentrate
          lstSalesRoom.AddRange(lstGoals.Select(c => c.salesRoom.srID));
          filters.Add(new Tuple<string, string>("Sales Room", string.Join("/", lstGoals.Select(c => c.salesRoom.srID).ToList())));
          #endregion

          list.AddRange(await BRReportsBySalesRoom.GetRptConcentrateDailySales(dtmStart, dtmEnd, lstGoals.Select(c => c.salesRoom.srID).ToList()));

          if (list.Count > 0)
            file = Reports.RptConcentrateDailySales(reporteName, dateRangeFileName, dtmEnd, filters,
              list.Cast<RptConcentrateDailySales>().ToList(), lstGoals);
          break;
        #endregion

        #region DailySales
        case EnumRptRoomSales.DailySales:
          list.AddRange(await BRReportsBySalesRoom.GetRptDailySalesDetail(dtmStart, dtmEnd, lstSalesRoom));
          List<RptDailySalesHeader> lstHeader = await BRReportsBySalesRoom.GetRptDailySalesHeader(dtmStart, dtmEnd, lstSalesRoom);
          if (list.Count > 0 && lstHeader.Count > 0)
            file = Reports.RptDailySales(reporteName, dateRange, filters, list.Cast<RptDailySalesDetail>().ToList(), 
              lstHeader, dtmStart,dtmEnd,goal);
          break;
        #endregion

        #region FtmIn&OutHouse
        case EnumRptRoomSales.FtmInAndOutHouse:

          list.AddRange(await BRReportsBySalesRoom.GetRptFTMInOutHouse(dtmStart, dtmEnd, lstSalesRoom));
          if (list.Count > 0)
          {
            file = Reports.RptFTMInOutHouse(reporteName, dateRangeFileName, filters, list.Cast<RptFTMInOutHouse>().ToList(), dtmStart, dtmEnd);
          }
          break; 
        #endregion
      }

      if (file != null)
        Process.Start(file.FullName);
      else
        UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);

      StaEnd();
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
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, shGroupsByTeams: true, groupByTeams: groupedByTeams,
            shAllSalesmen: true, allSalesmen: includeAllSalesmen, oneDate: _oneDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom);
          break;
        case EnumRptRoomSales.StatsBySegmentsCategoriesMultiDatesRanges:
          //Se usa para indicar que no se mostrara el filtro de datos y que las fechas se usaran las que tenga el grid
          _oneDate = true; 
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, oneDate: _oneDate, groupByTeams: groupedByTeams,
            shGroupsByTeams: true, allSalesmen: includeAllSalesmen, shAllSalesmen: true, shSr: false,
            shMultiDateRanges: true);
          break;
        case EnumRptRoomSales.StatsByFtb:
        case EnumRptRoomSales.StatsByCloser:
        case EnumRptRoomSales.StatsByExitCloser:
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, shGroupsByTeams: true, groupByTeams: groupedByTeams,
            shAllSalesmen: true, allSalesmen: includeAllSalesmen, oneDate: _oneDate, onlyOneRegister: _onlyOnRegister,
            shSegments: true, allSegments: _allSegments, shPrograms: true, allProgams: _allPrograms);
          break;
        case EnumRptRoomSales.DailySales:
          _onlyOnRegister = false;
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, oneDate: _oneDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, isGoal: true);
          break;
        case EnumRptRoomSales.ConcerntrateDailySales:
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, shSr: false, shConcentrate: true);
          break;
        case EnumRptRoomSales.EfficiencyWeekly:
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, period: EnumPeriod.Weekly, shWeeks: true, onePeriod: true);
          break;
        case EnumRptRoomSales.StatsByLocation:
        case EnumRptRoomSales.StatsByLocationAndSalesRoom:
          _onlyOnRegister = false;
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, oneDate: _oneDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom);
          break;
        case EnumRptRoomSales.StatsByLocationMonthly:
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, oneDate: _oneDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, period: EnumPeriod.Monthly, onePeriod: true);//queda pendiente blnoneperiod Obliga a que siempre sea de mes en mes
          break;
        case EnumRptRoomSales.SalesByLocationMonthly:
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, oneDate: _oneDate, onlyOneRegister: _onlyOnRegister,
           allSalesRoom: _allSalesRoom, period: EnumPeriod.Monthly);
          break;
        default:
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, oneDate: _oneDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom);
          break;
      }
      _frmFilter.ShowDialog();
      StaEnd();
      if (!_frmFilter.ok) return;
      ShowReportBySalesRoom();
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
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, oneDate: _oneDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, shGroupsByTeams: true, groupByTeams: groupedByTeams, salesman: _salesman,
            isBySalesman: true, shRoles: true, pr: pr, liner: liner, closer: closer, exit: exit);
          break;
        default:
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, oneDate: _oneDate, onlyOneRegister: _onlyOnRegister,
            allSalesRoom: _allSalesRoom, shGroupsByTeams: true, groupByTeams: groupedByTeams, salesman: _salesman,
            isBySalesman: true, shRoles: false, pr: pr, liner: liner, closer: closer, exit: exit);
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
      if(dtgSalesRoom.SelectedIndex < 0 )
        return;
      StaStart("Loading Date Range Window...");
      //obtener el nombre del reporte
      _rptRoomSales = ((KeyValuePair<EnumRptRoomSales, string>) dtgSalesRoom.SelectedItem).Key;
      //Reportes que solo necesitan una fecha 
      _oneDate = false;
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
      _oneDate = false;
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
        var dataGridRow = (DataGridRow) obj;
        if (dataGridRow.Item.Equals(dtgSalesRoom.CurrentItem)) PrepareReportBySalesRoom();
        else if (dataGridRow.Item.Equals(dtgSalesman.CurrentItem)) PrepareReportBySalesman();
       
      }
      else //de boton 
      {
        if (obj.Equals(btnPrintSr)) PrepareReportBySalesRoom();
        else if(obj.Equals(btnPrintSm)) PrepareReportBySalesman();
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
      lblUserName.Content = App.User.User.peN;
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
      LoadFilter(sender,1);
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
      LoadFilter(sender,0);
    }
    #endregion

    #region grdrpt_PreviewKeyDown
    private void grdrpt_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key != Key.Enter)return;
      LoadFilter(sender,0);
    }

    #endregion

    #endregion

  }

}
