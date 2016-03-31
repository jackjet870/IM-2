using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using IM.BusinessRules.BR;
using IM.Model.Classes;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmSalesRoomsLog.xaml
  /// </summary>
  public partial class frmSalesRoomsLog : Window
  {
    //private UserData _userData;

    public frmSalesRoomsLog()
    {
      //_userData = userData;

      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

      CollectionViewSource _salesRoomLog = ((CollectionViewSource)(this.FindResource("dsSalesRoomLog")));
      // Load data by setting the CollectionViewSource.Source property:
      // salesRoomLogViewSource.Source = [generic data source]

      _salesRoomLog.Source = BRSalesRoomsLogs.GetSalesRoomLog(App.User.SalesRoom.srID);

      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [lchairezReload] 09/Feb/2016 Created
    /// </history>
    private void frmSalesRoomsLog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }

    /// <summary>
    /// Revisa si alguna de las teclas Bloq Mayús, Bloq Núm o Insert está activo
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran valores si está activa la tecla</param>
    /// <param name="key">tecla que revisaremos si se encuentra activa</param>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }

    /// <summary>
    /// Asigna valores por defecto a los StatusBarItem cuando se carga el form
    /// </summary>
    /// <param name="statusBar">StatusBarItem al que se le asignaran los valores</param>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }

    private void dtgSalesRoomLog_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      StatusBarReg.Content = string.Format("{0}/{1}", dtgSalesRoomLog.Items.IndexOf(dtgSalesRoomLog.CurrentItem) + 1, dtgSalesRoomLog.Items.Count);
    }
  }
}
