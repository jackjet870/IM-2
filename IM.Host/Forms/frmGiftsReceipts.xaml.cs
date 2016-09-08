using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using PalaceResorts.Common.PalaceTools;
using IM.Host.Classes;
using IM.Services.Helpers;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using IM.Base.Classes;
using System.Data.Entity.Validation;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftsReceipts.xaml
  /// </summary>
  public partial class frmGiftsReceipts : Window, INotifyPropertyChanged
  {
    #region Variables
    private bool HasErrors;
    private DataGridCellInfo _currentCell;
    private frmGiftsReceipts _frmGiftsReceipt;
    private bool _Undo = false;

    private int _chargeToChanged = 0;
    public EnumMode _modeOpen; // Variable para saber si puede ser editado el formulario o solo carga en modo vista (Edit | Preview)
    public EnumOpenBy _modeOpenBy; // Variable para saber de que fuente fue invocado el formulario (Checkbox | boton)
    public EnumMode _modeOpenGrid;  // Tipo de visualizacion del datagrid

    public ObservableCollection<GiftsReceiptsShort> obsGiftsReceipt;
    public ObservableCollection<GiftsReceiptDetail> obsGifts;
    public ObservableCollection<GiftsReceipt> obsGiftsReceiptInfo;
    public ObservableCollection<GiftsReceiptDetail> obsGiftsComplet;
    public ObservableCollection<GiftsReceiptPaymentShort> obsPayments;
    public ObservableCollection<dynamic> obsDeposits;

    public List<GiftsReceiptPaymentShort> _lstPaymentsDelete = new List<GiftsReceiptPaymentShort>();

    public static Dictionary<string, List<GiftsReceiptPackageItem>> lstGiftsPacks = new Dictionary<string, List<GiftsReceiptPackageItem>>();   // Lista que contiene los paquetes de regalos
    public static List<KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>> logGiftDetail = new List<KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>>();  //Varible para guardar los cambios registrados durante el uso de host al momento de guardar con las claves iniciales
    public static bool useCxCCost;  //Variable para la validacion del cobro (Empleado || Publico)

    private int _guestID;
    private DateTime? _dtpClose;
    private bool _newExchangeGiftReceipt = false, _newGiftReceipt = false;
    private short _reimpresion = 0;
    public bool _validateMaxAuthGifts;  // Variable para saber si se valida el maximo autorizado de gifts
    private GuestStatusValidateData _guesStatusInfo;  //Variable que contendra la informacion del guest status info
    private GuestShort _guestShort = null;  //Variable que contendra la infomacion del guest
    private Guest _guest = null;
    private List<ExchangeRateShort> _lstExchangeRate;  //Variable para las tasas de cambio!
    private string _SRCurrency;  //Moneda de la sala de ventas
    #endregion

    #region Propiedades y Eventos Changed
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private GiftsReceipt _giftsReceiptDetail;
    public GiftsReceipt GiftsReceiptDetail
    {
      get { return _giftsReceiptDetail; }
      set
      {
        _giftsReceiptDetail = value;
        OnPropertyChanged("GiftsReceiptDetail");
      }
    }
    #endregion

    #region Constructor
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guestID">Clave del Huesped</param>
    /// <history>
    /// [vipacheco] 20/Abril/2016
    /// </history>
    public frmGiftsReceipts(int guestID = 0)
    {
      GiftsReceiptDetail = new GiftsReceipt() { grct = "Marketing", grcu = "US", grpt = "CS", grcucxcPRDeposit = "US", grcucxcTaxiOut = "US" };
      DataContext = this;

      _frmGiftsReceipt = this;
      // Asignamos el ID del guest
      _guestID = guestID;
      // Obtenemos la fecha de cierre de los recibos de regalos de la sala
      _dtpClose = BRSalesRooms.GetCloseSalesRoom(EnumEntities.GiftsReceipts, App.User.SalesRoom.srID);

      InitializeComponent();
      //UIHelper.SetUpControls(new GiftsReceiptDetail(), tbMain);
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Inicializa y carga los parametros y listas necesarias
    /// </summary>
    /// <history>
    /// [vipacheco] 20/Abril/2016
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      // Cargamos las listas
      Load_ComboBox();

      // Cargamos las fechas
      Load_Date();

      switch (_modeOpenBy)
      {
        case EnumOpenBy.Checkbox:
          Head.Visibility = Visibility.Collapsed;
          AdjustmentControls();
          switch (_modeOpen)
          {
            case EnumMode.Edit:
              await Load_Grid_GiftsReceipt(_guestID);
              break;
            case EnumMode.ReadOnly:
              await Load_Grid_GiftsReceipt(_guestID);
              break;
          }
          break;
        case EnumOpenBy.Button:
          txtgrID.Text = "";
          switch (_modeOpen)
          {
            case EnumMode.Edit:
              Controls_Reading_Mode();
              break;
            case EnumMode.ReadOnly:
              Controls_Reading_Mode();
              break;
          }
          break;
      }

      // si no se maneja promociones de Sistur
      if (!ConfigHelper.GetString("UseSisturPromotions").ToUpper().Equals("TRUE"))
        btnCancelSisturPromotions.Visibility = Visibility.Hidden;
    }
    #endregion

    #region Load_Date
    /// <summary>
    /// Inicializa los campos correspondientes a fechas
    /// </summary>
    /// <history>
    ///  [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private void Load_Date()
    {
      // Asignamos el valor inicial al campo Date
      dtpgrD.Value = frmHost.dtpServerDate.Date;
      // Asignamos el valor inicial al campo To
      dtpTo.Value = frmHost.dtpServerDate;
      // Asignamos el valor inicial al campo From
      dtpCgrDFrom.Value = frmHost.dtpServerDate.AddDays(-7);
    }
    #endregion

    #region Load_ComboBox
    /// <summary>
    /// Carga la lista para los combos utilizados en el formulario
    /// </summary>
    /// <history>
    /// [vipacheco] 27/Mayo/2016 Created
    /// </history>
    private void Load_ComboBox()
    {
      // Ofrecido por
      cmbgrpe.ItemsSource = frmHost._lstPersonnel;

      // Obtenemos Hostess de regalos
      cmbgrHost.ItemsSource = frmHost._lstPersonnelHOSTGIFTS;

      // Obtenemos los Charge To
      CollectionViewSource _dsChargeTo = ((CollectionViewSource)(this.FindResource("dsChargeTo")));
      _dsChargeTo.Source = frmHost._lstChargeTo;

      //Obtenemos los Parments Types
      CollectionViewSource _dsPaymentType = ((CollectionViewSource)(this.FindResource("dsPaymentType")));
      _dsPaymentType.Source = frmHost._lstPaymentsType;

      // Obtenemos las Monedas de la CxC de PR
      CollectionViewSource _dsCurrencyPRDeposit = ((CollectionViewSource)(this.FindResource("dsCurrencyPRDeposit")));
      _dsCurrencyPRDeposit.Source = frmHost._lstCurrencies;

      // Obtenemos las Monedas de la CxC de Taxi Out
      CollectionViewSource _dsCurrencyTaxiOut = ((CollectionViewSource)(this.FindResource("dsCurrencyTaxiOut")));
      _dsCurrencyTaxiOut.Source = frmHost._lstCurrencies;

      // Obtenemos las monedas
      CollectionViewSource _dsCurrency = ((CollectionViewSource)(this.FindResource("dsCurrency")));
      _dsCurrency.Source = frmHost._lstCurrencies;

      // Obtenemos las monedas para los deposits
      CollectionViewSource _dsCurrencyDeposits = ((CollectionViewSource)(this.FindResource("dsCurrencyDeposits")));
      _dsCurrencyDeposits.Source = frmHost._lstCurrencies;

      //Obtenemos los regalos
      CollectionViewSource _dsGifts = ((CollectionViewSource)(this.FindResource("dsGifts")));
      _dsGifts.Source = frmHost._lstGifts;

      // Obtenemos los bancos
      CollectionViewSource _dsBanks = ((CollectionViewSource)(this.FindResource("dsBanks")));
      _dsBanks.Source = frmHost._lstBanks;

      // Obtenemos los Source Payments
      CollectionViewSource _dsSourcePayments = ((CollectionViewSource)(this.FindResource("dsSourcePayments")));
      _dsSourcePayments.Source = frmHost._lstSourcePayments;

      // Obtenemos las salas
      CollectionViewSource _dsSalesRooms = ((CollectionViewSource)(this.FindResource("dsSalesRooms")));
      _dsSalesRooms.Source = frmHost._lstSalesRoom;

      // Obtenemos los hoteles
      CollectionViewSource _dsHotel = ((CollectionViewSource)(this.FindResource("dsHotels")));
      _dsHotel.Source = frmHost._lstHotel;

      // Obtenemos las locaciones
      CollectionViewSource _dsLocations = ((CollectionViewSource)(this.FindResource("dsLocations")));
      _dsLocations.Source = frmHost._lstLocations;
    }
    #endregion

    #region Load_Grid_GiftsReceipt
    /// <summary>
    /// Carga el grid de recibos de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Mayo/2016 Created
    /// </history>
    private async Task Load_Grid_GiftsReceipt(int pGuestID = 0)
    {
      _busyIndicator.IsBusy = true;

      int guest = 0, receipt = 0;
      string salesRoom = "ALL", folio = "ALL", name = "ALL", reservation = "ALL";
      DateTime? dateFrom = null, dateTo = null;

      // Si se esta buscando
      if (pGuestID == 0)
      {
        guest = txtCgrgu.Text != "" ? Convert.ToInt32(txtCgrgu.Text) : guest;
        salesRoom = App.User.SalesRoom.srID;
        receipt = txtCgrID.Text != "" ? Convert.ToInt32(txtCgrID.Text) : receipt;
        folio = txtCgrNum.Text != "" ? txtCgrNum.Text : folio;
        dateFrom = dtpCgrDFrom.Value.Value.Date;
        dateTo = dtpTo.Value.Value.Date;
        name = txtCgrGuest.Text != "" ? txtCgrGuest.Text : name;
        reservation = txtCReservation.Text != "" ? txtCReservation.Text : reservation;
      }
      // Si no se esta buscando
      else
      {
        // deshabilitamos la busqueda
        grbSelectionCriteria.Visibility = Visibility.Hidden;
        brdExchange.Visibility = Visibility.Hidden;

        // Busqueda por Guest ID
        guest = txtCgrgu.Text != "" ? Convert.ToInt32(txtCgrgu.Text) : _guestID;
        txtgrgu.IsReadOnly = true;
      }

      //Ejecutamos el procedimiento almancenado con los criterios de busqueda
      List<GiftsReceiptsShort> lstGiftsReceipt = await BRGiftsReceipts.GetGiftsReceipts(guest, salesRoom, receipt, folio, dateFrom, dateTo, name, reservation);
      // Creamos el observable con la lista resultado
      obsGiftsReceipt = new ObservableCollection<GiftsReceiptsShort>(lstGiftsReceipt);
      // Localizamos el recurso 
      CollectionViewSource dsReceipts = ((CollectionViewSource)(this.FindResource("dsReceipts")));
      // Asigamos el observable creado
      dsReceipts.Source = obsGiftsReceipt;

      // si no hay recibos de regalos
      if (obsGiftsReceipt.Count == 0 && _modeOpenBy != EnumOpenBy.Checkbox)
      {
        btnCancel.Visibility = Visibility.Hidden;
        btnCancelSisturPromotions.Visibility = Visibility.Hidden;
        _busyIndicator.IsBusy = false;
        UIHelper.ShowMessage("Not Found Gift Receipt with input specifications", MessageBoxImage.Information);
        return;
      }
      await Load_Record();

      dtgReceipts.Focus();
      // si es un supervisor, puede modificar la fecha del recibo
      dtpgrD.IsReadOnly = !App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special);
    }
    #endregion

    #region Load_Record
    /// <summary>
    /// Carga un recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private async Task Load_Record()
    {
      // Obtenemos los datos del huesped
      await GetGuestData(txtgrgu.Text != "" ? Convert.ToInt32(txtgrgu.Text) : _guestID);

      // si hay algun recibo de regalos
      if (obsGiftsReceipt != null && obsGiftsReceipt.Count > 0)
      {
        // cargamos el recibo de regalos para modificacion
        await Load_Receipt();

        // Establecemos el monto maximo de regalos
        SetMaxAuthGifts();

        // cargamos los regalos del recibo de regalos
        await Load_Gift_Of_GiftsReceipt(pReceiptID: Convert.ToInt32(txtgrID.Text));
        Controls_Reading_Mode();

        // calculamos el total del cargo
        string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text) + Convert.ToDecimal(txtgrCxCAdj.Text));
      }
      // Si es un recibo de regalos nuevo
      else
      {
        New_GiftReceipt();
        txtgrID.Text = "";
        _newGiftReceipt = false;

        // Cargamos los regalos de la invitacion
        await Load_Gift_Of_GiftsReceipt(pGuestID: _guestID);

        // si no se esta buscando
        if (_guestID > 0)
        {
          Controls_Editing_Mode();

          // calculamos el cargo
          frmCancelExternalProducts _fr = null;
          CalculateCharge(ref _fr);
        }
        else
        {
          Controls_Reading_Mode();
        }
      }
      // Cargamos los tipos de cambio
      LoadExchangeRates();

      // convertimos el deposito a dolares americanos
      ConvertDepositToUsDlls();

      // Caragamos el total de CxC Gifts
      txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text) + Convert.ToDecimal(txtgrCxCAdj.Text));

      // cargamos los depositos
      await Load_Deposits(txtgrgu.Text);

      // cargamos los pagos
      await Load_Payments(txtgrID.Text);

      _busyIndicator.IsBusy = false;

    }
    #endregion

    #region New_GiftReceipt
    /// <summary>
    /// Permite agregar un recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private async void New_GiftReceipt()
    {
      _newGiftReceipt = true;

      // Cargamos la estructura de la tabla de recibo de regalos
      if (_modeOpenBy == EnumOpenBy.Button)
        _guestID = 0;

      txtgrgu.IsReadOnly = false;
      dtpgrD.Value = frmHost.dtpServerDate;
      cmbSalesRoom.SelectedValue = App.User.SalesRoom.srID;

      // si no se esta buscando
      if (_guestID > 0)
      {
        // sugerimos los datos del huesped
        await GetGuestData(_guestID);
        txtgrgu.IsEnabled = false;
        txtgrGuest.IsReadOnly = txtgrGuest2.IsReadOnly = false;
        txtgrCxCComments.Text = txtgrComments.Text = "";
      }
      // Si se esta buscando
      else
      {
        txtgrgu.IsReadOnly = false;
        txtgrGuest.IsReadOnly = txtgrGuest2.IsReadOnly = false;
        txtgrgu.Text = txtgrNum.Text = txtgrls.Text = txtgrMemberNum.Text = txtGuestName.Text = txtReservationCaption.Text = "";
        txtAgencyCaption.Text = txtProgramCaption.Text = txtgrGuest.Text = txtgrGuest2.Text = "";
        txtgrCxCComments.Text = txtgrComments.Text = "";
        txtgrRoomNum.Text = "0";
        txtgrTaxiOut.Text = txtgrPax.Text = txtgrTaxiOutDiff.Text = txtgrCxCGifts.Text = txtgrCxCAdj.Text = txtTotalCxC.Text = txtgrCxCPRDeposit.Text = txtgrCxCTaxiOut.Text = "0.0";
        txtgrDeposit.Text = txtgrDepositTwisted.Text = "0.0";
      }

      txtDepositMN.Text = txtDepositUS.Text = "";

      cmbgrct.SelectedValue = "Marketing";

      // restauramos el valor para corregir calculo de costo para cada nuevo recibo
      useCxCCost = false;

      // establecemos el monto maximo de regalos
      SetMaxAuthGifts();

      // Depositos
      cmbgrcu.SelectedValue = "US";
      cmbgrpt.SelectedValue = "CS";
      cmbgrcuCxCTaxiOut.SelectedValue = "US";
      cmbgrcuCxCPRDeposit.SelectedValue = "US";
      txtDepositUS.Text = string.Format("{0:C2}", 0);
      txtDepositMN.Text = string.Format("{0:C2}", 0);

      // Regalos
      txtTotalCost.Text = string.Format("{0:C2}", 0);
      txtTotalPrice.Text = string.Format("{0:C2}", 0);
      txtTotalCxC.Text = string.Format("{0:C2}", 0);
      txtTotalToPay.Text = string.Format("{0:C2}", 0);
    }
    #endregion

    #region AddReceipt
    /// <summary>
    /// Permite agregar un recibo de regalos
    /// </summary>
    /// <param name="Exchange"></param>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private async Task AddReceipt(bool Exchange = false)
    {
      LoadExchangeRates();
      // Bindiamos la entidad vacia!
      var newGiftReceipt = new GiftsReceipt() { grct = "Marketing", grcu = "US", grWh = App.User.SalesRoom.srID, grpt = "CS", grExchangeRate = Math.Round(_lstExchangeRate.Where(w => w.excu == "MEX").Select(s => s.exExchRate).Single(), 4), grcucxcPRDeposit = "US", grcucxcTaxiOut = "US" };
      if (_guestID > 0)
      {
        newGiftReceipt.grpe = GiftsReceiptDetail.grpe;
        newGiftReceipt.grPax = 1;
        cmbgrHost.SelectedIndex = -1;
      }

      GiftsReceiptDetail = newGiftReceipt;

      New_GiftReceipt();

      // Si es un intercambio de regalos
      if (Exchange)
      {
        brdExchange.Visibility = Visibility.Visible;
        Head.Visibility = Visibility.Visible;
        chkgrExchange.IsChecked = true;
        _newExchangeGiftReceipt = true;
      }

      // cargamos los regalos de la invitacion
      await Load_Gift_Of_GiftsReceipt(pGuestID: _guestID);

      // cargamos los pagos
      await Load_Payments(txtgrID.Text);
      Controls_Editing_Mode();
    }
    #endregion

    #region Controls_Editing_Mode
    /// <summary>
    /// Establece el formulario en modo edicion
    /// </summary>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private void Controls_Editing_Mode()
    {
      bool _Enable = false;

      // Grid receipts
      dtgReceipts.IsEnabled = false;

      // Criterios de busqueda
      grbSelectionCriteria.Visibility = Visibility.Collapsed;

      // Botones
      btnNew.IsEnabled = false;
      btnExchange.IsEnabled = false;
      btnEdit.IsEnabled = false;
      btnPrint.IsEnabled = false;
      btnSave.IsEnabled = true;
      btnUndo.IsEnabled = true;
      btnClose.IsEnabled = false;
      btnCancel.IsEnabled = false;
      btnCancelSisturPromotions.IsEnabled = false;

      // Autentificacion automatica
      if (App.User.AutoSign)
      {
        txtChangedBy.Text = App.User.User.peID;
        txtPwd.Password = App.User.User.pePwd;
      }

      //Determinamos si se puede modificar el recibo
      _Enable = EnableEdit();

      //GuestId 
      txtgrgu.IsEnabled = _Enable;

      // Si no tiene Guest ID, permitimos modificar la localizacion
      cmbgrlo.IsEnabled = _Enable && txtgrgu.Text == "";

      // Hotel
      cmbHotel.IsEnabled = _Enable;

      // Offered by
      cmbgrpe.IsEnabled = _Enable;

      // Gifts Hostess
      cmbgrHost.IsEnabled = _Enable;

      // si no tiene Guest ID o si tiene permiso especial de recibos de regalos,
      // permitimos modificar la sala de ventas
      cmbSalesRoom.IsEnabled = _Enable && (txtgrgu.Text == "" || App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special));

      // Grid de regalos
      dtgGifts.IsReadOnly = !_Enable;
      dtgPayments.IsReadOnly = !_Enable;
      if (_Enable)
        _modeOpenGrid = EnumMode.Edit;
      else
        _modeOpenGrid = EnumMode.EditPartial;

      // Se verifica si es un nuevo Recibo
      if (_newGiftReceipt || _newExchangeGiftReceipt)
      {
        txtgrID.Text = null;

        // Limpiamos la lista de Gifts
        if (obsGifts != null)
        {
          obsGifts.Clear();
          obsGiftsComplet.Clear();
        }

        // Limpiamos la lista de Payments
        if (obsPayments != null) obsPayments.Clear();

        // Si esta en modo busqueda
        if (_guestID == 0)
        {
          // Limpiamos la lista de depositos
          if (obsDeposits != null) obsDeposits.Clear();
        }
      }

      txtgrRoomNum.IsReadOnly = !_Enable;
      txtgrPax.IsReadOnly = !_Enable;
      txtgrTaxiOut.IsReadOnly = !_Enable;
      txtgrTaxiOutDiff.IsReadOnly = !_Enable;
      // Rcpt Number
      txtgrNum.IsReadOnly = !_Enable;

      // Membership
      txtgrMemberNum.IsReadOnly = !_Enable;

      // Fecha
      dtpgrD.IsReadOnly = !_Enable;

      // charge To
      cmbgrct.IsEnabled = _Enable;

      // Comments
      txtgrComments.IsReadOnly = !_Enable;

      //Cxc
      cmbgrct.IsEnabled = _Enable;
      cmbgrcuCxCPRDeposit.IsEnabled = _Enable;
      cmbgrcuCxCTaxiOut.IsEnabled = _Enable;

      txtgrCxCAdj.IsReadOnly = !_Enable;
      txtgrCxCPRDeposit.IsReadOnly = !_Enable;
      txtgrCxCTaxiOut.IsReadOnly = !_Enable;
      txtgrCxCComments.IsReadOnly = !_Enable;
      txtgrDeposit.IsReadOnly = !_Enable;
      txtgrDepositTwisted.IsReadOnly = !_Enable;


      // si no es un recibo de intercambio de regalos, permitimos modificar los depositos y los montos de taxi
      EnableDepositsTaxis((_Enable && chkgrExchange.IsChecked.Value == false));
    }
    #endregion

    #region Controls_Reading_Mode
    /// <summary>
    /// Establece el formulario en modo lectura
    /// </summary>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private void Controls_Reading_Mode()
    {
      // Grid receipts
      dtgReceipts.IsEnabled = true;

      brdExchange.Visibility = chkgrExchange.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
      ControlEnable(false, false, false, false, false, false, true, true, false);

      //controlEditionGifts(Visibility.Hidden, Visibility.Hidden); --> Modificado por efectos de prueba
      dtgGifts.IsReadOnly = true;
      dtgPayments.IsReadOnly = true;

      // Comments
      txtgrComments.IsReadOnly = true;

      // si se esta buscando
      if (_guestID == 0)
        grbSelectionCriteria.Visibility = Visibility = Visibility.Visible;
      else
        grbSelectionCriteria.Visibility = Visibility.Hidden;

      // Botones
      btnNew.IsEnabled = true;
      btnExchange.IsEnabled = true;
      btnSave.IsEnabled = false;
      btnUndo.IsEnabled = false;
      btnClose.IsEnabled = true;

      // Fecha
      dtpgrD.IsReadOnly = true;

      // Si se esta buscando y no se ha cargado ningun recibo
      if (_guestID == 0 && txtgrID.Text == "")
      {
        btnEdit.IsEnabled = false;
        btnPrint.IsEnabled = false;
        btnCancel.IsEnabled = false;
        btnCancelSisturPromotions.IsEnabled = false;
      }
      else
      {
        // Se permite editar e imprimir si el recibo no esta cancelado
        btnEdit.IsEnabled = chkgrCancel.IsChecked.Value ? false : true;
        btnPrint.IsEnabled = chkgrCancel.IsChecked.Value ? false : true;

        // Habilitamos y deshabilitamos el boton de cancelar
        btnCancel.IsEnabled = EnableCancel();
        btnCancelSisturPromotions.IsEnabled = true;
      }

      // si tiene permiso de solo lectura para recibos de regalos
      if (!App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard))
      {
        btnNew.IsEnabled = false;
        btnExchange.IsEnabled = false;
        btnEdit.IsEnabled = false;
        btnCancelSisturPromotions.IsEnabled = false;
      }

      // Autentificacion
      txtChangedBy.Text = "";
      txtPwd.Password = "";

      //Cxc
      cmbgrct.IsEnabled = false;
      cmbgrcuCxCPRDeposit.IsEnabled = false;
      cmbgrcuCxCTaxiOut.IsEnabled = false;
      cmbgrcu.IsEnabled = false;
      cmbgrpt.IsEnabled = false;
      txtgrRoomNum.IsReadOnly = true;
      txtgrPax.IsReadOnly = true;
      txtgrTaxiOut.IsReadOnly = true;
      txtgrTaxiOutDiff.IsReadOnly = true;
      txtgrCxCAdj.IsReadOnly = true;
      txtgrCxCPRDeposit.IsReadOnly = true;
      txtgrCxCTaxiOut.IsReadOnly = true;
      txtgrCxCComments.IsReadOnly = true;
      txtgrDeposit.IsReadOnly = true;
      txtgrDepositTwisted.IsReadOnly = true;
    }
    #endregion

    #region Load_Payments
    /// <summary>
    /// Carga los pagos de un recibo de regalos
    /// </summary>
    /// <param name="GiftReceipt"></param>
    /// <history>
    /// [vipacheco] 28/Abril/2016 Created
    /// </history>
    private async Task Load_Payments(string GiftReceipt)
    {
      // Cargamos los regalos payments
      int GiftReceiptID = GiftReceipt != "" ? Convert.ToInt32(GiftReceipt) : 0;
      List<GiftsReceiptPaymentShort> lstPayments = await BRGiftsReceiptsPayments.GetGiftsReceiptPaymentsShort(GiftReceiptID);
      // Creamos el observable collection
      obsPayments = new ObservableCollection<GiftsReceiptPaymentShort>(lstPayments);
      // Localizamos el recurso
      CollectionViewSource dsPayments = ((CollectionViewSource)(this.FindResource("dsPayments")));
      // Asignamos el resultado
      dsPayments.Source = obsPayments;

      // Calculamos el total de pagos
      CalculateTotalPayments();
    }
    #endregion

    #region Load_Deposits
    /// <summary>
    /// Carga los depositos
    /// </summary>
    /// <param name="GiftReceipt"></param>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private async Task Load_Deposits(string GuestID)
    {
      if (!string.IsNullOrEmpty(GuestID))
      {
        //Cargamos los depositos
        List<BookingDeposit> lstDeposit = await BRBookingDeposits.GetBookingDeposits(Convert.ToInt32(GuestID));
        var _resultLst = lstDeposit.Select(c => new
        {
          bdAmount = c.bdAmount,
          bdReceived = c.bdReceived,
          CxC = c.bdAmount - c.bdReceived,
          bdcu = c.bdcu,
          bdpt = c.bdpt,
          bdpc = c.bdpc,
          bdcc = c.bdcc,
          bdCardNum = c.bdCardNum,
          bdExpD = c.bdExpD,
          bdAuth = c.bdAuth,
          bdFolioCXC = c.bdFolioCXC,
          bdUserCXC = c.bdUserCXC,
          bdEntryDCXC = c.bdEntryDCXC
        }).ToList();

        obsDeposits = new ObservableCollection<dynamic>(_resultLst);
        // Localizamos el recurso
        CollectionViewSource dsBookingDeposit = ((CollectionViewSource)(this.FindResource("dsBookingDeposit")));
        // Asignamos los resultados obtenidos
        dsBookingDeposit.Source = obsDeposits;
      }

    }
    #endregion

    #region Load_Receipt
    /// <summary>
    /// Cargamos el recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 28/Abril/2016 Created
    /// </history>
    private async Task Load_Receipt()
    {
      if (dtgReceipts.SelectedItem == null && dtgReceipts.Items.Count > 0)
        dtgReceipts.SelectedIndex = 0;

      GiftsReceiptsShort selected = dtgReceipts.SelectedItem as GiftsReceiptsShort;
      int GiftReceiptID = selected.grID;
      // Realizamos la consulta con los datos ingresados
      GiftsReceipt giftReceipt = await BRGiftsReceipts.GetGiftReceipt(selected.grID);
      // Obtenemos la cantidad de reimpresiones que tiene realizado este gift
      _reimpresion = giftReceipt.grReimpresion;
      // Asignamos el valor a la propiedad
      GiftsReceiptDetail = giftReceipt;
    }
    #endregion

    #region Load_Gift_Of_GiftsReceipt
    /// <summary>
    /// Carga los regalos de recibos de regalos
    /// </summary>
    /// <param name="ReceiptId"></param>
    /// <param name="pGuestID"></param>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private async Task Load_Gift_Of_GiftsReceipt(int pReceiptID = 0, int pGuestID = 0)
    {
      List<GiftsReceiptDetail> lstGifts = new List<GiftsReceiptDetail>();
      // si es un recibo de regalos nuevo
      if (pReceiptID == 0)
      {
        lstGifts = await Load_Gifts_Of_Invitation(pGuestID);  // cargamos los regalos de la invitacion
      }
      // si es un recibo de regalos existente
      else
        lstGifts = await BRGiftsReceiptDetail.GetGiftsReceiptDetail(pReceiptID);  // cargamos los regalos del recibo de regalos

      // Creamos los Observables
      obsGifts = new ObservableCollection<GiftsReceiptDetail>(lstGifts);
      obsGiftsComplet = new ObservableCollection<GiftsReceiptDetail>(lstGifts);

      // Buscamos los paquetes de regalos
      if (obsGifts.Count > 0)
      {
        lstGiftsPacks.Clear();
        foreach (GiftsReceiptDetail item in obsGifts)
          lstGiftsPacks.Add(item.gegi, await BRGiftsReceiptsPacks.GetGiftsReceiptPackage(pReceiptID, item.gegi));
      }
      // Localizamos el recurso
      CollectionViewSource dsGiftsDetail = ((CollectionViewSource)(this.FindResource("dsGiftsDetail")));

      // Asignamos los Gifts al grid
      dsGiftsDetail.Source = obsGifts;

      // calculamos el monto total de regalos
      Gifts.CalculateTotalGifts(dtgGifts, EnumGiftsType.ReceiptGifts, "geQty", "gegi", "gePriceM", "gePriceMinor", "gePriceAdult", "gePriceA", "gePriceExtraAdult", txtTotalCost, txtTotalPrice, txtTotalToPay, saleField: "geSale");

      // configuramos la informacion de GuestStatus que se validara
      if (_guestID > 0 || txtgrID.Text != "")
      {
        _guesStatusInfo = BRGuestStatus.GetGuestStatusInfo(_guestID, string.IsNullOrEmpty(txtgrID.Text) ? 0 : Convert.ToInt32(txtgrID.Text));
      }
    }
    #endregion

    #region Load_Gifts_Of_Invitation
    /// <summary>
    /// Carga los regalos de la invitacion
    /// </summary>
    /// <param name="GuestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private async Task<List<GiftsReceiptDetail>> Load_Gifts_Of_Invitation(int GuestID)
    {
      List<GiftsReceiptDetail> _lstGiftInvitation = await BRInvitsGifts.GetGiftsInvitationWithoutReceipt(_guestID);

      if (_lstGiftInvitation.Count > 0)
        _lstGiftInvitation.ForEach(_Gift => logGiftDetail.Add(new KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>(Model.Enums.EnumMode.Add, _Gift))); // Guardamos en el log de actividades de GIFTS

      return _lstGiftInvitation;
    }
    #endregion

    #region Metodos del Grid Gifts Receipt
    #region grdReceipts_DoubleClick
    /// <summary>
    /// Función encargada de Cargar la informacion de acuerdo al GIFT RECEIPT seleccionado!
    /// </summary>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private async void grdReceipts_DoubleClick(object sender, RoutedEventArgs e)
    {
      DataGridRow row = sender as DataGridRow;
      GiftsReceiptsShort selected = row.DataContext as GiftsReceiptsShort;

      _busyIndicator.IsBusy = true;
      _busyIndicator.BusyContent = "Loading Gift Receipt - " + selected.grID;

      await Load_Record();
      _busyIndicator.IsBusy = false;

    }
    #endregion

    #region grdReceipts_KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void grdReceipts_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            //grdReceipts_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }
      e.Handled = blnHandled;
    }
    #endregion 
    #endregion

    #region ControlEnable
    /// <summary>
    /// Habilita o Deshabilita los controles correspondientes segun sea el caso.
    /// </summary>
    /// <param name="_chkgrCancel"></param>
    /// <param name="_chkgrExchange"></param>
    /// <param name="_cbogrpe"></param>
    /// <param name="_cbogrHost"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void ControlEnable(bool _chkgrCancel, bool _chkgrExchange, bool _Personnel, bool _Host, bool _Hotel, bool _SalesRooms, bool _Currency, bool _Payment, bool _Location)
    {
      chkgrCancel.IsEnabled = _chkgrCancel;
      chkgrExchange.IsEnabled = _chkgrExchange;
      cmbgrpe.IsEnabled = _Personnel;
      cmbgrHost.IsEnabled = _Host;
      cmbHotel.IsEnabled = _Hotel;
      cmbSalesRoom.IsEnabled = _SalesRooms;
      cmbgrcu.IsEnabled = _Currency;
      cmbgrpt.IsEnabled = _Payment;
      cmbgrlo.IsEnabled = _Location;
    }
    #endregion

    #region SetMaxAuthGifts
    /// <summary>
    /// Establece el monto maximo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 28/Abril/2016 Created
    /// </history>
    private void SetMaxAuthGifts()
    {
      decimal curMaxAuthGifts;
      bool blnWithMaxAuthGifts = false;

      ChargeTo _chargeTo = cmbgrct.SelectedItem as ChargeTo;

      curMaxAuthGifts = CalculateMaxAuthGifts(_chargeTo.ctID, txtgrls.Text, ref blnWithMaxAuthGifts);
      txtgrMaxAuthGifts.Text = string.Format("${0}", curMaxAuthGifts);
      txbgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      txtgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      _validateMaxAuthGifts = blnWithMaxAuthGifts;
    }
    #endregion

    #region CalculateMaxAuthGifts
    /// <summary>
    /// Calcula el monto maximo de regalos
    /// </summary>
    /// <param name="chargeTo"></param>
    /// <param name="leadSource"></param>
    /// <param name="withMaxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    public decimal CalculateMaxAuthGifts(string chargeTo, string leadSource, ref bool withMaxAuthGifts)
    {
      decimal curMaxAuthGifts = 0;
      withMaxAuthGifts = true;

      ChargeTo _chargeTo = cmbgrct.SelectedItem as ChargeTo;

      switch (_chargeTo.ctCalcType)
      {
        //Monto maximo de regalos por Lead Source
        case "A":
          // Si tiene Lead Source
          if (leadSource != "")
          {
            LeadSource _leadSource = BRLeadSources.GetLeadSourceByID(leadSource);
            //si encontro el Lead Source
            if (_leadSource != null)
            {
              curMaxAuthGifts = _leadSource.lsMaxAuthGifts;
            }
          }
          break;
        // Monto maximo de regalos por Guest Status
        case "C":
          GuestStatus _guestsStatus = BRGuestStatus.GetGuestsStatus(_guestID);
          GuestStatusType _guestStatusType = BRGuestStatusTypes.GetGuestStatusTypeByID(_guestsStatus.gtgs);
          curMaxAuthGifts = _guestsStatus.gtQuantity * _guestStatusType.gsMaxAuthGifts;
          break;
        //  Sin monto maximo de regalos
        default:
          curMaxAuthGifts = 0;
          withMaxAuthGifts = false;
          break;
      }
      return curMaxAuthGifts;
    }
    #endregion

    #region GetGuestData
    /// <summary>
    /// Obtiene la informacion de un Guest
    /// </summary>
    /// <param name="guestID"> Identificador del Guest </param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private async Task GetGuestData(int guestID)
    {
      // validamos que se haya enviado una clave de huesped
      if (guestID == 0)
      {
        return;
      }
      // Obtenemos los datos del huesped
      _guestShort = await BRGuests.GetGuestShort(guestID);

      if (_guestShort != null)
      {
        txtgrgu.Text = guestID + "";
        txtGuestName.Text = Common.GetFullName(_guestShort.guLastName1, _guestShort.guFirstName1);
        txtReservationCaption.Text = _guestShort.guHReservID;
        txtAgencyCaption.Text = _guestShort.agN;
        txtProgramCaption.Text = _guestShort.pgN;
        txtgrRoomNum.Text = _guestShort.guRoomNum;

        List<PersonnelShort> lstPersonnel = cmbgrpe.ItemsSource as List<PersonnelShort>;
        int index = lstPersonnel.FindIndex(x => x.peID.Equals(_guestShort.guPRInvit1));
        if (index != -1)
          cmbgrpe.SelectedIndex = index;
        else
          cmbgrpe.SelectedItem = 1;

        // Obtenemos el hotel del Guest
        _guest = await BRGuests.GetGuest(_guestShort.guID);
        txtgrPax.Text = $"{_guest.guPax}";
        txtgrTaxiOut.Text = $"{_guest.guTaxiOut}";
        cmbHotel.SelectedValue = _guest.guHotel;
        // Cargamos su Sales Room
        cmbSalesRoom.SelectedValue = _guestShort.gusr;
        // Cargamos su Location
        cmbgrlo.SelectedValue = _guestShort.guls;
        // Cargamos su Lead Source
        txtgrls.Text = _guestShort.guls;
        // Cargamos el Guest 1
        txtgrGuest.Text = txtGuestName.Text;
      }
      else
      {
        txtGuestName.Text = "";
        txtReservationCaption.Text = "";
        txtAgencyCaption.Text = "";
        txtProgramCaption.Text = "";
      }
    }
    #endregion

    #region CalculateTotalPayments
    /// <summary>
    /// Calcula el total de pagos
    /// </summary>
    /// <history>
    /// [vipacheco] 13/Abril/2016 Created
    /// </history>
    private void CalculateTotalPayments()
    {
      decimal curTotalPaid = 0;
      DateTime dtmReceipt;
      decimal curExchangeRate = 0;

      // Obtenemos la fecha de recibo
      dtmReceipt = dtpgrD.Value.Value.Date;

      // Cargamos los tipos de cambio de la fecha de recibo
      BRExchangeRate.GetExchangeRatesByDate(dtmReceipt);

      // Recorremos los pagos
      foreach (var item in dtgPayments.Items)
      {
        GiftsReceiptPaymentShort _current;
        if (item is GiftsReceiptPaymentShort)
          _current = (GiftsReceiptPaymentShort)item;
        else
          break;
        // Si se ingreso la cantidad pagada
        if (_current.gyAmount >= 0)
        {
          // Localizamos el tipo de cambio
          ExchangeRate _exchangeRate = BRExchangeRate.GetExchangeRateByID(_current.gycu);

          if (_exchangeRate != null)
            curExchangeRate = _exchangeRate.exExchRate;
          else
            curExchangeRate = 1;

          curTotalPaid += _current.gyAmount * curExchangeRate;
        }
      }
      txtTotalPayments.Text = string.Format("{0:C2}", curTotalPaid);
    }
    #endregion

    #region LoadExchangeRates
    /// <summary>
    /// Carga los tipos de cambio
    /// </summary>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private void LoadExchangeRates()
    {
      // Obtenemos la fecha de recibo
      DateTime dtmReceipt = dtpgrD.Value.Value.Date;

      // Cargamos los tipos de cambio de la fecha de recibo
      _lstExchangeRate = BRExchangeRate.GetExchangeRatesByDate(dtmReceipt);

      //Cargamos la moneda
      SalesRoom _salesRoom = BRSalesRooms.GetSalesRoomByID((cmbSalesRoom.SelectedItem as SalesRoomShort).srID);
      _SRCurrency = _salesRoom.srcu;
    }
    #endregion

    #region ConvertDepositToUsDlls
    /// <summary>
    /// Convierte el deposito a dolares americanos
    /// </summary>
    /// <history>
    /// [vipacheco] 22/Abril/2016 Created
    /// </history>
    private void ConvertDepositToUsDlls()
    {
      decimal curDeposit = 0;
      decimal curExchangeRate = 0;
      decimal curExchangeRateMN = 0;

      // Localizamos el tipo de cambio de ese dia de la moneda en que esta el deposito
      if (_lstExchangeRate != null)
      {
        ExchangeRateShort _exchangeRate = _lstExchangeRate.Where(x => x.excu == (cmbgrcu.SelectedItem as Currency).cuID).FirstOrDefault();

        if (_exchangeRate != null)
          curExchangeRate = _exchangeRate.exExchRate;
        else
          curExchangeRate = 1;

        // Localizamos el tipo de cambio de ese dia de la moneda de la sala de ventas
        ExchangeRateShort _exchangeRateCurrency = _lstExchangeRate.Where(x => x.excu == _SRCurrency).FirstOrDefault();

        if (_exchangeRateCurrency != null)
          curExchangeRateMN = _exchangeRateCurrency.exExchRate;
        else
          curExchangeRateMN = 1;

        // Obtenemos el monto del deposito
        if (Convert.ToDecimal(txtgrDepositTwisted.Text) > 0)
          curDeposit = Convert.ToDecimal(txtgrDepositTwisted.Text);

        else if (Convert.ToDecimal(txtgrDeposit.Text) > 0)
          curDeposit = Convert.ToDecimal(txtgrDeposit.Text);
        else
          curDeposit = 0;

        // Monto en dolares
        txtDepositUS.Text = string.Format("{0:0.00}", Math.Round(curDeposit, 4) * Math.Round(curExchangeRate, 4));

        // Monto en moneda nacional
        txtDepositMN.Text = string.Format("{0:0.00}", (Math.Round(curDeposit, 4) * Math.Round(curExchangeRate, 4)) / Math.Round(curExchangeRateMN, 4));
      }
    }
    #endregion

    #region cbogrct_SelectionChanged
    /// <summary>
    /// Actualiza los precios de acuerdo al tipo de charge to
    /// </summary>
    /// <history>
    /// [vipacheco] 22/Abril/2016 Created
    /// </history>
    private void cbogrct_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (_chargeToChanged >= 1) // Esta validación se realiza para evitar el bug de doble changed del control
      {
        ChargeTo _ChargeTo = cmbgrct.SelectedItem as ChargeTo;

        if (_ChargeTo != null)
        {
          switch (_ChargeTo.ctID)
          {
            case "PR":
            case "LINER":
            case "CLOSER":
              useCxCCost = true;
              break;
            default:
              useCxCCost = false;
              break;
          }
        }
        frmCancelExternalProducts _frm = null;
        CalculateCharge(ref _frm);
      }
      _chargeToChanged++; // Para el manejador del bug del doble changed del control
    }
    #endregion

    #region btnClose_Click
    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 09/Abril/2016 Created
    /// </history>
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      // Liberamos las variables estaticas
      logGiftDetail.Clear();
      lstGiftsPacks.Clear();

      Close();
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Busca los Gifts Receipt con los criterios ingresados
    /// </summary>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private async void btnSearch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (ValidateCriteria())
      {
        await Load_Grid_GiftsReceipt();
      }
    }
    #endregion

    #region ValidateCriteria
    /// <summary>
    /// Valida los criterios de busqueda
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private bool ValidateCriteria()
    {
      bool blnValid = true;

      if (dtpCgrDFrom.Value == null)
      {
        UIHelper.ShowMessage("Specify the Start Date.", MessageBoxImage.Information);
        blnValid = false;
      }
      else if (dtpTo.Value == null)
      {
        UIHelper.ShowMessage("Specify the End Date.", MessageBoxImage.Information);
        blnValid = false;
      }
      else if (dtpCgrDFrom.Value.Value > dtpTo.Value.Value)
      {
        UIHelper.ShowMessage("Start Date can not be greater than End Date.", MessageBoxImage.Information);
        blnValid = false;
      }

      return blnValid;
    }
    #endregion

    #region Decimal_PreviewTextInput
    /// <summary>
    /// Valida que el texto introducido sea decimal
    /// </summary>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void Decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyDecimals(e.Text, sender as TextBox);
    }
    #endregion

    #region ValidateEnter_PreviewKeyDown
    /// <summary>
    /// Recalcula el deposito despues de un cambio en los textbox
    /// </summary>
    /// <history>
    /// [vipacheco] 15/Abril/2016 Created
    /// </history>
    private void ValidateEnter_PreviewKeyDown(object sender, KeyEventArgs e)

    {
      TextBox _controlSelected = sender as TextBox;

      if (e.Key == Key.Enter || e.Key == Key.Tab)
      {
        // convertimos el deposito a dolares americanos
        ConvertDepositToUsDlls();

        // si tiene los dos depositos, pone el deposito quemado en cero
        if (Convert.ToDecimal(txtgrDeposit.Text) > 0 && Convert.ToDecimal(txtgrDepositTwisted.Text) > 0)
        {
          if (_controlSelected.Name == "txtgrDeposit")
            txtgrDepositTwisted.Text = string.Format("{0:0.00}", 0);

          if (_controlSelected.Name == "txtgrDepositTwisted")
            txtgrDeposit.Text = string.Format("{0:0.00}", 0);
        }
      }
    }
    #endregion

    #region cbogrcu_SelectionChanged
    /// <summary>
    /// Metodo Evento cambio de combo currency
    /// </summary>
    /// <history>
    /// [vipacheco] 15/Abril/2016 Created
    /// </history>
    private void cbogrcu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // convertimos el deposito a dolares americanos
      ConvertDepositToUsDlls();
    }
    #endregion

    #region txtgrCxCAdj_PreviewKeyDown
    /// <summary>
    /// Actualiza el total de cXc
    /// </summary>
    /// <history>
    /// [vipacheco]  22/Abril/2016 Created
    /// </history>
    private void txtgrCxCAdj_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter || e.Key == Key.Tab)
      {
        txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text) + Convert.ToDecimal(txtgrCxCAdj.Text));
      }
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Permite cancelar un recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private async void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (UIHelper.ShowMessage("Are you sure you want to cancel this receipt? \r\n This change can not be undone.", MessageBoxImage.Question) == MessageBoxResult.Yes)
      {
        // si no tiene regalos pendientes por cancelar en los sistemas externos
        if (await CancelExternalProducts())
        {
          //cancelamos el recibo de regalos
          Cancel();
        }
      }
    }
    #endregion

    #region Cancel
    /// <summary>
    /// Cancela un recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 14/Mayo/2016 Created
    /// </history>
    private async void Cancel()
    {
      int ReceiptID = Convert.ToInt32(txtgrID.Text);
      // Cancelamos el recibo
      BRGiftsReceipts.CancelGiftsReceipt(ReceiptID, frmHost.dtpServerDate);

      // Guardamos el historico del recibos de regalos
      await BRGiftsReceiptLog.SaveGiftsReceiptsLog(ReceiptID, App.User.User.peID);

      // Actualizamos los datos en pantalla
      chkgrCancel.IsChecked = true;
      txtgrCancelD.Text = $"{frmHost.dtpServerDate.Month}/{frmHost.dtpServerDate.Day}/{frmHost.dtpServerDate.Year}";

      Controls_Reading_Mode();
    }
    #endregion

    #region CancelExternalProducts
    /// <summary>
    /// Cancela los productos de sistemas externos
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private async Task<bool> CancelExternalProducts()
    {
      int rowCountgeInElectronicPurse = 0, rowcountgeCancelElectronicPurse = 0, rowCountgeInPVPPromo = 0, rowCountgeCancelPVPPromo = 0;
      foreach (GiftsReceiptDetail item in dtgGifts.Items)
      {
        rowCountgeInElectronicPurse += Convert.ToInt32(item.geInElectronicPurse);
        rowcountgeCancelElectronicPurse += Convert.ToInt32(item.geCancelElectronicPurse);
        rowCountgeInPVPPromo += Convert.ToInt32(item.geInPVPPromo);
        rowCountgeCancelPVPPromo += Convert.ToInt32(item.geCancelPVPPromo);
      }
      // si tiene regalos pendientes por cancelar en el monedero electronico
      if (rowCountgeInElectronicPurse > rowcountgeCancelElectronicPurse)
      {
        // Desplegamos el formulario para cancelar los productos del monedero electronico
        if (await ShowCancelExternalProducts(EnumExternalProduct.expElectronicPurse, false) == MessageBoxResult.OK)
          return await CancelExternalProducts();

      }
      // Si tiene regalos pendientes por cancelar en Sistur
      else if (rowCountgeInPVPPromo > rowCountgeCancelPVPPromo)
      {
        if (await ShowCancelExternalProducts(EnumExternalProduct.expSisturPromotions, false) == MessageBoxResult.OK)
          return await CancelExternalProducts();
      }
      // si no tiene regalos pendientes por cancelar en los sistemas externos
      else
        return true;

      return false;
    }

    #endregion

    #region btnCancelSisturPromotions_Click
    /// <summary>
    /// Despliega el formulario de cancelacion de promociones de Sistur
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private async void btnCancelSisturPromotions_Click(object sender, RoutedEventArgs e)
    {
      // si los datos son validos
      if (ValidateCancelSisturPromotions())
      {
        // desplegamos el formulario de cancelacion de productos externos
        await ShowCancelExternalProducts(EnumExternalProduct.expSisturPromotions, true);
      }
    }
    #endregion

    #region ValidateCancelSisturPromotions
    /// <summary>
    /// Valida si se permite cancelar los regalos con promociones relacionadas
    /// </summary>
    /// <returns> True- Valido | False - No valido </returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private bool ValidateCancelSisturPromotions()
    {
      int rowCountgeInPVPPromo = 0, rowCountgeCancelPVPPromo = 0;
      foreach (GiftsReceiptDetail item in dtgGifts.Items)
      {
        rowCountgeInPVPPromo += Convert.ToInt32(item.geInPVPPromo);
        rowCountgeCancelPVPPromo += Convert.ToInt32(item.geCancelPVPPromo);
      }

      // validamos si tiene permiso especial de recibos de regalos
      if (!App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special))
      {
        UIHelper.ShowMessage("Access denied", MessageBoxImage.Exclamation);
        return false;
      }
      // validamos si tiene regalos de promociones de Sistur que no estan cancelados
      else if (rowCountgeInPVPPromo == rowCountgeCancelPVPPromo)
      {
        UIHelper.ShowMessage("No gifts to cancel from the Sistur promotion.", MessageBoxImage.Exclamation);
        return false;
      }
      return true;
    }
    #endregion

    #region btnLog_Click
    /// <summary>
    /// Despliega el historico de un recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      if (txtgrID.Text != "")
      {
        frmGiftsReceiptsLog frmLog = new frmGiftsReceiptsLog(Convert.ToInt32(txtgrID.Text)) { Owner = this };
        frmLog.ShowDialog();
      }
      else
        UIHelper.ShowMessage("Select a receipt", MessageBoxImage.Information);
    }
    #endregion

    #region btnExchange_Click
    /// <summary>
    /// Despliega el formulario del monedero electronico
    /// </summary>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private async void btnExchange_Click(object sender, RoutedEventArgs e)
    {
      await AddReceipt(true);
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    ///  Permite Guardar los cambios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (Validate())
      {
        _busyIndicator.IsBusy = true;
        _busyIndicator.BusyContent = "Saving Gifts Receipt";

        try
        {
          await Save();
        }
        catch (Exception ex)
        {
          UIHelper.ShowMessage(UIHelper.GetMessageError(ex), MessageBoxImage.Error, "Saving Gifts Receipt");
        }

        await Load_Record();

        _busyIndicator.IsBusy = false;
      }
    }
    #endregion

    #region Save
    /// <summary>
    /// Guarda los datos del recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Mayo/2016
    /// </history>
    private async Task Save()
    {
      int receiptID = 0;

      // Calculamos el cargo
      frmCancelExternalProducts _frmNull = null;
      CalculateCharge(ref _frmNull);
      //// Verificamos si es un Gift Receipt nuevo!
      if (string.IsNullOrEmpty(txtgrID.Text))
      {
        // Guardamos el GiftReceipt en la BD
        GiftsReceiptDetail.grWh = App.User.SalesRoom.srID;
        GiftsReceiptDetail.grExchangeRate = Math.Round(_lstExchangeRate.Where(w => w.excu == "MEX").Select(s => s.exExchRate).Single(), 4);
        receiptID = await BRGiftsReceipts.SaveGiftReceipt(GiftsReceiptDetail);
        // Asignamos el ID al campo.
        txtgrID.Text = $"{receiptID}";
        // Agregamos el recibo de regalos al grid
        GiftsReceiptsShort giftReceiptShort = new GiftsReceiptsShort() { grID = receiptID, grNum = txtgrNum.Text, grExchange = chkgrExchange.IsChecked.Value };
        obsGiftsReceipt.Add(giftReceiptShort);

        #region GiftsReceiptsAdditional
        // Cargamos los datos del huesped
        Guest guest = await BRGuests.GetGuest(Convert.ToInt32(txtgrgu.Text), true);

        if (guest.guQuinella && !guest.guGiftsReceived && App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard))
        {
          // esplegamos el formulario de generacion de recibos de regalos
          frmGiftsReceiptsAdditional _frmGifsAdditional = new frmGiftsReceiptsAdditional(this, Convert.ToInt32(txtgrgu.Text)) { Owner = this };
          _frmGifsAdditional.ShowDialog();
        }
        #endregion
      }
      // si es un recibo existente
      else
      {
        await BREntities.OperationEntity(GiftsReceiptDetail, EnumMode.Edit);
        receiptID = Convert.ToInt32(txtgrID.Text);
      }

      bool statusReceipt = (_newExchangeGiftReceipt == true || _newGiftReceipt == true) ? true : false;

      // Guardamos los regalos
      await ReceiptsGifts.Save(obsGifts, receiptID, Convert.ToInt32(txtgrgu.Text), _newExchangeGiftReceipt);

      // Guardamos los pagos
      await GiftsReceiptsPayments.SavePayments(receiptID, dtgPayments, (_newExchangeGiftReceipt || _newGiftReceipt), _lstPaymentsDelete);

      // Si el recibo no esta cancelado ni cerrado
      if (!chkgrCancel.IsChecked.Value)
      {
        // Si no se esta buscando
        if (_guestID > 0)
        {
          // Actualizamos los datos de la invitacion
          await UpdateGuest();
        }
      }

      // Guardamos el historico de recibo de regalos
      await BRGiftsReceiptLog.SaveGiftsReceiptsLog(Convert.ToInt32(txtgrID.Text), txtChangedBy.Text);

      // si se manejan promociones de sistur
      if (ConfigHelper.GetString("UseSisturPromotions").ToUpper().Equals("TRUE"))
      {
        // Guardamos las promociones de Sistur
        string msjSavePromotionsSistur = await SisturHelper.SavePromotionsSistur(receiptID, txtChangedBy.Text, App.User.User.peID);
        if (!string.IsNullOrWhiteSpace(msjSavePromotionsSistur))
          UIHelper.ShowMessage(msjSavePromotionsSistur, MessageBoxImage.Information, "Save Promotions Sistur");
      }

      // si se maneja cargos a habitacion en Opera
      if (ConfigHelper.GetString("UseRoomCharges").ToUpper().Equals("TRUE"))
      {
        // guardamos los cargos a habitacion en Opera
        string msjGiftsReceipt = WirePRHelper.SaveRoomChargesOpera(receiptID, txtChangedBy.Text);
        if (!string.IsNullOrWhiteSpace(msjGiftsReceipt))
          UIHelper.ShowMessage(msjGiftsReceipt, MessageBoxImage.Information, "Gifts Receipt");
      }

      // si se maneja promociones de Opera
      if (ConfigHelper.GetString("UsePromotions").ToUpper().Equals("TRUE"))
      {
        // guardamos las promociones de Opera
        SavePromotionsOpera(receiptID);
      }

      brdExchange.Visibility = Visibility.Hidden;

      // Actualizamos las banderas
      _newExchangeGiftReceipt = false;
      _newGiftReceipt = false;
      // Reiniciamos las variable globales
      logGiftDetail.Clear();
      _lstPaymentsDelete.Clear();

      Controls_Reading_Mode();
    }
    #endregion

    #region UpdateGuest
    /// <summary>
    /// Actualiza los datos de la invitacion
    /// </summary>
    /// <history>
    /// [vipacheco] 09/Mayo/2016 Created
    /// [jorcanche] 06072016 se agrego asincronia en del GetGuest
    /// </history>
    private async Task UpdateGuest()
    {
      bool Update = false;
      Guest guest = await BRGuests.GetGuest(_guestID);

      //Actualizamos la clave del recibo de deposito
      if (guest.guDepositgr == null && Convert.ToDecimal(txtgrDeposit.Text) != 0)
      {
        guest.guDepositgr = Convert.ToInt32(txtgrID.Text);
        Update = true;
      }

      // Actualizamos la clave del recibo de servicio de taxi de llegada
      if (guest.guTaxiIngr == null)
      {
        guest.guTaxiIngr = Convert.ToInt32(txtgrID.Text);
        Update = true;
      }

      // Actualizamos la clave del recibo de servicio de taxi de salida
      if (guest.guTaxiOutgr == null && Convert.ToDecimal(txtgrTaxiOut.Text) != 0)
      {
        guest.guTaxiOutgr = Convert.ToInt32(txtgrID.Text);
        Update = true;
      }

      // Indica que ya se recibieron los regalos
      if (!guest.guGiftsReceived)
      {
        guest.guGiftsReceived = true;
        Update = true;
      }

      // Actualizamos en la base de datos
      if (Update)
        await BREntities.OperationEntity(guest, Model.Enums.EnumMode.Edit);

    }
    #endregion

    #region Validate
    /// <summary>
    /// Valida que todos los datos sean correctos!
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 07/Mayo/2016 Created
    /// </history>
    private bool Validate()
    {
      // Verificamos que ningun Grid este en modo edicion
      if (GridHelper.IsInEditMode(dtgGifts))
      {
        UIHelper.ShowMessage("Gifts in edition mode or incomplete", MessageBoxImage.Information);
        return false;
      }
      // validamos quien hizo el cambio
      if (!ValidateChangedBy(txtChangedBy, txtPwd))
        return false;
      // validamos los datos generales
      else if (!ValidateGeneral())
        return false;

      return true;
    }
    #endregion

    #region ValidateChangedBy
    /// <summary>
    /// Valida que se ingrese quien hizo el cambio y su contraseña
    /// </summary>
    /// <param name="changeBy"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private bool ValidateChangedBy(TextBox changeBy, PasswordBox password)
    {
      if (!ValidateHelper.ValidateChangedBy(changeBy, password, (changeBy.Text != "") ? "PR" : ""))
        return false;

      // Se consulta con la informacion ingresada.
      ValidationData _validate = BRHelpers.ValidateChangedByExist(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), App.User.SalesRoom.srID).Single();

      // Se verifica si se encontro algun resultado.
      if (_validate.Focus != "" && _validate.Message != "")
        return false;
      else
        return true;
    }
    #endregion

    #region ValidateExist
    /// <summary>
    /// Valida que los datos de un recibo de regalos existan
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private bool ValidateExist()
    {
      ValidationData _validation = BRGiftsReceipts.ValidateGiftsReceipt(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), Convert.ToInt32(txtgrgu.Text), cmbgrlo.SelectedValue.ToString(),
                                                                        (cmbSalesRoom.SelectedItem as SalesRoomShort).srID, (cmbgrHost.SelectedItem as PersonnelShort).peID,
                                                                        (cmbgrpe.SelectedItem as PersonnelShort).peID);
      // si hay un error
      if (_validation.Focus != "")
      {
        // desplegamos el mensaje de error
        UIHelper.ShowMessage(_validation.Message, MessageBoxImage.Information);

        // establecemos el foco en el control que tiene el error
        switch (_validation.Focus)
        {
          case "ChangedBy":
            txtChangedBy.Focus();
            break;
          case "Password":
            txtPwd.Focus();
            break;
          case "Guest":
            txtgrgu.Focus();
            break;
          case "SalesRoom":
            cmbSalesRoom.Focus();
            break;
          case "Location":
            cmbgrlo.Focus();
            break;
          case "Personnel":
            cmbgrpe.Focus();
            break;
          case "GiftsHost":
            cmbgrHost.Focus();
            break;
        }
        return false;
      }
      return true;
    }
    #endregion

    #region ValidateGeneral
    /// <summary>
    /// Realiza las validaciones de monto maximo de regalos y total de importe a cubrir
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private bool ValidateGeneral()
    {
      int guest = 0;
      // Validamos los campos obligatorios
      string strmsj = ValidateHelper.ValidateForm(General, "");
      if (!string.IsNullOrEmpty(strmsj))
      {
        UIHelper.ShowMessage(strmsj, MessageBoxImage.Information, "Gifts Receipt");
        tbMain.SelectedIndex = 0;
        return false;
      }
      // validamos la fecha del recibo de regalos no este en una fecha cerrada
      else if (!Common.ValidateCloseDate(EnumEntities.GiftsReceipts, ref dtpgrD, (DateTime)_dtpClose))
      {
        tbMain.SelectedIndex = 0;
        return false;
      }
      // validamos quien ofrecio los regalos
      else if (cmbgrpe == null || cmbgrpe.SelectedValue == null)
      {
        UIHelper.ShowMessage("Who offered the gifts?", MessageBoxImage.Information);
        tbMain.SelectedIndex = 0;
        cmbgrpe.Focus();
        return false;
      }
      // Validamos los regalos
      else if (!ReceiptsGifts.Validate(obsGifts, _validateMaxAuthGifts, _guesStatusInfo, txtTotalCost.Text, txtgrMaxAuthGifts.Text, dtgGifts))
      {
        return false;
      }
      // validamos los pagos
      else if (!GiftsReceiptsPayments.Validate(dtgPayments))
      {
        return false;
      }
      // validamos que los pagos cubran el importe a pagar
      else if (Convert.ToDecimal(txtTotalToPay.Text.TrimStart('$')) > 0 && Convert.ToDecimal(txtTotalToPay.Text.TrimStart('$')) > Convert.ToDecimal(txtTotalPayments.Text.TrimStart('$')))
      {
        UIHelper.ShowMessage("The payments do not cover the amount to pay", MessageBoxImage.Exclamation);
        return false;
      }

      // validamos que los datos del recibo de regalos existan
      else if (!ValidateExist())
        return false;

      // Validamos que el GuestID no sea 0  y que exista
      else if (int.TryParse(txtgrgu.Text, out guest))
      {
        if (guest == 0)
        {
          UIHelper.ShowMessage("Guest ID not found", MessageBoxImage.Information);
          return false;
        }
      }
      else
      {
        UIHelper.ShowMessage("Guest ID not found", MessageBoxImage.Information);
        return false;
      }

      return true;
    }
    #endregion

    #region GiftsPacks_Expanded
    /// <summary>
    /// Despliega los gifts del paquete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// vipacheco] 25/Abril/2016 Created
    /// </history>
    private void GiftsPacks_Expanded(object sender, RoutedEventArgs e)
    {
      for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
        if (vis is DataGridRow)
        {
          var row = (DataGridRow)vis;
          GiftsReceiptDetail _GiftDetail = row.DataContext as GiftsReceiptDetail;

          if (_GiftDetail == null) return;
          // Buscamos el regalo seleccionado
          Gift _Gift = frmHost._lstGifts.Where(x => x.giID == _GiftDetail.gegi).Single();

          // Verificamos si tiene paquete de regalos
          if (_Gift.giPack)
          {
            // Buscamos los regalos del paquete
            var packs = frmHost._lstGiftsPacks.Where(x => x.gpPack == _Gift.giID).ToList();
            var giftsPacks = packs.Select(x => new GiftsReceiptPackageItem { gkQty = _GiftDetail.geQty, gkgi = frmHost._lstGifts.Where(w => w.giID == x.gpgi).Select(s => s.giN).First() }).ToList();
            List<GiftsReceiptPackageItem> lstResult = giftsPacks;

            row.DetailsVisibility = Visibility.Visible;
            // Localizamos el recurso
            CollectionViewSource dsGiftsReceiptPackage = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptPackage")));
            // Asignamos el valor a la lista
            dsGiftsReceiptPackage.Source = lstResult;
          }

          break;
        }
    }
    #endregion

    #region GiftsPacks_Collapsed
    /// <summary>
    /// Colapsa los gifts de regalo del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 25/Abril/2016 Created
    /// </history>
    private void GiftsPacks_Collapsed(object sender, RoutedEventArgs e)
    {
      for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
        if (vis is DataGridRow)
        {
          var row = (DataGridRow)vis;
          CollectionViewSource dsGiftsReceiptPackage = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptPackage")));
          dsGiftsReceiptPackage.Source = null;

          row.DetailsVisibility = Visibility.Collapsed; //row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
          break;
        }
    }
    #endregion

    #region btnEdit_Click
    /// <summary>
    /// Habilita | Deshabilita los controles correspondientes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      Controls_Editing_Mode();
    }
    #endregion

    #region EnableDepositsTaxis
    /// <summary>
    /// Determina si se puede editar los depositos y los montos de taxi
    /// </summary>
    /// <param name="pblnEnable"></param>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private void EnableDepositsTaxis(bool pblnEnable)
    {
      // Depositos
      txtgrDeposit.IsEnabled = pblnEnable;
      txtgrDepositTwisted.IsEnabled = pblnEnable;
      cmbgrcu.IsEnabled = pblnEnable;
      cmbgrpt.IsEnabled = pblnEnable;

      //Taxis
      txtgrTaxiOut.IsEnabled = pblnEnable;
    }
    #endregion

    #region EnableEdit
    /// <summary>
    /// Determina si se debe permitir modificar los datos de un recibo de regalos
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private bool EnableEdit()
    {
      // si el recibo es de una fecha cerrada
      if (dtpgrD.Value.Value.Date <= _dtpClose)
        return false;

      // si el recibo no es nuevo
      else if (txtgrID.Text != "")
      {
        // si el recibo es de una fecha anterior o igual a la fecha de cierre
        if (dtpgrD.Value.Value.Date <= _dtpClose)
          return false;

        // si el recibo es de una fecha posterior a la fecha de cierre
        else
        {
          // si el recibo no es de hoy y no tiene permiso especial de recibos de regalos
          if (dtpgrD.Value.Value.Date != frmHost.dtpServerDate && !App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special))//  
            return false;
        }
      }
      return true;
    }
    #endregion

    #region btnNew_Click
    /// <summary>
    /// Crea un nuevo Gift Receipt
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private async void btnNew_Click(object sender, RoutedEventArgs e)
    {
      await AddReceipt();
    }
    #endregion

    #region btnUndo_Click
    /// <summary>
    /// Cancela alguna edicion en proceso
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private async void btnUndo_Click(object sender, RoutedEventArgs e)
    {
      // Si no hay recibo de regalos
      if (dtgReceipts.Items.Count == 0)
      {
        // Si no se esta buscando
        if (_guestID > 0 && _modeOpenBy != EnumOpenBy.Button)
        {
          _Undo = true;
          Close();
        }
        else
        {
          brdExchange.Visibility = Visibility.Collapsed;
          Head.Visibility = Visibility.Visible;
          await Load_Record();
          _newExchangeGiftReceipt = false;
          if (chkgrExchange.IsChecked.Value == true)
            chkgrExchange.IsChecked = false;
        }
      }
      // Si hay recibos de regalos
      else
      {
        if (_guestID > 0)
        {
          Head.Visibility = Visibility.Collapsed;
        }
        else
        {
          Head.Visibility = Visibility.Visible;
        }

        await Load_Record();
        _newExchangeGiftReceipt = false;
      }
      dtgPayments.IsReadOnly = true;

      // Reiniciamos las variable globales
      logGiftDetail.Clear();
      lstGiftsPacks.Clear();
      _lstPaymentsDelete.Clear();
      _newExchangeGiftReceipt = false;
      _newGiftReceipt = false;
    }
    #endregion

    #region Update_LostFocus
    /// <summary>
    /// Actualiza los totales de los payments cuando se pierde el focus de la columna
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 25/Abril/2016 Created
    /// </history>
    private void Update_LostFocus(object sender, RoutedEventArgs e)
    {
      CalculateTotalPayments();
    }
    #endregion

    #region txtgrgu_LostFocus
    /// <summary>
    /// Actualiza los campos del guest
    /// </summary>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private async void txtgrgu_LostFocus(object sender, RoutedEventArgs e)
    {
      // Verificamos que el Guest ID exista
      if (txtgrgu.Text != "")
      {
        if (_guestShort == null)
        {
          await GetGuestData(Convert.ToInt32(txtgrgu.Text));
          if (_guestShort == null)
          {
            UIHelper.ShowMessage("Guest ID not found", MessageBoxImage.Information);
            return;
          }
          else
          {
            cmbgrlo.SelectedValue = _guestShort.guls;
            txtgrls.Text = _guestShort.guls;
            txtgrGuest.Text = txtGuestName.Text;
            _guestID = Convert.ToInt32(txtgrgu.Text);
          }
        }
      }
    }
    #endregion

    #region dgGiftsReceiptPaymentShort_AddingNewItem
    /// <summary>
    /// Crea el row nuevo con valores por default en el grid Payments
    /// </summary>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private void dgGiftsReceiptPaymentShort_AddingNewItem(object sender, AddingNewItemEventArgs e)
    {
      e.NewItem = new GiftsReceiptPaymentShort { gycu = "US", gypt = "CS", gyAmount = 1, gysb = "CL", gype = App.User.User.peID, UserName = App.User.User.peN };
    }
    #endregion

    #region dgGiftsReceiptPaymentShort_PreviewExecuted
    /// <summary>
    /// Realiza la eliminacion de un row del grid de payments
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 29/abril/2016 Created 
    /// </history>
    private void dgGiftsReceiptPaymentShort_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (e.Command == DataGrid.DeleteCommand)
      {
        GiftsReceiptPaymentShort giftDelete = dtgPayments.SelectedItem as GiftsReceiptPaymentShort;
        _lstPaymentsDelete.Add(giftDelete);
      }
    }
    #endregion

    #region txtgrgu_PreviewKeyDown
    /// <summary>
    /// Actualiza los campos del guest
    /// </summary>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private void txtgrgu_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        txtgrgu_LostFocus(null, null);
      }
    }
    #endregion

    #region SavePromotionsOpera
    /// <summary>
    /// Guarda las promociones de Opera
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <history>
    /// [vipacheco] 30/Mayo/2016 Created
    /// </history>
    private void SavePromotionsOpera(int ReceiptID)
    {
      // obtenemos los regalos con promociones de Opera
      List<GiftsReceiptDetailPromotionsOpera> lstGiftsOpera = BRGiftsReceiptDetail.GetGiftsReceiptDetailPromotionsOpera(ReceiptID);

      if (lstGiftsOpera.Count > 0)
      {
        foreach (GiftsReceiptDetailPromotionsOpera Current in lstGiftsOpera)
        {
          // guardamos la promocion
          BRGuestsPromotions.SaveGuestPromotion(ReceiptID, Current.gegi, Current.giPromotionOpera, Current.grgu, Current.geQty, frmHost.dtpServerDate.Date);

          // actualizamos los regalos para identificarlos como que se guardaron como promociones de Opera
          BRGiftsReceiptDetail.UpdateGiftsReceiptDetailPromotionOpera(ReceiptID, Current.gegi, Current.giPromotionOpera);
        }

        UIHelper.ShowMessage("Gifts were successfully saved as Opera Promotion", MessageBoxImage.Information);
      }
    }
    #endregion

    #region ShowCancelExternalProducts
    /// <summary>
    /// Despliega el formulario de cancelacion de productos externos
    /// </summary>
    /// <param name="ExternalProduct"></param>
    /// <param name="Exchange"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Mayo/2016 Created
    /// </history>
    private async Task<MessageBoxResult> ShowCancelExternalProducts(EnumExternalProduct ExternalProduct, bool Exchange)
    {
      MessageBoxResult Result = MessageBoxResult.Cancel;

      // Desplegamos el formulario de cancelacion de productos externos
      frmCancelExternalProducts _frmCancelExternalProducts = new frmCancelExternalProducts(ExternalProduct,
                                                                                           Convert.ToInt32(txtgrID.Text),
                                                                                           Convert.ToInt32(txtgrgu.Text),
                                                                                           txtgrGuest.Text,
                                                                                           Convert.ToDecimal(txtgrMaxAuthGifts.Text.TrimStart('$')),
                                                                                           Convert.ToDecimal(txtTotalCost.Text.TrimStart('$')),
                                                                                           Convert.ToDecimal(txtgrCxCAdj.Text.TrimStart('$')),
                                                                                           _validateMaxAuthGifts,
                                                                                           useCxCCost,
                                                                                           Exchange,
                                                                                           this,
                                                                                           GiftsReceiptDetail,
                                                                                           chkgrExchange.IsChecked.Value
                                                                                           );
      _frmCancelExternalProducts.Owner = this;

      // Si se acepto el formulario
      if (_frmCancelExternalProducts.ShowDialog() == true)
      {
        Result = MessageBoxResult.OK;

        // si se debe generar un recibo exchange
        if (Exchange)
        {
          // Si se guardo un recibo exchange
          if (_frmCancelExternalProducts.ReceiptExchangeID > 0)
          {
            // agregamos el recibo de regalos exchange en el grid
            GiftsReceiptsShort _grs = new GiftsReceiptsShort() { grID = _frmCancelExternalProducts.ReceiptExchangeID, grNum = _frmCancelExternalProducts.txtgrNum.Text, grExchange = true };
            obsGiftsReceipt.Add(_grs);

            dtgReceipts.SelectedItem = _grs;

            _busyIndicator.IsBusy = true;
            //refrescamos los regalos del recibo
            await Load_Record();
            _busyIndicator.IsBusy = false;

          }
        }
        // Si se debo cancelar el recibo
        else
        {
          // si se pudo cancelar los regalos
          if (_frmCancelExternalProducts._Cancelled)
          {
            //refrescamos los regalos del recibo
            await Load_Gift_Of_GiftsReceipt(Convert.ToInt32(txtgrID.Text));
          }
        }
      }

      return Result;
    }
    #endregion

    #region btnPrint_Click
    /// <summary>
    /// Imprime un recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// [edgrodriguez] 12/Jul/2016 Modified. Se agrego el proceso de impresion del recibo de regalo.
    /// </history>
    private async void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      byte iMotive = 0;

      // si no es la primera vez que se imprime
      if (_reimpresion > 0)
      {
        frmReimpresionMotives _frmReimpresion = new frmReimpresionMotives();
        _frmReimpresion.ShowInTaskbar = false;
        _frmReimpresion.Owner = this;

        if (_frmReimpresion.ShowDialog() == true)
          iMotive = (byte)_frmReimpresion.LstMotives.SelectedValue;
        else
          return;

        // actualizamos el motivo de reimpresion
        await BRReimpresionMotives.UpdateGiftReceiptsReimpresionMotive(Convert.ToInt32(txtgrID.Text), iMotive);
        ReceiptsGifts.printReceiptGift(Convert.ToInt32(txtgrID.Text));
      }
      else
      {
        // actualizamos el contador de reimpresion
        await BRReimpresionMotives.UpdateGiftReceiptsReimpresionNumber(Convert.ToInt32(txtgrID.Text));
        ReceiptsGifts.printReceiptGift(Convert.ToInt32(txtgrID.Text));
      }

      // guardamos el historico del recibos de regalos
      await BRGiftsReceiptLog.SaveGiftsReceiptsLog(Convert.ToInt32(txtgrID.Text), App.User.User.peID);

      if ((Convert.ToDecimal(txtgrCxCGifts.Text) + Convert.ToDecimal(txtgrCxCAdj.Text)) != 0 ||
        Convert.ToDecimal(txtgrCxCPRDeposit.Text) > 0 || Convert.ToDecimal(txtgrCxCTaxiOut.Text) > 0)
      {
        ReceiptsGifts.printReceiptGift(Convert.ToInt32(txtgrID.Text), true);
      }

    }
    #endregion

    #region Window_Closing
    /// <summary>
    /// Evento al cerrar la ventana
    /// </summary>
    /// <history>
    /// [vipacheco] 21/Junio/2016 Created
    /// </history>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      // Verificamos que no este en modo edicion
      if (!_Undo)
      {
        if (btnSave.IsEnabled || btnUndo.IsEnabled)
        {
          UIHelper.ShowMessage("This form is currently in edit mode. Please save or undo your changes before closing it.", MessageBoxImage.Information, "Intelligence Marketing");
          e.Cancel = true;
        }
      }

      // Liberamos la memoria de las variables estaticas
      logGiftDetail.Clear();
      lstGiftsPacks.Clear();
    }
    #endregion

    #region grdGifts_PreparingCellForEdit
    /// <summary>
    /// Determina si se puede editar la informacion del grid
    /// </summary>
    /// <history>
    /// [vipacheco] 25/Junio/2016 Created
    /// </history>
    private void grdGifts_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      DataGrid dataGrid = sender as DataGrid;
      GiftsReceiptDetail giftsReceiptDetail = dataGrid.Items.CurrentItem as GiftsReceiptDetail;
      _currentCell = dtgGifts.CurrentCell;

      ReceiptsGifts.StartEdit(_modeOpenGrid, giftsReceiptDetail, ref _currentCell, ref dtgGifts, ref HasErrors);
    }
    #endregion

    #region EnableCancel
    /// <summary>
    /// Determina si se permite cancelar un recibo de regalos
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 24/Mayo/2016 Created
    /// </history>
    private bool EnableCancel()
    {
      // si esta cancelado
      if (chkgrCancel.IsChecked.Value)
        return false;
      // si no esta cancelado
      else
      {
        // Si el recibo es de una fecha posterior a la fecha de cierre
        if (dtpgrD.Value.Value.Date <= _dtpClose)
          return false;
        else
        {
          // si el recibo no es de hoy y no tiene permiso especial de recibos de regalos
          if (dtpgrD.Value.Value.Date != frmHost.dtpServerDate && !App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard))
            return false;
        }
      }

      return true;
    }
    #endregion

    #region CalculateCharge
    /// <summary>
    /// Calcula el cargo de regalos segun el tipo de calculo
    /// </summary>
    /// <param name="frm"></param>
    /// <history>
    /// [vipacheco] 14/Abril/2016 Created
    /// </history>
    public void CalculateCharge(ref frmCancelExternalProducts frm)
    {
      decimal curCharge = 0;
      decimal curTotalCost = 0;
      decimal curMaxAuthGifts = 0;
      bool blnTour = false;

      curTotalCost = txtTotalCost.Text != "" ? Convert.ToDecimal(txtTotalCost.Text.TrimStart(' ', '$')) : 0;

      curTotalCost = frm != null ? (frm.txtTotalCost.Text != "" ? Convert.ToDecimal(frm.txtTotalCost.Text.TrimStart(' ', '$')) : 0) :
                                   (txtTotalCost.Text != "" ? Convert.ToDecimal(txtTotalCost.Text.TrimStart(' ', '$')) : 0);

      //Establecemos el monto maximo de regalos
      SetMaxAuthGifts();

      curMaxAuthGifts = txtgrMaxAuthGifts.Text != "" ? Convert.ToDecimal(txtgrMaxAuthGifts.Text.TrimStart(' ', '$')) : 0;

      // Si no es un intercambio de regalos
      if (!chkgrExchange.IsChecked.Value)
      {
        // Localizamos a quien se carga
        switch ((cmbgrct.SelectedItem as ChargeTo).ctCalcType)
        {
          // si el huesped tiene tour el cargo es por el total de regalos menos el monto maximo
          // autorizado. De lo contrario el cargo es por el total de regalos
          case "A":
            // Validamos si tiene tour
            blnTour = _guest.guTour;
            if (blnTour)
              curCharge = CalculateChargeBasedOnMaxAuthGifts(curTotalCost, curMaxAuthGifts);
            else
              curCharge = curTotalCost;
            break;
          // El cargo es por el costo total de regalos
          case "B":
            curCharge = curTotalCost;
            break;
          // El cargo es por el costo total de regalos menos el monto maximo autorizado
          case "C":
            curCharge = CalculateChargeBasedOnMaxAuthGifts(curTotalCost, curMaxAuthGifts);
            break;
          // No generan cargo
          case "Z":
            curCharge = 0;
            break;
        }
      }
      // Calculamos el total del cargo
      if (frm != null)
        frm.txtgrcxcGifts.Text = string.Format("{0:C2}", curCharge);
      else
        txtgrCxCGifts.Text = string.Format("{0:C2}", curCharge);

      // Calculamos el total del cargo
      if (frm != null)
        frm.txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(frm.txtgrcxcGifts.Text != "" ? frm.txtgrcxcGifts.Text.TrimStart(' ', '$') : "0") + Convert.ToDecimal(frm.txtgrcxcAdj.Text != "" ? txtgrCxCAdj.Text.TrimStart(' ', '$') : "0"));
      else
        txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text != "" ? txtgrCxCGifts.Text.TrimStart(' ', '$') : "0") + Convert.ToDecimal(txtgrCxCAdj.Text != "" ? txtgrCxCAdj.Text.TrimStart(' ', '$') : "0"));

    }
    #endregion

    #region CalculateChargeBasedOnMaxAuthGifts
    /// <summary>
    /// Calcula el cargo de recibo de regalos basado en el monto maximo de regalos
    /// </summary>
    /// <param name="curTotalCost"></param>
    /// <param name="curMaxAuthGifts"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 14/Abril/2016 Created
    /// [vipacheco] 16/Mayo/2016 Modified --> Migrado a esta clase general
    /// </history>
    private decimal CalculateChargeBasedOnMaxAuthGifts(decimal curTotalCost, decimal curMaxAuthGifts)
    {
      // Si el costo total de regalos es mayor al monto maximo autorizado
      if (curTotalCost > curMaxAuthGifts)
        return curTotalCost - curMaxAuthGifts;
      // Si el monto de regalos esta dentro del monto maximo autorizado
      else
        return 0; // Por la naturaleza del calculo el cargo es siempre 0 si el total de regalos no es mayor al autorizado
    }
    #endregion

    #region int_PreviewTextInput
    /// <summary>
    /// Valida que solo se puedan usar números
    /// </summary>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void int_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }
    #endregion

    #region grdGifts_CellEditEnding
    /// <summary>
    /// Evento para validar los cambios de una celda en el grid
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created 
    /// </history>
    private async void grdGifts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (!HasErrors)
      {
        dtgGifts.CellEditEnding -= grdGifts_CellEditEnding;
        DataGrid dataGrid = sender as DataGrid;
        GiftsReceiptDetail giftsReceiptDetail = dataGrid.Items.CurrentItem as GiftsReceiptDetail;

        bool isExchange = chkgrExchange.IsChecked.Value;
        // Validamos la celda
        bool HasErrorValidate = await ReceiptsGifts.ValidateEdit(dataGrid, giftsReceiptDetail, isExchange, _currentCell);

        // Si no tiene errores se hacen los calculos.
        if (!HasErrorValidate)
        {
          ReceiptsGifts.AfterEdit(ref dtgGifts, _guestShort == null ? 0 : _guestShort.guID, ref giftsReceiptDetail, pCell: _currentCell, pUseCxCCost: useCxCCost, pIsExchange: isExchange, pChargeTo: cmbgrct.SelectedItem as ChargeTo,
                                  pLeadSourceID: txtgrls.Text, pTxtTotalCost: txtTotalCost, pTxtTotalPrice: txtTotalPrice, pTxtTotalToPay: txtTotalToPay, pTxtgrCxCGifts: txtgrCxCGifts,
                                  pTxtTotalCxC: txtTotalCxC, pTxtgrCxCAdj: txtgrCxCAdj, pTxtgrMaxAuthGifts: txtgrMaxAuthGifts, pLblgrMaxAuthGifts: txbgrMaxAuthGifts);
        }
        dtgGifts.CellEditEnding += grdGifts_CellEditEnding;
      }

      // Verificamos si se puso en modo lectura la celda
      if (_currentCell.Column.IsReadOnly)
        _currentCell.Column.IsReadOnly = false;
    }
    #endregion

    #region grdGifts_RowEditEnding
    /// <summary>
    /// Evento para finalizar la edicion de un row en el grid
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created 
    /// </history>
    private void grdGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      DataGrid dataGrid = sender as DataGrid;

      if (e.EditAction == DataGridEditAction.Commit)
      {
        GiftsReceiptDetail giftsReceipt = e.Row.Item as GiftsReceiptDetail;

        // validamos que los campos obligatorio ya se encuentren ingresados.
        if (giftsReceipt.geQty > 0 && giftsReceipt.gegi != null)
        {
          e.Cancel = false;
        }
        // Si aun faltan campos obligatorios se cancela el commit de la fila
        else
        {
          e.Cancel = true;
        }
      }
    }
    #endregion

    #region grdPayments_SelectionChanged
    /// <summary>
    /// Recalcula los totales cuando es cancelado una insercion o es eliminado algun row de payments
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Julio/2016 Created
    /// </history>
    private void grdPayments_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      CalculateTotalPayments();
    }
    #endregion

    #region grdGifts_SelectionChanged
    /// <summary>
    /// Recalcula los totales cuando es cancelado una insercion o es eliminado algun row de gifts
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Julio/2016 Created
    /// </history>
    private void grdGifts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // calculamos el monto total de regalos
      Gifts.CalculateTotalGifts(dtgGifts, EnumGiftsType.ReceiptGifts, "geQty", "gegi", "gePriceM", "gePriceMinor", "gePriceAdult", "gePriceA", "gePriceExtraAdult", txtTotalCost, txtTotalPrice, txtTotalToPay, saleField: "geSale");
    }
    #endregion

    #region AdjustmentControls
    /// <summary>
    /// Actualiza las posiciones de los controles cuando no esta en modo busqueda
    /// </summary>
    /// <history>
    /// [vipacheco] 15/Julio/2016 Created
    /// </history>
    private void AdjustmentControls()
    {
      // Payments
      grbPayments.Height = 126;

      Thickness _margin = grbPayments.Margin;
      _margin.Left = 10;
      _margin.Top = 398;
      _margin.Right = 0;
      _margin.Bottom = 0;
      grbPayments.Margin = _margin;

      // Gifts
      grbGifts.Height = 225;
    }
    #endregion

    #region UpdateSale_Click
    /// <summary>
    /// Recalcula los costos cuando se pulsa un checkbox Sale
    /// </summary>
    /// <history>
    /// [vipacheco] 25/Mayo/2016 Created
    /// </history>
    private void UpdateSale_Click(object sender, RoutedEventArgs e)
    {
      if (!dtgGifts.IsReadOnly)
      {
        var row = dtgGifts.SelectedItem as GiftsReceiptDetail;
        row.geSale = Convert.ToBoolean((sender as CheckBox).IsChecked);
        Gifts.CalculateTotalGifts(dtgGifts, EnumGiftsType.ReceiptGifts, nameof(row.geQty), nameof(row.gegi), nameof(row.gePriceM), nameof(row.gePriceMinor), nameof(row.gePriceAdult), nameof(row.gePriceA), nameof(row.gePriceExtraAdult), txtTotalCost, txtTotalPrice, txtTotalToPay, saleField: nameof(row.geSale));
      }
      else
      {
        (sender as CheckBox).IsChecked = !(sender as CheckBox).IsChecked;
      }
    }
    #endregion

    #region dtgReceipts_PreviewMouseLeftButtonDown
    /// <summary>
    /// Actualiza la informacion del recibo de regalo de acuerdo al Receipt Selecionado
    /// </summary>
    /// <history>
    /// [vipacheco] 29/Ago/2016 Created
    /// </history>
    private async void dtgReceipts_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      DataGridRow row = sender as DataGridRow;

      // Verificamos que no sea null la seleccion
      if (row == null) return;

      // Verificamos que ses un row distinto al seleccionado actualmente
      if ((row.DataContext as GiftsReceiptsShort) != dtgReceipts.SelectedItem)
      {
        GiftsReceiptsShort selected = row.DataContext as GiftsReceiptsShort;

        _busyIndicator.IsBusy = true;
        _busyIndicator.BusyContent = "Loading Gift Receipt - " + selected.grID;

        await Load_Record();
        _busyIndicator.IsBusy = false;
        row.Focus();
      }
      row.Focus();
    } 
    #endregion

  }
}
