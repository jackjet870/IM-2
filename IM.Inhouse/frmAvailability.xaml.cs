using System.Windows;
using System.Windows.Controls;
using IM.Model.Classes;
using IM.Base.Forms;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmContact.xaml
  /// </summary>
  public partial class frmAvailability : Window
  {
    private LocationLogin _locationLogin = new LocationLogin();
    private UserLogin _userLogin = new UserLogin();
    public frmAvailability(UserData userData)
    {
      InitializeComponent();
      _userLogin = userData.User;      
      lblUserName.Content = _userLogin.peN;
      _locationLogin = userData.Location;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      cboguum.ItemsSource = BRUnavailableMotives.GetUnavailableMotives(1);
      EnablendControls();
    }
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      frmLogin log = new frmLogin(null, false, EnumLoginType.Normal, false);
      log.ShowDialog();
      if (log.IsAuthenticated && log.userData.HasPermission("AVAIL", EnumPermisionLevel.ReadOnly))
      {             
        UserLogin _user = new UserLogin();
        _user = log.userData.User;
        txtguPRAvail.Text = _user.peID;
        cboguPRAvail.Text = _user.peN;       
        btnSave.IsEnabled = true;
        cboguum.IsReadOnly = false;
        cboguPRAvail.IsReadOnly = chkguAvail.IsEnabled = true;
        if (log.userData.HasRole("PRCAPT"))        
        {
          chkguOriginAvail.IsEnabled = true;
        }
      }         
    }
    private void EnablendControls()
    {
      txtguPRAvail.IsReadOnly  = cboguPRAvail.IsReadOnly  = true;
      cboguum.IsEnabled = false;
    }

    private void cboguum_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!chkguAvail.IsChecked.Value)
      {
        txtguum.Text = cboguum.SelectedValue.ToString();
      }
      else {
        txtguum.Text = string.Empty;        
      }
    }

    private void chkguAvail_Checked(object sender, RoutedEventArgs e)
    {
      cboguum.IsEnabled = false;
      cboguum.SelectedIndex = -1;
      txtguum.Text = string.Empty;
    }

    private void chkguAvail_Unchecked(object sender, RoutedEventArgs e)
    {
      cboguum.IsEnabled = true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmGuestLog frmGuestLog = new frmGuestLog();
      frmGuestLog.ShowDialog();
    }
  }
}
