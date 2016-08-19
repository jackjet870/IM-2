using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Classes
{
  public class GuestShow
  {
    #region Properties

    #region Catalogos

    public List<LanguageShort> Languages { get; set; }

    public List<MaritalStatus> MaritalStatus { get; set; }

    public List<PersonnelShort> Personnel { get; set; }
    public List<Hotel> Hotels { get; set; }
    public List<AgencyShort> Agencies { get; set; }
    public List<CountryShort> Countries { get; set; }
    public List<GuestStatusType> GuestStatusTypes { get; set; }

    public List<Currency> Currencies { get; set; }
    public List<PaymentType> PaymentTypes { get; set; }
    public List<PaymentPlace> PaymentPlaces { get; set; }
    public List<CreditCardType> CreditCardTypes { get; set; }

    public List<GiftShort> Gifts { get; set; }

    public List<SalesRoomShort> SalesRoom { get; set; }

    public List<LocationByUser> Locations { get; set; }

    public List<DisputeStatus> DisputeStatus { get; set; }
    public DateTime? CloseDate { get; set; }

    public EnumProgram Program { get; set; }

    public EnumMode InvitationMode { get; set; }

    #endregion

    #region Listas 
    public List<InvitationGift> InvitationGiftList { get; set; }
    public List<BookingDeposit> BookingDepositList { get; set; }
    public List<GuestCreditCard> GuestCreditCardList { get; set; }
    public List<Guest> AdditionalGuestList { get; set; }
    public Guest Guest { get; set; }
    public List<TourTimeAvailable> TourTimes { get; set; }
    public GuestLog GuestLog { get; set; }


    #endregion

    #region Listas Clonadas
    public List<InvitationGift> CloneInvitationGiftList { get; set; }

    public List<BookingDeposit> CloneBookingDepositList { get; set; }

    public List<GuestCreditCard> CloneGuestCreditCardList { get; set; }

    public List<Guest> CloneAdditionalGuestList { get; set; }

    public Guest CloneGuest { get; set; }

    #endregion

    #endregion
  }
}
