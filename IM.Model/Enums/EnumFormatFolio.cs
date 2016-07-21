using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Enums
{
  /// <summary>
  /// Enumerado para el manejo de los diferentes formatos de folios
  /// </summary>
  /// <history>
  /// [vipacheco] 01/Julio/2016 Created
  /// </history>
  public enum EnumFormatFolio
  {
    Invalid = -1,
    None,
    Letters,
    Numbers,
    LettersNumbers,
    NumbersLetters
  }
}
