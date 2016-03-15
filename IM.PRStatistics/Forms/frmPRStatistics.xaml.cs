using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IM.Model;
using IM.BusinessRules.BR;
using IM.PRStatistics.Utilities;
using OfficeOpenXml.Style;
using System.IO;
using System.Diagnostics;
using System.Data;
using IM.Model.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.Model.Enums;

namespace IM.PRStatistics.Forms
{
  /// <summary>
  /// Interaction logic for frmPRStatistics.xaml
  /// </summary>
  public partial class frmPRStatistics : Window
  {
    #region Propiedades y Atributos
    List<Tuple<string,string>> filterTuple= null; // Agrega los filtros de busqueda
    public UserData userData { get; set; } // Contiene la informacion del usuario logeado

    #endregion

    #region Constructores y  Destructores
    public frmPRStatistics(UserData userData)
    {
      this.userData = userData;
      InitializeComponent();
    }
    #endregion

    #region Eventos de la ventana
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Inicializamos los catalogos
      DoGetLeadSources(userData.User.peID);
      DoGetSalesRooms(userData.User.peID);
      DoGetCountries();
      DoGetAgencies();
      DoGetMarkets();
      //Seleccionamos los días en el datapicker 
      dtpkFrom.SelectedDate = DateTime.Now.AddDays(-7);
      dtpkTo.SelectedDate = DateTime.Now;
    }
    /// <summary>
    /// Evento que se encarga de generar las estadisticas
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void imgButtonOk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (lsbxLeadSources.SelectedItems.Count > 0 && lsbxSalesRooms.SelectedItems.Count > 0
              && lsbxCountries.SelectedItems.Count > 0 && lsbxAgencies.SelectedItems.Count > 0
              && lsbxMarkets.SelectedItems.Count > 0)
      {

        filterTuple = new List<Tuple<string, string>>();
        StaStart("Loading Report...");
        imgButtonOk.IsEnabled = false;
        filterTuple.Add(new Tuple<string, string>("LeadSource", chbxLeadSources.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxLeadSources)));
        filterTuple.Add(new Tuple<string, string>("SalesRooms", chbxSalesRooms.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxSalesRooms)));
        filterTuple.Add(new Tuple<string, string>("Countries", chbxCountries.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxCountries)));
        filterTuple.Add(new Tuple<string, string>("Agencies", chbxAgencies.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxAgencies)));
        filterTuple.Add(new Tuple<string, string>("Markets", chbxMarkets.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxMarkets)));
    
        DoGetRptPrStats(dtpkFrom.SelectedDate.Value, dtpkTo.SelectedDate.Value, filterTuple);
      }
      else
      {
        UIHelper.ShowMessage("Select at least one item from each catalog ", MessageBoxImage.Warning);
      }
    }
    /// <summary>
    /// Evento que se encarga de generar el reporte de las estadisticas mostradas en el grid
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      List<RptPRStats> lstRptStats = dtgr.DataContext as List<RptPRStats>;
      if (lstRptStats != null )
      {
        string dateRange = DateHelper.DateRange(dtpkFrom.SelectedDate.Value,dtpkTo.SelectedDate.Value);
        FileInfo templatePath = new FileInfo(string.Concat(Directory.GetCurrentDirectory(), "\\ReportTemplate\\RptPRStatistics.xlsx"));
        DataTable dt = GridHelper.GetDataTableFromGrid<RptPRStats>(lstRptStats);
        Tuple<string, string> nombreReporte = new Tuple<string, string>("PR Statistics", dateRange);
        FileInfo finfo = IM.Base.Helpers.EpplusHelper.CreateRptExcelWithOutTemplate(filterTuple, dt, nombreReporte, UsefulMethods.getExcelFormatTable());
        if (finfo != null)
        {
          Process.Start(finfo.FullName);
        }
      }
      else
      {
        UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
      }
    }
    /// <summary>
    /// Eventos que muestra la ventana About
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void imgButtonAbout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      frmAbout frmAbout = new frmAbout();
      frmAbout.ShowDialog();
    }
    /// <summary>
    /// Evento que se encarga de cerrar la aplicacion.
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void imgButtonExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.Close();
    }

    #endregion

    #region StatusBar
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }

    /// <summary>
    /// Verifica que la tecla se encuentre activa/inactiva, para cambiar el estilo de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void CkeckKeysPress(System.Windows.Controls.Primitives.StatusBarItem statusBar, Key key)
    {
      var keyPess = Keyboard.GetKeyStates(key).ToString();

      if (keyPess.Contains("Toggled")) //si está activo el Bloq Mayús
      {
        statusBar.FontWeight = FontWeights.Bold;
        statusBar.Foreground = Brushes.Black;
      }
      else
      {
        KeyDefault(statusBar);
      }
    }

    /// <summary>
    /// Configuracion inicial de los StatusBarItem.
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void KeyDefault(System.Windows.Controls.Primitives.StatusBarItem statusBar)
    {
      statusBar.FontWeight = FontWeights.Normal;
      statusBar.Foreground = Brushes.Gray;
    }

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaStart(String message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
      
    }

    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    /// <param name="message">mensaje</param>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
    }

    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      CkeckKeysPress(StatusBarCap, Key.Capital);
      CkeckKeysPress(StatusBarIns, Key.Insert);
      CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Async Methods
    /// <summary>
    /// Obtiene el catalogo de LeadSources
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public void DoGetLeadSources(string user)
    {
      Task.Factory.StartNew(() => BRLeadSources.GetLeadSourcesByUser(user, "ALL", "ALL"))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            task1.Wait(1000);
            List<LeadSourceByUser> data = task1.Result;
            if (data.Count > 0)
            {
              lsbxLeadSources.DataContext = data;
            }
          }
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }

    /// <summary>
    /// Obtiene el catalogo de SalesRooms
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public void DoGetSalesRooms(string user)
    {
      Task.Factory.StartNew(() => BRSalesRooms.GetSalesRoomsByUser(user, "ALL"))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            task1.Wait(1000);
            List<SalesRoomByUser> data = task1.Result;
            if (data.Count > 0)
            {
              lsbxSalesRooms.DataContext = data;
              chbxSalesRooms.IsChecked = true;
            }
          }
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    /// <summary>
    /// Obtiene el catalogo de countries
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public void DoGetCountries()
    {
      Task.Factory.StartNew(() => BRCountries.GetCountries(1))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            task1.Wait(1000);
            List<CountryShort> data = task1.Result;
            if (data.Count > 0)
            {
              lsbxCountries.DataContext = data;
              chbxCountries.IsChecked = true;
            }
          }
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }

    /// <summary>
    /// Obtiene el catalogo de Agencies
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public void DoGetAgencies()
    {
      Task.Factory.StartNew(() => BRAgencies.GetAgencies(1))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            task1.Wait(1000);
            List<AgencyShort> data = task1.Result;
            if (data.Count > 0)
            {
              lsbxAgencies.DataContext = data;
              chbxAgencies.IsChecked = true;
            }
          }
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    /// <summary>
    /// Obtiene el catalogo de Markets
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public void DoGetMarkets()
    {
      Task.Factory.StartNew(() => BRMarkets.GetMarkets(1))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            task1.Wait(1000);
            List<MarketShort> data = task1.Result;
            if (data.Count > 0)
            {
              lsbxMarkets.DataContext = data;
              chbxMarkets.IsChecked = true;
            }
          }
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }

    /// <summary>
    /// Obtiene el resultado de las estadisticas
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public void DoGetRptPrStats(DateTime dateFrom, DateTime dateTo, List<Tuple<string, string>> filtros)
    {
      Task.Factory.StartNew(() => BRPrStats.GetPrStats(dateFrom, dateTo, filtros))
      .ContinueWith(
      (task1) =>
      {
        if (task1.IsFaulted)
        {
          UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
          StaEnd();
          imgButtonOk.IsEnabled = true;
          return false;
        }
        else
        {
          if (task1.IsCompleted)
          {
            List<Model.RptPRStats> data = task1.Result;
            if (data.Count > 0)
            {
              dtgr.DataContext = data;
              StatusBarReg.Content = string.Format("{0}/{1}", (dtgr.SelectedIndex + 1).ToString(), dtgr.Items.Count.ToString());
              
            }
            else
            {
              UIHelper.ShowMessage("There is no data");
            }
            StaEnd();
            imgButtonOk.IsEnabled = true;
          }
          return false;
        }
      },
      TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    #endregion

    #region Check & UnCheck
    /// <summary>
    /// Selecciona todos los elementos de las listas
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void chbx_Checked(object sender, RoutedEventArgs e)
    {
      if (sender.GetType().Name == "CheckBox")
      {
        string chbxName = ((CheckBox)(sender)).Content.ToString();

        switch (chbxName)
        {
          case "All Lead Sources":
            this.lsbxLeadSources.IsEnabled = false;
            this.lsbxLeadSources.SelectAll();
            break;
          case "All Sales Rooms":
            this.lsbxSalesRooms.IsEnabled = false;
            this.lsbxSalesRooms.SelectAll();
            break;
          case "All Countries":
            this.lsbxCountries.IsEnabled = false;
            this.lsbxCountries.SelectAll();
            break;
          case "All Agencies":
            this.lsbxAgencies.IsEnabled = false;
            this.lsbxAgencies.SelectAll();
            break;
          case "All Markets":
            this.lsbxMarkets.IsEnabled = false;
            this.lsbxMarkets.SelectAll();
            break;
          default:
            break;
        }
      }
    }
    /// <summary>
    /// Deselecciona los elementos de las listas
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void chbx_Unchecked(object sender, RoutedEventArgs e)
    {
      if (sender.GetType().Name == "CheckBox")
      {
        string chbxName = ((CheckBox)(sender)).Content.ToString();

        switch (chbxName)
        {
          case "All Lead Sources":
            this.lsbxLeadSources.IsEnabled = true;
            this.lsbxLeadSources.UnselectAll();
            break;
          case "All Sales Rooms":
            this.lsbxSalesRooms.IsEnabled = true;
            this.lsbxSalesRooms.UnselectAll();
            break;
          case "All Countries":
            this.lsbxCountries.IsEnabled = true;
            this.lsbxCountries.UnselectAll();
            break;
          case "All Agencies":
            this.lsbxAgencies.IsEnabled = true;
            this.lsbxAgencies.UnselectAll();
            break;
          case "All Markets":
            this.lsbxMarkets.IsEnabled = true;
            this.lsbxMarkets.UnselectAll();
            break;
          default:
            break;
        }
      }
    }

    #endregion

    #region SelectionChanged Listas & DataGrid
    /// <summary>
    /// Muestra en un Textblock cuantos elementos de la lista LeadSource estan seleccionados 
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void lsbxLeadSources_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtbLSSelected.Text = string.Format("{0}/{1}", lsbxLeadSources.SelectedItems.Count.ToString(), lsbxLeadSources.Items.Count.ToString());
    }
    /// <summary>
    /// Muestra en un Textblock cuantos elementos de la lista Sales Rooms estan seleccionados 
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void lsbxSalesRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtbSRSelected.Text = string.Format("{0}/{1}", lsbxSalesRooms.SelectedItems.Count.ToString(), lsbxSalesRooms.Items.Count.ToString());
    }
    /// <summary>
    /// Muestra en un Textblock cuantos elementos de la lista Countries estan seleccionados 
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void lsbxCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtbCOSelected.Text = string.Format("{0}/{1}", lsbxCountries.SelectedItems.Count.ToString(), lsbxCountries.Items.Count.ToString());
    }
    /// <summary>
    /// Muestra en un Textblock cuantos elementos de la lista Agencies estan seleccionados 
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void lsbxAgencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtbAGSelected.Text = string.Format("{0}/{1}", lsbxAgencies.SelectedItems.Count.ToString(), lsbxAgencies.Items.Count.ToString());
    }
    /// <summary>
    /// Muestra en un Textblock cuantos elementos de la lista Markets estan seleccionados 
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void lsbxMarkets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtbMKSelected.Text = string.Format("{0}/{1}", lsbxMarkets.SelectedItems.Count.ToString(), lsbxMarkets.Items.Count.ToString());
    }
    /// <summary>
    /// Nos indica en la barra de estatus el numero de elementos (Index) que esta seleccionado en el datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [erosado] 11/03/2016  Created
    /// </history>
    private void dtgr_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarReg.Content = string.Format("{0}/{1}", (dtgr.SelectedIndex + 1).ToString(), dtgr.Items.Count.ToString());

    }

    #endregion


  }
}
