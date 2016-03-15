using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.ProcessorGeneral.Forms;

namespace IM.ProcessorGeneral
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App() : base()
    {
      Dispatcher.UnhandledException += Dispatcher_UnhandledException;
    }

    private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      var frmError = new frmError(e.Exception);
      if (frmError.DialogResult.HasValue && !frmError.DialogResult.Value)
      {
        Application.Current.Shutdown();
      }
    }

    public static UserData userData { get; set; }
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash("Processor General");
      frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Normal);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        userData = frmLogin.userData;
        if (userData.HasRole(EnumRole.Manager))
        {
          frmProcessorGeneral frmProcessor = new frmProcessorGeneral();
          frmProcessor.Show();
          frmLogin.Close();
          frmSplash.Close();
        }
        else
          UIHelper.ShowMessage("User doesn't have access");
      }
    }
  }
}
