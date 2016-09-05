using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Linq.Dynamic;

namespace IM.Model.Helpers
{
  public static class ObjectHelper
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
    /// TODO: Eliminar este metodo cuando todos se migren a su sobre carga que retorna el objeto.
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
    /// <summary>
    /// Clona un objeto eliminando las referencias con el objeto original.
    /// </summary>
    /// <typeparam name="T">Tipo de objeto</typeparam>
    /// <param name="ObjToClone">Objeto que vamos a clonar</param>
    /// <param name="includeVirtualProperties">True si deseamos copiar las propiedades virtuales | False si NO </param>
    /// <returns>Objecto clon</returns>
    /// <history>
    /// [erosado, emoguel]  04/08/2016  Created.
    /// </history>
    public static T CopyProperties<T>(T ObjToClone, bool includeVirtualProperties = false) where T : class, new()
    {
      T objNew = new T();
      if (ObjToClone != null)
      {
        Type type = objNew.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        if (!includeVirtualProperties)
        {
          properties = properties.Where(pi => !pi.GetMethod.IsVirtual).ToList();
        }
        foreach (PropertyInfo pi in properties)
        {
          var oldValue = type.GetProperty(pi.Name).GetValue(ObjToClone, null);
          type.GetProperty(pi.Name).SetValue(objNew, oldValue);
        }
      }
      return objNew;
    }
    /// <summary>
    /// Clona una lista eliminando las referencias con la lista original.
    /// </summary>
    /// <typeparam name="T">Lista<T></typeparam>
    /// <param name="lstToClone"></param>
    /// <returns>Lista clon</returns>
    /// <history>
    /// [erosado, emoguel]  04/08/2016  Created.
    /// </history>
    public static List<T> CopyProperties<T>(List<T> lstToClone) where T : class, new()
    {
      List<T> lstClone = new List<T>();
      if (lstToClone != null && lstClone != null)
      {
        lstToClone.ForEach(obj =>
        {
          var objClone = CopyProperties(obj);
          lstClone.Add(objClone);
        });
      }
      return lstClone;
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
            TypeCode typeCode = Type.GetTypeCode(Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);
            #region TypeCode
            switch (typeCode)
            {
              case TypeCode.String:
              case TypeCode.Char:
                {
                  newValue = (newValue == null) ? "" : newValue;
                  oldValue = (oldValue == null) ? "" : oldValue;
                  break;
                }
              case TypeCode.Boolean:
                {
                  newValue = (newValue == null) ? true : newValue;
                  oldValue = (oldValue == null) ? true : oldValue;
                  break;
                }


              case TypeCode.Int16:
              case TypeCode.Int32:
              case TypeCode.Int64:
                {
                  newValue = (newValue == null) ? -1 : newValue;
                  oldValue = (oldValue == null) ? -1 : oldValue;
                  break;
                }

              case TypeCode.Byte:
              case TypeCode.Decimal:
              case TypeCode.Double:
              case TypeCode.SByte:
              case TypeCode.UInt16:
              case TypeCode.UInt32:
              case TypeCode.UInt64:
                {
                  newValue = (newValue == null) ? -1 : newValue;
                  oldValue = (oldValue == null) ? -1 : oldValue;
                  break;
                }
              case TypeCode.DateTime:
                {
                  newValue = (newValue == null) ? new DateTime() : newValue;
                  oldValue = (oldValue == null) ? new DateTime() : oldValue;
                  break;
                }
            }

            #endregion
            if (newValue != oldValue && !newValue.Equals(oldValue))
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
    public static bool IsListEquals<T>(List<T> lstNew, List<T> lstOld, string id = "")
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

    #region AreAnyDuplicates
    /// <summary>
    /// Valida que no se repitan los elementos en una lista
    /// </summary>
    /// <typeparam name="T">Object</typeparam>
    /// <param name="list">List<Object></param>
    /// <returns>True Hay duplicados| False no hay</returns>
    /// <history>
    /// [erosado] 08/08/2016  Created.
    /// </history>
    public static bool AreAnyDuplicates<T>(List<T> list, string specificField = "")
    {
      return list.GroupBy(e => e.GetType().GetProperty(specificField).GetValue(e)).Any(x => x.Count() > 1);
    }
    /// <summary>
    ///  Valida que no se repitan los elementos en una lista
    /// </summary>
    /// <typeparam name="T">Object</typeparam>
    /// <param name="list">List<Object></param>
    /// <param name="lstFieldName"> Field Name List</param></param>
    /// <param name="blnWithAnd">True. Compara todos los campos que vienen en lstFieldName | False. compara campo por campo de que los vienen en lstFieldName</param>
    /// <returns>string message Hay duplicados| string.Empty no hay duplicados</returns>
    /// <history>
    /// [erosado] 09/08/2016  Created.
    /// </history>
    public static string AreAnyDuplicates<T>(List<T> list, List<string> lstFieldName, bool blnWithAnd = false)
    {
      bool _isValid = false;
      string _message = string.Empty;
      if (blnWithAnd)
      {
        if (list.Any() && lstFieldName.Any())
        {
          Type type = list.GetType();
          List<T> lstResults = new List<T>();
          List<PropertyInfo> properties = type.GetProperties(BindingFlags.CreateInstance | BindingFlags.DeclaredOnly).Where(pi => pi.GetMethod.IsVirtual && lstFieldName.Contains(pi.Name)).ToList();
          foreach (var pi in properties)//Recorremos la lista de 
          {
            if (_isValid) { break; }//Romper el ciclo
            var value = type.GetProperty(pi.Name).GetValue(pi);
            foreach (var item in list)
            {
              lstResults = list.Where(property => type.GetProperty(pi.Name).GetValue(property) == value ||
                                      (value != null && value.Equals(type.GetProperty(pi.Name).GetValue(property)))).ToList();
              if (lstResults.Count > 1)
              {
                _isValid = true;
                _message = $"We found a duplicate element on {pi.Name} column";
              }
            }
          }
        }
      }
      else
      {
        if (list.Any() && lstFieldName.Any())
        {
          foreach (var item in lstFieldName)
          {
            _isValid = list.GroupBy(e => e.GetType().GetProperty(item).GetValue(e)).Any(x => x.Count() > 1);

            if (_isValid)
            {
              _message = $"We found a duplicate element on {item} column";
              break;
            }
          }
        }
      }
      return _message;
    }
    #endregion

    public static List<T> orderListBy<T>(this List<T> lst, string Order)
    {
      lst = lst.OrderBy(Order).ToList();
      return lst;
    } 
  }
}
