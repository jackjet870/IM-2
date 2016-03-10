using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using IM.Base.Forms;
using IM.Model;

namespace IM.Host
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
    public App()
      : base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }

    #endregion

    #region Metodos

    #region OnStartUp

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      /*
      //Creamos el Splash Base!
      frmSplash mfrmSplash = new frmSplash(); 

      //Creamos el tipo de login que se necesita!
      frmLoginPlace fr = new frmLoginPlace(mfrmSplash);

      //Mostramos el Splash
      mfrmSplash.Show();

      //Mandamos llamar el Login
      mfrmSplash.ShowLogin(fr);
      */
      frmHost mfrmHost = new frmHost();
      mfrmHost.ShowDialog();
    }

    #endregion

    #region App_UnhandledException

    /// <summary>
    /// Despliega los mensajes de error de la aplicacion
    /// </summary>
    /// <history>
    ///   [wtorres]  09/Mar/2016 Created
    /// </history>
    private void App_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      var frm = new frmError(e.Exception.Message);
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
