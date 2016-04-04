using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using IM.Model;
using IM.Model.Enums;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Classes;
using System.Data;
using OfficeOpenXml.Style;
using IM.Assignment.Classes;
using System.IO;
using System.Diagnostics;

namespace IM.Assignment
{
  /// <summary>
  /// Formulario de asignación de huespedes a PRs
  /// Interaction logic for frmAssignment.xaml
  /// </summary>
  /// <history>
  ///   [vku] 08/03/2016 Created
  /// </history>
  public partial class frmAssignment : Window
  {

    #region Atributos

    private string _markets = "ALL", _LeadSource = string.Empty, _strgPRs = string.Empty, _strgNamePR = string.Empty;

    private List<int> _strgGuestUnassigned = new List<int>();
    private List<int> _strgGuestAssigned = new List<int>();
    private List<MarketShort> lstMarkets;

    private DateTime mdtmDate;

    private CollectionViewSource _guestUnassignedViewSource;
    private CollectionViewSource _pRAssignedViewSource;
    private CollectionViewSource _guestAssignedViewSource;

    private List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
    private DataTable dt = new DataTable();
    private string dateRange;
    private string rptName;
    

    #endregion

    #region Constructores y destructores
    public frmAssignment()
    {
      InitializeComponent();
    }
    #endregion

    #region Metodos

    #region LoadListMarkets
    /// <summary>
    /// Carga los mercados
    /// </summary>
    private void LoadListMarkets()
    {
      lstMarkets = BRMarkets.GetMarkets(1);
      grdListMarkets.ItemsSource = lstMarkets;
      grdListMarkets.SelectAll();
    }
    #endregion

    #region LoadListGuestsUnassigned
    /// <summary>
    /// ///Carga los huespedes no asigandos
    /// </summary>
    private void LoadListGuestsUnassigned()
    {
      _guestUnassignedViewSource.Source = BRAssignment.GetGuestUnassigned(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets, chkShowOnlyAvail.IsChecked.Value);
      grdGuestUnassigned.UnselectAll();
    }
    #endregion

    #region LoadPRs
    ///<summary>
    ///Carga los PRs que tienen asignado al menos a un huesped
    ///</summary>
    private void LoadPRs()
    {
      _pRAssignedViewSource.Source = BRAssignment.GetPRsAssigned(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets, chkGuestsPRs.IsChecked.Value, chkMemberPRs.IsChecked.Value);
      LoadListGuestsAssigned();
    }
    #endregion

    #region LoadListGuestsAssigned
    /// <summary>
    ///Carga los huespedes asignados de los PRs Seleccionados
    /// </summary>
    private void LoadListGuestsAssigned()
    {
      var selectedItems = grdPRAssigned.SelectedItems;
      if (selectedItems.Count > 0)
      {
        _guestAssignedViewSource.Source = BRAssignment.GetGuestAssigned(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _strgPRs, _markets);
      }
    }

    #endregion

    #region FilterRecords
    /// <summary>
    /// Configura los filtros que se muestran en la ventana
    /// </summary>
    private void FilterRecords()
    {
      //Refrescamos los filtros
      filters.Clear();

      //Obtiene numero de la semana a partir de una fecha
      int weekYear = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(mdtmDate, CalendarWeekRule.FirstFullWeek, mdtmDate.DayOfWeek);
      lblWeek.Content = "Week " + weekYear;

      //Rango de fechas
      dateRange = DateHelper.DateRange(mdtmDate, mdtmDate.AddDays(6));
      lblDataRange.Content = dateRange;
      LoadListGuestsUnassigned();
      LoadPRs();
      int sumAssign = 0;
      foreach (PRAssigned item in grdPRAssigned.ItemsSource)
      {
        sumAssign += Convert.ToInt32(item.Assigned);
      }
      lblTotalAssign.Content = sumAssign;
      lblTotalW.Content = sumAssign + grdGuestUnassigned.Items.Count;

    }
    #endregion

