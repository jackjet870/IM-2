using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Model.Classes;
using IM.Model.Enums;
using System.Windows;
using System.Windows.Threading;

namespace IM.Inhouse
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
    ///   [wtorres]  09/Mar/2016 Created
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
    /// [jorcanche] 11/04/2016  created 
    ///   [erosado] 01/06/2016  Modified. Se agrego async
    /// </history>
    protected async override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Inhouse");

      //frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Location,true, EnumProgram.Inhouse);
      frmLogin frmLogin = new frmLogin(frmSplash, EnumLoginType.Location, EnumProgram.Inhouse, true,
        changePassword: true, autoSign: true, permission: EnumPermission.Register,
        permissionLevel: EnumPermisionLevel.ReadOnly);

      await frmLogin.getAllPlaces();

      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        User = frmLogin.UserData;        
        Forms.frmInhouse frmMain = new Forms.frmInhouse();
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

    #endregion
  }
}
