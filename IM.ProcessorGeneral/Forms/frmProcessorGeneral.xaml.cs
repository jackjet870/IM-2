using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.ProcessorGeneral.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;

namespace IM.ProcessorGeneral.Forms
{
  /// <summary>
  /// Interaction logic for frmProcessor.xaml
  /// </summary>
  public partial class frmProcessorGeneral : Window
  {
    #region Atributos

    private frmFilterDateRange _frmFilter;
    private frmReportQueue _frmReportQueue;
    private bool _blnOneDate;
    private bool _blnOnlyOneRegister;
    public clsFilter _clsFilter;

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
      SetupParameters();
      lblUserName.Content = Context.User.User.peN;
      _frmReportQueue = new frmReportQueue(Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly));
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
      PrepareReportBySalesRoom();
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
      if (e.Key != Key.Enter) return;
      
      if (grdrptLeadSources.SelectedIndex >= 0)
      {
        PrepareReportByLeadSource();
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
      if (e.Key != Key.Enter) return;
      
      if (grdrptGeneral.SelectedIndex >= 0)
      {
        PrepareGeneralReport();
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
      switch (e.Key)
      {
        case Key.Capital:
          KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
          break;
        case Key.Insert:
          KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
          break;
        case Key.NumLock:
          KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
          break;
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

    #region btnReportQueue_Click
    /// <summary>
    /// Configura la ruta para guardar los reportes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [edgrodriguez] 17/Jun/2016 Created
    /// </history>
    private void btnReportQueue_Click(object sender, RoutedEventArgs e)
    {    
      _frmReportQueue.Show();
      if (_frmReportQueue.WindowState == WindowState.Minimized)
        _frmReportQueue.WindowState = WindowState.Normal;
      _frmReportQueue.Activate();
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
        case "CxC By Type":
        case "CxC Not Authorized":
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
      _frmFilter = new frmFilterDateRange { FrmProcGen = this, Owner = this };
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
        case "Accounting Codes":
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
        case "Manifest by LS":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnPrograms: true, blnAllPrograms: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
        case "Gifts Used by Sistur":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnPrograms: true, blnAllPrograms: true, blnLeadSources: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnDateBaseOn: true,strReportName: "Gifts Used by Sistur");
          break;
        case "Production by Sales Room":
        case "Production by Sales Room & Market":
        case "Production by Sales Room, Program, Market & Submarket":
        case "Production by Show Program":
        case "Production by Show Program & Program":
          _frmFilter.ConfigurarFomulario(blnSalesRoom: true, blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, enumBasedOnArrival: EnumBasedOnArrival.BasedOnArrival, enumQuinellas: EnumQuinellas.Quinellas);
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
        ShowSalesRoomReport(strReport, _clsFilter);
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
    private async void ShowSalesRoomReport(string strReportName, clsFilter filter)
    {
      FileInfo finfo = null;
      var dateRange = (_blnOneDate) ? DateHelper.DateRange(filter.StartDate, filter.StartDate) : DateHelper.DateRange(filter.StartDate, filter.EndDate);
      var dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(filter.StartDate, filter.StartDate) : DateHelper.DateRangeFileName(filter.StartDate, filter.EndDate);

      var filters = new List<Tuple<string, string>>
      {
        new Tuple<string, string>("Date Range", dateRange),
        new Tuple<string, string>("Sales Room", filter.AllSalesRooms ? "ALL" : string.Join(",", filter.LstSalesRooms))
      };

      string fileFullPath = ReportBuilder.CreateEmptyExcel(strReportName, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, strReportName);
      try
      {
        switch (strReportName)
        {

          #region Reportes Bookings

          #region Bookings By Sales Room, Program & Time
          case "Bookings By Sales Room, Program & Time":
            var lstRptBbSalesRoom = await BRReportsBySalesRoom.GetRptBookingsBySalesRoomProgramTime(filter.StartDate, (filter.AllSalesRooms) ? "ALL" : string.Join(",", filter.LstSalesRooms));
            if (lstRptBbSalesRoom.Any())
              finfo = await clsReports.ExportRptBookingsBySalesRoomProgramTime(strReportName, fileFullPath, filters, lstRptBbSalesRoom);
            break;
          #endregion
          #region Bookings By Sales Room, Program, Lead Source & Time
          case "Bookings By Sales Room, Program, Lead Source & Time":
            var lstRptBbSalesRoomPlst = await BRReportsBySalesRoom.GetRptBookingsBySalesRoomProgramLeadSourceTime(filter.StartDate, (filter.AllSalesRooms) ? "ALL" : string.Join(",", filter.LstSalesRooms));
            if (lstRptBbSalesRoomPlst.Any())
              finfo = await clsReports.ExportRptBookingsBySalesRoomProgramLeadSourceTime(strReportName, fileFullPath, filters, lstRptBbSalesRoomPlst);
            break;
          #endregion

          #endregion

          #region Reportes CxC

          #region CxC
          case "CxC":
            var lstRptCxC = await BRReportsBySalesRoom.GetRptCxCByType(filter.StartDate, filter.EndDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptCxC.Any())
              finfo = await clsReports.ExportRptCxc(strReportName, fileFullPath, filters, lstRptCxC);
            break;
          #endregion
          #region CxC By Type
          case "CxC By Type":
           var lstRptCxCExcel = await BRReportsBySalesRoom.GetRptCxC(filter.StartDate, filter.EndDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptCxCExcel.Any())
              finfo = await clsReports.ExportRptCxcByType(strReportName, fileFullPath, filters, lstRptCxCExcel);
            break;
          #endregion
          #region CxC Deposits
          case "CxC Deposits":
            var lstRptCxCDeposits = await BRReportsBySalesRoom.GetRptCxCDeposits(filter.StartDate, filter.EndDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptCxCDeposits.Any())
              finfo = await clsReports.ExportRptCxCDeposits(strReportName, fileFullPath, filters, lstRptCxCDeposits);
            break;
          #endregion
          #region CxC Gifts
          case "CxC Gifts":
            var lstRptCxCGifts = await BRReportsBySalesRoom.GetRptCxCGifts(filter.StartDate, filter.EndDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptCxCGifts.Any())
              finfo = await clsReports.ExportRptCxCGift(strReportName, fileFullPath, filters, lstRptCxCGifts);
            break;
          #endregion
          #region CxC Not Authorized
          case "CxC Not Authorized":
            var lstRptCxCNotAut = await BRReportsBySalesRoom.GetRptCxCNotAuthorized(filter.StartDate, filter.EndDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptCxCNotAut.Any())
              finfo = await clsReports.ExportRptCxCNotAuthorized(strReportName, fileFullPath, filters, lstRptCxCNotAut);
            break;
          #endregion
          #region CxC Payments
          case "CxC Payments":
            var lstRptCxCPayments = await BRReportsBySalesRoom.GetRptCxCPayments(filter.StartDate, filter.EndDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptCxCPayments.Any())
              finfo = await clsReports.ExportRptCxCPayments(strReportName, fileFullPath, filters, lstRptCxCPayments);
            break;
          #endregion

          #endregion

          #region Reportes Deposits

          #region Deposits
          case "Deposits":
            var lstRptDeposits = await BRReportsBySalesRoom.GetRptDeposits(filter.StartDate, filter.EndDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptDeposits.Any())
              finfo = await clsReports.ExportRptDeposits(strReportName, fileFullPath, filters, lstRptDeposits);
            break;
          #endregion
          #region Burned Deposits
          case "Burned Deposits":
            var lstRptBurnedDeposits = await BRReportsBySalesRoom.GetRptDepositsBurned(filter.StartDate, filter.EndDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptBurnedDeposits.Any())
              finfo = await clsReports.ExportRptBurnedDeposits(strReportName, fileFullPath, filters, lstRptBurnedDeposits, filter.StartDate.Date, filter.EndDate.Date);
            break;
          #endregion
          #region Burned Deposits by Resorts
          case "Burned Deposits by Resort":
            var lstRptBurnedDepositsResort = await BRReportsBySalesRoom.GetRptDepositsBurnedByResort(filter.StartDate, filter.EndDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptBurnedDepositsResort.Any())
              finfo = await clsReports.ExportRptBurnedDepositsByResorts(strReportName, fileFullPath, filters, lstRptBurnedDepositsResort, filter.StartDate.Date, filter.EndDate.Date);
            break;
          #endregion
          #region Paid Deposits
          case "Paid Deposits":
            var lstRptPaidDeposits = await BRReportsBySalesRoom.GetRptPaidDeposits(filter.StartDate, filter.EndDate, (filter.AllSalesRooms) ? "ALL" : string.Join(",", filter.LstSalesRooms));
            if (lstRptPaidDeposits.Any())
              finfo = await clsReports.ExportRptPaidDeposits(strReportName, fileFullPath, filters, lstRptPaidDeposits);
            break;
          #endregion

          #endregion

          #region Reportes Gifts

          #region Cancelled Gifts Manifest
          case "Cancelled Gifts Manifest":
            var lstRptCancelledGift = await BRReportsBySalesRoom.GetRptGiftsCancelledManifest(filter.StartDate, filter.EndDate, (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms));
            if (lstRptCancelledGift.Any())
              finfo = await clsReports.ExportRptCancelledGiftsManifest(strReportName, fileFullPath, filters, lstRptCancelledGift);
            break;
          #endregion
          #region Daily Gifts (Simple)
          case "Daily Gifts (Simple)":
            var lstRptDailyG = await BRReportsBySalesRoom.GetRptDailyGiftSimple(filter.StartDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms));
            if (lstRptDailyG.Any())
              finfo = await clsReports.ExportRptDailyGiftSimple(strReportName, fileFullPath, filters, lstRptDailyG);
            break;

          #endregion
          #region Gifts By Category

          case "Gifts By Category":
            var lstRptGiftByCat = await BRReportsBySalesRoom.GetRptGiftsByCategory(filter.StartDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptGiftByCat.Any())
              finfo = await clsReports.ExportRptGiftsByCategory(strReportName, fileFullPath, filters, lstRptGiftByCat);
            break;

          #endregion
          #region Gifts By Category & Program

          case "Gifts By Category & Program":
            var lstRptGiftByCatP = await BRReportsBySalesRoom.GetRptGiftsByCategoryProgram(filter.StartDate, string.Join(",", filter.LstSalesRooms));
            if (lstRptGiftByCatP.Any())
              finfo = await clsReports.ExportRptGiftsByCategoryProgram(strReportName, fileFullPath, filters, lstRptGiftByCatP);
            break;

          #endregion
          #region Gifts Certificates
          case "Gifts Certificates":
            var lstRptGifsCerts = await BRReportsBySalesRoom.GetRptGiftsCertificates(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstSalesRooms),
              (filter.AllGiftsCate)
                ? "ALL"
                : string.Join(",", filter.LstGiftsCate),
              (filter.AllGifts)
                ? "ALL"
                : string.Join(",", filter.LstGifts));

            if (lstRptGifsCerts.Any())
            {
              filters.Add(Tuple.Create("Categories", (filter.AllGiftsCate ? "ALL" : string.Join(",", filter.LstGiftsCate))));
              filters.Add(Tuple.Create("Gifts", (filter.AllGifts ? "ALL" : string.Join(",", filter.LstGifts))));
              finfo = await clsReports.ExportRptGiftsCertificates(strReportName, fileFullPath, filters, lstRptGifsCerts);
            }
            break;

          #endregion
          #region Gifts Manifest
          case "Gifts Manifest":
            var lstRptGiftsManifest = await BRReportsBySalesRoom.GetRptGiftsManifest(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstSalesRooms),
              (filter.AllGiftsCate) ? "ALL" : string.Join(",", filter.LstGiftsCate),
              (filter.AllGifts) ? "ALL" : string.Join(",", filter.LstGifts),
              (EnumStatus)_frmFilter.cboStatus.SelectedValue);

            if (lstRptGiftsManifest.Any())
            {
              filters.Add(Tuple.Create("Categories", (filter.AllGiftsCate ? "ALL" : string.Join(",", filter.LstGiftsCate))));
              filters.Add(Tuple.Create("Gifts", (filter.AllGifts ? "ALL" : string.Join(",", filter.LstGifts))));
              filters.Add(Tuple.Create("Status", (EnumToListHelper.GetEnumDescription(filter.Status))));
              finfo = await clsReports.ExportRptGiftsManifest(strReportName, fileFullPath, filters, lstRptGiftsManifest);
            } break;

          #endregion
          #region Gifts Receipts
          case "Gifts Receipts":
            var lstRptGiftsReceipts = await BRReportsBySalesRoom.GetRptGiftsMReceipts(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstSalesRooms),
              (filter.AllGiftsCate) ? "ALL" : string.Join(",", filter.LstGiftsCate),
              (filter.AllGifts) ? "ALL" : string.Join(",", filter.LstGifts),
              (EnumStatus)_frmFilter.cboStatus.SelectedValue, (EnumGiftsReceiptType)_frmFilter.cboGiftsReceiptType.SelectedValue,
              (_frmFilter.txtGuestID.Text == string.Empty) ? 0 : Convert.ToInt32(_frmFilter.txtGuestID.Text));

            if (lstRptGiftsReceipts.Any())
            {
              filters.Add(Tuple.Create("Categories", (filter.AllGiftsCate ? "ALL" : string.Join(",", filter.LstGiftsCate))));
              filters.Add(Tuple.Create("Gifts", (filter.AllGifts ? "ALL" : string.Join(",", filter.LstGifts))));
              filters.Add(Tuple.Create("Status", (EnumToListHelper.GetEnumDescription(filter.Status))));
              filters.Add(Tuple.Create("Types", (EnumToListHelper.GetEnumDescription(filter.GiftsReceiptType))));
              filters.Add(Tuple.Create("Guest ID", (filter.GuestId == "0" || string.IsNullOrWhiteSpace(filter.GuestId) ? "ALL" : filter.GuestId)));

              finfo = await clsReports.ExportRptGiftsReceipts(strReportName, fileFullPath, filters, lstRptGiftsReceipts);
            }
            break;

          #endregion
          #region Gifts Receipts Payments
          case "Gifts Receipts Payments":
            var lstRptGifsReceiptsPay = await BRReportsBySalesRoom.GetRptGiftsReceiptsPayments(filter.StartDate,
                filter.EndDate,
                string.Join(",", filter.LstSalesRooms));

            if (lstRptGifsReceiptsPay.Any())
              finfo = await clsReports.ExportRptGiftsReceiptsPayments(strReportName, fileFullPath, filters,
                lstRptGifsReceiptsPay);
            break;

          #endregion
          #region Gifts Sale

          case "Gifts Sale":
            var lstRptGiftsSale = await BRReportsBySalesRoom.GetRptGiftsSale(filter.StartDate,
              filter.EndDate,
              (filter.AllSalesRooms) ? "ALL" : string.Join(",", filter.LstSalesRooms),
              (filter.AllGiftsCate) ? "ALL" : string.Join(",", filter.LstGiftsCate),
              (filter.AllGifts) ? "ALL" : string.Join(",", filter.LstGifts),
              (EnumGiftSale)_frmFilter.cboGiftSale.SelectedValue);

            if (lstRptGiftsSale.Any())
            {
              filters.Add(Tuple.Create("Categories", (filter.AllGiftsCate ? "ALL" : string.Join(",", filter.LstGiftsCate))));
              filters.Add(Tuple.Create("Gifts", (filter.AllGifts ? "ALL" : string.Join(",", filter.LstGifts))));
              filters.Add(Tuple.Create("Gift Sale", (EnumToListHelper.GetEnumDescription(filter.GiftSale))));

              finfo = await clsReports.ExportRptGiftsSale(strReportName, fileFullPath, filters, lstRptGiftsSale);
            }
            break;

          #endregion
          #region Gifts Used by Sistur

          case "Gifts Used by Sistur":
            var lstRptGiftsSistur = await BRReportsBySalesRoom.GetRptGiftsUsedBySistur(filter.StartDate, filter.EndDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms),
                (filter.AllPrograms)
                  ? "ALL"
                  : string.Join(",", filter.LstPrograms),
                (filter.AllLeadSources)
                  ? "ALL"
                  : string.Join(",",
                    filter.LstLeadSources),Convert.ToInt32(_clsFilter.DateBasedOn));
            if (lstRptGiftsSistur.Any())
            {
              filters.Add(Tuple.Create("Program", (filter.AllPrograms ? "ALL" : string.Join(",", filter.LstPrograms))));
              filters.Add(Tuple.Create("Lead Source", (filter.AllLeadSources ? "ALL" : string.Join(",", filter.LstLeadSources))));

              finfo = await clsReports.ExportRptGiftsUsedBySistur(strReportName, fileFullPath, filters, lstRptGiftsSistur);
            }
            break;

          #endregion
          #region Weekly Gifts (ITEMS) (Simple)

          case "Weekly Gifts (ITEMS) (Simple)":
            var lstWeeklyGiftsSimple = await BRReportsBySalesRoom.GetRptWeeklyGiftsItemsSimple(filter.StartDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms));
            if (lstWeeklyGiftsSimple.Any())
              finfo = await clsReports.ExportRptWeeklyGiftSimple(strReportName, fileFullPath, filters, lstWeeklyGiftsSimple);
            break;

