using System;
using System.Globalization;
using System.Windows.Data;

namespace IM.Base.Converters
{
  public class SubstringOperationConverter : IValueConverter
  {
    /// <summary>
    /// Obtiene el string con el maxlengh correspondiente a la propiedad.
    /// </summary>
    /// <param name="value">Valor asignado al control</param>
    /// <param name="targetType">Tipo de dato que tiene el value del control</param>
    /// <param name="trueValue">ConverterParameter</param>
    /// <param name="culture">CultureInfo</param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 23/08/2016 Created
    /// </history>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!string.IsNullOrEmpty(value as string))
      {
        // Verificamos que el string no sea mayor a 20
        if (value.ToString().Length > 20)
        {
          return value.ToString().Substring(0, 20).Trim();
        }
        else
        {
          return value;
        }

      }
      else
      {
        return value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">Valor asignado al control</param>
    /// <param name="targetType">Tipo de dato que tiene el value del control</param>
    /// <param name="trueValue">ConverterParameter</param>
    /// <param name="culture">CultureInfo</param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 23/08/2016 Created
    /// </history>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value;
    }
  }
}
