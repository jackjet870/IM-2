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
  public class CommonCatObject : INotifyPropertyChanged
  {
    EnumModule _module;
    EnumInvitationType _invitationType;
    UserData _user;
    int _guID;

    #region Properties
    #region Solo Lectura(Catalogos)
    private List<LanguageShort> _languages;
    public List<LanguageShort> Languages => _languages;
    private List<MaritalStatus> _maritalStatus;
    public List<MaritalStatus> MaritalStatus => _maritalStatus;
    private List<PersonnelShort> _personnel;
    public List<PersonnelShort> Personnel => _personnel;
    private List<Hotel> _hotels;
    public List<Hotel> Hotels => _hotels;
    private List<AgencyShort> _agencies;
    public List<AgencyShort> Agencies => _agencies;
    private List<CountryShort> _countries;
    public List<CountryShort> Countries => _countries;
    private List<GuestStatusType> _guestStatusTypes;
    public List<GuestStatusType> GuestStatusTypes => _guestStatusTypes;
    private List<Currency> _currencies;
    public List<Currency> Currencies => _currencies;
    private List<PaymentType> _paymentTypes;
    public List<PaymentType> PaymentTypes => _paymentTypes;
    private List<PaymentPlace> _paymentPlaces;
    public List<PaymentPlace> PaymentPlaces => _paymentPlaces;
    private List<CreditCardType> _creditCardTypes;
    public List<CreditCardType> CreditCardTypes => _creditCardTypes;
    private List<GiftShort> _gifts;
    public List<GiftShort> Gifts => _gifts;
    private List<SalesRoomShort> _salesRoom;
    public List<SalesRoomShort> SalesRoom => _salesRoom;
    private List<LocationByUser> _locations;
    public List<LocationByUser> Locations => _locations;
    private List<DisputeStatus> _disputeStatus;
    public List<DisputeStatus> DisputeStatus => _disputeStatus;
    private DateTime? _closeDate;
    public DateTime? CloseDate => _closeDate;
    private EnumProgram _program;
    public EnumProgram Program => _program;
    private EnumMode _invitationMode;
    public EnumMode InvitationMode => _invitationMode;

    #endregion

    #region Lectura & Escritura
    private ObservableCollection<InvitationGift> _invitationGiftList;
    public ObservableCollection<InvitationGift> InvitationGiftList
    {
      get { return _invitationGiftList; }
      set { SetField(ref _invitationGiftList, value); }
    }
    private ObservableCollection<BookingDeposit> _bookingDepositList;
    public ObservableCollection<BookingDeposit> BookingDepositList
    {
      get { return _bookingDepositList; }
      set { SetField(ref _bookingDepositList, value); }
    }
    private ObservableCollection<GuestCreditCard> _guestCreditCardList;
    public ObservableCollection<GuestCreditCard> GuestCreditCardList
    {
      get { return _guestCreditCardList; }
      set { SetField(ref _guestCreditCardList, value); }
    }
    private ObservableCollection<Guest> _additionalGuestList;
    public ObservableCollection<Guest> AdditionalGuestList
    {
      get { return _additionalGuestList; }
      set { SetField(ref _additionalGuestList, value); }
    }
    private Guest _guestObj;
    public Guest GuestObj
    {
      get { return _guestObj; }
      set
      { SetField(ref _guestObj, value); }
    }
    private List<TourTimeAvailable> _tourTimes;
    public List<TourTimeAvailable> TourTimes
    {
      get { return _tourTimes; }
      set { SetField(ref _tourTimes, value); }
    }


    #endregion

    #region Listas Clonadas
    private List<InvitationGift> _cInvitationGiftList;
    public List<InvitationGift> CInvitationGiftList => _cInvitationGiftList;
    private List<BookingDeposit> _cBookingDepositList;
    public List<BookingDeposit> CBookingDepositList => _cBookingDepositList;
    private List<GuestCreditCard> _cGuestCreditCardList;
    public List<GuestCreditCard> CGuestCreditCardList => _cGuestCreditCardList;
    private List<Guest> _cAdditionalGuestList;
    public List<Guest> CAdditionalGuestList => _cAdditionalGuestList;
    private Guest _cGuestObj;
    public Guest CGuestObj => _cGuestObj;

    #endregion
    #endregion

    #region Constructor
    public CommonCatObject(EnumModule module, EnumInvitationType invitationType, UserData user, int guID = 0)
    {
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
        //await LoadGuest(user, guID);
        await LoadInvitationGift(guID);
        await LoadBookingDeposit(guID);
        await LoadGuestCreditCard(guID);
        await LoadAdditionalGuest(guID);
      }
      //Si No trae GuestID Invitacion Nueva       
      else
      {
        SetField(ref _invitationGiftList, new ObservableCollection<InvitationGift>(), nameof(InvitationGiftList));
        SetField(ref _bookingDepositList, new ObservableCollection<BookingDeposit>(), nameof(BookingDepositList));
        SetField(ref _guestCreditCardList, new ObservableCollection<GuestCreditCard>(), nameof(GuestCreditCardList));
        SetField(ref _additionalGuestList, new ObservableCollection<Guest>(), nameof(AdditionalGuestList));
      }
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
      SetField(ref _invitationMode, invitationMode, nameof(invitationMode));
    }
    #endregion

    #endregion

    #region Metodos Carga de Catalogos

    #region Languages
    private async Task LoadLenguages()
    {
      var result = await BRLanguages.GetLanguages(1);
      SetField(ref _languages, result, nameof(Languages));
    }
    #endregion

    #region MaritalStatus
    private async Task LoadMaritalStatus()
    {
      var result = await BRMaritalStatus.GetMaritalStatus(1);
      SetField(ref _maritalStatus, result, nameof(MaritalStatus));
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
      SetField(ref _personnel, personnel, nameof(Personnel));
    }
    #endregion

    #region Hotels
    private async Task LoadHotels()
    {
      var result = await BRHotels.GetHotels(nStatus: 1);
      SetField(ref _hotels, result, nameof(Hotels));
    }
    #endregion

    #region Agencies
    private async Task LoadAgencies()
    {
      var result = await BRAgencies.GetAgencies(1);
      SetField(ref _agencies, result, nameof(Agencies));
    }
    #endregion

    #region Countries
    private async Task LoadCountries()
    {
      var result = await BRCountries.GetCountries(1);
      SetField(ref _countries, result, nameof(Countries));
    }
    #endregion

    #region GuestStatusType
    private async Task LoadGuestStatusType()
    {
      var result = await BRGuests.GetGuestStatusType(1);
      SetField(ref _guestStatusTypes, result, nameof(GuestStatusTypes));
    }
    #endregion

    #region Currencies
    private async Task LoadCurrencies()
    {
      var result = await BRCurrencies.GetCurrencies(nStatus: 1);
      SetField(ref _currencies, result, nameof(Currencies));
    }
    #endregion

    #region PaymentTypes
    private async Task LoadPaymentTypes()
    {
      var result = await BRPaymentTypes.GetPaymentTypes(1);
      SetField(ref _paymentTypes, result, nameof(PaymentTypes));

    }
    #endregion

    #region PaymentPlaces
    private async Task LoadPaymentPlaces()
    {
      var result = await BRPaymentPlaces.GetPaymentPlaces();
      SetField(ref _paymentPlaces, result, nameof(PaymentPlaces));
    }
    #endregion

    #region CreditCardTypes
    private async Task LoadCreditCardTypes()
    {
      var result = await BRCreditCardTypes.GetCreditCardTypes(nStatus: 1);
      SetField(ref _creditCardTypes, result, nameof(CreditCardTypes));
    }

    #endregion

    #region Gifts
    private async Task LoadGifts(UserData _user)
    {
      var result = await BRGifts.GetGiftsShort(_user.Location == null ? "ALL" : _user.Location.loID, 1);
      SetField(ref _gifts, result, nameof(Gifts));
    }
    #endregion

    #region SalesRooms
    private async Task LoadSalesRooms()
    {
      var result = await BRSalesRooms.GetSalesRooms(0);
      SetField(ref _salesRoom, result, nameof(SalesRoom));
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

      SetField(ref _locations, result, nameof(Locations));
    }
    #endregion

    #region DisputeStatus
    private async Task LoadDisputeStatus()
    {
      var result = await BRDisputeStatus.GetDisputeStatus();
      SetField(ref _disputeStatus, result, nameof(DisputeStatus));
    }
    #endregion

    #region LoadCloseDate
    private async Task LoadCloseDate()
    {
      var result = await BRConfiguration.GetCloseDate();
      SetField(ref _closeDate, result, nameof(CloseDate));
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
      SetField(ref _program, program, nameof(Program));
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
      SetField(ref _guestObj, guestObj, nameof(GuestObj));
      SetField(ref _cGuestObj, copyGuest, nameof(CGuestObj));

    }
    #endregion

    #region Load InvitationGift
    private async Task LoadInvitationGift(int guID)
    {
      var result = await BRInvitsGifts.GetInvitsGiftsByGuestID(guID);
      //Obtiene la informacion del InvitationGift
      SetField(ref _invitationGiftList, new ObservableCollection<InvitationGift>(result), nameof(InvitationGiftList));
      //Crea una copia de la lista
      SetField(ref _cInvitationGiftList, result.ToList(), nameof(CInvitationGiftList));
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
      SetField(ref _bookingDepositList, new ObservableCollection<BookingDeposit>(result), nameof(BookingDepositList));
      ////Crea una copia de la lista
      SetField(ref _cBookingDepositList, ObjectHelper.CopyProperties(result), nameof(CBookingDepositList));
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
      SetField(ref _guestCreditCardList, new ObservableCollection<GuestCreditCard>(result), nameof(GuestCreditCardList));
      ////Crea una copia de la lista
      SetField(ref _cGuestCreditCardList, result.ToList(), nameof(CGuestCreditCardList));
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
      SetField(ref _additionalGuestList, new ObservableCollection<Guest>(result), nameof(AdditionalGuestList));
      ////Crea una copia de la lista
      SetField(ref _cAdditionalGuestList, Model.Helpers.ObjectHelper.CopyProperties(result), nameof(CAdditionalGuestList));
    }
    #endregion
    #endregion

    #region Implementacion INotifyPropertyChange
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value)) return;
      field = value;
      OnPropertyChanged(propertyName);
    }
    #endregion

  }
}





