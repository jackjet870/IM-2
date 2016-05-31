using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Base.Helpers;
using IM.Model.Enums;
using System;
using System.Linq;
using IM.Model.Helpers;
using IM.Model.Extensions;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAreas.xaml
  /// </summary>
  public partial class frmAreas : Window
  {
    private Area _areaFiltro = new Area();//Objeto a filtrar en la lista
    private int _nStatus = -1;//Status a filtrar en la lista
    private bool _blnEdit = false;//boleano para saber si se tiene minimo permiso para editar|agregar 
    public frmAreas()
    {
      InitializeComponent();
    }

    #region eventos de los controles
    #region Loaded
    /// <summary>
    /// Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 02/003/2015
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadAreas();
    }
    #endregion
    #region KeyBoardFocusChaged
    /// <summary>
    /// Verifica teclas activas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 09/03/2016
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
    /// Valida que teclas fueron presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 02/003/2015
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

    #region Double Click Row
    /// <summary>
    /// Muestra la ventana de AreaDetalle en modo preview
    /// </summary>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Area Area = (Area)dgrAreas.SelectedItem;
      frmAreaDetalle frmAreaDetalle = new frmAreaDetalle();
      frmAreaDetalle.Owner = this;
      frmAreaDetalle.oldArea = Area;
      frmAreaDetalle.mode = ((_blnEdit == true) ? EnumMode.edit : EnumMode.preview);
      if (frmAreaDetalle.ShowDialog() == true)
      { 
        List<Area> lstAreas = (List<Area>)dgrAreas.ItemsSource;
        int nIndex = 0;
        if (!ValidateFilters(frmAreaDetalle.area))//VAlidamos si cumple con los filtros
        {
          lstAreas.Remove(Area);//quitamos el registro de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(Area, frmAreaDetalle.area);
          lstAreas.Sort((x, y) => string.Compare(x.arN, y.arN));//Ordenamos la lista    
          nIndex = lstAreas.IndexOf(Area);
        }
            
        dgrAreas.Items.Refresh();//Refrescamos la lista
        GridHelper.SelectRow(dgrAreas, nIndex);
        StatusBarReg.Content = lstAreas.Count + " Areas.";//Actualizamos el contador
      }
    }
    #endregion

    #region Add 
    /// <summary>
    /// Muestra la ventana de AreaDetalle para agregar un registro nuevo
    /// </summary>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {

      frmAreaDetalle frmAreaDetalle = new frmAreaDetalle();      
      frmAreaDetalle.Owner = this;
      frmAreaDetalle.oldArea = new Area();
      frmAreaDetalle.mode = EnumMode.add;//Agregar
      if (frmAreaDetalle.ShowDialog() == true)
      {
        if (ValidateFilters(frmAreaDetalle.area))//Validamos si cumple con los filtros
        {
          List<Area> lstAreas = (List<Area>)dgrAreas.ItemsSource;
          lstAreas.Add(frmAreaDetalle.area);//Agregamos el registro nuevo
          lstAreas.Sort((x, y) => string.Compare(x.arN, y.arN));//Ordenamos la lista
          int nIndex = lstAreas.IndexOf(frmAreaDetalle.area);//Obetenemos el index nuevo
          dgrAreas.Items.Refresh();//Refrescamos la lista
          GridHelper.SelectRow(dgrAreas, nIndex);
          StatusBarReg.Content = lstAreas.Count + " Areas.";//Actualizamos el contador
        }
      }

    }

    #endregion
    #region Refresh

    /// <summary>
    /// Recarga la lista de Areas
    /// </summary>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Area area = (Area)dgrAreas.SelectedItem;
      LoadAreas(area);
    }
    #endregion

    #region Row KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
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

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda cargando 
    /// los datos de busqueda que ya se hayan realizado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] 3/Mar/2016 Created
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.strID = _areaFiltro.arID;
      frmSearch.strDesc = _areaFiltro.arN;
      frmSearch.nStatus = _nStatus;
      frmSearch.Owner = this;
      //Abrir la ventana de Buscar y ver si decidió realizar algún filtro
      if (frmSearch.ShowDialog() == true)
      {
        _areaFiltro.arID = frmSearch.strID;
        _areaFiltro.arN = frmSearch.strDesc;
        _nStatus = frmSearch.nStatus;
        LoadAreas();
      }

    }
    #endregion
    #endregion
    #region METODOS
    #region LoadAreas
    /// <summary>
    /// carga la lista de Areas
    /// </summary>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// [emoguel] 30/05/2016 Modified
    /// </history>
    protected async void LoadAreas(Area area=null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Area> lstAreas =await BRAreas.GetAreas(_areaFiltro, _nStatus);
        dgrAreas.ItemsSource = lstAreas;
        if (area != null && lstAreas.Count > 0)
        {
          area = lstAreas.Where(ar => ar.arID == area.arID).FirstOrDefault();
          nIndex = lstAreas.IndexOf(area);
        }
        GridHelper.SelectRow(dgrAreas, nIndex);
        StatusBarReg.Content = lstAreas.Count + " Areas.";
        status.Visibility = Visibility.Collapsed;
      }
      catch(Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Area");
      }
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo Area coincide con los filtros
    /// </summary>
    /// <param name="newArea">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(Area newArea)
    {
      if (_nStatus != -1)
      {
        if (newArea.arA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_areaFiltro.arID))
      {
        if (_areaFiltro.arID != newArea.arID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_areaFiltro.arN))
      {
        if (!newArea.arN.Contains(_areaFiltro.arN,StringComparison.OrdinalIgnoreCase))
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
