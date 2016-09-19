using IM.Base.Classes;
using IM.Base.Forms;
using IM.MailOutsConfig.Forms;
using IM.Model.Enums;
using System.Windows;
using System.Windows.Threading;

namespace IM.MailOutsConfig
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
    ///   [erosado]  09/Mar/2016 Created
    /// </history>
    public App()
    {
      Dispatcher.UnhandledException += App_UnhandledException;
    }
    #endregion

    #region Metodos

    #region OnStartup
    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    /// <history>
    ///  [erosado]  21/04/2016 Created
    /// </history>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Mail Outs Configuration");
      var frmLogin = new frmLogin(frmSplash,changePassword: true, validatePermission:true, permission:EnumPermission.MailOutsTexts, permissionLevel:EnumPermisionLevel.ReadOnly);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        Context.User = frmLogin.UserData;
        frmMailOutsTexts frmMain = new frmMailOutsTexts();
        frmMain.ShowDialog();
        frmSplash.Close();
      }
    }
    #endregion

    #region App_UnhandledException
    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
    /// <history>
    ///  [erosado]  21/04/2016 Created
    /// </history>
    private void App_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      var frm = new frmError(e.Exception);
      frm.ShowDialog();
      if (frm.DialogResult.HasValue && !frm.DialogResult.Value)
      {
        Current.Shutdown();
      }
    }
    #endregion

    #endregion
  }
}
