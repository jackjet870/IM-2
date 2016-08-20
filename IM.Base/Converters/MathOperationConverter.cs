using System;
using System.Globalization;
using System.Windows.Data;


namespace IM.Base.Converters
{
  public class MathOperationConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {

      decimal amount = 0;
      decimal.TryParse(values[0].ToString(), out amount);
      decimal received = 0;
      decimal.TryParse(values[1].ToString(), out received);
      string result = "";


      switch (parameter.ToString())
      {
        case "SUM":
          result = (amount + received).ToString();
          break;

        case "REST":
          result = (amount - received).ToString();
          break;

        case "MULT":
          result = (amount * received).ToString();
          break;
        default:
          break;
      }
      return result;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
