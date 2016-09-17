using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMemberShipTypes.xaml
  /// </summary>
  public partial class frmMembershipTypes : Window
  {
    #region Variables
    private MembershipType _membershipTypeFilter = new MembershipType { mtGroup=""};//Objeto con los filtros del grid
    private int _nStatus = -1;//estatus de los registros del grid
    #endregion
    public frmMembershipTypes()
    {
      InitializeComponent();
    }

    #region Methods Form
    #region DoubleClick cell
    /// <summary>
    /// Muestra la ventana detalle en modo ReadOnly|edicion
    /// </summary>
    /// <history>
    /// [emoguel] 04/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      MembershipType membershipType = (MembershipType)dgrMemberShipTypes.SelectedItem;
      frmMembershipTypeDetail frmMemshipTypeDetail = new frmMembershipTypeDetail();
      frmMemshipTypeDetail.Owner = this;
      frmMemshipTypeDetail.enumMode = EnumMode.Edit;
      frmMemshipTypeDetail.oldMembershipType = membershipType;
      frmMemshipTypeDetail.oldMembershipType = membershipType;
      if (frmMemshipTypeDetail.ShowDialog() == true)
      {
        int nIndex = 0;
        List<MembershipType> lstMemberShipTypes = (List<MembershipType>)dgrMemberShipTypes.ItemsSource;
        if(!ValidateFilter(frmMemshipTypeDetail.membershipType))
        {
          lstMemberShipTypes.Remove(membershipType);
        }
        else
        {
          ObjectHelper.CopyProperties(membershipType, frmMemshipTypeDetail.membershipType);
          lstMemberShipTypes.Sort((x, y) => string.Compare(x.mtN, y.mtN));
          nIndex = lstMemberShipTypes.IndexOf(membershipType);
        }
        dgrMemberShipTypes.Items.Refresh();
        StatusBarReg.Content = lstMemberShipTypes.Count + " Membership Types.";
        GridHelper.SelectRow(dgrMemberShipTypes, nIndex);        
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
    /// [emoguel] created 04/04/2016
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

    #region Window KeyDown
    /// <summary>
    /// Verifica botones presionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
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

    #region IsKeyboardFocuChanged
    /// <summary>
    /// Verifica que botones fueron presionados con la ventana inactiva
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/06
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Window Loaded
    /// <summary>
    /// carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadMemberShipTypes();
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Actualiza el grid de MembershipTypes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      MembershipType membershipType = (MembershipType)dgrMemberShipTypes.SelectedItem;
      LoadMemberShipTypes(membershipType);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana de detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <hisotory>
    /// [emoguel] created 04/04/2016
    /// </hisotory>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmMembershipTypeDetail frmMemshipTypeDetail = new frmMembershipTypeDetail();
      frmMemshipTypeDetail.Owner = this;
      frmMemshipTypeDetail.enumMode = EnumMode.Add;
      if(frmMemshipTypeDetail.ShowDialog()==true)
      {
        MembershipType membershipType = frmMemshipTypeDetail.membershipType;
        if(ValidateFilter(membershipType))//validamos si cumple con los filtros actuales
        {
          List<MembershipType> lstMemberShipTypes = (List<MembershipType>)dgrMemberShipTypes.ItemsSource;
          lstMemberShipTypes.Add(membershipType);//Agregamos el registro
          lstMemberShipTypes.Sort((x, Y) => string.Compare(x.mtN, Y.mtN));//ordenamos la lista
          int nIndex = 0;
          nIndex = lstMemberShipTypes.IndexOf(membershipType);//Obtenemos el index
          dgrMemberShipTypes.Items.Refresh();//actualizamos la vista
          StatusBarReg.Content = lstMemberShipTypes.Count + " Membership Types.";
          GridHelper.SelectRow(dgrMemberShipTypes, nIndex);//Seleccionamos el nuevo registro
        }
      }
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana detalle en modo busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmMembershipTypeDetail frmMemshipTypeDetail = new frmMembershipTypeDetail();
      frmMemshipTypeDetail.Owner = this;
      frmMemshipTypeDetail.enumMode = EnumMode.Search;
      frmMemshipTypeDetail.nStatus = _nStatus;
      frmMemshipTypeDetail.oldMembershipType = _membershipTypeFilter;
      if(frmMemshipTypeDetail.ShowDialog()==true)
      {
        _nStatus = frmMemshipTypeDetail.nStatus;
        _membershipTypeFilter = frmMemshipTypeDetail.membershipType;
        LoadMemberShipTypes();
      }
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadMemberShipTypes
    /// <summary>
    /// Llena el grid de MemberShip Types
    /// </summary>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private async void LoadMemberShipTypes(MembershipType memberShipType=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<MembershipType> lstMemberShipTypes = await BRMemberShipTypes.GetMemberShipTypes(_nStatus, _membershipTypeFilter);
        dgrMemberShipTypes.ItemsSource = lstMemberShipTypes;
        if (lstMemberShipTypes.Count > 0 && memberShipType != null)
        {
          memberShipType = lstMemberShipTypes.Where(mt => mt.mtID == memberShipType.mtID).FirstOrDefault();
          nIndex = lstMemberShipTypes.IndexOf(memberShipType);
        }
        GridHelper.SelectRow(dgrMemberShipTypes, nIndex);
        StatusBarReg.Content = lstMemberShipTypes.Count + "MemberShip Types.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un objeto MembershipType cumpla con los filtros actuales
    /// </summary>
    /// <param name="memberShipType">Objeto a validar</param>
    /// <returns>True. si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 04/04/2016
    /// </history>
    private bool ValidateFilter(MembershipType memberShipType)
    {
      if (_nStatus != -1)//Filtro por estatus
      {
        if (memberShipType.mtA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_membershipTypeFilter.mtID))//Filtro por ID
      {
        if (_membershipTypeFilter.mtID != memberShipType.mtID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_membershipTypeFilter.mtN))//Filtro por descripció
      {
        if(!memberShipType.mtN.Contains(_membershipTypeFilter.mtN,StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_membershipTypeFilter.mtGroup))//Filtro por grupo
      {
        if(_membershipTypeFilter.mtGroup!=memberShipType.mtGroup)
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
