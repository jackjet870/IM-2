using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using IM.Base.Interfaces;
using IM.Base.Forms;
using IM.Model;
using IM.Host.Forms;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.Host.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using IM.Base.Reports;

namespace IM.Host
{
  /// <summary>
  /// Interaction logic for frmHost.xaml
  /// </summary>
  /// 
  public partial class frmHost : Window, IMetodosPublicos
  {
    private DateTime? _dtpCurrent = null;
    public static DateTime _dtpServerDate = new DateTime();
    CollectionViewSource _Host;

    #region Listas Globales
    public static List<Currency> _lstCurrencies;
    public static List<PaymentType> _lstPaymentsType;
    public static List<MaritalStatus> _lstMaritalStatus;
    public static List<AgencyShort> _lstAgencies;
    public static List<LanguageShort> _lstLanguaje;
    public static List<CountryShort> _lstCountries;
    public static List<Hotel> _lstHotel;
    public static List<Bank> _lstBanks;
    public static List<Location> _lstLocations;
    public static List<ChargeTo> _lstChargeTo;
    public static List<SalesRoomShort> _lstSalesRoom;
    public static List<SourcePayment> _lstSourcePayments;
    public static List<TeamSalesmen> _lstTeamSalesMen;
    public static List<PersonnelShort> _lstPersonnel;
    public static List<PersonnelShort> _lstPersonnelHOSTENTRY;
    public static List<PersonnelShort> _lstPersonnelPR;
    public static List<PersonnelShort> _lstPersonnelLINER;
    public static List<PersonnelShort> _lstPersonnelEXIT;
    public static List<PersonnelShort> _lstPersonnelPODIUM;
    public static List<PersonnelShort> _lstPersonnelCLOSER;
    public static List<PersonnelShort> _lstPersonnelCLOSEREXIT;
    public static List<PersonnelShort> _lstPersonnelHOSTGIFTS;
    public static List<PersonnelShort> _lstPersonnelHOSTEXIT;
    public static List<PersonnelShort> _lstPersonnelVLO;
    public static List<PersonnelShort> _lstPersonnelPRCAPT;
    public static List<PersonnelShort> _lstPersonnelLINERCAPT;
    public static List<PersonnelShort> _lstPersonnelCLOSERCAPT;
    public static List<Gift> _lstGifts;
    public static List<Program> _lstPrograms;
    public static List<LeadSource> _lstLeadSources;
    public static List<RefundType> _lstRefundTypes;
    #endregion


    public frmHost()
    {
      InitializeComponent();
    }

    #region Métodos de controles en el formulario

    #region Window_Loaded
    /// <summary>
    /// Realiza las configuraciones de los controles de la forma
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Host_Load();
    }
    #endregion

