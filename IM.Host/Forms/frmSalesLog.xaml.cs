using IM.BusinessRules.BR;
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

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmSalesLog.xaml
  /// </summary>
  public partial class frmSalesLog : Window
  {   
    int _sale = 0;
    int _membership = 0;
    public frmSalesLog(int sale, int membership)
    {
      InitializeComponent();
      _sale = sale;
      _membership = membership;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Title = $"IM Sales Log - Sale ID {_sale} / Membership Number {_membership}"; 
      var saleLog = BRSales.GetSaleLog(_sale);
      var saleLogDataViewSource = ((CollectionViewSource)(FindResource("saleLogDataViewSource")));
      saleLogDataViewSource.Source = saleLog;
    }

    private void btnSalesMan_Click(object sender, RoutedEventArgs e)
    {
      var salesmenchanges = new frmSalesmenChanges(_sale, _membership);
      salesmenchanges.Owner = this;
      salesmenchanges.ShowDialog();
    }
  }
}
