using System.ComponentModel;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections;

namespace IM.Base.Helpers
{
  public class ObjectHelper
  {
    #region CopyProperties
    /// <summary>
    /// Copia el valor de los atributos de un objeto a uno nuevo
    /// para no modificar directamente sobre el datacontext
    /// </summary>
    /// <param name="objNew">Objeto para llenar con los valores</param>
    /// <param name="ObjOld">Objeto a copiar los valores</param>
    /// <history>
    /// [emoguel] created 12/03/2016
    /// [emoguel] modified se agrego una bandera para clonar las propiedades "Virtual"
    /// </history>
    public static void CopyProperties <T> (T objNew, T ObjOld,bool blnIsVirtual=false)
    {
      if (objNew != null && ObjOld != null)
      {
        var type = typeof(T);
        var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        if (!blnIsVirtual)
        {
          property = property.Where(pi => !pi.GetMethod.IsVirtual).ToList();
        }

        foreach (PropertyInfo pi in property)
        {
          var oldValue = type.GetProperty(pi.Name).GetValue(ObjOld, null);
          type.GetProperty(pi.Name).SetValue(objNew, oldValue);
        }
      }
    }
    #endregion

    #region Equals
    /// <summary>
    /// Compara los atributos de  objetos para saber si sus valores son iguales
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objNew"></param>
    /// <param name="ObjOld"></param>
    /// <returns>True. Son iguales | false. No son iguales</returns>
    /// <history>
    /// [emoguel] created 28/03/2016
    /// [emoguel] modified se agrego una bandera para clonar las propiedades "Virtual"
    /// </history>
    public static bool IsEquals<T>(T objNew, T ObjOld,bool blnIsVirtual=false)
    {
      if (objNew != null && ObjOld != null)
      {
        var type = typeof(T);
        var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        if (!blnIsVirtual)
        {
          property = property.Where(pi => !pi.GetMethod.IsVirtual).ToList();
        }
        if (property.Count() > 0)
        {
          #region Recorrer y comparar las propiedades
          foreach (PropertyInfo pi in property)
          {
            var newValue = type.GetProperty(pi.Name).GetValue(objNew, null);
            var oldValue = type.GetProperty(pi.Name).GetValue(ObjOld, null);
            if (pi.PropertyType == typeof(string))//Validar los strings cuando sea null && ""
            {
              if (newValue != oldValue && !newValue.Equals(oldValue) && !(string.IsNullOrWhiteSpace((string)newValue) && string.IsNullOrWhiteSpace((string)oldValue)))
              {
                return false;
              }
            }
            else if (newValue != oldValue && (newValue == null || !newValue.Equals(oldValue)))
            {
              return false;
            }

          }
          #endregion
          return true;
        }
        else
        {
          return false;
        }
      } 
      return false;
    }
    #endregion
  }
}
