using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Administrator.Enums;
using IM.Base.Helpers;

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
      _blnEdit = App.User.HasPermission("LOCATIONS",Model.Enums.EnumPermisionLevel.Standard);
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
      DataGridRow row = sender as DataGridRow;
      Area Area = (Area)row.DataContext;
      frmAreaDetalle frmAreaDetalle = new frmAreaDetalle();
      frmAreaDetalle.Owner = this;
      ObjectHelper.CopyProperties(frmAreaDetalle.area, Area);
      frmAreaDetalle.mode = ((_blnEdit==true)?ModeOpen.edit: ModeOpen.preview);   
      if(frmAreaDetalle.ShowDialog()==true)
      {
        LoadAreas();
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
      frmAreaDetalle.area = new Area();
      frmAreaDetalle.Owner = this;
      frmAreaDetalle.mode = ModeOpen.add;//Agregar
      if (frmAreaDetalle.ShowDialog() == true)
      {
        LoadAreas();
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
      LoadAreas();
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
      frmSearch.sID = _areaFiltro.arID;
      frmSearch.sDesc = _areaFiltro.arN;
      frmSearch.nStatus = _nStatus;
      frmSearch.Owner = this;
      //Abrir la ventana de Buscar y ver si decidió realizar algún filtro
      if (frmSearch.ShowDialog() == true)
      {
        _areaFiltro.arID = frmSearch.sID;
        _areaFiltro.arN = frmSearch.sDesc;
        _nStatus = frmSearch.nStatus;
        LoadAreas();
      }

    } 
    #endregion
    #endregion
    #region METODOS
    /// <summary>
    /// carga la lista de Areas
    /// </summary>
    /// <history>
    /// [emoguel] 26/Feb/2016 Created
    /// </history>
    protected void LoadAreas()
    {
      List<Area> lstAreas = BRAreas.GetAreas(_areaFiltro, _nStatus);
      dgrAreas.ItemsSource = lstAreas;
      if (lstAreas.Count > 0)
      {        
        dgrAreas.SelectedIndex = 0;
      }
      StatusBarReg.Content = lstAreas.Count + " Areas.";

    }

    #endregion
    
  }
}
