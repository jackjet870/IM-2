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
    public CommonCatObject(UserData user, int guId, EnumInvitationType invitationType, EnumInvitationMode invitationMode = EnumInvitationMode.modAdd)
    {
      #region Inicializar Catalogos
      LoadLenguages();
      LoadMaritalStatus();
      LoadPersonnel(user, invitationType);
      LoadHotels();
      LoadAgencies();
      LoadCountries();
      LoadGuestStatusType();
      LoadCurrencies();
      LoadPaymentTypes();
      LoadPaymentPlaces();
      LoadCreditCardTypes();
      LoadSalesRooms();
      LoadLocations(user);
      LoadDisputeStatus();
      LoadGifts(user);
      LoadCloseDate();
      LoadProgram(user);
      #endregion

      //Si se va a Generar una Nueva Invitacion
      if (invitationMode == EnumInvitationMode.modAdd && guId == 0)
      {
        //Asignamos memoria para que pueden usarse
        SetField(ref _invitationGiftList, new ObservableCollection<InvitationGift>(), nameof(InvitationGiftList));
        SetField(ref _bookingDepositList, new ObservableCollection<BookingDeposit>(), nameof(BookingDepositList));
        SetField(ref _guestCreditCardList, new ObservableCollection<GuestCreditCard>(), nameof(GuestCreditCardList));
        SetField(ref _additionalGuestList, new ObservableCollection<Guest>(), nameof(AdditionalGuestList));
        SetField(ref _guestObj, new Guest(), nameof(GuestObj));
        DefaultValueForGuest(user);
      }
      //Si se va a modificar una Invitacion o se va hacer una nueva con los datos de un GuestID
      else
      {
        LoadGuest(user, guId);
        LoadInvitationGift(guId);
        LoadBookingDeposit(guId);
        LoadGuestCreditCard(guId);
        LoadAdditionalGuest(guId);
      }
    }
    #region DefaultValueForGuest
    /// <summary>
    /// Si ingresan valores default a los campos del Guest
    /// </summary>
    /// <param name="user"></param>
    private void DefaultValueForGuest(UserData user)
    {
      _guestObj.guloInvit = user.LeadSource.lsID;
    }

    #endregion

    #endregion

    #region Metodos Carga de Catalogos

    #region Languages
    private async void LoadLenguages()
    {
      var result = await BRLanguages.GetLanguages(1);
      SetField(ref _languages, result, nameof(Languages));
    }
    #endregion

    #region MaritalStatus
    private async void LoadMaritalStatus()
    {
      var result = await BRMaritalStatus.GetMaritalStatus(1);
      SetField(ref _maritalStatus, result, nameof(MaritalStatus));
    }
    #endregion

    #region Personnel
    /// <summary>
    /// carga al personal dependiendo del tipo de invitacion
    /// </summary>
    /// <param name="user"></param>
    /// <param name="inviType"></param>
    /// <history>
    /// [erosado] 09/08/2016
    /// </history>
    private async void LoadPersonnel(UserData user, EnumInvitationType inviType)
    {
      List<PersonnelShort> personnel = new List<PersonnelShort>();
      //Si es Host carga al personal con la sala de venta
      if (inviType == EnumInvitationType.Host)
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
    private async void LoadHotels()
    {
      var result = await BRHotels.GetHotels(nStatus: 1);
      SetField(ref _hotels, result, nameof(Hotels));
    }
    #endregion

    #region Agencies
    private async void LoadAgencies()
    {
      var result = await BRAgencies.GetAgencies(1);
      SetField(ref _agencies, result, nameof(Agencies));
    }
    #endregion

    #region Countries
    private async void LoadCountries()
    {
      var result = await BRCountries.GetCountries(1);
      SetField(ref _countries, result, nameof(Countries));
    }
    #endregion

    #region GuestStatusType
    private async void LoadGuestStatusType()
    {
      var result = await BRGuests.GetGuestStatusType(1);
      SetField(ref _guestStatusTypes, result, nameof(GuestStatusTypes));
    }
    #endregion

    #region Currencies
    private async void LoadCurrencies()
    {
      var result = await BRCurrencies.GetCurrencies(nStatus: 1);
      SetField(ref _currencies, result, nameof(Currencies));
    }
    #endregion

    #region PaymentTypes
    private async void LoadPaymentTypes()
    {
      var result = await BRPaymentTypes.GetPaymentTypes(1);
      SetField(ref _paymentTypes, result, nameof(PaymentTypes));

    }
    #endregion

    #region PaymentPlaces
    private async void LoadPaymentPlaces()
    {
      var result = await BRPaymentPlaces.GetPaymentPlaces();
      SetField(ref _paymentPlaces, result, nameof(PaymentPlaces));
    }
    #endregion

    #region CreditCardTypes
    private async void LoadCreditCardTypes()
    {
      var result = await BRCreditCardTypes.GetCreditCardTypes();
      SetField(ref _creditCardTypes, result, nameof(CreditCardTypes));
    }

    #endregion

    #region Gifts
    private async void LoadGifts(UserData _user)
    {
      var result = await BRGifts.GetGiftsShort(_user.Location == null ? "ALL" : _user.Location.loID, 1);
      SetField(ref _gifts, result, nameof(Gifts));
    }
    #endregion

    #region SalesRooms
    private async void LoadSalesRooms()
    {
      var result = await BRSalesRooms.GetSalesRooms(0);
      SetField(ref _salesRoom, result, nameof(SalesRoom));
    }
    #endregion

    #region Locations
    private async void LoadLocations(UserData _user)
    {
      var result = await BRLocations.GetLocationsByUser(_user.User.peID);
      SetField(ref _locations, result, nameof(Locations));
    }
    #endregion

    #region DisputeStatus
    private async void LoadDisputeStatus()
    {
      var result = await BRDisputeStatus.GetDisputeStatus();
      SetField(ref _disputeStatus, result, nameof(DisputeStatus));
    }
    #endregion

    #region LoadCloseDate
    private async void LoadCloseDate()
    {
      var result = await BRConfiguration.GetCloseDate();
      SetField(ref _closeDate, result, nameof(CloseDate));
    }
    #endregion

    #region LoadProgram
    private async void LoadProgram(UserData user)
    {
      var result = await BRLeadSources.GetLeadSourceProgram(user.LeadSource.lsID);
      EnumProgram program;

      if (result == EnumToListHelper.GetEnumDescription(EnumProgram.Inhouse))
      {
        program = EnumProgram.Inhouse;
      }
      else
      {
        program = EnumProgram.Outhouse;
      }

      SetField(ref _program, program, nameof(Program));
    }
    #endregion

    #endregion

    #region Invitation Info

    #region Load Guest
    private async void LoadGuest(UserData user, int guID)
    {
      var result = await BRGuests.GetGuest(guID, true);
      SetField(ref _guestObj, result, nameof(GuestObj));

      Guest copyGuest = new Guest();
      IM.Model.Helpers.ObjectHelper.CopyProperties(copyGuest, result);

      SetField(ref _cGuestObj, copyGuest, nameof(CGuestObj));
    }
    #endregion

    #region Load InvitationGift
    private async void LoadInvitationGift(int guID)
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
    private async void LoadBookingDeposit(int guID)
    {
      var result = await BRBookingDeposits.GetBookingDeposits(guID, true);
      ////Obtiene la informacion del Booking Deposits
      SetField(ref _bookingDepositList, new ObservableCollection<BookingDeposit>(result), nameof(BookingDepositList));
      ////Crea una copia de la lista
      SetField(ref _cBookingDepositList, Model.Helpers.ObjectHelper.CopyProperties(result), nameof(CBookingDepositList));
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
    private async void LoadGuestCreditCard(int guID)
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
    private async void LoadAdditionalGuest(int guID)
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





