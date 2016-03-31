using IM.Host.Enums;
using System.Windows;
using System.Windows.Data;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmShow.xaml
  /// </summary>
  /// <history>
  /// [vipacheco] 03/16/2016 Created
  /// </history>
  public partial class frmShow : Window
  {
    public static EnumModeOpen modeOpen;

    public frmShow()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      CollectionViewSource guestViewSource = ((CollectionViewSource)(this.FindResource("guestViewSource")));

    }
  }
}
