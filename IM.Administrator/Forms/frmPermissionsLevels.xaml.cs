using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmPermissionsLevels.xaml
  /// </summary>
  public partial class frmPermissionsLevels : Window
  {
    #region Variables
    private PermissionLevel _permissionLevelFilter = new PermissionLevel();//Contiene filtros del grid
    private int _nStatus = -1;//Estatus de los reggistros del grid
    #endregion
    public frmPermissionsLevels()
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
    /// [emoguel] created 11/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadPermissionsLevels();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 11/04/2016
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
    /// [emoguel] created 11/04/2016
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
    /// Muestra la ventana detalle
    /// </summary>
    /// <history>
    /// [emoguel] 11/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      PermissionLevel permissionLevel = (PermissionLevel)dgrPermissionsLevels.SelectedItem;
      frmPermissionLevelDetail frmPermissionLevelDetail = new frmPermissionLevelDetail();
      frmPermissionLevelDetail.Owner = this;
      frmPermissionLevelDetail.enumMode = EnumMode.edit;
      frmPermissionLevelDetail.oldPermissionLevel = permissionLevel;
      if (frmPermissionLevelDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<PermissionLevel> lstPermissionsLevels = (List<PermissionLevel>)dgrPermissionsLevels.ItemsSource;
        if (ValidateFilter(frmPermissionLevelDetail.permissionLevel))
        {
          ObjectHelper.CopyProperties(permissionLevel, frmPermissionLevelDetail.permissionLevel);//Actualizamos los datos
          lstPermissionsLevels.Sort((x, y) => string.Compare(x.plN, y.plN));//ordenamos la lista
          nIndex = lstPermissionsLevels.IndexOf(permissionLevel);//obtenemos la posición
        }
        else
        {
          lstPermissionsLevels.Remove(permissionLevel);//Elminamos el registro
        }
        dgrPermissionsLevels.Items.Refresh();//actualizamos la lista
        GridHelper.SelectRow(dgrPermissionsLevels, nIndex);//seleccionamos el registro
        StatusBarReg.Content = lstPermissionsLevels.Count + " Permission Levels.";
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
    /// [emoguel] created 11/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.enumWindow = EnumWindow.DefaultInt;
      frmSearch.strID = (_permissionLevelFilter.plID > 0)? _permissionLevelFilter.plID.ToString() : "";
      frmSearch.strDesc = _permissionLevelFilter.plN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _permissionLevelFilter.plID = Convert.ToInt32(frmSearch.strID);
        _permissionLevelFilter.plN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadPermissionsLevels();
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
    /// [Emoguel] created 11/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmPermissionLevelDetail frmPermissionLevelDetail = new frmPermissionLevelDetail();
      frmPermissionLevelDetail.Owner = this;
      frmPermissionLevelDetail.enumMode = EnumMode.add;
      if(frmPermissionLevelDetail.ShowDialog()==true)
      {
        PermissionLevel permissionLevel = frmPermissionLevelDetail.permissionLevel;
        if (ValidateFilter(permissionLevel))//Validamos si cumple con los permisos actuales
        {
          List<PermissionLevel> lstPermissionsLevels = (List<PermissionLevel>)dgrPermissionsLevels.ItemsSource;
          lstPermissionsLevels.Add(permissionLevel);//Agregamos el nuevo registro
          lstPermissionsLevels.Sort((x, y) => string.Compare(x.plN, y.plN));//ordenamos la lista
          int nIndex = lstPermissionsLevels.IndexOf(permissionLevel);//obtenemos la posicion del nuevo registro
          dgrPermissionsLevels.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrPermissionsLevels, nIndex);//Seleccionamos el registro nuevo
          StatusBarReg.Content = lstPermissionsLevels.Count + " Permission Levels.";//Actualizamos el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza el grid de permission Levels
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      PermissionLevel permissionLevel = (PermissionLevel)dgrPermissionsLevels.SelectedItem;
      LoadPermissionsLevels(permissionLevel);
    } 
    #endregion

    #endregion

    #region Method
    #region LoadPermissionsLevels
    /// <summary>
    /// Carga el grid de Permissions
    /// </summary>
    /// <param name="permissionLevel">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private async void LoadPermissionsLevels(PermissionLevel permissionLevel = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<PermissionLevel> lstPermisssionsLevels = await BRPermissionsLevels.GetPermissionsLevels(_nStatus, _permissionLevelFilter);
        dgrPermissionsLevels.ItemsSource = lstPermisssionsLevels;
        if (permissionLevel != null && lstPermisssionsLevels.Count > 0)
        {
          permissionLevel = lstPermisssionsLevels.Where(pl => pl.plID == permissionLevel.plID).FirstOrDefault();
          nIndex = lstPermisssionsLevels.IndexOf(permissionLevel);
        }
        GridHelper.SelectRow(dgrPermissionsLevels, nIndex);
        StatusBarReg.Content = lstPermisssionsLevels.Count + " Permission Levels.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Permission Levels");
      }
    }
    #endregion
    
    #region ValidateFilter
    /// <summary>
    /// Valida si un objeto tipo PermissionLevel con los filtros actuales
    /// </summary>
    /// <param name="permissionLevel">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    private bool ValidateFilter(PermissionLevel permissionLevel)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(permissionLevel.plA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(_permissionLevelFilter.plID>0)//FIltro por ID
      {
        if(permissionLevel.plID!=_permissionLevelFilter.plID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_permissionLevelFilter.plN))//filtro por descripción
      {
        if (!permissionLevel.plN.Contains(_permissionLevelFilter.plN,StringComparison.OrdinalIgnoreCase))
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
