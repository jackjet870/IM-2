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
    #region Attributes and Params
    private List<objInvitGift> _InitialInvitGift;
    #endregion

    #region Properties
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
    private ObservableCollection<InvitationGiftCustom> _invitationGiftList;
    public ObservableCollection<InvitationGiftCustom> InvitationGiftList => _invitationGiftList;
    private ObservableCollection<BookingDeposit> _bookingDepositList;
    public ObservableCollection<BookingDeposit> BookingDepositList => _bookingDepositList;
    private ObservableCollection<GuestCreditCard> _guestCreditCardList;
    public ObservableCollection<GuestCreditCard> GuestCreditCardList => _guestCreditCardList;
    private ObservableCollection<Guest> _aditionalGuestList;
    public ObservableCollection<Guest> AditionalGuestList => _aditionalGuestList;
    public List<InvitationGift> InvitationGiftListOld { get; private set; }
    #endregion

    public CommonCatObject(UserData user, int guId, EnumInvitationMode invitationType = EnumInvitationMode.modAdd)
    {
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

      if (invitationType == EnumInvitationMode.modAdd)
      {
        SetField(ref _invitationGiftList, new ObservableCollection<InvitationGiftCustom>(), "InvitationGiftList");
        SetField(ref _bookingDepositList, new ObservableCollection<BookingDeposit>(), "BookingDepositList");
        SetField(ref _guestCreditCardList, new ObservableCollection<GuestCreditCard>(), "GuestCreditCardList");
        SetField(ref _aditionalGuestList, new ObservableCollection<Guest>(), "AditionalGuestList");
      }
      else
      {
        //Search information 
        List<InvitationGiftCustom> lista = new List<InvitationGiftCustom>();
        lista.Add(new InvitationGiftCustom() { igQty = 1, igAdults = 2 });
        lista.Add(new InvitationGiftCustom() { igQty = 1, igAdults = 2 });
        lista.Add(new InvitationGiftCustom() { igQty = 1, igAdults = 2 });
        SetField(ref _invitationGiftList, new ObservableCollection<InvitationGiftCustom>(lista), "InvitationGiftList");
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

    #region InvitationGift Load
    private async void loadInvitationGift(int guId)
    {
      //var result = await BRInvitsGifts.GetInvitsGiftsByGuestID(guId);
      //SetField(ref _in)

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





