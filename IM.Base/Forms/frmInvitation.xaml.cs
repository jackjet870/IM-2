using System.Windows;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Classes;
using System.Collections.Generic;
using IM.Model;
using System.Collections.ObjectModel;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmInvitation.xaml
  /// </summary>
  public partial class frmInvitation : Window
  {
    #region Propiedades, Atributos
    public readonly UserData _user;
    private readonly EnumInvitationType _invitationType;
    private readonly int _guestId;
    private readonly EnumInvitationMode _invitationMode;
    private readonly bool _allowSchedule;
    List<LanguageShort> _languagesCat;
    List<MaritalStatus> _maritalStatusCat;
    List<PersonnelShort> _personnelCat;
    List<Hotel> _hotelsCat;
    List<AgencyShort> _agenciesCat;
    List<CountryShort> _countriesCat;
    List<GuestStatusType> _guestStatusTypesCat;
    List<Currency> _currenciesCat;
    List<PaymentType> _paymenTypesCat;
    List<PaymentPlace> _paymentPlacesCat;
    List<CreditCardType> _creditCardTypesCat;
    List<GiftShort> _giftsCat;
    List<SalesRoomShort> _salesRoomsCat;

    public ExecuteCommandHelper LoadCombo { get; set; }

    #endregion
    public frmInvitation(EnumInvitationType InvitationType, UserData User, int GuestId, EnumInvitationMode InvitationMode, bool AllowReschedule = true)
    {
      _invitationType = InvitationType;
      _user = User;
      _guestId = GuestId;
      _invitationMode = InvitationMode;
      InitializeComponent();

      // LoadCombo = new ExecuteCommandHelper(); sirve para cargar la informacion deseada al preciosar combinacion de teclas
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadCommonCatalog();
      ControlsConfiguration(_invitationType);//Cargamos la UI dependiendo del tipo de Invitacion

    }

    #region Metodos
    /// <summary>
    /// Prepara los controles para cada invitacion
    /// </summary>
    /// <param name="_invitationType">EnumInvitationType</param>
    /// [erosado] 16/05/2016  Created
    private void ControlsConfiguration(EnumInvitationType _invitationType)
    {
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
          break;
        default:
          break;
      }
    }

    private void LoadCommonCatalog()
    {
      try
      {
        txtUserName.Text = _user.User.peN;
        //txtUserName.Text = _user.Location ??  _user.SalesRoom.srN  ;
        loadLenguages();
        loadMaritalStatus();
        loadPersonnel();
        loadHotels();
        loadAgencies();
        loadCountries();
        loadGuestStatusType();
        loadPaymentTypes();
        loadPaymentPlaces();
        loadCreditCardTypes();
      }
      catch (System.Exception ex)
      {
        UIHelper.ShowMessage(ex, MessageBoxImage.Error, "Invitation");
      }

    }
    #endregion

    #region  LoadCombos
    #region Languages
    private async void loadLenguages()
    {
      var _languages = await BRLanguages.GetLanguages(1);
    }
    #endregion

    #region MaritalStatus
    private async void loadMaritalStatus()
    {
      _maritalStatusCat = await BRMaritalStatus.GetMaritalStatus(1);
      cmbMaritalStatusGuest1.ItemsSource =
        cmbMaritalStatusGuest2.ItemsSource = _maritalStatusCat;
    }
    #endregion

    #region Personnel
    private async void loadPersonnel()
    {
      _personnelCat = await BRPersonnel.GetPersonnel(_user.User.peID, roles: "PR");
      cmbPR.ItemsSource = cmbPRContract.ItemsSource = _personnelCat;
    }
    #endregion

    #region Hotels
    private async void loadHotels()
    {
      _hotelsCat = await BRHotels.GetHotels(nStatus: 1);
      cmbOtherInfoHotel.ItemsSource =
      cmbResorts.ItemsSource = _hotelsCat;
    }
    #endregion

    #region Agencies
    private async void loadAgencies()
    {
      _agenciesCat = await BRAgencies.GetAgencies(1);
      cmbOtherInfoAgency.ItemsSource = _agenciesCat;
    }
    #endregion

    #region Countries
    private async void loadCountries()
    {
      _countriesCat = await BRCountries.GetCountries(1);
      cmbOtherInfoCountry.ItemsSource = _countriesCat;
    }
    #endregion

    #region GuestStatusType
    private async void loadGuestStatusType()
    {
      _guestStatusTypesCat = await BRGuests.GetGuestStatusType(1);
      cmbGuestStatus.ItemsSource = _guestStatusTypesCat;
    }
    #endregion

    #region Currencies
    private async void loadCurrencies()
    {
      _currenciesCat = await BRCurrencies.GetCurrencies(nStatus: 1);
      cmbCurrency.ItemsSource = _currenciesCat;
    }
    #endregion

    #region PaymentTypes
    private async void loadPaymentTypes()
    {
      _paymenTypesCat = await BRPaymentTypes.GetPaymentTypes(1);
      cmbPaymentType.ItemsSource = _guestStatusTypesCat;
    }
    #endregion

    #region PaymentPlaces
    private async void loadPaymentPlaces()
    {
      _paymentPlacesCat = await BRPaymentPlaces.GetPaymentPlaces();

    }
    #endregion

    #region CreditCardTypes
    private async void loadCreditCardTypes()
    {
      _creditCardTypesCat = await BRCreditCardTypes.GetCreditCardTypes();
    }
    #endregion

    #region Gifts
    private async void loadGifts()
    {
      _giftsCat = await BRGifts.GetGiftsShort(_user.Location == null ? "ALL" : _user.Location.loID, 1);
    }
    #endregion

    #region SalesRooms
    private async void loadSalesRooms()
    {
      _salesRoomsCat = await BRSalesRooms.GetSalesRooms(0);
    }
    #endregion

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
      stkRescheduleTime.Visibility = Visibility.Collapsed;
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
         
  }
}
