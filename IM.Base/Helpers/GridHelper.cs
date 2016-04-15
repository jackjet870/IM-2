using System.Windows.Controls;

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
  }
}