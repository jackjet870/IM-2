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
using IM.Base.Helpers;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmContact.xaml
  /// </summary>
  public partial class frmContact : Window
  {
    #region Atributos

    private int _guestID;
    private UserData _user;

    #endregion

    #region Constructores y destructores

    public frmContact(int guestID)
    {
      InitializeComponent();

      _guestID = guestID;
      lblUserName.Content = App.User.User.peN;
      cboguPRInfo.ItemsSource = BRPersonnel.GetPersonnel(App.User.Location.loID, "PR");
    }

    #endregion
    
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      frmLogin log = new frmLogin(null, false, EnumLoginType.Normal, false);
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        if (log.userData.HasPermission(EnumPermission.Register, EnumPermisionLevel.ReadOnly))
        {         
          _user = log.userData;
          txtguInfoD.Text = BRHelpers.GetServerDate().Date.ToString();
          btnSave.IsEnabled = cboguPRInfo.IsEnabled = true;
        }
        else
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to modify the contact's information", MessageBoxImage.Asterisk, "Permissions");
        }
      }
    }
    private void cboguPRInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {      
      txtguPRInfo.Text = ((PersonnelShort)cboguPRInfo.SelectedItem).peID;
    }
   
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      cboguPRInfo.ItemsSource = BRPersonnel.GetPersonnel(App.User.Location.loID,"ALL", "PR");
      Guest _guest = BRGuests.GetGuest(_guestID);
      if(_guest.guInfoD.HasValue)
      { 
        txtguInfoD.Text = _guest.guInfoD.Value.Date.ToString();
      }
      cboguPRInfo.SelectedValue = _guest.guPRInfo;
      chkguInfo.IsChecked = _guest.guInfo;
      cboguPRInfo.IsEnabled = txtguPRInfo.IsEnabled = false;
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      chkguInfo.IsChecked = true;
      BRGuests.SaveGuestLog(_guestID, App.User.LeadSource.lsHoursDif, _user.User.peID);
    }

  
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
     frmGuestLog frmGuestLog = new frmGuestLog(_guestID);
     frmGuestLog.ShowDialog();
    }
  }
}
