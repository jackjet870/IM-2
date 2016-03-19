using System;
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

      #region Location Permision
      if (App.User.HasPermission(EnumPermission.Locations, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Areas", img = "pack://application:,,,/IM.Base;component/Images/World.ico", form = "frmAreas" });
      }
      #endregion

      #region HostInvitations Permision
      if (App.User.HasPermission(EnumPermission.HostInvitations, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Charge To", img = "pack://application:,,,/IM.Base;component/Images/Charge_To.png", form = "frmChargeTo" });
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
      }
      #endregion

      #region Currencies Permision
      if (App.User.HasPermission(EnumPermission.Currencies, EnumPermisionLevel.ReadOnly))
      {
        lstMenu.Add(new { nombre = "Currencies", img = "pack://application:,,,/IM.Base;component/Images/currency.png", form = "frmCurrencies" });
      }
      #endregion
      
      #region Sale Permision
      if(App.User.HasPermission(EnumPermission.Sales,EnumPermisionLevel.Standard))
      {
        lstMenu.Add(new { nombre = "Efficiency Types", img = "pack://application:,,,/IM.Base;component/Images/positioning.png", form = "frmEfficiencyTypes" });
      }
      #endregion

      #region Catalogos para Tipo Administrador
      if (App.User.HasRole(EnumRole.Administrator))//Si se tiene permiso como administrador
      {
        lstMenu.Add(new { nombre = "Computers", img = "pack://application:,,,/IM.Base;component/Images/computer.png", form = "frmComputers" });
        lstMenu.Add(new { nombre = "Desks", img = "pack://application:,,,/IM.Base;component/Images/desk.png", form = "frmDesks" });
      }
      #endregion
      lstMenuAdm.ItemsSource = lstMenu;
     
      #region Catalogos default
      
      #endregion
      
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
        //wd.Focus();
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
