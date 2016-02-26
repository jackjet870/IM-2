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
    }
}
