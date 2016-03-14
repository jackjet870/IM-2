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
      if (AddCatalog("SALES"))
      {
        lstMenu.Add(new { nombre = "Assitances Status", img = "pack://application:,,,/IM.Base;component/Images/Assistance.ico", form = "frmAssistancesStatus" });
        lstMenu.Add(new { nombre = "Credit Card Types", img = "pack://application:,,,/IM.Base;component/Images/Credit_Cards.png", form = "frmCreditCardTypes" });
      }
      if (AddCatalog("LOCATIONS")) { lstMenu.Add(new { nombre = "Areas", img = "pack://application:,,,/IM.Base;component/Images/World.ico", form = "frmAreas" }); }
      if (AddCatalog("HOSTINVIT")){ lstMenu.Add(new { nombre = "Charge To", img = "pack://application:,,,/IM.Base;component/Images/Charge_To.png", form = "frmChargeTo" });}
      if (AddCatalog("CONTRACTS")) { lstMenu.Add(new { nombre = "Contracts", img = "pack://application:,,,/IM.Base;component/Images/Contract.ico", form = "frmContracts" }); }
      if (AddCatalog("AGENCIES"))
      {
        lstMenu.Add(new { nombre = "Agencies", img = "pack://application:,,,/IM.Base;component/Images/Airplane.ico", form = "frmAgencies" });
      }
      if (AddCatalog("CURRENCIES"))
      {
        lstMenu.Add(new { nombre = "Currencies", img = "pack://application:,,,/IM.Base;component/Images/currency.png", form = "frmCurrencies" });
      }
      lstMenuAdm.ItemsSource = lstMenu;

      #region sort list
      CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstMenuAdm.ItemsSource);
      view.SortDescriptions.Add(new System.ComponentModel.SortDescription("nombre", System.ComponentModel.ListSortDirection.Ascending));
      #endregion
      
    }

    #endregion

    #region AddCatalog
    /// <summary>
    /// Validar si cuenta con el permiso para visualizar el catalogo
    /// </summary>
    /// <param name="sNameCat">Nombre del permiso a Agregar</param>
    /// <returns>true. tiene permiso | false. no tiene permiso</returns>
    /// <history>
    /// [emoguel] 03/03/2016
    /// </history>
    protected bool AddCatalog(string sNameCat)
    {

      return App.User.HasPermission(sNameCat, EnumPermisionLevel.ReadOnly);
      
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
      dynamic item = lstMenuAdm.SelectedItem;//Obtenemos el seleccionado de la lista
      string strNombreForm = "IM.Administrator.Forms."+ item.form;//Obtenemos el nombre del formulario

      //Verificar si la ventana está abierta
      Window wd = Application.Current.Windows.OfType<Window>().Where(x => x.Uid == item.form).FirstOrDefault();

      if (wd == null)//Se crea la ventana
      {
        Type type = System.Reflection.Assembly.GetExecutingAssembly().GetType(strNombreForm);
        if (type!=null)//Verificar si la ventana existe
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
  }
}
