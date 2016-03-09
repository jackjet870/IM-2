using System.Windows;
using System.Windows.Controls;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.Model;
using IM.Base.Forms;
using IM.BusinessRules.BR;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmFollowUp.xaml
  /// </summary>
  public partial class frmFollowUp : Window
  {
    private UserLogin _userLogin = new UserLogin();
    private LocationLogin _locationLogin = new LocationLogin();
    public frmFollowUp(UserData userData)
    {
      InitializeComponent();
      _userLogin = userData.User;
      lblUserName.Content = _userLogin.peN;
      _locationLogin = userData.Location;
      cboguPRInfo.ItemsSource = BRPersonnel.GetPersonnel(_locationLogin.loID);
    }
 
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      chkguInfo.IsChecked = true;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      frmLogin log = new frmLogin(null, false, EnumLoginType.Normal, false);
      log.ShowDialog();
      if (log.IsAuthenticated && log.userData.HasPermission("REGISTER", EnumPermisionLevel.ReadOnly))
      {
        UserLogin _user = new UserLogin();
        _user = log.userData.User;
        txtguInfoD.Text = BRHelpers.GetServerDate().Date.ToString();
        btnSave.IsEnabled = true;
      }
    }
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmGuestLog frmGuestLog = new frmGuestLog();
      frmGuestLog.ShowDialog();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void cboguPRInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtguPRInfo.Text = cboguPRInfo.SelectedValue.ToString();
    }
  }
}
