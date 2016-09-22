using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Reports;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Forms;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;

namespace IM.Host
{
  /// <summary>
  /// Interaction logic for frmHost.xaml
  /// </summary>
  public partial class frmHost : Window
  {

    #region Variables
    private DateTime? _dtpCurrent = null;
    public static DateTime dtpServerDate = new DateTime();
    private DataGridCellInfo _currentCell;
    private Guest _guestCurrent;
    private bool _blnUpdateGuest;
    #endregion

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
    public static List<GiftPackageItem> _lstGiftsPacks;
    public static List<RateType> _lstRateType;
    public static List<MealTicketType> _lstMealTicketType;
    public static List<CreditCardType> _lstCreditCardTypes;
    public static List<GuestStatusType> _lstGuestStatusTypes;
    public static List<DisputeStatus> _lstDisputeStatus;
    public static List<PaymentPlace> _lstPaymentPlaces;
    public static Configuration _configuration;
    #endregion

    #region Constructor
    public frmHost()
    {
      InitializeComponent();

      GridHelper.SetUpGrid(grdPremanifestHost, new Guest());
    }
    #endregion

    #region Métodos de controles en el formulario

    #region Window_Loaded
    /// <summary>
    /// Realiza las configuraciones de los controles de la forma
    /// </summary>
    /// <history>
    /// [vipacheco] 09/Feb/2016 Created
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

    #region dtgPremanifestHost_SelectionChanged
    /// <summary>
    /// Funcion que se encarga de validar el total de datos obtenidos en el datagrid
    /// </summary>
    /// <history>
    /// [vipacheco] 03/03/2016 Created
    /// </history>
    private void dtgPremanifestHost_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (grdPremanifestHost.Items.Count == 0)
      {
        StatusBarReg.Content = "No Records";
        return;
      }
      StatusBarReg.Content = string.Format("{0}/{1}", grdPremanifestHost.Items.CurrentPosition + 1, grdPremanifestHost.Items.Count);
    }
    #endregion

    #endregion

    #region Métodos privados

    #region KeyDefault
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
    #endregion

    #region CkeckKeysPress
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

    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
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

