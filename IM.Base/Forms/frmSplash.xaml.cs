using System.Windows;
using IM.BusinessRules.BR;
using IM.Base.Classes;

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
    /// [wtorres]   10/Mar/2016 Modified. Agregue el parametro title
    /// [wtorres]   12/Abr/2016 Modified. Ahora despliega el nombre del servidor y de la base de datos
    /// [wtorres]   19/Sep/2016 Modified. Ahora despliega el ambiente y elimine el parametro title
    /// </history>
    public frmSplash()
    {
      InitializeComponent();
      Title = Context.Module;
      lblTitle.Content = $"Intelligence Marketing - {Context.Module}";
      lblEnvironment.Content = Context.Environment;
    }

    #endregion Constructores y destructores

    #region Metodos

    #region Load Window
    /// <summary>
    /// Se agrego para implementar metodos async a la hora de consultar los datos del servidor
    /// </summary>
    /// <history>
    /// [erosado] 06/06/2016  Created,
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      // desplegamos el nombre del servidor y de la base de datos
      var serverInformation = await BRHelpers.GetServerInformation();
      lblServerName.Content = serverInformation[0];
      lblDatabaseName.Content = serverInformation[1];
    }
    #endregion

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