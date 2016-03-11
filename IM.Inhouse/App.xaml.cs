using System.Windows;
using IM.Model.Classes;
using IM.Base.Forms;
using IM.Model.Enums;

namespace IM.Inhouse
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
    ///   [wtorres]  09/Mar/2016 Created
    /// </history>
    public App()
      : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }

    #endregion

    #region Metodos

    #region OnStartUp

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Inhouse");
      frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Location);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        UserData userData = frmLogin.userData;
        frmRegister frmRegis = new frmRegister(userData);
        frmRegis.ShowDialog();
        frmSplash.Close();
      }
    }

    #endregion

    #region App_UnhandledException

    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
    /// <history>
    ///   [wtorres]  09/Mar/2016 Created
    /// </history>
    private void App_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
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
