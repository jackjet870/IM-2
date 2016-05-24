using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes estatus
  /// </summary>
  ///  <history>
  /// [edgrodriguez] 07/03/2016 Created
  /// </history>
  public enum EnumStatus
  {
    [Description("All")]
    staAll,
    [Description("Actives")]
    staActives,
    [Description("Cancelled")]
    staInactives
  }
}
