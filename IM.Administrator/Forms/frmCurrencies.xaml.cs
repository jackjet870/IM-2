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
using IM.Administrator.Helpers;
using IM.Administrator.Enums;

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

    /// <summary>
    /// Abre la ventana de detalles en modo edición
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [Emoguel] created 08/03/2016
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      Currency currency = (Currency)dgrCurrencies.SelectedItem;
      frmCurrencyDetail frmCurrencyDetail = new frmCurrencyDetail();
      frmCurrencyDetail.currency = currency;
      frmCurrencyDetail.mode = ModeOpen.edit;
      frmCurrencyDetail.Owner = this;
      if(frmCurrencyDetail.ShowDialog()==true)
      {
        LoadCurrencies();
      }
    }

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
      if(frmSearch.ShowDialog()==true)
      {
        _nStatus = frmSearch.nStatus;
        _currencyFilter.cuID = frmSearch.sID;
        _currencyFilter.cuN = frmSearch.sDesc;
        LoadCurrencies();
      }
    } 
  
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
      frmCurrencyDetail.mode = ModeOpen.preview;
      frmCurrencyDetail.currency = currency;
      frmCurrencyDetail.ShowDialog();

    }

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
      _blnEdit = PermisionHelper.EditPermision("CURRENCIES");
      LoadCurrencies();
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

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
      frmCurrencyDetail frmCurrency = new frmCurrencyDetail();
      frmCurrency.Owner = this;
      frmCurrency.mode = ModeOpen.add;
      if(frmCurrency.ShowDialog()==true)
      {
        LoadCurrencies();
      }
    }
    #endregion

    #region métodos
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
        btnEdit.IsEnabled = _blnEdit;
        dgrCurrencies.SelectedIndex = 0;
      }
      else
      {
        btnEdit.IsEnabled = false;
      }

      StatusBarReg.Content = lstCurrencies.Count + " Currencies.";
    }
    #endregion
    
  }
}
