using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace IM.Model.Helpers
{
  public static class EnumToListHelper
  {
    #region GetEnumDescription
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
    #endregion

    #region GetList
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
    #endregion

    #region StringToEnum
    /// <summary>
    /// Metodo para convertir un String con el valor de un enum a Enum
    /// </summary>
    /// <typeparam name="T">Tipo de Enumerado</typeparam>
    /// <param name="value">String con el valor a convertir</param>
    /// <history>
    /// [ecanul]   19/Abr/2016 Created
    /// [wtorres]  16/Jul/2016 Modified. En lugar de usar T, ahora se usa TEnum
    /// </history>
    public static TEnum StringToEnum<TEnum>(this string value, bool ignoreCase = true)
    {
      return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
    } 
    #endregion

    #region EnumListToString
    /// <summary>
    /// Metodo para convertir una lista de enumerados con descripcion a string. Concatenando las descripciones
    /// con una coma.
    /// </summary>
    /// <typeparam name="T">Enum</typeparam>
    /// <returns> string </returns>
    /// <history>
    /// [edgrodriguez] 05/07/2016 Created
    /// </history>
    public static string EnumListToString<TEnum>(this List<TEnum> EnumList)
    {
      return String.Join(",", EnumList.Cast<TEnum>().Select(x => GetEnumDescription(x)));
    } 
    #endregion
  }
}