using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    private frmFilterDateRange _frmFilter;
    private bool _oneDate, _onlyOnRegister;
    

    //Fechas Predefinidas
    public  EnumPredefinedDate predefinedDate;

    //Listas para los reportes
    public List<string> lstSalesRoom = new List<string>();
    public List<string> lstPrograms = new List<string>();
    public List<string> lstSegments = new List<string>();
    public List<GoalsHelpper> lstGoals = new List<GoalsHelpper>();
    public List<MultiDateHelpper> lstMultiDate = new List<MultiDateHelpper>();
    public List<PersonnelShort> lstPersonnel = new List<PersonnelShort>();
    public EnumBasedOnArrival basedOnArrival;
    public EnumQuinellas quinellas;
    public DateTime dtmStart, dtmEnd;
    public decimal goal;
    public bool groupedByTeams, includeAllSalesmen;
    public bool pr, liner, closer, exit;

    private bool _allSalesRoom;

    //Variables para el filtro de fechas, salas y meta

    public string salesRoom;
    //Variables para el filtro de multi fechas, salas y metas
    private List<string> _lstDateRanges = new List<string>();

    private IniFileHelper _iniFieldHelper;

    //Variables para el filtro de fechas, salas y equipos
    

    //Variables para el filtro de fechas, salas y vendedor
    private string _salesman, _salesmanName, _salesmenRoles;
    

    //Segmentos y Programas seleccionados
    private bool _allPrograms, _allSegments;

    private const string FilterDate = "FilterDate";


    private EnumRptRoomSales _rptRoomSales;
    private EnumRptSalesRoomAndSalesman _rptSalesman;

    #endregion

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
    private void LoadIniField()
    {
      string archivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(archivo)) return;
      _iniFieldHelper = new IniFileHelper(archivo);
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
      DateTime serverDate = BRHelpers.GetServerDate();
      // Fecha Inicial
      dtmStart = new DateTime(serverDate.Year, serverDate.Month,1);
      //FechaFinal
      dtmEnd = serverDate;
      //carga las fechas desde el archivo de configuracion
      if (_iniFieldHelper == null) return;
      dtmStart = _iniFieldHelper.readDate(FilterDate, "DateStart", dtmStart);
      dtmEnd = _iniFieldHelper.readDate(FilterDate, "DateEnd", dtmEnd);
      salesRoom = _iniFieldHelper.readText(FilterDate, "SalesRoom", string.Empty);
      if (!string.IsNullOrEmpty(salesRoom))
        lstSalesRoom.Add(salesRoom);
    }
    #endregion

    #region SetupParameters

    /// <summary>
    /// Configura los parametros de los reportes
    /// </summary>
    /// <history>
    /// [ecanul] 22/04/2016 Created
    /// </history>
    private void SetupParameters()
    {
      // Obtenemos las fechas iniciales de los reportes
      GetFirstDayValue();

      goal = Convert.ToDecimal(_iniFieldHelper.readDouble(FilterDate, "Goal", 0));
      groupedByTeams = _iniFieldHelper.readBool(FilterDate, "GroupedByTeams", false);
      includeAllSalesmen = _iniFieldHelper.readBool(FilterDate, "IncludeAllSalesmen", false);

      _salesman = _iniFieldHelper.readText(FilterDate, "Salesman", string.Empty);

      //roles de vendedores
      pr = liner = closer= exit = true;

      //Todos los segmentos
      _allSegments = _allPrograms = true;

      _iniFieldHelper = null;
    }

    #endregion

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
          _frmFilter.ConfigForm(dtmStart, dtmEnd, salesRoom, oneDate: true, groupByTeams: groupedByTeams,
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
      //muestraReportes
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

    #region Metodos del formulario
    
    #region Window_KeyDown

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        KeyboardHelper.CkeckKeysPress(statusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        KeyboardHelper.CkeckKeysPress(statusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        KeyboardHelper.CkeckKeysPress(statusBarNum, Key.NumLock);
      }
    }

    #endregion
    
    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      StaStart("Load Form...");
      LoadIniField();
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
