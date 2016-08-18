﻿using System.Windows;
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
using System.Threading.Tasks;
using IM.Services.WirePRService;
using System.Runtime.CompilerServices;
using System.Linq;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmInvitation.xaml
  /// </summary>
  public partial class frmInvitation : Window, INotifyPropertyChanged
  {
    #region Propiedades, Atributos
    //Parametros del constructor
    public readonly EnumModule _module;
    public readonly EnumInvitationType _invitationType;
    public UserData _user;
    private int _guestId;
    public readonly bool _allowReschedule;
    public bool _isEditing = false;
    //Grids Banderas
    private DataGridCellInfo _IGCurrentCell;//Celda que se esta modificando
    private bool _hasError = false; //Sirve para las validaciones True hubo Error | False NO
    private bool _isCellCancel = false;//Sirve para cuando se cancela la edicion de una Celda
    private bool _dontShowAgainGuestStatus = false;
    public bool _isRebook = false;
    public GuestInvitationRules catObj { get; set; }
    private bool _isCellCommitDeposit = false;//Valida si el commit se hace desde la celda

  
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #region SetField
    /// <summary>
    /// Sirve para setear valores a una propiedad, implementa INotifyPropertyChanged
    /// Si el nuevo valor es diferente del que ya tenia asignado Se lo asigna.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="field">ref _atributo o propiedad</param>
    /// <param name="value">value</param>
    /// <param name="propertyName">Nombre de la propiedad</param>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// </history>
    public void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value)) return;
      field = value;
      OnPropertyChanged(propertyName);
    }
    #endregion
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
      catObj = new GuestInvitationRules(module, invitationType, user, guestId);
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
        //Iniciamos el BusyIndicator
        _busyIndicator.IsBusy = true;
        _busyIndicator.BusyContent = "Please wait, we are preparing the invitation form...";
        //Cargamos la informacion
        await catObj.LoadAll();
        //Cargamos la UI dependiendo del tipo de Invitacion
        ControlsConfiguration();
        //Configuramos los controles (Maxlength, caracteres etc.)
        UIHelper.SetUpControls(new Guest(), this);
        //Detenemos el BusyIndicator
        _busyIndicator.IsBusy = false;

        GridHelper.SetUpGrid(dtgBookingDeposits, new BookingDeposit());
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
        _busyIndicator.IsBusy = false;
      }
    }
    #endregion

    #region imgButtonSave_MouseLeftButtonDown
    /// <summary>
    /// Evento que se genera al presionar el boton Save
    /// </summary>
    ///<history>
    ///[erosado]  17/08/2016  Created.
    /// </history>
    private void imgButtonSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      bool _isValid = true;

      //Validamos controles comunes y validaciones basicas
      if (!InvitationValidationRules.ValidateGeneral(this, catObj))
      {
        _isValid = false;
        tabGeneral.TabIndex = 0;
      }
      //Si paso la primer validacion, validamos los grids invitsGift, bookingDeposits, creditCard, additionalGuest
      if (_isValid)
      {
        _isValid = InvitationValidationRules.ValidateInformationGrids(this, catObj);
      }

    }
    #endregion

    #region imgButtonEdit_MouseLeftButtonDown
    /// <summary>
    /// Evento que se genera al presionar el boton Edit
    /// </summary>
    ///<history>
    ///[erosado]  17/08/2016  Created.
    /// </history>
    private void imgButtonEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      //Si el Guest ya hizo Show No podemos editar nada.
      if (catObj.Guest.guShow)
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
      }
    }
    #endregion

    #region imgButtonPrint_MouseLeftButtonDown
    /// <summary>
    /// Evento que se genera al presionar el boton Print
    /// </summary>
    ///<history>
    ///[erosado]  17/08/2016  Created.
    /// </history>
    private void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      //Generamos Reporte
      RptInvitationHelper.RptInvitation(_guestId, _user.User.peID);
    }
    #endregion

    #region imgButtonCancel_MouseLeftButtonDown
    /// <summary>
    /// Sirve para cancelar la edicion, y si no esta en modo edicion cierra el formulario de invitacion
    /// </summary>
    /// <history>
    /// [erosado] 11/08/2016  Created.
    /// </history>
    private async void imgButtonCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
        catObj = new GuestInvitationRules(_module, _invitationType, _user, _guestId);
        await catObj.LoadAll();
        DataContext = catObj;
        SetReadOnly();

        //Habilitamos los botones editar e imprimir
        imgButtonEdit.IsEnabled = true;
        imgButtonPrint.IsEnabled = true;
        imgButtonSave.IsEnabled = false;
        _busyIndicator.IsBusy = false;
      }
      //Si no estabamos editando cerramos la aplicacion
      else
      {
        Close();
      }
    }
    #endregion

    #region imgButtonLog_MouseLeftButtonDown
    /// <summary>
    /// Abre el formulario de GuestLog se la pasa el guID, sirve para mostrar los movimientos que ha tenido el guID 
    /// </summary>
    /// <history>
    /// [erosado] 11/08/2016  Created.
    /// </history>
    private void imgButtonLog_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      //Si viene de Host la invitacion le mandamos el SalesRoom, en lugar del leadsource, es solo informativo.
      frmGuestLog frmGuestLog = new frmGuestLog(_guestId);
      frmGuestLog.Owner = this;
      frmGuestLog.ShowDialog();
    }
    #endregion

    #region imgButtonReLogin_MouseLeftButtonDown
    /// <summary>
    /// Permite Re-Login en la invitacion
    /// </summary>
    /// <history>
    /// [erosado] 15/08/2016  Modified
    /// </history>
    private async void imgButtonReLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var login = new frmLogin(loginType: EnumLoginType.Location, program: catObj.Program,
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
        catObj = new GuestInvitationRules(_module, _invitationType, _user, _guestId);
        DataContext = catObj;
        //Configuramos de nuevo todo
        Window_Loaded(this, null);
      }
    }
    #endregion

    #region brdSearchButton_MouseLeftButtonDown
    /// <summary>
    /// Obtiene un Folio de Reservacion
    /// </summary>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// [vipacheco] 18/08/2016 Modified -> Se agregó la invocacion para la busqueda de huespedes por # de reservacion.
    /// </history>
    private void brdSearchButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
       frmSearchReservation search = new frmSearchReservation(_user) { Owner = this };

      // Verificamos si se selecciono un guest
      if (search.ShowDialog().Value)
      {
        //Seteamos la informacion de SearchGuest en nuestro objeto Guest
        SetRervationOrigosInfo(search._reservationInfo);

        catObj.SetRervationOrigosInfo(search._reservationInfo);

        UIHelper.UpdateTarget(this);
      }

    }
    #endregion

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
    #endregion

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
    #endregion

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
    #endregion

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

      txtGiftMaxAuth.Text = string.Format("{0:C2}", guStatusType != null ? guStatusType.gsMaxAuthGifts : 0);

      //TODO: GUESTSTATUSTYPES Revizar el caso cuando se traigan los regalos de la Base de datos
      //GuestStatus _guestsStatus = BRGuestStatus.GetGuestsStatus(_guestID);
      //GuestStatusType _guestStatusType = BRGuestStatusTypes.GetGuestStatusTypeByID(_guestsStatus.gtgs);
      //curMaxAuthGifts = _guestsStatus.gtQuantity * _guestStatusType.gsMaxAuthGifts;
    }
    #endregion

    #region cmbSalesRooms_SelectionChanged
    /// <summary>
    /// Obtiene la informacion de los tourTimes cada que se cambia de sala de ventas
    /// </summary>
    ///<history>
    ///[erosado]  16/08/2016  Created.
    /// </history>
    private void cmbSalesRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (dtpBookDate.Value.HasValue && dtpBookDate?.Value != DateTime.MinValue && cmbSalesRooms?.SelectedItem != null)
      {
        //Consultamos los horarios disponibles 
        catObj.TourTimes = LoadTourTimes(cmbSalesRooms.SelectedValue.ToString(), dtpBookDate.Value.Value);
      }
      if (dtpRescheduleDate.Value.HasValue && dtpRescheduleDate?.Value != DateTime.MinValue && cmbSalesRooms?.SelectedItem != null)
      {
        //Consultamos los horarios disponibles 
        catObj.TourTimes = LoadTourTimes(cmbSalesRooms.SelectedValue.ToString(), dtpRescheduleDate.Value.Value, false);
      }
    }
    #endregion

    #region dtpBookDate_ValueChanged
    /// <summary>
    /// Obtiene la informacion de los tourTimes cada que se cambia la fecha de Book
    /// </summary>
    ///<history>
    ///[erosado]  16/08/2016  Created.
    /// </history>
    private void dtpBookDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      //Si tiene una fecha y es una fecha valida
      if (dtpBookDate.Value.HasValue && dtpBookDate?.Value != DateTime.MinValue && cmbSalesRooms?.SelectedItem != null)
      {
        //Consultamos los horarios disponibles 
        catObj.TourTimes = LoadTourTimes(cmbSalesRooms.SelectedValue.ToString(), dtpBookDate.Value.Value);
      }
    }
    #endregion

    #region dtpRescheduleDate_ValueChanged
    /// <summary>
    /// Obtiene la informacion de los tourTimes cada que se cambia la fecha de Reschedule
    /// </summary>
    ///<history>
    ///[erosado]  16/08/2016  Created.
    /// </history>
    private void dtpRescheduleDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      //Si tiene una fecha y es una fecha valida
      if (dtpRescheduleDate.Value.HasValue && dtpRescheduleDate?.Value != DateTime.MinValue && cmbSalesRooms?.SelectedItem != null)
      {
        //Consultamos los horarios disponibles 
        cmbBookT.ItemsSource = LoadTourTimes(cmbSalesRooms.SelectedValue.ToString(), dtpRescheduleDate.Value.Value, false);
      }
    }
    #endregion

    #endregion

    #region Controls Configuration

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
      if (catObj.InvitationMode != EnumMode.ReadOnly)
      {
        //Disable controls
        ControlBehaviorConfiguration();
      }

      //Si es modo ReadOnly y Modo Edit deshabilitamos los contenedores principales.
      if (catObj.InvitationMode == EnumMode.ReadOnly || catObj.InvitationMode == EnumMode.Edit)
      {
        SetReadOnly();
      }

      //Configuramos los controles de Change,Reschedule,Rebook
      SetupChangeRescheduleRebook();
    }
    #endregion

    #region  MenuBarConfiguration
    /// <summary>
    /// Activa o desactiva los botones de la barra de menu dependiendo del modo de la invitacion
    /// </summary>
    /// <history>
    /// [erosado] 10/08/2016  Created.
    /// </history>
    private void MenuBarConfiguration()
    {
      if (catObj.InvitationMode == EnumMode.Add)
      {
        imgButtonEdit.IsEnabled = false;
        imgButtonPrint.IsEnabled = false;
        imgButtonSave.IsEnabled = true;
        imgButtonCancel.IsEnabled = true;
        imgButtonLog.IsEnabled = _guestId != 0 ? true : false; ;
      }
      else if (catObj.InvitationMode == EnumMode.Edit)
      {
        imgButtonEdit.IsEnabled = true;
        imgButtonPrint.IsEnabled = true;
        imgButtonSave.IsEnabled = false;
        imgButtonCancel.IsEnabled = true;
        imgButtonLog.IsEnabled = true;
      }
      else if (catObj.InvitationMode == EnumMode.ReadOnly)
      {
        imgButtonEdit.IsEnabled = false;
        imgButtonPrint.IsEnabled = true;
        imgButtonSave.IsEnabled = false;
        imgButtonCancel.IsEnabled = false;
        imgButtonLog.IsEnabled = true;
      }
    }
    #endregion

    #endregion

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
        default:
          break;
      }
    }
    #endregion

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
      stkPRContact.Visibility = Visibility.Collapsed;
      stkFlightNumber.Visibility = Visibility.Collapsed;
    }
    #endregion

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
      brdSearchButton.Visibility = Visibility.Collapsed;
      stkRebookRef.Visibility = Visibility.Collapsed;
      btnReschedule.Visibility = Visibility.Collapsed;
      btnRebook.Visibility = Visibility.Collapsed;
      stkRescheduleDate.Visibility = Visibility.Collapsed;
      brdCreditCard.Visibility = Visibility.Collapsed;
      brdRoomsQtyAndElectronicPurse.Visibility = Visibility.Collapsed;
    }
    #endregion

    #endregion

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
        default:
          break;
      }
    }
    #endregion

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
      chkDirect.IsEnabled = _module == EnumModule.Host ? true : false;
      cmbLocation.IsEnabled = _module == EnumModule.Host ? true : false;
      cmbSalesRooms.IsEnabled = _module != EnumModule.Host ? true : false;
      stkRescheduleDate.IsEnabled = false;
      btnChange.IsEnabled = false;
      btnReschedule.IsEnabled = false;
      btnRebook.IsEnabled = false;
      //btnAddGuestAdditional.IsEnabled = catObj.InvitationMode != EnumMode.ReadOnly;
      //btnSearchGuestAdditional.IsEnabled = catObj.InvitationMode != EnumMode.ReadOnly;
      #endregion

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
      dtgAdditionalGuest.IsReadOnly = catObj.InvitationMode == EnumMode.ReadOnly;

      #endregion

      //Si es una invitacion existente 
      if (catObj.InvitationMode != EnumMode.Add)
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

    #endregion

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
      btnChange.IsEnabled = false;
      #endregion

      #region IsReadOnly
      txtguID.IsReadOnly = true;
      dtpguInvitD.IsReadOnly = true;
      tpkguInvitT.IsReadOnly = true;
      txtguIdProfileOpera.IsReadOnly = true;
      txtguLastNameOriginal.IsReadOnly = true;
      txtguFirstNameOriginal.IsReadOnly = true;
      #endregion

      //Si OutHouse y es una invitacion existente
      if (_module == EnumModule.OutHouse && catObj.InvitationMode != EnumMode.Add)
      {
        //Desactivamos los siguientes controles.
        stkPR.IsEnabled = false;
        stkLocation.IsEnabled = false;
        stkSales.IsEnabled = false;
        dtpBookDate.IsEnabled = false;
        cmbBookT.IsEnabled = false;
      }

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
      if (_allowReschedule)
      {
        stkRescheduleDate.IsEnabled = false;
      }

      //BOOKING DEPOSITS      
      //Si viene de InHouse o Host
      if (_module != EnumModule.OutHouse)
      {
        //Si el huesped no se ha ido, o la fecha en que se hizo la invitacion ya pasó o (no tiene permiso de invitacion  y la fecha de Booking es menor a la fecha de hoy)
        if (catObj.Guest.guCheckOutD <= serverDate || catObj.Guest.guInvitD != serverDate || (!_user.HasPermission(permission, EnumPermisionLevel.Special) || catObj.CloneGuest.guBookD < serverDate))
        {
          //No permitimos modificacion de depositos desactivamos todo el contenedor
          brdBookingDeposits.IsEnabled = false;
        }
      }

      //GUEST ADDITIONAL
      //Si no es de OutHouse ni Host(es InHouse)
      if (_module == EnumModule.InHouse)
      {
        //Si la fecha de booking original es antes de hoy
        if (catObj.CloneGuest.guBookD < serverDate)
        {
          brdAdditionalGuest.IsEnabled = false;
        }
      }

      //Other Info
      if (_module == EnumModule.InHouse)
      {
        //Si tiene copia de folio de reservacion, no se permite modificar la agencia
        if (!string.IsNullOrWhiteSpace(catObj.Guest.guHReservIDC))
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
        if (catObj.CloneGuest.guBookD < serverDate)
        {
          brdCreditCard.IsEnabled = false;
          cmbGuestStatus.IsEnabled = false;
          stkRoomsQty.IsEnabled = false;
        }
      }
    }
    #endregion

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
      brdGifts.IsEnabled = false;
      brdBookingDeposits.IsEnabled = false;
      brdCreditCard.IsEnabled = false;
      //Additional Guest
      dtgAdditionalGuest.IsReadOnly = true;

      brdRoomsQtyAndElectronicPurse.IsEnabled = false;
    }
    #endregion

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
      brdGifts.IsEnabled = true;
      brdBookingDeposits.IsEnabled = true;
      brdCreditCard.IsEnabled = true;
      //Additional Guest
      dtgAdditionalGuest.IsEnabled = true;
      brdRoomsQtyAndElectronicPurse.IsEnabled = true;

      //Si esta iniciando una edicion de invitacion desactivamos los siguientes controles
      if (_isEditing)
      {
        imgButtonEdit.IsEnabled = false;
        imgButtonPrint.IsEnabled = false;
        imgButtonSave.IsEnabled = true;
      }
    }
    #endregion

    #endregion

    #region Metodos Complementarios

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
      if (catObj.Guest.guCheckOutD >= serverDate || _user.HasPermission(permission, EnumPermisionLevel.Special))
      {
        //Si es una invitacion Nueva
        if (catObj.InvitationMode == EnumMode.Add)
        {
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = false;
          btnRebook.IsEnabled = false;
        }
        //Si tiene show
        else if (catObj.Guest.guShow)
        {
          //Si guANtesIO tiene es true no permitimos modificar el control chkguAntesIO
          if (catObj.Guest.guAntesIO)
          {
            chkguAntesIO.IsEnabled = false;
          }

          //Solo se permite modificar Rebook
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = false;
          btnRebook.IsEnabled = true;
        }
        //Si la fecha de invitacion es hoy y no es un reschedule
        else if (catObj.Guest.guInvitD == serverDate && !catObj.Guest.guResch)
        {
          //Solo se permite modificar Rebook
          btnChange.IsEnabled = true;
          btnReschedule.IsEnabled = false;
          btnRebook.IsEnabled = true;
        }
        //si la fecha de invitacion es hoy y es un reschedule
        else if (catObj.Guest.guInvitD == serverDate && catObj.Guest.guResch)
        {
          //No se permite cambiar
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = true;
          btnRebook.IsEnabled = true;
        }
        //Si la fecha de invitacion es antes de hoy y No tiene reschedule y la fecha de booking es despues de hoy 
        else if (catObj.Guest.guInvitD < serverDate && !catObj.Guest.guResch &&
          catObj.Guest.guBookD > serverDate && _user.HasPermission(permission, EnumPermisionLevel.Standard))
        {
          //No se permite Reschedule y Rebook
          btnChange.IsEnabled = true;
          btnReschedule.IsEnabled = false;
          btnRebook.IsEnabled = false;
        }
        //si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es hoy
        else if (catObj.Guest.guInvitD < serverDate && !catObj.Guest.guResch && catObj.Guest.guBookD == serverDate)
        {
          //se permite cambiar, reschedule y rebook
          btnChange.IsEnabled = true;
          btnReschedule.IsEnabled = true;
          btnRebook.IsEnabled = true;
        }
        //si la fecha de invitacion es antes de hoy y no es un reschedule y su fecha de booking es antes de hoy
        else if (catObj.Guest.guInvitD < serverDate && !catObj.Guest.guResch && catObj.Guest.guBookD < serverDate)
        {
          //No se permite cambiar
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = true;
          btnRebook.IsEnabled = true;
        }
        //si la fecha de invitacion es antes de hoy y es un rechedule
        else if (catObj.Guest.guInvitD < serverDate && catObj.Guest.guResch)
        {
          //No se permite cambiar
          btnChange.IsEnabled = false;
          btnReschedule.IsEnabled = true;
          btnRebook.IsEnabled = true;
        }
        //si NO se permite hacer reschedule de invitaciones
        if (!_allowReschedule)
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
    #endregion

    #region LoadTourTimes
    /// <summary>
    /// Sirve para cargar los TourTimes disponibles
    /// </summary>
    /// <param name="salesRoom">Sala de ventas</param>
    /// <param name="selectedDate">Dia seleccionado</param>
    /// <param name="bookingDate">True Booking|False Reschedule </param>
    /// <returns></returns>
    private List<TourTimeAvailable> LoadTourTimes(string salesRoom, DateTime selectedDate, bool bookingDate = true)
    {
      List<TourTimeAvailable> tourTimes = new List<TourTimeAvailable>();
      //Obtenemos el LeadSource
      var leadSource = _module != EnumModule.Host ? _user.LeadSource.lsID : catObj.Guest.guls;

      //Obtenemos la fecha del servidor
      var serverDate = BRHelpers.GetServerDate();

      //Si es una invitacion nueva
      if (catObj.InvitationMode == EnumMode.Add)
      {
        tourTimes = BRTourTimesAvailables.GetTourTimesAvailables(leadSource, salesRoom, selectedDate);
      }
      //Si es en modo edicion
      if (catObj.InvitationMode == EnumMode.Edit)
      {
        //Booking
        if (bookingDate)
        {
          tourTimes = BRTourTimesAvailables.GetTourTimesAvailables(leadSource, salesRoom, selectedDate,
            catObj.CloneGuest != null ? catObj.CloneGuest.guBookD : null,
            catObj.CloneGuest != null ? catObj.CloneGuest.guBookT : null, serverDate);
        }
        //Reschedule
        else
        {
          tourTimes = BRTourTimesAvailables.GetTourTimesAvailables(leadSource, salesRoom, selectedDate,
            catObj.CloneGuest != null ? catObj.CloneGuest.guReschD : null,
            catObj.CloneGuest != null ? catObj.CloneGuest.guReschT : null, serverDate);
        }
      }
      return tourTimes;
    }
    #endregion

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
      if (_allowReschedule)
      {
        //si la fecha de invitacion es hoy o si la fecha de booking es despues de hoy
        if (catObj.Guest.guInvitD == serverDate || catObj.Guest.guBookD > serverDate)
        {
          //Se activa la fecha de Book
          dtpBookDate.IsEnabled = true;

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
        //Fecha de Booking 
        stkBookDateAndTime.IsEnabled = true;

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

    #endregion

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

      //Fecha y hora en que se hizo es reschedule
      catObj.Guest.guReschDT = BRHelpers.GetServerDateTime();

      //Reschedule TourTimes
      cmbReschT.IsEnabled = true;

      //Activamos el Check de reschedule
      chkReschedule.IsChecked = true;

    }
    #endregion

    #region Rebook
    private void Rebook()
    {
      /*Nota: El proceso del Rebook consiste en la creacion de un nuevo Guest teniendo como guRef el ID del Guest anterior
       * este proceso se interpreta como la creacion de una nueva invitacion en un nuevo Guest que tiene como referencia
       * guRef = guID del padre.
       * */
      
      ////Encendemos la bandera indicando que es un Rebook
      //_isRebook = true;
      
      //Cambiamos el modo de la invitacion esto sirve para futuras validaciones
      catObj.InvitationMode = EnumMode.Add;

      //Deshabilitamos los controles de Change, Reschedule y Rebook
      btnChange.IsEnabled = false;
      btnReschedule.IsEnabled = false;
      btnRebook.IsEnabled = false;

      //Si guRef es null o cero, SI NO le dejamos el guRef existente
      if (catObj.Guest.guRef == null || catObj.Guest.guRef == 0)
      {
        //Le asignamos el valor del GuestID
        catObj.Guest.guRef = catObj.Guest.guID;
      }
      //Limpiamos la informacion del Guest ID
      catObj.Guest.guID = 0;
      
      //Desactivamos Quinella
      catObj.Guest.guQuinella = false;
      //Desactivamos Show
      catObj.Guest.guShow = false;
      //Limpiamos la informacion del show
      if (catObj.Guest.guShowD != null)
      {
        catObj.Guest.guShowD = null;
      }

      //Booking

      //PR Contact
      catObj.Guest.guPRInfo = "";

      //PR
      catObj.Guest.guPRInvit1 = "";

      //Fecha y Hora del Booking
      catObj.Guest.guBookD = null;
      catObj.Guest.guBookT = null;

      if (catObj.Guest.guReschD != null || catObj.Guest.guReschD != DateTime.MinValue)
      {
        //Limpiamos fecha y hora Reschedule
        catObj.Guest.guReschD = null;
        catObj.Guest.guReschT = null;

        //Limpiamos fecha y hora en que se hizo el reschedule
        catObj.Guest.guReschDT = null;
      }

      if (_module == EnumModule.InHouse)
      {
        //No directa
        catObj.Guest.guDirect = false;

        //Invitacion No cancelada
        catObj.Guest.guBookCanc = false;

        //Contactacion

        //Fecha de contacto
        catObj.Guest.guInfoD = null;
       
      }

      //Depositos
      catObj.Guest.guDeposit = 0;
      catObj.Guest.guDepositTwisted = 0;
      catObj.Guest.guHotel = "";

      //Regalos
      catObj.InvitationGiftList = new ObservableCollection<InvitationGift>();

      //Numero de habitaciones
      if (catObj.Guest.guRoomsQty != 0)
      {
        catObj.Guest.guRoomsQty = 1;
      }
    }
    #endregion

    #region GetSearchReservationInfo
    /// <summary>
    /// Asignamos los valores de ReservationOrigos a nuestro objeto Guest
    /// </summary>
    /// <param name="reservationOrigos">ReservationOrigos</param>
    /// <history>
    /// [erosado] 18/08/2016  Created.
    /// </history>
    private void SetRervationOrigosInfo(ReservationOrigos reservationOrigos)
    {
      //catObj = DataContext as GuestInvitation;

      //Asignamos el folio de reservacion
      catObj.Guest.guHReservID = reservationOrigos.Folio;
      catObj.Guest.guLastName1 = reservationOrigos.LastName;
      catObj.Guest.guFirstName1 = reservationOrigos.FirstName;
      catObj.Guest.guCheckInD = reservationOrigos.Arrival;
      catObj.Guest.guCheckOutD = reservationOrigos.Departure;
      catObj.Guest.guRoomNum = reservationOrigos.Room;
      //Calculamos Pax
      decimal pax = 0;
      bool convertPax = decimal.TryParse($"{reservationOrigos.Adults}.{reservationOrigos.Children}", out pax);
      catObj.Guest.guPax = pax;

      catObj.Guest.guco = reservationOrigos.Country;
      catObj.Guest.guag = reservationOrigos.Agency;
      catObj.Guest.guHotel = reservationOrigos.Hotel;
      catObj.Guest.guCompany = reservationOrigos.Company;
      catObj.Guest.guMembershipNum = reservationOrigos.Membership;

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
          //TODO:Revisar este metodo 
          //InvitationValidationRules.ValidateEdit(ref invitationGift, ref _hasErrorValidateEdit, ref _IGCurrentCell);

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

    #region Datagrid Boking Deposits
    #region BeginningEdit 
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
    #endregion

    #region CellEditEnding
    private void dtgBookingDeposits_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        _isCellCommitDeposit = (Keyboard.IsKeyDown(Key.Enter));
        e.Cancel = !InvitationValidationRules.validateEditBookingDeposit(e.Column.SortMemberPath, e.Row.Item as BookingDeposit, dtgBookingDeposits, e.EditingElement as Control, catObj.CloneBookingDepositList, catObj.Guest.guID);
        e.EditingElement.Focus();
      }
    }
    #endregion

    #region RowEditEnding
    private void dtgBookingDeposits_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCommitDeposit)
        {
          _isCellCommitDeposit = false;
          e.Cancel = true;
        }
        else if (Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.Tab))
        {
          _isCellCommitDeposit = false;
          e.Cancel = !InvitationValidationRules.AfertEditBookingDeposits(e.Row.Item as BookingDeposit, sender as DataGrid, catObj.CloneBookingDepositList, catObj.Guest.guID);
        }
        else
        {
          e.Cancel = true;
        }
      }
    }
    #endregion

    #endregion

  }
}
