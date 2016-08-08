using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using IM.Model;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using System.Data;
using System.IO;
using IM.Assignment.Classes;
using System.Diagnostics;
using System.Windows.Input;

namespace IM.Assignment
{
  /// <summary>
  /// Formulario de asignación de huespedes a PRs
  /// Interaction logic for frmAssignment.xaml
  /// </summary>
  /// <history>
  ///   [vku] 08/Mar/2016 Created
  /// </history>
  public partial class frmAssignment: Window
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
    private string dateRange;
    int weekYear;
    FileInfo finfo;
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
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
    private async void LoadListMarkets()
    {  
      lstMarkets = await BRMarkets.GetMarkets(1);
      dtgListMarkets.ItemsSource = lstMarkets;
      dtgListMarkets.SelectAll();
    }
    #endregion

    #region LoadListGuestsUnassigned
    /// <summary>
    /// ///Carga los huespedes no asigandos
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Ahora el metodo es asincrono
    /// </history>
    private async void LoadListGuestsUnassigned()
    {
      txtStatus.Text = "Loading Guests Unassigned...";
      status.Visibility = Visibility.Visible;
      _guestUnassignedViewSource.Source = await BRAssignment.GetGuestUnassigned(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets, chkShowOnlyAvail.IsChecked.Value);
      status.Visibility = Visibility.Collapsed;
      dtgGuestUnassigned.UnselectAll();
    }
    #endregion

    #region LoadPRs
    ///<summary>
    ///Carga los PRs que tienen asignado al menos a un huesped
    ///</summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Ahora el metodo es asincrono
    /// </history>
    private async void LoadPRs()
    {
      txtStatus.Text = "Loading PRs...";
      status.Visibility = Visibility.Visible;
      _pRAssignedViewSource.Source = await BRAssignment.GetPRsAssigned(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets, chkGuestsPRs.IsChecked.Value, chkMemberPRs.IsChecked.Value);
      status.Visibility = Visibility.Collapsed;
      LoadListGuestsAssigned();   
    }
    #endregion

    #region LoadListGuestsAssigned
    /// <summary>
    ///Carga los huespedes asignados de los PRs Seleccionados
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Ahora el metodo es asincrono
    /// </history>
    private async void LoadListGuestsAssigned()
    {
      txtStatus.Text = "Loading Guests Assigned...";
      status.Visibility = Visibility.Visible;
      var selectedItems = dtgPRAssigned.SelectedItems;
      if (selectedItems.Count > 0)
      {
        _guestAssignedViewSource.Source = await BRAssignment.GetGuestAssigned(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _strgPRs, _markets);
        status.Visibility = Visibility.Collapsed;
      }
      int sumAssign = 0;
      foreach (PRAssigned item in dtgPRAssigned.ItemsSource)
      {
        sumAssign += Convert.ToInt32(item.Assigned);
      }
      txbTotalAssign.Text = sumAssign.ToString();
      txbTotalW.Text = (sumAssign + dtgGuestUnassigned.Items.Count).ToString();
    }

    #endregion

    #region FilterRecords
    /// <summary>
    /// Configura los filtros que se muestran en la ventana
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
    private void FilterRecords()
    {
      ///Refrescamos los filtros
      filters.Clear();

      weekYear = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(mdtmDate, CalendarWeekRule.FirstFullWeek, mdtmDate.DayOfWeek);
      txbWeek.Text = "Week " + weekYear;
      dateRange = DateHelper.DateRange(mdtmDate, mdtmDate.AddDays(6));
      txbDateRange.Text = dateRange;
      LoadListGuestsUnassigned();
      LoadPRs();
    }
    #endregion

