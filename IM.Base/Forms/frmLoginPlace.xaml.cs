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
  public partial class frmLoginPlace : Window
  {
    protected Window _frmBase = null;
    protected bool _blnChangePassword;
    protected LoginType _loginType;
    private const string encryptCode = "1O3r5i7g9o8s";
    public UserData userData { get; set; }
    public bool isAuthenticated { get; set; }
    #region  Variables login tipo Almacen
    private List<GetWarehousesByUser> _lstWhsByUsr = new List<GetWarehousesByUser>();
    private GetSalesRoom _gsrObj = new GetSalesRoom();
    #endregion

    public frmLoginPlace()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Contructor Modificado
    /// </summary>
    /// <param name="pParentSplash"> Instancia del frmSplash </param>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [edgrodriguez] 27/02/2016 Modified
    /// </history>
    public frmLoginPlace(Window pParentSplash, bool blnChangePassword = false, LoginType loginType = LoginType.Normal)
    {
      InitializeComponent();
      _frmBase = pParentSplash;
      _blnChangePassword = blnChangePassword;
      _loginType = loginType;
    }

    /// <summary>
    /// Funcion que recibe la instancia de 
    /// </summary>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [edgrodriguez] 27/02/2016 Modified
    /// </history>
    private void btnAceptar_Click(object sender, RoutedEventArgs e)
    {
      userData = BRPersonnel.login(_loginType, txtUser.Text, cmbLocation.SelectedValue.ToString());

      // validamos las contraseña
      if (!Base.Helpers.EncryptHelper.Encrypt(txtPassword.Password, encryptCode).Equals(userData.User.pePwd))
      {
        MessageBox.Show("Invalid password.", "IM Inventory Movements", MessageBoxButton.OK, MessageBoxImage.Information);
        txtPassword.Focus();
        return;
      }
      switch (_loginType)
      {
        case LoginType.Warehouse://Almacen

          // validamos que el usuario tenga permiso
          if (!userData.Permissions.Exists(c => c.pppm == "GIFTSRCPTS" && c.pppl > 1))
          {              
            MessageBox.Show("User doesn't have access", "IM Inventory Movements", MessageBoxButton.OK, MessageBoxImage.Information);
            btnCancelar_Click(null, null);
            return;
          }
          break;
      }
      isAuthenticated = true;
      Close();
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
          cmbLocation.ItemsSource = _lstWhsByUsr;
          cmbLocation.SelectedValuePath = "whID";
          cmbLocation.DisplayMemberPath = "whN";
          cmbLocation.IsEnabled = true;
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
    /// [edgrodriguez] 29/Feb/2016 Created
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
      }
    }
  }
}
