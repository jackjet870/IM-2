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
using IM.BusinessRules.BR;
using IM.Model.Enums;
using System.Windows.Controls.Primitives;

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
    /// [emoguel] 18/05/2016 created
    /// [emoguel] 11/08/2016 modified---> se agregó el case para los textbox
    /// </history>
    public static bool HasRepeatItem(Control control, DataGrid dgrItems, bool blnClone = false, string strPropGrid = "", string typeName = "")
    {
      var listaGrid = dgrItems.ItemsSource.Cast<object>().ToList();//Lista de registros del grid
      switch (control.GetType().Name)
      {
        #region Combobox
        case "ComboBox":
          {
            ComboBox combobox = control as ComboBox;
            
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

                var gridSelectValue = typeFromGrid.GetProperty(strPropGrid).GetValue(gridSelec);
                var selectItemValue = typeFromControl.GetProperty(strPropControl).GetValue(selectedItem);

                if (selectItemValue != null)
                {
                  if (gridSelectValue!=null && (gridSelectValue == selectItemValue || gridSelectValue.Equals(selectItemValue)))//SI el seleccionado es igual al del datagrid
                  {
                    var items = listaGrid.Where(it => typeFromGrid.GetProperty(strPropGrid).GetValue(it)!=null &&
                    (typeFromGrid.GetProperty(strPropGrid).GetValue(it) == selectItemValue || 
                    typeFromGrid.GetProperty(strPropGrid).GetValue(it).Equals(selectItemValue))).ToList();
                    if (items.Count > 1)
                    {
                      UIHelper.ShowMessage(((typeName != "") ? typeName : typeFromControl.Name) + " must not be repeated");
                      combobox.SelectedIndex = -1;
                      return true;
                    }
                    else if (blnClone)
                    {
                      ObjectHelper.CopyProperties(gridSelec, selectedItem);
                    }
                  }
                  else//Si los items no son iguales
                  {
                    var itemVal = listaGrid.Where(it => typeFromGrid.GetProperty(strPropGrid).GetValue(it, null) != gridSelectValue
                      && (typeFromGrid.GetProperty(strPropGrid).GetValue(it, null) == selectItemValue || typeFromGrid.GetProperty(strPropGrid).GetValue(it, null).Equals(selectItemValue))).FirstOrDefault();
                    if (itemVal != null)
                    {
                      UIHelper.ShowMessage(((typeName != "") ? typeName : typeFromControl.Name) + " must not be repeated");
                      combobox.SelectedIndex = -1;
                      return true;
                    }
                    else if (blnClone)
                    {
                      ObjectHelper.CopyProperties(gridSelec, selectedItem);
                    }
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

        #endregion

        #region TextBox
        case "TextBox":
          {
            TextBox textBox = control as TextBox;
            var gridSelec = dgrItems.SelectedItem;//Item seleccionado del grid            
            var bindingExpresion = textBox.GetBindingExpression(TextBox.TextProperty);//Buscamos la propiedad bindeada
            if (bindingExpresion != null)
            {
              string property = bindingExpresion.ResolvedSourcePropertyName;//obtenemos el nombre de la propiedad bindeada

              var strTextBox = textBox.GetValue(TextBox.TextProperty);//obtenemos el valor del textbox

              var items = listaGrid.Where(it => it.GetType().GetProperty(property).GetValue(it) == strTextBox).ToList();//Obtenemos el número de registros con el mismo valor

              if (items.Count > 1)//Si hay un registro con el mismo valor
              {
                UIHelper.ShowMessage(((typeName != "") ? typeName : gridSelec.GetType().Name) + " must not be repeated");
                return true;
              }
            }

            break;
          } 
          #endregion
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
                  if(txtBinding!=null)
                    txtBinding.UpdateTarget();
                  break;
                case "TextBlockComboBox":
                  ComboBox cmb = cell.Content as ComboBox;
                  var cmbBinding = cmb.GetBindingExpression(ComboBox.SelectedValueProperty);
                  if(cmbBinding!=null)
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
    /// <param name="dtgGrid">Grid a configurar</param>
    /// <param name="objBinding">Objeto con el que se quiere configurar el grid</param>
    /// <history>
    /// [emoguel] 28/07/2016  Created.
    /// [erosado] 29/07/2016  Modified. Se agregó el Tag para definir el Maxlength desde la columna.
    /// </history>
    public static void SetUpGrid<T>(DataGrid dtgGrid, T objBinding, EnumDatabase database = EnumDatabase.IntelligentMarketing) where T:class
    {
      List<DataGridTextColumn> lstColumns = dtgGrid.Columns.Where(dgc => dgc is DataGridTextColumn).OfType<DataGridTextColumn>().ToList();
      List<Model.Classes.ColumnDefinition> lstColumnsDefinitions = BRHelpers.GetFieldsByTable<T>(objBinding, database);//Obtenemos la propiedades desde la BD
      #region Object properties
      Type type = objBinding.GetType();
      List<PropertyInfo> lstProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => !pi.GetMethod.IsVirtual).ToList();      
      #endregion

      lstColumns.ForEach(dgc =>
      {

        if (!string.IsNullOrWhiteSpace(dgc.SortMemberPath))
        {
          PropertyInfo property = lstProperties.Where(pi => pi.Name == dgc.SortMemberPath).FirstOrDefault();
          var columnDefinition = lstColumnsDefinitions.FirstOrDefault(cd => cd.column == dgc.SortMemberPath);
          if (columnDefinition != null)
          {
            TypeCode typeCode = Type.GetTypeCode(Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            Style style = new Style(typeof(TextBox));
            EnumFormatInput formatInput = FormatInputPropertyClass.GetFormatInput(dgc);//Formato del campo de texto
            int maxLengthProp = MaxLengthPropertyClass.GetMaxLength(dgc);//Maxlength definido desde la columna
            switch (typeCode)
            {
              #region String
              case TypeCode.String:
              case TypeCode.Char:
                {                  
                  style.Setters.Add(new Setter(TextBox.MaxLengthProperty, (maxLengthProp > 0) ? maxLengthProp : columnDefinition.maxLength));//Asignamos el maxLength 
                  if (formatInput==EnumFormatInput.NotSpecialCharacters)//Bloquea caracteres especiales
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
                  if (columnDefinition.scale > 0 || PrecisionPropertyClass.GetPrecision(dgc) != "0,0")
                  {
                    style.Setters.Add(new EventSetter(UIElement.PreviewTextInputEvent, new TextCompositionEventHandler(TextBoxHelper.DecimalTextInput)));//Validar texto
                    var precisionProperty = PrecisionPropertyClass.GetPrecision(dgc);
                    if (precisionProperty=="0,0")
                    {
                      style.Setters.Add(new Setter(PrecisionPropertyClass.PrecisionProperty, columnDefinition.precision - columnDefinition.scale + "," + columnDefinition.scale));//Agregar Presicion
                    } 
                    else
                    {
                      var precision = precisionProperty.Split(',');
                      style.Setters.Add(new Setter(MaxLengthPropertyClass.MaxLengthProperty,Convert.ToInt16(precision[0]) + Convert.ToInt16(precision[1])+1));
                      style.Setters.Add(new Setter(PrecisionPropertyClass.PrecisionProperty, precisionProperty));//Agregar Presicion
                    }                 
                    style.Setters.Add(new EventSetter(UIElement.PreviewKeyDownEvent, new KeyEventHandler(TextBoxHelper.Decimal_PreviewKeyDown)));//Validar espacios en blanco y borrado

                  }
                  else
                  {
                    style.Setters.Add(new EventSetter(UIElement.PreviewTextInputEvent, new TextCompositionEventHandler(TextBoxHelper.IntTextInput)));//Validar enteros
                    style.Setters.Add(new EventSetter(UIElement.PreviewKeyDownEvent, new KeyEventHandler(TextBoxHelper.Decimal_PreviewKeyDown)));//Validar espacios en blanco
                  }                  
                  style.Setters.Add(new Setter(TextBox.MaxLengthProperty, (maxLengthProp > 0) ? maxLengthProp : columnDefinition.maxLength));//Asignamos el maxLength 
                  break;
                }
              #endregion

              #region Int
              case TypeCode.Int16:
              case TypeCode.Int32:
              case TypeCode.Int64:
                {
                  switch (formatInput)
                  {
                    case EnumFormatInput.Number:
                      style.Setters.Add(new EventSetter() { Event = UIElement.PreviewTextInputEvent, Handler = new TextCompositionEventHandler(TextBoxHelper.IntTextInput) });
                      style.Setters.Add(new Setter(TextBox.MaxLengthProperty, (maxLengthProp > 0) ? maxLengthProp : columnDefinition.maxLength));//Asignamos el maxLength
                      break;
                    case EnumFormatInput.NumberNegative:
                      style.Setters.Add(new EventSetter() { Event = UIElement.PreviewTextInputEvent, Handler = new TextCompositionEventHandler(TextBoxHelper.IntWithNegativeTextInput) });
                      style.Setters.Add(new Setter(TextBox.MaxLengthProperty, (maxLengthProp>0)? maxLengthProp + 1 : columnDefinition.maxLength+1));
                      break;
                  }
                  style.Setters.Add(new EventSetter() { Event = UIElement.PreviewKeyDownEvent, Handler = new System.Windows.Input.KeyEventHandler(TextBoxHelper.ValidateSpace) });
                  break;
                }
              #endregion

              #region Byte
              case TypeCode.Byte:
                {
                  style.Setters.Add(new Setter(TextBox.MaxLengthProperty, 3));//Asignamos el maxLength 
                  style.Setters.Add(new EventSetter(UIElement.PreviewTextInputEvent, new TextCompositionEventHandler(TextBoxHelper.ByteTextInput)));//Validar enteros
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
    /// <param name="dtg">Datagrid a validar</param>
    /// <returns>True. está en modo edición | False. No está en modo edición</returns>
    /// <history>
    /// [emoguel] 29/07/2016 created
    /// [erosado] 05/08/2016  Modified. Se agregó validacion, para prevenir Datagrid.ItemSource = NULL
    /// [emoguel] 11/08/2016 modified. Se agregó una bandera para validar si el item seleccionado es diferente al item en edición
    /// [emoguel] 13/08/2016 modified. Se Agrego validacion para saber si se está editando una celda antes de habilitar otra
    /// </history>
    public static bool IsInEditMode(DataGrid dtg,bool blnValidCurrentRow=true)
    {
      if (dtg != null && dtg?.ItemsSource != null)
      {
        List<object> lstObject = dtg.ItemsSource.OfType<object>().ToList();
        var lstRows = dtg.ItemsSource.OfType<object>().Select(obj => dtg.ItemContainerGenerator.ContainerFromIndex(lstObject.IndexOf(obj))).ToList().OfType<DataGridRow>().ToList();
        //Obtener la fila en edición        
        if (blnValidCurrentRow)
        {
          var rowEdit = lstRows.FirstOrDefault(rw => rw.IsEditing);
          DataGridRow rowSelected = new DataGridRow();
          if (dtg.SelectedIndex != -1)
          {
            //Fila a editar o seleccionada
            rowSelected = dtg.ItemContainerGenerator.ContainerFromIndex(dtg.SelectedIndex) as DataGridRow;
          }
          if (rowEdit != null && rowEdit != rowSelected)
          {
            return true;
          }
          else if(rowEdit!=null)//Buscamos si es otra celda la que se quiere habilitar
          {
            foreach(DataGridColumn column in dtg.Columns)
            {
              DataGridCell cell = dtg.Columns[column.DisplayIndex].GetCellContent(rowEdit).Parent as DataGridCell;
              if (cell != null && cell.IsEditing)
              {                
                return true;
              }
            }
          }
        }
        else
        {
          return lstRows.Where(rw=>rw.IsEditing).ToList().Count > 0;
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

    #region dtg_Sorting
    /// <summary>
    /// cancela el reordenado cuando el grid está en modo edición
    /// </summary>
    /// <history>
    /// [emoguel] 13/08/2016 created
    /// </history>
    public static void dtg_Sorting(object sender, DataGridSortingEventArgs e)
    {
      e.Handled = IsInEditMode(sender as DataGrid, false);
    } 
    #endregion
  }
}