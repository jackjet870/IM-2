using CrystalDecisions.CrystalReports.Engine;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Classes;
using IM.Host.Enums;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmShow.xaml
  /// </summary>
  /// <history>
  /// [vipacheco] 03/16/2016 Created
  /// </history>
  public partial class frmShow : Window
  {
    public GuestPremanifestHost _guestHostCurrent = new GuestPremanifestHost();
    CollectionViewSource _dsGuest;
    private int _guestCurrent = 0;
    private DateTime _dateCurrent;

    #region Colleciones
    CollectionViewSource _dsMaritalStatus;
    CollectionViewSource _dsAgencies;
    CollectionViewSource _dsCountries;
    CollectionViewSource _dsHotels;
    CollectionViewSource _dsLanguajes;
    CollectionViewSource _dsCurrencies;
    CollectionViewSource _dsPaymentTypes;
    CollectionViewSource _dsTeamsSalesMen;
    CollectionViewSource _dsPersonnel;
    CollectionViewSource _dsPersonnelPR;
    CollectionViewSource _dsPersonnelLINER;
    CollectionViewSource _dsPersonnelCLOSER;
    CollectionViewSource _dsPersonnelEXITCLOSER;
    CollectionViewSource _dsPersonnelPODIUM;
    CollectionViewSource _dsPersonnelVLO, _dsPersonnelHOSTSENTRY, _dsPersonnelHOSTSGIFTS, _dsPersonnelHOSTSEXIT;
    private Guest _guest;

    #endregion

    #region btnPrint_Click
    /// <summary>
    /// Imprime el reporte GuestRegistration
    /// </summary>
    /// <history>
    /// [edgrodriguez] 25/Jul/2016 Created
    /// </history>
    private async void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      byte iMotive = 0;
      if (_guest.guReimpresion > 0)
      {
        frmReimpresionMotives _frmReimpresion = new frmReimpresionMotives()
        {
          ShowInTaskbar = false,
          Owner = this
        };
        if (!(_frmReimpresion.ShowDialog() ?? false)) return;

        iMotive = (byte)_frmReimpresion.LstMotives.SelectedValue;
        
        //Actualizamos el motivo de reimpresion.
        await BRReimpresionMotives.UpdateGuestReimpresionMotive(_guest.guID, iMotive);
      }
      else
      //Actualizamos el contador de reimpresion.
      {
        await BRReimpresionMotives.UpdateGuestReimpresionNumber(_guest.guID);
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

        CrystalReportHelper.ShowReport(rptGuestRegistration, $"Guest Registration {_guest.guID.ToString()}", PrintDevice: EnumPrintDevice.pdScreen, numCopies: ((string.IsNullOrWhiteSpace(txtCopies.Text)) ? 1 : Convert.ToInt32(txtCopies.Text)));

        //Cerramos el Formulario.
        Close();
      }
    }
    #endregion

    public frmShow(int guestID)
    {
      _guestCurrent = guestID;
      InitializeComponent();
    }

    #region Window_Loaded
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsGuest = ((CollectionViewSource)(this.FindResource("guestViewSource")));

      // Obtenemos la fecha actual
      _dateCurrent = frmHost.dtpServerDate;

      #region Build Reference to Resources XAML
      // Obtemos las referencias de los datasource a utilizar
      _dsMaritalStatus = ((CollectionViewSource)(this.FindResource("dsMaritalStatus")));
      _dsAgencies = ((CollectionViewSource)(this.FindResource("dsAgencies")));
      _dsCountries = ((CollectionViewSource)(this.FindResource("dsCountries")));
      _dsLanguajes = ((CollectionViewSource)(this.FindResource("dsLanguajes")));
      _dsHotels = ((CollectionViewSource)(this.FindResource("dsHotels")));
      _dsCurrencies = ((CollectionViewSource)(this.FindResource("dsCurrencies")));
      _dsPaymentTypes = ((CollectionViewSource)(this.FindResource("dsPaymentTypes")));
      _dsTeamsSalesMen = ((CollectionViewSource)(this.FindResource("dsTeamsSalesMen")));
      _dsPersonnel = ((CollectionViewSource)(this.FindResource("dsPersonnel")));
      _dsPersonnelPR = ((CollectionViewSource)(this.FindResource("dsPersonnelPR")));
      _dsPersonnelLINER = ((CollectionViewSource)(this.FindResource("dsPersonnelLINER")));
      _dsPersonnelCLOSER = ((CollectionViewSource)(this.FindResource("dsPersonnelCLOSER")));
      _dsPersonnelEXITCLOSER = ((CollectionViewSource)(this.FindResource("dsPersonnelEXITCLOSER")));
      _dsPersonnelPODIUM = ((CollectionViewSource)(this.FindResource("dsPersonnelPODIUM")));
      _dsPersonnelVLO = ((CollectionViewSource)(this.FindResource("dsPersonnelVLO")));
      _dsPersonnelHOSTSENTRY = ((CollectionViewSource)(this.FindResource("dsPersonnelHOSTSENTRY")));
      _dsPersonnelHOSTSGIFTS = ((CollectionViewSource)(this.FindResource("dsPersonnelHOSTSGIFTS")));
      _dsPersonnelHOSTSEXIT = ((CollectionViewSource)(this.FindResource("dsPersonnelHOSTSEXIT")));
      #endregion

      LoadCombos();

      // Verificamos la autentificación automatica
      if (App.User.AutoSign)
      {
        txtChangedBy.Text = App.User.User.peID;
        txtPwd.Password = App.User.User.pePwd;
      }

      // Cargamos los datos del Show
      _guest = await BRGuests.GetGuest(_guestCurrent);
      DataContext = _guest;

    }
    #endregion

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
      _dsPersonnelLINER.Source = frmHost._lstPersonnelLINER; // Liner's
      _dsPersonnelCLOSER.Source = frmHost._lstPersonnelCLOSER; // Closer's
      _dsPersonnelEXITCLOSER.Source = frmHost._lstPersonnelCLOSEREXIT; // Exit Closer's
      _dsPersonnelPODIUM.Source = frmHost._lstPersonnelPODIUM; // Podium
      _dsPersonnelVLO.Source = frmHost._lstPersonnelVLO; // Verificador Legal
      _dsPersonnelHOSTSENTRY.Source = frmHost._lstPersonnelHOSTENTRY; // Host de llegada
      _dsPersonnelHOSTSGIFTS.Source = frmHost._lstPersonnelHOSTGIFTS; // Host de regalos
      _dsPersonnelHOSTSEXIT.Source = frmHost._lstPersonnelHOSTEXIT; // Host de salida
      _dsHotels.Source = frmHost._lstHotel; // Hoteles
      _dsLanguajes.Source = frmHost._lstLanguaje; //Idiomas
      _dsTeamsSalesMen.Source = frmHost._lstTeamSalesMen; // Equipos de vendefores
      _dsAgencies.Source = frmHost._lstAgencies; // Agencias
      _dsMaritalStatus.Source = frmHost._lstMaritalStatus;  // Estado marital
      _dsCountries.Source = frmHost._lstCountries; // Ciudades

      _dsPersonnel.Source = frmHost._lstPersonnel;
    } 
    #endregion


  }
}