    #region ValidateAssign
    /// <summary>
    /// Valida los datos para poder asignar huespedes a un PR
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
    private Boolean ValidateAssign()
    {
      Boolean blnValid;
      blnValid = true;

      ///valida que haya al menos un huesped seleccionado
      if (dtgGuestUnassigned.SelectedItems.Count == 0)
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
    /// <history>
    ///   [vku]08/Mar/2016 Created
    /// </history>
    private Boolean ValidateUnassign()
    {
      Boolean blnValid;
      blnValid = true;

      if (dtgGuestAssigned.SelectedItems.Count == 0)
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
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
    private Boolean ValidatePR()
    {
      Boolean blnValid;
      blnValid = true;
      //validamos que haya un PR seleccionado
      if (dtgPRAssigned.SelectedItems.Count == 0)
      {
        UIHelper.ShowMessage("Select a PR.", MessageBoxImage.Warning);
        blnValid = false;
      }
      else
      {
        //validamos que solo haya un PR seleccionado
        if (dtgPRAssigned.SelectedItems.Count > 1)
        {
          UIHelper.ShowMessage("Select only one PR", MessageBoxImage.Warning);
          blnValid = false;
        }
      }
      return blnValid;
    }
    #endregion

    #region OpenReport
    private void OpenReport(FileInfo finfo)
    {
      if (finfo != null)
      {
        Process.Start(finfo.FullName);
      }
    }
    #endregion

    #endregion

    #region Eventos

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      System.Windows.Data.CollectionViewSource marketShortViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("marketShortViewSource")));
      ///Inicializamos los filtros
      _LeadSource = App.User.Location.loID;
      mdtmDate = DateHelper.GetStartWeek(DateTime.Today.Date);

      ///Obtiene numero de la semana a partir de una fecha
      weekYear = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(mdtmDate, CalendarWeekRule.FirstFullWeek, mdtmDate.DayOfWeek);
      txbWeek.Text = "Week " + weekYear;

      ///Rango de fechas
      dateRange = DateHelper.DateRange(mdtmDate, mdtmDate.AddDays(6));
      txbDateRange.Text = dateRange;

      chkMemberPRs.IsChecked = true;

