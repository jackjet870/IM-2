using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Clase que sirve como asistente para operaciones matematicas y numericas 
  /// </summary>
  /// <history>
  /// [ecanul] 17/08/2016 Created
  /// </history>
  public class MathHelper
  {

    #region SecureDivision
    /// <summary>
    /// Realiza una division segura
    /// </summary>
    /// <param name="Dividend">Dividendo</param>
    /// <param name="Divisor">Divisor</param>
    /// <history>
    /// [ecanul] 17/08/2016 Created
    /// </history>
    public static decimal SecureDivision(decimal Dividend, decimal Divisor)
    {
      if (Divisor == 0)
        return 0;
      else
      {
        return Dividend / Divisor;
      }
    } 
    #endregion

  }
}
