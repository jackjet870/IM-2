using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IM.Base.Forms
{
    /// <summary>
    /// Interaction logic for frmSplash.xaml
    /// </summary>
    public partial class frmSplash : Window
    {
        public frmSplash()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Función para ejecutar el frmLogin sobre el Splash
        /// </summary>
        /// <param name="pParent"> Instancia del frmLogin segun sea el tipo </param>
        /// <history>
        /// [vipacheco] 2-26-2016 Created
        /// </history>
        public void ShowLogin(Window pChildLogin)
        {
            pChildLogin.WindowStyle = WindowStyle.None;
            pChildLogin.ShowInTaskbar = false;
            pChildLogin.Owner = this;

            pChildLogin.Left = this.Left + 240;
            pChildLogin.Top = this.Top + 83;
            pChildLogin.ShowDialog();
        }
    }
}
