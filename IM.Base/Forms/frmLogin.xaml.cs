using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using IM.Model;
using IM.Model.Classes;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using System.IO;
using System.Linq;
using IM.Base.Helpers;
using System.Threading.Tasks;
using IM.Model.Helpers;
using IM.Base.Classes;

namespace IM.Base.Forms
{
  public partial class frmLogin : Window
  {
    #region Propiedades y Atributos
    List<Item> places;
    readonly Window _splash;
    readonly EnumLoginType _loginType;
    readonly bool _changePassword;
    readonly bool _autoSign;
    readonly bool _modeSwitchLoginUser;
    readonly bool _validatePermission;
    readonly bool _validateRole;
    readonly bool _invitationMode;
    string _invitationPlaceId;
    readonly EnumPermission _permission;
    readonly EnumPermisionLevel _permissionLevel;
    readonly EnumProgram _program;
    readonly EnumRole _role;
    IniFileHelper _iniFileHelper;
    public UserData UserData { get; set; }
    public bool IsAuthenticated { get; set; }
    public ExecuteCommandHelper CloseWindow { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Contructor Login
    /// </summary>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [edgrodriguez] 27/02/2016 Modified
    /// [edgrodriguez] 29/02/2016 Modified
    /// [erosado] 25/04/2016  Modified. Se restructuro y se agregaron parametros _permission, _permissionLevel, _program, _modeSwitchLoginUser
    /// </history>
    public frmLogin(
      Window splash = null,
      EnumLoginType loginType = EnumLoginType.Normal,
      EnumProgram program = EnumProgram.All,
      bool validatePermission = false,
      bool validateRole = false,
      bool changePassword = false,
      bool autoSign = false,
      EnumPermission permission = EnumPermission.None,
      EnumPermisionLevel permissionLevel = EnumPermisionLevel.None,
      EnumRole role = EnumRole.None,
      bool switchLoginUserMode = false,
      bool invitationMode = false,
      string invitationPlaceId = ""
      )
    {

      InitializeComponent();
      _splash = splash;
      _loginType = loginType;
      _program = program;
      _validatePermission = validatePermission;
      _validateRole = validateRole;
      _changePassword = changePassword;
      _autoSign = autoSign;
      _permission = permission;
      _permissionLevel = permissionLevel;
      _role = role;
      _modeSwitchLoginUser = switchLoginUserMode;
      _invitationMode = invitationMode;
      _invitationPlaceId = invitationPlaceId;
      CloseWindow = new ExecuteCommandHelper(x => btnCancelar_Click(this, null));
    }

    #endregion

    #region Eventos del formulario
    /// <summary>
    /// Carga la informacion del Configuration.ini y de UiPrepare
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">e</param>
    /// <history>
    /// [erosado] 30/04/2016 Created
    /// [wtorres] 05/07/2016 Modified. Ahora solo se carga la informacion del archivo Ini cuando no es modo Switch
    /// </history>
    private void Login_Loaded(object sender, RoutedEventArgs e)
    {
      //Si es modo Switch
      if (_modeSwitchLoginUser)
      {
        if (UserData != null && UserData.AutoSign) // AutoSign Activo
        {
          // Se llenan los controles con informacion de UserData
          txtUser.Text = UserData.User.peID;
          txtPassword.Password = UserData.User.pePwd;
          LoadPlace(UserData.User.peID, true, false);
          chkAutoSign.IsChecked = UserData.AutoSign;
        }
      }
      else
      {
        //Cargamos Datos del Configuration.ini
        LoadFromFile();
      }
      //Preparamos la interfaz
      UiPrepare();
    }

    #region Boton Aceptar
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
    /// [jorcanche] 11/04/2016 Si chkAutoSign es verdadero desencriptamos el password y se guarda  Y AutoSign se deja como verdadero
    /// [erosado] 26/04/2016  Se restructuro el evento y se optimizo el codigo.
    /// </history>   
    private void btnAceptar_Click(object sender, RoutedEventArgs e)
    {
      //Validar que el usuario meta toda la informacion requerida para el Login
      var msj = ValidateHelper.ValidateForm(this, "Login");


      if (string.IsNullOrEmpty(msj))
      {
        var _encryptPassword = EncryptHelper.Encrypt(txtPassword.Password);
        UserData = BRPersonnel.Login(_loginType, txtUser.Text,
          (cmbPlace.Visibility == Visibility.Visible) ? cmbPlace.SelectedValue.ToString() : "");
        //Validar las credenciales del usuario sean correctas si devuelve 0 si tiene permiso.
        if (ValidateUserCredential(_encryptPassword) != 0) return;
        //Se valida que el usuario esta activo
        if (!UserData.User.peA)
        {
          UIHelper.ShowMessage("User ID is inactive.", MessageBoxImage.Error);
          txtUser.Focus();
          return;
        }
        //Revisar si se desea cambiar el usuario o si se tiene el check cambiar pass activado.
        if (ChangePassword() != 0) return;

        //Validamos los Permisos y Roles necesarios para entrar
        if (ValidatePermissionAndRole() != 0) return;

        if (chkAutoSign.IsChecked.Value)
        {
          UserData.AutoSign = true;
          UserData.User.pePwd = EncryptHelper.Encrypt(UserData.User.pePwd);
        }
        IsAuthenticated = true;
        Close();
        _splash?.Hide();
      }
      else
      {
        UIHelper.ShowMessage(msj, MessageBoxImage.Warning);
      }
    }
    #endregion

    #region Boton Cancelar
    /// <summary>
    /// Cierra el formulario padre y despues cierra el frmLogin
    /// </summary>
    ///<history>
    ///[edgar]  29/Feb/2016 Created
    ///[erosado]  19/Mar/2016 Se valido que el _frmBase No sea null
    /// </history>
    private void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
      _splash?.Close();
      Close();
    }
    #endregion

