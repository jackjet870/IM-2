using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using IM.Model;
using IM.Model.Entities;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using System.IO;
using IM.Base.Helpers;

namespace IM.Base.Forms
{
  public partial class frmLogin : Window
  {
    #region Atributos

    protected Window _frmBase = null;
    protected bool _blnChangePassword;
    protected LoginType _loginType;
    protected bool _blnAutoSign;
    private List<GetWarehousesByUser> _lstWhsByUsr = new List<GetWarehousesByUser>();
    private List<GetLocationsByUser> _lstLocaByUsr = new List<GetLocationsByUser>();
    private Helpers.IniFileHelper _iniFileHelper;

    #endregion
    
    #region Propiedades

    public UserData userData { get; set; }
    public bool isAuthenticated { get; set; }

    #endregion

    #region Constructores y destructores

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

    #endregion

    #region Metodos

    #region Validate

    /// <summary>
    /// Valida que los controles del formulario se encuentren llenos
    /// </summary>
    /// <returns>bool</returns>
    /// <history>
    /// [lchairez] 24/Feb/2016 Created
    /// [jorcanche] 01/03/2016 Modified (Se agrega el "Case" Location)
    /// </history>
    private bool Validate()
    {
      bool res = true;

      if (String.IsNullOrEmpty(txtUser.Text))
      {
        UIHelper.ShowMessage("Specify the User ID.", MessageBoxImage.Warning);
        res = false;
      }
      else if (String.IsNullOrEmpty(txtPassword.Password))
      {
        UIHelper.ShowMessage("Specify the Password.", MessageBoxImage.Warning);
        res = false;
      }
      else if (cmbLocation.Visibility == Visibility.Visible)
      {
        if (cmbLocation.SelectedItem == null)
        {
          switch (_loginType)
          {
            case LoginType.Warehouse:
              UIHelper.ShowMessage("Specify the Warehouse.", MessageBoxImage.Warning);              
              break;
            case LoginType.Location:
              UIHelper.ShowMessage("Specify the Location.", MessageBoxImage.Warning);
              break;
          }

          res = false;
        }
      }

      return res;
    }

    #endregion

    #region LoadFromFile

    private void LoadFromFile()
    {
      string strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (File.Exists(strArchivo))
      {
        _iniFileHelper = new Helpers.IniFileHelper(strArchivo);
        txtUser.Text = _iniFileHelper.readText("Login", "UserName", "");
        txtPassword.Password = _iniFileHelper.readText("Login", "Password", "");
        switch (_loginType)
        {
          case LoginType.Warehouse:
            if (cmbLocation.Visibility == Visibility.Visible)
            {
              txtUser_LostFocus(null, null);
              cmbLocation.SelectedValue = _iniFileHelper.readText("Login", "Warehouse", "");
            }
            btnAceptar.Focus();
            break;
        }
      }
    }

    #endregion

    #endregion

    #region Eventos del formulario

    #region btnAceptar_Click

    /// <summary>
    /// Funcion que recibe la instancia de 
    /// </summary>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [edgrodriguez] 27/02/2016 Modified
    /// [edgrodriguez] 29/02/2016 Modified
    /// [jorcanche] 01/03/2016 Modified (Se agrega el "Case" Location)
    /// </history>
    private void btnAceptar_Click(object sender, RoutedEventArgs e)
    {
      if (!Validate())
        return;

      var user = IM.BusinessRules.BR.BRPersonnel.login(Model.Enums.LoginType.Normal, txtUser.Text);

      userData = BRPersonnel.login(_loginType, txtUser.Text, (cmbLocation.Visibility == Visibility.Visible) ? cmbLocation.SelectedValue.ToString() : "");
      string _encryptPassword = Helpers.EncryptHelper.Encrypt(txtPassword.Password);
      if (userData.User == null)
      {
        UIHelper.ShowMessage("User ID does not exist.", MessageBoxImage.Error);
        txtUser.Focus();
        return;
      }
      else if (userData.User.pePwd != Helpers.EncryptHelper.Encrypt(txtPassword.Password))
      {
        UIHelper.ShowMessage("Invalid password.", MessageBoxImage.Error);
        txtPassword.Focus();
        return;
      }
      else if (!userData.User.peA)
      {
        UIHelper.ShowMessage("User ID is inactive.", MessageBoxImage.Error);
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
        UIHelper.ShowMessage("Invalid password.");
        txtPassword.Focus();
        return;
      }
      switch (_loginType)
      {
        case LoginType.Warehouse://Almacen

          // validamos que el usuario tenga permiso
          if (!userData.Permissions.Exists(c => c.pppm == "GIFTSRCPTS" && c.pppl > 1))
          {
            UIHelper.ShowMessage("User doesn't have access");
            btnCancelar_Click(null, null);
            return;
          }
          break;
        case LoginType.Location: //Hotel 
                                 // validamos que el usuario tenga permiso
          if (!userData.Permissions.Exists(c => c.pppm == "REGISTER" && c.pppl > 1))
          {
            UIHelper.ShowMessage("User doesn't have access");
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

    #endregion

    #region btnCancelar_Click

    private void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
      _frmBase.Close();
      Close();
    }

    #endregion

    #region txtUser_LostFocus

    /// <summary>
    /// Método de carga del formulario.
    /// </summary>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [edgrodriguez] 27/02/2016 Modified
    /// [jorcanche] 01/03/2016 Modified (Se agrega el "Case" Location)
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
        case LoginType.Location://Hotel
          using (var dbContext = new Model.IMEntities())
          {
            _lstLocaByUsr = dbContext.USP_OR_GetLocationsByUser(txtUser.Text, "IH").ToList();
          }
          if (_lstLocaByUsr.Count > 0)
          {
            cmbLocation.ItemsSource = _lstLocaByUsr;
            cmbLocation.SelectedValuePath = "loID";
            cmbLocation.DisplayMemberPath = "loN";
            cmbLocation.IsEnabled = true;         
          }
          else
            cmbLocation.IsEnabled = false;
          break;
      }
    }

    #endregion

    #region Window_Loaded

    /// <summary>
    /// Inicializacion del formulario.
    /// </summary>
    ///  <history>
    /// [edgrodriguez] 29/02/2016 Created
    /// [edgrodriguez] 29/02/2016 Modified
    /// [jorcanche] 01/03/2016 Modified (Se agrega el "Case" Location)
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
        case LoginType.Location: // Hotel
          lblPlace.Content = "Location";
          break;
      }

      //Se verifican las banderas de ChangePassword y AutoSign
      chkAutoSign.Visibility = (_blnAutoSign) ? Visibility.Visible : Visibility.Hidden;
      chkChangePwd.Visibility=(_blnChangePassword) ? Visibility.Visible : Visibility.Hidden;
      LoadFromFile();
    }

    #endregion

    #endregion    
  }
}
