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
    private bool IscellCancelGift = false;
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
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
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
      dtg.IsEnabled = true;
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

    //private void dtgGifts_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
    //{
    //  if (!Keyboard.IsKeyDown(Key.Escape))
    //  {
    //    bool blnIsCancel = false;
    //    IscellCancelGift = false;
    //    DataGrid dtgEdit = sender as DataGrid;
    //    var valueEdit = (Control)e.EditingElement;
    //    InvitationGiftCustom ObjInvitationGiftCustom = dtgGifts.SelectedItem as InvitationGiftCustom;

    //    switch (e.Column.SortMemberPath)
    //    {
    //      case "igQty":
    //        {
    //          TextBox value = valueEdit as TextBox;

    //          if (value.Text == "0" || string.IsNullOrEmpty(value.Text)) //No cero, No NUll o vacio
    //          {
    //            UIHelper.ShowMessage("Quantity can't be lower than 1", title: "Invitation");
    //            e.Cancel = true;
    //          }
    //          if (!string.IsNullOrEmpty(ObjInvitationGiftCustom.iggi))//Cantidad maxima por gift Seleccionado
    //          {
    //            var gift = BRGifts.GetGiftId(ObjInvitationGiftCustom.iggi);

    //            if (gift.giMaxQty > 0 && ObjInvitationGiftCustom.igQty > gift.giMaxQty)
    //            {
    //              UIHelper.ShowMessage($"The maximum quantity authorized of the gift {gift.giN} has been exceeded.\n Max authotized = {gift.giMaxQty} \n", title: "Invitation");
    //              e.Cancel = true;
    //            }
    //          }
    //          break;
    //        }
    //      case "IggiCustom":
    //        {
    //          ComboBox value = valueEdit as ComboBox;

    //          if (ObjInvitationGiftCustom.igQty == 0 || string.IsNullOrEmpty(ObjInvitationGiftCustom.igQty.ToString()))
    //          {
    //            UIHelper.ShowMessage("Please enter the quantity first", title: "Invitation");
    //            e.Cancel = true;
    //          }
    //          if (value.SelectedValue == null ||string.IsNullOrEmpty(value?.SelectedValue.ToString()))
    //          {
    //            UIHelper.ShowMessage("Please select a gift", title: "Invitation");
    //            e.Cancel = true;
    //          }
    //          else
    //          {
    //            var gift = BRGifts.GetGiftId(value.SelectedValue.ToString());
    //            if (gift.giMaxQty > 0 && ObjInvitationGiftCustom.igQty > gift.giMaxQty)
    //            {
    //              UIHelper.ShowMessage($"The maximum quantity authorized of the gift {gift.giN} has been exceeded.\n Max authotized = {gift.giMaxQty} \n", title: "Invitation");
    //              blnIsCancel = true;
    //            }
    //            else
    //            {
    //              ObjInvitationGiftCustom.igAdults = 1;
    //            }
    //          }
    //          break;
    //        }
    //      case "IgAdultsCustom":
    //        {
    //          if (ObjInvitationGiftCustom.igQty==0 && ObjInvitationGiftCustom.igMinors==0)
    //          {
    //            UIHelper.ShowMessage("Quantity of adult and quantity of minors can't be both zero", title: "Invitation");
    //            e.Cancel = true;
    //          }
    //          break;
    //        }
    //      default:
    //        {
    //          break;
    //        }
    //    }

    //    if (!blnIsCancel)
    //    {
    //      dtgEdit.CellEditEnding -= dtgGifts_CellEditEnding;
    //      dtgEdit.CommitEdit(DataGridEditingUnit.Row, true);
    //      //dtgGifts_RowEditEnding(sender,null);
    //      dtgEdit.CellEditEnding += dtgGifts_CellEditEnding;
    //      GridHelper.SelectRow(dtgEdit, dtgEdit.SelectedIndex, e.Column.DisplayIndex + 1, true);

    //    }
    //  }
    //  else
    //  {
    //    IscellCancelGift = true;
    //  }



    //  //TextBox textInput = e.EditingElement as TextBox;
    //  //ComboBox comboBoxSelectItem = e.EditingElement as ComboBox;
    //  //DataGrid dg = sender as DataGrid;
    //  //var objInvitationGiftCustom = dg?.Items?.CurrentItem as InvitationGiftCustom;

    //}

    //private void dtgGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    //{
    //  //DataGrid dg = sender as DataGrid;
    //  //dg.RowEditEnding -= dtgGifts_RowEditEnding;
    //  //if (IscellCancelGift)
    //  //{
    //  //  //dg.CancelEdit();
    //  //}
    //  //else
    //  //{
    //  //   dg.CommitEdit();
    //  //      dg.Items.Refresh();
    //  //}


    //  //dg.RowEditEnding += dtgGifts_RowEditEnding;
    //}

    private void CalculateMaxAuthGifts()
    {
      //decimal maxAuthGifts = 0;

      //foreach (var row in _lstObjInvitGuestStatus)
      //{
      //  if (row.gtgs != null && row.gtQuantity > 0)
      //  {
      //    var guestStatusType = IM.BusinessRules.BR.BRGuestStatusTypes.GetGuestStatusTypes(new Model.GuestStatusType { gsID = row.gtgs }).FirstOrDefault();
      //    maxAuthGifts += row.gtQuantity * guestStatusType.gsMaxAuthGifts;
      //  }
      //}

      //txtMaxAuth.Text = maxAuthGifts.ToString("$#,##0.00;$(#,##0.00)");
    }

    private void dtgGifts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {     
      dtgGifts_RowEditEnding(sender, null);
    }

    private void dtgGifts_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      DataGrid dtg = sender as DataGrid;
      InvitationGiftCustom item = e.Row.DataContext as InvitationGiftCustom;

      switch (e.Column.SortMemberPath)
      {
        case "igQty":
          {
            if (item.igQty == 0)
            {
              UIHelper.ShowMessage("Quantity can't be lower than 1", title: "Invitation");
              GridHelper.SelectRow(dtg, e.Row.GetIndex(), 0);
              return;
            }
            if (!string.IsNullOrEmpty(item.iggi))
            {
              var gift = BRGifts.GetGiftId(item.iggi);

              if (gift.giMaxQty > 0 && item.igQty > gift.giMaxQty)
              {
                UIHelper.ShowMessage($"The maximum quantity authorized of the gift {gift.giN} has been exceeded.\n Max authotized = {gift.giMaxQty} \n", title: "Invitation");
                GridHelper.SelectRow(dtg, e.Row.GetIndex(), 0);
                return;
              }
            }
            break;
          }
        case "Iggi":
          {
            if (string.IsNullOrEmpty(item.igQty.ToString()))
            {
              UIHelper.ShowMessage("Please enter the quantity first", title: "Invitation");
              GridHelper.SelectRow(dtg, e.Row.GetIndex(), 0);
              return;
            }
            break;
          }
        case "IgAdults":
          {
            //if (string.IsNullOrEmpty(item.iggi ))
            //{
            //  item.igAdults = 1;
            //}
            if (item.igAdults == 0 && item.igMinors == 0)
            {
              UIHelper.ShowMessage("Quantity of adult and quantity of minors can't be both zero", title: "Invitation");
              GridHelper.SelectRow(dtg, e.Row.GetIndex(), 3);
            }
            break;
          }
        default:
          {
            break;
          }
      }
    }

    private void dtgGifts_CurrentCellChanged(object sender, System.EventArgs e)
    {

    }

    DataRow _latestEditedObject = null;
    private void dtgGifts_CellEditEnding_1(object sender, DataGridCellEditEndingEventArgs e)
    {
      DataGrid dtg = sender as DataGrid;

      GridHelper.SelectRow(dtg, e.Row.GetIndex(), 3);
      dtg.CellEditEnding -= dtgGifts_CellEditEnding_1;
      dtg.CommitEdit(DataGridEditingUnit.Row, true);
     // dtgGifts_RowEditEnding(sender, null);
      dtg.CellEditEnding += dtgGifts_CellEditEnding_1;
    }

    private void dtgGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      DataGrid fi = sender as DataGrid;
      fi.RowEditEnding -= dtgGifts_RowEditEnding;
      fi.CommitEdit();
      fi.RowEditEnding += dtgGifts_RowEditEnding;

    }
  }
}
