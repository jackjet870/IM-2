using IM.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmInvitationBase.xaml
  /// </summary>
  /// <history>
  /// [lchairez] 16/02/2016 Created
  /// </history>
  public partial class frmInvitationBase : Window
  {

    private IM.BusinessRules.Enums.InvitationType _invitationType;
    private UserData _user;
    public frmInvitationBase()
    {
      InitializeComponent();
    }

    public frmInvitationBase(IM.BusinessRules.Enums.InvitationType invitationType, UserData user)
    {
      this._invitationType = invitationType;
      _user = user;
      InitializeComponent();
    }

    #region Métodos de la forma

    /// <summary>
    /// Configura los controles de la forma
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void frmInvitationBase_Loaded(object sender, RoutedEventArgs e)
    {
      ControlsConfiguration();
      LoadControls();
    }

    /// <summary>
    /// Ejecuta el botón Change
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnChange_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Reschedule
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnReschedule_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Rebook
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnRebook_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Search
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Editar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Imprimir
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void bntPrint_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Guardar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Canelar
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {

    }

    /// <summary>
    /// Ejecuta el botón Log
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Crated.
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {

    }


    private void cmbPRContract_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbPR_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbSalesRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbBookingTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbRescheduleTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbHotel_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbAgency_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbPaymentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbResort_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbMaritalStatusGuest1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbMaritalStatusGuest2_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
    #endregion

    #region Métodos privados

    /// <summary>
    /// Oculta o habilita los controles específicos de cada módulo.
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void ControlsConfiguration()
    {
      switch (_invitationType)
      {
        case BusinessRules.Enums.InvitationType.InHouse:
          InHouseControlsConfig();
          break;
        case BusinessRules.Enums.InvitationType.OutHouse:
          OutHouseControlsConfig();
          break;
        case BusinessRules.Enums.InvitationType.Host:
          HostControlsConfig();
          break;
        case BusinessRules.Enums.InvitationType.Animation:
          AnimationControlsConfig();
          break;
        case BusinessRules.Enums.InvitationType.Regen:
          RegenControlsConfig();
          break;
      }
    }

    private void LoadControls()
    {
      switch (_invitationType)
      {
        case BusinessRules.Enums.InvitationType.InHouse:
          LoadInHouseControls();
          break;
        case BusinessRules.Enums.InvitationType.OutHouse:
          LoadOutHouseControls();
          break;
        case BusinessRules.Enums.InvitationType.Host:
          LoadHostControls();
          break;
        case BusinessRules.Enums.InvitationType.Animation:
          LoadAnimationControls();
          break;
        case BusinessRules.Enums.InvitationType.Regen:
          LoadRegenControls();
          break;
      }
    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo In House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void InHouseControlsConfig()
    {
      colOutInvitation.Width = new GridLength(0); //Con esta instrucción desaparece la columna
      rowPRContract.Height = new GridLength(0); //Con esta instrucción desaparece el renglon
      rowPRContractTitles.Height = new GridLength(0); //Con esta instrucción desaparece el renglon
      lblFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;
    }

    /// <summary>
    /// /// Oculta o habilita los controles necesarios para el módulo Out House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void OutHouseControlsConfig()
    {
      colReservNum.Width = new GridLength(0);
      colBtnSearch.Width = new GridLength(0);
      colRebookRef.Width = new GridLength(0);
      btnReschedule.Visibility = Visibility.Hidden;
      btnRebook.Visibility = Visibility.Hidden;
      lblFlightNumber.Visibility = Visibility.Visible;
      txtFlightNumber.Visibility = Visibility.Visible;
      lblLocation2.SetValue(Grid.ColumnProperty, 1);
      txtLocation2.SetValue(Grid.ColumnProperty, 1);
      lblReschedule.Visibility = Visibility.Hidden;
      txtRescheduleDate.Visibility = Visibility.Hidden;
      cmbRescheduleTime.Visibility = Visibility.Hidden;
      lblRefresh.Visibility = Visibility.Hidden;
      chkRefresh.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;
      colElectronicPurseCreditCard.Width = new GridLength(0);
      rowRoomQuantity.Height = new GridLength(0);
    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Host
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void HostControlsConfig()
    {
      rowPRContractTitles.Height = new GridLength(0);
      rowPRContract.Height = new GridLength(0);
      lblSalesRoom.Visibility = Visibility.Hidden;
      txtSalesRoom.Visibility = Visibility.Hidden;
      cmbSalesRoom.Visibility = Visibility.Hidden;
      lblLocation2.Visibility = Visibility.Hidden;
      txtLocation2.Visibility = Visibility.Hidden;
      lblFlightNumber.Visibility = Visibility.Hidden;
      txtFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Visible;
      txtLocation.Visibility = Visibility.Visible;
      cmbLocation.Visibility = Visibility.Visible;
      lblSalesRoom2.Visibility = Visibility.Visible;
      txtSalesRoom2.Visibility = Visibility.Visible;
      grbElectronicPurse.Visibility = Visibility.Hidden;


    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Animation
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void AnimationControlsConfig()
    {
      colOutInvitation.Width = new GridLength(0);
      rowPRContractTitles.Height = new GridLength(0);
      rowPRContract.Height = new GridLength(0);
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblBeforeInOut.Visibility = Visibility.Hidden;
      chkBeforeInOut.Visibility = Visibility.Hidden;
      lblSalesRoom.Visibility = Visibility.Visible;
      txtSalesRoom.Visibility = Visibility.Visible;
      cmbSalesRoom.Visibility = Visibility.Visible;
      lblLocation2.Visibility = Visibility.Visible;
      txtLocation2.Visibility = Visibility.Visible;
      colElectronicPurseCreditCard.Width = new GridLength(0);
      
    }

    /// <summary>
    /// Oculta o habilita los controles necesarios para el módulo Regen
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created.
    /// </history>
    private void RegenControlsConfig()
    {
      colOutInvitation.Width = new GridLength(0);
      rowPRContractTitles.Height = new GridLength(0);
      rowPRContract.Height = new GridLength(0);
      lblFlightNumber.Visibility = Visibility.Hidden;
      txtFlightNumber.Visibility = Visibility.Hidden;
      lblLocation.Visibility = Visibility.Hidden;
      txtLocation.Visibility = Visibility.Hidden;
      cmbLocation.Visibility = Visibility.Hidden;
      lblSalesRoom2.Visibility = Visibility.Hidden;
      txtSalesRoom2.Visibility = Visibility.Hidden;
      lblBeforeInOut.Visibility = Visibility.Hidden;
      chkBeforeInOut.Visibility = Visibility.Hidden;
      colElectronicPurseCreditCard.Width = new GridLength(0);
    }

    /// <summary>
    /// Carga los controles del formulatio Invitation In House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private void LoadInHouseControls()
    {
      var catalog = IM.BusinessRules.BR.BRInvitationBase.GetCatalogInvitInHouse(_user.Location.loID, _user.User.peID, _user.LeadSource.lsID);

      LoadComboBoxes(catalog.Languages, cmbLanguage, "la", "ES");
      LoadComboBoxes(catalog.PersonnelByLocation, cmbPR, "pe");
      LoadComboBoxes(catalog.SalesRooms, cmbSalesRoom , "sr");
      LoadComboBoxes(catalog.Hotels, cmbHotel , "hoID", "hoID",_user.Location.loN);
      LoadComboBoxes(catalog.Agencies, cmbAgency , "ag");
      LoadComboBoxes(catalog.Countries, cmbCountry, "co");
      LoadComboBoxes(catalog.Currencies, cmbCurrency, "cu", "US");
      LoadComboBoxes(catalog.PaymentTypes, cmbPaymentType, "pt","CS");
      LoadComboBoxes(catalog.Hotels, cmbResort, "hoID", "hoID", String.Empty);
    }

    /// <summary>
    /// Carga los controles del formulatio Invitation Out House
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private void LoadOutHouseControls()
    {
      //var catalog = IM.BusinessRules.BR.BRInvitationBase.GetCatalogInvitInHouse(_user.Location.loID, _user.User.peID, _user.LeadSource.lsID);

      //LoadComboBoxes(catalog.Languages, cmbLanguage, "la", "ES");
      //LoadComboBoxes(catalog.PersonnelByLocation, cmbPR, "pe");
      //LoadComboBoxes(catalog.SalesRooms, cmbSalesRoom, "sr");
      //LoadComboBoxes(catalog.Hotels, cmbHotel, "hoID", "hoID", _user.Location.loN);
      //LoadComboBoxes(catalog.Agencies, cmbAgency, "ag");
      //LoadComboBoxes(catalog.Countries, cmbCountry, "co");
      //LoadComboBoxes(catalog.Currencies, cmbCurrency, "cu", "US");
      //LoadComboBoxes(catalog.PaymentTypes, cmbPaymentType, "pt", "CS");
      //LoadComboBoxes(catalog.Hotels, cmbResort, "hoID", "hoID", String.Empty);
    }

    /// <summary>
    /// Carga los controles del formulatio Invitation Host
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private void LoadHostControls()
    {
      //var catalog = IM.BusinessRules.BR.BRInvitationBase.GetCatalogInvitInHouse(_user.Location.loID, _user.User.peID, _user.LeadSource.lsID);

      //LoadComboBoxes(catalog.Languages, cmbLanguage, "la", "ES");
      //LoadComboBoxes(catalog.PersonnelByLocation, cmbPR, "pe");
      //LoadComboBoxes(catalog.SalesRooms, cmbSalesRoom, "sr");
      //LoadComboBoxes(catalog.Hotels, cmbHotel, "hoID", "hoID", _user.Location.loN);
      //LoadComboBoxes(catalog.Agencies, cmbAgency, "ag");
      //LoadComboBoxes(catalog.Countries, cmbCountry, "co");
      //LoadComboBoxes(catalog.Currencies, cmbCurrency, "cu", "US");
      //LoadComboBoxes(catalog.PaymentTypes, cmbPaymentType, "pt", "CS");
      //LoadComboBoxes(catalog.Hotels, cmbResort, "hoID", "hoID", String.Empty);
    }

    /// <summary>
    /// Carga los controles del formulatio Invitation Animation
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private void LoadAnimationControls()
    {
      //var catalog = IM.BusinessRules.BR.BRInvitationBase.GetCatalogInvitInHouse(_user.Location.loID, _user.User.peID, _user.LeadSource.lsID);

      //LoadComboBoxes(catalog.Languages, cmbLanguage, "la", "ES");
      //LoadComboBoxes(catalog.PersonnelByLocation, cmbPR, "pe");
      //LoadComboBoxes(catalog.SalesRooms, cmbSalesRoom, "sr");
      //LoadComboBoxes(catalog.Hotels, cmbHotel, "hoID", "hoID", _user.Location.loN);
      //LoadComboBoxes(catalog.Agencies, cmbAgency, "ag");
      //LoadComboBoxes(catalog.Countries, cmbCountry, "co");
      //LoadComboBoxes(catalog.Currencies, cmbCurrency, "cu", "US");
      //LoadComboBoxes(catalog.PaymentTypes, cmbPaymentType, "pt", "CS");
      //LoadComboBoxes(catalog.Hotels, cmbResort, "hoID", "hoID", String.Empty);
    }
    /// <summary>
    /// Carga los controles del formulatio Invitation Regen
    /// </summary>
    /// <history>
    /// [lchairez] 29/02/2016 Created
    /// </history>
    private void LoadRegenControls()
    {
      //var catalog = IM.BusinessRules.BR.BRInvitationBase.GetCatalogInvitInHouse(_user.Location.loID, _user.User.peID, _user.LeadSource.lsID);

      //LoadComboBoxes(catalog.Languages, cmbLanguage, "la", "ES");
      //LoadComboBoxes(catalog.PersonnelByLocation, cmbPR, "pe");
      //LoadComboBoxes(catalog.SalesRooms, cmbSalesRoom, "sr");
      //LoadComboBoxes(catalog.Hotels, cmbHotel, "hoID", "hoID", _user.Location.loN);
      //LoadComboBoxes(catalog.Agencies, cmbAgency, "ag");
      //LoadComboBoxes(catalog.Countries, cmbCountry, "co");
      //LoadComboBoxes(catalog.Currencies, cmbCurrency, "cu", "US");
      //LoadComboBoxes(catalog.PaymentTypes, cmbPaymentType, "pt", "CS");
      //LoadComboBoxes(catalog.Hotels, cmbResort, "hoID", "hoID", String.Empty);
    }

    /// <summary>
    /// Carga los combos de la forma
    /// </summary>
    /// <param name="items">Lista de elementos que contendrá el combo</param>
    /// <param name="combo">Combo que se llenará con los elementos</param>
    /// <param name="displayItem">Nombre del elemento</param>
    /// <param name="valueItem">Valor del elemento</param>
    /// <param name="defaultValue">Valor que será seleccionado por default</param>
    private void LoadComboBoxes(IEnumerable<object> items, ComboBox combo, string displayItem, string valueItem, string defaultValue = "")
    {
      combo.DisplayMemberPath = displayItem;
      combo.SelectedValuePath = valueItem;
      combo.SelectedValue = defaultValue;
      combo.ItemsSource = items;
    }

    /// <summary>
    /// Carga los combos de la forma
    /// </summary>
    /// <param name="items">Lista de elementos que contendrá el combo</param>
    /// <param name="combo">Combo que se llenará con los elementos</param>
    /// <param name="displayItem">Nombre del elemento</param>
    /// <param name="valueItem">Valor del elemento</param>
    /// <param name="defaultValue">Valor que será seleccionado por default</param>
    private void LoadComboBoxes(IEnumerable<object> items, ComboBox combo, string prefix, string defaultValue = "")
    {
      combo.DisplayMemberPath = String.Format("{0}N", prefix);
      combo.SelectedValuePath = String.Format("{0}ID", prefix);
      combo.SelectedValue = defaultValue;
      combo.ItemsSource = items;
    }

    #endregion

  }
}
