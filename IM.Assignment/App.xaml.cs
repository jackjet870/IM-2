using System.Windows;
using IM.Model.Classes;
using IM.Base.Forms;
using IM.Model.Enums;
using System.Windows.Threading;

namespace IM.Assignment
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
    ///   Constructor de la aplicacion
    /// </summary>
    /// <history>
    ///   [vku] 10/03/2016 Created
    /// </history>
    public App() : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }
    #endregion

    #region Metodos
    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    /// <history>
    ///   [vku]  10/03/2016 Created
    /// </history>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Assignment");
      frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Location, true);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        User = frmLogin.userData;
        frmAssignment frmAssig = new frmAssignment();
        frmAssig.ShowDialog();
        frmSplash.Close();
      }
    }
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
  }
}
