using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using IM.Model.Classes;
using IM.Base.Forms;
using IM.Model.Enums;
using IM.Model;

namespace IM.Administrator
{
  
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public static UserLogin _userLogin = new UserLogin();
    public static List<PermissionLogin> _lstPermision = new List<PermissionLogin>();
    #region constructores y desctructores
    public App():base()
    {
      this.Dispatcher.UnhandledException += App_UnhandledException;
    }
    #endregion

    /// <summary>
    /// Sirve para el manejo de excepciones no controladas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Metodos
    private void App_UnhandledException(object sender,System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      var frmError = new IM.Base.Forms.frmError(e.Exception.Message);
      if(frmError.DialogResult.HasValue && !frmError.DialogResult.Value)
      {
        Application.Current.Shutdown();
      }

    }

    /// <summary>
    /// LLama a la ventana de login 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);


      frmSplash frmSplash = new frmSplash();
      frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Normal);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        UserData userData = frmLogin.userData;
        if (userData.Roles.Exists(r => r.prro == "MANAGER" || r.prro == "ADMIN"))
        {
          Forms.frmAdmin frmAdm = new Forms.frmAdmin();
          _userLogin = userData.User;
          _lstPermision = userData.Permissions;
          frmAdm.ShowDialog();
          frmSplash.Close();
        }
        else
        {
          //NO tiene permiso para ver el Admin
          MessageBox.Show("User doesn't have acces");
        }
      }
    }
    #endregion
  }
}
