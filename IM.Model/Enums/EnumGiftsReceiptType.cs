using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes tipos de recibos de regalos
  /// </summary>
  /// <history>
  /// [edgrodriguez] 07/03/2016 Created
  /// </history>
  public enum EnumGiftsReceiptType
  {
    [Description("All")]
    grtAll,
    [Description("Gifts Receipts")]
    grtGiftsReceipts,
    [Description("CxC")]
    grtCxC
  }
}
