using System;
using System.Collections.Generic;
using IM.Model.Enums;

namespace IM.ProcessorGeneral.Classes
{
  public class clsFilter
  {
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string DateBasedOn { get; set; }

    public List<string> LstGifts { get; set; } = new List<string>();
    public List<string> LstGiftsCate { get; set; } = new List<string>();
    public List<string> LstLeadSources { get; set; } = new List<string>();
    public List<string> LstPrograms { get; set; } = new List<string>();
    public List<int> LstRateTypes { get; set; } = new List<int>();
    public List<string> LstSalesRooms { get; set; } = new List<string>();
    public List<string> LstWarehouses { get; set; } = new List<string>();

    public EnumPredefinedDate SelectedDate { get; set; }
    public EnumBasedOnArrival BasedOnArrival { get; set; }
    public EnumBasedOnBooking BasedOnBooking { get; set; }
    public EnumQuinellas Quinellas { get; set; }
    public EnumDetailGifts DetailGift { get; set; }
    public EnumSalesByMemberShipType SalesByMemberShipType { get; set; }
    public EnumStatus Status { get; set; }
    public EnumGiftsReceiptType GiftsReceiptType { get; set; }
    public EnumGiftSale GiftSale { get; set; }
    public EnumSaveCourtesyTours SaveCourtesyTours { get; set; }
    public EnumExternalInvitation ExternalInvitation { get; set; }

    public string  GuestId { get; set; }

    public bool AllGifts { get; set; }
    public bool AllGiftsCate { get; set; }
    public bool AllLeadSources { get; set; }
    public bool AllPrograms { get; set; }
    public bool AllRateTypes { get; set; }
    public bool AllSalesRooms { get; set; }
    public bool AllWarehouses { get; set; }
    
  }
}
