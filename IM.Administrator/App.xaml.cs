using System.Windows;
using IM.Model.Classes;
using IM.Base.Forms;
using IM.Model.Enums;
using IM.Administrator.Forms;

namespace IM.Administrator
{

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    #region Propiedades

    public static UserData User;

    #endregion

    #region Constructores y desctructores
    public App():base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }
    #endregion

    #region Metodos

    #region App_UnhandledException

    /// <summary>
    /// Sirve para el manejo de excepciones no controladas
    /// </summary>
    private void App_UnhandledException(object sender,System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      var frmError = new IM.Base.Forms.frmError(e.Exception);
      if(frmError.DialogResult.HasValue && !frmError.DialogResult.Value)
      {
        Application.Current.Shutdown();
      }
    }

    #endregion

    #region OnStartup

    /// <summary>
    /// LLama a la ventana de login 
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      frmSplash frmSplash = new frmSplash("Administrator");
      frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Normal);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        User = frmLogin.userData;
        if (User.HasRole(EnumRole.Manager) || User.HasRole(EnumRole.Administrator))
        {
          frmAdmin frmAdm = new frmAdmin();
          frmAdm.ShowDialog();
          frmSplash.Close();
        }
        else
        {
          //NO tiene permiso para ver el Admin
          MessageBox.Show("User doesn't have acces");
        }
      }
    }

    #endregion

    #endregion
  }
}
