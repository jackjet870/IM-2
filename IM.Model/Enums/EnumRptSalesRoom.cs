﻿using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado de los reportes por Sala de ventas de Processor Saldes
  /// </summary>
  /// <history>
  /// [ecanul] 03/05/2016 Created
  /// [ecanul] 05/08/2016 Modified. Corregida descripcion del enumerado ConcentrateDailySales
  /// [ecanul] 09/09/2016 Modified. Renombrado Enumerado de EnumRptRoomSales a EnumRptSalesRoom
  /// </history>
  public enum EnumRptSalesRoom
  {
    [Description("Stats by Segments")]
    StatsBySegments,
    [Description("Stats by Segments Categories")]
    StatsBySegmentsCategories,
    [Description("Stats by Segments (OWN)")]
    StatsBySegmentsOwn,
    [Description("Stats by Segments Categories (OWN)")]
    StatsBySegmentsCategoriesOwn,
    [Description("Stats by Segments Categories (Multi Date Ranges)")]
    StatsBySegmentsCategoriesMultiDatesRanges,
    [Description("Stats by F.T.B")]
    StatsByFtb,
    [Description("Stats by Closer")]
    StatsByCloser,
    [Description("Stats by Exit Closer")]
    StatsByExitCloser,
    [Description("F.T.M. In & Out House")]
    FtmInAndOutHouse,
    [Description("Self Gen & Self Gen Team")]
    SelfGenAndSelfGenTeam,
    [Description("Stats by F.T.B. & Locations")]
    StatsByFtbAndLocatios,
    [Description("Stats by F.T.B. & Locations Categories")]
    StatsByFtbAndLocatiosCategories,
    [Description("Daily Sales")]
    DailySales,
    [Description("Concentrate Daily Sales")]
    ConcerntrateDailySales,
    [Description("Stats by Location")]
    StatsByLocation,
    [Description("Efficiency Weekly")]
    EfficiencyWeekly,
    [Description("Manifest")]
    Manifest,
    [Description("Stats by Location (Monthly)")]
    StatsByLocationMonthly,
    [Description("Sales by Location (Monthly)")]
    SalesByLocationMonthly,
    [Description("Stats by Location & Sales Room")]
    StatsByLocationAndSalesRoom
  }
}
