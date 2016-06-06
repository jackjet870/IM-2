using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  ///Enumerado para indicar si un reporte se considera solo mayoristas.
  /// </summary>
  /// <history>
  /// [aalcocer] 27/05/2016 Created
  /// </history>
  public enum EnumOnlyWholesalers
  {
    NoOnlyWholesalers,

    [Description("Only wholesalers")]
    OnlyWholesalers
  }
}