      _guestUnassignedViewSource = ((CollectionViewSource)(this.FindResource("guestUnassignedViewSource")));
      _pRAssignedViewSource = ((CollectionViewSource)(this.FindResource("pRAssignedViewSource")));
      _guestAssignedViewSource = ((CollectionViewSource)(this.FindResource("guestAssignedViewSource")));
      LoadListMarkets();
      LoadPRs();
    }
    #endregion

    #region btnNext_Click
    /// <summary>
    /// Avanza a la siguiente semana  
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
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
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
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
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
    private void chkShowOnlyAvail_Click(object sender, RoutedEventArgs e)
    {
      FilterRecords();
    }
    #endregion

    #region chkMemberPRs_Click
    /// <summary>
    /// Actualiza la información al seleccionar la opcion de MembersPRs
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
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
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
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
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Ahora el metodo es asincrono
    ///   [vku] 20/Jul/2016 Modified. Ahora se utiliza la clase clsReports para exporar a excel.
    ///                               Se renombro el label donde se muestra el PR ID de la cabezara por PR. 
    /// </history>
    private async void btnAssignmentByPR_Click(object sender, RoutedEventArgs e)
    {
      filters.Clear();
      finfo = null;
      ///validamos el PR
      if (ValidatePR()) {
        filters.Add(Tuple.Create("Date Range", dateRange));
        filters.Add(Tuple.Create("Lead Source", _LeadSource));
        filters.Add(Tuple.Create("PR", _strgPRs + " - " + _strgNamePR));
        List<RptAssignmentByPR> lstAssignmentByPR = await BRAssignment.RptAssignmentByPR(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets, _strgPRs);
        if (lstAssignmentByPR.Count > 0)
        {       
          string dateRangeFileName = DateHelper.DateRangeFileName(mdtmDate, mdtmDate.AddDays(6));
          finfo = clsReports.ExportRptAssignmentByPR("Assignment by PR", dateRangeFileName, filters, lstAssignmentByPR);
        }
        else
        {
          UIHelper.ShowMessage("There is no data",MessageBoxImage.Warning);
        }
        OpenReport(finfo);
      }  
    }
    #endregion

    #region btnGeneralAssignment_Click
    /// <summary>
    /// Genera el reporte de huespedes asignados
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 20/Jul/2016 Modified. Ahora se utiliza la clase clsReports para exporar a excel.
    /// </history>
    private async void btnGeneralAssignment_Click(object sender, RoutedEventArgs e)
    {
      filters.Clear();
      filters.Add(Tuple.Create("Date Range", dateRange));
      filters.Add(Tuple.Create("Lead Source", _LeadSource));
      finfo = null;
      List<RptAssignment> lstAssignment = await BRAssignment.RptAssignment(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets) ;
      if (lstAssignment.Count > 0)
      {
        string dateRangeFileName = DateHelper.DateRangeFileName(mdtmDate, mdtmDate.AddDays(6));
        finfo = clsReports.ExportRptGeneralAssignment("General Assignment", dateRangeFileName, filters, lstAssignment);
      }
      else
      {
        UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning);
      }
      OpenReport(finfo);
    }
    #endregion

    #region btnAssignmentArrivals
    /// <summary>
    ///   Genera el reporte de llegadas y su asignación
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. El metodo ahora es asincrono
    ///   [vku] 20/Jul/2016 Modified. Ahora se utiliza la clase clsReports para exporar a excel.
    /// </history>
    private async void btnAssignmentArrivals_Click(object sender, RoutedEventArgs e)
    {
      filters.Clear();
      filters.Add(Tuple.Create("Date Range", dateRange));
      filters.Add(Tuple.Create("Lead Source", _LeadSource));
      finfo = null;
      List<RptAssignmentArrivals> lstAssignmentArrivals = await BRAssignment.RptAssignmetArrivals(mdtmDate, mdtmDate.AddDays(6), _LeadSource, _markets);
      if (lstAssignmentArrivals.Count > 0)
      {
        string dateRangeFileName = DateHelper.DateRangeFileName(mdtmDate, mdtmDate.AddDays(6));
        finfo = clsReports.ExportRptAssignmentArrivals("Assignment Arrivals", dateRangeFileName, filters, lstAssignmentArrivals);
      }
      else
      {
        UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning);
      }
      OpenReport(finfo);
    }
    #endregion

    #region btnAssign_Click
    /// <summary>
    /// Asigna huespedes a un PR
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 31/May/2016 Modified. Ahora el metodo es asincroino
    /// </history>
    private async void btnAssign_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateAssign())
      {
        txtStatus.Text = "Assigning...";
        status.Visibility = Visibility.Visible;
        int res = 0;
        res = await BRAssignment.SaveGuetsPRAssign(_strgGuestUnassigned, _strgPRs);
        FilterRecords();
        UIHelper.ShowMessageResult("Assignment", res);
      }
    }
    #endregion

    #region btnRemove_Click
    /// <summary>
    /// Remueve asignación de huespedes
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 31/May/2016 Modified. Ahora el metodo es asincrono
    /// </history>
    private async void btnRemove_Click(object sender, RoutedEventArgs e)
    {
      if (ValidateUnassign())
      {
        txtStatus.Text = "Removing...";
        status.Visibility = Visibility.Visible;
        int res = 0;
        res = await BRAssignment.SaveGuestUnassign(_strgGuestAssigned, _strgPRs);
        FilterRecords();
        UIHelper.ShowMessageResult("Remove Assignment", res);
        
      }
    }
    #endregion

    #region grdListMarkets_SelectionChanged
    /// <summary>
    /// Guarda lista de mercados seleccionados
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
    private void grdListMarkets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int cont = 0; _markets = string.Empty;
      var selectedItems = dtgListMarkets.SelectedItems;
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

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    ///   Verifica que teclas estan presionadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 18/Jun/2016 Created
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
    ///   [vku] 18/Jun/2016 Created
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

    #region grdPRAssigned_SelectionChanged
    /// <summary>
    /// Guarda lista de PRs Seleccionados que tienen huespedes asignados
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
    private void grdPRAssigned_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int cont = 0; _strgPRs = string.Empty; _strgNamePR = string.Empty;
      var selectedItems = dtgPRAssigned.SelectedItems;
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
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
    private void grdGuestUnassigned_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _strgGuestUnassigned.Clear();
      var selectedItems = dtgGuestUnassigned.SelectedItems;
      foreach (GuestUnassigned selectedItem in selectedItems)
      {
        _strgGuestUnassigned.Add(selectedItem.guID);
      }
      StatusBarReg.Content = string.Format("{0}/{1}", dtgGuestUnassigned.SelectedItems.Count, dtgGuestUnassigned.Items.Count);
    }
    #endregion

    #region grdGuestAssigned_SelectionChanged
    /// <summary>
    /// Guarda lista de huespedes seleccionados que estan asignados
    /// </summary>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    /// </history>
    private void grdGuestAssigned_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _strgGuestAssigned.Clear();
      var selectedItems = dtgGuestAssigned.SelectedItems;
      foreach (GuestAssigned selectedItem in selectedItems)
      {
        _strgGuestAssigned.Add(selectedItem.guID);
      }
    }
    #endregion

    #endregion
  }
}
