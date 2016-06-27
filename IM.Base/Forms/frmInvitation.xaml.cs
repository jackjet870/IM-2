using System.Windows;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.Model;
using IM.Base.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmInvitation.xaml
  /// </summary>
  public partial class frmInvitation : Window
  {
    #region Propiedades, Atributos
    public UserData _user;
    private readonly EnumInvitationType _invitationType;
    private readonly int _guestId;
    private readonly EnumInvitationMode _invitationMode;
    #endregion
    public frmInvitation(EnumInvitationType InvitationType, UserData User, int GuestId, EnumInvitationMode InvitationMode, bool AllowReschedule = true)
    {
      try
      {
        var catObj = new CommonCatObject(User, GuestId, InvitationMode);
        _invitationType = InvitationType;
        _guestId = GuestId;
        _user = User;
        _invitationMode = InvitationMode;
        DataContext = catObj;
        InitializeComponent();
        
      }
      catch (System.Exception ex)
      {
        UIHelper.ShowMessage(ex, MessageBoxImage.Error, "Invitation");
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Cargamos la UI dependiendo del tipo de Invitacion
      ControlsConfiguration(_invitationType);
    }

    private void imgButtonSave_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var j = DataContext as CommonCatObject;
      UIHelper.ShowMessage(j.InvitationGiftList.Count.ToString());
    }
    #region Metodos Privados
    /// <summary>
    /// Prepara los controles para cada invitacion
    /// </summary>
    /// <param name="_invitationType">EnumInvitationType</param>
    /// [erosado] 16/05/2016  Created
    private void ControlsConfiguration(EnumInvitationType _invitationType)
    {
      txtUserName.Text = _user.User.peN;
      txtPlaces.Text = _invitationType == EnumInvitationType.Host ? _user.SalesRoom.srN : _user.Location.loN;

      switch (_invitationType)
      {
        case EnumInvitationType.InHouse:
          InHouseControlsConfig();
          break;
        case EnumInvitationType.OutHouse:
          OutHouseControlsConfig();
          break;
        case EnumInvitationType.Host:
          HostControlsConfig();
          break;
        case EnumInvitationType.External:
          ExternalControlsConfig();
          EnableControlsExternal();
          break;
        default:
          break;
      }
    }
    #endregion

    #region ControlsConfig
    /// <summary>
    /// Prepara los controles para que trabaje con InHouseInvitation
    /// </summary>
    /// <history>
    /// [erosado] 14/05/2016  Created.
    /// </history>
    private void InHouseControlsConfig()
    {
      stkOutInvitation.Visibility = Visibility.Collapsed; //Quitamos Out.Invint de brdGuestInfo
      stkPRContact.Visibility = Visibility.Collapsed;//Quitamos PRContact de  brdPRInfo - Grid Column 0
      stkFlightNumber.Visibility = Visibility.Collapsed;//Ocultamos FlighInfo de  brdPRInfo - Grid Column 4 
      stkLocation.IsEnabled = false;
    }
    /// <summary>
    /// Prepara los controles para que trabaje con OutHouseInvitation
    /// </summary>
    /// <history>
    /// [erosado] 14/05/2016  Created.
    /// </history>
    private void OutHouseControlsConfig()
    {
      stkRsrvNum.Visibility = Visibility.Collapsed;
      imgSearch.Visibility = Visibility.Collapsed;
      stkRebookRef.Visibility = Visibility.Collapsed;
      btnReschedule.Visibility = Visibility.Collapsed;
      btnRebook.Visibility = Visibility.Collapsed;
      stkRescheduleDate.Visibility = Visibility.Collapsed;
      chkReschedule.Visibility = Visibility.Collapsed;
      stkFlightNumber.Visibility = Visibility.Visible;
      brdRoomsQtyAndElectronicPurse.Visibility = Visibility.Collapsed;
      brdCreditCard.Visibility = Visibility.Collapsed;
    }
    /// <summary>
    /// Prepara los controles para que trabaje con HostInvitation
    /// </summary>
    /// <history>
    /// [erosado] 14/05/2016  Created.
    /// </history>
    private void HostControlsConfig()
    {
      stkPRContact.Visibility = Visibility.Collapsed;
      stkSales.IsEnabled = false;
      stkLocation.IsEnabled = true;
      stkFlightNumber.Visibility = Visibility.Collapsed;
      stkElectronicPurse.Visibility = Visibility.Collapsed;
    }
    /// <summary>
    /// Prepara los controles para que trabaje con ExternalInvitation
    /// </summary>
    /// <history>
    /// [erosado] 14/05/2016  Created.
    /// </history>
    private void ExternalControlsConfig()
    {
      btnSearch.Visibility = Visibility.Visible; //Se visualiza el boton Search.
    }

    #endregion

    private void EnableControlsExternal()
    {
      #region Guest Information
      txtGuid.IsEnabled =
      txtReservationNumber.IsEnabled = false;
      btnSearch.IsEnabled = true;
      txtRebookRef.IsEnabled =
      txtDate.IsEnabled =
      txtTime.IsEnabled = false;
      #endregion

      #region Invitation Type & Languages
      chkQuiniella.IsEnabled = true;
      chkShow.IsEnabled =
      chkInterval.IsEnabled = false;
      cmbLanguage.IsEnabled = true;
      #endregion

      #region Profile Opera
      txtIDOpera.IsEnabled =
      txtLastNameOpera.IsEnabled =
      txtFirstNameOpera.IsEnabled = false;
      #endregion

      #region Guest1 & Guest 2
      txtLastNameGuest1.IsEnabled =
      txtFirstNameGuest1.IsEnabled =
      txtAgeGuest1.IsEnabled =
      cmbMaritalStatusGuest1.IsEnabled =
      txtOcuppationGuest1.IsEnabled =
      txtEmailGuest1.IsEnabled = true;

      txtLastNameGuest2.IsEnabled =
      txtFirstNameGuest2.IsEnabled =
      txtAgeGuest2.IsEnabled =
      cmbMaritalStatusGuest2.IsEnabled =
      txtOcuppationGuest2.IsEnabled =
      txtEmailGuest2.IsEnabled = true;
      #endregion

      #region PR, SalesRoom, etc..
      btnChange.IsEnabled =
      btnReschedule.IsEnabled =
      btnRebook.IsEnabled = false;
      cmbPRContact.IsEnabled =
      cmbPR.IsEnabled =
      cmbSalesRooms.IsEnabled = true;
      cmbLocation.IsEnabled = false;
      chkBeforeIO.IsEnabled =
      dtpBookDate.IsEnabled =
      cbxBookTime.IsEnabled = true;
      chkDirect.IsEnabled = false;
      dtpRescheduleDate.IsEnabled =
      cbxReschudeleTime.IsEnabled =
      chkReschedule.IsEnabled = false;
      #endregion

      #region OtherInfo
      txtOtherInfoComments.IsEnabled =
      txtOtherInfoRoomNum.IsEnabled =
      cmbOtherInfoHotel.IsEnabled =
      cmbOtherInfoAgency.IsEnabled =
      cmbOtherInfoCountry.IsEnabled =
      txtOtherInfoPax.IsEnabled =
      dtpOtherInfoArrivalD.IsEnabled =
      dtpOtherInfoDepartureD.IsEnabled = true;
      #endregion

      #region GuestStatus
      cmbGuestStatus.IsEnabled = true;
      #endregion

      #region Gifts
      dtgGifts.IsEnabled = true;
      txtGiftMaxAuth.IsReadOnly =
      txtGiftTotalCost.IsReadOnly =
      txtGiftTotalPrice.IsReadOnly = false;
      #endregion

      #region Deposits
      dtgDeposits.IsEnabled =
      txtBurned.IsEnabled =
      cmbCurrency.IsEnabled =
      cmbPaymentType.IsEnabled =
      cmbResorts.IsEnabled = true;
      #endregion

      #region Credit Cards
      txtCcCompany.IsEnabled = false;
      dtgCCCompany.IsEnabled = true;
      #endregion

      #region Rooms Qty And ElectronicPurse
      txtepAcount.IsEnabled = false;
      #endregion

    }

    private void dtgGifts_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
    {
      if (((TextBox)e.EditingElement).Text == "0")
      {
        UIHelper.ShowMessage("NO puede ser cero");
      }

      
    }
  }
}
