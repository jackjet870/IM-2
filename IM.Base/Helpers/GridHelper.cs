using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using IM.Model.Helpers;

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

    #region GetVisualChild
    /// <summary>
    /// Obtiene las propiedades Visuales del un FrameworkElement 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 22/Abril/2016 Created
    /// </history>
    public static T GetVisualChild<T>(Visual parent) where T : Visual
    {
      T child = default(T);
      int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
      for (int i = 0; i < numVisuals; i++)
      {
        Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
        child = v as T;
        if (child == null)
        {
          child = GetVisualChild<T>(v);
        }
        if (child != null)
        {
          break;
        }
      }
      return child;
    }
    #endregion

    #region GetCell
    /// <summary>
    /// Obtiene las propiedades de una celda
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 22/Abril/2016 Created
    /// </history>
    public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column)
    {
      if (row != null)
      {
        DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

        if (presenter == null)
        {
          grid.ScrollIntoView(row, grid.Columns[column]);
          presenter = GetVisualChild<DataGridCellsPresenter>(row);
        }

        DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
        return cell;
      }
      return null;
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
    /// <returns>True. Si existe | False.no existe</returns>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// </history>
    public static bool HasRepeatItem(Control control, DataGrid dgrItems,bool blnClone=false,string strPropGrid="")
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
                    UIHelper.ShowMessage(typeFromControl.Name + " must not be repeated");
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
  }
}