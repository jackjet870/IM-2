using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado de los diferentes Permisos
  /// </summary>
  /// <history>
  /// [jorcanche]  12/Mar/2016 Created
  /// </history>
  public enum EnumPermission
  {
    [Description("None")]
    None,
    [Description("REGISTER")]
    Register,
    [Description("AVAIL")]
    Available,
    [Description("PRINVIT")]
    PRInvitations,
    [Description("ASSIGNMENT")]
    Assignment,
    [Description("MAILOOUT")]
    MailOutsTexts,
    [Description("MAILOUTCON")]
    MailOutsConfig,
    [Description("HOST")]
    Host,
    [Description("HOSTINVIT")]
    HostInvitations,
    [Description("TAXIIN")]
    TaxiIn,
    [Description("MEALTICKET")]
    MealTicket,
    [Description("SHOW")]
    Show,
    [Description("GIFTSRCPTS")]
    GiftsReceipts,
    [Description("SALES")]
    Sales,
    [Description("CONTRACTS")]
    Contracts,
    [Description("EQUITY")]
    Equity,
    [Description("MARITAL")]
    MaritalStatus,
    [Description("GIFTS")]
    Gifts,
    [Description("CURRENCIES")]
    Currencies,
    [Description("PERSONNEL")]
    Personnel,
    [Description("TEAMS")]
    Teams,
    [Description("TOURTIMES")]
    TourTimes,
    [Description("MOTIVES")]
    Motives,
    [Description("LANGUAGES")]
    Languages,
    [Description("AGENCIES")]
    Agencies,
    [Description("LOCATIONS")]
    Locations,
    [Description("WAREHOUSES")]
    Warehouses,
    [Description("EXCHRATES")]
    ExchangeRates,
    [Description("FOLIOSOUT")]
    FolioInvitationsOuthouse,
    [Description("FOLIOSCXC")]
    FolioCXC,
    [Description("CXCAUTH")]
    CxCAuthorization,
    [Description("CLOSESR")]
    CloseSalesRoom,
    [Description("MTREPRINT")]
    MealTicketReprint,
    [Description("WHOLESALER")]
    WholeSalers
  }
}