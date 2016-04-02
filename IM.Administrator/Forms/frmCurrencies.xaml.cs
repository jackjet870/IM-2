using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
using IM.Model.Enums;
using System.Linq;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmCurrencies.xaml
  /// </summary>
  public partial class frmCurrencies : Window
  {
    private Currency _currencyFilter = new Currency();//Objeto para filtrar el grid
    private int _nStatus = -1;//Estatus de los registros a mostrar en el grid
    private bool _blnEdit = false;//boleano para saber si tiene permiso para editar|agregar
    public frmCurrencies()
    {
      InitializeComponent();
    }
    #region event controls
    #region Refresh
    /// <summary>
    /// Recarga la lista de currencies
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 08/03/2016
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      Currency currency = (Currency)dgrCurrencies.SelectedItem;
      LoadCurrencies(currency);
    }
    #endregion

    #region Search
    /// <summary>
    /// Abre la ventana de busqueda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 08/03/2016
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      frmSearch frmSearch = new frmSearch();
      frmSearch.nStatus = _nStatus;
      frmSearch.strID = _currencyFilter.cuID;
      frmSearch.strDesc = _currencyFilter.cuN;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _currencyFilter.cuID = frmSearch.strID;
        _currencyFilter.cuN = frmSearch.strDesc;
        LoadCurrencies();
      }
    }
    #endregion

    #region Cell Double Click
    /// <summary>
    /// Muestra la ventada Charge To preview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      Currency currency = (Currency)dgrCurrencies.SelectedItem;
      frmCurrencyDetail frmCurrencyDetail = new frmCurrencyDetail();
      frmCurrencyDetail.Owner = this;
      frmCurrencyDetail.mode = ((_blnEdit == true) ? EnumMode.edit : EnumMode.preview);
      frmCurrencyDetail.oldCurrency = currency;
      if(frmCurrencyDetail.ShowDialog()==true)
      {
        List<Currency> lstCurrencies = (List<Currency>)dgrCurrencies.ItemsSource;
        int nIndex = 0;
        if (!ValidateFilters(frmCurrencyDetail.currency))//Verificar si cumple con los filtros
        {
          lstCurrencies.Remove(currency);//se quita el registro
        }
        else
        {
          ObjectHelper.CopyProperties(currency, frmCurrencyDetail.currency);
          lstCurrencies.Sort((x, y) => string.Compare(x.cuN, y.cuN));//ordenamos la lista  
          nIndex = lstCurrencies.IndexOf(currency);
        }              
        dgrCurrencies.Items.Refresh();//refrescamos la lista
        GridHelper.SelectRow(dgrCurrencies, nIndex);
        StatusBarReg.Content = lstCurrencies.Count + " Currencies.";//Actualizamos el contador
      }

    }
    #endregion

    #region Loaded Form
    /// <summary>
    /// Llena los datos del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [emoguel] created 08/03/2016
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.Currencies, EnumPermisionLevel.Standard);
      LoadCurrencies();
    }
    #endregion

    #region KeyDownForm
    /// <summary>
    /// Valida los botones INS,MAYUSC,NUMLOCK
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    #region KeyBoardFocusChage
    /// <summary>
    /// Verfica que teclas están presionadas
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

    #region Add
    /// <summary>
    /// Abre la ventana de detalles en modo add
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 08/03/2016
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      frmCurrencyDetail frmCurrencyDetail = new frmCurrencyDetail();
      frmCurrencyDetail.Owner = this;
      frmCurrencyDetail.mode = EnumMode.add;
      if (frmCurrencyDetail.ShowDialog() == true)
      {
        if (ValidateFilters(frmCurrencyDetail.currency))//valida que cumpla con los filtros
        {
          List<Currency> lstCurrencies = (List<Currency>)dgrCurrencies.ItemsSource;
          lstCurrencies.Add(frmCurrencyDetail.currency);//Agregamos el registro nuevo
          lstCurrencies.Sort((x, y) => string.Compare(x.cuN, y.cuN));//ordenamos la lista
          int nIndex = lstCurrencies.IndexOf(frmCurrencyDetail.currency);//obtenemos el index del registro nuevo
          dgrCurrencies.Items.Refresh();//refrescamos la lista
          GridHelper.SelectRow(dgrCurrencies, nIndex);
          StatusBarReg.Content = lstCurrencies.Count + " Currencies.";//Actualizamos el contador
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

    #region métodos
    #region Load Currencies
    /// <summary>
    /// Llena el grid de currencies aplicando los filtros que se tengan
    /// </summary>
    /// <history>
    /// [Emoguel] created 08/03/2016
    /// </history>
    protected void LoadCurrencies(Currency currency=null)
    {
      int nIndex = 0;
      List<Currency> lstCurrencies = BRCurrencies.GetCurrencies(_currencyFilter, _nStatus);
      dgrCurrencies.ItemsSource = lstCurrencies;
      if(currency!=null && lstCurrencies.Count>0)
      {
        currency = lstCurrencies.Where(cu => cu.cuID == currency.cuID).FirstOrDefault();
        nIndex = lstCurrencies.IndexOf(currency);
      }
      GridHelper.SelectRow(dgrCurrencies,nIndex);
      StatusBarReg.Content = lstCurrencies.Count + " Currencies.";
    }
    #endregion

    #region Validate Filters
    /// <summary>
    /// Valida si una entidad de tipo Currency coincide con los filtros
    /// </summary>
    /// <param name="newCurrency">Objeto a validar</param>
    /// <returns>true. Si se muestra | false. Nose muestra</returns>
    /// <history>
    /// [emoguel] created 18/03/2016
    /// </history>
    private bool ValidateFilters(Currency newCurrency)
    {
      if (_nStatus != -1)
      {
        if (newCurrency.cuA != Convert.ToBoolean(_nStatus))
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_currencyFilter.cuID))
      {
        if (_currencyFilter.cuID != newCurrency.cuID)
        {
          return false;
        }
      }

      if (!string.IsNullOrWhiteSpace(_currencyFilter.cuN))
      {
        if (!newCurrency.cuN.Contains(_currencyFilter.cuN))
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
