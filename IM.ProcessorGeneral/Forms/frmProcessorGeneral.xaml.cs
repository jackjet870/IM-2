using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using System.Data;
using IM.Base.Helpers;
using System.IO;
using IM.ProcessorGeneral.Classes;
using System.Diagnostics;
using System.Collections;

namespace IM.ProcessorGeneral.Forms
{
  /// <summary>
  /// Interaction logic for frmProcessor.xaml
  /// </summary>
  public partial class frmProcessorGeneral : Window
  {
    #region Atributos

    private bool _skipSelectionChanged;//Bandera para evitar que el evento SelectionChanged se dispare al asignar el datasource.

    private frmFilterDateRange _frmFilter;
    private bool _blnOneDate;
    private bool _blnOnlyOneRegister;

    public List<int> _lstGifts = new List<int>();
    public List<int> _lstGiftsCate = new List<int>();
    public List<int> _lstSalesRoom = new List<int>();
    public List<int> _lstLeadSources = new List<int>();
    public List<int> _lstPrograms = new List<int>();
    public List<int> _lstRateTypes = new List<int>();
    public EnumPredefinedDate? _cboDateSelected;
    public DateTime _dtmStart = DateTime.Now.Date;
    public DateTime _dtmEnd = DateTime.Now.Date;
    public EnumBasedOnArrival _enumBasedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival;
    public EnumBasedOnBooking _enumBasedOnBooking = EnumBasedOnBooking.bobNoBasedOnBooking;
    public EnumQuinellas _enumQuinellas = EnumQuinellas.quNoQuinellas;
    public EnumDetailGifts _enumDetailsGift = EnumDetailGifts.dgNoDetailGifts;
    public EnumSalesByMemberShipType _enumSalesByMemberShipType = EnumSalesByMemberShipType.sbmNoDetail;
    public EnumStatus _enumStatus = EnumStatus.staActives;
    public EnumGiftsReceiptType _enumGiftsReceiptType = EnumGiftsReceiptType.grtAll;
    public string _GuestID = "";
    public EnumGiftSale _enumGiftSale = EnumGiftSale.gsAll;
    public EnumSaveCourtesyTours _enumSaveCourtesyTours = EnumSaveCourtesyTours.sctExcludeSaveCourtesyTours;
    public EnumExternalInvitation _enumExternalInvitation = EnumExternalInvitation.extExclude;

    #endregion

    #region Constructor
    public frmProcessorGeneral()
    {
      InitializeComponent();
    }

    #endregion

    #region Eventos Formulario

    #region Window_Loaded
    /// <summary>
    /// Método de inicio del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void Window_Loaded(object sender, EventArgs e)
    {
      ConfigurarGrids();
      lblUserName.Content = App.User.User.peN;
    }
    #endregion

    #region ReportsBySalesRoom

    #region grdrptSalesRooms_dblClick
    /// <summary>
    /// Método para abrir la ventana de filtros
    /// al hacer doble clic sobre alguno registro del
    /// gridSalesRooms.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void grdrptSalesRooms_dblClick(object sender, RoutedEventArgs e)
    {
      PrepareReportBySalesRoom();
    }
    #endregion

    #region grdrptSalesRooms_PreviewKeyDown
    /// <summary>
    /// Activa/Desactiva el botón de Diseñador de reportes. 
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    private void grdrptSalesRooms_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter) return;

