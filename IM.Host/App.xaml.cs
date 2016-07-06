using IM.Base.Forms;
using IM.Host.Forms;
using IM.Model.Classes;
using IM.Model.Enums;
using System.Windows;
using System.Windows.Threading;

namespace IM.Host
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
    ///   [erosado] 01/06/2016  Modified. Se agrego async
    /// </history>
    protected async override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      
      //Creamos el Splash Base!
      frmSplash frmSplash = new frmSplash("Host");

      //Creamos el tipo de login que se necesita!
      var frmLogin = new frmLogin(frmSplash, EnumLoginType.SalesRoom,changePassword: true, autoSign: true, validatePermission:true, permission:EnumPermission.Host, permissionLevel:EnumPermisionLevel.ReadOnly);
      await frmLogin.getAllPlaces();
      //Mostramos el Splash
      frmSplash.Show();

      //Mandamos llamar el Login
      frmSplash.ShowLogin(ref frmLogin);

      if (frmLogin.IsAuthenticated)
      {
        User = frmLogin.UserData;
       // frmSales frmMain = new frmSales(Enums.EnumSale.Sale);
        frmHost frmMain = new frmHost();
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
