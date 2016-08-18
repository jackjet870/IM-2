using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IM.Base.Classes
{
  public class GuestInvitationRules : EntityBase
  {
    private EnumModule _module;
    private EnumInvitationType _invitationType;
    private UserData _user;
    private int _guID;

    private GuestInvitation _guestInvitation;
    public GuestInvitation GuestInvitation
    {
      get { return _guestInvitation; }
      set { SetField(ref _guestInvitation, value); }
    }

    #region Constructor
    public GuestInvitationRules(EnumModule module, EnumInvitationType invitationType, UserData user, int guID = 0)
    {
      GuestInvitation = new GuestInvitation();
      _module = module;
      _invitationType = invitationType;
      _user = user;
      _guID = guID;
    }
    #endregion

    #region DefaultValues
    #region LoadAll
    /// <summary>
    /// Este metodo se encarga de cargar la  informacion necesaria para el formulario de invitaciones
    /// </summary>
    /// <returns>Task</returns>
    /// <history>
    /// [erosado] 11/08/2016  Created.
    /// </history>
    public async Task LoadAll()
    {
      await Task.WhenAll(
      //Cargamos la invitacion
      DefaultValueInvitation(_user, _guID),
      //Cargamos los catalogos comunes
      LoadLenguages(),
      LoadMaritalStatus(),
      LoadPersonnel(_user, _module),
      LoadHotels(),
      LoadAgencies(),
      LoadCountries(),
      LoadGuestStatusType(),
      LoadCurrencies(),
      LoadPaymentTypes(),
      LoadPaymentPlaces(),
      LoadCreditCardTypes(),
      LoadSalesRooms(),
      LoadLocations(_user, _module),
      LoadDisputeStatus(),
      LoadGifts(_user),
      LoadCloseDate(),
      LoadProgram(_module, _invitationType, _guID)
        );
      
    }
    #endregion

    #region LoadInvitationInfo
    /// <summary>
    /// Load Invitation Info es el mensaje que sale a lado del menu bar en la invitacion
    /// </summary>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// </history>
    private void LoadInvitationInfo()
    {
      string invitType = string.Empty;

      switch (_invitationType)
      {
        case EnumInvitationType.existing:
          invitType = "Existing Guest";
          break;
        case EnumInvitationType.newOutHouse:
          invitType = "New OutHouse";
          break;
        case EnumInvitationType.newExternal:
          invitType = "New External";
          break;
        default:
          break;
      }
      _guestInvitation.InvitationInfo = $"Mode: {EnumToListHelper.GetEnumDescription(GuestInvitation.InvitationMode)} | Module: {_module} | Invitation Type: {invitType}";
    }
    #endregion

    #region DefaultValueInvitation
    /// <summary>
    /// Carga la informacion del Guest, InvitationGift, BookingDeposit, CreditCardList, AdditionalGuest
    /// dependiento del tipo de invitacion.
    /// </summary>
    /// <param name="user">UserData</param>
    /// <param name="guID">Guest ID</param>
    private async Task DefaultValueInvitation(UserData user, int guID)
    {

      //Llenamos la informacion del guest
      await LoadGuest(user, guID);

      //Si trae GuestID, Nueva o existente.
      if (guID != 0)
      {
        await LoadInvitationGift(guID);
        await LoadBookingDeposit(guID);
        await LoadGuestCreditCard(guID);
        await LoadAdditionalGuest(guID);
      }
      //Si No trae GuestID Invitacion Nueva       
      else
      {
        _guestInvitation.InvitationGiftList = new ObservableCollection<InvitationGift>();
        _guestInvitation.BookingDepositList = new ObservableCollection<BookingDeposit>();
        _guestInvitation.GuestCreditCardList = new ObservableCollection<GuestCreditCard>();
        _guestInvitation.AdditionalGuestList = new ObservableCollection<Guest>();
      }

      //Carga el Mensaje de la invitacion
      LoadInvitationInfo();
    }
    #endregion

    #region GetInvitationMode
    /// <summary>
    /// Analiza si el huesped ya ha sido invitado y entra en modo lectura o si se trata de una invitacion nueva.
    /// </summary>
    ///<history>
    ///[erosado]  11/08/2016  Created.
    /// </history>
    private void GetInvitationMode(Guest guestObj)
    {
      EnumMode invitationMode;
      var permission = _module != EnumModule.Host ? EnumPermission.PRInvitations : EnumPermission.HostInvitations;

      //Si es una invitacion existente
      if (guestObj != null && guestObj.guInvit)
      {
        //Revisamos que tenga permisos para editar >= Standard
        if (_user.HasPermission(permission, EnumPermisionLevel.Standard))
        {
          invitationMode = EnumMode.Edit;
        }
        //Si No asiganamos permisos de solo lectura
        else
        {
          invitationMode = EnumMode.ReadOnly;
        }
      }
      //Si es una invitacion nueva 
      else
      {
        //Revisamos que tenga permisoss para Agregar 
        if (_user.HasPermission(permission, EnumPermisionLevel.Standard))
        {
          invitationMode = EnumMode.Add;
        }
        else
        {
          invitationMode = EnumMode.ReadOnly;
        }
      }
      //Notificamos el cambio
      _guestInvitation.InvitationMode = invitationMode;
    }
    #endregion

    #endregion

    #region Metodos Carga de Catalogos

    #region Languages
    private async Task LoadLenguages()
    {
      var result = await BRLanguages.GetLanguages(1);
      _guestInvitation.Languages = result;
    }
    #endregion

    #region MaritalStatus
    private async Task LoadMaritalStatus()
    {
      var result = await BRMaritalStatus.GetMaritalStatus(1);
      _guestInvitation.MaritalStatus = result;
    }
    #endregion

    #region Personnel
    /// <summary>
    /// Carga al personal dependiendo del tipo de invitacion
    /// </summary>
    /// <param name="user">UserData</param>
    /// <param name="module">EnumModule</param>
    /// <history>
    /// [erosado] 09/08/2016
    /// </history>
    private async Task LoadPersonnel(UserData user, EnumModule module)
    {
      List<PersonnelShort> personnel = new List<PersonnelShort>();
      //Si es Host carga al personal con la sala de venta
      if (module == EnumModule.Host)
      {
        personnel = await BRPersonnel.GetPersonnel(salesRooms: user.SalesRoom.srID, roles: "PR");
      }
      //Si es cualquier otro lo hace con el leadSource
      else
      {
        personnel = await BRPersonnel.GetPersonnel(user.LeadSource.lsID, roles: "PR");
      }
      _guestInvitation.Personnel = personnel;
    }
    #endregion

    #region Hotels
    private async Task LoadHotels()
    {
      var result = await BRHotels.GetHotels(nStatus: 1);
      _guestInvitation.Hotels = result;
    }
    #endregion

    #region Agencies
    private async Task LoadAgencies()
    {
      var result = await BRAgencies.GetAgencies(1);
      _guestInvitation.Agencies = result;
    }
    #endregion

    #region Countries
    private async Task LoadCountries()
    {
      var result = await BRCountries.GetCountries(1);
      _guestInvitation.Countries = result;
    }
    #endregion

    #region GuestStatusType
    private async Task LoadGuestStatusType()
    {
      var result = await BRGuests.GetGuestStatusType(1);
      _guestInvitation.GuestStatusTypes = result;
    }
    #endregion

    #region Currencies
    private async Task LoadCurrencies()
    {
      var result = await BRCurrencies.GetCurrencies(nStatus: 1);
      _guestInvitation.Currencies = result;
    }
    #endregion

    #region PaymentTypes
    private async Task LoadPaymentTypes()
    {
      var result = await BRPaymentTypes.GetPaymentTypes(1);
      _guestInvitation.PaymentTypes = result;
    }
    #endregion

    #region PaymentPlaces
    private async Task LoadPaymentPlaces()
    {
      var result = await BRPaymentPlaces.GetPaymentPlaces();
      _guestInvitation.PaymentPlaces = result;
    }
    #endregion

    #region CreditCardTypes
    private async Task LoadCreditCardTypes()
    {
      var result = await BRCreditCardTypes.GetCreditCardTypes(nStatus: 1);
      _guestInvitation.CreditCardTypes = result;
    }

    #endregion

    #region Gifts
    private async Task LoadGifts(UserData _user)
    {
      var result = await BRGifts.GetGiftsShort(_user.Location == null ? "ALL" : _user.Location.loID, 1);
      _guestInvitation.Gifts = result;
    }
    #endregion

    #region SalesRooms
    private async Task LoadSalesRooms()
    {
      var result = await BRSalesRooms.GetSalesRooms(0);
      _guestInvitation.SalesRoom = result;
    }
    #endregion

    #region Locations
    /// <summary>
    /// Carga las locaciones del usuario logeado (Todas)
    /// </summary>
    /// <param name="user"></param>
    /// <param name="module"></param>
    private async Task LoadLocations(UserData user, EnumModule module)
    {
      var result = await BRLocations.GetLocationsByUser(user.User.peID);
      _guestInvitation.Locations = result;
    }
    #endregion

    #region DisputeStatus
    private async Task LoadDisputeStatus()
    {
      var result = await BRDisputeStatus.GetDisputeStatus();
      _guestInvitation.DisputeStatus = result;
    }
    #endregion

    #region LoadCloseDate
    private async Task LoadCloseDate()
    {
      var result = await BRConfiguration.GetCloseDate();
      _guestInvitation.CloseDate = result;
    }
    #endregion

    #region LoadProgram
    /// <summary>
    /// Carga el Program para la invitacion
    /// </summary>
    /// <param name="module">EnumModule</param>
    /// <param name="invitationType">EnumInvitationType</param>
    /// <param name="guID">GuestID</param>
    /// <history>
    /// [erosado] 10/08/2016  Created.
    /// </history>
    private async Task LoadProgram(EnumModule module, EnumInvitationType invitationType, int guID)
    {
      EnumProgram program = EnumProgram.All;
      //Si se tiene el GuestID
      if (guID != 0 && module != EnumModule.Host)
      {
        //Obtenemos la informacion del Guest
        var guest = await BRGuests.GetGuest(guID);
        //Obtenemos la informacion de program 
        var result = await BRLeadSources.GetLeadSourceProgram(guest.gulsOriginal);
        //Asignamos el Program
        if (result == EnumToListHelper.GetEnumDescription(EnumProgram.Inhouse))
        {
          program = EnumProgram.Inhouse;
        }
        else
        {
          program = EnumProgram.Outhouse;
        }
      }
      //Si NO hay un Guest para obtener el program 
      else
      {
        //De que modulo me estan hablando
        switch (module)
        {
          case EnumModule.InHouse:
            if (invitationType == EnumInvitationType.newExternal)
            {
              program = EnumProgram.Inhouse;
            }
            break;
          case EnumModule.OutHouse:
            if (invitationType == EnumInvitationType.newOutHouse)
            {
              program = EnumProgram.Outhouse;
            }
            break;
          case EnumModule.Host:
            if (invitationType == EnumInvitationType.newOutHouse)
            {
              program = EnumProgram.Outhouse;
            }
            else
            {
              program = EnumProgram.Inhouse;
            }
            break;
          default:
            break;
        }
      }
      _guestInvitation.Program = program;
    }
    #endregion

    #endregion

    #region Invitation Info

    #region Load Guest
    private async Task LoadGuest(UserData user, int guID)
    {
      Guest guestObj = new Guest();

      //Si tiene GuestID obtemos la informacion del GuestID
      if (guID != 0)
      {
        guestObj = await BRGuests.GetGuest(guID, true);
      }

      //Obtenemos la fecha y hora del servidor
      var serverDate = BRHelpers.GetServerDate();
      var serverTime = BRHelpers.GetServerDateTime();

      //Calculamos el Modo de la invitacion
      GetInvitationMode(guestObj);

      //Si es una invitacion Nueva SIN GuestID
      if (_guestInvitation.InvitationMode == EnumMode.Add && guestObj.guID == 0)
      {
        //Valores Defualt Comunes entre los tipos de invitacion 
        guestObj.guInvitD = serverDate;
        guestObj.guInvitT = serverTime;
        guestObj.guCheckInD = serverDate;
        guestObj.guCheckOutD = serverDate.AddDays(7);
        guestObj.guRoomsQty = 1;
        guestObj.gucu = "US";
        guestObj.gula = "EN";
        guestObj.gums1 = "N";
        guestObj.gums2 = "N";
        guestObj.gupt = "CS";

        //Casos especiales de asignacion de valores default
        switch (_module)
        {
          case EnumModule.InHouse:
            guestObj.guloInvit = user.LeadSource.lsID;
            guestObj.guls = user.LeadSource.lsID;
            if (_invitationType == EnumInvitationType.newExternal)
            {
              guestObj.guag = "EXTERNAL";
            }
            break;
          case EnumModule.OutHouse:
            guestObj.guloInvit = user.LeadSource.lsID;
            guestObj.guag = "OUTSIDE";
            break;
          case EnumModule.Host:
            guestObj.guDirect = true;
            guestObj.gusr = user.SalesRoom.srN;
            break;
          default:
            break;
        }
      }
      //Si es un invitacion nueva CON GuestID
      else if (_guestInvitation.InvitationMode == EnumMode.Add && guestObj.guID != 0)
      {
        //Fecha y hora Invitacion
        guestObj.guInvitD = serverDate;
        guestObj.guInvitT = serverTime;

        //Casos especiales de asignacion de valores default
        switch (_module)
        {
          case EnumModule.InHouse:
            guestObj.guloInvit = user.LeadSource.lsID;
            if (_invitationType == EnumInvitationType.newExternal)
            {
              guestObj.guag = "EXTERNAL";
            }
            break;
          case EnumModule.OutHouse:
            guestObj.guloInvit = user.SalesRoom.srN;
            guestObj.guag = "OUTSIDE";
            break;
          case EnumModule.Host:
            guestObj.guDirect = true;
            guestObj.gusr = user.SalesRoom.srN;
            break;
          default:
            break;
        }

      }

      //Hacemos una copia del objeto
      Guest copyGuest = new Guest();
      ObjectHelper.CopyProperties(copyGuest, guestObj);

      //Notificamos cambios 
      _guestInvitation.Guest = guestObj;
      _guestInvitation.CloneGuest = guestObj;
    }
    #endregion

    #region Load InvitationGift
    private async Task LoadInvitationGift(int guID)
    {
      var result = await BRInvitsGifts.GetInvitsGiftsByGuestID(guID);
      //Obtiene la informacion del InvitationGift
      _guestInvitation.InvitationGiftList = new ObservableCollection<InvitationGift>(result);
      //Crea una copia de la lista
      _guestInvitation.CloneInvitationGiftList = ObjectHelper.CopyProperties(result);
    }
    #endregion

    #region Load Deposit
    /// <summary>
    /// Carga la informacion de los depositos, esta informacion se presenta en el dtgDeposits 
    /// </summary>
    /// <param name="guID">Guest ID</param>
    /// <history>
    /// [erosado] 04/08/2016  Created.
    /// </history>
    private async Task LoadBookingDeposit(int guID)
    {
      var result = await BRBookingDeposits.GetBookingDeposits(guID, true);
      ////Obtiene la informacion del Booking Deposits
      _guestInvitation.BookingDepositList = new ObservableCollection<BookingDeposit>(result);
      ////Crea una copia de la lista
      _guestInvitation.CloneBookingDepositList = ObjectHelper.CopyProperties(result);
    }
    #endregion

    #region Load CreditCard
    /// <summary>
    /// Carga la informacion de las tarjetas de credito del Guest, esta informacion se presenta en el dtgCCCompany
    /// </summary>
    /// <param name="guID">Guest ID</param>
    /// <history>
    /// [erosado] 04/08/2016  Created.
    /// </history>
    private async Task LoadGuestCreditCard(int guID)
    {
      var result = await BRGuestCreditCard.GetGuestCreditCard(guID);
      ////Obtiene la informacion del GuestCreditCard
      _guestInvitation.GuestCreditCardList = new ObservableCollection<GuestCreditCard>(result);
      ////Crea una copia de la lista
      _guestInvitation.CloneGuestCreditCardList = ObjectHelper.CopyProperties(result);
    }
    #endregion

    #region Load AdditionalGuest
    /// <summary>
    /// Carga la informacion de los Guest adicionales, esta informacion se presenta en el dtgAdditionalGuest
    /// </summary>
    /// <param name="guID">Guest ID</param>
    /// <history>
    /// [erosado] 04/08/2016  Created.
    /// </history>
    private async Task LoadAdditionalGuest(int guID)
    {
      var result = await BRGuests.GetAdditionalGuest(guID);
      ////Obtiene la informacion del AdditionalGuest
      _guestInvitation.AdditionalGuestList = new ObservableCollection<Guest>(result);
      ////Crea una copia de la lista
      _guestInvitation.CloneAdditionalGuestList = ObjectHelper.CopyProperties(result);
    }
    #endregion
    #endregion

  }
}





