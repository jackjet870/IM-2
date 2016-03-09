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

namespace IM.Inhouse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      frmSplash frmSplash = new frmSplash();
      frmLogin frmLogin = new frmLogin(frmSplash, true, EnumLoginType.Location);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.IsAuthenticated)
      {
        UserData userData = frmLogin.userData;
        frmRegister frmRegis = new frmRegister(userData);
        frmRegis.ShowDialog();
        frmSplash.Close();
      }      
    }
  }
}
