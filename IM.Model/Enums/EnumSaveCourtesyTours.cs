using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado que indica si un reporte debe incluir los tours de cortesia y  rescate.
  /// </summary>
  ///  <history>
  /// [erodriguez] 05/03/2016 Created
  /// [aalcocer]   27/05/2016 Modified. Se cambian los nombres
  /// </history>
  public enum EnumSaveCourtesyTours
  {
    [Description("Include Save Courtesy Tours")]
    IncludeSaveCourtesyTours,

    [Description("Exclude Save Courtesy Tours")]
    ExcludeSaveCourtesyTours,

    [Description("Exclude Save Courtesy Tours Without Sales")]
    ExcludeSaveCourtesyToursWithoutSales
  }
}