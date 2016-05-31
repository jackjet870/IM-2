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
  /// Interaction logic for frmRegions.xaml
  /// </summary>
  public partial class frmRegions : Window
  {
    #region Variables
    private Region _regionFilter = new Region();//objeto con filtros adicionales
    private int _nSatus = -1;//estatus de los registros del grid
    private bool _blnEdit = false;//boleano para saber si se tiene permiso para editar
    #endregion
    public frmRegions()
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
    /// [emoguel] 14/04/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadRegions();
    } 
    #endregion

    #region KeyBoardFocusChage
    /// <summary>
    /// Verifica teclas oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel created 14/04/2016
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
    /// [emoguel] created 14/04/2016
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
    /// [emoguel] created 14/04/2016
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
    /// [emoguel] 15/04/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Region region = (Region)dgrRegions.SelectedItem;
      frmRegionDetail frmRegionDetail = new frmRegionDetail();
      frmRegionDetail.Owner = this;
      frmRegionDetail.enumMode = (_blnEdit) ? EnumMode.edit : EnumMode.preview;
      frmRegionDetail.oldRegion = region;
      if(frmRegionDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Region> lstRegions = (List<Region>)dgrRegions.ItemsSource;
        if(ValidateFilter(frmRegionDetail.region))
        {
          ObjectHelper.CopyProperties(region, frmRegionDetail.region);//Actualizamos los datos
          lstRegions.Sort((x, y) => string.Compare(x.rgN, y.rgN));//ordenamos la lista
          nIndex = lstRegions.IndexOf(region);//obtenemos la posicion del registro
        }
        else
        {
          lstRegions.Remove(region);//Quitamos el registro de la lista
        }
        dgrRegions.Items.Refresh();//Actualizamos la lista
        GridHelper.SelectRow(dgrRegions, nIndex);//Seleccionamos el registro
        StatusBarReg.Content = lstRegions.Count + " Regions.";//Actualizamos el contador
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
    /// [emoguel] created 15/04/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _regionFilter.rgID;
      frmSearch.strDesc = _regionFilter.rgN;
      frmSearch.nStatus = _nSatus;
      if (frmSearch.ShowDialog() == true)
      {
        _nSatus = frmSearch.nStatus;
        _regionFilter.rgID = frmSearch.strID;
        _regionFilter.rgN = frmSearch.strDesc;
        LoadRegions();
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
    /// [emoguel] created 15/04/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmRegionDetail frmRegionDetail = new frmRegionDetail();
      frmRegionDetail.Owner = this;
      frmRegionDetail.enumMode = EnumMode.add;
      if(frmRegionDetail.ShowDialog()==true)
      {
        Region region =frmRegionDetail.region;
        if(ValidateFilter(region))
        {
          List<Region> lstRegions = (List<Region>)dgrRegions.ItemsSource;
          lstRegions.Add(region);//Agregamos el nuevo registro
          lstRegions.Sort((x, y) => string.Compare(x.rgN, y.rgN));//Ordenamos la lista
          int nIndex = lstRegions.IndexOf(region);//Obtenemos la posición del nuevo registro
          dgrRegions.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrRegions, nIndex);//Seleccionamos el nuevo registro
          StatusBarReg.Content = lstRegions.Count + " Regions.";//Actualizamos el contador
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
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Region region = (Region)dgrRegions.SelectedItem;
      LoadRegions(region);
    } 
    #endregion
    #endregion
    #region Methods
    #region LoadRegions
    /// <summary>
    /// Carga los datos del grid
    /// </summary>
    /// <param name="region">Objeto a seleccionar</param>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    private async void LoadRegions(Region region=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Region> lstRegions = await BRRegions.GetRegions(_nSatus, _regionFilter);
        dgrRegions.ItemsSource = lstRegions;
        if (lstRegions.Count > 0 && region != null)
        {
          region = lstRegions.Where(rg => rg.rgID == region.rgID).FirstOrDefault();
          nIndex = lstRegions.IndexOf(region);
        }
        GridHelper.SelectRow(dgrRegions, nIndex);
        StatusBarReg.Content = lstRegions.Count + " Regions.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Regions");
      }
    }
    #endregion
    #region ValidateFilter
    /// <summary>
    /// Valida si un registro cumple con los filtros actuales
    /// </summary>
    /// <param name="region">Objeto a validar</param>
    /// <returns>True. Si cumple | False. No cumple</returns>
    /// <history>
    /// [emoguel] created 15/04/2016
    /// </history>
    private bool ValidateFilter(Region region)
    {
      if(_nSatus!=-1)//FIltro por estatus
      {
        if(region.rgA!=Convert.ToBoolean(_nSatus))
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_regionFilter.rgID))//Filtro por ID
      {
        if(_regionFilter.rgID!=region.rgID)
        {
          return false;
        }
      }

      if(!string.IsNullOrWhiteSpace(_regionFilter.rgN))//filtro por descripción
      {
        if(!region.rgN.Contains(_regionFilter.rgN))
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
