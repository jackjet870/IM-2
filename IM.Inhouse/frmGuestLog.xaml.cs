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

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmGuestLog.xaml
  /// </summary>
  public partial class frmGuestLog : Window
  {
    public frmGuestLog()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      System.Windows.Data.CollectionViewSource uSP_OR_GetGuestLogViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("uSP_OR_GetGuestLogViewSource")));
      // Load data by setting the CollectionViewSource.Source property:
      // uSP_OR_GetGuestLogViewSource.Source = [generic data source]
    }
  }
}
