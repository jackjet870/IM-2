using System.Windows;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Classes;

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

    public ExecuteCommandHelper LoadCombo { get; set; }

    #endregion
    public frmInvitation(EnumInvitationType InvitationType, UserData User, int GuestId, EnumInvitationMode InvitationMode, bool AllowReschedule = true)
    {
      _invitationType = InvitationType;
      _user = User;
      _guestId = GuestId;
      _invitationMode = InvitationMode;

      InitializeComponent();
      LoadCatalog();
      // LoadCombo = new ExecuteCommandHelper(); sirve para cargar la informacion deseada al preciosar combinacion de teclas
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
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

    private async void LoadCatalog()
    {
      var _languages = await BRLanguages.GetLanguages(1);
      var _maritalStatus =await BRMaritalStatus.GetMaritalStatus(1);
      var _personnel = await BRPersonnel.GetPersonnel(_user.User.peID, roles: "PR");
      var _hotels = await BRHotels.GetHotels(nStatus: 1);
      var _agencies = await BRAgencies.GetAgencies(1);
      var _countries = await BRCountries.GetCountries(1);
      var _guestStatusTypes= await BRGuests.GetGuestStatusType(1);
      var _currencies = await BRCurrencies.GetCurrencies(nStatus: 1);
      var _paymenTypes = await BRPaymentTypes.GetPaymentTypes(1);
      var _paymentPlaces = await BRPaymentPlaces.GetPaymentPlaces();
      var _creditCardTypes =await BRCreditCardTypes.GetCreditCardTypes();
      var _gifts = await BRGifts.GetGifts(_user.Location == null ? "ALL" : _user.Location.loID, 1);
      var _salesRooms = BRSalesRooms.GetSalesRooms(0);
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
