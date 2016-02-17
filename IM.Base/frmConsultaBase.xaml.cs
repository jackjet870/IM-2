using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using IM.Base.Entities;
using IM.Base.Enums;

namespace IM.ConsultaBase
{
  /// <summary>
  /// Interaction logic for frmConsultaBase.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 09/Feb/2016 Created
  /// </history>
  public partial class frmConsultaBase : Window
  {
    private string _titleForm = String.Empty;
    private string _user = String.Empty;
    private string _location = String.Empty;

    public frmConsultaBase()
    {
      this._titleForm = "Consulta Base";
      this._user = "USUARIO DE PRUEBA";
      this._location = "LUGAR DE PRUEBA";
      InitializeComponent();
    }

    public frmConsultaBase(Parametros parametros)
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
    }

    #region Métodos de la forma

    /// <summary>
    /// Realiza las configuraciones de los controles de la forma
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void frmConsultaBase_Loaded(object sender, RoutedEventArgs e)
    {
      ConfigControls();
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [lchairezReload] 09/Feb/2016 Created
    /// </history>
    private void frmConsultaBase_KeyDown(object sender, KeyEventArgs e)
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
      ConfigForm();
      ConfigGrbInfoUser();
      ConfigDataGrid();
      KeyDefault(StatusBarCap);
      KeyDefault(StatusBarIns);
      KeyDefault(StatusBarNum);
      ConfigControlDate();
    }

    /// <summary>
    /// Confugra los valores predeterminados de la forma al ser cargado
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void ConfigForm()
    {

      this.Title = _titleForm;

      //Asignamos tamaño de la pantalla a la forma
      this.Width = SystemParameters.PrimaryScreenWidth;
      this.Height = SystemParameters.PrimaryScreenHeight - 40;

      //Centramos la forma
      Rect workArea = SystemParameters.WorkArea;
      this.Left = (workArea.Width - this.Width) / 2 + workArea.Left;
      this.Top = (workArea.Height - this.Height) / 2 + workArea.Top;
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

      dtgDatos.Margin = new Thickness(5, heigthgrdMenu + 5, 5, heightStatusBar + 5);//Asigamos la posición del grid, así como su alto y ancho
    }

    /// <summary>
    /// Configura el control de fechas para cargar la fecha actual
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void ConfigControlDate()
    {
      dtpDate.SelectedDate = DateTime.Now;
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

    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      //ShowPrintReport();
    }
  }
}
