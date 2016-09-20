using IM.Base.Classes;
using IM.Base.Forms;
using IM.Model.Enums;
using System.Windows;
using System.Windows.Threading;

namespace IM.InventoryMovements
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
    /// [erosado] 01/06/2016  Modified. se agrego async
    /// </history>
    protected override async void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash();
      frmLogin frmLogin = new frmLogin(frmSplash, EnumLoginType.Warehouse, validatePermission:true, permission:EnumPermission.GiftsReceipts, permissionLevel:EnumPermisionLevel.Standard, changePassword:true);
      await frmLogin.getAllPlaces();
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (!frmLogin.IsAuthenticated) return;
      Context.User = frmLogin.UserData;
      frmInventoryMovements frmMain = new frmInventoryMovements();
      frmMain.Title = $"{Context.Module} - [{Context.Environment}]";
      frmMain.ShowDialog();
      frmSplash.Close();
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
