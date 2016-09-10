using System;
using System.Windows;
using System.Windows.Controls;
using IM.Model.Classes;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Base.Helpers;
using System.Windows.Input;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmContact.xaml
  /// </summary>
  public partial class frmContact : Window
  {
    #region Atributos

    private int _guestID;
    private Guest _guest;
    private UserData _userLoguedo, _userPrimero;
    public bool _wasSave;

    public string PRInfo
    {
      get
      {
        return cboguPRInfo.Text;
      }
    }
    public DateTime InfoD
    {
      get
      {
        return Convert.ToDateTime(txtguInfoD.Text).Date;
      }
    } 

    #endregion

    #region Constructores y destructores

    public frmContact(int guestId,UserData userData)
    {
      InitializeComponent();
      _guestID = guestId;
      lblUserName.Content = userData.User.peN;
      _userPrimero = userData;
      Title = $"Contact - Guest ID: {guestId}";
    }

    #endregion

    #region Metodos   

    #region Validate

    /// <summary>
    /// Valida 
    /// </summary>
    /// <history>
    /// [jorcanche]  created 25/04/2016
    /// </history>
    public bool Validate()
    {
      // validamos el PR
      if (!ValidateHelper.ValidateRequired(cboguPRInfo, "Unavailable PR", condition: true)) return false;

      // validamos que el motivo de indisponibilidad exista      
      Personnel pr = BRPersonnel.GetPersonnelById(cboguPRInfo.SelectedValue.ToString());
      if (pr != null) return true;
      UIHelper.ShowMessage("The PR not exist");
      cboguPRInfo.Focus();
      return false;
    }

    #endregion

    #endregion

    #region Eventos del formulario

    #region btnEdit_Click
    /// <summary>
    /// Abre login para modificar cintactación
    /// </summary>
    /// <historyy>
    /// [jorcanche] 24/04/2016
    /// </historyy>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      var log = new frmLogin(switchLoginUserMode:true);
      if (_userPrimero.AutoSign)
      {
        //App.User.User.pePwd = EncryptHelper.Encrypt(App.User.User.pePwd);
        log.UserData = _userPrimero;
      }
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        if (log.UserData.HasPermission(EnumPermission.Register, EnumPermisionLevel.Standard))
        {         
          if (_guest.guInfo == false || (log.UserData.HasRole(EnumRole.PRCaptain) || log.UserData.HasRole(EnumRole.PRSupervisor)))
          {
            _userLoguedo = log.UserData;
            txtguInfoD.Text = BRHelpers.GetServerDate().ToString("dd-MM-yyyy");
            btnCancel.IsEnabled =  btnSave.IsEnabled = cboguPRInfo.IsEnabled = true;
            btnEdit.IsEnabled =  false;            
            lblUserName.Content = log.UserData.User.peN;
          }
          else
          {
            UIHelper.ShowMessage("You do not have sufficient permissions to modify the contact's information", MessageBoxImage.Asterisk, "Permissions");
          }
        }
        else
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to modify the contact's information", MessageBoxImage.Asterisk, "Permissions");
        }
      }
    }
    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      Mouse.OverrideCursor = Cursors.Wait;
      LoadContact();
    }
    #endregion

    #region btnLog_Click
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      var frmGuestLog = new frmGuestLog(_guestID) {Owner = this};
      frmGuestLog.ShowDialog();
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga e inicializa las variables 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [jorcanche] created  05/03/016
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Mouse.OverrideCursor = Cursors.Wait;
      _guest = await BRGuests.GetGuest(_guestID);
      LoadContact();
    }

    private async void LoadContact()
    {
       
      cboguPRInfo.ItemsSource = await BRPersonnel.GetPersonnel(_userPrimero.Location.loID, "ALL", "PR");
      var guest = await BRGuests.GetGuest(_guestID);      
      if (guest.guInfoD.HasValue)
      {
        txtguInfoD.Text = guest.guInfoD.Value.Date.ToString("dd-MM-yyyy");
      }    
      if (guest.guPRInfo != string.Empty)
      {
        cboguPRInfo.SelectedValue = guest.guPRInfo;
      }
      chkguInfo.IsChecked = guest.guInfo;

      btnEdit.IsEnabled = true; btnSave.IsEnabled = btnCancel.IsEnabled = false;      
      cboguPRInfo.IsEnabled = false;
      Mouse.OverrideCursor = null;
    }
    #endregion

    #region btnSave_Click
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      Mouse.OverrideCursor = Cursors.Wait;     
      try
      {
        if (Validate())
        {
          //guardamos la informacion de contacto
          _guest.guloInfo = _userLoguedo.User.peID;
          _guest.guPRInfo = cboguPRInfo.SelectedValue.ToString();
          _guest.guInfoD = Convert.ToDateTime(txtguInfoD.Text).Date;
          _guest.guInfo = true;

          //Enviamos los parametros para que guarde los cambios del guest y el log del Guest y de igual forma los moviemientos de este (SaveGuestMovement).
          //Si hubo un erro al ejecutar el metodo SaveGuestContact nos devolvera 0, indicando que ningun paso 
          //se realizo, es decir ni se guardo el Guest, el Log ni los movimientos de este, siendo así ya no modificamos la variable
          //_wasSaved que es la que indica que se guardo el Avail.
          if (await BRGuests.SaveGuestContact(_guest, _userPrimero.LeadSource.lsHoursDif, _userLoguedo.User.peID,
              EnumGuestsMovementsType.Contact, Environment.MachineName, ComputerHelper.GetIpMachine()) != 0)
          {
            //Modificamos las variable indicando que si se guardo la variable
            _wasSave = true;
            chkguInfo.IsChecked = true;
          }
          else
          {
            UIHelper.ShowMessage("There was an error saving the information, consult your system administrator",
              MessageBoxImage.Error, "Information can not keep");
          }
          Close();
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
      Mouse.OverrideCursor = null;
    }
    #endregion

    #endregion
  }
}