    #region txtUser LostFocus
    /// <summary>
    /// Método de carga del formulario.
    /// </summary>
    /// <history>
    /// [vipacheco] 2-26-2016 Created
    /// [edgrodriguez] 27/02/2016 Modified
    /// [jorcanche] 01/03/2016 Modified (Se agrega el "Case" Location)
    /// [vipacheco] 01/03/2016 Modified --> Se agrego case Sales Room
    /// [erosado] 19/Mar/2016 Se agregaron metodos Asincronos para que no se trabe la interfaz.
    /// [erosado] 28/04/2016  Se optimizo el metodo
    /// </history>
    private void txtUser_LostFocus(object sender, RoutedEventArgs e)
    {
      ValidationLoginMode();
    }
    #endregion

    #endregion Eventos del formulario

    #region Async Methods

    #region GetAllPlaces
    /// <summary>
    /// Obtiene todos los place y los mantiene en memoria para utilizarlos posteriormente
    /// </summary>
    /// <returns>lista dynamica</returns>
    /// <history>
    /// [erosado] 29/04/2016  Created
    /// </history>
    public async Task<int> getAllPlaces()
    {
      places = new List<Item>();
      switch (_loginType)
      {
        case EnumLoginType.SalesRoom:
          var sr = await BRSalesRooms.GetSalesRoomsByUser();
          sr.ForEach(s => places.Add(new Item() { Id = s.srID, Name = s.srN, UserId = s.plpe }));
          break;
        case EnumLoginType.Warehouse:
          var wh = await BRWarehouses.GetWarehousesByUser();
          wh.ForEach(w => places.Add(new Item() { Id = w.whID, Name = w.whN, UserId = w.plpe }));
          break;
        case EnumLoginType.Location:
          var lo = await BRLocations.GetLocationsByUser(programs: EnumToListHelper.GetEnumDescription(_program));
          lo.ForEach(l => places.Add(new Item() { Id = l.loID, Name = l.loN, UserId = l.plpe }));
          break;
      }
      return 1;
    }
    #endregion

