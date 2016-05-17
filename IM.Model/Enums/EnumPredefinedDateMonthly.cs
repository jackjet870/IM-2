using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de las diferentes fechas
  /// predefinidas mensuales.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 02/Mar/2016 Created
  /// [emoguel] 10/05/2016 Modified
  /// </history>
  public enum EnumPredefinedDateMonthly
  {   

    [Description("Dates Specified")]
    DatesSpecified,

    [Description("This month")]
    ThisMonth,

    [Description("Previous month")]
    PreviousMonth,    

    [Description("Two months ago")]
    TwoMonthsAgo,

    [Description("Three months ago")]
    PreviousWeek,
  }
}
