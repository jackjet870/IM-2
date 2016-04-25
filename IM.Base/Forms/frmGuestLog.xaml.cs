using System.Windows;
using System.Windows.Data;
using IM.BusinessRules.BR;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmGuestLog.xaml
  /// </summary>
  public partial class frmGuestLog : Window
  {
    #region Atributos
    private int _idGuest;
    #endregion

    #region Constructores y destructores
    public frmGuestLog(int idGuest)
    {
      InitializeComponent();
      _idGuest = idGuest;
    }
    #endregion

    #region Eventos del formulario
    #region Window_Loaded

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      CollectionViewSource GuestLogViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("GuestLogViewSource")));
      dgGuestLog.DataContext = BRGuestsLogs.GetGuestLog(_idGuest);
    }

    #endregion 
    #endregion
  }
}
