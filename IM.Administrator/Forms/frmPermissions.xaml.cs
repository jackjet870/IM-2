using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPermisions.xaml
  /// </summary>
  public partial class frmPermissions : Window
  {
    #region Variables
    private Permission _permissionFilter = new Permission();//Objeto con las variables del 
    private int _nStatus = -1;//Estatus de los registros del grid
    #endregion
    public frmPermissions()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Window Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPermissions();
    }
    #endregion

    #region Cell Double click
    /// <summary>
    /// Muestra la ventada detalle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Permission permission = (Permission)dgrPermissions.SelectedItem;
      frmPermissionDetail frmPermissionDetail = new frmPermissionDetail();
      frmPermissionDetail.Owner = this;
      frmPermissionDetail.oldPermission = permission;
      frmPermissionDetail.enumMode = EnumMode.Edit;
      if (frmPermissionDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<Permission> lstPermissions = (List<Permission>)dgrPermissions.ItemsSource;
        if(validateFilter(frmPermissionDetail.permission))//verificamos si cumple con los filtros actuales
        {
          ObjectHelper.CopyProperties(permission, frmPermissionDetail.permission);//Actualizamos los datos del permiso
          lstPermissions.Sort((x, y) => string.Compare(x.pmN, y.pmN));//ordenamos la lista
          nIndex = lstPermissions.IndexOf(permission);//obtenemos la posición del permiso
        }
        dgrPermissions.Items.Refresh();//Actualizamos la vista del grid
        GridHelper.SelectRow(dgrPermissions, nIndex);//Seleccionamos un registro
        StatusBarReg.Content = lstPermissions.Count + " Permissions.";
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
    /// [created] 07/04/2016
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

    #region KeyBoardFocuChange
    /// <summary>
    /// Verifica las teclas presinadas al cambio de ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window Key Down
    /// <summary>
    /// Verifica las teclas presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    #region Refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Permission permission = (Permission)dgrPermissions.SelectedItem;
      LoadPermissions(permission);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPermissionDetail frmPermissionDetail = new frmPermissionDetail();
      frmPermissionDetail.Owner = this;
      frmPermissionDetail.enumMode = EnumMode.Add;
      if(frmPermissionDetail.ShowDialog()==true)
      {
        Permission permission = frmPermissionDetail.permission;
        if (validateFilter(permission))//Verificamos si cumple con los filtros actuales
        {
          List<Permission> lstPermissions = (List<Permission>)dgrPermissions.ItemsSource;
          lstPermissions.Add(permission);//Agregamos el registro a la lista
          lstPermissions.Sort((x, y) => string.Compare(x.pmN, y.pmN));//ordenamos la lista
          int nIndex = lstPermissions.IndexOf(permission);//obtenemos la posición del permiso
          dgrPermissions.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPermissions, nIndex);//Seleccionamos un registro
          StatusBarReg.Content = lstPermissions.Count + " Permissions.";
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
    /// [emoguel] created 07/06/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _permissionFilter.pmID;
      frmSearch.strDesc = _permissionFilter.pmN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _permissionFilter.pmID = frmSearch.strID;
        _permissionFilter.pmN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadPermissions();
      }
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadPermisssions
    /// <summary>
    /// Llena el grid de permissions
    /// </summary>
    /// <param name="permision">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// [emoguel] Modified 11/06/2016 se volvió async
    /// </history>
    private async void LoadPermissions(Permission permision = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Permission> lstPermissions = await BRPermissions.GetPermissions(_nStatus, _permissionFilter);
        dgrPermissions.ItemsSource = lstPermissions;

        if (lstPermissions.Count > 0 && permision != null)
        {
          permision = lstPermissions.Where(pm => pm.pmID == permision.pmID).FirstOrDefault();
          nIndex = lstPermissions.IndexOf(permision);
        }
        GridHelper.SelectRow(dgrPermissions, nIndex);
        StatusBarReg.Content = lstPermissions.Count + " Permissions.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Permission");
      }
    }
    #endregion
    #region validateFilter
    /// <summary>
    /// Valida que un objeto tipo permission cumpla con los filtros actuales
    /// </summary>
    /// <param name="permission">Objeto a validar</param>
    /// <returns>True. SI cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private bool validateFilter(Permission permission)
    {
      if (_nStatus != -1)//Filtro por estatus
      {
        if (permission.pmA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_permissionFilter.pmID))//Filtro por ID
      {
        if (permission.pmID != _permissionFilter.pmID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_permissionFilter.pmN))//Filtro por descripción
      {
        if(!permission.pmN.Contains(_permissionFilter.pmN,StringComparison.OrdinalIgnoreCase))
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
