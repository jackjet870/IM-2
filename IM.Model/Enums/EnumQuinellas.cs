using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para indicar si un reporte se considera para Quiniela.
  /// </summary>
  /// <history>
  /// [edgrodriguez] 02/Mar/2016 Created
  /// [aalcocer]     27/May/2016 Modified. Se cambian los nombres y se agrega Descripcion
  /// </history>
  public enum EnumQuinellas
  {
    NoQuinellas,

    [Description("Consider Quinellas")]
    Quinellas
  }
}