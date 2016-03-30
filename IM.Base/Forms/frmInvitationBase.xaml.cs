using IM.Model.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IM.BusinessRules.Enums;
using System.Linq;
using System.Windows.Data;
using IM.Model.Enums;
using System.Globalization;
using IM.BusinessRules.Classes;
using System.IO;

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
    List<GiftInvitation> gifts = new List<GiftInvitation>();
    List<IM.Model.InvitationGift> _lstGifts = new List<Model.InvitationGift>();
    List<IM.Model.BookingDeposit> _lstDeposits = new List<Model.BookingDeposit>();
    List<IM.Model.GuestStatus> _lstGuestStatus = new List<Model.GuestStatus>();
    List<IM.Model.GuestCreditCard> _lstCreditsCard = new List<Model.GuestCreditCard>();
    List<IM.Model.Guest> _lstAdditionals = new List<Model.Guest>();

    List<GiftInvitation> _lstNewGifts = new List<GiftInvitation>();
    List<BookingDepositInvitation> _lstNewDeposits = new List<BookingDepositInvitation>();
    List<GuestStatusInvitation> _lstNewGuestStatus = new List<GuestStatusInvitation>();
    List<GuestCreditCardInvitation> _lstNewCreditsCard = new List<GuestCreditCardInvitation>();
    List<GuestAdditionalInvitation> _lstNewAdditionals = new List<GuestAdditionalInvitation>();
    #endregion

    #region Objetos
    private InvitationType _invitationType;
    private UserData _user;
    private IM.Model.Guest _guestAdditional;
    private GiftInvitation _previousGift;
    private BookingDepositInvitation _previousDeposit;
    private GuestStatusInvitation _previousGuestStatus;
    private GuestCreditCardInvitation _previousCreditCard;
    private IM.Model.Guest _previousGuestAdditional;
    #endregion

    private const bool allowReschedule = true;

    private bool _wasSelectedByKeyboard = false;
    private int _guestID;
    private IM.Model.Enums.EnumInvitationMode _invitationMode;
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

    private bool _wasUpdated = false;

    #endregion

    #region Constructores y destructores

    public frmInvitationBase()
    {
      InitializeComponent();
    }

    public frmInvitationBase(IM.BusinessRules.Enums.InvitationType invitationType, UserData userData, int guestID, IM.Model.Enums.EnumInvitationMode invitationMode)
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

      this._invitationType = invitationType;
      this._user = userData;
      this._guestID = guestID;
      this._invitationMode = invitationMode;
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
    /// </history>
    private void frmInvitationBase_Loaded(object sender, RoutedEventArgs e)
    {
      if (_invitationMode == EnumInvitationMode.modAdd)//' si el huesped no ha sido invitado
      {
        ValidateEdit();
      }
      ControlsConfiguration();
      LoadControls();
      BackupOriginalValues();
      EnableControls();

      _serverDateTime = IM.BusinessRules.BR.BRConfiguration.GetServerDateTime();
      _bookingDate = txtBookingDate.SelectedDate.HasValue ? txtBookingDate.SelectedDate.Value : (DateTime?)null;
      _closeDate = IM.BusinessRules.BR.BRConfiguration.GetCloseDate();

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

      if (allowReschedule)//si se permite hacer reschedules
      {
        var invitD = Convert.ToDateTime(txtDate.Text);
        //si la fecha de invitacion es hoy o si la fecha de booking es despues hoy
        if (invitD == _serverDateTime || txtBookingDate.SelectedDate.Value > _serverDateTime)
        {
          //' Fecha de booking
          txtBookingDate.IsEnabled = true;

          //si tiene permiso especial de invitaciones
          if (_user.HasPermission(_invitationType == InvitationType.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations, EnumPermisionLevel.Special)
              || _user.HasPermission(_invitationType == InvitationType.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations, EnumPermisionLevel.SuperSpecial))
          {
            //PR Contract
            if (_invitationType == InvitationType.OutHouse)
            {
              txtPRContract.IsEnabled = true;
              cmbPRContract.IsEnabled = true;
            }

            //PR
            txtPR.IsEnabled = true;
            cmbPR.IsEnabled = true;

            //Sala /Location
            if (_invitationType == InvitationType.Host)
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
        if (_invitationType == InvitationType.OutHouse)
        {
          txtPRContract.IsEnabled = true;
          cmbPRContract.IsEnabled = true;
        }

        //PR
        txtPR.IsEnabled = true;
        cmbPR.IsEnabled = true;

        //Sala /Location
        if (_invitationType == InvitationType.Host)
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

      if (_invitationType == InvitationType.Host)
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
      if (_invitationType == InvitationType.OutHouse)
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

      if (_invitationType == InvitationType.InHouse || _invitationType == InvitationType.Animation || _invitationType == InvitationType.Regen)
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
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region Métodos de los controles de la sección BUTTONS AREA (Edit / Print / Save / Cancel / Log)

    /// <summary>
    /// Ejecuta el botón Editar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      _wasPressedEditButton = true;
      if (ValidateEdit())
      {
        //si no tiene show
        if (chkShow.IsChecked.HasValue && !chkShow.IsChecked.Value)
        {
          _invitationMode = EnumInvitationMode.modEdit;
          EnableControls();
        }
        else
        {
          Helpers.UIHelper.ShowMessage("Guest has made Show");
          //establecemos el modo lectura
          _invitationMode = EnumInvitationMode.modOnlyRead;
          EnableControls();
        }
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

    #region Métodos de los controles de la sección OTHER INFORMATION

    /// <summary>
    /// Selecciona el hotel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbHotel_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

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

    #region Métodos de los controles de la sección GIFTS
    /// <summary>
    /// Guardamos en una lista temporal los regalos asignados al invitado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddGift_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateNewGift())
      {
        IM.Model.InvitationGift gift;
        var g = IM.BusinessRules.BR.BRGifts.GetGiftId(cmbGifts.SelectedValue.ToString());
        if (!_wasUpdated)
        {
          gift = new IM.Model.InvitationGift();
          gift.iggu = _guestID;
          gift.igQty = Convert.ToInt32(txtQtyGift.Text);
          gift.iggi = cmbGifts.SelectedValue.ToString();
          gift.igAdults = Convert.ToInt32(txtAdultGift.Text);
          gift.igMinors = Convert.ToInt32(txtMinortGift.Text);
          gift.igExtraAdults = Convert.ToInt32(txtEAdultGift.Text);
          gift.igct = "MARKETING";
          gift.igPriceA = g.giPrice1;
          gift.igPriceAdult = g.giPrice1;
          gift.igPriceM = g.giPriceMinor;
          gift.igPriceMinor = g.giPriceMinor;
          gift.igPriceExtraAdult = g.giPriceExtraAdult;

          _lstNewGifts.Add(ConvertInvitationGiftToGiftInvitationObject(gift, true));
        }
        else
        {
          //buscamos si el regalo que editó es de los que acaba de ingresar para actualizarlo
          var giftNew = _lstNewGifts.Where(gi => gi.iggu == _guestID && gi.iggi == _previousGift.iggi).SingleOrDefault();
          if (giftNew != null)
          {
            giftNew.igQty = Convert.ToInt32(txtQtyGift.Text);
            giftNew.iggi = cmbGifts.SelectedValue.ToString();
            giftNew.igAdults = Convert.ToInt32(txtAdultGift.Text);
            giftNew.igMinors = Convert.ToInt32(txtMinortGift.Text);
            giftNew.igExtraAdults = Convert.ToInt32(txtEAdultGift.Text);
            giftNew.igct = "MARKETING";
            giftNew.igPriceA = g.giPrice1;
            giftNew.igPriceAdult = g.giPrice1;
            giftNew.igPriceM = g.giPriceMinor;
            giftNew.igPriceMinor = g.giPriceMinor;
            giftNew.igPriceExtraAdult = g.giPriceExtraAdult;
          }

          //buscamos si el regalo que editó es de los que acaba de ingresar para actualizarlo
          var giftUpdated = _lstGifts.Where(gi => gi.iggu == _guestID && gi.iggi == _previousGift.iggi).SingleOrDefault();

          if (giftUpdated != null)
          {
            giftUpdated.igQty = Convert.ToInt32(txtQtyGift.Text);
            giftUpdated.iggi = cmbGifts.SelectedValue.ToString();
            giftUpdated.igAdults = Convert.ToInt32(txtAdultGift.Text);
            giftUpdated.igMinors = Convert.ToInt32(txtMinortGift.Text);
            giftUpdated.igExtraAdults = Convert.ToInt32(txtEAdultGift.Text);
            giftUpdated.igct = "MARKETING";
            giftUpdated.igPriceA = g.giPrice1;
            giftUpdated.igPriceAdult = g.giPrice1;
            giftUpdated.igPriceM = g.giPriceMinor;
            giftUpdated.igPriceMinor = g.giPriceMinor;
            giftUpdated.igPriceExtraAdult = g.giPriceExtraAdult;
            _lstNewGifts.Add(ConvertInvitationGiftToGiftInvitationObject(giftUpdated, false, _previousGift.iggi));
            _lstGifts.Remove(giftUpdated);
          }
        }
        
        if (_wasUpdated)
        {
          tbiNewGift.Header = "New Gift";
          btnAddGift.Content = "Add";
        }

        LoadGiftGrid();

        txtQtyGift.Text = String.Empty;
        cmbGifts.SelectedIndex = -1;
        txtAdultGift.Text = "0";
        txtMinortGift.Text = "0";
        txtEAdultGift.Text = "0";

        tbiGiftsList.IsSelected = true;
        _wasUpdated = false;
      }
    }

    /// <summary>
    /// Editamos alguno de los registros dela lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtgGifts_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if ((GiftInvitation)dtgGifts.CurrentItem == null) return;
      _previousGift = (IM.BusinessRules.Classes.GiftInvitation)dtgGifts.CurrentItem;

      tbiNewGift.IsSelected = true;
      tbiNewGift.Header = "Update Gift";

      txtQtyGift.Text = _previousGift.igQty.ToString();
      cmbGifts.SelectedValue = _previousGift.iggi;
      txtAdultGift.Text = _previousGift.igAdults.ToString();
      txtMinortGift.Text = _previousGift.igMinors.ToString();
      txtEAdultGift.Text = _previousGift.igExtraAdults.ToString();
      _wasUpdated = true;
      btnAddGift.Content = "Update";
    }
    #endregion

    #region Métodos de los controles de la sección DEPOSIT
    /// <summary>
    /// Habilita los controles para el ingreso de los datos de las tarjetas de crédito 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbPaymentTypeDeposit_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      bool enable = cmbPaymentTypeDeposit.SelectedValue != null && cmbPaymentTypeDeposit.SelectedValue.ToString() == "CC";
      cmbCreditCardDeposit.IsEnabled = enable;
      txtCardNumberDeposit.IsEnabled = enable;
      txtExpirationCard.IsEnabled = enable;
      txtAuthorizathionId.IsEnabled = enable;

    }

    /// <summary>
    /// Guardamos en una lista temporal los depositos asignados al invitado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddDeposit_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateNewDesposit())
      {
        if (!_wasUpdated)
        {
          var dep = new IM.Model.BookingDeposit();
          dep.bdgu = _guestID;
          dep.bdAmount = Convert.ToDecimal(txtDeposit.Text);
          dep.bdReceived = Convert.ToDecimal(txtReceived.Text);
          dep.bdcu = cmbCurrencyDeposit.SelectedValue.ToString();
          dep.bdpt = cmbPaymentTypeDeposit.SelectedValue.ToString();
          dep.bdpc = txtRefundPlace.Text;
          dep.bdcc = cmbCreditCardDeposit.IsEnabled ? cmbCreditCardDeposit.SelectedValue.ToString() : null;
          dep.bdCardNum = txtCardNumberDeposit.IsEnabled ? txtCardNumberDeposit.Text : null;
          dep.bdExpD = txtExpirationCard.IsEnabled ? txtExpirationCard.Text : null;
          dep.bdAuth = txtAuthorizathionId.IsEnabled && !String.IsNullOrEmpty(txtAuthorizathionId.Text) ? int.Parse(txtAuthorizathionId.Text) : (int?) null;
          dep.bdFolioCXC = String.IsNullOrEmpty(txtFolioCxC.Text) ? (int?)null : int.Parse(txtFolioCxC.Text);
          dep.bdUserCXC = _user.User.peID;
          dep.bdEntryDCXC = _serverDateTime;

          _lstNewDeposits.Add(ConvertBookingDepositToBookingDepositInvitationObject(dep, true));
        }
        else
        {
          //buscamos si el depostio que editó es de los que acaba de ingresar para actualizarlo
          var depostitNew = _lstNewDeposits.Where(d => d.bdID == _previousDeposit.bdID).SingleOrDefault();
          if (depostitNew != null)
          {
            depostitNew.bdAmount = Convert.ToDecimal(txtDeposit.Text);
            depostitNew.bdReceived = Convert.ToDecimal(txtReceived.Text);
            depostitNew.bdcu = cmbCurrencyDeposit.SelectedValue.ToString();
            depostitNew.bdpt = cmbPaymentTypeDeposit.SelectedValue.ToString();
            depostitNew.bdpc = txtRefundPlace.Text;
            depostitNew.bdcc = cmbCreditCardDeposit.IsEnabled ? cmbCreditCardDeposit.SelectedValue.ToString() : null;
            depostitNew.bdCardNum = txtCardNumberDeposit.IsEnabled ? txtCardNumberDeposit.Text : null;
            depostitNew.bdExpD = txtExpirationCard.IsEnabled ? txtExpirationCard.Text : null;
            depostitNew.bdAuth = txtAuthorizathionId.IsEnabled && !String.IsNullOrEmpty(txtAuthorizathionId.Text) ? int.Parse(txtAuthorizathionId.Text) : (int?)null;
            depostitNew.bdFolioCXC = String.IsNullOrEmpty(txtFolioCxC.Text) ? (int?)null : int.Parse(txtFolioCxC.Text);
            depostitNew.bdUserCXC = _user.User.peID;
            depostitNew.bdEntryDCXC = _serverDateTime;
          }

          //buscamos si el depósito que editó es de los que acaba de ingresar para actualizarlo
          var depositUpdated = _lstDeposits.Where(d => d.bdID == _previousDeposit.bdID).SingleOrDefault();
          if (depositUpdated != null)
          {
            depositUpdated.bdAmount = Convert.ToDecimal(txtDeposit.Text);
            depositUpdated.bdReceived = Convert.ToDecimal(txtReceived.Text);
            depositUpdated.bdcu = cmbCurrencyDeposit.SelectedValue.ToString();
            depositUpdated.bdpt = cmbPaymentTypeDeposit.SelectedValue.ToString();
            depositUpdated.bdpc = txtRefundPlace.Text;
            depositUpdated.bdcc = cmbCreditCardDeposit.IsEnabled ? cmbCreditCardDeposit.SelectedValue.ToString() : null;
            depositUpdated.bdCardNum = txtCardNumberDeposit.IsEnabled ? txtCardNumberDeposit.Text : null;
            depositUpdated.bdExpD = txtExpirationCard.IsEnabled ? txtExpirationCard.Text : null;
            depositUpdated.bdAuth = txtAuthorizathionId.IsEnabled && !String.IsNullOrEmpty(txtAuthorizathionId.Text) ? int.Parse(txtAuthorizathionId.Text) : (int?)null;
            depositUpdated.bdFolioCXC = String.IsNullOrEmpty(txtFolioCxC.Text) ? (int?)null : int.Parse(txtFolioCxC.Text);
            depositUpdated.bdUserCXC = _user.User.peID;
            depositUpdated.bdEntryDCXC = _serverDateTime;
            _lstNewDeposits.Add(ConvertBookingDepositToBookingDepositInvitationObject(depositUpdated, false));
            _lstDeposits.Remove(depositUpdated);
          }

        }

        if (_wasUpdated)
        {
          tbiNewDeposit.Header = "New Deposit";
          btnAddDeposit.Content = "Add";
        }

        LoadDepositGrid();

        tbiDepositList.IsSelected = true;

        txtDeposit.Text = "0.0";
        txtReceived.Text = "0.0";
        cmbCurrencyDeposit.SelectedIndex = -1;
        cmbPaymentTypeDeposit.SelectedIndex = -1;
        txtRefundPlace.Text = String.Empty;
        cmbCreditCardDeposit.SelectedIndex = -1;
        txtCardNumberDeposit.Text = String.Empty;
        txtExpirationCard.Text = String.Empty;
        txtAuthorizathionId.Text = String.Empty;
        txtFolioCxC.Text = String.Empty;

        _wasUpdated = false;
      }
    }

    /// <summary>
    /// Editamos alguno de los registros dela lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtgDeposits_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if ((BookingDepositInvitation)dtgDeposits.CurrentItem == null) return;

      _previousDeposit = (BookingDepositInvitation)dtgDeposits.CurrentItem;

      tbiNewDeposit.Header = "Update Deposit";
      tbiNewDeposit.IsSelected = true;

      txtDeposit.Text = _previousDeposit.bdAmount.ToString();
      txtReceived.Text = _previousDeposit.bdReceived.ToString();
      cmbCurrencyDeposit.SelectedValue = _previousDeposit.bdcu;
      cmbPaymentTypeDeposit.SelectedValue = _previousDeposit.bdpt;
      txtRefundPlace.Text = _previousDeposit.bdpc;
      cmbCreditCardDeposit.SelectedValue = _previousDeposit.bdcc;
      txtCardNumberDeposit.Text = _previousDeposit.bdCardNum;
      txtExpirationCard.Text = _previousDeposit.bdExpD;
      txtAuthorizathionId.Text = !_previousDeposit.bdAuth.HasValue ? String.Empty : _previousDeposit.bdAuth.Value.ToString();
      txtFolioCxC.Text = _previousDeposit.bdFolioCXC.HasValue ? _previousDeposit.bdFolioCXC.Value.ToString() : String.Empty;
      _wasUpdated = true;
      btnAddDeposit.Content = "Update";
    }
    #endregion

    #region Métodos de los controles de la sección GUESTSTATUS
    /// <summary>
    ///Agrega un estado del invitado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddGuestStatus_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateNewGuestStatus())
      {
        if(!_wasUpdated)
        {
          var gs = new IM.Model.GuestStatus();

          gs.gtQuantity = Convert.ToByte(txtQtyGuestStatus.Text);
          gs.gtgs = cmbGuestStatus.SelectedValue.ToString();
          gs.gtgu = _guestID;

          _lstNewGuestStatus.Add(ConvertGuestStatusToGuestStatusInvitationObject(gs, true));
        }
        else
        {
          //buscamos si el estatus que editó es de los que acaba de ingresar para actualizarlo
          var gsNew = _lstNewGuestStatus.SingleOrDefault(g => g.gsID == _previousGuestStatus.gsID && g.gsgu == _previousGuestStatus.gsgu);
          if (gsNew != null)
          {
            gsNew.gsQty = Convert.ToByte(txtQtyGuestStatus.Text);
            gsNew.gsID = cmbGuestStatus.SelectedValue.ToString();
          }

          //buscamos si el estatus que editó es de que extraímos de la base de datos
          var gsUpdated = _lstGuestStatus.SingleOrDefault(g => g.gtgs == _previousGuestStatus.gsID && g.gtgu == _previousGuestStatus.gsgu);
          if(gsUpdated != null)
          {
            gsUpdated.gtQuantity = Convert.ToByte(txtQtyGuestStatus.Text);
            gsUpdated.gtgs = cmbGuestStatus.SelectedValue.ToString();

            _lstNewGuestStatus.Add(ConvertGuestStatusToGuestStatusInvitationObject(gsUpdated, false, _previousGuestStatus.gsID));
            _lstGuestStatus.Remove(gsUpdated);
          }
        }

        if (_wasUpdated)
        {
          tbiNewGuestStatus.Header = "New Guest Status";
          tbiNewGuestStatus.Content = "Add";
        }

        LoadGuestStatusGrid();

        tbiGuestStatusList.IsSelected = true;

        txtQtyGuestStatus.Text = String.Empty;
        cmbGuestStatus.SelectedIndex = -1;
        _wasUpdated = false;
      }
    }

    private void dtgGuestStatus_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if ((GuestStatusInvitation)dtgGuestStatus.CurrentItem == null) return;

      _previousGuestStatus = (GuestStatusInvitation)dtgGuestStatus.CurrentItem;

      tbiNewGuestStatus.Header = "Update Guest Status";
      tbiNewGuestStatus.IsSelected = true;

      txtQtyGuestStatus.Text = _previousGuestStatus.gsQty.ToString();
      cmbGuestStatus.SelectedValue = _previousGuestStatus.gsID;
      _wasUpdated = true;
      btnAddGuestStatus.Content = "Update";
    }
    #endregion

    #region Métodos de los controles de la sección CREDIT CARD
    /// <summary>
    /// agrega unanueva tarjeta de Credito
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddCreditCard_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateNewCreditCard())
      {
        if (!_wasUpdated)
        {
          var cc = new IM.Model.GuestCreditCard();
          cc.gdQuantity = Convert.ToByte(txtQtyCreditCard.Text);
          cc.gdgu = _guestID;
          cc.gdcc = cmbCreditCards.SelectedValue.ToString();

          _lstNewCreditsCard.Add(ConvertGuestCreditCardToGuestCreditCardInvitationObject(cc, true));
        }
        else
        {
          //buscamos si el estatus que editó es de los que acaba de ingresar para actualizarlo
          var ccNew = _lstNewCreditsCard.SingleOrDefault(c => c.ccID == _previousCreditCard.ccID && c.ccgu == _previousCreditCard.ccgu);
          if (ccNew != null)
          {
            ccNew.ccID = cmbCreditCards.SelectedValue.ToString();
            ccNew.ccQty = Convert.ToInt32(txtQtyCreditCard.Text);
          }

          //buscamos si el estatus que editó es de que extraímos de la base de datos
          var ccUpdated = _lstCreditsCard.SingleOrDefault(c => c.gdcc == _previousCreditCard.ccID && c.gdgu == _previousCreditCard.ccgu);
          if (ccUpdated != null)
          {
            ccUpdated.gdcc = cmbCreditCards.SelectedValue.ToString();
            ccUpdated.gdQuantity = Convert.ToByte(txtQtyCreditCard.Text);

            _lstNewCreditsCard.Add(ConvertGuestCreditCardToGuestCreditCardInvitationObject(ccUpdated, false, _previousCreditCard.ccID));
            _lstCreditsCard.Remove(ccUpdated);
          }
        }

        if (_wasUpdated)
        {
          tbiNewCreditCard.Header = "New Guest Status";
          btnAddCreditCard.Content = "Add";
        }

        LoadCreditCardGrid();

        tbiCreditCardList.IsSelected = true;
        txtQtyCreditCard.Text = String.Empty;
        cmbCreditCards.SelectedIndex = -1;
        _wasUpdated = false;
      }
    }

    /// <summary>
    /// Edita una tarjeta de crédito
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dtgCCCompany_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if ((GuestCreditCardInvitation)dtgCCCompany.CurrentItem == null) return;

      _previousCreditCard = (GuestCreditCardInvitation)dtgCCCompany.CurrentItem;

      tbiNewCreditCard.Header = "Update Credit Card";
      tbiNewCreditCard.IsSelected = true;

      txtQtyCreditCard.Text = _previousCreditCard.ccQty.ToString();
      cmbCreditCards.SelectedValue = _previousCreditCard.ccID;
      _wasUpdated = true;
      btnAddCreditCard.Content = "Update";
    }
    #endregion

    #region Métodos de los controles de la sección ADDITIONAL INFORMATION
    /// <summary>
    /// Agrega un huésped adicional
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddAdditional_Click(object sender, RoutedEventArgs e)
    {
      if (_guestAdditional != null)
      {
        if(!_wasUpdated)
        {
          _lstNewAdditionals.Add(ConvertGuestToGuestAdditionalInvitationObject(_guestAdditional, true));
        }
        else
        {
          var newGA = _lstNewAdditionals.SingleOrDefault(g=> g.guID == _previousGuestAdditional.guID);
          if(newGA != null)
          {
            newGA.guID = Convert.ToInt32(txtGuidAdditional.Text);
            newGA.guLastName1 = txtLastNameAdditional.Text;
            newGA.guFirstName1 = txtFirstNameAdditional.Text;
          }

          var updatedGA = _lstAdditionals.SingleOrDefault(g => g.guID == _previousGuestAdditional.guID);
          if (updatedGA != null)
          {
            _lstAdditionals.Remove(updatedGA);

            var idPrevious = _previousGuestAdditional.guID;
            updatedGA.guID = Convert.ToInt32(txtGuidAdditional.Text);
            updatedGA.guLastName1 = txtLastNameAdditional.Text;
            updatedGA.guFirstName1 = txtFirstNameAdditional.Text;

            _lstNewAdditionals.Add(ConvertGuestToGuestAdditionalInvitationObject(updatedGA, false, idPrevious));

          }
        }

        if (_wasUpdated)
        {
          tbiNewAdditional.Header = "New Additional";
          btnAddCreditCard.Content = "Add";
        }

        LoadAdditionalInformartionGrid();

        tbiAdditionalList.IsSelected = true;
        txtGuidAdditional.Text = String.Empty;
        txtLastNameAdditional.Text = String.Empty;
        txtFirstNameAdditional.Text = String.Empty;

        _guestAdditional = null;
        _wasUpdated = false;
      }
    }

    /// <summary>
    /// Busca un huésped en la base de datos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtGuidAdditional_LostFocus(object sender, RoutedEventArgs e)
    {
      int id;
      if (!String.IsNullOrEmpty(txtGuidAdditional.Text) && int.TryParse(txtGuidAdditional.Text, out id) && id > 0)
      {
        var guestAdd = IM.BusinessRules.BR.BRGuests.GetGuestById(id);
        if (guestAdd == null)
        {
          Helpers.UIHelper.ShowMessage("Guest does not exist");
          txtGuidAdditional.Text = String.Empty;
          return;
        }
        txtLastNameAdditional.Text = guestAdd.guLastName1;
        txtFirstNameAdditional.Text = guestAdd.guFirstName1;
        _guestAdditional = guestAdd;
      }

    }

    /// <summary>
    /// Edita un invitado adicional
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void grbAdditionalInfo_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if ((IM.Model.Guest)dtgAdditInform.CurrentItem == null) return;

      _previousGuestAdditional = (IM.Model.Guest)dtgAdditInform.CurrentItem;

      tbiNewAdditional.Header = "Update Additional";
      tbiNewAdditional.IsSelected = true;

      txtGuidAdditional.Text = _previousGuestAdditional.guID.ToString();
      txtLastNameAdditional.Text = _previousGuestAdditional.guLastName1;
      txtFirstNameAdditional.Text = _previousGuestAdditional.guFirstName1;

      btnAddAdditional.Content = "Update";
      _wasUpdated = true;
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

    #region Métodos privados

    #region Métodos de permisos
    /// <summary>
    /// Validamos que se permita editar
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateEdit()
    {
      var login = new IM.Base.Forms.frmLogin(null, false, EnumLoginType.Normal, false);
      login.ShowDialog();
      if (!login.IsAuthenticated)
      {
        Helpers.UIHelper.ShowMessage("Error of Authentication");
        this.Close();
        return false;
      }
      // validamos que tenga permiso estandar de invitaciones
      bool permission = false;
      if (_invitationType == InvitationType.Host)
        permission = _user.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.Standard);
      else
        permission = _user.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.Standard);

      if (!permission)
      {
        Helpers.UIHelper.ShowMessage("You do not have sufficient permissions to modify the invitation");
        this.Close();
        return false;
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
        case BusinessRules.Enums.InvitationType.InHouse:
          InHouseControlsConfig();
          break;
        case BusinessRules.Enums.InvitationType.OutHouse:
          OutHouseControlsConfig();
          break;
        case BusinessRules.Enums.InvitationType.Host:
          HostControlsConfig();
          break;
        case BusinessRules.Enums.InvitationType.Animation:
          AnimationControlsConfig();
          break;
        case BusinessRules.Enums.InvitationType.Regen:
          RegenControlsConfig();
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
      colElectronicPurseCreditCard.Width = new GridLength(0);
      rowRoomQuantity.Height = new GridLength(0);
      tbiAdditionalCreditCard.Header = "Additional Information";
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
      colElectronicPurseCreditCard.Width = new GridLength(0);

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
      colElectronicPurseCreditCard.Width = new GridLength(0);
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
        if (_invitationType == InvitationType.OutHouse && _invitationMode == EnumInvitationMode.modAdd)
        {
          txtOutInvitation.IsEnabled = true;
        }
        else if (_invitationMode == EnumInvitationMode.modEdit && _invitationType == InvitationType.InHouse) //si es una invitacion inhouse
        {
          // si no tiene un folio de reservacion definido, permitimos definirlo
          if (String.IsNullOrEmpty(txtReservationNumber.Text))
          {
            btnSearch.IsEnabled = true;
          }

        }
        //si es una invitacion outhouse, permitimos definir el folio de la invitacion outhouse
        else if (_invitationMode == EnumInvitationMode.modEdit && _invitationType == InvitationType.InHouse)
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

      if (_invitationType == InvitationType.OutHouse)
      {
        txtPRContract.IsEnabled = enable;
        cmbPRContract.IsEnabled = enable;
      }

      txtPR.IsEnabled = enable;
      cmbPR.IsEnabled = enable;

      if (_invitationType == InvitationType.Host)
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

      if (_invitationType != InvitationType.Regen && _invitationType != InvitationType.Animation)
      {
        chkBeforeInOut.IsEnabled = enable;
      }

      if (_invitationType != InvitationType.OutHouse)
      {
        txtRescheduleDate.IsEnabled = enable;
        cmbRescheduleTime.IsEnabled = enable;
        chkResch.IsEnabled = enable;
      }

      if (_invitationType == InvitationType.OutHouse)
      {
        txtFlightNumber.IsEnabled = enable;
      }

      btnChange.IsEnabled = enable;
      btnRebook.IsEnabled = enable;
      btnReschedule.IsEnabled = enable;

      if (enable) //si se esta modificando o agregando
      {
        if (_invitationType != InvitationType.Host) // si no es el modulo host
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
        if (_invitationType != InvitationType.OutHouse)
        {
          var dtInvit = Convert.ToDateTime(txtDate.Text);
          //si la fecha de salida es hoy o despues y (es una invitacion nueva o la fecha de invitacion es hoy o
          //(tiene permiso especial de invitaciones y la fecha de booking original Mayor o igual a hoy))
          if (txtDeparture.SelectedDate.HasValue && (txtDeparture.SelectedDate.Value >= _serverDateTime)
              && ((_isNewInvitation || dtInvit == _serverDateTime)
                  || (_user.HasPermission(_invitationType == InvitationType.Host ? EnumPermission.Host : EnumPermission.PRInvitations, EnumPermisionLevel.Special)
                  && _bookinDateOriginal >= _serverDateTime)
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
        if (_invitationType != InvitationType.OutHouse && _invitationType != InvitationType.Host)
        {
          //si la fecha de booking original es hoy o despues o es una invitacion nueva
          if ((_bookinDateOriginal >= _serverDateTime) || _isNewInvitation)
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
        if (_invitationType != InvitationType.OutHouse && _invitationType != InvitationType.Host)
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

        if (_invitationType != InvitationType.OutHouse)//si no es el modulo outside
        {
          //solo se permite modificar la fecha de salida si tiene permiso especial de invitaciones
          if (!_user.HasPermission(_invitationType == InvitationType.Host ? EnumPermission.Host : EnumPermission.PRInvitations, EnumPermisionLevel.Special))
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
        if (_invitationType != InvitationType.OutHouse && _invitationType != InvitationType.Host)//si no es el modulo outside ni el de Host
        {

          //si la fecha de booking original es hoy o despues o es una invitacion nueva
          if ((_bookinDateOriginal.HasValue && _bookinDateOriginal.Value >= _serverDateTime) || _isNewInvitation)
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
      if (_invitationType == InvitationType.InHouse || _invitationType == InvitationType.Host)
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
      if (_invitationType == InvitationType.InHouse)
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
      if ((txtDeparture.SelectedDate.HasValue && txtDeparture.SelectedDate.Value >= _serverDateTime)
        || _user.HasPermission(_invitationType == InvitationType.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations, EnumPermisionLevel.SuperSpecial))
      {
        if (_invitationMode == EnumInvitationMode.modAdd)//si es una invitacion nueva
        {
          //no se permite cambiar, reschedule, ni rebook
          EnableButtonsChangeRescheduleRebook(false, false, false);
        }
        else if (chkShow.IsChecked.HasValue && chkShow.IsChecked.Value) // si tiene show
        {
          //no se puede modificar Antes In & Out
          if (_invitationType != InvitationType.Animation || _invitationType != InvitationType.Regen)
          {
            chkBeforeInOut.IsChecked = false;
          }

          //solo se permite rebook
          EnableButtonsChangeRescheduleRebook(false, false, true);
        }
        else if (Convert.ToDateTime(txtDate.Text) == _serverDateTime && (chkResch.IsChecked.HasValue && !chkResch.IsChecked.Value)) //si la fecha de invitacion es hoy y no es un reschedule
        {
          // no se permite reschedule
          EnableButtonsChangeRescheduleRebook(true, false, true);
        }
        else if (Convert.ToDateTime(txtDate.Text) == _serverDateTime && (chkResch.IsChecked.HasValue && chkResch.IsChecked.Value)) // si la fecha de invitacion es hoy y es un reschedule
        {
          // no se permite cambiar
          EnableButtonsChangeRescheduleRebook(false, true, true);
        }
        // si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es despues de hoy y tiene permiso estandar de invitaciones
        else if ((Convert.ToDateTime(txtDate.Text) < _serverDateTime)
                  && (chkResch.IsChecked.HasValue && !chkResch.IsChecked.Value)
                  && (txtBookingDate.SelectedDate.HasValue && (txtBookingDate.SelectedDate.Value > _serverDateTime))
                  && _user.HasPermission(_invitationType == InvitationType.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations, EnumPermisionLevel.Standard)
                )
        {
          // no se permite reschedule
          EnableButtonsChangeRescheduleRebook(true, false, true);
        }
        // si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es hoy
        else if ((Convert.ToDateTime(txtDate.Text) < _serverDateTime)
                  && (chkResch.IsChecked.HasValue && !chkResch.IsChecked.Value)
                  && (txtBookingDate.SelectedDate.HasValue && (txtBookingDate.SelectedDate.Value == _serverDateTime))
                )
        {
          // se permite cambiar, reschedule y rebook
          EnableButtonsChangeRescheduleRebook(true, true, true);
        }
        //' si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es antes de hoy
        else if ((Convert.ToDateTime(txtDate.Text) < _serverDateTime)
                  && (chkResch.IsChecked.HasValue && !chkResch.IsChecked.Value)
                  && (txtBookingDate.SelectedDate.HasValue && (txtBookingDate.SelectedDate.Value < _serverDateTime))
                )
        {
          //No se permite cambiar
          EnableButtonsChangeRescheduleRebook(false, true, true);
        }
        //si la fecha de invitacion es antes de hoy y no es un reschedule
        else if ((Convert.ToDateTime(txtDate.Text) < _serverDateTime)
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
      if (_invitationType != InvitationType.OutHouse)
      {
        btnReschedule.IsEnabled = enabledChange;
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
      if (_invitationType != InvitationType.Host)//si no es modulo Host
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
      if (_invitationType != InvitationType.Host)
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

    /// <summary>
    /// Convierte un objeto del tipo IM.Model.InvitationGift a un objeto IM.BussinesRules.Classes.GiftInvitation
    /// </summary>
    /// <param name="gift">Objeto InvitationGift</param>
    /// <param name="isNew">Indica si es un regalo nuevo</param>
    /// <param name="previousID">Asigna el ID anterior</param>
    /// <returns>GiftInvitation</returns>
    private GiftInvitation ConvertInvitationGiftToGiftInvitationObject(IM.Model.InvitationGift gift, bool isNew, string previousID = null)
    {
      var g = new GiftInvitation();
      g.igAdults = gift.igAdults;
      g.igct = gift.igct;
      g.igExtraAdults = gift.igExtraAdults;
      g.iggi = gift.iggi;
      g.iggu = gift.iggu;
      g.igMinors = gift.igMinors;
      g.igPriceA = gift.igPriceA;
      g.igPriceAdult = gift.igPriceAdult;
      g.igPriceExtraAdult = gift.igPriceExtraAdult;
      g.igPriceM = gift.igPriceM;
      g.igPriceMinor = gift.igPriceMinor;
      g.igQty = gift.igQty;
      g.isNew = isNew;
      g.isUpdate = !isNew;
      g.iggiPrevious = !isNew ? previousID : null;

      return g;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lst"></param>
    /// <returns></returns>
    private List<IM.Model.InvitationGift> ConvertGiftInvitationToInvitationGiftList(List<GiftInvitation> lst)
    {
      return (from i in lst
              select new IM.Model.InvitationGift
              {
                igAdults = i.igAdults,
                igct = i.igct,
                igExtraAdults = i.igExtraAdults,
                iggi = i.iggi,
                iggu = i.iggu,
                igPriceA = i.igPriceA,
                igMinors = i.igMinors,
                igPriceAdult = i.igPriceAdult,
                igPriceExtraAdult = i.igPriceExtraAdult,
                igPriceM = i.igPriceM,
                igPriceMinor = i.igPriceMinor,
                igQty = i.igQty
              }).ToList();
    }

    /// Convierte un objeto del tipo IM.Model.BookingDeposit a un objeto IM.BussinesRules.Classes.BookingDepositInvitation
    /// </summary>
    /// <param name="deposit">Objeto BookingDeposit</param>
    /// <param name="isNew">Indica si es un regalo nuevo</param>
    /// <param name="previousID">Asigna el ID anterior</param>
    /// <returns>BookingDepositInvitation</returns>
    private BookingDepositInvitation ConvertBookingDepositToBookingDepositInvitationObject(IM.Model.BookingDeposit deposit, bool isNew)
    {
      var d = new BookingDepositInvitation();
      d.bdAmount = deposit.bdAmount;
      d.bdAuth = deposit.bdAuth;
      d.bdCardNum = deposit.bdCardNum;
      d.bdcc = deposit.bdcc;
      d.bdEntryDCXC = deposit.bdEntryDCXC;
      d.bdExpD = deposit.bdExpD;
      d.bdFolioCXC = deposit.bdFolioCXC;
      d.bdgu = deposit.bdgu;
      d.bdID = deposit.bdID;
      d.bdpc = deposit.bdpc;
      d.bdpt = deposit.bdpt;
      d.bdReceived = deposit.bdReceived;
      d.bdUserCXC = deposit.bdUserCXC;
      d.bdcu = deposit.bdcu;
      d.isNew = isNew;
      d.isUpdated = !isNew;
      return d;
    }

    /// <summary>
    /// Convierte una lista del tipo  IM.BussinesRules.Classes.BookingDepositInvitation a una lista IM.Model.BookingDeposit 
    /// </summary>
    /// <param name="lst"></param>
    /// <returns></returns>
    private List<IM.Model.BookingDeposit> ConvertBookingDepositInvitationToBookingDepositList(List<BookingDepositInvitation> lst)
    {
      return (from deposit in lst
              select new IM.Model.BookingDeposit
              {
                bdAmount = deposit.bdAmount,
                bdAuth = deposit.bdAuth,
                bdCardNum = deposit.bdCardNum,
                bdcc = deposit.bdcc,
                bdEntryDCXC = deposit.bdEntryDCXC,
                bdExpD = deposit.bdExpD,
                bdFolioCXC = deposit.bdFolioCXC,
                bdgu = deposit.bdgu,
                bdID = deposit.bdID,
                bdpc = deposit.bdpc,
                bdpt = deposit.bdpt,
                bdReceived = deposit.bdReceived,
                bdUserCXC = deposit.bdUserCXC,
                bdcu = deposit.bdcu
              }).ToList();
    }

    /// Convierte un objeto del tipo IM.Model.GuestStatus a un objeto IM.BussinesRules.Classes.GuestStatusInvitation
    /// </summary>
    /// <param name="deposit">Objeto BookingDeposit</param>
    /// <param name="isNew">Indica si es un regalo nuevo</param>
    /// <param name="previousID">Asigna el ID anterior</param>
    /// <returns>BookingDepositInvitation</returns>
    private GuestStatusInvitation ConvertGuestStatusToGuestStatusInvitationObject(IM.Model.GuestStatus guestStatus, bool isNew, string previousID = null)
    {
      var gs = new GuestStatusInvitation();
      gs.gsgu = guestStatus.gtgu;
      gs.gsID = guestStatus.gtgs;
      gs.gsQty = guestStatus.gtQuantity;
      gs.isNew = isNew;
      gs.isUpdate = !isNew;
      gs.gsIDPrevious = isNew ? null : previousID;
      return gs;
    }

    /// <summary>
    /// Convierte una lista del tipo  IM.BussinesRules.Classes.GuestStatusInvitation a una lista IM.Model.GuestStatus 
    /// </summary>
    /// <param name="lst"></param>
    /// <returns></returns>
    private List<IM.Model.GuestStatus> ConvertGuestStatusInvitationToGuestStatusList(List<GuestStatusInvitation> lst)
    {
      return (from gs in lst
              select new IM.Model.GuestStatus
              {
                gtgs = gs.gsID,
                gtQuantity = Convert.ToByte(gs.gsQty),
                gtgu = gs.gsgu
              }).ToList();
    }

    /// Convierte un objeto del tipo IM.Model.GuestCreditCard a un objeto IM.BussinesRules.Classes.GuestCreditCardInvitation
    /// </summary>
    /// <param name="deposit">Objeto GuestCreditCard</param>
    /// <param name="isNew">Indica si es un regalo nuevo</param>
    /// <param name="previousID">Asigna el ID anterior</param>
    /// <returns>BookingDepositInvitation</returns>
    private GuestCreditCardInvitation ConvertGuestCreditCardToGuestCreditCardInvitationObject(IM.Model.GuestCreditCard creditCard, bool isNew, string previousID = null)
    {
      var cc = new GuestCreditCardInvitation();
      cc.ccgu = creditCard.gdgu;
      cc.ccID = creditCard.gdcc;
      cc.ccQty = Convert.ToInt32(creditCard.gdQuantity);
      cc.isNew = isNew;
      cc.isUpdate = !isNew;
      cc.ccIDPrevious = !isNew ? previousID : null;
      return cc;
    }

    /// <summary>
    /// Convierte una lista del tipo  IM.BussinesRules.Classes.GuestCreditCardInvitation a una lista IM.Model.GuestCreditCard 
    /// </summary>
    /// <param name="lst"></param>
    /// <returns>GuestCreditCard</returns>
    private List<IM.Model.GuestCreditCard> ConvertGuestStatusInvitationToGuestCreditCardList(List<GuestCreditCardInvitation> lst)
    {
      return (from cc in lst
              select new IM.Model.GuestCreditCard
              {
                gdcc = cc.ccID,
                gdgu = cc.ccgu,
                gdQuantity = Convert.ToByte(cc.ccQty)
              }).ToList();
    }

    /// Convierte un objeto del tipo IM.Model.Guest a un objeto IM.BussinesRules.Classes.GuestAdditionalInvitation
    /// </summary>
    /// <param name="guest">Objeto Guest</param>
    /// <param name="isNew">Indica si es un regalo nuevo</param>
    /// <param name="previousID">Asigna el ID anterior</param>
    /// <returns>BookingDepositInvitation</returns>
    private GuestAdditionalInvitation ConvertGuestToGuestAdditionalInvitationObject(IM.Model.Guest guest, bool isNew, int? previousID = null)
    {
      var g = new GuestAdditionalInvitation();
      g.guID = guest.guID;
      g.guLastName1 = guest.guLastName1;
      g.guFirstName1 = guest.guFirstName1;
      g.isNew = isNew;
      g.isUpdate = !isNew;
      g.guIDPrevious = previousID.HasValue ? previousID.Value : (int?) null;
      return g;
    }

    /// <summary>
    /// Convierte una lista del tipo  IM.BussinesRules.Classes.GuestAdditionalInvitation a una lista IM.Model.GuestCreditCard 
    /// </summary>
    /// <param name="lst"></param>
    /// <returns>GuestCreditCard</returns>
    private List<IM.Model.Guest> ConvertGuestAdditionalInvitationToGuestList(List<GuestAdditionalInvitation> lst)
    {
      return (from g in lst
              select new IM.Model.Guest
              {
                guID = g.guID,
                guLastName1 = g.guLastName1,
                guFirstName1 = g.guFirstName1
              }).ToList();
    }

    #endregion

    #region Métodos para cargar Controles e información del invitado

    /// <summary>
    /// Manda llamar los métodos para cargar la información del guest.
    /// </summary>
    private void LoadControls()
    {
      LoadCommonControls(); //Se cargan los controles que son comunes en todos los tipos de invitacion.

      //Aqui cargamos los controles que son específicos por tipo de invitación
      switch (_invitationType)
      {
        case BusinessRules.Enums.InvitationType.InHouse:
          LoadInHouseControls();
          break;
        case BusinessRules.Enums.InvitationType.OutHouse:
          LoadOutHouseControls();
          break;
        case BusinessRules.Enums.InvitationType.Host:
          LoadHostControls();
          break;
        case BusinessRules.Enums.InvitationType.Animation:
          LoadAnimationControls();
          break;
        case BusinessRules.Enums.InvitationType.Regen:
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
    private void LoadCommonControls()
    {
      #region User
      lblUser.Content = _user.User.peN;
      #endregion

      #region ComboBoxes
      var languages = IM.BusinessRules.BR.BRLanguages.GetLanguages(1);
      LoadComboBox(languages, cmbLanguage, "la", "ES");

      var personnels = IM.BusinessRules.BR.BRPersonnel.GetPersonnel(_user.LeadSource.lsID);
      LoadComboBox(personnels, cmbPR, "pe");

      var hotels = IM.BusinessRules.BR.BRHotels.GetHotels(1);
      LoadComboBox(hotels, cmbHotel, "hoID", "hoID", _user.Location.loN);
      LoadComboBox(hotels, cmbResort, "hoID", "hoID", String.Empty);

      var agencies = IM.BusinessRules.BR.BRAgencies.GetAgencies(1);
      LoadComboBox(agencies, cmbAgency, "ag");

      var countries = IM.BusinessRules.BR.BRCountries.GetCountries(1);
      LoadComboBox(countries, cmbCountry, "co");

      var currencies = IM.BusinessRules.BR.BRCurrencies.GetCurrencies(null, 1);
      LoadComboBox(currencies, cmbCurrency, "cu", "US");
      LoadComboBox(currencies, cmbCurrencyDeposit, "cu");

      var paymentTypes = IM.BusinessRules.BR.BRPaymentTypes.GetPaymentTypes(1);
      LoadComboBox(paymentTypes, cmbPaymentType, "pt", "CS");
      LoadComboBox(paymentTypes, cmbPaymentTypeDeposit, "pt");
      
      var maritalStatus = IM.BusinessRules.BR.BRMaritalStatus.GetMaritalStatus();
      LoadComboBox(maritalStatus, cmbMaritalStatusGuest1, "ms");
      LoadComboBox(maritalStatus, cmbMaritalStatusGuest2, "ms");

      var creditCards = IM.BusinessRules.BR.BRCreditCardTypes.GetCreditCardTypes(null, -1);
      LoadComboBox(creditCards, cmbCreditCardDeposit, "cc");
      LoadComboBox(creditCards, cmbCreditCards, "cc");

      var guestStatus = IM.BusinessRules.BR.BRGuests.GetGuestStatusType(_guestID);
      LoadComboBox(guestStatus, cmbGuestStatus, "gs");

      var gifts = IM.BusinessRules.BR.BRGifts.GetGifts();
      LoadComboBox(gifts, cmbGifts, "gi");
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

      if (_invitationType != InvitationType.OutHouse)
      {
        txtReservationNumber.Text = guest.guHReservID;
        txtRebookRef.Text = guest.guRef.ToString();
      }
      _hReservIDC = guest.guHReservIDC;
      txtDate.Text = guest.guInvitD.HasValue ? guest.guInvitD.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
      txtTime.Text = guest.guInvitT.HasValue ? guest.guInvitT.Value.ToString("hh:mm") : DateTime.Now.ToString("hh:mm");

      if (_invitationType == InvitationType.OutHouse || _invitationType == InvitationType.Host)
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
      if (_invitationType == InvitationType.OutHouse)
      {
        cmbPRContract.SelectedValue = String.IsNullOrEmpty(guest.guPRInfo) ? String.Empty : guest.guPRInfo;
        txtPRContract.Text = String.IsNullOrEmpty(guest.guPRInfo) ? String.Empty : guest.guPRInfo;
      }

      cmbPR.SelectedValue = String.IsNullOrEmpty(guest.guPRInvit1) ? String.Empty : guest.guPRInvit1;
      txtPR.Text = String.IsNullOrEmpty(guest.guPRInvit1) ? String.Empty : guest.guPRInvit1;

      if (_invitationType != InvitationType.Host)
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


      if (_invitationType != InvitationType.OutHouse)
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


      if (_invitationType != InvitationType.Animation && _invitationType != InvitationType.Regen)
        chkBeforeInOut.IsChecked = guest.guAntesIO;

      chkDirect.IsChecked = guest.guDirect;

      if (_invitationType != InvitationType.Host)
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
      if (_invitationType != InvitationType.OutHouse)
        txtRoomQuantity.Text = guest.guRoomsQty.ToString();
      #endregion

      #region Credit Cards *************FALTA LLENAR GRID
      if (_invitationType == InvitationType.InHouse || _invitationType == InvitationType.Host)
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
    private void LoadInHouseControls()
    {
      var salesRooms = IM.BusinessRules.BR.BRSalesRooms.GetSalesRooms(0);
      LoadComboBox(salesRooms, cmbSalesRoom, "sr");
    }

    /// <summary>
    /// Carga los controles del formulatio Invitation Out House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private void LoadOutHouseControls()
    {
      var personnels = IM.BusinessRules.BR.BRPersonnel.GetPersonnel(_user.LeadSource.lsID);
      LoadComboBox(personnels, cmbPRContract, "pe");

      var salesRooms = IM.BusinessRules.BR.BRSalesRooms.GetSalesRooms(0);
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

    #region Métodos para cargar ComboBoxes y DataGridComboBoxColumn

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
    /// Carga los DataGridComboBoxColumn de los grids
    /// </summary>
    /// <param name="items">Lista de elementos que contendrá el combo</param>
    /// <param name="combo">Combo que se llenará con los elementos</param>
    /// <param name="prefix">prefijo</param>
    /// <param name="defaultValue">Valor que será seleccionado por default</param>
    private void LoadDataGridComboBoxColumn(IEnumerable<object> items, DataGridComboBoxColumn combo, string prefix, string defaultValue = "")
    {
      combo.DisplayMemberPath = String.Format("{0}N", prefix);
      combo.SelectedValuePath = String.Format("{0}ID", prefix);
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
    private void LoadTimeComboBoxes(IEnumerable<IM.Model.TourTimeAvailable> items, ComboBox combo, string displayItem, string valueItem, string defaultValue = "")
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
    private void LoadGiftGrid()
    {
      
      if(!dtgGifts.HasItems && !_lstGifts.Any())//sino tiene registros cargamos por primera vez el grid
      {
        gifts = IM.BusinessRules.BR.BRGifts.GetGiftsInvitation(_guestID);

        //convetidmos la lista de GiftInvitation a InvitationGift

        _lstGifts = (from g in gifts
                     select new IM.Model.InvitationGift
                     {
                       iggu = g.iggu,
                       igQty = g.igQty,
                       iggi = g.iggi,
                       igAdults = g.igAdults,
                       igMinors = g.igMinors,
                       igExtraAdults = g.igExtraAdults,
                       igPriceA = g.igPriceA,
                       igPriceAdult = g.igPriceAdult,
                       igPriceExtraAdult = g.igPriceExtraAdult,
                       igPriceM = g.igPriceM,
                       igPriceMinor = g.igPriceMinor,
                       igct = g.igct
                     }).ToList();
      }
      else //si tiene registros y se le añaden depositos se convierte la lista de InvitationGift a GiftInvitation para recargar el grid
      {
        gifts.Clear();
        //unimos la lista de gift que se encuentran en la base de datos con los nuevos y actualizados
        gifts = (from g in _lstGifts
                 select new GiftInvitation
                  {
                    iggu = g.iggu,
                    igQty = g.igQty,
                    iggi = g.iggi,
                    igAdults = g.igAdults,
                    igMinors = g.igMinors,
                    igExtraAdults = g.igExtraAdults,
                    Gift = IM.BusinessRules.BR.BRGifts.GetGiftId(g.iggi).giN,
                    igPriceA = g.igPriceA,
                    igPriceAdult = g.igPriceAdult,
                    igPriceExtraAdult = g.igPriceExtraAdult,
                    igPriceM = g.igPriceM,
                    igPriceMinor = g.igPriceMinor,
                    igct = g.igct
                    
                 }).ToList().Union(
                  from g in _lstNewGifts
                  select new GiftInvitation
                  {
                    iggu = g.iggu,
                    igQty = g.igQty,
                    iggi = g.iggi,
                    igAdults = g.igAdults,
                    igMinors = g.igMinors,
                    igExtraAdults = g.igExtraAdults,
                    Gift = IM.BusinessRules.BR.BRGifts.GetGiftId(g.iggi).giN,
                    igPriceA = g.igPriceA,
                    igPriceAdult = g.igPriceAdult,
                    igPriceExtraAdult = g.igPriceExtraAdult,
                    igPriceM = g.igPriceM,
                    igPriceMinor = g.igPriceMinor,
                    igct = g.igct
                  }).ToList();
      }

      dtgGifts.ItemsSource = gifts;

      CalculateCostsPrices();
      CalculateTotalGifts();
      
    }

    /// <summary>
    /// Carga la información del Grid de estado de los invitados
    /// </summary>
    private void LoadGuestStatusGrid()
    {
      var guestStatus = new List<IM.BusinessRules.Classes.GuestStatusInvitation>();

      if (!dtgGuestStatus.HasItems && !_lstNewGuestStatus.Any())//sino tiene registros cargamos por primera vez el grid
      {
        guestStatus = IM.BusinessRules.BR.BRGuests.GetGuestStatusTypeInvit(_guestID);
        //convetidmos la lista de GuestStatusInvitation a GuestStatus
        _lstGuestStatus = (from gs in guestStatus
                           select new IM.Model.GuestStatus
                           {
                             gtgs = gs.gsID,
                             gtgu = gs.gsgu,
                             gtQuantity = Convert.ToByte(gs.gsQty)
                           }).ToList();
      }
      else
      {
        //convetidmos la lista de GuestStatus a GuestStatusInvitation
        guestStatus = (from gs in _lstGuestStatus
                    select new IM.BusinessRules.Classes.GuestStatusInvitation
                    {
                      gsID = gs.gtgs,
                      gsgu = gs.gtgu,
                      gsQty = Convert.ToInt32(gs.gtQuantity),
                      GuestStatus = IM.BusinessRules.BR.BRGuests.GetGuestStatusTypeId(gs.gtgs).gsN
                    }).ToList().Union(
                    from gs in _lstNewGuestStatus
                    select new GuestStatusInvitation
                    {
                      gsID = gs.gsID,
                      gsgu = gs.gsgu,
                      gsQty = Convert.ToInt32(gs.gsQty),
                      GuestStatus = IM.BusinessRules.BR.BRGuests.GetGuestStatusTypeId(gs.gsID).gsN
                    }).ToList();
      }
      dtgGuestStatus.ItemsSource = guestStatus;

      txtGuestStatus.Text = guestStatus!= null && guestStatus.Any() ? guestStatus.Single().gsID: String.Empty;

      CalculateMaxAuthGifts();
    }

    /// <summary>
    /// Carga la información del Grid de depósitos de los invitados
    /// </summary>
    private void LoadDepositGrid()
    {
      var deposits = new List<BusinessRules.Classes.BookingDepositInvitation>();

      if(!dtgDeposits.HasItems)//sino tiene registros cargamos por primera vez el grid
      {
        deposits = IM.BusinessRules.BR.BRBookingDeposits.GetBookingDepositsByInvitation(_guestID);

        //convetidmos la lista de BookingDepositInvitation a BookindDeposit
        _lstDeposits = (from d in deposits
                        select new IM.Model.BookingDeposit
                        {
                          bdAmount = d.bdAmount,
                          bdAuth = d.bdAuth,
                          bdCardNum = d.bdCardNum,
                          bdcc = d.bdcc,
                          bdcu = d.bdcu,
                          bdEntryDCXC = d.bdEntryDCXC,
                          bdExpD = d.bdExpD,
                          bdFolioCXC = d.bdFolioCXC,
                          bdgu = d.bdgu,
                          bdID = d.bdID,
                          bdpc = d.bdpc,
                          bdpt = d.bdpt,
                          bdReceived = d.bdReceived,
                          bdUserCXC = d.bdUserCXC,
                        }).ToList();
      }
      else // si tiene registros y se le añaden depositos se convierte la lista de BookingDeposit a BookindDepositInvitation para recargar el grid
      {
        deposits = (from d in _lstDeposits
                        select new BookingDepositInvitation
                        {
                          bdAmount = d.bdAmount,
                          bdAuth = d.bdAuth,
                          bdCardNum = d.bdCardNum,
                          bdcc = d.bdcc,
                          bdcu = d.bdcu,
                          bdEntryDCXC = d.bdEntryDCXC,
                          bdExpD = d.bdExpD,
                          bdFolioCXC = d.bdFolioCXC,
                          bdgu = d.bdgu,
                          bdID = d.bdID,
                          bdpc = d.bdpc,
                          bdpt = d.bdpt,
                          bdReceived = d.bdReceived,
                          bdUserCXC = d.bdUserCXC,
                          Currency = IM.BusinessRules.BR.BRCurrencies.GetCurrencyId(d.bdcu).cuN,
                          PaymentType = IM.BusinessRules.BR.BRPaymentTypes.GetPaymentTypeId(d.bdpt).ptN,
                          CreditCard = String.IsNullOrEmpty(d.bdcc) ? String.Empty : IM.BusinessRules.BR.BRCreditCardTypes.GetCreditCardTypeId(d.bdcc).ccN
                        }).ToList().Union(
                        from d in _lstNewDeposits
                        select new BookingDepositInvitation
                        {
                          bdAmount = d.bdAmount,
                          bdAuth = d.bdAuth,
                          bdCardNum = d.bdCardNum,
                          bdcc = d.bdcc,
                          bdcu = d.bdcu,
                          bdEntryDCXC = d.bdEntryDCXC,
                          bdExpD = d.bdExpD,
                          bdFolioCXC = d.bdFolioCXC,
                          bdgu = d.bdgu,
                          bdID = d.bdID,
                          bdpc = d.bdpc,
                          bdpt = d.bdpt,
                          bdReceived = d.bdReceived,
                          bdUserCXC = d.bdUserCXC,
                          Currency = IM.BusinessRules.BR.BRCurrencies.GetCurrencyId(d.bdcu).cuN,
                          PaymentType = IM.BusinessRules.BR.BRPaymentTypes.GetPaymentTypeId(d.bdpt).ptN,
                          CreditCard = String.IsNullOrEmpty(d.bdcc) ? String.Empty : IM.BusinessRules.BR.BRCreditCardTypes.GetCreditCardTypeId(d.bdcc).ccN
                        }).ToList();


      }

      dtgDeposits.ItemsSource = deposits;
    }

    /// Carga la información del Grid de las tarjetas de crédito de los invitados
    /// </summary>
    private void LoadCreditCardGrid()
    {
      var cc = new List<GuestCreditCardInvitation>();
      if (!dtgCCCompany.HasItems && !_lstNewCreditsCard.Any()) //sino tiene registros cargamos por primera vez el grid
      {
        cc = IM.BusinessRules.BR.BRGuests.GetGuestCreditCardInvitation(_guestID);
        _lstCreditsCard = (from c in cc
                           select new IM.Model.GuestCreditCard
                           {
                             gdcc = c.ccID,
                             gdgu = c.ccgu,
                             gdQuantity = Convert.ToByte(c.ccQty)
                           }).ToList();
      }
      else
      {
        cc = (from c in _lstCreditsCard
              select new GuestCreditCardInvitation
              {
                ccID = c.gdcc,
                ccgu = c.gdgu,
                ccQty = Convert.ToByte(c.gdQuantity),
                CreditCard = IM.BusinessRules.BR.BRCreditCardTypes.GetCreditCardTypeId(c.gdcc).ccN
              }).ToList().Union(
              from c in _lstNewCreditsCard
              select new GuestCreditCardInvitation
              {
                ccID = c.ccID,
                ccgu = c.ccgu,
                ccQty = Convert.ToByte(c.ccQty),
                CreditCard = IM.BusinessRules.BR.BRCreditCardTypes.GetCreditCardTypeId(c.ccID).ccN
              }    
          ).ToList();
      }

      dtgCCCompany.ItemsSource = cc;

      txtCCCompany.Text = cc != null && cc.Any() ? cc.First().ccID : String.Empty;
    }

    /// Carga la información del Grid de los invitados extra
    /// </summary>
    private void LoadAdditionalInformartionGrid()
    {
      var ad = new List<IM.Model.Guest>();
      if(!dtgAdditInform.HasItems && !_lstNewAdditionals.Any())
      {
        ad = IM.BusinessRules.BR.BRGuestsAdditional.GetGuestsAdditional(_guestID);
        _lstAdditionals = ad;
      }
      else
      {
        ad = _lstAdditionals.Union(
                                  from g in _lstNewAdditionals
                                  select new IM.Model.Guest
                                  {
                                    guID = g.guID,
                                    guLastName1 = g.guLastName1,
                                    guFirstName1 = g.guFirstName1
                                  }).ToList();
      }
      
      dtgAdditInform.ItemsSource = ad;
    }

    #endregion

    #endregion

    #region Métodos para guardar la información del invitado
    /// <summary>
    /// Guarda la informaciónde la forma en la base de datos
    /// </summary>
    private void SaveGuestInformation()
    {
      var guest = IM.BusinessRules.BR.BRGuests.GetGuestById(_guestID);

      #region Tipos de invitacion
      guest.guQuinella = ForBooleanValue(chkQuiniella.IsChecked);
      guest.guShow = ForBooleanValue(chkShow.IsChecked);
      guest.guInterval = ForBooleanValue(chkInterval.IsChecked);
      #endregion

      #region Lenguaje
      guest.gula = ForStringValue(cmbLanguage.SelectedValue);
      #endregion

      #region Información del invitado
      guest.guInvitD = String.IsNullOrEmpty(txtDate.Text) ?  DateTime.Today : Convert.ToDateTime(txtDate.Text);
      guest.guInvitT = String.IsNullOrEmpty(txtTime.Text) ?  DateTime.Now : Convert.ToDateTime(txtTime.Text);
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

      var newGifts = _lstNewGifts.Where(g => g.isNew == true).ToList();
      var updatedGifts = _lstNewGifts.Where(g => g.isUpdate == true).ToList();
      #endregion

      #region Información PR
      if (_invitationType == InvitationType.OutHouse)
      {
        guest.guPRInfo = ForStringValue(txtPRContract.Text);
      }

      guest.guPRInvit1 = ForStringValue(txtPR.Text);

      if (_invitationType != InvitationType.Host)
      {
        guest.gusr = ForStringValue(cmbSalesRoom.SelectedValue);
      }
      else
      {
        guest.guloInvit = ForStringValue(txtLocation.Text);
      }

      guest.guBookD = txtBookingDate.SelectedDate.HasValue ? txtBookingDate.SelectedDate.Value : (DateTime?)null;
      guest.guBookT = txtBookingDate.SelectedDate.HasValue && !String.IsNullOrEmpty(cmbBookingTime.SelectedValue.ToString()) ? Convert.ToDateTime(cmbBookingTime.SelectedValue.ToString()) : (DateTime?)null;

      if (_invitationType != InvitationType.OutHouse)
      {
        guest.guReschD = txtRescheduleDate.SelectedDate.HasValue ? txtRescheduleDate.SelectedDate : null;
        guest.guReschT = txtRescheduleDate.SelectedDate.HasValue && !String.IsNullOrEmpty(cmbRescheduleTime.SelectedValue.ToString()) ? Convert.ToDateTime(cmbRescheduleTime.SelectedValue.ToString()) : (DateTime?) null;
        guest.guResch = ForBooleanValue(chkResch.IsChecked);
      }


      if (_invitationType != InvitationType.Animation && _invitationType != InvitationType.Regen)
        guest.guAntesIO = ForBooleanValue(chkBeforeInOut.IsChecked);

      guest.guDirect = ForBooleanValue(chkDirect.IsChecked);

      if (_invitationType != InvitationType.Host)
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

      var newDeposits = _lstNewDeposits.Where(d => d.isNew == true).ToList();
      var updatedDeposits = _lstNewDeposits.Where(d => d.isUpdated == true).ToList();
      #endregion

      #region Guest Status 
      guest.guGStatus = ForStringValue(txtGuestStatus.Text);
      var newGS = _lstNewGuestStatus.Where(d => d.isNew == true).ToList();
      var updatedGS = _lstNewGuestStatus.Where(d => d.isUpdate == true).ToList();
      #endregion

      #region Room Quantity
      if (_invitationType != InvitationType.OutHouse)
        guest.guRoomsQty = ForIntegerValue(txtRoomQuantity.Text);
      #endregion

      #region Credit Cards
      var newCC = new List<GuestCreditCardInvitation>();
      var updatedCC = new List<GuestCreditCardInvitation>();
      if (_invitationType == InvitationType.InHouse || _invitationType == InvitationType.Host)
      {
        guest.guCCType = ForStringValue(txtCCCompany.Text);

        newCC = _lstNewCreditsCard.Where(d => d.isNew == true).ToList();
        updatedCC = _lstNewCreditsCard.Where(d => d.isUpdate == true).ToList();
      }


      #endregion

      #region Additional Information *********FALTA LLENAR EL GRID
      var newGA = _lstNewAdditionals.Where(d => d.isNew == true).ToList();
      var updatedGA = _lstNewAdditionals.Where(d => d.isUpdate == true).ToList();

      
      #endregion


      IM.BusinessRules.BR.BRGuests.SaveGuestInvitation(guest
                                                      , ConvertGiftInvitationToInvitationGiftList(newGifts)
                                                      , updatedGifts
                                                      , ConvertBookingDepositInvitationToBookingDepositList(newDeposits)
                                                      , updatedDeposits
                                                      , ConvertGuestStatusInvitationToGuestStatusList(newGS)
                                                      , updatedGS
                                                      , ConvertGuestStatusInvitationToGuestCreditCardList(newCC)
                                                      , updatedCC
                                                      , ConvertGuestAdditionalInvitationToGuestList(newGA)
                                                      , updatedGA);

    }

    #endregion

    #region Métodos para validar al información

    /// <summary>
    /// Revisa que todas las validaciones sean aplicadas antes de guardar
    /// </summary>
    /// <returns></returns>
    private bool Validate()
    {
      bool res = GeneralValidation();

      if (res)
      {

      }

      return res;

    }

    /// <summary>
    /// Valida los datos generales
    /// </summary>
    /// <returns>Boolean</returns>
    private bool GeneralValidation()
    {
      bool res = true;

      if (_invitationType == InvitationType.InHouse)
      {
        if (!ValidateFolioInHouse()) //validamos que el folio de invitación inhouse
        {
          Helpers.UIHelper.ShowMessage("The reservation number has been assigned previously");
          res = false;
        }
      }
      else if (_invitationType == InvitationType.OutHouse)
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
      if (!txtBookingDate.SelectedDate.HasValue)//Validamos que haya ingresado una fecha de booking
      {
        Helpers.UIHelper.ShowMessage("Specify the Booking date");
        txtBookingDate.Focus();
        res = false;
      }
      else if (!ValidateClosedDate())//validamos que la fecha de bookin no este en fecha cerrada
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
      else if (_invitationType == InvitationType.OutHouse && String.IsNullOrEmpty(txtPRContract.Text))//validamos que haya seleccionado un proveedor Contact
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
      else if (_invitationType != InvitationType.Host && String.IsNullOrEmpty(txtSalesRoom.Text))//validamos que haya seleccionado una sala de ventas
      {
        Helpers.UIHelper.ShowMessage("Specify a Sales Room");
        cmbSalesRoom.Focus();
        res = false;
      }
      else if (!ValidateBookindAndRescheduleDate()) //validamos las fechas de booking y reschedule
      {
        res = false;
      }
      else if (_invitationType == InvitationType.Host && String.IsNullOrEmpty(txtLocation.Text))  // validamos la locacion
      {
        Helpers.UIHelper.ShowMessage("Specify a Location");
        cmbLocation.Focus();
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
      else if (System.Text.RegularExpressions.Regex.IsMatch(txtPax.Text, @"[^0-9.-]+"))//^[0-9]([.][0-9]{1,2})?$
      {
        Helpers.UIHelper.ShowMessage("Pax control accepts numbers");
        txtPax.Focus();
        res = false;
      }
      else if (Convert.ToDecimal(txtPax.Text) < 0.1m || Convert.ToDecimal(txtPax.Text) > 1000)
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
      //else if ()//' validamos los regalos
      //{

      //}
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
      return _closeDate.HasValue && txtBookingDate.SelectedDate.Value <= _closeDate.Value;
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
        if(txtBookingDate.SelectedDate.Value < DateTime.Today)//validamos que la fecha de booking no sea antes de hoy
        {
          Helpers.UIHelper.ShowMessage("Booking date can not be before today.");
          txtBookingDate.Focus();
          res = false;
        }
      }

      if (!res) return res;

      if (txtDeparture.SelectedDate.HasValue && (txtDeparture.SelectedDate.Value < txtBookingDate.SelectedDate.Value)) //Validamos que la fecha de entrada no sea mayor ala fecha de salida
      {
        Helpers.UIHelper.ShowMessage("Booking date can not be after Check Out date");
        txtBookingDate.Focus();
        res = false;
      }
      else if (txtArrival.SelectedDate.HasValue && (txtArrival.SelectedDate.Value > txtBookingDate.SelectedDate.Value))
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
      if (!res) return false;// si se permite reschedule

      if (!txtRescheduleDate.IsEnabled)// si se puede modificar la fecha de reschedule
        return false;

      if (!txtRescheduleDate.SelectedDate.HasValue)
      {
        errorMessage ="Specify the reschedule date.";
        res = false;
      }
      else if (txtDeparture.SelectedDate.HasValue && (txtRescheduleDate.SelectedDate.Value > txtDeparture.SelectedDate.Value)) //validamos que la fecha de reschedule no sea despues de la fecha de salida
      {
        errorMessage = "Reschedule date can not be after check out date.";
        res = false;
      }
      else if (txtArrival.SelectedDate.HasValue && (txtRescheduleDate.SelectedDate.Value < txtArrival.SelectedDate.Value)) // validamos que la fecha de reschedule no sea antes de la fecha de llegada
      {
        errorMessage = "Reschedule date can not be before booking date.";
        res = false;
      }
      else if (txtRescheduleDate.SelectedDate.Value < DateTime.Today) // validamos que la fecha de reschedule no sea antes de hoy
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
    /// Validamos la información de los nuevos regalos
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateNewGift()
    {
      int qty, adult, minor, eAdult;
      bool res = true;
      
      var gift =new  IM.Model.Gift();
      if (cmbGifts.SelectedIndex != -1)
      {
        gift = IM.BusinessRules.BR.BRGifts.GetGiftId(cmbGifts.SelectedValue.ToString());
      }

        if (String.IsNullOrEmpty(txtQtyGift.Text))
      {
        Helpers.UIHelper.ShowMessage("Specify the Quantity of Gifts");
        txtQtyGift.Focus();
        res = false;
      }
      else if (!int.TryParse(txtQtyGift.Text, out qty) && qty <= 0)
      {
        Helpers.UIHelper.ShowMessage("Input a correct value in Quantity");
        txtQtyGift.Focus();
        res = false;
      }
      else if(cmbGifts.SelectedIndex == -1)
      {
        Helpers.UIHelper.ShowMessage("Specify a Gifts");
        cmbGifts.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtAdultGift.Text))
      {
        Helpers.UIHelper.ShowMessage("Specify the Quantity of Adults");
        txtAdultGift.Focus();
        res = false;
      }
      else if (!int.TryParse(txtAdultGift.Text, out adult) && adult < 0)
      {
        Helpers.UIHelper.ShowMessage("Input a correct value in Adult");
        txtAdultGift.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtMinortGift.Text))
      {
        Helpers.UIHelper.ShowMessage("Specify the Quantity of Minors");
        txtMinortGift.Focus();
        res = false;
      }
      else if (!int.TryParse(txtMinortGift.Text, out minor) && minor < 0)
      {
        Helpers.UIHelper.ShowMessage("Input a correct value in Quantity");
        txtMinortGift.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtEAdultGift.Text))
      {
        Helpers.UIHelper.ShowMessage("Specify the Quantity of Extra Adults");
        txtEAdultGift.Focus();
        res = false;
      }
      else if (!int.TryParse(txtEAdultGift.Text, out eAdult) && eAdult < 0)
      {
        Helpers.UIHelper.ShowMessage("Input a correct value in Extra Adults");
        txtEAdultGift.Focus();
        res = false;
      }
      else if(adult == 0 && minor == 0)
      {
        Helpers.UIHelper.ShowMessage("You have not asigned the gift to people");
        txtAdultGift.Focus();
        res = false;
      }
      else if(gift != null && gift.giMaxQty < qty)
      {
        string error = String.Format("The maximu quantity authorized of the gift {0} has been exceeded.\n Max authotized = {1}",gift.giN, gift.giMaxQty);
        Helpers.UIHelper.ShowMessage(error);
        res = false;
      }
      return res;
    }

    /// <summary>
    /// Validamos la información de los nuevos depósitos
    /// </summary>
    /// <returns></returns>
    private bool ValidateNewDesposit()
    {
      bool res = true;
      decimal deposit, received;

      if(String.IsNullOrEmpty(txtDeposit.Text))
      {
        Helpers.UIHelper.ShowMessage("Specify the amount of deposit");
        txtDeposit.Focus();
        res = false;
      }
      else if (! Decimal.TryParse(txtDeposit.Text,out deposit) && deposit <= 0)
      {
        Helpers.UIHelper.ShowMessage("the Input is incorrect or deposit cannot be a negative amount or zero");
        txtDeposit.Focus();
        res = false;
      }
      if (String.IsNullOrEmpty(txtReceived.Text))
      {
        Helpers.UIHelper.ShowMessage("Specify the amount of received");
        txtReceived.Focus();
        res = false;
      }
      else if (!Decimal.TryParse(txtReceived.Text, out received) && received < 0)
      {
        Helpers.UIHelper.ShowMessage("the Input is incorrect or reveiced cannot be a negative amount");
        txtReceived.Focus();
        res = false;
      }
      else if(cmbCurrencyDeposit.SelectedIndex == -1)
      {
        Helpers.UIHelper.ShowMessage("Choose a currency type");
        cmbCurrencyDeposit.Focus();
        res = false;
      }
      else if (cmbPaymentTypeDeposit.SelectedIndex == -1)
      {
        Helpers.UIHelper.ShowMessage("Choose a payment type");
        cmbPaymentTypeDeposit.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtRefundPlace.Text))
      {
        Helpers.UIHelper.ShowMessage("Specify theRefund place");
        txtRefundPlace.Focus();
        res = false;
      }
      else if (cmbPaymentTypeDeposit.SelectedValue.ToString() == "CC" && cmbCreditCardDeposit.SelectedIndex == -1)
      {
        Helpers.UIHelper.ShowMessage("Choose a credit card type");
        cmbCreditCardDeposit.Focus();
        res = false;
      }
      else if (cmbPaymentTypeDeposit.SelectedValue.ToString() == "CC" && String.IsNullOrEmpty(txtCardNumberDeposit.Text))
      {
        Helpers.UIHelper.ShowMessage("Enter the last four credit card number");
        txtCardNumberDeposit.Focus();
        res = false;
      }
      else if(cmbPaymentTypeDeposit.SelectedValue.ToString() == "CC" && String.IsNullOrEmpty(txtExpirationCard.Text))
      {
        Helpers.UIHelper.ShowMessage("Enter the expiration date of credit card");
        txtExpirationCard.Focus();
        res = false;
      }
      else if (cmbPaymentTypeDeposit.SelectedValue.ToString() == "CC" && !ValidateExpirationDate(txtExpirationCard.Text))
      {
        Helpers.UIHelper.ShowMessage("The correct format is  MM/YY");
        txtExpirationCard.Focus();
        res = false;
      }
      else if(cmbPaymentTypeDeposit.SelectedValue.ToString() == "CC" && String.IsNullOrEmpty(txtAuthorizathionId.Text))
      {
        Helpers.UIHelper.ShowMessage("Enter the autorizathion ID");
        txtExpirationCard.Focus();
        res = false;
      }
      else if (String.IsNullOrEmpty(txtFolioCxC.Text))
      {
        Helpers.UIHelper.ShowMessage("Enter the folio Cxc");
        txtFolioCxC.Focus();
        res = false;
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

    /// <summary>
    /// Validamos la información de un nuevo guest status
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateNewGuestStatus()
    {
      bool res = true;
      int qty;
      if(String.IsNullOrEmpty(txtQtyGuestStatus.Text))
      {
        Helpers.UIHelper.ShowMessage("Specify a Guest Status Quantity");
        txtQtyGuestStatus.Focus();
        res = false;
      }
      else if(!int.TryParse(txtQtyGuestStatus.Text, out qty) && qty<1)
      {
        Helpers.UIHelper.ShowMessage("The Guest Status Quantity cannot be less than 1");
        txtQtyGuestStatus.Focus();
        res = false;
      }
      else if(cmbGuestStatus.SelectedIndex == -1)
      {
        Helpers.UIHelper.ShowMessage("Choose a Guest Status");
        cmbGuestStatus.Focus();
        res = false;
      }

      return res;
    }

    /// <summary>
    /// Validamos la información de una nueva tarjeta de credito
    /// </summary>
    /// <returns>Boolean</returns>
    private bool ValidateNewCreditCard()
    {
      bool res = true;

      int qty;
      if (String.IsNullOrEmpty(txtQtyCreditCard.Text))
      {
        Helpers.UIHelper.ShowMessage("Specify a Credit Card Quantity");
        txtQtyCreditCard.Focus();
        res = false;
      }
      else if (!int.TryParse(txtQtyCreditCard.Text, out qty) && qty < 1)
      {
        Helpers.UIHelper.ShowMessage("The Credit Card Quantity cannot be less than 1");
        txtQtyCreditCard.Focus();
        res = false;
      }
      else if (cmbCreditCards.SelectedIndex == -1)
      {
        Helpers.UIHelper.ShowMessage("Choose a Credit Card");
        cmbCreditCards.Focus();
        res = false;
      }

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
    private void CalculateCostsPrices(bool useCxCCost = false)
    {
      decimal costAdult, costMinor, priceAdult, priceMinor, priceExtraAdult, quantity;
      
      foreach(var row in gifts)
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
          row.costAdults = quantity * (row.igAdults + row.igExtraAdults) * costAdult;
          // Total del costo de menores
          row.costMinors = quantity * row.igMinors * costMinor;
          // Total del precio adultos
          row.priceAdults = quantity * row.igAdults * priceAdult;
          //Total del precio de menores
          row.priceMinor = quantity * row.igMinors * priceMinor;
          // Total del precio de adultos extra
          row.priceExtraAdults = quantity * row.igExtraAdults * priceExtraAdult;
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

      foreach(var row in gifts)
      {
        // calculamos el costo del regalo
        cost = row.costAdults + row.costMinors;

        //calculamos el precio del regalo
        price = row.priceAdults + row.priceMinor + row.priceExtraAdults;

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
    private void CalculateMaxAuthGifts()
    {
      decimal maxAuthGifts = 0;

      foreach(GuestStatusInvitation row in dtgGuestStatus.Items)
      {
        var guestStatusType = IM.BusinessRules.BR.BRGuestStatusType.GetGuestStatusTypeById(row.gsID);
        maxAuthGifts += row.gsQty * guestStatusType.gsMaxAuthGifts;
      }

      txtMaxAuth.Text = maxAuthGifts.ToString("$#,##0.00;$(#,##0.00)");
    }
    #endregion

    #endregion

  }
}
