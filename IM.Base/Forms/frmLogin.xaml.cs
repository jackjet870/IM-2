using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IM.Model;
using IM.Model.Classes;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using System.IO;
using IM.Base.Helpers;
using System.Threading.Tasks;

namespace IM.Base.Forms
{
  public partial class frmLogin : Window
  {
    #region Atributos

    protected Window _frmBase = null;
    protected bool _blnChangePassword;
    protected EnumLoginType _loginType;
    protected bool _blnAutoSign;
    private IniFileHelper _iniFileHelper;

    #endregion

    #region Propiedades

    public UserData userData { get; set; }
    public bool IsAuthenticated { get; set; }

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
    public frmLogin(Window pParentSplash = null, bool blnChangePassword = false, EnumLoginType loginType = EnumLoginType.Normal, bool blnAutoSign = false)
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
    /// [vipacheco] 07/03/2016 Modified --> se agrego case para Sales Room
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
      else if (cmbPlace.Visibility == Visibility.Visible)
      {
        if (cmbPlace.SelectedItem == null)
        {
          switch (_loginType)
          {
            case EnumLoginType.Warehouse:
              UIHelper.ShowMessage("Specify the Warehouse.", MessageBoxImage.Warning);
              break;
            case EnumLoginType.Location:
              UIHelper.ShowMessage("Specify the Location.", MessageBoxImage.Warning);
              break;
            case EnumLoginType.SalesRoom:
              UIHelper.ShowMessage("Specify the Sales Room.", MessageBoxImage.Warning);
              break;
          }

          res = false;
        }
      }

      return res;
    }

    #endregion

    #region LoadFromFile

    /// <summary>
    /// Funcion encargado de cargar los datos desde el archivo de configuracion
    /// </summary>
    /// <history>
    /// [vipacheco] 08/03/2016 Modified --> se agrego case para sales room
    /// [erosado]   14/04/2016  Modified. Se elimino la seleccion del archivo de configuracion de este metodo.
    /// </history>
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
          case EnumLoginType.Warehouse:
            if (cmbPlace.Visibility == Visibility.Visible)
            {
              txtUser_LostFocus(null, null);
            }
            btnAceptar.Focus();
            break;
          case EnumLoginType.Location:
            if (cmbPlace.Visibility == Visibility.Visible)
            {
              txtUser_LostFocus(null, null);
            }
            btnAceptar.Focus();
            break;
          case EnumLoginType.SalesRoom:
            if (cmbPlace.Visibility == Visibility.Visible)
            {
              txtUser_LostFocus(null, null);
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
    /// [vipacheco] 01/03/2016 Modified --> Se agrego validacion case para Sales Room
    /// [erosado] 19/Mar/2016 Validamos que el _frmBase no sea null
    /// [jorcanche] 11/04/2016 Si chkAutoSign es verdadero desencriptamos el password y se guarda 
    ///                         Y AutoSign se deja como verdadero
    /// </history>   
  private void btnAceptar_Click(object sender, RoutedEventArgs e)
    {
      if (!Validate())
        return;

      userData = BRPersonnel.Login(_loginType, txtUser.Text, (cmbPlace.Visibility == Visibility.Visible) ? cmbPlace.SelectedValue.ToString() : "");
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
        frmChangePassword changePwd = new frmChangePassword();
        changePwd.userLogin = userData.User;
        changePwd.serverDate = _serverDate;
        changePwd.ShowDialog();
        if (changePwd.blnOk)
        {
          userData.User = changePwd.userLogin;
          _encryptPassword = changePwd.userLogin.pePwd;
        }
        else
        {
          if (_serverDate > _expireDate)
          {
            UIHelper.ShowMessage("Password expired.");
            return;
          }
        }
      }

      // validamos las contraseña
      if (!_encryptPassword.Equals(userData.User.pePwd) )
      {
        UIHelper.ShowMessage("Invalid password.");
        txtPassword.Focus();
        return;
      }
      switch (_loginType)
      {
        case EnumLoginType.Warehouse://Almacen

          // validamos que el usuario tenga permiso
          if (!userData.Permissions.Exists(c => c.pppm == "GIFTSRCPTS" && c.pppl > 1))
          {
            UIHelper.ShowMessage("User doesn't have access");
            btnCancelar_Click(null, null);
            return;
          }
          break;
        case EnumLoginType.Location: //Hotel 
                                     // validamos que el usuario tenga permiso
          if (!userData.Permissions.Exists(c => c.pppm == "REGISTER" && c.pppl > 1))
          {
            UIHelper.ShowMessage("User doesn't have access");
            btnCancelar_Click(null, null);
            return;
          }
          break;
        case EnumLoginType.SalesRoom://Sales Room - HOST

          // validamos que el usuario tenga permiso
          if (!userData.Permissions.Exists(c => c.pppm == "HOST" && c.pppl >= 1))
          {
            UIHelper.ShowMessage("User doesn't have access");
            btnCancelar_Click(null, null);
            return;
          }
          break;
      }
      if ((bool)chkAutoSign.IsChecked)
      {
        this.userData.AutoSign = true;
        this.userData.User.pePwd = EncryptHelper.Encrypt(this.userData.User.pePwd);
      }

      IsAuthenticated = true;
      Close();
      _frmBase?.Hide();
    }

