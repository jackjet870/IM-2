using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using IM.Model.Enums;
using System.Linq;
using IM.Model.Helpers;

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
      _blnEdit = App.User.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.Standard);
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
      ChargeTo chargeTo = (ChargeTo)dgrChargeTo.SelectedItem;
      frmChargeToDetail frmChargeToDetail = new frmChargeToDetail();
      frmChargeToDetail.Owner = this;
      frmChargeToDetail.mode = ((_blnEdit == true) ? EnumMode.edit : EnumMode.preview);
      frmChargeToDetail.oldChargeTo = chargeTo;
      if(frmChargeToDetail.ShowDialog()==true)
      {
        List<ChargeTo> lstCargeTos = (List<ChargeTo>)dgrChargeTo.ItemsSource;
        int nIndex = 0;
        if(!ValidateFilters(frmChargeToDetail.chargeTo))
        {
          lstCargeTos.Remove(chargeTo);//quitamos el registro de la lista
        }
        else
        {
          ObjectHelper.CopyProperties(chargeTo, frmChargeToDetail.chargeTo);
          lstCargeTos.Sort((x, y) => string.Compare(x.ctID, y.ctID));//Ordenamos la lista   
          nIndex = lstCargeTos.IndexOf(chargeTo);
        }             
        dgrChargeTo.Items.Refresh();//Refrescamos el grid
        GridHelper.SelectRow(dgrChargeTo, nIndex);
        StatusBarReg.Content = lstCargeTos.Count + " Carge Tos.";
      }
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
      ChargeTo chargeTo = (ChargeTo)dgrChargeTo.SelectedItem;
      LoadChargeTo( chargeTo);
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
      frmSearch.strID = _chargeToFilter.ctID;
      frmSearch.strDesc = _chargeToFilter.ctPrice.ToString();
      frmSearch.nStatus = _nStatus;
      frmSearch.enumWindow = EnumWindow.ChargeTos;
      frmSearch.Owner = this;
      //Abrir la ventana de Buscar y ver si decidió realizar algún filtro
      if (frmSearch.ShowDialog() == true)
      {
        _chargeToFilter.ctID = frmSearch.strID;
        _chargeToFilter.ctPrice = Convert.ToByte(frmSearch.strDesc);
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
      frmChargeToDetail.Owner = this;
      frmChargeToDetail.mode = EnumMode.add;//insertar

      if (frmChargeToDetail.ShowDialog() == true)
      {
        if (ValidateFilters(frmChargeToDetail.chargeTo))//Valida si cumple con los filtros actuales
        {
          List<ChargeTo> lstCargeTos = (List<ChargeTo>)dgrChargeTo.ItemsSource;
          lstCargeTos.Add(frmChargeToDetail.chargeTo);//Agregamos el nuevo registro
          lstCargeTos.Sort((x, y) => string.Compare(x.ctID, y.ctID));//Ordenamos la lista
          int nIndex = lstCargeTos.IndexOf(frmChargeToDetail.chargeTo);//Obtenemos el index
          dgrChargeTo.Items.Refresh();//Refrescamos el grid
          GridHelper.SelectRow(dgrChargeTo, nIndex);
          StatusBarReg.Content = lstCargeTos.Count + " Carge Tos.";
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
    #endregion

    #region Metodos
    #region LoadChargeTos
    /// <summary>
    /// Llena el dataset con la lista de chargeTo
    /// </summary>
    /// <history>
    /// [Emoguel] created 02/03/2016
    /// </history>
    protected void LoadChargeTo(ChargeTo chargeTo = null)
    {
      int nIndex = 0;
      List<ChargeTo> lstChargeTo = BRChargeTos.GetChargeTos(_chargeToFilter, _nStatus);
      dgrChargeTo.ItemsSource = lstChargeTo;
      if (chargeTo != null && lstChargeTo.Count>0)
      {
        chargeTo= lstChargeTo.Where(ch => ch.ctID == chargeTo.ctID).FirstOrDefault();
        nIndex = lstChargeTo.IndexOf(chargeTo);
      }
      GridHelper.SelectRow(dgrChargeTo, nIndex);
      StatusBarReg.Content = lstChargeTo.Count + " Charge To.";
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo ChargeTo coincide con los filtros
    /// </summary>
    /// <param name="newChargeTo">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(ChargeTo newChargeTo)
    {
      if (_nStatus != -1)
      {
        if (newChargeTo.ctIsCxC !=Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_chargeToFilter.ctID))
      {
        if (_chargeToFilter.ctID != newChargeTo.ctID)
        {
          return false;
        }
      }

      if (_chargeToFilter.ctPrice>0)
      {
        if (newChargeTo.ctPrice==_chargeToFilter.ctPrice)
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