    #region DoGetWareHousesByUser
    /// <summary>
    /// Obtiene la lista de WareHouses dependiendo el IdUsuraio
    /// </summary>
    /// <history>
    /// [erosado] 19/Mar/2016 Created
    /// [erosado] 14/04/2016  Modified. Se agrego la seleccion de datos desde el archivo Configuration.ini
    /// [erosado] 14/04/2016  Modified. Se modifico para busque en la fuente de datos que esta en memoria places.
    /// [wtorres] 05/07/2016  Modified. Ahora se busca el almacen por clave
    /// </history>
    public void DoGetWareHousesByUser(string IdUsuario, bool Autosign, bool ConfigurationIni)
    {
      var data = places.Where(x => x.UserId == IdUsuario).ToList();

      if (data.Count > 0)
      {
        cmbPlace.ItemsSource = data;
        cmbPlace.IsEnabled = true;
        if (Autosign)
        {
          var lstPS = cmbPlace.ItemsSource as List<Item>;
          int index = lstPS.FindIndex(x => x.Id.Equals(UserData.Warehouse.whID));
          cmbPlace.SelectedIndex = index;
        }
        else
        {
          if (ConfigurationIni)
          {
            var value = _iniFileHelper.ReadText("Login", "Warehouse", "");
            if (!string.IsNullOrEmpty(value))
            {
              cmbPlace.SelectedValue = value;
            }
            else { cmbPlace.SelectedIndex = 0; }
          }
          else { cmbPlace.SelectedIndex = 0; }
        }
      }
      else { cmbPlace.IsEnabled = false; cmbPlace.Text = "No data found"; }
      cmbPlace.IsEnabled = data.Count > 1;
    }
    #endregion

    #region DoGetLocationsByUser
    /// <summary>
    /// Obtiene la lista de Location dependiendo el IdUsuario y PROGRAMAS IH
    /// </summary>
    /// <history>
    /// [erosado] 19/03/2016  Created
    /// [erosado] 14/04/2016  Modified. Se agrego la seleccion de datos desde el archivo Configuration.ini
    /// [wtorres] 05/07/2016  Modified. Ahora se busca la locacion por clave
    /// [erosado] 10/08/2016  Modified. Se cambio para que location busque por ID antes estaba por Name
    /// </history>
    public void DoGetLocationsByUser(string IdUsuario, bool Autosign, bool ConfigurationIni)
    {
      var data = places.Where(x => x.UserId == IdUsuario).ToList();
      if (data.Count > 0)
      {
        cmbPlace.ItemsSource = data;
        cmbPlace.IsEnabled = true;

        if (Autosign)
        {
          var lstPS = cmbPlace.ItemsSource as List<Item>;
          int index = lstPS.FindIndex(x => x.Id.Equals(UserData.Location.loID));
          cmbPlace.SelectedIndex = index;
        }
        else
        {
          if (ConfigurationIni)
          {
            var value = _iniFileHelper.ReadText("Login", "Location", "");
            if (!string.IsNullOrEmpty(value))
            {
              cmbPlace.SelectedValue = value;
            }
            else { cmbPlace.SelectedIndex = 0; }
          }
          else { cmbPlace.SelectedIndex = 0; }
        }

        if (_invitationMode)
        {
          var lstPS = cmbPlace.ItemsSource as List<Item>;
          int index = lstPS.FindIndex(x => x.Id.Equals(_invitationPlaceId));
          if (index == -1)
          {
            UIHelper.ShowMessage("You do not have permissions on this location", MessageBoxImage.Exclamation, "Intelligence Marketing");
          }
          else
          {
            cmbPlace.SelectedIndex = index;
          }
        }
      }
      else
      {
        if (_invitationMode)
        {
          cmbPlace.IsEnabled = false; UIHelper.ShowMessage("You do not have permissions on this location", MessageBoxImage.Exclamation, "Intelligence Marketing"); ;
        }
        else
        {
          cmbPlace.IsEnabled = false; cmbPlace.Text = "No data found";
        }
      }
      cmbPlace.IsEnabled = data.Count > 1 && !_invitationMode;
    }
    #endregion

