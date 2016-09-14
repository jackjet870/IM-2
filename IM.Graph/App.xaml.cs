using IM.Base.Forms;
using IM.Graph.Forms;
using IM.Model;
using System.Windows;
using System.Windows.Threading;

namespace IM.Graph
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
    ///   [aalcocer]  10/03/2016 Created
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
    ///   [aalcocer]  10/03/2016 Created
    /// </history>
    protected override async void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      frmSplash frmSplash = new frmSplash("Graph");
      Window frmLS = new frmLS(frmSplash);
      await ((frmLS)frmLS).GetLeadSources();
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLS);
      if (((frmLS)frmLS).IsValidated)
      {
        LeadSource leadsource = ((frmLS)frmLS).LeadSource;
        frmGraph frmMain = new frmGraph(leadsource);
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
    ///   [aalcocer]  10/03/2016 Created
    /// </history>
    private void App_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      var frm = new frmError(e.Exception, null);
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