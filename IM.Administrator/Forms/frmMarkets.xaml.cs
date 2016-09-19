using IM.Base.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Extensions;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmMarkets.xaml
  /// </summary>
  public partial class frmMarkets : Window
  {
    #region Variables
    private Market _marketFilter = new Market();//Objeto con los filtros de la ventana
    private int _nStatus = -1;//Estatus de los filtros del grid
    private bool _blnEdit = false;//Boleano para daber si se puede editar
    #endregion
    public frmMarkets()
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
    /// [emoguel] created 18/05/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = Context.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.Standard);
      btnAdd.IsEnabled = _blnEdit;
      LoadMarkets();
    }
    #endregion

    #region keyboardFocusChage
    /// <summary>
    /// Verifica que teclas están presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/05/2016
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
    /// [emoguel] created 18/05/2016
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
    /// [emoguel] created 18/05/2016
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
    /// [emoguel] created 18/05/2016
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Market market = (Market)dgrMarkets.SelectedItem;
      frmMarketDetail frmMarketDetail = new frmMarketDetail();
      frmMarketDetail.Owner = this;
      frmMarketDetail.enumMode = (_blnEdit) ? EnumMode.Edit : EnumMode.ReadOnly;
      frmMarketDetail.oldMarket = market;
      if(frmMarketDetail.ShowDialog()==true)
      {
        int nIndex = 0;
        List<Market> lstmarkets = (List<Market>)dgrMarkets.ItemsSource;
        if(ValidateFilter(frmMarketDetail.market))//Validamos si cumple con los filtros
        {
          ObjectHelper.CopyProperties(market, frmMarketDetail.market);//Actualizamos el objeto
          lstmarkets.Sort((x, y) => string.Compare(x.mkN, y.mkN));//Ordenamos la lista
          nIndex = lstmarkets.IndexOf(market);//Obtenemos la posición del registro
        }
        else
        {
          lstmarkets.Remove(market);//QUitamos el registro
        }
        dgrMarkets.Items.Refresh();//Actualizamos la vista
        StatusBarReg.Content = lstmarkets.Count + " Markets.";//Actualizamos el contador
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
    /// [emoguel] created 18/05/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.Owner = this;
      frmSearch.strID = _marketFilter.mkID;
      frmSearch.strDesc = _marketFilter.mkN;
      frmSearch.nStatus = _nStatus;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _marketFilter.mkID = frmSearch.strID;
        _marketFilter.mkN = frmSearch.strDesc;
        LoadMarkets();
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
    /// [emoguel] created 18/05/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmMarketDetail frmMarketDetail = new frmMarketDetail();
      frmMarketDetail.Owner = this;
      frmMarketDetail.enumMode = EnumMode.Add;
      if(frmMarketDetail.ShowDialog()==true)
      {
        if(ValidateFilter(frmMarketDetail.market))//Validamos que cumpla con los filtros
        {
          List<Market> lstmarkets = (List<Market>)dgrMarkets.ItemsSource;
          lstmarkets.Add(frmMarketDetail.market);//Agregamos el registro
          lstmarkets.Sort((x, y) => string.Compare(x.mkN, y.mkN));//Ordenamos la lista
          int nIndex = lstmarkets.IndexOf(frmMarketDetail.market);//obtenemos la posicion del registro
          dgrMarkets.Items.Refresh();//Actualizamos la vista
          GridHelper.SelectRow(dgrMarkets, nIndex);//Seleccionamos el registro
          StatusBarReg.Content = lstmarkets.Count + " Markets.";//Actualizamos el contador
        }
      }

    }
    #endregion

    #region refresh
    /// <summary>
    /// Actualiza los registros del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Market market = (Market)dgrMarkets.SelectedItem;
      LoadMarkets(market);
    }
    #endregion
    #endregion

    #region Methods
    #region LoadMarkets
    /// <summary>
    /// Llena el grid de markets
    /// </summary>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// </history>
    private async void LoadMarkets(Market market = null)
    {
      try
      {
        status.Visibility = Visibility.Visible;
        int nIndex = 0;
        List<Market> lstMarkets =await BRMarkets.GetMarkets(_marketFilter, _nStatus);
        dgrMarkets.ItemsSource = lstMarkets;
        if (lstMarkets.Count > 0 && market != null)
        {
          market = lstMarkets.Where(mk => mk.mkID == market.mkID).FirstOrDefault();
          nIndex = lstMarkets.IndexOf(market);
        }
        GridHelper.SelectRow(dgrMarkets, nIndex);
        StatusBarReg.Content = lstMarkets.Count + " Markets.";
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
    /// Valida que un Market cumpla con los filtros actuales
    /// </summary>
    /// <param name="market">Objeto a validar</param>
    /// <returns>True. Si cumple | False. no cumple</returns>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// </history>
    private bool ValidateFilter(Market market)
    {

      if (_nStatus != -1)//Filtro pot estatus
      {
        if (market.mkA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_marketFilter.mkID))//Filtro por ID
      {
        if (market.mkID != _marketFilter.mkID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_marketFilter.mkN))//Filtro por descripción
      {
        if (!market.mkN.Contains(_marketFilter.mkN,StringComparison.OrdinalIgnoreCase))
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
