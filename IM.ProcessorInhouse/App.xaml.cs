using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.ProcessorInhouse.Forms;
using System.Windows;
using System.Windows.Threading;

namespace IM.ProcessorInhouse
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    #region Propiedades

    public static UserData User;

    #endregion Propiedades

    #region Constructores y destructores

    /// <summary>
    /// Constructor de la aplicacion
    /// </summary>
    public App() : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }

    #endregion Constructores y destructores

    #region Metodos

    #region OnStartup

    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Processor Inhouse");
      frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Normal);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        User = frmLogin.userData;
        if (User.HasRole(EnumRole.Manager))
        {
          frmProcessorInhouse frmMain = new frmProcessorInhouse();
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

    #endregion OnStartup

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

    #endregion App_UnhandledException

    #endregion Metodos
  }
}