    #endregion

    #region btnCancelar_Click

    /// <summary>
    /// Cierra el formulario padre y despues cierra el frmLogin
    /// </summary>
    ///<history>
    ///[edgar]  29/Feb/2016 Created
    ///[erosado]  19/Mar/2016 Se valido que el _frmBase No sea null
    /// </history>
    private void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
      _frmBase?.Close();
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
    /// [vipacheco] 01/03/2016 Modified --> Se agrego case Sales Room
    /// [erosado] 19/Mar/2016 Se agregaron metodos Asincronos para que no se trabe la interfaz.
    /// </history>
    private void txtUser_LostFocus(object sender, RoutedEventArgs e)
    {
      switch (_loginType)
      {
        case EnumLoginType.Warehouse://Almacen
          DoGetWareHousesByUser(txtUser.Text.Trim());
          break;
        case EnumLoginType.Location://Locacion
          DoGetLocationsByUser(txtUser.Text.Trim());
          break;
        case EnumLoginType.SalesRoom://Sales Room
          DoGetSalesRoomsByUser(txtUser.Text.Trim());
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
    /// [vipacheco] 01/03/2016 Modified --> Se agrego case para Sales Room
    /// [jorcanche] 11/04/2016 Modified se agrega la condicion AutoSign
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Segun el tipo de login se cambia el texto
      //de la etiqueta lblPlace.
      switch (_loginType)
      {
        case EnumLoginType.Warehouse:
          lblPlace.Content = "Warehouse";
          break;
        case EnumLoginType.Normal:
          cmbPlace.Visibility = Visibility.Hidden;
          lblPlace.Visibility = Visibility.Hidden;
          Height = Height - 25;
          break;
        case EnumLoginType.Location: // Hotel
          lblPlace.Content = "Location";
          break;
        case EnumLoginType.SalesRoom:
          lblPlace.Content = "Sales Room";
          break;
      }

      //Se verifican las banderas de ChangePassword y AutoSign
      chkAutoSign.Visibility = (_blnAutoSign) ? Visibility.Visible : Visibility.Hidden;
      chkChangePwd.Visibility = (_blnChangePassword) ? Visibility.Visible : Visibility.Hidden;
      LoadFromFile();
      if (userData != null)
      {
        if (userData.AutoSign)
        {
          txtUser.Text = this.userData.User.peID;
          txtPassword.Password = this.userData.User.pePwd;

          if (userData.Warehouse != null)
          {
            DoGetWareHousesByUser(this.userData.User.peID, true);
          }
          if (userData.Location != null)
          {
            DoGetLocationsByUser(this.userData.User.peID, true);
          }
          if (userData.SalesRoom != null)
          {
            DoGetSalesRoomsByUser(this.userData.User.peID, true);
          }
          if (userData.AutoSign)
          {
            chkAutoSign.IsChecked = true;
          }
        }
      }
    }

    #endregion
        
    #region Login_KeyDown
    /// <summary>
    /// Función que evalua cuando se pulsa Ctrl + F4 para cerrar la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/09/2016 Created
    /// </history>
    private void Login_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key == Key.System && e.SystemKey == Key.F4)
      {
        _frmBase.Close();
      }
    }
    #endregion

    #endregion

    #region Async Methods
    /// <summary>
    /// Obtiene la lista de WareHouses dependiendo el IdUsuraio y TODAS LAS REGIONES
    /// </summary>
    /// <history>
    /// [erosado] 19/Mar/2016 Created
    /// [erosado] 14/04/2016  Modified Se agrego la seleccion de datos desde el archivo Configuration.ini
    /// </history>
    public void DoGetWareHousesByUser(string IdUsuario, bool AutoAsignLogin = false)
    {
      Task.Factory.StartNew(() => BRWarehouses.GetWarehousesByUser(IdUsuario, "ALL"))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            List<WarehouseByUser> data = task1.Result;
            if (data.Count > 0)
            {
              cmbPlace.ItemsSource = data;
              cmbPlace.SelectedValuePath = "whID";
              cmbPlace.DisplayMemberPath = "whN";
              cmbPlace.IsEnabled = true;

              if (AutoAsignLogin)
              {
                List<WarehouseByUser> lstPS = cmbPlace.ItemsSource as List<WarehouseByUser>;
                int index = lstPS.FindIndex(x => x.whN.Equals(this.userData.Warehouse.whN));
                cmbPlace.SelectedIndex = index;
              }
              else if (_iniFileHelper != null)
              {
                cmbPlace.SelectedValue = _iniFileHelper.readText("Login", "Warehouse", "");
              }
              else
              {
                cmbPlace.SelectedIndex = 0;
              }
            }
            else
            {
              cmbPlace.IsEnabled = false;
              cmbPlace.Text = "No data found";
            }
          }
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    /// <summary>
    /// Obtiene la lista de Location dependiendo el IdUsuario y PROGRAMAS IH
    /// </summary>
    /// <history>
    /// [erosado] 19/Mar/2016 Created
    /// [erosado] 14/04/2016  Modified Se agrego la seleccion de datos desde el archivo Configuration.ini
    /// </history>
    public void DoGetLocationsByUser(string IdUsuario, bool AutoAsignLogin = false)
    {
      Task.Factory.StartNew(() => BRLocations.GetLocationsByUser(IdUsuario, "IH"))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            List<LocationByUser> data = task1.Result;
            if (data.Count > 0)
            {
                cmbPlace.ItemsSource = data;
                cmbPlace.SelectedValuePath = "loID";
                cmbPlace.DisplayMemberPath = "loN";
                cmbPlace.IsEnabled = true;

              if (AutoAsignLogin)
              {
                List<LocationByUser> lstPS = cmbPlace.ItemsSource as List<LocationByUser>;
                int index = lstPS.FindIndex(x => x.loN.Equals(this.userData.Location.loN));
                cmbPlace.SelectedIndex = index;
              }
              else if (_iniFileHelper != null)
              {
                cmbPlace.SelectedValue = _iniFileHelper.readText("Login", "Location", "");
              }
              else
              {
                cmbPlace.SelectedIndex = 0;
              }

            }
            else
            {
              cmbPlace.IsEnabled = false;
              cmbPlace.Text = "No data found";
            }

          }
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    /// <summary>
    /// Obtiene la lista de SalesRooms dependiendo el IDUsuario y TODAS LAS REGIONES
    /// </summary>
    /// <history>
    /// [erosado] 19/Mar/2016 Created
    /// [erosado] 14/04/2016  Modified Se agrego la seleccion de datos desde el archivo Configuration.ini
    /// </history>
    public void DoGetSalesRoomsByUser(string IdUsuario, bool AutoAsignLogin = false)
    {
      Task.Factory.StartNew(() => BRSalesRooms.GetSalesRoomsByUser(IdUsuario, "ALL"))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            List<SalesRoomByUser> data = task1.Result;
            if (data.Count > 0)
            {
              cmbPlace.ItemsSource = data;
              cmbPlace.SelectedValuePath = "srID";
              cmbPlace.DisplayMemberPath = "srN";
              cmbPlace.IsEnabled = true;

              if (AutoAsignLogin)
              {
                List<SalesRoomByUser> lstPS = cmbPlace.ItemsSource as List<SalesRoomByUser>;
                int index = lstPS.FindIndex(x => x.srN.Equals(this.userData.SalesRoom.srN));
                cmbPlace.SelectedIndex = index;
              }
              else if (_iniFileHelper != null)
              {
                cmbPlace.SelectedValue = _iniFileHelper.readText("Login", "SalesRoom", "");
              }
              else
              {
                cmbPlace.SelectedIndex = 0;
              }
            }
            else
            {
              cmbPlace.IsEnabled = false;
              cmbPlace.Text = "No data found";
            }
          }
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    #endregion

  }
}
