using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmGuest.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 09/Feb/2016 Created
  /// </history>
  public partial class frmGuest : Window
  {
    #region Atributos

    private UserData _user;
    private readonly int _guestId;
    private EnumModule _module;
    private EnumProgram _program;
    private EnumMode _mode;
    private bool _isInvitation;

    //private bool _isInvitation;


    private DataGridCellInfo _igCurrentCell;//Celda que se esta modificando
    private bool _hasError; //Sirve para las validaciones True hubo Error | False NO
    private bool _isCellCancel;//Sirve para cuando se cancela la edicion de una Celda
    private bool _dontShowAgainGuestStatus;
    public Guest NewGuest => ((GuestInvitationRules)DataContext).Guest;
    private GuestInvitationRules _catObj;
    public Guest GuestParent;
    
    #region Objetos

    

    #endregion Objetos

    #endregion Atributos

    #region Constructores y destructores

    public frmGuest(UserData user, int guestId, EnumModule module,EnumProgram program, EnumMode mode, bool isInvitation)
    {
      WindowStartupLocation = WindowStartupLocation.CenterScreen;
      _user = user;
      _guestId = guestId;
      _module = module;
      _program = program;
      _mode = mode;
      _isInvitation = isInvitation;
      InitializeComponent();
    }

    #endregion Constructores y destructores

    #region Métodos de la forma

    /// <summary>
    /// Configura los elementos de la forma
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void frmGuest_Loaded(object sender, RoutedEventArgs e)
    {
      //Iniciamos el BusyIndicator
      _busyIndicator.IsBusy = true;
      _busyIndicator.BusyContent = "Please wait, we are preparing the Guest form...";
      _catObj = new GuestInvitationRules(_module,_user, _guestId);
      await _catObj.LoadAll();
      ConfigurationControls();
      LoadControls();
      UIHelper.SetUpControls(new Guest(), this);
      DataContext = _catObj;
      Gifts.CalculateTotalGifts(dtgGifts, EnumGiftsType.InvitsGifts, "igQty", "iggi", "igPriceM", "igPriceMinor", "igPriceAdult", "igPriceA", "igPriceExtraAdult", txtGiftTotalCost, txtGiftTotalPrice);
      UpdateLayout();

      //Detenemos el BusyIndicator
      _busyIndicator.IsBusy = false;
    }

    /// <summary>
    /// Guarda/Actualiza los datos de un Guest.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (!Validate()) return;
      try
      {
        var agency = (cmbOtherInfoAgency.SelectedItem as AgencyShort);
        ((GuestInvitation)DataContext).Guest.gumk = (await BRAgencies.GetAgenciesByIds(new List<string> { agency.agID })).FirstOrDefault()?.agmk;
        _catObj.Guest.guCheckOutHotelD = (DateTime)dtpDeparture.Value;
        var result = await BRGuests.SaveGuest((GuestInvitation)DataContext);
        if (result > 0)
        {
          ((GuestInvitation)DataContext).Guest = await BRGuests.GetGuest(result);
          UIHelper.ShowMessage("Guest saved successfully.");
        }
        DialogResult = true;
        Close();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    /// <summary>
    /// Evento del Combobox GuestStatus
    /// Sirve para actualizar la caja de texto txtGiftMaxAuth
    /// dependiendo del GuestStatus que elija el usuario.
    /// </summary>
    ///<history>
    ///[edgrodriguez]  11/08/2016  Created.
    /// </history>
    private void cmbGuestStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //Obtenemos el GuestStatusType del combobox cmbGuestStatus
      var guStatusType = cmbGuestStatus.SelectedItem as GuestStatusType;

      
      txtGiftMaxAuth.Text = $"{guStatusType?.gsMaxAuthGifts ?? 0:C2}";

      //TODO: GUESTSTATUSTYPES Revizar el caso cuando se traigan los regalos de la Base de datos
      //GuestStatus _guestsStatus = BRGuestStatus.GetGuestsStatus(_guestID);
      //GuestStatusType _guestStatusType = BRGuestStatusTypes.GetGuestStatusTypeByID(_guestsStatus.gtgs);
      //curMaxAuthGifts = _guestsStatus.gtQuantity * _guestStatusType.gsMaxAuthGifts;
    }

    /// <summary>
    /// Cancela el proceso de creacion o actualizacion de un guest.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }

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

    #endregion Métodos Genéricos

    #region Eventos del GRID Invitation Gift

    #region BeginningEdit

    /// <summary>
    /// Se ejecuta antes de que entre en modo edicion alguna celda
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/08/2016  Created.
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
        _igCurrentCell = dtgGifts.CurrentCell;
        //Hacemos la primera validacion
        InvitationValidationRules.StartEdit(ref invitationGift, ref _igCurrentCell, dtgGifts, ref _hasError);
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

    #endregion BeginningEdit

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
      ctrl?.Focus();
    }

    #endregion PreparingCellForEdit

    #region CellEditEnding

    /// <summary>
    /// Se ejecuta cuando la celda en edicion pierde el foco
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/08/2016  Created.
    /// </history>
    private void dtgGifts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
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
            //TODO: Revisar el program Edgar
            //var program = await BRLeadSources.GetLeadSourceProgram(_user.LeadSource.lsID);

            InvitationValidationRules.AfterEdit(_guestId,dtgGifts, ref invitationGift, _igCurrentCell, ref txtGiftTotalCost, ref txtGiftTotalPrice, ref txtGiftMaxAuth, cmbGuestStatus.SelectedItem as GuestStatusType, _catObj.Program);
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

    #endregion CellEditEnding

    #region RowEditEnding

    /// <summary>
    /// Se ejecuta cuando la fila pierde el foco, o termina la edicion (Commit o Cancel)
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/08/2016  Created.
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
          if (invitationGift.igQty != 0 && !string.IsNullOrEmpty(invitationGift.iggi)) return;
          UIHelper.ShowMessage("Please enter the required fields Qty and Gift to continue", MessageBoxImage.Exclamation, "Intelligence Marketing");
          e.Cancel = true;
        }
      }
    }

    #endregion RowEditEnding

    #endregion Eventos del GRID Invitation Gift

    #endregion Métodos de la forma

    #region Métodos privados

    #region Configuración de controles

    private void ConfigurationControls()
    {
      btnSave.IsEnabled = !(_mode == EnumMode.ReadOnly);
      dtgGifts.IsReadOnly = _mode == EnumMode.ReadOnly;
      grbGuestInfo.IsEnabled =
        grbOtherInformation.IsEnabled =
          grbGuest1.IsEnabled =
            grbGuest2.IsEnabled =
              grbGuestStatus.IsEnabled = !(_mode == EnumMode.ReadOnly);

      dtpArrival.IsEnabled =
        dtpDeparture.IsEnabled = !(_program == EnumProgram.Inhouse);

      stpTotalPrice.Visibility = (_isInvitation) ? Visibility.Collapsed : Visibility.Visible;
    }

    #endregion Configuración de controles

    #region Cargar datos del invitado

    /// <summary>
    /// Manda llamar los métodos para cargar la información del guest.
    /// </summary>
    private void LoadControls()
    {
      LoadCommonControls(); //Se cargan los controles que son comunes en todos los tipos de invitacion.
    }

    /// <summary>
    /// Carga los controles comunes de todos los tipos de invitación
    /// </summary>
    /// <history>
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private void LoadCommonControls()
    {
      #region User

      txtUser.Text = _user.User.peID;
      txtPassword.Password = _user.User.pePwd;
      if (_mode == EnumMode.ReadOnly) return;
      cmbOtherInfoAgency.SelectedValue = cmbOtherInfoAgency.SelectedValue == null ? GuestParent.guag : cmbOtherInfoAgency.SelectedValue;
      dtpArrival.Value = dtpArrival.Value == null ? _catObj.Guest.guCheckInD : dtpArrival.Value;
      dtpDeparture.Value = dtpArrival.Value == null ? _catObj.Guest.guCheckOutD : dtpDeparture.Value;
      _catObj.Guest.gulsOriginal = _catObj.Guest.gulsOriginal == null ? GuestParent.guls : _catObj.Guest.gulsOriginal;
      _catObj.Guest.guls = _catObj.Guest.guls == null ? GuestParent.guls : _catObj.Guest.guls;
      _catObj.Guest.gusr = _catObj.Guest.gusr == null ? GuestParent.gusr : _catObj.Guest.gusr;
      _catObj.Guest.guCheckOutHotelD = _catObj.Guest.guCheckOutHotelD == null ? GuestParent.guCheckOutD : _catObj.Guest.guCheckOutHotelD;

      #endregion User
    }

    #endregion Cargar datos del invitado

    #region Métodos para Válidar la información

    private bool Validate()
    {
      bool res = true;

      if (!dtpArrival.Value.HasValue) //validamos la fecha de llegada
      {
        res = false;
        UIHelper.ShowMessage("Specify an arrival date");
        dtpArrival.Focus();
      }
      else if (!dtpDeparture.Value.HasValue) //validamos la fecha de salida
      {
        res = false;
        UIHelper.ShowMessage("Specify an departure date");
        dtpDeparture.Focus();
      }
      else if (cmbOtherInfoAgency.SelectedIndex == -1)
      {
        res = false;
        UIHelper.ShowMessage("Specify an agency");
        cmbOtherInfoAgency.Focus();
      }
      else if (string.IsNullOrWhiteSpace(txtguLastName1.Text)) //validamos el apellido
      {
        res = false;
        UIHelper.ShowMessage("Input the guest last name");
        txtguLastName1.Focus();
      }
      else if (string.IsNullOrWhiteSpace(txtguFirstName1.Text)) //validamos el nombre
      {
        res = false;
        UIHelper.ShowMessage("Input the guest first name");
        txtguFirstName1.Focus();
      }
      else if (!ValidateChangedBy()) //validamos quien hizo el cambio y su contraseña
      {
        res = false;
      }
      else if (!ValidateChangedByExist())//validamos que los datos de quien hizo el cambio y su contraseña existan
      {
        res = false;
      }

      return res;
    }

    /// <summary>
    /// Validamos la información de quien realiza los cambios
    /// </summary>
    /// <returns></returns>
    private bool ValidateChangedBy()
    {
      bool res = true;

      if (string.IsNullOrWhiteSpace(txtUser.Text))
      {
        res = false;
        UIHelper.ShowMessage("Specify who is making the change.");
        txtUser.Focus();
      }
      else if (string.IsNullOrWhiteSpace(txtPassword.Password))
      {
        res = false;
        UIHelper.ShowMessage("Specify who is making the change.");
        txtUser.Focus();
      }

      return res;
    }

    private bool ValidateChangedByExist()
    {
      var pass = EncryptHelper.Encrypt(txtPassword.Password);

      var valid = BRHelpers.ValidateChangedByExist(txtUser.Text, pass, GuestParent.guls).FirstOrDefault();//BRGuests.ChangedByExist(txtUser.Text, pass, _user.LeadSource.lsID);
       
      if (string.IsNullOrWhiteSpace(valid?.Focus)) return true;

      //desplegamos el mensaje de error
      UIHelper.ShowMessage(valid.Message);

      //establecemos el foco en el control que tiene el error
      switch (valid.Focus)
      {
        case "ID":
        case "ChangedBy":
          txtUser.Focus();
          break;

        case "Password":
          txtPassword.Focus();
          break;
      }
      return false;
    }

    #endregion Métodos para Válidar la información

    #endregion Métodos privados
  }
}