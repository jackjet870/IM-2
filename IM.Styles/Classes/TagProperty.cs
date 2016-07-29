﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IM.Styles.Classes
{
  /// <summary>
  /// Esta clase fue creada para agregar la propiedad Tag a controles que no la tengan 
  /// </summary>
  /// <history>
  /// [erosado] 29/07/2016
  /// </history>
  public class TagPropertyClass
  {
    public static readonly DependencyProperty TagProperty = DependencyProperty.RegisterAttached(
       "Tag",
       typeof(object),
       typeof(TagPropertyClass),
       new FrameworkPropertyMetadata(null));

    public static object GetTag(DependencyObject dependencyObject)
    {      
      return (dependencyObject.GetValue(TagProperty)==null)?"": dependencyObject.GetValue(TagProperty);
    }

    public static void SetTag(DependencyObject dependencyObject, object value)
    {
      dependencyObject.SetValue(TagProperty, value);
    }
  }
}
