﻿using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace IM.Base.Helpers
{
  public class ConvertHelper
  {
    #region CurrencyToStandar
    /// <summary>
    /// Cambiar el formato number currency a formato int
    /// </summary>
    /// <param name="textCurrency">texto a convertir</param>
    /// <returns>Numero en formato estandar</returns>
    /// <history>
    /// [emoguel] created 28/03/2016
    /// [emoguel] modified 09/08/2016
    /// </history>
    public static string DoubleCurrencyToStandar(string textCurrency)
    {      
      return new string(textCurrency.Where(c => char.IsNumber(c) || c=='.' ).ToArray());
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
      if (textCurrency.Length > 3)
      {
        textCurrency = textCurrency.Remove(textCurrency.Length - 3);
        textCurrency = new string(textCurrency.Where(s => char.IsDigit(s)).ToArray());
      }
      return textCurrency;
    }
    #endregion

    #region StringEmptyToNull
    /// <summary>
    /// Convierte una cadena vacia a un valor nulo
    /// </summary>
    /// <param name="strValue">Cadena a convertir nula si es vacilla</param>
    /// <returns></returns>
    public static string StringEmptyToNull(string strValue)
    {
      string stringEmptyOrNull = null;
      if(!String.IsNullOrEmpty(strValue))
      {
        stringEmptyOrNull = strValue;
      }
      return stringEmptyOrNull;
    }
    #endregion

  }
}
