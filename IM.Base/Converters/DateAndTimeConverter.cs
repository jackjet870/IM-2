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
