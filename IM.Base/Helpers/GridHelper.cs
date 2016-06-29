using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using IM.Model.Helpers;
using System.Windows;

namespace IM.Base.Helpers
{
  /// <summary>
  /// Clase que contiene las funciones comunes de grids
  /// </summary>
  public static class GridHelper
  {    
    #region SelectRow

    /// <summary>
    /// Selecciona un registro del grid
    /// </summary>
    /// <param name="grid">Grid para seleccionar el registro</param>
    /// <param name="nIndex">index del registro</param>
    /// <param name="column">Columna a seleccionar</param>
    /// <history>
    /// [emoguel] created 15/03/2016
    /// [emoguel] modified 23/03/2016 Se agregó la validacion HasItems
    /// [michan] modified 28/04/2016 Se agregó el parametro de la columna a seleccionar
    /// </history>
    public static void SelectRow(DataGrid grid, int nIndex, int? column = 0,bool blnEdit=false)
    {
      if (nIndex > -1)
      {
        grid.Focus();
        grid.SelectedIndex = nIndex;
        if (grid.SelectedItem != null)
        {
          grid.ScrollIntoView(grid.Items[nIndex]);
          grid.UpdateLayout();
          grid.ScrollIntoView(grid.SelectedItem);
          grid.CurrentCell = new DataGridCellInfo(grid.SelectedItem, grid.Columns[column.Value]);
          if(blnEdit)
          {
            grid.BeginEdit();
          }
        }
      }
    }

    #endregion SelectRow     

    #region GetItemsChecked
    /// <summary>
    /// Devuelve los elementos checados / deschecados de un grid en base a una columna
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="Field"></param>
    /// <param name="CancelField"></param>
    /// <param name="Checked"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static List<string> GetItemsChecked(DataGrid Grid, string Field, string CancelField, bool Checked = true)
    {
      List<string> _lstResult = new List<string>();

      // Recorremos los registros del grid
      foreach (var item in Grid.Items)
      {
        Type type = item.GetType();
        var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        if (property.Count > 0)
        {
          // Si el registro esta checado o deschecado (segun se desee)
          if (((bool)type.GetProperty(CancelField).GetValue(item, null)) == Checked)
            _lstResult.Add((string)type.GetProperty(Field).GetValue(item, null));
        }
      }

      return _lstResult;
      }
    #endregion

    #region GetItems
    /// <summary>
    /// Devuelve los elementos checados / deschecados de un grid en base a una columna
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="Field"></param>
    /// <param name="CancelField"></param>
    /// <param name="Checked"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 26/Mayo/2016 Created
    /// </history>
    public static List<string> GetItems(DataGrid Grid, string Field)
    {
      List<string> _lstResult = new List<string>();

      // Recorremos los registros del grid
      foreach (var item in Grid.Items)
      {
        Type type = item.GetType();
        var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        if (property.Count > 0)
        {
          // Si el registro esta checado o deschecado (segun se desee)
          if (((string)type.GetProperty(Field).GetValue(item, null)) != "")
            _lstResult.Add((string)type.GetProperty(Field).GetValue(item, null));
        }
      }

      return _lstResult;
    }
    #endregion

    #region HasRepeatItem
    /// <summary>
    /// Verifica que el registro no extista en el grid
    /// </summary>
    /// <param name="blnClone">Define si se va a clonar toda la fila seleccionada</param>
    /// <param name="control">Control que está siendo verificado</param>
    /// <param name="dgrItems">Grid que está siendo validado</param>
    /// <param name="strPropGrid">id para validar si son diferentes entidades</param>
    /// <param name="typeName">nombre que aparecerá en el titulo del mensaje</param>
    /// <returns>True. Si existe | False.no existe</returns>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// </history>
    public static bool HasRepeatItem(Control control, DataGrid dgrItems,bool blnClone=false,string strPropGrid="", string typeName="")
    {      
      switch (control.GetType().Name)
      {
        case "ComboBox":
          {
            ComboBox combobox = control as ComboBox;

            var listaGrid = dgrItems.ItemsSource.Cast<object>().ToList();//Lista de registros del grid
            var gridSelec = dgrItems.SelectedItem;//Item seleccionado del grid
            var selectedItem = combobox.SelectedItem;//Nuevo item seleccionado                                   
            
            if (selectedItem != null)
            {              
              if (gridSelec != selectedItem)
              {
                string strPropControl = combobox.SelectedValuePath;//Valor para hacer el filtro
                Type typeFromControl = selectedItem.GetType();//En caso de que sea de otro tipo el grid

                strPropGrid = (strPropGrid != "") ? strPropGrid : strPropControl;
                Type typeFromGrid = gridSelec.GetType();//En caso de que sea de otro tipo el grid

                var gridSelectValue = typeFromGrid.GetProperty(strPropGrid).GetValue(gridSelec, null);
                var selectItemValue = typeFromControl.GetProperty(strPropControl).GetValue(selectedItem, null);
                
                if (selectItemValue != null)
                {
                  var itemVal = listaGrid.Where(it => typeFromGrid.GetProperty(strPropGrid).GetValue(it, null) != gridSelectValue
                    && (typeFromGrid.GetProperty(strPropGrid).GetValue(it, null) == selectItemValue || typeFromGrid.GetProperty(strPropGrid).GetValue(it, null).Equals(selectItemValue))).FirstOrDefault();
                  if (itemVal != null)
                  {
                    UIHelper.ShowMessage(((typeName!="")?typeName:typeFromControl.Name)+ " must not be repeated");
                    return true;
                  }
                  else if(blnClone)
                  {
                    ObjectHelper.CopyProperties(gridSelec, selectedItem);
                  }
                }
              }
            }
            else
            {
              UIHelper.ShowMessage("Please select a Value");
              return true;
            }
            break;
          }

      }

      return false;
    }
    #endregion

