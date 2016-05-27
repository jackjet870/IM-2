using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerador para indicar si un reporte debe estar basado en la fecha de llegada.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 03/Mar/2016 Created
  /// [aalcocer]     27/05/2016 Modified. Se cambian los nombres y se agrega Descripcion
  /// </history>
  public enum EnumBasedOnArrival
  {
    NoBasedOnArrival,

    [Description("Based on arrival date")]
    BasedOnArrival
  }
}