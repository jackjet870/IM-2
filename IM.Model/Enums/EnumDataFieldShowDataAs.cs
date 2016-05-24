using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado de los distintos cálculos en los campos de valores de tabla dinámica
  /// </summary>
  /// <history>
  /// [aalcocer] 23/05/2016 Created.
  /// </history>
  public enum EnumDataFieldShowDataAs
  {
    [Description("difference")]
    Difference,

    [Description("index")]
    Index,

    [Description("normal")]
    Normal,

    [Description("percentDiff")]
    PercentDiff,

    [Description("percent")]
    Percent,

    [Description("percentOfCol")]
    PercentOfCol,

    [Description("percentOfRow")]
    PercentOfRow,

    [Description("percentOfTotal")]
    PercentOfTotal,

    [Description("runTotal")]
    RunTotal
  }
}