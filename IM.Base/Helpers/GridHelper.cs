using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using IM.Model.Helpers;
using System.Windows;
using System.Data.Entity.Core.Metadata.Edm;
using System.Windows.Input;
using IM.Styles.Classes;
using IM.Styles.Enums;

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
    /// [emoguel] modified Se agregó para cambiar el campo de busqueda
    /// </history>
    public static void SelectRow(DataGrid grid, int nIndex, int? column = 0, bool blnEdit = false)
    {
      if (nIndex > -1)
      {
        grid.Focus();
        grid.SelectedIndex = nIndex;
        if (grid.SelectedItem != null)
        {
          grid.ScrollIntoView(grid.Items[nIndex]);
          grid.CurrentCell = new DataGridCellInfo(grid.SelectedItem, grid.Columns[column.Value]);
          //Se cambia el campo de busqueda
          if (grid.CurrentCell != null)
          {
            grid.Resources["SearchField"] = grid.CurrentColumn.SortMemberPath;
          }
          if (blnEdit)
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
    public static bool HasRepeatItem(Control control, DataGrid dgrItems, bool blnClone = false, string strPropGrid = "", string typeName = "")
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
                    UIHelper.ShowMessage(((typeName != "") ? typeName : typeFromControl.Name) + " must not be repeated");
                    return true;
                  }
                  else if (blnClone)
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
      //Grid.IsReadOnly = true;

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

            if (propertyTemp.Count > 0 && i != j) // Para evitar que sea el mismo row
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
      //Grid.IsReadOnly = false;

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

    #region ValidateEditNumber
    /// <summary>
    /// Valida los datos de una columna de tipo numerico de un grid
    /// </summary>
    /// <param name="pNumber"> El numero obtenido del campo en edicion </param>
    /// <param name="pCancel"></param>
    /// <param name="pTitle"></param>
    /// <param name="pUpperBound"></param>
    /// <param name="pLowerBound"></param>
    /// <param name="pDefaultValue"></param>
    /// <param name="pValidateBounds"></param>
    /// <history>
    /// [vipacheco] 30/Junio/2016 Created
    /// </history>
    public static void ValidateEditNumber(ref int pNumber, bool pCancel, string pTitle, int pUpperBound, int pLowerBound, int pDefaultValue = 0, bool pValidateBounds = true)
    {
      // si se ingreso un valor
      if (pNumber >= 0)
      {
        // si se desea validar los limites
        if (pValidateBounds)
        {
          // validamos que no sea mayor al limite superior
          if (pNumber > pUpperBound)
          {
            UIHelper.ShowMessage("Quantity can not be greater than " + pUpperBound, MessageBoxImage.Information, "Intelligence Marketing");
            pCancel = true;
          }
          // validamos que no sea menor al limite inferior
          else if (pNumber < pLowerBound)
          {
            UIHelper.ShowMessage("Quantity can not be lower than " + pLowerBound, MessageBoxImage.Information, "Intelligence Marketing");
            pCancel = true;
          }
        }
      }
      else
      {
        // si se envio un valor default
        if (pDefaultValue != 0)
          pNumber = pDefaultValue;
        else
          pNumber = pLowerBound;
      }

    }
    #endregion

    #region Update All Cells From a Row
    /// <summary>
    /// Sirve para actualizar nuestras celdas del grid con los valores de el objeto enlazado
    /// </summary>
    /// <param name="dtg">DataGrid que vamos a actualizar</param>
    /// <history>
    /// [erosado] 29/07/2016  Created.
    /// </history>
    public static void UpdateCellsFromARow(DataGrid dtg)
    {
      //Si el Grid es diferente de NULL
      if (dtg != null && dtg?.SelectedIndex != -1)
      {
        //Obtenemos el DataGridRow seleccionado
        DataGridRow dtgRow = (DataGridRow)dtg.ItemContainerGenerator.ContainerFromIndex(dtg.SelectedIndex);
        //Obtenemos la lista de las columnas
        List<DataGridColumn> columns = dtg?.Columns?.ToList();

        //Si tiene alguna columna
        if (columns.Any())
        {
          columns.ForEach(column =>
          {
            //Obtenemos la celda
            DataGridCell cell = column.GetCellContent(dtgRow).Parent as DataGridCell;

            if (cell != null)
            {

              Type controlType = cell.Content.GetType();

              switch (controlType.Name)
              {
                case "TextBlock":
                  TextBlock txtb = cell.Content as TextBlock;
                  var txtBinding = txtb.GetBindingExpression(TextBlock.TextProperty);
                  txtBinding.UpdateTarget();
                  break;
                case "TextBlockComboBox":
                  ComboBox cmb = cell.Content as ComboBox;
                  var cmbBinding = cmb.GetBindingExpression(ComboBox.SelectedValueProperty);
                  cmbBinding.UpdateTarget();
                  break;
                default:
                  break;
              }
            }
          });
        }
      }
    }

    #endregion

    #region Update Source From a Row
    /// <summary>
    ///   Actualiza el source de una fila en el grid con valores insertados
    /// </summary>
    /// <param name="dtg">DataGrid que vamos a actualizar</param>
    /// <history>
    ///   [vku] 03/Ago/2017  Created.
    /// </history>
    public static void UpdateSourceFromARow(DataGrid dtg)
    {
      //Si el Grid es diferente de NULL
      if (dtg != null && dtg?.SelectedIndex != -1)
      {
        //Obtenemos el DataGridRow seleccionado
        DataGridRow dtgRow = (DataGridRow)dtg.ItemContainerGenerator.ContainerFromIndex(dtg.SelectedIndex);
        //Obtenemos la lista de las columnas
        List<DataGridColumn> columns = dtg?.Columns?.ToList();

        //Si tiene alguna columna
        if (columns.Any())
        {
          columns.ForEach(column =>
          {
            //Obtenemos la celda
            DataGridCell cell = column.GetCellContent(dtgRow).Parent as DataGridCell;

            if (cell != null)
            {

              Type controlType = cell.Content.GetType();

              switch (controlType.Name)
              {
                case "TextBox":
                  TextBox txtb = cell.Content as TextBox;
                  var txtBinding = txtb.GetBindingExpression(TextBox.TextProperty);
                  txtBinding.UpdateSource();
                  break;
              }
            }
          });
        }
      }
    }

    #endregion

    #region SetUpGrid
    /// <summary>
    /// Configura el Estilo de los DataGridTextColumn 
    /// Maxlength, Admision de caracteres especiales, Numerico, Decimal , Precision, Escala,
    /// </summary>
    /// <param name="dgrGrid">Grid a configurar</param>
    /// <param name="objBinding">Objeto con el que se quiere configurar el grid</param>
    /// <history>
    /// [emoguel] 28/07/2016  Created.
    /// [erosado] 29/07/2016  Modified. Se agregó el Tag para definir el Maxlength desde la columna.
    /// </history>
    public static void SetUpGrid(DataGrid dgrGrid, object objBinding, bool blnCharacters = false)
    {
      List<DataGridTextColumn> lstColumns = dgrGrid.Columns.Where(dgc => dgc is DataGridTextColumn).OfType<DataGridTextColumn>().ToList();

      #region Object properties
      Type type = objBinding.GetType();
      List<PropertyInfo> lstProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => !pi.GetMethod.IsVirtual).ToList();
      EntityTypeBase entityTypeBase = EntityHelper.GetEntityTypeBase(type);
      #endregion

      lstColumns.ForEach(dgc =>
      {

        if (!string.IsNullOrWhiteSpace(dgc.SortMemberPath))
        {
          PropertyInfo property = lstProperties.Where(pi => pi.Name == dgc.SortMemberPath).FirstOrDefault();
          if (property != null)
          {
            EdmMember edmMember = entityTypeBase.Members.Where(em => em.Name == property.Name).FirstOrDefault();
            TypeCode typeCode = Type.GetTypeCode(Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            Facet facet;
            Style style = new Style(typeof(TextBox));
            EnumFormatInput formatInput = FormatInputPropertyClass.GetFormatInput(dgc);
            switch (typeCode)
            {
              #region String
              case TypeCode.String:
              case TypeCode.Char:
                {
                  facet = edmMember.TypeUsage.Facets.Where(fc => fc.Name == "MaxLength").FirstOrDefault();//Obtenemos el length                  
                  style.Setters.Add(new Setter(TextBox.MaxLengthProperty, Convert.ToInt32(facet.Value)));
                  if (blnCharacters)//Bloquea caracteres especiales
                  {
                    style.Setters.Add(new EventSetter(UIElement.PreviewTextInputEvent, new TextCompositionEventHandler(TextBoxHelper.TextInputSpecialCharacters)));
                  }
                  dgc.EditingElementStyle = style;
                  break;
                }
              #endregion

              #region Decimal
              case TypeCode.Decimal:
              case TypeCode.Double:
                {
                  int precision = Convert.ToInt32(edmMember?.TypeUsage.Facets.FirstOrDefault(fc => fc.Name == "Precision")?.Value);
                  int scale = Convert.ToInt32(edmMember?.TypeUsage.Facets.FirstOrDefault(fc => fc.Name == "Scale")?.Value);
                  if (scale > 0)
                  {
                    style.Setters.Add(new EventSetter(UIElement.PreviewTextInputEvent, new TextCompositionEventHandler(TextBoxHelper.DecimalTextInput)));
                  }
                  else
                  {
                    style.Setters.Add(new EventSetter(UIElement.PreviewTextInputEvent, new TextCompositionEventHandler(TextBoxHelper.IntTextInput)));
                  }
                  style.Setters.Add(new Setter(TextBox.MaxLengthProperty, precision));
                  break;
                }
              #endregion

              #region Int
              case TypeCode.Int16:
              case TypeCode.Int32:
              case TypeCode.Int64:
                {
                  int maxLengthValue= MaxLengthPropertyClass.GetMaxLength(dgc);                  
                  if (maxLengthValue > 0)
                  {
                    style.Setters.Add(new Setter(TextBox.MaxLengthProperty, (formatInput == EnumFormatInput.NumberNegative) ? maxLengthValue + 1 : maxLengthValue));
                  }
                  switch (formatInput)
                  {
                    case EnumFormatInput.Number:
                      style.Setters.Add(new EventSetter() { Event = UIElement.PreviewTextInputEvent, Handler = new TextCompositionEventHandler(TextBoxHelper.IntTextInput) });
                      break;
                    case EnumFormatInput.NumberNegative:
                      style.Setters.Add(new EventSetter() { Event = UIElement.PreviewTextInputEvent, Handler = new TextCompositionEventHandler(TextBoxHelper.IntWithNegativeTextInput) });
                      break;
                  }
                  style.Setters.Add(new EventSetter() { Event = UIElement.PreviewKeyDownEvent, Handler = new System.Windows.Input.KeyEventHandler(TextBoxHelper.ValidateSpace) });
                  break;
                }
                #endregion
            }

            dgc.EditingElementStyle = style;
          }
        }

      });
    }
    #endregion

    #region IsInEditMode
    /// <summary>
    /// Valida si un datagrid está en modo edición
    /// </summary>
    /// <param name="dgr">Datagrid a validar</param>
    /// <returns>True. está en modo edición | False. No está en modo edición</returns>
    /// <history>
    /// [emoguel] created 29/07/2016
    /// [erosado] 05/08/2016  Modified. Se agregó validacion, para prevenir Datagrid.ItemSource = NULL
    /// </history>
    public static bool IsInEditMode(DataGrid dgr)
    {
      if (dgr != null && dgr?.ItemsSource != null)
      {
        List<object> lstObject = dgr.ItemsSource.OfType<object>().ToList();
        var lstRows = dgr.ItemsSource.OfType<object>().Select(obj => dgr.ItemContainerGenerator.ContainerFromIndex(lstObject.IndexOf(obj))).ToList().OfType<DataGridRow>().ToList();
        //Obtener la fila en edición
        var rowEdit = lstRows.FirstOrDefault(rw => rw.IsEditing);
        DataGridRow rowSelected = new DataGridRow();
        if (dgr.SelectedIndex != -1)
        {
          //Fila a editar o seleccionada
          rowSelected = dgr.ItemContainerGenerator.ContainerFromIndex(dgr.SelectedIndex) as DataGridRow;
        }
        if (rowEdit != null && rowEdit != rowSelected)
        {
          return true;
        }
      }
      return false;
    }
    #endregion

    #region BeginEdit
    /// <summary>
    /// Valida que no se puedan habilitar mas de una fila en modo edición
    /// </summary>
    /// <history>
    /// [emoguel] created 29/07/2016
    /// </history>
    public static void dgr_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      e.Cancel = IsInEditMode(sender as DataGrid);
    }
    #endregion

  }
}