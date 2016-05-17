using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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

  }
}