using IM.Transfer.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IM.Transfer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            frmTransferLauncher _frm = new frmTransferLauncher();
            _frm.ShowInTaskbar = false;
            _frm.ShowDialog();


        }

       
    }
}
