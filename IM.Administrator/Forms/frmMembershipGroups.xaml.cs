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
  /// Interaction logic for frmMembershipGroups.xaml
  /// </summary>
  public partial class frmMembershipGroups : Window
  {
    #region variables
    private MembershipGroup _memberShipGroupFilter = new MembershipGroup();//Filtros de la ventana
    private int _nStatus = -1;//Estatus de los registros de la ventana
    #endregion
    public frmMembershipGroups()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadMembershipGroups();
    } 
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region window keyDown
    /// <summary>
    /// Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
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
    /// cambia de fila con el boton tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
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

    #region DobleClic Grid
    /// <summary>
    /// Abre la ventana detalle en modo "detalle" o "edición" dependiendo de sus permisos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      MembershipGroup membershipGroup = (MembershipGroup)dgrMembershipGroup.SelectedItem;
      frmMembershipGroupDetail frmMembershipGroup = new frmMembershipGroupDetail();
      frmMembershipGroup.Owner = this;
      frmMembershipGroup.oldMembershipGroup = membershipGroup;
      frmMembershipGroup.enumMode = EnumMode.edit;
      if(frmMembershipGroup.ShowDialog()==true)
      {
        List<MembershipGroup> lstMembershipGroups = (List<MembershipGroup>)dgrMembershipGroup.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmMembershipGroup.membershipGroup))//Verificamos que cumpla con los filtros
        {
          ObjectHelper.CopyProperties(membershipGroup, frmMembershipGroup.membershipGroup);//Actualizamos los datos del registro
          lstMembershipGroups.Sort((x, y) => string.Compare(x.mgN, y.mgN));//Ordenamos la lista
          nIndex = lstMembershipGroups.IndexOf(membershipGroup);//Obtenemos la posición del registro
        }
        else
        {
          lstMembershipGroups.Remove(membershipGroup);//Quitamos el registro
        }
        dgrMembershipGroup.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrMembershipGroup, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstMembershipGroups.Count + " Membership Groups";//Actualizamos el contador
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
    /// [emoguel] created 19/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _memberShipGroupFilter.mgID;
      frmSearch.strDesc = _memberShipGroupFilter.mgN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _memberShipGroupFilter.mgID = frmSearch.strID;
        _memberShipGroupFilter.mgN = frmSearch.strDesc;
        LoadMembershipGroups();
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmMembershipGroupDetail frmMembershipGroupDet = new frmMembershipGroupDetail();
      frmMembershipGroupDet.Owner = this;
      frmMembershipGroupDet.enumMode = EnumMode.add;
      if(frmMembershipGroupDet.ShowDialog()==true)
      {
        if(ValidateFilter(frmMembershipGroupDet.membershipGroup))//Verificamos que cumpla con los filtros
        {
          List<MembershipGroup> lstMembershipGroup = (List<MembershipGroup>)dgrMembershipGroup.ItemsSource;
          lstMembershipGroup.Add(frmMembershipGroupDet.membershipGroup);//Agregamos el registro
          lstMembershipGroup.Sort((x, y) => string.Compare(x.mgN, y.mgN));//Ordenamos la lista
          int nIndex = lstMembershipGroup.IndexOf(frmMembershipGroupDet.membershipGroup);//Obtenemos la posición del registro
          dgrMembershipGroup.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrMembershipGroup, nIndex);//Seleccionar el registro
          StatusBarReg.Content = lstMembershipGroup.Count + " Membership Groups.";//Actualizar el contador
        }
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 19/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      MembershipGroup membershipGroup = (MembershipGroup)dgrMembershipGroup.SelectedItem;
      LoadMembershipGroups(membershipGroup);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadMembershipGroups
    /// <summary>
    /// Llena el grid de MembershipGroups
    /// </summary>
    /// <param name="membershipGroup">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private async void LoadMembershipGroups(MembershipGroup membershipGroup = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<MembershipGroup> lstMembershipGroups = await BRMembershipGroups.GetMembershipGroups(_nStatus, _memberShipGroupFilter);
        dgrMembershipGroup.ItemsSource = lstMembershipGroups;
        if (lstMembershipGroups.Count > 0 && membershipGroup != null)
        {
          membershipGroup = lstMembershipGroups.Where(mg => mg.mgID == membershipGroup.mgID).FirstOrDefault();
          nIndex = lstMembershipGroups.IndexOf(membershipGroup);
        }
        GridHelper.SelectRow(dgrMembershipGroup, nIndex);
        StatusBarReg.Content = lstMembershipGroups.Count + " Membership Groups.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.InnerException.Message, MessageBoxImage.Error, "Membership Group");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un membershipGroup cumpla con los filtros actuales
    /// </summary>
    /// <param name="membershipGroup">Objeto  validar</param>
    /// <returns>True. si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 19/05/2016
    /// </history>
    private bool ValidateFilter(MembershipGroup membershipGroup)
    {
      if(_nStatus!=-1)
      {
        if(membershipGroup.mgA!=Convert.ToBoolean(_nStatus))//Filtro por estatus
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_memberShipGroupFilter.mgID))//FIltro por ID
      {
        if(membershipGroup.mgID!=_memberShipGroupFilter.mgID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_memberShipGroupFilter.mgN))//Filtro por estatus
      {
        if(!membershipGroup.mgN.Contains(_memberShipGroupFilter.mgN,StringComparison.OrdinalIgnoreCase))
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
