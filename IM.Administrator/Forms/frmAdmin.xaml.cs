using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model.Enums;
using IM.Base.Helpers;

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
      var lstMenu = new List<object>();
      #region Sales permision
      if (App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Assitances Status", img = "pack://application:,,,/IM.Base;component/Images/Assistance.ico", form = "frmAssistancesStatus" });
        lstMenu.Add(new { nombre = "Credit Card Types", img = "pack://application:,,,/IM.Base;component/Images/Credit_Cards.png", form = "frmCreditCardTypes" });
      }
      #endregion

      #region HostInvitations Permision
      if (App.User.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Charge To", img = "pack://application:,,,/IM.Base;component/Images/Charge_To.png", form = "frmChargeTo" });
        lstMenu.Add(new { nombre = "Guest Status Types", img = "pack://application:,,,/IM.Base;component/Images/Guests.ico", form = "frmGuestStatusTypes" });
      }
      #endregion

      #region Contracts Permision
      if (App.User.HasPermission(EnumPermission.Contracts, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Contracts", img = "pack://application:,,,/IM.Base;component/Images/Contract.ico", form = "frmContracts" });
      }
      #endregion

      #region Agencies permision
      if (App.User.HasPermission(EnumPermission.Agencies, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Agencies", img = "pack://application:,,,/IM.Base;component/Images/Airplane.ico", form = "frmAgencies" });
        lstMenu.Add(new { nombre = "Countries", img = "pack://application:,,,/IM.Base;component/Images/World.ico", form = "frmCountries" });
        lstMenu.Add(new { nombre = "Reps", img = "pack://application:,,,/IM.Base;component/Images/Rep.png", form = "frmReps" });
        lstMenu.Add(new { nombre = "Markets", img = "pack://application:,,,/IM.Base;component/Images/Market.png", form = "frmMarkets" });
        lstMenu.Add(new { nombre = "Segments By Agency", img = "pack://application:,,,/IM.Base;component/Images/Segments.png", form = "frmSegmentsByAgency" });
        lstMenu.Add(new { nombre = "Segments By Lead Source", img = "pack://application:,,,/IM.Base;component/Images/Segments.png", form = "frmSegmentsByLeadSource" });
        lstMenu.Add(new { nombre = "Segments Categories", img = "pack://application:,,,/IM.Base;component/Images/Segments.png", form = "frmSegmentsCategories" });
      }
      #endregion

      #region Currencies Permision
      if (App.User.HasPermission(EnumPermission.Currencies, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Currencies", img = "pack://application:,,,/IM.Base;component/Images/currency.png", form = "frmCurrencies" });
      }
      #endregion

      #region Sales Permision
      if (App.User.HasPermission(EnumPermission.Sales, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Efficiency Types", img = "pack://application:,,,/IM.Base;component/Images/positioning.png", form = "frmEfficiencyTypes" });
        lstMenu.Add(new { nombre = "Payment Places", img = "pack://application:,,,/IM.Base;component/Images/money.ico", form = "frmPaymentPlaces" });
        lstMenu.Add(new { nombre = "Periods", img = "pack://application:,,,/IM.Base;component/Images/IconDate.png", form = "frmPeriods" });
        lstMenu.Add(new { nombre = "Sources Payments", img = "pack://application:,,,/IM.Base;component/Images/money.ico", form = "frmSourcePayments" });
      }
      #endregion

      #region Folios CxC
      if (App.User.HasPermission(EnumPermission.FolioCXC, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Folios CXC", img = "pack://application:,,,/IM.Base;component/Images/Reports.ico", form = "frmFoliosCXC" });
        lstMenu.Add(new { nombre = "Folios CxC By PR", img = "pack://application:,,,/IM.Base;component/Images/Reports.ico", form = "frmFoliosCxCPR" });                
      }
      #endregion

      #region Folio InvitationsOutHouse
      if (App.User.HasPermission(EnumPermission.FolioInvitationsOuthouse, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Folios Invitations Outhouse", img = "pack://application:,,,/IM.Base;component/Images/Reports.ico", form = "frmFoliosInvitationsOuthouse" });
        lstMenu.Add(new { nombre = "Reason for Cancellation of Folios", img = "pack://application:,,,/IM.Base;component/Images/Forbidden.png", form = "frmReasonCancellationFolios" });
        lstMenu.Add(new { nombre = "Folios Invitations Outhouse by PR", img = "pack://application:,,,/IM.Base;component/Images/Reports.ico", form = "frmFoliosInvitationsOuthouseByPR" });        
      }
      #endregion

      #region Gifts
      if (App.User.HasPermission(EnumPermission.Gifts, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Gifts Categories", img = "pack://application:,,,/IM.Base;component/Images/GiftCategory.png", form = "frmGiftsCategories" });
        lstMenu.Add(new { nombre = "Products", img = "pack://application:,,,/IM.Base;component/Images/Product.ico", form = "frmProducts" });
        //lstMenu.Add(new { nombre = "Gifts", img = "pack://application:,,,/IM.Base;component/Images/Gift.png", form = "frmGifts" });
      }
      #endregion

      #region Locations
      if (App.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Areas", img = "pack://application:,,,/IM.Base;component/Images/World.ico", form = "frmAreas" });
        lstMenu.Add(new { nombre = "Hotels", img = "pack://application:,,,/IM.Base;component/Images/Hotel.png", form = "frmHotels" });
        lstMenu.Add(new { nombre = "Locations", img = "pack://application:,,,/IM.Base;component/Images/locations.png", form = "frmLocations" });
        lstMenu.Add(new { nombre = "Regions", img = "pack://application:,,,/IM.Base;component/Images/World.ico", form = "frmRegions" });
        lstMenu.Add(new { nombre = "Sales Room", img = "pack://application:,,,/IM.Base;component/Images/Sales_Room.png", form = "frmSalesRooms" });
        lstMenu.Add(new { nombre = "Hotel Groups", img = "pack://application:,,,/IM.Base;component/Images/Hotel.png", form = "frmHotelGroups" });
        lstMenu.Add(new { nombre = "Lead Sources", img = "pack://application:,,,/IM.Base;component/Images/Lead_Sources.png", form = "frmLeadSources" });
        lstMenu.Add(new { nombre = "Programs", img = "pack://application:,,,/IM.Base;component/Images/Lead_Sources.png", form = "frmPrograms" });
        lstMenu.Add(new { nombre = "Zones", img = "pack://application:,,,/IM.Base;component/Images/Lead_Sources.png", form = "frmZones" });
      }
      #endregion

      #region Languages
      if (App.User.HasPermission(EnumPermission.Languages,EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Languages", img = "pack://application:,,,/IM.Base;component/Images/World.ico", form = "frmLanguages" });
      }
      #endregion

      #region Marital Status
      if(App.User.HasPermission(EnumPermission.MaritalStatus,EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Marital Status", img = "pack://application:,,,/IM.Base;component/Images/Marital_Status.png", form = "frmMaritalStatus" });
      }
      #endregion

      #region Motives
      if(App.User.HasPermission(EnumPermission.Motives,EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Not Booking Motives", img = "pack://application:,,,/IM.Base;component/Images/DateTime_Forbidden.png", form = "frmNotBookingMotives" });
        lstMenu.Add(new { nombre = "Under Payment  Motives", img = "pack://application:,,,/IM.Base;component/Images/Forbidden.png", form = "frmUnderPaymentMotives" });
        lstMenu.Add(new { nombre = "Unavailable  Motives", img = "pack://application:,,,/IM.Base;component/Images/Forbidden.png", form = "frmUnavailableMotives" });
      }
      #endregion

      #region Teams
      if(App.User.HasPermission(EnumPermission.Teams,EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Posts", img = "pack://application:,,,/IM.Base;component/Images/Posts.png", form = "frmPosts" });
        lstMenu.Add(new { nombre = "Posts Log", img = "pack://application:,,,/IM.Base;component/Images/Log.ico", form = "frmPostsLog" });
        lstMenu.Add(new { nombre = "Teams Log", img = "pack://application:,,,/IM.Base;component/Images/Log.ico", form = "frmTeamsLog" });
      }
      #endregion

      #region Catalogos para Tipo Administrador
      if (App.User.HasRole(EnumRole.Administrator))//Si se tiene permiso como administrador
      {
        lstMenu.Add(new { nombre = "Computers", img = "pack://application:,,,/IM.Base;component/Images/computer.png", form = "frmComputers" });
        lstMenu.Add(new { nombre = "Configuration", img = "pack://application:,,,/IM.Base;component/Images/Configuration.ico", form = "frmConfigurationDetails" });
        lstMenu.Add(new { nombre = "Desks", img = "pack://application:,,,/IM.Base;component/Images/desk.png", form = "frmDesks" });
        lstMenu.Add(new { nombre = "Close Invitation", img = "pack://application:,,,/IM.Base;component/Images/DateTime_Forbidden.png", form = "frmCloseInvitation" });
        lstMenu.Add(new { nombre = "Meal Ticket Types", img = "pack://application:,,,/IM.Base;component/Images/Cofee.png", form = "frmMealTicketsTypes" });
        lstMenu.Add(new { nombre = "Membership Types", img = "pack://application:,,,/IM.Base;component/Images/member.ico", form = "frmMembershipTypes" });
        lstMenu.Add(new { nombre = "Membership Groups", img = "pack://application:,,,/IM.Base;component/Images/member.ico", form = "frmMembershipGroups" });
        lstMenu.Add(new { nombre = "Permissions", img = "pack://application:,,,/IM.Base;component/Images/Police.ico", form = "frmPermissions" });
        lstMenu.Add(new { nombre = "Permissions Levels", img = "pack://application:,,,/IM.Base;component/Images/Police.ico", form = "frmPermissionsLevels" });
        lstMenu.Add(new { nombre = "Place Types", img = "pack://application:,,,/IM.Base;component/Images/Lead_Sources.png", form = "frmPlaceTypes" });
        lstMenu.Add(new { nombre = "Rate Types", img = "pack://application:,,,/IM.Base;component/Images/Reports.ico", form = "frmRateTypes" });
        lstMenu.Add(new { nombre = "Reimpresion Motives", img = "pack://application:,,,/IM.Base;component/Images/Printer.png", form = "frmReimpresionMotives" });
        lstMenu.Add(new { nombre = "Roles", img = "pack://application:,,,/IM.Base;component/Images/Posts.png", form = "frmRoles" });
        lstMenu.Add(new { nombre = "Room Types", img = "pack://application:,,,/IM.Base;component/Images/Bedroom.png", form = "frmRoomTypes" });
        lstMenu.Add(new { nombre = "Sale Types", img = "pack://application:,,,/IM.Base;component/Images/money.ico", form = "frmSaleTypes" });
        lstMenu.Add(new { nombre = "Score Rules Concepts", img = "pack://application:,,,/IM.Base;component/Images/Score.gif", form = "frmScoreRulesConcepts" });
        lstMenu.Add(new { nombre = "Score Rules Types", img = "pack://application:,,,/IM.Base;component/Images/Score.gif", form = "frmScoreRulesTypes" });
        lstMenu.Add(new { nombre = "Score Rules", img = "pack://application:,,,/IM.Base;component/Images/Score.gif", form = "frmScoreRules" });
        lstMenu.Add(new { nombre = "Score Rules By Lead Source", img = "pack://application:,,,/IM.Base;component/Images/Score.gif", form = "frmScoreRulesByLeadSource" });
        lstMenu.Add(new { nombre = "Show Programs", img = "pack://application:,,,/IM.Base;component/Images/Catalog.ico", form = "frmShowPrograms" });
        lstMenu.Add(new { nombre = "Clubs", img = "pack://application:,,,/IM.Base;component/Images/Member.ico", form = "frmClubs" });
        lstMenu.Add(new { nombre = "Depts", img = "pack://application:,,,/IM.Base;component/Images/Posts.png", form = "frmDepts" });
        lstMenu.Add(new { nombre = "Shows Programs Categories", img = "pack://application:,,,/IM.Base;component/Images/Catalog.ico", form = "frmShowProgramsCategories" });
      }
      #endregion

      #region catalogo tipo Secretary
      if(App.User.HasRole(EnumRole.Secretary))
      {
        lstMenu.Add(new { nombre = "Sales Amount Ranges", img = "pack://application:,,,/IM.Base;component/Images/Money_Bag.png", form = "frmSalesAmountRanges" });
        lstMenu.Add(new { nombre = "Goals", img = "pack://application:,,,/IM.Base;component/Images/Goal.png", form = "frmGoals" });
        lstMenu.Add(new { nombre = "Locations Categories", img = "pack://application:,,,/IM.Base;component/Images/locations.png", form = "frmLocationsCategories" });
      }
      #endregion

      #region TourTimes
      if(App.User.HasPermission(EnumPermission.TourTimes,EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Tour Times Schemas", img = "pack://application:,,,/IM.Base;component/Images/IconDate.png", form = "frmTourTimesSchemas" });
        lstMenu.Add(new { nombre = "Tour Times", img = "pack://application:,,,/IM.Base;component/Images/IconDate.png", form = "frmTourTimes" });
      }
      #endregion

      #region Catalogos Default
      lstMenu.Add(new { nombre = "Payment Schemas", img = "pack://application:,,,/IM.Base;component/Images/Payment.png", form = "frmPaymentSchemas" });
      lstMenu.Add(new { nombre = "Payment Types", img = "pack://application:,,,/IM.Base;component/Images/Payment.png", form = "frmPaymentTypes" });
      lstMenu.Add(new { nombre = "Refund Types", img = "pack://application:,,,/IM.Base;component/Images/Money_Bag.png", form = "frmRefundTypes" });
      lstMenu.Add(new { nombre = "Banks", img = "pack://application:,,,/IM.Base;component/Images/money.ico", form = "frmBanks" });
      #endregion

      #region Warehouses
      if (App.User.HasPermission(EnumPermission.Warehouses,EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Warehouses", img = "pack://application:,,,/IM.Base;component/Images/Warehouse.png", form = "frmWarehouses" });
      }
      #endregion

      #region Wholesalers
      if(App.User.HasPermission(EnumPermission.WholeSalers,EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Wholesalers", img = "pack://application:,,,/IM.Base;component/Images/shopping_cart.png", form = "frmWholesalers" });
      }
      #endregion

      #region Personnel
      if (App.User.HasPermission(EnumPermission.Personnel, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Personnel", img = "pack://application:,,,/IM.Base;component/Images/Personnel.png", form = "frmPersonnel" });
      }

      #endregion
      #region sort list
      //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstMenuAdm.ItemsSource);
      //view.SortDescriptions.Add(new System.ComponentModel.SortDescription("nombre", System.ComponentModel.ListSortDirection.Ascending));
      lstMenu.Sort((x, y) => string.Compare(x.GetType().GetProperty("nombre").GetValue(x, null).ToString(), y.GetType().GetProperty("nombre").GetValue(y, null).ToString()));

      #endregion
      lstMenuAdm.ItemsSource = lstMenu;
      int nIndex = 0;
      var personnelCatalog=lstMenu.Where(item => item.GetType().GetProperty("form").GetValue(item,null).ToString().Trim() == "frmPersonnel").FirstOrDefault();      
      if(personnelCatalog!=null)
      {        
        nIndex = lstMenu.IndexOf(personnelCatalog);
      }
      lstMenuAdm.SelectedIndex = nIndex;
      lstMenuAdm.Focus();
      status.Visibility = Visibility.Collapsed;
      StatusBarReg.Content = lstMenuAdm.Items.Count + " Items.";
    }

    #endregion

    #region Open Window
    /// <summary>
    /// Abre la ventana seleccionada
    /// </summary>
    private void OpenWindow()
    {
      dynamic item = lstMenuAdm.SelectedItem;//Obtenemos el seleccionado de la lista
      string strNombreForm = "IM.Administrator.Forms." + item.form;//Obtenemos el nombre del formulario

      //Verificar si la ventana está abierta
      Window wd = Application.Current.Windows.OfType<Window>().Where(x => x.Uid == item.form).FirstOrDefault();

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
          string sNombre = item.nombre;
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
