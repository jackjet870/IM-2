using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IM.Model.Classes
{
  public class GuestInvitation : EntityBase
  {

    #region Properties

    #region Catalogos

    private List<LanguageShort> _languages;
    public List<LanguageShort> Languages
    {
      get { return _languages; }
      set { SetField(ref _languages, value); }
    }

    private List<MaritalStatus> _maritalStatus;
    public List<MaritalStatus> MaritalStatus
    {
      get { return _maritalStatus; }
      set { SetField(ref _maritalStatus, value); }
    }

    private List<PersonnelShort> _personnel;
    public List<PersonnelShort> Personnel
    {
      get { return _personnel; }
      set { SetField(ref _personnel, value); }
    }

    private List<Hotel> _hotels;
    public List<Hotel> Hotels
    {
      get { return _hotels; }
      set { SetField(ref _hotels, value); }
    }

    private List<AgencyShort> _agencies;
    public List<AgencyShort> Agencies
    {
      get { return _agencies; }
      set { SetField(ref _agencies, value); }
    }

    private List<CountryShort> _countries;
    public List<CountryShort> Countries
    {
      get { return _countries; }
      set { SetField(ref _countries, value); }
    }

    private List<GuestStatusType> _guestStatusTypes;
    public List<GuestStatusType> GuestStatusTypes
    {
      get { return _guestStatusTypes; }
      set { SetField(ref _guestStatusTypes, value); }
    }

    private List<Currency> _currencies;
    public List<Currency> Currencies
    {
      get { return _currencies; }
      set { SetField(ref _currencies, value); }
    }

    private List<PaymentType> _paymentTypes;
    public List<PaymentType> PaymentTypes
    {
      get { return _paymentTypes; }
      set { SetField(ref _paymentTypes, value); }
    }

    private List<PaymentPlace> _paymentPlaces;
    public List<PaymentPlace> PaymentPlaces
    {
      get { return _paymentPlaces; }
      set { SetField(ref _paymentPlaces, value); }
    }

    private List<CreditCardType> _creditCardTypes;
    public List<CreditCardType> CreditCardTypes
    {
      get { return _creditCardTypes; }
      set { SetField(ref _creditCardTypes, value); }
    }

    private List<GiftShort> _gifts;
    public List<GiftShort> Gifts
    {
      get { return _gifts; }
      set { SetField(ref _gifts, value); }
    }

    private List<SalesRoomShort> _salesRoom;
    public List<SalesRoomShort> SalesRoom
    {
      get { return _salesRoom; }
      set { SetField(ref _salesRoom, value); }
    }

    private List<LocationByUser> _locations;
    public List<LocationByUser> Locations
    {
      get { return _locations; }
      set { SetField(ref _locations, value); }
    }

    private List<DisputeStatus> _disputeStatus;
    public List<DisputeStatus> DisputeStatus
    {
      get { return _disputeStatus; }
      set { SetField(ref _disputeStatus, value); }
    }

    private DateTime? _closeDate;
    public DateTime? CloseDate
    {
      get { return _closeDate; }
      set { SetField(ref _closeDate, value); }
    }

    private EnumProgram _program;
    public EnumProgram Program
    {
      get { return _program; }
      set { SetField(ref _program, value); }
    }

    private EnumMode _invitationMode;
    public EnumMode InvitationMode
    {
      get { return _invitationMode; }
      set { SetField(ref _invitationMode, value); }
    }

    private string _invitationInfo;

    public string InvitationInfo
    {
      get { return _invitationInfo; }
      set { SetField(ref _invitationInfo, value); }
    }


    #endregion

    #region Listas 
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
    private Guest _guest;
    public Guest Guest
    {
      get { return _guest; }
      set
      { SetField(ref _guest, value); }
    }
    private List<TourTimeAvailable> _tourTimes;
    public List<TourTimeAvailable> TourTimes
    {
      get { return _tourTimes; }
      set { SetField(ref _tourTimes, value); }
    }


    #endregion

    #region Listas Clonadas
    private List<InvitationGift> _cloneInvitationGiftList;
    public List<InvitationGift> CloneInvitationGiftList
    {
      get { return _cloneInvitationGiftList; }
      set { SetField(ref _cloneInvitationGiftList, value); }
    }

    private List<BookingDeposit> _cloneBookingDepositList;
    public List<BookingDeposit> CloneBookingDepositList
    {
      get { return _cloneBookingDepositList; }
      set { SetField(ref _cloneBookingDepositList, value); }
    }

    private List<GuestCreditCard> _cloneGuestCreditCardList;
    public List<GuestCreditCard> CloneGuestCreditCardList
    {
      get { return _cloneGuestCreditCardList; }
      set { SetField(ref _cloneGuestCreditCardList, value); }
    }

    private List<Guest> _cloneAdditionalGuestList;
    public List<Guest> CloneAdditionalGuestList
    {
      get { return _cloneAdditionalGuestList; }
      set { SetField(ref _cloneAdditionalGuestList, value); }
    }

    private Guest _cloneGuest;
    public Guest CloneGuest
    {
      get { return _cloneGuest; }
      set { SetField(ref _cloneGuest, value); }
    }

    #endregion

    #endregion
  }
}





