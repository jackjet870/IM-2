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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IM.Base.Entities;
using IM.Base.Enums;
using IM.Base.Interfaces;
using IM.Base.Forms;
using IM.Model;
using IM.Host.Forms;

namespace IM.Host
{
    /// <summary>
    /// Interaction logic for frmHost.xaml
    /// </summary>
    public partial class frmHost : Window, IMetodosPublicos
    {
        private string _titleForm = String.Empty;
        private string _user = String.Empty;
        private string _location = String.Empty;

        public frmHost()
        {
            //this._titleForm = "Consulta Base";
            this._user = "USUARIO DE PRUEBA";
            this._location = "LUGAR DE PRUEBA";
            InitializeComponent();
        }

        /*public frmHost(Parametros parametros)
        {
            this._user = parametros.UserName;
            this._location = parametros.Location;

            switch (parametros.Modulo)
            {
                case Modulo.InHouse:
                    this._titleForm = "In House";
                    break;
                case Modulo.OutHouse:
                    this._titleForm = "Out House";
                    break;
                case Modulo.Host:
                    this._titleForm = "Host";
                    break;
                case Modulo.Animation:
                    this._titleForm = "Animation";
                    break;
                case Modulo.Regen:
                    this._titleForm = "Regen";
                    break;
            }

            InitializeComponent();
        }*/

        #region Métodos de la forma

        /// <summary>
        /// Realiza las configuraciones de los controles de la forma
        /// </summary>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void frmHost_Loaded(object sender, RoutedEventArgs e)
        {
            //ConfigControls();
            CkeckKeysPress(StatusBarCap, Key.Capital);
            CkeckKeysPress(StatusBarIns, Key.Insert);
            CkeckKeysPress(StatusBarNum, Key.NumLock);

            CollectionViewSource guestPremanifestHostViewSource = ((CollectionViewSource)(this.FindResource("guestPremanifestHostViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // guestPremanifestHostViewSource.Source = [generic data source]

            IMEntities ef = new IMEntities();

            guestPremanifestHostViewSource.Source = ef.GetPremanifestHost(Convert.ToDateTime("02-25-2016"), "MPS");
        }

        /// <summary>
        /// Activa los StatusBarItem CAP, NUM, INS
        /// </summary>
        /// <history>
        /// [lchairezReload] 09/Feb/2016 Created
        /// </history>
        private void frmHost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Capital)
            {
                CkeckKeysPress(StatusBarCap, Key.Capital);
            }
            else if (e.Key == Key.Insert)
            {
                CkeckKeysPress(StatusBarIns, Key.Insert);
            }
            else if (e.Key == Key.NumLock)
            {
                CkeckKeysPress(StatusBarNum, Key.NumLock);
            }
        }

        /// <summary>
        /// Llama la ventana de Login
        /// </summary>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = new frmLogin();
            login.Owner = this;
            login.ShowDialog();
        }

        /// <summary>
        /// Llama la ventana de About
        /// </summary>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new frmAbout();
            about.Owner = this;
            about.ShowDialog();
        }

        /// <summary>
        /// Imprime el reporte
        /// </summary>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintReport();
        }

        /// <summary>
        /// Muestra una vista previa del reporte
        /// </summary>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            ShowReport();
        }

        /// <summary>
        /// Muestra el diseño del reporte.
        /// </summary>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            ShowReportDesigner();
        }

        #endregion

        #region Métodos privados

        /// <summary>
        /// Manda llamar a todos los métodos de configuración de los controles al ser cargada la forma
        /// </summary>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void ConfigControls()
        {
            //ConfigForm();
            ConfigGrbInfoUser();
            ConfigDataGrid();
            KeyDefault(StatusBarCap);
            KeyDefault(StatusBarIns);
            KeyDefault(StatusBarNum);
        }

        /// <summary>
        /// Configura el GroupBox Información del Usuario al ser cargada la forma
        /// </summary>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void ConfigGrbInfoUser()
        {
            txtUser.Text = _user;
            txtLocation.Text = _location;
        }

        /// <summary>
        /// Configura el Datagrid al cargar la forma
        /// </summary>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void ConfigDataGrid()
        {
            var heigthgrdMenu = grdPanel.ActualHeight;
            var heightStatusBar = stbStatusBar.ActualHeight;

            //dtgDatos.Margin = new Thickness(5, heigthgrdMenu + 5, 5, heightStatusBar + 5);//Asigamos la posición del grid, así como su alto y ancho
        }

        /// <summary>
        /// Asigna valores por defecto a los StatusBarItem cuando se carga el form
        /// </summary>
        /// <param name="statusBar">StatusBarItem al que se le asignaran los valores</param>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
        {
            statusBar.FontWeight = FontWeights.Normal;
            statusBar.Foreground = Brushes.Gray;
        }

        /// <summary>
        /// Revisa si alguna de las teclas Bloq Mayús, Bloq Núm o Insert está activo
        /// </summary>
        /// <param name="statusBar">StatusBarItem al que se le asignaran valores si está activa la tecla</param>
        /// <param name="key">tecla que revisaremos si se encuentra activa</param>
        /// <history>
        /// [lchairez] 09/Feb/2016 Created
        /// </history>
        private void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
        {
            var keyPess = Keyboard.GetKeyStates(key).ToString();

            if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
            {
                statusBar.FontWeight = FontWeights.Bold;
                statusBar.Foreground = Brushes.Black;
            }
            else
            {
                KeyDefault(statusBar);
            }
        }

        #endregion

        #region Métodos públicos

        public void PrintReport()
        {
            throw new NotImplementedException();
        }

        public void ShowReport()
        {
            throw new NotImplementedException();
        }

        public void ShowReportDesigner()
        {
            throw new NotImplementedException();
        }

        #endregion

        private void StatusBarCap_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnCloseSalesRoom_Click(object sender, RoutedEventArgs e)
        {
            frmCloseSalesRoom mfrCloseSalesRoom = new frmCloseSalesRoom(this);
            mfrCloseSalesRoom.ShowInTaskbar = false;
            mfrCloseSalesRoom.Owner = this;
            AplicarEfecto(this);
            mfrCloseSalesRoom.ShowDialog();
        }

        private void AplicarEfecto(Window win)
        {
            var objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 5;
            win.Effect = objBlur;
        }
    }

}
