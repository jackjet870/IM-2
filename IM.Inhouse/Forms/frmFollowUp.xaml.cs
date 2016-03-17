using System.Windows;
using System.Windows.Controls;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.Model;
using IM.Base.Forms;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

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

    #endregion

    #region Constructores y destructores

    public frmFollowUp(int guestID)
    {
      InitializeComponent();

      _guestID = guestID;
      lblUserName.Content = App.User.User.peN;
      cboguPRFollow.ItemsSource = BRPersonnel.GetPersonnel(App.User.Location.loID, "PR");
    }
    #endregion

    #region Eventos del formulario    

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      cboguPRFollow.ItemsSource = BRPersonnel.GetPersonnel(App.User.Location.loID, "ALL", "PR");
      Guest _guest = BRGuests.GetGuest(_guestID);
      if (_guest.guInfoD.HasValue)
      {
        txtguFollowD.Text = _guest.guInfoD.Value.Date.ToString();
      }
      cboguPRFollow.SelectedValue = _guest.guPRInfo;
      chkguFollow.IsChecked = _guest.guInfo;

      cboguPRFollow.IsEnabled = txtguPRFollow.IsEnabled = false;
    }
    #endregion

    #region cboguPRInfo_SelectionChanged
    private void cboguPRInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtguPRFollow.Text = ((PersonnelShort)cboguPRFollow.SelectedItem).peID;
    }
    #endregion 

    #region btnEdit_Click
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      frmLogin log = new frmLogin(null, false, EnumLoginType.Normal, false);
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        if (log.userData.HasPermission(EnumPermission.Register, EnumPermisionLevel.ReadOnly))
        {
          _user = log.userData;
          txtguFollowD.Text = BRHelpers.GetServerDate().Date.ToString();
          btnSave.IsEnabled = cboguPRFollow.IsEnabled = true;
        }
        else
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to modify the contact's information", MessageBoxImage.Asterisk, "Permissions");
        }
      }
    }
    #endregion

    #region btnLog_Click
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmGuestLog frmGuestLog = new frmGuestLog(_guestID);
      frmGuestLog.ShowDialog();
    }

    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
    #endregion

    #region btnSave_Click
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      chkguFollow.IsChecked = true;
      BRGuests.SaveGuestLog(_guestID, App.User.LeadSource.lsHoursDif, _user.User.peID);
    }
    #endregion
    
    #endregion
  }
}
