using System.ComponentModel;

namespace IM.Model.Enums
{
  public enum EnumPermisionLevel
  {
    [Description("0 - No Acces")]
    None,
    [Description("1 - Read Only")]
    ReadOnly,
    [Description("2 - Standar")]
    Standard,
    [Description("3 - Special")]
    Special,
    [Description("4 - Super Special")]
    SuperSpecial
  }
}