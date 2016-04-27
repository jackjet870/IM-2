using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes períodos de tiempo.
  /// </summary>
  ///  <history>
  /// [edgrodriguez] 03/Mar/2016 Created
  /// [aalcocer]     22/04/2016  Modified. Se cambian los nombres y se agrega Descripcion
  /// </history>
  public enum EnumPeriod
  {
    None,

    [Description("Weekly")]
    Weekly,

    [Description("Monthly")]
    Monthly
  }
}