using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.ProcessorInhouse.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IM.ProcessorInhouse
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public static UserData userData { get; set; }

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

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      userData = new UserData
      {
        User = new Model.UserLogin
        {
          peID = "AALCOCER"
        }
      };

      //frmSplash frmSplash = new frmSplash("Processor General");
      //frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Normal);
      //frmSplash.Show();
      //frmSplash.ShowLogin(ref frmLogin);
      //if (frmLogin.IsAuthenticated)
      //{
      //  userData = frmLogin.userData;
      //  if (userData.HasRole("MANAGER"))
      //  {
      //    frmProcessorInhouse frmProcessor = new frmProcessorInhouse();
      //    frmProcessor.Show();
      //    frmLogin.Close();
      //    frmSplash.Close();
      //  }
      //  else
      //    UIHelper.ShowMessage("User doesn't have access");
      //}
    }
  }
}