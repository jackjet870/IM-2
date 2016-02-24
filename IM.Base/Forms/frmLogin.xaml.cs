using System;
using System.Windows;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmLogin.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 09/Feb/2016 Created
  /// </history>
  public partial class frmLogin : Window
  {
    public frmLogin()
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      InitializeComponent();
    }

    /// <summary>
    /// Cierra la ventana del login
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    /// <summary>
    /// Autentifca un usuario
    /// </summary>
    /// <history>
    /// [lchairez] 09/Feb/2016 Created
    /// </history>
    private void btnAceptar_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
