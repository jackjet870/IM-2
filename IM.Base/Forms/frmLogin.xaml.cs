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
using IM.Model;
using IM.Model.Entities;
using IM.BusinessRules.BR;
using IM.Model.Enums;

namespace IM.Base.Forms
{
  public partial class frmLogin : Window
  {
    protected Window _frmBase = null;
    protected bool _blnChangePassword;
    protected LoginType _loginType;
    protected bool _blnAutoSign;
    public UserData userData { get; set; }
    public bool isAuthenticated { get; set; }
    #region  Variables login tipo Almacen
    private List<GetWarehousesByUser> _lstWhsByUsr = new List<GetWarehousesByUser>();
    private GetSalesRoom _gsrObj = new GetSalesRoom();
    #endregion

    /// <summary>
    /// Contructor Modificado
    /// </summary>
    /// <param name="pParentSplash"> Instancia del frmSplash </param>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [edgrodriguez] 27/02/2016 Modified
    /// [edgrodriguez] 29/02/2016 Modified
    /// </history>
    public frmLogin(Window pParentSplash=null, bool blnChangePassword = false, LoginType loginType = LoginType.Normal, bool blnAutoSign = false)
    {
      InitializeComponent();
      _frmBase = pParentSplash;
      _blnChangePassword = blnChangePassword;
      _loginType = loginType;
      _blnAutoSign = blnAutoSign;
    }

    /// <summary>
    /// Funcion que recibe la instancia de 
    /// </summary>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [edgrodriguez] 27/02/2016 Modified
    /// [edgrodriguez] 29/02/2016 Modified
    /// </history>
    private void btnAceptar_Click(object sender, RoutedEventArgs e)
    {
      if (!Validar())
        return;

      var user = IM.BusinessRules.BR.BRPersonnel.login(Model.Enums.LoginType.Normal, txtUser.Text);

      userData = BRPersonnel.login(_loginType, txtUser.Text, (cmbLocation.Visibility == Visibility.Visible) ? cmbLocation.SelectedValue.ToString() : "");
      string _encryptPassword = Helpers.EncryptHelper.Encrypt(txtPassword.Password);
      if (userData.User == null)
      {
        CustomMessage("User ID does not exist.", "Error", MessageBoxImage.Error);
        txtUser.Focus();
        return;
      }
      else if (userData.User.pePwd != Helpers.EncryptHelper.Encrypt(txtPassword.Password))
      {
        CustomMessage("Invalid password.", "Error", MessageBoxImage.Error);
        txtPassword.Focus();
        return;
      }
      else if (!userData.User.peA)
      {
        CustomMessage("User ID is inactive.", "Error", MessageBoxImage.Error);
        txtUser.Focus();
        return;
      }


      //Validamos la contraseña no haya expirado o el check este activo.
      DateTime _expireDate = userData.User.pePwdD.AddDays(userData.User.pePwdDays);
      DateTime _serverDate = BRHelpers.GetServerDate();
      if (_serverDate > _expireDate || (bool)chkChangePwd.IsChecked)
      {
        frmChangePassword changePwd = new frmChangePassword(userData.User,_serverDate);
        changePwd.ShowDialog();
      }

      // validamos las contraseña
      if (!_encryptPassword.Equals(userData.User.pePwd))
      {
        CustomMessage("Invalid password.", "Intelligence Marketing", MessageBoxImage.Information);
        txtPassword.Focus();
        return;
      }
      switch (_loginType)
      {
        case LoginType.Warehouse://Almacen

          // validamos que el usuario tenga permiso
          if (!userData.Permissions.Exists(c => c.pppm == "GIFTSRCPTS" && c.pppl > 1))
          {
            CustomMessage("User doesn't have access", "IM Inventory Movements", MessageBoxImage.Information);
            btnCancelar_Click(null, null);
            return;
          }
          break;
      }
      isAuthenticated = true;
      Close();

      if (_frmBase != null)
        _frmBase.Hide();
    }

    private void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
      _frmBase.Close();
      Close();
    }

    /// <summary>
    /// Método de carga del formulario.
    /// </summary>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [edgrodriguez] 27/02/2016 Modified
    /// </history>
    private void txtUser_LostFocus(object sender, RoutedEventArgs e)
    {
      switch (_loginType)
      {
        case  LoginType.Warehouse://Almacen
          using (var dbContext = new Model.IMEntities())
          {
            _lstWhsByUsr = dbContext.USP_OR_GetWarehousesByUser(txtUser.Text, "ALL").ToList();
          }
          if (_lstWhsByUsr.Count > 0)
          {
            cmbLocation.ItemsSource = _lstWhsByUsr;
            cmbLocation.SelectedValuePath = "whID";
            cmbLocation.DisplayMemberPath = "whN";
            cmbLocation.IsEnabled = true;
          }
          else
            cmbLocation.IsEnabled = false;
          break;       
      }
    }

    /// <summary>
    /// Inicializa los campos de Usuario y Password.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 26/Feb/2016 Created
    /// </history>
    private void iniciarCampos()
    {
      txtUser.Focus();
      txtUser.SelectAll();
    }


    /// <summary>
    /// Inicializacion del formulario.
    /// </summary>
    ///  <history>
    /// [edgrodriguez] 29/02/2016 Created
    /// [edgrodriguez] 29/02/2016 Modified
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Segun el tipo de login se cambia el texto
      //de la etiqueta lblPlace.
      switch (_loginType)
      {
        case LoginType.Warehouse:
          lblPlace.Content = "Warehouse";
          break;
        case LoginType.Normal:
          cmbLocation.Visibility = Visibility.Hidden;
          lblPlace.Visibility = Visibility.Hidden;
          Height = Height - 25;
          break;
      }

      //Se verifican las banderas de ChangePassword y AutoSign
      chkAutoSign.Visibility = (_blnAutoSign) ? Visibility.Visible : Visibility.Hidden;
      chkChangePwd.Visibility=(_blnChangePassword) ? Visibility.Visible : Visibility.Hidden;
    }

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

      if (String.IsNullOrEmpty(txtUser.Text))
      {
        CustomMessage("Specify the User ID.", "Warning", MessageBoxImage.Exclamation);
        res = false;
      }
      else if (String.IsNullOrEmpty(txtPassword.Password))
      {
        CustomMessage("Specify the Password.", "Warning", MessageBoxImage.Exclamation);
        res = false;
      }
      else if (cmbLocation.Visibility == Visibility.Visible)
      {
        if (cmbLocation.SelectedItem == null)
        {
          switch (_loginType)
          {
            case LoginType.Warehouse:
              CustomMessage("Specify the Warehouse.", "Warning", MessageBoxImage.Exclamation);              
              break;
          }

          res = false;
        }
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


    private void LoadFromFile()
    {

    }
  }
}
