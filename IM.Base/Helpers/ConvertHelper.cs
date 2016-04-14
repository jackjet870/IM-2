using System;
using System.Globalization;
using System.Linq;

namespace IM.Base.Helpers
{
  public class ConvertHelper
  {
    #region CurrencyToStandar
    /// <summary>
    /// Cambiar el formato number currency a formato int
    /// </summary>
    /// <param name="textCurrency"></param>
    /// <returns>Numero en formato estandar</returns>
    /// <history>
    /// [emoguel] created 28/03/2016
    /// </history>
    public static string DoubleCurrencyToStandar(string textCurrency)
    {
      double dRes = 0;      
      return double.TryParse(textCurrency, NumberStyles.Currency, CultureInfo.CurrentCulture, out dRes) ? dRes.ToString() : "0";
    }

    /// <summary>
    /// Cambiar el formato number currency a formato decimal
    /// </summary>
    /// <param name="textCurrency"></param>
    /// <returns>Numero en formato estandar</returns>
    /// <history>
    /// [emoguel] created 28/03/2016
    /// </history>
    public static string IntCurrencyToStandar(string textCurrency)
    {
      textCurrency = textCurrency.Remove(textCurrency.Length - 3);
      textCurrency = new string(textCurrency.Where(s => char.IsDigit(s)).ToArray());
      return textCurrency;
    }
    #endregion
  }
}
