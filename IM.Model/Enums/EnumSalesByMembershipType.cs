using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para indicar si un reporte debe detallar las ventas por tipo de membresia
  /// </summary>
  /// <history>
  /// [erodriguez] 05/03/2016 Created
  /// [aalcocer]   27/05/2016 Modified. Se cambian los nombres y se agrega Descripcion
  /// </history>
  public enum EnumSalesByMemberShipType
  {
    NoDetail,

    [Description("Detail the Sales by Membership Type")]
    Detail
  }
}