    #region DateRange
    /// <summary>
    /// Obtiene un rango de fechas
    /// Se omite este metodo.. se utiliza DateHelpers.DateRange
    /// </summary>
    public string DateRange(DateTime pdtmStart, DateTime pdtmEnd)
    {
      string stgDateFormat = "";
      //mismo dia
      if (pdtmStart == pdtmEnd)
      {
        stgDateFormat = pdtmStart.ToString("MMMM d, yyyy");
      }
      else
      {
        //diferente año
        if (pdtmStart.Year != pdtmEnd.Year)
        {
          stgDateFormat = pdtmStart.ToString("MMMM d, yyyy - ") + pdtmEnd.ToString("MMMM d, yyyy");
        }
        else
        {
          //mismo año, diferente mes
          if (pdtmStart.Month != pdtmEnd.Month)
          {
            stgDateFormat = pdtmStart.ToString("MMMM d - ") + pdtmEnd.ToString("MMMMM d, yyyy");
          }
          else
          {
            //mismo año, mismo mes
            stgDateFormat = pdtmStart.ToString("MMMM d - ") + pdtmEnd.ToString("d, yyyy");
          }
        }
      }
      return stgDateFormat;
    }
    #endregion

    #region getstartweek
    <
    public static System.DateTime getstartweek(DateTime dt)
     {
       System.DayOfWeek dmon = System.DayOfWeek.Monday;
       int span = dt.DayOfWeek - dmon;
       dt = dt.AddDays(-span);
       return dt;
     }
    #endregion

    #region ValidateAssign
    /// <summary>
    /// Valida los datos para poder asignar huespedes a un PR
    /// </summary>
    private Boolean ValidateAssign()
    {
      Boolean blnValid;
      blnValid = true;

      ///valida que haya al menos un huesped seleccionado
      if (grdGuestUnassigned.SelectedItems.Count == 0)
      {
        UIHelper.ShowMessage("Select at least one guest.",MessageBoxImage.Warning);
        blnValid = false;
      }
      else
      {
       ///valida que haya un PR seleccionado
        if (ValidatePR() == false)
        {
          blnValid = false;
        }
      }
      return blnValid;
    }

    #endregion

    #region ValidateUnassign
    /// <summary>
    /// Valida los datos para poder remover un huesped asignado
    /// </summary>
    private Boolean ValidateUnassign()
    {
      Boolean blnValid;
      blnValid = true;

      if (grdGuestAssigned.SelectedItems.Count == 0)
      {
        UIHelper.ShowMessage("Select at leas one guest",MessageBoxImage.Warning);
        blnValid = false;
      }
      else
      {
        if (ValidatePR() == false)
        {
          blnValid = false;
        }
      }
      return blnValid;
    }

    #endregion

    #region ValidatePR
    /// <summary>
    /// Valida que se haya seleccionado solo un PR
    /// </summary>
    private Boolean ValidatePR()
    {
      Boolean blnValid;
      blnValid = true;
      //validamos que haya un PR seleccionado
      if (grdPRAssigned.SelectedItems.Count == 0)
      {
        UIHelper.ShowMessage("Select a PR.", MessageBoxImage.Warning);
        blnValid = false;
      }
      else
      {
        //validamos que solo haya un PR seleccionado
        if (grdPRAssigned.SelectedItems.Count > 1)
        {
          UIHelper.ShowMessage("Select only one PR", MessageBoxImage.Warning);
          blnValid = false;
        }
      }
      return blnValid;
    }
    #endregion

    #endregion

