using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de las diferentes fechas
  /// predefinidas.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 02/Mar/2016 Created
  /// [aalcocer]     22/04/2016 Modified. Se cambian los nombres y se les agrega Descripcion
  /// </history>
  public enum EnumPredefinedDate
  {
    AllDates = -1,

    [Description("Dates Specified")]
    DatesSpecified,

    [Description("Today")]
    Today,

    [Description("Yesterday")]
    Yesterday,

    [Description("This week")]
    ThisWeek,

    [Description("Previous week")]
    PreviousWeek,

    [Description("This half")]
    ThisHalf,

    [Description("Previous half")]
    PreviousHalf,

    [Description("This month")]
    ThisMonth,

    [Description("Previous month")]
    PreviousMonth,

    [Description("This year")]
    ThisYear,

    [Description("Previous year")]
    PreviousYear,

    // Estas opciones no se despliegan en la lista de fechas predefinidas "sin periodo"
    [Description("Two weeks ago")]
    TwoWeeksAgo,

    [Description("Three weeks ago")]
    ThreeWeeksAgo,

    [Description("Two months ago")]
    TwoMonthsAgo,

    [Description("Three months ago")]
    ThreeMonthsAgo,
  }
}