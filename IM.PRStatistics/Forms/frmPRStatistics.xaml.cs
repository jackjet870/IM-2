using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IM.Model;
using IM.BusinessRules.BR;
using IM.PRStatistics.Utilities;
using System.IO;
using System.Data;
using System.Linq;
using IM.Base.Forms;
using IM.Base.Helpers;
using Xceed.Wpf.Toolkit;
using IM.Model.Helpers;
using IM.Model.Enums;

namespace IM.PRStatistics.Forms
{
  /// <summary>
  /// Interaction logic for frmPRStatistics.xaml
  /// </summary>
  public partial class frmPRStatistics : Window
  {
    #region Propiedades y Atributos
    List<Tuple<string, string>> filterTuple; // Agrega los filtros de busqueda
    public ExecuteCommandHelper LoadCatalogList { get; set; }
    #endregion

    #region Constructores y  Destructores

    /// <summary>
    /// Constructor
    /// </summary>
    /// <history>
    /// [wtorres]  15/Mar/2016 Modified. Elimine el parametro UserData
    /// </history>
    public frmPRStatistics()
    {
      InitializeComponent();
      //Instanciamos nuestro delegado
      LoadCatalogList = new ExecuteCommandHelper(x => LoadCatalogs());
    }
    #endregion

    #region Eventos de la ventana
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Inicializamos los catalogos
      LoadCatalogs();
      //Seleccionamos los días en el datapicker 
      dtpkFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      dtpkTo.Value = DateTime.Now;
      //Agregamos la informacion del usuario en la interfaz
      txtbUserName.Text = App.User.User.peN;

    }
    /// <summary>
    /// Evento que se encarga de generar las estadisticas
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void imgButtonOk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      imgButtonOk.Focus();
      if (lsbxLeadSources.SelectedItems.Count > 0 && lsbxSalesRooms.SelectedItems.Count > 0
              && lsbxCountries.SelectedItems.Count > 0 && lsbxAgencies.SelectedItems.Count > 0
              && lsbxMarkets.SelectedItems.Count > 0)
      {

        if (DateHelper.ValidateValueDate(dtpkFrom,dtpkTo))
        {
          filterTuple = new List<Tuple<string, string>>();
          StaStart("Loading Data...");
          imgButtonOk.IsEnabled = false;
          filterTuple.Add(new Tuple<string, string>("DateRange", DateHelper.DateRange(dtpkFrom.Value.Value, dtpkTo.Value.Value)));
          filterTuple.Add(new Tuple<string, string>("LeadSource", chbxLeadSources.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxLeadSources)));
          filterTuple.Add(new Tuple<string, string>("SalesRooms", chbxSalesRooms.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxSalesRooms)));
          filterTuple.Add(new Tuple<string, string>("Countries", chbxCountries.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxCountries)));
          filterTuple.Add(new Tuple<string, string>("Agencies", chbxAgencies.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxAgencies)));
          filterTuple.Add(new Tuple<string, string>("Markets", chbxMarkets.IsChecked == true ? "ALL" : UsefulMethods.SelectedItemsIdToString(lsbxMarkets)));

          DoGetRptPrStats(dtpkFrom.Value.Value, dtpkTo.Value.Value, filterTuple);
        }
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
    /// [emoguel] 09/09/2016 Modified. Ahora abre el visor de reportes
    /// </history>
    private async void imgButtonPrint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      List<RptPRStats> lstRptStats = dtgr.DataContext as List<RptPRStats>;
      if (lstRptStats != null)
      {
        string dateRangeFileName = DateHelper.DateRangeFileName(dtpkFrom.Value.Value, dtpkTo.Value.Value);
        FileInfo templatePath = new FileInfo(string.Concat(Directory.GetCurrentDirectory(), "\\ReportTemplate\\RptPRStatistics.xlsx"));
        DataTable dt = TableHelper.GetDataTableFromList(lstRptStats);
        string nombreReporte = "PR Statistics";




        FileInfo finfo = await EpplusHelper.CreateCustomExcel(dt, filterTuple, nombreReporte, dateRangeFileName, UsefulMethods.getExcelFormatTable(), addEnumeration: true, blnShowSubtotal:true, blnRowGrandTotal:true);

        if (finfo != null)
        {
          frmDocumentViewer documentViewer = new frmDocumentViewer(finfo, App.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly), false);
          documentViewer.ShowDialog();
        }
      }
      else
      {
        UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Information, "PR Statistics");
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
      frmAbout.Owner = this;
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
      Close();
    }
    /// <summary>
    /// Enviamos el Focus al siguiente DatePicker To o si esta en DatePicker From se va el focus al boton Search
    /// </summary>
    /// <history>
    /// [erosado] 01/07/2016  Created.
    /// </history>
    private void dtpkEnterKey(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        DateTimePicker dtpk = sender as DateTimePicker;
        if (dtpk.Name == "dtpkFrom")
        {
          dtpkTo.Focus();
        }
        else
        {
          imgButtonOk.Focus();
        }
      }
    }
    #endregion

    #region StatusBar
    /// <summary>
    /// Activa los StatusBarItem CAP, NUM, INS
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
      Cursor = System.Windows.Input.Cursors.Wait;

    }

    /// <summary>
    /// Indica en la barra de estado que se termina un proceso
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      Cursor = null;
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
    /// [edgrodriguez] 21/May/2016 Modified El método GetLeadSourcesByUser se volvió asincrónico.
    /// </history>
    public async void DoGetLeadSources(string user)
    {
      try
      {
        List<LeadSourceByUser> data = await BRLeadSources.GetLeadSourcesByUser(user);
        if (data.Any())
        {
          lsbxLeadSources.ItemsSource = data;
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }

    /// <summary>
    /// Obtiene el catalogo de SalesRooms
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public async void DoGetSalesRooms(string user)
    {
      try
      {
        List<SalesRoomByUser> data = await BRSalesRooms.GetSalesRoomsByUser(user);
        if (data.Any())
        {
          lsbxSalesRooms.ItemsSource = data;
          chbxSalesRooms.IsChecked = true;
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }

    }
    /// <summary>
    /// Obtiene el catalogo de countries
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public async void DoGetCountries()
    {
      try
      {
        List<CountryShort> data = await BRCountries.GetCountries(1);
        if (data.Any())
        {
          lsbxCountries.ItemsSource = data;
          chbxCountries.IsChecked = true;
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }

    /// <summary>
    /// Obtiene el catalogo de Agencies
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public async void DoGetAgencies()
    {
      try
      {
        List<AgencyShort> data = await BRAgencies.GetAgencies(1);
        if (data.Any())
        {
          lsbxAgencies.ItemsSource = data;
          chbxAgencies.IsChecked = true;
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
    }
    /// <summary>
    /// Obtiene el catalogo de Markets
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public async void DoGetMarkets()
    {
      try
      {
        List<MarketShort> data = await BRMarkets.GetMarkets(1);
        if (data.Any())
        {
          lsbxMarkets.ItemsSource = data;
          chbxMarkets.IsChecked = true;
        }
        StaEnd();
      }
      catch (Exception ex)
      {
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
     
    }

    /// <summary>
    /// Obtiene el resultado de las estadisticas
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    public async void DoGetRptPrStats(DateTime dateFrom, DateTime dateTo, List<Tuple<string, string>> filtros)
    {
      try
      {
        var data = await BRPRStats.GetPRStats(dateFrom, dateTo, filtros);
        if (data.Any())
        {
          dtgr.DataContext = data;
          StatusBarReg.Content = $"{(dtgr.SelectedIndex + 1).ToString()}/{ dtgr.Items.Count.ToString()}";
        }
        else
        {
          UIHelper.ShowMessage("There is no data");
        }
        StaEnd();
        imgButtonOk.IsEnabled = true;
      }
      catch (Exception ex)
      {
        imgButtonOk.IsEnabled = true;
        StaEnd();
        UIHelper.ShowMessage(ex);
      }
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
        string chbxName = ((System.Windows.Controls.CheckBox)(sender)).Content.ToString();

        switch (chbxName)
        {
          case "All Lead Sources":
            lsbxLeadSources.IsEnabled = false;
            lsbxLeadSources.SelectAll();
            break;
          case "All Sales Rooms":
            lsbxSalesRooms.IsEnabled = false;
            lsbxSalesRooms.SelectAll();
            break;
          case "All Countries":
            lsbxCountries.IsEnabled = false;
            lsbxCountries.SelectAll();
            break;
          case "All Agencies":
            lsbxAgencies.IsEnabled = false;
            lsbxAgencies.SelectAll();
            break;
          case "All Markets":
            lsbxMarkets.IsEnabled = false;
            lsbxMarkets.SelectAll();
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
        string chbxName = ((System.Windows.Controls.CheckBox)(sender)).Content.ToString();

        switch (chbxName)
        {
          case "All Lead Sources":
            lsbxLeadSources.IsEnabled = true;
            lsbxLeadSources.UnselectAll();
            break;
          case "All Sales Rooms":
            lsbxSalesRooms.IsEnabled = true;
            lsbxSalesRooms.UnselectAll();
            break;
          case "All Countries":
            lsbxCountries.IsEnabled = true;
            lsbxCountries.UnselectAll();
            break;
          case "All Agencies":
            lsbxAgencies.IsEnabled = true;
            lsbxAgencies.UnselectAll();
            break;
          case "All Markets":
            lsbxMarkets.IsEnabled = true;
            lsbxMarkets.UnselectAll();
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
      txtbLSSelected.Text = $"{lsbxLeadSources.SelectedItems.Count.ToString()}/{lsbxLeadSources.Items.Count.ToString()}";
    }
    /// <summary>
    /// Muestra en un Textblock cuantos elementos de la lista Sales Rooms estan seleccionados 
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void lsbxSalesRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtbSRSelected.Text = $"{lsbxSalesRooms.SelectedItems.Count.ToString()}/{lsbxSalesRooms.Items.Count.ToString()}";
    }
    /// <summary>
    /// Muestra en un Textblock cuantos elementos de la lista Countries estan seleccionados 
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void lsbxCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtbCOSelected.Text = $"{lsbxCountries.SelectedItems.Count.ToString()}/{lsbxCountries.Items.Count.ToString()}";
    }
    /// <summary>
    /// Muestra en un Textblock cuantos elementos de la lista Agencies estan seleccionados 
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void lsbxAgencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtbAGSelected.Text = $"{lsbxAgencies.SelectedItems.Count.ToString()}/{lsbxAgencies.Items.Count.ToString()}";
    }
    /// <summary>
    /// Muestra en un Textblock cuantos elementos de la lista Markets estan seleccionados 
    /// </summary>
    /// <history>
    /// [erosado] 08/Mar/2016 Created
    /// </history>
    private void lsbxMarkets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      txtbMKSelected.Text = $"{lsbxMarkets.SelectedItems.Count.ToString()}/{lsbxMarkets.Items.Count.ToString()}";
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
      StatusBarReg.Content = $"{(dtgr.SelectedIndex + 1).ToString()}/{dtgr.Items.Count.ToString()}";
    }
    #endregion

    #region Metodos
    /// <summary>
    /// Carga los catalogos de LeadSource, SalesRooms, Countries, Agencies, Markets
    /// </summary>
    /// <history>
    /// [erosado] 18/04/2016  Created.
    /// </history>
    protected void LoadCatalogs()
    {
      chbxLeadSources.IsChecked = false;
      chbxSalesRooms.IsChecked = false;
      chbxCountries.IsChecked = false;
      chbxAgencies.IsChecked = false;
      chbxMarkets.IsChecked = false;
      DoGetLeadSources(App.User.User.peID);
      DoGetSalesRooms(App.User.User.peID);
      DoGetCountries();
      DoGetAgencies();
      DoGetMarkets();
    }
    #endregion


  }
}
