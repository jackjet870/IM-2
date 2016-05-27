using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para indicar si un reporte utiliza DetailGifts
  /// </summary>
  /// <history>
  /// [edgrodriguez] 05/Mar/2016 Created
  /// [aalcocer]     27/May/2016 Modified. Se cambian los nombres y se agrega Descripcion
  /// </history>
  public enum EnumDetailGifts
  {
    NoDetailGifts,

    [Description("Details Gifts")]
    DetailGifts
  }
}