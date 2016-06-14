using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Host.Enums;
using IM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    #endregion

    public frmShow(int guestID)
    {
      _guestCurrent = guestID;
      InitializeComponent();
    }

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _dsGuest = ((CollectionViewSource)(this.FindResource("guestViewSource")));

      // Obtenemos la fecha actual
      _dateCurrent = frmHost._dtpServerDate;

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
