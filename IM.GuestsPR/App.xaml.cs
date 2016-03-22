using IM.Base.Forms;
using IM.Model.Classes;
using IM.Model.Enums;
using System.Windows;
using IM.GuestsPR.Forms;

namespace IM.GuestsPR
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {

    #region Propiedades

    public static UserData User;

    #endregion

    #region Constructores y destructores

    /// <summary>
    /// Constructor de la aplicacion
    /// </summary>
    /// <history>
    ///   [erosado]  09/Mar/2016 Created
    /// </history>
    public App()
      : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }

    #endregion

    #region Metodos

    #region OnStartUp
    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Guests PR");
      frmLogin frmLogin = new frmLogin(frmSplash,true,EnumLoginType.Location,true);
      frmSplash.Show();

      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        User = frmLogin.userData;
        frmGuestsPR frmPrGuestsPR = new frmGuestsPR();
        frmPrGuestsPR.ShowDialog();
        frmSplash.Close();
      }
    }

    #endregion

    #region App_UnhandledException

    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
    /// <history>
    ///  [erosado]  09/Mar/2016 Created
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
