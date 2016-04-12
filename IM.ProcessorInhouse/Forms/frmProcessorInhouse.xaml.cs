using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.ProcessorInhouse.Classes;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.ProcessorInhouse.Forms
{
  /// <summary>
  ///   Interaction logic for frmProcessorInhouse.xaml
  /// </summary>
  public partial class frmProcessorInhouse : Window
  {
    #region Constructor

    public frmProcessorInhouse()
    {
      InitializeComponent();
    }

    #endregion Constructor

    #region Atributos

    private frmFilterDateRange _frmFilter;
    private bool _blnOneDate;
    private bool _blnOnlyOneRegister;

    public List<string> _lstPersonnel = new List<string>();
    public List<string> _lstLeadSources = new List<string>();
    public List<string> _lstMarkets = new List<string>();
    public List<string> _lstAgencies = new List<string>();

    public List<int> _lstGifts = new List<int>();
    public List<int> _lstGiftsCate = new List<int>();
    public List<int> _lstSalesRoom = new List<int>();
    public List<int> _lstPrograms = new List<int>();
    public List<int> _lstRateTypes = new List<int>();
    public string _cboDateSelected;
    public DateTime _dtmStart;
    public DateTime _dtmEnd;
    public EnumBasedOnArrival _enumBasedOnArrival;

    //public EnumBasedOnBooking _enumBasedOnBooking = EnumBasedOnBooking.bobNoBasedOnBooking;
    public EnumQuinellas _enumQuinellas;

    public EnumDetailGifts _enumDetailsGift; // = EnumDetailGifts.dgNoDetailGifts;
    public EnumSalesByMemberShipType _enumSalesByMemberShipType;
    public EnumStatus _enumStatus = EnumStatus.staActives;

    //public EnumGiftsReceiptType _enumGiftsReceiptType = EnumGiftsReceiptType.grtAll;
    public string _guestID = "";

    //public EnumGiftSale _enumGiftSale = EnumGiftSale.gsAll;
    public EnumSaveCourtesyTours _enumSaveCourtesyTours;

    public EnumExternalInvitation _enumExternalInvitation;

    private EnumProgram _enumPrograms;
    private EnumRptLeadSource _rptLeadSource;
    private EnumRptGeneral _rptGeneral;
    private EnumRptPR _rptPR;

    #endregion Atributos

    #region Eventos Formulario

    #region Window_ContentRendered

    /// <summary>
    ///   Método de inicio del formulario.
    /// </summary>
    /// <history>
    ///   [aalcocer] 11/03/2016 Created
    /// </history>
    private void Window_ContentRendered(object sender, EventArgs e)
    {
      ConfigurarGrids();
      SetupParameters();
      lblUserName.Content = App.userData.User.peN;
    }

    #endregion Window_ContentRendered

    #region Window_Closing

    /// <summary>
    ///   Cambia la propiedad CloseAllowed de la ventana frmFilterDateRange
    ///   para permitir el cierre de la misma.
    /// </summary>
    /// <history>
    ///   [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      _frmFilter?.Close();
      Application.Current.Shutdown();
    }

    #endregion Window_Closing

    #region Window_KeyDown

    /// <summary>
    ///   Verifica si los botones estan activos
    /// </summary>
    /// <history>
    ///   [aalcocer] 16/Mar/2016 Created
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

    #endregion Window_KeyDown

    #region Window_IsKeyboardFocusedChanged

    /// <summary>
    ///   Verifica si los botones estan activos
    /// </summary>
    /// <history>
    ///   [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    #endregion Window_IsKeyboardFocusedChanged

    #region btnExit_Click

    /// <summary>
    ///   Cierra la aplicacion Processor Inhouse.
    /// </summary>
    /// <history>
    ///   [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    #endregion btnExit_Click

    #region grdrpt_MouseDoubleClick

    /// <summary>
    ///   Método para abrir la ventana de filtros  al hacer doble clic sobre alguno registro de un Grid
    /// </summary>
    /// <history>
    ///   [aalcocer] 22/Mar/2016 Created
    /// </history>
    private void grdrpt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var _dataGridRow = (DataGridRow)sender;
      if (_dataGridRow.Item.Equals(grdrptLeadSources.CurrentItem))
        PrepareReportByLeadSource();
      else if (_dataGridRow.Item.Equals(grdrptPR.CurrentItem))
        PrepareReportByPR();
      else if (_dataGridRow.Item.Equals(grdrptGeneral.CurrentItem))
        PrepareReportGeneral();
    }

    #endregion grdrpt_MouseDoubleClick

    #region grdrp_PreviewKeyDown

    /// <summary>
    ///   Método para abrir la ventana de filtros  al hacer Enter sobre alguno registro de un Grid
    /// </summary>
    /// <history>
    ///   [aalcocer] 22/Mar/2016 Created
    /// </history>
    private void grdrp_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter) return;
      var _dataGridRow = (DataGridRow)sender;
      if (_dataGridRow.Item.Equals(grdrptLeadSources.CurrentItem))
        PrepareReportByLeadSource();
      else if (_dataGridRow.Item.Equals(grdrptPR.CurrentItem))
        PrepareReportByPR();
      else if (_dataGridRow.Item.Equals(grdrptGeneral.CurrentItem))
        PrepareReportGeneral();
    }

    #endregion grdrp_PreviewKeyDown

    #region btnPrint_Click

    /// <summary>
    ///   Método para abrir la ventana de filtros al apretar el boton
    /// </summary>
    /// <history>
    ///   [aalcocer] 22/Mar/2016 Created
    /// </history>
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      if (sender.Equals(btnPrintLS))
        PrepareReportByLeadSource();
      else if (sender.Equals(btnPrintPR))
        PrepareReportByPR();
      else if (sender.Equals(btnPrintGeneral))
        PrepareReportGeneral();
    }

    #endregion btnPrint_Click

    #endregion Eventos Formulario

    #region Métodos Privados

    #region ConfigurarGrids

    /// <summary>
    ///   Se configuran los treeview, agregando los reportes.
    /// </summary>
    /// <history>
    ///   [aalcocer] 11/03/2016 Created
    /// </history>
    private void ConfigurarGrids()
    {
      //Reportes por Lead Source
      grdrptLeadSources.ItemsSource = EnumToListHelper.GetList<EnumRptLeadSource>().OrderBy(x => x.Value);
      //Reportes por PR
      grdrptPR.ItemsSource = EnumToListHelper.GetList<EnumRptPR>().OrderBy(x => x.Value);
      //Reportes generales
      grdrptGeneral.ItemsSource = EnumToListHelper.GetList<EnumRptGeneral>().OrderBy(x => x.Value);

      StatusBarReg.Content =
        $"{grdrptLeadSources.Items.Count + grdrptPR.Items.Count + grdrptGeneral.Items.Count} Reports";
    }

    #endregion ConfigurarGrids

    #region SetupParameters

    /// <summary>
    ///   Configura los parametros de los reportes
    /// </summary>
    /// <history>
    ///   [aalcocer] 18/Mar/2016 Created
    /// </history>
    private void SetupParameters()
    {
      // obtenemos las fechas iniciales de los reportes
      GetFirstDayValue();

      _enumPrograms = EnumProgram.Inhouse; //Programa
      _enumBasedOnArrival = EnumBasedOnArrival.boaBasedOnArrival; //Basado en llegada
      _enumQuinellas = EnumQuinellas.quNoQuinellas; //No considerar quinielas
      _enumSaveCourtesyTours = EnumSaveCourtesyTours.sctIncludeSaveCourtesyTours; //Incluir tours de rescate y cortesia
      _enumSalesByMemberShipType = EnumSalesByMemberShipType.sbmDetail; //Detallar ventas por tipo de membresia
      _enumExternalInvitation = EnumExternalInvitation.extInclude; //Incluir invitaciones externas
    }

    #endregion SetupParameters

    #region GetFirstDayValue

    /// <summary>
    ///   Obtiene las fechas iniciales y finales de los reportes
    /// </summary>
    /// <history>
    ///   [aalcocer] 18/Mar/2016 Created
    /// </history>
    private void GetFirstDayValue()
    {
      var _serverDate = BRHelpers.GetServerDate();
      // Fecha inicial
      _dtmStart = new DateTime(_serverDate.Year, _serverDate.Month, 1);

      //Fecha final
      _dtmEnd = _serverDate;
      string strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(strArchivo)) return;
      var _iniFileHelper = new IniFileHelper(strArchivo);
      _dtmStart = _iniFileHelper.readDate("FilterDate", "DateStart", _dtmStart);
      _dtmEnd = _iniFileHelper.readDate("FilterDate", "DateEnd", _dtmEnd);
      string strLeadSource = _iniFileHelper.readText("FilterDate", "LeadSource", string.Empty);
      if (!string.IsNullOrEmpty(strLeadSource))
        _lstLeadSources.Add(strLeadSource);
    }

    #endregion GetFirstDayValue

    #region WaitMessage

    /// <summary>
    ///   Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    ///   [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void WaitMessage(bool show, string message = "")
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = show ? Visibility.Visible : Visibility.Hidden;
      Cursor = show ? Cursors.Wait : null;
      UIHelper.ForceUIToUpdate();
    }

    #endregion WaitMessage

    #region PrepareReportByLeadSource

    /// <summary>
    ///   Prepara un reporte por Lead Source
    /// </summary>
    /// <history>
    ///   [aalcocer] 22/Mar/2016 Created
    /// </history>
    private void PrepareReportByLeadSource()
    {
      //Validamos que haya un reporte seleccionado.
      if (grdrptLeadSources.SelectedIndex < 0)
        return;

      WaitMessage(true, "Loading Date Range Window...");

      //Obtenemos el nombre del reporte.
      _rptLeadSource = ((KeyValuePair<EnumRptLeadSource, string>)grdrptLeadSources.SelectedItem).Key;

      #region Configurando Fecha

      switch (_rptLeadSource)
      {
        //Reportes que utilizan solo una fecha
        case EnumRptLeadSource.ProductionGraphic:
        case EnumRptLeadSource.UnavailableArrivalsGraphic:
        case EnumRptLeadSource.NotBookingArrivalsGraphic:
          _blnOneDate = true;
          break;

        default:
          _blnOneDate = false;
          break;
      }

      #endregion Configurando Fecha

      #region Configurando Grids Modo de Seleccion

      switch (_rptLeadSource)
      {
        //Reportes que permiten seleccionar solo un registro.
        case EnumRptLeadSource.OccupationContactBookShow:
          _blnOnlyOneRegister = true;
          break;

        default:
          _blnOnlyOneRegister = false;
          break;
      }

      #endregion Configurando Grids Modo de Seleccion

      AbrirFilterDateRangeLS();
    }

    #endregion PrepareReportByLeadSource

    #region PrepareReportByPR

    /// <summary>
    ///   Prepara un reporte por PR
    /// </summary>
    private void PrepareReportByPR()
    {
      //Validamos que haya un reporte seleccionado.
      if (grdrptPR.SelectedIndex < 0)
        return;

      WaitMessage(true, "Loading Date Range Window...");

      //Obtenemos el nombre del reporte.
      _rptPR = ((KeyValuePair<EnumRptPR, string>)grdrptPR.SelectedItem).Key;

      //Reportes que utilizan solo una fecha
      _blnOneDate = false;

      //Reportes que permiten seleccionar solo un registro.
      _blnOnlyOneRegister = false;

      AbrirFilterDateRangePR();
    }

    #endregion PrepareReportByPR

    #region PrepareReportGeneral

    /// <summary>
    ///   Prepara un reporte general
    /// </summary>
    private void PrepareReportGeneral()
    {
      //Validamos que haya un reporte seleccionado.
      if (grdrptGeneral.SelectedIndex < 0)
        return;

      WaitMessage(true, "Loading Date Range Window...");

      //Obtenemos el nombre del reporte.
      _rptGeneral = ((KeyValuePair<EnumRptGeneral, string>)grdrptGeneral.SelectedItem).Key;

      //Reportes que utilizan solo una fecha
      _blnOneDate = false;

      //Reportes que permiten seleccionar solo un registro.
      _blnOnlyOneRegister = false;

      AbrirFilterDateRangeGeneral();
    }

    #endregion PrepareReportGeneral

    #region AbrirFilterDateRangeLS

    /// <summary>
    ///   Abre la ventana frmFilterDateRange configurando
    ///   los controles segun el reporte seleccionado.
    /// </summary>
    /// <history>
    ///   [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void AbrirFilterDateRangeLS()
    {
      _frmFilter = new frmFilterDateRange { frmIH = this };

      #region Abriendo FrmFilter segun reporte seleccionado.

      switch (_rptLeadSource)
      {
        case EnumRptLeadSource.GiftsReceivedbySalesRoom:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumPrograms,
            blnLeadSources: true, blnChargeTo: true, blnGifts: true, blnAllChargeTo: true, blnAllGifts: true);
          break;

        case EnumRptLeadSource.ProductionbyGiftQuantity:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumPrograms,
            blnLeadSources: true, blnGiftsQuantity: true, blnAllGiftsQuantity: true,
            enumBasedOnArrival: _enumBasedOnArrival,
            enumQuinellas: _enumQuinellas);
          break;

        case EnumRptLeadSource.OccupationContactBookShow:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumPrograms,
            blnLeadSources: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas,
            enumExternalInvitation: _enumExternalInvitation, blnLsHotelNotNull: true);

          break;

        case EnumRptLeadSource.ProductionbyAgencyNights:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: EnumProgram.Inhouse,
            blnLeadSources: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas,
            enumSalesByMemberShipType: _enumSalesByMemberShipType, enumExternalInvitation: _enumExternalInvitation,
            blnNight: true);
          break;

        case EnumRptLeadSource.ProductionbyNationality:
        case EnumRptLeadSource.ProductionbyNationalityMarketOriginallyAvailable:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: EnumProgram.Inhouse,
            blnLeadSources: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas,
            enumDetailGifts: _enumDetailsGift);
          break;

        case EnumRptLeadSource.ProductionbyAgency:
        case EnumRptLeadSource.ProductionbyAgencyOnlyQuinellas:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: EnumProgram.Inhouse,
            blnLeadSources: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas,
            enumSalesByMemberShipType: _enumSalesByMemberShipType,
            enumExternalInvitation: _enumExternalInvitation);
          break;

        case EnumRptLeadSource.CostbyPR:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: EnumProgram.Inhouse,
            blnLeadSources: true, enumQuinellas: _enumQuinellas, enumDetailGifts: _enumDetailsGift);
          break;

        case EnumRptLeadSource.ScorebyPR:
        case EnumRptLeadSource.ShowFactorbyBookingDate:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: EnumProgram.Inhouse,
            blnLeadSources: true, enumQuinellas: _enumQuinellas);
          break;

        case EnumRptLeadSource.FollowUpbyAgency:
        case EnumRptLeadSource.FollowUpbyPR:
        case EnumRptLeadSource.ProductionGraphic:
        case EnumRptLeadSource.ProductionbyAge:
        case EnumRptLeadSource.ProductionbyAgeMarketOriginallyAvailable:
        case EnumRptLeadSource.ProductionbyDesk:
        case EnumRptLeadSource.ProductionbyCoupleType:
        case EnumRptLeadSource.ProductionbyGroup:
        case EnumRptLeadSource.ProductionbyGuestStatus:
        case EnumRptLeadSource.ProductionbyPR:
        case EnumRptLeadSource.ProductionbyPRSalesRoom:
        case EnumRptLeadSource.ProductionbyPRGroup:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: EnumProgram.Inhouse, blnLeadSources: true,
            enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas);
          break;

        case EnumRptLeadSource.ProductionbyMemberTypeAgency:
        case EnumRptLeadSource.ProductionbyMemberTypeAgencyMarketOriginallyAvailable:
        case EnumRptLeadSource.ProductionbyContractAgency:
        case EnumRptLeadSource.ProductionbyContractAgencyMarketOriginallyAvailable:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: EnumProgram.Inhouse, blnLeadSources: true,
            blnMarkets: true, blnAllMarkets: true, blnAgencies: true, blnAllAgencies: true,
            enumQuinellas: _enumQuinellas);
          break;

        case EnumRptLeadSource.UnavailableMotivesByAgency:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: EnumProgram.Inhouse, blnLeadSources: true,
            blnMarkets: true, blnAllMarkets: true, blnAgencies: true, blnAllAgencies: true);
          break;

        default:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: EnumProgram.Inhouse, blnLeadSources: true);
          break;
      }
      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (!_frmFilter._blnOK) return;
      ShowLeadSourceReport();
      _frmFilter.Close();

      #endregion Abriendo FrmFilter segun reporte seleccionado.
    }

    #endregion AbrirFilterDateRangeLS

    #region AbrirFilterDateRangePR

    /// <summary>
    ///   Abre la ventana frmFilterDateRange configurando los controles segun el reporte seleccionado.
    /// </summary>
    /// <history>
    ///   [aalcocer] 22/Mar/2016 Created
    /// </history>
    private void AbrirFilterDateRangePR()
    {
      _frmFilter = new frmFilterDateRange { frmIH = this };

      #region Abriendo FrmFilter segun reporte seleccionado.

      _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumPrograms,
        blnPersonnel: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas);

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowPRReport();
        _frmFilter.Close();
      }

      #endregion Abriendo FrmFilter segun reporte seleccionado.
    }

    #endregion AbrirFilterDateRangePR

    #region AbrirFilterDateRangePR

    /// <summary>
    ///   Abre la ventana frmFilterDateRange configurando los controles segun el reporte seleccionado.
    /// </summary>
    /// <history>
    ///   [aalcocer] 22/Mar/2016 Created
    /// </history>
    private void AbrirFilterDateRangeGeneral()
    {
      _frmFilter = new frmFilterDateRange { frmIH = this };

      #region Abriendo FrmFilter segun reporte seleccionado.

      switch (_rptGeneral)
      {
        case EnumRptGeneral.ProductionbyAgencyMonthly:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumPrograms,
            enumPeriod: EnumPeriod.pdMonthly, blnAgencies: true, blnAllAgencies: true,
            enumBasedOnArrival: _enumBasedOnArrival,
            enumQuinellas: _enumQuinellas);
          break;

        case EnumRptGeneral.ProductionbyMember:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumPrograms,
            blnClub: true);
          break;
      }
      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (!_frmFilter._blnOK) return;
      ShowGeneralReport();
      _frmFilter.Close();

      #endregion Abriendo FrmFilter segun reporte seleccionado.
    }

    #endregion AbrirFilterDateRangePR

    #region ShowLeadSourceReport

    /// <summary>
    ///   Muestra el reporte seleccionado
    /// </summary>
    /// <history>
    ///   [aalcocer] 23/Mar/2016 Created
    /// </history>
    private void ShowLeadSourceReport()
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmStart.Value.Value) : DateHelper.DateRange(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmStart.Value.Value) : DateHelper.DateRangeFileName(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value);

      string reportname = EnumToListHelper.GetEnumDescription(_rptLeadSource);
      var filters = new List<Tuple<string, string>>();
      WaitMessage(true, "Loading report...");

      switch (_rptLeadSource)
      {
        #region Cost by PR

        case EnumRptLeadSource.CostbyPR:
          if (_enumDetailsGift == EnumDetailGifts.dgNoDetailGifts)
          {
            List<RptCostByPR> listRptCostByPrs = BRReportsByLeadSource.GetRptCostByPR(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas);
            if (!listRptCostByPrs.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
            else
            {
              filters.Add(new Tuple<string, string>("Date Range", dateRange));
              filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
              if (_enumQuinellas == EnumQuinellas.quQuinellas)
                filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));

              finfo = clsReports.ExportRptCostByPR(reportname, dateRangeFileNameRep, filters, listRptCostByPrs);
            }
          }
          else if (_enumDetailsGift == EnumDetailGifts.dgDetailGifts)
          {
            var listRptCostByPRWithDetailGifts = BRReportsByLeadSource.GetRptCostByPRWithDetailGifts(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas);
            if (!listRptCostByPRWithDetailGifts.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
            else
            {
              reportname += " With Details Gifts";
              filters.Add(new Tuple<string, string>("Date Range", dateRange));
              filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
              if (_enumQuinellas == EnumQuinellas.quQuinellas)
                filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));

              finfo = clsReports.ExportRptCostByPRWithDetailGifts(reportname, dateRangeFileNameRep, filters, listRptCostByPRWithDetailGifts);
            }
          }
          break;

        #endregion Cost by PR

        #region Follow Up by Agency

        case EnumRptLeadSource.FollowUpbyAgency:
          List<RptFollowUpByAgency> listRptFollowUpByAgencies = BRReportsByLeadSource.GetRptFollowUpByAgencies(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival);
          if (!listRptFollowUpByAgencies.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (_enumQuinellas == EnumQuinellas.quQuinellas)
              filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (_enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival)
              filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptFollowUpByAgencies(reportname, dateRangeFileNameRep, filters, listRptFollowUpByAgencies);
          }
          break;

        #endregion Follow Up by Agency

        #region Follow Up by PR

        case EnumRptLeadSource.FollowUpbyPR:
          List<RptFollowUpByPR> listRptFollowUpByPR = BRReportsByLeadSource.GeRptFollowUpByPRs(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival);
          if (!listRptFollowUpByPR.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (_enumQuinellas == EnumQuinellas.quQuinellas)
              filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (_enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival)
              filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptFollowUpByPRs(reportname, dateRangeFileNameRep, filters, listRptFollowUpByPR);
          }
          break;

        #endregion Follow Up by PR

        #region Production by Age

        case EnumRptLeadSource.ProductionbyAge:
          List<RptProductionByAgeInhouse> listRptProductionByAgeInhouses = BRReportsByLeadSource.GetProductionByAgeInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival);
          if (!listRptProductionByAgeInhouses.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (_enumQuinellas == EnumQuinellas.quQuinellas)
              filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (_enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival)
              filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportProductionByAgeInhouses(reportname, dateRangeFileNameRep, filters, listRptProductionByAgeInhouses);
          }
          break;

        #endregion Production by Age

        #region Production by Age, Market & Originally Available

        case EnumRptLeadSource.ProductionbyAgeMarketOriginallyAvailable:
          List<RptProductionByAgeMarketOriginallyAvailableInhouse> listProductionByAgeMarketOriginallyAvailableInhouses = BRReportsByLeadSource.GetProductionByAgeMarketOriginallyAvailableInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival);
          if (!listProductionByAgeMarketOriginallyAvailableInhouses.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (_enumQuinellas == EnumQuinellas.quQuinellas)
              filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (_enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival)
              filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportProductionByAgeMarketOriginallyAvailableInhouses(reportname, dateRangeFileNameRep, filters, listProductionByAgeMarketOriginallyAvailableInhouses);
          }
          break;

        #endregion Production by Age, Market & Originally Available

        #region Production by Agency

        case EnumRptLeadSource.ProductionbyAgency:

          break;

        #endregion Production by Agency

        #region Production by Contract & Agency

        case EnumRptLeadSource.ProductionbyContractAgency:
          List<RptProductionByContractAgencyInhouse> listRptProductionByContractAgencyInhouses = BRReportsByLeadSource.GetRptProductionByContractAgencyInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _lstMarkets, _lstAgencies, _enumQuinellas);
          if (!listRptProductionByContractAgencyInhouses.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdMarkets.Columns[0].Header.ToString(), _frmFilter.grdMarkets.Items.Count == _lstMarkets.Count ? "ALL" : string.Join(",", _lstMarkets)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdAgencies.Columns[0].Header.ToString(), _frmFilter.grdAgencies.Items.Count == _lstAgencies.Count ? "ALL" : string.Join(",", _lstAgencies)));
            if (_enumQuinellas == EnumQuinellas.quQuinellas)
              filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByContractAgencyInhouses(reportname, dateRangeFileNameRep, filters, listRptProductionByContractAgencyInhouses);
          }
          break;

        #endregion Production by Contract & Agency

        case EnumRptLeadSource.ProductionbyContractAgencyMarketOriginallyAvailable:
          List<RptProductionByContractAgencyMarketOriginallyAvailableInhouse> listRptProductionByContractAgencyMarketOriginallyAvailableInhouses =
            BRReportsByLeadSource.GetRptProductionByContractAgencyMarketOriginallyAvailableInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _lstMarkets, _lstAgencies, _enumQuinellas);
          if (!listRptProductionByContractAgencyMarketOriginallyAvailableInhouses.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdMarkets.Columns[0].Header.ToString(), _frmFilter.grdMarkets.Items.Count == _lstMarkets.Count ? "ALL" : string.Join(",", _lstMarkets)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdAgencies.Columns[0].Header.ToString(), _frmFilter.grdAgencies.Items.Count == _lstAgencies.Count ? "ALL" : string.Join(",", _lstAgencies)));
            if (_enumQuinellas == EnumQuinellas.quQuinellas)
              filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            finfo = clsReports.ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses(reportname, dateRangeFileNameRep, filters, listRptProductionByContractAgencyMarketOriginallyAvailableInhouses);
          }
          break;
      }

      if (finfo != null)
      {
        Process.Start(finfo.FullName);
      }

      WaitMessage(false);
    }

    #endregion ShowLeadSourceReport

    #region ShowPRReport

    /// <summary>
    ///   Muestra el reporte seleccionado
    /// </summary>
    private void ShowPRReport()
    {
    }

    #endregion ShowPRReport

    #region ShowGeneralReport

    /// <summary>
    ///   Muestra el reporte seleccionado
    /// </summary>
    private void ShowGeneralReport()
    {
    }

    #endregion ShowGeneralReport

    #endregion Métodos Privados
  }
}