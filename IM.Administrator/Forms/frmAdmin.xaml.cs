﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model.Enums;

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
      }
      #endregion

      #region Folios CxC
      if (App.User.HasPermission(EnumPermission.FolioCXC, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Folios CXC", img = "pack://application:,,,/IM.Base;component/Images/Reports.ico", form = "frmFoliosCXC" });
      }
      #endregion

      #region Folio InvitationsOutHouse
      if (App.User.HasPermission(EnumPermission.FolioInvitationsOuthouse, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Folios Invitations Outhouse", img = "pack://application:,,,/IM.Base;component/Images/Reports.ico", form = "frmFoliosInvitationsOuthouse" });
        lstMenu.Add(new { nombre = "Reason for Cancellation of Folios", img = "pack://application:,,,/IM.Base;component/Images/Forbidden.png", form = "frmReasonCancellationFolios" });
      }
      #endregion

      #region Gifts
      if (App.User.HasPermission(EnumPermission.Gifts, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Gifts Categories", img = "pack://application:,,,/IM.Base;component/Images/GiftCategory.png", form = "frmGiftsCategories" });
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
      }
      #endregion

      #region Teams
      if(App.User.HasPermission(EnumPermission.Teams,EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Posts", img = "pack://application:,,,/IM.Base;component/Images/Posts.png", form = "frmPosts" });
        lstMenu.Add(new { nombre = "Posts Log", img = "pack://application:,,,/IM.Base;component/Images/Log.ico", form = "frmPostsLog" });
      }
      #endregion

      #region Catalogos para Tipo Administrador
      if (App.User.HasRole(EnumRole.Administrator))//Si se tiene permiso como administrador
      {
        lstMenu.Add(new { nombre = "Computers", img = "pack://application:,,,/IM.Base;component/Images/computer.png", form = "frmComputers" });
        lstMenu.Add(new { nombre = "Desks", img = "pack://application:,,,/IM.Base;component/Images/desk.png", form = "frmDesks" });
        lstMenu.Add(new { nombre = "Meal Ticket Types", img = "pack://application:,,,/IM.Base;component/Images/Cofee.png", form = "frmMealTicketsTypes" });
        lstMenu.Add(new { nombre = "Membership Types", img = "pack://application:,,,/IM.Base;component/Images/member.ico", form = "frmMembershipTypes" });
        lstMenu.Add(new { nombre = "Permissions", img = "pack://application:,,,/IM.Base;component/Images/Police.ico", form = "frmPermissions" });
        lstMenu.Add(new { nombre = "Permissions Levels", img = "pack://application:,,,/IM.Base;component/Images/Police.ico", form = "frmPermissionsLevels" });
        lstMenu.Add(new { nombre = "Place Types", img = "pack://application:,,,/IM.Base;component/Images/Lead_Sources.png", form = "frmPlaceTypes" });
        lstMenu.Add(new { nombre = "Rate Types", img = "pack://application:,,,/IM.Base;component/Images/Reports.ico", form = "frmRateTypes" });
        lstMenu.Add(new { nombre = "Reimpresion Motives", img = "pack://application:,,,/IM.Base;component/Images/Printer.png", form = "frmReimpresionMotives" });
        lstMenu.Add(new { nombre = "Roles", img = "pack://application:,,,/IM.Base;component/Images/Posts.png", form = "frmRoles" });
        lstMenu.Add(new { nombre = "Room Types", img = "pack://application:,,,/IM.Base;component/Images/Bedroom.png", form = "frmRoomTypes" });
        lstMenu.Add(new { nombre = "Sale Types", img = "pack://application:,,,/IM.Base;component/Images/money.ico", form = "frmSaleTypes" });
        lstMenu.Add(new { nombre = "Score Rules Concepts", img = "pack://application:,,,/IM.Base;component/Images/Score.gif", form = "frmScoreRulesConcepts" });
      }
      #endregion

      #region catalogo tipo Secretary
      if(App.User.HasRole(EnumRole.Secretary))
      {
        lstMenu.Add(new { nombre = "Sales Amount Ranges", img = "pack://application:,,,/IM.Base;component/Images/Money_Bag.png", form = "frmSalesAmountRanges" });
      }
      #endregion

      #region Catalogos Default
      lstMenu.Add(new { nombre = "Payment Schemas", img = "pack://application:,,,/IM.Base;component/Images/Payment.png", form = "frmPaymentSchemas" });
      lstMenu.Add(new { nombre = "Payment Types", img = "pack://application:,,,/IM.Base;component/Images/Payment.png", form = "frmPaymentTypes" });
      lstMenu.Add(new { nombre = "Refund Types", img = "pack://application:,,,/IM.Base;component/Images/Money_Bag.png", form = "frmRefundTypes" });
      #endregion

      lstMenuAdm.ItemsSource = lstMenu;
      
      #region sort list
      CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstMenuAdm.ItemsSource);
      view.SortDescriptions.Add(new System.ComponentModel.SortDescription("nombre", System.ComponentModel.ListSortDirection.Ascending));
      #endregion

      lstMenuAdm.SelectedIndex = 0;
      lstMenuAdm.Focus();
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
    #endregion


  }
}
