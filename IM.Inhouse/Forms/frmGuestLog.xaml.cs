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
using IM.BusinessRules.BR;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmGuestLog.xaml
  /// </summary>
  public partial class frmGuestLog : Window
  {
     private int _idGuest;
    public frmGuestLog(int idGuest)
    {
      InitializeComponent();
      _idGuest = idGuest;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      CollectionViewSource GuestLogViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("GuestLogViewSource")));
      dgGuestLog.DataContext = BRGuests.GetGuestLog(_idGuest);
    }
  }
}
