using IM.Model.Enums;

namespace IM.Model.Classes
{
  public class StrToEnums
  {
    #region EnumTeamTypeToString

    /// <summary>
    /// Convierte un Enumerado tipo EnumTeamType a String
    /// </summary>
    /// <param name="teamType">Enumerado TeamType</param>
    /// <returns>Valor del Enumerado en String</returns>
    /// <history>
    /// [ECANUL] 09-03-2016 Created
    /// </history>
    public static string EnumTeamTypeToString(EnumTeamType teamType)
    {
      string str;
      if (teamType == EnumTeamType.TeamPRs)
        str = "GS";
      else
        str = "SA";
      return str;
    }

    #endregion

    #region EnumRoleToString

    /// <summary>
    /// Convierte un Enumerado tipo EnumRole a String
    /// </summary>
    /// <param name="str">Cadena</param>
    /// <history>
    /// [jorcanche] 12/Mar/2016 Created
    /// </history>
    public static string EnumRoleToString(EnumRole role)
    {
      string str = string.Empty;

      switch (role)
      {
        case EnumRole.Administrator:
          str = "ADMIN";
          break;
        case EnumRole.Boss:
          str = "BOSS";
          break;
        case EnumRole.Closer:
          str = "CLOSER";
          break;
        case EnumRole.CloserCaptain:
          str = "CLOSERCAPT";
          break;
        case EnumRole.EntryHost:
          str = "HOSTENTRY";
          break;
        case EnumRole.ExitCloser:
          str = "EXIT";
          break;
        case EnumRole.ExitHost:
          str = "HOSTEXIT";
          break;
        case EnumRole.GiftsHost:
          str = "HOSTGIFTS";
          break;
        case EnumRole.Liner:
          str = "LINER";
          break;
        case EnumRole.LinerCaptain:
          str = "LINERCAPT";
          break;
        case EnumRole.Manager:
          str = "MANAGER";
          break;
        case EnumRole.Podium:
          str = "PODIUM";
          break;
        case EnumRole.PR:
          str = "PR";
          break;
        case EnumRole.PRCaptain:
          str = "PRCAPT";
          break;
        case EnumRole.PRMembers:
          str = "PRMEMBER";
          break;
        case EnumRole.PRSupervisor:
          str = "PRSUPER";
          break;
        case EnumRole.Secretary:
          str = "SECRETARY";
          break;
        case EnumRole.VLO:
          str = "VLO";
          break;
      }

      return str;
    }

    #endregion

    #region EnumPermisoToString

    /// <summary>
    /// Convierte un Enumerado tipo EnumPermission a String
    /// </summary>
    /// <param name="str">Cadena</param>
    /// <history>
    /// [jorcanche] 12/Mar/2016 Created
    /// </history>
    public static string EnumPermisoToString(EnumPermission permission)
    {
      string str = string.Empty;

      switch (permission)
      {
        case EnumPermission.Agencies:
          str = "AGENCIES";
          break;
        case EnumPermission.Assignment:
          str = "ASSIGNMENT";
          break;
        case EnumPermission.Available:
          str = "AVAIL";
          break;
        case EnumPermission.CloseSalesRoom:
          str = "CLOSESR";
          break;
        case EnumPermission.Contracts:
          str = "CONTRACTS";
          break;
        case EnumPermission.Currencies:
          str = "CURRENCIES";
          break;
        case EnumPermission.CxCAuthorization:
          str = "CXCAUTH";
          break;
        case EnumPermission.Equity:
          str = "EQUITY";
          break;
        case EnumPermission.ExchangeRates:
          str = "EXCHRATES";
          break;
        case EnumPermission.FolioCXC:
          str = "FOLIOSCXC";
          break;
        case EnumPermission.FolioInvitationsOuthouse:
          str = "FOLIOSOUT";
          break;
        case EnumPermission.Gifts:
          str = "GIFTS";
          break;
        case EnumPermission.GiftsReceipts:
          str = "GIFTSRCPTS";
          break;
        case EnumPermission.HostInvitations:
          str = "HOSTINVIT";
          break;
        case EnumPermission.Host:
          str = "HOST";
          break;
        case EnumPermission.Languages:
          str = "LANGUAGES";
          break;
        case EnumPermission.Locations:
          str = "LOCATIONS";
          break;
        case EnumPermission.MailOutsConfig:
          str = "MAILOUTCON";
          break;
        case EnumPermission.MailOutsTexts:
          str = "MAILOOUT";
          break;
        case EnumPermission.MaritalStatus:
          str = "MARITAL";
          break;
        case EnumPermission.MealTicket:
          str = "MEALTICKET";
          break;
        case EnumPermission.MealTicketReprint:
          str = "MTREPRINT";
          break;
        case EnumPermission.Motives:
          str = "MOTIVES";
          break;
        case EnumPermission.Personnel:
          str = "PERSONNEL";
          break;
        case EnumPermission.PRInvitations:
          str = "PRINVIT";
          break;
        case EnumPermission.Register:
          str = "REGISTER";
          break;
        case EnumPermission.Sales:
          str = "SALES";
          break;
        case EnumPermission.Show:
          str = "SHOW";
          break;
        case EnumPermission.TaxiIn:
          str = "TAXIIN";
          break;
        case EnumPermission.Teams:
          str = "TEAMS";
          break;
        case EnumPermission.TourTimes:
          str = "TOURTIMES";
          break;
        case EnumPermission.Warehouses:
          str = "WAREHOUSES";
          break;
        case EnumPermission.WholeSalers:
          str ="WHOLESALER";
          break;
      }

      return str;
    }

    #endregion

    #region EnumGuestsMovementsTypeToString

    /// <summary>
    /// Convierte un Enumerado tipo EnumRole a String
    /// </summary>
    /// <param name="str">Cadena</param>
    /// <history>
    /// [jorcanche] 12/Mar/2016 Created
    /// </history>
    public static string EumGuestsMovementsTypeToString(EnumGuestsMovementsType MovementsType)
    {
      string str = string.Empty;
      switch (MovementsType)
      {
        case EnumGuestsMovementsType.Contact:
           str = "CN";
          break;
        case EnumGuestsMovementsType.Booking:
            str = "BK";
          break;
        case EnumGuestsMovementsType.Show:
            str = "SH";
          break;
        case EnumGuestsMovementsType.Sale:
            str = "SL";
          break;
        }
      return str;
    }

    #endregion

    #region EnumPalaceTypeToString
    /// <summary>
    /// Convierte un enumerado EnumPalaceType a String
    /// </summary>
    /// <param name="palaceType">Enumerado PalaceType</param>
    /// <returns>Valor del enumerado en string</returns>
    /// <history>[ECANUL] 16-03-2016 Created</history>
    public static string EnumPalaceTypeToSting(EnumPlaceType palaceType)
    {
      string str = string.Empty;
      switch(palaceType)
      {
        case EnumPlaceType.LeadSource:
          str = "LS";
        break;
        case EnumPlaceType.SalesRoom:
          str = "SR";
          break;
        case EnumPlaceType.Warehouse:
          str = "WH";
          break;
      }
      return str;
    }

    #endregion
  }
}
