using IM.Model.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using IM.Model.Enums;
using System.Globalization;
using IM.Base.Classes;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Reports;
using System.Text;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmInvitationBase.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 16/02/2016 Created
  /// </history>
  public partial class frmInvitationBase : Window
  { 
    #region Atributos

    #region Listas
    private List<InvitationGift> _lstGifts = new List<InvitationGift>();
    private List<BookingDeposit> _lstDeposits = new List<BookingDeposit>();
    private List<GuestCreditCard> _lstCreditsCard = new List<GuestCreditCard>();
    private List<Guest> _lstAdditionals = new List<Guest>();
    private List<GuestStatus> _lstGuestStatus = new List<GuestStatus>();
    
    private List<objInvitGift> _lstObjInvitGift = null;
    private List<objInvitGuestStatus> _lstObjInvitGuestStatus = null;
    private List<objInvitCreditCard> _lstObjInvitCreditCard = null;
    private List<objInvitBookingDeposit> _lstObjInvitBookingDeposit = null;
    private List<objInvitAdditionalGuest> _lstObjInvitAdditionalGuest = null;

    #endregion

    #region Objetos
    private Invitation invitation;
    private EnumInvitationType _invitationType;
    private UserData _user;
    private LocationLogin _locationLogin;
    private LeadSourceLogin _leadSourceLogin;
    
    #region Regalos
    CollectionViewSource objInvitGiftViewSource;
    CollectionViewSource giftShortViewSource;
    #endregion

    #region Guest Status
    CollectionViewSource objInvitGuestStatusViewSource;
    CollectionViewSource guestStatusTypeViewSource;
    objInvitGuestStatus objInvitGuestStatusTemp = new objInvitGuestStatus();
    #endregion

    #region Credit Card
    CollectionViewSource objInvitCreditCardViewSource;
    CollectionViewSource creditCardTypeViewSource;
    #endregion

    #region Deposits
    CollectionViewSource objInvitBookingDepositViewSource;
    CollectionViewSource currencyViewSource;
    CollectionViewSource creditCardTypeDepositViewSource;
    CollectionViewSource paymentTypeViewSource;
    CollectionViewSource paymentPlaceViewSource;
    #endregion

    #region Additional Guest
    CollectionViewSource objInvitAdditionalGuestViewSource;
    #endregion

    #endregion

    private const bool allowReschedule = true;

    private bool _wasSelectedByKeyboard = false;
    private int _guestID;
    private EnumInvitationMode _invitationMode;
    private DateTime _serverDateTime;
    private DateTime? _bookingDate;
    private DateTime? _closeDate;
    private bool _wasPressedEditButton = false;
    private bool _isRebook = false;
    private bool _isNewInvitation = false;
    private string _locationOriginal;
    private DateTime? _bookinDateOriginal;
    private DateTime? _bookinTimeOriginal;
    private DateTime? _rescheduleDateOriginal;
    private DateTime? _rescheduleTimeOriginal;
    private string _hReservIDC;
    private DateTime? _showDate;
    private bool _bookingCancel;
    private decimal _qtyGift;
    private bool _isCCPaymentPay = false;
    private bool _allowReschedule = true;
    #endregion

    #region Constructores y destructores

    public frmInvitationBase()
    {
      InitializeComponent();
    }

    public frmInvitationBase(IM.Model.Enums.EnumInvitationType invitationType, UserData userData, int guestID, EnumInvitationMode invitationMode, bool allowReschedule = true)
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

      this._invitationType = invitationType;
      this._user = userData;
      this._guestID = guestID;
      this._invitationMode = invitationMode;
      this._allowReschedule = allowReschedule;
      this._locationLogin = userData.Location;
      this._leadSourceLogin = userData.LeadSource;
      InitializeComponent();
    }

    #endregion

    #region Métodos de la forma

    #region Método Loaded de la forma
    /// <summary>
    /// Configura los controles de la forma
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// [erosado] 05/08/2016  Modified. Se agregó async
    /// </history>
    private async void frmInvitationBase_Loaded(object sender, RoutedEventArgs e)
    {
      _serverDateTime = BRHelpers.GetServerDateTime();
      _bookingDate = txtBookingDate.SelectedDate.HasValue ? txtBookingDate.SelectedDate.Value : (DateTime?)null;
      _closeDate = await BRConfiguration.GetCloseDate();

      if (!_closeDate.HasValue && _bookingDate.HasValue && (_bookingDate <= _closeDate.Value)) //no permitimos modificar invitaciones en fechas cerradas
      {
        btnEdit.IsEnabled = false;
      }
    }
    #endregion

    #region Métodos de los controles de la sección PR INFORMATION

    /// <summary>
    /// Ejecuta el botón Change
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnChange_Click(object sender, RoutedEventArgs e)
    {
      // deshabilitamos los botones de Change, Reschedule y Rebook
      EnableButtonsChangeRescheduleRebook(false, false, false);
      cmbBookingTime.IsEnabled = true;
      grbPrInfo.IsEnabled = true;

      if (allowReschedule)//si se permite hacer reschedules
      {
        var invitD = Convert.ToDateTime(txtDate.Text);
        //si la fecha de invitacion es hoy o si la fecha de booking es despues hoy
        if (invitD.Date == _serverDateTime.Date || txtBookingDate.SelectedDate.Value.Date > _serverDateTime.Date)
        {
          //' Fecha de booking
          txtBookingDate.IsEnabled = true;

          //si tiene permiso especial de invitaciones
          if (_user.HasPermission(_invitationType == EnumInvitationType.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations, EnumPermisionLevel.Special)
              || _user.HasPermission(_invitationType == EnumInvitationType.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations, EnumPermisionLevel.SuperSpecial))
          {
            //PR Contract
            if (_invitationType == EnumInvitationType.OutHouse)
            {
              txtPRContract.IsEnabled = true;
              cmbPRContract.IsEnabled = true;
            }

            //PR
            txtPR.IsEnabled = true;
            cmbPR.IsEnabled = true;

            //Sala /Location
            if (_invitationType == EnumInvitationType.Host)
            {
              txtLocation.IsEnabled = true;
              cmbLocation.IsEnabled = true;
            }
            else
            {
              txtSalesRoom.IsEnabled = true;
              cmbSalesRoom.IsEnabled = true;
            }
          }
        }
      }
      else //si no se permite hacer reschedules
      {
        txtBookingDate.IsEnabled = true;

        //PR Contract
        if (_invitationType == EnumInvitationType.OutHouse)
        {
          txtPRContract.IsEnabled = true;
          cmbPRContract.IsEnabled = true;
        }

        //PR
        txtPR.IsEnabled = true;
        cmbPR.IsEnabled = true;

        //Sala /Location
        if (_invitationType == EnumInvitationType.Host)
        {
          txtLocation.IsEnabled = true;
          cmbLocation.IsEnabled = true;
        }
        else
        {
          txtSalesRoom.IsEnabled = true;
          cmbSalesRoom.IsEnabled = true;
        }
      }
    }

    /// <summary>
    /// Permite hacer reschedule a un booking de un huesped (se puede hacer varias veces, sin restricciones)
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnReschedule_Click(object sender, RoutedEventArgs e)
    {
      //deshabilitamos los botones de Change, Reschedule y Rebook
      EnableButtonsChangeRescheduleRebook(false, false, false);

      if (_invitationType == EnumInvitationType.Host)
      {
        txtLocation.IsEnabled = true;
        cmbLocation.IsEnabled = true;
      }
      else
      {
        txtSalesRoom.IsEnabled = true;
        cmbSalesRoom.IsEnabled = true;
      }

      txtRescheduleDate.IsEnabled = true;
      cmbRescheduleTime.IsEnabled = true;

      chkResch.IsChecked = true;
    }

    /// <summary>
    /// Ejecuta el botón Rebook
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnRebook_Click(object sender, RoutedEventArgs e)
    {
      _isNewInvitation = true;
      _isRebook = true;
      _guestID = 0;

      // Se establece el modo agregar
      _invitationMode = EnumInvitationMode.modAdd;

      // deshabilitamos los botones de Change, Reschedule y Rebook
      EnableButtonsChangeRescheduleRebook(false, false, false);

      #region ID
      if (String.IsNullOrEmpty(txtRebookRef.Text))
        txtRebookRef.Text = txtGuid.Text;

      txtGuid.Text = String.Empty;
      #endregion

      #region Tipo de invitación
      chkQuiniella.IsChecked = false;

      chkShow.IsChecked = false;
      if (_showDate.HasValue)
      {
        _showDate = null;
      }


      #endregion

      #region Booking
      //Pr Contract
      if (_invitationType == EnumInvitationType.OutHouse)
      {
        txtPRContract.Text = String.Empty;
        cmbPRContract.SelectedIndex = -1;
      }

      //PR
      txtPR.Text = String.Empty;
      cmbPR.SelectedIndex = -1;

      //Fecha y hora booking
      txtBookingDate.SelectedDate = null;
      cmbBookingTime.SelectedIndex = -1;

      #endregion
      // si tiene fecha de reschedule, limpia la informacion del reschedule
      if (txtRescheduleDate.SelectedDate.HasValue)
      {
        //Indica que no es un reschedule
        chkResch.IsChecked = false;

        // Fecha de reschedule
        txtRescheduleDate.SelectedDate = null;

        // Hora de reschedule
        cmbRescheduleTime.SelectedIndex = -1;
      }

      if (_invitationType == EnumInvitationType.InHouse || _invitationType == EnumInvitationType.Animation || _invitationType == EnumInvitationType.Regen)
      {
        //No directa
        chkDirect.IsChecked = false;

        //Invitación no cancelada
        _bookingCancel = false;

        #region Contacto
        txtPRContract.Text = String.Empty;
        cmbPRContract.SelectedIndex = -1;
        #endregion
      }

      #region Depositos
      txtBurned.Text = "0.0";
      cmbResort.SelectedIndex = -1;
      #endregion

      #region Regalos
      dtgGifts.ItemsSource = null;
      #endregion

      #region Número de habitaciones
      if (!String.IsNullOrEmpty(txtRoomQuantity.Text))
      {
        txtRoomQuantity.Text = "1";
      }
      #endregion

      EnableControls();
    }

    /// <summary>
    /// Asigna la cable del PR Contract al TextBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbPRContract_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!_wasSelectedByKeyboard)
      {
        txtPRContract.Text = cmbPRContract.SelectedValue.ToString();
      }
      _wasSelectedByKeyboard = false;
    }

    /// <summary>
    /// Asigna la cable del PR Contract al ComboBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void txtPRContract_LostFocus(object sender, RoutedEventArgs e)
    {
      _wasSelectedByKeyboard = true;
      cmbPRContract.SelectedValue = txtPRContract.Text;
    }

    /// <summary>
    /// Asigna la cable del PR al TextBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbPR_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!_wasSelectedByKeyboard && cmbPR.SelectedIndex != -1)
      {
        txtPR.Text = cmbPR.SelectedValue.ToString();
      }
      _wasSelectedByKeyboard = false;
    }

    /// <summary>
    /// Asigna la cable del PR al ComboBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void txtPR_LostFocus(object sender, RoutedEventArgs e)
    {
      _wasSelectedByKeyboard = true;
      cmbPR.SelectedValue = txtPR.Text;
    }

    /// <summary>
    /// Asigna la cable de la Sala de Ventas al TextBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbSalesRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!_wasSelectedByKeyboard)
      {
        txtSalesRoom.Text = cmbSalesRoom.SelectedValue.ToString();
      }
      _wasSelectedByKeyboard = false;
    }

    /// <summary>
    /// Asigna la cable de la Sala de Ventas al ComboBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void txtSalesRoom_LostFocus(object sender, RoutedEventArgs e)
    {
      _wasSelectedByKeyboard = true;
      cmbSalesRoom.SelectedValue = txtSalesRoom.Text;
    }

    /// <summary>
    /// Asigna la cable de la Locacion al TextBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!_wasSelectedByKeyboard)
      {
        txtLocation.Text = cmbLocation.SelectedValue.ToString();
      }
      _wasSelectedByKeyboard = false;
    }

    /// <summary>
    /// Asigna la cable de la Locacion al ComboBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void txtLocation_LostFocus(object sender, RoutedEventArgs e)
    {
      _wasSelectedByKeyboard = true;
      cmbLocation.SelectedValue = txtLocation.Text;
    }

    /// <summary>
    /// Selecciona la hota de booking
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbBookingTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    /// <summary>
    /// Selecciona la hora de de la nueva fecha
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbRescheduleTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    /// <summary>
    /// Carga las horas disponibles para realizar el Booking
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbBookingTime_DropDownOpened(object sender, EventArgs e)
    {
      if (!cmbBookingTime.HasItems && txtBookingDate.SelectedDate.HasValue)
      {
        var times = IM.BusinessRules.BR.BRTourTimesAvailables.GetTourTimesAvailables(_user.LeadSource.lsID, txtSalesRoom.Text, txtBookingDate.SelectedDate.Value);
        LoadTimeComboBoxes(times, cmbBookingTime, "Text", "PickUp", String.Empty);
      }
    }

    /// <summary>
    /// Carga las horas disponibles para realizar una reagendación
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbRescheduleTime_DropDownOpened(object sender, EventArgs e)
    {
      if (!cmbRescheduleTime.HasItems && txtRescheduleDate.SelectedDate.HasValue)
      {
        var times = IM.BusinessRules.BR.BRTourTimesAvailables.GetTourTimesAvailables(_user.LeadSource.lsID, txtSalesRoom.Text, txtRescheduleDate.SelectedDate.Value);
        LoadTimeComboBoxes(times, cmbRescheduleTime, "Text", "PickUp", String.Empty);
      }
    }

    #endregion

    #region Métodos de los controles de la sección GUEST INFORMATION (Search)
    /// <summary>
    /// Ejecuta el botón Search
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// [vipacheco] 04/Agosto/2016 Modified -> Se agrego el EnumModule
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      var search = new frmSearchGuest(_user, EnumModule.InHouse);
      search.Owner = this;
      bool? res = search.ShowDialog();
      if (res.HasValue && res.Value && search.lstGuestAdd[0] != null)
      {
        txtReservationNumber.Text = search.lstGuestAdd[0].guHReservID.ToString();
      }
    }
    #endregion

    #region Métodos de los controles de la sección BUTTONS AREA (Edit / Print / Save / Cancel / Log)

    /// <summary>
    /// Ejecuta el botón Editar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private async void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      _wasPressedEditButton = true;
      if (await ValidateEdit())
      {
        //si no tiene show
        if (chkShow.IsChecked.HasValue && !chkShow.IsChecked.Value)
        {
          _invitationMode = EnumInvitationMode.modEdit;
        }
        else
        {
          Helpers.UIHelper.ShowMessage("Guest has made Show");
          //establecemos el modo lectura
          _invitationMode = EnumInvitationMode.modOnlyRead;
        }
        AssignUser();
        EnableControls();
      }

      _wasPressedEditButton = false;
    }

    /// <summary>
    /// Ejecuta el botón Imprimir
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      RptInvitationHelper.RptInvitation(_guestID, _user.User.peID);
    }

    /// <summary>
    /// Ejecuta el botón Guardar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (Validate())
      {
        SaveGuestInformation();
        Helpers.UIHelper.ShowMessage("Guest has been saved successfully", image: MessageBoxImage.Information);
        _invitationMode = EnumInvitationMode.modOnlyRead;
        EnableControls();
        this.DialogResult = true;
      }
    }

    /// <summary>
    /// Ejecuta el botón Canelar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      bool close = true;

      if (_wasPressedEditButton)
      {
        var resp = MessageBox.Show("Are you sure to cancel the operation? You'll loss the information", "Intelligence Marketing", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
        if (resp != MessageBoxResult.Yes)
        {
          close = false;
        }
      }

      if (close)
      {
        this.Close();
      }
    }

    /// <summary>
    /// Ejecuta el botón Log
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {

    }

    #endregion
    
    #region Métodos de los controles de la sección GIFTS
    /// <summary>
    /// Revisa que se ingrese la informacion correcta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      int qty = 0;
      switch (e.Column.SortMemberPath)
      {
        case "igQty":
          var igQty = e.EditingElement as TextBox;
          int.TryParse(igQty.Text, out qty);
          if (qty == 0)
          {
            Helpers.UIHelper.ShowMessage("Quantity can not be lower than 1");
            e.EditingElement.Focus();
          }
          else
          {
            _qtyGift = qty;
            _lstObjInvitGift[e.Row.GetIndex()].igQty = qty;
          }
          break;
        case "iggi":
          var iggi = e.EditingElement as ComboBox;
          if (iggi.SelectedValue != null && !String.IsNullOrEmpty(iggi.SelectedValue.ToString()))
          {
            _lstObjInvitGift[e.Row.GetIndex()].iggi = iggi.SelectedValue.ToString();
            _lstObjInvitGift[e.Row.GetIndex()].igAdults = 1;
          }
          break;
        case "igAdults":
          var asd = (objInvitGift)e.Row.Item;
          var igAdults = e.EditingElement as TextBox;
          if(!int.TryParse(igAdults.Text, out qty) || qty == 0)
          {
            Helpers.UIHelper.ShowMessage("Adults can not be lower than 1");
            e.EditingElement.Focus();
            e.Cancel = true;
          }
          break;

        case "igMinors":
        case "igExtraAdult":
          var igMinorExtraA = e.EditingElement as TextBox;
          if (!int.TryParse(igMinorExtraA.Text, out qty))
          {
            Helpers.UIHelper.ShowMessage("The Cells Minors and E Adults acept number only");
            e.EditingElement.Focus();
            e.Cancel = true;
          }
          break;
      }
    }

    /// <summary>
    /// Revisa que se ingresen solo datos númericos en ciertos campos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtgGifts_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      switch (dtgGifts.CurrentCell.Column.SortMemberPath)
      {
        case "igQty":
        case "igAdults":
        case "igMinors":
        case "igExtraAdults":
          if (e.Text == ".")
            e.Handled = false;
          else if (!char.IsDigit(e.Text, e.Text.Length - 1))
            e.Handled = true;
        break;
      }
    }
    
    /// <summary>
    /// Calcula el valor de los regalos al cambiar de renglón
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtgGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      CalculateCostsPrices();
      CalculateTotalGifts();
    }
    #endregion

    #region Métodos de los controles de la sección GUEST STATUS
    private void dtgGuestStatus_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      switch (e.Column.SortMemberPath)
      {
        case "gtgs":
          var gtgs = e.EditingElement as ComboBox;
          if(gtgs.SelectedIndex != -1 )
          {
            _lstObjInvitGuestStatus[e.Row.GetIndex()].gtgs = gtgs.SelectedValue.ToString();
            _lstObjInvitGuestStatus[e.Row.GetIndex()].gtQuantity = Convert.ToByte(1);
            CalculateMaxAuthGifts();
          }
          break;
      }
    }

    //Permite solo Agregar un Registro al Grid.
    private void dtgGuestStatus_LoadingRow(object sender, DataGridRowEventArgs e)
    {
      System.ComponentModel.IEditableCollectionView itemView = dtgGuestStatus.Items;
      if (_lstObjInvitGuestStatus.Count == 1 && itemView.IsAddingNew)
      {
        itemView.CommitNew();
        dtgGuestStatus.CanUserAddRows = false;
      }
    }
    #endregion

    #region Métodos de los controles de la sección OTHER INFORMATION

    /// <summary>
    /// Asigna la cable de la agencia al TextBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbAgency_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!_wasSelectedByKeyboard)
      {
        txtAgency.Text = cmbAgency.SelectedValue.ToString();
      }
      _wasSelectedByKeyboard = false;
    }

    /// <summary>
    /// Asigna la cable de la agencia al ComboBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void txtAgency_LostFocus(object sender, RoutedEventArgs e)
    {
      _wasSelectedByKeyboard = true;
      cmbAgency.SelectedValue = txtAgency.Text;
    }

    /// <summary>
    /// Asigna la cable del País al TextBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!_wasSelectedByKeyboard)
      {
        txtCountry.Text = cmbCountry.SelectedValue.ToString();
      }
      _wasSelectedByKeyboard = false;
    }

    /// <summary>
    /// Asigna la cable del País al ComboBox asociado
    /// </summary>
    /// <history>
    /// [lchairez] 10/03/2016 Crated.
    /// </history>
    private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
    {
      _wasSelectedByKeyboard = true;
      cmbCountry.SelectedValue = txtCountry.Text;
    }

    #endregion

    #region Métodos de los controles de la sección ADDITIONAL INFORMATION
    /// <summary>
    /// Agrega un huésped adicional
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtgAdditionalGuest_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      var guid = e.EditingElement as TextBox;
      if (guid != null)
      {
        var guest = IM.BusinessRules.BR.BRGuests.GetGuestById(int.Parse(guid.Text));
        if (guest != null)
        {
          _lstObjInvitAdditionalGuest[e.Row.GetIndex()].guID = guest.guID;
          _lstObjInvitAdditionalGuest[e.Row.GetIndex()].guLastName1 = guest.guLastName1;
          _lstObjInvitAdditionalGuest[e.Row.GetIndex()].guFirstName1 = guest.guFirstName1;
        }
        else
        {
          Helpers.UIHelper.ShowMessage("The Guest ID does not exist");
          e.Cancel = true;
        }
      }
    }

    /// <summary>
    /// Agrega un invitado adicional
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 04/Agosto/2016 Modified -> se agrego el EnumModulo
    /// </history>
    private void btnSearchAdditional_Click(object sender, RoutedEventArgs e)
    {
      var search = new frmSearchGuest(_user, EnumModule.InHouse);
      search.Owner = this;
      bool? res = search.ShowDialog();
      if (res.HasValue && res.Value && search.lstGuestAdd != null)
      {
        var objAddGuest = new objInvitAdditionalGuest();
        objAddGuest.guID = search.lstGuestAdd[0].guID;
        objAddGuest.guLastName1 = search.lstGuestAdd[0].guLastName1;
        objAddGuest.guFirstName1 = search.lstGuestAdd[0].guFirstName1;

        _lstObjInvitAdditionalGuest.Add(objAddGuest);
        dtgAdditionalGuest.Items.Refresh();
      }
    }

    /// <summary>
    /// Agrega un invitado adicional
    /// </summary>
    /// <history>
    /// [vipacheco] 04/Agosto/2016 Modified -> Se agrego el parametro EnumModule al frmSearchGuest
    /// </history>
    private void imgSearchGuest_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      //if (dtgAdditionalGuest.SelectedIndex == -1)
      //  dtgAdditionalGuest.SelectedIndex = 0;

      objInvitAdditionalGuest item = null;
      
      if (dtgAdditionalGuest.CurrentItem is objInvitAdditionalGuest)
        item = (objInvitAdditionalGuest)dtgAdditionalGuest.CurrentItem;

      var search = new frmSearchGuest(_user, EnumModule.InHouse);
      search.Owner = this;
      bool? res = search.ShowDialog();
      if (res.HasValue && res.Value && search.lstGuestAdd != null)
      {
        var objAddGuest = new objInvitAdditionalGuest();
        objAddGuest.guID = search.lstGuestAdd[0].guID;
        objAddGuest.guLastName1 = search.lstGuestAdd[0].guLastName1;
        objAddGuest.guFirstName1 = search.lstGuestAdd[0].guFirstName1;

        if (item != null)
        {
          var addGuest = _lstObjInvitAdditionalGuest.SingleOrDefault(g => g.guID == item.guID);
          if (addGuest != null)
          {
            addGuest.guID = search.lstGuestAdd[0].guID;
            addGuest.guLastName1 = search.lstGuestAdd[0].guLastName1;
            addGuest.guFirstName1 = search.lstGuestAdd[0].guFirstName1;
          }
        }
        else
        {
          _lstObjInvitAdditionalGuest.Add(objAddGuest);
        }

        dtgAdditionalGuest.CommitEdit();
        dtgAdditionalGuest.Items.Refresh();
      }
      
    }

    /// <summary>
    /// Abre el detalle del invitado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void  imgDetails_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if(!(dtgAdditionalGuest.CurrentItem is objInvitAdditionalGuest)) return;
      
      var item = (objInvitAdditionalGuest)dtgAdditionalGuest.CurrentItem;
      if (item == null || item.guID == 0)
      {
        return;
      }
      var frmGuest = new frmGuest(_user, item.guID, true, _invitationType == EnumInvitationType.InHouse, false);
      frmGuest.Owner = this;
      frmGuest.ShowDialog();
      
    }
    #endregion

    #region Métodos de los controles de la sección DEPOSIT
    private void dtgDeposits_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      switch(dtgDeposits.CurrentCell.Column.SortMemberPath)
      {
        case "bdAmount":
        case "bdReceived":
        case "bdCardNum":
        case "bdFolioCXC":
          if (e.Text == ".")
            e.Handled = false;
          else if (!char.IsDigit(e.Text, e.Text.Length - 1))
            e.Handled = true;
          break;
      }
    }

    private void dtgDeposits_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      switch(e.Column.SortMemberPath)
      {
        case "bdpt":
          var bdpt = e.EditingElement as ComboBox;
          if(bdpt.SelectedValue.ToString() == "CC")
          {
            _isCCPaymentPay = true;
          }
          else
          {
            _isCCPaymentPay = false;
          }
          break;
        case "bdcc":
          var bdcc = e.EditingElement as ComboBox;
          if(_isCCPaymentPay && bdcc.SelectedIndex == -1)
          {
            Helpers.UIHelper.ShowMessage("Choose a Credit Card");
            e.Cancel = true;
          }
          break;
        case "":
          break;
        case "bdExpD":
          var dt = new DateTime();
          var bdExpD = e.EditingElement as TextBox;
          if(_isCCPaymentPay)
            if(String.IsNullOrEmpty(bdExpD.Text))
            {
              Helpers.UIHelper.ShowMessage("Input a expire date (MM/YY)");
              e.Cancel = true;
            }
            else
            {
              var exp = bdExpD.Text;
              exp = "01/" + bdExpD.Text;
              if (!DateTime.TryParse(exp, out dt))
              {
                Helpers.UIHelper.ShowMessage("The expire day does not has a correct format. (MM/YY)");
                e.Cancel = true;
              }
            }
          
          break;
        case "bdCardNum":
          var cardNum = e.EditingElement as TextBox;
          if(_isCCPaymentPay && String.IsNullOrEmpty(cardNum.Text))
          {
            Helpers.UIHelper.ShowMessage("Type the last four Creedit Card's numbers");
            e.Cancel = true;
          }
          else if (cardNum.Text.Length > 4)
          {
            Helpers.UIHelper.ShowMessage("Type the last four numbers.");
            e.Cancel = true;
            cardNum.Text = String.Empty;
          }
          break;
        case "bdAuth":
          var bdAuth = e.EditingElement as TextBox;
          if(_isCCPaymentPay && String.IsNullOrEmpty(bdAuth.Text))
          {
            Helpers.UIHelper.ShowMessage("Input the Authorizathion ID");
            e.Cancel = true;
          }

          break;
      }
    }
    #endregion

    #region Métodos Genéricos
    /// <summary>
    /// Revisa que los textbox númericos solo resivan números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private new void PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      if (e.Text == ".")
        e.Handled = false;
      else if (!char.IsDigit(e.Text, e.Text.Length - 1))
        e.Handled = true;

    }

    #endregion

    #endregion

    #region Métodos públicos
    public async Task<bool> AccessValidate()
    {
      bool res = true;
      
      if (_invitationMode == EnumInvitationMode.modAdd)//' si el huesped no ha sido invitado
      {
        res =await ValidateEdit();
      }

      if (res)
      {
        ControlsConfiguration();
        LoadControls();
        BackupOriginalValues();
        EnableControls();
      }

      return res;
    }
    #endregion

    #region Métodos privados

    #region Métodos de permisos
    /// <summary>
    /// Validamos que se permita editar
    /// </summary>
    /// <returns>Boolean</returns>
    private async Task<bool> ValidateEdit()
    {
      var login = new frmLogin(loginType: EnumLoginType.Normal, program: EnumProgram.Inhouse, validatePermission: false,switchLoginUserMode:true);
      if (_user.AutoSign)
      {
        login.UserData = _user;
      }
      login.ShowDialog();
      if (!login.IsAuthenticated)
      {
        Helpers.UIHelper.ShowMessage("Error of Authentication");
        this.Close();
        return false;
      }
      //_user = login.UserData;
      // validamos que tenga permiso estandar de invitaciones
      bool permission = false;
      if (_invitationType == EnumInvitationType.Host)
        permission = _user.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.Standard);
      else
        permission = _user.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.Standard);

      if (!permission)
      {
        Helpers.UIHelper.ShowMessage("You do not have sufficient permissions to modify the invitation");
        this.Close();
        return false;
      }

      //validamos si tiene el usuario permiso sobre la locacion que se está trabajando
      if(login.UserData.Location == null)
      {
        var locationByUser = await BRLocations.GetLocationsByUser(login.UserData.User.peID);
        if(locationByUser.FirstOrDefault(l=> l.loID == _user.Location.loID) == null)
        {
          Helpers.UIHelper.ShowMessage("You do not have sufficient permissions to modify the invitation");
          this.Close();
          return false;
        }
        else //Asignamos el nuevo Usuario
        {
          _user = login.UserData;
          _user.Location = _locationLogin;
          _user.LeadSource = _leadSourceLogin;
        }
      }
      
      return true;
    }
    #endregion

    #region Métodos para la configuración de los controles
    /// <summary>
    /// Oculta o habilita los controles específicos de cada módulo.
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void ControlsConfiguration()
    {
      switch (_invitationType)
      {
        case EnumInvitationType.InHouse:
          InHouseControlsConfig();
          break;
        case EnumInvitationType.OutHouse:
          OutHouseControlsConfig();
          break;
        case EnumInvitationType.Host:
          HostControlsConfig();
          break;
        case EnumInvitationType.Animation:
          AnimationControlsConfig();
          break;
        case EnumInvitationType.Regen:
          RegenControlsConfig();
          break;
        case EnumInvitationType.External:
          InHouseControlsConfig();
          ExternalControlsConfig();
          break;
      }
    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo In House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void InHouseControlsConfig()
    {
      colOutInvitation.Width = new GridLength(0); //Con esta instrucción desaparece la columna
      rowPRContract.Height = new GridLength(0); //Con esta instrucción desaparece el renglon
      rowPRContractTitles.Height = new GridLength(0); //Con esta instrucción desaparece el renglon
      lblFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;
    }

    /// <summary>
    /// /// Oculta o habilita los controles necesarios para el módulo Out House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void OutHouseControlsConfig()
    {
      colReservNum.Width = new GridLength(0);
      colBtnSearch.Width = new GridLength(0);
      colRebookRef.Width = new GridLength(0);
      btnReschedule.Visibility = Visibility.Hidden;
      btnRebook.Visibility = Visibility.Hidden;
      lblFlightNumber.Visibility = Visibility.Visible;
      txtFlightNumber.Visibility = Visibility.Visible;
      lblLocation2.SetValue(Grid.ColumnProperty, 1);
      txtLocation2.SetValue(Grid.ColumnProperty, 1);
      lblReschedule.Visibility = Visibility.Hidden;
      txtRescheduleDate.Visibility = Visibility.Hidden;
      cmbRescheduleTime.Visibility = Visibility.Hidden;
      lblResch.Visibility = Visibility.Hidden;
      chkResch.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;
      grbElectronicPurse.Visibility = Visibility.Hidden;
      grbElectronicPurse.Visibility = Visibility.Hidden;
      grbRoomQuantity.Visibility = Visibility.Hidden;
    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Host
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void HostControlsConfig()
    {
      rowPRContractTitles.Height = new GridLength(0);
      rowPRContract.Height = new GridLength(0);
      lblSalesRoom.Visibility = Visibility.Hidden;
      txtSalesRoom.Visibility = Visibility.Hidden;
      cmbSalesRoom.Visibility = Visibility.Hidden;
      lblLocation2.Visibility = Visibility.Hidden;
      txtLocation2.Visibility = Visibility.Hidden;
      lblFlightNumber.Visibility = Visibility.Hidden;
      txtFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Visible;
      txtLocation.Visibility = Visibility.Visible;
      cmbLocation.Visibility = Visibility.Visible;
      lblSalesRoom2.Visibility = Visibility.Visible;
      txtSalesRoom2.Visibility = Visibility.Visible;
      grbElectronicPurse.Visibility = Visibility.Hidden;


    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Animation
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void AnimationControlsConfig()
    {
      colOutInvitation.Width = new GridLength(0);
      rowPRContractTitles.Height = new GridLength(0);
      rowPRContract.Height = new GridLength(0);
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblBeforeInOut.Visibility = Visibility.Hidden;
      chkBeforeInOut.Visibility = Visibility.Hidden;
      lblSalesRoom.Visibility = Visibility.Visible;
      txtSalesRoom.Visibility = Visibility.Visible;
      cmbSalesRoom.Visibility = Visibility.Visible;
      lblLocation2.Visibility = Visibility.Visible;
      txtLocation2.Visibility = Visibility.Visible;
      grbElectronicPurse.Visibility = Visibility.Hidden;

    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Regen
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void RegenControlsConfig()
    {
      colOutInvitation.Width = new GridLength(0);
      rowPRContractTitles.Height = new GridLength(0);
      rowPRContract.Height = new GridLength(0);
      lblFlightNumber.Visibility = Visibility.Hidden;
      txtFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;
      lblBeforeInOut.Visibility = Visibility.Hidden;
      chkBeforeInOut.Visibility = Visibility.Hidden;
      grbElectronicPurse.Visibility = Visibility.Hidden;
    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Regen
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void ExternalControlsConfig()
    {
      btnSearch.IsEnabled = true;
    }

    /// <summary>
    /// Permite habilitar o deshabilitar los controles
    /// </summary>
    private void EnableControls()
    {
      bool enable;
      bool enableDeposits;
      bool enableGuestsAdditional;
      bool enableCreditCardsGuestsStatusRoomsQuantity;

      enable = !(_invitationMode == EnumInvitationMode.modOnlyRead);
      _isNewInvitation = _invitationMode == EnumInvitationMode.modAdd || _isRebook;
      #region ID
      if (enable)
      {
        //si es una invitacion outhouse nueva, permitimos definir el folio de la invitacion 
        if (_invitationType == EnumInvitationType.OutHouse && _invitationMode == EnumInvitationMode.modAdd)
        {
          txtOutInvitation.IsEnabled = true;
        }
        else if (_invitationMode == EnumInvitationMode.modEdit && _invitationType == EnumInvitationType.InHouse) //si es una invitacion inhouse
        {
          // si no tiene un folio de reservacion definido, permitimos definirlo
          if (String.IsNullOrEmpty(txtReservationNumber.Text))
          {
            btnSearch.IsEnabled = true;
          }

        }
        //si es una invitacion outhouse, permitimos definir el folio de la invitacion outhouse
        else if (_invitationMode == EnumInvitationMode.modEdit && _invitationType == EnumInvitationType.InHouse)
        {
          txtOutInvitation.IsEnabled = true;
        }
      }
      #endregion

      #region Profile Opera
      grbOperaInfo.IsEnabled = false;
      #endregion

      #region Quinella Show
      grbTypeInvitations.IsEnabled = enable;
      #endregion

      #region Language
      grbLanguage.IsEnabled = enable;
      #endregion

      #region Guest 1 y 2
      grbGuest1.IsEnabled = enable;
      grbGuest2.IsEnabled = enable;
      #endregion

      #region PR information
      grbPrInfo.IsEnabled = enable;

      if (_invitationType == EnumInvitationType.OutHouse)
      {
        txtPRContract.IsEnabled = enable;
        cmbPRContract.IsEnabled = enable;
      }

      txtPR.IsEnabled = enable;
      cmbPR.IsEnabled = enable;

      if (_invitationType == EnumInvitationType.Host)
      {
        txtLocation.IsEnabled = enable;
        cmbLocation.IsEnabled = enable;
        txtSalesRoom2.IsEnabled = enable;
      }
      else
      {
        txtSalesRoom.IsEnabled = enable;
        cmbSalesRoom.IsEnabled = enable;
      }

      txtBookingDate.IsEnabled = enable;
      cmbBookingTime.IsEnabled = enable;
      chkDirect.IsEnabled = enable;

      if (_invitationType != EnumInvitationType.Regen && _invitationType != EnumInvitationType.Animation)
      {
        chkBeforeInOut.IsEnabled = enable;
      }

      if (_invitationType != EnumInvitationType.OutHouse)
      {
        txtRescheduleDate.IsEnabled = enable;
        cmbRescheduleTime.IsEnabled = enable;
        chkResch.IsEnabled = enable;
      }

      if (_invitationType == EnumInvitationType.OutHouse)
      {
        txtFlightNumber.IsEnabled = enable;
      }

      btnChange.IsEnabled = enable;
      btnRebook.IsEnabled = enable;
      btnReschedule.IsEnabled = enable;

      if (enable) //si se esta modificando o agregando
      {
        if (_invitationType != EnumInvitationType.Host) // si no es el modulo host
        {
          //no se permite modificar el PR, sala, fecha y hora de booking si es una invitacion existente
          if (_invitationMode != EnumInvitationMode.modAdd)
          {
            //PR
            txtPR.IsEnabled = false;
            cmbPR.IsEnabled = false;

            //Sale Room
            txtSalesRoom.IsEnabled = false;
            cmbSalesRoom.IsEnabled = false;

            //Booking Date & Time
            txtBookingDate.IsEnabled = false;
            cmbBookingTime.IsEnabled = false;
          }
        }

        if (allowReschedule)// si se permite reschedules, no permitimos modificar la informacion de reschedule
        {
          chkResch.IsEnabled = false;
          txtRescheduleDate.IsEnabled = false;
          cmbRescheduleTime.IsEnabled = false;
        }
      }

      #endregion

      #region Deposits
      enableDeposits = enable;

      if (_invitationMode == EnumInvitationMode.modEdit)
      {
        if (_invitationType != EnumInvitationType.OutHouse)
        {
          var dtInvit = Convert.ToDateTime(txtDate.Text);
          //si la fecha de salida es hoy o despues y (es una invitacion nueva o la fecha de invitacion es hoy o
          //(tiene permiso especial de invitaciones y la fecha de booking original Mayor o igual a hoy))
          if (txtDeparture.SelectedDate.HasValue && (txtDeparture.SelectedDate.Value.Date >= _serverDateTime.Date)
              && ((_isNewInvitation || dtInvit.Date == _serverDateTime.Date)
                  || (_user.HasPermission(_invitationType == EnumInvitationType.Host ? EnumPermission.Host : EnumPermission.PRInvitations, EnumPermisionLevel.Special)
                  && _bookinDateOriginal.HasValue && (_bookinDateOriginal.Value.Date>= _serverDateTime.Date))
                  )
            )
          {
            //OK
          }
          else
          {
            //no permitimos modificar los depositos
            enableDeposits = false;
          }
        }
      }

      grbDeposits.IsEnabled = enableDeposits;
      #endregion

      #region Guests Additional
      enableGuestsAdditional = enable;
      if (_invitationMode == EnumInvitationMode.modEdit)
      {
        //si no es el modulo outside ni el de Host
        if (_invitationType != EnumInvitationType.OutHouse && _invitationType != EnumInvitationType.Host)
        {
          //si la fecha de booking original es hoy o despues o es una invitacion nueva
          if ((_bookinDateOriginal.HasValue && (_bookinDateOriginal.Value.Date >= _serverDateTime.Date)) || _isNewInvitation)
          {
            //OK
          }
          else
          {
            //no permitimos modificar los huéspedes adicionales
            enableGuestsAdditional = false;
          }
        }
      }
      grbAdditionalInfo.IsEnabled = enableGuestsAdditional;
      #endregion

      #region Gifts
      grbGifts.IsEnabled = enable;
      #endregion

      #region Other Info
      grbOtherInfo.IsEnabled = enable;

      if (_invitationMode == EnumInvitationMode.modEdit)//si se esta modificando
      {
        //si no es el modulo outside ni el de Host
        if (_invitationType != EnumInvitationType.OutHouse && _invitationType != EnumInvitationType.Host)
        {
          //si tiene copia de folio de reservacion, no se permite modificar la agencia
          if (!String.IsNullOrEmpty(_hReservIDC))
          {
            txtAgency.IsEnabled = false;
            cmbAgency.IsEnabled = false;
          }

          //no se permite modificar el hotel y la fecha de llegada
          cmbHotel.IsEnabled = false;
          txtArrival.IsEnabled = false;
        }

        if (_invitationType != EnumInvitationType.OutHouse)//si no es el modulo outside
        {
          //solo se permite modificar la fecha de salida si tiene permiso especial de invitaciones
          if (!_user.HasPermission(_invitationType == EnumInvitationType.Host ? EnumPermission.Host : EnumPermission.PRInvitations, EnumPermisionLevel.Special))
          {
            txtDeparture.IsEnabled = false;
          }
        }

      }
      #endregion

      #region Credit Cards, Guests Status, Rooms Quantity
      enableCreditCardsGuestsStatusRoomsQuantity = enable;

      if (_invitationMode == EnumInvitationMode.modEdit)//si se esta modificando
      {
        if (_invitationType != EnumInvitationType.OutHouse && _invitationType != EnumInvitationType.Host)//si no es el modulo outside ni el de Host
        {

          //si la fecha de booking original es hoy o despues o es una invitacion nueva
          if ((_bookinDateOriginal.HasValue && _bookinDateOriginal.Value.Date >= _serverDateTime.Date) || _isNewInvitation)
          {
            //OK 
          }
          else
          {
            // no permitimos modificar las tarjetas de credito, los estatus de los huespedes y el numero de habitaciones
            enableCreditCardsGuestsStatusRoomsQuantity = false;
          }
        }
      }

      #region Creadit Cards
      if (_invitationType == EnumInvitationType.InHouse || _invitationType == EnumInvitationType.Host)
      {
        grbCreditCards.IsEnabled = enableCreditCardsGuestsStatusRoomsQuantity;
        if (enable)
          txtCCCompany.IsEnabled = false;
      }
      #endregion

      #region Guest Status
      grbGuestStatus.IsEnabled = enableCreditCardsGuestsStatusRoomsQuantity;
      if (enable)
        txtGuestStatus.IsEnabled = false;
      #endregion

      #region Rooms Quantity
      grbRoomQuantity.IsEnabled = enableCreditCardsGuestsStatusRoomsQuantity;
      #endregion

      #endregion

      #region Electronic Purse
      if (_invitationType == EnumInvitationType.InHouse)
      {
        grbElectronicPurse.IsEnabled = enable;
      }
      #endregion

      #region Botones

      btnEdit.IsEnabled = !enable;
      btnPrint.IsEnabled = !enable;
      btnSave.IsEnabled = enable;
      btnCancel.IsEnabled = enable;
      #endregion

      if (enable)//si se debe habilitar
      {
        //establecemos los valores iniciales de la invitacion
        SetInitialValues();
      }

      if (_wasPressedEditButton)// si se presiono el boton de editar
      {
        //lanzamos el evento para configurar el cambio de booking, reschedule y rebook
        SetupChangeRescheduleRebook();
      }
      else
      {
        //deshabilitamos los botones de Change, Reschedule y Rebook
        EnableButtonsChangeRescheduleRebook(false, false, false, enable);
      }


    }

    /// <summary>
    /// Descripcion: Permite / impide el cambio de booking, reschedule y rebook
    /// </summary>
    private void SetupChangeRescheduleRebook()
    {
      //si la fecha de salida es hoy o despues o el usuario tiene permiso especial de invitaciones
      if ((txtDeparture.SelectedDate.HasValue && txtDeparture.SelectedDate.Value.Date >= _serverDateTime.Date)
        || _user.HasPermission(_invitationType == EnumInvitationType.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations, EnumPermisionLevel.SuperSpecial))
      {
        if (_invitationMode == EnumInvitationMode.modAdd)//si es una invitacion nueva
        {
          //no se permite cambiar, reschedule, ni rebook
          EnableButtonsChangeRescheduleRebook(false, false, false);
        }
        else if (chkShow.IsChecked.HasValue && chkShow.IsChecked.Value) // si tiene show
        {
          //no se puede modificar Antes In & Out
          if (_invitationType != EnumInvitationType.Animation || _invitationType != EnumInvitationType.Regen)
          {
            chkBeforeInOut.IsChecked = false;
          }

          //solo se permite rebook
          EnableButtonsChangeRescheduleRebook(false, false, true);
        }
        else if (Convert.ToDateTime(txtDate.Text).Date == _serverDateTime.Date && (chkResch.IsChecked.HasValue && !chkResch.IsChecked.Value)) //si la fecha de invitacion es hoy y no es un reschedule
        {
          // no se permite reschedule
          EnableButtonsChangeRescheduleRebook(true, false, true);
        }
        else if (Convert.ToDateTime(txtDate.Text).Date == _serverDateTime.Date && (chkResch.IsChecked.HasValue && chkResch.IsChecked.Value)) // si la fecha de invitacion es hoy y es un reschedule
        {
          // no se permite cambiar
          EnableButtonsChangeRescheduleRebook(false, true, true);
        }
        // si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es despues de hoy y tiene permiso estandar de invitaciones
        else if ((Convert.ToDateTime(txtDate.Text).Date < _serverDateTime.Date)
                  && (chkResch.IsChecked.HasValue && !chkResch.IsChecked.Value)
                  && (txtBookingDate.SelectedDate.HasValue && (txtBookingDate.SelectedDate.Value.Date > _serverDateTime.Date))
                  && _user.HasPermission(_invitationType == EnumInvitationType.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations, EnumPermisionLevel.Standard)
                )
        {
          // no se permite reschedule
          EnableButtonsChangeRescheduleRebook(true, false, true);
        }
        // si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es hoy
        else if ((Convert.ToDateTime(txtDate.Text).Date < _serverDateTime.Date)
                  && (chkResch.IsChecked.HasValue && !chkResch.IsChecked.Value)
                  && (txtBookingDate.SelectedDate.HasValue && (txtBookingDate.SelectedDate.Value.Date == _serverDateTime.Date))
                )
        {
          // se permite cambiar, reschedule y rebook
          EnableButtonsChangeRescheduleRebook(true, true, true);
        }
        //' si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es antes de hoy
        else if ((Convert.ToDateTime(txtDate.Text).Date < _serverDateTime.Date)
                  && (chkResch.IsChecked.HasValue && !chkResch.IsChecked.Value)
                  && (txtBookingDate.SelectedDate.HasValue && (txtBookingDate.SelectedDate.Value.Date < _serverDateTime.Date))
                )
        {
          //No se permite cambiar
          EnableButtonsChangeRescheduleRebook(false, true, true);
        }
        //si la fecha de invitacion es antes de hoy y no es un reschedule
        else if ((Convert.ToDateTime(txtDate.Text).Date < _serverDateTime.Date)
                  && (chkResch.IsChecked.HasValue && chkResch.IsChecked.Value)
                )
        {
          // no se permite cambiar
          EnableButtonsChangeRescheduleRebook(false, true, true);
        }

        if (!allowReschedule)
        {
          // ocultamos los controles de reschedule y rebook
          btnReschedule.Visibility = Visibility.Hidden;
          btnRebook.Visibility = Visibility.Hidden;
          txtRescheduleDate.Visibility = Visibility.Hidden;
          cmbRescheduleTime.Visibility = Visibility.Hidden;
          lblReschedule.Visibility = Visibility.Hidden;
          chkResch.Visibility = Visibility.Hidden;
          lblResch.Visibility = Visibility.Hidden;
          //permitimos cambiar
          btnChange.IsEnabled = true;
        }

      }
      else
      {
        //no se permite cambiar, reschedule, ni rebook
        EnableButtonsChangeRescheduleRebook(false, false, true);
      }

    }

    /// <summary>
    /// Habilita / deshabilita los botones de Change, Reschedule y Rebook
    /// </summary>
    /// <param name="enabledChange">Habilita el botón Change</param>
    /// <param name="enabledReschedule">Habilita el botón Reschedule</param>
    /// <param name="enabledRebook">Habilita el botón Rebook</param>
    private void EnableButtonsChangeRescheduleRebook(bool enabledChange, bool enabledReschedule, bool enabledRebook, bool enableGroupBox = false)
    {
      grbPrInfo.IsEnabled = enabledChange || enabledRebook || enabledReschedule || enableGroupBox;

      btnChange.IsEnabled = enabledChange;
      if (_invitationType != EnumInvitationType.OutHouse)
      {
        btnReschedule.IsEnabled = enabledReschedule;
        btnRebook.IsEnabled = enabledRebook;
      }
    }

    /// <summary>
    /// Descripcion: Establece los valores iniciales de la invitacion
    /// </summary>
    private void SetInitialValues()
    {
      //Fecha y hora de invitacion
      if (String.IsNullOrEmpty(txtDate.Text))
      {
        txtDate.Text = _serverDateTime.ToString("dd/MM/yyyy");
        txtTime.Text = _serverDateTime.ToString("hh:mm");
      }

      //Location
      if (_invitationType != EnumInvitationType.Host)//si no es modulo Host
      {
        if (String.IsNullOrEmpty(txtLocation.Text))
        {
          txtLocation.Text = _user.Location.loID;
          cmbLocation.SelectedValue = _user.Location.loID;
        }
      }
    }

    /// <summary>
    /// respaldamos los valores de los controles originales
    /// </summary>
    private void BackupOriginalValues()
    {
      if (_invitationType != EnumInvitationType.Host)
      {
        //Location Original
        _locationOriginal = txtLocation2.Text;
      }

      //Fecha y hora de booking Original
      _bookinDateOriginal = txtBookingDate.SelectedDate;
      _bookinTimeOriginal = cmbBookingTime.SelectedValue != null ? DateTime.Parse(cmbBookingTime.SelectedValue.ToString()) : (DateTime?)null;

      //Fecha y hora de reschedule original
      if (allowReschedule)
      {
        _rescheduleDateOriginal = txtRescheduleDate.SelectedDate;
        _rescheduleTimeOriginal = cmbRescheduleTime.SelectedValue != null ? DateTime.Parse(cmbRescheduleTime.SelectedValue.ToString()) : (DateTime?)null;
      }
    }

    #endregion

    #region Métodos para cargar Controles e información del invitado

    #region Controles generales, TextBox, DateTimaPickers

    /// <summary>
    /// Manda llamar los métodos para cargar la información del guest.
    /// </summary>
    private void LoadControls()
    {
      LoadCommonControls(); //Se cargan los controles que son comunes en todos los tipos de invitacion.

      //Aqui cargamos los controles que son específicos por tipo de invitación
      switch (_invitationType)
      {
        case EnumInvitationType.InHouse:
          LoadInHouseControls();
          break;
        case EnumInvitationType.OutHouse:
          LoadOutHouseControls();
          break;
        case EnumInvitationType.Host:
          LoadHostControls();
          break;
        case EnumInvitationType.Animation:
          LoadAnimationControls();
          break;
        case EnumInvitationType.Regen:
          LoadRegenControls();
          break;
      }

      #region Datos del Invitado
      LoadGuestInformation();
      #endregion
    }

    /// <summary>
    /// Carga los controles comunes de todos los tipos de invitación
    /// </summary>
    /// <history>
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadCommonControls()
    {
      #region User
      lblUser.Content = _user.User.peN;
      #endregion

      #region ComboBoxes
      var languages = await IM.BusinessRules.BR.BRLanguages.GetLanguages(1);
      LoadComboBox(languages, cmbLanguage, "la", "ES");

      var personnels = await IM.BusinessRules.BR.BRPersonnel.GetPersonnel(_user.LeadSource != null ? _user.LeadSource.lsID : _leadSourceLogin.lsID, roles:"PR");
      LoadComboBox(personnels, cmbPR, "pe");

      var hotels = await IM.BusinessRules.BR.BRHotels.GetHotels(nStatus:1);
      LoadComboBox(hotels, cmbHotel, "hoID", "hoID", _user.Location!= null ? _user.Location.loN : _locationLogin.loN);
      LoadComboBox(hotels, cmbResort, "hoID", "hoID", String.Empty);

      var agencies = await IM.BusinessRules.BR.BRAgencies.GetAgencies(1);
      LoadComboBox(agencies, cmbAgency, "ag");

      var countries = await IM.BusinessRules.BR.BRCountries.GetCountries(1);
      LoadComboBox(countries, cmbCountry, "co");

      var currencies =await IM.BusinessRules.BR.BRCurrencies.GetCurrencies(null, 1);
      LoadComboBox(currencies, cmbCurrency, "cu", "US");
      //Combo del Grid de Depositos
      currencyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("currencyViewSource")));
      currencyViewSource.Source = currencies;

      var paymentTypes = await IM.BusinessRules.BR.BRPaymentTypes.GetPaymentTypes(1);
      LoadComboBox(paymentTypes, cmbPaymentType, "pt", "CS");
      //Combo del Grid de Depositos
      paymentTypeViewSource = ((CollectionViewSource)(this.FindResource("paymentTypeViewSource")));
      paymentTypeViewSource.Source = paymentTypes;

      //Combo del Grid de Depositos
      var paymentePlaces =await BRPaymentPlaces.GetPaymentPlaces();
      paymentPlaceViewSource = ((CollectionViewSource)(this.FindResource("paymentPlaceViewSource")));
      paymentPlaceViewSource.Source = paymentePlaces;

      var maritalStatus =await IM.BusinessRules.BR.BRMaritalStatus.GetMaritalStatus(1);
      LoadComboBox(maritalStatus, cmbMaritalStatusGuest1, "ms");
      LoadComboBox(maritalStatus, cmbMaritalStatusGuest2, "ms");

      var creditCards = await IM.BusinessRules.BR.BRCreditCardTypes.GetCreditCardTypes(null, -1);
      
      //Combo del Grid de Depositos
      creditCardTypeDepositViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("creditCardTypeViewSource")));
      creditCardTypeDepositViewSource.Source = creditCards;

      creditCardTypeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("creditCardTypeViewSource")));
      creditCardTypeViewSource.Source = creditCards;

      guestStatusTypeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestStatusTypeViewSource")));
      guestStatusTypeViewSource.Source = await IM.BusinessRules.BR.BRGuests.GetGuestStatusType(1);

      giftShortViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("giftShortViewSource")));
      giftShortViewSource.Source = await IM.BusinessRules.BR.BRGifts.GetGiftsShort(_user.Location == null ? "ALL":_user.Location.loID, 1);
      #endregion

    }

    /// <summary>
    /// Carga la información extraida de la base de datos
    /// </summary>
    private void LoadGuestInformation()
    {
      var guest = IM.BusinessRules.BR.BRGuests.GetGuestById(_guestID);

      if (guest == null) return;
      #region Tipos de invitacion
      chkQuiniella.IsChecked = guest.guQuinella;
      chkShow.IsChecked = guest.guShow;
      chkInterval.IsChecked = guest.guInterval;
      _showDate = guest.guShowD;
      #endregion

      #region Lenguaje
      cmbLanguage.SelectedValue = String.IsNullOrEmpty(guest.gula) ? String.Empty : guest.gula;
      #endregion

      #region Información del invitado
      txtGuid.Text = guest.guID.ToString();

      if (_invitationType != EnumInvitationType.OutHouse)
      {
        txtReservationNumber.Text = guest.guHReservID;
        txtRebookRef.Text = guest.guRef.ToString();
      }
      _hReservIDC = guest.guHReservIDC;
      txtDate.Text = guest.guInvitD.HasValue ? guest.guInvitD.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
      txtTime.Text = guest.guInvitT.HasValue ? guest.guInvitT.Value.ToString("hh:mm") : DateTime.Now.ToString("hh:mm");

      if (_invitationType == EnumInvitationType.OutHouse || _invitationType == EnumInvitationType.Host)
      {
        txtOutInvitation.Text = guest.guOutInvitNum;
      }

      #endregion

      #region Perfil de Opera

      txtIDOpera.Text = guest.guIdProfileOpera;
      txtLastNameOpera.Text = guest.guLastNameOriginal;
      txtFirstNameOpera.Text = guest.guFirstNameOriginal;
      #endregion

      #region Guest 1
      txtLastNameGuest1.Text = guest.guLastName1;
      txtFirstNameGuest1.Text = guest.guFirstName1;
      txtAgeGuest1.Text = guest.guAge1.HasValue ? guest.guAge1.Value.ToString() : String.Empty;
      cmbMaritalStatusGuest1.SelectedValue = String.IsNullOrEmpty(guest.gums1) ? String.Empty : guest.gums1;
      txtOcuppationGuest1.Text = guest.guOccup1;
      txtEmailGuest1.Text = guest.guEmail1;
      #endregion

      #region Guest 2
      txtLastNameGuest2.Text = guest.guLastname2;
      txtFirstNameGuest2.Text = guest.guFirstName2;
      txtAgeGuest2.Text = guest.guAge2.HasValue ? guest.guAge2.Value.ToString() : String.Empty;
      cmbMaritalStatusGuest2.SelectedValue = String.IsNullOrEmpty(guest.gums2) ? String.Empty : guest.gums2;
      txtOcuppationGuest2.Text = guest.guOccup2;
      txtEmailGuest2.Text = guest.guEmail2;
      #endregion

      #region Gifts
      LoadGiftGrid();
      #endregion

      #region Información PR
      if (_invitationType == EnumInvitationType.OutHouse)
      {
        cmbPRContract.SelectedValue = String.IsNullOrEmpty(guest.guPRInfo) ? String.Empty : guest.guPRInfo;
        txtPRContract.Text = String.IsNullOrEmpty(guest.guPRInfo) ? String.Empty : guest.guPRInfo;
      }

      cmbPR.SelectedValue = String.IsNullOrEmpty(guest.guPRInvit1) ? String.Empty : guest.guPRInvit1;
      txtPR.Text = String.IsNullOrEmpty(guest.guPRInvit1) ? String.Empty : guest.guPRInvit1;

      if (_invitationType != EnumInvitationType.Host)
      {
        cmbSalesRoom.SelectedValue = "MPS";// String.IsNullOrEmpty(guest.gusr) ? String.Empty : guest.gusr;
        txtSalesRoom.Text = String.IsNullOrEmpty(guest.gusr) ? String.Empty : guest.gusr;
      }
      else
      {
        cmbLocation.SelectedValue = String.IsNullOrEmpty(guest.guloInvit) ? String.Empty : guest.guloInvit;
        txtLocation.Text = String.IsNullOrEmpty(guest.guloInvit) ? String.Empty : guest.guloInvit;
      }

      txtBookingDate.Text = guest.guBookD.HasValue ? guest.guBookD.Value.ToString("dd/MM/yyyy") : String.Empty;
      if (guest.guBookD.HasValue)
      {

        var times = IM.BusinessRules.BR.BRTourTimesAvailables.GetTourTimesAvailables(_user.LeadSource.lsID, txtSalesRoom.Text, txtBookingDate.SelectedDate.Value);
        LoadTimeComboBoxes(times, cmbBookingTime, "Text", "PickUp", guest.guBookT.HasValue ? guest.guBookT.Value.ToString() : String.Empty);
        cmbBookingTime.SelectedValue = guest.guBookT.HasValue ? guest.guBookT.Value.ToString() : String.Empty;
      }


      if (_invitationType != EnumInvitationType.OutHouse)
      {
        txtRescheduleDate.Text = guest.guReschD.HasValue ? guest.guReschD.Value.ToString("dd/MM/yyyy") : String.Empty;
        if (guest.guReschD.HasValue)
        {
          var times = IM.BusinessRules.BR.BRTourTimesAvailables.GetTourTimesAvailables(_user.LeadSource.lsID, txtSalesRoom.Text, txtBookingDate.SelectedDate.Value);
          LoadTimeComboBoxes(times, cmbBookingTime, "Text", "PickUp", guest.guBookT.HasValue ? guest.guBookT.Value.ToString() : String.Empty);
          cmbRescheduleTime.SelectedValue = guest.guReschT.HasValue ? guest.guReschT.Value.ToString() : String.Empty;
        }


        chkResch.IsChecked = guest.guResch;
      }
      
      if (_invitationType != EnumInvitationType.Animation && _invitationType != EnumInvitationType.Regen)
        chkBeforeInOut.IsChecked = guest.guAntesIO;

      chkDirect.IsChecked = guest.guDirect;

      if (_invitationType != EnumInvitationType.Host)
      {
        txtLocation2.Text = String.IsNullOrEmpty(guest.guloInvit) ? txtSalesRoom.Text : guest.guloInvit;
      }

      _bookingCancel = guest.guBookCanc;

      #endregion

      #region Other Information
      txtOtherInformation.Text = guest.guExtraInfo;
      txtRoom.Text = guest.guRoomNum;
      cmbHotel.SelectedValue = guest.guHotel;
      cmbAgency.SelectedValue = guest.guag;
      txtAgency.Text = guest.guag;
      cmbCountry.SelectedValue = guest.guco;
      txtCountry.Text = guest.guco;
      txtPax.Text = guest.guPax.ToString("#.00");
      txtArrival.Text = guest.guCheckInD.ToString("dd/MM/yyyy");
      txtDeparture.Text = guest.guCheckOutD.ToString("dd/MM/yyyy");

      #endregion

      #region Deposits
      txtBurned.Text = guest.guDepositTwisted.ToString("#.00");
      cmbPaymentType.SelectedValue = guest.gupt;
      cmbCurrency.SelectedValue = guest.gucu;
      cmbResort.SelectedValue = guest.guHotelB;
      LoadDepositGrid();

      #endregion

      #region Guest Status ***********************FALTA LLENAR EL GRID
      txtGuestStatus.Text = guest.guGStatus;

      LoadGuestStatusGrid();
      //LLENAR GRID
      #endregion

      #region Room Quantity
      if (_invitationType != EnumInvitationType.OutHouse)
        txtRoomQuantity.Text = guest.guRoomsQty.ToString();
      #endregion

      #region Credit Cards *************FALTA LLENAR GRID
      if (_invitationType == EnumInvitationType.InHouse || _invitationType == EnumInvitationType.Host)
      {
        txtCCCompany.Text = guest.guCCType;
        LoadCreditCardGrid();
      }


      #endregion

      #region Additional Information *********FALTA LLENAR EL GRID
      LoadAdditionalInformartionGrid();
      #endregion
    }

    /// <summary>
    /// Carga los controles del formulatio Invitation In House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private async void LoadInHouseControls()
    {
      var salesRooms = await IM.BusinessRules.BR.BRSalesRooms.GetSalesRooms(0);
      LoadComboBox(salesRooms, cmbSalesRoom, "sr");
    }

    /// <summary>
    /// Carga los controles del formulatio Invitation Out House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void LoadOutHouseControls()
    {
      var personnels =await IM.BusinessRules.BR.BRPersonnel.GetPersonnel(_user.LeadSource.lsID);
      LoadComboBox(personnels, cmbPRContract, "pe");

      var salesRooms = await IM.BusinessRules.BR.BRSalesRooms.GetSalesRooms(0);
      LoadComboBox(salesRooms, cmbSalesRoom, "sr");

    }

    /// <summary>
    /// Carga los controles del formulatio Invitation Host
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private void LoadHostControls()
    {
      
    }

    /// <summary>
    /// Carga los controles del formulatio Invitation Animation
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private void LoadAnimationControls()
    {
      
    }
    
    /// <summary>
    /// Carga los controles del formulatio Invitation Regen
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private void LoadRegenControls()
    {
      
    }

    #endregion

    #region Métodos genéricos para cargar ComboBoxes

    /// <summary>
    /// Carga los combos de la forma
    /// </summary>
    /// <param name="items">Lista de elementos que contendrá el combo</param>
    /// <param name="combo">Combo que se llenará con los elementos</param>
    /// <param name="displayItem">Nombre del elemento</param>
    /// <param name="valueItem">Valor del elemento</param>
    /// <param name="defaultValue">Valor que será seleccionado por default</param>
    private void LoadComboBox(IEnumerable<object> items, ComboBox combo, string displayItem, string valueItem, string defaultValue = "")
    {
      combo.DisplayMemberPath = displayItem;
      combo.SelectedValuePath = valueItem;
      combo.SelectedValue = defaultValue;
      combo.ItemsSource = items;
    }

    /// <summary>
    /// Carga los combos de tiempo de la forma
    /// </summary>
    /// <param name="items">Lista de elementos que contendrá el combo</param>
    /// <param name="combo">Combo que se llenará con los elementos</param>
    /// <param name="displayItem">Nombre del elemento</param>
    /// <param name="valueItem">Valor del elemento</param>
    /// <param name="defaultValue">Valor que será seleccionado por default</param>
    private void LoadTimeComboBoxes(IEnumerable<TourTimeAvailable> items, ComboBox combo, string displayItem, string valueItem, string defaultValue = "")
    {
      combo.DisplayMemberPath = displayItem;
      combo.SelectedValuePath = valueItem;
      combo.SelectedValue = defaultValue;
      combo.ItemsSource = (from i in items select new { PickUp = i.PickUp.ToString(), i.Text }).ToList();
    }

    /// <summary>
    /// Carga los combos de la forma
    /// </summary>
    /// <param name="items">Lista de elementos que contendrá el combo</param>
    /// <param name="combo">Combo que se llenará con los elementos</param>
    /// <param name="prefix">prefijo</param>
    /// <param name="defaultValue">Valor que será seleccionado por default</param>
    private void LoadComboBox(IEnumerable<object> items, ComboBox combo, string prefix, string defaultValue = "")
    {
      combo.DisplayMemberPath = String.Format("{0}N", prefix);
      combo.SelectedValuePath = String.Format("{0}ID", prefix);
      combo.SelectedValue = defaultValue;
      combo.ItemsSource = items;
    }

    #endregion

    #region Métodos para cargar los grids

    /// <summary>
    /// Carga la información del Grid de regalos
    /// </summary>
    private async void LoadGiftGrid()
    {
      _lstObjInvitGift = new List<objInvitGift>();

      var invitGift = await BRInvitsGifts.GetInvitsGiftsByGuestID(_guestID);

      _lstObjInvitGift = invitGift.Select(c => new objInvitGift
      {
        igAdults = c.igAdults,
        igComments = c.igComments,
        igct = c.igct,
        igExtraAdults = c.igExtraAdults,
        igFolios = c.igFolios,
        iggi = c.iggi,
        iggr = c.iggr,
        iggu = c.iggu,
        igMinors = c.igMinors,
        igPriceA = c.igPriceA,
        igPriceAdult = c.igPriceAdult,
        igPriceExtraAdult = c.igPriceExtraAdult,
        igPriceM = c.igPriceM,
        igPriceMinor = c.igPriceMinor,
        igQty = c.igQty
      }).ToList();

      _lstGifts = invitGift;

      objInvitGiftViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("objInvitGiftViewSource")));
      objInvitGiftViewSource.Source = _lstObjInvitGift;

      CalculateCostsPrices();
      CalculateTotalGifts();
    }

    /// <summary>
    /// Carga la información del Grid de estado de los invitados
    /// </summary>
    private void LoadGuestStatusGrid()
    {
      var guestStatus = IM.BusinessRules.BR.BRGuestStatusTypes.GetGuestStatus(_guestID);

      _lstObjInvitGuestStatus = guestStatus.Select(c => new objInvitGuestStatus
      {
        gtgs = c.gtgs,
        gtgu = c.gtgu,
        gtQuantity = c.gtQuantity
      }).ToList();
      _lstGuestStatus = guestStatus; // esta lista mantiene los registros de la base de datos sin modificaciones.

      objInvitGuestStatusViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("objInvitGuestStatusViewSource")));
      objInvitGuestStatusViewSource.Source = _lstObjInvitGuestStatus;

      txtGuestStatus.Text = guestStatus.Any() ? guestStatus.First().gtgs : String.Empty;
      CalculateMaxAuthGifts();

      if (_lstObjInvitGuestStatus.Any()) dtgGuestStatus.CanUserAddRows = false;
    }

    /// <summary>
    /// Carga la información del Grid de depósitos de los invitados
    /// </summary>
    private async void LoadDepositGrid()
    {
      var deposits = await BRBookingDeposits.GetBookingDeposits(_guestID, true);

      _lstObjInvitBookingDeposit = deposits.Select(c => new objInvitBookingDeposit
      {
        bdAmount = c.bdAmount,
        bdAuth = c.bdAuth,
        bdCardNum = c.bdCardNum,
        bdcc = c.bdcc,
        bdcu = c.bdcu,
        bddr = c.bddr,
        bdEntryDCXC = c.bdEntryDCXC,
        bdExpD = c.bdExpD,
        bdFolioCXC = c.bdFolioCXC,
        bdgu = c.bdgu,
        bdID = c.bdID,
        bdpc = c.bdpc,
        bdpt = c.bdpt,
        bdReceived = c.bdReceived,
        bdRefund = c.bdRefund,
        bdUserCXC = c.bdUserCXC
      }).ToList();

      _lstDeposits = deposits;  // esta lista mantiene los registros de la base de datos sin modificaciones.

      objInvitBookingDepositViewSource = (CollectionViewSource)this.FindResource("objInvitBookingDepositViewSource");
      objInvitBookingDepositViewSource.Source = _lstObjInvitBookingDeposit;
    }

    /// Carga la información del Grid de las tarjetas de crédito de los invitados
    /// </summary>
    private async void LoadCreditCardGrid()
    {
      var creditCard = await BRGuestCreditCard.GetGuestCreditCard(_guestID);

      _lstObjInvitCreditCard = creditCard.Select(c => new objInvitCreditCard
      {
        gdcc = c.gdcc,
        gdgu = c.gdgu,
        gdQuantity = c.gdQuantity
      }).ToList();

      _lstCreditsCard = creditCard; // esta lista mantiene los registros de la base de datos sin modificaciones.
      objInvitCreditCardViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("objInvitCreditCardViewSource")));
      objInvitCreditCardViewSource.Source = _lstObjInvitCreditCard;
    }
    /// Carga la información del Grid de los invitados extra
    /// </summary>
    private async void LoadAdditionalInformartionGrid()
    {
      var aGuests = await BRGuests.GetAdditionalGuest(_guestID);

      _lstObjInvitAdditionalGuest = aGuests.Select(c => new objInvitAdditionalGuest
      {
        guIDParent = _guestID,
        guID = c.guID,
        guLastName1 = c.guLastName1,
        guFirstName1 = c.guFirstName1
      }).ToList();

      _lstAdditionals = aGuests;
      objInvitAdditionalGuestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("objInvitAdditionalGuestViewSource")));
      objInvitAdditionalGuestViewSource.Source = _lstObjInvitAdditionalGuest;
      
    }

    #endregion

    #region Método para asignación de Usuario
    private void AssignUser()
    {
      lblUser.Content = _user.User.peN;
    }
    #endregion

    #endregion

    #region Métodos para guardar la información del invitado
    /// <summary>
    /// Guarda la informaciónde la forma en la base de datos
    /// </summary>
    private void SaveGuestInformation()
    {
      invitation = new Invitation();
      var guest = IM.BusinessRules.BR.BRGuests.GetGuestById(_guestID);

      #region Tipos de invitacion
      guest.guQuinella = ForBooleanValue(chkQuiniella.IsChecked);
      guest.guShow = ForBooleanValue(chkShow.IsChecked);
      guest.guInterval = true;// ForBooleanValue(chkInterval.IsChecked);
      #endregion

      #region Lenguaje
      guest.gula = ForStringValue(cmbLanguage.SelectedValue);
      #endregion

      #region Información del invitado
      guest.guInvitD = String.IsNullOrEmpty(txtDate.Text) ?  DateTime.Today : Convert.ToDateTime(txtDate.Text);
      guest.guInvitT = String.IsNullOrEmpty(txtTime.Text) ?  DateTime.Now : Convert.ToDateTime(txtTime.Text);
      guest.guInvit = true;
      #endregion

      #region Guest 1
      guest.guLastName1 = ForStringValue(txtLastNameGuest1.Text);
      guest.guFirstName1 = ForStringValue(txtFirstNameGuest1.Text);
      guest.guAge1 = String.IsNullOrEmpty(txtAgeGuest1.Text) ? (byte?) null : Convert.ToByte(txtAgeGuest1.Text);
      guest.gums1 = ForStringValue(cmbMaritalStatusGuest1.SelectedValue);
      guest.guOccup1 = ForStringValue(txtOcuppationGuest1.Text);
      guest.guEmail1 = ForStringValue(txtEmailGuest1.Text);
      #endregion

      #region Guest 2
      guest.guLastname2 = ForStringValue(txtLastNameGuest2.Text);
      guest.guFirstName2 = ForStringValue(txtFirstNameGuest2.Text);
      guest.guAge2 = String.IsNullOrEmpty(txtAgeGuest2.Text) ? (byte?)null : Convert.ToByte(txtAgeGuest2.Text);
      guest.gums2 = ForStringValue(cmbMaritalStatusGuest2.SelectedValue);
      guest.guOccup2 = ForStringValue(txtOcuppationGuest2.Text);
      guest.guEmail2 = ForStringValue(txtEmailGuest2.Text);
      #endregion

      #region Gifts
      SaveGifts();
      
      #endregion

      #region Información PR
      if (_invitationType == EnumInvitationType.OutHouse)
      {
        guest.guPRInfo = ForStringValue(txtPRContract.Text);
      }

      guest.guPRInvit1 = ForStringValue(txtPR.Text);

      if (_invitationType != EnumInvitationType.Host)
      {
        guest.gusr = ForStringValue(cmbSalesRoom.SelectedValue);
      }
      else
      {
        guest.guloInvit = ForStringValue(txtLocation.Text);
      }

      guest.guBookD = txtBookingDate.SelectedDate.HasValue ? txtBookingDate.SelectedDate.Value : (DateTime?)null;
      guest.guBookT = txtBookingDate.SelectedDate.HasValue && !String.IsNullOrEmpty(cmbBookingTime.SelectedValue.ToString()) ? Convert.ToDateTime(cmbBookingTime.SelectedValue.ToString()) : (DateTime?)null;

      if (_invitationType != EnumInvitationType.OutHouse)
      {
        guest.guReschD = txtRescheduleDate.SelectedDate.HasValue ? txtRescheduleDate.SelectedDate : null;
        guest.guReschT = txtRescheduleDate.SelectedDate.HasValue && !String.IsNullOrEmpty(cmbRescheduleTime.SelectedValue.ToString()) ? Convert.ToDateTime(cmbRescheduleTime.SelectedValue.ToString()) : (DateTime?) null;
        guest.guResch = ForBooleanValue(chkResch.IsChecked);
      }


      if (_invitationType != EnumInvitationType.Animation && _invitationType != EnumInvitationType.Regen)
        guest.guAntesIO = ForBooleanValue(chkBeforeInOut.IsChecked);

      guest.guDirect = ForBooleanValue(chkDirect.IsChecked);

      if (_invitationType != EnumInvitationType.Host)
      {
        guest.guloInvit = ForStringValue(txtLocation2.Text);
      }

      #endregion

      #region Other Information
      guest.guExtraInfo = ForStringValue(txtOtherInformation.Text);
      guest.guRoomNum = ForStringValue(txtRoom.Text);
      guest.guHotel = ForStringValue(cmbHotel.SelectedValue);
      guest.guag= ForStringValue(txtAgency.Text);
      guest.guco = ForStringValue(txtCountry.Text);
      guest.guPax = ForDecimalValue(txtPax.Text);
      guest.guCheckInD = txtArrival.SelectedDate.HasValue ? txtArrival.SelectedDate.Value : guest.guCheckInD;
      guest.guCheckOutD = txtDeparture.SelectedDate.HasValue ? txtDeparture.SelectedDate.Value : guest.guCheckOutD;

      #endregion

      #region Deposits
      guest.guDepositTwisted = ForDecimalValue(txtBurned.Text);
      guest.gupt = ForStringValue(cmbPaymentType.SelectedValue);
      guest.gucu = ForStringValue(cmbCurrency.SelectedValue);
      guest.guHotelB = ForStringValue(cmbResort.SelectedValue);

      SaveBookingDeposits();
      #endregion

      #region Guest Status 
      guest.guGStatus = ForStringValue(txtGuestStatus.Text);
      SaveGuestStatus();
      #endregion

      #region Room Quantity
      if (_invitationType != EnumInvitationType.OutHouse)
        guest.guRoomsQty = ForIntegerValue(txtRoomQuantity.Text);
      #endregion

      #region Credit Cards
      if (_invitationType == EnumInvitationType.InHouse || _invitationType == EnumInvitationType.Host)
      {
        guest.guCCType = ForStringValue(txtCCCompany.Text);
        SaveCreditCards();
      }


      #endregion

      #region Additional Information 
      SaveAdditionalGuests();
      #endregion

      invitation.Guest = guest;

      
      IM.BusinessRules.BR.BRGuests.SaveGuestInvitation(invitation);

    }
    
    /// <summary>
    /// Guarda todos los regalos asignados
    /// </summary>
    private void SaveGifts()
    {
      invitation.NewGifts = new List<Model.InvitationGift>();
      invitation.UpdatedGifts = new List<Model.InvitationGift>();
      invitation.DeletedGifts = new List<Model.InvitationGift>();

      if (!_lstObjInvitGift.Any()) return;

      //Convertimos la lista a un objeto de la capa Model
      var gifts = _lstObjInvitGift.Select(c => new InvitationGift
      {
        igAdults = c.igAdults,
        igComments = c.igComments,
        igct = c.igct,
        igExtraAdults = c.igExtraAdults,
        igFolios = c.igFolios,
        iggi = c.iggi,
        iggr = c.iggr,
        iggu = c.iggu,
        igMinors = c.igMinors,
        igPriceA = c.igPriceA,
        igPriceAdult = c.igPriceAdult,
        igPriceExtraAdult = c.igPriceExtraAdult,
        igPriceM = c.igPriceM,
        igPriceMinor = c.igPriceMinor,
        igQty = c.igQty
      }).ToList();

      //Obtenemos los regalos que se modificarán
      invitation.UpdatedGifts = gifts.Where(c => c.iggu != 0).ToList();
      
      //Obtenemos los regalos nuevos para asignales los precios y el invitado
      var newGifts = gifts.Where(c => c.iggu == 0).ToList();
      if (newGifts.Any())
      {
        //Asignamos el guest a los nuevos regalos
        newGifts.ForEach(c=> 
        {
          c.iggu = c.iggu != 0 ? c.iggu : _guestID;
          c.igct = !String.IsNullOrEmpty(c.igct) ? c.igct : "MARKETING";
        });
        invitation.NewGifts.AddRange(newGifts);
      }

      
      

    }

    /// <summary>
    /// Guarda los estados del invitado
    /// </summary>
    private void SaveGuestStatus()
    {
      invitation.NewGuestStatus = new List<Model.GuestStatus>();
      invitation.UpdatedGuestStatus = new List<Model.GuestStatus>();
      invitation.DeletedGuestStatus = new List<Model.GuestStatus>();
      CalculateMaxAuthGifts();

      if (_lstObjInvitGuestStatus.Any())
      {

        //Asignamos el guest
        _lstObjInvitGuestStatus.ForEach(c =>
        {
          c.gtgu = c.gtgu != 0 ? c.gtgu : _guestID;
        });

        //Convertimos la lista a un objeto de la capa Model
        var gs = _lstObjInvitGuestStatus.Select(c => new GuestStatus
        {
          gtgs = c.gtgs,
          gtgu = c.gtgu,
          gtQuantity = c.gtQuantity
        }).ToList();

        invitation.DeletedGuestStatus.AddRange(_lstGuestStatus); //Borramos lo que tenia la base de datos
        invitation.NewGuestStatus.AddRange(gs); //Agregamos lo que tiene el grid
      }
    }
    
    /// <summary>
    /// Guarda las tarjetas de credito del invitado
    /// </summary>
    private void SaveCreditCards()
    {
      invitation.NewCreditCards = new List<Model.GuestCreditCard>();
      invitation.UpdatedCreditCards = new List<Model.GuestCreditCard>();
      invitation.DeletedCreditCards = new List<Model.GuestCreditCard>();

      if(_lstObjInvitCreditCard.Any())
      {
        //Asignamos el guest a la lista
        _lstObjInvitCreditCard.ForEach(c =>
        {
          c.gdgu = c.gdgu != 0 ? c.gdgu : _guestID;
        });
        //Convertimos la lista a un objeto de la capa Model
        var cc = _lstObjInvitCreditCard.Select(c => new GuestCreditCard
        {
          gdcc = c.gdcc,
          gdgu = c.gdgu,
          gdQuantity = c.gdQuantity
        }).ToList();

        invitation.DeletedCreditCards.AddRange(_lstCreditsCard);//Borramos lo que tenia la base de datos
        invitation.NewCreditCards.AddRange(cc);
      }
    }

    /// <summary>
    /// Guarda los depósitos
    /// </summary>
    private void SaveBookingDeposits()
    {
      invitation.NewDeposits = new List<Model.BookingDeposit>();
      invitation.UpdatedDeposits = new List<Model.BookingDeposit>();
      invitation.DeletedDeposits = new List<Model.BookingDeposit>();

      if (_lstObjInvitBookingDeposit.Any())
      {
        //Asignamos el guest a la lista
        _lstObjInvitBookingDeposit.ForEach(c =>
        {
          c.bdgu = c.bdgu != 0 ? c.bdgu : _guestID;
        });

        var deposits = _lstObjInvitBookingDeposit.Select(c => new BookingDeposit
        {
          bdAmount = c.bdAmount,
          bdAuth = c.bdAuth,
          bdCardNum = c.bdCardNum,
          bdcc = c.bdcc,
          bdcu = c.bdcu,
          bddr = c.bddr,
          bdEntryDCXC = c.bdEntryDCXC.HasValue ? c.bdEntryDCXC : DateTime.Today,
          bdExpD = c.bdExpD,
          bdFolioCXC = c.bdFolioCXC,
          bdgu = c.bdgu,
          bdID = c.bdID,
          bdpc = c.bdpc,
          bdpt = c.bdpt,
          bdReceived = c.bdReceived,
          bdRefund = c.bdRefund,
          bdUserCXC = !String.IsNullOrEmpty(c.bdUserCXC) ? c.bdUserCXC : _user.User.peID
        }).ToList();

        //Agregamos los nuevos depósitos
        var newDeposit = deposits.Where(c => c.bdID == 0).ToList();
        invitation.NewDeposits.AddRange(newDeposit);

        foreach (var row in deposits.Where(c => c.bdID > 0).ToList())
        {
          //Buscamos los depositos en la lista que es igual a la base de datos
          var d = _lstDeposits.SingleOrDefault(de => de.bdID == row.bdID);
          if(d != null)//si está se actualiza
          {
            d.bdAmount = row.bdAmount;
            d.bdAuth = row.bdAuth;
            d.bdCardNum = row.bdCardNum;
            d.bdcc = row.bdcc;
            d.bdcu = row.bdcu;
            d.bddr = row.bddr;
            d.bdEntryDCXC = row.bdEntryDCXC;
            d.bdExpD = row.bdExpD;
            d.bdFolioCXC = row.bdFolioCXC;
            d.bdgu = row.bdgu;
            d.bdID = row.bdID;
            d.bdpc = row.bdpc;
            d.bdpt = row.bdpt;
            d.bdReceived = row.bdReceived;
            d.bdRefund = row.bdRefund;
            d.bdUserCXC = row.bdUserCXC;
            invitation.UpdatedDeposits.Add(d);
          }
        }
      }
      
    }

    /// <summary>
    /// Guarda los invitados adicionales
    /// </summary>
    private void SaveAdditionalGuests()
    {
      invitation.NewAdditional = new List<Guest>();
      invitation.UpdatedAdditional = new List<Guest>();
      invitation.DeletedAdditional = new List<Guest>();

      foreach(var row in _lstObjInvitAdditionalGuest)
      {
        var guest = IM.BusinessRules.BR.BRGuests.GetGuestById(row.guID);
        invitation.NewAdditional.Add(guest); //Agregamos lo que tiene el grid
        invitation.DeletedAdditional.Add(guest);//Borramos lo que tenia la base de datos
      }
    }
    #endregion

    #region Métodos para validar al información

    /// <summary>
    /// Revisa que todas las validaciones sean aplicadas antes de guardar
    /// </summary>
    /// <returns></returns>
    private bool Validate()
    {
      bool res = true;

      if (!GeneralValidation())
        res = false;
      else if (!ValidateGifts())
        res = false;
      else if (!ValidateGuestStatus())
        res = false;
      else if (!ValidateDesposits())
        res = false;
      else if (!ValidateCreditCard())
        res = false;

      return res;

    }

    /// <summary>
    /// Valida los datos generales
    /// </summary>
    /// <returns>Boolean</returns>
    private bool GeneralValidation()
    {
      bool res = true;
      tbiGeneral.IsSelected = true;
      if (_invitationType == EnumInvitationType.InHouse && !ValidateFolioInHouse()) //validamos que el folio de invitación inhouse
      {
        Helpers.UIHelper.ShowMessage("The reservation number has been assigned previously");
        res = false;
      }
      else if (_invitationType == EnumInvitationType.OutHouse && String.IsNullOrEmpty(txtPRContract.Text))//validamos que haya seleccionado un proveedor Contact
      {
        Helpers.UIHelper.ShowMessage("Specify a PR Contract");
        cmbPRContract.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtPR.Text))//validamos que haya seleccionado un proveedor
      {
        Helpers.UIHelper.ShowMessage("Specify a PR");
        cmbPR.Focus();
        res = false;
      }
      else if (_invitationType != EnumInvitationType.Host && String.IsNullOrEmpty(txtSalesRoom.Text))//validamos que haya seleccionado una sala de ventas
      {
        Helpers.UIHelper.ShowMessage("Specify a Sales Room");
        cmbSalesRoom.Focus();
        res = false;
      }
      else if (!ValidateBookindAndRescheduleDate()) //validamos las fechas de booking y reschedule
      {
        res = false;
      }
      else if (_invitationType == EnumInvitationType.Host && String.IsNullOrEmpty(txtLocation.Text))  // validamos la locacion
      {
        Helpers.UIHelper.ShowMessage("Specify a Location");
        cmbLocation.Focus();
        res = false;
      }
      else if (_invitationType == EnumInvitationType.OutHouse)
      {
        if (String.IsNullOrEmpty(txtOutInvitation.Text)) //validamos que el campo de folio Out está lleno
        {
          Helpers.UIHelper.ShowMessage("Enter an Outhouse Invitation Folio");
          txtOutInvitation.Focus();
          res = false;
        }
        else if (!ValidateFolioOutHouse()) //validamos el la invitacion Outhouse
        {
          Helpers.UIHelper.ShowMessage("Outhouse Invitation Folio has been used or the format is incorrect(AAA-000)");
          txtOutInvitation.Focus();
          res = false;
        }
      }
      else if (!txtBookingDate.SelectedDate.HasValue)//Validamos que haya ingresado una fecha de booking
      {
        Helpers.UIHelper.ShowMessage("Specify the Booking date");
        txtBookingDate.Focus();
        res = false;
      }
      else if (cmbBookingTime.SelectedIndex == -1)//Validamos que haya ingresado una fecha de booking
      {
        Helpers.UIHelper.ShowMessage("Specify the Booking time");
        cmbBookingTime.Focus();
        res = false;
      }
      else if (ValidateClosedDate())//validamos que la fecha de bookin no este en fecha cerrada
      {
        Helpers.UIHelper.ShowMessage("It's not allowed to make Invitations for a closed date.");
        txtBookingDate.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtLastNameGuest1.Text))//' validamos el apellido
      {
        Helpers.UIHelper.ShowMessage("Enter last name of Guest 1.");
        txtLastNameGuest1.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtFirstNameGuest1.Text))//' validamos el nombre
      {
        Helpers.UIHelper.ShowMessage("Enter firts name of Guest 1.");
        txtFirstNameGuest1.Focus();
        res = false;
      }
      else if (cmbHotel.SelectedIndex == -1 || cmbHotel.SelectedValue == null || String.IsNullOrEmpty(cmbHotel.SelectedValue.ToString()))// validamos el hotel
      {
        Helpers.UIHelper.ShowMessage("Specify a Hotel");
        cmbHotel.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtAgency.Text)) //validamos la agencia
      {
        Helpers.UIHelper.ShowMessage("Specify an Agency");
        cmbAgency.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtPax.Text)) // validamos el pax
      {
        Helpers.UIHelper.ShowMessage("Specify a Pax");
        txtPax.Focus();
        res = false;
      }
      else if (System.Text.RegularExpressions.Regex.IsMatch(txtPax.Text, @"[^0-9.-]+"))//validamos que el pax sea númerico 
      {
        Helpers.UIHelper.ShowMessage("Pax control accepts numbers");
        txtPax.Focus();
        res = false;
      }
      else if (Convert.ToDecimal(txtPax.Text) < 0.1m || Convert.ToDecimal(txtPax.Text) > 1000) //vlidamos que el pax se entre entre 1y mil
      {
        Helpers.UIHelper.ShowMessage("Pax is out of range. Allowed values are 0.1 to 1000");
        txtPax.Focus();
        res = false;
      }
      else if (!txtArrival.SelectedDate.HasValue)//validamos la fecha de llegada
      {
        Helpers.UIHelper.ShowMessage("Specify a check-in date");
        txtArrival.Focus();
        res = false;
      }
      else if (!txtDeparture.SelectedDate.HasValue)//validamos la fecha de salida
      {
        Helpers.UIHelper.ShowMessage("Specify a check-out date");
        txtDeparture.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtCountry.Text))//validamos el país
      {
        Helpers.UIHelper.ShowMessage("Specify a country");
        cmbCountry.Focus();
        res = false;
      }
      
      return res;
    }

    /// <summary>
    /// Valida que almenos se haya registrado un estatus
    /// </summary>
    /// <returns></returns>
    private bool ValidateGuestStatus()
    {
      bool res = true;
      string title = "Guest Status Section";
      if (!_lstObjInvitGuestStatus.Any())
      {
        Helpers.UIHelper.ShowMessage("Specify at least one status", title: title);
        res = false;
      }
      else if (_lstObjInvitGuestStatus.Where(g => g.gtQuantity == 0).Any())
      {
        Helpers.UIHelper.ShowMessage("Any status does not has a quantity", title: title);
        res = false;
      }
      else if (_lstObjInvitGuestStatus.Where(g => String.IsNullOrEmpty(g.gtgs)).Any())
      {
        Helpers.UIHelper.ShowMessage("Any status does not has a status", title: title);
        res = false;
      }

      if (!res)
      {
        tbiOtherInformation.IsSelected = true;
        dtgGuestStatus.Focus();
      }
      return res;
    }

    /// <summary>
    /// Validamos la información de los nuevos regalos
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateGifts()
    {
      bool res = true;
      string title = "Gifts Section";
      //Revisamos que todos los regalos tengan una cantidad
      if (_lstObjInvitGift.Where(g => g.igQty == 0).Any())
      {
        res = false;
        Helpers.UIHelper.ShowMessage("any of the gifts does not have a specific quantity", title: title);
      }
      else if (_lstObjInvitGift.Where(g => String.IsNullOrEmpty(g.iggi)).Any()) //Revisamos que todos los registros tengan un regalo
      {
        res = false;
        Helpers.UIHelper.ShowMessage("any of the gifts does not have a specific gift", title: title);
      }
      else if (_lstObjInvitGift.Where(g => g.igAdults == 0).Any()) //Revisamos que todos los registros tengan almenos un Adulto asignado
      {
        res = false;
        Helpers.UIHelper.ShowMessage("any of the gifts does not have a specific adults", title: title);
      }

      if (res)
      {
        var msjErrorGifts = new StringBuilder();
        foreach (var row in _lstObjInvitGift)
        {
          var gift = BRGifts.GetGiftId(row.iggi);
          if (gift.giMaxQty > 0 && row.igQty > gift.giMaxQty)
          {
            msjErrorGifts.Append(String.Format("The maximum quantity authorized of the gift {0} has been exceeded.\n Max authotized = {1} \n", gift.giN, gift.giMaxQty));
            res = false;
            break;
          }
        }
        if(msjErrorGifts.Length > 0)
        {
          msjErrorGifts.Append("Do you want to modify the gifts?");
          var msgRes = MessageBox.Show(msjErrorGifts.ToString(), title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
          if (msgRes != MessageBoxResult.No)
          {
            res = false;
          }
          else 
          {
            res = true;
          }
        }
        
      }
      
      if (!res)
      {
        tbiGeneral.IsSelected = true;
        dtgGifts.Focus();
      }
      return res;
    }

    /// <summary>
    /// Validamos la información de los nuevos depósitos
    /// </summary>
    /// <returns></returns>
    private bool ValidateDesposits()
    {
      bool res = true;
      string title = "Deposit Section";
      
      if (_lstObjInvitBookingDeposit.Where(c => String.IsNullOrEmpty(c.bdcu)).Any()) // validamos que se especifique la moneda
      {
        foreach (var row in _lstObjInvitBookingDeposit.Where(c => String.IsNullOrEmpty(c.bdcu)).ToList())
        {
          if (row.bdAmount != 0 || row.bdReceived != 0)//' si se capturo alguno de los montos
          {
            Helpers.UIHelper.ShowMessage("Any Amount without currency specified.", title: title);
            res = false;
            break;
          }
        }
      }
      else if (_lstObjInvitBookingDeposit.Where(c => String.IsNullOrEmpty(c.bdpt)).Any())// validamos que se especifique la forma de pago
      {
        Helpers.UIHelper.ShowMessage("Any Deposit does not has a paymente type specified.", title: title);
        res = false;
      }
      else if (_lstObjInvitBookingDeposit.Where(c => c.bdAmount == 0 && c.bdReceived == 0).Any())// validamos que se especifique alguno de los montos
      {
        Helpers.UIHelper.ShowMessage("Currency without Amount specified.", title: title);
        res = false;
      }
      else if (_lstObjInvitBookingDeposit.Where(c => String.IsNullOrEmpty(c.bdpc)).Any())// validamos si especifico un lugar de pago
      {
        Helpers.UIHelper.ShowMessage("Select a place of payment.", title: title);
        res = false;
      }
      else if (_lstObjInvitBookingDeposit.Where(c => c.bdpt.ToUpper() == "CC").Any()) //Validamos la informacion de la tarjeta  de credito
      {
        foreach (var row in _lstObjInvitBookingDeposit.Where(c => c.bdpt.ToUpper() == "CC").ToList())
        {
          if (String.IsNullOrEmpty(row.bdcc))
          {
            Helpers.UIHelper.ShowMessage("Select a credit card", title: title);
            res = false;
            break;
          }
          else if (String.IsNullOrEmpty(row.bdCardNum))
          {
            Helpers.UIHelper.ShowMessage("Input the last four numbers", title: title);
            res = false;
            break;
          }
          else if (String.IsNullOrEmpty(row.bdExpD))//Validamos la fecha de expiracion
          {
            Helpers.UIHelper.ShowMessage("Input the expire date", title: title);
            res = false;
            break;
          }
          else if (!String.IsNullOrEmpty(row.bdExpD))//Validamos la fecha de expiracion
          {
            var dt = new DateTime();
            var exp = "01/" + row.bdExpD;
            if (!DateTime.TryParse(exp, out dt))
            {
              Helpers.UIHelper.ShowMessage("The expire date does not has a correct format. (MM/YY)", title: title);
              res = false;
              break;
            }
          }
          else if (!string.IsNullOrEmpty(row.bdAuth))
          {
            Helpers.UIHelper.ShowMessage("Input the Authorizathion number", title: title);
            res = false;
            break;
          }
        }
      }
      else if (_lstObjInvitBookingDeposit.Where(c => (c.bdAmount - c.bdReceived) > 0 && !c.bdFolioCXC.HasValue).Any())//Validamos el importe de CXC  bdAmount - bdReceived
      {
        Helpers.UIHelper.ShowMessage("Add Folio CXC information.", title: title);
        res = false;
      }

      if (!res)
      {
        tbiOtherInformation.IsSelected = true;
        dtgDeposits.Focus();
      }

      return res;
    }

    /// <summary>
    /// Validamos la información de una nueva tarjeta de credito
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateCreditCard()
    {
      bool res = true;
      string title = "Credit Card Section";
      if (!dtgCCCompany.HasItems) return res;

      if (_lstObjInvitCreditCard.Where(c => String.IsNullOrEmpty(c.gdcc)).Any())
      {
        Helpers.UIHelper.ShowMessage("Select a credit card", title: title);
        res = false;
      }
      else if (_lstObjInvitCreditCard.Where(c => c.gdQuantity == 0).Any())
      {
        Helpers.UIHelper.ShowMessage("Input a quantity", title: title);
        res = false;
      }

      if (!res)
      {
        tbiOtherInformation.IsSelected = true;
        dtgCCCompany.Focus();
      }
      return res;
    }

    /// <summary>
    /// Validamos que el folio de invitación no se repita
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateFolioInHouse()
    {
      bool res = true;

      if (!btnSearch.IsEnabled) return res;

      res = IM.BusinessRules.BR.BRFolios.ValidateFolioReservation(_user.LeadSource.lsID, txtReservationNumber.Text, Convert.ToInt32(txtGuid.Text));

      return res;
    }

    /// <summary>
    /// Validamos que el folio de invitación no se repita
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateFolioOutHouse()
    {
      bool res = true;

      if (!btnSearch.IsEnabled) return res;

      string serie = String.Empty;
      int numero = -1;

      if (!ValidateFolioOuthouseFormat(ref serie, ref numero))
        return false;

      res = IM.BusinessRules.BR.BRFolios.ValidateFolioInvitationOutside(Convert.ToInt32(txtGuid.Text), serie, numero);
      return res;
    }

    /// <summary>
    /// Validamos que el formato del folio de una invitación OutHouse sea correcto. Letra(s) - Número(s) (AAA-123)
    /// </summary>
    /// <param name="serie">Ref Serie del folio</param>
    /// <param name="numero">Ref Número del folio</param>
    /// <returns>Boolean</returns>
    private bool ValidateFolioOuthouseFormat(ref string serie, ref int numero)
    {
      try
      {
        bool res = true;

        //creamos un arreglo apartir del guión.
        var array = txtOutInvitation.Text.Split('-');

        if (array.Length != 2) //Revisamos que sea un arreglo de 2 exclusivamente
          return false;

        if (!System.Text.RegularExpressions.Regex.IsMatch(array[0], @"^[a-zA-Z]+$"))//Revisamos que la serie solo contenga letras
          return false;

        serie = array[0]; //Asignamos la serie
        numero = int.Parse(array[1]); //Asignamos el número, en caso de crear una excepción se retornará false

        return res;
      }
      catch (Exception)
      {
        return false;
      }
    }

    /// <summary>
    /// Validamos que la fecha de booking no este en fecha cerrada
    /// </summary>
    /// <returns></returns>
    private bool ValidateClosedDate()
    {
      if (!_closeDate.HasValue) return false;
      return txtBookingDate.SelectedDate.Value.Date <= _closeDate.Value.Date;
    }

    /// <summary>
    /// Validamos las fechas de booking y de reschedule
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateBookindAndRescheduleDate()
    {
      bool res;
      res = ValidateBookingDateTime();

      if(res)
      {
        ValidateRescheduleDateTime();
      }

      return res;
    }

    /// <summary>
    /// Validamos la fecha y hora booking
    /// </summary>
    /// <returns></returns>
    private bool ValidateBookingDateTime()
    {
      bool res = true;

      if(!txtBookingDate.SelectedDate.HasValue)
      {
        Helpers.UIHelper.ShowMessage("Specify a Booking Date");
        txtBookingDate.Focus();
        res = false;
      }
      else if(_user.Permissions.Where(p=> p.plN == "SuperSpecial").Any())// si tiene permiso super especial de invitaciones
      {
        #region Instrucciones
        /*
        Permiso super especial (4) cuando hace una nueva.
        1. No puede hacer invitacion antes de la fecha de llegada
        2. Si puede hacer invitaciones despues de la fecha de salida pero la fecha de booking debe estar dentro del periodo de estadia del huesped
        3. Si puede hacer invitaciones de dias anteriores
        4. Tiene un periodo de 30 dias despues de la fecha de salida para hacer la invitacion
        */
        #endregion

        if (DateTime.Today >= txtBookingDate.SelectedDate.Value.AddDays(30)) //validamos que el invitado no tenga un mes de que haya salido
        {
          Helpers.UIHelper.ShowMessage("Guest already made check out.");
          txtBookingDate.Focus();
          res = false;
        }
      }
      else
      {
        if(txtBookingDate.SelectedDate.Value.Date < DateTime.Today.Date)//validamos que la fecha de booking no sea antes de hoy
        {
          Helpers.UIHelper.ShowMessage("Booking date can not be before today.");
          txtBookingDate.Focus();
          res = false;
        }
      }

      if (!res) return res;

      if (txtDeparture.SelectedDate.HasValue && (txtDeparture.SelectedDate.Value.Date < txtBookingDate.SelectedDate.Value.Date)) //Validamos que la fecha de entrada no sea mayor ala fecha de salida
      {
        Helpers.UIHelper.ShowMessage("Booking date can not be after Check Out date");
        txtBookingDate.Focus();
        res = false;
      }
      else if (txtArrival.SelectedDate.HasValue && (txtArrival.SelectedDate.Value.Date > txtBookingDate.SelectedDate.Value.Date))
      {
        Helpers.UIHelper.ShowMessage("Booking date can not be before Check In date.");
        txtBookingDate.Focus();
        res = false;
      }

      if(cmbBookingTime.SelectedValue == null || String.IsNullOrEmpty(cmbBookingTime.SelectedValue.ToString()) || cmbBookingTime.SelectedIndex == -1)
      {
        Helpers.UIHelper.ShowMessage("Specify a Booking Time.");
        cmbBookingTime.Focus();
        res = false;
      }

      return res;
    }

    /// <summary>
    /// Validamos la fecha y hora reschedule
    /// </summary>
    /// <returns></returns>
    private bool ValidateRescheduleDateTime()
    {
      bool res = true;
      string errorMessage = String.Empty;
      if (!_allowReschedule) return false;// si se permite reschedule

      if (!txtRescheduleDate.IsEnabled)// si se puede modificar la fecha de reschedule
        return false;

      if (!txtRescheduleDate.SelectedDate.HasValue)
      {
        errorMessage ="Specify the reschedule date.";
        res = false;
      }
      else if (txtDeparture.SelectedDate.HasValue && (txtRescheduleDate.SelectedDate.Value.Date > txtDeparture.SelectedDate.Value.Date)) //validamos que la fecha de reschedule no sea despues de la fecha de salida
      {
        errorMessage = "Reschedule date can not be after check out date.";
        res = false;
      }
      else if (txtArrival.SelectedDate.HasValue && (txtRescheduleDate.SelectedDate.Value.Date < txtArrival.SelectedDate.Value.Date)) // validamos que la fecha de reschedule no sea antes de la fecha de llegada
      {
        errorMessage = "Reschedule date can not be before booking date.";
        res = false;
      }
      else if (txtRescheduleDate.SelectedDate.Value.Date < DateTime.Today.Date) // validamos que la fecha de reschedule no sea antes de hoy
      {
        errorMessage = "Reschedule date can not be before today.";
        res = false;
      }
      else if (cmbRescheduleTime.SelectedValue == null || String.IsNullOrEmpty(cmbRescheduleTime.SelectedValue.ToString()) || cmbRescheduleTime.SelectedIndex == -1)// validamos la hora de reschedule
      {
        errorMessage = "Specify a reschedule time.";
        res = false;
      }
      else if (txtRescheduleDate.SelectedDate.Value == txtBookingDate.SelectedDate.Value) // validamos que la hora de reschedule no sea la misma o antes de la hora de booking
      {
        if (DateTime.Parse(cmbRescheduleTime.SelectedValue.ToString()) <= DateTime.Parse(cmbBookingTime.SelectedValue.ToString()))
        {
          errorMessage = "Reschedule time can not be before booking time.";
          res = false;
        }
      }

      if (!res)
      {
        Helpers.UIHelper.ShowMessage(errorMessage);
        txtRescheduleDate.Focus();
      }

      return res;
    }
       
    /// <summary>
    /// Validamos la información introducida en el campo Expiration
    /// </summary>
    /// <returns></returns>
    private bool ValidateExpirationDate(string date)
    {
      bool res = true;
      
      DateTime dt = new DateTime();
      res = DateTime.TryParseExact(date, "MM/yy", new CultureInfo("es-MX"), DateTimeStyles.None, out dt);

      return res;
    }

    #endregion

    #region Métodos para asignar valores al objeto Guest

    /// <summary>
    /// Devuelme una cadena
    /// </summary>
    /// <param name="value">objeto que se recibe</param>
    /// <returns>string</returns>
    private string ForStringValue(object value)
    {
      if (value == null) return String.Empty;
      return String.IsNullOrEmpty(value.ToString()) ? String.Empty : value.ToString().Trim();
    }
    
    /// <summary>
    /// Devuelme un decimal
    /// </summary>
    /// <param name="value">objeto que se recibe</param>
    /// <returns>string</returns>
    private decimal ForDecimalValue(object value)
    {
      return String.IsNullOrEmpty(value.ToString().Trim()) ? 0 : Convert.ToDecimal(value.ToString().Trim());
    }

    /// <summary>
    /// Devuelme un intero
    /// </summary>
    /// <param name="value">objeto que se recibe</param>
    /// <returns>string</returns>
    private int ForIntegerValue(object value)
    {
      return String.IsNullOrEmpty(value.ToString().Trim()) ? 0 : Convert.ToInt32(value.ToString().Trim());
    }

    /// <summary>
    /// Devuelve un booleano
    /// </summary>
    /// <param name="value">objeto que se recibe</param>
    /// <returns>bool</returns>
    private bool ForBooleanValue(bool? value)
    {
      return value.HasValue ? value.Value : false;
    }






    #endregion

    #region Métodos para calcular los costos de los regalos

    /// <summary>
    /// Calcula los costos y precios de adultos y menores de un regalo
    /// </summary>
    /// <param name="useCxCCost">Indica si se utilizará el costo del empleado</param>
    private void CalculateCostsPrices (bool useCxCCost = false)
    {
      decimal costAdult, costMinor, priceAdult, priceMinor, priceExtraAdult, quantity;
      
      foreach(var row in _lstObjInvitGift)
      {
        var gift = IM.BusinessRules.BR.BRGifts.GetGiftId(row.iggi);
        if(gift != null)
        {
          // Costos
          // si se va a usar el costo de empleado
          if (useCxCCost)
          {
            costAdult = gift.giPrice1;
            costMinor = gift.giPrice4;
          }
          else // se va a usar el cosrto al público
          {
            costAdult = gift.giPrice1;
            costMinor = gift.giPrice2;
          }

          // Precios
          priceAdult = gift.giPublicPrice;
          priceMinor = gift.giPriceMinor;
          priceExtraAdult = gift.giPriceExtraAdult;
          quantity = row.igQty;

          // Total del costo adultos
          row.igPriceA = quantity * (row.igAdults + row.igExtraAdults) * costAdult;
          // Total del costo de menores
          row.igPriceM = quantity * row.igMinors * costMinor;
          // Total del precio adultos
          row.igPriceAdult = quantity * row.igAdults * priceAdult;
          //Total del precio de menores
          row.igPriceMinor = quantity * row.igMinors * priceMinor;
          // Total del precio de adultos extra
          row.igPriceExtraAdult = quantity * row.igExtraAdults * priceExtraAdult;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="onlyCancellled"></param>
    /// <param name="cancel"></param>
    private void CalculateTotalGifts(bool onlyCancellled = false, string cancel = "")
    {
      decimal cost, price, totalCost = 0, totalPrice = 0;

      foreach(var row in _lstObjInvitGift)
      {
        
        // calculamos el costo del regalo
        cost = row.igPriceA + row.igPriceM;

        //calculamos el precio del regalo
        price = row.igPriceAdult + row.igPriceMinor + row.igPriceExtraAdult;

        //si se desean todos los regalos
        if(!onlyCancellled)
        {
          totalCost += cost;
          totalPrice += price;
        }
       

      }
      txtTotalCost.Text = totalCost.ToString("$#,##0.00;$(#,##0.00)");
      txtTotalPrice.Text = totalPrice.ToString("$#,##0.00;$(#,##0.00)");
    }

    /// <summary>
    /// Calcula el monto maximo de regalos
    /// </summary>
    /// <history>
    /// [emoguel]  modified 27/06/2016
    /// </history>
    private async void CalculateMaxAuthGifts()
    {
      decimal maxAuthGifts = 0;

      foreach(var row in _lstObjInvitGuestStatus)
      {
        if (row.gtgs != null && row.gtQuantity > 0)
        {
          var guestStatusType = await BRGuestStatusTypes.GetGuestStatusTypes(new Model.GuestStatusType { gsID = row.gtgs });
          var guestStaType=guestStatusType.FirstOrDefault();
          maxAuthGifts += row.gtQuantity * guestStaType.gsMaxAuthGifts;
        }
      }

      txtMaxAuth.Text = maxAuthGifts.ToString("$#,##0.00;$(#,##0.00)");
    }
    #endregion

    #endregion

  }
}