      if (grdrptSalesRooms.SelectedIndex >= 0)
      {
        PrepareReportBySalesRoom();
      }
    }

    #endregion

    #endregion

    #region ReportsByLeadSource

    #region grdrptLeadSource_dblClick

    /// <summary>
    /// Método para abrir la ventana de filtros
    /// al hacer doble clic sobre alguno registro del
    /// gridLeadSource.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    private void grdrptLeadSource_dblClick(object sender, RoutedEventArgs e)
    {
      PrepareReportByLeadSource();
    }

    #endregion

    #region grdrptLeadSources_PreviewKeyDown

    /// <summary>
    /// Activa/Desactiva el botón de Diseñador de reportes. 
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    private void grdrptLeadSources_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        if (grdrptLeadSources.SelectedIndex >= 0)
        {
          PrepareReportByLeadSource();
        }
      }
    }
    #endregion

    #endregion

    #region ReportsGeneral

    #region grdrptGeneral_dblClick
    /// <summary>
    /// Método para abrir la ventana de filtros
    /// al hacer doble clic sobre alguno registro del
    /// gridSalesRooms.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    private void grdrptGeneral_dblClick(object sender, RoutedEventArgs e)
    {
      PrepareGeneralReport();
    }

    #endregion

    #region grdrptGeneral_PreviewKeyDown
    /// <summary>
    /// Activa/Desactiva el botón de Diseñador de reportes. 
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    private void grdrptGeneral_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        if (grdrptGeneral.SelectedIndex >= 0)
        {
          PrepareGeneralReport();
        }
      }
    }
    #endregion

    #endregion

    #region Window_Closing
    /// <summary>
    /// Cambia la propiedad CloseAllowed de la ventana frmFilterDateRange
    /// para permitir el cierre de la misma.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Mar/2016 Created
    /// </history>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      Application.Current.Shutdown();
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [edgrodriguez] 14/Mar/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Capital)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      }
      else if (e.Key == Key.Insert)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      }
      else if (e.Key == Key.NumLock)
      {
        KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
      }
    }
    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// Verifica si los botones estan activos
    /// </summary>
    /// <history>
    /// [edgrodriguez] 14/Mar/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region btnPrintSR_Click
    /// <summary>
    /// Mediante el botónPrint abre el formulario FilterDateRange
    /// para posteriormente guardar el reporte en excel.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void btnPrintSR_Click(object sender, RoutedEventArgs e)
    {
      PrepareReportBySalesRoom();
    }
    #endregion

    #region btnPrintLS_Click

    /// <summary>
    /// Mediante el botónPrint abre el formulario FilterDateRange
    /// para posteriormente guardar el reporte en excel.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    private void btnPrintLS_Click(object sender, RoutedEventArgs e)
    {
      PrepareReportByLeadSource();
    }

    #endregion

    #region btnPrintGral_Click

    /// <summary>
    /// Mediante el botónPrint abre el formulario FilterDateRange
    /// para posteriormente guardar el reporte en excel.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void btnPrintGral_Click(object sender, RoutedEventArgs e)
    {
      PrepareGeneralReport();
    }

    #endregion

    #region btnExit_Click
    /// <summary>
    /// Cierra la aplicacion Processor General.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }
    #endregion

    #endregion

    #region Métodos Privados

    #region Reports By SalesRoom
    #region PrepareReportsBySalesRoom
    /// <summary>
    /// Prepara un reporte por Sala de venta.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    private void PrepareReportBySalesRoom()
    {
      //Validamos que haya un reporte seleccionado.
      if (grdrptSalesRooms.SelectedIndex < 0)
        return;

      WaitMessage(true, "Loading Date Range Window...");

      //Obtenemos el nombre del reporte.
      string strReport = ((dynamic)grdrptSalesRooms.SelectedItem).rptNombre;
      #region Configurando Fecha
      switch (strReport)
      {
        //Reportes que utilizan solo una fecha
        case "Gifts By Category":
        case "Gifts By Category & Program":
        case "Daily Gifts (Simple)":
        case "Weekly Gifts (ITEMS) (Simple)":
        case "Weekly and Monthly Hostess":
        case "Weekly and Monthly Hostess (Golf & Sunrise)":
        case "Bookings By Sales Room, Program & Time":
        case "Bookings By Sales Room, Program, Lead Source & Time":
          _blnOneDate = true;
          break;
        default:
          _blnOneDate = false;
          break;
      }
      #endregion
      #region Configurando Grids Modo de Seleccion
      switch (strReport)
      {
        //Reportes que permiten seleccionar solo un registro.
        case "CxC":
        case "CxC (Excel)":
        case "CxC Not Authorized":
        case "CxC Payments":
          _blnOnlyOneRegister = true;
          break;
        default:
          _blnOnlyOneRegister = false;
          break;
      }
      #endregion
      AbrirFilterDateRangeSalesRoom(strReport);
    }
    #endregion

    #region AbrirFilterDateRangeSalesRoom
    /// <summary>
    /// Abre la ventana frmFilterDateRange configurando
    /// los controles segun el reporte seleccionado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    private void AbrirFilterDateRangeSalesRoom(string strReport)
    {
      _frmFilter = new frmFilterDateRange { FrmProcGen = this };
      #region Abriendo FrmFilter segun reporte seleccionado.
      switch (strReport)
      {
        case "Weekly and Monthly Hostess (Golf & Sunrise)":
          _frmFilter.ConfigurarFomulario(blnOneDate: _blnOneDate);
          break;
        case "Deposits":
        case "Guests No Buyers":
        case "Memberships":
        case "Memberships by Host":
        case "Bookings By Sales Room, Program & Time":
        case "Bookings By Sales Room, Program, Lead Source & Time":
        case "In & Out":
        case "No Shows":
        case "Burned Deposits":
        case "Burned Deposits by Resort":
        case "Paid Deposits":
        case "Taxis In":
        case "Taxis Out":
        case "Gifts By Category":
        case "Gifts By Category & Program":
        case "Memberships by Agency & Market":
        case "Liner Statistics":
        case "Closer Statistics":
        case "Cancelled Gifts Manifest":
        case "CxC Not Authorized":
        case "CxC Gifts":
        case "CxC Deposits":
        case "Gifts Receipts Payments":
        case "CxC Payments":
        case "Guest CECO":
          _frmFilter.ConfigurarFomulario(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnSalesRoom: true);
          break;
        case "Gifts Manifest":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnGifts: true, blnAllGifts: true, blnCategories: true, blnAllCategories: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister,
            blncbStatus: true);
          break;
        case "Gifts Receipts":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnGifts: true, blnAllGifts: true, blnCategories: true, blnAllCategories: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister,
            blncbStatus: true, blnGiftReceiptType: true, blnGuestId: true);
          break;
        case "Gifts Certificates":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnGifts: true, blnAllGifts: true, blnCategories: true, blnAllCategories: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
        case "Gifts Sale":
          //case "Gifts Sale (Excel)":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnGifts: true, blnAllGifts: true, blnCategories: true, blnAllCategories: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister,
            blnGiftSale: true);
          break;
        case "Manifest":
        case "Manifest (Excel)":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnPrograms: true, blnAllPrograms: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
        case "Gifts Used by Sistur":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnPrograms: true, blnAllPrograms: true, blnLeadSources: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
        case "Production by Sales Room":
        case "Production by Sales Room & Market":
        case "Production by Sales Room, Program, Market & Submarket":
        case "Production by Show Program":
        case "Production by Show Program & Program":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, enumBasedOnArrival: EnumBasedOnArrival.boaBasedOnArrival, enumQuinellas: EnumQuinellas.quQuinellas);
          break;
        case "Meal Tickets":
        case "Meal Tickets by Host":
        case "Meal Tickets Cancelled":
        case "Meal Tickets with Cost":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnRatetypes: true, blnAllRatetypes: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
        default:
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
      }
      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter.BlnOk)
      {
        ShowSalesRoomReport(strReport);
        _frmFilter.Close();
      }
      else
      {
        _frmFilter.Close();
        _frmFilter = null;
      }
      #endregion
    }
    #endregion

    #region ShowSalesRoomReport
    /// <summary>
    /// Muestra el reporte seleccionado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 09/Mar/2016 Created
    /// </history>
    private void ShowSalesRoomReport(string strReport)
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_dtmStart, _dtmStart) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      WaitMessage(true, "Loading report...");
      filters.Add(new Tuple<string, string>("Date Range", dateRange));
      filters.Add(new Tuple<string, string>("Sales Room", _frmFilter.grdSalesRoom.SelectedItems.Count == _frmFilter.grdSalesRoom.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList())));

      switch (strReport)
      {

        #region Reportes Bookings

        #region Bookings By Sales Room, Program & Time
        case "Bookings By Sales Room, Program & Time":
          List<RptBookingsBySalesRoomProgramTime> lstRptBbSalesRoom = BRReportsBySalesRoom.GetRptBookingsBySalesRoomProgramTime(_dtmStart, (_frmFilter.chkAllSalesRoom.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptBbSalesRoom.Count > 0)
          {
            finfo = clsReports.ExportRptBookingsBySalesRoomProgramTime(strReport, dateRangeFileNameRep, filters, lstRptBbSalesRoom);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion
        #region Bookings By Sales Room, Program, Lead Source & Time
        case "Bookings By Sales Room, Program, Lead Source & Time":
          List<RptBookingsBySalesRoomProgramLeadSourceTime> lstRptBbSalesRoomPlst = BRReportsBySalesRoom.GetRptBookingsBySalesRoomProgramLeadSourceTime(_dtmStart, (_frmFilter.chkAllSalesRoom.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptBbSalesRoomPlst.Count > 0)
          {
            finfo = clsReports.ExportRptBookingsBySalesRoomProgramLeadSourceTime(strReport, dateRangeFileNameRep, filters, lstRptBbSalesRoomPlst);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion

        #endregion

        #region Reportes CxC

        #region CxC
        case "CxC":
          List<RptCxCExcel> lstRptCxCExcel = BRReportsBySalesRoom.GetRptCxC(_dtmStart, _dtmEnd, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList()));
          if (lstRptCxCExcel.Count > 0)
          {
            finfo = clsReports.ExportRptCxC(strReport, dateRangeFileNameRep, filters, lstRptCxCExcel);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion
        #region CxC By Type
        case "CxC By Type":
          List<RptCxC> lstRptCxC = BRReportsBySalesRoom.GetRptCxCByType(_dtmStart, _dtmEnd, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList()));
          if (lstRptCxC.Count > 0)
          {
            finfo = clsReports.ExportRptCxCByType(strReport, dateRangeFileNameRep, filters, lstRptCxC);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion
        #region CxC Deposits
        case "CxC Deposits":
          List<object> lstRptCxCDeposits = BRReportsBySalesRoom.GetRptCxCDeposits(_dtmStart, _dtmEnd, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList()));
          if (lstRptCxCDeposits.Any())
          {
            finfo = clsReports.ExportRptCxCDeposits(strReport, dateRangeFileNameRep, filters, lstRptCxCDeposits);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion
        #region CxC Gifts
        case "CxC Gifts":
          List<object> lstRptCxCGifts = BRReportsBySalesRoom.GetRptCxCGifts(_dtmStart, _dtmEnd, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptCxCGifts.Any())
          {
            finfo = clsReports.ExportRptCxCGift(strReport, dateRangeFileNameRep, filters, lstRptCxCGifts);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion
        #region CxC Not Authorized
        case "CxC Not Authorized":
          List<RptCxCNotAuthorized> lstRptCxCNotAut = BRReportsBySalesRoom.GetRptCxCNotAuthorized(_dtmStart, _dtmEnd, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList()));
          if (lstRptCxCNotAut.Count > 0)
          {
            finfo = clsReports.ExportRptCxCNotAuthorized(strReport, dateRangeFileNameRep, filters, lstRptCxCNotAut);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion
        #region CxC Payments
        case "CxC Payments":
          List<RptCxCPayments> lstRptCxCPayments = BRReportsBySalesRoom.GetRptCxCPayments(_dtmStart, _dtmEnd, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList()));
          if (lstRptCxCPayments.Count > 0)
          {
            finfo = clsReports.ExportRptCxCPayments(strReport, dateRangeFileNameRep, filters, lstRptCxCPayments);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion

        #endregion

        #region Reportes Deposits

        #region Deposits
        case "Deposits":
          List<object> lstRptDeposits = BRReportsBySalesRoom.GetRptDeposits(_dtmStart, _dtmEnd, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList()));
          if (lstRptDeposits.Count > 0)
          {
            finfo = clsReports.ExportRptDeposits(strReport, dateRangeFileNameRep, filters, lstRptDeposits);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion
        #region Burned Deposits
        case "Burned Deposits":
          List<object> lstRptBurnedDeposits = BRReportsBySalesRoom.GetRptDepositsBurned(_dtmStart, _dtmEnd, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList()));
          if (lstRptBurnedDeposits.Count > 0)
          {
            finfo = clsReports.ExportRptBurnedDeposits(strReport, dateRangeFileNameRep, filters, lstRptBurnedDeposits, _dtmStart.Date, _dtmEnd.Date);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion
        #region Burned Deposits by Resorts
        case "Burned Deposits by Resort":
          List<object> lstRptBurnedDepositsResort = BRReportsBySalesRoom.GetRptDepositsBurnedByResort(_dtmStart, _dtmEnd, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList()));
          if (lstRptBurnedDepositsResort.Count > 0)
          {
            finfo = clsReports.ExportRptBurnedDepositsByResorts(strReport, dateRangeFileNameRep, filters, lstRptBurnedDepositsResort, _dtmStart.Date, _dtmEnd.Date);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion
        #region Paid Deposits
        case "Paid Deposits":
          List<object> lstRptPaidDeposits = BRReportsBySalesRoom.GetRptPaidDeposits(_dtmStart, _dtmEnd, (_frmFilter.chkAllSalesRoom.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptPaidDeposits.Count > 0)
          {
            finfo = clsReports.ExportRptPaidDeposits(strReport, dateRangeFileNameRep, filters, lstRptPaidDeposits);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;
        #endregion

        #endregion

        #region Reportes Gifts

        #region Daily Gifts (Simple)
        case "Daily Gifts (Simple)":
          List<RptDailyGiftSimple> lstRptDailyG =
            BRReportsBySalesRoom.GetRptDailyGiftSimple(_dtmStart,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptDailyG.Count > 0)
          {
            finfo = clsReports.ExportRptDailyGiftSimple(strReport, dateRangeFileNameRep, filters, lstRptDailyG);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Gifts By Category

        case "Gifts By Category":
          List<RptGiftsByCategory> lstRptGiftByCat = BRReportsBySalesRoom.GetRptGiftsByCategory(_dtmStart, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptGiftByCat.Count > 0)
          {
            finfo = clsReports.ExportRptGiftsByCategory(strReport, dateRangeFileNameRep, filters, lstRptGiftByCat);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Gifts By Category & Program

        case "Gifts By Category & Program":
          List<RptGiftsByCategoryProgram> lstRptGiftByCatP = BRReportsBySalesRoom.GetRptGiftsByCategoryProgram(_dtmStart, string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptGiftByCatP.Count > 0)
          {
            finfo = clsReports.ExportRptGiftsByCategoryProgram(strReport, dateRangeFileNameRep, filters, lstRptGiftByCatP);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Gifts Certificates
        case "Gifts Certificates":
          List<object> lstRptGifsCerts = BRReportsBySalesRoom.GetRptGiftsCertificates(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
            (_frmFilter.chkAllCategories.IsChecked ?? false)
              ? "ALL"
              : string.Join(",", _frmFilter.grdCategories.SelectedItems.OfType<GiftCategory>().Select(c => c.gcID)),
            (_frmFilter.chkAllGifts.IsChecked ?? false)
              ? "ALL"
              : string.Join(",", _frmFilter.grdGifts.SelectedItems.OfType<GiftShort>().Select(c => c.giID)));

          if (lstRptGifsCerts.Count > 0)
          {
            finfo = clsReports.ExportRptGiftsCertificates(strReport, dateRangeFileNameRep, filters, lstRptGifsCerts);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Gifts Manifest
        case "Gifts Manifest":
          List<IEnumerable> lstRptGiftsManifest = BRReportsBySalesRoom.GetRptGiftsManifest(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
            (_frmFilter.chkAllCategories.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdCategories.SelectedItems.OfType<GiftCategory>().Select(c => c.gcID)),
            (_frmFilter.chkAllGifts.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdGifts.SelectedItems.OfType<GiftShort>().Select(c => c.giID)),
            (EnumStatus)_frmFilter.cboStatus.SelectedValue);

          if (lstRptGiftsManifest.Count > 0)
          {
            finfo = clsReports.ExportRptGiftsManifest(strReport, dateRangeFileNameRep, filters, lstRptGiftsManifest);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Gifts Receipts
        case "Gifts Receipts":
          List<IEnumerable> lstRptGiftsReceipts = BRReportsBySalesRoom.GetRptGiftsMReceipts(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
            (_frmFilter.chkAllCategories.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdCategories.SelectedItems.OfType<GiftCategory>().Select(c => c.gcID)),
            (_frmFilter.chkAllGifts.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdGifts.SelectedItems.OfType<GiftShort>().Select(c => c.giID)),
            (EnumStatus)_frmFilter.cboStatus.SelectedValue, (EnumGiftsReceiptType)_frmFilter.cboGiftsReceiptType.SelectedValue,
            (_frmFilter.txtGuestID.Text == string.Empty) ? 0 : Convert.ToInt32(_frmFilter.txtGuestID.Text));

          if (lstRptGiftsReceipts.Count > 0)
          {
            finfo = clsReports.ExportRptGiftsReceipts(strReport, dateRangeFileNameRep, filters, lstRptGiftsReceipts);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Gifts Receipts Payments
        case "Gifts Receipts Payments":
          List<object> lstRptGifsReceiptsPay =
            BRReportsBySalesRoom.GetRptGiftsReceiptsPayments(_dtmStart,
              _dtmEnd,
              string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));

          if (lstRptGifsReceiptsPay.Count > 0)
          {
            finfo = clsReports.ExportRptGiftsReceiptsPayments(strReport, dateRangeFileNameRep, filters,
              lstRptGifsReceiptsPay);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Gifts Sale

        case "Gifts Sale":
          List<IEnumerable> lstRptGiftsSale = BRReportsBySalesRoom.GetRptGiftsSale(_dtmStart,
            _dtmEnd,
            (_frmFilter.chkAllSalesRoom.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
            (_frmFilter.chkAllGifts.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdGifts.SelectedItems.OfType<GiftShort>().Select(c => c.giID)),
            (_frmFilter.chkAllCategories.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdCategories.SelectedItems.OfType<GiftCategory>().Select(c => c.gcID)),
            (EnumGiftSale)_frmFilter.cboGiftSale.SelectedValue);

          if (lstRptGiftsSale.Count > 0)
          {
            finfo = clsReports.ExportRptGiftsSale(strReport, dateRangeFileNameRep, filters, lstRptGiftsSale);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Gifts Used by Sistur

        case "Gifts Used by Sistur":
          List<RptGiftsUsedBySistur> lstRptGiftsSistur =
            BRReportsBySalesRoom.GetRptGiftsUsedBySistur(_dtmStart, _dtmEnd,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
              (_frmFilter.chkAllPrograms.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdPrograms.SelectedItems.OfType<Program>().Select(c => c.pgID)),
              (_frmFilter.chkAllLeadSources.IsChecked ?? false)
                ? "ALL"
                : string.Join(",",
                  _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID)));
          if (lstRptGiftsSistur.Count > 0)
          {
            finfo = clsReports.ExportRptGiftsUsedBySistur(strReport, dateRangeFileNameRep, filters, lstRptGiftsSistur);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Weekly Gifts (ITEMS) (Simple)

        case "Weekly Gifts (ITEMS) (Simple)":
          List<RptWeeklyGiftsItemsSimple> lstWeeklyGiftsSimple =
            BRReportsBySalesRoom.GetRptWeeklyGiftsItemsSimple(_dtmStart,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstWeeklyGiftsSimple.Count > 0)
          {
            finfo = clsReports.ExportRptWeeklyGiftSimple(strReport, dateRangeFileNameRep, filters, lstWeeklyGiftsSimple);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #endregion

        #region Reportes Guest

        #region Guest CECO

        case "Guest CECO":
          List<RptGuestCeco> lstRptGuestCeco = BRReportsBySalesRoom.GetRptGuestCeco(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptGuestCeco.Count > 0)
          {
            finfo = clsReports.ExportRptGuestCeco(strReport, dateRangeFileNameRep, filters, lstRptGuestCeco);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Guests No Buyers

        case "Guests No Buyers":
          List<RptGuestsNoBuyers> lstRptGuestNoBuyers =
            BRReportsBySalesRoom.GetRptGuestNoBuyers(_dtmStart, _dtmEnd,
              string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptGuestNoBuyers.Count > 0)
          {
            finfo = clsReports.ExportRptGuestNoBuyers(strReport, dateRangeFileNameRep, filters, lstRptGuestNoBuyers);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region In & Out

        case "In & Out":
          List<RptInOut> lstRptInOut = BRReportsBySalesRoom.GetRptInOut(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstRptInOut.Count > 0)
          {
            finfo = clsReports.ExportRptInOut(strReport, dateRangeFileNameRep, filters, lstRptInOut);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region No Shows

        case "No Shows":
          List<RptGuestsNoShows> lstGuestNoShows =
            BRReportsBySalesRoom.GetRptGuestNoShows(_dtmStart, _dtmEnd,
              string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstGuestNoShows.Count > 0)
          {
            finfo = clsReports.ExportRptGuestNoShows(strReport, dateRangeFileNameRep, filters, lstGuestNoShows);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #endregion

        #region Reportes Meal Tickets

        #region Meal Tickets Simple, ByHost & Cancelled)

        case "Meal Tickets":
        case "Meal Tickets by Host":
        case "Meal Tickets Cancelled":
          List<RptMealTickets> lstMealTickets = BRReportsBySalesRoom.GetRptMealTickets(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
            (strReport == "Meal Tickets Cancelled"),
            (_frmFilter.chkAllRatetypes.IsChecked ?? false)
              ? "ALL"
              : string.Join(",", _frmFilter.grdRatetypes.SelectedItems.OfType<RateType>().Select(c => c.raID)));
          if (lstMealTickets.Count > 0)
          {
            finfo = clsReports.ExportRptMealTickets(strReport, dateRangeFileNameRep, filters, lstMealTickets,
              (strReport == "Meal Tickets by Host"));
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Meal Tickets Cost

        case "Meal Tickets with Cost":
          List<RptMealTicketsCost> lstMealTicketsCost =
            BRReportsBySalesRoom.GetRptMealTicketsCost(_dtmStart, _dtmEnd,
              string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
              (_frmFilter.chkAllRatetypes.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdRatetypes.SelectedItems.OfType<RateType>().Select(c => c.raID)));
          if (lstMealTicketsCost.Count > 0)
          {
            finfo = clsReports.ExportRptMealTicketsCost(strReport, dateRangeFileNameRep, filters, lstMealTicketsCost);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #endregion

        #region Reportes Membership

        #region Memberships

        case "Memberships":
          List<RptMemberships> lstMemberships = BRReportsBySalesRoom.GetRptMemberships(_dtmStart,
            _dtmEnd,
            (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
              ? "ALL"
              : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstMemberships.Count > 0)
          {
            finfo = clsReports.ExportRptMemberships(strReport, dateRangeFileNameRep, filters, lstMemberships);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Memberships by Agency & Market

        case "Memberships by Agency & Market":
          List<RptMembershipsByAgencyMarket> lstMembershipsAgencyM =
            BRReportsBySalesRoom.GetRptMembershipsByAgencyMarket(_dtmStart,
              _dtmEnd,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstMembershipsAgencyM.Count > 0)
          {
            finfo = clsReports.ExportRptMembershipsByAgencyMarket(strReport, dateRangeFileNameRep, filters,
              lstMembershipsAgencyM);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Memberships by Host

        case "Memberships by Host":
          List<RptMembershipsByHost> lstMembershipsHost =
            BRReportsBySalesRoom.GetRptMembershipsByHost(_dtmStart, _dtmEnd,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstMembershipsHost.Count > 0)
          {
            finfo = clsReports.ExportRptMembershipsByHost(strReport, dateRangeFileNameRep, filters, lstMembershipsHost);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #endregion

        #region Reportes Production

        #region Production by Sales Room

        case "Production by Sales Room":
          List<RptProductionBySalesRoom> lstProductionBySr =
            BRReportsBySalesRoom.GetRptProductionBySalesRoom(_dtmStart,
              _dtmEnd,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
              _frmFilter.chkQuinellas.IsChecked ?? false, _frmFilter.chkBasedOnArrival.IsChecked ?? false);
          if (lstProductionBySr.Count > 0)
          {
            if (_frmFilter.chkBasedOnArrival.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
            if (_frmFilter.chkQuinellas.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

            finfo = clsReports.ExportRptProductionBySalesRoom(strReport, dateRangeFileNameRep, filters,
              lstProductionBySr);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Production by Sales Room & Market

        case "Production by Sales Room & Market":
          List<RptProductionBySalesRoomMarket> lstProductionBySrm =
            BRReportsBySalesRoom.GetRptProductionBySalesRoomMarket(_dtmStart,
              _dtmEnd,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
              _frmFilter.chkQuinellas.IsChecked ?? false, _frmFilter.chkBasedOnArrival.IsChecked ?? false);
          if (lstProductionBySrm.Count > 0)
          {
            if (_frmFilter.chkBasedOnArrival.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
            if (_frmFilter.chkQuinellas.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

            finfo = clsReports.ExportRptProductionBySalesRoomMarket(strReport, dateRangeFileNameRep, filters,
              lstProductionBySrm);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Production by Sales Room, Program, Market & Submarket

        case "Production by Sales Room, Program, Market & Submarket":
          List<RptProductionBySalesRoomProgramMarketSubmarket> lstProductionBySrmSm =
            BRReportsBySalesRoom.GetRptProductionBySalesRoomMarketSubMarket(_dtmStart,
              _dtmEnd,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
              _frmFilter.chkQuinellas.IsChecked ?? false, _frmFilter.chkBasedOnArrival.IsChecked ?? false);
          if (lstProductionBySrmSm.Count > 0)
          {
            if (_frmFilter.chkBasedOnArrival.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
            if (_frmFilter.chkQuinellas.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

            finfo = clsReports.ExportRptProductionBySalesRoomMarketSubMarket(strReport, dateRangeFileNameRep, filters,
              lstProductionBySrmSm);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Production by Show Program

        case "Production by Show Program":
          List<RptProductionByShowProgram> lstProductionByShowProgram =
            BRReportsBySalesRoom.GetRptProductionByShowProgram(_dtmStart,
              _dtmEnd,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
              _frmFilter.chkQuinellas.IsChecked ?? false, _frmFilter.chkBasedOnArrival.IsChecked ?? false);
          if (lstProductionByShowProgram.Count > 0)
          {
            if (_frmFilter.chkBasedOnArrival.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
            if (_frmFilter.chkQuinellas.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

            finfo = clsReports.ExportRptProductionByShowProgram(strReport, dateRangeFileNameRep, filters,
              lstProductionByShowProgram);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Production by Show, Program & Program

        case "Production by Show Program & Program":
          List<RptProductionByShowProgramProgram> lstProductionByShowProgramPro =
            BRReportsBySalesRoom.GetRptProductionByShowProgramProgram(_dtmStart,
              _dtmEnd,
              (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
                ? "ALL"
                : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)),
              _frmFilter.chkQuinellas.IsChecked ?? false, _frmFilter.chkBasedOnArrival.IsChecked ?? false);
          if (lstProductionByShowProgramPro.Count > 0)
          {
            if (_frmFilter.chkBasedOnArrival.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
            if (_frmFilter.chkQuinellas.IsChecked ?? false)
              filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

            finfo = clsReports.ExportRptProductionByShowProgramProgram(strReport, dateRangeFileNameRep, filters,
              lstProductionByShowProgramPro);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #endregion

        #region Reportes Salesmen

        #region Closer Statistics

        case "Closer Statistics":
          List<RptCloserStatistics> lstCloserStatistics = BRReportsBySalesRoom.GetRptCloserStatistics(_dtmStart,
              _dtmEnd, (_frmFilter.chkAllSalesRoom.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstCloserStatistics.Count > 0)
          {            
            finfo = clsReports.ExportRptCloserStatistics(strReport, dateRangeFileNameRep, filters, lstCloserStatistics);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Liner Statistics

        case "Liner Statistics":
          List<RptLinerStatistics> lstLinerStatistics = BRReportsBySalesRoom.GetRptLinerStatistics(_dtmStart,
              _dtmEnd, (_frmFilter.chkAllSalesRoom.IsChecked ?? false) ? "ALL" : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstLinerStatistics.Count > 0)
          {
            finfo = clsReports.ExportRptLinerStatistics(strReport, dateRangeFileNameRep, filters, lstLinerStatistics);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #endregion

        #region Reportes Taxis

        #region Taxis In

        case "Taxis In":
          List<RptTaxisIn> lstTaxisIn = BRReportsBySalesRoom.GetRptTaxisIn(_dtmStart,
            _dtmEnd,
            (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
              ? "ALL"
              : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstTaxisIn.Count > 0)
          {
            finfo = clsReports.ExportRptTaxiIn(strReport, dateRangeFileNameRep, filters, lstTaxisIn);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Taxis Out

        case "Taxis Out":
          List<RptTaxisOut> lstTaxisOut = BRReportsBySalesRoom.GetRptTaxisOut(_dtmStart,
            _dtmEnd,
            (_frmFilter.chkAllSalesRoom.IsChecked ?? false)
              ? "ALL"
              : string.Join(",", _frmFilter.grdSalesRoom.SelectedItems.OfType<SalesRoomByUser>().Select(c => c.srID)));
          if (lstTaxisOut.Count > 0)
          {
            finfo = clsReports.ExportRptTaxiOut(strReport, dateRangeFileNameRep, filters, lstTaxisOut);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

          #endregion

          #endregion
      }

      if (finfo != null)
      {
        Process.Start(finfo.FullName);
      }

      WaitMessage(false);
    }

    #endregion

    #endregion

    #region Reports By LeadSource

    #region PrepareReportByLeadSource

    /// <summary>
    /// Prepara un reporte por Lead Source.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    private void PrepareReportByLeadSource()
    {
      //Validamos que haya un reporte seleccionado.
      if (grdrptLeadSources.SelectedIndex < 0)
        return;

      _blnOneDate = false;
      _blnOnlyOneRegister = false;

      WaitMessage(true, "Loading Date Range Window...");

      //Obtenemos el nombre del reporte.
      string strReport = ((dynamic)grdrptLeadSources.SelectedItem).rptNombre;
      AbrirFilterDateRangeLeadsSource(strReport);
    }

    #endregion

    #region AbrirFilterDateRangeLeadsSource

    /// <summary>
    /// Abre la ventana frmFilterDateRange configurando
    /// los controles segun el reporte seleccionado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    private void AbrirFilterDateRangeLeadsSource(string strReport)
    {
      _frmFilter = new frmFilterDateRange { FrmProcGen = this };

      #region Abriendo FrmFilter segun reporte seleccionado.

      _frmFilter.ConfigurarFomulario(blnLeadSources: true, blnOneDate: _blnOneDate,
        blnOnlyOneRegister: _blnOnlyOneRegister);

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter.BlnOk)
      {
        ShowLeadSourceReport(strReport);
        _frmFilter.Close();
      }
      else
      {
        _frmFilter.Close();
        _frmFilter = null;
      }

      #endregion
    }

    #endregion

    #region ShowLeadSourceReport

    /// <summary>
    /// Muestra el reporte seleccionado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 18/Abr/2016 Created
    /// </history>
    private void ShowLeadSourceReport(string strReport)
    {
      string dateRange = (_blnOneDate)
        ? DateHelper.DateRange(_dtmStart, _dtmStart)
        : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = (_blnOneDate)
        ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart)
        : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);
      FileInfo finfo = null;
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      string leadSources = (_frmFilter.chkAllLeadSources.IsChecked ?? false)
        ? "ALL"
        : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID));
      filters.Add(new Tuple<string, string>("Date Range", dateRange));
      filters.Add(new Tuple<string, string>("Lead Sources",
        _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count
          ? "ALL"
          : string.Join(",",
            _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));
      WaitMessage(true, "Loading report...");

      switch (strReport)
      {
        #region Burned Deposits Guests

        case "Burned Deposits Guests":
          List<object> lstRptBurnedDepsGuest =
            BRReportsByLeadSource.GetRptDepositsBurnedGuests(_dtmStart,
              _dtmEnd,
              string.Join(",", _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID)));
          if (lstRptBurnedDepsGuest.Count > 0)
          {
            finfo = clsReports.ExportRptBurnedDepositsGuests(strReport, dateRangeFileNameRep, filters,
              lstRptBurnedDepsGuest);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Deposit Refund

        case "Deposit Refund":
          List<RptDepositRefund> lstRptDepRef =
            BRReportsByLeadSource.GetRptDepositRefunds(_dtmStart, _dtmEnd,
              leadSources);
          if (lstRptDepRef.Count > 0)
          {
            finfo = clsReports.ExportRptDepositRefunds(strReport, dateRangeFileNameRep, filters, lstRptDepRef);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Deposits by PR

        case "Deposits by PR":
          List<object> lstRptDepPr = BRReportsByLeadSource.GetRptDepositByPR(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID)));
          if (lstRptDepPr.Count > 0)
          {
            finfo = clsReports.ExportRptDepositByPr(strReport, dateRangeFileNameRep, filters, lstRptDepPr);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Deposits No Show

        case "Deposits No Show":
          List<object> lstRptDepNoShow = BRReportsByLeadSource.GetRptDepositsNoShow(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID)));
          if (lstRptDepNoShow.Count > 0)
          {
            finfo = clsReports.ExportRptDepositsNoShow(strReport, dateRangeFileNameRep, filters, lstRptDepNoShow);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region In & Out by PR

        case "In & Out by PR":
          List<RptInOutByPR> lstRptInOutPr = BRReportsByLeadSource.GetRptInOutByPR(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID)));
          if (lstRptInOutPr.Count > 0)
          {
            finfo = clsReports.ExportRptInOutByPr(strReport, dateRangeFileNameRep, filters, lstRptInOutPr);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Memberships

        case "Memberships":
          List<RptMemberships> lstMemberships = BRReportsBySalesRoom.GetRptMemberships(_dtmStart,
            _dtmEnd, leadSources: leadSources);
          if (lstMemberships.Count > 0)
          {
            finfo = clsReports.ExportRptMemberships(strReport, dateRangeFileNameRep, filters, lstMemberships);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Memberships by Host

        case "Memberships by Host":
          List<RptMembershipsByHost> lstMembershipsHost =
            BRReportsBySalesRoom.GetRptMembershipsByHost(_dtmStart, _dtmEnd,
              leadSources: leadSources);
          if (lstMembershipsHost.Count > 0)
          {
            finfo = clsReports.ExportRptMembershipsByHost(strReport, dateRangeFileNameRep, filters, lstMembershipsHost);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Paid Deposits

        case "Paid Deposits by PR":
          List<object> lstRptPaidDeposits = BRReportsBySalesRoom.GetRptPaidDeposits(_dtmStart,
            _dtmEnd, leadSources: leadSources);
          if (lstRptPaidDeposits.Count > 0)
          {
            finfo = clsReports.ExportRptPaidDeposits(strReport, dateRangeFileNameRep, filters, lstRptPaidDeposits, true);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Personnel Access

        case "Personnel Access":
          List<RptPersonnelAccess> lstPersonnelAccess =
            BRReportsByLeadSource.GetRptPersonnelAccess(string.Join(",",
              _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID)));
          if (lstPersonnelAccess.Count > 0)
          {
            finfo = clsReports.ExportRptPersonnelAccess(strReport, dateRangeFileNameRep, filters, lstPersonnelAccess);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Self Gen

        case "Self Gen":
          var lstRptSelfGen = BRReportsByLeadSource.GetRptSelfGen(_dtmStart,
            _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID)));
          if (lstRptSelfGen.Item1.Count > 0)
          {
            finfo = clsReports.ExportRptSelfGen(strReport, dateRangeFileNameRep, filters, lstRptSelfGen,
              _dtmStart, _dtmEnd);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

          #endregion
      }

      if (finfo != null)
      {
        Process.Start(finfo.FullName);
      }

      WaitMessage(false);
    }

    #endregion

    #endregion

    #region General Reports

    #region PrepareGeneralReport

    /// <summary>
    /// Prepara un reporte general.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    private void PrepareGeneralReport()
    {
      //Validamos que haya un reporte seleccionado.
      if (grdrptGeneral.SelectedIndex < 0)
        return;

      //Obtenemos el nombre del reporte.
      string strReport = ((dynamic)grdrptGeneral.SelectedItem).rptNombre;

      _blnOneDate = false;

      switch (strReport)
      {
        //Reportes que solo deben permitir selecionar un registro
        case "Gifts Kardex":
        case "Warehouse Movements":
          _blnOnlyOneRegister = true;
          break;

        //Reportes que permiten selecionar mas de un registro
        default:
          _blnOnlyOneRegister = false;
          break;
      }
      AbrirFilterDateRangeGeneral(strReport);

    }

    #endregion

    #region AbrirFilterDateRangeGeneral

    /// <summary>
    /// Abre la ventana frmFilterDateRange configurando
    /// los controles segun el reporte seleccionado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    private void AbrirFilterDateRangeGeneral(string strReport)
    {
      _frmFilter = new frmFilterDateRange { FrmProcGen = this };

      #region Abriendo FrmFilter segun reporte seleccionado.

      //desplegamos el filtro de fechas
      switch (strReport)
      {
        case "Gifts Kardex":
        case "Warehouse Movements":
          _frmFilter.ConfigurarFomulario(blnOnlyOneRegister: _blnOnlyOneRegister, blnWarehouse: true);
          break;
        case "Sales By Program, Lead Source & Market":
          _frmFilter.ConfigurarFomulario();
          break;
        case "Production Referral":
          _frmFilter.ConfigurarFomulario(enumPeriod: EnumPeriod.Monthly);
          break;
        //case "Gifts Given In Kind Electronic Purse":
        //blnOK = ShowDateRangeGifts(strGifts, "giN, giProductGiftsCard", "Gift, Product E-Purse", "giProductGiftsCard")
        //break;
        case "Production by Lead Source & Market (Monthly)":
          _frmFilter.ConfigurarFomulario(enumPeriod: EnumPeriod.Monthly,
            enumBasedOnArrival: EnumBasedOnArrival.boaBasedOnArrival, enumQuinellas: EnumQuinellas.quQuinellas,
            enumExternalInvitation: EnumExternalInvitation.extInclude);
          break;
        default:
          _frmFilter = null;
          WaitMessage(true, (strReport != "Logins Log") ? "Loading Report..." : "Loading Date Range Window...");
          ShowGeneralReport(strReport);
          WaitMessage(false);
          break;
      }
      if (_frmFilter == null) return;

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter.BlnOk)
      {
        ShowGeneralReport(strReport, true);
        _frmFilter.Close();
      }
      else
      {
        _frmFilter.Close();
        _frmFilter = null;
      }

      #endregion
    }

    #endregion

    #region ShowGeneralReport

    /// <summary>
    /// Muestra el reporte seleccionado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    private void ShowGeneralReport(string strReport, bool blndateRange = false)
    {
      var dateRange = (blndateRange)
        ? ((_blnOneDate)
          ? DateHelper.DateRange(_dtmStart, _dtmStart)
          : DateHelper.DateRange(_dtmStart, _dtmEnd))
        : "";
      var dateRangeFileNameRep = (blndateRange)
        ? ((_blnOneDate)
          ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart)
          : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd))
        : "";
      FileInfo finfo = null;
      var filters = new List<Tuple<string, string>>();
      WaitMessage(true, "Loading report...");
      switch (strReport)
      {
        #region Agencies

        case "Agencies":
          List<RptAgencies> lstRptAgencies = BRGeneralReports.GetRptAgencies();
          if (lstRptAgencies.Count > 0)
          {
            finfo = clsReports.ExportRptAgencies(strReport, "", filters, lstRptAgencies);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Gifts

        case "Gifts":
          List<RptGifts> lstRptGift = BRGeneralReports.GetRptGifts();
          if (lstRptGift.Count > 0)
          {
            finfo = clsReports.ExportRptGifts(strReport, "", filters, lstRptGift);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Logins Log

        case "Logins Log":
          frmLoginLog loginLog = new frmLoginLog();
          loginLog.ShowDialog();
          break;

        #endregion

        #region Personnel

        case "Personnel":
          List<RptPersonnel> lstRptPersonnel = BRGeneralReports.GetRptPersonnel();
          if (lstRptPersonnel.Count > 0)
          {
            finfo = clsReports.ExportRptPersonnel(strReport, "", filters, lstRptPersonnel);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Production by Lead Source & Market (Monthly)

        case "Production by Lead Source & Market (Monthly)":
          List<RptProductionByLeadSourceMarketMonthly> lstRptProductionByLsMarketMonthly =
            BRGeneralReports.GetRptProductionByLeadSourceMarketMonthly(_dtmStart,
              _dtmEnd,
              (_frmFilter.chkQuinellas.IsChecked ?? false) ? EnumQuinellas.quQuinellas : EnumQuinellas.quNoQuinellas,
              (EnumExternalInvitation)_frmFilter.cboExternal.SelectedValue,
              (_frmFilter.chkBasedOnArrival.IsChecked ?? false)
                ? EnumBasedOnArrival.boaBasedOnArrival
                : EnumBasedOnArrival.boaNoBasedOnArrival);
          if (lstRptProductionByLsMarketMonthly.Count > 0)
          {
            finfo = clsReports.ExportRptProductionByLeadSourceMarketMonthly(strReport, dateRangeFileNameRep, filters,
              lstRptProductionByLsMarketMonthly);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Production Referral

        case "Production Referral":
          List<RptProductionReferral> lstRptProductionReferral =
            BRGeneralReports.GetRptProductionReferral(_dtmStart, _dtmEnd);
          if (lstRptProductionReferral.Count > 0)
          {
            finfo = clsReports.ExportRptProductionReferral(strReport, dateRangeFileNameRep, filters,
              lstRptProductionReferral);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Reps

        case "Reps":
          List<Rep> lstRptReps = BRGeneralReports.GetRptReps();
          if (lstRptReps.Count > 0)
          {
            finfo = clsReports.ExportRptReps(strReport, dateRangeFileNameRep, filters, lstRptReps);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Sales By Program, Lead Source & Market

        case "Sales By Program, Lead Source & Market":
          List<RptSalesByProgramLeadSourceMarket> lstRptSalesByProgramLeadSourceMarkets =
            BRGeneralReports.GetRptSalesByProgramLeadSourceMarket(_dtmStart,
              _dtmEnd);
          if (lstRptSalesByProgramLeadSourceMarkets.Count > 0)
          {
            finfo = clsReports.ExportRptSalesByProgramLeadSourceMarket(strReport, dateRangeFileNameRep, filters,
              lstRptSalesByProgramLeadSourceMarkets);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

        #endregion

        #region Warehouse Movements

        case "Warehouse Movements":
          List<RptWarehouseMovements> lstRptWarehouseMovements =
            BRGeneralReports.GetRptWarehouseMovements(_dtmStart, _dtmEnd,
              ((WarehouseByUser)_frmFilter.grdWarehouse.SelectedItem).whID);
          if (lstRptWarehouseMovements.Count > 0)
          {
            finfo = clsReports.ExportRptWarehouseMovements(strReport, dateRangeFileNameRep, filters,
              lstRptWarehouseMovements);
          }
          else
            UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          break;

          #endregion
      }

      if (finfo != null)
      {
        Process.Start(finfo.FullName);
      }

      WaitMessage(false);
    }

    #endregion

    #endregion

    #region ConfigurarGrids

    /// <summary>
    /// Se configuran los treeview, agregando 
    /// los reportes.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void ConfigurarGrids()
    {
      try
      {
        _skipSelectionChanged = true;

        #region Grid SalesRooms

        ListCollectionView lstrptSalesRooms = new ListCollectionView(new List<dynamic>
        {
          new {rptNombre = "Bookings By Sales Room, Program & Time", rptGroup = "Bookings"},
          new {rptNombre = "Bookings By Sales Room, Program, Lead Source & Time", rptGroup = "Bookings"},

          new {rptNombre = "CxC By Type", rptGroup = "CxC"},
          new {rptNombre = "CxC Gifts", rptGroup = "CxC"},
          new {rptNombre = "CxC Deposits", rptGroup = "CxC"},
          new {rptNombre = "CxC", rptGroup = "CxC"},
          new {rptNombre = "CxC Not Authorized", rptGroup = "CxC"},
          new {rptNombre = "CxC Payments", rptGroup = "CxC"},

          new {rptNombre = "Deposits", rptGroup = "Deposits"},
          new {rptNombre = "Paid Deposits", rptGroup = "Deposits"},
          new {rptNombre = "Burned Deposits", rptGroup = "Deposits"},
          new {rptNombre = "Burned Deposits by Resort", rptGroup = "Deposits"},

          new {rptNombre = "Daily Gifts (Simple)", rptGroup = "Gifts"},
          new {rptNombre = "Gifts Manifest", rptGroup = "Gifts"},
          new {rptNombre = "Cancelled Gifts Manifest", rptGroup = "Gifts"},
          new {rptNombre = "Gifts By Category", rptGroup = "Gifts"},
          new {rptNombre = "Gifts By Category & Program", rptGroup = "Gifts"},
          new {rptNombre = "Weekly Gifts (ITEMS) (Simple)", rptGroup = "Gifts"},
          new {rptNombre = "Gifts Certificates", rptGroup = "Gifts"},
          new {rptNombre = "Gifts Receipts", rptGroup = "Gifts"},
          new {rptNombre = "Gifts Sale", rptGroup = "Gifts"},
          new {rptNombre = "Gifts Receipts Payments", rptGroup = "Gifts"},
          //new {rptNombre = "Gifts Sale (Excel)", rptGroup = "Gifts"},
          new {rptNombre = "Gifts Used by Sistur", rptGroup = "Gifts"},

          new {rptNombre = "Manifest", rptGroup = "Guests"},
          new {rptNombre = "Manifest (Excel)", rptGroup = "Guests"},
          new {rptNombre = "No Shows", rptGroup = "Guests"},
          new {rptNombre = "Guests No Buyers", rptGroup = "Guests"},
          new {rptNombre = "In & Out", rptGroup = "Guests"},
          new {rptNombre = "Guest CECO", rptGroup = "Guests"},

          new {rptNombre = "Meal Tickets", rptGroup = "Meal Tickets"},
          new {rptNombre = "Meal Tickets by Host", rptGroup = "Meal Tickets"},
          new {rptNombre = "Meal Tickets Cancelled", rptGroup = "Meal Tickets"},
          new {rptNombre = "Meal Tickets with Cost", rptGroup = "Meal Tickets"},

          new {rptNombre = "Memberships", rptGroup = "Memberships"},
          new {rptNombre = "Memberships by Host", rptGroup = "Memberships"},
          new {rptNombre = "Memberships by Agency & Market", rptGroup = "Memberships"},

          new {rptNombre = "Production by Sales Room", rptGroup = "Production"},
          new {rptNombre = "Production by Sales Room & Market", rptGroup = "Production"},
          new {rptNombre = "Production by Sales Room, Program, Market & Submarket", rptGroup = "Production"},
          new {rptNombre = "Production by Show Program", rptGroup = "Production"},
          new {rptNombre = "Production by Show Program & Program", rptGroup = "Production"},

          new {rptNombre = "Liner Statistics", rptGroup = "Salesmen"},
          new {rptNombre = "Closer Statistics", rptGroup = "Salesmen"},
          new {rptNombre = "Weekly and Monthly Hostess", rptGroup = "Salesmen"},
          new {rptNombre = "Weekly and Monthly Hostess (Golf & Sunrise)", rptGroup = "Salesmen"},

          new {rptNombre = "Taxis In", rptGroup = "Taxis"},
          new {rptNombre = "Taxis Out", rptGroup = "Taxis"}
        }.OrderBy(c => c.rptGroup).ThenBy(c => c.rptNombre).ToList());

        lstrptSalesRooms.GroupDescriptions?.Add(new PropertyGroupDescription("rptGroup"));
        grdrptSalesRooms.ItemsSource = lstrptSalesRooms;

        #endregion

        #region Grid LeadSources

        List<dynamic> lstrptLeadSources = new List<dynamic>
        {
          new {rptNombre = "Deposits by PR"},
          new {rptNombre = "Deposits No Show"},
          new {rptNombre = "Paid Deposits by PR"},
          new {rptNombre = "Memberships"},
          new {rptNombre = "Memberships by Host"},
          new {rptNombre = "Self Gen"},
          new {rptNombre = "Personnel Access"},
          new {rptNombre = "In & Out by PR"},
          new {rptNombre = "Burned Deposits Guests"},
          new {rptNombre = "Deposit Refund"}
        }.OrderBy(c => c.rptNombre).ToList();
        grdrptLeadSources.ItemsSource = lstrptLeadSources;

        #endregion

        #region Grid General

        var lstrptGeneral = new List<dynamic>
        {
          new {rptNombre = "Personnel"},
          new {rptNombre = "Reps"},
          new {rptNombre = "Agencies"},
          new {rptNombre = "Gifts"},
          new {rptNombre = "Gifts Kardex"},
          new {rptNombre = "Warehouse Movements"},
          new {rptNombre = "Logins Log"},
          new {rptNombre = "Production Referral"},
          //new {rptNombre="Gifts Given In Kind Electronic Purse"},
          new {rptNombre = "Sales By Program, Lead Source & Market"},
          new {rptNombre = "Production by Lead Source & Market (Monthly)"}
        }.OrderBy(c => c.rptNombre).ToList();
        grdrptGeneral.ItemsSource = lstrptGeneral;

        #endregion

        StatusBarReg.Content = $"{lstrptGeneral.Count() + lstrptLeadSources.Count + lstrptSalesRooms.Count} Reports";
      }
      finally
      {
        _skipSelectionChanged = false;
      }
    }

    #endregion

    #region WaitMessage

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    /// [edgrodriguez] 02/Mar/2016 Created
    /// </history>
    private void WaitMessage(bool show, string message = "")
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = (show) ? Visibility.Visible : Visibility.Hidden;
      Cursor = (show) ? Cursors.Wait : null;
      UIHelper.ForceUIToUpdate();
    }

    #endregion

    #endregion
  }
}