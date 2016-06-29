using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmRoles.xaml
  /// </summary>
  public partial class frmRoles : Window
  {
    #region variables
    private Role _roleFilter = new Role();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid    
    #endregion
    public frmRoles()
    {
      InitializeComponent();
    }

    #region Method Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventna
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 19/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadRoles();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 19/04/2016
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
    /// [emoguel] created 19/04/2016
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
    /// [emoguel] created 19/04/2016
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
    /// [emoguel] 19/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Role role=(Role)dgrRoles.SelectedItem;
      frmRoleDetail frmRoleDetail = new frmRoleDetail();
      frmRoleDetail.Owner = this;
      frmRoleDetail.enumMode = EnumMode.edit;
      frmRoleDetail.oldRole=role;
      if(frmRoleDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Role> lstRoles = (List<Role>)dgrRoles.ItemsSource;
        if(ValidateFilter(frmRoleDetail.role))//Verificamos que cumpla con los filtros actuales
        {
          ObjectHelper.CopyProperties(role, frmRoleDetail.role);//Actualizamos los datos del registro
          lstRoles.Sort((x, y) => string.Compare(x.roN, y.roN));//Ordenamos la lista
          nIndex = lstRoles.IndexOf(role);//Buscamos la posicion del registro
        }
        else
        {
          lstRoles.Remove(role);//Quitamos el registro
        }
        dgrRoles.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrRoles, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstRoles.Count + " Roles.";//Actualizamos el contador
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
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _roleFilter.roID;
      frmSearch.strDesc = _roleFilter.roN;
      frmSearch.nStatus = _nStatus;

      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _roleFilter.roID = frmSearch.strID;
        _roleFilter.roN = frmSearch.strDesc;
        LoadRoles();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmRoleDetail frmRoleDetail = new frmRoleDetail();
      frmRoleDetail.Owner = this;
      frmRoleDetail.enumMode = EnumMode.add;
      if(frmRoleDetail.ShowDialog()==true)
      {
        Role role = frmRoleDetail.role;
        if(ValidateFilter(role))//Verificamos que cumpla con los filtros actuales
        {
          List<Role> lstRoles = (List<Role>)dgrRoles.ItemsSource;
          lstRoles.Add(role);//Agregamos el registro
          lstRoles.Sort((x, y) =>string.Compare(x.roN,y.roN));//Ordenamos la lista
          int nIndex = lstRoles.IndexOf(role);//Obtenemos la posición del registro
          dgrRoles.Items.Refresh();//Actualizamos la vista del grid
          GridHelper.SelectRow(dgrRoles, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstRoles.Count + " Roles.";//Actualizamos el contador
        }
      }
    } 
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los datos del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Role role = (Role)dgrRoles.SelectedItem;
      LoadRoles(role);
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadRoles
    /// <summary>
    /// Llena el grid de roles
    /// </summary>
    /// <param name="role">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private async void LoadRoles(Role role = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Role> lstRoles = await BRRoles.GetRoles(_nStatus, _roleFilter);
        dgrRoles.ItemsSource = lstRoles;
        if (lstRoles.Count > 0 && role != null)
        {
          role = lstRoles.Where(ro => ro.roID == role.roID).FirstOrDefault();
          nIndex = lstRoles.IndexOf(role);
        }
        GridHelper.SelectRow(dgrRoles, nIndex);
        StatusBarReg.Content = lstRoles.Count + " Roles.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Roles");
      }
    }
    #endregion
    #region ValidateFilter
    /// <summary>
    /// Verifica que un objeto tipo role cumpla con los filtros actuales
    /// </summary>
    /// <param name="roles">objeto a validar</param>
    /// <returns>True. Si cumple | false. No cumple</returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    private bool ValidateFilter(Role role)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(role.roA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_roleFilter.roID))//Filtro por ID
      {
        if(_roleFilter.roID!=role.roID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_roleFilter.roN))//Filtro por descripción
      {
        if(!role.roN.Contains(_roleFilter.roN,StringComparison.OrdinalIgnoreCase))
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
