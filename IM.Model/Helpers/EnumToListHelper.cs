using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace IM.Model.Helpers
{
  public class EnumToListHelper
  {
    /// <summary>
    /// Método para obtener su atributo Descripcion, y retornarlo como
    /// una cadena de texto.
    /// </summary>
    /// <param name="value"> Enumerador a convertir.</param>
    /// <returns>String</returns>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    public static string GetEnumDescription(Object value)
    {
      FieldInfo fi = value.GetType().GetField(value.ToString());

      DescriptionAttribute[] attributes =
          (DescriptionAttribute[])fi.GetCustomAttributes(
          typeof(DescriptionAttribute),
          false);

      if (attributes != null &&
          attributes.Length > 0)
        return attributes[0].Description;
      else
        return value.ToString();
    }

    /// <summary>
    /// Metodo para obtener Un diccionario a traves de un Enum con descripcion
    /// </summary>
    /// <typeparam name="T">Enum</typeparam>
    /// <returns> Dictionary<Enum, string></returns>
    /// <history>
    /// [aalcocer] 14/03/2016 Created
    /// </history>
    public static Dictionary<TEnum, string> GetList<TEnum>() where TEnum : struct
    {
      return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToDictionary(x => x, x => GetEnumDescription(x));
    }
  }
}