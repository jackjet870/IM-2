using IM.Base.Forms;
using IM.Model.Classes;
using IM.Model.Enums;
using System.Windows;
using System.Windows.Threading;
using IM.MailOuts.Forms;

namespace IM.MailOuts
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
    ///   [aalcocer]  04/03/2016 Created
    /// </history>
    public App():base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }
    #endregion

    #region Metodos

    #region App_UnhandledException

    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
    /// <history>
    ///   [aalcocer]  04/03/2016 Created
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

    #region OnStartup

    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      frmSplash frmSplash = new frmSplash("Mail Outs");
      frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Location, true);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        UserData userData = frmLogin.userData;
        frmMailOuts frmInvMovs = new frmMailOuts(userData);
        frmInvMovs.ShowDialog();
        frmSplash.Close();
      }
    }

    #endregion

    #endregion
  }
}