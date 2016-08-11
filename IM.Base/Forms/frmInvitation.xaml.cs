using System.Windows;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.Model;
using IM.Base.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using System.ComponentModel;
using System;
using System.Windows.Data;
using IM.Styles.Classes;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmInvitation.xaml
  /// </summary>
  public partial class frmInvitation : Window
  {
    #region Propiedades, Atributos
    //Parametros del constructor
    public readonly EnumModule _module;
    public readonly EnumInvitationType _invitationType;
    public UserData _user;
    private readonly int _guestId;
    public readonly bool _allowReschedule;
    //Grids Banderas
    private DataGridCellInfo _IGCurrentCell;//Celda que se esta modificando
    private bool _hasError = false; //Sirve para las validaciones True hubo Error | False NO
    private bool _isCellCancel = false;//Sirve para cuando se cancela la edicion de una Celda
    private bool _dontShowAgainGuestStatus = false;
    #endregion
    /// <summary>
    /// Inicializa en formulario de invitacion
    /// </summary>
    /// <param name="module">Tipo de invitacion</param>
    /// <param name="invitationType">Tipo de acceso a la invitacion</param>
    /// <param name="user">Usuario Login</param>
    /// <param name="guestId">guID - valor default 0</param>
    /// <param name="allowReschedule">Si permite Reschedule - valor default true</param>
    /// <history>
    /// [erosado] 09/08/2016  Created.
    /// </history>
    public frmInvitation(EnumModule module, EnumInvitationType invitationType, UserData user, int guestId = 0, bool allowReschedule = true)
    {
      try
      {

        var catObj = new CommonCatObject(module, invitationType, user, guestId);
        _module = module;
        _guestId = guestId;
        _user = user;
        _invitationType = invitationType;
        DataContext = catObj;
        _allowReschedule = allowReschedule;
        InitializeComponent();

        #region Inicializar Grids

        #region dtgGift
        dtgGifts.InitializingNewItem += ((object sender, InitializingNewItemEventArgs e) =>
        {
          if (e.NewItem != null)
          {
            ((InvitationGift)e.NewItem).igQty = 1;
          }
        });
        GridHelper.SetUpGrid(dtgGifts, new InvitationGift());
        #endregion

        #endregion
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #region Eventos de la ventana
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Cargamos la UI dependiendo del tipo de Invitacion
      ControlsConfiguration();
      //Configuramos los controles (Maxlength, caracteres etc.)
      UIHelper.SetUpControls(new Guest(), this);
    }

    private void imgButtonSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      bool _isValid = true;

      //Obtenemos el DataContext
      CommonCatObject dataContext = DataContext as CommonCatObject;

      //Validamos controles comunes y validaciones basicas
      if (!InvitationValidationRules.ValidateGeneral(this, dataContext))
      {
        _isValid = false;
        tabGeneral.TabIndex = 0;
      }
      //Si paso la primer validacion, validamos los grids invitsGift, bookingDeposits, creditCard, additionalGuest
      if (_isValid)
      {
        _isValid = InvitationValidationRules.ValidateInformationGrids(this, dataContext);
      }

    }

    private void imgButtonEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {

    }
    private void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {

    }
    private void imgButtonCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {

    }
    private void imgButtonLog_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      //Si viene de Host la invitacion le mandamos el SalesRoom, en lugar del leadsource, es solo informativo.
      frmGuestLog frmGuestLog = new frmGuestLog(_guestId, _user.LeadSource != null ? _user.LeadSource.lsID : _user.SalesRoom.srID);
      frmGuestLog.Owner = this;
      frmGuestLog.ShowDialog();
    }
    /// <summary>
    /// Evento del Combobox GuestStatus
    /// Sirve para actualizar la caja de texto txtGiftMaxAuth
    /// dependiendo del GuestStatus que elija el usuario.
    /// </summary>
    ///<history>
    ///[erosado]  02/08/2016  Created.
    /// </history>    
    private void cmbGuestStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //Obtenemos el GuestStatusType del combobox cmbGuestStatus
      var guStatusType = cmbGuestStatus.SelectedItem as GuestStatusType;

      txtGiftMaxAuth.Text = string.Format("{0:C2}", guStatusType != null ? guStatusType.gsMaxAuthGifts : 0);

      //TODO: GUESTSTATUSTYPES Revizar el caso cuando se traigan los regalos de la Base de datos
      //GuestStatus _guestsStatus = BRGuestStatus.GetGuestsStatus(_guestID);
      //GuestStatusType _guestStatusType = BRGuestStatusTypes.GetGuestStatusTypeByID(_guestsStatus.gtgs);
      //curMaxAuthGifts = _guestsStatus.gtQuantity * _guestStatusType.gsMaxAuthGifts;
    }
    #endregion

    #region Controls Configuration

    #region ControlsConfiguration
    /// <summary>
    /// Prepara los controles para cada invitacion
    /// </summary>
    /// <param name="module">EnumInvitationType</param>
    /// <history>
    /// [erosado] 16/05/2016  Created
    /// </history>
    private void ControlsConfiguration()
    {
      //Configuramos el MenuBar
      MenuBarConfiguration();

      //Agregamos la informacion del usuario logeado
      txtUserName.Text = _user.User.peN;
      txtPlaces.Text = _module == EnumModule.Host ? _user.SalesRoom.srN : _user.Location.loN;

      //Dependiendo de cada modulo 
      switch (_module)
      {
        case EnumModule.InHouse:
          InHouseControlVisibility();
          break;
        case EnumModule.OutHouse:
          OutHouseControlVisibility();
          break;
        case EnumModule.Host:
          HostControlVisibility();
          break;
        default:
          break;
      }
    }
    #endregion

    #region ControlsConfig Hide, Visible, Collapse
    /// <summary>
    /// Prepara los controles para que trabaje con InHouseInvitation
    /// </summary>
    /// <history>
    /// [erosado] 14/05/2016  Created.
    /// </history>
    private void InHouseControlVisibility()
    {
      if (_invitationType == EnumInvitationType.newExternal)
      {
        stkFlightNumber.Visibility = Visibility.Collapsed;//Ocultamos FlighInfo de  brdPRInfo - Grid Column 4 
        stkOutInvitation.Visibility = Visibility.Collapsed;//Ocultamos el OutInvitation de brdPRInfo 
      }
      else if (_invitationType == EnumInvitationType.existing)
      {
        stkOutInvitation.Visibility = Visibility.Collapsed; //Quitamos Out.Invint de brdGuestInfo
        stkPRContact.Visibility = Visibility.Collapsed;//Quitamos PRContact de  brdPRInfo - Grid Column 0
        stkFlightNumber.Visibility = Visibility.Collapsed;//Ocultamos FlighInfo de  brdPRInfo - Grid Column 4 
        imgSearch.Visibility = Visibility.Collapsed; //Quitamos el boton de search.
      }
    }
    /// <summary>
    /// Prepara los controles para que trabaje con OutHouseInvitation
    /// </summary>
    /// <history>
    /// [erosado] 14/05/2016  Created.
    /// </history>
    private void OutHouseControlVisibility()
    {
      stkRsrvNum.Visibility = Visibility.Collapsed;
      imgSearch.Visibility = Visibility.Collapsed;
      stkRebookRef.Visibility = Visibility.Collapsed;
      btnReschedule.Visibility = Visibility.Collapsed;
      btnRebook.Visibility = Visibility.Collapsed;
      stkRescheduleDate.Visibility = Visibility.Collapsed;
      chkReschedule.Visibility = Visibility.Collapsed;
      brdRoomsQtyAndElectronicPurse.Visibility = Visibility.Collapsed;
      brdCreditCard.Visibility = Visibility.Collapsed;
    }
    /// <summary>
    /// Prepara los controles para que trabaje con HostInvitation
    /// </summary>
    /// <history>
    /// [erosado] 14/05/2016  Created.
    /// </history>
    private void HostControlVisibility()
    {
      stkPRContact.Visibility = Visibility.Collapsed;
      stkSales.IsEnabled = false;
      stkLocation.IsEnabled = true;
      stkFlightNumber.Visibility = Visibility.Collapsed;
      stkElectronicPurse.Visibility = Visibility.Collapsed;
    }
    #region  MenuBarConfiguration
    /// <summary>
    /// Activa o desactiva los botones de la barra de menu dependiendo del tipo de invitacion
    /// </summary>
    /// <history>
    /// [erosado] 10/08/2016  Created.
    /// </history>
    private void MenuBarConfiguration()
    {
      //Modo lectura
      if (_invitationType == EnumInvitationType.existing)
      {
        MenuBarModeReadOnly();
      }
      //Modo Nuevo
      else
      {
        MenuBarModeAddOrEdit();
      }
    }
    /// <summary>
    /// Activa los controles en modo ReadOnly
    /// </summary>
    /// <history>
    /// [erosado] 10/08/2016  Created.
    /// </history>
    private void MenuBarModeReadOnly()
    {
      imgButtonEdit.IsEnabled = true;
      imgButtonPrint.IsEnabled = true;
      imgButtonSave.IsEnabled = false;
      imgButtonCancel.IsEnabled = true;
      imgButtonLog.IsEnabled = true;
    }
    /// <summary>
    /// Activa los controles en modo Add or Edit
    /// </summary>
    /// <history>
    /// [erosado] 10/08/2016  Created.
    /// </history>
    private void MenuBarModeAddOrEdit()
    {
      imgButtonEdit.IsEnabled = false;
      imgButtonPrint.IsEnabled = false;
      imgButtonSave.IsEnabled = false;
      imgButtonCancel.IsEnabled = true;
      imgButtonLog.IsEnabled = false;
    }
    #endregion
    #endregion


    #region EnableControlsExternal
    private void EnableControlsExternal()
    {
      #region Guest Information
      txtguID.IsEnabled =
      txtguHReservID.IsEnabled = false;
      btnSearch.IsEnabled = true;
      txtguRef.IsEnabled =
      txtguInvitD.IsEnabled =
      txtguInvitT.IsEnabled = false;
      #endregion

      #region Invitation Type & Languages
      chkguQuinella.IsEnabled = true;
      chkguShow.IsEnabled =
      chkguInterval.IsEnabled = false;
      cmbLanguage.IsEnabled = true;
      #endregion

      #region Profile Opera
      txtguIdProfileOpera.IsEnabled =
      txtguLastNameOriginal.IsEnabled =
      txtguFirstNameOriginal.IsEnabled = false;
      #endregion

      #region Guest1 & Guest 2
      txtguLastName1.IsEnabled =
      txtguFirstName1.IsEnabled =
      txtguAge1.IsEnabled =
      cbogums1.IsEnabled =
      txtguOccup1.IsEnabled =
      txtguEmail1.IsEnabled = true;

      txtguLastName2.IsEnabled =
      txtguFirstName2.IsEnabled =
      txtguAge2.IsEnabled =
      cbogums2.IsEnabled =
      txtguOccup2.IsEnabled =
      txtguEmail2.IsEnabled = true;
      #endregion

      #region PR, SalesRoom, etc..
      btnChange.IsEnabled =
      btnReschedule.IsEnabled =
      btnRebook.IsEnabled = false;
      cmbPRContact.IsEnabled =
      cmbPR.IsEnabled =
      cmbSalesRooms.IsEnabled = true;
      cmbLocation.IsEnabled = false;
      chkguAntesIO.IsEnabled =
      dtpBookDate.IsEnabled =
      cmbBookT.IsEnabled = true;
      chkDirect.IsEnabled = false;
      dtpRescheduleDate.IsEnabled =
      cmbReschT.IsEnabled =
      chkReschedule.IsEnabled = false;
      #endregion

      #region OtherInfo
      txtguExtraInfo.IsEnabled =
      txtguRoomNum.IsEnabled =
      cmbOtherInfoHotel.IsEnabled =
      cmbOtherInfoAgency.IsEnabled =
      cmbOtherInfoCountry.IsEnabled =
      txtguPax.IsEnabled =
      dtpOtherInfoArrivalD.IsEnabled =
      dtpOtherInfoDepartureD.IsEnabled = true;
      #endregion

      #region GuestStatus
      cmbGuestStatus.IsEnabled = true;
      #endregion

      #region Gifts
      dtgGifts.IsEnabled =
      txtGiftMaxAuth.IsReadOnly =
      txtGiftTotalCost.IsReadOnly =
      txtGiftTotalPrice.IsReadOnly = true;
      #endregion

      #region Deposits
      dtgBookingDeposits.IsEnabled =
      txtBurned.IsEnabled =
      cmbCurrency.IsEnabled =
      cmbPaymentType.IsEnabled =
      cmbResorts.IsEnabled = true;
      #endregion

      #region Credit Cards
      txtguCCType.IsEnabled = false;
      dtgCCCompany.IsEnabled = true;
      #endregion

      #region Rooms Qty And ElectronicPurse
      txtguAccountGiftsCard.IsEnabled = false;
      #endregion

    }
    #endregion

    #region EnableControlsInHouse
    private void EnableControlsInHouse()
    {

      #region Barra de Menu
      //imgButtonEdit.IsEnabled = _invitationType == EnumInvitationMode.modAdd ? false : true;
      //imgButtonPrint.IsEnabled = _invitationType == EnumInvitationMode.modAdd ? false : true;
      //imgButtonSave.IsEnabled = _invitationType == EnumInvitationMode.modAdd ? true : false;
      //imgButtonCancel.IsEnabled = _invitationType == EnumInvitationMode.modAdd ? true : false;
      //imgButtonLog.IsEnabled = dbcontex && txtguID.Text != "0" ? true : false;
      #endregion

      #region Guest Information
      txtguID.IsEnabled =
      txtguHReservID.IsEnabled =
      btnSearch.IsEnabled =
      txtguRef.IsEnabled =
      txtguInvitD.IsEnabled =
      txtguInvitT.IsEnabled = false;
      #endregion

      #region Invitation Type & Languages
      chkguQuinella.IsEnabled = true;
      chkguShow.IsEnabled =
      chkguInterval.IsEnabled = false;
      cmbLanguage.IsEnabled = true;
      #endregion

      #region Profile Opera
      txtguIdProfileOpera.IsEnabled =
      txtguLastNameOriginal.IsEnabled =
      txtguFirstNameOriginal.IsEnabled = false;
      #endregion

      #region Guest1 & Guest 2
      txtguLastName1.IsEnabled =
      txtguFirstName1.IsEnabled =
      txtguAge1.IsEnabled =
      cbogums1.IsEnabled =
      txtguOccup1.IsEnabled =
      txtguEmail1.IsEnabled = true;

      txtguLastName2.IsEnabled =
      txtguFirstName2.IsEnabled =
      txtguAge2.IsEnabled =
      cbogums2.IsEnabled =
      txtguOccup2.IsEnabled =
      txtguEmail2.IsEnabled = true;
      #endregion

      #region PR, SalesRoom, etc..
      btnChange.IsEnabled =
      btnReschedule.IsEnabled =
      btnRebook.IsEnabled = false;
      cmbPRContact.IsEnabled =
      cmbPR.IsEnabled =
      cmbSalesRooms.IsEnabled = true;
      cmbLocation.IsEnabled = false;
      chkguAntesIO.IsEnabled =
      dtpBookDate.IsEnabled =
      cmbBookT.IsEnabled = true;
      chkDirect.IsEnabled = false;
      dtpRescheduleDate.IsEnabled =
      cmbReschT.IsEnabled =
      chkReschedule.IsEnabled = false;
      #endregion

      #region OtherInfo
      txtguExtraInfo.IsEnabled =
      txtguRoomNum.IsEnabled =
      cmbOtherInfoHotel.IsEnabled =
      cmbOtherInfoAgency.IsEnabled =
      cmbOtherInfoCountry.IsEnabled =
      txtguPax.IsEnabled =
      dtpOtherInfoArrivalD.IsEnabled =
      dtpOtherInfoDepartureD.IsEnabled = true;
      #endregion

      #region GuestStatus
      cmbGuestStatus.IsEnabled = true;
      #endregion

      #region Gifts
      dtgGifts.IsEnabled =
      txtGiftMaxAuth.IsReadOnly =
      txtGiftTotalCost.IsReadOnly =
      txtGiftTotalPrice.IsReadOnly = true;
      #endregion

      #region Deposits
      dtgBookingDeposits.IsEnabled =
      txtBurned.IsEnabled =
      cmbCurrency.IsEnabled =
      cmbPaymentType.IsEnabled =
      cmbResorts.IsEnabled = true;
      #endregion

      #region Credit Cards
      txtguCCType.IsEnabled = false;
      dtgCCCompany.IsEnabled = true;
      #endregion

      #region Rooms Qty And ElectronicPurse
      txtguAccountGiftsCard.IsEnabled = false;
      txtguRoomsQty.IsEnabled = true;
      #endregion

    }
    #endregion


    #region EnableControls
    private void IsEnableControls()
    {
      #region Guest Information
      txtguID.IsEnabled = false;
      txtguHReservID.IsEnabled = false;
      btnSearch.IsEnabled = false;//Nueva INhouse False|external true|OutHouseNew oculto
      txtguOutInvitNum.IsEnabled = true; //InHouseExternal oculto| OutHouseNew true
      txtguRef.IsEnabled = false;//Nueva INHouse false|external false|OutHouseNew oculto
      txtguInvitD.IsEnabled = false;//Nueva InHouse False|external false|OutHouseNew oculto
      txtguInvitT.IsEnabled = false;//Nueva InHouse False|external false|OutHouseNew oculto
      #endregion

      switch (_module)
      {
        case EnumModule.InHouse:
          break;
        case EnumModule.OutHouse:
          break;
        case EnumModule.Host:
          cmbSalesRooms.IsEnabled = false;
          break;
        default:
          break;
      }

    }

    #endregion


    #endregion

    #region Eventos del GRID Invitation Gift

    #region BeginningEdit
    /// <summary>
    /// Se ejecuta antes de que entre en modo edicion alguna celda
    /// </summary>
    /// <history>
    /// [erosado] 08/08/2016  Created.
    /// </history>
    private void dtgGifts_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      //Preguntamos si desea agregar GuestStatusType para el calculo de costos
      if (cmbGuestStatus.SelectedValue == null && !_dontShowAgainGuestStatus)
      {
        MessageBoxResult result = UIHelper.ShowMessage("We recommend first select the status of the guest, that would help us calculate costs and prices, do you want to select it now?", MessageBoxImage.Question, "Intelligence Marketing");
        if (result == MessageBoxResult.Yes)
        {
          e.Cancel = true;
          _hasError = true;
          _isCellCancel = true;
          _dontShowAgainGuestStatus = false;
          cmbGuestStatus.Focus();
        }
        else
        {
          _dontShowAgainGuestStatus = true;
        }

      }
      else
      {
        _hasError = false;
        _isCellCancel = false;
      }

      //Si el grid no esta en modo edicion, permite hacer edicion.
      if (!GridHelper.IsInEditMode(dtgGifts) && !_hasError)
      {
        dtgGifts.BeginningEdit -= dtgGifts_BeginningEdit;
        //Obtenemos el objeto de la fila que se va a editar
        InvitationGift invitationGift = e.Row.Item as InvitationGift;
        //Obtenemos la celda que vamos a validar
        _IGCurrentCell = dtgGifts.CurrentCell;
        //Hacemos la primera validacion 
        InvitationValidationRules.StartEdit(ref invitationGift, ref _IGCurrentCell, dtgGifts, ref _hasError);
        //Si tuvo algun error de validacion cancela la edicion de la celda.
        e.Cancel = _hasError;
        dtgGifts.BeginningEdit += dtgGifts_BeginningEdit;
      }
      //Si ya se encuenta en modo EDIT cancela la edicion, para no salirse de la celda sin hacer Commit antes
      else
      {
        e.Cancel = true;
      }
    }
    #endregion

    #region PreparingCellForEdit
    /// <summary>
    /// Se ejecuta cuando la celda entra en modo edicion
    /// </summary>
    /// <history>
    /// [erosado] 08/08/2016  Created.
    /// </history>
    private void dtgGifts_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      //Sirve para agregar el Focus a las celdas
      Control ctrl = e.EditingElement as Control;
      ctrl.Focus();
    }
    #endregion

    #region CellEditEnding
    /// <summary>
    /// Se ejecuta cuando la celda en edicion pierde el foco
    /// </summary>
    /// <history>
    /// [erosado] 08/08/2016  Created.
    /// </history>
    private async void dtgGifts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      //Si paso las validaciones del preparingCellForEdit
      if (!_hasError)
      {
        //Si viene en modo Commit
        if (e.EditAction == DataGridEditAction.Commit)
        {
          //esta bandera se pone en falso por que No se ha cancelado la edicion de la celda
          _isCellCancel = false;
          //Obtenemos el Objeto 
          InvitationGift invitationGift = e.Row.Item as InvitationGift;

          //Bandera que checata que todo salga bien en la validacion siguiente.
          bool _hasErrorValidateEdit = false;
          //Validamos la celda
          // InvitationValidationRules.ValidateEdit(ref invitationGift, ref _hasErrorValidateEdit, ref _IGCurrentCell);

          //Si Paso las validaciones
          if (!_hasErrorValidateEdit)
          {
            //Obtenemos el program
            var program = await BRLeadSources.GetLeadSourceProgram(_user.LeadSource.lsID);

            InvitationValidationRules.AfterEdit(dtgGifts, ref invitationGift, _IGCurrentCell, ref txtGiftTotalCost, ref txtGiftTotalPrice, ref txtGiftMaxAuth, cmbGuestStatus.SelectedItem as GuestStatusType, program);
          }
          //Si fallaron las validaciones del AfterEdit se cancela la edicion de la celda.
          else
          {
            e.Cancel = true;
          }
        }
        //Si entra en modo Cancel Se enciende esta bandera ya que servira en RowEditEnding
        else
        {
          _isCellCancel = true;
        }
      }
    }
    #endregion

    #region RowEditEnding
    /// <summary>
    /// Se ejecuta cuando la fila pierde el foco, o termina la edicion (Commit o Cancel)
    /// </summary>
    /// <history>
    /// [erosado] 02/08/2016  Created.
    /// </history>
    private void dtgGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)

    {
      DataGrid dtg = sender as DataGrid;
      InvitationGift invitationGift = e.Row.Item as InvitationGift;

      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCancel)
        {
          dtg.RowEditEnding -= dtgGifts_RowEditEnding;
          dtg.CancelEdit();
          dtg.RowEditEnding -= dtgGifts_RowEditEnding;
        }
        else
        {
          if (invitationGift.igQty == 0 || string.IsNullOrEmpty(invitationGift.iggi))
          {
            UIHelper.ShowMessage("Please enter the required fields Qty and Gift to continue", MessageBoxImage.Exclamation, "Intelligence Marketing");
            e.Cancel = true;
          }
        }
      }
      else
      {
        //CommonCatObject dtContext = DataContext as CommonCatObject;
        //dtContext.InvitationGiftList.RemoveAt(e.Row.GetIndex());
      }
    }
    #endregion

    #endregion


  }
}
