using IM.Base.Forms;
using IM.Graph.Forms;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
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

    #endregion Constructores y destructores

    #region Metodos

    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
    /// <history>
    ///   [aalcocer]  10/03/2016 Created
    /// </history>
    private void App_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      var frm = new frmError(e.Exception.Message);
      frm.ShowDialog();
      if (frm.DialogResult.HasValue && !frm.DialogResult.Value)
      {
        Application.Current.Shutdown();
      }
    }

    /// <summary>
    /// Inicializa el modulo con el Login y el Splash
    /// </summary>
    /// <history>
    ///   [aalcocer]  10/03/2016 Created
    /// </history>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      frmSplash frmSplash = new frmSplash("Graph");
      Window frmLS = new frmLS(frmSplash);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLS);
      if (((frmLS)frmLS).IsValidated)
      {
        LeadSource leadsource = ((frmLS)frmLS).leadSource;
        frmGraph frmGraph = new frmGraph(leadsource);
        frmGraph.ShowDialog();
        frmSplash.Close();
      }
    }

    #endregion Metodos
  }
}