    #region DoGetSalesRoomsByUser
    /// <summary>
    /// Obtiene la lista de SalesRooms dependiendo el IDUsuario y TODAS LAS REGIONES
    /// </summary>
    /// <history>
    /// [erosado] 19/Mar/2016 Created
    /// [erosado] 14/04/2016  Modified. Se agrego la seleccion de datos desde el archivo Configuration.ini
    /// [wtorres] 05/07/2016  Modified. Ahora se busca la sala de ventas por clave
    /// [erosado] 10/08/2016  Modified. Se cambio para que location busque por ID antes estaba por Name
    /// </history>
    public void DoGetSalesRoomsByUser(string IdUsuario, bool Autosign, bool ConfigurationIni)
    {
      var data = places.Where(x => x.UserId == IdUsuario).ToList();
      if (data.Count > 0)
      {
        cmbPlace.ItemsSource = data;
        cmbPlace.IsEnabled = true;
        if (Autosign)
        {
          var lstPS = cmbPlace.ItemsSource as List<Item>;
          int index = lstPS.FindIndex(x => x.Id.Equals(UserData.SalesRoom.srID));
          cmbPlace.SelectedIndex = index;
        }
        else
        {
          if (ConfigurationIni)
          {
            var value = _iniFileHelper.ReadText("Login", "SalesRoom", "");
            if (!string.IsNullOrEmpty(value))
            {
              cmbPlace.SelectedValue = value;
            }
            else { cmbPlace.SelectedIndex = 0; }
          }
          else
          {
            cmbPlace.SelectedIndex = 0;
          }
        }
        if (_invitationMode)
        {
          cmbPlace.IsEnabled = false;
          var lstPS = cmbPlace.ItemsSource as List<Item>;
          int index = lstPS.FindIndex(x => x.Id.Equals(_invitationPlaceId));
          if (index == -1)
          {
            UIHelper.ShowMessage("You do not have permissions on this location", MessageBoxImage.Exclamation, "Intelligence Marketing");
          }
          else
          {
            cmbPlace.SelectedIndex = index;
          }
        }

      }
      else
      {
        if (_invitationMode)
        {
          cmbPlace.IsEnabled = false; cmbPlace.Text = "You do not have permissions on this salesroom";
        }
        else
        {
          cmbPlace.IsEnabled = false; cmbPlace.Text = "No data found";
        }
      }
      cmbPlace.IsEnabled = data.Count > 1 && !_invitationMode;
    }
    #endregion

    #endregion

    #region Metodos

    #region LoadFromFile
    /// <summary>
    /// Funcion encargado de cargar los datos desde el archivo de configuracion
    /// </summary>
    /// <history>
    /// [vipacheco] 08/03/2016 Modified. Se agrego case para sales room
    /// [erosado]   14/04/2016 Modified. Se elimino la seleccion del archivo de configuracion de este metodo.
    /// [erosado]   27/04/2016 Modified. Se simplifico la carga del archivo de configuracion.
    /// [wtorres]   05/07/2016 Modified. Ahora se lee del archivo de configuracion el lugar y el Autosign
    /// </history>
    private void LoadFromFile()
    {
      var strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(strArchivo)) return;
      _iniFileHelper = new IniFileHelper(strArchivo);

      //Se llenan los controles con informacion de Configuration.ini
      txtUser.Text = _iniFileHelper.ReadText("Login", "UserName", "");
      txtPassword.Password = _iniFileHelper.ReadText("Login", "Password", "");
      chkAutoSign.IsChecked = _iniFileHelper.ReadBoolean("Login", "Autosign", false);
      LoadPlace(txtUser.Text.Trim(), false, true);
    }

    #endregion

    #region Validacion de credenciales de usuario
    /// <summary>
    /// Revisa si la contraseña esta proxima a expirar o si el usuario marco el checkBox changePassword
    /// </summary>
    /// <history>
    /// [erosado] 26/04/2016  Created
    /// </history>
    private int ChangePassword()
    {
      var value = 0;
      //Validamos la contraseña no haya expirado o el check este activo.
      var _expireDate = UserData.User.pePwdD.AddDays(UserData.User.pePwdDays);
      var _serverDate = BRHelpers.GetServerDate();

      if (_serverDate > _expireDate || (bool)chkChangePwd.IsChecked)
      {
        var changePwd = new frmChangePassword();
        changePwd.userLogin = UserData.User;
        changePwd.serverDate = _serverDate;
        changePwd.ShowDialog();

        if (changePwd.blnOk)
        {
          UserData.User = changePwd.userLogin;
        }
        else
        {
          if (_serverDate > _expireDate)
          {
            UIHelper.ShowMessage("Password expired.");
            value = 1;
          }
        }
      }
      return value;
    }
    /// <summary>
    /// Verifica que el usuario exista y que la contraseña sea correcta
    /// </summary>
    /// <param name="_encryptPassword">Contraña encriptada </param>
    /// <returns>0 continuar 1 usuario no existe 2 contraseña incorrecta</returns>
    /// <history>
    /// [erosado] 27/04/2016  Created
    /// </history>
    private int ValidateUserCredential(string _encryptPassword)
    {
      var value = 0;
      if (UserData.User == null)
      {
        UIHelper.ShowMessage("User ID does not exist.", MessageBoxImage.Error);
        txtUser.Focus();
        value = 1;
      }
      else if (!_encryptPassword.Equals(UserData.User.pePwd))
      {
        UIHelper.ShowMessage("Invalid password.");
        txtPassword.Focus();
        value = 2;
      }
      return value;
    }
    /// <summary>
    /// Verifica que el usuario tenga los permisos definidos en la interfaz
    /// </summary>
    /// <returns>int 0 continuar 1 Sin permiso 2 Sin rol</returns>
    /// <history>
    /// [erosado] 27/04/2016  Created
    /// </history>
    private int ValidatePermissionAndRole()
    {
      var value = 0;
      if (_validatePermission)
      {
        if (!UserData.HasPermission(_permission, _permissionLevel))
        {
          value = 1;
          UIHelper.ShowMessage("You don't have enough permissions to get in", MessageBoxImage.Error);
          txtUser.Focus();
        }
      }
      if (_validateRole)
      {
        if (!UserData.HasRole(_role))
        {
          value = 2;
          UIHelper.ShowMessage("You don't have the role to get in", MessageBoxImage.Error);
          txtUser.Focus();
        }
      }
      return value;
    }
    #endregion

