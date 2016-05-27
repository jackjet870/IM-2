using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para indicar si un reporte debe estar basado en la fecha de booking
  /// </summary>
  /// <history>
  /// [edgrodriguez] 05/Mar/2016 Created
  /// [aalcocer]     27/05/2016 Modified. Se cambian los nombres y se agrega Descripcion
  /// </history>
  public enum EnumBasedOnBooking
  {
    NoBasedOnBooking,

    [Description("Based on booking date")]
    BasedOnBooking
  }
}