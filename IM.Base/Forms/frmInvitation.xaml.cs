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
using System.Windows.Input;
using System.Data;
using System.ComponentModel;
using System;
using System.Windows.Data;
using IM.Styles.Classes;

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
    //Grids Banderas
    private DataGridCellInfo _IGCurrentCell;
    private bool _hasError = false;
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


        dtgGifts.InitializingNewItem += ((object sender, InitializingNewItemEventArgs e) =>
        {
          if (e.NewItem != null)
          {
            ((InvitationGift)e.NewItem).igQty = 1;
          }
        });

        GridHelper.setUpGrid(dtgGifts, new InvitationGift());

      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Cargamos la UI dependiendo del tipo de Invitacion
      ControlsConfiguration(_invitationType);
      UIHelper.SetUpControls(new Guest(), this);

    }

    private void imgButtonSave_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      CommonCatObject k = DataContext as CommonCatObject;
      var h = k.GuestObj.guID;


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
      stkFlightNumber.Visibility = Visibility.Collapsed;//Ocultamos FlighInfo de  brdPRInfo - Grid Column 4 
    }

    #endregion

    private void EnableControlsExternal()
    {
      #region Guest Information
      txtguID.IsEnabled =
      txtguHReservID.IsEnabled = false;
      btnSearch.IsEnabled = true;
      txtguRef.IsEnabled =
      txtguInvitD.IsEnabled =
      txtguInvitT.IsEnabled = false;
      #endregion

      #region Invitation Type & Languages
      chkguQuinella.IsEnabled = true;
      chkguShow.IsEnabled =
      chkguInterval.IsEnabled = false;
      cmbLanguage.IsEnabled = true;
      #endregion

      #region Profile Opera
      txtguIdProfileOpera.IsEnabled =
      txtguLastNameOriginal.IsEnabled =
      txtguFirstNameOriginal.IsEnabled = false;
      #endregion

      #region Guest1 & Guest 2
      txtguLastName1.IsEnabled =
      txtguFirstName1.IsEnabled =
      txtguAge1.IsEnabled =
      cbogums1.IsEnabled =
      txtguOccup1.IsEnabled =
      txtguEmail1.IsEnabled = true;

      txtguLastName2.IsEnabled =
      txtguFirstName2.IsEnabled =
      txtguAge2.IsEnabled =
      cbogums2.IsEnabled =
      txtguOccup2.IsEnabled =
      txtguEmail2.IsEnabled = true;
      #endregion

      #region PR, SalesRoom, etc..
      btnChange.IsEnabled =
      btnReschedule.IsEnabled =
      btnRebook.IsEnabled = false;
      cmbPRContact.IsEnabled =
      cmbPR.IsEnabled =
      cmbSalesRooms.IsEnabled = true;
      cmbLocation.IsEnabled = false;
      chkguAntesIO.IsEnabled =
      dtpBookDate.IsEnabled =
      cbxBookTime.IsEnabled = true;
      chkDirect.IsEnabled = false;
      dtpRescheduleDate.IsEnabled =
      cbxReschudeleTime.IsEnabled =
      chkReschedule.IsEnabled = false;
      #endregion

      #region OtherInfo
      txtguExtraInfo.IsEnabled =
      txtguRoomNum.IsEnabled =
      cmbOtherInfoHotel.IsEnabled =
      cmbOtherInfoAgency.IsEnabled =
      cmbOtherInfoCountry.IsEnabled =
      txtguPax.IsEnabled =
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
      txtguCCType.IsEnabled = false;
      dtgCCCompany.IsEnabled = true;
      #endregion

      #region Rooms Qty And ElectronicPurse
      txtguAccountGiftsCard.IsEnabled = false;
      #endregion

    }

    private void dtgGifts_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      
      Control ctrl = e.EditingElement as Control;
      ctrl.Focus();
      
      
      DataGrid dataGrid = sender as DataGrid;
      InvitationGift invitationGiftCustom = dataGrid.Items.CurrentItem as InvitationGift;
      _IGCurrentCell = dtgGifts.CurrentCell;
      InvitationGiftCustom.StartEdit(_invitationMode, invitationGiftCustom, _IGCurrentCell, dataGrid, _hasError);


    }

    private void dtgGifts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      //Si paso las validaciones del preparingCellForEdit
      if (!_hasError)
      {
        if (e.EditAction == DataGridEditAction.Commit)
        {
          //Obtenemos el acceso a los controles que necesitamos
          DataGrid dtg = sender as DataGrid;
          //Obtenemos el Objeto 
          InvitationGift invitationGift = e.Row.Item as InvitationGift;

          bool _hasErrorValidateEdit = false;
          //Validamos la celda
          InvitationGiftCustom.ValidateEdit(invitationGift, _hasErrorValidateEdit, _IGCurrentCell);

          //Si Paso las validaciones
          if (!_hasErrorValidateEdit)
          {
            InvitationGiftCustom.AfterEdit(dtg, invitationGift, _IGCurrentCell, txtGiftTotalCost, txtGiftTotalPrice, txtGiftMaxAuth);
          }

        }

      }
    }

    private void dtgGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {

        //DataGrid fi = sender as DataGrid;
        //InvitationGiftCustom koko = fi.CurrentItem as InvitationGiftCustom;

        //fi.RowEditEnding -= dtgGifts_RowEditEnding;
        //fi.CommitEdit();
        //fi.RowEditEnding += dtgGifts_RowEditEnding;
        e.Cancel = false;
      }
    }

    private void dtgGifts_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {

    }

    private void dtgGifts_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
    {

    }


  }
}