          #endregion

          #endregion

          #region Reportes Guest

          #region Accounting Codes

          case "Accounting Codes":
            var lstRptGuestCeco = await BRReportsBySalesRoom.GetRptGuestCeco(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstSalesRooms));
            if (lstRptGuestCeco.Any())
              finfo = clsReports.ExportRptGuestCeco(strReportName, fileFullPath, filters, lstRptGuestCeco);
            break;

          #endregion
          #region Guests No Buyers

          case "Guests No Buyers":
            var lstRptGuestNoBuyers = await BRReportsBySalesRoom.GetRptGuestNoBuyers(filter.StartDate, filter.EndDate,
                string.Join(",", filter.LstSalesRooms));
            if (lstRptGuestNoBuyers.Any())
              finfo = await clsReports.ExportRptGuestNoBuyers(strReportName, fileFullPath, filters, lstRptGuestNoBuyers);
            break;

          #endregion
          #region In & Out

          case "In & Out":
            var lstRptInOut = await BRReportsBySalesRoom.GetRptInOut(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstSalesRooms));
            if (lstRptInOut.Any())
              finfo = await clsReports.ExportRptInOut(strReportName, fileFullPath, filters, lstRptInOut);
            break;

          #endregion
          #region Manifest

          case "Manifest":
            var lstRptManifestRange = await BRReportsBySalesRoom.GetRptManifestRange(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstSalesRooms));
            if (lstRptManifestRange.Any())
            {
              filters.Add(Tuple.Create("Program", (filter.AllPrograms ? "ALL" : string.Join(",", filter.LstPrograms))));

              finfo = await clsReports.ExportRptManifestRange(strReportName, fileFullPath, filters, lstRptManifestRange);
            }
            break;

          #endregion
          #region Manifest by Lead Source

          case "Manifest by Lead Source":
            var lstRptManifestRangeByLs = await BRReportsBySalesRoom.GetRptManifestRangeByLs(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstSalesRooms));
            if (lstRptManifestRangeByLs.Any())
            {
              filters.Add(Tuple.Create("Program", (filter.AllPrograms ? "ALL" : string.Join(",", filter.LstPrograms))));

              finfo = await clsReports.ExportRptManifestRangeByLs(strReportName, fileFullPath, filters, lstRptManifestRangeByLs);
            }
            break;

          #endregion
          #region No Shows

          case "No Shows":
            var lstGuestNoShows = await BRReportsBySalesRoom.GetRptGuestNoShows(filter.StartDate, filter.EndDate,
                string.Join(",", filter.LstSalesRooms));
            if (lstGuestNoShows.Any())
              finfo = await clsReports.ExportRptGuestNoShows(strReportName, fileFullPath, filters, lstGuestNoShows);
            break;

          #endregion

          #endregion

          #region Reportes Meal Tickets

          #region Meal Tickets Simple, ByHost & Cancelled)

          case "Meal Tickets":
          case "Meal Tickets by Host":
          case "Meal Tickets Cancelled":
            var lstMealTickets = await BRReportsBySalesRoom.GetRptMealTickets(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstSalesRooms),
              (strReportName == "Meal Tickets Cancelled"),
              (filter.AllRateTypes)
                ? "ALL"
                : string.Join(",", filter.LstRateTypes));
            if (lstMealTickets.Any())
            {
              filters.Add(Tuple.Create("Rate Type", (filter.AllRateTypes ? "ALL" : string.Join(",", filter.LstRateTypes))));

              finfo = await clsReports.ExportRptMealTickets(strReportName, fileFullPath, filters, lstMealTickets,
                (strReportName == "Meal Tickets by Host"));
            }
            break;

          #endregion
          #region Meal Tickets Cost

          case "Meal Tickets with Cost":
            var lstMealTicketsCost = await BRReportsBySalesRoom.GetRptMealTicketsCost(filter.StartDate, filter.EndDate,
                string.Join(",", filter.LstSalesRooms),
                (filter.AllRateTypes)
                  ? "ALL"
                  : string.Join(",", filter.LstRateTypes));
            if (lstMealTicketsCost.Any())
            {
              filters.Add(Tuple.Create("Rate Type", (filter.AllRateTypes ? "ALL" : string.Join(",", filter.LstRateTypes))));

              finfo = await clsReports.ExportRptMealTicketsCost(strReportName, fileFullPath, filters, lstMealTicketsCost);
            }
            break;

          #endregion

          #endregion

          #region Reportes Membership

          #region Memberships

          case "Memberships":
            var lstMemberships = await BRReportsBySalesRoom.GetRptMemberships(filter.StartDate,
              filter.EndDate,
              (filter.AllSalesRooms)
                ? "ALL"
                : string.Join(",", filter.LstSalesRooms));
            if (lstMemberships.Any())
              finfo = await clsReports.ExportRptMemberships(strReportName, fileFullPath, filters, lstMemberships);
            break;

          #endregion
          #region Memberships by Agency & Market

          case "Memberships by Agency & Market":
            var lstMembershipsAgencyM = await BRReportsBySalesRoom.GetRptMembershipsByAgencyMarket(filter.StartDate,
                filter.EndDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms));
            if (lstMembershipsAgencyM.Any())
              finfo = await clsReports.ExportRptMembershipsByAgencyMarket(strReportName, fileFullPath, filters,
                lstMembershipsAgencyM);
            break;

          #endregion
          #region Memberships by Host

          case "Memberships by Host":
            var lstMembershipsHost = await BRReportsBySalesRoom.GetRptMembershipsByHost(filter.StartDate, filter.EndDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms));
            if (lstMembershipsHost.Any())
              finfo = await clsReports.ExportRptMembershipsByHost(strReportName, fileFullPath, filters, lstMembershipsHost);
            break;

          #endregion

          #endregion

          #region Reportes Production

          #region Production by Sales Room

          case "Production by Sales Room":
            var lstProductionBySr = await BRReportsBySalesRoom.GetRptProductionBySalesRoom(filter.StartDate,
                filter.EndDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms),
                Convert.ToBoolean(filter.Quinellas), Convert.ToBoolean(filter.BasedOnArrival));
            if (lstProductionBySr.Any())
            {
              if (Convert.ToBoolean(filter.BasedOnArrival))
                filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
              if (Convert.ToBoolean(filter.Quinellas))
                filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

              finfo = await clsReports.ExportRptProductionBySalesRoom(strReportName, fileFullPath, filters,
                lstProductionBySr);
            }
            break;

          #endregion
          #region Production by Sales Room & Market

          case "Production by Sales Room & Market":
            var lstProductionBySrm = await BRReportsBySalesRoom.GetRptProductionBySalesRoomMarket(filter.StartDate,
                filter.EndDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms),
                Convert.ToBoolean(filter.Quinellas), Convert.ToBoolean(filter.BasedOnArrival));
            if (lstProductionBySrm.Any())
            {
              if (Convert.ToBoolean(filter.BasedOnArrival))
                filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
              if (Convert.ToBoolean(filter.Quinellas))
                filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

              finfo = await clsReports.ExportRptProductionBySalesRoomMarket(strReportName, fileFullPath, filters,
                lstProductionBySrm);
            }
            break;

          #endregion
          #region Production by Sales Room, Program, Market & Submarket

          case "Production by Sales Room, Program, Market & Submarket":
            var lstProductionBySrmSm = await BRReportsBySalesRoom.GetRptProductionBySalesRoomMarketSubMarket(filter.StartDate,
                filter.EndDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms),
                Convert.ToBoolean(filter.Quinellas), Convert.ToBoolean(filter.BasedOnArrival));
            if (lstProductionBySrmSm.Any())
            {
              if (Convert.ToBoolean(filter.BasedOnArrival))
                filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
              if (Convert.ToBoolean(filter.Quinellas))
                filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

              finfo = await clsReports.ExportRptProductionBySalesRoomMarketSubMarket(strReportName, fileFullPath, filters,
                lstProductionBySrmSm);
            }
            break;

          #endregion
          #region Production by Show Program

          case "Production by Show Program":
            var lstProductionByShowProgram = await BRReportsBySalesRoom.GetRptProductionByShowProgram(filter.StartDate,
                filter.EndDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms),
                Convert.ToBoolean(filter.Quinellas), Convert.ToBoolean(filter.BasedOnArrival));
            if (lstProductionByShowProgram.Any())
            {
              if (Convert.ToBoolean(filter.BasedOnArrival))
                filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
              if (Convert.ToBoolean(filter.Quinellas))
                filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

              finfo = await clsReports.ExportRptProductionByShowProgram(strReportName, fileFullPath, filters,
                lstProductionByShowProgram);
            }
            break;

          #endregion
          #region Production by Show, Program & Program

          case "Production by Show Program & Program":
            var lstProductionByShowProgramPro = await BRReportsBySalesRoom.GetRptProductionByShowProgramProgram(filter.StartDate,
                filter.EndDate,
                (filter.AllSalesRooms)
                  ? "ALL"
                  : string.Join(",", filter.LstSalesRooms),
                Convert.ToBoolean(filter.Quinellas), Convert.ToBoolean(filter.BasedOnArrival));
            if (lstProductionByShowProgramPro.Any())
            {
              if (Convert.ToBoolean(filter.BasedOnArrival))
                filters.Add(new Tuple<string, string>("*Based On Arrivals", ""));
              if (Convert.ToBoolean(filter.Quinellas))
                filters.Add(new Tuple<string, string>("*Considering Quinellas", ""));

              finfo = await clsReports.ExportRptProductionByShowProgramProgram(strReportName, fileFullPath, filters,
                lstProductionByShowProgramPro);
            }
            break;

          #endregion

          #endregion

          #region Reportes Salesmen

          #region Closer Statistics

          case "Closer Statistics":
            var lstCloserStatistics = await BRReportsBySalesRoom.GetRptCloserStatistics(filter.StartDate,
                filter.EndDate, (filter.AllSalesRooms) ? "ALL" : string.Join(",", filter.LstSalesRooms));
            if (lstCloserStatistics.Any())
              finfo = await clsReports.ExportRptCloserStatistics(strReportName, fileFullPath, filters, lstCloserStatistics);
            break;

          #endregion
          #region Liner Statistics

          case "Liner Statistics":
            var lstLinerStatistics = await BRReportsBySalesRoom.GetRptLinerStatistics(filter.StartDate,
                filter.EndDate, (filter.AllSalesRooms) ? "ALL" : string.Join(",", filter.LstSalesRooms));
            if (lstLinerStatistics.Any())
              finfo = await clsReports.ExportRptLinerStatistics(strReportName, fileFullPath, filters, lstLinerStatistics);
            break;

          #endregion
          #region Weekly and Monthly Hostess

          case "Weekly and Monthly Hostess":
            var lstWeeklyMontly = await BRReportsBySalesRoom.GetRptWeeklyMonthlyHostess(filter.StartDate, (filter.AllSalesRooms) ? "ALL" : string.Join(",", filter.LstSalesRooms));
            if (lstWeeklyMontly.Any())
              finfo = clsReports.ExportRptWeeklyMonthlyHostess(strReportName, fileFullPath, filters, lstWeeklyMontly);
            break;

          #endregion

          #endregion

          #region Reportes Taxis

          #region Taxis In

          case "Taxis In":
            var lstTaxisIn = await BRReportsBySalesRoom.GetRptTaxisIn(filter.StartDate,
              filter.EndDate,
              (filter.AllSalesRooms)
                ? "ALL"
                : string.Join(",", filter.LstSalesRooms));
            if (lstTaxisIn.Any())
              finfo = await clsReports.ExportRptTaxiIn(strReportName, fileFullPath, filters, lstTaxisIn);
            break;

          #endregion
          #region Taxis Out

          case "Taxis Out":
            var lstTaxisOut = await BRReportsBySalesRoom.GetRptTaxisOut(filter.StartDate,
              filter.EndDate,
              (filter.AllSalesRooms)
                ? "ALL"
                : string.Join(",", filter.LstSalesRooms));
            if (lstTaxisOut.Any())
              finfo = await clsReports.ExportRptTaxiOut(strReportName, fileFullPath, filters, lstTaxisOut);
            break;

            #endregion

            #endregion
        }
        if (finfo == null)
        {
          finfo = ReportBuilder.CreateNoInfoRptExcel(filters, strReportName, fileFullPath);          
        }
        frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(finfo,Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly));
        frmDocumentViewver.Show();
        _frmReportQueue.SetExist(finfo.FullName, finfo);
        _frmReportQueue.Activate();
      }
      catch (Exception ex)
      {
        
        UIHelper.ShowMessage(ex);
      }
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
      _frmFilter = new frmFilterDateRange { FrmProcGen = this, Owner = this };

      #region Abriendo FrmFilter segun reporte seleccionado.

      switch (strReport)
      {
        case "Personnel Access":
          _frmFilter.ConfigurarFomulario(blnLeadSources: true, blnOneDate: _blnOneDate,
        blnOnlyOneRegister: _blnOnlyOneRegister, blnDateRange: false);
          break;
        default:
          _frmFilter.ConfigurarFomulario(blnLeadSources: true, blnOneDate: _blnOneDate,
        blnOnlyOneRegister: _blnOnlyOneRegister);
          break;
      }

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter.BlnOk)
      {
        ShowLeadSourceReport(strReport, _clsFilter);
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
    private async void ShowLeadSourceReport(string strReportName, clsFilter filter)
    {
      var dateRange = _blnOneDate ? DateHelper.DateRange(filter.StartDate, filter.StartDate) : DateHelper.DateRange(filter.StartDate, filter.EndDate);
      var dateRangeFileNameRep = _blnOneDate ? DateHelper.DateRangeFileName(filter.StartDate, filter.StartDate) : DateHelper.DateRangeFileName(filter.StartDate, filter.EndDate);
      FileInfo finfo = null;
      var filters = new List<Tuple<string, string>>();
      var leadSources = (filter.AllLeadSources) ? "ALL" : string.Join(",", filter.LstLeadSources);
      filters.Add(new Tuple<string, string>("Date Range", dateRange));
      filters.Add(new Tuple<string, string>("Lead Sources",
        filter.AllLeadSources ? "ALL" : string.Join(",", filter.LstLeadSources)));

      string fileFullPath = ReportBuilder.CreateEmptyExcel(strReportName, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, strReportName);
      try
      {
        switch (strReportName)
        {
          #region Burned Deposits Guests

          case "Burned Deposits Guests":
            var lstRptBurnedDepsGuest = await BRReportsByLeadSource.GetRptDepositsBurnedGuests(filter.StartDate,
                filter.EndDate,
                string.Join(",", filter.LstLeadSources));
            if (lstRptBurnedDepsGuest.Any())
              finfo = await clsReports.ExportRptBurnedDepositsGuests(strReportName, fileFullPath, filters,
                lstRptBurnedDepsGuest);
            break;

          #endregion
          #region Deposit Refund

          case "Deposit Refund":
            var lstRptDepRef = await BRReportsByLeadSource.GetRptDepositRefunds(filter.StartDate, filter.EndDate,
                leadSources);
            if (lstRptDepRef.Any())
              finfo = await clsReports.ExportRptDepositRefunds(strReportName, fileFullPath, filters, lstRptDepRef);
            break;

          #endregion
          #region Deposits by PR

          case "Deposits by PR":
            var lstRptDepPr = await BRReportsByLeadSource.GetRptDepositByPR(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstLeadSources));
            if (lstRptDepPr.Any())
              finfo = await clsReports.ExportRptDepositByPr(strReportName, fileFullPath, filters, lstRptDepPr);
            break;

          #endregion
          #region Deposits No Show

          case "Deposits No Show":
            var lstRptDepNoShow = await BRReportsByLeadSource.GetRptDepositsNoShow(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstLeadSources));
            if (lstRptDepNoShow.Any())
              finfo = await clsReports.ExportRptDepositsNoShow(strReportName, fileFullPath, filters, lstRptDepNoShow);
            break;

          #endregion
          #region In & Out by PR

          case "In & Out by PR":
            var lstRptInOutPr = await BRReportsByLeadSource.GetRptInOutByPR(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstLeadSources));
            if (lstRptInOutPr.Any())
              finfo = await clsReports.ExportRptInOutByPr(strReportName, fileFullPath, filters, lstRptInOutPr);
            break;

          #endregion
          #region Memberships

          case "Memberships":
            var lstMemberships = await BRReportsBySalesRoom.GetRptMemberships(filter.StartDate,
              filter.EndDate, leadSources: leadSources);
            if (lstMemberships.Any())
              finfo = await clsReports.ExportRptMemberships(strReportName, fileFullPath, filters, lstMemberships);
            break;

          #endregion
          #region Memberships by Host

          case "Memberships by Host":
            var lstMembershipsHost = await BRReportsBySalesRoom.GetRptMembershipsByHost(filter.StartDate, filter.EndDate,
                leadSources: leadSources);
            if (lstMembershipsHost.Any())
              finfo = await clsReports.ExportRptMembershipsByHost(strReportName, fileFullPath, filters, lstMembershipsHost);
            break;

          #endregion
          #region Paid Deposits

          case "Paid Deposits by PR":
            var lstRptPaidDeposits = await BRReportsBySalesRoom.GetRptPaidDeposits(filter.StartDate,
              filter.EndDate, leadSources: leadSources);
            if (lstRptPaidDeposits.Any())
              finfo = await clsReports.ExportRptPaidDeposits(strReportName, fileFullPath, filters, lstRptPaidDeposits, true);
            break;

          #endregion
          #region Personnel Access

          case "Personnel Access":
            var lstPersonnelAccess = await BRReportsByLeadSource.GetRptPersonnelAccess(string.Join(",",
                filter.LstLeadSources));
            if (lstPersonnelAccess.Any())
              finfo = await clsReports.ExportRptPersonnelAccess(strReportName, fileFullPath, filters.Where(c=>c.Item1!= "Date Range").ToList(), lstPersonnelAccess);
            break;

          #endregion
          #region Self Gen

          case "Self Gen":
            var lstRptSelfGen = await BRReportsByLeadSource.GetRptSelfGen(filter.StartDate,
              filter.EndDate,
              string.Join(",", filter.LstLeadSources));
            if (lstRptSelfGen.Item1.Any())
              finfo = await clsReports.ExportRptSelfGen(strReportName, fileFullPath, filters, lstRptSelfGen,
                filter.StartDate, filter.EndDate);
            break;

            #endregion
        }

        if (finfo == null)
        {
          finfo = ReportBuilder.CreateNoInfoRptExcel(filters, strReportName, fileFullPath);          
        }
        frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(finfo,Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly));
        frmDocumentViewver.Show();
        _frmReportQueue.SetExist(finfo.FullName, finfo);
        _frmReportQueue.Activate();
      }
      catch (Exception ex)
      {        
        UIHelper.ShowMessage(ex);
      }
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
      _frmFilter = new frmFilterDateRange { FrmProcGen = this, Owner = this };
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
            enumBasedOnArrival: EnumBasedOnArrival.BasedOnArrival, enumQuinellas: EnumQuinellas.Quinellas,
            enumExternalInvitation: EnumExternalInvitation.Include);
          break;
        default:
          _frmFilter = null;
          WaitMessage(true, (strReport != "Logins Log") ? "Loading Report..." : "Loading Date Range Window...");
          ShowGeneralReport(strReport, _clsFilter);
          WaitMessage(false);
          break;
      }
      if (_frmFilter == null) return;

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter.BlnOk)
      {
        ShowGeneralReport(strReport, _clsFilter, true);
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
    private async void ShowGeneralReport(string strReportName, clsFilter filter, bool blndateRange = false)
    {
      var dateRange = (blndateRange)
        ? ((_blnOneDate)
          ? DateHelper.DateRange(_clsFilter.StartDate, _clsFilter.StartDate)
          : DateHelper.DateRange(_clsFilter.StartDate, _clsFilter.EndDate))
        : "";
      var dateRangeFileNameRep = (blndateRange) ? (_blnOneDate ? DateHelper.DateRangeFileName(filter.StartDate, filter.StartDate) : DateHelper.DateRangeFileName(filter.StartDate, filter.EndDate)) : "";
      FileInfo finfo = null;
      var filters = new List<Tuple<string, string>>();
      string fileFullPath = (strReportName != "Logins Log") ? ReportBuilder.CreateEmptyExcel(strReportName, dateRangeFileNameRep) : "";
      if (strReportName != "Logins Log") { _frmReportQueue.AddReport(fileFullPath, strReportName); }
      try
      {
        switch (strReportName)
        {
          #region Agencies

          case "Agencies":
            var lstRptAgencies = await BRGeneralReports.GetRptAgencies();
            if (lstRptAgencies.Any())
              finfo = await clsReports.ExportRptAgencies(strReportName, fileFullPath, filters, lstRptAgencies);
            break;

          #endregion
          #region Gifts

          case "Gifts":
            var lstRptGift = await BRGeneralReports.GetRptGifts();
            if (lstRptGift.Any())
              finfo = await clsReports.ExportRptGifts(strReportName, fileFullPath, filters, lstRptGift);
            break;

          #endregion
          #region Gifts Kardex

          case "Gifts Kardex":
            var lstRptGiftKardex = await BRGeneralReports.GetRptGiftsKardex(filter.StartDate, filter.EndDate, filter.LstWarehouses.FirstOrDefault());
            if (lstRptGiftKardex.Any())
              finfo = await clsReports.ExportRptGiftsKardex(strReportName, fileFullPath, filters, lstRptGiftKardex);
            break;

          #endregion
          #region Logins Log

          case "Logins Log":
            var loginLog = new frmLoginLog() { Owner = this, frmReportQ = _frmReportQueue };
            loginLog.ShowDialog();
            return;

          #endregion
          #region Personnel

          case "Personnel":
            var lstRptPersonnel = await BRGeneralReports.GetRptPersonnel();
            if (lstRptPersonnel.Any())
              finfo = await clsReports.ExportRptPersonnel(strReportName, fileFullPath, filters, lstRptPersonnel);
            break;

          #endregion
          #region Production by Lead Source & Market (Monthly)

          case "Production by Lead Source & Market (Monthly)":
            var lstRptProductionByLsMarketMonthly = await BRGeneralReports.GetRptProductionByLeadSourceMarketMonthly(filter.StartDate,
                filter.EndDate,
                (Convert.ToBoolean(filter.Quinellas)) ? EnumQuinellas.Quinellas : EnumQuinellas.NoQuinellas,
                (EnumExternalInvitation)_frmFilter.cboExternal.SelectedValue,
                (Convert.ToBoolean(filter.BasedOnArrival))
                  ? EnumBasedOnArrival.BasedOnArrival
                  : EnumBasedOnArrival.NoBasedOnArrival);
            if (lstRptProductionByLsMarketMonthly.Any())
            {
              if (Convert.ToBoolean(filter.BasedOnArrival))
                filters.Add(Tuple.Create("*Based On Arrivals", ""));
              if (Convert.ToBoolean(filter.Quinellas))
                filters.Add(Tuple.Create("*Considering Quinellas", ""));
              filters.Add(Tuple.Create("External Invitation", (EnumToListHelper.GetEnumDescription(filter.ExternalInvitation))));

              finfo = await clsReports.ExportRptProductionByLeadSourceMarketMonthly(strReportName, fileFullPath, filters,
                lstRptProductionByLsMarketMonthly);
            }
            break;

          #endregion
          #region Production Referral

          case "Production Referral":
            var lstRptProductionReferral = await BRGeneralReports.GetRptProductionReferral(filter.StartDate, filter.EndDate);
            if (lstRptProductionReferral.Any())
            {
              filters.Add(Tuple.Create("Date Range", dateRange));
              finfo = await clsReports.ExportRptProductionReferral(strReportName, fileFullPath, filters,
                lstRptProductionReferral);
            }
            break;

          #endregion
          #region Reps

          case "Reps":
            var lstRptReps = await BRGeneralReports.GetRptReps();
            if (lstRptReps.Any())
              finfo = await clsReports.ExportRptReps(strReportName, fileFullPath, filters, lstRptReps);
            break;

          #endregion
          #region Sales By Program, Lead Source & Market

          case "Sales By Program, Lead Source & Market":
            var lstRptSalesByProgramLeadSourceMarkets = await BRGeneralReports.GetRptSalesByProgramLeadSourceMarket(filter.StartDate,
                filter.EndDate);
            if (lstRptSalesByProgramLeadSourceMarkets.Any())
            {
              filters.Add(Tuple.Create("Date Range", dateRange));
              finfo = await clsReports.ExportRptSalesByProgramLeadSourceMarket(strReportName, fileFullPath, filters, lstRptSalesByProgramLeadSourceMarkets);
            }
            break;

          #endregion
          #region Warehouse Movements

          case "Warehouse Movements":
            var lstRptWarehouseMovements = await BRGeneralReports.GetRptWarehouseMovements(filter.StartDate, filter.EndDate, filter.LstWarehouses.FirstOrDefault());
            if (lstRptWarehouseMovements.Any())             
            {
              filters.Add(Tuple.Create("Date Range", dateRange));
              filters.Add(Tuple.Create("Warehouse", (filter.AllWarehouses ? "ALL" : string.Join(",", filter.LstWarehouses))));
              finfo = await clsReports.ExportRptWarehouseMovements(strReportName, fileFullPath, filters,
                lstRptWarehouseMovements);
            }
            break;

            #endregion
        }

        if (finfo == null)
        {
          finfo = ReportBuilder.CreateNoInfoRptExcel(filters, strReportName, fileFullPath);          
        }
        frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(finfo,Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly));
        frmDocumentViewver.Show();
        _frmReportQueue.SetExist(finfo.FullName, finfo);
        _frmReportQueue.Activate();
      }
      catch (Exception ex)
      {        
        UIHelper.ShowMessage(ex);
      }
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
        #region Grid SalesRooms

        var lstrptSalesRooms = new ListCollectionView(new List<dynamic>
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
          new {rptNombre = "Gifts Used by Sistur", rptGroup = "Gifts"},

          new {rptNombre = "Manifest", rptGroup = "Guests"},
          new {rptNombre = "Manifest by Lead Source", rptGroup = "Guests"},
          new {rptNombre = "No Shows", rptGroup = "Guests"},
          new {rptNombre = "Guests No Buyers", rptGroup = "Guests"},
          new {rptNombre = "In & Out", rptGroup = "Guests"},
          //new {rptNombre = "Accounting Codes", rptGroup = "Guests"},

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

          new {rptNombre = "Taxis In", rptGroup = "Taxis"},
          new {rptNombre = "Taxis Out", rptGroup = "Taxis"}
        }.OrderBy(c => c.rptGroup).ThenBy(c => c.rptNombre).ToList());

        lstrptSalesRooms.GroupDescriptions?.Add(new PropertyGroupDescription("rptGroup"));
        grdrptSalesRooms.ItemsSource = lstrptSalesRooms;

        #endregion

        #region Grid LeadSources

        var lstrptLeadSources = new List<dynamic>
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

        StatusBarReg.Content = $"{lstrptGeneral.Count + lstrptLeadSources.Count + lstrptSalesRooms.Count} Reports";      
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

    #region GetFirstDayValue

    /// <summary>
    ///   Obtiene las fechas iniciales y finales de los reportes
    /// </summary>
    /// <history>
    ///   [edgrodriguez] 09/Jun/2016 Created
    /// </history>
    private void GetFirstDayValue()
    {
      DateTime serverDate = BRHelpers.GetServerDate();
      // Fecha inicial
      _clsFilter.StartDate = new DateTime(serverDate.Year, serverDate.Month, 1);

      // obtenemos la fecha de inicio de la semana
      //_clsFilter.DtmInit = DateHelper.GetStartWeek(_serverDate.AddDays(-7)).Date;

      //Fecha final
      _clsFilter.EndDate = serverDate;
      string strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(strArchivo)) return;
      var iniFileHelper = new IniFileHelper(strArchivo);
      _clsFilter.StartDate = iniFileHelper.readDate("FilterDate", "DateStart", _clsFilter.StartDate);
      _clsFilter.EndDate = iniFileHelper.readDate("FilterDate", "DateEnd", _clsFilter.EndDate);
      //string strSalesRoom = _iniFileHelper.readText("FilterDate", "SalesRoom", string.Empty);
      //if (!string.IsNullOrEmpty(strSalesRoom)) _clsFilter.LstLeadSources.Add(strSalesRoom);
    }

    #endregion GetFirstDayValue

    #region SetupParameters

    /// <summary>
    ///   Configura los parametros de los reportes
    /// </summary>
    /// <history>
    ///   [edgrodriguez] 09/Jun/2016 Created
    /// </history>
    private void SetupParameters()
    {
      _clsFilter = new clsFilter();
      // obtenemos las fechas iniciales de los reportes
      GetFirstDayValue();

      _clsFilter.BasedOnArrival = EnumBasedOnArrival.NoBasedOnArrival; //Basado en llegada
      _clsFilter.Quinellas = EnumQuinellas.NoQuinellas; //No considerar quinielas
      _clsFilter.SaveCourtesyTours = EnumSaveCourtesyTours.IncludeSaveCourtesyTours; //Incluir tours de rescate y cortesia
      _clsFilter.SalesByMemberShipType = EnumSalesByMemberShipType.Detail; //Detallar ventas por tipo de membresia
      _clsFilter.ExternalInvitation = EnumExternalInvitation.Include; //Incluir invitaciones externas
    }

    #endregion SetupParameters

    #endregion
  }
}