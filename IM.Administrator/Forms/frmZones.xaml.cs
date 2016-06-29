using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Extensions;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmZones.xaml
  /// </summary>
  public partial class frmZones : Window
  {
    #region Variables
    private Zone _zoneFilter = new Zone();//OBjeto con los filtros del grid
    private int _nStatus = -1;//Estatus de los registros del grid
    private bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar|agregar
    #endregion
    public frmZones()
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
    /// [emoguel] created 07/06/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadZones();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
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
    /// [emoguel] created 07/06/2016
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
    /// [emoguel]07/06/2016
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
    /// [emoguel] created 07/06/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Zone zone = (Zone)dgrZones.SelectedItem;      
      frmZoneDetail frmZoneDetail = new frmZoneDetail();
      frmZoneDetail.Owner = this;
      frmZoneDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.preview;
      frmZoneDetail.oldZone = zone;
      if(frmZoneDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Zone> lstZones = (List<Zone>)dgrZones.ItemsSource;
        if(ValidateFilter(frmZoneDetail.zone))
        {
          ObjectHelper.CopyProperties(zone, frmZoneDetail.zone);//Actualizamos los datos
          lstZones.Sort((x, y) => string.Compare(x.znN, y.znN));//ordenamos la lista
          nIndex = lstZones.IndexOf(zone);//obtenemos la posición del registro
        }
        else
        {
          lstZones.Remove(zone);//Quitamos el registro
        }
        dgrZones.Items.Refresh();//Actualizamos la vista
        GridHelper.SelectRow(dgrZones, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstZones.Count + " Zones.";//Actualizamos el contador
      }
    }
    #endregion

    #region Add
    /// <summary>
    /// Abre la ventana detalle en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmZoneDetail frmZoneDatail = new frmZoneDetail();
      frmZoneDatail.Owner = this;
      frmZoneDatail.enumMode = EnumMode.add;
      if (frmZoneDatail.ShowDialog() == true)
      {
        if (ValidateFilter(frmZoneDatail.zone))//Verificamos que cumpla con los filtros
        {
          List<Zone> lstZones = (List<Zone>)dgrZones.ItemsSource;
          lstZones.Add(frmZoneDatail.zone);//Agregamos el registro
          lstZones.Sort((x, y) => string.Compare(x.znN, y.znN));//ordenamos la lista
          int nIndex = lstZones.IndexOf(frmZoneDatail.zone);//obtenemos la posición del registro
          dgrZones.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrZones, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstZones.Count + "Zones";//Actualizamos el contador
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
      frmSearch.strID = _zoneFilter.znID;
      frmSearch.strDesc = _zoneFilter.znN;
      frmSearch.nStatus = _nStatus;
      if(frmSearch.ShowDialog()==true)
      {
        _zoneFilter.znID = frmSearch.strID;
        _zoneFilter.znN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadZones();
      }
    }
    #endregion

    #region refresh
    /// <summary>
    /// Recarga los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Zone zone = (Zone)dgrZones.SelectedItem;
      LoadZones(zone);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadZones
    /// <summary>
    /// Llena el grid de Zones
    /// </summary>
    /// <param name="zone">objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private async void LoadZones(Zone zone = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Zone> lstZones = await BRZones.GetZones(_nStatus, _zoneFilter);
        dgrZones.ItemsSource = lstZones;
        if (lstZones.Count > 0 && zone != null)
        {
          zone = lstZones.Where(zn => zn.znID == zone.znID).FirstOrDefault();
          nIndex = lstZones.IndexOf(zone);
        }
        GridHelper.SelectRow(dgrZones, nIndex);
        StatusBarReg.Content = lstZones.Count + " Zones.";
        status.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Zones");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que el objeto cumpla con los filtros del grid
    /// </summary>
    /// <param name="zone">objeto a validar</param>
    /// <returns>True. si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    private bool ValidateFilter(Zone zone)
    {
      if(_nStatus!=-1)//FIltro por estatus
      {
        if(zone.znA!=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_zoneFilter.znID))//Filtro por ID
      {
        if(_zoneFilter.znID!=zone.znID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_zoneFilter.znN))//Filtro por descripción
      {
        if (!zone.znN.Contains(_zoneFilter.znN, StringComparison.OrdinalIgnoreCase))
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
