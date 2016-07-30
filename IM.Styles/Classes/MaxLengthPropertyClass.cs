using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IM.Styles.Classes
{
  /// <summary>
  /// Esta clase fue creada para agregar la propiedad MaxLength a controles que no la tengan 
  /// </summary>
  /// <history>
  /// [erosado] 30/07/2016
  /// </history>
  public class MaxLengthPropertyClass
  {
    public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.RegisterAttached(
      "MaxLength",
      typeof(object),
      typeof(MaxLengthPropertyClass),
      new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Obtiene el valor del MaxLength
    /// </summary>
    /// <param name="dependencyObject">Control</param>
    /// <returns>Int MaxLength</returns>
    /// <history>
    /// [erosado] 30/07/2016
    /// </history>
    public static int GetMaxLength(DependencyObject dependencyObject)
    {
      return (dependencyObject.GetValue(MaxLengthProperty) == null) ? 0: Convert.ToInt32(dependencyObject.GetValue(MaxLengthProperty));
    }
    /// <summary>
    /// Setea la propiedad MaxLength
    /// </summary>
    /// <param name="dependencyObject">Control</param>
    /// <param name="value">Value</param>
    /// <history>
    /// [erosado] 30/07/2016
    /// </history>
    public static void SetMaxLength(DependencyObject dependencyObject, int value)
    {
      dependencyObject.SetValue(MaxLengthProperty, value);
    }
  }
}
