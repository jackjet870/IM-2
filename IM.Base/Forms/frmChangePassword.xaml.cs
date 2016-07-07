using System;
using System.Windows;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmChangePassword.xaml
  /// </summary>
  public partial class frmChangePassword : Window
  {
    #region Atributos

    public UserLogin userLogin { get; set; }
    public DateTime serverDate { get; set; }
    public bool blnOk = false;

    #endregion

    #region Constructores y destructores

    /// <summary>
    /// Constructor de la clase frmChangePassword.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/03/2016 Modified. Se quitaron los parámetros de entrada. Y se pusieron como Propiedades- 
    /// </history>
    public frmChangePassword()
    {
      InitializeComponent();
    }

    #endregion

    #region Eventos del formulario

    #region btnOK_Click

    /// <summary>
    /// Valida los datos del usuario
    /// </summary>
    /// <history>
    /// [edgrodriguez] 29/Feb/2016 Created
    /// [wtorres]      06/Jul/2016 Modified. Ahora se despliega un mensaje de error cuando la contraseña nueva
    ///                            es igual a la anterior
    /// </history>
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      //Si los textbox se encuentran vacios.
      if (txtNewPwd.Password == string.Empty)
      {
        UIHelper.ShowMessage("Specify the New Password");
        return;
      }
      if (txtConfirmNewPwd.Password == string.Empty)
      {
        UIHelper.ShowMessage("Please confirm the new password");
        return;
      }

      //Si el password y la confirmacion son diferentes.
      if (!txtNewPwd.Password.Equals(txtConfirmNewPwd.Password))
      {
        UIHelper.ShowMessage("New password and confirm password are different");
        return;
      }

      string _encryptPass = Helpers.EncryptHelper.Encrypt(txtNewPwd.Password);
      //Si la nueva contraseña es igual a la anterior.
      if (userLogin.pePwd.Equals(_encryptPass))
      {
        UIHelper.ShowMessage("The new password can not be the same as the previous password");
        return;
      }

      //Si ocurrio un error al cambiar el password.
      if (!BRPersonnel.ChangePassword(userLogin.peID, _encryptPass, serverDate))
      {
        UIHelper.ShowMessage("Could not change password");
        return;
      }

      blnOk = true;
      userLogin.pePwd = _encryptPass;
      userLogin.pePwdD = serverDate.Date;
      UIHelper.ShowMessage("Your password was changed succesfully");
      Close();
    }

    #endregion

    #region btnCancel_Click

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      blnOk = false;
      Close();
    }

    #endregion

    #endregion
  }
}
