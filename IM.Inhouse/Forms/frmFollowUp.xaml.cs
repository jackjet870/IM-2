using System.Windows;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.Model;
using IM.Base.Forms;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using System;
using System.Windows.Input;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmFollowUp.xaml
  /// </summary>
  public partial class frmFollowUp : Window
  {
    #region Atributos

    private int _guestID;
    private UserData _user;
    private Guest _guest;    
    public DateTime FollowD => Convert.ToDateTime(txtguFollowD.Text).Date;

    public string PrFollow => cboguPRFollow.SelectedValue.ToString();

    public bool _wasSaved;
 
    #endregion

    #region Constructores y destructores

    public frmFollowUp(int guestID)
    {
      InitializeComponent();
      _guestID = guestID;
      lblUserName.Content = App.User.User.peN;
      Title = $"Follow Up - Guest ID: {guestID}";
    }
    #endregion

    #region Metodos

    #region Validate
    public bool Validate()
    {
      // validamos el PR
      if (!ValidateHelper.ValidateRequired(cboguPRFollow, "Unavailable PR", condition: true)) return false;

      // validamos que el motivo de indisponibilidad exista      
      Personnel PR = BRPersonnel.GetPersonnelById(cboguPRFollow.SelectedValue.ToString());
      if (PR == null)
      {
        UIHelper.ShowMessage("The PR not exist");
        cboguPRFollow.Focus();
        return false;
      }
      return true;
    }

    #endregion

    #endregion

    #region Eventos del formulario    

    #region Window_Loaded
    /// <summary>
    /// Carga e inicializa las variables del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [jorcanche] 01/02/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {     

      LoadFollwUp();
    }

    private async void LoadFollwUp()
    {
      cboguPRFollow.IsEnabled = false;
      cboguPRFollow.ItemsSource = await BRPersonnel.GetPersonnel(App.User.Location.loID, "ALL", "PR");
      _guest = await BRGuests.GetGuest(_guestID);
      if (_guest.guFollowD.HasValue)
      {
        txtguFollowD.Text = _guest.guFollowD.Value.Date.ToString("dd-MM-yyyy");
      }
      if (_guest.guPRFollow != string.Empty)
      {
        cboguPRFollow.SelectedValue = _guest.guPRFollow;     
      }
      chkguFollow.IsChecked = _guest.guFollow;

      btnEdit.IsEnabled = true; btnSave.IsEnabled = btnCancel.IsEnabled = false;      
    
    }
    #endregion


    #region btnEdit_Click
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      var log = new frmLogin(switchLoginUserMode:true);
      if (App.User.AutoSign)
      {
        //App.User.User.pePwd = EncryptHelper.Encrypt(App.User.User.pePwd);
        log.UserData = App.User;
      }
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        if (log.UserData.HasPermission(EnumPermission.Register, EnumPermisionLevel.Standard))
        {
          if (_guest.guFollow == false || (log.UserData.HasRole(EnumRole.PRCaptain) || log.UserData.HasRole(EnumRole.PRSupervisor)))
          {
            _user = log.UserData;
            txtguFollowD.Text = BRHelpers.GetServerDate().ToString("dd-MM-yyyy");
            btnCancel.IsEnabled = btnSave.IsEnabled = cboguPRFollow.IsEnabled = true;
            btnEdit.IsEnabled  = false;           
            lblUserName.Content = log.UserData.User.peN;            
          }
          else
          {
            UIHelper.ShowMessage("You do not have sufficient permissions to modify the follow up's information", MessageBoxImage.Asterisk, "Permissions");
          }
        }
        else
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to modify the follow up's information", MessageBoxImage.Asterisk, "Permissions");
        }
      }
    }
    #endregion

    #region btnLog_Click
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      var frmGuestLog = new frmGuestLog(_guestID) {Owner = this};
      frmGuestLog.ShowDialog();
    }

    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      LoadFollwUp();
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
          //guardamos la informacion del seguimiento
          _guest.guPRFollow = cboguPRFollow.SelectedValue.ToString();
          _guest.guFollowD = Convert.ToDateTime(txtguFollowD.Text).Date;
          _guest.guFollow = true;

          //Enviamos los parametros para que guarde los cambios del guest y el log del Guest.
          //Si hubo un erro al ejecutar el metodo SaveGuestAvailOrFollowUp nos devolvera 0, indicando que ningun paso 
          //se realizo, es decir ni se guardo el Guest ni el Log, y siendo así ya no modificamos la variable
          //_wasSaved que es la que indica que se guardo el FollowUp.
          if (await BRGuests.SaveChangedOfGuest(_guest, App.User.LeadSource.lsHoursDif, _user.User.peID) != 0)
          {
            _wasSaved = true;
            chkguFollow.IsChecked = true;
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

