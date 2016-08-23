using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.Model.Classes;

namespace IM.Administrator.Forms
{
  /// <summary>
  /// Interaction logic for frmAdmin.xaml
  /// </summary>
  public partial class frmAdmin : Window
  {
    #region Constructores y destructores

    public frmAdmin()
    {
      InitializeComponent();      
    }

    #endregion

    #region Metodos

    #region CreateMenu

    /// <summary>
    /// Crea la lista del Menu
    /// </summary>
    /// <history>
    /// [Emoguel] created
    /// </history>
    protected void CreateMenu()
    {      
      status.Visibility = Visibility.Visible;
      lblUser.Content = App.User.User.peN;
      var lstMenu = new ItemsList();

      #region Sales Permision
      if (App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add("frmAssistancesStatus", "Assitances Status", "Assistance.ico");
        lstMenu.Add("frmCreditCardTypes", "Credit Card Types", "Credit_Cards.png");
        lstMenu.Add("frmEfficiencyTypes", "Efficiency Types", "Positioning.png");
        lstMenu.Add("frmPaymentPlaces", "Payment Places", "Money.ico");
        lstMenu.Add("frmPeriods", "Periods", "IconDate.png");
        lstMenu.Add("frmSourcePayments", "Sources Payments", "Money.ico");
        lstMenu.Add("frmPaymentTypes", "Payment Types", "Payment.png");
        lstMenu.Add("frmSeasons", "Seasons", "Season.ico");
      }
      #endregion

      #region Host Invitations Permision
      if (App.User.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmChargeTo", "Charge To", "Charge_To.png");
        lstMenu.Add("frmGuestStatusTypes", "Guest Status Types", "Guests.ico");
      }
      #endregion

      #region Contracts Permision
      if (App.User.HasPermission(EnumPermission.Contracts, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmContracts", "Contracts", "Contract.ico");
      }
      #endregion

      #region Agencies Permision
      if (App.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmAgencies", "Agencies", "Airplane.ico");
        lstMenu.Add("frmCountries", "Countries", "World.ico");
        lstMenu.Add("frmReps", "Reps", "Rep.png");
        lstMenu.Add("frmMarkets", "Markets", "Market.png");
        lstMenu.Add("frmSegmentsByAgency", "Segments By Agency", "Segments.png");
        lstMenu.Add("frmSegmentsByLeadSource", "Segments By Lead Source", "Segments.png");
        lstMenu.Add("frmSegmentsCategories", "Segments Categories", "Segments.png");
      }
      #endregion

      #region Currencies Permision
      if (App.User.HasPermission(EnumPermission.Currencies, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmCurrencies", "Currencies", "currency.png");
      }
      #endregion

      #region Folio Invitations OutHouse Permission
      if (App.User.HasPermission(EnumPermission.FoliosInvitationsOuthouse, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmFoliosInvitationsOuthouse", "Folios Invitations Outhouse", "Reports.ico");
        lstMenu.Add("frmReasonCancellationFolios", "Reason for Cancellation of Folios", "Forbidden.png");
      }
      #endregion

      #region Gifts Permission
      if (App.User.HasPermission(EnumPermission.Gifts, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmGiftsCategories", "Gifts Categories", "GiftCategory.png");
        lstMenu.Add("frmProducts", "Products", "Product.ico");
        lstMenu.Add("frmGifts", "Gifts", "Gift.png");
      }
      #endregion

      #region Locations Permission
      if (App.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmAreas", "Areas", "World.ico");
        lstMenu.Add("frmHotels", "Hotels", "Hotel.png");
        lstMenu.Add("frmLocations", "Locations", "Locations.png");
        lstMenu.Add("frmRegions", "Regions", "World.ico");
        lstMenu.Add("frmSalesRooms", "Sales Rooms", "Sales_Room.png");
        lstMenu.Add("frmHotelGroups", "Hotel Groups", "Hotel.png");
        lstMenu.Add("frmLeadSources", "Lead Sources", "Lead_Sources.png");
        lstMenu.Add("frmPrograms", "Programs", "Lead_Sources.png");
        lstMenu.Add("frmZones", "Zones", "Lead_Sources.png");
      }
      #endregion

      #region Languages Permission
      if (App.User.HasPermission(EnumPermission.Languages, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmLanguages", "Languages", "World.ico");
      }
      #endregion

      #region Marital Status Permission
      if (App.User.HasPermission(EnumPermission.MaritalStatus, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmMaritalStatus", "Marital Status", "Marital_Status.png");
      }
      #endregion

      #region Motives Permission
      if (App.User.HasPermission(EnumPermission.Motives, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmNotBookingMotives", "Not Booking Motives", "DateTime_Forbidden.png");
        lstMenu.Add("frmUnderPaymentMotives", "Under Payment  Motives", "Forbidden.png");
        lstMenu.Add("frmUnavailableMotives", "Unavailable  Motives", "Forbidden.png");
      }
      #endregion

      #region Teams Permission
      if (App.User.HasPermission(EnumPermission.Teams, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmPosts", "Posts", "Posts.png");
        lstMenu.Add("frmPostsLog", "Posts Log", "Log.ico");
        lstMenu.Add("frmTeamsLog", "Teams Log", "Log.ico");
        lstMenu.Add("frmTeamsPRs", "Teams PR´s", "PR.png");
        lstMenu.Add("frmTeamsSalesmen", "Teams Salesmen", "Salesmen.png");
      }
      #endregion

      #region Administrator Role
      if (App.User.HasRole(EnumRole.Administrator))//Si se tiene permiso como administrador
      {
        lstMenu.Add("frmComputers", "Computers", "Computer.png");
        lstMenu.Add("frmConfigurationDetails", "Configuration", "Configuration.ico");
        lstMenu.Add("frmDesks", "Desks", "Desk.png");
        lstMenu.Add("frmMealTicketsTypes", "Meal Ticket Types", "Cofee.png");
        lstMenu.Add("frmMembershipTypes", "Membership Types", "Member.ico");
        lstMenu.Add("frmMembershipGroups", "Membership Groups", "Member.ico");
        lstMenu.Add("frmPermissions", "Permissions", "Police.ico");
        lstMenu.Add("frmPermissionsLevels", "Permissions Levels", "Police.ico");
        lstMenu.Add("frmPlaceTypes", "Place Types", "Lead_Sources.png");
        lstMenu.Add("frmRateTypes", "Rate Types", "Reports.ico");
        lstMenu.Add("frmReimpresionMotives", "Reimpresion Motives", "Printer.png");
        lstMenu.Add("frmRoles", "Roles", "Posts.png");
        lstMenu.Add("frmRoomTypes", "Room Types", "Bedroom.png");
        lstMenu.Add("frmSaleTypes", "Sale Types", "Money.ico");
        lstMenu.Add("frmScoreRulesConcepts", "Score Rules Concepts", "Score.gif");
        lstMenu.Add("frmScoreRulesTypes", "Score Rules Types", "Score.gif");
        lstMenu.Add("frmScoreRules", "Score Rules", "Score.gif");
        lstMenu.Add("frmScoreRulesByLeadSource", "Score Rules By Lead Source", "Score.gif");
        lstMenu.Add("frmShowPrograms", "Show Programs", "Catalog.ico");
        lstMenu.Add("frmClubs", "Clubs", "Member.ico");
        lstMenu.Add("frmDepts", "Depts", "Posts.png");
        lstMenu.Add("frmShowProgramsCategories", "Shows Programs Categories", "Catalog.ico");
        lstMenu.Add("frmPaymentSchemas", "Payment Schemas", "Payment.png");
        lstMenu.Add("frmRefundTypes", "Refund Types", "Money_Bag.png");
        lstMenu.Add("frmBanks", "Banks", "Money.ico");
        lstMenu.Add("frmSaleTypesCategories", "Sale Types Categories", "Money.ico");
      }
      #endregion

      #region Secretary Role
      if (App.User.HasRole(EnumRole.Secretary) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmSalesAmountRanges", "Sales Amount Ranges", "Money_Bag.png");
        lstMenu.Add("frmGoals", "Goals", "Goal.png");
        lstMenu.Add("frmLocationsCategories", "Locations Categories", "Locations.png");
      }
      #endregion

      #region Tour Times Permission
      if (App.User.HasPermission(EnumPermission.TourTimes, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmTourTimesSchemas", "Tour Times Schemas", "IconDate.png");
        lstMenu.Add("frmTourTimes", "Tour Times", "IconDate.png");
      }
      #endregion

      #region Warehouses Permission
      if (App.User.HasPermission(EnumPermission.Warehouses, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmWarehouses", "Warehouses", "Warehouse.png");
      }
      #endregion

      #region Wholesalers Permission
      if (App.User.HasPermission(EnumPermission.WholeSalers, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmWholesalers", "Wholesalers", "shopping_cart.png");
      }
      #endregion

      #region Personnel Permission
      if (App.User.HasPermission(EnumPermission.Personnel, EnumPermisionLevel.ReadOnly) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmPersonnel", "Personnel", "Personnel.png");
      }

      #endregion

      #region PRCaptian Roles
      if (App.User.HasRole(EnumRole.PRCaptain) || App.User.HasRole(EnumRole.Administrator))
      {
        lstMenu.Add("frmNotices", "Notices", "Notice.png");
      }
      #endregion

      // ordenamos la lista
      lstMenu.Sort((x, y) => string.Compare(x.Name, y.Name));

      lstMenuAdm.ItemsSource = lstMenu;

      // seleccionamos por default el catalogo de Personnel
      int nIndex = 0;
      Item personnelCatalog = lstMenu.Where(item => item.Id == "frmPersonnel").FirstOrDefault();      
      if(personnelCatalog != null)
      {        
        nIndex = lstMenu.IndexOf(personnelCatalog);
      }
      lstMenuAdm.SelectedIndex = nIndex;

      lstMenuAdm.Focus();
      status.Visibility = Visibility.Collapsed;

      // indicamos el numero de catalogos
      StatusBarReg.Content = $"{lstMenuAdm.Items.Count} Catalogs.";
    }

    #endregion

    #region Open Window
    /// <summary>
    /// Abre la ventana seleccionada
    /// </summary>
    private void OpenWindow()
    {
      Item item = (Item)lstMenuAdm.SelectedItem;//Obtenemos el seleccionado de la lista
      string strNombreForm = $"IM.Administrator.Forms.{item.Id}";//Obtenemos el nombre del formulario

      //Verificar si la ventana está abierta
      Window wd = Application.Current.Windows.OfType<Window>().Where(x => x.Uid == item.Id).FirstOrDefault();

      if (wd == null)//Se crea la ventana
      {
        Type type = System.Reflection.Assembly.GetExecutingAssembly().GetType(strNombreForm);
        if (type != null)//Verificar si la ventana existe
        {
          System.Runtime.Remoting.ObjectHandle handle = Activator.CreateInstance(null, strNombreForm);
          Window wdwWindow = (Window)handle.Unwrap();
          wdwWindow.Owner = this;
          wdwWindow.Show();
        }
        else
        {
          string sNombre = item.Name;
          MessageBox.Show("could not show the window " + sNombre);
        }
      }
      else//Se pone el foco en la ventana
      {
        wd.Activate();
      }
    }
    #endregion
    #endregion

    #region Eventos del formulario

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      CreateMenu();
    }
    #endregion

    #region Window_Closed
    private void Window_Closed(object sender, EventArgs e)
    {
      Application.Current.Shutdown(); 

    }
    #endregion

    #region lstMenuAdm_MouseDoubleClick

    /// <summary>
    /// Abre la ventana de 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void lstMenuAdm_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      OpenWindow();
    }
    #endregion

    #region KeyDown
    /// <summary>
    /// Abre la ventana seleccionada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [created] emoguel 15/03/2016
    /// </history>
    private void lstMenuAdm_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key==Key.Enter)
      {
        OpenWindow();
      }
    }
    #endregion

    #region Window_IsKeyboarFocusedChanged
    /// <summary>
    ///   Verfica que teclas estan presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 17/Jun/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    ///   Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 17/Jun/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
      }
    }
    #endregion

    #endregion
  }
}
