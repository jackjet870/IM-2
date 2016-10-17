using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IM.Styles.Classes
{
  /// <summary>
  /// Esta clase fue creada para agregar la propiedad FormatInput a controles que no la tengan 
  /// </summary>
  /// <history>
  /// [erosado] 30/07/2016
  /// </history>
  public class FormatInputPropertyClass
  {
    public static readonly DependencyProperty FormatInputProperty = DependencyProperty.RegisterAttached(
       "FormatInput",
       typeof(Enums.EnumFormatInput),
       typeof(FormatInputPropertyClass),
       new FrameworkPropertyMetadata(null));
    /// <summary>
    /// Obtiene el valor de FormatInput
    /// </summary>
    /// <param name="dependencyObject">Control</param>
    /// <returns>EnumFormatInput</returns>
    /// <history>
    /// [erosado] 30/07/2016
    /// </history>
    public static Enums.EnumFormatInput GetFormatInput(DependencyObject dependencyObject)
    {      
      return (dependencyObject.GetValue(FormatInputProperty) == null) ? Enums.EnumFormatInput.Text : (Enums.EnumFormatInput)dependencyObject.GetValue(FormatInputProperty);
    }
    /// <summary>
    /// Setea la propiedad FormatInput
    /// </summary>
    /// <param name="dependencyObject">Control</param>
    /// <param name="value">Value</param>
    /// <history>
    /// [erosado] 30/07/2016
    /// </history>
    public static void SetFormatInput(DependencyObject dependencyObject, Enums.EnumFormatInput value)
    {
      dependencyObject.SetValue(FormatInputProperty, value);
    }
  }
}

