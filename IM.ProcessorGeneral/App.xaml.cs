using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.ProcessorGeneral.Forms;
using System.Windows;
using System.Windows.Threading;

namespace IM.ProcessorGeneral
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
    public App() : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }
    #endregion

    #region Metodos

    #region OnStartup
    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash();
      frmLogin frmLogin = new frmLogin(frmSplash, EnumLoginType.Normal, validateRole:true, role:EnumRole.Manager, changePassword:true);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        Context.User = frmLogin.UserData;
        if (Context.User.HasRole(EnumRole.Manager))
        {
          frmProcessorGeneral frmMain = new frmProcessorGeneral();
          frmMain.Title = $"{Context.Module} - [{Context.Environment}]";
          frmMain.Show();
          frmLogin.Close();
          frmSplash.Close();
        }
        else
        {
          // No tiene permiso para ingresar al modulo
          UIHelper.ShowMessage("User doesn't have access");
        }
      }
    }
    #endregion

    #region App_UnhandledException
    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
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
