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

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmHotelGroups.xaml
  /// </summary>
  public partial class frmHotelGroups : Window
  {
    #region Variables
    private HotelGroup _hotelGroupFilter = new HotelGroup();//Objeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar|Guardar
    #endregion
    public frmHotelGroups()
    {
      InitializeComponent();
    }

    #region Method Forms
    #region Loaded
    /// <summary>
    /// Carga los datos de la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadHotelGroups();
    } 
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 12/05/2016
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
    /// [emoguel] created 12/05/2016
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
    /// [emoguel] created 12/05/2016
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
    /// [emoguel] created 12/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      HotelGroup hotelGroup = (HotelGroup)dgrHotelGroups.SelectedItem;
      frmHotelGroupDetail frmHotelGroupDetail = new frmHotelGroupDetail();
      frmHotelGroupDetail.Owner = this;
      frmHotelGroupDetail.oldHotelGroup = hotelGroup;
      frmHotelGroupDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.add;
      if (frmHotelGroupDetail.ShowDialog()==true)
      {
        List<HotelGroup> lstHotelGroup = (List<HotelGroup>)dgrHotelGroups.ItemsSource;
        int nIndex = 0;
        if(ValidateFilter(frmHotelGroupDetail.hotelGroup))
        {
          ObjectHelper.CopyProperties(hotelGroup, frmHotelGroupDetail.hotelGroup);//Actualizar los datos
          lstHotelGroup.Sort((x, y) => string.Compare(x.hgN, y.hgN));//Ordenar la lista
          nIndex = lstHotelGroup.IndexOf(hotelGroup);//Buscamos la posición del registro
        }
        else
        {
          lstHotelGroup.Remove(hotelGroup);
        }
        dgrHotelGroups.Items.Refresh();//Actualizar la vista
        GridHelper.SelectRow(dgrHotelGroups, nIndex);//Seleccionamos l registro
        StatusBarReg.Content = lstHotelGroup.Count + " Hotel Groups.";//Actualizar el contador
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
    /// [emoguel] created 12/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      HotelGroup hotelGroup = (HotelGroup)dgrHotelGroups.SelectedItem;
      LoadHotelGroups(hotelGroup);
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo Add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmHotelGroupDetail frmHotelGroupDetail = new frmHotelGroupDetail();
      frmHotelGroupDetail.Owner = this;
      frmHotelGroupDetail.enumMode = EnumMode.add;
      if(frmHotelGroupDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmHotelGroupDetail.hotelGroup))
        {
          List<HotelGroup> lstHotelGroup = (List<HotelGroup>)dgrHotelGroups.ItemsSource;
          lstHotelGroup.Add(frmHotelGroupDetail.hotelGroup);//Agregamos el registro
          lstHotelGroup.Sort((x, y) => string.Compare(x.hgN, y.hgN));//Ordenamos la lista
          int nIndex = lstHotelGroup.IndexOf(frmHotelGroupDetail.hotelGroup);//Buscamos la posición del registro
          dgrHotelGroups.Items.Refresh();
          GridHelper.SelectRow(dgrHotelGroups, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstHotelGroup.Count + " Hotel Groups.";//Actualizamos el contador
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
    /// [emoguel] created 12/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _hotelGroupFilter.hgID;
      frmSearch.strDesc = _hotelGroupFilter.hgN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _hotelGroupFilter.hgID = frmSearch.strID;
        _hotelGroupFilter.hgN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadHotelGroups();
      }
    } 
    #endregion
    #endregion

    #region Methods
    #region LoadHotelGroups
    /// <summary>
    /// Llena el grid de Hotel Groups
    /// </summary>
    /// <param name="hotelGroup">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private void LoadHotelGroups(HotelGroup hotelGroup = null)
    {
      List<HotelGroup> lstHotelsGroup = BRHotelGroups.GetHotelGroups(_hotelGroupFilter, _nStatus);
      dgrHotelGroups.ItemsSource = lstHotelsGroup;
      int nIndex = 0;
      if (lstHotelsGroup.Count > 0 && hotelGroup != null)
      {
        hotelGroup = lstHotelsGroup.Where(ho => ho.hgID == hotelGroup.hgID).FirstOrDefault();
        nIndex = lstHotelsGroup.IndexOf(hotelGroup);
      }
      GridHelper.SelectRow(dgrHotelGroups, nIndex);
      StatusBarReg.Content = lstHotelsGroup.Count + " Hotel Groups";

    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un registro cumpla con los filtros de grid
    /// </summary>
    /// <param name="hotelGroup">onjeto a valida</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 12/05/2016
    /// </history>
    private bool ValidateFilter(HotelGroup hotelGroup)
    {
      if (_nStatus != -1)
      {
        if (hotelGroup.hgA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_hotelGroupFilter.hgID))
      {
        if (hotelGroup.hgID != _hotelGroupFilter.hgID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_hotelGroupFilter.hgN))
      {
        if (!hotelGroup.hgN.Contains(_hotelGroupFilter.hgN))
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
