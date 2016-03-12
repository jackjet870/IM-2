using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;

namespace IM.Base.Helpers
{
  public class GridHelper
  {
    /// <summary>
    /// Convierte una lista tipada a un dataTable
    /// </summary>
    /// <typeparam name="T">Tipo de objeto de la lista</typeparam>
    /// <param name="lst">Lista tipada</param>
    /// <returns>DataTable</returns>
    /// <history>
    /// [erosado] 12/03/2016  Created.
    /// </history>
    public static DataTable GetDataTableFromObjectList<T>(List<T> lst)
    {
      PropertyDescriptorCollection properties =
           TypeDescriptor.GetProperties(typeof(T));
      DataTable table = new DataTable();
      foreach (PropertyDescriptor prop in properties)
        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
      foreach (T item in lst)
      {
        DataRow row = table.NewRow();
        foreach (PropertyDescriptor prop in properties)
          row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        table.Rows.Add(row);
      }
      return table;
    }
  }
}
