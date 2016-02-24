using IM.Base.Helpers;
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
    private const string encryptCode = "1O3r5i7g9o8s";
    public frmLogin()
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      InitializeComponent();
    }

    /// <summary>
    /// Configura los controles de la forma al cargarse
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      txtUser.Focus();
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
      if (!Validar())
      {
        return;
      }

      var user = new IM.BusinessRules.Login.Login().GetUserLogin(txtUser.Text, txtPassword.Password);

      if(user == null)
      {
        CustomMessage("User ID does not exist.", "Error",MessageBoxImage.Error);
        txtUser.Focus();
        return;
      }
      else if(user.PePwd != EncryptHelper.Encrypt(txtPassword.Password, encryptCode))
      {
        CustomMessage("Invalid password.", "Error", MessageBoxImage.Error);
        txtPassword.Focus();
        return;
      }
      else if(!user.PeA)
      {
        CustomMessage("User ID is inactive.", "Error", MessageBoxImage.Error);
        txtUser.Focus();
        return;
      }

      this.DialogResult = true;
    }

    #region Métodos Privados

    /// <summary>
    /// Valida que los controles del formulario se encuentren llenos
    /// </summary>
    /// <returns>bool</returns>
    /// <history>
    /// [lchairez] 24/Feb/2016 Created
    /// </history>
    private bool Validar()
    {
      bool res = true;

      if(String.IsNullOrEmpty(txtUser.Text))
      {
        CustomMessage("Complete the User ID please", "Wargin", MessageBoxImage.Exclamation);
        res = false;
      }
      else if (String.IsNullOrEmpty(txtPassword.Password))
      {
        CustomMessage("Complete the Password please", "Wargin", MessageBoxImage.Exclamation);
        res = false;
      }
      return res;
    }

    /// <summary>
    /// Envia un mensaje al usuario
    /// </summary>
    /// <param name="message">Mensaje que se mostrará</param>
    /// <param name="titulo">Título de la ventana</param>
    /// <param name="image">Imagen que se mostrará en la ventana</param>
    /// <history>
    /// [lchairez] 24/Feb/2016 Created
    /// </history>
    private void CustomMessage(string message, string titulo, MessageBoxImage image)
    {
      MessageBox.Show(message, titulo, MessageBoxButton.OK, image);
    }

    #endregion

    
  }
}
