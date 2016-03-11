using System.Windows;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmSplash.xaml
  /// </summary>
  public partial class frmSplash : Window
  {
    #region Atributos

    private Window _frmWindow;

    #endregion Atributos

    #region Constructores y destructores

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="title">Formulario principal del modulo</param>
    /// <history>
    /// [wtorres]  10/Mar/2016 Modified. Agregue el parametro title
    /// </history>
    public frmSplash(string title)
    {
      InitializeComponent();

      this.Title = title;
    }

    #endregion Constructores y destructores

    #region Metodos

    #region ShowLogin

    /// <summary>
    /// Función para ejecutar el frmLogin sobre el Splash
    /// </summary>
    /// <param name="pParent"> Instancia del frmLogin segun sea el tipo </param>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [aalcocer]  10/03/2016 Modified. se pasa el contenido a una funcion
    /// </history>
    public void ShowLogin(ref frmLogin frmLogin)
    {
      _frmWindow = frmLogin;
      ShowWindow();
    }

    /// <summary>
    /// Función para ejecutar la ventana sobre el Splash
    /// </summary>
    /// <param name="pChildLogin"> Instancia la ventana </param>
    /// <history>
    /// [aalcocer] 10/03/2016 Created
    /// </history>
    public void ShowLogin(ref Window pChildLogin)
    {
      _frmWindow = pChildLogin;
      ShowWindow();
    }

    #endregion ShowLogin

    #region ShowWindow

    /// <summary>
    /// Ajusta la ventana o el frmLogin al Splash
    /// </summary>
    /// <history>
    /// [aalcocer] 10/03/2016 Created
    /// </history>
    private void ShowWindow()
    {
      _frmWindow.WindowStyle = WindowStyle.None;
      _frmWindow.ShowInTaskbar = false;
      _frmWindow.Owner = this;

      _frmWindow.Left = this.Left + 240;
      _frmWindow.Top = this.Top + 83;
      _frmWindow.ShowDialog();
    }

    #endregion ShowWindow

    #endregion Metodos
  }
}