﻿using System;
using System.Windows;
using System.Windows.Controls;
using IM.Model.Classes;
using IM.Base.Forms;
using IM.Model;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Base.Helpers;
using System.Collections.Generic;

namespace IM.Inhouse
{
  /// <summary>
  /// Interaction logic for frmContact.xaml
  /// </summary>
  public partial class frmAvailability : Window
  {
    #region Atributos

    private int _guestID;
    private UserData _user;
    private Guest _guest;
    private bool _searchUmByTxt;

    #endregion

    #region Constructores y destructores

    public frmAvailability(int guestID)
    {
      InitializeComponent();

      _guestID = guestID;
      lblUserName.Content = App.User.User.peN;
    }

    #endregion

    #region Metodos

    #region LoadControls
    private void LoadControls()
    {
      _guest = BRGuests.GetGuest(_guestID);
      cboguum.ItemsSource = BRUnavailableMotives.GetUnavailableMotives(1);
      if (_guest.guPRAvail != null)
      {
        txtguPRAvail.Text = _guest.guPRAvail;
        //Obtiene el Nombre del PR Ingresando su ID.
        Personnel PRAvailability = BRPersonnel.GetPersonnelById(_guest.guPRAvail);
        if (PRAvailability != null)
        {
          txtguPRAvailName.Text = PRAvailability.peN;
        }
      }
      if (_guest.guum != 0)
      {
        cboguum.SelectedValue = _guest.guum;
        txtguum.Text = _guest.guum.ToString();
      }
      chkguOriginAvail.IsChecked = _guest.guOriginAvail;
      chkguAvail.IsChecked = _guest.guAvail;
      chkguAvailBySystem.IsChecked = _guest.guAvailBySystem;
      EnableControls();
    }

    #endregion

    #region EnableControls
    private void EnableControls()
    {
      txtguPRAvail.IsReadOnly = true;
      cboguum.IsEnabled = false;
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
        if (_guest.guCheckOutD < BRHelpers.GetServerDate().Date && !(_user.HasRole(EnumRole.PRCaptain) || _user.HasRole(EnumRole.PRSupervisor)))
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
          UnavailableMotive motive = BRUnavailableMotives.GetUnavailableMotive(Convert.ToInt16(txtguum.Text), true);
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
      cboguum.IsEnabled = false;
      txtguum.IsReadOnly = true;
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
      cboguum.IsEnabled = true;
      txtguum.IsReadOnly = false;
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
      if (Validate())
      {
        _guest.guum = Convert.ToByte(txtguum.Text);
        _guest.guOriginAvail = chkguOriginAvail.IsChecked.Value;
        _guest.guAvail = chkguAvail.IsChecked.Value;
        _guest.guPRAvail = txtguPRAvail.Text;
        BRGuests.SaveGuest(_guest);
        //Se guarda el Log del Guest 
        BRGuests.SaveGuestLog(_guestID, App.User.LeadSource.lsHoursDif, _user.User.peID);
        this.Close();
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

    #region btnEdit_Click
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      frmLogin log = new frmLogin();
      log.ShowDialog();
      if (log.IsAuthenticated)
      {
        if (log.userData.HasPermission(EnumPermission.Available, EnumPermisionLevel.ReadOnly))
        {
          _user = log.userData;
          txtguPRAvail.Text = _user.User.peID;
          txtguPRAvailName.Text = _user.User.peN;
          btnSave.IsEnabled = true;
          cboguum.IsReadOnly = _guest.guAvail;
          cboguum.IsEnabled = !_guest.guAvail;
          txtguum.IsReadOnly = _guest.guAvail;
          chkguAvail.IsEnabled = true;
          if (_user.HasRole(EnumRole.PRCaptain))
          {
            chkguOriginAvail.IsEnabled = true;
          }
        }
        else
        {
          UIHelper.ShowMessage("You do not have sufficient permissions to modify the Avility's information", MessageBoxImage.Asterisk, "Permissions");
        }
      }
    }

    #endregion

    #endregion
  }
}