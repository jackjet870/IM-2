using IM.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IM.Base.Converters
{
  public class DateAndTimeConverter : IValueConverter
  {
    /// <summary>
    /// Convierte un string a una hora o fecha valida
    /// </summary>
    /// <param name="value">valor a convertir</param>
    /// <param name="targetType">tipo de dato del control</param>
    /// <param name="parameter">ConverterParameter</param>
    /// <param name="culture">CultureInfo</param>
    /// <return>información valida para el control</returns>
    /// <history>
    /// [erosado] 26/08/2016  Created.
    /// </history>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      DateTime dateTimeValue = new DateTime();
      var isDateTime = DateTime.TryParse(value.ToString(),out dateTimeValue);

      //Si es una fecha valida
      if (isDateTime)
      {
        //Si la fecha es el valor default
        if (dateTimeValue == DateTime.MinValue) return "";
        
        //Si la fecha no es el valor default 
        switch (parameter.ToString())
        {
          case "DATE":
            return dateTimeValue.ToString("dd/MM/yyyy");
          case "TIME":
            return dateTimeValue.ToString("hh:mm tt");
          default:
            break;
        }
      }
      return "";
    }
    /// <summary>
    /// Convierte la informacion del Target al tipo de dato del objeto
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      DateTime dateTimeValue = new DateTime();
      var isDateTime = DateTime.TryParse(value.ToString(), out dateTimeValue);

      if (isDateTime)
      {
        //Si la fecha es el valor default
        if (dateTimeValue == DateTime.MinValue) return "";

        //Si la fecha no es el valor default 
        switch (parameter.ToString())
        {
          case "DATE":
            return dateTimeValue;
          case "TIME":
            //Revisamos que cumpla con el formato valido
            if (!ValidateHelper.IsValidTimeFormat(value.ToString())) return "";
            //Si cumple con el formato
            return dateTimeValue;
          default:
            break;
        }
      }
      return dateTimeValue;
    }
  }
}
