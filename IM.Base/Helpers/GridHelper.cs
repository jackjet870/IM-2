using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Controls;

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
    /// <param name="replaceStringNullOrWhiteSpace">Cambia las columnas string vacias a "-" </param>
    /// <returns>DataTable</returns>
    /// <history>
    /// [erosado] 12/Mar/2016  Created.
    /// [erosado] 17/Mar/2016  Se agrego la opcion para cambiar los tipos de datos de la Boolean a string,
    ///                        esto nos sirve para que en el reporte se muestren palomitas en lugar de la palabra "VERDADERO"
    /// [edgrodriguez] 19/Mar/2016 Modified. Opcion para cambiar los tipos de datos de Boolean a string
    ///                                      cambiando la palabra "True" por la palabra "Yes".
    ///                                      Se hizo modificaciones en el proceso de creacion del datatable.
    /// [aalcocer] 11/04/216 Modified. Opcion para reemplazar campos vacios.
    /// </history>
    public static DataTable GetDataTableFromGrid<T>(List<T> lst, bool changeDataTypeBoolToString = false, bool showCheckMark = true, bool replaceStringNullOrWhiteSpace = false)
    {
      DataTable table = new DataTable();
      List<PropertyDescriptor> properties = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>().ToList();
      List<int> columnsChanged = new List<int>();

      properties.ForEach(c =>
      {
        Type type = Nullable.GetUnderlyingType(c.PropertyType) ?? c.PropertyType;
        //Cambia las columnas boleanas a string
        if (changeDataTypeBoolToString && (type == typeof(bool) || type == typeof(bool?)))
        {
          table.Columns.Add(c.Name, typeof(string));
          columnsChanged.Add(properties.IndexOf(c));
        }
        else if (type == typeof(string))
          table.Columns.Add(new DataColumn { ColumnName = c.Name, DataType = type, DefaultValue = (replaceStringNullOrWhiteSpace || showCheckMark) ? "" : "-" });
        else
          table.Columns.Add(c.Name, type);
      });

      lst.ForEach(c =>
      {
        DataRow newRow = table.NewRow();
        var values = properties.Select(v => v.GetValue(c) ?? DBNull.Value).ToArray();

        columnsChanged.ForEach(col =>
        {
          if (showCheckMark)
            values[col] = (values[col].ToString().ToLower() == "true") ? "ü" : "";
          else
            values[col] = (values[col].ToString().ToLower() == "true") ? "Yes" : " ";
        });

        if (replaceStringNullOrWhiteSpace)
        {
          for (int i = 0; i < values.Length; i++)
            if (string.IsNullOrWhiteSpace(values[i].ToString()))
              values[i] = "-";
        }

        newRow.ItemArray = values;
        table.Rows.Add(newRow);
      });

      return table;
    }

    #endregion GetDatatableFromGrid

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

    #endregion GetAnonymousObject

    #region SelectRow

    /// <summary>
    /// Selecciona un registro del grid
    /// </summary>
    /// <param name="grid">Grid para seleccionar el registro</param>
    /// <param name="nIndex">index del registro</param>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// [emoguel] modified 23/03/2016 Se agregó la validacion HasItems
    /// </history>
    public static void SelectRow(DataGrid grid, int nIndex)
    {
      if (nIndex > -1)
      {
        grid.Focus();
        grid.SelectedIndex = nIndex;
        if (grid.SelectedItem != null)
        {
          grid.ScrollIntoView(grid.SelectedItem);
          grid.CurrentCell = new DataGridCellInfo(grid.SelectedItem, grid.Columns[0]);
        }
      }
    }

    #endregion SelectRow

    #region ToPivot

    /// <summary>
    /// Obtiene una lista de arrays con los valores de la tabla
    /// con la estructura proporcionada.
    /// </summary>
    /// <returns> List<object[]> </returns>
    /// <history>
    /// [edgrodriguez] created 28/03/2016
    /// </history>
    public static List<object[]> ToPivot<T, TColumn, TRow, TData>(
this IEnumerable<T> source,
Func<T, TColumn> columnSelector,
Expression<Func<T, TRow>> rowSelector,
Func<IEnumerable<T>, TData> dataSelector)
    {
      DataTable dt = new DataTable();
      var arr = new List<object>();
      var cols = new List<string>();
      var pivotrow = rowSelector.ReturnType.GetProperties().Select(c => c.Name).ToList();
      var pivotcolumns = source.Select(columnSelector).Where(c => c != null).Distinct();

      cols = (pivotrow).Concat(pivotcolumns.Select(x => x.ToString())).ToList();

      var rows = source.GroupBy(rowSelector.Compile())
                       .Select(rowGroup => new
                       {
                         Key = rowGroup.Key,
                         Values = pivotcolumns.GroupJoin(
                               rowGroup,
                               c => c,
                           r => columnSelector(r),
                           (c, columnGroup) => dataSelector(columnGroup))
                       });

      List<object[]> values = new List<object[]>();

      rows.ToList().ForEach(row =>
      {
        var keyValues = row.Key.GetType().GetProperties().Select(c => c.GetValue(row.Key) ?? null);
        var valuesProperties = row.Values.ToList();
        var items = valuesProperties.SelectMany(c => c.GetType().GetProperties().Select(d => d.GetValue(c) ?? null)).ToList();
        items.InsertRange(0, keyValues);
        values.Add(items.ToArray());
      });

      return values;
    }

    #endregion ToPivot
  }
}