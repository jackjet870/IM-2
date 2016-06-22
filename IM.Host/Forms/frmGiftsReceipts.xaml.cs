using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Enums;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using PalaceResorts.Common.PalaceTools;
using IM.Host.Classes;
using IM.Model.Helpers;
using IM.Services.Helpers;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmGiftsReceipts.xaml
  /// </summary>
  public partial class frmGiftsReceipts : Window
  {
    #region Variables
    private bool _newExchangeGiftReceipt = false, _newGiftReceipt = false, _edition = false, _blnInvitationGifts = false;
    private bool _blnPublicOrEmpleadoCost = false; // false - precio publico || true - precio empleado
    private static int _GuestID;
    private GiftsReceipt _GiftReceipt;
    private int _chargeToChanged = 0;
    private short _reimpresion = 0;
    private DateTime? _dtpClose;
    public EnumModeOpen modeOpen; // Variable para saber si puede ser editado el formulario o solo carga en modo vista (Edit | Preview)
    public EnumOpenBy modeOpenBy; // Variable para saber de que fuente fue invocado el formulario (Checkbox | boton)

    List<GiftsReceiptsShort> _lstGiftsReceipt = new List<GiftsReceiptsShort>();
    List<BookingDeposit> _lstDeposit = new List<BookingDeposit>();
    List<GiftsReceiptDetailShort> _lstGifts = new List<GiftsReceiptDetailShort>();
    List<GiftsReceiptPaymentShort> _lstPayments = new List<GiftsReceiptPaymentShort>();
    List<GiftInvitationWithoutReceipt> _lstGiftInvitation = new List<GiftInvitationWithoutReceipt>();

    ObservableCollection<GiftsReceiptsShort> _obsGiftsReceipt;
    ObservableCollection<GiftsReceiptDetailShort> _obsGifts;
    ObservableCollection<GiftsReceiptDetailShort> _obsGiftsComplet;
    ObservableCollection<GiftsReceiptPaymentShort> _obsPayments;
    ObservableCollection<dynamic> _obsDeposits;

    //Varible para guardar los cambios registrados durante el uso de host al momento de guardar con las claves iniciales
    public static List<KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>> _LogGiftDetail = new List<KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>>();
    public List<GiftsReceiptPaymentShort> _lstPaymentsDelete = new List<GiftsReceiptPaymentShort>();

    private GuestShort _guestShort;

    // Variable para las tasas de cambio!
    private List<ExchangeRateShort> _lstExchangeRate;

    // Moneda de la sala de ventas
    private string _SRCurrency;

    // Variable para saber si se valida el maximo autorizado de gifts
    public bool _blnValidateMaxAuthGifts;

    // Varibale para la validacion del guest status
    private bool _blnApplyGuestStatusValidation;

    //Variable para la validacion del cobro (Empleado || Publico)
    private bool _useCxCCost;

    // Variable que contendra la informacion del guest status info
    private GuestStatusValidateData _GuesStatusInfo;

    // Variable para validar el row seleccionado del grid regalos
    private string _giftIDSelected = "";

    // Variable que contiene el row seleccionado de Host
    GuestPremanifestHost _GuestHost = null;

    #region CollectionViewSource Globales
    CollectionViewSource _dsReceipts;
    CollectionViewSource _dsGiftsReceipt;
    CollectionViewSource _dsGiftsDetail;
    CollectionViewSource _dsPayments;
    CollectionViewSource _dsBookingDeposit;
    CollectionViewSource _dsGiftsReceiptPackage;
    #endregion

    #endregion

    #region Constructor
    public frmGiftsReceipts(int guestID = 0)
    {
      _GuestID = guestID;

      // Obtenemos la fecha de cierre de los recibos de regalos de la sala
      _dtpClose = BRSalesRooms.GetCloseSalesRoom(EnumSalesRoomType.GiftsReceipts, App.User.SalesRoom.srID);

      InitializeComponent();
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Inicializa y carga los parametros y listas necesarias
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 20/Abril/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsReceipts = ((CollectionViewSource)(this.FindResource("dsReceipts")));
      _dsGiftsReceipt = ((CollectionViewSource)(this.FindResource("dsGiftsReceipt")));
      _dsGiftsDetail = ((CollectionViewSource)(this.FindResource("dsGiftsDetail")));
      _dsPayments = ((CollectionViewSource)(this.FindResource("dsPayments")));
      _dsBookingDeposit = ((CollectionViewSource)(this.FindResource("dsBookingDeposit")));
      _dsGiftsReceiptPackage = ((CollectionViewSource)(this.FindResource("dsGiftsReceiptPackage")));

      // Cargamos las listas
      Load_ComboBox();

      // Obtenemos la fecha del servidor
      dtpgrD.Value = frmHost._dtpServerDate.Date;

      // Ocultamos los botones edicion de grid's
      controlEditionGifts(Visibility.Hidden, Visibility.Hidden, false);

      switch (modeOpenBy)
      {
        case EnumOpenBy.Checkbox:
          switch (modeOpen)
          {
            case EnumModeOpen.Edit:
              Load_Grid_GiftsReceipt(_GuestID);
              break;
            case EnumModeOpen.Preview:
              Load_Grid_GiftsReceipt(_GuestID);
              break;
          }

          break;
        case EnumOpenBy.Button:
          switch (modeOpen)
          {
            case EnumModeOpen.Edit:
              Controls_Reading_Mode();
              break;
            case EnumModeOpen.Preview:
              Controls_Reading_Mode();
              break;
          }
          break;
      }

      // si no se maneja monedero electronico
      if (!ConfigHelper.GetString("ElectronicPurseUse").ToUpper().Equals("TRUE"))
      {
        btnEPurse.Visibility = Visibility.Hidden;
        btnCancelElectronicPurse.Visibility = Visibility.Hidden;
      }

      // si no se maneja promociones de Sistur
      if (!ConfigHelper.GetString("UseSisturPromotions").ToUpper().Equals("TRUE"))
        btnCancelSisturPromotions.Visibility = Visibility.Hidden;

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
      CollectionViewSource _dsPersonnel_Offered = ((CollectionViewSource)(this.FindResource("dsPersonnel_Offered")));
      _dsPersonnel_Offered.Source = frmHost._lstPersonnel;

      // Obtenemos Hostess de regalos
      CollectionViewSource _dsPersonnel_Gifts = ((CollectionViewSource)(this.FindResource("dsPersonnel_Gifts")));
      _dsPersonnel_Gifts.Source = frmHost._lstPersonnelHOSTGIFTS;

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
    private void Load_Grid_GiftsReceipt(int GuestID = 0)
    {
      int _guest = 0, _receipt = 0;
      string _salesRoom = "ALL", _folio = "ALL", _name = "ALL", _reservation = "ALL";
      DateTime? _dateFrom = null, _dateTo = null;

      // Si se esta buscando
      if (GuestID == 0)
      {
        _guest = txtCgrgu.Text != "" ? Convert.ToInt32(txtCgrgu.Text) : _guest;
        _salesRoom = App.User.SalesRoom.srID;
        _receipt = txtCgrID.Text != "" ? Convert.ToInt32(txtCgrID.Text) : _receipt;
        _folio = txtCgrNum.Text != "" ? txtCgrNum.Text : _folio;
        _dateFrom = dtpCgrDFrom.Value.Value.Date;
        _dateTo = dtpTo.Value.Value.Date;
        _name = txtCgrGuest.Text != "" ? txtCgrGuest.Text : _name;
        _reservation = txtCReservation.Text != "" ? txtCReservation.Text : _reservation;
      }
      // Si no se esta buscando
      else
      {
        // deshabilitamos la busqueda
        grbSelectionCriteria.Visibility = Visibility.Hidden;
        brdExchange.Visibility = Visibility.Hidden;

        // Busqueda por Guest ID
        _guest = txtCgrgu.Text != "" ? Convert.ToInt32(txtCgrgu.Text) : _GuestID;
        txtgrgu.IsReadOnly = true;
      }

      //Ejecutamos el procedimiento almancenado con los criterios de busqueda
      _lstGiftsReceipt = BRGiftsReceipts.GetGiftsReceipts(_guest, _salesRoom, _receipt, _folio, _dateFrom, _dateTo, _name, _reservation);
      _obsGiftsReceipt = new ObservableCollection<GiftsReceiptsShort>(_lstGiftsReceipt);
      _dsReceipts.Source = _obsGiftsReceipt;

      // si no hay recibos de regalos
      if (_lstGiftsReceipt.Count == 0 && modeOpenBy != EnumOpenBy.Checkbox)
      {
        btnCancel.Visibility = Visibility.Hidden;
        btnEPurse.Visibility = Visibility.Hidden;
        btnCancelElectronicPurse.Visibility = Visibility.Hidden;
        btnCancelSisturPromotions.Visibility = Visibility.Hidden;

        UIHelper.ShowMessage("Not Found Gift Receipt with input specifications", MessageBoxImage.Information);
        return;
      }

      Load_Record();

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
    private void Load_Record()
    {
      // si hay algun recibo de regalos
      if (_lstGiftsReceipt.Count > 0)
      {
        // cargamos el recibo de regalos para modificacion
        Load_Receipt();

        // Establecemos el monto maximo de regalos
        SetMaxAuthGifts();

        // cargamos los regalos del recibo de regalos
        Load_Gift_Of_GiftsReceipt(ReceiptID: Convert.ToInt32(txtgrID.Text));
        Controls_Reading_Mode();

        // calculamos el total del cargo
        string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text) + Convert.ToDecimal(txtgrCxCAdj.Text));
      }
      // Si es un recibo de regalos nuevo
      else
      {
        New_GiftReceipt();

        // Cargamos los regalos de la invitacion
        Load_Gift_Of_GiftsReceipt(GuestID: _GuestID);

        // si no se esta buscando
        if (_GuestID > 0)
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

      // cargamos los depositos
      Load_Deposits(txtgrgu.Text);

      // cargamos los pagos
      Load_Payments(txtgrID.Text);

      // Obtenemos los datos del huesped
      GetGuestData(txtgrgu.Text != "" ? Convert.ToInt32(txtgrgu.Text) : 0);
    }
    #endregion

    #region New_GiftReceipt
    /// <summary>
    /// Permite agregar un recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private void New_GiftReceipt()
    {
      _newGiftReceipt = true;

      // Cargamos la estructura de la tabla de recibo de regalos
      txtgrgu.Text = txtgrgu.Text != "" ? txtgrgu.Text : "";
      txtgrgu.IsReadOnly = false;
      dtpgrD.Value = frmHost._dtpServerDate;
      cboSalesRoom.SelectedValue = App.User.SalesRoom.srID;

      // si no se esta buscando
      if (_GuestID > 0)
      {
        // sugerimos los datos del huesped
        GetGuestData(_GuestID);
        txtgrgu.IsEnabled = false;
      }
      // Si se esta buscando
      else
        txtgrgu.IsReadOnly = false;

      cbogrct.SelectedValue = "Marketing";

      // restauramos el valor para corregir calculo de costo para cada nuevo recibo
      _useCxCCost = false;

      // establecemos el monto maximo de regalos
      SetMaxAuthGifts();

      // Depositos
      cbogrcu.SelectedValue = "US";
      cbogrpt.SelectedValue = "CS";
      cbogrcuCxCTaxiOut.SelectedValue = "US";
      cbogrcuCxCPRDeposit.SelectedValue = "US";
      txtDepositUS.Text = string.Format("0:C2", 0);
      txtDepositMN.Text = string.Format("0:C2", 0);

      // Regalos
      txtTotalCost.Text = string.Format("0:C2", 0);
      txtTotalPrice.Text = string.Format("0:C2", 0);
      txtTotalCxC.Text = string.Format("0:C2", 0);

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
    private void AddReceipt(bool Exchange = false)
    {
      New_GiftReceipt();

      // Si es un intercambio de regalos
      if (Exchange)
      {
        brdExchange.Visibility = Visibility.Visible;
        chkgrExchange.IsChecked = true;
        _newExchangeGiftReceipt = true;
      }

      // cargamos los regalos de la invitacion
      Load_Gift_Of_GiftsReceipt(GuestID: _GuestID);

      // cargamos los pagos
      Load_Payments(txtgrID.Text);
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

      // Criterios de busqueda
      grbSelectionCriteria.Visibility = Visibility.Hidden;

      // Botones
      btnNew.Visibility = Visibility.Hidden;
      btnExchange.Visibility = Visibility.Hidden;
      btnEdit.Visibility = Visibility.Hidden;
      btnPrint.Visibility = Visibility.Hidden;
      btnSave.Visibility = Visibility.Visible;
      btnUndo.Visibility = Visibility.Visible;
      btnClose.Visibility = Visibility.Hidden;
      btnCancel.Visibility = Visibility.Hidden;
      btnEPurse.Visibility = Visibility.Hidden;
      btnCancelElectronicPurse.Visibility = Visibility.Hidden;
      btnCancelSisturPromotions.Visibility = Visibility.Hidden;

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
      cbogrlo.IsEnabled = _Enable && txtgrgu.Text == "";

      // Hotel
      cboHotel.IsEnabled = _Enable;

      // Offered by
      cbogrpe.IsEnabled = _Enable;

      // Gifts Hostess
      cbogrHost.IsEnabled = _Enable;

      // si no tiene Guest ID o si tiene permiso especial de recibos de regalos,
      // permitimos modificar la sala de ventas
      cboSalesRoom.IsEnabled = _Enable && (txtgrgu.Text == "" || App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special));

      // Grid de regalos
      controlEditionGifts(Visibility.Visible, Visibility.Visible, true);

      // Se verifica si es un nuevo Recibo
      if (_newGiftReceipt || _newExchangeGiftReceipt)
      {
        txtgrID.Text = "";

        if (_obsGifts != null)
        {
          _obsGifts.Clear();
          _obsGiftsComplet.Clear();
        }

        if (_obsPayments != null)
          _obsPayments.Clear();

        if (_obsDeposits != null)
          _obsDeposits.Clear();
      }

      // Rcpt Number
      txtgrNum.IsReadOnly = false;

      // Membership
      txtgrMemberNum.IsReadOnly = false;

      // Fecha
      dtpgrD.IsReadOnly = false;

      // Payments
      grdPayments.IsReadOnly = false;

      // charge To
      cbogrct.IsEnabled = _Enable;

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
      brdExchange.Visibility = chkgrExchange.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
      ControlEnable(false, false, false, false, false, false, true, true, false);

      controlEditionGifts(Visibility.Hidden, Visibility.Hidden);
      grdPayments.IsReadOnly = true;

      // si se esta buscando
      if (_GuestID == 0)
        grbSelectionCriteria.Visibility = Visibility = Visibility.Visible;
      else
        grbSelectionCriteria.Visibility = Visibility.Hidden;

      // Botones
      btnNew.Visibility = Visibility.Visible;
      btnExchange.Visibility = Visibility.Visible;
      btnSave.Visibility = Visibility.Hidden;
      btnUndo.Visibility = Visibility.Hidden;
      btnClose.Visibility = Visibility.Visible;

      // Fecha
      dtpgrD.IsReadOnly = true;

      // Si se esta buscando y no se ha cargado ningun recibo
      if (_GuestID == 0 && txtgrID.Text == "")
      {
        btnEdit.Visibility = Visibility.Hidden;
        btnPrint.Visibility = Visibility.Hidden;
        btnCancel.Visibility = Visibility.Hidden;
        btnEPurse.Visibility = Visibility.Hidden;
        btnCancelElectronicPurse.Visibility = Visibility.Hidden;
        btnCancelSisturPromotions.Visibility = Visibility.Hidden;
      }
      else
      {
        // Se permite editar e imprimir si el recibo no esta cancelado
        btnEdit.Visibility = chkgrCancel.IsChecked.Value ? Visibility.Hidden : Visibility.Visible;
        btnPrint.Visibility = chkgrCancel.IsChecked.Value ? Visibility.Hidden : Visibility.Visible;

        // Habilitamos y deshabilitamos el boton de cancelar
        btnCancel.Visibility = EnableCancel();
        btnCancelSisturPromotions.Visibility = Visibility.Visible;
        //btnEPurse.Visibility = Visibility.Visible;
        //btnCancelElectronicPurse.Visibility = Visibility.Visible;
      }

      // si tiene permiso de solo lectura para recibos de regalos
      if (!App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard))
      {
        btnNew.Visibility = Visibility.Hidden;
        btnExchange.Visibility = Visibility.Hidden;
        btnEdit.Visibility = Visibility.Hidden;
        btnEPurse.Visibility = Visibility.Hidden;
        btnCancelElectronicPurse.Visibility = Visibility.Hidden;
        btnCancelSisturPromotions.Visibility = Visibility.Hidden;
      }

      // Autentificacion
      txtChangedBy.Text = "";
      txtPwd.Password = "";
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
    private void Load_Payments(string GiftReceipt)
    {
      // Cargamos los regalos payments
      int GiftReceiptID = GiftReceipt != "" ? Convert.ToInt32(GiftReceipt) : 0;
      _lstPayments = BRGiftsReceiptsPayments.GetGiftsReceiptPaymentsShort(GiftReceiptID);

      _obsPayments = new ObservableCollection<GiftsReceiptPaymentShort>(_lstPayments);
      _dsPayments.Source = _obsPayments;

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
    private void Load_Deposits(string GuestID)
    {
      int GuestSearch = (!string.IsNullOrEmpty(GuestID) || GuestID != "") ? Convert.ToInt32(GuestID) : 0;
      //Cargamos los depositos
      _lstDeposit = BRBookingDeposits.GetBookingDeposits(GuestSearch);
      var _resultLst = _lstDeposit.Select(c => new
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

      _obsDeposits = new ObservableCollection<dynamic>(_resultLst);
      _dsBookingDeposit.Source = _obsDeposits;
    } 
    #endregion

    #region Load_Receipt
    /// <summary>
    /// Cargamos el recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 28/Abril/2016 Created
    /// </history>
    private void Load_Receipt()
    {
      if (grdReceipts.SelectedItem == null && grdReceipts.Items.Count > 0)
        grdReceipts.SelectedIndex = 0;

      GiftsReceiptsShort _Selected = (GiftsReceiptsShort)grdReceipts.SelectedItem;
      int GiftReceiptID = _Selected.grID;

      _GiftReceipt = BRGiftsReceipts.GetGiftReceipt(_Selected.grID);
      _reimpresion = _GiftReceipt.grReimpresion;  // Obtenemos la cantidad de reimpresiones que tiene realizado este gift

      grdMain.DataContext = _GiftReceipt;
    }
    #endregion

    #region Load_Gift_Of_GiftsReceipt
    /// <summary>
    /// Carga los regalos de recibos de regalos
    /// </summary>
    /// <param name="ReceiptId"></param>
    /// <param name="GuestID"></param>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private void Load_Gift_Of_GiftsReceipt(int ReceiptID = 0, int GuestID = 0)
    {
      // si es un recibo de regalos nuevo
      if (ReceiptID == 0)
        _lstGifts = Load_Gifts_Of_Invitation(GuestID);  // cargamos los regalos de la invitacion
      // si es un recibo de regalos existente
      else
        _lstGifts = BRGiftsReceiptDetail.GetGiftsReceiptDetail(ReceiptID);  // cargamos los regalos del recibo de regalos

      // Creamos los Observables
      _obsGifts = new ObservableCollection<GiftsReceiptDetailShort>(_lstGifts);
      _obsGiftsComplet = new ObservableCollection<GiftsReceiptDetailShort>(_lstGifts);
      _dsGiftsDetail.Source = _obsGifts;

      // calculamos el monto total de regalos
      Gifts.CalculateTotalGifts(grdGifts, EnumGiftsType.ReceiptGifts, ref txtTotalCost, ref txtTotalPrice, ref txtTotalToPay);

      // configuramos la informacion de GuestStatus que se validara
      if (_GuestID > 0 || txtgrID.Text != "")
        LoadGuesStatusInfo(string.IsNullOrEmpty(txtgrID.Text) ? 0 : Convert.ToInt32(txtgrID.Text), _GuestID);
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
    private List<GiftsReceiptDetailShort> Load_Gifts_Of_Invitation(int GuestID)
    {
      List<GiftsReceiptDetailShort> Result = new List<GiftsReceiptDetailShort>();
      List<GiftInvitationWithoutReceipt> _lstGiftInvitation = BRInvitsGifts.GetGiftsInvitationWithoutReceipt(_GuestID);

      if (_lstGiftInvitation.Count > 0)
      {
        // Contruimos el source!
        Result = _lstGiftInvitation.Select(x => new GiftsReceiptDetailShort
        {
          QtyUnit = x.QtyUnit,
          geAdults = x.geAdults,
          geAsPromotionOpera = (bool)x.geAsPromotionOpera,
          geCancelElectronicPurse = (bool)x.geCancelElectronicPurse,
          geCancelPVPPromo = (bool)x.geCancelPVPPromo,
          geCharge = (decimal)x.geCharge,
          geComments = x.geComments,
          geConsecutiveElectronicPurse = string.IsNullOrEmpty(x.geConsecutiveElectronicPurse) ? 0 : Convert.ToInt32(x.geConsecutiveElectronicPurse),
          gect = x.gect,
          geCxC = x.geCxC,
          geExtraAdults = x.geExtraAdults,
          geFolios = x.geFolios,
          gegi = x.gegi,
          gegr = string.IsNullOrEmpty(x.gegr) ? 0 : Convert.ToInt32(x.gegr),
          geInElectronicPurse = (bool)x.geInElectronicPurse,
          geInOpera = (bool)x.geInOpera,
          geInPVPPromo = (bool)x.geInPVPPromo,
          geMinors = x.geMinors,
          gePriceA = (decimal)x.gePriceA,
          gePriceAdult = (decimal)x.gePriceAdult,
          gePriceExtraAdult = (decimal)x.gePriceExtraAdult,
          gePriceM = (decimal)x.gePriceM,
          gePriceMinor = (decimal)x.gePriceMinor,
          geQty = x.geQty,
          geSale = (bool)x.geSale
        }).ToList();

        // Guardamos los GiftInvitation encontrados
        foreach (GiftsReceiptDetailShort Current in Result)
        {
          GiftsReceiptDetail _Gift = new GiftsReceiptDetail
          {
            gegr = Current.gegr,
            gegi = Current.gegi,
            gect = Current.gect,
            geQty = Current.geQty,
            geAdults = Current.geAdults,
            geMinors = Current.geMinors,
            geFolios = Current.geFolios,
            gePriceA = Current.gePriceA,
            gePriceM = Current.gePriceM,
            geCharge = Current.geCharge,
            gecxc = Current.geCxC,
            geComments = Current.geComments,
            geInElectronicPurse = Current.geInElectronicPurse,
            geConsecutiveElectronicPurse = Current.geConsecutiveElectronicPurse,
            geCancelElectronicPurse = Current.geCancelElectronicPurse,
            geExtraAdults = Current.geExtraAdults,
            geInPVPPromo = Current.geInPVPPromo,
            geCancelPVPPromo = Current.geCancelPVPPromo,
            geInOpera = Current.geInOpera,
            geAsPromotionOpera = Current.geAsPromotionOpera,
            gePriceAdult = Current.gePriceAdult,
            gePriceMinor = Current.gePriceMinor,
            gePriceExtraAdult = Current.gePriceExtraAdult,
            geSale = Current.geSale
          };

          // Guardamos en el log de actividades de GIFTS
          _LogGiftDetail.Add(new KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>(Model.Enums.EnumMode.add, _Gift));
        }
      }

      return Result;
    }
    #endregion






    #region controlEditionGifts
    /// <summary>
    /// Metodo encargado de mostrar, ocultar, ajustar los botos de agregar y eliminar del grid gifts detail
    /// </summary>
    /// <param name="_btnAdd"></param>
    /// <param name="_btnRemove"></param>
    /// <param name="_gridAdjustment"></param>
    private void controlEditionGifts(Visibility _btnAdd, Visibility _btnRemove, bool _gridAdjustment = false)
    {
      btnAddGift.Visibility = _btnAdd;
      btnRemoveGift.Visibility = _btnRemove;

      if (_gridAdjustment)
      {
        //Ajustamos el margen del grid
        Thickness _margin = grdGifts.Margin;
        _margin.Left = 0;
        _margin.Top = 27;
        _margin.Right = 9;
        _margin.Bottom = 25;
        grdGifts.Margin = _margin;
      }
      else
      {
        //Ajustamos el margen del grid ORGINALMENTE si se movieron los margenes
        Thickness _margin = grdGifts.Margin;
        _margin.Left = 0;
        _margin.Top = 2;
        _margin.Right = 9;
        _margin.Bottom = 25;
        grdGifts.Margin = _margin;
      }
    }
    #endregion

    #region grdGiftsReceipt_DoubleClick
    /// <summary>
    /// Función encargada de Cargar la informacion de acuerdo al GIFT RECEIPT seleccionado!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void grdGiftsReceipt_DoubleClick(object sender, RoutedEventArgs e)
    {
      Load_Record();
    }
    #endregion

    #region grdGifts_DoubleClick
    /// <summary>
    /// Función encargada de Cargar la informacion para edicion de acuerdo al GIFT seleccionado!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void grdGiftsReceiptDetail_DoubleClick(object sender, RoutedEventArgs e)
    {
      if (_edition || _newExchangeGiftReceipt || _newGiftReceipt)
      {
        GiftsReceiptDetailShort _giftSelected = (GiftsReceiptDetailShort)grdGifts.SelectedItem;
        GiftsReceiptsShort _giftReceipt = (GiftsReceiptsShort)grdReceipts.SelectedItem;

        frmGiftsReceiptsDetail _frmGiftsDetail = new frmGiftsReceiptsDetail(ref _obsGifts, _obsGiftsComplet, _GuestID, _giftReceipt, _giftSelected, _blnPublicOrEmpleadoCost);
        _frmGiftsDetail.Owner = this;
        _frmGiftsDetail.ShowInTaskbar = false;
        _frmGiftsDetail.modeOpen = EnumModeOpen.Edit;
        ObjectHelper.CopyProperties(_frmGiftsDetail._GiftCurrent, _giftSelected); // Se copian las propiedades a una temporañ
        _frmGiftsDetail.ShowDialog();

      }
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            grdGiftsReceipt_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion

    #region ControlsReadOnly
    /// <summary>
    /// Asigna parametro de solo lectura a los controles correspondientes.
    /// </summary>
    /// <param name="_grdGiftsReceiptDetail"></param>
    /// <param name="_dgGiftsReceiptPaymentShort"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void ControlsReadOnly(bool _grdGiftsReceiptDetail, bool _dgGiftsReceiptPaymentShort)
    {
      grdGifts.IsReadOnly = _grdGiftsReceiptDetail;
      grdPayments.IsReadOnly = _dgGiftsReceiptPaymentShort;
    }
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
      cbogrpe.IsEnabled = _Personnel;
      cbogrHost.IsEnabled = _Host;
      cboHotel.IsEnabled = _Hotel;
      cboSalesRoom.IsEnabled = _SalesRooms;
      cbogrcu.IsEnabled = _Currency;
      cbogrpt.IsEnabled = _Payment;
      cbogrlo.IsEnabled = _Location;
    }
    #endregion

    #region ControlVisibility
    /// <summary>
    /// Oculta y deshabilita los controles necesarios cuando la vista es Preview
    /// </summary>
    /// <param name="_hidden"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void ControlVisibility(Visibility _sectionCriterio = Visibility.Visible, Visibility _brdExchange = Visibility.Visible,
                                   Visibility _lblgrMaxAuthGifts = Visibility.Visible, Visibility _txtgrMaxAuthGifts = Visibility.Visible)
    {
      grbSelectionCriteria.Visibility = _sectionCriterio;
      brdExchange.Visibility = _brdExchange;
      lblgrMaxAuthGifts.Visibility = _lblgrMaxAuthGifts;
      txtgrMaxAuthGifts.Visibility = _txtgrMaxAuthGifts;
    }
    #endregion

    #region ControlBottonEdition
    /// <summary>
    /// Oculta los boton segun el modo de apertura del formulario
    /// </summary>
    /// <param name="_btnNew"></param>
    /// <param name="_btnExchange"></param>
    /// <param name="_btnEdit"></param>
    /// <param name="_btnSave"></param>
    /// <param name="_btnUndo"></param>
    /// <param name="_btnPrint"></param>
    /// <param name="_btnCancel"></param>
    /// <param name="_btnCancelSistur"></param>
    /// <param name="_btnEPurse"></param>
    /// <param name="_btnCancelElectronicPurse"></param>
    /// <history>
    /// [vipacheco] 06/04/2016 Created
    /// </history>
    private void ControlBottonEdition(Visibility _btnNew, Visibility _btnExchange, Visibility _btnEdit, Visibility _btnSave,
                                      Visibility _btnUndo, Visibility _btnPrint, Visibility _btnCancel, Visibility _btnCancelSistur, Visibility _btnEPurse,
                                      Visibility _btnCancelElectronicPurse, Visibility _btnClose = Visibility.Visible)
    {
      btnNew.Visibility = _btnNew;
      btnExchange.Visibility = _btnExchange;
      btnEdit.Visibility = _btnEdit;
      btnSave.Visibility = _btnSave;
      btnUndo.Visibility = _btnUndo;
      btnPrint.Visibility = _btnPrint;
      btnCancel.Visibility = _btnCancel;
      btnCancelSisturPromotions.Visibility = _btnCancelSistur;
      btnEPurse.Visibility = _btnEPurse;
      btnCancelElectronicPurse.Visibility = _btnCancelElectronicPurse;
      btnClose.Visibility = _btnClose;
    }
    #endregion

    private void Load_grdReceipts()
    {
      _blnInvitationGifts = false;
      if (_lstGiftsReceipt != null)
      {
        if (_lstGiftsReceipt.Count > 0)
        {
          GiftsReceiptShort_Selected();

          // Verificamos si esta en modo busqueda para obtener el guestID!
          if (modeOpen == EnumModeOpen.Search || modeOpen == EnumModeOpen.Edit)
          {
            _GuestID = Convert.ToInt32(txtgrgu.Text);
          }

          // Cargamos los Booking Deposits
          BookinDeposits_Loaded();
          SetMaxAuthGifts();
          ReceiptGifts.CalculateTotalGifts(grdGifts, EnumGiftsType.ReceiptGifts, ref txtTotalCost, ref txtTotalPrice, ref txtTotalToPay);
          CalculateTotalPayments();
          LoadExchangeRates();
          ConvertDepositToUsDlls();
          GetGuestData(_GuestID);
          txtgrcxcAdj_Validate();
        }

        else if (_lstGiftsReceipt.Count == 0 && modeOpen == EnumModeOpen.PreviewEdit) // cargamos los regalos de la invitacion 
        {
          _lstGiftInvitation = BRInvitsGifts.GetGiftsInvitationWithoutReceipt(_GuestID);

          if (_lstGiftInvitation != null && _lstGiftInvitation.Count > 0)
          {
            // Contruimos el source!
            _lstGifts = _lstGiftInvitation.Select(x => new GiftsReceiptDetailShort
            {
              QtyUnit = x.QtyUnit,
              geAdults = x.geAdults,
              geAsPromotionOpera = (bool)x.geAsPromotionOpera,
              geCancelElectronicPurse = (bool)x.geCancelElectronicPurse,
              geCancelPVPPromo = (bool)x.geCancelPVPPromo,
              geCharge = (decimal)x.geCharge,
              geComments = x.geComments,
              geConsecutiveElectronicPurse = string.IsNullOrEmpty(x.geConsecutiveElectronicPurse) ? 0 : Convert.ToInt32(x.geConsecutiveElectronicPurse),
              gect = x.gect,
              geCxC = x.geCxC,
              geExtraAdults = x.geExtraAdults,
              geFolios = x.geFolios,
              gegi = x.gegi,
              gegr = string.IsNullOrEmpty(x.gegr) ? 0 : Convert.ToInt32(x.gegr),
              geInElectronicPurse = (bool)x.geInElectronicPurse,
              geInOpera = (bool)x.geInOpera,
              geInPVPPromo = (bool)x.geInPVPPromo,
              geMinors = x.geMinors,
              gePriceA = (decimal)x.gePriceA,
              gePriceAdult = (decimal)x.gePriceAdult,
              gePriceExtraAdult = (decimal)x.gePriceExtraAdult,
              gePriceM = (decimal)x.gePriceM,
              gePriceMinor = (decimal)x.gePriceMinor,
              geQty = x.geQty,
              geSale = (bool)x.geSale
            }).ToList();

            if (_lstGifts != null && _lstGifts.Count > 0)
            {
              // Cargamos los datos al grid
              _obsGiftsReceipt = new ObservableCollection<GiftsReceiptsShort>();
              _obsPayments = new ObservableCollection<GiftsReceiptPaymentShort>();
              _dsGiftsDetail.Source = _obsGifts;
              _dsReceipts.Source = _obsGiftsReceipt;
              _dsPayments.Source = _obsPayments;
              _obsGifts = new ObservableCollection<GiftsReceiptDetailShort>(_lstGifts);
              _obsGiftsComplet = new ObservableCollection<GiftsReceiptDetailShort>(_lstGifts);
              _dsGiftsDetail.Source = _obsGifts;

              BookinDeposits_Loaded();
              SetMaxAuthGifts();
              ReceiptGifts.CalculateTotalGifts(grdGifts, EnumGiftsType.ReceiptGifts, ref txtTotalCost, ref txtTotalPrice, ref txtTotalToPay);
              CalculateTotalPayments();
              LoadExchangeRates();
              ConvertDepositToUsDlls();
              GetGuestData(_GuestID);
              txtgrcxcAdj_Validate();

              controlEditionGifts(Visibility.Visible, Visibility.Visible, true);
              _edition = true;
              _blnInvitationGifts = true; // Bandera que indica que los gifts vienen de InvitsGifts
              grdPayments.IsReadOnly = false;

              // Guardamos los GiftInvitation encontrados
              foreach (GiftsReceiptDetailShort item in _obsGifts)
              {
                GiftsReceiptDetail _gift = new GiftsReceiptDetail
                {
                  gegr = item.gegr,
                  gegi = item.gegi,
                  gect = item.gect,
                  geQty = item.geQty,
                  geAdults = item.geAdults,
                  geMinors = item.geMinors,
                  geFolios = item.geFolios,
                  gePriceA = item.gePriceA,
                  gePriceM = item.gePriceM,
                  geCharge = item.geCharge,
                  gecxc = item.geCxC,
                  geComments = item.geComments,
                  geInElectronicPurse = item.geInElectronicPurse,
                  geConsecutiveElectronicPurse = item.geConsecutiveElectronicPurse,
                  geCancelElectronicPurse = item.geCancelElectronicPurse,
                  geExtraAdults = item.geExtraAdults,
                  geInPVPPromo = item.geInPVPPromo,
                  geCancelPVPPromo = item.geCancelPVPPromo,
                  geInOpera = item.geInOpera,
                  geAsPromotionOpera = item.geAsPromotionOpera,
                  gePriceAdult = item.gePriceAdult,
                  gePriceMinor = item.gePriceMinor,
                  gePriceExtraAdult = item.gePriceExtraAdult,
                  geSale = item.geSale
                };
                _LogGiftDetail.Add(new KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>(Model.Enums.EnumMode.add, _gift));
              }
            }
          }
        }
      }

      // Configuramos la informacion de GuestStatus que se validara
      if (_GuestID > 0 || txtgrID.Text != "")
        LoadGuesStatusInfo(string.IsNullOrEmpty(txtgrID.Text) ? 0 : Convert.ToInt32(txtgrID.Text), _GuestID);

      // si es un supervisor puede modificar la fecha del recibo.
      dtpgrD.IsReadOnly = App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special);
    }

    #region LoadGuesStatusInfo
    /// <summary>
    /// Carga la informacion de GuestStatus para validaicon de nuevo schema de regalos
    /// </summary>
    /// <param name="receiptID"></param>
    /// <param name="guestID"></param>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private void LoadGuesStatusInfo(int receiptID, int guestID)
    {
      _blnApplyGuestStatusValidation = false;

      _GuesStatusInfo = BRGuestStatus.GetStatusValidateInfo(guestID, receiptID);

      // Solo si esta configurado se realiza la revision
      if (_GuesStatusInfo != null)
        if (_GuesStatusInfo.gsMaxQtyTours > 0)
          _blnApplyGuestStatusValidation = true;
    }
    #endregion

    #region Search_GiftsReceipt
    /// <summary>
    /// Obtiene los datos ingresados por el usuario para la busqueda de algun Gift Receipt
    /// </summary>
    /// <returns> True - Encontro el Gift Receipt | False - Ningun resultado encontrado </returns>
    /// <history>
    /// 12/Abril/2016 Created
    /// </history>
    private bool Search_GiftsReceipt()
    {
      int _guest = txtCgrgu.Text != "" ? Convert.ToInt32(txtCgrgu.Text) : 0;
      string _salesRoom = App.User.SalesRoom.srID;
      int _receipt = txtCgrID.Text != "" ? Convert.ToInt32(txtCgrID.Text) : 0;
      string _folio = txtCgrNum.Text != "" ? txtCgrNum.Text : "ALL";
      DateTime? _dateFrom = dtpCgrDFrom.Value.Value.Date;
      DateTime? _dateTo = dtpTo.Value.Value.Date;
      string _name = txtCgrGuest.Text != "" ? txtCgrGuest.Text : "ALL";
      string _reservation = txtCReservation.Text != "" ? txtCReservation.Text : "ALL";

      //Ejecutamos el procedimiento almancenado con los criterios de busqueda
      _lstGiftsReceipt = BRGiftsReceipts.GetGiftsReceipts(_guest, _salesRoom, _receipt, _folio, _dateFrom, _dateTo, _name, _reservation);
      _obsGiftsReceipt = new ObservableCollection<GiftsReceiptsShort>(_lstGiftsReceipt);

      // Si no hay recibos de regalos
      if (_lstGiftsReceipt.Count == 0)
        return false;
      else
      {
        _dsReceipts.Source = _obsGiftsReceipt;
        return true;
      }
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

      ChargeTo _chargeTo = (ChargeTo)cbogrct.SelectedItem;

      curMaxAuthGifts = CalculateMaxAuthGifts(_chargeTo.ctID, txtgrls.Text, ref blnWithMaxAuthGifts);
      txtgrMaxAuthGifts.Text = string.Format("${0}", curMaxAuthGifts);
      lblgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      txtgrMaxAuthGifts.Visibility = (blnWithMaxAuthGifts) ? Visibility.Visible : Visibility.Hidden;
      _blnValidateMaxAuthGifts = blnWithMaxAuthGifts;
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

      ChargeTo _chargeTo = (ChargeTo)cbogrct.SelectedItem;

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
          GuestStatus _guestsStatus = BRGuestStatus.GetGuestsStatus(_GuestID);
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
    private void GetGuestData(int guestID)
    {
      // validamos que se haya enviado una clave de huesped
      if (guestID == 0)
      {
        return;
      }
      // Obtenemos los datos del huesped
      _guestShort = BRGuests.GetGuestShort(guestID);

      if (_guestShort != null)
      {
        txtgrgu.Text = guestID + "";
        txtGuestName.Text = _guestShort.Name;
        txtReservationCaption.Text = _guestShort.guHReservID;
        txtAgencyCaption.Text = _guestShort.agN;
        txtProgramCaption.Text = _guestShort.pgN;

        // Obtenemos el hotel del Guest
        Guest _guest = BRGuests.GetGuestById(_guestShort.guID);
        cboHotel.SelectedValue = _guest.guHotel;
        // Cargamos su Sales Room
        cboSalesRoom.SelectedValue = _guestShort.gusr;
        // Cargamos su Location
        cbogrlo.SelectedValue = _guestShort.guls;
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
      foreach (var item in grdPayments.Items)
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

    private void LoadExchangeRates()
    {
      // Obtenemos la fecha de recibo
      DateTime dtmReceipt = dtpgrD.Value.Value.Date;

      // Cargamos los tipos de cambio de la fecha de recibo
      _lstExchangeRate = BRExchangeRate.GetExchangeRatesByDate(dtmReceipt);

      //Cargamos la moneda
      SalesRoomShort _sr = (SalesRoomShort)cboSalesRoom.SelectedItem;
      SalesRoom _salesRoom = BRSalesRooms.GetSalesRoomByID(_sr.srID);
      _SRCurrency = _salesRoom.srcu;
    }

    private void ConvertDepositToUsDlls()
    {
      decimal curDeposit = 0;
      decimal curExchangeRate = 0;
      decimal curExchangeRateMN = 0;

      // Localizamos el tipo de cambio de ese dia de la moneda en que esta el deposito
      Currency _currency = (Currency)cbogrcu.SelectedItem;

      if (_lstExchangeRate != null)
      {
        ExchangeRateShort _exchangeRate = _lstExchangeRate.Where(x => x.excu == _currency.cuID).FirstOrDefault();

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
        //if (txtgrDepositTwisted.Text != "")
        //{
        if (Convert.ToDecimal(txtgrDepositTwisted.Text) > 0)
          curDeposit = Convert.ToDecimal(txtgrDepositTwisted.Text);
        //else if (txtgrDeposit.Text != "")
        //{
        else if (Convert.ToDecimal(txtgrDeposit.Text) > 0)
          curDeposit = Convert.ToDecimal(txtgrDeposit.Text);
        // }
        else
          curDeposit = 0;
        //}

        // Monto en dolares
        txtDepositUS.Text = string.Format("{0:0.00}", Math.Round(curDeposit, 4) * Math.Round(curExchangeRate, 4));

        // Monto en moneda nacional
        txtDepositMN.Text = string.Format("{0:0.00}", (Math.Round(curDeposit, 4) * Math.Round(curExchangeRate, 4)) / Math.Round(curExchangeRateMN, 4));

      }
    }

    #region GiftsReceiptShort_Selected
    /// <summary>
    /// Obtienen la seleccion del row en el dataGrid gifts
    /// </summary>
    /// <history>
    /// [vipacheco] 08/04/2016 Created
    /// </history>
    private void GiftsReceiptShort_Selected()
    {
      if (grdReceipts.SelectedItem == null && grdReceipts.Items.Count > 0)
        grdReceipts.SelectedIndex = 0;

      GiftsReceiptsShort _grSelected = (GiftsReceiptsShort)grdReceipts.SelectedItem;
      int GiftReceiptID = _grSelected.grID;

      // Si es intercambio de regalo se muestra el titulo
      if (_grSelected.grExchange)
        brdExchange.Visibility = Visibility.Visible;
      else
        brdExchange.Visibility = Visibility.Hidden;

      _GiftReceipt = BRGiftsReceipts.GetGiftReceipt(GiftReceiptID);
      _reimpresion = _GiftReceipt.grReimpresion;  // Obtenemos la cantidad de reimpresiones que tiene realizado este gift

      grdMain.DataContext = _GiftReceipt;

      // Cargamos los regalos del recibo de regalo seleccionado
      _lstGifts = BRGiftsReceiptDetail.GetGiftsReceiptDetail(GiftReceiptID);

      _obsGifts = new ObservableCollection<GiftsReceiptDetailShort>(_lstGifts);
      _obsGiftsComplet = new ObservableCollection<GiftsReceiptDetailShort>(_lstGifts);
      _dsGiftsDetail.Source = _obsGifts;

      // Cargamos los regalos payments
      _lstPayments = BRGiftsReceiptsPayments.GetGiftsReceiptPaymentsShort(GiftReceiptID);

      _obsPayments = new ObservableCollection<GiftsReceiptPaymentShort>(_lstPayments);
      _dsPayments.Source = _obsPayments;

    }
    #endregion

    private void BookinDeposits_Loaded()
    {
      //Cargamos los depositos
      _lstDeposit = BRBookingDeposits.GetBookingDeposits(_GuestID);
      var _resultLst = _lstDeposit.Select(c => new
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

      _obsDeposits = new ObservableCollection<dynamic>(_resultLst);
      _dsBookingDeposit.Source = _obsDeposits;
    }

    private void cbogrct_Loaded(object sender, RoutedEventArgs e)
    {
      /*ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;

      if (_ChargeTo != null)
      {
        //txtTotalCxC.Text = string.Format("${}", tx);
      }
      */
    }

    private void cbogrct_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

      if (_chargeToChanged >= 1) // Esta validación se realiza para evitar el bug de doble changed del control
      {
        ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;

        if (_ChargeTo != null)
        {
          switch (_ChargeTo.ctID)
          {
            case "PR":
            case "LINER":
            case "CLOSER":
              _blnPublicOrEmpleadoCost = true;
              _useCxCCost = true;
              break;
            default:
              _blnPublicOrEmpleadoCost = false;
              _useCxCCost = false;
              break;
          }

          CalculateAllCostsPrices(_useCxCCost);

          //ReceiptGifts.CalculateCharge(_guestID, _ChargeTo, txtTotalCost, chkgrExchange, txtgrgu, ref txtgrCxCGifts,
          //                                    ref txtTotalCxC, ref txtgrCxCAdj, ref _blnValidateMaxAuthGifts, ref txtgrls,
          //                                    ref txtgrMaxAuthGifts, ref lblgrMaxAuthGifts);   
          frmCancelExternalProducts _frm2 = null;
          CalculateCharge(ref _frm2);
        }
        //ReceiptGifts.CalculateCharge(_guestID, _ChargeTo, txtTotalCost, chkgrExchange, txtgrgu, ref txtgrCxCGifts, ref txtTotalCxC,
        //                ref txtgrCxCAdj, ref _blnValidateMaxAuthGifts, ref txtgrls, ref txtgrMaxAuthGifts, ref lblgrMaxAuthGifts);
        frmCancelExternalProducts _frm = null;
        CalculateCharge(ref _frm);
      }
      _chargeToChanged++; // Para el manejador del bug del doble changed del control
    }

    private void CalculateAllCostsPrices(bool CalculateAllPrices = false)
    {
      // Recorremos los regalos
      foreach (GiftsReceiptDetailShort item in grdGifts.Items)
      {
        // Se verifica si se ingreso los regalos
        if (item.gegi != "")
        {
          // Si es encuentra el regalo
          Gift _gift = BRGifts.GetGiftId(item.gegi);
          if (_gift != null)
          {
            // Costos
            // Si se va a usar costo de empleado
            if (CalculateAllPrices)
            {
              //curC
            }
          }

        }
      }
    }

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
      Close();
    }
    #endregion

    #region ValidateNumber
    /// <summary>
    /// Valida que el texto introducido sea solamente de tipo numero
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void ValidateNumber(object sender, TextCompositionEventArgs e)
    {
      string _value = e.Text;

      // Se valida que solamente sean Digitos Numericos
      if (!char.IsDigit(Convert.ToChar(_value)))
      {
        e.Handled = true;
      }
    }
    #endregion

    #region Initialize_Date
    /// <summary>
    /// Inicializa lo DatePicker con la fecha del servidor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void Initialize_Date(object sender, RoutedEventArgs e)
    {
      dtpTo.Value = frmHost._dtpServerDate;
      dtpCgrDFrom.Value = frmHost._dtpServerDate.AddDays(-7);
    }
    #endregion

    #region btnSearch_Click
    /// <summary>
    /// Busca los Gifts Receipt con los criterios ingresados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateCriteria())
      {
        Load_Grid_GiftsReceipt();
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

      if (dtpCgrDFrom.Value.Value == null)
      {
        UIHelper.ShowMessage("Specify the Start Date.", MessageBoxImage.Information);
        blnValid = false;
      }
      else if (dtpTo.Value.Value == null)
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

    private void grdGiftsReceiptDetail_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
    {
      if (btnEdit.IsVisible)
      {
        if (_giftIDSelected != "")
        {
          DataGrid innerDataGrid = e.DetailsElement as DataGrid;
          var _result = BRGiftsReceiptsPacks.GetGiftsReceiptPackage(Convert.ToInt32(txtgrID.Text), _giftIDSelected);

          if (_result.Count > 0)
          {
            var _r = _result.Select(x => new { gkgi = x.gkgi, gkQty = x.gkQty }).ToList();
            _dsGiftsReceiptPackage.Source = _r;
          }
          else
            _dsGiftsReceiptPackage.Source = null;
        }
      }
    }

    #region ValidateDecimal
    /// <summary>
    /// Valida que el texto introducido sea decimal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 12/Abril/2016 Created
    /// </history>
    private void ValidateDecimal(object sender, TextCompositionEventArgs e)
    {
      Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
      e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
    }
    #endregion

    #region ValidateEnter_PreviewKeyDown
    /// <summary>
    /// Recalcula el deposito despues de un cambio en los textbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 15/Abril/2016 Created
    /// </history>
    private void ValidateEnter_PreviewKeyDown(object sender, KeyEventArgs e)

    {
      TextBox _controlSelected = (TextBox)sender;

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
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    private void txtgrCxCAdj_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter || e.Key == Key.Tab)
      {
        txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text) + Convert.ToDecimal(txtgrCxCAdj.Text));
      }
    }
    #endregion

    #region txtgrcxcAdj_Validate
    private void txtgrcxcAdj_Validate()
    {
      if (txtgrCxCAdj.Text != "")
      {
        txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text) + Convert.ToDecimal(txtgrCxCAdj.Text));
      }

    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Permite cancelar un recibo de regalos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (UIHelper.ShowMessage("Are you sure you want to cancel this receipt? \r\n This change can not be undone.", MessageBoxImage.Question) == MessageBoxResult.Yes)
      {
        // si no tiene regalos pendientes por cancelar en los sistemas externos
        if (CancelExternalProducts())
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
      BRGiftsReceipts.CancelGiftsReceipt(ReceiptID, frmHost._dtpServerDate);

      // Guardamos el historico del recibos de regalos
      await BRGiftsReceiptLog.SaveGiftsReceiptsLog(ReceiptID, App.User.User.peID);

      // Actualizamos los datos en pantalla
      chkgrCancel.IsChecked = true;
      txtgrCancelD.Text = $"{frmHost._dtpServerDate.Month}/{frmHost._dtpServerDate.Day}/{frmHost._dtpServerDate.Year}";

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
    private bool CancelExternalProducts()
    {
      int rowCountgeInElectronicPurse = 0, rowcountgeCancelElectronicPurse = 0, rowCountgeInPVPPromo = 0, rowCountgeCancelPVPPromo = 0;
      foreach (GiftsReceiptDetailShort item in grdGifts.Items)
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
        if (ShowCancelExternalProducts(EnumExternalProduct.expElectronicPurse, false) == MessageBoxResult.OK)
          return CancelExternalProducts();

      }
      // Si tiene regalos pendientes por cancelar en Sistur
      else if (rowCountgeInPVPPromo > rowCountgeCancelPVPPromo)
      {
        if (ShowCancelExternalProducts(EnumExternalProduct.expSisturPromotions, false) == MessageBoxResult.OK)
          return CancelExternalProducts();
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnCancelSisturPromotions_Click(object sender, RoutedEventArgs e)
    {
      // si los datos son validos
      if (ValidateCancelSisturPromotions())
      {
        // desplegamos el formulario de cancelacion de productos externos
        ShowCancelExternalProducts(EnumExternalProduct.expSisturPromotions, true);
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
      foreach (GiftsReceiptDetailShort item in grdGifts.Items)
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

    /// <summary>
    /// Despliega el formulario de cancelacion de productos del monedero electronico
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnCancelElectronicPurse_Click(object sender, RoutedEventArgs e)
    {
      // si los datos son validos
      if (ValidateCancelElectronicPurse())
      {
        // desplegamos el formulario de cancelacion de productos externos
        //----------------------------------------------------------------------------------------> Falta OJO Checar (btnCancelElectronicPurse_Click())

        //---------------------------------------------------------------------------------------->
      }
    }

    #region ValidateCancelElectronicPurse
    /// <summary>
    /// Valida si se permite cancelar los regalos de un monedero electronico
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    public bool ValidateCancelElectronicPurse()
    {
      int rowCountgeInElectronicPurse = 0, rowcountgeCancelElectronicPurse = 0;
      foreach (GiftsReceiptDetailShort item in grdGifts.Items)
      {
        rowCountgeInElectronicPurse += Convert.ToInt32(item.geInElectronicPurse);
        rowcountgeCancelElectronicPurse += Convert.ToInt32(item.geCancelElectronicPurse);
      }

      // validamos si tiene permiso especial de recibos de regalos
      if (!App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special))
      {
        UIHelper.ShowMessage("Access Denied.", MessageBoxImage.Exclamation);
        return false;
      }
      else if (rowCountgeInElectronicPurse > rowcountgeCancelElectronicPurse)
      {
        UIHelper.ShowMessage("No gifts to cancel from the electronic purse.", MessageBoxImage.Exclamation);
        return false;
      }
      return true;
    }
    #endregion

    /// <summary>
    /// Despliega el formulario del monedero electronico
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnEPurse_Click(object sender, RoutedEventArgs e)
    {
      // si los datos son validos
      if (ValidateEPurse())
      {
        //----------------------------------------------------------------------------------------> Falta OJO Checar (btnEPurse_Click())

        //---------------------------------------------------------------------------------------->
      }
    }

    #region ValidateEPurse
    /// <summary>
    /// Valida si se permite guardar los regalos en un monedero electronico
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private bool ValidateEPurse()
    {
      // Buscamos el guest con el Id correspondiente.
      Guest _guest = BRGuests.GetGuest(Convert.ToInt32(txtgrgu.Text));

      // Validamos si tiene show.
      if (!_guest.guShow)
      {
        UIHelper.ShowMessage("Invitation without show.", MessageBoxImage.Exclamation);
        return false;
      }
      return true;
    }

    #endregion

    #region btnLog_Click
    /// <summary>
    /// Despliega el historico de un recibo de regalos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      if (txtgrID.Text != "")
      {
        frmGiftsReceiptsLog _frmLog = new frmGiftsReceiptsLog(Convert.ToInt32(txtgrID.Text));
        _frmLog.ShowInTaskbar = false;
        _frmLog.Owner = this;
        _frmLog.ShowDialog();
      }
      else
        UIHelper.ShowMessage("Select a receipt", MessageBoxImage.Information);
    }
    #endregion

    #region btnExchange_Click
    /// <summary>
    /// Despliega el formulario del monedero electronico
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private void btnExchange_Click(object sender, RoutedEventArgs e)
    {
      AddReceipt(true);
    } 
    #endregion

    /// <summary>
    ///  Permite Guardar los cambios
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 16/Abril/2016 Created
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (Validate())
      {
        Save();
        Load_Record();
      }
    }

    #region Save
    /// <summary>
    /// Guarda los datos del recibo de regalos
    /// </summary>
    /// <history>
    /// [vipacheco] 30/Mayo/2016
    /// </history>
    private async void Save()
    { 
      GiftsReceipt New_GiftReceipt;

      LoadExchangeRates();
      ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;

      frmCancelExternalProducts _frm = null;
      CalculateCharge(ref _frm);
      int ID = 0;
      // Verificamos si es un Gift Receipt nuevo!
      if (string.IsNullOrEmpty(txtgrID.Text))
      {
        // Agregamos el recibo de regalos en la BD
        New_GiftReceipt = GetGiftsReceipt();
        ID = BRGiftsReceipts.SaveGiftReceipt(New_GiftReceipt); // Guardamos el ReceiptGifts y obtenemos su PK generado
        txtgrID.Text = ID + "";

        // Agregamos el recibo de regalos al grid
        GiftsReceiptsShort _grs = new GiftsReceiptsShort() { grID = ID, grNum = txtgrNum.Text, grExchange = chkgrExchange.IsChecked.Value };
        _obsGiftsReceipt.Add(_grs);
        _lstGiftsReceipt.Add(_grs);

        // Actualizamos el ID de las listas de GiftsReceiptDetail y GiftsReceiptPayment
        if (_LogGiftDetail.Count > 0)
          _LogGiftDetail.ToList().ForEach(f => f.Value.gegr = ID);

        #region GiftsReceiptsAdditional
        // si es una quiniela y es su primer recibo de regalos y el sistema no esta en modo de solo lectura
        bool _Quinella = false, _GiftsReceived = false;
        if (_GuestHost != null)
        {
          _Quinella = _GuestHost.guQuinella;
          _GiftsReceived = _GuestHost.guGiftsReceived;
        }
        else
        {
          // Cargamos los datos del huesped
          Guest _guest = BRGuests.GetGuestById(_GuestID);
          _Quinella = _guest.guQuinella;
          _GiftsReceived = _guest.guGiftsReceived;
        }

        if (_Quinella && !_GiftsReceived && App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard))
        {
          // esplegamos el formulario de generacion de recibos de regalos
          frmGiftsReceiptsAdditional _frmGifsAdditional = new frmGiftsReceiptsAdditional(this, _GuestHost.guID);
          _frmGifsAdditional.ShowInTaskbar = false;
          _frmGifsAdditional.Owner = this;
          _frmGifsAdditional.ShowDialog();
        }
        #endregion

      }
      else // Actualizamos el Gift Receipt
      {
        New_GiftReceipt = GetGiftsReceipt(true);
        ID = New_GiftReceipt.grID;
        await BREntities.OperationEntity(New_GiftReceipt, Model.Enums.EnumMode.edit);
      }

      #region SAVE GIFTS
      // Guardamos los regalos
      if (_blnInvitationGifts)  // Si las invitaciones vienen de InvitsGifts
      {
        if (_LogGiftDetail.Count > 0)
        {
          // guardamos en GiftsReceiptsC
          foreach (KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail> item in _LogGiftDetail)
          {
            await BREntities.OperationEntity(item.Value, Model.Enums.EnumMode.add);
          }
          //_LogGiftDetail.ForEach(x => await BREntities.OperationEntity(x.Value, Model.Enums.EnumMode.add));

          // Actualizamos los campos iggr de la tabla iggr de la tabla  InvitsGifts
          foreach (KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail> item in _LogGiftDetail)
          {
            InvitationGift _invitationGift = BRInvitsGifts.GetInvitGift(_GuestID, item.Value.gegi);

            if (_invitationGift != null)
              _invitationGift.iggr = ID;

            // Guardamos el cambio
            await BREntities.OperationEntity(_invitationGift, Model.Enums.EnumMode.edit);
          }
        }
      }
      else
      {
        // guardamos en GiftsReceiptsC
        foreach (KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail> item in _LogGiftDetail)
        {
          await BREntities.OperationEntity(item.Value, item.Key);
        }
      }
      #endregion

      // si se manejan promosiones de sistur
      if (ConfigHelper.GetString("UseSisturPromotions").ToUpper().Equals("TRUE"))
      {
        // Guardamos las promociones de Sistur
        SisturHelper.SavePromotionsSistur(ID, txtChangedBy.Text, App.User.User.peID);
      }

      // si se maneja cargos a habitacion en Opera
      if (ConfigHelper.GetString("UseRoomCharges").ToUpper().Equals("TRUE"))
      {
        // guardamos los cargos a habitacion en Opera
        WirePRHelper.SaveRoomChargesOpera(ID, txtChangedBy.Text);
      }

      // si se maneja promociones de Opera
      if (ConfigHelper.GetString("UsePromotions").ToUpper().Equals("TRUE"))
      {
        // guardamos las promociones de Opera
        SavePromotionsOpera(ID);
      }

      // Guardamos los pagos
      SavePayments(ID);

      // Si el recibo no esta cancelado ni cerrado
      if (!chkgrCancel.IsChecked.Value)
      {
        // Si no se esta buscando
        if (modeOpen != EnumModeOpen.Search && modeOpen != EnumModeOpen.Edit && _GuestID > 0)
        {
          // Actualizamos los datos de la invitacion
          UpdateGuest();
        }
      }

      brdExchange.Visibility = Visibility.Hidden;

      // Actualizamos las banderas
      _newExchangeGiftReceipt = false;
      _newGiftReceipt = false;
      _edition = false;
      // Reiniciamos las variable globales
      _LogGiftDetail.Clear();
      _lstPaymentsDelete.Clear();

      // Guardamos el historico de recibo de regalos
      await BRGiftsReceiptLog.SaveGiftsReceiptsLog(Convert.ToInt32(txtgrID.Text), txtChangedBy.Text);

      Controls_Reading_Mode();
      /*GiftsReceipt _giftReceipt;

      LoadExchangeRates();
      ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;

      frmCancelExternalProducts _frm = null;
      CalculateCharge(ref _frm);
      int ID = 0;
      // Verificamos si es un Gift Receipt nuevo!
      if (string.IsNullOrEmpty(txtgrID.Text))
      {
        // Agregamos el recibo de regalos en la BD
        _giftReceipt = GetGiftsReceipt();
        ID = BRGiftsReceipts.SaveGiftReceipt(_giftReceipt); // Guardamos el ReceiptGifts y obtenemos su PK generado
        txtgrID.Text = ID + "";

        // Agregamos el recibo de regalos al grid
        GiftsReceiptsShort _grs = new GiftsReceiptsShort() { grID = ID, grNum = txtgrNum.Text, grExchange = chkgrExchange.IsChecked.Value };
        _obsGiftsReceipt.Add(_grs);
        _lstGiftsReceipt.Add(_grs);

        // Actualizamos el ID de las listas de GiftsReceiptDetail y GiftsReceiptPayment
        if (_LogGiftDetail.Count > 0)
          _LogGiftDetail.ToList().ForEach(f => f.Value.gegr = ID);

        #region GiftsReceiptsAdditional
        // si es una quiniela y es su primer recibo de regalos y el sistema no esta en modo de solo lectura
        bool _Quinella = false, _GiftsReceived = false;
        if (_rowHost != null)
        {
          _Quinella = _rowHost.guQuinella;
          _GiftsReceived = _rowHost.guGiftsReceived;
        }
        else
        {
          // Cargamos los datos del huesped
          Guest _guest = BRGuests.GetGuestById(_GuestID);
          _Quinella = _guest.guQuinella;
          _GiftsReceived = _guest.guGiftsReceived;
        }

        if (_Quinella && !_GiftsReceived && App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard))
        {
          // esplegamos el formulario de generacion de recibos de regalos
          frmGiftsReceiptsAdditional _frmGifsAdditional = new frmGiftsReceiptsAdditional(this, _rowHost.guID);
          _frmGifsAdditional.ShowInTaskbar = false;
          _frmGifsAdditional.Owner = this;
          _frmGifsAdditional.ShowDialog();
        }
        #endregion

      }
      else // Actualizamos el Gift Receipt
      {
        _giftReceipt = GetGiftsReceipt(true);
        ID = _giftReceipt.grID;
        BREntities.OperationEntity<Model.GiftsReceipt>(_giftReceipt, Model.Enums.EnumMode.edit);
      }

      #region SAVE GIFTS
      // Guardamos los regalos
      if (_blnInvitationGifts)  // Si las invitaciones vienen de InvitsGifts
      {
        if (_LogGiftDetail.Count > 0)
        {
          // guardamos en GiftsReceiptsC
          _LogGiftDetail.ForEach(x => BREntities.OperationEntity(x.Value, Model.Enums.EnumMode.add));

          // Actualizamos los campos iggr de la tabla iggr de la tabla  InvitsGifts
          foreach (KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail> item in _LogGiftDetail)
          {
            InvitationGift _invitationGift = BRInvitsGifts.GetInvitGift(_GuestID, item.Value.gegi);

            if (_invitationGift != null)
              _invitationGift.iggr = ID;

            // Guardamos el cambio
            BREntities.OperationEntity(_invitationGift, Model.Enums.EnumMode.edit);
          }
        }
      }
      else
      {
        // guardamos en GiftsReceiptsC
        _LogGiftDetail.ForEach(x => BREntities.OperationEntity(x.Value, x.Key));
      }
      #endregion

      // si se manejan promosiones de sistur
      if (ConfigHelper.GetString("UseSisturPromotions").ToUpper().Equals("TRUE"))
      {
        // Guardamos las promociones de Sistur
        SavePromotionsSistur(ID);
      }

      // si se maneja cargos a habitacion en Opera
      if (ConfigHelper.GetString("UseRoomCharges").ToUpper().Equals("TRUE"))
      {
        // guardamos los cargos a habitacion en Opera

      }

      // si se maneja promociones de Opera
      if (ConfigHelper.GetString("UsePromotions").ToUpper().Equals("TRUE"))
      {
        // guardamos las promociones de Opera

      }

      // Guardamos los pagos
      SavePayments(ID);

      // Si el recibo no esta cancelado ni cerrado
      if (!chkgrCancel.IsChecked.Value)
      {
        // Si no se esta buscando
        if (modeOpen != EnumModeOpen.Search && modeOpen != EnumModeOpen.Edit && _GuestID > 0)
        {
          // Actualizamos los datos de la invitacion
          UpdateGuest();
        }
      }

      // Guardamos el historico de recibo de regalos
      BRGiftsReceiptLog.SaveGiftsReceiptsLog(Convert.ToInt32(txtgrID.Text), txtChangedBy.Text);

      //Activamos y desactivamos los controles necesarios
      ControlsReadOnly(true, true);
      txtgrDeposit.IsReadOnly = true;
      txtgrDepositTwisted.IsReadOnly = true;
      txtChangedBy.Text = "";
      txtPwd.Password = "";

      //// Habilitamos los botones correspondientes
      ControlBottonEdition(Visibility.Visible, Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Visible,
                           EnableCancel(), Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
      ControlEnable(false, false, false, false, false, false, false, false, false);
      controlEditionGifts(Visibility.Hidden, Visibility.Hidden);

      if (modeOpen == EnumModeOpen.Search || modeOpen == EnumModeOpen.Edit)
        ControlVisibility(Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden);

      // Actualizamos las banderas
      _newExchangeGiftReceipt = false;
      _newGiftReceipt = false;
      _edition = false;
      // Reiniciamos las variable globales
      _LogGiftDetail.Clear();
      _lstPaymentsDelete.Clear();  */
    }
  #endregion

  #region UpdateGuest
  /// <summary>
  /// Actualiza los datos de la invitacion
  /// </summary>
  /// <history>
  /// [vipacheco] 09/Mayo/2016 Created
  /// </history>
  private async void UpdateGuest()
    {
      bool Update = false;
      Guest _guest = BRGuests.GetGuest(_GuestID);

      //Actualizamos la clave del recibo de deposito
      if (_guest.guDepositgr == null && Convert.ToDecimal(txtgrDeposit.Text) != 0)
      {
        _guest.guDepositgr = Convert.ToInt32(txtgrID.Text);
        Update = true;
      }

      // Actualizamos la clave del recibo de servicio de taxi de llegada
      if (_guest.guTaxiIngr == null)
      {
        _guest.guTaxiIngr = Convert.ToInt32(txtgrID.Text);
        Update = true;
      }

      // Actualizamos la clave del recibo de servicio de taxi de salida
      if (_guest.guTaxiOutgr == null && Convert.ToDecimal(txtgrTaxiOut.Text) != 0)
      {
        _guest.guTaxiOutgr = Convert.ToInt32(txtgrID.Text);
        Update = true;
      }

      // Indica que ya se recibieron los regalos
      if (!_guest.guGiftsReceived)
      {
        _guest.guGiftsReceived = true;
        Update = true;
      }

      // Actualizamos en la base de datos
      if (Update)
        await BREntities.OperationEntity(_guest, Model.Enums.EnumMode.edit);

    }
    #endregion

    #region SavePayments
    /// <summary>
    /// Realiza las validaciones de los campos introducidos en el grid de Payments y los guarda en la BD
    /// </summary>
    /// <param name="GiftReceipt"></param>
    /// <history>
    /// [vipacheco] 27/Abril/2016 Created
    /// </history>
    private async void SavePayments(int GiftReceipt)
    {
      foreach (var item in grdPayments.Items)
      {
        GiftsReceiptPaymentShort _selected;

        if (item is GiftsReceiptPaymentShort)
        {
          _selected = (GiftsReceiptPaymentShort)item;

          // Construimos la entidad tipo GiftsReceiptsPayments
          GiftsReceiptPayment _newGiftsPayments = new GiftsReceiptPayment
          {
            gypt = _selected.gypt,
            gycu = _selected.gycu,
            gyAmount = _selected.gyAmount,
            gyRefund = _selected.gyRefund,
            gysb = string.IsNullOrEmpty(_selected.gysb) ? null : _selected.gysb,
            gype = _selected.gype,
            gybk = _selected.gybk,
          };
          if (_selected.gygr == 0)
            _newGiftsPayments.gygr = GiftReceipt;

          if (_newGiftReceipt || _newExchangeGiftReceipt) // Si es de nueva creacion se agregan todos.
          {
            await BREntities.OperationEntity(_newGiftsPayments, Model.Enums.EnumMode.add);
          }
          else // Si se estan editando
          {
            // Verificamos si el Gift se encuentra en la BD.
            GiftsReceiptPayment _giftPayment = BRGiftsReceiptsPayments.GetGiftReceiptPayment(_selected.gygr, _selected.gyID);

            if (_giftPayment != null) // Si existe este registro se verifica si algun campo se edito
            {
              if (_giftPayment.gyAmount != _selected.gyAmount || _giftPayment.gypt != _selected.gypt ||
                  _giftPayment.gybk != _selected.gybk || _giftPayment.gycu != _selected.gycu || _giftPayment.gype != _selected.gype ||
                  _giftPayment.gyRefund != _selected.gyRefund || _giftPayment.gysb != _selected.gysb)
              {
                _newGiftsPayments.gyID = _selected.gyID;
                _newGiftsPayments.gygr = _selected.gygr;
                await BREntities.OperationEntity(_newGiftsPayments, Model.Enums.EnumMode.edit);
              }
            }
            else // registro nuevo
            {
              await BREntities.OperationEntity(_newGiftsPayments, Model.Enums.EnumMode.add);
            }

            // Se verifica si se elimino alguno de la lista original
            if (_lstPaymentsDelete.Count > 0)
            {
              foreach (GiftsReceiptPaymentShort _item in _lstPaymentsDelete)
              {
                await BREntities.OperationEntity(_item, Model.Enums.EnumMode.deleted);
              }
              //_lstPaymentsDelete.ForEach(x => BREntities.OperationEntity(x, Model.Enums.EnumMode.deleted));
            }
          }
        }
      }
    }
    #endregion

    #region GetGiftsReceipt
    /// <summary>
    /// Obtiene el Gifts Receipt con sus respectivos parametros
    /// </summary>
    /// <param name="Edit"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Abril/2016 Created
    /// </history>
    private GiftsReceipt GetGiftsReceipt(bool Edit = false)
    {
      GiftsReceipt _new = new GiftsReceipt()
      {
        grNum = string.IsNullOrEmpty(txtgrNum.Text) ? null : txtgrNum.Text,
        grD = dtpgrD.Value.Value.Date,
        grgu = Convert.ToInt32(txtgrgu.Text),
        grExchange = _newExchangeGiftReceipt == true ? true : false,
        grGuest = string.IsNullOrEmpty(txtgrGuest.Text) ? null : (txtgrGuest.Text.Length > 20 ? txtgrGuest.Text.Substring(0, 20) : txtgrGuest.Text),
        grPax = txtgrPax.Text != "" ? Convert.ToDecimal(txtgrPax.Text) : 0,
        grHotel = cboHotel.SelectedValue.ToString(),
        grRoomNum = string.IsNullOrEmpty(txtgrRoomNum.Text) ? null : txtgrRoomNum.Text,
        grpe = txtgrpe.Text,
        grlo = cbogrlo.SelectedValue.ToString(),
        grls = cbogrlo.SelectedValue.ToString(),
        grsr = cboSalesRoom.SelectedValue.ToString(),
        grWh = cboSalesRoom.SelectedValue.ToString(),
        grMemberNum = string.IsNullOrEmpty(txtgrMemberNum.Text) ? null : txtgrMemberNum.Text,
        grHost = txtgrHost.Text,
        grComments = string.IsNullOrEmpty(txtgrComments.Text) ? null : txtgrComments.Text,
        grDeposit = txtgrDeposit.Text != "" ? Convert.ToDecimal(txtgrDeposit.Text.Trim(new char[] { '$' })) : 0,
        grDepositTwisted = txtgrDepositTwisted.Text != "" ? Convert.ToDecimal(txtgrDepositTwisted.Text.Trim(new char[] { '$' })) : 0,
        grcu = cbogrcu.SelectedValue.ToString(),
        grcxcPRDeposit = txtgrCxCPRDeposit.Text != "" ? Convert.ToDecimal(txtgrCxCPRDeposit.Text) : 0,
        grcucxcPRDeposit = cbogrcuCxCPRDeposit.SelectedValue.ToString(),
        grCxCClosed = false,   /// Preguntar
        grExchangeRate = Math.Round(_lstExchangeRate.Where(w => w.excu == "MEX").Select(s => s.exExchRate).Single(), 4),
        grct = cbogrct.SelectedValue.ToString(),
        grMaxAuthGifts = Convert.ToDecimal(txtgrMaxAuthGifts.Text.Trim(new char[] { '$' })),
        grcxcGifts = Convert.ToDecimal(txtgrCxCGifts.Text.Trim(new char[] { '$' })),
        grcxcAdj = Convert.ToDecimal(txtgrCxCAdj.Text),
        grcxcComments = string.IsNullOrEmpty(txtgrCxCComments.Text) ? null : txtgrCxCComments.Text,
        grTaxiIn = 0,  /// Preguntar
        grTaxiOut = Convert.ToDecimal(txtgrTaxiOut.Text),
        grCancel = chkgrCancel.IsChecked.Value,
        grClosed = false,  /// Preguntar
        grCxCAppD = null,  /// Preguntar
        grTaxiOutDiff = Convert.ToDecimal(txtgrTaxiOutDiff.Text),
        grGuest2 = string.IsNullOrEmpty(txtgrGuest2.Text) ? null : txtgrGuest2.Text,
        grpt = cbogrpt.SelectedValue.ToString(),
        grReimpresion = Convert.ToByte(_reimpresion),
        grrm = null,
        grAuthorizedBy = null,
        grAmountToPay = null,
        grup = null,
        grcxcTaxiOut = Convert.ToDecimal(txtgrTaxiOut.Text),
        grcucxcTaxiOut = cbogrcuCxCTaxiOut.SelectedValue.ToString(),
        grcxcAuthComments = null,
        grCancelD = null,
        grAmountPaid = 0,
        grBalance = 0
      };

      if (Edit) // TRUE - EDIT | FALSE - NEW  --> Si es edit se le agrega el ID
      {
        _new.grID = Convert.ToInt32(txtgrID.Text);
      }

      return _new;
    }
    #endregion

    #region validateFilds
    /// <summary>
    /// Realiza las validaciones necesarias sobre los campos obligatorios
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 27/Abril/2016 Created
    /// </history>
    private bool validateFilds()
    {
      if (string.IsNullOrEmpty(cbogrlo.SelectedValue.ToString()))
      {
        UIHelper.ShowMessage("Specify the location", MessageBoxImage.Information);
        return false;
      }
      else if (string.IsNullOrEmpty(cboSalesRoom.SelectedValue.ToString()))
      {
        UIHelper.ShowMessage("Specify Sales Room", MessageBoxImage.Information);
        return false;
      }
      else if (IsClosed(dtpgrD.Value.Value.Date, _dtpClose))
      {
        UIHelper.ShowMessage("It's not allowed to make gifts receipts for a closed date.", MessageBoxImage.Information);
        return false;
      }
      else if (string.IsNullOrEmpty(cbogrpe.SelectedValue.ToString()) || string.IsNullOrEmpty(txtgrpe.Text))
      {
        cbogrpe.Focus();
        txtgrpe.Focus();
        UIHelper.ShowMessage("Who offered the gifts?", MessageBoxImage.Information);
        return false;
      }
      else if (string.IsNullOrEmpty(cbogrHost.SelectedValue.ToString()) || string.IsNullOrEmpty(txtgrHost.Text))
      {
        txtgrHost.Focus();
        cbogrHost.Focus();
        UIHelper.ShowMessage("Specify the host(ess).", MessageBoxImage.Information);
        return false;
      }
      else if (string.IsNullOrEmpty(txtgrgu.Text))
      {
        UIHelper.ShowMessage("Specify the guest ID", MessageBoxImage.Information);
        return false;
      }

      return true;
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
      if (!ValidateChangedBy(txtChangedBy, txtPwd))
        return false;
      else if (!validateFilds())
        return false;
      else if (!ValidatePayments())
        return false;
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
      SalesRoomShort _sr = (SalesRoomShort)cboSalesRoom.SelectedItem;

      ValidationData _validation = BRGiftsReceipts.ValidateGiftsReceipt(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), Convert.ToInt32(txtgrgu.Text),
                                                                        cbogrlo.SelectedValue.ToString(), _sr.srID, txtgrHost.Text, txtgrpe.Text);

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
            cboSalesRoom.Focus();
            break;
          case "Location":
            cbogrlo.Focus();
            break;
          case "Personnel":
            txtgrpe.Focus();
            break;
          case "GiftsHost":
            txtgrHost.Focus();
            break;
        }
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
      // Si se debe validar el monto maximo de regalos
      if (_blnValidateMaxAuthGifts)
      {
        // validamos el monto maximo de regalos
        return Gifts.ValidateMaxAuthGifts(txtTotalCost.Text, txtgrMaxAuthGifts.Text);
      }

      // Si hay GuestStatus o se debe validar
      if (_blnApplyGuestStatusValidation)
      {
        return ValidateGiftsGuestStatus();
      }

      // validamos que los pagos cubran el importe a pagar
      if (Convert.ToDecimal(txtTotalToPay.Text.Trim(new char[] { '$' })) > 0 &&
               Convert.ToDecimal(txtTotalToPay.Text.Trim(new char[] { '$' })) > Convert.ToDecimal(txtTotalPayments.Text.Trim(new char[] { '$' })))
      {
        UIHelper.ShowMessage("The payments do not cover the amount to pay", MessageBoxImage.Exclamation);
        return false;
      }

      // validamos que los datos del recibo de regalos existan
      else if (!ValidateExist())
        return false;

      return true;
    }
    #endregion

    private void Expander_Expanded(object sender, RoutedEventArgs e)
    {
      if (btnEdit.IsVisible)
      {
        for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
          if (vis is DataGridRow)
          {
            var row = (DataGridRow)vis;
            GiftsReceiptDetailShort _gifts = (GiftsReceiptDetailShort)row.DataContext;
            _giftIDSelected = _gifts.gegi;
            row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            break;
          }
      }
    }

    private void Expander_Collapsed(object sender, RoutedEventArgs e)
    {
      if (btnEdit.IsVisible)
      {
        for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
          if (vis is DataGridRow)
          {
            var row = (DataGridRow)vis;
            _giftIDSelected = "";
            _dsGiftsReceiptPackage.Source = null;
            row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            break;
          }
      }
    }

    #region ValidatePayments
    /// <summary>
    /// Realiza las validaciones general sobre el Grid Payments
    /// </summary>
    /// <returns> True - Todo bien || False - Algun campo incorrecto o faltante </returns>
    /// <history>
    /// [vipacheco] 05/Mayo/2016 Created
    /// </history>                                                                        
    private bool ValidatePayments()
    {
      if (!ValidateGridPayments())
        return false;
      //string message = "";
      //if (!GridHelper.HasRepeatedItems(grdPayments, "Payments", "Payment", ref message, new List<string> { "gycu", "gypt", "gysb" }))
      //{
      //  UIHelper.ShowMessage(message, MessageBoxImage.Information);
      //  return false;
      //}
      if (!ValidateGridPaymentsBySource())
        return false;

      return true;
    }
    #endregion

    #region ValidateGridPaymentsBySource
    /// <summary>
    /// Valida los pagos segun su origen
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 15/Mayo/2016
    /// </history>
    private bool ValidateGridPaymentsBySource()
    {
      foreach (var item in grdPayments.Items)
      {
        GiftsReceiptPaymentShort _current;

        if (item is GiftsReceiptPaymentShort)
          _current = (GiftsReceiptPaymentShort)item;
        else
          break;

        // Si se ingreso la cantidad pagada
        if (_current.gyAmount > 0)
        {
          switch (_current.gysb)
          {
            case "DEP":
              // Verificamos que el pago tenga una forma de pago valida
              if (_current.gypt != "CC" && _current.gypt != "CS" && _current.gypt != "TC")
              {
                UIHelper.ShowMessage("Payment type invalid for source ", MessageBoxImage.Information);
                return false;
              }
              // validamos la moneda
              // solo se permite tarjeta de credito en dolares americanos o pesos mexicanos
              else if (_current.gypt == "CC" && (_current.gycu != "US" || _current.gycu != "MEX"))
              {
                UIHelper.ShowMessage("Currency invalid for source ", MessageBoxImage.Information);
                return false;
              }
              break;
            case "CXC":
            case "MK":
              if (_current.gypt != "CS")
              {
                UIHelper.ShowMessage("Payment type invalid for source ", MessageBoxImage.Information);
                return false;
              }
              break;
          }

          // Validamos si es pago con CC y se anexe el banco
          if (_current.gypt == "CC" && _current.gybk == "")
          {
            UIHelper.ShowMessage("For payments with credit card select a bank option.", MessageBoxImage.Information);
            return false;
          }
        }
      }

      return true;
    }
    #endregion

    #region ValidateGridPayments
    /// <summary>
    /// valida los datos del grid
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 25/Abril/2016 Created
    /// </history>
    private bool ValidateGridPayments()
    {
      int i = 0, j = 0;
      string currency = "", Payment = "", Source = "";

      // Se valida si un elemento esta repetido
      string message = "Payments must not be repeated.\r\n Payment repetead is ";

      grdPayments.IsReadOnly = true;
      // si esta repetido algun elemento
      foreach (GiftsReceiptPaymentShort _gr in grdPayments.Items)
      {
        foreach (GiftsReceiptPaymentShort _gr2 in grdPayments.Items)
        {
          if (i != j)
          {
            if (_gr.gycu == _gr2.gycu) // si se ingreso el mismo currency
              currency = frmHost._lstCurrencies.Where(x => x.cuID == _gr.gycu).Select(s => s.cuN).First();
            if (_gr.gypt == _gr2.gypt) // Si se ingreso el mismo payment types
              Payment = frmHost._lstPaymentsType.Where(x => x.ptID == _gr.gypt).Select(s => s.ptN).First();
            if (_gr.gysb == _gr2.gysb) // Si se ingreso el mismo Source Payments
              Source = frmHost._lstSourcePayments.Where(x => x.sbID == _gr.gysb).Select(s => s.sbN).First();
          }

          if (currency != "" && Payment != "" && Source != "")
          {
            message += currency + " " + Payment + " " + Source;
            grdPayments.IsReadOnly = false;
            UIHelper.ShowMessage(message, MessageBoxImage.Information);
            return false;
          }
          j++;
        }
        j = 0;
        i++;
      }
      grdPayments.IsReadOnly = false;

      return true;
    }
    #endregion

    #region ValidateGiftsGuestStatus
    /// <summary>
    /// Valida la informacion del GuestStaus x los regalos || Valida los regalos y el GuestStatus
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    private bool ValidateGiftsGuestStatus()
    {
      int iToursUsed, iDiscsUsed, iTourAllowed, iTours, iTCont = 0, iDCont = 0, iMaxTours, iAdults = 0, iMinors = 0, TotPax = 0;
      decimal iPax, iDiscAllowed, iDisc;
      bool? blnDisc;
      string strMsg = "";

      // Asignamos los valores del GuestStatus para validar
      iMaxTours = (int)_GuesStatusInfo.gsMaxQtyTours;
      iToursUsed = _GuesStatusInfo.TourUsed;
      blnDisc = _GuesStatusInfo.gsAllowTourDisc;
      iDiscsUsed = _GuesStatusInfo.DiscUsed;
      iPax = _GuesStatusInfo.guPax;

      // Calculamos el total Pax
      CalculateAdultsMinorsByPax(iPax, ref iAdults, ref iMinors);
      TotPax = iAdults + iMinors;

      // Los Tours permitidos
      iTourAllowed = iMaxTours - iToursUsed;
      iTours = iTourAllowed;

      // Validamos con cada registro de tour
      foreach (GiftsReceiptDetailShort _item in grdGifts.Items)
      {
        Gift _giftResult = frmHost._lstGifts.Where(x => x.giID == _item.gegi).SingleOrDefault();

        if (_giftResult != null)
        {
          // Evaluamos si son de toures y con descuento
          if (_giftResult.gigc == "TOURS" && !(bool)_giftResult.giDiscount)
          {
            iTours += iTours - (_giftResult.giQty * _item.geQty);
            iTCont += iTCont + (_giftResult.giQty * _item.geQty);
          }
        }
      }

      // Los descuentos permitidos son los restantes de los PAX restantes
      iDiscAllowed = iPax - iMaxTours;
      iDiscAllowed = iDiscAllowed - iDiscsUsed;
      iDisc = iDiscAllowed;

      // Validamos con cada registro de descuentos
      foreach (GiftsReceiptDetailShort _item in grdGifts.Items)
      {
        Gift _giftResult = frmHost._lstGifts.Where(x => x.giID == _item.gegi).SingleOrDefault(); //  BRGifts.GetGiftId(_item.gegi);

        if (_giftResult != null)
        {
          if (_giftResult.gigc == "TOURS" && (bool)_giftResult.giDiscount)
          {
            iDisc = iDisc - (_giftResult.giQty * _item.geQty);
            iDCont = iDCont + (_giftResult.giQty * _item.geQty);
          }
        }
      }

      //Revisamos el remanente de la revision de Gifts
      if (iTours < 0)
        strMsg = "The maximum number of tours " + iTourAllowed + " has been exceeded. \r\n There are " + iTCont + " tours on this receipt";

      if (iDisc < 0 && strMsg == "")
        strMsg = "The maximum number of discount tours " + iDiscAllowed + " has been exceeded.\r\n There are " + iDCont + " discount tours on this receipt";


      //Revisamos el remanente de la revision de Gifts
      if (strMsg != "")
      {
        UIHelper.ShowMessage(strMsg, MessageBoxImage.Exclamation);
        return false;
      }
      else
        return true;
    }
    #endregion

    #region CalculateAdultsMinorsByPax
    /// <summary>
    /// Calcula el numero de adultos y menores en base al Pax
    /// </summary>
    /// <param name="pcurPax"></param>
    /// <param name="piAdults"></param>
    /// <param name="piMinors"></param>
    /// <history>
    /// [vipacheco] 10/Mayo/2016 Created
    /// </history>
    private void CalculateAdultsMinorsByPax(decimal pcurPax, ref int piAdults, ref int piMinors)
    {
      piAdults = Convert.ToInt32(pcurPax);
      piMinors = ((int)pcurPax - piAdults) * 10;
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
      cbogrcu.IsEnabled = pblnEnable;
      cbogrpt.IsEnabled = pblnEnable;

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
      if (IsClosed(dtpgrD.Value.Value.Date, _dtpClose))
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
          if (dtpgrD.Value.Value.Date != frmHost._dtpServerDate && !App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Special))//  .Permissions.GiftsReceipts < perSpecial Then
            return false;
        }
      }
      return true;
    }
    #endregion

    #region IsClosed
    /// <summary>
    /// Determina si una entidad esta cerrada
    /// </summary>
    /// <param name="date"></param>
    /// <param name="dateClosed"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016
    /// </history>
    private bool IsClosed(DateTime date, DateTime? dateClosed)
    {
      // Si la fecha es menor o igual a la fecha de cierre
      if (date <= _dtpClose)
      {
        return true;
      }

      return false;
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
    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      AddReceipt();
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
    private void btnUndo_Click(object sender, RoutedEventArgs e)
    {
      //Controls_Reading_Mode();

      // Si no hay recibo de regalos
      if (grdReceipts.Items.Count == 0)
      {
        // Si no se esta buscando
        if (_GuestID > 0)
          Close();
        else
        {
          Load_Record();
          brdExchange.Visibility = Visibility.Hidden;
          _newExchangeGiftReceipt = false;
          if (chkgrExchange.IsChecked.Value == true)
            chkgrExchange.IsChecked = false;
        }
      }
      // Si hay recibos de regalos
      else
      {
        Load_Record();
        brdExchange.Visibility = Visibility.Hidden;
        _newExchangeGiftReceipt = false;
      }
      grdPayments.IsReadOnly = true;

      // Reiniciamos las variable globales
      _LogGiftDetail.Clear();
      _lstPaymentsDelete.Clear();
      _edition = false;
      _newExchangeGiftReceipt = false;
      _newGiftReceipt = false;

    }
    #endregion

    #region btnAddGift_Click
    /// <summary>
    /// Metodo que invoca el formulario para agregar un nuevo Gift Detail
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 18/Abril/2016 Created
    /// </history>
    private void btnAddGift_Click(object sender, RoutedEventArgs e)
    {
      GiftsReceiptsShort _giftReceipt = (GiftsReceiptsShort)grdReceipts.SelectedItem;

      frmGiftsReceiptsDetail _frmGiftsDetail = new frmGiftsReceiptsDetail(ref _obsGifts, _obsGiftsComplet, _GuestID, _giftReceipt, null, _blnPublicOrEmpleadoCost);
      _frmGiftsDetail.Owner = this;
      _frmGiftsDetail.modeOpen = EnumModeOpen.Add;
      _frmGiftsDetail.ShowInTaskbar = false;
      _frmGiftsDetail.ShowDialog();

      ReceiptGifts.CalculateTotalGifts(grdGifts, EnumGiftsType.ReceiptGifts, ref txtTotalCost, ref txtTotalPrice, ref txtTotalToPay);
    }
    #endregion

    #region cbogrpe_SelectionChanged
    private void cbogrpe_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      PersonnelShort _personnel = (PersonnelShort)cbogrpe.SelectedItem;

      if (_personnel != null)
        txtgrpe.Text = _personnel.peID;

    }
    #endregion

    #region cbogrHost_SelectionChanged
    private void cbogrHost_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      PersonnelShort _personnel = (PersonnelShort)cbogrHost.SelectedItem;

      if (_personnel != null)
        txtgrHost.Text = _personnel.peID;

    }
    #endregion

    #region Update_LostFocus
    private void Update_LostFocus(object sender, RoutedEventArgs e)
    {
      CalculateTotalPayments();
    }
    #endregion

    #region txtgrgu_LostFocus
    /// <summary>
    /// Actualiza los campos del guest
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 20/Abril/2016 Created
    /// </history>
    private void txtgrgu_LostFocus(object sender, RoutedEventArgs e)
    {
      // Verificamos que el Guest ID exista
      if (txtgrgu.Text != "")
      {

        GetGuestData(Convert.ToInt32(txtgrgu.Text));

        if (_guestShort == null)
        {
          UIHelper.ShowMessage("Guest ID not found", MessageBoxImage.Information);
          return;
        }
        else
        {
          cbogrlo.SelectedValue = _guestShort.guls;
          txtgrls.Text = _guestShort.guls;
          txtgrGuest.Text = txtGuestName.Text;
          _GuestID = Convert.ToInt32(txtgrgu.Text);
        }
      }
    }
    #endregion

    private void dgGiftsReceiptPaymentShort_AddingNewItem(object sender, AddingNewItemEventArgs e)
    {
      e.NewItem = new GiftsReceiptPaymentShort { gycu = "US", gypt = "CS", gyAmount = 1, gysb = "CL", gype = App.User.User.peID, UserName = App.User.User.peN };
    }

    private void dgGiftsReceiptPaymentShort_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (e.Command == DataGrid.DeleteCommand)
      {
        if (!(MessageBox.Show("Are you sure you want to delete?", "Please confirm.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
        {
          // Cancel Delete.
          e.Handled = true;
        }
        else
        {
          GiftsReceiptPaymentShort _deleteGift = (GiftsReceiptPaymentShort)grdPayments.SelectedItem;
          _lstPaymentsDelete.Add(_deleteGift);
        }
      }
    }

    #region txtgrgu_PreviewKeyDown
    private void txtgrgu_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        txtgrgu_LostFocus(null, null);
      }
    }
    #endregion

    #region btnRemoveGift_Click
    /// <summary>
    /// Funcion que elimina uno o varios row seleccionados del grid Gifts Detail
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRemoveGift_Click(object sender, RoutedEventArgs e)
    {
      if (grdGifts.SelectedItems.Count == 0)
      {
        UIHelper.ShowMessage("Select a row!", MessageBoxImage.Information);
        return;
      }

      // Obtenemos los index de los row a eliminar
      string giftsNoDelete = "";
      List<GiftsReceiptDetailShort> indexRemove = new List<GiftsReceiptDetailShort>();
      foreach (GiftsReceiptDetailShort item in grdGifts.SelectedItems)
      {
        GiftsReceiptDetailShort _selected;
        if (item is GiftsReceiptDetailShort)
        {
          _selected = item;

          if (_selected.geInPVPPromo)
            giftsNoDelete += frmHost._lstGifts.Where(x => x.giID == _selected.gegi).Select(s => s.giN).First() + "\r\n";
          else
            indexRemove.Add(_selected);
        }
      }

      //Verificamos si algun gift de los seleccionados no se elimino
      if (giftsNoDelete != "")
        UIHelper.ShowMessage("You can not delete the gifts: \r\n" + giftsNoDelete + " because have been given in Sistur promotions", MessageBoxImage.Information);

      // eliminamos los row seleccionados
      for (int i = 0; i <= indexRemove.Count - 1; i++)
      {
        GiftsReceiptDetailShort item = indexRemove[i];

        if (_newExchangeGiftReceipt || _newGiftReceipt)
          _obsGifts.Remove(item);
        else
        {
          var _cointains = _obsGiftsComplet.Where(x => x.gegi == item.gegi).SingleOrDefault(); // Verificamos si existe en la lista inicial

          // si se encuentra
          if (_cointains != null)
          {
            if (modeOpen == EnumModeOpen.PreviewEdit || modeOpen == EnumModeOpen.Edit)
            {
              GiftsReceiptDetail _deleteGift = BRGiftsReceiptDetail.GetGiftReceiptDetail(item.gegr, item.gegi);

              // Verificamos que no se encuentre en el log de acciones
              List<KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>> _actionpreview = _LogGiftDetail.Where(x => x.Value.gegi == _deleteGift.gegi).ToList();

              if (_actionpreview != null && _actionpreview.Count > 0)
              {
                // eliminamos todas las acciones anteriores
                foreach (KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail> current in _actionpreview)
                {
                  _LogGiftDetail.Remove(current);
                }
              }
              _LogGiftDetail.Add(new KeyValuePair<Model.Enums.EnumMode, GiftsReceiptDetail>(Model.Enums.EnumMode.deleted, _deleteGift));
            }
          }

          _obsGifts.Remove(item);
        }
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
          BRGuestsPromotions.SaveGuestPromotion(ReceiptID, Current.gegi, Current.giPromotionOpera, Current.grgu, Current.geQty, frmHost._dtpServerDate.Date);

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
    private MessageBoxResult ShowCancelExternalProducts(EnumExternalProduct ExternalProduct, bool Exchange)
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
                                                                                           _blnValidateMaxAuthGifts,
                                                                                           _useCxCCost,
                                                                                           Exchange,
                                                                                           this
                                                                                           );
      _frmCancelExternalProducts.ShowInTaskbar = false;
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
            _obsGiftsReceipt.Add(_grs);
            _lstGiftsReceipt.Add(_grs);
          }
        }
        // Si se debo cancelar el recibo
        else
        {
          // si se pudo cancelar los regalos
          if (_frmCancelExternalProducts._Cancelled)
          {
            //refrescamos los regalos del recibo
            Load_Gift_Of_GiftsReceipt(Convert.ToInt32(txtgrID.Text));
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
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
        await BRReimpresionMotives.UpdateReimpresionMotive(Convert.ToInt32(txtgrID.Text), iMotive);
      }
      else
      {
        // actualizamos el contador de reimpresion
        await BRReimpresionMotives.UpdateReimpresionNumber(Convert.ToInt32(txtgrID.Text));
      }

      // guardamos el historico del recibos de regalos
      await BRGiftsReceiptLog.SaveGiftsReceiptsLog(Convert.ToInt32(txtgrID.Text), App.User.User.peID);
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
    private Visibility EnableCancel()
    {
      // si esta cancelado
      if (chkgrCancel.IsChecked.Value)
        return Visibility.Hidden;
      // si no esta cancelado
      else
      {
        // Si el recibo es de una fecha posterior a la fecha de cierre
        if ((IsClosed(dtpgrD.Value.Value.Date, _dtpClose)))
          return Visibility.Hidden;
        else
        {
          // si el recibo no es de hoy y no tiene permiso especial de recibos de regalos
          if (dtpgrD.Value.Value.Date != frmHost._dtpServerDate && !App.User.HasPermission(EnumPermission.GiftsReceipts, EnumPermisionLevel.Standard))
            return Visibility.Hidden;
        }
      }

      return Visibility.Visible;
    }
    #endregion

    public void CalculateCharge(ref frmCancelExternalProducts frm)
    {
      decimal curCharge = 0;
      decimal curTotalCost = 0;
      decimal curMaxAuthGifts = 0;
      bool blnTour = false;

      ChargeTo _ChargeTo = (ChargeTo)cbogrct.SelectedItem;

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
        switch (_ChargeTo.ctCalcType)
        {
          // si el huesped tiene tour el cargo es por el total de regalos menos el monto maximo
          // autorizado. De lo contrario el cargo es por el total de regalos
          case "A":
            // Validamos si tiene tour
            Guest _guest = BRGuests.GetGuest(Convert.ToInt32(txtgrgu.Text));
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
          default:
            break;
        }
      }
      txtgrCxCGifts.Text = string.Format("{0:C2}", curCharge);

      // Calculamos el total del cargo
      if (frm != null)
        frm.txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(frm.txtgrcxcGifts.Text != "" ? frm.txtgrcxcGifts.Text.TrimStart(' ', '$') : "0") + Convert.ToDecimal(frm.txtgrcxcAdj.Text != "" ? txtgrCxCAdj.Text.TrimStart(' ', '$') : "0"));
      else
        txtTotalCxC.Text = string.Format("{0:C2}", Convert.ToDecimal(txtgrCxCGifts.Text != "" ? txtgrCxCGifts.Text.TrimStart(' ', '$') : "0") + Convert.ToDecimal(txtgrCxCAdj.Text != "" ? txtgrCxCAdj.Text.TrimStart(' ', '$') : "0"));

    }

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

  }
}
