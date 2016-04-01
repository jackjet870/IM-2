using System.ComponentModel;

namespace IM.Model.Enums
{
  public enum EnumRptLeadSource
  {
    [Description("Production (Graphic)")]
    ProductionGraphic,

    [Description("Production by Agency (Nights)")]
    ProductionbyAgencyNights,

    [Description("Production by Agency")]
    ProductionbyAgency,

    [Description("Production by PR")]
    ProductionbyPR,

    [Description("Production by PR & Sales Room")]
    ProductionbyPRSalesRoom,

    [Description("Reps Payment")]
    RepsPayment,

    [Description("Reps Payment Summary")]
    RepsPaymentSummary,

    [Description("Unavailable Arrivals (Graphic)")]
    UnavailableArrivalsGraphic,

    [Description("Production by Guest Status")]
    ProductionbyGuestStatus,

    [Description("Production by Age")]
    ProductionbyAge,

    [Description("Production by Age, Market & Originally Available")]
    ProductionbyAgeMarketOriginallyAvailable,

    [Description("Gifts Received by Sales Room")]
    GiftsReceivedbySalesRoom,

    [Description("Occupation, Contact, Book & Show")]
    OccupationContactBookShow,

    [Description("Production by Nationality")]
    ProductionbyNationality,

    [Description("Production by Nationality, Market & Originally Available")]
    ProductionbyNationalityMarketOriginallyAvailable,

    [Description("Production by Agency (Only Quinellas)")]
    ProductionbyAgencyOnlyQuinellas,

    [Description("Production by Gift & Quantity")]
    ProductionbyGiftQuantity,

    [Description("Follow Up by Agency")]
    FollowUpbyAgency,

    [Description("Follow Up by PR")]
    FollowUpbyPR,

    [Description("Production by Desk")]
    ProductionbyDesk,

    [Description("Production by Group")]
    ProductionbyGroup,

    [Description("Not Booking Arrivals (Graphic)")]
    NotBookingArrivalsGraphic,

    [Description("Production by PR & Group")]
    ProductionbyPRGroup,

    [Description("Production by Couple Type")]
    ProductionbyCoupleType,

    [Description("Score by PR")]
    ScorebyPR,

    [Description("Cost by PR")]
    CostbyPR,

    [Description("Show Factor by Booking Date")]
    ShowFactorbyBookingDate,

    [Description("Production by Member Type & Agency")]
    ProductionbyMemberTypeAgency,

    [Description("Production by Member Type, Agency, Market & Originally Available")]
    ProductionbyMemberTypeAgencyMarketOriginallyAvailable,

    [Description("Production by Contract & Agency")]
    ProductionbyContractAgency,

    [Description("Production by Contract, Agency, Market & Originally Available")]
    ProductionbyContractAgencyMarketOriginallyAvailable,

    [Description("Unavailable Motives By Agency")]
    UnavailableMotivesByAgency,
  }
}