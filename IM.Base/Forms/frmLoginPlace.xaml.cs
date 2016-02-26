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
    public partial class frmLoginPlace : Window
    {
        protected Window gfrmBase = null;


        public frmLoginPlace()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Contructor Modificado
        /// </summary>
        /// <param name="pParentSplash"> Instancia del frmSplash </param>
        /// <history>
        /// [vipacheco] 2-26-2016 Created
        /// </history>
        public frmLoginPlace(Window pParentSplash)
        {
            InitializeComponent();
            gfrmBase = pParentSplash;
        }

        /// <summary>
        /// Funcion que recibe la instancia de 
        /// </summary>

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            gfrmBase.Close();
            Close();
        }
    }
}
