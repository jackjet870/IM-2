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
using IM.Model.Classes;
using IM.Model;
using IM.Base.Forms;
using IM.BusinessRules.BR;
using IM.Model.Enums;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmContact.xaml
  /// </summary>
  public partial class frmContact : Window
  {
    private UserLogin _userLogin = new UserLogin();
    private LocationLogin _locationLogin = new LocationLogin();
     
    public frmContact(UserData userData)
    {
      InitializeComponent();
      _userLogin = userData.User;
      lblUserName.Content = _userLogin.peN;
      _locationLogin = userData.Location;
      cboguPRInfo.ItemsSource = BRPersonnel.GetPersonnel(_locationLogin.loID);
    }
 
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      frmLogin log = new frmLogin(null, false, EnumLoginType.Normal, false);
      log.ShowDialog();
      if (log.IsAuthenticated && log.userData.HasPermission( "REGISTER", EnumPermisionLevel.ReadOnly))
      {
        UserLogin _user = new UserLogin();
        _user = log.userData.User;
        txtguInfoD.Text = BRHelpers.GetServerDate().Date.ToString();
        btnSave.IsEnabled = true;
      }
    }

    private void cboguPRInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       txtguPRInfo.Text = cboguPRInfo.SelectedValue.ToString();
    }
   
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      chkguInfo.IsChecked = true;
    }

    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmGuestLog frmGuestLog = new frmGuestLog();
      frmGuestLog.ShowDialog();
    }
  }
}