    #region Eventos

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      System.Windows.Data.CollectionViewSource marketShortViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("marketShortViewSource")));
      //Load data by setting the CollectionViewSource.Source property:
      // marketShortViewSource.Source = [generic data source]

      // System.Windows.Data.CollectionViewSource guestUnassignedViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestUnassignedViewSource")));
      // Load data by setting the CollectionViewSource.Source property:
      // guestUnassignedViewSource.Source = [generic data source]

      //System.Windows.Data.CollectionViewSource pRAssignedViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pRAssignedViewSource")));
      // Load data by setting the CollectionViewSource.Source property:
      // pRAssignedViewSource.Source = [generic data source]

      //System.Windows.Data.CollectionViewSource guestAssignedViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestAssignedViewSource")));
      // Load data by setting the CollectionViewSource.Source property:
      // guestAssignedViewSource.Source = [generic data source]

      _LeadSource = App.User.Location.loID;
      mdtmDate = getstartweek(DateTime.Today.Date);

      //Inicializamos los filtros
      chkMemberPRs.IsChecked = true;


      _guestUnassignedViewSource = ((CollectionViewSource)(this.FindResource("guestUnassignedViewSource")));
      _pRAssignedViewSource = ((CollectionViewSource)(this.FindResource("pRAssignedViewSource")));
      _guestAssignedViewSource = ((CollectionViewSource)(this.FindResource("guestAssignedViewSource")));
      LoadListMarkets();
      FilterRecords();

    
    }
    #endregion

    #region btnNext_Click
    /// <summary>
    /// Avanza a la siguiente semana  
    /// </summary>
    private void btnNext_Click(object sender, RoutedEventArgs e)
    {
      mdtmDate = mdtmDate.AddDays(7);
      FilterRecords();
    }
    #endregion

    #region btnBack_Click
    /// <summary>
    /// Retrocede una semana
    /// </summary>
    private void btnBack_Click(object sender, RoutedEventArgs e)
    {
      mdtmDate = mdtmDate.AddDays(-7);
      FilterRecords();
    }
    #endregion

    #region chkShowOnlyAvail_Click
    /// <summary>
    /// Verifica si filtrar solo los disponibles
    /// </summary>
    private void chkShowOnlyAvail_Click(object sender, RoutedEventArgs e)
    {
      FilterRecords();
    }
    #endregion

    #region chkMemberPRs_Click
    /// <summary>
    /// Actualiza la información al seleccionar la opcion de MembersPRs
    /// </summary>
    private void chkMemberPRs_Click(object sender, RoutedEventArgs e)
      {
        ///Condiciona que tenga seleccionado una opcion GuestsPRs o MembersPRs
        if (chkGuestsPRs.IsChecked == false & chkMemberPRs.IsChecked == false)
        {
          chkGuestsPRs.IsChecked = true;
        }
        FilterRecords();
      }
    #endregion

    #region chkGuestsPRs_Click
    /// <summary>
    /// Actualiza la información al seleccionar la opcion de GuestsPRs
    /// </summary>
    private void chkGuestsPRs_Click(object sender, RoutedEventArgs e)
   {
     ///Condiciona que tenga seleccionado una opcion GuestsPRs o MembersPRs
     if (chkGuestsPRs.IsChecked.Value == false & chkMemberPRs.IsChecked.Value == false)
     {
       chkMemberPRs.IsChecked = true;
     }
     FilterRecords();
   }
    #endregion

    #region btnAssignmentByPR_Click
    /// <summary>
    /// Genera el reporte de huespedes asignados por PR
    /// </summary>
    private void btnAssignmentByPR_Click(object sender, RoutedEventArgs e)
    {
      //validamos el PR
      if (ValidatePR()) {
        filters.Add(Tuple.Create("Date Range", dateRange));
        filters.Add(Tuple.Create("Lead Source", _LeadSource));
        filters.Add(Tuple.Create(_strgPRs, _strgNamePR));
        List<RptAssignmentByPR> lstAssignmentByPR = BRAssignment.RptAssignmentByPR(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets, _strgPRs);
        if (lstAssignmentByPR.Count > 0)
        {
          dt = GridHelper.GetDataTableFromGrid(lstAssignmentByPR, true);
          rptName = "Assignment by PR";
          string dateRangeFileName = DateHelper.DateRangeFileName(mdtmDate, mdtmDate.AddDays(6));
          EpplusHelper.CreateGeneralRptExcel(filters, dt, rptName,dateRangeFileName, clsFormatTable.getExcelFormatTableAssignByPR());
        }
        else
        {
          UIHelper.ShowMessage("There is no data",MessageBoxImage.Warning);
        }
      }
    }
    #endregion

    #region btnGeneralAssignment_Click
    /// <summary>
    /// Genera el reporte de huespedes asignados
    /// </summary>
    private void btnGeneralAssignment_Click(object sender, RoutedEventArgs e)
    {
      filters.Add(Tuple.Create("Date Range", dateRange));
      filters.Add(Tuple.Create("Lead Source", _LeadSource));
      List<RptAssignment> lstAssignment = BRAssignment.RptAssignment(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets) ;
      if (lstAssignment.Count > 0)
      {
        dt = GridHelper.GetDataTableFromGrid(lstAssignment, true);
        rptName = "General Assignment";
        string dateRangeFileName = DateHelper.DateRangeFileName(mdtmDate, mdtmDate.AddDays(6));
        EpplusHelper.CreateGeneralRptExcel(filters, dt, rptName, dateRangeFileName, clsFormatTable.getExcelFormatTableGenAsignyArvls());
      }
      else
      {
        UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning);
      }
    }
    #endregion

    #region btnAssignmentArrivals
    /// <summary>
    /// Genera el reporte de llegadas y su asignación
    /// </summary>
    private void btnAssignmentArrivals_Click(object sender, RoutedEventArgs e)
    {
      filters.Add(Tuple.Create("Date Range", dateRange));
      filters.Add(Tuple.Create("Lead Source", _LeadSource));
      List<RptAssignmentArrivals> lstAssignmentArrivals = BRAssignment.RptAssignmetArrivals(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets);
      if (lstAssignmentArrivals.Count > 0)
      {
        dt = GridHelper.GetDataTableFromGrid(lstAssignmentArrivals, true);
        rptName = "Arrivals";
        string dateRangeFileName = DateHelper.DateRangeFileName(mdtmDate, mdtmDate.AddDays(6));
        EpplusHelper.CreateGeneralRptExcel(filters, dt, rptName,dateRangeFileName, clsFormatTable.getExcelFormatTableGenAsignyArvls());
      }
      else
      {
        UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning);
      }
    }
    #endregion

    #region btnAssign_Click
    /// <summary>
    /// Asigna huespedes a un PR
    /// </summary>
    private void btnAssign_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateAssign())
      {
        BRAssignment.SaveGuetsPRAssign(_strgGuestUnassigned, _strgPRs);
        FilterRecords();
      }
    }
    #endregion

    #region btnRemove_Click
    /// <summary>
    /// Remueve asignación de huespedes
    /// </summary>
    private void btnRemove_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateUnassign())
      {
        BRAssignment.SaveGuestUnassign(_strgGuestAssigned, _strgPRs);
        FilterRecords();
      }
    }
    #endregion

    #region grdListMarkets_SelectionChanged
    /// <summary>
    /// Guarda lista de mercados seleccionados
    /// </summary>
    private void grdListMarkets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int cont = 0; _markets = string.Empty;
      var selectedItems = grdListMarkets.SelectedItems;
      foreach (MarketShort selectedItem in selectedItems)
      {
        cont = cont + 1;
        _markets += selectedItem.mkID.ToString();
        if (selectedItems.Count > 1 && cont < selectedItems.Count)
        {
          _markets = _markets + ",";
        }
      }
      LoadListGuestsUnassigned();
    }
    #endregion

    #region grdPRAssigned_SelectionChanged
    /// <summary>
    /// Guarda lista de PRs Seleccionados que tienen huespedes asignados
    /// </summary>
    private void grdPRAssigned_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int cont = 0; _strgPRs = string.Empty; _strgNamePR = string.Empty;
      var selectedItems = grdPRAssigned.SelectedItems;
      foreach (PRAssigned selectedItem in selectedItems)
      {
        cont = cont + 1;
        _strgPRs += selectedItem.peID.ToString();
        _strgNamePR = selectedItem.peN;
        if (selectedItems.Count > 1 && cont < selectedItems.Count)
        {
          _strgPRs = _strgPRs + ",";
          _strgNamePR = _strgNamePR + ",";
        }
      }
      LoadListGuestsAssigned();
    }

    #endregion

    #region grdGuestUnassigned_SelectionChanged
    /// <summary>
    /// Guarda lista de huespedes seleccionados que no se hayan sido asignados
    /// </summary>
    private void grdGuestUnassigned_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _strgGuestUnassigned.Clear();
      var selectedItems = grdGuestUnassigned.SelectedItems;
      foreach (GuestUnassigned selectedItem in selectedItems)
      {
        _strgGuestUnassigned.Add(selectedItem.guID);
      }
    }
    #endregion

    #region grdGuestAssigned_SelectionChanged
    /// <summary>
    /// Guarda lista de huespedes seleccionados que estan asignados
    /// </summary>
    private void grdGuestAssigned_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _strgGuestAssigned.Clear();
      var selectedItems = grdGuestAssigned.SelectedItems;
      foreach (GuestAssigned selectedItem in selectedItems)
      {
        _strgGuestAssigned.Add(selectedItem.guID);
      }
    }
    #endregion

    #endregion
  }
}
