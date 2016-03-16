using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes tipos de regalo venta
  /// </summary>
  /// <history>
  /// [edgrodriguez] 09/Mar/2016 Created
  /// </history>
  public enum EnumGiftSale
  {
    [Description("All")]
    gsAll,
    [Description("Gifts Sale")]
    gsSale,
    [Description("Gifts No Sale")]
    gsGift
  }
}
