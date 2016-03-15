using System.Windows;
using System.Windows.Data;
using IM.BusinessRules.BR;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmExchangeRateLog.xaml
  /// </summary>
  public partial class frmExchangeRateLog : Window
  {
    private string _currency;
    CollectionViewSource _cvsExchangeRate;
    public frmExchangeRateLog(string currency)
    {
      _currency = currency;

      InitializeComponent();

    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _cvsExchangeRate = ((CollectionViewSource)(this.FindResource("dsExchangeRateLog")));

      // Obtenemos el historico del currency
      _cvsExchangeRate.Source = BRExchangeRate.GetExchangeRateLog(_currency);
    }
  }
}
