using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
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
    /// <returns>DataTable</returns>
    /// <history>
    /// [erosado] 12/Mar/2016  Created.
    /// [erosado] 17/Mar/2016  Se agrego la opcion para cambiar los tipos de datos de la Boolean a string, 
    ///                        esto nos sirve para que en el reporte se muestren palomitas en lugar de la palabra "VERDADERO"
    /// </history>
    public static DataTable GetDataTableFromGrid<T>(bool changeDataTypeBoolToString, List<T> lst)
    {
      if (changeDataTypeBoolToString)
      {
        #region Covertimos lista tipada en DataTable
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

        #region Cambio de DataType Clonedt
        //Copiamos la estructura de table y le cambiamos los DataType Boolean a string
        List<int> columnsChanged = new List<int>();
        DataTable cloneDt = table.Clone();
        int contColumn = 0;
        foreach (DataColumn item in table.Columns)
        {
          if (item.DataType == typeof(Boolean))
          {
            cloneDt.Columns[contColumn].DataType = typeof(string);
            columnsChanged.Add(contColumn);
          }
          contColumn++;
        }
        #endregion

        #region Agregamos la informacion de Table a Clonedt
        //LLenamos CLonedt Con la informacion de table
        foreach (DataRow tableRows in table.Rows)
        {
          cloneDt.ImportRow(tableRows);
        }

        #endregion

        #region Cambiamos los valores True y False 
        //Recorremos Clonedt buscando cambiar todos los true por "ü" y los false y nulls por ""
        foreach (int item in columnsChanged)
        {
          for (int i = 0; i < cloneDt.Rows.Count; i++)
          {
            if (cloneDt.Rows[i][item].ToString() == "True")
            {
              cloneDt.Rows[i][item] = "ü";
            }
            else
            {
              cloneDt.Rows[i][item] = "";
            }
          }
        }
        #endregion

        return cloneDt; //Regresamos el nuevo DataTable con su nueva estructura.
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
  }
}
