using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmContact.xaml
  /// </summary>
  public partial class frmAvailability : Window
  {
    #region Atributos

    private readonly int _guestId;
    private UserData _user;
    private Guest _guest;
    private bool _searchUmByTxt;
    public bool WasSaved;
    public byte Guum => txtguum.Text != string.Empty ? Convert.ToByte(txtguum.Text) : (byte)0;

    public string GuPrAvail => cboguPRAvail.SelectedValue.ToString();

    public bool Avail => chkguAvail.IsChecked != null && chkguAvail.IsChecked.Value;

    #endregion

    #region Constructores y destructores

    public frmAvailability(int guestId)
    {
      InitializeComponent();
      _guestId = guestId;
      lblUserName.Content = Context.User.User.peN;
      Title = $"Availability - Guest ID: {guestId}";
    }

    #endregion

    #region Metodos

    #region LoadControls

    private async void LoadControls()
    {        
      _guest = await BRGuests.GetGuest(_guestId);
      cboguum.ItemsSource = await BRUnavailableMotives.GetUnavailableMotives(1);
      cboguPRAvail.ItemsSource = await BRPersonnel.GetPersonnel(Context.User.Location.loID, "ALL", "PR");
      if (_guest.guPRAvail != null)
      {
        cboguPRAvail.SelectedValue = _guest.guPRAvail;
      }
      if (_guest.guum != 0)
      {
        cboguum.SelectedValue = _guest.guum;
        txtguum.Text = _guest.guum.ToString();
      }
      chkguOriginAvail.IsChecked = _guest.guOriginAvail;
      chkguAvail.IsChecked = _guest.guAvail;
      chkguAvailBySystem.IsChecked = _guest.guAvailBySystem;
      txtguum.IsEnabled = cboguum.IsEnabled = cboguPRAvail.IsEnabled = btnCancel.IsEnabled =
       btnSave.IsEnabled =
         chkguOriginAvail.IsEnabled = chkguAvail.IsEnabled = txtguum.IsEnabled = cboguum.IsEnabled = false;
      btnEdit.IsEnabled = true;
    }

    #endregion  

    #region Validate
    /// <summary>
    /// Valida los datos
    /// </summary>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// </history>
    private bool Validate()
    {
      // validamos el motivo de indisponibilidad
      if (!ValidateHelper.ValidateRequired(txtguum, "Unavailable Motive", condition: !chkguAvail.IsChecked.Value)) return false;

      // validamos que el motivo de indisponibilidad exista
      if (!chkguAvail.IsChecked.Value)
      {
        UnavailableMotive motive = BRUnavailableMotives.GetUnavailableMotive(Convert.ToInt16(txtguum.Text), true);
        if (motive == null)
        {
          UIHelper.ShowMessage("The unavailable motive does not exist");
          txtguum.Focus();
          return false;
        }
      }
      // validamos los permisos
      if (!ValidatePermissions()) return false;

      return true;
    }
    #endregion

    #region ValidatePermissions
    /// <summary>
    /// Valida los permisos
    /// </summary>
    /// <history>
    /// [jorcanche]  12/Mar/2016 Created
    /// </history>
    private bool ValidatePermissions()
    {
      // si se desea marcar como no disponible
      if (!chkguAvail.IsChecked.Value)
      {
        // validamos que el huesped no haya hecho Check Out (No aplica para gerentes y supervisores de PR)
        if (_guest.guCheckOutD < BRHelpers.GetServerDate() && !(_user.HasRole(EnumRole.PRCaptain) || _user.HasRole(EnumRole.PRSupervisor)))
        {
          UIHelper.ShowMessage("Guest already made Check-out.");
          return false;
        }
        // validamos que el huesped no haya sido invitado (No aplica para gerentes de PR)
        if (_guest.guInvit && !_user.HasRole(EnumRole.PRCaptain))
        {
          UIHelper.ShowMessage("Guest already invited.");
          return false;
        }
        // si no tiene permiso estandar de disponibilidad
        if (!_user.HasPermission(EnumPermission.Available, EnumPermisionLevel.Standard))
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to mark as unavailable");
          return false;
        }
      }
      // si se desea marcar como disponible
      else
      {
        // si no tiene permiso especial de disponibilidad
        if (!_user.HasPermission(EnumPermission.Available, EnumPermisionLevel.Standard))
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to mark as unavailable");
          return false;
        }
      }
      return true;
    }

    #endregion

    #endregion

    #region Eventos del formulario

    #region Window_Loaded

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadControls();
    }

    #endregion

    #region cboguum_SelectionChanged
    private void cboguum_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cboguum.SelectedIndex != -1 || txtguum.Text == string.Empty)
      {
        if (!chkguAvail.IsChecked.Value)
        {
          if (cboguum.SelectedValue != null)
          {
            if (!_searchUmByTxt)
            {
              txtguum.Text = cboguum.SelectedValue.ToString();
            }
          }
        }
        else {
          txtguum.Text = string.Empty;
        }
      }
    }
    #endregion

    #region txtguum_LostFocus
    private void txtguum_LostFocus(object sender, RoutedEventArgs e)
    {
      _searchUmByTxt = true;
      int umid;
      if (txtguum.Text != string.Empty)
      {
        //validosmos q no ingrese datos numericos    
        if (!int.TryParse(txtguum.Text, out umid))
        {
          UIHelper.ShowMessage("you must enter a numeric value");
          txtguum.Text = string.Empty;
          txtguum.Focus();
        }
        else
        {
          // validamos que el motivo de indisponibilidad exista en los activos
          UnavailableMotive motive = BRUnavailableMotives.GetUnavailableMotive(Convert.ToInt32(txtguum.Text), true);
          if (motive == null)
          {
            UIHelper.ShowMessage("The unavailable motive does not exist");
            txtguum.Text = string.Empty;
            txtguum.Focus();
          }
          else
          {
            cboguum.SelectedValue = txtguum.Text;
          }
        }
      }
      else
      {
        cboguum.SelectedIndex = -1;
      }
      _searchUmByTxt = false;
    }
    #endregion

    #region chkguAvail_Checked
    private void chkguAvail_Checked(object sender, RoutedEventArgs e)
    {
      txtguum.IsEnabled = cboguum.IsEnabled = false;   
      if (_guest != null)
      {
        cboguum.SelectedValue = _guest.guum;
        txtguum.Text = _guest.guum == 0 ? string.Empty : _guest.guum.ToString();
      }
    }
    #endregion

    #region chkguAvail_Unchecked
    private void chkguAvail_Unchecked(object sender, RoutedEventArgs e)
    {
      txtguum.IsEnabled = cboguum.IsEnabled = true;
    }
    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      LoadControls();
    }
    #endregion  

    #region btnSave_Click
    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (Validate())
        {         
          //guardamos la informacion de disponibilidad
          _guest.guum = txtguum.Text != string.Empty ? Convert.ToByte(txtguum.Text) : (byte)0;
          _guest.guOriginAvail = chkguOriginAvail.IsChecked.Value;
          _guest.guAvail = chkguAvail.IsChecked.Value;
          _guest.guPRAvail = cboguPRAvail.SelectedValue.ToString();

          //Enviamos los parametros para que guarde los cambios del guest y el log del Guest.
          //Si hubo un erro al ejecutar el metodo SaveChangedOfGuest nos devolvera 0, indicando que ningun paso 
          //se realizo, es decir ni se guardo el Guest ni el Log, y siendo así ya no modificamos la variable
          //_wasSaved que es el que indica que se guardo el Avail.
          if (await BRGuests.SaveChangedOfGuest(_guest, Context.User.LeadSource.lsHoursDif, _user.User.peID) != 0)
          {
            WasSaved = true;
          }
          else
          {
            //De no ser así informamos que no se guardo la información por algun motivo
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
    }
    #endregion

    #region btnLog_Click
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      var frmGuestLog = new frmGuestLog(_guestId) {Owner = this};
      frmGuestLog.ShowDialog();
    }
    #endregion

    #region btnEdit_Click
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      var log = new frmLogin(switchLoginUserMode: true, windowStartupLocation: WindowStartupLocation.CenterScreen);
      if (Context.User.AutoSign)
      {        
        log.UserData = Context.User;
      }
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        if (log.UserData.HasPermission(EnumPermission.Available, EnumPermisionLevel.ReadOnly))
        {
          _user = log.UserData;
          cboguPRAvail.SelectedValue = _user.User.peID;          

          btnCancel.IsEnabled = btnSave.IsEnabled  = chkguAvail.IsEnabled  = true;
          btnEdit.IsEnabled = false;

          //No se permite modificar el motivo de indisponibilidad si esta disponible          
            txtguum.IsEnabled = !_guest.guAvail;
            cboguum.IsEnabled = !_guest.guAvail;         

          //No se permite modificar el campo de Originalmente disponible si el usuario no es un gerente 
          if (_user.HasRole(EnumRole.PRCaptain))
          {
            chkguOriginAvail.IsEnabled = true;
          }

          lblUserName.Content = log.UserData.User.peN;
        }
        else
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to modify the Avility's information", MessageBoxImage.Asterisk, "Permissions");
        }
      }
    }

    #endregion

    #region txtguum_PreviewTextInput
    /// <summary>
    /// Valida que solo se ingresen numeros
    /// </summary>
    /// <history>
    /// [JORCANCHE]  28/06/2016
    /// </history>
    private void txtguum_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
    }
  }

  #endregion

  #endregion


}
