using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Services.WirePRService;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace IM.Base.Classes
{
  public class GuestInvitationRules : GuestInvitation
  {
    private EnumModule _module;
    private EnumInvitationType _invitationType;
    private UserData _user;
    private int _guID;
    private bool _fromFrmGuest;

    #region Constructores

    public GuestInvitationRules(EnumModule module, EnumInvitationType invitationType, UserData user, int guId = 0)
    {
      _module = module;
      _invitationType = invitationType;
      _user = user;
      _guID = guId;
    }

    public GuestInvitationRules(EnumModule module, UserData user, int guId = 0)
    {
      _user = user;
      _guID = guId;
      _module = module;
      _fromFrmGuest = true;
    }

    #endregion Constructor

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
      if (_fromFrmGuest)
      {
        await Task.WhenAll(
          LoadAgencies(),
          LoadMaritalStatus(),
          LoadGuestStatusType(),
          LoadGifts(_user),
          LoadAdditionalGuest(_guID),
           //Cargamos la invitacion
           DefaultValueInvitation(_user, _guID)
          );
      }
      else
      {
        await Task.WhenAll(
          //Cargamos los catalogos comunes
          LoadAgencies(),
          LoadLenguages(),
          LoadMaritalStatus(),
          LoadPersonnel(_user, _module),
          LoadHotels(),
          LoadCountries(),
          LoadGuestStatusType(),
          LoadCurrencies(),
          LoadPaymentTypes(),
          LoadPaymentPlaces(),
          LoadCreditCardTypes(),
          LoadSalesRooms(),
          LoadLocations(_user),
          LoadGifts(_user),
          LoadProgram(_module, _invitationType, _guID),
          LoadAdditionalGuest(_guID),
          //Cargamos la invitacion
          DefaultValueInvitation(_user, _guID)
          );
      }

    }

    #region LoadInvitationInfo
    /// <summary>
    /// Load Invitation Info es el mensaje que sale a lado del menu bar en la invitacion
    /// </summary>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// </history>
    public void LoadInvitationInfo()
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
      }
      InvitationInfo = $"Mode: {EnumToListHelper.GetEnumDescription(InvitationMode)} | Module: {_module} | Invitation Type: {invitType}";
    }
    #endregion

    #endregion LoadAll

    #region DefaultValueInvitation

    /// <summary>
    /// Carga la informacion del Guest, InvitationGift, BookingDeposit, CreditCardList, AdditionalGuest
    /// dependiento del tipo de invitacion.
    /// </summary>
    /// <param name="user">UserData</param>
    /// <param name="guId">Guest ID</param>
    private async Task DefaultValueInvitation(UserData user, int guId)
    {
      //Llenamos la informacion del guest
      await LoadGuest(user, guId);

      //Si trae GuestID, Nueva o existente.
      if (guId != 0)
      {
        await LoadInvitationGift(guId);
        await LoadAdditionalGuest(guId);
        if (_fromFrmGuest) return;
        await LoadBookingDeposit(guId);
        await LoadGuestCreditCard(guId);
      }
      //Si No trae GuestID Invitacion Nueva
      else
      {
        CloneInvitationGiftList = new List<InvitationGift>();
        CloneAdditionalGuestList = new List<Guest>();
        InvitationGiftList = new ObservableCollection<InvitationGift>();
        AdditionalGuestList = new ObservableCollection<Guest>();
        if (_fromFrmGuest) return;
        BookingDepositList = new ObservableCollection<BookingDeposit>();
        GuestCreditCardList = new ObservableCollection<GuestCreditCard>();
        CloneGuestCreditCardList = new List<GuestCreditCard>();
        CloneBookingDepositList = new List<BookingDeposit>();
      }

      //Carga el Mensaje de la invitacion
      LoadInvitationInfo();
    }

    #endregion DefaultValueInvitation

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
      InvitationMode = invitationMode;
    }

    #endregion GetInvitationMode

    #endregion DefaultValues

    #region Metodos Carga de Catalogos

    #region Languages

    private async Task LoadLenguages()
    {
      var result = await BRLanguages.GetLanguages(1);
      Languages = result;
    }

    #endregion Languages

    #region MaritalStatus

    private async Task LoadMaritalStatus()
    {
      var result = await BRMaritalStatus.GetMaritalStatus(1);
      MaritalStatus = result;
    }

    #endregion MaritalStatus

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
      List<PersonnelShort> personnel;
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
      Personnel = personnel;
    }

    #endregion Personnel

    #region Hotels

    private async Task LoadHotels()
    {
      var result = await BRHotels.GetHotels(nStatus: 1);
      Hotels = result;
    }

    #endregion Hotels

    #region Agencies

    private async Task LoadAgencies()
    {
      var result = await BRAgencies.GetAgencies(1);
      Agencies = result;
    }

    #endregion Agencies

    #region Countries

    private async Task LoadCountries()
    {
      var result = await BRCountries.GetCountries(1);
      Countries = result;
    }

    #endregion Countries

    #region GuestStatusType

    private async Task LoadGuestStatusType()
    {
      var result = await BRGuests.GetGuestStatusType(1);
      GuestStatusTypes = result;
    }

    #endregion GuestStatusType

    #region Currencies

    private async Task LoadCurrencies()
    {
      var result = await BRCurrencies.GetCurrencies(nStatus: 1);
      Currencies = result;
    }

    #endregion Currencies

    #region PaymentTypes

    private async Task LoadPaymentTypes()
    {
      var result = await BRPaymentTypes.GetPaymentTypes(1);
      PaymentTypes = result;
    }

    #endregion PaymentTypes

    #region PaymentPlaces

    private async Task LoadPaymentPlaces()
    {
      var result = await BRPaymentPlaces.GetPaymentPlaces();
      PaymentPlaces = result;
    }

    #endregion PaymentPlaces

    #region CreditCardTypes

    private async Task LoadCreditCardTypes()
    {
      var result = await BRCreditCardTypes.GetCreditCardTypes(nStatus: 1);
      CreditCardTypes = result;
    }

    #endregion CreditCardTypes

    #region Gifts

    private async Task LoadGifts(UserData user)
    {
      var result = await BRGifts.GetGifts(nStatus: 1, location: user.Location == null ? "ALL" : user.Location.loID);
      Gifts = result;
    }

    #endregion Gifts

    #region SalesRooms

    private async Task LoadSalesRooms()
    {
      var result = await BRSalesRooms.GetSalesRooms(1);
      SalesRoom = result;
    }

    #endregion SalesRooms

    #region Locations

    /// <summary>
    /// Carga las locaciones del usuario logeado (Todas)
    /// </summary>
    /// <param name="user"></param>
    private async Task LoadLocations(UserData user)
    {
      var result = await BRLocations.GetLocationsByUser(user.User.peID);
      Locations = result;
    }

    #endregion Locations

    #region LoadProgram

    /// <summary>
    /// Carga el Program para la invitacion
    /// </summary>
    /// <param name="module">EnumModule</param>
    /// <param name="invitationType">EnumInvitationType</param>
    /// <param name="guId">GuestID</param>
    /// <history>
    /// [erosado] 10/08/2016  Created.
    /// </history>
    private async Task LoadProgram(EnumModule module, EnumInvitationType invitationType, int guId)
    {
      EnumProgram program = EnumProgram.All;
      //Si se tiene el GuestID
      if (guId != 0 && module != EnumModule.Host)
      {
        //Obtenemos la informacion del Guest
        var guest = await BRGuests.GetGuest(guId);
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
        }
      }
      Program = program;
      AllowReschedule = program == EnumProgram.Inhouse;
    }

    #endregion LoadProgram

    #endregion Metodos Carga de Catalogos

    #region Invitation Info

    #region Load Guest

    private async Task LoadGuest(UserData user, int guId)
    {
      Guest guestObj = new Guest();

      //Si tiene GuestID obtemos la informacion del GuestID
      if (guId != 0)
      {
        guestObj = await BRGuests.GetGuest(guId, true);
      }

      //Clonamos el Guest
      CloneGuest = new Guest();
      ObjectHelper.CopyProperties(CloneGuest, guestObj);

      //Obtenemos la fecha y hora del servidor
      var serverDate = BRHelpers.GetServerDate();
      var serverTime = BRHelpers.GetServerDateTime();

      //Calculamos el Modo de la invitacion
      GetInvitationMode(guestObj);

      //Si es una invitacion Nueva SIN GuestID
      if (InvitationMode == EnumMode.Add && guestObj.guID == 0)
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
            guestObj.gusr = user.SalesRoom.srID;
            break;
        }
      }
      //Si es un invitacion nueva CON GuestID
      else if (InvitationMode == EnumMode.Add && guestObj.guID != 0)
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
            guestObj.guloInvit = user.SalesRoom.srID;
            guestObj.guag = "OUTSIDE";
            break;

          case EnumModule.Host:
            guestObj.guDirect = true;
            guestObj.gusr = user.SalesRoom.srID;
            break;
        }
      }
      //Notificamos cambios
      Guest = guestObj;
    }

    #endregion Load Guest

    #region Load InvitationGift

    private async Task LoadInvitationGift(int guId)
    {
      var result = await BRInvitsGifts.GetInvitsGiftsByGuestID(guId);
      //Obtiene la informacion del InvitationGift
      InvitationGiftList = new ObservableCollection<InvitationGift>(result);
      //Crea una copia de la lista
      CloneInvitationGiftList = ObjectHelper.CopyProperties(result);
    }

    #endregion Load InvitationGift

    #region Load Deposit

    /// <summary>
    /// Carga la informacion de los depositos, esta informacion se presenta en el dtgDeposits
    /// </summary>
    /// <param name="guId">Guest ID</param>
    /// <history>
    /// [erosado] 04/08/2016  Created.
    /// </history>
    private async Task LoadBookingDeposit(int guId)
    {
      var result = await BRBookingDeposits.GetBookingDeposits(guId, true);
      ////Obtiene la informacion del Booking Deposits
      BookingDepositList = new ObservableCollection<BookingDeposit>(result);
      ////Crea una copia de la lista
      CloneBookingDepositList = ObjectHelper.CopyProperties(result);
    }

    #endregion Load Deposit

    #region Load CreditCard

    /// <summary>
    /// Carga la informacion de las tarjetas de credito del Guest, esta informacion se presenta en el dtgCCCompany
    /// </summary>
    /// <param name="guId">Guest ID</param>
    /// <history>
    /// [erosado] 04/08/2016  Created.
    /// </history>
    private async Task LoadGuestCreditCard(int guId)
    {
      var result = await BRGuestCreditCard.GetGuestCreditCard(guId);
      ////Obtiene la informacion del GuestCreditCard
      GuestCreditCardList = new ObservableCollection<GuestCreditCard>(result);
      ////Crea una copia de la lista
      CloneGuestCreditCardList = ObjectHelper.CopyProperties(result);
    }
    #endregion Load CreditCard

    #region Load AdditionalGuest

    /// <summary>
    /// Carga la informacion de los Guest adicionales, esta informacion se presenta en el dtgAdditionalGuest
    /// </summary>
    /// <param name="guId">Guest ID</param>
    /// <history>
    /// [erosado] 04/08/2016  Created.
    /// </history>
    private async Task LoadAdditionalGuest(int guId)
    {
      var result = await BRGuests.GetAdditionalGuest(guId);
      ////Obtiene la informacion del AdditionalGuest
      AdditionalGuestList = new ObservableCollection<Guest>(result);
      ////Crea una copia de la lista
      CloneAdditionalGuestList = ObjectHelper.CopyProperties(result);
    }
    #endregion Load AdditionalGuest

    #endregion Invitation Info

    #region Other Methods

    #region SetRervationOrigosInfo
    /// <summary>
    /// Asignamos los valores de ReservationOrigos a nuestro objeto Guest
    /// </summary>
    /// <param name="reservationOrigos">ReservationOrigos</param>
    /// <history>
    /// [erosado] 18/08/2016  Created.
    /// </history>
    public void SetRervationOrigosInfo(ReservationOrigos reservationOrigos)
    {
      //Asignamos el folio de reservacion
      Guest.guHReservID = reservationOrigos.Folio;
      Guest.guLastName1 = reservationOrigos.LastName;
      Guest.guFirstName1 = reservationOrigos.FirstName;
      Guest.guCheckInD = reservationOrigos.Arrival;
      Guest.guCheckOutD = reservationOrigos.Departure;
      Guest.guRoomNum = reservationOrigos.Room;
      //Calculamos Pax
      decimal pax;
      bool convertPax = decimal.TryParse($"{reservationOrigos.Adults}.{reservationOrigos.Children}", out pax);
      Guest.guPax = convertPax ? pax : 0;
      //Obtenemos el Id del Hotel
      var ls = BRLeadSources.GetLeadSourceByID(reservationOrigos.Hotel);
      Guest.guHotel = ls?.lsho;
      //Country
      Guest.guco = Countries.Where(x => x.coN == reservationOrigos.Country).Select(x => x.coID).FirstOrDefault();
      //Agency
      Guest.guag = Agencies.Where(x => x.agN.ToUpper() == reservationOrigos.Agency.ToUpper()).Select(x => x.agID).FirstOrDefault();
      //Company
      Guest.guCompany = reservationOrigos.Company;
      //Membership
      Guest.guMembershipNum = reservationOrigos.Membership;
      //Notificamos el cambio
      OnPropertyChanged(nameof(Guest));
    }
    #endregion
    
    #endregion

  }
}