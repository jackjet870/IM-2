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

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmLocations.xaml
  /// </summary>
  public partial class frmLocations : Window
  {
    #region Variables
    public Location _locationFilter = new Location();//Objeto con los filtros de la ventana
    public int _nStatus = -1;//contiene el estatus de los registros de la ventana
    public bool _blnEdit = false;//Boleano para saber si se tiene permiso para editar|Agregar
    #endregion
    public frmLocations()
    {
      InitializeComponent();
    }

    #region Method Forms
    #region Search
    /// <summary>
    /// Abre la venatana detalle en modo busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmLocationDetail frmLocationDetail = new frmLocationDetail();
      frmLocationDetail.Owner = this;
      frmLocationDetail.enumMode = EnumMode.search;
      frmLocationDetail.oldLocation = _locationFilter;
      frmLocationDetail.nStatus = _nStatus;
      if(frmLocationDetail.ShowDialog()==true)
      {
        _locationFilter = frmLocationDetail.location;
        _nStatus = frmLocationDetail.nStatus;
        LoadLocations();
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
    /// [emoguel] created 01/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmLocationDetail frmLocationDetail = new frmLocationDetail();
      frmLocationDetail.Owner = this;
      frmLocationDetail.enumMode = EnumMode.add;
      if(frmLocationDetail.ShowDialog()==true)
      {
        Location location = frmLocationDetail.location;
        if (ValidateFilter(location))//Validar si cumple con los filtros actuales
        {
          List<Location> lstLocations = (List<Location>)dgrLanguages.ItemsSource;
          lstLocations.Add(location);//Agregamos el nuevo registro al grid
          lstLocations.Sort((x, y) => string.Compare(x.loN, y.loN));//ordenamos la lista
          int nIndex = lstLocations.IndexOf(location);//Obtenemos el indez del nuevo registro
          dgrLanguages.Items.Refresh();//Actualizamos la vista del grid
          StatusBarReg.Content = lstLocations.Count + "Locations.";//Actualizamos el contador
        }
            
      }
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Recarga el grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Location location = (Location)dgrLanguages.SelectedItem;
      LoadLocations(location);
    }
    #endregion

    #region Window KeyDown
    /// <summary>
    /// Verifica botones presionados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
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

    #region Loaded
    /// <summary>
    /// Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(Model.Enums.EnumPermission.Locations, Model.Enums.EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadLocations();
    }

    #endregion
    #region IsKeyboardFocuChanged
    /// <summary>
    /// Verifica que botones fueron presionados con la ventana inactiva
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 01/04/2016
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
    /// Muestra la ventana detalle en modo preview|edicion
    /// </summary>
    /// <history>
    /// [emoguel] 01/04/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Location location = (Location)dgrLanguages.SelectedItem;
      frmLocationDetail frmLocationDetail = new frmLocationDetail();
      frmLocationDetail.Owner = this;
      frmLocationDetail.oldLocation = location;
      frmLocationDetail.enumMode = ((_blnEdit)?EnumMode.edit:EnumMode.preview);
      if(frmLocationDetail.ShowDialog()==true)
      {
        int nIndex = dgrLanguages.SelectedIndex;
        List<Location> lstLocations = (List<Location>)dgrLanguages.ItemsSource;
        if (!ValidateFilter(frmLocationDetail.location))
        {
          lstLocations.Remove(location);
        }
        else
        {
          ObjectHelper.CopyProperties(location, frmLocationDetail.location);
          lstLocations.Sort((x, y) => string.Compare(x.loN, y.loN));
          nIndex = lstLocations.IndexOf(location);
        }
        dgrLanguages.Items.Refresh();
        GridHelper.SelectRow(dgrLanguages, nIndex);
        StatusBarReg.Content = lstLocations.Count+" Locations.";
      }
    }

    #endregion
    #endregion

    #region Methods
    #region LoadLocations
    /// <summary>
    /// Llena el grid de locations
    /// </summary>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    private async void LoadLocations(Location location=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Location> lstLocations =await BRLocations.GetLocations(_nStatus, _locationFilter);
        dgrLanguages.ItemsSource = lstLocations;
        if (location != null && lstLocations.Count > 0)
        {
          location = lstLocations.Where(lo => lo.loID == location.loID).FirstOrDefault();
          nIndex = lstLocations.IndexOf(location);
        }
        GridHelper.SelectRow(dgrLanguages, nIndex);
        StatusBarReg.Content = lstLocations.Count + " Locations.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Locations");
      }
    }
    #endregion

    #region ValidateFilter
    /// <summary>
    /// Valida que un objeto Location cumpla con los filtros actuales
    /// </summary>
    /// <param name="location">Objeto a validar</param>
    /// <returns>True. si cumple | false. No cumple</returns>
    /// <history>
    /// [emoguel] created 01/04/2014
    /// </history>
    private bool ValidateFilter(Location location)
    {
      if(_nStatus!=-1)//Filtro por estatus
      {
        if(location.loA==Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_locationFilter.loID))//Filtro por ID
      {
        if(location.loID!=_locationFilter.loID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_locationFilter.losr))//Filtro Sales Room
      {
        if(_locationFilter.losr!=location.losr)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_locationFilter.lolc))//Filtro por Categoria
      {
        if(_locationFilter.lolc!=location.lolc)
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
