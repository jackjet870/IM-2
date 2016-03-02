using System.Windows;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for ErrorMessage.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 11/Feb/2016 Created
  /// </history>
  public partial class ErrorMessage : Window
  {
    #region Atributos

    private string _errorMessage;

    #endregion

    #region Constructores y destructores
    /// <summary>
    /// Constructor de la aplicación
    /// </summary>
    /// <param name="errorMessage">Mensaje de Error</param>
    /// <history>
    ///  [lchairez] 11/Feb/2016 Created
    /// </history>
    public ErrorMessage(string errorMessage)
    {
      this._errorMessage = errorMessage;
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      InitializeComponent();
    }

    #endregion

    #region Eventos del formulario

    /// <summary>
    /// Configura la forma
    /// </summary>
    /// <history>
    /// [lchairez] 11/feb/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      txtDescription.Text = _errorMessage;
    }

    /// <summary>
    /// Evento del botón YES
    /// </summary>
    /// <history>
    /// [lchairez] 11/feb/2016 Created
    /// </history>
    private void btnYes_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }

    /// <summary>
    /// Evento del botón NO
    /// </summary>
    /// <history>
    /// [lchairez] 11/feb/2016 Created
    /// </history>
    private void btnNo_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

    #endregion
  }
}
