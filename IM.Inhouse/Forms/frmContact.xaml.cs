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
using System.Net;


namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmContact.xaml
  /// </summary>
  public partial class frmContact : Window
  {
    #region Atributos

    private int _guestID;
    private Guest _guest;
    private UserData _user;
    private bool _searchPRByTxt;

    #endregion

    #region Constructores y destructores

    public frmContact(int guestID)
    {
      InitializeComponent();
      _guestID = guestID;
      lblUserName.Content = App.User.User.peN;
      _guest = BRGuests.GetGuest(_guestID);
    }

    #endregion

    #region Metodos

    #region GetIPMachine
    public string GetIPMachine()
    {
      IPHostEntry host;
      string localIP = "";
      host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (IPAddress ip in host.AddressList)
      {
        if (ip.AddressFamily.ToString() == "InterNetwork")
        {
          localIP = ip.ToString();
        }
      }
      return localIP;
    }
    #endregion

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
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      frmLogin log = new frmLogin(null, false, EnumLoginType.Normal, false);
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
          if (log.userData.HasPermission(EnumPermission.Register, EnumPermisionLevel.Standard))
        {
          _guest.guInfo = true;
          if (_guest.guInfo == true && (log.userData.HasRole(EnumRole.PRCaptain) || log.userData.HasRole(EnumRole.PRSupervisor)))
          {
            _user = log.userData;
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
        Personnel PR = BRPersonnel.GetPersonnelById(_guest.guPRAvail);
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
      cboguPRInfo.ItemsSource = BRPersonnel.GetPersonnel(App.User.Location.loID, "ALL", "PR");
      Guest _guest = BRGuests.GetGuest(_guestID);
      if (_guest.guInfoD.HasValue)
      {
        txtguInfoD.Text = _guest.guInfoD.Value.Date.ToString();
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
        _guest.guloInfo = _user.User.peID;
        _guest.guPRInfo = txtguPRInfo.Text;
        _guest.guInfoD = Convert.ToDateTime(txtguInfoD.Text).Date;
        _guest.guInfo = true;
        BRGuests.SaveGuest(_guest);

        //guardamos la informacion de contacto
        BRGuests.SaveGuestLog(_guestID, App.User.LeadSource.lsHoursDif, _user.User.peID);

        //guardamos el movimiento de contactacion del huesped
        BRGuests.SaveGuestMovement(_guestID, EnumGuestsMovementsType.Contact, _user.User.peID, Environment.MachineName.ToString(), GetIPMachine());

        chkguInfo.IsChecked = true;
        this.Close();
      }
    }
    #endregion

    #endregion
  }
}
