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
      _cvsExchangeRate.Source = BRExchangeRatesLogs.GetExchangeRateLog(_currency);
    }
    
    /// <summary>
    /// Función para contabilizar los recordset obtenidos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 14/03/2016 Created
    /// </history>
    private void getExchangeRateLogDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if (getExchangeRateLogDataGrid.Items.Count == 0)
      {
        StatusBarReg.Content = "No Records";
        return;
      }
      StatusBarReg.Content = string.Format("{0}/{1}", getExchangeRateLogDataGrid.Items.IndexOf(getExchangeRateLogDataGrid.CurrentItem) + 1, getExchangeRateLogDataGrid.Items.Count);
    }
  }
}
