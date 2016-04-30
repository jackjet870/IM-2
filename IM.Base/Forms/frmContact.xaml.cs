using System;
using System.Windows;
using System.Windows.Controls;
using IM.Model.Classes;
using IM.Model;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Base.Helpers;

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
    private UserData _userLoguedo,_userPrimero;
    private bool _searchPRByTxt;
    public bool _wasSave;

    public string PRInfo
    {
      get
      {
        return txtguPRInfo.Text;
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

    public frmContact(int guestID,UserData userData)
    {
      InitializeComponent();
      _guestID = guestID;
      lblUserName.Content = userData.User.peN;
      _userPrimero = userData;
      _guest = BRGuests.GetGuest(_guestID);
    }

    #endregion

    #region Metodos   

    #region Validate
    public bool Validate()
    {
      // validamos el PR
      if (!ValidateHelper.ValidateRequired(txtguPRInfo, "Unavailable PR", condition: true)) return false;

      // validamos que el motivo de indisponibilidad exista      
      Personnel PR = BRPersonnel.GetPersonnelById(txtguPRInfo.Text); 
      if (PR == null)
      {
        UIHelper.ShowMessage("The PR not exist");
        txtguPRInfo.Focus();
        return false;
      }
      return true;
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
      frmLogin log = new frmLogin(null, EnumLoginType.Normal, changePassword: false, autoSign: false);
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
            txtguInfoD.Text = BRHelpers.GetServerDate().Date.ToString();
            btnSave.IsEnabled = cboguPRInfo.IsEnabled = true;
            txtguPRInfo.IsReadOnly = false;
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

    #region cboguPRInfo_SelectionChanged
    /// <summary>
    /// Abre login para modificar cintactación
    /// </summary>
    /// <historyy>
    /// [jorcanche] 24/04/2016
    /// </historyy>
    private void cboguPRInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cboguPRInfo.SelectedIndex != -1 || txtguPRInfo.Text == string.Empty)
      {
        if (cboguPRInfo.SelectedValue != null)
        {
          if (!_searchPRByTxt)
          {
            txtguPRInfo.Text = ((PersonnelShort)cboguPRInfo.SelectedItem).peID;
          }
        }
        else
        {
          txtguPRInfo.Text = string.Empty;
        }
      }
    }


    #endregion

    #region txtguPRInfo_LostFocus
    private void txtguPRInfo_LostFocus(object sender, RoutedEventArgs e)
    {
      _searchPRByTxt = true;
      if (txtguPRInfo.Text != string.Empty)
      {
        // validamos que el motivo de indisponibilidad exista en los activos
        Personnel PR = BRPersonnel.GetPersonnelById(txtguPRInfo.Text);
        if (PR == null)
        {
          UIHelper.ShowMessage("The PR not exist");
          txtguPRInfo.Text = string.Empty;
          txtguPRInfo.Focus();
        }
        else
        {
          cboguPRInfo.SelectedValue = txtguPRInfo.Text;
        }
      }
      else
      {
        cboguPRInfo.SelectedIndex = -1;
      }
      _searchPRByTxt = false;
    }
    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
    #endregion

    #region btnLog_Click
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmGuestLog frmGuestLog = new frmGuestLog(_guestID);
      frmGuestLog.ShowDialog();
    }
    #endregion

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      cboguPRInfo.ItemsSource = BRPersonnel.GetPersonnel(_userPrimero.Location.loID, "ALL", "PR");
      Guest _guest = BRGuests.GetGuest(_guestID);
      if (_guest.guInfoD.HasValue)
      {
        txtguInfoD.Text = _guest.guInfoD.Value.Date.ToString("dd-MM-yyyy");
      }
   
      if (_guest.guPRInfo != string.Empty)
      {
        cboguPRInfo.SelectedValue = _guest.guPRInfo;
        txtguPRInfo.Text = _guest.guPRInfo;
      }
      chkguInfo.IsChecked = _guest.guInfo;
      //cboguPRInfo.IsEnabled = txtguPRInfo.IsEnabled = false;
    }
    #endregion

    #region btnSave_Click
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (Validate())
      {
        //guardamos la informacion de contacto
        _guest.guloInfo = _userLoguedo.User.peID;
        _guest.guPRInfo = txtguPRInfo.Text;
        _guest.guInfoD = Convert.ToDateTime(txtguInfoD.Text).Date;
        _guest.guInfo = true;     

        //Enviamos los parametros para que guarde los cambios del guest y el log del Guest y de igual forma los moviemientos de este (SaveGuestMovement).
        //Si hubo un erro al ejecutar el metodo SaveGuestContact nos devolvera 0, indicando que ningun paso 
        //se realizo, es decir ni se guardo el Guest, el Log ni los movimientos de este, siendo así ya no modificamos la variable
        //_wasSaved que es la que indica que se guardo el Avail.
        if (BRGuests.SaveGuestContact(_guest, _userPrimero.LeadSource.lsHoursDif, _userLoguedo.User.peID,
            EnumGuestsMovementsType.Contact,Environment.MachineName.ToString(),ComputerHelper.GetIPMachine()) != 0)
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
        this.Close();
      }
      //BRGuests.SaveGuest(_guest);        
      //BRGuestsLogs.SaveGuestLog(_guestID, App.User.LeadSource.lsHoursDif, _user.User.peID);
      //BRGuests.SaveGuestMovement(_guestID, EnumGuestsMovementsType.Contact, _user.User.peID, Environment.MachineName.ToString(), GetIPMachine());
    }
    #endregion

    #endregion
  }
}
