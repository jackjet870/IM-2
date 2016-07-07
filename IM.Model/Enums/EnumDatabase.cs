using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de las diferentes bases de datos
  /// </summary>
  /// <history>
  /// [emoguel] 6/25/2016 Created
  /// </history>
  public enum EnumDatabase
  {
    [Description("IMModel")]
    IntelligentMarketing,

    [Description("AsistenciaModel")]
    Asistencia,

    [Description("ICModel")]
    IntelligenceContracts
  }
}
