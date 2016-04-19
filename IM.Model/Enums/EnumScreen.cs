using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de las diferentes pantallas del manifiesto
  /// </summary>
  /// <history>
  /// [ecanul] 18/04/2016 Created
  /// </history>
  public enum EnumScreen
  {
    [Description("arrivals")]
    Arrivals,
    [Description("availables")]
    Availables,
    [Description("premanifest")]
    Premanifest,
    [Description("search")]
    Search
  }
}
