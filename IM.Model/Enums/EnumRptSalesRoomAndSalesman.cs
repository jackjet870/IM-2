using System.ComponentModel;

namespace IM.Model.Enums
{
  public enum EnumRptSalesRoomAndSalesman
  {
    [Description("Stats by Segments")]
    StatsBySegments,
    [Description("Stats by Segments Categories")]
    StatsBySegmentsCategories,
    [Description("Stats by Segments (OWN)")]
    StatsBySegmentsOwn,
    [Description("Stats by Segments Categories (OWN)")]
    StatsBySegmentsCategoriesOwn,
    [Description("Stats by F.T.B.")]
    StatsByFtb,
    [Description("Stats by Closer")]
    StatsByCloser,
    [Description("Stats by Exit Closer")]
    StatsByExitCloser,
    [Description("F.T.M. In & Out House ")]
    FtmInAndOutHouse,
    [Description("Self Gen & Self Gen Team")]
    SelfGenAndSelfGenTeam,
    [Description("Stats by F.T.B. & Locations")]
    StatsByFtbAndLocations,
    [Description("Stats by F.T.B. & Locations Categories")]
    StatsByFtbAndLocationsCategories,
    [Description("Manifest")]
    Manifest
  }
}
