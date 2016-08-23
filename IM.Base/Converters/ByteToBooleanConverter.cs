using System;
using System.Globalization;
using System.Windows.Data;

namespace IM.Base.Converters
{
  public class ByteToBoleanValueConverterHelper : IValueConverter
  {
    #region Convert
    /// <summary>
    /// Convierte byte a Boolean para usar un radioButton
    /// </summary>
    /// <param name="value">Valor asignado al control</param>
    /// <param name="targetType">Tipo de dato que tiene el value del control</param>
    /// <param name="trueValue">ConverterParameter</param>
    /// <param name="culture">CultureInfo</param>
    /// <history>
    /// [erosado] 15/04/2016  Created
    /// </history>
    public object Convert(object value, Type targetType, object trueValue, CultureInfo culture)
    {
      return value.Equals(byte.Parse(trueValue.ToString()));
    }
    #endregion

    #region ConvertBack
    /// <summary>
    /// Convierte un bool en el ConverterParameter
    /// </summary>
    /// <param name="value">Valor asignado al control</param>
    /// <param name="targetType">Tipo de dato que tiene el value del control</param>
    /// <param name="trueValue">ConverterParameter</param>
    /// <param name="culture">CultureInfo</param>
    /// <history>
    /// [erosado] 15/04/2016  Created
    /// </history>
    public object ConvertBack(object value, Type targetType, object trueValue, CultureInfo culture)
    {
      return value.Equals(true) ? trueValue : Binding.DoNothing;
    }
    #endregion
  }
}
