using System;
using IM.Model;
using System.Collections.Generic;

namespace IM.BusinessRules.Classes
{
  public class Invitation
  {
    public Guest Guest { get; set; }

    public List<InvitationGift> NewGifts { get; set; }

    public List<InvitationGift> UpdatedGifts { get; set; }

    public List<InvitationGift> DeletedGifts { get; set; }

    public List<GuestStatus> NewGuestStatus { get; set; }

    public List<GuestStatus> UpdatedGuestStatus { get; set; }

    public List<GuestStatus> DeletedGuestStatus { get; set; }

    public List<GuestCreditCard> NewCreditCards { get; set; }

    public List<GuestCreditCard> UpdatedCreditCards { get; set; }

    public List<GuestCreditCard> DeletedCreditCards { get; set; }

    public List<BookingDeposit> NewDeposits { get; set; }

    public List<BookingDeposit> UpdatedDeposits { get; set; }

    public List<BookingDeposit> DeletedDeposits { get; set; }

    public List<Guest> UpdatedAdditional { get; set; }

    public List<Guest> NewAdditional { get; set; }

    public List<Guest> DeletedAdditional { get; set; }

  }
}
