using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;

namespace IM.Model.Helpers
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
    public static void CopyProperties<T>(T objNew, T ObjOld, bool blnIsVirtual = false)
    {
      if (objNew != null && ObjOld != null)
      {
        var type = objNew.GetType();
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
    #region Compare Objects
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
    public static bool IsEquals<T>(T objNew, T ObjOld, bool blnIsVirtual = false)
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

    #region CompareList
    /// <summary>
    /// Compara que los items de ambas listas sean los mismos
    /// </summary>
    /// <param name="lstNew">Lista Nueva</param>
    /// <param name="lstOld">Lista Antigua</param>
    /// <returns>True. Contienen los mismos registro | False.Tienen registros diferentes</returns>
    /// <history>
    /// [emoguel] created 15/04/2016
    /// [emoguel] Modified--Ignora registros vacios y verifica si las lista son null
    /// </history>
    public static bool IsListEquals<T>(List<T> lstNew, List<T> lstOld,string id="")
    {
      if (lstNew != null && lstOld != null)
      {
        Type type = typeof(T);

        #region Obtener la llave primaria
        if (id == "")
        {
          EntityTypeBase entityTypeBase = EntityHelper.GetEntityTypeBase(type);//Obtenemos las propiedades de la entidad
          EdmMember edmMember = entityTypeBase.KeyMembers.FirstOrDefault();//Obtenemos la llave primaria
          id = edmMember.Name;
        }

        var lst1 = lstNew.Where(p => !lstOld.Any(p1 => type.GetProperty(id).GetValue(p1) == type.GetProperty(id).GetValue(p)) && type.GetProperty(id).GetValue(p) != null).ToList();
        var lst2 = lstOld.Where(p => !lstNew.Any(p1 => type.GetProperty(id).GetValue(p1) == type.GetProperty(id).GetValue(p)) && type.GetProperty(id).GetValue(p) != null).ToList();
        #endregion

        if (lst1.Count() > 0 || lst2.Count() > 0)
        {
          return false;
        }
      }
      return true;
    }
    #endregion
    #endregion

    #region ObjectToList
    /// <summary>
    /// Obtiene un objeto y lo converte a lista de objetos
    /// </summary>
    /// <param name="obj">objeto a convertir a lista</param>
    /// <history>
    /// [jorcanche] 13/Abr/2015 Created
    /// [ecanul]    14/Abr/2016 Modified. Metodo movido a Base.Helpers
    /// [wtorres]   15/Abr/2016 Modified. Ahora devuelve una lista tipeada
    /// </history>
    public static List<T> ObjectToList<T>(T obj)
    {
      var lst = new List<T>();
      lst.Add(obj);
      return lst;
    }

    #endregion
    
   
  }
}
