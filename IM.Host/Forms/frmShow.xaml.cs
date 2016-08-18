using CrystalDecisions.CrystalReports.Engine;
using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Classes;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using PalaceResorts.Common.PalaceTools;
using PalaceResorts.Common.Notifications.WinForm;
using Xceed.Wpf.Toolkit;
using LanguageHelper = IM.Base.Helpers.LanguageHelper;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmShow.xaml
  /// </summary>
  /// <history>
  /// [vipacheco] 03/16/2016 Created
  /// </history>
  public partial class frmShow : INotifyPropertyChanged
  {
    #region Atributos

    private readonly int _guestCurrent;
    private EnumProgram _enumProgram;
    private DateTime _dateCurrent;
    private SalesRoomCloseDates _salesRoom = new SalesRoomCloseDates();
    private Guest _guestObj;
    private ObservableCollection<GuestCreditCard> _guestCreditCardList = new ObservableCollection<GuestCreditCard>();
    private ObservableCollection<BookingDeposit> _bookingDepositList = new ObservableCollection<BookingDeposit>();
    private ObservableCollection<InvitationGift> _invitationGiftList = new ObservableCollection<InvitationGift>();
    private bool _blnLoading;
    private byte _ocWelcomeCopies;

    private ObservableCollection<Guest> _guestAdditionalList = new ObservableCollection<Guest>();
    //Vendedores
    private List<ShowSalesman> _showSalesmanList;
    private GuestShow _guestShow;

    #region Colleciones

    private CollectionViewSource _dsMaritalStatus, _dsAgencies, _dsCountries, _dsHotels, _dsLanguajes, _dsCurrencies, _dsPaymentTypes,
      _dsTeamsSalesMen, _dsPersonnel, _dsPersonnelPR, _dsPersonnelLiner, _dsPersonnelCloser, _dsPersonnelExitcloser,
      _dsPersonnelPodium, _dsPersonnelVlo, _dsPersonnelHostsentry, _dsPersonnelHostsgifts, _dsPersonnelHostsexit,
      _dsCreditCardTypes, _dsGuestStatusTypes, _dsDisputeStatus, _dsPaymentPlaces, _dsGifts;
    private bool _hasError;
    private bool _isCellCancel;
    DataGridCellInfo _IGCurrentCellGuestAdditional;
    private bool _isCellCommitGuestAdditional;

    #endregion Colleciones

    #endregion Atributos

    #region Propiedades

    public Guest GuestObj
    {
      get { return _guestObj; }
      set { SetField(ref _guestObj, value); }
    }

    public ObservableCollection<GuestCreditCard> GuestCreditCardList
    {
      get { return _guestCreditCardList; }
      set { SetField(ref _guestCreditCardList, value); }
    }

    public ObservableCollection<BookingDeposit> BookingDepositList
    {
      get { return _bookingDepositList; }
      set { SetField(ref _bookingDepositList, value); }
    }

    public ObservableCollection<InvitationGift> InvitationGiftList
    {
      get { return _invitationGiftList; }
      set { SetField(ref _invitationGiftList, value); }
    }

    public byte OcWelcomeCopies
    {
      get { return _ocWelcomeCopies; }
      set { SetField(ref _ocWelcomeCopies, value); }
    }

    public ObservableCollection<Guest> GuestAdditionalList
    {
      get { return _guestAdditionalList; }
      set { SetField(ref _guestAdditionalList, value); }
    }
    #endregion Propiedades

    #region Constructores y destructores

    public frmShow(int guestID)
    {
      _guestCurrent = guestID;
      InitializeComponent();
      DataContext = this;
      _guestShow = new GuestShow();
    }

    #endregion Constructores y destructores

    #region Metodos

    #region ValidateClosedDate

    /// <summary>
    /// Valida la fecha de cierre de shows
    /// </summary>
    /// <returns></returns>
    private bool ValidateClosedDate(bool blnSetupControls)
    {
      var blnValid = true;
      //si el show es de una fecha cerrada
      if (_guestObj.guShowD != null && Common.IsClosed(_guestObj.guShowD.Value, _salesRoom.srShowsCloseD))
      {
        var iDiffDay = (_guestObj.guShowD.Value.Date - _salesRoom.srShowsCloseD.Date).TotalDays;

        // si todavia no han pasado 7 dias de la fecha de ciere
        if (iDiffDay < 7)
        {
          StaStart("Inside a closed date only shows 7 days ago.", $"(Last closing date: {_salesRoom.srShowsCloseD.Date})");

          // deshabilitamos todos los controles, menos el tipo de show
          if (blnSetupControls)
            EnableControls(false, true);
        }
        //si ya pasaron 7 dias de la fecha de ciere
        else
        {
          StaStart("Inside a closed date, is more than 7 days.", $"(Last closing date: {_salesRoom.srShowsCloseD.Date})");

          // deshabilitamos todos los controles
          if (blnSetupControls)
            EnableControls(false, false);

          blnValid = false;
        }
      }
      else
      {
        StaStart("Out of a closed date.", $"(Last closing date: {_salesRoom.srShowsCloseD.Date})");
      }
      return blnValid;
    }

    #endregion ValidateClosedDate

    #region EnableControls

    /// <summary>
    /// Habilita / deshabilita los controles
    /// </summary>
    /// <param name="blnEnable"></param>
    /// <param name="blnEnableShowType"></param>
    private void EnableControls(bool blnEnable, bool blnEnableShowType)
    {
      // Pestaña General
      brdGuestInfo.IsEnabled =
        brdShow.IsEnabled = blnEnable;

      // permitimos modificar el tipo de show
      rdbguTour.IsEnabled =
        rdbguInOut.IsEnabled =
          rdbguWalkOut.IsEnabled =
            rdbguCTour.IsEnabled =
              rdbguSaveProgram.IsEnabled =
                rdbguWithQuinella.IsEnabled = blnEnableShowType;

      brdTaxi.IsEnabled =
        brdPlaces.IsEnabled =
          brdOtherInfo.IsEnabled =
            brdGuest1.IsEnabled =
              brdGuest2.IsEnabled =
                brdAdditionalGuest.IsEnabled =
                  txtocWelcomeCopies.IsEnabled = blnEnable;

      // Pestaña Gifts, CC, Status & Comments
      brdGifts.IsEnabled =
        brdCreditCards.IsEnabled =
          brdGuestStatus.IsEnabled =
            brdComments.IsEnabled = blnEnable;

      // Pestaña Deposits & Salesmen
      brdDeposits.IsEnabled =
        brdDepositSale.IsEnabled =
          brdSalesmen.IsEnabled = blnEnable;
    }

    #endregion EnableControls

    #region LoadCombos

    /// <summary>
    /// Metodo que carga los datos a los combos correspondientes
    /// </summary>
    /// <history>
    /// [vipacheco] 02/Mayo/2016 Created
    /// </history>
    private void LoadCombos()
    {
      // Cargamos los catalogs en los combos correspondientes
      _dsCurrencies.Source = frmHost._lstCurrencies; // Monedas
      _dsPaymentTypes.Source = frmHost._lstPaymentsType; // Formas de Pago
      _dsPersonnelPR.Source = frmHost._lstPersonnelPR; // PR's
      _dsPersonnelLiner.Source = frmHost._lstPersonnelLINER; // Liner's
      _dsPersonnelCloser.Source = frmHost._lstPersonnelCLOSER; // Closer's
      _dsPersonnelExitcloser.Source = frmHost._lstPersonnelCLOSEREXIT; // Exit Closer's
      _dsPersonnelPodium.Source = frmHost._lstPersonnelPODIUM; // Podium
      _dsPersonnelVlo.Source = frmHost._lstPersonnelVLO; // Verificador Legal
      _dsPersonnelHostsentry.Source = frmHost._lstPersonnelHOSTENTRY; // Host de llegada
      _dsPersonnelHostsgifts.Source = frmHost._lstPersonnelHOSTGIFTS; // Host de regalos
      _dsPersonnelHostsexit.Source = frmHost._lstPersonnelHOSTEXIT; // Host de salida
      _dsHotels.Source = frmHost._lstHotel; // Hoteles
      _dsLanguajes.Source = frmHost._lstLanguaje; //Idiomas
      _dsTeamsSalesMen.Source = frmHost._lstTeamSalesMen; // Equipos de vendefores
      _dsAgencies.Source = frmHost._lstAgencies; // Agencias
      _dsMaritalStatus.Source = frmHost._lstMaritalStatus;  // Estado marital
      _dsCountries.Source = frmHost._lstCountries; // Ciudades
      _dsCreditCardTypes.Source = frmHost._lstCreditCardTypes; // Tipos Tarjetas Credito
      _dsGuestStatusTypes.Source = frmHost._lstGuestStatusTypes; //Tipos de Status
      _dsDisputeStatus.Source = frmHost._lstDisputeStatus; // Estatus de disputas
      _dsPaymentPlaces.Source = frmHost._lstPaymentPlaces; // Lugares de Pago
      _dsGifts.Source = frmHost._lstGifts; //Regalos

      _dsPersonnel.Source = frmHost._lstPersonnel;
    }

    #endregion LoadCombos

    #region LoadRecord

    /// <summary>
    /// Carga los datos del show
    /// </summary>
    /// <history>
    /// [aalcocer] 03/08/2016 Created
    /// </history>
    private async Task LoadRecord()
    {
      busyIndicator.IsBusy = _blnLoading = true;

      var lstTasks = new List<Task>();

      lstTasks.Add(Task.Run(async () =>
      {
        // cargamos los datos del huesped
        GuestObj = await BRGuests.GetGuest(_guestCurrent);
        _guestShow.CloneGuest = ObjectHelper.CopyProperties(GuestObj);
      }));

      lstTasks.Add(Task.Run(async () =>
      {
        //cargamos los regalos de invitacion
        var result = await BRInvitsGifts.GetInvitsGiftsByGuestID(_guestCurrent);
        InvitationGiftList = new ObservableCollection<InvitationGift>(result);
        _guestShow.CloneInvitationGiftList = ObjectHelper.CopyProperties(result);
      }));

      lstTasks.Add(Task.Run(async () =>
      {
        //cargamos los depositos
        var result = await BRBookingDeposits.GetBookingDeposits(_guestCurrent);
        BookingDepositList = new ObservableCollection<BookingDeposit>(result);
        _guestShow.CloneBookingDepositList = ObjectHelper.CopyProperties(result);
      }));

      lstTasks.Add(Task.Run(async () =>
      {
        //cargamos las tarjetas de credito
        var result = await BRGuestCreditCard.GetGuestCreditCard(_guestCurrent);
        GuestCreditCardList = new ObservableCollection<GuestCreditCard>(result);
        _guestShow.CloneGuestCreditCardList = ObjectHelper.CopyProperties(result);
      }));

      lstTasks.Add(Task.Run(async () =>
      {
        //cargamos las tarjetas de credito
        var result = await BRGuests.GetAdditionalGuest(_guestCurrent);
        GuestAdditionalList = new ObservableCollection<Guest>(result);
        _guestShow.CloneAdditionalGuestList = ObjectHelper.CopyProperties(result);
      }));

      await Task.WhenAll(lstTasks);

      if (_guestObj.guShow && _guestObj.guShowD != null)
        chkguDirect.IsEnabled = true;

      // establecemos el numero de copias
      OcWelcomeCopies = frmHost._configuration.ocWelcomeCopies;

      // habilitamos / deshabilitamos la invitacion externa
      EnableOutsideInvitation();
      busyIndicator.IsBusy = _blnLoading = false;
    }

    #endregion LoadRecord

    #region StaStart

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [aalcocer] 05/08/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    /// <param name="strToolTip"></param>
    private void StaStart(string message, string strToolTip = null)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;

      //si se envio un Tool Tip
      if (string.IsNullOrWhiteSpace(strToolTip))
        lblStatusBarMessage.ToolTip = strToolTip;
    }

    #endregion StaStart

    #region StaEnd

    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    /// [aalcocer] 05/08/2016 Created
    /// </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      lblStatusBarMessage.ToolTip = null;
    }

    #endregion StaEnd

    #region GetSalesmen

    /// <summary>
    /// Obtiene los vendedores
    /// </summary>
    /// <history>
    /// [aalcocer] 05/08/2016 Created
    /// </history>
    private void GetSalesmen()
    {
      var salesmen = new List<string>
      {
        _guestObj.guLiner1,
        _guestObj.guLiner2,
        _guestObj.guCloser1,
        _guestObj.guCloser2,
        _guestObj.guCloser3,
        _guestObj.guExit1,
        _guestObj.guExit2
      };

      salesmen = salesmen.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
      //Agregamos los datos del los SaleMen
      _showSalesmanList = new List<ShowSalesman>();
      salesmen.ForEach(s =>
      {
        PersonnelShort personnelShort = frmHost._lstPersonnel.First(p => p.peID == s);
        var showSalesman = new ShowSalesman
        {
          Guest = _guestObj,
          shgu = _guestCurrent,
          shUp = true,
          shpe = personnelShort.peID,
          Personnel = new Personnel
          {
            peID = personnelShort.peID,
            peN = personnelShort.peN
          }
        };
        _showSalesmanList.Add(showSalesman);
      });
    }

    #endregion GetSalesmen

    #region Validate

    /// <summary>
    /// Valida los datos
    /// </summary>
    /// <history>
    /// [aalcocer]  10/08/2016 Created.
    /// </history>
    private async Task<bool> Validate()
    {
      var blnValid = true;

      // validamos quien hizo el cambio y su contraseña
      if (!ValidateHelper.ValidateChangedBy(txtChangedBy, txtPwd))
        blnValid = false;

      //validamos los datos generales
      else if (!await ValidateGeneral())
      {
        blnValid = false;
        tabGeneral.IsSelected = true;
      }
      // validamos la informacion adicional, regalos, tarjetas de credito y los estatus de invitados
      else if (!ValidateOtherInfoGiftsCreditCardsGuestStatus())
      {
        blnValid = false;
        tabOtherInfoGiftsCreditCardsGuestStatus.IsSelected = true;
      }
      // validamos los depositos y vendedores
      else if (!ValidateDepositsSalesmen())
      {
        blnValid = false;
        tabDepositsSalesmen.IsSelected = true;
      }
      //validamos que los datos del show existan
      else if (!await ValidateExist())
        blnValid = false;

      return blnValid;
    }

    #endregion Validate

    #region ValidateGeneral

    /// <summary>
    /// Valida los datos generales
    /// </summary>
    /// <history>
    /// [aalcocer]  10/08/2016 Created.
    /// </history>
    private async Task<bool> ValidateGeneral()
    {
      var blnValid = true;

      // validamos la fecha de show
      if (_guestObj.guShow && !string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkShows, string.Empty, false, showMessage: true)))
        blnValid = false;

      //validamos que la fecha de show no este en una fecha cerrada
      else if (!Common.ValidateCloseDate(EnumEntities.Shows, ref dtpguShowD, _salesRoom.srShowsCloseD, pCondition: _guestObj.guShow))
      {
        ValidateClosedDate(false);
        blnValid = false;
      }

      // validamos que indique si presento invitacion
      else if (_guestObj.guShow && _enumProgram == EnumProgram.Outhouse
        && !string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkPresentedInvitation, string.Empty, false, showMessage: true)))
        blnValid = false;

      // validamos el tipo de show
      else if (!ValidateShowType())
        blnValid = false;

      //validamos que el folio de la invitacion outhouse exista y que no haya sido usada
      else if (_enumProgram == EnumProgram.Outhouse && !InvitationValidationRules.ValidateFolio(_guestObj, _enumProgram, txtguOutInvitNum, brdSearchReservation))
        blnValid = false;

      // validamos el folio de la reservacion inhouse
      else if (_enumProgram == EnumProgram.Inhouse && !InvitationValidationRules.ValidateFolio(_guestObj, _enumProgram, txtguHReservID, brdSearchReservation))
        blnValid = false;

      // validamos la fecha de llegada y la fecha de salida
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkCheckInOut, string.Empty, false, showMessage: true)))
        blnValid = false;

      // validamos el estado civil, apellido y nombre
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdGuest1, string.Empty, false, showMessage: true)))
        blnValid = false;

      // validamos el estado civil de su acompañante
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdGuest2, string.Empty, false, showMessage: true)))
        blnValid = false;

      // validamos la locacion
      else if (!ValidateLocation())
      {
        txtguloInvit.Focus();
        blnValid = false;
      }

      // validamos si la invitacion es una quiniela split
      else if (!await ValidateQuinellaSplit())
        blnValid = false;

      // validamos el show
      else if (!ValidateShow())
        blnValid = false;


      /*
      // TODO validamos los huespedes adicionales
      else if (!mGuestsAdditional.Validate(basCheckBox.GetValue(chkguQuinella), txtguID.Text)
        blnValid = false;
        */

      return blnValid;
    }



    #endregion ValidateGeneral

    #region ValidateOtherInfoGiftsCreditCardsGuestStatus

    /// <summary>
    /// Valida la informacion adicional, regalos, tarjetas de credito y los estatus
    /// </summary>
    /// <history>
    /// [aalcocer]  11/08/2016 Created.
    /// </history>
    private bool ValidateOtherInfoGiftsCreditCardsGuestStatus()
    {
      var blnValid = true;

      // validamos el pais, la agencia, el hotel y el numero de habitacion
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdOtherInfo, string.Empty, false, showMessage: true)))
        blnValid = false;

      // validamos el numero de bookings
      else if (!ValidateHelper.ValidateNumber(_guestObj.guRoomsQty, 1, 9, "books quantity"))
        blnValid = false;

      // validamos el pax
      else if (!ValidateHelper.ValidateNumber(_guestObj.guPax, 0.1m, 1000, "Pax number"))
        blnValid = false;

      /*
      // TODO validamos los regalos
      else if (!mInvitsGifts.Validate )
          blnValid = false;

      //TODO validamos las tarjetas de credito
      else if (!mCreditCards.Validate )
          blnValid = false;
           */
      // validamos los estatus de invitados
      else if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdGuestStatus, string.Empty, false, showMessage: true)))
        blnValid = false;

      return blnValid;
    }

    #endregion ValidateOtherInfoGiftsCreditCardsGuestStatus

    #region ValidateDepositsSalesmen

    /// <summary>
    /// Valida los depositos y vendedores
    /// </summary>
    /// <history>
    /// [aalcocer]  11/08/2016 Created.
    /// </history>
    private bool ValidateDepositsSalesmen()
    {
      var blnValid = true;

      //TODO validamos los depositos
      //if (false/*!mDeposits.Validate()*/)
      //  blnValid = false;

      // validamos el PR 1
      //else
      if (_guestObj.guSelfGen && !string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkPR1, string.Empty, false, showMessage: true)))
        blnValid = false;

      // validamos el equipo de vendedores si es un Self Gen
      else if (_guestObj.guSelfGen && !string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkTeam, string.Empty, false, showMessage: true)))
        blnValid = false;

      return blnValid;
    }

    #endregion ValidateDepositsSalesmen

    #region ValidateLocation

    /// <summary>
    /// Valida la locacion
    /// </summary>
    /// <history>
    /// [aalcocer]  11/08/2016 Created.
    /// </history>
    private bool ValidateLocation()
    {
      var blnValid = true;

      // validamos que se haya ingresado la locacion
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdPlaces, string.Empty, false, showMessage: true)))
        blnValid = false;

      // validamos que la locacion exista
      else if (!txtguloInvit.IsReadOnly)
      {
        // localizamos la locacion
        var location = frmHost._lstLocations.FirstOrDefault(lo => lo.loID == _guestObj.guloInvit);
        // si encontramos la locacion
        if (location != null)
        {
          // establecemos el Lead Source de la locacion
          txtguls.Text = location.lols;

          // habilitamos / deshabilitamos la invitacion externa
          EnableOutsideInvitation();
        }
        else
        {
          UIHelper.ShowMessage("Invalid Location ID.", MessageBoxImage.Exclamation);
          blnValid = false;
        }
      }

      return blnValid;
    }

    #endregion ValidateLocation

    #region EnableOutsideInvitation

    /// <summary>
    /// Habilita / deshabilita la invitacion externa
    /// </summary>
    /// <history>
    /// [aalcocer] 08/08/2016 Created
    /// </history>
    private void EnableOutsideInvitation()
    {
      string program = frmHost._lstLeadSources.First(x => x.lsID == _guestObj.guls).lspg;
      _enumProgram = EnumToListHelper.GetList<EnumProgram>().Single(x => x.Value == program).Key;

      bool blnIsInHouse = _enumProgram == EnumProgram.Inhouse;

      // Invitacion externa
      txtguOutInvitNum.IsEnabled =
        // Fecha de llegada
        dtpguCheckInD.IsEnabled =
          // Fecha de salida
          dtpguCheckOutD.IsEnabled = !blnIsInHouse;

      // si es una invitacion inhouse y no tiene un folio de reservacion definido, permitimos definirlo
      brdSearchReservation.IsEnabled = blnIsInHouse && string.IsNullOrWhiteSpace(_guestObj.guHReservID);

      // si la locacion es In House
      if (blnIsInHouse)
        txtguOutInvitNum.Text = string.Empty;

      btnAddGuestAdditional.Visibility = (blnIsInHouse) ? Visibility.Collapsed : Visibility.Visible;

    }

    #endregion EnableOutsideInvitation

    #region ValidateExist

    /// <summary>
    /// Valida que los datos del show existan
    /// </summary>
    /// <history>
    /// [aalcocer] 11/08/2016 Created
    /// </history>
    private async Task<bool> ValidateExist()
    {
      var validateExist = await BRShows.GetValidateShow(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), _guestObj);

      if (string.IsNullOrEmpty(validateExist.Focus)) return true;

      //Desplegamos el mensaje de error
      UIHelper.ShowMessage(validateExist.Message);
      //Establecemos el foco en el control que tiene el error

      switch (validateExist.Focus)
      {
        case "ChangedBy":
          txtChangedBy.Focus(); break;
        case "Password":
          txtPwd.Focus(); break;
        case "SalesRoom":
          txtgusr.Focus(); break;
        case "Agency":
          cmbguag.Focus(); break;
        case "Country":
          cmbguco.Focus(); break;
        case "PR1":
          cmbguPRInvit1.Focus(); break;
        case "PR2":
          cmbguPRInvit2.Focus(); break;
        case "PR3":
          cmbguPRInvit3.Focus(); break;
        case "Liner1":
          cmbguLiner1.Focus(); break;
        case "Liner2":
          cmbguLiner2.Focus(); break;
        case "Closer1":
          cmbguCloser1.Focus(); break;
        case "Closer2":
          cmbguCloser2.Focus(); break;
        case "Closer3":
          cmbguCloser3.Focus(); break;
        case "Exit1":
          cmbguExit1.Focus(); break;
        case "Exit2":
          cmbguExit2.Focus(); break;
        case "Podium":
          cmbguPodium.Focus(); break;
        case "VLO":
          cmbguVLO.Focus(); break;
        case "EntryHost":
          cmbguEntryHost.Focus(); break;
        case "GiftsHost":
          cmbguGiftsHost.Focus(); break;
        case "ExitHost":
          cmbguExitHost.Focus(); break;
      }

      // establecemos el foco en la pestaña del control que tiene el error
      switch (validateExist.Focus)
      {
        // Sin pestaña
        case "ChangedBy":
        case "Password":
          //OK
          break;

        // Pestaña de datos generales
        case "SalesRoom":
          tabGeneral.IsSelected = true; break;

        // Pestaña de informacion adicional
        case "Country":
        case "Agency":
          tabOtherInfoGiftsCreditCardsGuestStatus.IsSelected = true;
          break;

        // Pestaña de depositos y vendedores
        default:
          tabDepositsSalesmen.IsSelected = true;
          break;
      }

      return false;
    }



    #endregion ValidateExist

    #region ValidateQuinellaSplit

    /// <summary>
    /// Valida si es una invitacion quiniela split
    /// </summary>
    /// <history>
    /// [aalcocer] 11/08/2016 Created
    /// </history>
    private async Task<bool> ValidateQuinellaSplit()
    {
      var blnValid = true;
      // si es una quiniela split
      if (_guestObj.guQuinellaSplit)
      {
        // validamos la invitacion principal
        blnValid = await ValidateMainInvitation(true);
      }
      return blnValid;
    }

    #region ValidateMainInvitation

    /// <summary>
    /// Valida la invitacion principal
    /// </summary>
    /// <history>
    /// [aalcocer] 13/08/2016 Created
    /// </history>
    private async Task<bool> ValidateMainInvitation(bool blnRequired)
    {
      // si tiene invitacion principal
      if (_guestObj.guMainInvit != null)
      {
        // si la invitacion principal no es la invitacion actual
        if (_guestObj.guMainInvit != _guestObj.guID)
        {
          // obtenemos el nombre del huesped
          txtMainInvitName.Text = await Common.GetGuestName(_guestObj.guMainInvit);

          //si no se encontro el huesped
          if (!string.IsNullOrWhiteSpace(txtMainInvitName.Text)) return true;
          txtMainInvitName.Focus();
        }
        // si la invitacion principal es la invitacion actual
        else
        {
          UIHelper.ShowMessage("Main Invitation should not be the current.");
          txtguMainInvit.Focus();
        }
      }
      // si no tiene invitacion principal
      else
      {
        txtMainInvitName.Text = string.Empty;
        if (!blnRequired) return true;
        UIHelper.ShowMessage("Main Invitation not specified.");
        txtguMainInvit.Focus();
      }
      return false;
    }

    #endregion ValidateMainInvitation

    #endregion ValidateQuinellaSplit

    #region ValidateShow

    /// <summary>
    /// Valida el show
    /// </summary>
    /// <history>
    /// [aalcocer] 11/08/2016 Created
    /// </history>
    private bool ValidateShow()
    {
      var blnValid = true;

      // si tiene ingresado algun vendedor
      if (new List<string>
      {
        _guestObj.guLiner1,
        _guestObj.guLiner2,
        _guestObj.guCloser1,
        _guestObj.guCloser2,
        _guestObj.guCloser3,
        _guestObj.guExit1,
        _guestObj.guExit2
      }.Any(s => !string.IsNullOrWhiteSpace(s)))
      {
        // validamos que sea show
        if (!_guestObj.guShow)
        {
          UIHelper.ShowMessage("This case must be ''Show'' because it has defined salesmen");
          chkguShow.Focus();
          blnValid = false;
        }

        // validamos que sea tour, walk out, tour de cortesia o tour de rescate
        else if (!_guestObj.guTour && !_guestObj.guWalkOut && !_guestObj.guCTour && !_guestObj.guSaveProgram)
        {
          if (UIHelper.ShowMessage(
            "This case must be ''Tour'', ''Walk Out'', ''Courtesy Tour'' or ''Save Tour'' because it has defined salesmen.Save anyway ? ",
            MessageBoxImage.Question) == MessageBoxResult.No)
            blnValid = false;
        }
      }
      // si no tiene ingresado ningun vendedor
      else
      {
        // validamos que no sea tour
        if (_guestObj.guTour)
        {
          if (UIHelper.ShowMessage("This case not must be ''Tour'' because it has not defined salesmen. Save anyway?", MessageBoxImage.Question) == MessageBoxResult.No)
          {
            rdbguTour.Focus();
            blnValid = false;
          }
        }
        // validamos que no sea walk out
        else if (_guestObj.guWalkOut)
        {
          if (UIHelper.ShowMessage("This case not must be ''Walk Out'' because it has not defined salesmen. Save anyway?", MessageBoxImage.Question) == MessageBoxResult.No)
          {
            rdbguWalkOut.Focus();
            blnValid = false;
          }
        }
        // validamos que no sea tour de cortesia
        else if (_guestObj.guCTour)
        {
          if (UIHelper.ShowMessage("This case not must be ''Courtesy Tour'' because it has not defined salesmen. Save anyway?", MessageBoxImage.Question) == MessageBoxResult.No)
          {
            rdbguCTour.Focus();
            blnValid = false;
          }
        }
        // validamos que no sea tour de rescate
        else if (_guestObj.guSaveProgram)
        {
          if (UIHelper.ShowMessage("This case not must be ''Save Tour'' because it has not defined salesmen. Save anyway?", MessageBoxImage.Question) == MessageBoxResult.No)
          {
            rdbguSaveProgram.Focus();
            blnValid = false;
          }
        }
      }

      // si es valido y es show
      if (blnValid && _guestObj.guShow)
      {
        // validamos el numero de shows
        if (!ValidateHelper.ValidateNumber(_guestObj.guShowsQty, 1, 9, "shows quantity"))
        {
          blnValid = false;
        }
      }

      return blnValid;
    }



    #endregion ValidateShow

    #region ValidateShowType

    /// <summary>
    /// Valida el tipo de show
    /// </summary>
    /// <history>
    /// [aalcocer] 11/08/2016 Created
    /// </history>
    private bool ValidateShowType()
    {
      // si es show
      if (!_guestObj.guShow) return true;
      // si no tiene un tipo de show
      if (_guestObj.guTour || _guestObj.guInOut || _guestObj.guWalkOut || _guestObj.guCTour || _guestObj.guSaveProgram || _guestObj.guWithQuinella) return true;
      UIHelper.ShowMessage("Please specify the show type");
      rdbguTour.Focus();
      return false;
    }

    #endregion ValidateShowType

    #region Save

    /// <summary>
    /// Guarda los datos del show
    /// </summary>
    /// <history>
    /// [aalcocer] 11/08/2016 Created
    /// </history>
    private async void Save()
    {
      // indicamos que ya se le dio el show al invitado
      if (_guestObj.guTimeInT != null && !_guestObj.guShow)
        chkguShow.IsChecked = true;

      // checamos si los datos de Self Gen estan correctamente llenados
      await CheckSelfGen();

      // establecemos el deposito
      SetDeposit();

      // definimos al huesped interval
      chkguInterval.IsChecked = true;

      // si es una invitacion outside y esta habilitado el uso de perfiles de Opera
      if (_enumProgram == EnumProgram.Outhouse && ConfigHelper.GetString("ReadOnly").ToUpper().Equals("TRUE"))
      {
        //TODO guardamos el perfil en Opera
        //WirePRHelper.SaveProfileOpera Me, mData
      }
      //TODO guardar
      // guardamos el huesped

      _guestShow.Guest = GuestObj;
      _guestShow.InvitationGiftList = InvitationGiftList.OfType<InvitationGift>().ToList();
      _guestShow.BookingDepositList = BookingDepositList.OfType<BookingDeposit>().ToList();
      _guestShow.GuestCreditCardList = GuestCreditCardList.OfType<GuestCreditCard>().ToList();
      _guestShow.GuestStatusTypes = new List<GuestStatusType>() { (GuestStatusType)cmbGuestStatus.SelectedItem };
      _guestShow.AdditionalGuestList = GuestAdditionalList.OfType<Guest>().ToList();
      // guardamos el historico del huesped



    }



    #endregion Save

    #region SetLinerType
    /// <summary>
    /// Establece el tipo del liner 1
    /// </summary>
    /// <history>
    /// [aalcocer] 16/08/2016 Created
    /// </history>
    private async Task SetLinerType()
    {
      var dtmDate = _guestObj.guShowD != null ? Convert.ToDateTime(_guestObj.guShowD) : Convert.ToDateTime(_guestObj.guBookD);

      // establecemos el tipo de liner
      PostShort postShort = await BRPosts.GetPersonnelPostByDate(_guestObj.guLiner1, dtmDate);

      switch (postShort.Post)
      {
        // Liner
        case "LINER":
          rdbguLiner1Type.IsChecked = true;
          break;
        // Front To Middle
        case "FTM":
          rdbguLiner2Type.IsChecked = true;
          break;
        // Front To Back
        case "FTB":
        case "CLOSER":
        case "EXIT":
          rdbguLiner3Type.IsChecked = true;
          break;
        // para los que no tienen puesto todavia, se les establece como liners
        default:
          rdbguLiner1Type.IsChecked = true;
          break;
      }
    }

    #endregion

    #region SetSelfGen
    /// <summary>
    /// Determina si es un Self Gen
    /// </summary>
    /// <history>
    /// [aalcocer] 16/08/2016 Created
    /// </history>
    private async Task SetSelfGen(string strSalesmenID)
    {
      // si es un Self Gen
      if (await BRPersonnel.IsFrontToMiddle(strSalesmenID))
        chkguSelfGen.IsChecked = false;
      else
        chkguSelfGen.IsChecked = false;

    }


    #endregion

    #region CheckSelfGen
    /// <summary>
    /// Checa si es un Self Gen y de ser asi avisa al usuario
    /// </summary>
    /// <history>
    /// [aalcocer] 15/08/2016 Created
    /// </history>
    private async Task CheckSelfGen()
    {
      // si es un Self Gen
      if (!string.IsNullOrWhiteSpace(_guestObj.guLiner1) && await BRPersonnel.IsFrontToMiddle(_guestObj.guLiner1))
      {
        // si no esta como Self Gen
        if (!_guestObj.guSelfGen &&
          UIHelper.ShowMessage($"This case must be Self Gen. {Environment.NewLine}Mark the checkbox ''SELF GEN''?", MessageBoxImage.Question) == MessageBoxResult.Yes)
          chkguSelfGen.IsChecked = true;
      }
      // si no es un Self Gen
      else
      {
        //si no esta como Self Gen
        if (_guestObj.guSelfGen &&
            UIHelper.ShowMessage($"This case not must be Self Gen.. {Environment.NewLine}Unmark the checkbox ''SELF GEN''?", MessageBoxImage.Question) == MessageBoxResult.Yes)
          chkguSelfGen.IsChecked = false;
      }      
    }





    #endregion

    #region SetDeposit
    /// <summary>
    /// Establece el deposito
    /// </summary>
    /// <history>
    /// [aalcocer] 15/08/2016 Created
    /// </history>
    private void SetDeposit()
    {
      // establecemos el primer deposito
      _guestObj.guDeposit = _bookingDepositList.Where(d => d.bdAmount > 0).Select(d => d.bdAmount).FirstOrDefault();

      // si no tuvo deposito, establece el deposito quemado
      if (_guestObj.guDeposit == 0)
        _guestObj.guDeposit = _guestObj.guDepositTwisted;
    }
    #endregion

    #region SendEmail
    /// <summary>
    /// Envia un correo de nofiticacion indicando que un huesped se presento sin invitacion
    /// </summary>
    /// <history>
    /// [aalcocer] 17/08/2016 Created
    /// </history>
    private async void SendEmail()
    {
      //// si se presento sin invitacion y es Outhouse y No ha sido notificado
      if (!_guestObj.guShow || Convert.ToBoolean(_guestObj.guPresentedInvitation) ||
          Convert.ToBoolean(_guestObj.guNotifiedEmailShowNotInvited) || _enumProgram != EnumProgram.Outhouse) return;

      // obtenemos la sala de ventas
      var salesRoom = frmHost._lstSalesRoom.FirstOrDefault(s => s.srID == _guestObj.gusr);

      // obtenemos el Lead Source
      var leadSource = frmHost._lstLeadSources.FirstOrDefault(s => s.lsID == _guestObj.guls);

      // Obtenermos el PR1
      var personnel = frmHost._lstPersonnelPR.FirstOrDefault(p => p.peID == _guestObj.guPRInvit1);

      var cont = 1;
      var x = _bookingDepositList.ToList().Select(c => new
      {
        N = cont++,
        Deposit = $"$ {c.bdAmount:0.##}",
        Received = $"$ {c.bdReceived:0.##}",
        Currency = frmHost._lstCurrencies.FirstOrDefault(cu => cu.cuID == c.bdcu)?.cuN,
        PaymentType = frmHost._lstPaymentsType.FirstOrDefault(pt => pt.ptID == c.bdpt)?.ptN
      }).ToList();

      DataTable dtData = TableHelper.GetDataTableFromList(x);
      // Renombramos las columnas.
      dtData.Columns.Cast<DataColumn>().Where(t => t.ColumnName == "N").ToList().ForEach(t => t.ColumnName = "#");
      dtData.Columns.Cast<DataColumn>().Where(t => t.ColumnName == "PaymentType").ToList().ForEach(t => t.ColumnName = "Payment Type");


      // enviamos el correo electronico
      var res = clsEmail.SendMail(_guestObj, salesRoom, leadSource, personnel, dtData);

      //'Si la respuesta es diferente de vacio, mandamos el mensaje de error
      if (res == string.Empty)
      {
        _guestObj.guNotifiedEmailShowNotInvited = true;
        await BRGuests.SaveGuest(_guestObj);
      }
      else
      {
        UIHelper.ShowMessage(res, MessageBoxImage.Error);
      }


    }
    #endregion


    #endregion Metodos

    #region Eventos del formulario

    #region Window_Loaded

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      // Obtenemos la fecha actual
      _dateCurrent = frmHost.dtpServerDate;

      #region Build Reference to Resources XAML

      // Obtemos las referencias de los datasource a utilizar
      _dsMaritalStatus = (CollectionViewSource)FindResource("dsMaritalStatus");
      _dsAgencies = (CollectionViewSource)FindResource("dsAgencies");
      _dsCountries = (CollectionViewSource)FindResource("dsCountries");
      _dsLanguajes = (CollectionViewSource)FindResource("dsLanguajes");
      _dsHotels = (CollectionViewSource)FindResource("dsHotels");
      _dsCurrencies = (CollectionViewSource)FindResource("dsCurrencies");
      _dsPaymentTypes = (CollectionViewSource)FindResource("dsPaymentTypes");
      _dsTeamsSalesMen = (CollectionViewSource)FindResource("dsTeamsSalesMen");
      _dsPersonnel = (CollectionViewSource)FindResource("dsPersonnel");
      _dsPersonnelPR = (CollectionViewSource)FindResource("dsPersonnelPR");
      _dsPersonnelLiner = (CollectionViewSource)FindResource("dsPersonnelLiner");
      _dsPersonnelCloser = (CollectionViewSource)FindResource("dsPersonnelCloser");
      _dsPersonnelExitcloser = (CollectionViewSource)FindResource("dsPersonnelExitCloser");
      _dsPersonnelPodium = (CollectionViewSource)FindResource("dsPersonnelPodium");
      _dsPersonnelVlo = (CollectionViewSource)FindResource("dsPersonnelVlo");
      _dsPersonnelHostsentry = (CollectionViewSource)FindResource("dsPersonnelHostsEntry");
      _dsPersonnelHostsgifts = (CollectionViewSource)FindResource("dsPersonnelHostsGifts");
      _dsPersonnelHostsexit = (CollectionViewSource)FindResource("dsPersonnelHostsExit");
      _dsCreditCardTypes = (CollectionViewSource)FindResource("dsCreditCardTypes");
      _dsCreditCardTypes = (CollectionViewSource)FindResource("dsCreditCardTypes");
      _dsGuestStatusTypes = (CollectionViewSource)FindResource("dsGuestStatusTypes");
      _dsDisputeStatus = (CollectionViewSource)FindResource("dsDisputeStatus");
      _dsPaymentPlaces = (CollectionViewSource)FindResource("dsPaymentPlaces");
      _dsGifts = (CollectionViewSource)FindResource("dsGifts");

      #endregion Build Reference to Resources XAML

      //cargamos los combos
      LoadCombos();

      //si tiene permiso especial
      txtgusr.IsReadOnly = !App.User.HasPermission(EnumPermission.Show, EnumPermisionLevel.Special);

      // Verificamos la autentificación automatica
      if (App.User.AutoSign)
      {
        txtChangedBy.Text = App.User.User.peID;
        txtPwd.Password = App.User.User.pePwd;
      }

      //configuramos la clase de datos
      UIHelper.SetUpControls(new Guest(), this);

      //obtenemos la fechas de cierre
      _salesRoom = BRSalesRooms.GetSalesRoom(App.User.SalesRoom.srID);

      //cargamos los datos del show
      await LoadRecord();

      //mostramos la clave y el nombre del huesped en el titulo del formulario
      Title = $"{Title} - {_guestObj.guID}, {Common.GetFullName(_guestObj.guLastName1, _guestObj.guFirstName1)}";

      // si aun no es guardado como show y su fecha de booking no es del dia actual ni del dia siguiente, se impide imprimir
      if (!_guestObj.guShow && _guestObj.guBookD.HasValue && !DateHelper.IsInRangeDate(_guestObj.guBookD.Value, _dateCurrent, _dateCurrent.AddDays(1)))
        imgButtonPrint.IsEnabled = false;

      //  si el sistema esta en modo de solo lectura o el usuario tiene cuando mucho permiso de lectura
      // o si el show es de una fecha cerrada
      if (ConfigHelper.GetString("ReadOnly").ToUpper().Equals("TRUE") ||
        !App.User.HasPermission(EnumPermission.Show, EnumPermisionLevel.Standard) ||
        !ValidateClosedDate(true))
        imgButtonSave.IsEnabled = imgButtonPrint.IsEnabled = false;
      if (!App.User.HasPermission(EnumPermission.Show, EnumPermisionLevel.Standard) || !ValidateClosedDate(true)) { }
      imgButtonSave.IsEnabled =
        imgButtonPrint.IsEnabled = false;
      btnSearchGuestAdditional.IsEnabled = btnAddGuestAdditional.IsEnabled = false;
      dtgGuestAdditional.IsReadOnly = true;

      //Si de la BD el campo guNotifiedEmailShowNotInvited es null, lo ponemos en unChecked el control
      if (_guestObj.guNotifiedEmailShowNotInvited == null)
        chkguNotifiedEmailShowNotInvited.IsChecked = false;
    }


    #endregion Window_Loaded

    #region imgButtonCancel_OnMouseLeftButtonDown

    /// <summary>
    /// Cierra el formulario
    /// </summary>
    /// <history>
    /// [aalcocer] 08/16/2016 Created
    /// </history>
    private void imgButtonCancel_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Close();
    }

    #endregion imgButtonCancel_OnMouseLeftButtonDown

    #region btnIn_Click

    /// <summary>
    /// Establece la hora de llegada
    /// </summary>
    /// <history>
    /// [aalcocer] 08/16/2016 Created
    /// </history>
    private void btnIn_Click(object sender, RoutedEventArgs e)
    {
      if (_guestObj.guTimeInT == null)
        tpkguTimeInT.Value = DateTime.Now;
      chkguShow.IsChecked = true;
      chkguShow.IsEnabled = false;
    }

    #endregion btnIn_Click

    #region imgButtonLog_OnMouseLeftButtonDown

    /// <summary>
    /// Despliega el formulario del registro historico del invitado
    /// </summary>
    /// <history>
    /// [aalcocer] 08/16/2016 Created
    /// </history>
    private void imgButtonLog_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var frmGuestLog = new frmGuestLog(_guestCurrent) { Owner = this };
      frmGuestLog.ShowDialog();
    }

    #endregion imgButtonLog_OnMouseLeftButtonDown

    #region btnOut_Click

    /// <summary>
    /// Establece la hora de salida
    /// </summary>
    /// <history>
    /// [aalcocer] 08/16/2016 Created
    /// </history>
    private void btnOut_Click(object sender, RoutedEventArgs e)
    {
      if (_guestObj.guTimeOutT == null)
      {
        tpkguTimeOutT.Value = DateTime.Now;
        if (_guestObj.guTimeInT == null)
          tpkguTimeInT.Value = _guestObj.guTimeOutT;
      }
      chkguShow.IsChecked = true;
    }

    #endregion btnOut_Click

    #region imgButtonPrint_MouseLeftButtonDown

    /// <summary>
    /// Imprime el reporte GuestRegistration
    /// </summary>
    /// <history>
    /// [edgrodriguez] 25/Jul/2016 Created
    /// </history>
    private async void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      byte iMotive = 0;
      if (_guestObj.guReimpresion > 0)
      {
        frmReimpresionMotives _frmReimpresion = new frmReimpresionMotives()
        {
          ShowInTaskbar = false,
          Owner = this
        };
        if (!(_frmReimpresion.ShowDialog() ?? false)) return;

        iMotive = (byte)_frmReimpresion.LstMotives.SelectedValue;

        //Actualizamos el motivo de reimpresion.
        await BRReimpresionMotives.UpdateGuestReimpresionMotive(_guestObj.guID, iMotive);
      }
      else
      //Actualizamos el contador de reimpresion.
      {
        await BRReimpresionMotives.UpdateGuestReimpresionNumber(_guestObj.guID);
      }
      //Salvamos la informacion del show.
      //GuardarInfo();

      //Se imprime el reporte.
      var lstRptGuestRegistration = await BRGuests.GetGuestRegistration(_guestCurrent);
      if (lstRptGuestRegistration.Any())
      {
        var guestRegistration = (lstRptGuestRegistration[0] as List<RptGuestRegistration>).Select(c => new objRptGuestRegistrationIM(c)).FirstOrDefault();
        var guReg_Guest = (lstRptGuestRegistration[1] as List<RptGuestRegistration_Guests>)?.Select(c => new objRptGuestRegistrationGuestIM(c)).ToList() ?? new List<objRptGuestRegistrationGuestIM>();
        var guReg_Deposits = (lstRptGuestRegistration[2] as List<RptGuestRegistration_Deposits>)?.Select(c => new objRptGuestRegistrationDepositsIM(c)).ToList() ?? new List<objRptGuestRegistrationDepositsIM>();
        var guReg_Gifts = (lstRptGuestRegistration[3] as List<RptGuestRegistration_Gifts>)?.Select(c => new objRptGuestRegistrationGiftsIM(c)).ToList() ?? new List<objRptGuestRegistrationGiftsIM>();
        var guReg_Salesmen = (lstRptGuestRegistration[4] as List<RptGuestRegistration_Salesmen>)?.Select(c => new objRptGuestRegistrationSalesmenIM(c)).ToList() ?? new List<objRptGuestRegistrationSalesmenIM>();
        var guReg_CreditCard = (lstRptGuestRegistration[5] as List<RptGuestRegistration_CreditCards>)?.Select(c => new objRptGuestRegistrationCreditCardsIM(c)).ToList() ?? new List<objRptGuestRegistrationCreditCardsIM>();
        var guReg_Comments = (lstRptGuestRegistration[6] as List<RptGuestRegistration_Comments>)?.Select(c => new objRptGuestRegistrationCommentsIM(c)).ToList() ?? new List<objRptGuestRegistrationCommentsIM>();
        var rptGuestRegistration = new Reports.rptGuestRegistration();

        rptGuestRegistration.Database.Tables[0].SetDataSource(TableHelper.GetDataTableFromList(ObjectHelper.ObjectToList(guestRegistration)));
        rptGuestRegistration.Subreports["rptGuestRegistration_Guests.rpt"].SetDataSource(TableHelper.GetDataTableFromList(guReg_Guest));
        rptGuestRegistration.Subreports["rptGuestRegistration_Deposits.rpt"].SetDataSource(TableHelper.GetDataTableFromList(guReg_Deposits));
        rptGuestRegistration.Subreports["rptGuestRegistration_Gifts.rpt"].SetDataSource(TableHelper.GetDataTableFromList(guReg_Gifts));
        rptGuestRegistration.Subreports["rptGuestRegistration_CreditCards.rpt"].SetDataSource(TableHelper.GetDataTableFromList(guReg_CreditCard));
        rptGuestRegistration.Subreports["rptGuestRegistration_Salesmen.rpt"].SetDataSource(TableHelper.GetDataTableFromList(guReg_Salesmen));
        rptGuestRegistration.Subreports["rptGuestRegistration_Comments.rpt"].SetDataSource(TableHelper.GetDataTableFromList(guReg_Comments));

        CrystalReportHelper.SetLanguage(rptGuestRegistration, guestRegistration.gula);

        //Si es reimpresion reemplazamos los campos clave.
        if (guestRegistration.guReimpresion > 1)
        {
          var msgReimpresion = LanguageHelper.GetMessage(EnumMessage.msglblReimpresion);
          msgReimpresion = (string.IsNullOrEmpty(msgReimpresion)) ? "" : msgReimpresion.Replace("[grReimpresion]", guestRegistration.guReimpresion.ToString()).Replace("[rmN]", guestRegistration.rmN?.ToString() ?? "");
          (rptGuestRegistration.ReportDefinition.ReportObjects["lblReimpresion"] as TextObject).Text = msgReimpresion;
        }

        CrystalReportHelper.ShowReport(rptGuestRegistration, $"Guest Registration {_guestObj.guID.ToString()}", PrintDevice: EnumPrintDevice.pdScreen, numCopies: ((string.IsNullOrWhiteSpace(txtocWelcomeCopies.Text)) ? 1 : Convert.ToInt32(txtocWelcomeCopies.Text)));

        //Cerramos el Formulario.
        Close();
      }
    }

    #endregion imgButtonPrint_MouseLeftButtonDown

    #region btnShowsSalesmen_Click

    /// <summary>
    /// Despliega el formulario de vendedores para especificar los datos del show por vendedor
    /// </summary>
    /// <history>
    /// [aalcocer]  8/08/2016 Created.
    /// </history>
    private void btnShowsSalesmen_Click(object sender, RoutedEventArgs e)
    {
      //si es una secretaria
      if (App.User.HasRole(EnumRole.Secretary))
      {
        //Obtenermos los vendedores

        GetSalesmen();
        var salessalesmen = new frmShowsSalesmen(_guestCurrent, _showSalesmanList)
        { Owner = this };
        salessalesmen.ShowDialog();
      }
      else
      {
        UIHelper.ShowMessage("Access denied.");
      }
    }

    #endregion btnShowsSalesmen_Click

    #region imgButtonSave_MouseLeftButtonDown

    /// <summary>
    /// Permite guardar los cambios
    /// </summary>
    /// <history>
    /// [aalcocer]  8/08/2016 Created.
    /// </history>
    private async void imgButtonSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!await Validate()) return;
      Save();
      SendEmail();
      Close();
    }

    #endregion imgButtonSave_MouseLeftButtonDown

    #region chkguQuinellaSplit_Checked

    /// <summary>
    /// Habilita / deshabilita el editor de invitacion principal
    /// </summary>
    /// <history>
    /// [aalcocer]  11/08/2016 Created.
    /// </history>
    private void chkguQuinellaSplit_Checked(object sender, RoutedEventArgs e)
    {
      if (_guestObj.guQuinellaSplit)
        txtguMainInvit.IsEnabled = true;
      else
      {
        txtguMainInvit.IsEnabled = false;
        txtguMainInvit.Text = string.Empty;
        txtMainInvitName.Text = string.Empty;
      }
    }

    #endregion chkguQuinellaSplit_Checked

    #region chkguSelfGen_Checked

    /// <summary>
    /// Habilita / deshabilita el combo de equipo
    /// </summary>
    /// <history>
    /// [aalcocer]  11/08/2016 Created.
    /// </history>
    private void chkguSelfGen_Checked(object sender, RoutedEventArgs e)
    {
      if (_guestObj.guSelfGen)
        cmbguts.IsEnabled = true;
      else
      {
        cmbguts.IsEnabled = false;
        cmbguts.SelectedIndex = -1;
      }
    }

    #endregion chkguSelfGen_Checked

    #region chkguShow_Checked

    /// <summary>
    /// Show
    /// </summary>
    /// <history>
    /// [aalcocer]  11/08/2016 Created.
    /// </history>
    private void chkguShow_Checked(object sender, RoutedEventArgs e)
    {
      if (_blnLoading) return;

      if (!_guestObj.guShow)
      {
        dtpguShowD.Value = null;
        tpkguTimeInT.Value = null;

        // validamos que no se pueda desmarcar el show si tiene cupon de comida o venta
        if (!_guestObj.guMealTicket && !_guestObj.guSale) return;
        UIHelper.ShowMessage("You cannot uncheck the Show if there is a Meal Ticket or Sale.");
        chkguShow.IsChecked = true;
      }
      else if (_guestObj.guShowD == null)
      {
        dtpguShowD.Value = BRHelpers.GetServerDateTime();
        if (_guestObj.guTimeInT == null)
          tpkguTimeInT.Value = DateTime.Now;
      }
    }

    #endregion chkguShow_Checked

    #region Window_OnContentRendered
    private async void Window_OnContentRendered(object sender, EventArgs e)
    {
      if (_guestObj == null) return;
      // checamos si los datos de Self Gen estan correctamente llenados
      await CheckSelfGen();

      // checamos el equipo de vendedores si es un Self Gen
      if (_guestObj.guSelfGen && string.IsNullOrWhiteSpace(_guestObj.guts))
      {
        UIHelper.ShowMessage("Specify the Team.");
        cmbguts.Focus();
      }

      // establecemos el nombre de la invitacion principal
      await ValidateMainInvitation(false);
    }

    #endregion

    #region Window_IsKeyboarFocusedChanged

    /// <summary>
    ///   Verfica que teclas estan presionadas
    /// </summary>
    /// <history>
    ///   [aalcocer] 05/08/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(statusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(statusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(statusBarNum, Key.NumLock);
    }

    #endregion Window_IsKeyboarFocusedChanged

    #region Window_KeyDown

    /// <summary>
    ///   Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <history>
    ///   [aalcocer] 05/08/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(statusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(statusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(statusBarNum, Key.NumLock);
            break;
          }
      }
    }

    #endregion Window_KeyDown

    #region Control_KeyDown

    /// <summary>
    /// Si es un ComboBox funcionara nada mas cuando presionen la tecla eliminar para quitar el registro que esta actualmente seleccionado y dejarlo vacio
    /// </summary>
    /// <history>
    /// [aalcocer] 11/08/2016  Created.
    /// </history>
    private void Control_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Delete) return;

      switch (sender.GetType().Name)
      {
        case nameof(ComboBox):
          ((ComboBox)sender).SelectedIndex = -1;
          break;
        case nameof(DateTimePicker):
          ((DateTimePicker)sender).Value = null;
          ((DateTimePicker)sender).IsOpen = false;
          break;
      }
    }

    #endregion Control_KeyDown

    #region cmb_SelectionChanged
    /// <summary>
    /// Valida cuando se cambia de Item los Combox de los vendedores
    /// </summary>
    /// <history>
    /// [aalcocer] 17/08/2016 Created.
    /// </history>
    private async void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var cmb = sender as ComboBox;
      if (cmb?.SelectedValue == null || _blnLoading) return;

      if (sender.Equals(cmbguLiner1) && !string.IsNullOrWhiteSpace(_guestObj.guLiner1))
      {
        // establecemos el tipo del liner 1
        await SetLinerType();

        // determinamos si es un Self Gen
        await SetSelfGen(_guestObj.guLiner1);
      }

      var personnelValidando = cmb.Name.Substring(5).ToUpper();
      var lstcmb = UIHelper.GetChildParentCollection<ComboBox>(brdSalesmen);
      if (cmb.SelectedIndex == -1) return;
      foreach (var item in lstcmb)
      {
        var personnelFound = item.Name.Substring(5).ToUpper();
        //Validacion que sirve para saber si no es mismo ComboBox que se esta validando, PR1 == PR1 
        if (personnelFound == personnelValidando) continue;
        //Validacion que sirve para siempre se compare los del mismo rol PR == PR
        if (personnelValidando.Trim('1', '2', '3') != personnelFound.Trim('1', '2', '3')) continue;
        //Ahora como ya se sabe que no es mismo ComboBox y es el mismo rol entonces ya podemos hacer 
        //la validacion de ser el mismo texto no permitimos que se seleccione
        var rol = personnelValidando.Trim('1', '2', '3');
        if (cmb.SelectedValue.ToString() != item.Text) continue;
        UIHelper.ShowMessage(
          $"Please select another person. \nThe person with the Id: {item.Text} already selected with the role of {rol}");
        cmb.SelectedIndex = -1;
        break;
      }
    }


    #endregion

    #region txtocWelcomeCopies_PreviewTextInput
    /// <summary>
    /// Valida que solo se pueda ingresar numeros y que sean del 1 al 4 en  TxtocWelcomeCopies 
    /// </summary>
    /// <history>
    /// [aalcocer] 17/08/2016 Created
    /// </history>
    private void txtocWelcomeCopies_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
      if (e.Handled) return;
      if (Convert.ToInt16(e.Text) < 1)
      {
        UIHelper.ShowMessage("The copies number can not be smaller than 1.");
        e.Handled = true;
      }
      else if (Convert.ToInt16(e.Text) > 4)
      {
        UIHelper.ShowMessage("The copies number can not be greater than 4.");
        e.Handled = true;
      }
    }
    #endregion

    #region TxtocWelcomeCopies_OnLostFocus
    /// <summary>
    /// Pone por Default el valor 1 en  TxtocWelcomeCopies al perder el foco y no tiene valor
    /// </summary>
    /// <history>
    /// [aalcocer] 17/08/2016 Created
    /// </history>
    private void TxtocWelcomeCopies_OnLostFocus(object sender, RoutedEventArgs e)
    {
      var textBox = (TextBox)sender;
      if (textBox?.Text == string.Empty)
        textBox.Text = "1";
    }
    #endregion

    #region GuestAdditional

    #region Eventos del GRID GuestAdditional

    #region BeginningEdit

    /// <summary>
    /// Se ejecuta antes de que entre en modo edicion alguna celda
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private void dtgGuestAdditional_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(txtgusr.Text) && e.Column.SortMemberPath == "guID")
      {
        UIHelper.ShowMessage("First select a Sales Room ", MessageBoxImage.Warning, "Intelligence Marketing");

        e.Cancel = true;
        _hasError = true;
        _isCellCancel = true;
        txtgusr.Focus();
      }
      else
      {
        _hasError = false;
        _isCellCancel = false;
      }

      //Si el grid no esta en modo edicion, permite hacer edicion.
      if (!GridHelper.IsInEditMode(dtgGuestAdditional) && !_hasError)
      {
        dtgGuestAdditional.BeginningEdit -= dtgGuestAdditional_BeginningEdit;
        //Obtenemos la celda que vamos a validar
        _IGCurrentCellGuestAdditional = dtgGuestAdditional.CurrentCell;
        //Hacemos la primera validacion
        InvitationValidationRules.dtgGuestAdditional_StartEdit(ref _IGCurrentCellGuestAdditional, dtgGuestAdditional, ref _hasError);
        //Si tuvo algun error de validacion cancela la edicion de la celda.
        e.Cancel = _hasError;
        dtgGuestAdditional.BeginningEdit += dtgGuestAdditional_BeginningEdit;
      }
      //Si ya se encuenta en modo EDIT cancela la edicion, para no salirse de la celda sin hacer Commit antes
      else
      {
        e.Cancel = true;
      }
    }

    #endregion BeginningEdit

    #region PreparingCellForEdit

    /// <summary>
    /// Se ejecuta cuando la celda entra en modo edicion
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private void dtgGuestAdditional_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
      //Sirve para agregar el Focus a las celdas
      Control ctrl = e.EditingElement as Control;
      ctrl?.Focus();
    }

    #endregion PreparingCellForEdit

    #region CellEditEnding

    /// <summary>
    /// Se ejecuta cuando la celda en edicion pierde el foco
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private void dtgGuestAdditional_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      //Si paso las validaciones del preparingCellForEdit
      if (_hasError) return;
      //Si viene en modo Commit
      if (e.EditAction == DataGridEditAction.Commit)
      {
        _isCellCommitGuestAdditional = (Keyboard.IsKeyDown(Key.Enter));
        //esta bandera se pone en falso por que No se ha cancelado la edicion de la celda
        _isCellCancel = false;
        //Obtenemos el Objeto
        Guest guestAdditionalRow = e.Row.Item as Guest;
        Guest guestAdditional = AsyncHelper.RunSync(() => BRGuests.GetGuest(guestAdditionalRow.guID));//await BRGuests.GetGuest(guestAdditionalRow.guID);
        var NotValid = AsyncHelper.RunSync(() => InvitationValidationRules.dtgGuestAdditional_ValidateEdit(GuestObj, guestAdditional, _IGCurrentCellGuestAdditional));
        //Si Paso las validaciones
        if (!NotValid)
        {
          e.Row.Item = guestAdditional;
        }
        //Si fallaron las validaciones del AfterEdit se cancela la edicion de la celda.
        else
        {
          e.Cancel = true;
          _isCellCancel = true;
        }
      }
      //Si entra en modo Cancel Se enciende esta bandera ya que servira en RowEditEnding
      else
      {
        _isCellCancel = true;
      }
    }

    #endregion CellEditEnding

    #region RowEditEnding

    /// <summary>
    /// Se ejecuta cuando la fila pierde el foco, o termina la edicion (Commit o Cancel)
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/08/2016  Created.
    /// </history>
    private void dtgGuestAdditional_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      DataGrid dtg = sender as DataGrid;

      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCommitGuestAdditional)
        {
          _isCellCommitGuestAdditional = false;
          e.Cancel = true;
        }
        else if (Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.Tab))
        {
          _isCellCommitGuestAdditional = false;
          e.Cancel = !AsyncHelper.RunSync(() => InvitationValidationRules.ValidateAdditionalGuest(GuestObj, (Guest)e.Row.Item, true)).Item1;
          GridHelper.SelectRow(dtgGuestAdditional, e.Row.GetIndex(), blnEdit: true);
        }
        else
        {
          e.Cancel = true;
        }
        //  if (_isCellCancel)
        //{
        //  dtg.RowEditEnding -= dtgGifts_RowEditEnding;
        //  dtg.CancelEdit();
        //  dtg.RowEditEnding -= dtgGifts_RowEditEnding;
        //}
      }
    }

    #endregion RowEditEnding

    #endregion Eventos del GRID GuestAdditional

    #region btnSearchGuestAdditional_Click

    /// <summary>
    /// Abre la ventana SearchGuest
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private async void btnSearchGuestAdditional_Click(object sender, RoutedEventArgs e)
    {
      frmSearchGuest frmSrchGu = new frmSearchGuest(App.User, EnumProgram.Outhouse)
      {
        Owner = this
      };
      frmSrchGu.ShowDialog();
      //Recuperar lista de guests e insertarlas en la lista de GuestAdditionals.
      var guestAdditionalList = frmSrchGu.lstGuestAdd ?? new List<Guest>();
      if (guestAdditionalList.Any())
      {
        List<string> lstMsg = new List<string>();
        foreach (var ga in guestAdditionalList)
        {
          //Si la invitacion esta en modo ReadOnly y el ID del guestadditional es igual al guest principal
          //O si el guestadditional ya tiene una invitacion.Ya no se agrega a la lista.
          var validate = await InvitationValidationRules.ValidateAdditionalGuest(GuestObj, ga);
          if (!validate.Item1) { lstMsg.Add($"Guest ID: {ga.guID} \t{validate.Item2}"); continue; }
          if (validate.Item1 && GuestAdditionalList.Any(c => c.guID == ga.guID)) { lstMsg.Add($"Guest ID: {ga.guID} \tIt is already in the list."); continue; }
          GuestAdditionalList.Add(ga);
        };

        if (lstMsg.Any())
        {
          UIHelper.ShowMessage(string.Join("\n", lstMsg));
        }
      }
    }

    #endregion btnSearchGuestAdditional_Click

    #region guestDetails_Click

    /// <summary>
    /// Abre la ventana Guest, para mostrar la informacion.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private void guestDetails_Click(object sender, RoutedEventArgs e)
    {
      var guest = dtgGuestAdditional.Items[dtgGuestAdditional.Items.CurrentPosition] as Guest;
      if (guest == null || guest.guID == 0) return;
      if (GuestObj != null && string.IsNullOrWhiteSpace(txtguls.Text))
      {
        UIHelper.ShowMessage("Specify the Lead Source", title: "Intelligence Marketing");
        return;
      }
      if (GuestObj != null && string.IsNullOrWhiteSpace(txtgusr.Text))
      {
        UIHelper.ShowMessage("Specify the Sales Room", title: "Intelligence Marketing");
        return;
      }
      frmGuest frmGuest = new frmGuest(App.User, guest.guID, false, EnumModule.Host, dtgGuestAdditional.IsReadOnly) { Owner = this };
      frmGuest.ShowDialog();
    }

    #endregion guestDetails_Click

    #region btnAddGuestAdditional_OnClick

    /// <summary>
    /// Abre la ventana Guest, para crear el nuevo guest adicional.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private void BtnAddGuestAdditional_OnClick(object sender, RoutedEventArgs e)
    {
      if (GuestObj != null && string.IsNullOrWhiteSpace(txtguls.Text))
      {
        UIHelper.ShowMessage("Specify the Lead Source", title: "Intelligence Marketing");
        return;
      }
      if (GuestObj != null && string.IsNullOrWhiteSpace(txtgusr.Text))
      {
        UIHelper.ShowMessage("Specify the Sales Room", title: "Intelligence Marketing");
        return;
      }

      frmGuest frmGuest = new frmGuest(App.User, 0, false, EnumModule.Host, dtgGuestAdditional.IsReadOnly);
      frmGuest.ShowDialog();
      //Validacion del nuevo guest.
    }

    #endregion btnAddGuestAdditional_OnClick

    #endregion GuestAdditional

    #endregion Eventos del formulario

    #region Implementacion INotifyPropertyChange

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value)) return;
      field = value;
      OnPropertyChanged(propertyName);
    }

    #endregion Implementacion INotifyPropertyChange



  }
}