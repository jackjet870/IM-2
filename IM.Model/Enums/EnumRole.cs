using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado de los diferentes roles
  /// </summary>
  /// <history>
  /// [jorcanche]  12/Mar/2016 Created
  /// </history>
  public enum EnumRole
  {
    [Description("None")]
    None,
    [Description("PR")]
    PR,
    [Description("PRMEMBER")]
    PRMembers,
    [Description("PRCAPT")]
    PRCaptain,
    [Description("PRSUPER")]
    PRSupervisor,
    [Description("LINER")]
    Liner,
    [Description("LINERCAPT")]
    LinerCaptain,
    [Description("CLOSER")]
    Closer,
    [Description("CLOSERCAPT")]
    CloserCaptain,
    [Description("EXIT")]
    ExitCloser,
    [Description("PODIUM")]
    Podium,
    [Description("HOSTENTRY")]
    EntryHost,
    [Description("HOSTGIFTS")]
    GiftsHost,
    [Description("HOSTEXIT")]
    ExitHost,
    [Description("VLO")]
    VLO,
    [Description("MANAGER")]
    Manager,
    [Description("ADMIN")]
    Administrator,
    [Description("SECRETARY")]
    Secretary,
    [Description("BOSS")]
    Boss
  }
}