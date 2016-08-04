using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
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
    private ObservableCollection<Guest> _aditionalGuestList;
    public ObservableCollection<Guest> AditionalGuestList
    {
      get { return _aditionalGuestList; }
      set { SetField(ref _aditionalGuestList, value); }
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
    private Guest _cGuestObj;
    public Guest CGuestObj => _cGuestObj;

    #endregion
    #endregion
    public CommonCatObject(UserData user, int guId, EnumInvitationMode invitationType = EnumInvitationMode.modAdd)
    {
      #region Inicializar Catalogos
      LoadLenguages();
      LoadMaritalStatus();
      LoadPersonnel(user);
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
      #endregion

      //Si se va a Generar una Nueva Invitacion
      if (invitationType == EnumInvitationMode.modAdd) 
      {
        //Asignamos memoria para que pueden usarse
        SetField(ref _invitationGiftList, new ObservableCollection<InvitationGift>(), nameof(InvitationGiftList));
        SetField(ref _bookingDepositList, new ObservableCollection<BookingDeposit>(), nameof(BookingDepositList));
        SetField(ref _guestCreditCardList, new ObservableCollection<GuestCreditCard>(), nameof(GuestCreditCardList));
        SetField(ref _aditionalGuestList, new ObservableCollection<Guest>(), nameof(AditionalGuestList));
        SetField(ref _guestObj, new Guest(), nameof(GuestObj));
      }
      //Si se va a modificar una Invitacion
      else
      {
        //Search information
        List<InvitationGift> lista = new List<InvitationGift>();
        lista.Add(new InvitationGift() { igQty = 1, iggi= "$160PSHOPM", igAdults = 1 });
        lista.Add(new InvitationGift() { igQty = 1, iggi = "$160PSHOPM", igAdults = 1 });
        lista.Add(new InvitationGift() { igQty = 1, iggi = "CR1600VIP", igAdults = 1 });
        SetField(ref _invitationGiftList, new ObservableCollection<InvitationGift>(lista), nameof(InvitationGiftList));


        //Creamos la copia
        SetField(ref _cInvitationGiftList, lista, nameof(CInvitationGiftList));
      

        Guest gue = new Guest() { gums1 = "N", gums2 = "W", guGStatus = "S" };
        SetField(ref _guestObj, gue, nameof(GuestObj));
      }
    }

    #region Metodos Carga de Catalogos

    #region Languages
    private async void LoadLenguages()
    {
      var result = await BRLanguages.GetLanguages(1);
      SetField(ref _languages, result, "Languages");
    }
    #endregion

    #region MaritalStatus
    private async void LoadMaritalStatus()
    {
      var result = await BRMaritalStatus.GetMaritalStatus(1);
      SetField(ref _maritalStatus, result, "MaritalStatus");
    }
    #endregion

    #region Personnel
    private async void LoadPersonnel(UserData _user)
    {
      var result = await BRPersonnel.GetPersonnel(_user.LeadSource.lsID, roles: "PR");
      SetField(ref _personnel, result, "Personnel");
    }
    #endregion

    #region Hotels
    private async void LoadHotels()
    {
      var result = await BRHotels.GetHotels(nStatus: 1);
      SetField(ref _hotels, result, "Hotels");
    }
    #endregion

    #region Agencies
    private async void LoadAgencies()
    {
      var result = await BRAgencies.GetAgencies(1);
      SetField(ref _agencies, result, "Agencies");
    }
    #endregion

    #region Countries
    private async void LoadCountries()
    {
      var result = await BRCountries.GetCountries(1);
      SetField(ref _countries, result, "Countries");
    }
    #endregion

    #region GuestStatusType
    private async void LoadGuestStatusType()
    {
      var result = await BRGuests.GetGuestStatusType(1);
      SetField(ref _guestStatusTypes, result, "GuestStatusTypes");
    }
    #endregion

    #region Currencies
    private async void LoadCurrencies()
    {
      var result = await BRCurrencies.GetCurrencies(nStatus: 1);
      SetField(ref _currencies, result, "Currencies");
    }
    #endregion

    #region PaymentTypes
    private async void LoadPaymentTypes()
    {
      var result = await BRPaymentTypes.GetPaymentTypes(1);
      SetField(ref _paymentTypes, result, "PaymentTypes");

    }
    #endregion

    #region PaymentPlaces
    private async void LoadPaymentPlaces()
    {
      var result = await BRPaymentPlaces.GetPaymentPlaces();
      SetField(ref _paymentPlaces, result, "PaymentPlaces");
    }
    #endregion

    #region CreditCardTypes
    private async void LoadCreditCardTypes()
    {
      var result = await BRCreditCardTypes.GetCreditCardTypes();
      SetField(ref _creditCardTypes, result, "CreditCardTypes");
    }
    #endregion

    #region Gifts
    private async void LoadGifts(UserData _user)
    {
      var result = await BRGifts.GetGiftsShort(_user.Location == null ? "ALL" : _user.Location.loID, 1);
      SetField(ref _gifts, result, "Gifts");
    }
    #endregion

    #region SalesRooms
    private async void LoadSalesRooms()
    {
      var result = await BRSalesRooms.GetSalesRooms(0);
      SetField(ref _salesRoom, result, "SalesRoom");
    }
    #endregion

    #region Locations
    private async void LoadLocations(UserData _user)
    {
      var result = await BRLocations.GetLocationsByUser(_user.User.peID);
      SetField(ref _locations, result, "Locations");
    }
    #endregion

    #region DisputeStatus
    private async void LoadDisputeStatus()
    {
      var result = await BRDisputeStatus.GetDisputeStatus();
      SetField(ref _disputeStatus, result, "DisputeStatus");
    }
    #endregion

    #endregion

    #region Invitation Info

    #region Load Guest
    private void LoadGuest(int guID)
    {
      var result =  BRGuests.GetGuestById(guID);
      SetField(ref _guestObj, result, "Languages");
    }
    #endregion

    #region Load InvitationGift

    #endregion

    #region Load Deposit

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





