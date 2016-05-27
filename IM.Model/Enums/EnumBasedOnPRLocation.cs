using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para indicar si un reporte debe estar basado en la locacion del PR
  /// </summary>
  /// <history>
  /// [aalcocer] 27/05/2016 Created
  /// </history>
  public enum EnumBasedOnPRLocation
  {
    NoBasedOnPRLocation,

    [Description("Based on PR Location")]
    BasedOnPRLocation
  }
}