using IM.Base.Classes;
using IM.Base.Forms;
using IM.Transfer.Forms;
using System.Windows;
using System.Windows.Threading;

namespace IM.Transfer
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    #region Constructores y destructores
    /// <summary>
    /// Constructor de la aplicacion
    /// </summary>
    /// <history>
    ///   [michan]  03/May/2016 Created
    /// </history>
    public App() : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }

    #endregion

    #region Metodos

    #region OnStartup
    protected override void OnStartup(StartupEventArgs e)
    {
      //EventManager.RegisterClassHandler(typeof(Window), Window.KeyDownEvent, new RoutedEventHandler(KeyDown));
      frmTransferLauncher frmMain = new frmTransferLauncher();
      frmMain.ShowInTaskbar = true;
      frmMain.Title = $"{Context.Module} - [{Context.Environment}]";
      frmMain.ShowDialog();
    }
    #endregion

    #region OnExit
    protected override void OnExit(ExitEventArgs e)
    {
      base.OnExit(e);
    } 
    #endregion

    #region App_UnhandledException
    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
    /// <history>
    ///  [michan]  03/May/2016 Created
    /// </history>
    private void App_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      var frm = new frmError(e.Exception);
      frm.ShowDialog();
      if (frm.DialogResult.HasValue && !frm.DialogResult.Value)
      {
        Application.Current.Shutdown();
      }
    }
    #endregion

    #endregion
  }
}
