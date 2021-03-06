﻿using IM.Model.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IM.Model.Classes
{
  public class GuestShow : EntityBase
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

    private List<Gift> _gifts;

    public List<Gift> Gifts
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

    private List<Location> _locations;

    public List<Location> Locations
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

    private EnumProgram _program;

    public EnumProgram Program
    {
      get { return _program; }
      set { SetField(ref _program, value); }
    }

    private byte _ocWelcomeCopies;

    public byte OcWelcomeCopies
    {
      get { return _ocWelcomeCopies; }
      set { SetField(ref _ocWelcomeCopies, value); }
    }

    private List<LeadSource> _leadSources;

    public List<LeadSource> LeadSources
    {
      get { return _leadSources; }
      set { SetField(ref _leadSources, value); }
    }

    private List<TeamSalesmen> _teamSalesMen;

    public List<TeamSalesmen> TeamSalesMen
    {
      get { return _teamSalesMen; }
      set { SetField(ref _teamSalesMen, value); }
    }

    private List<PersonnelShort> _personnelHOSTENTRY;

    public List<PersonnelShort> PersonnelHOSTENTRY
    {
      get { return _personnelHOSTENTRY; }
      set { SetField(ref _personnelHOSTENTRY, value); }
    }

    private List<PersonnelShort> _personnelPR1;

    public List<PersonnelShort> PersonnelPR1
    {
      get { return _personnelPR1; }
      set { SetField(ref _personnelPR1, value); }
    }

    private List<PersonnelShort> _personnelPR2;

    public List<PersonnelShort> PersonnelPR2
    {
      get { return _personnelPR2; }
      set { SetField(ref _personnelPR2, value); }
    }

    private List<PersonnelShort> _personnelPR3;

    public List<PersonnelShort> PersonnelPR3
    {
      get { return _personnelPR3; }
      set { SetField(ref _personnelPR3  , value); }
    }

    private List<PersonnelShort> _personnelLINER1;

    public List<PersonnelShort> PersonnelLINER1
    {
      get { return _personnelLINER1; }
      set { SetField(ref _personnelLINER1, value); }
    }

    private List<PersonnelShort> _personnelLINER2;

    public List<PersonnelShort> PersonnelLINER2
    {
      get { return _personnelLINER2; }
      set { SetField(ref _personnelLINER2, value); }
    }

    private List<PersonnelShort> _personnelLINER3;

    public List<PersonnelShort> PersonnelLINER3
    {
      get { return _personnelLINER3; }
      set { SetField(ref _personnelLINER3, value); }
    }

    private List<PersonnelShort> _personnelPODIUM;

    public List<PersonnelShort> PersonnelPODIUM
    {
      get { return _personnelPODIUM; }
      set { SetField(ref _personnelPODIUM, value); }
    }

    private List<PersonnelShort> _personnelCLOSER1;

    public List<PersonnelShort> PersonnelCLOSER1
    {
      get { return _personnelCLOSER1; }
      set { SetField(ref _personnelCLOSER1, value); }
    }

    private List<PersonnelShort> _personnelCLOSER2;

    public List<PersonnelShort> PersonnelCLOSER2
    {
      get { return _personnelCLOSER2; }
      set { SetField(ref _personnelCLOSER2, value); }
    }

    private List<PersonnelShort> _personnelCLOSER3;

    public List<PersonnelShort> PersonnelCLOSER3
    {
      get { return _personnelCLOSER3; }
      set { SetField(ref _personnelCLOSER3, value); }
    }

    private List<PersonnelShort> _personnelCLOSER4;

    public List<PersonnelShort> PersonnelCLOSER4
    {
      get { return _personnelCLOSER4; }
      set { SetField(ref _personnelCLOSER4, value); }
    }

    private List<PersonnelShort> _personnelCLOSEREXIT1;

    public List<PersonnelShort> PersonnelCLOSEREXIT1
    {
      get { return _personnelCLOSEREXIT1; }
      set { SetField(ref _personnelCLOSEREXIT1, value); }
    }

    private List<PersonnelShort> _personnelCLOSEREXIT2;

    public List<PersonnelShort> PersonnelCLOSEREXIT2
    {
      get { return _personnelCLOSEREXIT2; }
      set { SetField(ref _personnelCLOSEREXIT2, value); }
    }

    private List<PersonnelShort> _personnelCLOSEREXIT3;

    public List<PersonnelShort> PersonnelCLOSEREXIT3
    {
      get { return _personnelCLOSEREXIT3; }
      set { SetField(ref _personnelCLOSEREXIT3, value); }
    }

    private List<PersonnelShort> _personnelHOSTGIFTS;

    public List<PersonnelShort> PersonnelHOSTGIFTS
    {
      get { return _personnelHOSTGIFTS; }
      set { SetField(ref _personnelHOSTGIFTS, value); }
    }

    private List<PersonnelShort> _personnelHOSTEXIT;

    public List<PersonnelShort> PersonnelHOSTEXIT
    {
      get { return _personnelHOSTEXIT; }
      set { SetField(ref _personnelHOSTEXIT, value); }
    }

    private List<PersonnelShort> _personnelVLO;

    public List<PersonnelShort> PersonnelVLO
    {
      get { return _personnelVLO; }
      set { SetField(ref _personnelVLO, value); }
    }

    private List<PersonnelShort> _personnelFRONTTOBACK1;
    public List<PersonnelShort> PersonnelFRONTTOBACK1
    {
      get { return _personnelFRONTTOBACK1; }
      set { SetField(ref _personnelFRONTTOBACK1, value); }
    }

    private List<PersonnelShort> _personnelFRONTTOBACK2;
    public List<PersonnelShort> PersonnelFRONTTOBACK2
    {
      get { return _personnelFRONTTOBACK2; }
      set { SetField(ref _personnelFRONTTOBACK2, value); }
    }

    private List<PersonnelShort> _personnelFRONTTOMIDDLE1;
    public List<PersonnelShort> PersonnelFRONTTOMIDDLE1
    {
      get { return _personnelFRONTTOMIDDLE1; }
      set { SetField(ref _personnelFRONTTOMIDDLE1, value); }
    }

    private List<PersonnelShort> _personnelFRONTTOMIDDLE2;
    public List<PersonnelShort> PersonnelFRONTTOMIDDLE2
    {
      get { return _personnelFRONTTOMIDDLE2; }
      set { SetField(ref _personnelFRONTTOMIDDLE2, value); }
    }
    #endregion Catalogos

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

    #endregion Listas

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

    #endregion Listas Clonadas

    #endregion Properties
  }
}