using System;
using System.Globalization;

namespace IM.Base.Helpers
{
  public class ConvertHelper
  {
    #region CurrencyToStandar
    /// <summary>
    /// Cambiar el formato number currency a formato normal
    /// </summary>
    /// <param name="textCurrency"></param>
    /// <returns>Numero en formato estandar</returns>
    /// <history>
    /// [emoguel] created 28/03/2016
    /// </history>
    public static string CurrencyToStandar(string textCurrency)
    {
      int nRes;
      return Int32.TryParse(textCurrency, NumberStyles.Currency, CultureInfo.CurrentCulture, out nRes) ? nRes.ToString("N0") : "0";
    }
    #endregion
  }
}
