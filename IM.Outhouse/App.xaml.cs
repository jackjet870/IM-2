using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Outhouse.Forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;


namespace IM.Outhouse
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
    ///   [jorcache]  22/04/2016 Created
    /// </history>
    public App() : base()
    {
      Dispatcher.UnhandledException += App_UnhandledException;
    }
    #endregion

    #region OnStartup
    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    /// <history>
    /// [jorcanche] 11/04/2016  created 
    /// [erosado] 01/06/2016  Modified. se agrego async
    /// </history>
    protected override async void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Outhouse");

      frmLogin frmLogin = new frmLogin(frmSplash, EnumLoginType.Location, EnumProgram.Outhouse, true, changePassword: true,
        autoSign: true, permission: EnumPermission.PRInvitations, permissionLevel: EnumPermisionLevel.ReadOnly);
      await frmLogin.getAllPlaces();

      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        EventManager.RegisterClassHandler(typeof(DataGrid), UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(dataGrid_MouseLeftButtonUp));
        User = frmLogin.UserData;
        frmOuthouse frmMain = new frmOuthouse();
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
    ///   [jorcanche]  22/04/2016 Created
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

    #region dataGrid_MouseLeftButtonUp
    /// <summary>
    /// Cambia el cmapo de busqueda del grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [jorcanche] created 21/06/2016
    /// </history>
    private void dataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      DataGrid dgr = sender as DataGrid;
      if (dgr.CurrentColumn != null)
      {
        dgr.Resources["SearchField"] = dgr.CurrentColumn.SortMemberPath;
      }
    }
    #endregion
  }
}