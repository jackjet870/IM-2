using IM.Base.Classes;
using IM.Base.Forms;
using IM.InvitConfig.Forms;
using IM.Model.Enums;
using System.Windows;
using System.Windows.Threading;

namespace IM.InvitConfig
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
    ///   [jorcanche]  09/Mar/2016 Created
    /// </history>
    public App() : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }
    #endregion

    #region OnStartup
    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    /// <history>
    /// [jorcanche] 11/04/2016  created 
    /// [erosado] 01/06/2016  Modified. se agrego async
    /// </history>
    protected async override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash();
      frmLogin frmLogin = new frmLogin(frmSplash, EnumLoginType.Normal,validatePermission:true,
        changePassword: true, permission: EnumPermission.HostInvitations,
        permissionLevel: EnumPermisionLevel.ReadOnly);

      await frmLogin.getAllPlaces();
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        Context.User = frmLogin.UserData;
        frmInvitConfig frmMain = new frmInvitConfig();
        frmMain.Title = $"{Context.Module} - [{Context.Environment}]";
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
    ///   [wtorres]  09/Mar/2016 Created
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