    #region ValidateGuest
    /// <summary>
    /// Función encargada de validar que sea un usuario valido con sus respectivos permisos
    /// </summary>
    /// <param name="guest"> Objeto de del Guest </param>
    /// <param name="permission"> Permiso que se requiere validar</param>
    /// <param name="source">Formulario del cual se esta invocando</param>
    /// <returns> true - user valid | false - user no valid </returns>
    /// <history>
    /// [vipacheco] 02/15/2016 Created
    /// [vipacheco] 29/Julio/2016 Modified --> Se cambio el tipo de parametro string por un EnumEntities, para hacer mas entendible el metodo.
    /// </history>
    private bool ValidateGuest(GuestPremanifestHost guest, EnumPermission permission, EnumEntities source)
    {
      if (guest.guBookCanc) // Validamos que no sea un booking cancelado
      {
        UIHelper.ShowMessage("Cancelled booking.", MessageBoxImage.Exclamation);
        return false;
      }
      else if (!Context.User.HasPermission(permission, EnumPermisionLevel.ReadOnly)) // validamos los permisos del usuario - SIN PERMISOS
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Exclamation);
        return false;
      }
      else if (!Context.User.HasPermission(permission, EnumPermisionLevel.Standard)) // PERMISO - Solo Lectura
      {
        if (!guest.guMealTicket && source == EnumEntities.MealTickets)
        {
          UIHelper.ShowMessage("You have read access.", MessageBoxImage.Exclamation);
          return false;
        }
        else if (!guest.guGiftsReceived && source == EnumEntities.GiftsReceipts)
        {
          UIHelper.ShowMessage("You have read access.", MessageBoxImage.Exclamation);
          return false;
        }
        else if (!guest.guShow && source == EnumEntities.Shows)
        {
          UIHelper.ShowMessage("You have read access.", MessageBoxImage.Exclamation);
          return false;
        }
        else if (!guest.guSale && source == EnumEntities.Sales)
        {
          UIHelper.ShowMessage("You have read access.", MessageBoxImage.Exclamation);
          return false;
        }
      }
      else if (guest.guShow == false && (source == EnumEntities.MealTickets || source == EnumEntities.Sales))
      {
        UIHelper.ShowMessage("This option should have a show marked.", MessageBoxImage.Exclamation);
        return false;
      }
      return true;
    }
    #endregion

    #region GetAllCatalogsHost
    /// <summary>
    /// Función que obtiene todos los catalogos utilizados en los combos del modulo Host
    /// </summary>
    /// <history>
    /// [vipacheco] 02/Mayo/2016 Created
    /// [aalcocer] 11/08/2016 Modified. Se agregan mas catalogos, se agregan a una lista de hilos para que sea de manera asíncrona,
    ///   el proceso continua cuando los catalogos ya esten cargados
    /// </history>
    private async Task GetAllCatalogsHost()
    {
      // Obtenemos el id de la sala de ventas.
      string _salesRoom = Context.User.SalesRoom.srID;

      List<Task> _lstTasks = new List<Task>();

      _lstTasks.Add(Task.Run(async () =>
      {
        // Currencies
        _lstCurrencies = await BRCurrencies.GetCurrencies(null, 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        //Payment Types
        _lstPaymentsType = await BRPaymentTypes.GetPaymentTypes(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Marital Status
        _lstMaritalStatus = await BRMaritalStatus.GetMaritalStatus(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Agencies
        _lstAgencies = await BRAgencies.GetAgencies(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Countries
        _lstCountries = await BRCountries.GetCountries(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Languajes
        _lstLanguaje = await BRLanguages.GetLanguages(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Hotels
        _lstHotel = await BRHotels.GetHotels(null, 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Team Sales Men
        _lstTeamSalesMen = await BRTeamsSalesMen.GetTeamsSalesMen(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Personnel
        _lstPersonnel = await BRPersonnel.GetPersonnel("ALL", "ALL", "ALL", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Host (ess) de llegada
        _lstPersonnelHOSTENTRY = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "HOSTENTRY", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Host (ess) de regalos
        _lstPersonnelHOSTGIFTS = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "HOSTGIFTS", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Host (ess) de salida
        _lstPersonnelHOSTEXIT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "HOSTEXIT", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // PR's
        _lstPersonnelPR = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "PR", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Closer´s
        _lstPersonnelCLOSER = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "CLOSER", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Exit Closer´s
        _lstPersonnelCLOSEREXIT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "EXIT", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Podium
        _lstPersonnelPODIUM = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "PODIUM", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Verificador Legal
        _lstPersonnelVLO = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "VLO", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Liner's
        _lstPersonnelLINER = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "LINER", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Gifts
        _lstGifts = await BRGifts.GetGifts(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Banks
        _lstBanks = await BRBanks.GetBanks(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Source Payments
        _lstSourcePayments = await BRSourcePayments.GetSourcePayments(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // SalesRoomShort
        _lstSalesRoom = await BRSalesRooms.GetSalesRooms(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Locations
        _lstLocations = await BRLocations.GetLocations(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Charge To
        _lstChargeTo = await BRChargeTos.GetChargeTos();
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Capitanes de PR's
        _lstPersonnelPRCAPT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "PRCAPT", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Capitanes de Liner's
        _lstPersonnelLINERCAPT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "LINERCAPT", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Capitanes de Closer's
        _lstPersonnelCLOSERCAPT = await BRPersonnel.GetPersonnel("ALL", _salesRoom, "CLOSERCAPT", 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Program's
        _lstPrograms = await BRPrograms.GetPrograms();
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // LeadSources
        _lstLeadSources = await BRLeadSources.GetLeadSources(1, EnumProgram.All);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Refund Types
        _lstRefundTypes = await BRRefundTypes.GetRefundTypes(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // GiftsPacks
        _lstGiftsPacks = await BRGiftsPacks.GetGiftsPacks();
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Tipos de tarifa
        _lstRateType = await BRRateTypes.GetRateTypes(null, 1, true, true);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        //Tipos de cupones de comida.
        _lstMealTicketType = await BRMealTicketTypes.GetMealTicketType();
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        //Credit Card Types
        _lstCreditCardTypes = await BRCreditCardTypes.GetCreditCardTypes(nStatus: 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        //Guest Status Types
        _lstGuestStatusTypes = await BRGuestStatusTypes.GetGuestStatusTypes(nStatus: 1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        //Payment Places
        _lstPaymentPlaces = await BRPaymentPlaces.GetPaymentPlaces(1);
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        //Estatus de disputa
        _lstDisputeStatus = await BRDisputeStatus.GetDisputeStatus();
      }));
      _lstTasks.Add(Task.Run(async () =>
      {
        // Configuracion
        _configuration = (await BRConfiguration.GetConfigurations()).FirstOrDefault();
      }));

      await Task.WhenAll(_lstTasks);
    }
    #endregion

    #region dtpDate_PreviewKeyDown
    /// <summary>
    /// Funcion que evalua cuando se pulsa enter dentro del datepicker
    /// </summary>
    /// <history>
    /// [vipacheco] 26/02/2016 Created
    /// </history>
    private void dtpDate_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        dtpDate_ValueChanged(null, null);
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
      _busyIndicator.IsBusy = true;
      _busyIndicator.BusyContent = "Loading catalogs...";

      // Agregamos la informacion del usuario
      txtUser.Text = Context.User.User.peN.ToString();
      txtSalesRoom.Text = Context.User.SalesRoom.srN.ToString();

      // Obtenemos los catalogos
      await GetAllCatalogsHost();

      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);

      dtpServerDate = BRHelpers.GetServerDate();

      // Actualizamos los tipos de cambio de monedas hasta el dia de hoy
      _busyIndicator.BusyContent = "Updating exchange rate...";
      await BRExchangeRate.InsertExchangeRate(dtpServerDate.Date);

      // Actualizamos las fechas de temporada hasta el año actual
      _busyIndicator.BusyContent = "Updating seasons's dates...";
      await BRSeasons.UpdateSeasonDates(dtpServerDate.Year);

      _busyIndicator.IsBusy = false;
    }
    #endregion

    #region dtpDate_ValueChanged
    /// <summary>
    /// Recarga los resultados del grid principal conforme a la fecha ingresada
    /// </summary>
    /// <history>
    /// [vipacheco] 26/02/2016 Created
    /// </history>
    private void dtpDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (_dtpCurrent != dtpDate.Value)
      {
        if (dtpDate.Value != null)
        {
          // Asignamos la fecha seleccionada.
          _dtpCurrent = dtpDate.Value.Value.Date;
          CollectionViewSource hostInfo = ((CollectionViewSource)(this.FindResource("dsPremanifestHost")));
          hostInfo.Source = BRGuests.GetPremanifestHost(_dtpCurrent, Context.User.SalesRoom.srID);
        }
      }
    }
    #endregion

    #region Metodos de Botones

    #region btnExchangeRate_Click
    /// <summary>
    /// Despliega el formulario de tipos de cambio de monedas
    /// </summary>
    /// <history>
    /// [vipacheco] 04/03/2016 Created
    /// </history>
    private void btnExchangeRate_Click(object sender, RoutedEventArgs e)
    {
      //Verificamos si el usuario cuenta con los permisos suficientes
      if (!Context.User.HasPermission(EnumPermission.ExchangeRates, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("User doesn't have access");
        return;
      }

      var exchange = new frmExchangeRate() { Owner = this };
      exchange.ShowDialog();
    }
    #endregion

    #region btnSales_Click
    /// <summary>
    /// Despliega el formulario de ventas en modo busqueda
    /// </summary>
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// </history>
    private void btnSales_Click(object sender, RoutedEventArgs e)
    {
      var sales = new frmSales(EnumOpenBy.Button) { Owner = this };
      sales.ShowDialog();
    }
    #endregion

    #region btnExcel_Click
    /// <summary>
    /// Despliega el reporte de manifiesto por Lead Source para Excel
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// [edgrodriguez] 22/Junio/2016 Modified. Se agregó el proceso de creación del reporte Manifest By LS
    /// [edgrodriguez] 14/Septiembre/2016 Modified. Se agregó un Try para poder detener el BusyIndicator cuando ocurra un error.
    /// </history>
    private async void btnExcel_Click(object sender, RoutedEventArgs e)
    {
      _busyIndicator.IsBusy = true;
      _busyIndicator.BusyContent = "Loading Manifest";
      try
      {
        var dateRange = DateHelper.DateRange(dtpDate.Value.Value, dtpDate.Value.Value);
        var dateRangeFileName = DateHelper.DateRangeFileName(dtpDate.Value.Value, dtpDate.Value.Value);
        var filters = new List<Tuple<string, string>>();

        var lstManifestRange = await BRReportsBySalesRoom.GetRptManifestRangeByLs(dtpDate.Value, dtpDate.Value, Context.User.SalesRoom.srID);
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
          filters.Add(new Tuple<string, string>("Sales Room", Context.User.SalesRoom.srID));
          var fileinfo = await EpplusHelper.ExportRptManifestRangeByLs(new List<Tuple<DataTable, IM.Model.Classes.ExcelFormatItemsList>> {
        Tuple.Create(dtRptManifest, clsFormatReports.RptManifestRangeByLs()),
        Tuple.Create(dtBookings, clsFormatReports.RptManifestRangeByLs_Bookings())
      }, filters, "Manifest By LS", dateRangeFileName, blnRowGrandTotal: true, blnShowSubtotal: true);

          if (fileinfo != null)
          {
            frmDocumentViewer documentViewer = new frmDocumentViewer(fileinfo, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly), false) { Owner = this };
            documentViewer.Owner = this;
            documentViewer.ShowDialog();
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        _busyIndicator.IsBusy = false;
      }
    }
    #endregion

    #region btnMealTickets_Click
    /// <summary>
    /// Invoca  el formulario Meal Ticket por medio del boton correspondiente.
    /// </summary>
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// </history>
    private void btnMealTickets_Click(object sender, RoutedEventArgs e)
    {
      // Se verifica si el usuario tiene permisos de edicion!
      //bool modeEdit = Context.User.HasPermission(EnumPermission.MealTicket, EnumPermisionLevel.Standard);

      // Se invoca el formulario de acuerdo al permiso del usuario!
      var mealTickets = new frmMealTickets(EnumOpenBy.Button)
      {
        Owner = this,
      };
      mealTickets.ShowDialog();
    }
    #endregion

    #region btnGiftsReceipts_Click
    /// <summary>
    /// Función para madar ejecutar el formulario Gifts Receipts
    /// </summary>
    /// <history>
    /// [vipacheco] 03/17/2016 Created
    /// </history>
    private void btnGiftsReceipts_Click(object sender, RoutedEventArgs e)
    {
      // Se verifica si el usuario tiene permisos de edicion!
      bool modeEdit = Context.User.HasPermission(EnumPermission.MealTicket, EnumPermisionLevel.Standard);

      // Se invoca el formulario de acuerdo al permiso del usuario!
      var giftsReceipts = new frmGiftsReceipts()
      {
        _modeOpenBy = EnumOpenBy.Button,
        _modeOpen = ((modeEdit == true) ? EnumMode.Edit : EnumMode.ReadOnly),
        Owner = this
      };
      giftsReceipts.ShowDialog();
    }
    #endregion

    #region btnCxCAuthorization_Click
    /// <summary>
    /// Despliega el formulario de autorizacion de CxC
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// [michan] 14/Junio/2016 Modified --> Se agrego el formulario correspondiente.
    /// </history>
    private void btnCxCAuthorization_Click(object sender, RoutedEventArgs e)
    {
      // Verificamos si el usuario cuenta con los permisos suficientes
      if (!Context.User.HasPermission(EnumPermission.CxCAuthorization, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("Access denied.", MessageBoxImage.Exclamation, "CxCAuthorization");
        return;
      }

      // Desplegamos el formulario de autorizacion de CxC
      frmCxCAuthorization _frmCxCAuthorization = new frmCxCAuthorization();
      _frmCxCAuthorization.Owner = this;
      _frmCxCAuthorization.ShowDialog();

    }


    #endregion

    #region btnDepositRefund_Click
    /// <summary>
    /// Despliega el formulario de busqueda
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnDepositRefund_Click(object sender, RoutedEventArgs e)
    {
      var depositRefund = new frmSearchDepositRefund() { Owner = this };
      depositRefund.ShowDialog();
    }
    #endregion

    #region btnCloseSalesRoom_Click
    /// <summary>
    /// Función para mostrar el formulario Close sales Room
    /// </summary>
    /// <history>
    /// [vipacheco] 26-02-2016 Created
    /// </history>
    private void btnCloseSalesRoom_Click(object sender, RoutedEventArgs e)
    {
      // Validamos que tenga permiso de lectura de cierre de sala de ventas
      if (!Context.User.HasPermission(EnumPermission.CloseSalesRoom, EnumPermisionLevel.ReadOnly))
      {
        UIHelper.ShowMessage("Access denied.");
        return;
      }

      // Mostramos el formulario Close Sales Room
      var closeSalesRoom = new frmCloseSalesRoom(dtpServerDate) { Owner = this };
      closeSalesRoom.ShowDialog();
    }
    #endregion

    #region btnAssistance_Click
    /// <summary>
    /// Despliega el formulario de asistencias
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnAssistance_Click(object sender, RoutedEventArgs e)
    {
      var assistance = new frmAssistance(EnumPlaceType.SalesRoom, Context.User)
      {
        Owner = this
      }
      .ShowDialog();
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
      var _frmDaysOff = new frmDaysOff(EnumTeamType.TeamSalesmen, Context.User)
      {
        Owner = this
      }
      .ShowDialog();
    }
    #endregion

    #region btnReport_Click
    /// <summary>
    /// Muestra el diseño del reporte.
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// [vipacheco] 29/Julio/2016 Modified --> Se agregó la invocacion del formulario Host Reports
    /// </history>
    private void btnReport_Click(object sender, RoutedEventArgs e)
    {
      var frm = new frmHostReports(dtpDate.Value.Value)
      {
        Owner = this
      }
      .ShowDialog();
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
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private async void btnTransfer_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        frmSearchGeneral frmSearch = new frmSearchGeneral(dtpDate.Value.Value, EnumSearchHostType.Transfer) { Owner = this };
        bool blnResult = (bool)frmSearch.ShowDialog();

        // Verificamos si se debe realizar alguna tranferencia
        if (blnResult == true)
        {
          Guest guest = frmSearch.grdGuest.SelectedItem as Guest;
          guest.gusr = Context.User.SalesRoom.srID;
          guest.guBookCanc = false;

          if (await BRGuests.SaveChangedOfGuest(guest, Context.User.SalesRoom.srHoursDif, Context.User.User.peID) == 0)
          {
            //De no ser así informamos que no se guardo la información por algun motivo
            UIHelper.ShowMessage("There was an error saving the information, consult your system administrator",
              MessageBoxImage.Error, "Information can not keep");

            // TODO: Falta implemetar la creacion de cebes y cecos, esta en fase de prueba actualmente 09/Agosto/2016
            //With mAccountingCode
            //  .Load CStr(lngGuestID)
            //  .SetAccountingCode "MK"
            //End With
          }
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region btnInvitationOutside_Click
    /// <summary>
    /// Permite crear una invitacion outside
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// [vipacheco] 16/Agosto/2016 Modified -> Se agrego la invocacion del formulario Invitacion.
    /// </history>
    private async void btnInvitationOuthouse_Click(object sender, RoutedEventArgs e)
    {
      var login = new frmLogin(loginType: EnumLoginType.SalesRoom, program: EnumProgram.Outhouse, validatePermission: true, permission: EnumPermission.HostInvitations,
                               permissionLevel: EnumPermisionLevel.Standard, switchLoginUserMode: true, invitationMode: true, invitationPlaceId: Context.User.SalesRoom.srID,
                               windowStartupLocation: WindowStartupLocation.CenterScreen);

      if (Context.User.AutoSign)
      {
        login.UserData = Context.User;
      }
      await login.getAllPlaces();
      login.ShowDialog();

      if (login.IsAuthenticated)
      {
        // Verificamos que permisos tiene el usuario
        if (login.UserData.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.Standard))
        {
          var invitacion = new frmInvitation(EnumModule.Host, EnumInvitationType.newOutHouse, login.UserData)
          {
            Owner = this
          };
          invitacion.ShowDialog();

          // si se guardo la invitacion
          if (invitacion.CatObj.Guest.guInvit)
          {
            dtpDate_ValueChanged(null, null);
            // Seleccionamos el row invitado
            if (grdPremanifestHost != null && grdPremanifestHost.Items.Count > 0)
            {
              grdPremanifestHost.SelectedIndex = grdPremanifestHost.Items.Cast<GuestPremanifestHost>().ToList().FindIndex(x => x.guID == invitacion.CatObj.Guest.guID);
            }
          }
        }
        else
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to modify the invitation", MessageBoxImage.Information);
          return;
        }
      }
    }
    #endregion

    #region btnInvitationInhouse_Click
    /// <summary>
    /// Permite invitar a un huesped inhouse
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// [vipacheco] 09/Agosto/2016 Modified -> Se agrego la invocacion del formulario Invitacion.
    /// </history>
    private async void btnInvitationInhouse_Click(object sender, RoutedEventArgs e)
    {
      frmSearchGeneral frmSearch = new frmSearchGeneral(dtpDate.Value.Value, EnumSearchHostType.Invit) { Owner = this };
      bool blnResult = (bool)frmSearch.ShowDialog();

      // Verificamos si se debe realizar alguna tranferencia
      if (blnResult == true)
      {
        Guest guest = frmSearch.grdGuest.SelectedItem as Guest;

        var login = new frmLogin(loginType: EnumLoginType.SalesRoom, program: EnumProgram.Inhouse, validatePermission: true, permission: EnumPermission.HostInvitations,
                                 permissionLevel: EnumPermisionLevel.Standard, switchLoginUserMode: true, invitationMode: true, invitationPlaceId: Context.User.SalesRoom.srID,
                                 windowStartupLocation: WindowStartupLocation.CenterScreen);

        if (Context.User.AutoSign)
        {
          login.UserData = Context.User;
        }
        await login.getAllPlaces();
        login.ShowDialog();

        if (login.IsAuthenticated)
        {
          // Verificamos que permisos tiene el usuario
          if (login.UserData.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.Standard))
          {
            var invitacion = new frmInvitation(EnumModule.Host, EnumInvitationType.existing, login.UserData, guest.guID)
            {
              Owner = this
            };
            invitacion.ShowDialog();

            // si se guardo la invitacion
            if (invitacion.CatObj.Guest.guInvit)
            {
              dtpDate_ValueChanged(null, null);
              // Seleccionamos el row invitado
              if (grdPremanifestHost != null && grdPremanifestHost.Items.Count > 0)
              {
                grdPremanifestHost.SelectedIndex = grdPremanifestHost.Items.Cast<GuestPremanifestHost>().ToList().FindIndex(x => x.guID == invitacion.CatObj.Guest.guID);
              }
            }
          }
          else
          {
            UIHelper.ShowMessage("You do not have sufficient permissions to modify the invitation", MessageBoxImage.Information);
            return;
          }
        }
      }
    }
    #endregion

    #region btnInvitationExternal_Click
    /// <summary>
    /// Permite crear una nueva invitacion externa
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private async void btnInvitationExternal_Click(object sender, RoutedEventArgs e)
    {
      var login = new frmLogin(loginType: EnumLoginType.SalesRoom, program: EnumProgram.Outhouse, validatePermission: true, permission: EnumPermission.HostInvitations,
                               permissionLevel: EnumPermisionLevel.Standard, switchLoginUserMode: true, invitationMode: true, invitationPlaceId: Context.User.SalesRoom.srID,
                               windowStartupLocation: WindowStartupLocation.CenterScreen);

      if (Context.User.AutoSign)
      {
        login.UserData = Context.User;
      }
      await login.getAllPlaces();
      login.ShowDialog();

      if (login.IsAuthenticated)
      {
        // Verificamos que permisos tiene el usuario
        if (login.UserData.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.Standard))
        {
          var invitacion = new frmInvitation(EnumModule.Host, EnumInvitationType.newExternal, login.UserData)
          {
            Owner = this
          };
          invitacion.ShowDialog();

          // si se guardo la invitacion
          if (invitacion.CatObj.Guest.guInvit)
          {
            dtpDate_ValueChanged(null, null);
            // Seleccionamos el row invitado
            if (grdPremanifestHost != null && grdPremanifestHost.Items.Count > 0)
            {
              grdPremanifestHost.SelectedIndex = grdPremanifestHost.Items.Cast<GuestPremanifestHost>().ToList().FindIndex(x => x.guID == invitacion.CatObj.Guest.guID);
            }
          }
        }
        else
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to modify the invitation", MessageBoxImage.Information);
          return;
        }
      }
    }
    #endregion

    #region btnGuests_Click
    /// <summary>
    /// Despliega el formulario de busqueda general de invitaciones
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    private void btnGuests_Click(object sender, RoutedEventArgs e)
    {
      frmSearchGeneral _frmSearch = new frmSearchGeneral(dtpDate.Value.Value) { Owner = this };
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
      frmLogin Login = new frmLogin(null, EnumLoginType.SalesRoom, changePassword: false, autoSign: true, validatePermission: true, permission: EnumPermission.Host,
                                    permissionLevel: EnumPermisionLevel.ReadOnly, switchLoginUserMode: true, windowStartupLocation: WindowStartupLocation.CenterScreen);

      await Login.getAllPlaces();
      if (Context.User.AutoSign)
      {
        Login.UserData = Context.User;
      }
      Login.ShowDialog();
      if (Login.IsAuthenticated)
      {
        Context.User = Login.UserData;
        Host_Load();
      }
    }
    #endregion

    #endregion

    #region Metodos de CheckBox

    #region ShowDepositsRefund_Click
    /// <summary>
    /// Despliega el formulario de depositos devueltos
    /// </summary>
    /// <history>
    /// [vipacheco] 23/Junio/2016 Created
    /// </history>
    private void ShowDepositsRefund_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      CheckBox chkDepositsRefund = sender as CheckBox;
      GuestPremanifestHost guestHost = chkDepositsRefund.DataContext as GuestPremanifestHost;

      frmSearchDepositRefund frmSearchDeposit = new frmSearchDepositRefund(guestHost.guID);
      frmSearchDeposit.Owner = this;
      frmSearchDeposit.ShowDialog();

      // Si se cambio el status
      if (frmSearchDeposit.HasRefund) { chkDepositsRefund.IsChecked = true; }
      else { chkDepositsRefund.IsChecked = false; }
    }
    #endregion

    #region ShowShow_Click
    /// <summary>
    /// Método que despliega el formulario Show
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Abril/2016 Created
    /// </history>
    private void ShowShow_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      CheckBox chkSelected = sender as CheckBox;
      var chekedValue = sender as CheckBox;
      chekedValue.IsChecked = !chekedValue.IsChecked;
      GuestPremanifestHost guest = (GuestPremanifestHost)grdPremanifestHost.SelectedItem;

      //Validamos que sea un invitado valido
      if (ValidateGuest(guest, EnumPermission.Show, EnumEntities.Shows))
      {
        // Desplegamos el formulario Show
        var frmShow = new frmShow(guest.guID) { Owner = this };
        frmShow.ShowDialog();

        ((GuestPremanifestHost) grdPremanifestHost.SelectedItem).guShow = frmShow.GuestShow.Guest.guShow;
        ((GuestPremanifestHost)grdPremanifestHost.SelectedItem).guTour = frmShow.GuestShow.Guest.guTour;
        grdPremanifestHost.Items.Refresh();
      }
    }
    #endregion

    #region ShowMealTickets_Click
    /// <summary>
    /// Invoca el formulario Meal Ticket por medio del CheckBox correspondiente!
    /// </summary>
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// </history>
    private void ShowMealTickets_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      CheckBox chkSelected = sender as CheckBox;
      GuestPremanifestHost guest = chkSelected.DataContext as GuestPremanifestHost;

      //Validamos que sea un invitado valido
      if (ValidateGuest(guest, EnumPermission.MealTicket, EnumEntities.MealTickets))
      {
        // Desplegamos el formulario Show
        frmMealTickets _frmMealTickets = new frmMealTickets(EnumOpenBy.Checkbox, guest.guID) { Owner = this };
        _frmMealTickets.ShowDialog();

        // si existe algun MaalTicket guardado.
        if (_frmMealTickets.HasMealTicket) { chkSelected.IsChecked = true; }
        else { chkSelected.IsChecked = false; }
      }
      else
      {
        chkSelected.IsChecked = false;
      }
    }
    #endregion

    #region ShowSale_Click
    /// <summary>
    /// Despliega el formulario Sales pulsado desde el CheckBox del grid
    /// </summary>
    /// <history>
    /// [vipacheco] 18/03/2016 Created
    /// </history>
    private void ShowSale_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      var row = sender as DataGridRow;
      var chekedValue = sender as CheckBox;
      chekedValue.IsChecked = !chekedValue.IsChecked;
      if (chekedValue.IsChecked == false) return;
      var guestHost = (GuestPremanifestHost)grdPremanifestHost.SelectedItem;

      if (ValidateGuest(guestHost, EnumPermission.Sales, EnumEntities.Sales))
      {
        var frmSales = new frmSales(EnumOpenBy.Checkbox, guestHost.guID) { Owner = this };
        frmSales.ShowDialog();

        if (chekedValue.IsChecked.Value == false && !string.IsNullOrEmpty(frmSales.txtsaID.Text)
          || chekedValue.IsChecked.Value && string.IsNullOrEmpty(frmSales.txtsaID.Text))
        {
          chekedValue.IsChecked = true;
        }
      }
      else
      {
        if (chekedValue != null) { chekedValue.IsChecked = false; }
      }
    }
    #endregion

    #region ShowGiftsReceived_Click
    /// <summary>
    /// Invoca el formulario Gifts Received
    /// </summary>
    /// <history>
    /// [vipacheco] 04/04/2016 Created
    /// </history>
    private void ShowGiftsReceived_Click(object sender, RoutedEventArgs e)
    {
      // Obtenemos el row seleccionado!
      CheckBox chkSelected = sender as CheckBox;
      GuestPremanifestHost guest = chkSelected.DataContext as GuestPremanifestHost;

      //Validamos que sea un invitado valido
      if (ValidateGuest(guest, EnumPermission.GiftsReceipts, EnumEntities.GiftsReceipts))
      {
        bool canEdit = Context.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard);

        frmGiftsReceipts frmGiftsReceipts = new frmGiftsReceipts(guest.guID)
        {
          Owner = this,
          _modeOpenBy = EnumOpenBy.Checkbox,
          _modeOpen = (canEdit) ? EnumMode.Edit : EnumMode.ReadOnly
        };
        frmGiftsReceipts.ShowDialog();

        // si cambio el estatus de regalos recibidos
        if (!string.IsNullOrEmpty(frmGiftsReceipts.txtgrID.Text.Trim()))
        {
          chkSelected.IsChecked = true;
        }
        else
        {
          chkSelected.IsChecked = false;
        }
      }
      else
      {
        chkSelected.IsChecked = false;
      }
    }
    #endregion

    #endregion

    #region dtgPremanifestHost_PreparingCellForEdit
    /// <summary>
    /// Verifica si cuenta con los permisos de edicion
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Created
    /// </history>
    private async void dtgPremanifestHost_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      DataGrid dataGrid = sender as DataGrid;
      GuestPremanifestHost guest = dataGrid.Items.CurrentItem as GuestPremanifestHost;
      _currentCell = grdPremanifestHost.CurrentCell;
      // Verificamos que no exista el guest current
      if (_guestCurrent == null) { _guestCurrent =await  BRGuests.GetGuest(guest.guID,true); }
      else if (_guestCurrent.guID != guest.guID) { _guestCurrent = await BRGuests.GetGuest(guest.guID,true); }

      switch (_currentCell.Column.SortMemberPath)
      {
        case nameof(guest.guShowSeq):
          // si tiene al menos permiso estandar de Hostess
          if (Context.User.HasPermission(EnumPermission.Host, EnumPermisionLevel.Standard)) { stkMenu.IsEnabled = false; }
          else { _currentCell.Column.IsReadOnly = true; }
          break;
        case nameof(guest.guWComments):
          // si tiene al menos permiso estandar de Hostess
          if (Context.User.HasPermission(EnumPermission.Host, EnumPermisionLevel.Standard)) { stkMenu.IsEnabled = false; }
          else { _currentCell.Column.IsReadOnly = true; }
          break;
        case nameof(guest.guTaxiIn):
          // si tiene al menos permiso estandar de Taxi In
          if (Context.User.HasPermission(EnumPermission.TaxiIn, EnumPermisionLevel.Standard)) { stkMenu.IsEnabled = false; }
          else { _currentCell.Column.IsReadOnly = true; }
          break;
      }
    }
    #endregion

    #region grdPremanifestHost_CellEditEnding
    /// <summary>
    /// Verifica si existen cambios de la celda editada.
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Created.
    /// </history>
    private void grdPremanifestHost_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      GuestPremanifestHost guest = e.Row.Item as GuestPremanifestHost;
      switch (_currentCell.Column.SortMemberPath)
      {
        case nameof(guest.guShowSeq):
          stkMenu.IsEnabled = true;
          var txtShow = e.EditingElement as TextBox;
          if (!string.IsNullOrEmpty(txtShow.Text))
          {
            byte cvtResult;
            if (byte.TryParse(txtShow.Text, out cvtResult))
            {
              // Verificamos si cambiaron los datos
              if (_guestCurrent.guShowSeq != guest.guShowSeq)
              {
                _guestCurrent.guShowSeq = cvtResult;
                _blnUpdateGuest = true;
              }
            }
          }
          else
          {
            txtShow.Text = null;
            guest.guShowSeq = null;
          }
          break;
        case nameof(guest.guWComments):
          stkMenu.IsEnabled = true;
          var txtComments = e.EditingElement as TextBox;
          if (!string.IsNullOrEmpty(txtComments.Text))
          {
            if (_guestCurrent.guWComments != guest.guWComments)
            {
              _guestCurrent.guWComments = txtComments.Text;
              _blnUpdateGuest = true;
            }
          }
          break;
        case nameof(guest.guTaxiIn):
          stkMenu.IsEnabled = true;
          var txtTaxiIn = e.EditingElement as TextBox;
          if (!string.IsNullOrEmpty(txtTaxiIn.Text))
          {
            decimal cvtResult;
            if (decimal.TryParse(txtTaxiIn.Text, out cvtResult))
            {
              if (_guestCurrent.guTaxiIn != guest.guTaxiIn)
              {
                _guestCurrent.guTaxiIn = cvtResult;
                _blnUpdateGuest = true;
              }
            }
          }
          break;
      }

      if (_currentCell.Column.IsReadOnly)
      {
        _currentCell.Column.IsReadOnly = false;
      }
    }
    #endregion

    #region grdPremanifestHost_RowEditEnding
    /// <summary>
    /// Actualiza los campos correspondientes en la base de datos.
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Created.
    /// </history>
    private async void grdPremanifestHost_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      // Verificamos si es necesario actualizar el guest
      if (_blnUpdateGuest)
      {
        await BREntities.OperationEntity(_guestCurrent, EnumMode.Edit);
      }
      _blnUpdateGuest = false;
    }
    #endregion

  }
}