    #region frmHost_KeyDown
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [lchairezReload] 09/Feb/2016 Created
    /// </history>
    private void frmHost_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }
    #endregion

    /// <summary>
    /// Funcion del evento Changed del DatePicker
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 26/02/2016 Created
    /// </history>
    private void dtpDateChanged()
    {
      if (_dtpCurrent != dtpDate.Value)
      {
        // Asignamos la fecha seleccionada.
        _dtpCurrent = dtpDate.Value.Value.Date;

        _Host = ((CollectionViewSource)(this.FindResource("dsPremanifestHost")));
        _Host.Source = BRGuests.GetPremanifestHost(_dtpCurrent, App.User.SalesRoom.srID);
      }
    }

    /// <summary>
    /// Funcion que se encarga de validar el total de datos obtenidos en el datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/03/2016 Created
    /// </history>
    private void dtgPremanifestHost_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (dtgPremanifestHost.Items.Count == 0)
      {
        StatusBarReg.Content = "No Records";
        return;
      }
      StatusBarReg.Content = string.Format("{0}/{1}", dtgPremanifestHost.Items.IndexOf(dtgPremanifestHost.CurrentItem) + 1, dtgPremanifestHost.Items.Count);
    }

    /// <summary>
    /// Función encargada de aplicar efecto a la ventana
    /// </summary>
    /// <param name="win"></param>
    /// <history>
    /// [vipacheco] 26-02-2016 Created
    /// </history>
    private void AplicarEfecto(Window win)
    {
      var objBlur = new System.Windows.Media.Effects.BlurEffect();
      objBlur.Radius = 5;
      win.Effect = objBlur;
    }

    #endregion

    #region Métodos privados

    /// <summary>
    /// Manda llamar a todos los métodos de configuración de los controles al ser cargada la forma
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void ConfigControls()
    {
      ConfigDataGrid();
      KeyDefault(StatusBarCap);
      KeyDefault(StatusBarIns);
      KeyDefault(StatusBarNum);
    }

    /// <summary>
    /// Configura el Datagrid al cargar la forma
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void ConfigDataGrid()
    {
      var heigthgrdMenu = grdPanel.ActualHeight;
      var heightStatusBar = stbStatusBar.ActualHeight;
    }

    /// <summary>
    /// Asigna valores por defecto a los StatusBarItem cuando se carga el form
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran los valores</param>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }

    /// <summary>
    /// Revisa si alguna de las teclas Bloq Mayús, Bloq Núm o Insert está activo
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran valores si está activa la tecla</param>
    /// <param name="key">tecla que revisaremos si se encuentra activa</param>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }

    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 09/Feb/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Métodos públicos

    public void PrintReport()
    {
    }

    public void ShowReport()
    {
    }

    public void ShowReportDesigner()
    {
      frmHostReports frm = new frmHostReports(dtpDate.Value.Value) { Owner=this};
      frm.ShowDialog();
    }

    #endregion

    #region ShowShow_Click
    /// <summary>
    /// Método que despliega el formulario Show
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 30/Abril/2016 Created
    /// </history>
    private void ShowShow_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      DataGridRow row = sender as DataGridRow;
      CheckBox _chekedValue = sender as CheckBox;
      GuestPremanifestHost guestHost = (GuestPremanifestHost)dtgPremanifestHost.SelectedItem;

      //CheckBox _chekedValue = sender as CheckBox;

      //Validamos que sea un invitado valido
      if (ValidateGuest(guestHost, EnumPermission.Show, "guShow"))
      {
        // Desplegamos el formulario Show
        frmShow _frmShow = new frmShow(guestHost.guID);
        _frmShow.ShowInTaskbar = false;
        _frmShow.Owner = this;
        _frmShow.ShowDialog();

      }
      else
      {
        if (!guestHost.guShow)
        {
          _chekedValue.IsChecked = false;
        }
      }
    }
    #endregion

    #region ValidateGuest
    /// <summary>
    /// Función encargada de validar que sea un usuario valido con sus respectivos permisos
    /// </summary>
    /// <param name="guestHost"></param>
    /// <returns> true - user valid | false - user no valid </returns>
    /// <history>
    /// [vipacheco] 02/15/2016 Created
    /// </history>
    private bool ValidateGuest(GuestPremanifestHost guestHost, EnumPermission permission, string strField)
    {
      if (guestHost.guBookCanc) // Validamos que no sea un booking cancelado
      {
        UIHelper.ShowMessage("Cancelled booking.", MessageBoxImage.Exclamation);
        return false;
      }
      else if (!App.User.HasPermission(permission, EnumPermisionLevel.ReadOnly)) // validamos los permisos del usuario - SIN PERMISOS
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Exclamation);
        return false;
      }
      else if (!App.User.HasPermission(permission, EnumPermisionLevel.Standard)) // PERMISO - Solo Lectura
      {
        if (!guestHost.guMealTicket && strField == "guMealTicket")
        {
          UIHelper.ShowMessage("You have read access.", MessageBoxImage.Exclamation);
          return false;
        }
        else if (!guestHost.guGiftsReceived && strField == "guGiftsReceived")
        {
          UIHelper.ShowMessage("You have read access.", MessageBoxImage.Exclamation);
          return false;
        }
        else if (!guestHost.guShow && strField == "guShow")
        {
          UIHelper.ShowMessage("You have read access.", MessageBoxImage.Exclamation);
          return false;
        }
      }
      else if (guestHost.guShow == false && (strField == "guMealTicket" || strField == "guSale"))
      {
        UIHelper.ShowMessage("This option should have a show marked.", MessageBoxImage.Exclamation);
        return false;
      }
      return true;
    }
    #endregion

    #region guMealTickets_Click
    /// <summary>
    /// Invoca el formulario Meal Ticket por medio del CheckBox correspondiente!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// </history>
    private void guMealTickets_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      CheckBox _chekedValue = sender as CheckBox;
      GuestPremanifestHost guestHost = (GuestPremanifestHost)_chekedValue.DataContext;

      //Validamos que sea un invitado valido
      if (ValidateGuest(guestHost, EnumPermission.MealTicket, "guMealTicket"))
      {
        // Desplegamos el formulario Show
        frmMealTickets _frmMealTickets = new frmMealTickets(guestHost.guID);
        _frmMealTickets.ShowInTaskbar = false;

        List<MealTicket> _valuePreview = BRMealTickets.GetMealTickets(guestHost.guID);
        SalesRoomCloseDates _closeSalesRoom = BRSalesRooms.GetSalesRoom(App.User.SalesRoom.srID);

        if (guestHost.guMealTicket)
        {
          // Verificamos si alguno de sus cupones de comida es de una fecha cerrada, impedimos modificar los datos
          _valuePreview.ForEach(x =>
                                    {
                                      if (IsClosed_MealTicket(x.meD, _closeSalesRoom.srMealTicketsCloseD))
                                      {
                                        _frmMealTickets.modeOpen = EnumModeOpen.Preview;
                                        return;
                                      }
                                      else
                                        _frmMealTickets.modeOpen = EnumModeOpen.PreviewEdit;
                                    });
        }
        else
          _frmMealTickets.modeOpen = EnumModeOpen.PreviewEdit;

        _frmMealTickets.Owner = this;
        _frmMealTickets.ShowDialog();

        //dtgPremanifestHost.Items.Refresh();
      }
      else
        _chekedValue.IsChecked = false;

      dtgPremanifestHost.Items.Refresh();
      dtpDateChanged();

    }
    #endregion

    #region IsClosed_MealTicket
    /// <summary>
    /// Evalua si el Mealticket no se ha cerrado!
    /// </summary>
    /// <param name="pdtmDate"></param>
    /// <param name="pdtmClose"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 23/03/2016 Created
    /// </history>
    private bool IsClosed_MealTicket(DateTime pdtmDate, DateTime pdtmClose)
    {
      bool blnClosed = false;
      DateTime _pdtmDate;

      if (DateTime.TryParse(pdtmDate + "", out _pdtmDate))
      {
        if (_pdtmDate <= pdtmClose)
        {
          blnClosed = true;
        }
      }
      return blnClosed;
    }
    #endregion

    #region ShowGiftsReceived_Click
    /// <summary>
    /// Invoca el formulario Gifts Received
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 04/04/2016 Created
    /// </history>
    private void ShowGiftsReceived_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      CheckBox _chekedValue = sender as CheckBox;
      GuestPremanifestHost guestHost = (GuestPremanifestHost)_chekedValue.DataContext;

      //Validamos que sea un invitado valido
      if (ValidateGuest(guestHost, EnumPermission.GiftsReceipts, "guGiftsReceived"))
      {
        bool _edit = App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard);

        frmGiftsReceipts _frmGiftsReceipts = new frmGiftsReceipts(guestHost.guID);
        _frmGiftsReceipts.ShowInTaskbar = false;
        _frmGiftsReceipts.Owner = this;
        _frmGiftsReceipts.modeOpenBy = EnumOpenBy.Checkbox;
        _frmGiftsReceipts.modeOpen = (_edit) ? EnumModeOpen.Edit : EnumModeOpen.Preview;
        _frmGiftsReceipts.ShowDialog();

        _chekedValue.IsChecked = true;
      }
      else
      {
        _chekedValue.IsChecked = guestHost.guGiftsReceived;
      }
    }
    #endregion

    #region GetAllCatalogsHost
    /// <summary>
    /// Función que obtiene todos los catalogos utilizados en los combos del modulo Host
    /// </summary>
    /// <history>
    /// [vipacheco] 02/Mayo/2016 Created
    /// </history>
    private async void GetAllCatalogsHost()
    {
      // Obtenemos el id de la sala de ventas.
      string _salesRoom = App.User.SalesRoom.srID;

      #region Currencies
      _lstCurrencies = await BRCurrencies.GetCurrencies(null, 1);
      #endregion
      #region Payment Types
      _lstPaymentsType = await BRPaymentTypes.GetPaymentTypes(1);
      #endregion
      #region Marital Status
      _lstMaritalStatus = await BRMaritalStatus.GetMaritalStatus(1);
      #endregion
      #region Agencies
      _lstAgencies = await BRAgencies.GetAgencies(1);
      #endregion
      #region Countries
      _lstCountries = await BRCountries.GetCountries(1);
      #endregion
      #region Languajes
      _lstLanguaje = await BRLanguages.GetLanguages(1);
      #endregion
      #region Hotels
      _lstHotel = await BRHotels.GetHotels(null, 1);
      #endregion
      #region Team Sales Men
      _lstTeamSalesMen = BRTeamsSalesMen.GetTeamsSalesMen(1);
      #endregion
      #region Personnel
      _lstPersonnel = await BRPersonnel.GetPersonnel("ALL", "ALL", "ALL", 1);
      #endregion
      #region Host (ess) de llegada
      _lstPersonnelHOSTENTRY = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "HOSTENTRY", 1);
      #endregion
      #region Host (ess) de regalos
      _lstPersonnelHOSTGIFTS = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "HOSTGIFTS", 1);
      #endregion
      #region Host (ess) de salida
      _lstPersonnelHOSTEXIT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "HOSTEXIT", 1);
      #endregion
      #region PR's
      _lstPersonnelPR = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "PR", 1);
      #endregion
      #region Closer´s
      _lstPersonnelCLOSER = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "CLOSER", 1);
      #endregion
      #region Exit Closer´s
      _lstPersonnelCLOSEREXIT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "EXIT", 1);
      #endregion
      #region Podium
      _lstPersonnelPODIUM = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "PODIUM", 1);
      #endregion
      #region Verificador Legal
      _lstPersonnelVLO = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "VLO", 1);
      #endregion
      #region Liner's
      _lstPersonnelLINER = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "LINER", 1);
      #endregion
      #region Gifts
      _lstGifts = BRGifts.GetGifts(1);
      #endregion
      #region Banks
      _lstBanks = await BRBanks.GetBanks(1);
      #endregion
      #region Source Payments
      _lstSourcePayments = BRSourcePayments.GetSourcePayments(1);
      #endregion
      #region SalesRoomShort
      _lstSalesRoom = await BRSalesRooms.GetSalesRooms(1);
      #endregion
      #region Locations
      _lstLocations = BRLocations.GetLocations(1);
      #endregion
      #region Charge To
      _lstChargeTo = await BRChargeTos.GetChargeTos();
      #endregion
      #region Capitanes de PR's
      _lstPersonnelPRCAPT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "PRCAPT", 1);
      #endregion
      #region Capitanes de Liner's
      _lstPersonnelLINERCAPT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "LINERCAPT", 1);
      #endregion
      #region Capitanes de Closer's
      _lstPersonnelCLOSERCAPT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "CLOSERCAPT", 1);
      #endregion
      #region Program's
      _lstPrograms = await BRPrograms.GetPrograms();
      #endregion
      #region LeadSources
      _lstLeadSources = await BRLeadSources.GetLeadSources(1, EnumProgram.All);
      #endregion
      #region Refund Types
      _lstRefundTypes = BRRefundTypes.GetRefundTypes(1);
      #endregion
    }
    #endregion

    private void ShowSale_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      DataGridRow row = sender as DataGridRow;
      CheckBox _chekedValue = sender as CheckBox;
      GuestPremanifestHost guestHost = (GuestPremanifestHost)dtgPremanifestHost.SelectedItem;

      if (ValidateGuest(guestHost, EnumPermission.Sales, "guSale"))
      {
        // Desplegamos el formulario Show
        frmShow _frmShow = new frmShow(guestHost.guID);
        _frmShow.ShowInTaskbar = false;
        _frmShow.Owner = this;
        _frmShow.ShowDialog();

      }
    }

    #region dtpDate_PreviewKeyDown
    /// <summary>
    /// Funcion que evalua cuando se pulsa enter dentro del datepicker
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtpDate_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        dtpDateChanged();
      }
    }
    #endregion

    #region Host_Load
    /// <summary>
    /// Carga los controles e informacion necesaria cuando se inicia el modulo
    /// </summary>
    /// <history>
    /// [vipacheco] 02/Junio/2016 Created
    /// </history>
    private async void Host_Load()
    {
      // Agregamos la informacion del usuario
      txtUser.Text = App.User.User.peN.ToString();
      txtSalesRoom.Text = App.User.SalesRoom.srN.ToString();

      // Obtenemos los catalogos
      GetAllCatalogsHost();

      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);

      _dtpServerDate = BRHelpers.GetServerDate();

      // Se verifica que el tipo de permiso del usuario para habilitar y/o deshabilitar opciones necesarias.
      if (App.User.HasPermission(EnumPermission.Host, EnumPermisionLevel.ReadOnly))// 
      {
        guShowSeqColumn.IsReadOnly = true;
        guQuinellaColumn.IsReadOnly = true;
        guWCommentsColumn.IsReadOnly = true;
      }

      // Actualizamos los tipos de cambio de monedas hasta el dia de hoy
      await BRExchangeRate.InsertExchangeRate(_dtpServerDate.Date);

      // Actualizamos las fechas de temporada hasta el año actual
      await BRSeasons.UpdateSeasonDates(_dtpServerDate.Year);

    }
    #endregion

    private void dtpDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (_dtpCurrent != dtpDate.Value)
      {
        // Asignamos la fecha seleccionada.
        _dtpCurrent = dtpDate.Value.Value.Date;

        _Host = ((CollectionViewSource)(this.FindResource("dsPremanifestHost")));
        _Host.Source = BRGuests.GetPremanifestHost(_dtpCurrent, App.User.SalesRoom.srID);

      }
    }

    #region Metodos de Botones

    #region btnExchangeRate_Click
    /// <summary>
    /// Despliega el formulario de tipos de cambio de monedas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 04/03/2016 Created
    /// </history>
    private void btnExchangeRate_Click(object sender, RoutedEventArgs e)
    {
      //Verificamos si el usuario cuenta con los permisos suficientes
      if (!App.User.HasPermission(EnumPermission.ExchangeRates, EnumPermisionLevel.ReadOnly))
      {
        MessageBox.Show("User doesn't have access", "Exchange Rate");
        return;
      }

      frmExchangeRate _frExtChangeRate = new frmExchangeRate(_dtpServerDate);
      _frExtChangeRate.ShowInTaskbar = false;
      _frExtChangeRate.Owner = this;
      _frExtChangeRate.ShowDialog();
    }
    #endregion

    #region btnSales_Click
    /// <summary>
    /// Despliega el formulario de ventas en modo busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco]
    /// </history>
    private void btnSales_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region btnExcel_Click
    /// <summary>
    /// Despliega el reporte de manifiesto por Lead Source para Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// [edgrodriguez] 22/Junio/2016 Modified. Se agregó el proceso de creación del reporte Manifest By LS
    /// </history>
    private async void btnExcel_Click(object sender, RoutedEventArgs e)
    {
      
      var dateRange = DateHelper.DateRange(dtpDate.Value.Value, dtpDate.Value.Value);
      var dateRangeFileName = DateHelper.DateRangeFileName(dtpDate.Value.Value, dtpDate.Value.Value);
      var filters = new List<Tuple<string, string>>();

      var lstManifestRange = await BRReportsBySalesRoom.GetRptManifestRangeByLs(dtpDate.Value, dtpDate.Value, App.User.SalesRoom.srID);
      if (lstManifestRange.Any())
      {
        var lstRptManifest = lstManifestRange[0] as List<RptManifestByLSRange>;
        var lstBookings = lstManifestRange[1] as List<RptManifestByLSRange_Bookings>; 
               
        var dtRptManifest = TableHelper.GetDataTableFromList(lstRptManifest, true);

        var dtBookings = TableHelper.GetDataTableFromList(lstBookings.Select(c => new
        {
          c.guloInvit,
          c.LocationN,
          guBookTime = c.guBookT,
          c.guBookT,
          c.Bookings
        }).ToList(), true, false);

        filters.Add(new Tuple<string, string>("Date Range", dateRange));
        filters.Add(new Tuple<string, string>("Sales Room", App.User.SalesRoom.srID));
        EpplusHelper.ExportRptManifestRangeByLs(new List<Tuple<DataTable, List<Model.Classes.ExcelFormatTable>>> {
        Tuple.Create(dtRptManifest, clsFormatReports.RptManifestRangeByLs()),
        Tuple.Create(dtBookings, clsFormatReports.RptManifestRangeByLs_Bookings())
      }, filters, "Manifest By LS", dateRangeFileName, blnRowGrandTotal: true, blnShowSubtotal: true);
      }
    }
    #endregion

    #region btnPrint_Click
    /// <summary>
    /// Imprime el reporte
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      PrintReport();
    }
    #endregion

    #region btnPreview_Click
    /// <summary>
    /// Muestra una vista previa del reporte
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnPreview_Click(object sender, RoutedEventArgs e)
    {
      ShowReport();
    }
    #endregion

    #region btnMealTickets_Click
    /// <summary>
    /// Invoca  el formulario Meal Ticket por medio del boton correspondiente.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// </history>
    private void btnMealTickets_Click(object sender, RoutedEventArgs e)
    {
      // Se verifica si el usuario tiene permisos de edicion!
      bool modeEdit = App.User.HasPermission(EnumPermission.MealTicket, EnumPermisionLevel.Standard);

      // Se invoca el formulario de acuerdo al permiso del usuario!
      frmMealTickets _frmMealTickets = new frmMealTickets();
      _frmMealTickets.ShowInTaskbar = false;
      _frmMealTickets.modeOpen = ((modeEdit == true) ? EnumModeOpen.Edit : EnumModeOpen.Search);
      _frmMealTickets.Owner = this;
      _frmMealTickets.ShowDialog();
    }
    #endregion

    #region btnGiftsReceipts_Click
    /// <summary>
    /// Función para madar ejecutar el formulario Gifts Receipts
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/17/2016 Created
    /// </history>
    private void btnGiftsReceipts_Click(object sender, RoutedEventArgs e)
    {
      // Se verifica si el usuario tiene permisos de edicion!
      bool modeEdit = App.User.HasPermission(EnumPermission.MealTicket, EnumPermisionLevel.Standard);

      // Se invoca el formulario de acuerdo al permiso del usuario!
      frmGiftsReceipts _frmGiftsReceipts = new frmGiftsReceipts();
      _frmGiftsReceipts.ShowInTaskbar = false;
      _frmGiftsReceipts.modeOpenBy = EnumOpenBy.Button;
      _frmGiftsReceipts.modeOpen = ((modeEdit == true) ? EnumModeOpen.Edit : EnumModeOpen.Preview);
      _frmGiftsReceipts.Owner = this;
      _frmGiftsReceipts.ShowDialog();
    }


    #endregion

    #region btnCxCAuthorization_Click
    /// <summary>
    /// Despliega el formulario de autorizacion de CxC
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// [michan] 14/Junio/2016 Modified --> Se agrego el formulario correspondiente.
    /// </history>
    private void btnCxCAuthorization_Click(object sender, RoutedEventArgs e)
    {
      // Verificamos si el usuario cuenta con los permisos suficientes
      if (!App.User.HasPermission(EnumPermission.CxCAuthorization, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Exclamation, "CxCAuthorization");
        return;
      }

      // Desplegamos el formulario de autorizacion de CxC
      frmCxCAuthorization _frmCxCAuthorization = new frmCxCAuthorization();
      _frmCxCAuthorization.ShowInTaskbar = false;
      _frmCxCAuthorization.Owner = this;
      _frmCxCAuthorization.ShowDialog();

    }


    #endregion

    #region btnDepositRefund_Click
    /// <summary>
    /// Despliega el formulario de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnDepositRefund_Click(object sender, RoutedEventArgs e)
    {
      frmSearchDepositRefund _frmSearch = new frmSearchDepositRefund();
      _frmSearch.ShowInTaskbar = false;
      _frmSearch.Owner = this;
      _frmSearch.ShowDialog();
    }
    #endregion

    #region btnCloseSalesRoom_Click
    /// <summary>
    /// Función para mostrar el formulario Close sales Room
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 26-02-2016 Created
    /// </history>
    private void btnCloseSalesRoom_Click(object sender, RoutedEventArgs e)
    {
      // Validamos que tenga permiso de lectura de cierre de sala de ventas
      if (!App.User.HasPermission(EnumPermission.CloseSalesRoom, EnumPermisionLevel.ReadOnly)) //  _userData.Permissions.Exists(c => c.pppm == "CLOSESR" && c.pppl >= 1))
      {
        MessageBox.Show("Access denied.", "Close Sales Room");
        return;
      }

      // Mostramos el formulario Close Sales Room
      frmCloseSalesRoom mfrCloseSalesRoom = new frmCloseSalesRoom(this, _dtpServerDate);
      mfrCloseSalesRoom.ShowInTaskbar = false;
      mfrCloseSalesRoom.Owner = this;
      AplicarEfecto(this);
      mfrCloseSalesRoom.ShowDialog();
    }
    #endregion

    #region btnAssistance_Click
    /// <summary>
    /// Despliega el formulario de asistencias
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnAssistance_Click(object sender, RoutedEventArgs e)
    {
      frmAssistance _frmAssistance = new frmAssistance(EnumPlaceType.SalesRoom, App.User);
      _frmAssistance.ShowInTaskbar = false;
      _frmAssistance.Owner = this;
      _frmAssistance.ShowDialog();
    }
    #endregion

    #region btnDaysOff_Click
    /// <summary>
    /// Despliega el formulario de dias de descanso
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnDaysOff_Click(object sender, RoutedEventArgs e)
    {
      frmDaysOff _frmDaysOff = new frmDaysOff(EnumTeamType.TeamSalesmen, App.User);
      _frmDaysOff.ShowInTaskbar = false;
      _frmDaysOff.Owner = this;
      _frmDaysOff.ShowDialog();
    }
    #endregion

    #region btnReport_Click
    /// <summary>
    /// Muestra el diseño del reporte.
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnReport_Click(object sender, RoutedEventArgs e)
    {
      ShowReportDesigner();
    }
    #endregion

    #region btnAbout_Click
    /// <summary>
    /// Llama la ventana de About
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
      var about = new frmAbout();
      about.Owner = this;
      about.ShowDialog();
    }
    #endregion

    #region btnTransfer_Click
    /// <summary>
    /// Permite transferir invitaciones
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnTransfer_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region btnInvitationOutside_Click
    /// <summary>
    /// Permite crear una invitacion outside
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnInvitationOutside_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region btnInvitationInhouse_Click
    /// <summary>
    /// Permite invitar a un huesped inhouse
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnInvitationInhouse_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region btnInvitationExternal_Click
    /// <summary>
    /// Permite crear una nueva invitacion externa
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnInvitationExternal_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region btnGuests_Click
    /// <summary>
    /// Despliega el formulario de busqueda general de invitaciones
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnGuests_Click(object sender, RoutedEventArgs e)
    {
      frmSearchGeneral _frmSearch = new frmSearchGeneral();
      _frmSearch.ShowInTaskbar = false;
      _frmSearch.Owner = this;
      _frmSearch.ShowDialog();
    }
    #endregion

    #region btnSelfGen_Click
    /// <summary>
    /// Despliega el formulario de busqueda casos Self Gen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnSelfGen_Click(object sender, RoutedEventArgs e)
    {
      frmSearchSelfGen _frmSearch = new frmSearchSelfGen();
      _frmSearch.ShowInTaskbar = false;
      _frmSearch.Owner = this;
      _frmSearch.ShowDialog();
    }
    #endregion

    #region btnLogin_Click
    /// <summary>
    /// Llama la ventana de Login
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// [vipacheco] 02/Junio/2016 Modified --> Se agrego funcion de login correspondiente
    /// </history>
    private async void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      frmLogin Login = new frmLogin(null, EnumLoginType.SalesRoom, changePassword: false, autoSign: true, validatePermission: true, permission: EnumPermission.Host, permissionLevel: EnumPermisionLevel.ReadOnly, modeSwitchLoginUser: false);

      await Login.getAllPlaces();
      if (App.User.AutoSign)
      {
        Login.UserData = App.User;
      }
      Login.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      Login.ShowDialog();
      if (Login.IsAuthenticated)
      {
        App.User = Login.UserData;
        Host_Load();
      }
    }
    #endregion

    #endregion

  }
}
