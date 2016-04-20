using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes PalaceType
  /// </summary>
  /// <history>
  /// [ecanul] 16/03/2016 Created
  /// [ecanul] 19/04/2016 Modificated Agredado Descripcion
  /// </history>
  public enum EnumPlaceType
  {
    [Description("LS")]
    LeadSource,
    [Description("SR")]
    SalesRoom,
    [Description("WH")]
    Warehouse
  }
}
