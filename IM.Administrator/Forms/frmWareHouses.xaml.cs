using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmWareHouses.xaml
  /// </summary>
  public partial class frmWarehouses : Window
  {
    #region variables
    private Warehouse _warehouseFilter = new Warehouse { whar=""};//Objeto con los filtros del grid
    private int _nStatus=-1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar|Agregar
    #endregion
    public frmWarehouses()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Lena los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Motives, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadWarehouses();
    }
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 28/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region KeyDown Form
    /// <summary>
    /// Verfica teclas precionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
      }
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            Cell_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }

      e.Handled = blnHandled;
    }

    #endregion

    #region DoubleClick cell
    /// <summary>
    /// Muestra la ventana detalle en modo edit
    /// </summary>
    /// <history>
    /// [emoguel] 28/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Warehouse warehouse = (Warehouse)dgrWarehouses.SelectedItem;
      frmWarehouseDetail frmWarehouseDetail = new frmWarehouseDetail();
      frmWarehouseDetail.Owner = this;
      frmWarehouseDetail.oldWarehouse = warehouse;
      frmWarehouseDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.add;
      if(frmWarehouseDetail.ShowDialog()==true)
      {
        List<Warehouse> lstWarehouses = (List<Warehouse>)dgrWarehouses.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmWarehouseDetail.warehouse))//Validamos que cumpla con los filtros actuales
        {
          ObjectHelper.CopyProperties(warehouse, frmWarehouseDetail.warehouse);//Actualizamos los datos
          lstWarehouses.Sort((x, y) => string.Compare(x.whN, y.whN));//Ordenamos la lista
          nIndex = lstWarehouses.IndexOf(warehouse);//Buscamos la posición del registro
        }
        else
        {
          lstWarehouses.Remove(warehouse);//Eliminamos el registro
        }
        dgrWarehouses.Items.Refresh();//Actualizamos la vista del grid
        GridHelper.SelectRow(dgrWarehouses, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstWarehouses.Count + " Warehouses.";//Actualizamos el contador
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Warehouse warehouse = (Warehouse)dgrWarehouses.SelectedItem;
      LoadWarehouses(warehouse);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmWarehouseDetail frmWarehouseDetail = new frmWarehouseDetail();
      frmWarehouseDetail.Owner = this;
      frmWarehouseDetail.enumMode = EnumMode.add;
      if(frmWarehouseDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmWarehouseDetail.warehouse))//Validamos que cumpla con los filtros actuales
        {
          List<Warehouse> lstWarehouses = (List<Warehouse>)dgrWarehouses.ItemsSource;
          lstWarehouses.Add(frmWarehouseDetail.warehouse);//Agregamos el registro a la lista
          lstWarehouses.Sort((x, y) => string.Compare(x.whN, y.whN));//Ordenamos la lista
          int nIndex = lstWarehouses.IndexOf(frmWarehouseDetail.warehouse);//Obtenemos la posición del registro
          dgrWarehouses.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrWarehouses, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstWarehouses.Count + " Warehouses";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmWarehouseDetail frmWarehouseDetail = new frmWarehouseDetail();
      frmWarehouseDetail.Owner = this;
      frmWarehouseDetail.enumMode = EnumMode.search;
      frmWarehouseDetail.oldWarehouse = _warehouseFilter;
      frmWarehouseDetail.nStatus = _nStatus;
      if(frmWarehouseDetail.ShowDialog()==true)
      {
        _nStatus = frmWarehouseDetail.nStatus;
        _warehouseFilter = frmWarehouseDetail.warehouse;
        LoadWarehouses();
      }
    }
    #endregion

    #endregion

    #region Methods
    #region loadWarehouses
    /// <summary>
    /// Llena el grid de warehouses
    /// </summary>
    /// <param name="warehouse">OBjeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private async void LoadWarehouses(Warehouse warehouse = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Warehouse> lstWareHouse = await BRWarehouses.GetWareHouses(_nStatus, _warehouseFilter);
        dgrWarehouses.ItemsSource = lstWareHouse;
        if (lstWareHouse.Count > 0 && warehouse != null)
        {
          warehouse = lstWareHouse.Where(wh => wh.whID == warehouse.whID).FirstOrDefault();
          nIndex = lstWareHouse.IndexOf(warehouse);
        }
        GridHelper.SelectRow(dgrWarehouses, nIndex);
        StatusBarReg.Content = lstWareHouse.Count + " Warehouses.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Warehouses");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un registro warehouse cumpla con los filtros actuales
    /// </summary>
    /// <param name="warehouse">Objeto a validar</param>
    /// <returns>True. Si cumple | false. No cumple</returns>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    private bool ValidateFilter(Warehouse warehouse)
    {
      if (_nStatus != -1)//filtro por estatus
      {
        if (warehouse.whA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_warehouseFilter.whID))//filtro po ID
      {
        if (_warehouseFilter.whID != warehouse.whID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_warehouseFilter.whN))//Filtro por descripción
      {
        if (!warehouse.whN.Contains(_warehouseFilter.whN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_warehouseFilter.whar))//Filtro por area
      {
        if (_warehouseFilter.whar != warehouse.whar)
        {
          return false;
        }
      }
      return true;
    } 
    #endregion
    #endregion
  }
}
