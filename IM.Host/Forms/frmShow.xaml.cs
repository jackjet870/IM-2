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
using IM.Services.WirePRService;
using PalaceResorts.Common.PalaceTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
  public partial class frmShow
  {
    #region Atributos

    private readonly UserData _user;
    private readonly int _guestId;
    private EnumProgram _enumProgram;
    private DateTime _dateCurrent;
    private SalesRoomCloseDates _salesRoom = new SalesRoomCloseDates();
    private bool _blnLoading;
   

    //Vendedores
    private List<ShowSalesman> _showSalesmanList;

    public readonly GuestShow GuestShow;

    //Grids Banderas
    private DataGridCellInfo _IGCurrentCell;//Celda que se esta modificando
    private bool _hasError; //Sirve para las validaciones True hubo Error | False NO
    private bool _isCellCancel;//Sirve para cuando se cancela la edicion de una Celda
    private bool _dontShowAgainGuestStatus;
    private bool _isCellCommitDeposit;//Valida si el commit se hace desde la celda de Deposits
    private bool _isCellCommitCC;//Valida si el commit se hace desde la celda de credit cards
    private bool _isCellCommitGuestAdditional;//Valida si el commit se hace desde la celda de GuestAdditional

  

    #endregion Atributos

    #region Constructores y destructores

    public frmShow(int guestID)
    {
      _guestId = guestID;
      _user = App.User;
      InitializeComponent();
      GuestShow = new GuestShow();
      DataContext = GuestShow;
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
      if (GuestShow.Guest.guShowD != null && Common.IsClosed(GuestShow.Guest.guShowD.Value, _salesRoom.srShowsCloseD))
      {
        var iDiffDay = (GuestShow.Guest.guShowD.Value.Date - _salesRoom.srShowsCloseD.Date).TotalDays;

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
      brdBookingDeposits.IsEnabled =
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
      GuestShow.Currencies = frmHost._lstCurrencies; // Monedas
      GuestShow.PaymentTypes = frmHost._lstPaymentsType; // Formas de Pago
      GuestShow.PersonnelPR = frmHost._lstPersonnelPR; // PR's
      GuestShow.PersonnelLINER = frmHost._lstPersonnelLINER; // Liner's
      GuestShow.PersonnelCLOSER = frmHost._lstPersonnelCLOSER; // Closer's
      GuestShow.PersonnelCLOSEREXIT = frmHost._lstPersonnelCLOSEREXIT; // Exit Closer's
      GuestShow.PersonnelPODIUM = frmHost._lstPersonnelPODIUM; // Podium
      GuestShow.PersonnelVLO = frmHost._lstPersonnelVLO; // Verificador Legal
      GuestShow.PersonnelHOSTENTRY = frmHost._lstPersonnelHOSTENTRY; // Host de llegada
      GuestShow.PersonnelHOSTGIFTS = frmHost._lstPersonnelHOSTGIFTS; // Host de regalos
      GuestShow.PersonnelHOSTEXIT = frmHost._lstPersonnelHOSTEXIT; // Host de salida
      GuestShow.Hotels = frmHost._lstHotel; // Hoteles
      GuestShow.Languages = frmHost._lstLanguaje; //Idiomas
      GuestShow.TeamSalesMen = frmHost._lstTeamSalesMen; // Equipos de vendefores
      GuestShow.Agencies = frmHost._lstAgencies; // Agencias
      GuestShow.MaritalStatus = frmHost._lstMaritalStatus;  // Estado marital
      GuestShow.Countries = frmHost._lstCountries; // Ciudades
      GuestShow.CreditCardTypes = frmHost._lstCreditCardTypes; // Tipos Tarjetas Credito
      GuestShow.GuestStatusTypes = frmHost._lstGuestStatusTypes; //Tipos de Status
      GuestShow.DisputeStatus = frmHost._lstDisputeStatus; // Estatus de disputas
      GuestShow.PaymentPlaces = frmHost._lstPaymentPlaces; // Lugares de Pago
      GuestShow.Gifts = frmHost._lstGifts; //Regalos
      GuestShow.SalesRoom = frmHost._lstSalesRoom; //Sales Room
      GuestShow.Locations = frmHost._lstLocations;
      GuestShow.LeadSources = frmHost._lstLeadSources;

      GuestShow.Personnel = frmHost._lstPersonnel;
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
        var result = await BRGuests.GetGuest(_guestId);
        GuestShow.Guest = result;
        GuestShow.CloneGuest = ObjectHelper.CopyProperties(result);
      }));

      lstTasks.Add(Task.Run(async () =>
      {
        //cargamos los regalos de invitacion
        var result = await BRInvitsGifts.GetInvitsGiftsByGuestID(_guestId);
        GuestShow.InvitationGiftList = new ObservableCollection<InvitationGift>(result);
        GuestShow.CloneInvitationGiftList = ObjectHelper.CopyProperties(result);
      }));

      lstTasks.Add(Task.Run(async () =>
      {
        //cargamos los depositos
        var result = await BRBookingDeposits.GetBookingDeposits(_guestId);
        GuestShow.BookingDepositList = new ObservableCollection<BookingDeposit>(result);
        GuestShow.CloneBookingDepositList = ObjectHelper.CopyProperties(result);
      }));

      lstTasks.Add(Task.Run(async () =>
      {
        //cargamos las tarjetas de credito
        var result = await BRGuestCreditCard.GetGuestCreditCard(_guestId);
        GuestShow.GuestCreditCardList = new ObservableCollection<GuestCreditCard>(result);
        GuestShow.CloneGuestCreditCardList = ObjectHelper.CopyProperties(result);
      }));

      lstTasks.Add(Task.Run(async () =>
      {
        //cargamos las tarjetas de credito
        var result = await BRGuests.GetAdditionalGuest(_guestId);
        GuestShow.AdditionalGuestList = new ObservableCollection<Guest>(result);
        GuestShow.CloneAdditionalGuestList = ObjectHelper.CopyProperties(result);
      }));

      await Task.WhenAll(lstTasks);

      if (GuestShow.Guest.guShow && GuestShow.Guest.guShowD != null)
        chkguDirect.IsEnabled = true;

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
        GuestShow.Guest.guLiner1,
        GuestShow.Guest.guLiner2,
        GuestShow.Guest.guCloser1,
        GuestShow.Guest.guCloser2,
        GuestShow.Guest.guCloser3,
        GuestShow.Guest.guExit1,
        GuestShow.Guest.guExit2
      };

      salesmen = salesmen.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
      //Agregamos los datos del los SaleMen
      _showSalesmanList = new List<ShowSalesman>();
      salesmen.ForEach(s =>
      {
        PersonnelShort personnelShort = frmHost._lstPersonnel.First(p => p.peID == s);
        var showSalesman = new ShowSalesman
        {
          Guest = GuestShow.Guest,
          shgu = _guestId,
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
      if (GuestShow.Guest.guShow && !string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkShows, string.Empty, showMessage: true)))
        blnValid = false;

      //validamos que la fecha de show no este en una fecha cerrada
      else if (!Common.ValidateCloseDate(EnumEntities.Shows, ref dtpguShowD, _salesRoom.srShowsCloseD, pCondition: GuestShow.Guest.guShow))
      {
        ValidateClosedDate(false);
        blnValid = false;
      }

      // validamos que indique si presento invitacion
      else if (GuestShow.Guest.guShow && _enumProgram == EnumProgram.Outhouse
        && !string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkPresentedInvitation, string.Empty, showMessage: true)))
        blnValid = false;

      // validamos el tipo de show
      else if (!ValidateShowType())
        blnValid = false;

      //validamos que el folio de la invitacion outhouse exista y que no haya sido usada
      else if (_enumProgram == EnumProgram.Outhouse && !InvitationValidationRules.ValidateFolio(GuestShow.Guest, _enumProgram, txtguOutInvitNum, brdSearchReservation, tabGeneral))
        blnValid = false;

      // validamos el folio de la reservacion inhouse
      else if (_enumProgram == EnumProgram.Inhouse && !InvitationValidationRules.ValidateFolio(GuestShow.Guest, _enumProgram, txtguHReservID, brdSearchReservation, tabGeneral))
        blnValid = false;

      // validamos la fecha de llegada y la fecha de salida
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkCheckInOut, string.Empty, showMessage: true)))
        blnValid = false;

      // validamos el estado civil, apellido y nombre
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdGuest1, string.Empty, showMessage: true)))
        blnValid = false;

      // validamos el estado civil de su acompañante
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdGuest2, string.Empty, showMessage: true)))
        blnValid = false;

      // validamos la locacion
      else if (!ValidateLocation())
      {
        cmbguloInvit.Focus();
        blnValid = false;
      }

      // validamos si la invitacion es una quiniela split
      else if (!await ValidateQuinellaSplit())
        blnValid = false;

      // validamos el show
      else if (!ValidateShow())
        blnValid = false;

      // validamos los huespedes adicionales
      else if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdAdditionalGuest, string.Empty, true, true)))
        blnValid = false;

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
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdOtherInfo, string.Empty, showMessage: true)))
        blnValid = false;

      // validamos el numero de bookings
      else if (!ValidateHelper.ValidateNumber(GuestShow.Guest.guRoomsQty, 1, 9, "books quantity"))
        blnValid = false;

      // validamos el pax
      else if (!ValidateHelper.ValidateNumber(GuestShow.Guest.guPax, 0.1m, 1000, "Pax number"))
        blnValid = false;

      // validamos los regalos
      else if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdGifts, string.Empty, true, true)))
        blnValid = false;

      // validamos las tarjetas de credito
      else if (!InvitationValidationRules.ValidateGuestCreditCard(GuestShow.GuestCreditCardList.ToList()))
        blnValid = false;

      // validamos los estatus de invitados
      else if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdGuestStatus, string.Empty, showMessage: true)))
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

      // validamos los depositos
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdBookingDeposits, string.Empty, true, true)))
        blnValid = false;

      // validamos el PR 1
      else if (GuestShow.Guest.guSelfGen && !string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkPR1, string.Empty, showMessage: true)))
        blnValid = false;

      // validamos el equipo de vendedores si es un Self Gen
      else if (GuestShow.Guest.guSelfGen && !string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(stkTeam, string.Empty, showMessage: true)))
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
      if (!string.IsNullOrWhiteSpace(ValidateHelper.ValidateForm(brdPlaces, string.Empty, showMessage: true)))
        blnValid = false;

      // validamos que la locacion exista
      else if (cmbguloInvit.IsEnabled)
      {
        // localizamos la locacion
        var location = frmHost._lstLocations.FirstOrDefault(lo => lo.loID == GuestShow.Guest.guloInvit);
        // si encontramos la locacion
        if (location != null)
        {
          // establecemos el Lead Source de la locacion
          cmbguls.SelectedValuePath = location.lols;

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
      string program = frmHost._lstLeadSources.First(x => x.lsID == GuestShow.Guest.guls).lspg;
      _enumProgram = EnumToListHelper.GetList<EnumProgram>().Single(x => x.Value == program).Key;

      bool blnIsInHouse = _enumProgram == EnumProgram.Inhouse;

      // Invitacion externa
      txtguOutInvitNum.IsEnabled =
        // Fecha de llegada
        dtpguCheckInD.IsEnabled =
          // Fecha de salida
          dtpguCheckOutD.IsEnabled = !blnIsInHouse;

      // si es una invitacion inhouse y no tiene un folio de reservacion definido, permitimos definirlo
      brdSearchReservation.IsEnabled = blnIsInHouse && string.IsNullOrWhiteSpace(GuestShow.Guest.guHReservID);

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
      var validateExist = await BRShows.GetValidateShow(txtChangedBy.Text, EncryptHelper.Encrypt(txtPwd.Password), GuestShow.Guest);

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
          cmbgusr.Focus(); break;
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
      if (GuestShow.Guest.guQuinellaSplit)
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
      if (GuestShow.Guest.guMainInvit != null)
      {
        // si la invitacion principal no es la invitacion actual
        if (GuestShow.Guest.guMainInvit != GuestShow.Guest.guID)
        {
          // obtenemos el nombre del huesped
          txtMainInvitName.Text = await Common.GetGuestName(GuestShow.Guest.guMainInvit);

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
        GuestShow.Guest.guLiner1,
        GuestShow.Guest.guLiner2,
        GuestShow.Guest.guCloser1,
        GuestShow.Guest.guCloser2,
        GuestShow.Guest.guCloser3,
        GuestShow.Guest.guExit1,
        GuestShow.Guest.guExit2
      }.Any(s => !string.IsNullOrWhiteSpace(s)))
      {
        // validamos que sea show
        if (!GuestShow.Guest.guShow)
        {
          UIHelper.ShowMessage("This case must be ''Show'' because it has defined salesmen");
          chkguShow.Focus();
          blnValid = false;
        }

        // validamos que sea tour, walk out, tour de cortesia o tour de rescate
        else if (!GuestShow.Guest.guTour && !GuestShow.Guest.guWalkOut && !GuestShow.Guest.guCTour && !GuestShow.Guest.guSaveProgram)
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
        if (GuestShow.Guest.guTour)
        {
          if (UIHelper.ShowMessage("This case not must be ''Tour'' because it has not defined salesmen. Save anyway?", MessageBoxImage.Question) == MessageBoxResult.No)
          {
            rdbguTour.Focus();
            blnValid = false;
          }
        }
        // validamos que no sea walk out
        else if (GuestShow.Guest.guWalkOut)
        {
          if (UIHelper.ShowMessage("This case not must be ''Walk Out'' because it has not defined salesmen. Save anyway?", MessageBoxImage.Question) == MessageBoxResult.No)
          {
            rdbguWalkOut.Focus();
            blnValid = false;
          }
        }
        // validamos que no sea tour de cortesia
        else if (GuestShow.Guest.guCTour)
        {
          if (UIHelper.ShowMessage("This case not must be ''Courtesy Tour'' because it has not defined salesmen. Save anyway?", MessageBoxImage.Question) == MessageBoxResult.No)
          {
            rdbguCTour.Focus();
            blnValid = false;
          }
        }
        // validamos que no sea tour de rescate
        else if (GuestShow.Guest.guSaveProgram)
        {
          if (UIHelper.ShowMessage("This case not must be ''Save Tour'' because it has not defined salesmen. Save anyway?", MessageBoxImage.Question) == MessageBoxResult.No)
          {
            rdbguSaveProgram.Focus();
            blnValid = false;
          }
        }
      }

      // si es valido y es show
      if (blnValid && GuestShow.Guest.guShow)
      {
        // validamos el numero de shows
        if (!ValidateHelper.ValidateNumber(GuestShow.Guest.guShowsQty, 1, 9, "shows quantity"))
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
      if (!GuestShow.Guest.guShow) return true;
      // si no tiene un tipo de show
      if (GuestShow.Guest.guTour || GuestShow.Guest.guInOut || GuestShow.Guest.guWalkOut || GuestShow.Guest.guCTour || GuestShow.Guest.guSaveProgram || GuestShow.Guest.guWithQuinella) return true;
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
    private async Task Save()
    {
      busyIndicator.IsBusy = true;
      busyIndicator.BusyContent = "Saving show...";
      // indicamos que ya se le dio el show al invitado
      if (GuestShow.Guest.guTimeInT != null && !GuestShow.Guest.guShow)
        chkguShow.IsChecked = true;

      // checamos si los datos de Self Gen estan correctamente llenados
      await CheckSelfGen();

      //// si es una invitacion outside y esta habilitado el uso de perfiles de Opera
      //if (_enumProgram == EnumProgram.Outhouse && ConfigHelper.GetString("ReadOnly").ToUpper().Equals("TRUE"))
      //{
      //  //WirePRHelper.SaveProfileOpera Me, mData
      //}
      // guardar
      if (await BRGuests.SaveGuestShow(GuestShow, App.User, txtChangedBy.Text, Environment.MachineName, ComputerHelper.GetIpMachine()) == 0)
      {
        UIHelper.ShowMessage("There was an error saving the information, consult your system administrator",
          MessageBoxImage.Error, "Information can not keep");
      }
      await LoadRecord();
      busyIndicator.IsBusy = false;
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
      var dtmDate = GuestShow.Guest.guShowD != null ? Convert.ToDateTime(GuestShow.Guest.guShowD) : Convert.ToDateTime(GuestShow.Guest.guBookD);

      // establecemos el tipo de liner
      PostShort postShort = await BRPosts.GetPersonnelPostByDate(GuestShow.Guest.guLiner1, dtmDate);

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

    #endregion SetLinerType

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

    #endregion SetSelfGen

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
      if (!string.IsNullOrWhiteSpace(GuestShow.Guest.guLiner1) && await BRPersonnel.IsFrontToMiddle(GuestShow.Guest.guLiner1))
      {
        // si no esta como Self Gen
        if (!GuestShow.Guest.guSelfGen &&
          UIHelper.ShowMessage($"This case must be Self Gen. {Environment.NewLine}Mark the checkbox ''SELF GEN''?", MessageBoxImage.Question) == MessageBoxResult.Yes)
          chkguSelfGen.IsChecked = true;
      }
      // si no es un Self Gen
      else
      {
        //si no esta como Self Gen
        if (GuestShow.Guest.guSelfGen &&
            UIHelper.ShowMessage($"This case not must be Self Gen.. {Environment.NewLine}Unmark the checkbox ''SELF GEN''?", MessageBoxImage.Question) == MessageBoxResult.Yes)
          chkguSelfGen.IsChecked = false;
      }
    }

    #endregion CheckSelfGen

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
      if (!GuestShow.Guest.guShow || Convert.ToBoolean(GuestShow.Guest.guPresentedInvitation) ||
          Convert.ToBoolean(GuestShow.Guest.guNotifiedEmailShowNotInvited) || _enumProgram != EnumProgram.Outhouse) return;

      // obtenemos la sala de ventas
      var salesRoom = frmHost._lstSalesRoom.FirstOrDefault(s => s.srID == GuestShow.Guest.gusr);

      // obtenemos el Lead Source
      var leadSource = frmHost._lstLeadSources.FirstOrDefault(s => s.lsID == GuestShow.Guest.guls);

      // Obtenermos el PR1
      var personnel = frmHost._lstPersonnelPR.FirstOrDefault(p => p.peID == GuestShow.Guest.guPRInvit1);

      var cont = 1;
      var x = GuestShow.BookingDepositList.ToList().Select(c => new
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
      var res = clsEmail.SendMail(GuestShow.Guest, salesRoom, leadSource, personnel, dtData);

      //'Si la respuesta es diferente de vacio, mandamos el mensaje de error
      if (res == string.Empty)
      {
        GuestShow.Guest.guNotifiedEmailShowNotInvited = true;
        await BREntities.OperationEntity(GuestShow.Guest, EnumMode.Edit);
      }
      else
      {
        UIHelper.ShowMessage(res, MessageBoxImage.Error);
      }
    }

    #endregion SendEmail

    #region GetSearchReservationInfo

    /// <summary>
    /// Asignamos los valores de ReservationOrigos a nuestro objeto Guest
    /// </summary>
    /// <param name="reservationOrigos">ReservationOrigos</param>
    /// <history>
    /// [aalcocer] 19/08/2016  Created.
    /// </history>
    private void SetRervationOrigosInfo(ReservationOrigos reservationOrigos)
    {
      //Asignamos el folio de reservacion
      GuestShow.Guest.guHReservID = reservationOrigos.Folio;
      GuestShow.Guest.guLastName1 = reservationOrigos.LastName;
      GuestShow.Guest.guFirstName1 = reservationOrigos.FirstName;
      GuestShow.Guest.guCheckInD = reservationOrigos.Arrival;
      GuestShow.Guest.guCheckOutD = reservationOrigos.Departure;
      GuestShow.Guest.guRoomNum = reservationOrigos.Room;
      //Calculamos Pax
      decimal pax = 0;
      bool convertPax = decimal.TryParse($"{reservationOrigos.Adults}.{reservationOrigos.Children}", out pax);
      GuestShow.Guest.guPax = pax;

      GuestShow.Guest.guco = frmHost._lstCountries.Where(x => x.coN == reservationOrigos.Country).Select(x => x.coID).FirstOrDefault();
      GuestShow.Guest.guag = frmHost._lstAgencies.Where(x => x.agN == reservationOrigos.Agency).Select(x => x.agID).FirstOrDefault();
      GuestShow.Guest.guHotel = frmHost._lstHotel.Where(x => x.hoGroup == reservationOrigos.Hotel).Select(x => x.hoID).FirstOrDefault();
      GuestShow.Guest.guCompany = reservationOrigos.Company;
      GuestShow.Guest.guMembershipNum = reservationOrigos.Membership;
    }

    #endregion GetSearchReservationInfo

    #endregion Metodos

    #region Eventos del formulario

    #region Window_Loaded

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      // Obtenemos la fecha actual
      _dateCurrent = frmHost.dtpServerDate;

      //cargamos los combos
      LoadCombos();

      //si tiene permiso especial
      cmbgusr.IsEnabled = App.User.HasPermission(EnumPermission.Show, EnumPermisionLevel.Special);

      // Verificamos la autentificación automatica
      if (App.User.AutoSign)
      {
        txtChangedBy.Text = App.User.User.peID;
        txtPwd.Password = App.User.User.pePwd;
      }

      //configuramos la clase de datos
      UIHelper.SetUpControls(new Guest(), this);
      // configuramos el grid de depositos
      GridHelper.SetUpGrid(dtgBookingDeposits, new BookingDeposit());
      // configuramos el grid de tarjetas de credito
      GridHelper.SetUpGrid(dtgCCCompany, new GuestCreditCard());
      // configuramos el grid de invitados adicionales
      GridHelper.SetUpGrid(dtgGuestAdditional, new Guest());
      GridHelper.SetUpGrid(dtgGifts, new InvitationGift());

      //obtenemos la fechas de cierre
      _salesRoom = BRSalesRooms.GetSalesRoom(App.User.SalesRoom.srID);

      //cargamos los datos del show
      await LoadRecord();

      // establecemos el numero de copias
      GuestShow.OcWelcomeCopies = frmHost._configuration.ocWelcomeCopies;

      Gifts.CalculateTotalGifts(dtgGifts, EnumGiftsType.InvitsGifts, "igQty", "iggi", "igPriceM", "igPriceMinor", "igPriceAdult", "igPriceA", "igPriceExtraAdult", txtGiftTotalCost, txtGiftTotalPrice);

      //mostramos la clave y el nombre del huesped en el titulo del formulario
      Title = $"{Title} - {GuestShow.Guest.guID}, {Common.GetFullName(GuestShow.Guest.guLastName1, GuestShow.Guest.guFirstName1)}";

      // si aun no es guardado como show y su fecha de booking no es del dia actual ni del dia siguiente, se impide imprimir
      if (!GuestShow.Guest.guShow && GuestShow.Guest.guBookD.HasValue && !DateHelper.IsInRangeDate(GuestShow.Guest.guBookD.Value, _dateCurrent, _dateCurrent.AddDays(1)))
        imgButtonPrint.IsEnabled = false;

      //  si el sistema esta en modo de solo lectura o el usuario tiene cuando mucho permiso de lectura
      // o si el show es de una fecha cerrada
      if (ConfigHelper.GetString("ReadOnly").ToUpper().Equals("TRUE") ||
          !App.User.HasPermission(EnumPermission.Show, EnumPermisionLevel.Standard) || !ValidateClosedDate(true))
      {
        imgButtonSave.IsEnabled = imgButtonPrint.IsEnabled = false;
        btnSearchGuestAdditional.IsEnabled = btnAddGuestAdditional.IsEnabled = false;
        dtgGuestAdditional.IsReadOnly = true;
      }

      //Si de la BD el campo guNotifiedEmailShowNotInvited es null, lo ponemos en unChecked el control
      if (GuestShow.Guest.guNotifiedEmailShowNotInvited == null)
        chkguNotifiedEmailShowNotInvited.IsChecked = false;
    }

    #endregion Window_Loaded

    #region brdSearchButton_MouseLeftButtonDown

    /// <summary>
    /// Obtiene un Folio de Reservacion
    /// </summary>
    /// <history>
    /// [aalcocer] 19/08/2016  Created.
    /// </history>
    private void brdSearchButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      frmSearchReservation search = new frmSearchReservation(_user) { Owner = this };

      // Verificamos si se selecciono un guest
      if (search.ShowDialog().Value)
      {
        //Seteamos la informacion de SearchGuest en nuestro objeto Guest
        SetRervationOrigosInfo(search._reservationInfo);

        UIHelper.UpdateTarget(this);
      }
    }

    #endregion brdSearchButton_MouseLeftButtonDown

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
      if (GuestShow.Guest.guTimeInT == null)
        tpkguTimeInT.Value = BRHelpers.GetServerDateTime();
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
      var frmGuestLog = new frmGuestLog(_guestId) { Owner = this };
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
      if (GuestShow.Guest.guTimeOutT == null)
      {
        tpkguTimeOutT.Value = BRHelpers.GetServerDateTime();
        if (GuestShow.Guest.guTimeInT == null)
          tpkguTimeInT.Value = GuestShow.Guest.guTimeOutT;
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
      imgButtonPrint.Focus();
      if (!await Validate()) return;
      if (GuestShow.Guest.guReimpresion > 0)
      {
        frmReimpresionMotives _frmReimpresion = new frmReimpresionMotives()
        {
          ShowInTaskbar = false,
          Owner = this
        };
        if (!(_frmReimpresion.ShowDialog() ?? false)) return;

        var iMotive = (byte)_frmReimpresion.LstMotives.SelectedValue;

        //Actualizamos el motivo de reimpresion.
        await BRReimpresionMotives.UpdateGuestReimpresionMotive(GuestShow.Guest.guID, iMotive);
      }
      else
      //Actualizamos el contador de reimpresion.
      {
        await BRReimpresionMotives.UpdateGuestReimpresionNumber(GuestShow.Guest.guID);
      }
      //Salvamos la informacion del show.
      await Save();

      //Se imprime el reporte.
      var lstRptGuestRegistration = await BRGuests.GetGuestRegistration(_guestId);
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
          msgReimpresion = (string.IsNullOrEmpty(msgReimpresion)) ? "" : msgReimpresion.Replace("[grReimpresion]", guestRegistration.guReimpresion.ToString()).Replace("[rmN]", guestRegistration.rmN ?? "");
          (rptGuestRegistration.ReportDefinition.ReportObjects["lblReimpresion"] as TextObject).Text = msgReimpresion;
        }

        CrystalReportHelper.ShowReport(rptGuestRegistration, $"Guest Registration {GuestShow.Guest.guID}", PrintDevice: EnumPrintDevice.pdScreen, numCopies: GuestShow.OcWelcomeCopies);

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
        var salessalesmen = new frmShowsSalesmen(_guestId, _showSalesmanList)
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
      imgButtonSave.Focus();
      if (!await Validate()) return;
      try
      {
        await Save();
        SendEmail();

        UIHelper.ShowMessage("The data was saved successfully");

        Close();
      }
      catch (Exception ex)
      {
        busyIndicator.IsBusy = false;
        UIHelper.ShowMessage(ex);
      }
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
      if (GuestShow.Guest.guQuinellaSplit)
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
      if (GuestShow.Guest.guSelfGen)
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

      if (!GuestShow.Guest.guShow)
      {
        dtpguShowD.Value = null;
        tpkguTimeInT.Value = null;

        // validamos que no se pueda desmarcar el show si tiene cupon de comida o venta
        if (!GuestShow.Guest.guMealTicket && !GuestShow.Guest.guSale) return;
        UIHelper.ShowMessage("You cannot uncheck the Show if there is a Meal Ticket or Sale.");
        chkguShow.IsChecked = true;
      }
      else if (GuestShow.Guest.guShowD == null)
      {
        dtpguShowD.Value = BRHelpers.GetServerDate();
        if (GuestShow.Guest.guTimeInT == null)
          tpkguTimeInT.Value = BRHelpers.GetServerDateTime();
      }
    }

    #endregion chkguShow_Checked

    #region Window_OnContentRendered

    private async void Window_OnContentRendered(object sender, EventArgs e)
    {
      if (GuestShow.Guest == null) return;
      // checamos si los datos de Self Gen estan correctamente llenados
      await CheckSelfGen();

      // checamos el equipo de vendedores si es un Self Gen
      if (GuestShow.Guest.guSelfGen && string.IsNullOrWhiteSpace(GuestShow.Guest.guts))
      {
        UIHelper.ShowMessage("Specify the Team.");
        cmbguts.Focus();
      }

      // establecemos el nombre de la invitacion principal
      await ValidateMainInvitation(false);
    }

    #endregion Window_OnContentRendered

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

      if (sender.Equals(cmbguLiner1) && !string.IsNullOrWhiteSpace(GuestShow.Guest.guLiner1))
      {
        // establecemos el tipo del liner 1
        await SetLinerType();

        // determinamos si es un Self Gen
        await SetSelfGen(GuestShow.Guest.guLiner1);
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

    #endregion cmb_SelectionChanged

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

    #endregion txtocWelcomeCopies_PreviewTextInput

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

    #endregion TxtocWelcomeCopies_OnLostFocus

    #region cmbGuestStatus_SelectionChanged

    /// <summary>
    /// Evento del Combobox GuestStatus, Sirve para actualizar la caja de texto txtGiftMaxAuth dependiendo del GuestStatus que elija el usuario.
    /// </summary>
    ///<history>
    ///[aalcocer]  19/08/2016  Created.
    /// </history>
    private void cmbGuestStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //Obtenemos el GuestStatusType del combobox cmbGuestStatus
      var guStatusType = cmbGuestStatus.SelectedItem as GuestStatusType;
      //Asigamos el MaxAuth a la caja de texto
      txtGiftMaxAuth.Text = $"{guStatusType?.gsMaxAuthGifts ?? 0:C2}";
    }

    #endregion cmbGuestStatus_SelectionChanged

    #endregion Eventos del formulario

    #region Eventos del GRID Gift

    #region BeginningEdit

    /// <summary>
    /// Se ejecuta antes de que entre en modo edicion alguna celda
    /// </summary>
    /// <history>
    /// [aalcocer] 08/08/2016  Created.
    /// </history>
    private void dtgGifts_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      //Preguntamos si desea agregar GuestStatusType para el calculo de costos
      if (cmbGuestStatus.SelectedValue == null && !_dontShowAgainGuestStatus)
      {
        MessageBoxResult result = UIHelper.ShowMessage("We recommend first select the status of the guest, that would help us calculate costs and prices, do you want to select it now?", MessageBoxImage.Question, "Intelligence Marketing");
        if (result == MessageBoxResult.Yes)
        {
          e.Cancel = true;
          _hasError = true;
          _isCellCancel = true;
          _dontShowAgainGuestStatus = false;
          cmbGuestStatus.Focus();
        }
        else
        {
          _dontShowAgainGuestStatus = true;
        }
      }
      else
      {
        _hasError = false;
        _isCellCancel = false;
      }

      //Si el grid no esta en modo edicion, permite hacer edicion.
      if (!GridHelper.IsInEditMode(dtgGifts) && !_hasError)
      {
        dtgGifts.BeginningEdit -= dtgGifts_BeginningEdit;
        //Obtenemos el objeto de la fila que se va a editar
        InvitationGift invitationGift = e.Row.Item as InvitationGift;
        //Obtenemos la celda que vamos a validar
        _IGCurrentCell = dtgGifts.CurrentCell;
        //Hacemos la primera validacion
        InvitationValidationRules.StartEdit(ref invitationGift, ref _IGCurrentCell, dtgGifts, ref _hasError);
        //Si tuvo algun error de validacion cancela la edicion de la celda.
        e.Cancel = _hasError;
        dtgGifts.BeginningEdit += dtgGifts_BeginningEdit;
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
    /// [aalcocer] 08/08/2016  Created.
    /// </history>
    private void dtgGifts_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
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
    /// [aalcocer] 08/08/2016  Created.
    /// </history>
    private void dtgGifts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      //Si paso las validaciones del preparingCellForEdit
      if (!_hasError)
      {
        //Si viene en modo Commit
        if (e.EditAction == DataGridEditAction.Commit)
        {
          //esta bandera se pone en falso por que No se ha cancelado la edicion de la celda
          _isCellCancel = false;
          //Obtenemos el Objeto
          InvitationGift invitationGift = e.Row.Item as InvitationGift;

          //Si Paso las validaciones
          if (!InvitationValidationRules.ValidateEdit(ref invitationGift, ref _IGCurrentCell))
          {
            InvitationValidationRules.AfterEdit(_guestId,dtgGifts, ref invitationGift, _IGCurrentCell, ref txtGiftTotalCost, ref txtGiftTotalPrice, ref txtGiftMaxAuth, cmbGuestStatus.SelectedItem as GuestStatusType, GuestShow.Program);
          }
          //Si fallaron las validaciones del AfterEdit se cancela la edicion de la celda.
          else
          {
            e.Cancel = true;
          }
        }
        //Si entra en modo Cancel Se enciende esta bandera ya que servira en RowEditEnding
        else
        {
          _isCellCancel = true;
        }
      }
    }

    #endregion CellEditEnding

    #region RowEditEnding

    /// <summary>
    /// Se ejecuta cuando la fila pierde el foco, o termina la edicion (Commit o Cancel)
    /// </summary>
    /// <history>
    /// [aalcocer] 02/08/2016  Created.
    /// </history>
    private void dtgGifts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)

    {
      DataGrid dtg = sender as DataGrid;
      InvitationGift invitationGift = e.Row.Item as InvitationGift;

      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCancel)
        {
          dtg.RowEditEnding -= dtgGifts_RowEditEnding;
          dtg.CancelEdit();
          dtg.RowEditEnding -= dtgGifts_RowEditEnding;
        }
        else
        {
          if (invitationGift?.igQty == 0 || string.IsNullOrEmpty(invitationGift?.iggi))
          {
            UIHelper.ShowMessage("Please enter the required fields Qty and Gift to continue", MessageBoxImage.Exclamation, "Intelligence Marketing");
            e.Cancel = true;
          }
        }
      }
      else
      {
        //CommonCatObject dtContext = DataContext as CommonCatObject;
        //dtContext.InvitationGiftList.RemoveAt(e.Row.GetIndex());
      }
    }

    #endregion RowEditEnding

    #endregion Eventos del GRID Gift

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
      if (cmbgusr.SelectedIndex == -1 && e.Column.SortMemberPath == "guID")
      {
        UIHelper.ShowMessage("First select a Sales Room ", MessageBoxImage.Warning, "Intelligence Marketing");
        e.Cancel = true;
      }
      else if (!GridHelper.IsInEditMode(dtgGuestAdditional) && !_hasError)
      {
        _IGCurrentCell = dtgGuestAdditional.CurrentCell;
        e.Cancel = InvitationValidationRules.dtgGuestAdditional_StartEdit(ref _IGCurrentCell, dtgGuestAdditional, ref _hasError);
      }
    }

    #endregion BeginningEdit

    #region CellEditEnding

    /// <summary>
    /// Se ejecuta cuando la celda en edicion pierde el foco
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    private void dtgGuestAdditional_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        _isCellCommitGuestAdditional = (Keyboard.IsKeyDown(Key.Enter));
        Guest guestAdditionalRow = e.Row.Item as Guest;
        Guest guestAdditional = AsyncHelper.RunSync(() => BRGuests.GetGuest(guestAdditionalRow?.guID ?? 0));//await BRGuests.GetGuest(guestAdditionalRow.guID);
        var notValid = AsyncHelper.RunSync(() => InvitationValidationRules.dtgGuestAdditional_ValidateEdit(GuestShow.Guest, guestAdditional, _IGCurrentCell));
        if (!notValid)
        {
          e.Row.Item = guestAdditional;
        }
        else
        {
          e.Cancel = true;
        }
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
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCommitGuestAdditional && e.Row.GetIndex() == dtgGuestAdditional.ItemsSource.OfType<object>().ToList().Count)
        {
          _isCellCommitGuestAdditional = false;
          e.Cancel = true;
        }
        else if (Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.Tab))
        {
          int columnIndex = 0;
          _isCellCommitGuestAdditional = false;
          e.Cancel = !AsyncHelper.RunSync(() => InvitationValidationRules.ValidateAdditionalGuest(GuestShow.Guest, (Guest)e.Row.Item, true)).Item1;
          if (e.Cancel)
          {
            _isCellCommitGuestAdditional = true;//true para que no haga el commit
            GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), columnIndex, true);
          }
        }
        else//Cancela el commit de la fila
        {
          e.Cancel = true;
        }
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
      frmSearchGuest frmSrchGu = new frmSearchGuest(_user, EnumProgram.Outhouse)
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
          var validate = await InvitationValidationRules.ValidateAdditionalGuest(GuestShow.Guest, ga);
          if (!validate.Item1) { lstMsg.Add($"Guest ID: {ga.guID} \t{validate.Item2}"); continue; }
          if (validate.Item1 && GuestShow.AdditionalGuestList.Any(c => c.guID == ga.guID)) { lstMsg.Add($"Guest ID: {ga.guID} \tIt is already in the list."); continue; }
          GuestShow.AdditionalGuestList.Add(ga);
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
      if (dtgGuestAdditional.Items.CurrentPosition == -1) return;

      var guest = dtgGuestAdditional.Items[dtgGuestAdditional.Items.CurrentPosition] as Guest;
      if (guest == null || guest.guID == 0) return;
      if (GuestShow != null && string.IsNullOrWhiteSpace(GuestShow.Guest.guls))
      {
        UIHelper.ShowMessage("Specify the Lead Source", title: "Intelligence Marketing");
        return;
      }
      if (GuestShow != null && string.IsNullOrWhiteSpace(GuestShow.Guest.gusr))
      {
        UIHelper.ShowMessage("Specify the Sales Room", title: "Intelligence Marketing");
        return;
      }
      frmGuest frmGuest = new frmGuest(_user, guest.guID, EnumModule.Host,_enumProgram, dtgGuestAdditional.IsReadOnly) { Owner = this };
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
    private async void BtnAddGuestAdditional_OnClick(object sender, RoutedEventArgs e)
    {
      if (GuestShow != null && string.IsNullOrWhiteSpace(GuestShow.Guest.guls))
      {
        UIHelper.ShowMessage("Specify the Lead Source", title: "Intelligence Marketing");
        return;
      }
      if (GuestShow != null && string.IsNullOrWhiteSpace(GuestShow.Guest.gusr))
      {
        UIHelper.ShowMessage("Specify the Sales Room", title: "Intelligence Marketing");
        return;
      }

      frmGuest frmGuest = new frmGuest(_user, 0, EnumModule.Host, _enumProgram, dtgGuestAdditional.IsReadOnly) { GuestParent = GuestShow?.Guest, Owner = this };
      frmGuest.ShowDialog();
      //Validacion del nuevo guest.
      //Recuperar lista de guests e insertarlas en la lista de GuestAdditionals.
      var guestAdditional = frmGuest.NewGuest ?? new Guest();
      if (guestAdditional.guID == 0) return;
      //Si la invitacion esta en modo ReadOnly y el ID del guestadditional es igual al guest principal
      //O si el guestadditional ya tiene una invitacion.Ya no se agrega a la lista.
      var validate = await InvitationValidationRules.ValidateAdditionalGuest(GuestShow?.Guest, guestAdditional, true);
      if (validate.Item1)
        GuestShow?.AdditionalGuestList.Add(guestAdditional);
    }

    #endregion btnAddGuestAdditional_OnClick

    #endregion GuestAdditional

    #region Datagrid Boking Deposits

    #region BeginningEdit

    /// <summary>
    /// Valida que no se puedan habilitar mas de una fila
    /// </summary>
    /// <history>
    /// [emoguel] 19/08/2016
    /// </history>
    private void dtgBookingDeposits_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (!GridHelper.IsInEditMode(dtgBookingDeposits))
      {
        e.Cancel = !InvitationValidationRules.StartEditBookingDeposits(e.Column.SortMemberPath, e.Row.Item as BookingDeposit, true);
      }
      else
      {
        e.Cancel = true;
      }
    }

    #endregion BeginningEdit

    #region CellEditEnding

    /// <summary>
    /// Valida que no se le haga commit a la celda si el dato es erroneo
    /// </summary>
    /// <history>
    /// [emoguel] 17/08/2016 created
    /// </history>
    private void dtgBookingDeposits_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        _isCellCommitDeposit = (Keyboard.IsKeyDown(Key.Enter));
        if (!InvitationValidationRules.validateEditBookingDeposit(e.Column.SortMemberPath, e.Row.Item as BookingDeposit, dtgBookingDeposits, e.EditingElement as Control, GuestShow.CloneBookingDepositList, GuestShow.Guest.guID))
        {
          if (dtgBookingDeposits.CurrentColumn != null && e.Column.DisplayIndex != dtgBookingDeposits.CurrentColumn.DisplayIndex)//Validamos si la columna validada es diferente a la seleccionada
          {
            //Regresamos el foco a la columna con el dato mal
            dtgBookingDeposits.CellEditEnding -= dtgBookingDeposits_CellEditEnding;
            dtgBookingDeposits.CurrentCell = new DataGridCellInfo(e.Row.Item, dtgBookingDeposits.Columns[0]);
            //GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), e.Column.DisplayIndex, true);
            dtgBookingDeposits.CellEditEnding += dtgBookingDeposits_CellEditEnding;
          }
          else
          {
            //Cancelamos el commit de la celda
            e.Cancel = true;
          }
        }
      }
    }

    #endregion CellEditEnding

    #region RowEditEnding

    /// <summary>
    /// Valida que no se haga commit la fila si hay datos erroneos
    /// </summary>
    /// <history>
    /// [emoguel] 17/08/2016 created
    /// </history>
    private void dtgBookingDeposits_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCommitDeposit && e.Row.GetIndex() == dtgBookingDeposits.ItemsSource.OfType<object>().ToList().Count)//Verificar si es un registro nuevo
        {
          _isCellCommitDeposit = false;
          e.Cancel = true;
        }
        else if (Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.Tab))//Si fue commit con el enter desde el la fila
        {
          int columnIndex = 0;
          _isCellCommitDeposit = false;
          e.Cancel = !InvitationValidationRules.EndingEditBookingDeposits(e.Row.Item as BookingDeposit, sender as DataGrid, GuestShow.CloneBookingDepositList, GuestShow.Guest.guID, ref columnIndex);
          if (e.Cancel)
          {
            _isCellCommitDeposit = true;//true para que no haga el commit
            GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), columnIndex, true);
          }
        }
        else//Cancela el commit de la fila
        {
          e.Cancel = true;
        }
      }
    }

    #endregion RowEditEnding

    #endregion Datagrid Boking Deposits

    #region Eventos del Grid Guest Credit Card

    #region dtgCCCompany_RowEditEnding

    /// <summary>
    /// Valida que esten completos los Row si al terminar la edicion quedo algo nulo la elimina
    /// </summary>
    /// <history>
    /// [jorcanche] created 17/ago/2016
    /// [aalcocer] modified 23/ago/2016 Modified. Se valida igual al formulario de frmInvitacion
    /// </history>
    private void dtgCCCompany_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        if (_isCellCommitCC && e.Row.GetIndex() == dtgCCCompany.ItemsSource.OfType<object>().ToList().Count)
        {
          _isCellCommitCC = false;
          e.Cancel = true;
        }
        else if (Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.Tab))
        {
          int columnIndex = 0;
          _isCellCommitCC = false;
          GuestCreditCard guestCreditCard = e.Row.Item as GuestCreditCard;
          if (guestCreditCard != null && guestCreditCard.gdQuantity == 0)
          {
            e.Cancel = true;
            columnIndex = dtgCCCompany.Columns.FirstOrDefault(cl => cl.SortMemberPath == nameof(GuestCreditCard.gdQuantity)).DisplayIndex;
          }
          else if (string.IsNullOrWhiteSpace(guestCreditCard.gdcc))
          {
            columnIndex = dtgCCCompany.Columns.FirstOrDefault(cl => cl.SortMemberPath == nameof(GuestCreditCard.gdcc)).DisplayIndex;
            e.Cancel = true;
          }
          if (e.Cancel)
          {
            _isCellCommitCC = true;//true para que no haga el commit
            GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), columnIndex, true);
          }
        }
        else
        {
          e.Cancel = true;
        }
      }
    }

    #endregion dtgCCCompany_RowEditEnding

    #region dtgCCCompany_BeginningEdit

    /// <summary>
    /// Bloquea la opcion de crear un nuevo registro a menos que se le haya hecho commit a un regristro
    /// No deja habilitar el combobox de creditCard
    /// </summary>
    /// <history>
    /// [aalcocer] 23/08/2016 created
    /// </history>
    private void dtgCCCompany_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (!GridHelper.IsInEditMode(dtgCCCompany))
      {
        if (e.Column.SortMemberPath == nameof(GuestCreditCard.gdcc))
        {
          GuestCreditCard guestCreditCard = e.Row.Item as GuestCreditCard;
          if (guestCreditCard?.gdQuantity <= 0)
          {
            UIHelper.ShowMessage("Enter the quantity first");
            e.Cancel = true;
          }
        }
      }
      else
      {
        e.Cancel = true;
      }
    }

    #endregion dtgCCCompany_BeginningEdit

    #region dtgCCCompany_CellEditEnding

    /// <summary>
    /// Verificar que el valor insertado en la columna sea un valor valido
    /// </summary>
    /// <history>
    /// [aalcocer] 23/08/2016 created
    /// </history>
    private void dtgCCCompany_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (e.EditAction == DataGridEditAction.Commit)
      {
        _isCellCommitCC = (Keyboard.IsKeyDown(Key.Enter));
        GuestCreditCard guestCreditCard = e.Row.Item as GuestCreditCard;
        switch (e.Column.SortMemberPath)
        {
          case nameof(GuestCreditCard.gdQuantity):
            {
              if (guestCreditCard?.gdQuantity == 0)
              {
                UIHelper.ShowMessage("Quantity can not be 0.");
                e.Cancel = true;
              }
              break;
            }
          case nameof(GuestCreditCard.gdcc):
            {
              e.Cancel = GridHelper.HasRepeatItem(e.EditingElement as Control, dtgCCCompany, false, nameof(GuestCreditCard.gdcc));
              break;
            }
        }
        if (e.Cancel)
        {
          //Regresamos el foco a la columna con el dato mal
          dtgCCCompany.CellEditEnding -= dtgCCCompany_CellEditEnding;
          GridHelper.SelectRow(sender as DataGrid, e.Row.GetIndex(), e.Column.DisplayIndex, true);
          dtgCCCompany.CellEditEnding += dtgCCCompany_CellEditEnding;
        }
      }
    }

    #endregion dtgCCCompany_CellEditEnding

    #endregion Eventos del Grid Guest Credit Card
  }
}