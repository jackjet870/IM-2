using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Clase que contiene las funciones comunes de tablas
  /// </summary>
  public static class TableHelper
  {
    #region GetDataTableFromList

    /// <summary>
    /// Convierte una lista tipada a un DataTable
    /// </summary>
    /// <typeparam name="T">Tipo de dato de la lista</typeparam>
    /// <param name="changeDataTypeBoolToString">true : Cambia las columnas boleanas a string - false: las deja como boleanas</param>
    /// <param name="lst">Lista tipada</param>
    /// <param name="showCheckMark"> false: Cambia las columnas booleanas a string y convierte la palabra </param>
    /// <param name="replaceStringNullOrWhiteSpace">Cambia las columnas string vacias a "-" </param>
    /// <param name="sortFields">Ordena los campos segun la direccion especificada. "Campo1 asc, Campo2 desc"</param>
    /// <returns>DataTable</returns>
    /// <history>
    /// [erosado] 12/Mar/2016  Created.
    /// [erosado] 17/Mar/2016  Se agrego la opcion para cambiar los tipos de datos de la Boolean a string,
    ///                        esto nos sirve para que en el reporte se muestren palomitas en lugar de la palabra "VERDADERO"
    /// [edgrodriguez] 19/Mar/2016 Modified. Opcion para cambiar los tipos de datos de Boolean a string
    ///                                      cambiando la palabra "True" por la palabra "Yes".
    ///                                      Se hizo modificaciones en el proceso de creacion del datatable.
    /// [aalcocer] 11/Abr/2016 Modified. Opcion para reemplazar campos vacios.
    /// [wtorres]  15/Abr/2016 Modified. Movido desde el GridHelper y renombrado. Antes se llamaba GetDatatableFromGrid
    /// </history>
    public static DataTable GetDataTableFromList<T>(List<T> lst, bool changeDataTypeBoolToString = false,
      bool showCheckMark = true, bool replaceStringNullOrWhiteSpace = false)
    {
      DataTable table = new DataTable();
      Type t = lst[0].GetType();
      List<PropertyDescriptor> properties = TypeDescriptor.GetProperties(t).Cast<PropertyDescriptor>().ToList();

      properties.ForEach(propInfo =>
      {
        Type type = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
        //Cambia las columnas booleanas a string
        if (changeDataTypeBoolToString && (type == typeof(bool) || type == typeof(bool?)))
        {
          table.Columns.Add(propInfo.Name, typeof(string));
        }
        else if (type == typeof(string))
          table.Columns.Add(new DataColumn { ColumnName = propInfo.Name, DataType = type, DefaultValue = (replaceStringNullOrWhiteSpace || showCheckMark) ? "" : "-" });
        else
          table.Columns.Add(propInfo.Name, type);
      });

      lst.ForEach(c =>
      {
        object[] values = new object[properties.Count];
        for (int i = 0; i < values.Length; i++)
        {
          object value = properties[i].GetValue(c) ?? DBNull.Value;
          Type type = Nullable.GetUnderlyingType(properties[i].PropertyType) ?? properties[i].PropertyType;
          if (changeDataTypeBoolToString && (type == typeof(bool) || type == typeof(bool?)))
            value = showCheckMark ? (value.ToString().ToLower() == "true" ? "ü" : "") : (value.ToString().ToLower() == "true" ? (object)"Yes" : DBNull.Value);
          if (replaceStringNullOrWhiteSpace && type == typeof(string) && string.IsNullOrWhiteSpace(value.ToString()))
            value = "-";
          values[i] = value;
        }
        table.Rows.Add(values);
      });

      return table;
    }

    #endregion
  }
}