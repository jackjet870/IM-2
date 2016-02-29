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
using IM.Model.Entities;
using IM.Model;
using IM.BusinessRules.BR;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmChangePassword.xaml
  /// </summary>
  public partial class frmChangePassword : Window
  {
    userLogin _userLogin;
    DateTime _serverDate;
    public frmChangePassword(userLogin UserLogin,DateTime serverDate)
    {
      InitializeComponent();
      _userLogin = UserLogin;
      _serverDate = serverDate;
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      //Si los textbox se encuentran vacios.
      if (txtNewPwd.Password == string.Empty) {
        MessageBox.Show("Specify the NewPassword.", "Intelligence Marketing", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      if (txtConfirmNewPwd.Password == string.Empty)
      {
        MessageBox.Show("Please confirm the new password.", "Intelligence Marketing", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      //Si el password y la confirmacion son diferentes.
      if (!txtNewPwd.Password.Equals(txtConfirmNewPwd.Password))
      {
        MessageBox.Show("New password and confirm password are different", "Intelligence Marketing", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      string _encryptPass = Helpers.EncryptHelper.Encrypt(txtNewPwd.Password);
      //Si la nueva contraseña es igual a la anterior.
      if (_userLogin.pePwd.Equals(_encryptPass))
      {
        Close();
        return;
      }

      //Si ocurrio un error al cambiar el password.
      if (!BRPersonnel.ChangePassword(_userLogin.peID, _encryptPass, _serverDate))
      {
        MessageBox.Show("Could not change password", "Intelligence Marketing", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      else
        MessageBox.Show("Your password was changed succesfully.", "Intelligence Marketing", MessageBoxButton.OK, MessageBoxImage.Information);

      Close();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