    #region Validate
    /// <summary>
    /// Valida el grid:
    ///             1. Que tenga al menos un registro
    ///             2. Que no tenga registros repetido
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="ValidateEmpty"></param>
    /// <param name="NumMinItems"></param>
    /// <param name="strItem"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 10/Junio/2016 Created
    /// </history>
    public static bool Validate(DataGrid Grid, bool ValidateEmpty = true, int NumMinItems = 1, string NamePluralItem = "", string NameSingularItem = "", List<string> Fields = null)
    {
      // si se debe validar que el grid no este vacio
      if (ValidateEmpty)
      {
        // si el numero minimo de elementos es menor de 2
        if (NumMinItems < 2)
        {
          // validamos que se haya ingresado al menos un registro
          if (Grid.Items.Count == 0)
          {
            UIHelper.ShowMessage("Specify at least one " + NameSingularItem + ".", MessageBoxImage.Information);
            return false;
          }
        }
        // si el numero minimo de elementos es al menos 2
        else
        {
          // validamos que tenga al menos determinado numero de elementos
          if (!(Grid.Items.Count >= NumMinItems))
          {
            UIHelper.ShowMessage("Specify at least " + NumMinItems + " " + NameSingularItem + ".", MessageBoxImage.Information);
            return false;
          }
        }
      }

      string message = "";
      // si esta repetido algun elemento
      if (HasRepeatedItems(Grid, NamePluralItem, NameSingularItem, ref message, Fields))
      {
        UIHelper.ShowMessage(message, MessageBoxImage.Exclamation, "Cancel External Products");
        return false;
      }

      return true;
    }
    #endregion

    #region HasRepeatedItems
    /// <summary>
    /// Indica si un grid tiene elementos repetidos mediante los campos especificados
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="NamePlural"></param>
    /// <param name="NameSingular"></param>
    /// <param name="message"></param>
    /// <param name="Fields"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco]  10/Junio/2016 Created
    /// </history>
    public static bool HasRepeatedItems(DataGrid Grid, string NamePlural, string NameSingular, ref string message, List<string> Fields = null)
    {
      int i = 0, j = 0;
      int valid = 0;

      // Construimos el mensaje inicial
      message = NamePlural + " must not be repeated.\r\n" + NameSingular + " repetead is ";

      string RepeatFields = "";
      Grid.IsReadOnly = true;

      // recorremos las filas
      foreach (var _Grid in Grid.Items)
      {
        Type type = _Grid.GetType();
        var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        // verificamos que el registro no este vacio
        if (property.Count > 0 && ((string)type.GetProperty(Fields[0]).GetValue(_Grid, null) != ""))
        {
          // recorremos las filas que quedan
          foreach (var _GridTemp in Grid.Items)
          {
            Type typeTemp = _GridTemp.GetType();
            var propertyTemp = typeTemp.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            if (i != j) // Para evitar que sea el mismo row
            {
              // recorremos los campos que forman la llave primaria
              foreach (string field in Fields)
              {
                if ((type.GetProperty(field).GetValue(_Grid, null)).Equals(typeTemp.GetProperty(field).GetValue(_GridTemp, null)))
                {
                  int num = Grid.Columns.ToList().FindIndex(x => x.SortMemberPath == field); // Obtenemos el index de la columna
                  RepeatFields += " " + GetDescription(Grid, num, field, (string)(type.GetProperty(field).GetValue(_Grid, null)), ref valid); // Obtnemos su descripcion
                }
              }
            }

            if (!string.IsNullOrEmpty(RepeatFields) && valid == Fields.Count)
            {
              Grid.IsReadOnly = false;
              message += RepeatFields;
              return true;
            }
            j++;
          }
          j = 0;
          i++;
        }
      }
      Grid.IsReadOnly = false;

      return false;
    }
    #endregion

    #region GetDescription
    /// <summary>
    /// Obtienen la descripcion del campo repetido
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="Column"></param>
    /// <param name="ColumnName"></param>
    /// <param name="value"></param>
    /// <param name="valid"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 10/Junio/2016 Created
    /// </history>
    private static string GetDescription(DataGrid Grid, int Column, string ColumnName, object value, ref int valid)
    {
      var ColumnSelected = Grid.Columns[Column];

      if (ColumnSelected is DataGridComboBoxColumn)
      {
        DataGridComboBoxColumn _Column = (DataGridComboBoxColumn)ColumnSelected;
        IEnumerable<object> list = _Column.ItemsSource.Cast<object>().ToList();

        foreach (var item in list)
        {
          string ID = _Column.SelectedValuePath;
          var ValueID = item.GetType().GetProperty(ID).GetValue(item, null);

          if (ValueID.Equals(value))
          {
            string name = _Column.DisplayMemberPath;
            var sd = item.GetType().GetProperty(name).GetValue(item, null);
            valid++;
            return sd.ToString();
          }
        }
      }

      return string.Empty;
    } 
    #endregion

  }
}