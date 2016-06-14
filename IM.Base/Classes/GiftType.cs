using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Base.Classes
{
  /// <summary>
  /// Struct para el manejo de regalos
  /// </summary>
  /// <history>
  /// [vipacheco] 27/Mayo/2016 Created
  /// </history>
  public struct GiftType
  {
    public string ID;
    public string Descripcion;
    public int Quantity;
    public int Receipt;
    public string Promotion;
    public string TransactionTypeOpera;
  }
}
