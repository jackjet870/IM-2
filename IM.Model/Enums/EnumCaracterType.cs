using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes tipos de caracteres
  /// </summary>
  /// <history>
  /// [vipacheco] 01/Julio/2016 Created
  /// </history>
  public enum EnumCaracterType
  {
    None = 0,
    Letter,
    Number,
    Blank,  //Espacio en blanco
    Indent, //Guion
    Colon   //Coma
  }
}
