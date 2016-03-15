using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Administrator.Enums;
using IM.Model.Enums;

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
      LoadCurrencies();
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
      frmSearch.sID = _currencyFilter.cuID;
      frmSearch.sDesc = _currencyFilter.cuN;
      frmSearch.Owner = this;
      if (frmSearch.ShowDialog() == true)
      {
        _nStatus = frmSearch.nStatus;
        _currencyFilter.cuID = frmSearch.sID;
        _currencyFilter.cuN = frmSearch.sDesc;
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
      DataGridRow row = sender as DataGridRow;
      Currency currency = (Currency)row.DataContext;
      frmCurrencyDetail frmCurrencyDetail = new frmCurrencyDetail();
      frmCurrencyDetail.Owner = this;
      frmCurrencyDetail.mode = ((_blnEdit == true) ? ModeOpen.edit : ModeOpen.preview);
      ObjectHelper.CopyProperties(frmCurrencyDetail.currency,currency);
      if(frmCurrencyDetail.ShowDialog()==true)
      {
        ObjectHelper.CopyProperties(currency, frmCurrencyDetail.currency);
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
      frmCurrencyDetail.mode = ModeOpen.add;
      if (frmCurrencyDetail.ShowDialog() == true)
      {
        List<Currency> lstCurrencies = (List<Currency>)dgrCurrencies.ItemsSource;
        lstCurrencies.Add(frmCurrencyDetail.currency);//Agregamos el registro nuevo
        lstCurrencies.Sort((x, y) => string.Compare(x.cuN, y.cuN));//ordenamos la lista
        int nIndex = lstCurrencies.IndexOf(frmCurrencyDetail.currency);//obtenemos el index del registro nuevo
        dgrCurrencies.Items.Refresh();//refrescamos la lista
        dgrCurrencies.SelectedIndex = nIndex;//seleccionamos el registro nuevo
        dgrCurrencies.ScrollIntoView(dgrCurrencies.Items[nIndex]);//movemos el foco hacía el registro nuevo
        StatusBarReg.Content = lstCurrencies.Count + " Currencies.";//Actualizamos el contador
      }
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
    protected void LoadCurrencies()
    {
      List<Currency> lstCurrencies = BRCurrencies.GetCurrencies(_currencyFilter, _nStatus);
      dgrCurrencies.ItemsSource = lstCurrencies;
      if (lstCurrencies.Count > 0)
      {        
        dgrCurrencies.SelectedIndex = 0;
      }

      StatusBarReg.Content = lstCurrencies.Count + " Currencies.";
    }
    #endregion

    #endregion

    
  }
}
