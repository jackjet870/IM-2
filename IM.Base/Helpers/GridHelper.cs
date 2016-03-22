using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq.Expressions;
using System.Linq;
using System.Dynamic;

namespace IM.Base.Helpers
{
  public static class GridHelper
  {
    #region GetDatatableFromGrid

    /// <summary>
    /// Convierte una lista tipada a un DataTable
    /// </summary>
    /// <typeparam name="T">Tipo de dato de la lista</typeparam>
    /// <param name="changeDataTypeBoolToString">true : Cambia las columnas boleanas a string - false: las deja como boleanas</param>
    /// <param name="lst">Lista tipada</param>
    /// <param name="showCheckMark"> false: Cambia las columnas booleanas a string y convierte la palabra </param>
    /// <returns>DataTable</returns>
    /// <history>
    /// [erosado] 12/Mar/2016  Created.
    /// [erosado] 17/Mar/2016  Se agrego la opcion para cambiar los tipos de datos de la Boolean a string, 
    ///                        esto nos sirve para que en el reporte se muestren palomitas en lugar de la palabra "VERDADERO"
    /// [edgrodriguez] 19/Mar/2016 Modified. Opcion para cambiar los tipos de datos de Boolean a string
    ///                                      cambiando la palabra "True" por la palabra "Yes".
    ///                                      Se hizo modificaciones en el proceso de creacion del datatable.
    /// </history>
    public static DataTable GetDataTableFromGrid<T>(List<T> lst, bool changeDataTypeBoolToString = false, bool showCheckMark=true)
    {      
      if (changeDataTypeBoolToString)
      {
        List<PropertyDescriptor> properties =
            TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>().ToList();
        List<int> columnsChanged = new List<int>();
        DataTable table = new DataTable();
        properties.ForEach(c =>
        {
          var type = Nullable.GetUnderlyingType(c.PropertyType) ?? c.PropertyType;
          if (type == typeof(bool) || type == typeof(bool?))
          {
            table.Columns.Add(c.Name, typeof(string));
            columnsChanged.Add(properties.IndexOf(c));
          }
          else
          {
            if (type == typeof(string))
              table.Columns.Add(new DataColumn { ColumnName = c.Name, DataType = type, DefaultValue = (showCheckMark) ? "" : "-" });
            else
              table.Columns.Add(c.Name, type);
          }
        });

        lst.ForEach(c =>
        {
          DataRow newRow = table.NewRow();
          var values = properties.Select(v => v.GetValue(c)).ToArray();
          columnsChanged.ForEach(col =>
          {
            if (showCheckMark)
              values[col] = (values[col].ToString().ToLower() == "true") ? "ü" : "";
            else
              values[col] = (values[col].ToString().ToLower() == "true") ? "Yes" : " ";
          });
          newRow.ItemArray = values;
          table.Rows.Add(newRow);
        });

        return table;
      }
      else
      {
        #region Convertimos lista tipada en DataTable
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
        #endregion

        return table;
      }
    }
    #endregion

    #region ToPivotArray
    /// <summary>
    /// Obtener una tabla pivot en base a una lista de entidades.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 14/03/2016  Created.
    /// </history>
    public static dynamic[] ToPivotArray<T, TColumn, TRows, TData>(
this IEnumerable<T> source,
Func<T, TColumn> columnSelector,
Expression<Func<T, TRows>> rowSelector,
Func<IEnumerable<T>, TData> dataSelector)
    {
      bool multipleColumnsGroup = false;
      var arr = new List<object>();
      var cols = new List<string>();
      //Obtengo nombres de las columnas para agrupar.
      IEnumerable<string> rowName;

      try
      {
        rowName = ((MemberExpression)rowSelector.Body).Member.Name.Split(',');
      }
      catch
      {
        multipleColumnsGroup = true;
        rowName = rowSelector.ReturnType.GetProperties().Select(c => c.Name);
      }

      //Obtengo las nuevas columnas.El Pivot.
      var columns = source.Select(columnSelector).Distinct();

      //Concateno los arreglos de columnas.
      cols = rowName.Concat(columns.Select(x => x.ToString())).ToList();


      //Se realiza la agrupacion.
      var rows = source.GroupBy(rowSelector.Compile())
                       .Select(rowGroup => new
                       {
                         Key = rowGroup.Key,
                         Values = columns.GroupJoin(
                               rowGroup,
                               c => c,
                               r => columnSelector(r),
                               (c, columnGroup) => dataSelector(columnGroup))
                       }).ToArray();




      foreach (var row in rows)
      {
        //Se obtienen los valores de cada fila.        
        var items = row.Values.Cast<object>().ToList();

        if (multipleColumnsGroup)
        {
          //Obtengo los valores de las columnas agrupadas.
          Type type = row.Key.GetType();
          List<string> keyPropertyName = type.GetProperties().Select(c => c.Name).ToList();
          List<Object> keyValues = keyPropertyName.Select(c => type.GetProperty(c).GetValue(row.Key, null)).ToList();

          //Realizo la insercion en la lista principal de valores.
          items.InsertRange(0, keyValues);
        }
        else
          items.Insert(0, row.Key);

        var obj = GetAnonymousObject(cols, items);
        arr.Add(obj);
      }
      //Retorno el arreglo de rows.
      return arr.ToArray();
    }
    #endregion

    #region GetAnonymousObject
    /// <summary>
    /// 
    /// </summary>
    /// <history>
    /// [edgrodriguez] 14/03/2016  Created.
    /// </history>
    private static dynamic GetAnonymousObject(IEnumerable<string> columns, IEnumerable<object> values)
    {
      IDictionary<string, object> eo = new ExpandoObject() as IDictionary<string, object>;
      int i;

      for (i = 0; i < columns.Count(); i++)
      {
        eo.Add(columns.ElementAt(i), values.ElementAt(i));
      }
      return eo;
    } 
    #endregion

    #region SelectRow
    /// <summary>
    /// Selecciona un registro del grid
    /// </summary>
    /// <param name="grid">Grid para seleccionar el registro</param>
    /// <param name="nIndex">index del registro</param>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// </history>
    public static void SelectRow(DataGrid grid, int nIndex)
    {
      grid.Focus();
      grid.SelectedIndex = nIndex;
      grid.ScrollIntoView(grid.SelectedItem);
      grid.CurrentCell = new DataGridCellInfo(grid.SelectedItem, grid.Columns[0]);
    }
    #endregion
  }
}
