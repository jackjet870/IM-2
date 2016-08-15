using System;
using System.Windows;

namespace IM.Styles.Classes
{
  /// <summary>
  /// Esta clase fue creada para agregar la propiedad Precision a controles que no la tengan 
  /// </summary>
  /// <history>
  /// [emoguel] 06/08/2016
  /// </history>
  public class PrecisionPropertyClass
  {
    public static readonly DependencyProperty PrecisionProperty = DependencyProperty.RegisterAttached(
       "Precision",
       typeof(string),
       typeof(PrecisionPropertyClass),
       new FrameworkPropertyMetadata(null));

    #region GetPrecision
    /// <summary>
    /// Obtiene el valor de Precision
    /// </summary>
    /// <param name="dependencyObject">Control</param>
    /// <returns>string</returns>
    /// <history>
    /// [emoguel] 08/08/2016
    /// </history>
    public static string GetPrecision(DependencyObject dependencyObject)
    {
      string result = ((dependencyObject.GetValue(PrecisionProperty) == null) ? "" : dependencyObject.GetValue(PrecisionProperty).ToString());
      var arrResult = result.Split(',');
      if (arrResult.Length == 0)
      {
        result = "0,0";
      }
      else if (arrResult.Length == 1)
      {
        int nInts = 0;
        result = (Int32.TryParse(arrResult[0], out nInts)) ? nInts + ",0" : "0,0";
      }
      else if (arrResult.Length >= 2)
      {
        int nInts = 0;
        int nDecimals = 0;
        result = ((Int32.TryParse(arrResult[0], out nInts)) ? nInts.ToString() : "0") +","+ ((Int32.TryParse(arrResult[1], out nDecimals)) ? nDecimals.ToString() : "0");
      }
      return result;
    }
    #endregion

    #region SetPrecision
    /// <summary>
    /// Setea la propiedad FormatInput
    /// </summary>
    /// <param name="dependencyObject">Control</param>
    /// <param name="value">Value</param>
    /// <history>
    /// [emoguel] 08/08/2016
    /// </history>
    public static void SetPrecision(DependencyObject dependencyObject, string value)
    {
      dependencyObject.SetValue(PrecisionProperty, value);
    } 
    #endregion
  }
 }
