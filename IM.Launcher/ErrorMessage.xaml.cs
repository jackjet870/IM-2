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

namespace IM.Launcher
{
  /// <summary>
  /// Interaction logic for ErrorMessage.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 11/Feb/2016 Created
  /// </history>
  public partial class ErrorMessage : Window
  {
    private string _errorMessage;

    #region Constructores y Destructores
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
  }
}
