using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.ProcessorInhouse.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    public List<string> _lstGifts = new List<string>();
    public List<string> _lstCharteTo = new List<string>();
    public Dictionary<string, int> _lstGiftsQuantity = new Dictionary<string, int>();

    public EnumPredefinedDate _cboDateSelected;
    public DateTime _dtmInit;
    public DateTime _dtmStart;
    public DateTime _dtmEnd;
    public EnumBasedOnArrival _enumBasedOnArrival;
    public EnumQuinellas _enumQuinellas;

    public EnumDetailGifts _enumDetailsGift;
    public EnumSalesByMemberShipType _enumSalesByMemberShipType;

    public string _strApplication;
    public int _iCompany;
    public Club _club;
    public bool _blnOnlyWholesalers;

    public EnumSaveCourtesyTours _enumSaveCourtesyTours;

    public EnumExternalInvitation _enumExternalInvitation;

    private EnumProgram _enumProgram;
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
      lblUserName.Content = App.User.User.peN;
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
      if (_dataGridRow.Item.Equals(grdrptLeadSources.CurrentItem)) PrepareReportByLeadSource();
      else if (_dataGridRow.Item.Equals(grdrptPR.CurrentItem)) PrepareReportByPR();
      else if (_dataGridRow.Item.Equals(grdrptGeneral.CurrentItem)) PrepareReportGeneral();
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
      if (_dataGridRow.Item.Equals(grdrptLeadSources.CurrentItem)) PrepareReportByLeadSource();
      else if (_dataGridRow.Item.Equals(grdrptPR.CurrentItem)) PrepareReportByPR();
      else if (_dataGridRow.Item.Equals(grdrptGeneral.CurrentItem)) PrepareReportGeneral();
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
      if (sender.Equals(btnPrintLS)) PrepareReportByLeadSource();
      else if (sender.Equals(btnPrintPR)) PrepareReportByPR();
      else if (sender.Equals(btnPrintGeneral)) PrepareReportGeneral();
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

      _enumProgram = EnumProgram.Inhouse; //Programa
      _enumBasedOnArrival = EnumBasedOnArrival.BasedOnArrival; //Basado en llegada
      _enumQuinellas = EnumQuinellas.NoQuinellas; //No considerar quinielas
      _enumSaveCourtesyTours = EnumSaveCourtesyTours.IncludeSaveCourtesyTours; //Incluir tours de rescate y cortesia
      _enumSalesByMemberShipType = EnumSalesByMemberShipType.Detail; //Detallar ventas por tipo de membresia
      _enumExternalInvitation = EnumExternalInvitation.Include; //Incluir invitaciones externas
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
      DateTime _serverDate = BRHelpers.GetServerDate();
      // Fecha inicial
      _dtmStart = new DateTime(_serverDate.Year, _serverDate.Month, 1);

      // obtenemos la fecha de inicio de la semana
      _dtmInit = DateHelper.GetStartWeek(_serverDate.AddDays(-7)).Date;

      //Fecha final
      _dtmEnd = _serverDate.Date;
      string strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(strArchivo)) return;
      var _iniFileHelper = new IniFileHelper(strArchivo);
      _dtmStart = _iniFileHelper.readDate("FilterDate", "DateStart", _dtmStart);
      _dtmEnd = _iniFileHelper.readDate("FilterDate", "DateEnd", _dtmEnd);
      string strLeadSource = _iniFileHelper.readText("FilterDate", "LeadSource", string.Empty);
      if (!string.IsNullOrEmpty(strLeadSource)) _lstLeadSources.Add(strLeadSource);
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
      if (grdrptLeadSources.SelectedIndex < 0) return;

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

      //Reportes que permiten seleccionar solo un registro.
      _blnOnlyOneRegister = false;

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
      if (grdrptPR.SelectedIndex < 0) return;

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
      if (grdrptGeneral.SelectedIndex < 0) return;

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
      _frmFilter = new frmFilterDateRange { _frmIh = this };

      #region Abriendo FrmFilter segun reporte seleccionado.

      switch (_rptLeadSource)
      {
        case EnumRptLeadSource.GiftsReceivedbySalesRoom:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            blnLeadSources: true, blnChargeTo: true, blnGifts: true, blnAllChargeTo: true, blnAllGifts: true);
          break;

        case EnumRptLeadSource.ProductionbyGiftQuantity:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            blnLeadSources: true, blnGiftsQuantity: true, blnAllGiftsQuantity: true,
            enumBasedOnArrival: _enumBasedOnArrival,
            enumQuinellas: _enumQuinellas);
          break;

        case EnumRptLeadSource.OccupationContactBookShow:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            blnLeadSources: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas,
            enumExternalInvitation: _enumExternalInvitation, blnLsHotelNotNull: true);

          break;

        case EnumRptLeadSource.ProductionbyAgencyNights:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            blnLeadSources: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas,
            enumSalesByMemberShipType: _enumSalesByMemberShipType, enumExternalInvitation: _enumExternalInvitation,
            blnNight: true);
          break;

        case EnumRptLeadSource.ProductionbyNationality:
        case EnumRptLeadSource.ProductionbyNationalityMarketOriginallyAvailable:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            blnLeadSources: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas,
            enumDetailGifts: _enumDetailsGift);
          break;

        case EnumRptLeadSource.ProductionbyAgency:
        case EnumRptLeadSource.ProductionbyAgencyOnlyQuinellas:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: _enumProgram,
            blnLeadSources: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas,
            enumSalesByMemberShipType: _enumSalesByMemberShipType,
            enumExternalInvitation: _enumExternalInvitation);
          break;

        case EnumRptLeadSource.CostbyPR:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: _enumProgram,
            blnLeadSources: true, enumQuinellas: _enumQuinellas, enumDetailGifts: _enumDetailsGift);
          break;

        case EnumRptLeadSource.ScorebyPR:
        case EnumRptLeadSource.ShowFactorbyBookingDate:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: _enumProgram,
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
            program: _enumProgram, blnLeadSources: true,
            enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas);
          break;

        case EnumRptLeadSource.ProductionbyMemberTypeAgency:
        case EnumRptLeadSource.ProductionbyMemberTypeAgencyMarketOriginallyAvailable:
        case EnumRptLeadSource.ProductionbyContractAgency:
        case EnumRptLeadSource.ProductionbyContractAgencyMarketOriginallyAvailable:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: _enumProgram, blnLeadSources: true,
            blnMarkets: true, blnAllMarkets: true, blnAgencies: true, blnAllAgencies: true,
            enumQuinellas: _enumQuinellas);
          break;

        case EnumRptLeadSource.UnavailableMotivesByAgency:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: _enumProgram, blnLeadSources: true,
            blnMarkets: true, blnAllMarkets: true, blnAgencies: true, blnAllAgencies: true);
          break;

        default:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister,
            program: _enumProgram, blnLeadSources: true);
          break;
      }
      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowLeadSourceReport();
        _frmFilter.Close();
      }
      else
      {
        _frmFilter.Close();
        _frmFilter = null;
      }

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
      _frmFilter = new frmFilterDateRange { _frmIh = this };

      #region Abriendo FrmFilter segun reporte seleccionado.

      _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
        blnPersonnel: true, enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas);

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowPRReport();
        _frmFilter.Close();
      }
      else
      {
        _frmFilter.Close();
        _frmFilter = null;
      }

      #endregion Abriendo FrmFilter segun reporte seleccionado.
    }

    #endregion AbrirFilterDateRangePR

    #region AbrirFilterDateRangeGeneral

    /// <summary>
    ///   Abre la ventana frmFilterDateRange configurando los controles segun el reporte seleccionado.
    /// </summary>
    /// <history>
    ///   [aalcocer] 22/Mar/2016 Created
    /// </history>
    private void AbrirFilterDateRangeGeneral()
    {
      _frmFilter = new frmFilterDateRange { _frmIh = this };

      #region Abriendo FrmFilter segun reporte seleccionado.

      switch (_rptGeneral)
      {
        case EnumRptGeneral.ProductionbyAgencyMonthly:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            enumPeriod: EnumPeriod.Monthly, blnAgencies: true, blnAllAgencies: true,
            enumBasedOnArrival: _enumBasedOnArrival,
            enumQuinellas: _enumQuinellas, blnAgencyMonthly: true);
          break;

        case EnumRptGeneral.ProductionbyMember:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
             enumBasedOnArrival: _enumBasedOnArrival, enumQuinellas: _enumQuinellas, blnOnlyWholesalers: true, blnClub: true);
          break;
      }

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowGeneralReport();
        _frmFilter.Close();
      }
      else
      {
        _frmFilter.Close();
        _frmFilter = null;
      }

      #endregion Abriendo FrmFilter segun reporte seleccionado.
    }

    #endregion AbrirFilterDateRangeGeneral

    #region ShowLeadSourceReport

    /// <summary>
    ///   Muestra el reporte seleccionado
    /// </summary>
    /// <history>
    /// [aalcocer] 23/Mar/2016 Created
    /// [aalcocer] 24/05/2016 Modified. Se agregó asincronía
    /// </history>
    private async void ShowLeadSourceReport()
    {
      FileInfo finfo = null;
      string dateRange = _blnOneDate ? DateHelper.DateRange(_dtmInit, _dtmInit.AddDays(6)) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = _blnOneDate ? DateHelper.DateRangeFileName(_dtmInit, _dtmInit) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);

      string reportname = EnumToListHelper.GetEnumDescription(_rptLeadSource);
      List<Tuple<string, string>> filters = new List<Tuple<string, string>> { new Tuple<string, string>("Date Range", dateRange) };
      List<dynamic> list = new List<dynamic>();
      WaitMessage(true, "Loading report...");

      switch (_rptLeadSource)
      {
        #region Cost by PR

        case EnumRptLeadSource.CostbyPR:
          if (!Convert.ToBoolean(_enumDetailsGift))
          {
            list.AddRange(await BRReportsByLeadSource.GetRptCostByPR(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas));
            if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
            else
            {
              filters.Add(new Tuple<string, string>("Lead Sources",
                _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
              if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));

              finfo = clsReports.ExportRptCostByPR(reportname, dateRangeFileNameRep, filters, list.Cast<RptCostByPR>().ToList());
            }
          }
          else
          {
            list.AddRange(await BRReportsByLeadSource.GetRptCostByPRWithDetailGifts(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas));
            if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
            else
            {
              reportname += " With Details Gifts";
              filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
                _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
              if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));

              finfo = clsReports.ExportRptCostByPRWithDetailGifts(reportname, dateRangeFileNameRep, filters, list.Cast<RptCostByPRWithDetailGifts>().ToList());
            }
          }
          break;

        #endregion Cost by PR

        #region Follow Up by Agency

        case EnumRptLeadSource.FollowUpbyAgency:
          list.AddRange(await BRReportsByLeadSource.GetRptFollowUpByAgencies(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptFollowUpByAgencies(reportname, dateRangeFileNameRep, filters, list.Cast<RptFollowUpByAgency>().ToList());
          }
          break;

        #endregion Follow Up by Agency

        #region Follow Up by PR

        case EnumRptLeadSource.FollowUpbyPR:
          list.AddRange(await BRReportsByLeadSource.GeRptFollowUpByPRs(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptFollowUpByPRs(reportname, dateRangeFileNameRep, filters, list.Cast<RptFollowUpByPR>().ToList());
          }
          break;

        #endregion Follow Up by PR

        #region Gifts Received by Sales Room

        case EnumRptLeadSource.GiftsReceivedbySalesRoom:
          list.Add(BRReportsByLeadSource.GetRptGiftsReceivedBySRData(_dtmStart, _dtmEnd, string.Join(",", _lstLeadSources),
            string.Join(",", _lstCharteTo), string.Join(",", _lstGifts)));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdChargeTo.Columns[0].Header.ToString(),
              _frmFilter.grdChargeTo.Items.Count == _lstCharteTo.Count ? "ALL" : string.Join(",", _lstCharteTo)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdGifts.Columns[0].Header.ToString(),
              _frmFilter.grdGifts.Items.Count == _lstGifts.Count ? "ALL" : string.Join(",", _lstGifts)));

            finfo = clsReports.ExportRptGiftsReceivedBySR(reportname, dateRangeFileNameRep, filters, list.First());
          }
          break;

        #endregion Gifts Received by Sales Room

        #region Not Booking Arrivals (Graphic)

        case EnumRptLeadSource.NotBookingArrivalsGraphic:
          list.Add(await BRReportsByLeadSource.GetGraphNotBookingArrival(_dtmInit, _lstLeadSources));
          if (list.First() == null) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));

            finfo = clsReports.ExportGraphNotBookingArrival(reportname, dateRangeFileNameRep, filters, _dtmInit, list.First());
          }
          break;

        #endregion Not Booking Arrivals (Graphic)

        #region Occupation, Contact, Book & Show

        case EnumRptLeadSource.OccupationContactBookShow:
          if (!Convert.ToBoolean(_enumQuinellas))
          {
            list.AddRange(await BRReportsByLeadSource.GetRptProductionByMonths(_dtmStart, _dtmEnd, _lstLeadSources, external: _enumExternalInvitation, basedOnArrival: _enumBasedOnArrival));
            if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
            else
            {
              filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
                _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
              if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));
              if (Convert.ToBoolean(_enumExternalInvitation)) filters.Add(new Tuple<string, string>(EnumToListHelper.GetEnumDescription(_enumExternalInvitation), string.Empty));

              finfo = clsReports.ExportRptProductionByMonths(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByMonth>().ToList());
            }
          }
          else
          {
            list.AddRange(await BRReportsByLeadSource.GetRptContactBookShowQuinellas(_dtmStart, _dtmEnd, _lstLeadSources, _enumExternalInvitation, _enumBasedOnArrival));
            if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
            else
            {
              filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
                _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
              if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
              if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));
              if (Convert.ToBoolean(_enumExternalInvitation)) filters.Add(new Tuple<string, string>(EnumToListHelper.GetEnumDescription(_enumExternalInvitation), string.Empty));

              finfo = clsReports.ExportRptContactBookShowQuinellas(reportname, dateRangeFileNameRep, filters, list.Cast<RptContactBookShowQuinellas>().ToList());
            }
          }
          break;

        #endregion Occupation, Contact, Book & Show

        #region Production (Graphic)

        case EnumRptLeadSource.ProductionGraphic:
          list.Add(await BRReportsByLeadSource.GetGraphProduction(_dtmInit, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (list.First() == null) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportGraphProduction(reportname, dateRangeFileNameRep, filters, _dtmInit, list.First());
          }
          break;

        #endregion Production (Graphic)

        #region Production by Age

        case EnumRptLeadSource.ProductionbyAge:
          list.AddRange(await BRReportsByLeadSource.GetProductionByAgeInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportProductionByAgeInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByAgeInhouse>().ToList());
          }
          break;

        #endregion Production by Age

        #region Production by Age, Market & Originally Available

        case EnumRptLeadSource.ProductionbyAgeMarketOriginallyAvailable:
          list.AddRange(await BRReportsByLeadSource.GetProductionByAgeMarketOriginallyAvailableInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportProductionByAgeMarketOriginallyAvailableInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByAgeMarketOriginallyAvailableInhouse>().ToList());
          }
          break;

        #endregion Production by Age, Market & Originally Available

        #region Production by Agency

        case EnumRptLeadSource.ProductionbyAgency:
          list.Add(await BRReportsByLeadSource.GetRptProductionByAgencyInhouses(_dtmStart, _dtmEnd, _lstLeadSources,
            _enumQuinellas, salesByMembershipType: _enumSalesByMemberShipType, basedOnArrival: _enumBasedOnArrival,
            external: _enumExternalInvitation));

          if (!list.Cast<ProductionByAgencyInhouseData>().First().ProductionByAgencyInhouses.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
               _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumSalesByMemberShipType)) filters.Add(new Tuple<string, string>(_frmFilter.chkSalesByMembershipType.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumExternalInvitation)) filters.Add(new Tuple<string, string>(EnumToListHelper.GetEnumDescription(_enumExternalInvitation), string.Empty));
            finfo = clsReports.ExportProductionByAgencyInhouses(reportname, dateRangeFileNameRep, filters, list.First());
          }
          break;

        #endregion Production by Agency

        #region Production by Agency (Nights)

        case EnumRptLeadSource.ProductionbyAgencyNights:
          list.Add(await BRReportsByLeadSource.GetRptProductionByAgencyInhouses(_dtmStart, _dtmEnd, _lstLeadSources,
            _enumQuinellas, true, Convert.ToInt32(_frmFilter.txtStartN.Text), Convert.ToInt32(_frmFilter.txtEndN.Text), _enumSalesByMemberShipType,
            basedOnArrival: _enumBasedOnArrival, external: _enumExternalInvitation));
          if (!list.Cast<ProductionByAgencyInhouseData>().First().ProductionByAgencyInhouses.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumSalesByMemberShipType)) filters.Add(new Tuple<string, string>(_frmFilter.chkSalesByMembershipType.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumExternalInvitation)) filters.Add(new Tuple<string, string>(EnumToListHelper.GetEnumDescription(_enumExternalInvitation), string.Empty));
            filters.Add(Tuple.Create($"{Convert.ToInt32(_frmFilter.txtStartN.Text)}-{Convert.ToInt32(_frmFilter.txtEndN.Text)} nights", string.Empty));
            finfo = clsReports.ExportProductionByAgencyInhouses(reportname, dateRangeFileNameRep, filters, list.First());
          }
          break;

        #endregion Production by Agency (Nights)

        #region Production by Agency (Only Quinellas)

        case EnumRptLeadSource.ProductionbyAgencyOnlyQuinellas:
          list.Add(await BRReportsByLeadSource.GetRptProductionByAgencyInhouses(_dtmStart, _dtmEnd, _lstLeadSources,
           _enumQuinellas, salesByMembershipType: _enumSalesByMemberShipType, basedOnArrival: _enumBasedOnArrival,
           external: _enumExternalInvitation, onlyQuinellas: true));
          if (!list.Cast<ProductionByAgencyInhouseData>().First().ProductionByAgencyInhouses.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(), _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumSalesByMemberShipType)) filters.Add(new Tuple<string, string>(_frmFilter.chkSalesByMembershipType.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumExternalInvitation)) filters.Add(new Tuple<string, string>(EnumToListHelper.GetEnumDescription(_enumExternalInvitation), string.Empty));
            filters.Add(Tuple.Create("Only Quinellas", string.Empty));
            finfo = clsReports.ExportProductionByAgencyInhouses(reportname, dateRangeFileNameRep, filters, list.First());
          }
          break;

        #endregion Production by Agency (Only Quinellas)

        #region Production by Contract & Agency

        case EnumRptLeadSource.ProductionbyContractAgency:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByContractAgencyInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _lstMarkets, _lstAgencies, _enumQuinellas));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdMarkets.Columns[0].Header.ToString(), _frmFilter.grdMarkets.Items.Count == _lstMarkets.Count ? "ALL" : string.Join(",", _lstMarkets)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdAgencies.Columns[0].Header.ToString(), _frmFilter.grdAgencies.Items.Count == _lstAgencies.Count ? "ALL" : string.Join(",", _lstAgencies)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByContractAgencyInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByContractAgencyInhouse>().ToList());
          }
          break;

        #endregion Production by Contract & Agency

        #region Production by Contract, Agency, Market & Originally Available

        case EnumRptLeadSource.ProductionbyContractAgencyMarketOriginallyAvailable:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByContractAgencyMarketOriginallyAvailableInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _lstMarkets, _lstAgencies, _enumQuinellas));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdMarkets.Columns[0].Header.ToString(), _frmFilter.grdMarkets.Items.Count == _lstMarkets.Count ? "ALL" : string.Join(",", _lstMarkets)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdAgencies.Columns[0].Header.ToString(), _frmFilter.grdAgencies.Items.Count == _lstAgencies.Count ? "ALL" : string.Join(",", _lstAgencies)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses(reportname, dateRangeFileNameRep, filters,
              list.Cast<RptProductionByContractAgencyMarketOriginallyAvailableInhouse>().ToList());
          }
          break;

        #endregion Production by Contract, Agency, Market & Originally Available

        #region Production by Couple Type

        case EnumRptLeadSource.ProductionbyCoupleType:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByCoupleTypeInhouses(_dtmStart, _dtmEnd, _lstLeadSources, considerQuinellas: _enumQuinellas, basedOnArrival: _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByCoupleTypeInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByCoupleTypeInhouse>().ToList());
          }
          break;

        #endregion Production by Couple Type

        #region Production by Desk

        case EnumRptLeadSource.ProductionbyDesk:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByDeskInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByDeskInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByDeskInhouse>().ToList());
          }
          break;

        #endregion Production by Desk

        #region Production by Gift & Quantity

        case EnumRptLeadSource.ProductionbyGiftQuantity:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByGiftQuantities(_dtmStart, _dtmEnd, _lstLeadSources, _lstGiftsQuantity, _enumQuinellas,
            _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdGiftsQuantity.Columns[1].Header.ToString(), string.Join(",", _lstGiftsQuantity.Select(c => c.Value + "-" + c.Key))));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByGiftQuantities(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByGiftQuantity>().ToList());
          }
          break;

        #endregion Production by Gift & Quantity

        #region Production by Group

        case EnumRptLeadSource.ProductionbyGroup:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByGroupInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByGroupInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByGroupInhouse>().ToList());
          }
          break;

        #endregion Production by Group

        #region Production by Guest Status

        case EnumRptLeadSource.ProductionbyGuestStatus:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByGuestStatusInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByGuestStatusInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByGuestStatusInhouse>().ToList());
          }
          break;

        #endregion Production by Guest Status

        #region Production by Member Type & Agency

        case EnumRptLeadSource.ProductionbyMemberTypeAgency:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByMemberTypeAgencyInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _lstMarkets, _lstAgencies, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdMarkets.Columns[0].Header.ToString(), _frmFilter.grdMarkets.Items.Count == _lstMarkets.Count ? "ALL" : string.Join(",", _lstMarkets)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdAgencies.Columns[0].Header.ToString(), _frmFilter.grdAgencies.Items.Count == _lstAgencies.Count ? "ALL" : string.Join(",", _lstAgencies)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByMemberTypeAgencyInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByMemberTypeAgencyInhouse>().ToList());
          }
          break;

        #endregion Production by Member Type & Agency

        #region Production by Member Type, Agency, Market & Originally Available

        case EnumRptLeadSource.ProductionbyMemberTypeAgencyMarketOriginallyAvailable:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _lstMarkets, _lstAgencies, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdMarkets.Columns[0].Header.ToString(), _frmFilter.grdMarkets.Items.Count == _lstMarkets.Count ? "ALL" : string.Join(",", _lstMarkets)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdAgencies.Columns[0].Header.ToString(), _frmFilter.grdAgencies.Items.Count == _lstAgencies.Count ? "ALL" : string.Join(",", _lstAgencies)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse>().ToList());
          }
          break;

        #endregion Production by Member Type, Agency, Market & Originally Available

        #region Production by Nationality

        case EnumRptLeadSource.ProductionbyNationality:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByNationalityInhouses(_dtmStart, _dtmEnd, _lstLeadSources, considerQuinellas: _enumQuinellas, basedOnArrival: _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByNationalityInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByNationalityInhouse>().ToList());
          }
          break;

        #endregion Production by Nationality

        #region Production by Nationality, Market & Originally Available

        case EnumRptLeadSource.ProductionbyNationalityMarketOriginallyAvailable:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByNationalityMarketOriginallyAvailableInhouses(_dtmStart, _dtmEnd, _lstLeadSources, considerQuinellas: _enumQuinellas, filterSaveCourtesyTours: _enumSaveCourtesyTours, basedOnArrival: _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));
            filters.Add(new Tuple<string, string>("Including Save Courtesy Tours", string.Empty));

            finfo = clsReports.ExportRptProductionByNationalityMarketOriginallyAvailableInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByNationalityMarketOriginallyAvailableInhouse>().ToList());
          }
          break;

        #endregion Production by Nationality, Market & Originally Available

        #region Production by PR

        case EnumRptLeadSource.ProductionbyPR:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByPRInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByPRInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByPRInhouse>().ToList());
          }
          break;

        #endregion Production by PR

        #region Production by PR & Group

        case EnumRptLeadSource.ProductionbyPRGroup:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByPRGroupInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByPRGroupInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByPRGroupInhouse>().ToList());
          }
          break;

        #endregion Production by PR & Group

        #region Production by PR & Sales Room

        case EnumRptLeadSource.ProductionbyPRSalesRoom:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByPRSalesRoomInhouses(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByPRSalesRoomInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByPRSalesRoomInhouse>().ToList());
          }
          break;

        #endregion Production by PR & Sales Room

        #region Reps Payment

        case EnumRptLeadSource.RepsPayment:
          list.AddRange(await BRReportsByLeadSource.GetRptRepsPayments(_dtmStart, _dtmEnd, _lstLeadSources));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));

            finfo = clsReports.ExportRptRepsPayments(reportname, dateRangeFileNameRep, filters, list.Cast<RptRepsPayment>().ToList());
          }
          break;

        #endregion Reps Payment

        #region Reps Payment Summary

        case EnumRptLeadSource.RepsPaymentSummary:
          list.AddRange(await BRReportsByLeadSource.GetRptRepsPaymentSummaries(_dtmStart, _dtmEnd, _lstLeadSources));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));

            finfo = clsReports.ExportRptRepsPaymentSummaries(reportname, dateRangeFileNameRep, filters, list.Cast<RptRepsPaymentSummary>().ToList());
          }
          break;

        #endregion Reps Payment Summary

        #region Score by PR

        case EnumRptLeadSource.ScorebyPR:
          list.Add(await BRReportsByLeadSource.GetRptScoreByPrs(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas));
          if (!list.Cast<ScoreByPRData>().First().ScoreByPR.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptScoreByPrs(reportname, dateRangeFileNameRep, filters, list.First());
          }
          break;

        #endregion Score by PR

        #region Show Factor by Booking Date

        case EnumRptLeadSource.ShowFactorbyBookingDate:
          list.AddRange(await BRReportsByLeadSource.GetRptShowFactorByBookingDates(_dtmStart, _dtmEnd, _lstLeadSources, _enumQuinellas));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptShowFactorByBookingDates(reportname, dateRangeFileNameRep, filters, list.Cast<RptShowFactorByBookingDate>().ToList());
          }
          break;

        #endregion Show Factor by Booking Date

        #region Unavailable Arrivals (Graphic)

        case EnumRptLeadSource.UnavailableArrivalsGraphic:
          list.Add(await BRReportsByLeadSource.GetGraphUnavailableArrivals(_dtmInit, _lstLeadSources));
          if (list.First() == null) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            finfo = clsReports.ExportGraphUnavailableArrivals(reportname, dateRangeFileNameRep, filters, _dtmInit, list.First());
          }
          break;

        #endregion Unavailable Arrivals (Graphic)

        #region Unavailable Motives By Agency

        case EnumRptLeadSource.UnavailableMotivesByAgency:
          list.AddRange(await BRReportsByLeadSource.GetRptUnavailableMotivesByAgencies(_dtmStart, _dtmEnd, _lstLeadSources, _lstMarkets, _lstAgencies));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdLeadSources.Columns[0].Header.ToString(),
              _frmFilter.grdLeadSources.Items.Count == _lstLeadSources.Count ? "ALL" : string.Join(",", _lstLeadSources)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdMarkets.Columns[0].Header.ToString(),
              _frmFilter.grdMarkets.Items.Count == _lstMarkets.Count ? "ALL" : string.Join(",", _lstMarkets)));
            filters.Add(new Tuple<string, string>(_frmFilter.grdAgencies.Columns[0].Header.ToString(),
              _frmFilter.grdAgencies.Items.Count == _lstAgencies.Count ? "ALL" : string.Join(",", _lstAgencies)));

            finfo = clsReports.ExportRptUnavailableMotivesByAgencies(reportname, dateRangeFileNameRep, filters, list.Cast<RptUnavailableMotivesByAgency>().ToList());
          }
          break;

          #endregion Unavailable Motives By Agency
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
    ///   Muestra el reporte seleccionado por PR
    /// </summary>
    /// <history>
    /// [aalcocer] 23/Mar/2016 Created
    /// [aalcocer] 24/05/2016 Modified. Se agregó asincronía
    /// </history>
    private async void ShowPRReport()
    {
      FileInfo finfo = null;
      string dateRange = _blnOneDate ? DateHelper.DateRange(_dtmStart, _dtmStart) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = _blnOneDate ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);

      string reportname = EnumToListHelper.GetEnumDescription(_rptPR);
      List<Tuple<string, string>> filters = new List<Tuple<string, string>> { new Tuple<string, string>("Date Range", dateRange) };
      List<dynamic> list = new List<dynamic>();
      WaitMessage(true, "Loading report...");

      switch (_rptPR)
      {
        #region Production by Couple Type

        case EnumRptPR.ProductionbyCoupleType:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByCoupleTypeInhouses(_dtmStart, _dtmEnd, new[] { "ALL" }, _lstPersonnel, _enumProgram, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPersonnel.Items.Count == _lstPersonnel.Count ? "ALL" : string.Join(",", _lstPersonnel)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByCoupleTypeInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByCoupleTypeInhouse>().ToList());
          }
          break;

        #endregion Production by Couple Type

        #region Production by Nationality

        case EnumRptPR.ProductionbyNationality:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByNationalityInhouses(_dtmStart, _dtmEnd, new[] { "ALL" }, _lstPersonnel,
            _enumProgram, _enumQuinellas, _enumSaveCourtesyTours, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPersonnel.Items.Count == _lstPersonnel.Count ? "ALL" : string.Join(",", _lstPersonnel)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByNationalityInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByNationalityInhouse>().ToList());
          }
          break;

        #endregion Production by Nationality

        #region Production by Nationality, Market & Originally Available

        case EnumRptPR.ProductionbyNationalityMarketOriginallyAvailable:
          list.AddRange(await BRReportsByLeadSource.GetRptProductionByNationalityMarketOriginallyAvailableInhouses(_dtmStart, _dtmEnd, new[] { "ALL" }, _lstPersonnel,
           _enumProgram, _enumQuinellas, _enumSaveCourtesyTours, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPersonnel.Items.Count == _lstPersonnel.Count ? "ALL" : string.Join(",", _lstPersonnel)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByNationalityMarketOriginallyAvailableInhouses(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByNationalityMarketOriginallyAvailableInhouse>().ToList());
          }
          break;

          #endregion Production by Nationality, Market & Originally Available
      }
      if (finfo != null)
      {
        Process.Start(finfo.FullName);
      }

      WaitMessage(false);
    }

    #endregion ShowPRReport

    #region ShowGeneralReport

    /// <summary>
    ///   Muestra el reporte seleccionado
    /// </summary>
    /// <history>
    /// [aalcocer] 22/04/2016 Created
    /// [aalcocer] 24/05/2016 Modified. Se agregó asincronía
    /// </history>
    private async void ShowGeneralReport()
    {
      FileInfo finfo = null;
      string dateRange = _blnOneDate ? DateHelper.DateRange(_dtmStart, _dtmStart) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = _blnOneDate ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);

      string reportname = EnumToListHelper.GetEnumDescription(_rptGeneral);
      List<Tuple<string, string>> filters = new List<Tuple<string, string>> { new Tuple<string, string>("Date Range", dateRange) };
      List<dynamic> list = new List<dynamic>();
      WaitMessage(true, "Loading report...");

      switch (_rptGeneral)
      {
        #region Production by Agency (Monthly)

        case EnumRptGeneral.ProductionbyAgencyMonthly:
          list.AddRange(await BRGeneralReports.GetRptProductionByAgencyMonthly(_dtmStart, _dtmEnd, _lstAgencies, _enumQuinellas, _enumBasedOnArrival));
          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>(_frmFilter.grdAgencies.Columns[0].Header.ToString(), _frmFilter.grdAgencies.Items.Count == _lstAgencies.Count ? "ALL" : string.Join(",", _lstAgencies)));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));

            finfo = clsReports.ExportRptProductionByAgencyMonthly(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByAgencyMonthly>().ToList());
          }
          break;

        #endregion Production by Agency (Monthly)

        #region Production by Member

        case EnumRptGeneral.ProductionbyMember:
          list.AddRange(await BRGeneralReports.GetRptProductionByMembers(_dtmStart, _dtmEnd, new[] { "ALL" }, program: _enumProgram, company: _iCompany,
            club: _club, aplication: _strApplication, onlyWholesalers: _blnOnlyWholesalers, considerQuinellas: _enumQuinellas, basedOnArrival: _enumBasedOnArrival));

          if (!list.Any()) UIHelper.ShowMessage("There is no info to make a report", MessageBoxImage.Warning);
          else
          {
            filters.Add(new Tuple<string, string>("Membership", "ALL"));
            if (Convert.ToBoolean(_enumQuinellas)) filters.Add(new Tuple<string, string>(_frmFilter.chkQuinellas.Content.ToString(), string.Empty));
            if (Convert.ToBoolean(_enumBasedOnArrival)) filters.Add(new Tuple<string, string>(_frmFilter.chkBasedOnArrival.Content.ToString(), string.Empty));
            if (_blnOnlyWholesalers) filters.Add(new Tuple<string, string>(_frmFilter.chkOnlyWholesalers.Content.ToString(), string.Empty));
            finfo = clsReports.ExportRptProductionByMembers(reportname, dateRangeFileNameRep, filters, list.Cast<RptProductionByMember>().ToList());
          }
          break;

          #endregion Production by Member
      }
      if (finfo != null)
      {
        Process.Start(finfo.FullName);
      }

      WaitMessage(false);
    }

    #endregion ShowGeneralReport

    #endregion Métodos Privados
  }
}