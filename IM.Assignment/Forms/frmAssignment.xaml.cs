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
    private Tuple<string, string> rptName;

    #endregion

    #region Constructores y destructores
    public frmAssignment()
    {
      InitializeComponent();
    }
    #endregion

    #region Metodos

    #region LoadListMarkets
    private void LoadListMarkets()
    {
      lstMarkets = BRMarkets.GetMarkets(1);
      grdListMarkets.ItemsSource = lstMarkets;
      grdListMarkets.SelectAll();
    }
    #endregion

    #region LoadListGuestsUnassigned
    private void LoadListGuestsUnassigned()
    {
      _guestUnassignedViewSource.Source = BRAssignment.GetGuestUnassigned(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets, chkShowOnlyAvail.IsChecked.Value);
      grdGuestUnassigned.UnselectAll();
    }
    #endregion

    #region LoadPRs

    private void LoadPRs()
    {
      _pRAssignedViewSource.Source = BRAssignment.GetPRsAssigned(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets, chkGuestsPRs.IsChecked.Value, chkMemberPRs.IsChecked.Value);
      LoadListGuestsAssigned();
    }
    #endregion

    #region LoadListGuestsAssigned
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
    private void FilterRecords()
    {
      
      //Obtiene numero de la semana a partir de una fecha
      int weekYear = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(mdtmDate, CalendarWeekRule.FirstFullWeek, mdtmDate.DayOfWeek);
      lblWeek.Content = "Week " + weekYear;
   
      //Rango de fechas
      lblDataRange.Content = DateRange(mdtmDate, mdtmDate.AddDays(6));

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
    //Obtiene un rango de fechas
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
    //Obtiene el primer dia de la semana
    public static System.DateTime getstartweek(DateTime dt)
     {
       System.DayOfWeek dmon = System.DayOfWeek.Monday;
       int span = dt.DayOfWeek - dmon;
       dt = dt.AddDays(-span);
       return dt;
     }
    #endregion

    #region ValidateAssign
    //Valida los datos para poder asignar huespedes a un PR
    private Boolean ValidateAssign()
    {
      Boolean blnValid;
      blnValid = true;

      //valida que haya al menos un huesped seleccionado
      if (grdGuestUnassigned.SelectedItems.Count == 0)
      {
        MessageBox.Show("Select at least one guest.", "Intelligence Marketing Assignment", MessageBoxButton.OK, MessageBoxImage.Information);
        blnValid = false;
      }
      else
      {
       //valida que haya un PR seleccionado
        if (ValidatePR() == false)
        {
          blnValid = false;
        }
      }
      return blnValid;
    }

    #endregion

    #region ValidateUnassign
    private Boolean ValidateUnassign()
    {
      Boolean blnValid;
      blnValid = true;

      if (grdGuestAssigned.SelectedItems.Count == 0)
      {
        MessageBox.Show("Select at least one guest.", "Intelligence Marketing Assignment", MessageBoxButton.OK, MessageBoxImage.Information);
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
    //Valida que se haya seleccionado solo un PR
    private Boolean ValidatePR()
    {
      Boolean blnValid;
      blnValid = true;
      //validamos que haya un PR seleccionado
      if (grdPRAssigned.SelectedItems.Count == 0)
      {
        MessageBox.Show("Select a PR.", "Intelligence Marketing Assignment", MessageBoxButton.OK, MessageBoxImage.Information);
        blnValid = false;
      }
      else
      {
        //validamos que solo haya un PR seleccionado
        if (grdPRAssigned.SelectedItems.Count > 1)
        {
          MessageBox.Show("Select only one PR.", "Intelligence Marketing Assignment", MessageBoxButton.OK, MessageBoxImage.Information);
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
    ///Avanza a la siguiente semana  
    private void btnNext_Click(object sender, RoutedEventArgs e)
    {
      mdtmDate = mdtmDate.AddDays(7);
      FilterRecords();
    }
    #endregion

    #region btnBack_Click
    //Retrocede una semana
    private void btnBack_Click(object sender, RoutedEventArgs e)
    {
      mdtmDate = mdtmDate.AddDays(-7);
      FilterRecords();
    }
    #endregion

    #region chkShowOnlyAvail_Click
    private void chkShowOnlyAvail_Click(object sender, RoutedEventArgs e)
    {
      FilterRecords();
    }
    #endregion
   
    #region chkMemberPRs_Click
      //Actualiza la información al seleccionar la opcion de MembersPRs
      private void chkMemberPRs_Click(object sender, RoutedEventArgs e)
      {
        //Condiciona que tenga seleccionado una opcion GuestsPRs o MembersPRs
        if (chkGuestsPRs.IsChecked == false & chkMemberPRs.IsChecked == false)
        {
          chkGuestsPRs.IsChecked = true;
        }
        FilterRecords();
      }
      #endregion
   
    #region chkGuestsPRs_Click
   //Actualiza la información al seleccionar la opcion de GuestsPRs
   private void chkGuestsPRs_Click(object sender, RoutedEventArgs e)
   {
     //Condiciona que tenga seleccionado una opcion GuestsPRs o MembersPRs
     if (chkGuestsPRs.IsChecked.Value == false & chkMemberPRs.IsChecked.Value == false)
     {
       chkMemberPRs.IsChecked = true;
     }
     FilterRecords();
   }
    #endregion

    #region btnAssignmentByPR_Click
    //Genera el reporte de huespedes asignados por PR
    private void btnAssignmentByPR_Click(object sender, RoutedEventArgs e)
    {
      //validamos el PR
      if (ValidatePR()) {
        filters.Add(Tuple.Create("Lead Source", _LeadSource));
        filters.Add(Tuple.Create(_strgPRs, _strgNamePR));
        List<RptAssignmentByPR> lstAssignmentByPR = BRAssignment.RptAssignmentByPR(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets, _strgPRs);
        if (lstAssignmentByPR.Count > 0)
        {
          dt = GridHelper.GetDataTableFromGrid(lstAssignmentByPR, true);
          rptName = new Tuple<string, string>("Assignment by PR", lblWeek.Content + " " + lblDataRange.Content);
          List<ExcelFormatTable> format = new List<ExcelFormatTable>();
          format.Add(new ExcelFormatTable() { Title = "In", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "Out", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "Room", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "Last Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "First Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "Member #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "01", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "Comments", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "PR ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "PR N Assigned", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
          format.Add(new ExcelFormatTable() { Title = "Pax", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
          EpplusHelper.CreateGeneralRptExcel(filters, dt, rptName, format);
        }
        else
        {
          MessageBox.Show("There is no date.", "Intelligence Marketing Assignment", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
    }
    #endregion

    #region btnGeneralAssignment_Click
    private void btnGeneralAssignment_Click(object sender, RoutedEventArgs e)
    {
      filters.Add(Tuple.Create("Lead Source", _LeadSource));
      List<RptAssignment> lstAssignment = BRAssignment.RptAssignment(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets) ;
      if (lstAssignment.Count > 0)
      {
        dt = GridHelper.GetDataTableFromGrid(lstAssignment, true);
        rptName = new Tuple<string, string>("General Assignment", ""+lblDataRange.Content);
        List<ExcelFormatTable> format = new List<ExcelFormatTable>();
        format.Add(new ExcelFormatTable() { Title = "ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Check In D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "In", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Room", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Last Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "First Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "PR ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "PR N Assigned", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Member #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Gross", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Liner", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Closer", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        EpplusHelper.CreateGeneralRptExcel(filters, dt, rptName, format);
      }
      else
      {
        MessageBox.Show("There is no date.", "Intelligence Marketing Assignment", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }
    #endregion

    #region btnAssignmentArrivals
    private void btnAssignmentArrivals_Click(object sender, RoutedEventArgs e)
    {
      filters.Add(Tuple.Create("Lead Source", _LeadSource));
      List<RptAssignmentArrivals> lstAssignmentArrivals = BRAssignment.RptAssignmetArrivals(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets);
      if (lstAssignmentArrivals.Count > 0)
      {
        dt = GridHelper.GetDataTableFromGrid(lstAssignmentArrivals, true);
        rptName = new Tuple<string, string>("Arrivals", "" + lblDataRange.Content);
        List<ExcelFormatTable> format = new List<ExcelFormatTable>();
        format.Add(new ExcelFormatTable() { Title = "ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Check In D", Format = EnumFormatTypeExcel.Date, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "In", Format = EnumFormatTypeExcel.Boolean, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Room", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Last Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "First Name", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "PR ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "PR N Assigned", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Agency ID", Format = EnumFormatTypeExcel.Number, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Agency", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Member #", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Gross", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "PR", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Liner", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        format.Add(new ExcelFormatTable() { Title = "Closer", Format = EnumFormatTypeExcel.General, Alignment = ExcelHorizontalAlignment.Left });
        EpplusHelper.CreateGeneralRptExcel(filters, dt, rptName, format);
      }
      else
      {
        MessageBox.Show("There is no date.", "Intelligence Marketing Assignment", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }
    #region

    #region btnAssign_Click
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

    #endregion

    #endregion
  }
}
