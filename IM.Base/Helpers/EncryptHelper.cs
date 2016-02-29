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
  public class EncryptHelper
  {
    private const string _encryptCode = "1O3r5i7g9o8s";
    #region Metodos publicos

    /// <summary>
    /// Encripta y desencripta un texto
    /// </summary>
    /// <param name="text">Texto</param>
    /// <param name="encryptCode">Codigo de encriptacion</param>
    /// <history>
    ///   [wtorres]  23/02/2016 Created
    /// </history>
    public static string Encrypt(string text)
    {
      StringBuilder sb = new StringBuilder(_encryptCode.Length);
      int n = _encryptCode.Length;
      for (int i = 1; i <= text.Length; i++)
      {
        int a = Convert.ToInt32(_encryptCode[((i % n) - n * Convert.ToInt32((i % n) == 0)) - 1]);
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
