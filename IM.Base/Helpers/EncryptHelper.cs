using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Clase que sirve como asistente para la encriptacion
  /// </summary>
  /// <history>
  ///   [wtorres]  23/02/2016 Created
  /// </history>
  class EncryptHelper
  {
    #region Metodos publicos
    
    /// <summary>
    /// Encripta y desencripta un texto
    /// </summary>
    /// <param name="text">Texto</param>
    /// <param name="encryptCode">Codigo de encriptacion</param>
    /// <history>
    ///   [wtorres]  23/02/2016 Created
    /// </history>
    public static string Encrypt(string text, string encryptCode)
    {
      StringBuilder sb = new StringBuilder(encryptCode.Length);
      int n = encryptCode.Length;
      for (int i = 1; i <= text.Length; i++)
      {
        int a = Convert.ToInt32(encryptCode[((i % n) - n * Convert.ToInt32((i % n) == 0)) - 1]);
        char ch = text[i - 1];
        int ax = Convert.ToInt32(ch) ^ a;
        char character;
        if (ax == 13 | ax == 10 | ax == 0 | ax == 39 | ax == 34)
        {
          character = ch;
        }
        else
        {
          character = Convert.ToChar(ax);
        }
        sb.Append(character);
      }
      return sb.ToString();
    }

    #endregion
  }
}
