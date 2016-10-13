using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model.Helpers;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmInvitation.xaml
  /// </summary>
  public partial class frmInvitation
  {
    #region Propiedades, Atributos

    //Parametros del constructor
    public readonly EnumModule _module;
    public readonly EnumInvitationType _invitationType;
    public UserData _user;
    private readonly int _guestId;
    public bool _isEditing;

    //Grids Banderas
    private DataGridCellInfo _IGCurrentCell;//Celda que se esta modificando
    private bool _hasError; //Sirve para las validaciones True hubo Error | False NO
    private bool _isCellCancel;//Sirve para cuando se cancela la edicion de una Celda
    private bool _dontShowAgainGuestStatus;
    private bool _isCellCommitDeposit;//Valida si el commit se hace desde la celda de Deposits
    private bool _isCellCommitCC;//Valida si el commit se hace desde la celda de credit cards
    private bool _isCellCommitGuestAdditional;//Valida si el commit se hace desde la celda de GuestAdditional
    private EnumMode guestFormMode = EnumMode.Edit;
    public GuestInvitationRules dbContext { get; set; }
    public bool SaveGuestInvitation { get; set; }

    public delegate void statusBarInfo(bool show, string message = "");
    public event statusBarInfo _statusBarInfo;

    private bool _saveStatus;

    #endregion Propiedades, Atributos

    /// <summary>
    /// Inicializa en formulario de invitacion
    /// </summary>
    /// <param name="module">Tipo de invitacion</param>
    /// <param name="invitationType">Tipo de acceso a la invitacion</param>
    /// <param name="user">Usuario Login</param>
    /// <param name="guestId">guID - valor default 0</param>
    /// <history>
    /// [erosado] 09/08/2016  Created.
    /// </history>
    public frmInvitation(EnumModule module, EnumInvitationType invitationType, UserData user, int guestId = 0)
    {
      dbContext = new GuestInvitationRules(module, invitationType, user, guestId);
      _module = module;
      _guestId = guestId;
      _user = user;
      _invitationType = invitationType;
      DataContext = dbContext;
      InitializeComponent();
      
      _statusBarInfo += StatusBarInfo;
      #region Inicializar Grids

      #region dtgGift

      dtgGifts.InitializingNewItem += (sender, e) =>
      {
        if (e.NewItem != null)
        {
          ((InvitationGift)e.NewItem).igQty = 1;
        }
      };
      GridHelper.SetUpGrid(dtgGifts, new InvitationGift());

      GridHelper.SetUpGrid(dtgCCCompany, new GuestCreditCard());

      GridHelper.SetUpGrid(dtgGuestAdditional, new Guest());

      GridHelper.SetUpGrid(dtgBookingDeposits, new BookingDeposit());
      #endregion
      #endregion dtgGift
    }

    #region Eventos de la ventana

    #region Window_Loaded
    /// <summary>
    /// Evento que se genera al cargar la ventana
    /// </summary>
    ///<history>
    ///[erosado]  17/08/2016  Created.
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        //Cargamos la informacion 
        await LoadInvitationForm();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
        _busyIndicator.IsBusy = false;
      }
    }
    #endregion Window_Loaded

    #region BtnEdit_OnClick
    /// <summary>
    /// Evento que se genera al presionar el boton Edit
    /// </summary>
    ///<history>
    ///[erosado]  17/08/2016  Created.
    /// </history>
    private void BtnEdit_OnClick(object sender, RoutedEventArgs e)
    {
      //Si el Guest ya hizo Show No podemos editar nada.
      if (dbContext.Guest.guShow)
      {
        UIHelper.ShowMessage("Guest has made show");
      }
      else
      {
        //Activamos modo edicion
        _isEditing = true;

        //Activamos los contenedores para poder editarlos
        SetAdd();

        //Activamos el Modo Edit
        EditModeControlsBehavior();

        dtgBookingDeposits.IsReadOnly = IsReaOnlyBookingDeposits();
      }
    }

    #endregion BtnEdit_OnClick

    #region BtnPrint_OnClick
    /// <summary>
    /// Evento que se genera al presionar el boton Print
    /// </summary>
    ///<history>
    ///[erosado]  17/08/2016  Created.
    /// </history>
    private void BtnPrint_OnClick(object sender, RoutedEventArgs e)
    {
      //Generamos Reporte
      RptInvitationHelper.RptInvitation(_guestId, _user.User.peID);
    }
    #endregion BtnPrint_OnClick

    #region BtnSave_OnClick
    /// <summary>
    /// Evento que se genera al presionar el boton Save
    /// </summary>
    ///<history>
    ///[erosado]  17/08/2016  Created.
    /// </history>
    private async void BtnSave_OnClick(object sender, RoutedEventArgs e)
    {
      try
      {
        if (_saveStatus) return;
        _saveStatus = true;
        //Desactivamos el boton de guardar
        btnSave.IsEnabled = false;
        //Forzamos LostFocus de Pax para tener su valor real
        var element = Keyboard.FocusedElement as UIElement;
        element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        //Validamos la informacion de la invitacion Antes de guardar
        if (await InfoValidation())
        {
          //Si la informacion es valida guardamos la invitacion
          await SaveInvitation();
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
      finally
      {
        //Desactiva mensajes en la barra de estado y regresa el cursos del usuario
        _statusBarInfo?.Invoke(false);
        //Volvemos activar el boton de guardar
        btnSave.IsEnabled = true;
        //Regresamos _saveStatus a false
        _saveStatus = false;
      }
    }
    #endregion BtnSave_OnClick

    #region BtnCancel_OnClick

    /// <summary>
    /// Sirve para cancelar la edicion, y si no esta en modo edicion cierra el formulario de invitacion
    /// </summary>
    /// <history>
    /// [erosado] 11/08/2016  Created.
    /// </history>
    private async void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
      //Si estamos cancelando la edicion de una invitacion
      if (_isEditing)
      {
        //Iniciamos el BusyIndicator
        _busyIndicator.IsBusy = true;
        _busyIndicator.BusyContent = "Please wait, we are reverting all changes...";

        //Desactivamos el modo edicion
        _isEditing = false;

        //Volvemos a cargar la invitacion
        DataContext = null;
        UpdateLayout();
        dbContext = new GuestInvitationRules(_module, _invitationType, _user, _guestId);
        DataContext = dbContext;
        await dbContext.LoadAll();
        //Configuramos nuevamente el formulario
        ControlsConfiguration();

        //Habilitamos los botones editar, imprimir, guardar
        btnEdit.IsEnabled = true;
        btnPrint.IsEnabled = true;
        btnSave.IsEnabled = false;
        _busyIndicator.IsBusy = false;
      }
      //Si no estabamos editando cerramos la aplicacion
      else
      {
        Close();
      }
    }
    #endregion BtnCancel_OnClick

    #region BtnLog_OnClick
    /// <summary>
    /// Abre el formulario de GuestLog se la pasa el guID, sirve para mostrar los movimientos que ha tenido el guID
    /// </summary>
    /// <history>
    /// [erosado] 11/08/2016  Created.
    /// </history>
    private void BtnLog_OnClick(object sender, RoutedEventArgs e)
    {
      //Si viene de Host la invitacion le mandamos el SalesRoom, en lugar del leadsource, es solo informativo.
      frmGuestLog frmGuestLog = new frmGuestLog(_guestId) { Owner = this };
      frmGuestLog.ShowDialog();
    }
    #endregion BtnLog_OnClick

    #region BtnReLogin_OnClick

    /// <summary>
    /// Permite Re-Login en la invitacion
    /// </summary>
    /// <history>
    /// [erosado] 15/08/2016  Modified
    /// </history>
    private async void BtnReLogin_OnClick(object sender, RoutedEventArgs e)
    {
      var login = new frmLogin(loginType: EnumLoginType.Location, program: dbContext.Program,
      validatePermission: true, permission: _module != EnumModule.Host ? EnumPermission.PRInvitations : EnumPermission.HostInvitations,
      permissionLevel: EnumPermisionLevel.Standard, switchLoginUserMode: true, invitationMode: true, invitationPlaceId: _user.Location.loID);

      if (_user.AutoSign)
      {
        login.UserData = _user;
      }
      await login.getAllPlaces();
      login.ShowDialog();

      if (login.IsAuthenticated)
      {
        //Limpiamos el DataSource
        DataContext = null;
        //Cambiamos al usuario
        _user = login.UserData;
        //Cargar de nuevo la invitacion
        dbContext = new GuestInvitationRules(_module, _invitationType, _user, _guestId);
        DataContext = dbContext;
        //Configuramos de nuevo el formulario
        //Window_Loaded(this, null);
        await LoadInvitationForm();
      }
    }
    #endregion BtnReLogin_OnClick

    #region BtnSearchButton_OnClick
    /// <summary>
    /// Obtiene un Folio de Reservacion
    /// </summary>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// [vipacheco] 18/08/2016 Modified -> Se agregó la invocacion para la busqueda de huespedes por # de reservacion.
    /// </history>
    private void BtnSearchButton_OnClick(object sender, RoutedEventArgs e)
    {
      frmSearchReservation search = new frmSearchReservation(_user) { Owner = this };

      // Verificamos si se selecciono un guest
      var showDialog = search.ShowDialog();

      if (showDialog != null && showDialog.Value)
      {
        //Seteamos la informacion de SearchGuest en nuestro objeto Guest
        dbContext.SetRervationOrigosInfo(search._reservationInfo);
      }
    }
    #endregion BtnSearchButton_OnClick

    #region btnChange_Click

    /// <summary>
    /// Evento del boton Change dentro de OtherInformation
    /// </summary>
    /// <history>
    /// [erosado] 16/08/2016  Created.
    /// </history>
    private void btnChange_Click(object sender, RoutedEventArgs e)
    {
      Change();
    }

    #endregion btnChange_Click

    #region btnReschedule_Click

    /// <summary>
    /// Evento del boton Reschedule dentro de OtherInformation
    /// </summary>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// </history>
    private void btnReschedule_Click(object sender, RoutedEventArgs e)
    {
      Reschedule();
    }

    #endregion btnReschedule_Click

    #region btnRebook_Click

    /// <summary>
    /// Evento del boton Rebook dentro de OtherInformation
    /// </summary>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// </history>
    private void btnRebook_Click(object sender, RoutedEventArgs e)
    {
      Rebook();
    }

    #endregion btnRebook_Click

    #region cmbGuestStatus_SelectionChanged

    /// <summary>
    /// Evento del Combobox GuestStatus, Sirve para actualizar la caja de texto txtGiftMaxAuth dependiendo del GuestStatus que elija el usuario.
    /// </summary>
    ///<history>
    ///[erosado]  02/08/2016  Created.
    /// </history>
    private void cmbGuestStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //Obtenemos el GuestStatusType del combobox cmbGuestStatus
      var guStatusType = cmbGuestStatus.SelectedItem as GuestStatusType;
      //Asigamos el MaxAuth a la caja de texto
      txtGiftMaxAuth.Text = $"{guStatusType?.gsMaxAuthGifts ?? 0:C2}";

    }

    #endregion cmbGuestStatus_SelectionChanged

    #region cmbSalesRooms_SelectionChanged

    /// <summary>
    /// Obtiene la informacion de los tourTimes cada que se cambia de sala de ventas
    /// </summary>
    ///<history>
    ///[erosado]  16/08/2016  Created.
    /// </history>
    private async void cmbSalesRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      try
      {
        if (dtpBookDate.Value.HasValue && dtpBookDate?.Value != DateTime.MinValue &&
          cmbSalesRooms?.SelectedItem != null && cmbBookT.IsEnabled)
        {
          //Consultamos los horarios disponibles
          dbContext.TourTimesBook = await LoadTourTimes(cmbSalesRooms.SelectedValue.ToString(), dtpBookDate.Value.Value);
        }
        if (dtpRescheduleDate.Value.HasValue && dtpRescheduleDate?.Value != DateTime.MinValue &&
            cmbSalesRooms?.SelectedItem != null && cmbReschT.IsEnabled)
        {
          //Consultamos los horarios disponibles
          dbContext.TourTimesReschedule = await LoadTourTimes(cmbSalesRooms.SelectedValue.ToString(), dtpRescheduleDate.Value.Value, false);
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion cmbSalesRooms_SelectionChanged

    #region cmbLocation_SelectionChanged
    /// <summary>
    /// Cuando cambia la seleccion del location se actualiza el guls del Guest
    /// </summary>
    /// [erosado] 02/09/2016  Created.
    private void cmbLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cmbLocation.SelectedValue == null) return;
      var location = cmbLocation.SelectedItem as LocationByUser;
      if (location != null) dbContext.Guest.guls = location.lols;
    }
    #endregion

    #region cmbOtherInfoAgency_SelectionChanged
    /// <summary>
    /// Cuando cambia la seleccion de Agency se actualiza el campo Guest.guMK con el campo Agency.agMK
    /// </summary>
    /// [erosado] 03/09/2016  Created.
    private async void cmbOtherInfoAgency_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cmbOtherInfoAgency.SelectedValue == null) return;
      var agency = await BRAgencies.GetAgenciesByIds(new List<string>() { cmbOtherInfoAgency?.SelectedValue?.ToString() });
      var ag = agency.FirstOrDefault();
      if (ag != null) dbContext.Guest.gumk = ag.agmk;
    }
    #endregion

    #region dtpBookDate_ValueChanged

    /// <summary>
    /// Obtiene la informacion de los tourTimes cada que se cambia la fecha de Book
    /// </summary>
    ///<history>
    ///[erosado]  16/08/2016  Created.
    /// </history>
    private async void dtpBookDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      try
      {
        //Si tiene una fecha y es una fecha valida
        if (dtpBookDate.Value.HasValue && dtpBookDate?.Value != DateTime.MinValue && cmbSalesRooms?.SelectedItem != null)
        {
          //Consultamos los horarios disponibles
          dbContext.TourTimesBook = await LoadTourTimes(cmbSalesRooms.SelectedValue.ToString(), dtpBookDate.Value.Value);
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion dtpBookDate_ValueChanged

    #region dtpRescheduleDate_ValueChanged

    /// <summary>
    /// Obtiene la informacion de los tourTimes cada que se cambia la fecha de Reschedule
    /// </summary>
    ///<history>
    ///[erosado]  16/08/2016  Created.
    /// </history>
    private async void dtpRescheduleDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      try
      {
        //Si tiene una fecha y es una fecha valida
        if (dtpRescheduleDate.Value.HasValue && dtpRescheduleDate?.Value != DateTime.MinValue && cmbSalesRooms?.SelectedItem != null)
        {
          //Consultamos los horarios disponibles
          dbContext.TourTimesReschedule = await LoadTourTimes(cmbSalesRooms.SelectedValue.ToString(), dtpRescheduleDate.Value.Value, false);
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion dtpRescheduleDate_ValueChanged

    #endregion Eventos de la ventana

    #region ControlsConfiguration

    /// <summary>
    /// Prepara los controle para los diferentes tipos de invitacion
    /// </summary>
    /// <history>
    /// [erosado] 16/05/2016  Created
    /// </history>
    private void ControlsConfiguration()
    {
      //Agregamos la informacion del usuario logeado
      txtUserName.Text = _user.User.peN;
      txtPlaces.Text = _module == EnumModule.Host ? _user.SalesRoom.srN : _user.Location.loN;

      //Collapsed Controls Dependiendo del EnumModule
      CollapsedControls();

      //Configuramos el MenuBar
      MenuBarConfiguration();

      //Si el usuario viene con permisos diferentes a solo lectura configuramos los controles
      if (dbContext.InvitationMode != EnumMode.ReadOnly)
      {
        //Disable controls
        ControlBehaviorConfiguration();
      }

      //Si es modo ReadOnly y Modo Edit deshabilitamos los contenedores principales.
      if (dbContext.InvitationMode == EnumMode.ReadOnly || dbContext.InvitationMode == EnumMode.Edit)
      {
        SetReadOnly();
      }

      //Configuramos los controles de Change,Reschedule,Rebook
      SetupChangeRescheduleRebook();
    }

    #endregion ControlsConfiguration

    #region MenuBarConfiguration

    /// <summary>
    /// Activa o desactiva los botones de la barra de menu dependiendo del modo de la invitacion
    /// </summary>
    /// <history>
    /// [erosado] 10/08/2016  Created.
    /// </history>
    private void MenuBarConfiguration()
    {
      if (dbContext.InvitationMode == EnumMode.Add)
      {
        btnEdit.IsEnabled = false;
        btnPrint.IsEnabled = false;
        btnSave.IsEnabled = true;
        btnCancel.IsEnabled = true;
        btnLog.IsEnabled = _guestId != 0;
      }
      else if (dbContext.InvitationMode == EnumMode.Edit)
      {
        btnEdit.IsEnabled = true;
        btnPrint.IsEnabled = true;
        btnSave.IsEnabled = false;
        btnCancel.IsEnabled = true;
        btnLog.IsEnabled = true;
      }
      else if (dbContext.InvitationMode == EnumMode.ReadOnly)
      {
        btnEdit.IsEnabled = false;
        btnPrint.IsEnabled = true;
        btnSave.IsEnabled = false;
        btnCancel.IsEnabled = false;
        btnLog.IsEnabled = true;
      }
    }

    #endregion MenuBarConfiguration

    #region CollapsedControlsConfiguration

    #region CollapsedControls

    /// <summary>
    /// Colapsa los controles dependiendo del EnumModule
    /// </summary>
    /// <history>
    /// [erosado] 11/08/2016  Created.
    /// </history>
    private void CollapsedControls()
    {
      /*Todos los controles por Default estan en Modo Visible
       *Esta ventana solo colapsa controles que no se van a utilizar
       *Esto depende del EnumInvitationType */
      switch (_module)
      {
        case EnumModule.InHouse:
          InHouseCollapsed();
          break;

        case EnumModule.OutHouse:
          OutHouseCollapsed();
          break;

        case EnumModule.Host:

          if (_invitationType == EnumInvitationType.newExternal || _invitationType == EnumInvitationType.existing)
          {
            InHouseCollapsed();
          }
          else if (_invitationType == EnumInvitationType.newOutHouse)
          {
            OutHouseCollapsed();
          }
          break;
      }
    }

    #endregion CollapsedControls

    #region InHouseCollapsed

    /// <summary>
    /// Colapsa controles para la invitacion InHouse
    /// </summary>
    /// <history>
    /// [erosado] 11/08/2016  Created.
    /// </history>
    private void InHouseCollapsed()
    {
      stkOutInvitation.Visibility = Visibility.Collapsed;
      //Para que funcione el ValidateHelper debemos collapsar el control que se valida, aunque su padre ya este collapsado
      stkPRContact.Visibility = Visibility.Collapsed;
      cmbPRContact.Visibility = Visibility.Collapsed;
      stkFlightNumber.Visibility = Visibility.Collapsed;
    }

    #endregion InHouseCollapsed

    #region OutHouseCollapsed

    /// <summary>
    /// Colapsa controles para la invitacion OutHouse
    /// </summary>
    /// <history>
    /// [erosado] 11/08/2016  Created.
    /// </history>
    private void OutHouseCollapsed()
    {
      stkRsrvNum.Visibility = Visibility.Collapsed;
      btnSearchButton.Visibility = Visibility.Collapsed;
      stkRebookRef.Visibility = Visibility.Collapsed;
      btnReschedule.Visibility = Visibility.Collapsed;
      btnRebook.Visibility = Visibility.Collapsed;
      stkRescheduleDate.Visibility = Visibility.Collapsed;
      brdCreditCard.Visibility = Visibility.Collapsed;
      brdRoomsQtyAndElectronicPurse.Visibility = Visibility.Collapsed;
    }

    #endregion OutHouseCollapsed

    #endregion CollapsedControlsConfiguration

    #region ControlBehavior

    #region ControlBehaviorConfiguration

    /// <summary>
    /// Sirve para configurar el comportamiento de los controles dependiendo del modo de la invitacion y el modulo desde donde se abre.
    /// </summary>
    /// <history>
    /// [erosado] 15/08/2016  Created.
    /// </history>
    private void ControlBehaviorConfiguration()
    {
      switch (_module)
      {
        case EnumModule.InHouse:
          ControlsBehaviorInHouse();
          break;

        case EnumModule.OutHouse:
          ControlsBehaviorOutHouse();
          break;

        case EnumModule.Host:
          if (_invitationType == EnumInvitationType.newExternal || _invitationType == EnumInvitationType.existing)
          {
            ControlsBehaviorInHouse();
          }
          else if (_invitationType == EnumInvitationType.newOutHouse)
          {
            ControlsBehaviorOutHouse();
          }
          break;
      }
    }

    #endregion ControlBehaviorConfiguration

    #region ControlsBehaviorInHouse

    /// <summary>
    /// Cambia el comportamiento de los controles visbles en la invitacion InHouse
    /// </summary>
    /// <history>
    /// [erosado] 12/08/2016  Created.
    /// </history>
    private void ControlsBehaviorInHouse()
    {
      #region Enable false

      chkguShow.IsEnabled = false;
      chkguInterval.IsEnabled = false;
      chkDirect.IsEnabled = _module == EnumModule.Host;
      cmbLocation.IsEnabled = _module == EnumModule.Host;
      cmbSalesRooms.IsEnabled = _module != EnumModule.Host;
      stkRescheduleDate.IsEnabled = false;
      btnChange.IsEnabled = false;
      btnReschedule.IsEnabled = false;
      btnRebook.IsEnabled = false;
      btnAddGuestAdditional.IsEnabled = _invitationType == EnumInvitationType.newExternal;
      btnSearchGuestAdditional.IsEnabled = dbContext.InvitationMode != EnumMode.ReadOnly;
      btnSearchButton.IsEnabled = string.IsNullOrWhiteSpace(dbContext.Guest.guHReservID);
      #endregion Enable false

      #region IsReadOnly

      txtguID.IsReadOnly = true;
      txtguHReservID.IsReadOnly = true;
      txtguRef.IsReadOnly = true;
      dtpguInvitD.IsReadOnly = true;
      tpkguInvitT.IsReadOnly = true;
      txtguIdProfileOpera.IsReadOnly = true;
      txtguLastNameOriginal.IsReadOnly = true;
      txtguFirstNameOriginal.IsReadOnly = true;
      txtguAccountGiftsCard.IsReadOnly = true;
      dtgGuestAdditional.IsReadOnly = dbContext.InvitationMode == EnumMode.ReadOnly;

      #endregion IsReadOnly

      #region Mandatory Controls
      cmbGuestStatus.Tag = "Guest Status";
      #endregion

      //Si es una invitacion existente
      if (dbContext.InvitationMode != EnumMode.Add)
      {
        //Desactivamos los siguientes controles.
        stkPRContact.IsEnabled = false;
        stkPR.IsEnabled = false;
        stkLocation.IsEnabled = false;
        stkSales.IsEnabled = false;
        dtpBookDate.IsEnabled = false;
        cmbBookT.IsEnabled = false;
        stkRescheduleDate.IsEnabled = false;
      }
    }

    #endregion ControlsBehaviorInHouse

    #region ControlsBehaviorOutHouse

    /// <summary>
    /// Cambia el comportamiento de los controles visbles en la invitacion OutHouse
    /// </summary>
    /// <history>
    /// [erosado] 12/08/2016  Created.
    /// </history>
    private void ControlsBehaviorOutHouse()
    {
      #region Enable false

      chkguShow.IsEnabled = false;
      chkguInterval.IsEnabled = false;
      chkDirect.IsEnabled = false;
      btnChange.IsEnabled = dbContext.InvitationMode != EnumMode.Add;
      cmbLocation.IsEnabled = _module == EnumModule.Host;
      cmbSalesRooms.IsEnabled = _module != EnumModule.Host;
      //Si viene del modulo Host, se valida que la invitacion es NewOuthouse y tenga el modo Add.
      //Si viene del modulo Outhouse el modo ReadOnly se desactiva.
      btnAddGuestAdditional.IsEnabled = (_module == EnumModule.Host) ? _invitationType == EnumInvitationType.newOutHouse && dbContext.InvitationMode==EnumMode.Add : true; //!(dbContext.InvitationMode == EnumMode.ReadOnly && _invitationType == EnumInvitationType.existing);
      btnSearchGuestAdditional.IsEnabled = (_module == EnumModule.Host) ? _invitationType == EnumInvitationType.newOutHouse && dbContext.InvitationMode == EnumMode.Add : true;//!(dbContext.InvitationMode == EnumMode.ReadOnly && _invitationType == EnumInvitationType.existing);

      #endregion Enable false

      #region IsReadOnly

      txtguID.IsReadOnly = true;
      dtpguInvitD.IsReadOnly = true;
      tpkguInvitT.IsReadOnly = true;
      txtguIdProfileOpera.IsReadOnly = true;
      txtguLastNameOriginal.IsReadOnly = true;
      txtguFirstNameOriginal.IsReadOnly = true;
      //Si viene del modulo Host, se valida que la invitacion se NewOuthouse y tenga el modo Add.
      //Si viene del modulo Outhouse el modo ReadOnly se desactiva.
      dtgGuestAdditional.IsReadOnly = (_module == EnumModule.Host) ? !(_invitationType == EnumInvitationType.newOutHouse && dbContext.InvitationMode == EnumMode.Add) : false;
      #endregion IsReadOnly

      //Si OutHouse y es una invitacion existente
      if (_module == EnumModule.OutHouse && dbContext.InvitationMode != EnumMode.Add)
      {
        //Desactivamos los siguientes controles.
        stkPR.IsEnabled = false;
        stkLocation.IsEnabled = false;
        stkSales.IsEnabled = false;
        dtpBookDate.IsEnabled = false;
        cmbBookT.IsEnabled = false;
      }
    }

    #endregion ControlsBehaviorOutHouse

    #region StarModeControls
    /// <summary>
    /// Reinicia los controles para que luego sean configurados dependiendo del modulo y invitationType
    /// </summary>
    private void StarModeControls()
    {
      //Barra Menu
      btnEdit.IsEnabled = btnPrint.IsEnabled = btnSave.IsEnabled = btnCancel.IsEnabled = btnLog.IsEnabled = btnReLogin.IsEnabled = true;
      //Guest Information
      brdGuestInfo.IsEnabled = stkGuestInfo.IsEnabled = stkGuid.IsEnabled = stkRsrvNum.IsEnabled = btnSearchButton.IsEnabled = stkOutInvitation.IsEnabled = stkRebookRef.IsEnabled = stkInvitationTypeAndLenguage.IsEnabled = chkguQuinella.IsEnabled = chkguShow.IsEnabled = chkguInterval.IsEnabled = cmbLanguage.IsEnabled = true;
      //Guest Profile Opera
      brdProfileOpera.IsEnabled = stkProfileOpera.IsEnabled = true;
      //Guest 1
      brdGuest1.IsEnabled = stkGuest1.IsEnabled = true;
      //Guest 2
      brdGuest2.IsEnabled = stkGuest2.IsEnabled = true;
      //PR
      brdPRInfo.IsEnabled = btnChange.IsEnabled = btnReschedule.IsEnabled = btnRebook.IsEnabled = stkPRContact.IsEnabled = cmbPRContact.IsEnabled = stkPR.IsEnabled = cmbPR.IsEnabled = stkSales.IsEnabled = cmbSalesRooms.IsEnabled = stkLocation.IsEnabled = cmbLocation.IsEnabled = stkBookDateAndTime.IsEnabled = chkguAntesIO.IsEnabled = dtpBookDate.IsEnabled = cmbBookT.IsEnabled = chkDirect.IsEnabled = stkRescheduleDate.IsEnabled = dtpRescheduleDate.IsEnabled = cmbReschT.IsEnabled = chkReschedule.IsEnabled = stkFlightNumber.IsEnabled = true;
      //Other Information
      brdOtherInformation.IsEnabled = txtguRoomNum.IsEnabled = cmbOtherInfoHotel.IsEnabled = cmbOtherInfoAgency.IsEnabled = cmbOtherInfoCountry.IsEnabled = txtguPax.IsEnabled = dtpOtherInfoArrivalD.IsEnabled = dtpOtherInfoDepartureD.IsEnabled = true;
      //Guest Status
      brdGuestStatus.IsEnabled = cmbGuestStatus.IsEnabled = true;
      //Gifts
      brdGifts.IsEnabled = dtgGifts.IsEnabled = true;
      dtgGifts.IsReadOnly = false;
      //Deposits
      brdBookingDeposits.IsEnabled = dtgBookingDeposits.IsEnabled = txtBurned.IsEnabled = cmbCurrency.IsEnabled = cmbPaymentType.IsEnabled = cmbResorts.IsEnabled = true;
      dtgBookingDeposits.IsReadOnly = false;
      //Credit Card
      brdCreditCard.IsEnabled = txtguCCType.IsEnabled = dtgCCCompany.IsEnabled = true;
      dtgCCCompany.IsReadOnly = false;
      //Additional Guest
      brdAdditionalGuest.IsEnabled = btnSearchGuestAdditional.IsEnabled = btnAddGuestAdditional.IsEnabled = dtgGuestAdditional.IsEnabled = true;
      dtgGuestAdditional.IsReadOnly = false;
      //RoomsQtyAndElectronicPurse
      brdRoomsQtyAndElectronicPurse.IsEnabled = stkRoomsQty.IsEnabled = txtguRoomsQty.IsEnabled = stkElectronicPurse.IsEnabled = txtguAccountGiftsCard.IsEnabled = true;
    }
    #endregion

    #region EditModeControlsBehavior

    /// <summary>
    /// Sirve para saber que se puede o que no se puede editar en una invitacion, dependiendo de los permisos del usuario
    /// </summary>
    ///<history>
    ///[erosado]  15/08/2016  Created.
    /// </history>
    private void EditModeControlsBehavior()
    {
      //Se usa el Objeto CGuestObj cuando referenciamos a datos sin modificar en origos le llamaban (Original) eso quiere decir que es el backup del Guest que se esta modificando
      var serverDate = BRHelpers.GetServerDate();
      //Si viene de Host cambiamos a HostInvitations, si es de cualquier otro tipo utilizamos PRInvitations
      var permission = _module != EnumModule.Host ? EnumPermission.PRInvitations : EnumPermission.HostInvitations;

      //Edit Mode Siempre sera en modo InvitationMode = ReadOnly
      if (_module != EnumModule.Host)
      {
        //PR
        cmbPR.IsEnabled = false;
        //SalesRoom
        cmbSalesRooms.IsEnabled = false;
        //Fecha y hora del booking
        stkBookDateAndTime.IsEnabled = false;
      }
      //Si el modo en que se abre la invitacion se permiten reschedule (alloreschedule = true) desahabilitamos el stkRescheduleDate
      if (dbContext.AllowReschedule)
      {
        stkRescheduleDate.IsEnabled = false;
      }

      //BOOKING DEPOSITS
      //Si viene de InHouse o Host
      if (_module != EnumModule.OutHouse)
      {
        //Si el huesped no se ha ido, o la fecha en que se hizo la invitacion ya pasó o (no tiene permiso de invitacion  y la fecha de Booking es menor a la fecha de hoy)
        if (dbContext.Guest.guCheckOutD <= serverDate || dbContext.Guest.guInvitD != serverDate || (!_user.HasPermission(permission, EnumPermisionLevel.Special) || dbContext.
          Guest.guBookD < serverDate))
        {
          //No permitimos modificacion de depositos 
          dtgBookingDeposits.IsReadOnly = true;
          txtBurned.IsEnabled = false;
          cmbCurrency.IsEnabled = false;
          cmbPaymentType.IsEnabled = false;
          cmbResorts.IsEnabled = false;
        }
      }

      //GUEST ADDITIONAL
      //Si no es de OutHouse ni Host(es InHouse)
      if (_module == EnumModule.InHouse)
      {
        //Si la fecha de booking original es antes de hoy
        if (dbContext.CloneGuest.guBookD < serverDate)
        {
          brdAdditionalGuest.IsEnabled = false;
          guestFormMode = EnumMode.ReadOnly;
        }
        else
        {
          brdAdditionalGuest.IsEnabled = true;
          guestFormMode = EnumMode.Edit;
        }
      }
      else
      {
        brdAdditionalGuest.IsEnabled = true;
        guestFormMode = EnumMode.Edit;
      }


      //Other Info
      if (_module == EnumModule.InHouse)
      {
        //Si tiene copia de folio de reservacion, no se permite modificar la agencia
        if (!string.IsNullOrWhiteSpace(dbContext.Guest.guHReservIDC))
        {
          cmbOtherInfoAgency.IsEnabled = false;
        }
        //No se permite modificar el hotel y la fecha de llegada
        cmbOtherInfoHotel.IsEnabled = false;
        dtpOtherInfoArrivalD.IsEnabled = false;
      }

      if (_module != EnumModule.OutHouse)
      {
        //Si no tiene permiso especial el usuario NO se le permite modificar la fecha de salida
        if (!_user.HasPermission(permission, EnumPermisionLevel.Special))
        {
          dtpOtherInfoDepartureD.IsEnabled = false;
        }
      }

      //CREDIT CARDS, GUEST STATUS Y ROOMS QUANTITY

      if (_module == EnumModule.InHouse)
      {
        //Si la fecha de Booking origial es antes de hoy No permitimos modificar
        if (dbContext.CloneGuest.guBookD < serverDate)
        {
          txtguCCType.IsEnabled = false;
          dtgCCCompany.IsReadOnly = true;
          cmbGuestStatus.IsEnabled = false;
          stkRoomsQty.IsEnabled = false;
        }
      }
    }

    #endregion EditModeControlsBehavior

    #region SetReadOnly

    /// <summary>
    /// Deshabilita los controles principales
    /// </summary>
    /// <history>
    /// [erosado] 15/08/2016  Created.
    /// </history>
    private void SetReadOnly()
    {
      //Contenedores principales de la aplicacion
      brdGuestInfo.IsEnabled = false;
      brdProfileOpera.IsEnabled = false;
      brdGuest1.IsEnabled = false;
      brdGuest2.IsEnabled = false;
      brdPRInfo.IsEnabled = false;
      brdOtherInformation.IsEnabled = false;
      brdGuestStatus.IsEnabled = false;
      //Gift
      dtgGifts.IsReadOnly = true;
      //Booking Deposits
      dtgBookingDeposits.IsReadOnly = true;
      txtBurned.IsEnabled = false;
      cmbCurrency.IsEnabled = false;
      cmbPaymentType.IsEnabled = false;
      cmbResorts.IsEnabled = false;
      //CreditCard
      txtguCCType.IsEnabled = false;
      dtgCCCompany.IsReadOnly = true;
      //Additional Guest
      brdAdditionalGuest.IsEnabled = false;
      brdRoomsQtyAndElectronicPurse.IsEnabled = false;
    }

    #endregion SetReadOnly

    #region SetAdd

    /// <summary>
    /// Habilita los controles principales
    /// </summary>
    /// <history>
    /// [erosado] 15/08/2016  Created.
    /// </history>
    private void SetAdd()
    {
      //Contenedores principales de la aplicacion
      brdGuestInfo.IsEnabled = true;
      brdProfileOpera.IsEnabled = true;
      brdGuest1.IsEnabled = true;
      brdGuest2.IsEnabled = true;
      brdPRInfo.IsEnabled = true;
      brdOtherInformation.IsEnabled = true;
      brdGuestStatus.IsEnabled = true;
      //Gift
      dtgGifts.IsReadOnly = false;
      //BookingDeposits
      dtgBookingDeposits.IsReadOnly = IsReaOnlyBookingDeposits();
      txtBurned.IsEnabled = true;
      cmbCurrency.IsEnabled = true;
      cmbPaymentType.IsEnabled = true;
      cmbResorts.IsEnabled = true;
      //CreditCard
      txtguCCType.IsEnabled = true;
      dtgCCCompany.IsReadOnly = false;
      //Additional Guest
      dtgGuestAdditional.IsEnabled = true;
      brdRoomsQtyAndElectronicPurse.IsEnabled = true;

      //Si esta iniciando una edicion de invitacion desactivamos los siguientes controles
      if (_isEditing)
      {
        btnEdit.IsEnabled = false;
        btnPrint.IsEnabled = false;
        btnSave.IsEnabled = true;
      }
    }

    #endregion SetAdd

    #endregion ControlBehavior

    #region Metodos Complementarios
    #region Load Invitation Form
    /// <summary>
    /// Sirve para preparar la ventana de invitacion, con la informacion pasada en el constructor.
    /// </summary>
    /// <history>
    /// [erosado] 08/09/2016  Created.
    /// </history>
    private async Task LoadInvitationForm()
    {
      //Iniciamos el BusyIndicator
      _busyIndicator.IsBusy = true;
      _busyIndicator.BusyContent = "Please wait, we are preparing the invitation form...";
      //Cargamos la informacion
      await dbContext.LoadAll();
      //Calculamos el valor de las cajes de texto que acompañan los calculos del grid de Gift
      Gifts.CalculateTotalGifts(dtgGifts, EnumGiftsType.InvitsGifts, "igQty", "iggi", "igPriceM", "igPriceMinor", "igPriceAdult", "igPriceA", "igPriceExtraAdult", txtGiftTotalCost, txtGiftTotalPrice);
      //Cargamos la UI dependiendo del tipo de Invitacion
      ControlsConfiguration();
      //Configuramos los controles (Maxlength, caracteres etc.)
      UIHelper.SetUpControls(new Guest(), this);
      //Detenemos el BusyIndicator
      _busyIndicator.IsBusy = false;
      //Agregamos la configuracion de los grids
      GridHelper.SetUpGrid(dtgBookingDeposits, new BookingDeposit());
      //Seleccionamos el tab General 
      tabGeneral.IsSelected = true;
    }
    #endregion

    #region SetupChangeRescheduleRebook

    /// <summary>
    /// Permite / impide el cambio de booking, reschedule y rebook
    /// </summary>
    /// <history>
    /// [erosado] 15/085/2016 Created.
    /// </history>
    private void SetupChangeRescheduleRebook()
    {
      var serverDate = BRHelpers.GetServerDate();
      var permission = _module != EnumModule.Host ? EnumPermission.PRInvitations : EnumPermission.HostInvitations;

      //Si la fecha de salida es hoy o despues o el usuario tiene permiso especial de invitaciones
      if (dbContext.Guest.guCheckOutD >= serverDate || _user.HasPermission(permission, EnumPermisionLevel.Special))
      {
        //Si es una invitacion Nueva
        if (dbContext.InvitationMode == EnumMode.Add)
        {
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = false;
          btnRebook.IsEnabled = false;
        }
        //Si tiene show
        else if (dbContext.Guest.guShow)
        {
          //Si guANtesIO tiene es true no permitimos modificar el control chkguAntesIO
          if (dbContext.Guest.guAntesIO)
          {
            chkguAntesIO.IsEnabled = false;
          }

          //Solo se permite modificar Rebook
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = false;
          btnRebook.IsEnabled = true;
        }
        //Si la fecha de invitacion es hoy y no es un reschedule
        else if (dbContext.Guest.guInvitD == serverDate && !dbContext.Guest.guResch)
        {
          //Solo se permite modificar Rebook
          btnChange.IsEnabled = true;
          btnReschedule.IsEnabled = false;
          btnRebook.IsEnabled = true;
        }
        //si la fecha de invitacion es hoy y es un reschedule
        else if (dbContext.Guest.guInvitD == serverDate && dbContext.Guest.guResch)
        {
          //No se permite cambiar
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = true;
          btnRebook.IsEnabled = true;
        }
        //Si la fecha de invitacion es antes de hoy y No tiene reschedule y la fecha de booking es despues de hoy
        else if (dbContext.Guest.guInvitD < serverDate && !dbContext.Guest.guResch &&
          dbContext.Guest.guBookD > serverDate && _user.HasPermission(permission, EnumPermisionLevel.Standard))
        {
          //No se permite Reschedule y Rebook
          btnChange.IsEnabled = true;
          btnReschedule.IsEnabled = false;
          btnRebook.IsEnabled = false;
        }
        //si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es hoy
        else if (dbContext.Guest.guInvitD < serverDate && !dbContext.Guest.guResch && dbContext.Guest.guBookD == serverDate)
        {
          //se permite cambiar, reschedule y rebook
          btnChange.IsEnabled = true;
          btnReschedule.IsEnabled = true;
          btnRebook.IsEnabled = true;
        }
        //si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es antes de hoy
        else if (dbContext.Guest.guInvitD < serverDate && !dbContext.Guest.guResch && dbContext.Guest.guBookD < serverDate)
        {
          //No se permite cambiar
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = true;
          btnRebook.IsEnabled = true;
        }
        //si la fecha de invitacion es antes de hoy y es un rechedule
        else if (dbContext.Guest.guInvitD < serverDate && dbContext.Guest.guResch)
        {
          //No se permite cambiar
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = true;
          btnRebook.IsEnabled = true;
        }
        //si NO se permite hacer reschedule de invitaciones
        if (!dbContext.AllowReschedule && dbContext.InvitationMode != EnumMode.Add)
        {
          //Ocultamos los controles de reschedule y rebook
          btnReschedule.Visibility = Visibility.Collapsed;
          btnRebook.Visibility = Visibility.Collapsed;
          stkRescheduleDate.Visibility = Visibility.Collapsed;
          //Permitimos cambiar
          btnChange.IsEnabled = true;
        }
      }
      //Si la fecha de salida es antes que hoy o el usuario NO tiene permisos speciales
      else
      {
        //No se permite cambiar reschedule ni rebook
        btnChange.IsEnabled = false;
        btnReschedule.IsEnabled = false;
        btnRebook.IsEnabled = false;
      }
    }

    #endregion SetupChangeRescheduleRebook

    #region LoadTourTimes

    /// <summary>
    /// Sirve para cargar los TourTimes disponibles
    /// </summary>
    /// <param name="salesRoom">Sala de ventas</param>
    /// <param name="selectedDate">Dia seleccionado</param>
    /// <param name="bookingDate">True Booking|False Reschedule </param>
    /// <returns></returns>
    private async Task<List<TourTimeAvailable>> LoadTourTimes(string salesRoom, DateTime selectedDate, bool bookingDate = true)
    {
      List<TourTimeAvailable> tourTimes = new List<TourTimeAvailable>();
      //Obtenemos el LeadSource
      var leadSource = _module != EnumModule.Host ? _user.LeadSource.lsID : dbContext.Guest.guls;

      //Obtenemos la fecha del servidor
      var serverDate = BRHelpers.GetServerDate();

      //Si es una invitacion nueva
      if (dbContext.InvitationMode == EnumMode.Add)
      {
        tourTimes = await BRTourTimesAvailables.GetTourTimesAvailables(leadSource, salesRoom, selectedDate);
      }
      //Si es en modo edicion
      if (dbContext.InvitationMode == EnumMode.Edit)
      {
        //Booking
        if (bookingDate)
        {
          tourTimes = await BRTourTimesAvailables.GetTourTimesAvailables(leadSource, salesRoom, selectedDate,
            dbContext.CloneGuest?.guBookD,
            dbContext.CloneGuest?.guBookT, serverDate);
        }
        //Reschedule
        else
        {
          tourTimes = await BRTourTimesAvailables.GetTourTimesAvailables(leadSource, salesRoom, selectedDate,
            dbContext.CloneGuest?.guReschD,
            dbContext.CloneGuest?.guReschT, serverDate);
        }
      }
      return tourTimes;
    }

    #endregion LoadTourTimes

    #region Change

    /// <summary>
    /// Este evento de ejecuta desde el boton btnChange
    /// </summary>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// </history>
    private void Change()
    {
      var serverDate = BRHelpers.GetServerDate();
      var permission = _module != EnumModule.Host ? EnumPermission.PRInvitations : EnumPermission.HostInvitations;

      //Deshabilitamos los controles de Change, Reschedule y Rebook
      btnChange.IsEnabled = false;
      btnReschedule.IsEnabled = false;
      btnRebook.IsEnabled = false;

      //Si se permite hacer reschedule
      if (dbContext.AllowReschedule)
      {
        //si la fecha de invitacion es hoy o si la fecha de booking es despues de hoy
        if (dbContext.Guest.guInvitD == serverDate || dbContext.Guest.guBookD > serverDate)
        {
          //Fecha y hora del booking
          stkBookDateAndTime.IsEnabled = true;
          //Se activa la fecha de Book
          dtpBookDate.IsEnabled = true;
          //Se activa la Hora de Book
          cmbBookT.IsEnabled = true;

          //Si tiene permiso especial
          if (_user.HasPermission(permission, EnumPermisionLevel.Special))
          {
            //PR Contact
            if (!cmbPRContact.IsEnabled)
            {
              stkPRContact.IsEnabled = true;
              cmbPRContact.IsEnabled = true;
            }
            //PR
            stkPR.IsEnabled = true;
            cmbPR.IsEnabled = true;
            //Sala
            if (!cmbSalesRooms.IsEnabled)
            {
              stkSales.IsEnabled = true;
              cmbSalesRooms.IsEnabled = true;
            }
          }
        }
      }
      //Si no se permite Reschedule
      else
      {
        //Fecha y hora del booking
        stkBookDateAndTime.IsEnabled = true;
        //Se activa la fecha de Book
        dtpBookDate.IsEnabled = true;
        //Se activa la Hora de Book
        cmbBookT.IsEnabled = true;

        //PR Contact
        if (!cmbPRContact.IsEnabled)
        {
          stkPRContact.IsEnabled = true;
          cmbPRContact.IsEnabled = true;
        }
        //PR
        stkPR.IsEnabled = true;
        cmbPR.IsEnabled = true;
        //Sala
        if (!cmbSalesRooms.IsEnabled)
        {
          stkSales.IsEnabled = true;
          cmbSalesRooms.IsEnabled = true;
        }
      }
    }

    #endregion Change

    #region Reschedule

    /// <summary>
    /// Este metodo se ejecuta desde el boton btnReschedule
    /// </summary>
    private void Reschedule()
    {
      //Deshabilitamos los controles de Change, Reschedule y Rebook
      btnChange.IsEnabled = false;
      btnReschedule.IsEnabled = false;
      btnRebook.IsEnabled = false;

      //SalesRoom
      if (!cmbSalesRooms.IsEnabled)
      {
        stkSales.IsEnabled = true;
        cmbSalesRooms.IsEnabled = true;
      }

      //Fecha de Reschedule
      stkRescheduleDate.IsEnabled = true;
      dtpRescheduleDate.IsEnabled = true;
      cmbReschT.IsEnabled = true;

      //Fecha y hora en que se hizo es reschedule
      dbContext.Guest.guReschDT = BRHelpers.GetServerDateTime();

      //Activamos el Check de reschedule
      chkReschedule.IsChecked = true;
    }

    #endregion Reschedule

    #region Rebook

    private void Rebook()
    {
      /*Nota: El proceso del Rebook consiste en la creacion de un nuevo Guest teniendo como guRef el ID del Guest anterior
       * este proceso se interpreta como la creacion de una nueva invitacion en un nuevo Guest que tiene como referencia
       * guRef = guID del padre.
       * */

      //Cambiamos el modo de la invitacion esto sirve para futuras validaciones
      dbContext.InvitationMode = EnumMode.Add;

      //Deshabilitamos los controles de Change, Reschedule y Rebook
      btnChange.IsEnabled = false;
      btnReschedule.IsEnabled = false;
      btnRebook.IsEnabled = false;

      //Si guRef es null o cero, SI NO le dejamos el guRef existente
      if (dbContext.Guest.guRef == null || dbContext.Guest.guRef == 0)
      {
        //Le asignamos el valor del GuestID
        dbContext.Guest.guRef = dbContext.Guest.guID;
      }
      //Limpiamos la informacion del Guest ID
      dbContext.Guest.guID = 0;

      //Desactivamos Quinella
      dbContext.Guest.guQuinella = false;
      //Desactivamos Show
      dbContext.Guest.guShow = false;
      //Limpiamos la informacion del show
      if (dbContext.Guest.guShowD != null)
      {
        dbContext.Guest.guShowD = null;
      }

      //Booking

      //PR Contact
      dbContext.Guest.guPRInfo = "";

      //PR
      dbContext.Guest.guPRInvit1 = "";

      //Fecha y Hora del Booking
      dbContext.Guest.guBookD = null;
      dbContext.Guest.guBookT = null;

      if (dbContext.Guest.guReschD != null || dbContext.Guest.guReschD != DateTime.MinValue)
      {
        //Limpiamos fecha y hora Reschedule
        dbContext.Guest.guReschD = null;
        dbContext.Guest.guReschT = null;

        //Limpiamos fecha y hora en que se hizo el reschedule
        dbContext.Guest.guReschDT = null;
      }

      if (_module == EnumModule.InHouse)
      {
        //No directa
        dbContext.Guest.guDirect = false;

        //Invitacion No cancelada
        dbContext.Guest.guBookCanc = false;

        //Contactacion

        //Fecha de contacto
        dbContext.Guest.guInfoD = null;

      }

      //Depositos
      dbContext.Guest.guDeposit = 0;
      dbContext.Guest.guDepositTwisted = 0;
      dbContext.Guest.guHotel = "";

      //Regalos
      dbContext.InvitationGiftList.Clear();


      //Numero de habitaciones
      if (dbContext.Guest.guRoomsQty != 0)
      {
        dbContext.Guest.guRoomsQty = 1;
      }

      //Reset DataContext
      var context = DataContext;
      DataContext = null;
      DataContext = context;

      dbContext.LoadInvitationInfo();

      StarModeControls();

      ControlsConfiguration();

    }
    #endregion Rebook

    #region ValidateExist
    /// <summary>
    /// Valida que los datos sean correctos, consulta al USP_OR_ValidateInvitation
    /// </summary>
    /// <history>
    /// [erosado] 19/08/2016  Created.
    /// </history>
    /// <returns>True isValid | False No</returns>
    private async Task<bool> ValidateExist()
    {
      bool isValid = true;
      var result = await BRGuests.ValidateInvitation(_user.User.peID, dbContext.Guest.guPRInvit1, dbContext.Guest.guloInvit, dbContext.Guest.guls, dbContext.Guest.gusr, dbContext.Guest.guag, dbContext.Guest.guco);

      if (result.Any())
      {
        //Recorremos el resultado en busca de algun error
        result.ForEach(x =>
        {
          if (!string.IsNullOrEmpty(x.Focus))
          {
            isValid = false;

            switch (x.Focus)
            {
              case "ChangedBy":
                break;
              case "PR":
                cmbPR.Focus();
                break;
              case "SalesRoom":
                cmbSalesRooms.Focus();
                break;
              case "Location":
                cmbLocation.Focus();
                break;
              case "Agency":
                cmbOtherInfoAgency.Focus();
                break;
              case "Country":
                cmbOtherInfoCountry.Focus();
                break;
            }
            UIHelper.ShowMessage(x.Message);
            tabGeneral.IsSelected = true;
          }
        });
      }
      return isValid;
    }
    #endregion

    #region InfoValidation
    /// <summary>
    /// Valida la informacion completa de la invitacion antesde de guardar la invitacion
    /// </summary>
    /// <returns>True Valido | False No</returns>
    /// <history>
    /// [erosado] 10/10/2016  Created.
    /// </history>
    private async Task<bool> InfoValidation()
    {
      //Mensaje en la barra de estado
      _statusBarInfo?.Invoke(true, "Validating Data...");
      //Validamos controles comunes y validaciones basicas
      if (!await InvitationValidationRules.ValidateGeneral(this, dbContext)) return false;
      //Validamos Grids
      if (!InvitationValidationRules.ValidateInformationGrids(this, dbContext)) return false;
      //Validamos Grid Guest
      if (!ValidateAdditionalGuest()) return false;
      //Validamos que la informacion exista
      return await ValidateExist();
    }
    #endregion

    #region SaveInvitation
    /// <summary>
    /// Guarda la invitacion
    /// </summary>
    /// <history>
    /// [erosado] 10/10/2016  Created.
    /// </history>
    private async Task SaveInvitation()
    {
      //Mensaje en la barra de estado 
      //WaitMessage(true, "Saving data ...");
      _statusBarInfo?.Invoke(true, "Saving Data...");
      //Obtenemos la informacion completa de la invitacion 
      var guestInvitation = dbContext as GuestInvitation;
      //Obtenemos el atributo hoursDiff
      var hoursDiff = _module != EnumModule.Host ? _user.LeadSource.lsHoursDif : _user.SalesRoom.srHoursDif;
      //Mandamos a guardar la informacion
      await BRGuests.SaveGuestInvitation(guestInvitation, dbContext.Program, _module, _user, dbContext.InvitationMode,
          ComputerHelper.GetMachineName(), ComputerHelper.GetIpMachine(), EnumGuestsMovementsType.Booking, hoursDiff);
      //Si la informacion se guardo correctamente enviamos un mensaje
      UIHelper.ShowMessage("The data was saved successfully");
      //Si es del modulo OutHouse y el tipo de invitacion es NewOutHouse, NO cerramos la ventana, solo la reiniciamos
      if (_module == EnumModule.OutHouse && _invitationType == EnumInvitationType.newOutHouse)
      {
        //WaitMessage(true, "Please wait, we are preparing the invitation form...");
        _statusBarInfo?.Invoke(true, "Please wait, we are preparing the invitation form...");
        //Volvemos a cargar la invitacion
        DataContext = null;
        UpdateLayout();
        dbContext = new GuestInvitationRules(_module, _invitationType, _user, _guestId);
        DataContext = dbContext;
        await dbContext.LoadAll();
      }
      else
      {
        SaveGuestInvitation = true;
        Close();
      }
    }
    #endregion

    #region StatusBarInfo
    /// <summary>
    /// Se encarga de actualizar la informacion del status Bar
    /// </summary>
    /// <param name="show">true Muestra el icono</param>
    /// <param name="message">Mensaje que aparecera en la barra de estado</param>
    /// <history>
    /// [erosado] 10/10/2016  Created.
    /// </history>
    private void StatusBarInfo(bool show, string message = "")
    {
      if (!Dispatcher.CheckAccess())
      {
        Dispatcher.Invoke(new statusBarInfo(StatusBarInfo), show, message);
      }
      else
      {
        lblStatusBarMessage.Text = message;
        imgStatusBarMessage.Visibility = (show) ? Visibility.Visible : Visibility.Hidden;
        Mouse.OverrideCursor = (show) ? Cursors.Wait : null;
        lblStatusBarMessage.UpdateLayout();
        imgStatusBarMessage.UpdateLayout();
        UIHelper.ForceUIToUpdate();
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
    /// [erosado] 08/08/2016  Created.
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

          //Si Paso las validaciones
          if (!InvitationValidationRules.ValidateEdit(ref invitationGift, ref _IGCurrentCell))
          {

            InvitationValidationRules.AfterEdit(_guestId, dtgGifts, ref invitationGift, _IGCurrentCell, ref txtGiftTotalCost, ref txtGiftTotalPrice, ref txtGiftMaxAuth, cmbGuestStatus.SelectedItem as GuestStatusType, dbContext.Program);
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
    /// [erosado] 02/08/2016  Created.
    /// </history>
    public void dtgGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      DataGrid dtg = sender as DataGrid;
      InvitationGift invitationGift = e.Row.Item as InvitationGift;

      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCancel)
        {
          dtg.RowEditEnding -= dtgGifts_RowEditEnding;
          dtg.CancelEdit();
          dtg.RowEditEnding += dtgGifts_RowEditEnding;
        }
        else
        {
          if (invitationGift?.igQty == 0 || string.IsNullOrEmpty(invitationGift?.iggi))
          {
            if ((UIHelper.ShowMessage("Please enter the required fields Qty and Gift to add a new gift. Do you want to keep adding the gift?", MessageBoxImage.Question) == MessageBoxResult.Yes))
            {
              e.Cancel = true;
            }
            else
            {
              dtg.RowEditEnding -= dtgGifts_RowEditEnding;
              dtg.CancelEdit();
              dtg.RowEditEnding += dtgGifts_RowEditEnding;
            }
          }
        }
      }
    }

    #endregion RowEditEnding

    #endregion Eventos del GRID Invitation Gift

    #region GuestAdditional

    #region Eventos del GRID GuestAdditional

    #region BeginningEdit

    /// <summary>
    /// Se ejecuta antes de que entre en modo edicion alguna celda
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private void dtgGuestAdditional_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (cmbSalesRooms.SelectedIndex == -1 && e.Column.SortMemberPath == "guID")
      {
        UIHelper.ShowMessage("First select a Sales Room ", MessageBoxImage.Warning, "Intelligence Marketing");
        e.Cancel = true;
        tabGeneral.IsSelected = true;
        tabGeneral.UpdateLayout();
        cmbSalesRooms.Focus();
      }
      else if (dbContext != null && !dbContext.Guest.guQuinella)
      {
        UIHelper.ShowMessage("Invitations that are not Quinellas can not have additional guests.", title: "Intelligence Marketing");
        e.Cancel = true;
      }
      else if (!GridHelper.IsInEditMode(dtgGuestAdditional) && !_hasError)
      {
        _IGCurrentCell = dtgGuestAdditional.CurrentCell;
        e.Cancel = InvitationValidationRules.dtgGuestAdditional_StartEdit(ref _IGCurrentCell, dtgGuestAdditional, ref _hasError);
      }
    }

    #endregion BeginningEdit

    #region CellEditEnding

    /// <summary>
    /// Se ejecuta cuando la celda en edicion pierde el foco
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private void dtgGuestAdditional_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        _isCellCommitGuestAdditional = (Keyboard.IsKeyDown(Key.Enter));
        Guest guestAdditionalRow = dbContext.AdditionalGuestList[e.Row.GetIndex()];
        Guest guestAdditional = AsyncHelper.RunSync(() => BRGuests.GetGuest(guestAdditionalRow?.guID ?? 0));
        var notValid = AsyncHelper.RunSync(() => InvitationValidationRules.dtgGuestAdditional_ValidateEdit(dbContext.Guest, guestAdditional, _IGCurrentCell, dbContext.Program));
        if (!notValid)
        {
          guestAdditionalRow.guFirstName1 = guestAdditional.guFirstName1;
          guestAdditionalRow.guLastName1 = guestAdditional.guLastName1;
          guestAdditionalRow.guCheckIn = guestAdditional.guCheckIn;
          guestAdditionalRow.guRef = guestAdditional.guRef;
          GridHelper.UpdateCellsFromARow(dtgGuestAdditional);
        }
        else
        {
          e.Cancel = true;
        }
      }
    }

    #endregion CellEditEnding

    #region RowEditEnding

    /// <summary>
    /// Se ejecuta cuando la fila pierde el foco, o termina la edicion (Commit o Cancel)
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/08/2016  Created.
    /// </history>
    public void dtgGuestAdditional_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCommitGuestAdditional && e.Row.GetIndex() == dtgGuestAdditional.ItemsSource.OfType<object>().ToList().Count)
        {
          _isCellCommitGuestAdditional = false;
          e.Cancel = true;
        }
        else if (Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.Tab))
        {
          int columnIndex = 0;
          _isCellCommitGuestAdditional = false;
          e.Cancel = !AsyncHelper.RunSync(() => InvitationValidationRules.ValidateAdditionalGuest(dbContext.Guest, (Guest)e.Row.Item, dbContext.Program, true)).Item1;
          if (e.Cancel)
          {
            _isCellCommitGuestAdditional = true;//true para que no haga el commit
            GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), columnIndex, true);
          }
        }
        else//Cancela el commit de la fila
        {
          e.Cancel = true;
        }
      }
    }

    #endregion RowEditEnding

    #region ValidateAdditionalGuest
    /// <summary>
    /// Valida el grid de GuestAdditional antes de guardar
    /// Verifica que no haya ni un registro en modo edición
    /// </summary>
    /// <param name="form">Formulario de invitación</param>
    /// <returns>True. Es valido | false. No es valido</returns>
    /// <history>
    /// [edgrodriguez] 07/10/2016 created
    /// </history>
    private bool ValidateAdditionalGuest()
    {
      bool isValid = true;
      //Validar que ya se haya salido del modo edición del Grid de Booking Deposits
      DataGridRow row = GridHelper.GetRowEditing(dtgGuestAdditional);
      if (row != null)
      {
        bool gridvalid = AsyncHelper.RunSync(() => InvitationValidationRules.ValidateAdditionalGuest(dbContext.Guest, row.Item as Guest, dbContext.Program, true)).Item1;
        if (gridvalid)
        {
          dtgGuestAdditional.RowEditEnding -= dtgGuestAdditional_RowEditEnding;
          dtgGuestAdditional.CommitEdit();
          dtgGuestAdditional.RowEditEnding += dtgGuestAdditional_RowEditEnding;
        }
        else
        {
          isValid = false;
          GridHelper.SelectRow(dtgGuestAdditional, row.GetIndex(), 0, true);
          tabStatusGiftsOthers.IsSelected = true;
        }
      }
      return isValid;
    }
    #endregion

    #endregion Eventos del GRID GuestAdditional

    #region btnSearchGuestAdditional_Click

    /// <summary>
    /// Abre la ventana SearchGuest
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private async void btnSearchGuestAdditional_Click(object sender, RoutedEventArgs e)
    {
      if (dbContext != null && !dbContext.Guest.guQuinella)
      {
        UIHelper.ShowMessage("Invitations that are not Quinellas can not have additional guests.", title: "Intelligence Marketing");
        return;
      }
      frmSearchGuest frmSrchGu = new frmSearchGuest(_user, _module == EnumModule.InHouse ? EnumProgram.Inhouse : EnumProgram.Outhouse)
      {
        Owner = this
      };
      frmSrchGu.ShowDialog();
      //Recuperar lista de guests e insertarlas en la lista de GuestAdditionals.
      var guestAdditionalList = frmSrchGu.lstGuestAdd ?? new List<Guest>();
      if (guestAdditionalList.Any())
      {
        List<string> lstMsg = new List<string>();
        foreach (var ga in guestAdditionalList)
        {
          //Si la invitacion esta en modo ReadOnly y el ID del guestadditional es igual al guest principal
          //O si el guestadditional ya tiene una invitacion.Ya no se agrega a la lista.
          var validate = await InvitationValidationRules.ValidateAdditionalGuest(dbContext.Guest, ga, dbContext.Program);
          if (!validate.Item1) { lstMsg.Add($"Guest ID: {ga.guID} \t{validate.Item2}"); continue; }
          if (validate.Item1 && dbContext.AdditionalGuestList.Any(c => c.guID == ga.guID)) { lstMsg.Add($"Guest ID: {ga.guID} \tIt is already in the list."); continue; }
          dbContext.AdditionalGuestList.Add(ga);
        };

        if (lstMsg.Any())
        {
          UIHelper.ShowMessage(string.Join("\n", lstMsg));
        }
      }
    }

    #endregion btnSearchGuestAdditional_Click

    #region guestDetails_Click

    /// <summary>
    /// Abre la ventana Guest, para mostrar la informacion.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private void guestDetails_Click(object sender, RoutedEventArgs e)
    {
      if (dtgGuestAdditional.Items.CurrentPosition == -1) return;

      var guest = dtgGuestAdditional.Items[dtgGuestAdditional.Items.CurrentPosition] as Guest;
      if (guest == null || guest.guID == 0) return;
      if (dbContext != null && string.IsNullOrWhiteSpace(dbContext.Guest.guls))
      {
        UIHelper.ShowMessage("Specify the Lead Source", title: "Intelligence Marketing");
        return;
      }
      if (dbContext != null && string.IsNullOrWhiteSpace(dbContext.Guest.gusr))
      {
        UIHelper.ShowMessage("Specify the Sales Room", title: "Intelligence Marketing");
        return;
      }
      if (dbContext != null && !dbContext.Guest.guQuinella)
      {
        UIHelper.ShowMessage("Invitations that are not Quinellas can not have additional guests.", title: "Intelligence Marketing");
        return;
      }
      if (_user.Permissions.Exists(c => c.pppm == IM.Model.Helpers.EnumToListHelper.GetEnumDescription((_module == EnumModule.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations)) && c.pppl <= 0))
        guestFormMode = EnumMode.ReadOnly;

      frmGuest frmGuest = new frmGuest(_user, guest.guID, _module, dbContext.Program, guestFormMode, true) { GuestParent = dbContext?.Guest, Owner = this };
      frmGuest.ShowDialog();
    }

    #endregion guestDetails_Click

    #region btnAddGuestAdditional_OnClick

    /// <summary>
    /// Abre la ventana Guest, para crear el nuevo guest adicional.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private async void BtnAddGuestAdditional_OnClick(object sender, RoutedEventArgs e)
    {
      if (dbContext != null && string.IsNullOrWhiteSpace(dbContext.Guest.guls))
      {
        UIHelper.ShowMessage("Specify the Lead Source", title: "Intelligence Marketing");
        return;
      }
      if (dbContext != null && string.IsNullOrWhiteSpace(dbContext.Guest.gusr))
      {
        UIHelper.ShowMessage("Specify the Sales Room", title: "Intelligence Marketing");
        return;
      }
      if (dbContext != null && !dbContext.Guest.guQuinella)
      {
        UIHelper.ShowMessage("Invitations that are not Quinellas can not have additional guests.", title: "Intelligence Marketing");
        return;
      }
      if (_user.Permissions.Exists(c => c.pppm == IM.Model.Helpers.EnumToListHelper.GetEnumDescription((_module == EnumModule.Host ? EnumPermission.HostInvitations : EnumPermission.PRInvitations)) && c.pppl == 0))
        guestFormMode = EnumMode.ReadOnly;
      else
        guestFormMode = EnumMode.Add;

      frmGuest frmGuest = new frmGuest(_user, 0, _module, dbContext.Program, guestFormMode, true) { GuestParent = dbContext?.Guest, Owner = this };
      frmGuest.ShowDialog();
      if (frmGuest.DialogResult.Value)
      {
        //Validacion del nuevo guest.
        //Recuperar lista de guests e insertarlas en la lista de GuestAdditionals.
        var guestAdditional = frmGuest.NewGuest ?? new Guest();
        if (guestAdditional.guID == 0) return;
        //Si la invitacion esta en modo ReadOnly y el ID del guestadditional es igual al guest principal
        //O si el guestadditional ya tiene una invitacion.Ya no se agrega a la lista.
        var validate = await InvitationValidationRules.ValidateAdditionalGuest(dbContext?.Guest, guestAdditional, dbContext.Program, true);
        if (validate.Item1)
          dbContext?.AdditionalGuestList.Add(guestAdditional);
      }
    }

    #endregion btnAddGuestAdditional_OnClick

    #endregion GuestAdditional

    #region Datagrid Boking Deposits

    #region BeginningEdit
    /// <summary>
    /// Valida que no se puedan habilitar mas de una fila
    /// </summary>
    /// <history>
    /// [emoguel] 19/08/2016
    /// </history>
    private void dtgBookingDeposits_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (!GridHelper.IsInEditMode(dtgBookingDeposits))
      {
        e.Cancel = !InvitationValidationRules.StartEditBookingDeposits(e.Column.SortMemberPath, e.Row.Item as BookingDeposit, true);
      }
      else
      {
        e.Cancel = true;
      }
    }

    #endregion BeginningEdit

    #region CellEditEnding
    /// <summary>
    /// Valida que no se le haga commit a la celda si el dato es erroneo
    /// </summary>
    /// <history>
    /// [emoguel] 17/08/2016 created
    /// </history>
    private void dtgBookingDeposits_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        string otherColumn = string.Empty;
        _isCellCommitDeposit = (Keyboard.IsKeyDown(Key.Enter));
        if (!InvitationValidationRules.ValidateEditBookingDeposit(e.Column.SortMemberPath, e.Row.Item as BookingDeposit, dtgBookingDeposits, e.EditingElement as Control, dbContext.CloneBookingDepositList, dbContext.Guest.guID,ref otherColumn))
        {
          if (dtgBookingDeposits.CurrentColumn != null && e.Column.DisplayIndex != dtgBookingDeposits.CurrentColumn.DisplayIndex)//Validamos si la columna validada es diferente a la seleccionada
          {
            //Regresamos el foco a la columna con el dato mal
            dtgBookingDeposits.CellEditEnding -= dtgBookingDeposits_CellEditEnding;
            dtgBookingDeposits.CurrentCell = new DataGridCellInfo(e.Row.Item, dtgBookingDeposits.Columns[0]);
            //GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), e.Column.DisplayIndex, true);
            dtgBookingDeposits.CellEditEnding += dtgBookingDeposits_CellEditEnding;
          }
          else
          {
            //Cancelamos el commit de la celda
            e.Cancel = true;
          }
        }
      }
    }
    #endregion

    #region RowEditEnding
    /// <summary>
    /// Valida que no se haga commit la fila si hay datos erroneos
    /// </summary>
    /// <history>
    /// [emoguel] 17/08/2016 created
    /// </history>
    public void dtgBookingDeposits_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCommitDeposit && e.Row.GetIndex() == dtgBookingDeposits.ItemsSource.OfType<object>().ToList().Count)//Verificar si es un registro nuevo
        {
          _isCellCommitDeposit = false;
          e.Cancel = true;
        }
        else if (Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.Tab))//Si fue commit con el enter desde el la fila
        {
          int columnIndex = 0;
          _isCellCommitDeposit = false;
          e.Cancel = !InvitationValidationRules.EndingEditBookingDeposits(e.Row.Item as BookingDeposit, sender as DataGrid, dbContext.CloneBookingDepositList, dbContext.Guest.guID, ref columnIndex);
          if (e.Cancel)
          {
            _isCellCommitDeposit = true;//true para que no haga el commit
            GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), columnIndex, true);
          }
        }
        else//Cancela el commit de la fila
        {
          e.Cancel = true;
        }
      }
      else
      {
        BookingDeposit bookingDeposit = e.Row.Item as BookingDeposit;
        if (bookingDeposit.bdID > 0)
        {          
          ObjectHelper.CopyProperties(bookingDeposit, dbContext.CloneBookingDepositList.FirstOrDefault(bd => bd.bdID == bookingDeposit.bdID));
        }
      }
    }
    #endregion

    #region IsReaOnlyBookingDeposits
    /// <summary>
    /// indica si el grid sólo va a ser lectura
    /// </summary>
    /// <returns>True. Va a ser modo lectura | False. Se puede editar</returns>
    /// <history>
    /// [emoguel] 12/08/2016 created 
    /// </history>
    public bool IsReaOnlyBookingDeposits()
    {
      //Validar si se está editando
      if (dbContext.InvitationMode == EnumMode.Edit || dbContext.InvitationMode == EnumMode.Add)
      {
        if (_module != EnumModule.OutHouse)//Validar que no sea Outhouse
        {
          bool blnInvitations = (_module == EnumModule.Host) ? _user.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.Special) : _user.HasPermission(EnumPermission.PRInvitations, EnumPermisionLevel.Special);
          // si la fecha de salida es hoy o despues y (es una invitacion nueva o la fecha de invitacion es hoy o
          // (tiene permiso especial de invitaciones y la fecha de booking original Mayor o igual a hoy))
          if (!(dbContext.Guest.guCheckOutD >= DateTime.Now && (dbContext.InvitationMode == EnumMode.Add || dbContext.Guest.guInvitD == DateTime.Now || (blnInvitations && dbContext.Guest.guBookD >= DateTime.Now))))
          {
            return true;
          }
        }

      }
      return false;
    }
    #endregion
    #endregion

    #region Eventos del Grid Guest Credit Card 

    #region dtgCCCompany_RowEditEnding
    /// <summary>
    /// Valida que esten completos los Row si al terminar la edicion quedo algo nulo la elimina 
    /// </summary>
    /// <history>
    /// [jorcanche] created 17/ago/2016
    /// </history> 
    private void dtgCCCompany_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCommitCC && e.Row.GetIndex() == dtgCCCompany.ItemsSource.OfType<object>().ToList().Count)
        {
          _isCellCommitCC = false;
          e.Cancel = true;
        }
        else if (Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.Tab))
        {
          int columnIndex = 0;
          _isCellCommitCC = false;
          GuestCreditCard guestCreditCard = e.Row.Item as GuestCreditCard;
          if (guestCreditCard != null && guestCreditCard.gdQuantity == 0)
          {
            e.Cancel = true;
            columnIndex = dtgCCCompany.Columns.FirstOrDefault(cl => cl.SortMemberPath == "gdQuantity").DisplayIndex;
          }
          else if (string.IsNullOrWhiteSpace(guestCreditCard.gdcc))
          {
            columnIndex = dtgCCCompany.Columns.FirstOrDefault(cl => cl.SortMemberPath == "gdcc").DisplayIndex;
            e.Cancel = true;
          }
          if (e.Cancel)
          {
            _isCellCommitCC = true;//true para que no haga el commit
            GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), columnIndex, true);
          }
        }
        else
        {
          e.Cancel = true;
        }

      }
    }
    #endregion

    #region dtgCCCompany_BeginningEdit
    /// <summary>
    /// Bloquea la opcion de crear un nuevo registro a menos que se le haya hecho commit a un regristro
    /// No deja habilitar el combobox de creditCard
    /// </summary>
    /// <history>
    /// [emoguel] 19/08/2016 created
    /// </history>
    private void dtgCCCompany_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (!GridHelper.IsInEditMode(dtgBookingDeposits))
      {
        if (e.Column.SortMemberPath == "gdcc")
        {
          GuestCreditCard guestCreditCard = e.Row.Item as GuestCreditCard;
          if (guestCreditCard?.gdQuantity <= 0)
          {
            UIHelper.ShowMessage("Enter the quantity first");
            e.Cancel = true;
          }
        }
      }
      else
      {
        e.Cancel = true;
      }
    }
    #endregion

    #region dtgCCCompany_CellEditEnding
    /// <summary>
    /// Verificar que el valor insertado en la columna sea un valor valido
    /// </summary>
    /// <history>
    /// [emoguel] 19/08/2016 created
    /// </history>
    private void dtgCCCompany_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        _isCellCommitCC = (Keyboard.IsKeyDown(Key.Enter));
        GuestCreditCard guestCreditCard = e.Row.Item as GuestCreditCard;
        switch (e.Column.SortMemberPath)
        {
          case "gdQuantity":
            {
              if (guestCreditCard?.gdQuantity == 0)
              {
                UIHelper.ShowMessage("Quantity can not be 0.");
                e.Cancel = true;
              }
              break;
            }
          case "gdcc":
            {
              e.Cancel = GridHelper.HasRepeatItem(e.EditingElement as Control, dtgCCCompany, false, "gdcc");
              break;
            }
        }
        if (e.Cancel)
        {
          //Regresamos el foco a la columna con el dato mal
          dtgCCCompany.CellEditEnding -= dtgCCCompany_CellEditEnding;
          GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), e.Column.DisplayIndex, true);
          dtgCCCompany.CellEditEnding += dtgCCCompany_CellEditEnding;
        }
      }
    }

    #endregion

    #endregion   
  }
}