using IM.Base.Classes;
using IM.Base.Forms;
using IM.Model.Enums;
using System.Windows;
using System.Windows.Threading;

namespace IM.Assignment
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
    ///   [vku] 10/03/2016 Created
    /// </history>
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
    /// <history>
    ///   [vku]  10/03/2016 Created
    ///   [erosado] 01/06/2016  Modified. Se agrego async
    /// </history>
    protected async override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Assignment");
      frmLogin frmLogin = new frmLogin(frmSplash, EnumLoginType.Location,EnumProgram.Inhouse, validatePermission:true ,changePassword: true, autoSign: true, permission:EnumPermission.Assignment, permissionLevel:EnumPermisionLevel.ReadOnly);
      await frmLogin.getAllPlaces();
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        Context.User = frmLogin.UserData;
        frmAssignment frmMain = new frmAssignment();
        frmMain.ShowDialog();
        frmSplash.Close();
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
