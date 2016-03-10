using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmChargeTo.xaml
  /// </summary>
  public partial class frmChargeTo : Window
  {
    private ChargeTo _chargeToFilter = new ChargeTo();//Objeto para filtrar el grid
    private int _nStatus = -1;//Estado de los datos del grid
    private bool _blnEdit = false;//boleano para saber si se puede editar|agregar
    public frmChargeTo()
    {
      InitializeComponent();
    }

    #region event controls
    #region Loaded
    /// <summary>
    /// Carga los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 02/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Helpers.PermisionHelper.EditPermision("HOSTINVIT");
      btnAdd.IsEnabled = _blnEdit;
      LoadChargeTo();
    }

    #endregion
    #region KeyDown Form
    /// <summary>
    /// Valida la tecla presionada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// [Emoguel] created 02/03/2016
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
    #region Cell Double click
    /// <summary>
    /// Muestra la ventada Charge To preview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {

      DataGridRow row = sender as DataGridRow;
      ChargeTo chargeTo = (ChargeTo)row.DataContext;
      frmChargeToDetail frmChargeToDetail = new frmChargeToDetail();
      frmChargeToDetail.Owner = this;
      frmChargeToDetail.mode = ((_blnEdit == true) ? ModeOpen.edit : ModeOpen.preview);
      frmChargeToDetail.chargeTo = chargeTo;
      frmChargeToDetail.ShowDialog();
    }

    #endregion
    #region  Refresh
    /// <summary>
    /// Recarga los datos del datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadChargeTo();
    }
    #endregion
    #region KeyBoardChange
    /// <summary>
    /// Verifica que teclas están oprimidas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {

      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion
    #region Search
    /// <summary>
    /// Abre la ventana de busqueda o filtros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.sID = _chargeToFilter.ctID;
      frmSearch.sDesc = _chargeToFilter.ctPrice.ToString();
      frmSearch.nStatus = _nStatus;
      frmSearch.sForm = "ChargeTo";
      frmSearch.Owner = this;
      //Abrir la ventana de Buscar y ver si decidió realizar algún filtro
      if (frmSearch.ShowDialog() == true)
      {
        _chargeToFilter.ctID = frmSearch.sID;
        _chargeToFilter.ctPrice = Convert.ToByte(frmSearch.sDesc);
        _nStatus = frmSearch.nStatus;
        LoadChargeTo();
      }
    }

    #endregion
    #region Add
    /// <summary>
    /// Muestra la ventana Detail ChargeTo para agregar un nuevo registro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmChargeToDetail frmChargeToDetail = new frmChargeToDetail();
      frmChargeToDetail.chargeTo = new ChargeTo();
      frmChargeToDetail.Owner = this;
      frmChargeToDetail.mode = ModeOpen.add;
      if (frmChargeToDetail.ShowDialog() == true)
      {
        LoadChargeTo();
      }
    }
    #endregion
    
    #endregion

    #region Metodos
    #region LoadChargeTos
    /// <summary>
    /// Llena el dataset con la lista de chargeTo
    /// </summary>
    /// <history>
    /// [Emoguel] created 02/03/2016
    /// </history>
    protected void LoadChargeTo()
    {
      List<ChargeTo> lstChargeTo = BRChargeTos.GetChargeTos(_chargeToFilter, _nStatus);
      dgrChargeTo.ItemsSource = lstChargeTo;
      if (lstChargeTo.Count > 0)
      {
        dgrChargeTo.SelectedIndex = 0;
      }

      StatusBarReg.Content = lstChargeTo.Count + " Charge To.";
    }
    #endregion

    #endregion
    
  }
}
