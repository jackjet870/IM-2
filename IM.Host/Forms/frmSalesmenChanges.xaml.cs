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
  /// Interaction logic for frmSalesmenChanges.xaml
  /// </summary>
  public partial class frmSalesmenChanges : Window
  {
    private int _membership;
    int _sale = 0;

    public frmSalesmenChanges(int sale, int membership)
    {
      InitializeComponent();
      _sale = sale;
      _membership = membership;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Title = $"IM Salesmen Changes - Sale ID {_sale} / Membership Number {_membership}";
      var salesSalesMan = BRSalesSalesmen.GetSalesmenChanges(_sale);         
      System.Windows.Data.CollectionViewSource salesmenChangesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("salesmenChangesViewSource")));
      salesmenChangesViewSource.Source = salesSalesMan;      
    }
  }
}