    #region UIPrepare
    /// <summary>
    /// Prepara los elementos que se mostraran en la interfaz y carga ejecuta LoadPlaces
    /// </summary>
    /// <history>
    /// [erosado] 27/04/2016  Created
    /// </history>
    private void UiPrepare()
    {
      //Revisamos la disponibilidad de los checkbox Autosign & ChangePassword
      chkAutoSign.Visibility = (_autoSign) ? Visibility.Visible : Visibility.Hidden;
      chkChangePwd.Visibility = (_changePassword) ? Visibility.Visible : Visibility.Hidden;

      //Revisamos el tipo de LoginType y activamos lo necesario para cada tipo de login
      switch (_loginType)
      {
        case EnumLoginType.Normal:
          cmbPlace.Visibility = Visibility.Hidden;
          lblPlace.Visibility = Visibility.Hidden;
          Height = Height - 25;
          break;
        case EnumLoginType.Location:
          cmbPlace.Visibility = Visibility.Visible;
          lblPlace.Content = "Location";
          btnAceptar.Focus();
          break;
        case EnumLoginType.SalesRoom:
          cmbPlace.Visibility = Visibility.Visible;
          lblPlace.Content = "Sales Room";
          btnAceptar.Focus();
          break;
        case EnumLoginType.Warehouse:
          cmbPlace.Visibility = Visibility.Visible;
          lblPlace.Content = "Warehouse";
          btnAceptar.Focus();
          break;
      }
    }
    #endregion

    #region ValidationLoginMode
    /// <summary>
    /// Valida el ModeSwitchLoginUser
    /// </summary>
    /// <history>
    /// [erosado] 30/04/2016 Created
    /// </history>
    private void ValidationLoginMode()
    {
      bool autoSign = false;
      if (_modeSwitchLoginUser) // SwitchUser
      {
        if (UserData != null && UserData.AutoSign && UserData.User.peID == txtUser.Text.Trim()) // AutoSign Activo
        {
          autoSign = true;
        }
      }
      LoadPlace(txtUser.Text.Trim(), autoSign, false);
    }
    #endregion

    #region LoadPlace

    /// <summary>
    /// Filtra los lugares del usuario actual dependientdo del tipo de login y del usuario
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="autosign">Si trae Activo Autosign</param>
    /// <param name="configurationIni">Indica si tiene archivo de configuracion</param>
    /// <history>
    /// [erosado] 30/04/2016  Created
    /// </history>
    private void LoadPlace(string user, bool autosign, bool configurationIni)
    {
      switch (_loginType)
      {
        case EnumLoginType.Normal://Normal
          break;
        case EnumLoginType.SalesRoom://Sales Room
          DoGetSalesRoomsByUser(user, autosign, configurationIni);
          break;
        case EnumLoginType.Warehouse://Almacen
          DoGetWareHousesByUser(user, autosign, configurationIni);
          break;
        case EnumLoginType.Location://Locacion
          DoGetLocationsByUser(user, autosign, configurationIni);
          break;
      }
    }
    #endregion

    #endregion Metodos

  }




}
