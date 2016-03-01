using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using IM.Model.Entities;
using IM.Base.Forms;
using IM.Model.Enums;

namespace IM.InventoryMoves
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
      frmLogin frmLogin = new frmLogin(frmSplash, true, LoginType.Warehouse);
      frmSplash.Show();
      frmSplash.ShowLogin(ref frmLogin);
      if (frmLogin.isAuthenticated)
      {
        UserData userData = frmLogin.userData;
        frmInventoryMoves frmInvMovs = new frmInventoryMoves(userData);
        frmInvMovs.ShowDialog();
        frmSplash.Close();
      }
    }
  }
}
