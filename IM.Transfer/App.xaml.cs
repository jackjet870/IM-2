using IM.Transfer.Forms;
using System.Windows;
using System.Windows.Threading;
using IM.Base.Forms;

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

    #region OnStartup
    protected override void OnStartup(StartupEventArgs e)
      {
        frmTransferLauncher _frm = new frmTransferLauncher();
        _frm.ShowInTaskbar = false;
        _frm.ShowDialog();
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
  }
}
