using System.Text.RegularExpressions;

namespace IM.Base.Helpers
{
  public class ValidateHelper
  {
    #region OnlyNumbers
    /// <summary>
    /// Valida que el texto sea sólo números un número
    /// </summary>
    /// <param name="text"></param>
    /// <returns>true es un númerico | false No es númerico </returns>
    /// <history>
    /// [Emoguel] created 02/03/2015
    /// </history>
    public static bool OnlyNumbers(string text)
    {
      Regex regex = new Regex("^[0-9]+$");
      return regex.IsMatch(text);
    }
    #endregion
  }
}
