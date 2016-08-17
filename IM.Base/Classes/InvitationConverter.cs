using System;
using System.Globalization;
using System.Windows.Data;


namespace IM.Base.Classes
{
  public class InvitationConverter:IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      decimal amount = 0;
      decimal.TryParse(values[0].ToString(), out amount);
      decimal received = 0;
      decimal.TryParse(values[1].ToString(), out received);

      return (amount - received).ToString();
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
