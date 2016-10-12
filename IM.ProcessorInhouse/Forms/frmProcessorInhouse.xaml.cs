using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.ProcessorInhouse.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public ClsFilter _clsFilter;
    private frmFilterDateRange _frmFilter;
    private frmReportQueue _frmReportQueue;

    private bool _blnOneDate;
    private bool _blnOnlyOneRegister;

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
      lblUserName.Content = Context.User.User.peN;
      _frmReportQueue = new frmReportQueue(Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly));
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

    #region btnReportQueue_Click

    /// <summary>
    ///   Método para abrir la ventana de cola de Reportes
    /// </summary>
    /// <history>
    ///   [aalcocer] 03/06/2016 Created
    /// </history>
    private void btnReportQueue_Click(object sender, RoutedEventArgs e)
    {
      _frmReportQueue.Show();
      if (_frmReportQueue.WindowState == WindowState.Minimized) _frmReportQueue.WindowState = WindowState.Normal;
      _frmReportQueue.Activate();

    }

    #endregion btnReportQueue_Click

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
      _clsFilter = new ClsFilter();
      // obtenemos las fechas iniciales de los reportes
      GetFirstDayValue();

      _enumProgram = EnumProgram.Inhouse; //Programa
      _clsFilter.EnumBasedOnArrival = EnumBasedOnArrival.BasedOnArrival; //Basado en llegada
      _clsFilter.EnumQuinellas = EnumQuinellas.NoQuinellas; //No considerar quinielas
      _clsFilter.EnumSaveCourtesyTours = EnumSaveCourtesyTours.IncludeSaveCourtesyTours; //Incluir tours de rescate y cortesia
      _clsFilter.EnumSalesByMemberShipType = EnumSalesByMemberShipType.Detail; //Detallar ventas por tipo de membresia
      _clsFilter.EnumExternalInvitation = EnumExternalInvitation.Include; //Incluir invitaciones externas
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
      DateTime serverDate = BRHelpers.GetServerDate();
      // Fecha inicial
      _clsFilter.DtmStart = new DateTime(serverDate.Year, serverDate.Month, 1);

      // obtenemos la fecha de inicio de la semana
      _clsFilter.DtmInit = DateHelper.GetStartWeek(serverDate.AddDays(-7)).Date;

      //Fecha final
      _clsFilter.DtmEnd = serverDate;
      string strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(strArchivo)) return;
      var _iniFileHelper = new IniFileHelper(strArchivo);
      _clsFilter.DtmStart = _iniFileHelper.readDate("FilterDate", "DateStart", _clsFilter.DtmStart);
      _clsFilter.DtmEnd = _iniFileHelper.readDate("FilterDate", "DateEnd", _clsFilter.DtmEnd);
      string strLeadSource = _iniFileHelper.ReadText("FilterDate", "LeadSource", string.Empty);
      if (!string.IsNullOrEmpty(strLeadSource)) _clsFilter.LstLeadSources.Add(strLeadSource);
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
      _frmFilter = new frmFilterDateRange { _frmIh = this, Owner = this};

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
            enumBasedOnArrival: _clsFilter.EnumBasedOnArrival,
            enumQuinellas: _clsFilter.EnumQuinellas);
          break;

        case EnumRptLeadSource.OccupationContactBookShow:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            blnLeadSources: true, enumBasedOnArrival: _clsFilter.EnumBasedOnArrival, enumQuinellas: _clsFilter.EnumQuinellas,
            enumExternalInvitation: _clsFilter.EnumExternalInvitation, blnLsHotelNotNull: true);

          break;

        case EnumRptLeadSource.ProductionbyAgencyNights:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            blnLeadSources: true, enumBasedOnArrival: _clsFilter.EnumBasedOnArrival, enumQuinellas: _clsFilter.EnumQuinellas,
            enumSalesByMemberShipType: _clsFilter.EnumSalesByMemberShipType, enumExternalInvitation: _clsFilter.EnumExternalInvitation,
            blnNight: true);
          break;

        case EnumRptLeadSource.ProductionbyNationality:
        case EnumRptLeadSource.ProductionbyNationalityMarketOriginallyAvailable:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram, blnLeadSources: true,
            enumBasedOnArrival: _clsFilter.EnumBasedOnArrival, enumQuinellas: _clsFilter.EnumQuinellas);
          break;

        case EnumRptLeadSource.ProductionbyAgency:
        case EnumRptLeadSource.ProductionbyAgencyOnlyQuinellas:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram, blnLeadSources: true,
            enumBasedOnArrival: _clsFilter.EnumBasedOnArrival, enumQuinellas: _clsFilter.EnumQuinellas,
            enumSalesByMemberShipType: _clsFilter.EnumSalesByMemberShipType, enumExternalInvitation: _clsFilter.EnumExternalInvitation);
          break;

        case EnumRptLeadSource.CostbyPR:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram, blnLeadSources: true,
            enumQuinellas: _clsFilter.EnumQuinellas, enumDetailGifts: _clsFilter.EnumDetailGifts);
          break;

        case EnumRptLeadSource.ScorebyPR:
        case EnumRptLeadSource.ShowFactorbyBookingDate:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram, blnLeadSources: true,
            enumQuinellas: _clsFilter.EnumQuinellas);
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
        case EnumRptLeadSource.ProductionbyPRSalesRoom:
        case EnumRptLeadSource.ProductionbyPRGroup:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram, blnLeadSources: true,
            enumBasedOnArrival: _clsFilter.EnumBasedOnArrival, enumQuinellas: _clsFilter.EnumQuinellas);
          break;

        case EnumRptLeadSource.ProductionbyPR:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram, blnLeadSources: true,
            enumBasedOnArrival: _clsFilter.EnumBasedOnArrival, enumQuinellas: _clsFilter.EnumQuinellas,
            enumBasedOnPRLocation: _clsFilter.EnumBasedOnPRLocation);
          break;

        case EnumRptLeadSource.ProductionbyMemberTypeAgency:
        case EnumRptLeadSource.ProductionbyMemberTypeAgencyMarketOriginallyAvailable:
        case EnumRptLeadSource.ProductionbyContractAgency:
        case EnumRptLeadSource.ProductionbyContractAgencyMarketOriginallyAvailable:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram, blnLeadSources: true,
            blnMarkets: true, blnAllMarkets: true, blnAgencies: true, blnAllAgencies: true,
            enumQuinellas: _clsFilter.EnumQuinellas);
          break;

        case EnumRptLeadSource.UnavailableMotivesByAgency:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram, blnLeadSources: true,
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
        ShowLeadSourceReport(_rptLeadSource, _clsFilter);
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
      _frmFilter = new frmFilterDateRange { _frmIh = this, Owner = this};

      #region Abriendo FrmFilter segun reporte seleccionado.

      _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
        blnPersonnel: true, enumBasedOnArrival: _clsFilter.EnumBasedOnArrival, enumQuinellas: _clsFilter.EnumQuinellas);

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowPRReport(_rptPR, _clsFilter);
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
      _frmFilter = new frmFilterDateRange { _frmIh = this, Owner = this};

      #region Abriendo FrmFilter segun reporte seleccionado.

      switch (_rptGeneral)
      {
        case EnumRptGeneral.ProductionbyAgencyMonthly:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            enumPeriod: EnumPeriod.Monthly, blnAgencies: true, blnAllAgencies: true,
            enumBasedOnArrival: _clsFilter.EnumBasedOnArrival,
            enumQuinellas: _clsFilter.EnumQuinellas, blnAgencyMonthly: true);
          break;

        case EnumRptGeneral.ProductionbyMember:
          _frmFilter.ConfigurarFomulario(_blnOneDate, _blnOnlyOneRegister, program: _enumProgram,
            enumBasedOnArrival: _clsFilter.EnumBasedOnArrival, enumQuinellas: _clsFilter.EnumQuinellas, enumOnlyWholesalers: _clsFilter.EnumOnlyWholesalers, blnClub: true);
          break;
      }

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowGeneralReport(_rptGeneral, _clsFilter);
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
    /// <param name="rptLeadSource"></param>
    /// <param name="clsFilter"></param>
    /// <history>
    ///   [aalcocer] 23/Mar/2016 Created
    ///   [aalcocer] 24/05/2016 Modified. Se agregó asincronía
    /// </history>
    private async void ShowLeadSourceReport(EnumRptLeadSource rptLeadSource, ClsFilter clsFilter)
    {
      FileInfo finfo = null;
      string dateRange = _blnOneDate ? DateHelper.DateRange(clsFilter.DtmInit, clsFilter.DtmInit.AddDays(6)) : DateHelper.DateRange(clsFilter.DtmStart, clsFilter.DtmEnd);
      string dateRangeFileNameRep = _blnOneDate ? DateHelper.DateRangeFileName(clsFilter.DtmInit, clsFilter.DtmInit) : DateHelper.DateRangeFileName(clsFilter.DtmStart, clsFilter.DtmEnd);

      string reportname = EnumToListHelper.GetEnumDescription(rptLeadSource);
      if (rptLeadSource == EnumRptLeadSource.CostbyPR && Convert.ToBoolean(clsFilter.EnumDetailGifts))
        reportname += " With Details Gifts";
      var filters = new List<Tuple<string, string>> { new Tuple<string, string>("Date Range", dateRange) };
      var list = new List<dynamic>();
      string fileFullPath = ReportBuilder.CreateEmptyExcel(reportname, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, reportname);
      try
      {
        switch (rptLeadSource)
        {
          #region Cost by PR

          case EnumRptLeadSource.CostbyPR:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));

            if (!Convert.ToBoolean(clsFilter.EnumDetailGifts))
            {
              list.AddRange(await BRReportsByLeadSource.GetRptCostByPR(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources, clsFilter.EnumQuinellas));
              if (list.Any()) finfo = await clsReports.ExportRptCostByPR(reportname, fileFullPath, filters, list.Cast<RptCostByPR>().ToList());
            }
            else
            {
              list.AddRange(await BRReportsByLeadSource.GetRptCostByPRWithDetailGifts(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
                clsFilter.EnumQuinellas));
              if (list.Any()) finfo = await clsReports.ExportRptCostByPRWithDetailGifts(reportname, fileFullPath, filters, list.Cast<RptCostByPRWithDetailGifts>().ToList());
            }
            break;

          #endregion Cost by PR

          #region Follow Up by Agency

          case EnumRptLeadSource.FollowUpbyAgency:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptFollowUpByAgencies(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
              clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptFollowUpByAgencies(reportname, fileFullPath, filters, list.Cast<RptFollowUpByAgency>().ToList());
            break;

          #endregion Follow Up by Agency

          #region Follow Up by PR

          case EnumRptLeadSource.FollowUpbyPR:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GeRptFollowUpByPRs(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
              clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptFollowUpByPRs(reportname, fileFullPath, filters, list.Cast<RptFollowUpByPR>().ToList());
            break;

          #endregion Follow Up by PR

          #region Gifts Received by Sales Room

          case EnumRptLeadSource.GiftsReceivedbySalesRoom:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            filters.Add(Tuple.Create(GetSettings.StrChargeTo, clsFilter.BlnAllChargeTo ? GetSettings.StrAll : string.Join(",", clsFilter.LstChargeTo)));
            filters.Add(Tuple.Create(GetSettings.StrGifts, clsFilter.BlnAllGifts ? GetSettings.StrAll : string.Join(",", clsFilter.LstGifts)));

            list.Add(await BRReportsByLeadSource.GetRptGiftsReceivedBySRData(clsFilter.DtmStart, clsFilter.DtmEnd, string.Join(",", clsFilter.LstLeadSources),
              string.Join(",", clsFilter.LstChargeTo), clsFilter.BlnAllGifts ? GetSettings.StrAll : string.Join(",", clsFilter.LstGifts)));
            if (list.Cast<GiftsReceivedBySRData>().First().GiftsReceivedBySR.Any()) finfo = await clsReports.ExportRptGiftsReceivedBySR(reportname, fileFullPath, filters, list.First());
            break;

          #endregion Gifts Received by Sales Room

          #region Not Booking Arrivals (Graphic)

          case EnumRptLeadSource.NotBookingArrivalsGraphic:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));

            list.Add(await BRReportsByLeadSource.GetGraphNotBookingArrival(clsFilter.DtmInit, clsFilter.LstLeadSources));
            if (list.First() != null) finfo = clsReports.ExportGraphNotBookingArrival(reportname, fileFullPath, filters, clsFilter.DtmInit, list.First());
            break;

          #endregion Not Booking Arrivals (Graphic)

          #region Occupation, Contact, Book & Show

          case EnumRptLeadSource.OccupationContactBookShow:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumExternalInvitation)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumExternalInvitation), string.Empty));

            if (!Convert.ToBoolean(clsFilter.EnumQuinellas))
            {
              list.AddRange(await BRReportsByLeadSource.GetRptProductionByMonths(clsFilter.DtmStart, clsFilter.DtmEnd,
                clsFilter.LstLeadSources, external: clsFilter.EnumExternalInvitation, basedOnArrival: clsFilter.EnumBasedOnArrival));
              if (list.Any()) finfo = clsReports.ExportRptProductionByMonths(reportname, fileFullPath, filters, list.Cast<RptProductionByMonth>().ToList());
            }
            else
            {
              list.AddRange(
                await BRReportsByLeadSource.GetRptContactBookShowQuinellas(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
                  clsFilter.EnumExternalInvitation, clsFilter.EnumBasedOnArrival));
              if (list.Any()) finfo = clsReports.ExportRptContactBookShowQuinellas(reportname, fileFullPath, filters, list.Cast<RptContactBookShowQuinellas>().ToList());
            }
            break;

          #endregion Occupation, Contact, Book & Show

          #region Production (Graphic)

          case EnumRptLeadSource.ProductionGraphic:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.Add(await BRReportsByLeadSource.GetGraphProduction(clsFilter.DtmInit, clsFilter.LstLeadSources, clsFilter.EnumQuinellas,
              clsFilter.EnumBasedOnArrival));
            if (list.First() != null) finfo = clsReports.ExportGraphProduction(reportname, fileFullPath, filters, clsFilter.DtmInit, list.First());
            break;

          #endregion Production (Graphic)

          #region Production by Age

          case EnumRptLeadSource.ProductionbyAge:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetProductionByAgeInhouses(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
              clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportProductionByAgeInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByAgeInhouse>().ToList());
            break;

          #endregion Production by Age

          #region Production by Age, Market & Originally Available

          case EnumRptLeadSource.ProductionbyAgeMarketOriginallyAvailable:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetProductionByAgeMarketOriginallyAvailableInhouses(clsFilter.DtmStart,
              clsFilter.DtmEnd, clsFilter.LstLeadSources, clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportProductionByAgeMarketOriginallyAvailableInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByAgeMarketOriginallyAvailableInhouse>().ToList());
            break;

          #endregion Production by Age, Market & Originally Available

          #region Production by Agency

          case EnumRptLeadSource.ProductionbyAgency:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumSalesByMemberShipType)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumSalesByMemberShipType), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumExternalInvitation)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumExternalInvitation), string.Empty));

            list.Add(await BRReportsByLeadSource.GetRptProductionByAgencyInhouses(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
              clsFilter.EnumQuinellas, salesByMembershipType: clsFilter.EnumSalesByMemberShipType, basedOnArrival: clsFilter.EnumBasedOnArrival,
              external: clsFilter.EnumExternalInvitation));
            if (list.Cast<ProductionByAgencyInhouseData>().First().ProductionByAgencyInhouses.Any()) finfo = await clsReports.ExportProductionByAgencyInhouses(reportname, fileFullPath, filters, list.First());
            break;

          #endregion Production by Agency

          #region Production by Agency (Nights)

          case EnumRptLeadSource.ProductionbyAgencyNights:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumSalesByMemberShipType)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumSalesByMemberShipType), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumExternalInvitation)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumExternalInvitation), string.Empty));
            filters.Add(Tuple.Create($"{clsFilter.IntStartN}-{clsFilter.IntEndN} nights", string.Empty));

            list.Add(await BRReportsByLeadSource.GetRptProductionByAgencyInhouses(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
              clsFilter.EnumQuinellas, true, clsFilter.IntStartN, clsFilter.IntEndN, clsFilter.EnumSalesByMemberShipType,
              basedOnArrival: clsFilter.EnumBasedOnArrival, external: clsFilter.EnumExternalInvitation));
            if (list.Cast<ProductionByAgencyInhouseData>().First().ProductionByAgencyInhouses.Any()) finfo = await clsReports.ExportProductionByAgencyInhouses(reportname, fileFullPath, filters, list.First());
            break;

          #endregion Production by Agency (Nights)

          #region Production by Agency (Only Quinellas)

          case EnumRptLeadSource.ProductionbyAgencyOnlyQuinellas:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumSalesByMemberShipType)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumSalesByMemberShipType), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumExternalInvitation)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumExternalInvitation), string.Empty));
            filters.Add(Tuple.Create("Only Quinellas", string.Empty));

            list.Add(await BRReportsByLeadSource.GetRptProductionByAgencyInhouses(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
              clsFilter.EnumQuinellas, salesByMembershipType: clsFilter.EnumSalesByMemberShipType, basedOnArrival: clsFilter.EnumBasedOnArrival,
              external: clsFilter.EnumExternalInvitation, onlyQuinellas: true));
            if (list.Cast<ProductionByAgencyInhouseData>().First().ProductionByAgencyInhouses.Any()) finfo = await clsReports.ExportProductionByAgencyInhouses(reportname, fileFullPath, filters, list.First());
            break;

          #endregion Production by Agency (Only Quinellas)

          #region Production by Contract & Agency

          case EnumRptLeadSource.ProductionbyContractAgency:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            filters.Add(Tuple.Create(GetSettings.StrMarkets, clsFilter.BlnAllMarkets ? GetSettings.StrAll : string.Join(",", clsFilter.LstMarkets)));
            filters.Add(Tuple.Create(GetSettings.StrAgencies, clsFilter.BlnAllAgencies ? GetSettings.StrAll : string.Join(",", clsFilter.LstAgencies)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            //if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByContractAgencyInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, clsFilter.BlnAllMarkets ? null : clsFilter.LstMarkets,
              clsFilter.BlnAllAgencies ? null : clsFilter.LstAgencies, clsFilter.EnumQuinellas));
            //, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByContractAgencyInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByContractAgencyInhouse>().ToList());
            break;

          #endregion Production by Contract & Agency

          #region Production by Contract, Agency, Market & Originally Available

          case EnumRptLeadSource.ProductionbyContractAgencyMarketOriginallyAvailable:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            filters.Add(Tuple.Create(GetSettings.StrMarkets, clsFilter.BlnAllMarkets ? GetSettings.StrAll : string.Join(",", clsFilter.LstMarkets)));
            filters.Add(Tuple.Create(GetSettings.StrAgencies, clsFilter.BlnAllAgencies ? GetSettings.StrAll : string.Join(",", clsFilter.LstAgencies)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            //if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByContractAgencyMarketOriginallyAvailableInhouses(clsFilter.DtmStart,
              clsFilter.DtmEnd, clsFilter.LstLeadSources, clsFilter.BlnAllMarkets ? null : clsFilter.LstMarkets,
              clsFilter.BlnAllAgencies ? null : clsFilter.LstAgencies, clsFilter.EnumQuinellas));
            //, clsFilter.EnumBasedOnArrival));
            if (list.Any())
              finfo = await clsReports.ExportRptProductionByContractAgencyMarketOriginallyAvailableInhouses(reportname, fileFullPath, filters,
                list.Cast<RptProductionByContractAgencyMarketOriginallyAvailableInhouse>().ToList());
            break;

          #endregion Production by Contract, Agency, Market & Originally Available

          #region Production by Couple Type

          case EnumRptLeadSource.ProductionbyCoupleType:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByCoupleTypeInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, considerQuinellas: clsFilter.EnumQuinellas, basedOnArrival: clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByCoupleTypeInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByCoupleTypeInhouse>().ToList());
            break;

          #endregion Production by Couple Type

          #region Production by Desk

          case EnumRptLeadSource.ProductionbyDesk:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByDeskInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByDeskInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByDeskInhouse>().ToList());
            break;

          #endregion Production by Desk

          #region Production by Gift & Quantity

          case EnumRptLeadSource.ProductionbyGiftQuantity:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            filters.Add(Tuple.Create(GetSettings.StrGifts, string.Join(",", clsFilter.LstGiftsQuantity.Select(c => c.Value + "-" + c.Key))));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByGiftQuantities(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, clsFilter.LstGiftsQuantity, clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = clsReports.ExportRptProductionByGiftQuantities(reportname, fileFullPath, filters, list.Cast<RptProductionByGiftQuantity>().ToList());
            break;

          #endregion Production by Gift & Quantity

          #region Production by Group

          case EnumRptLeadSource.ProductionbyGroup:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByGroupInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByGroupInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByGroupInhouse>().ToList());
            break;

          #endregion Production by Group

          #region Production by Guest Status

          case EnumRptLeadSource.ProductionbyGuestStatus:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByGuestStatusInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByGuestStatusInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByGuestStatusInhouse>().ToList());
            break;

          #endregion Production by Guest Status

          #region Production by Member Type & Agency

          case EnumRptLeadSource.ProductionbyMemberTypeAgency:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            filters.Add(Tuple.Create(GetSettings.StrMarkets, clsFilter.BlnAllMarkets ? GetSettings.StrAll : string.Join(",", clsFilter.LstMarkets)));
            filters.Add(Tuple.Create(GetSettings.StrAgencies, clsFilter.BlnAllAgencies ? GetSettings.StrAll : string.Join(",", clsFilter.LstAgencies)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            //if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByMemberTypeAgencyInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, clsFilter.BlnAllMarkets ? null : clsFilter.LstMarkets,
              clsFilter.BlnAllAgencies ? null : clsFilter.LstAgencies, clsFilter.EnumQuinellas));
            //, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByMemberTypeAgencyInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByMemberTypeAgencyInhouse>().ToList());
            break;

          #endregion Production by Member Type & Agency

          #region Production by Member Type, Agency, Market & Originally Available

          case EnumRptLeadSource.ProductionbyMemberTypeAgencyMarketOriginallyAvailable:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            filters.Add(Tuple.Create(GetSettings.StrMarkets, clsFilter.BlnAllMarkets ? GetSettings.StrAll : string.Join(",", clsFilter.LstMarkets)));
            filters.Add(Tuple.Create(GetSettings.StrAgencies, clsFilter.BlnAllAgencies ? GetSettings.StrAll : string.Join(",", clsFilter.LstAgencies)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            //if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(
              await
                BRReportsByLeadSource.GetRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
                  clsFilter.LstLeadSources, clsFilter.BlnAllMarkets ? null : clsFilter.LstMarkets,
                  clsFilter.BlnAllAgencies ? null : clsFilter.LstAgencies, clsFilter.EnumQuinellas));
            //, clsFilter.EnumBasedOnArrival));
            if (list.Any())
              finfo = await clsReports.ExportRptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouses(reportname, fileFullPath, filters,
                list.Cast<RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse>().ToList());
            break;

          #endregion Production by Member Type, Agency, Market & Originally Available

          #region Production by Nationality

          case EnumRptLeadSource.ProductionbyNationality:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));
            filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumSaveCourtesyTours), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByNationalityInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, considerQuinellas: clsFilter.EnumQuinellas,
              filterSaveCourtesyTours: clsFilter.EnumSaveCourtesyTours, basedOnArrival: clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByNationalityInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByNationalityInhouse>().ToList());
            break;

          #endregion Production by Nationality

          #region Production by Nationality, Market & Originally Available

          case EnumRptLeadSource.ProductionbyNationalityMarketOriginallyAvailable:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));
            filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumSaveCourtesyTours), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByNationalityMarketOriginallyAvailableInhouses(clsFilter.DtmStart,
              clsFilter.DtmEnd, clsFilter.LstLeadSources, considerQuinellas: clsFilter.EnumQuinellas,
              filterSaveCourtesyTours: clsFilter.EnumSaveCourtesyTours, basedOnArrival: clsFilter.EnumBasedOnArrival));
            if (list.Any())
              finfo = await clsReports.ExportRptProductionByNationalityMarketOriginallyAvailableInhouses(reportname, fileFullPath, filters,
                list.Cast<RptProductionByNationalityMarketOriginallyAvailableInhouse>().ToList());
            break;

          #endregion Production by Nationality, Market & Originally Available

          #region Production by PR

          case EnumRptLeadSource.ProductionbyPR:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnPRLocation)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnPRLocation), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByPRInhouses(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
              clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival, clsFilter.EnumBasedOnPRLocation));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByPRInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByPRInhouse>().ToList());
            break;

          #endregion Production by PR

          #region Production by PR & Group

          case EnumRptLeadSource.ProductionbyPRGroup:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(
              await BRReportsByLeadSource.GetRptProductionByPRGroupInhouses(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
                clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = clsReports.ExportRptProductionByPRGroupInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByPRGroupInhouse>().ToList());
            break;

          #endregion Production by PR & Group

          #region Production by PR & Sales Room

          case EnumRptLeadSource.ProductionbyPRSalesRoom:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByPRSalesRoomInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = clsReports.ExportRptProductionByPRSalesRoomInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByPRSalesRoomInhouse>().ToList());
            break;

          #endregion Production by PR & Sales Room

          #region Reps Payment

          case EnumRptLeadSource.RepsPayment:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));

            list.AddRange(await BRReportsByLeadSource.GetRptRepsPayments(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources));
            if (list.Any()) finfo = clsReports.ExportRptRepsPayments(reportname, fileFullPath, filters, list.Cast<RptRepsPayment>().ToList());
            break;

          #endregion Reps Payment

          #region Reps Payment Summary

          case EnumRptLeadSource.RepsPaymentSummary:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));

            list.AddRange(await BRReportsByLeadSource.GetRptRepsPaymentSummaries(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources));
            if (list.Any()) finfo = await clsReports.ExportRptRepsPaymentSummaries(reportname, fileFullPath, filters, list.Cast<RptRepsPaymentSummary>().ToList());
            break;

          #endregion Reps Payment Summary

          #region Score by PR

          case EnumRptLeadSource.ScorebyPR:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));

            list.Add(await BRReportsByLeadSource.GetRptScoreByPrs(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources, clsFilter.EnumQuinellas));
            if (list.Cast<ScoreByPRData>().First().ScoreByPR.Any()) finfo = await clsReports.ExportRptScoreByPrs(reportname, fileFullPath, filters, list.First());
            break;

          #endregion Score by PR

          #region Show Factor by Booking Date

          case EnumRptLeadSource.ShowFactorbyBookingDate:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptShowFactorByBookingDates(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstLeadSources,
              clsFilter.EnumQuinellas));
            if (list.Any()) finfo = clsReports.ExportRptShowFactorByBookingDates(reportname, fileFullPath, filters, list.Cast<RptShowFactorByBookingDate>().ToList());
            break;

          #endregion Show Factor by Booking Date

          #region Unavailable Arrivals (Graphic)

          case EnumRptLeadSource.UnavailableArrivalsGraphic:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));

            list.Add(await BRReportsByLeadSource.GetGraphUnavailableArrivals(clsFilter.DtmInit, clsFilter.LstLeadSources));
            if (list.First() != null) finfo = clsReports.ExportGraphUnavailableArrivals(reportname, fileFullPath, filters, clsFilter.DtmInit, list.First());
            break;

          #endregion Unavailable Arrivals (Graphic)

          #region Unavailable Motives By Agency

          case EnumRptLeadSource.UnavailableMotivesByAgency:
            filters.Add(Tuple.Create(GetSettings.StrLeadSources, clsFilter.BlnAllLeadSources ? GetSettings.StrAll : string.Join(",", clsFilter.LstLeadSources)));
            filters.Add(Tuple.Create(GetSettings.StrMarkets, clsFilter.BlnAllMarkets ? GetSettings.StrAll : string.Join(",", clsFilter.LstMarkets)));
            filters.Add(Tuple.Create(GetSettings.StrAgencies, clsFilter.BlnAllAgencies ? GetSettings.StrAll : string.Join(",", clsFilter.LstAgencies)));

            list.AddRange(await BRReportsByLeadSource.GetRptUnavailableMotivesByAgencies(clsFilter.DtmStart, clsFilter.DtmEnd,
              clsFilter.LstLeadSources, clsFilter.BlnAllMarkets ? null : clsFilter.LstMarkets,
              clsFilter.BlnAllAgencies ? null : clsFilter.LstAgencies));
            if (list.Any()) finfo = clsReports.ExportRptUnavailableMotivesByAgencies(reportname, fileFullPath, filters, list.Cast<RptUnavailableMotivesByAgency>().ToList());
            break;

            #endregion Unavailable Motives By Agency
        }

        if (finfo == null)
        {
          finfo = ReportBuilder.CreateNoInfoRptExcel(filters, reportname, fileFullPath);
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

    #endregion ShowLeadSourceReport

    #region ShowPRReport

    /// <summary>
    ///   Muestra el reporte seleccionado por PR
    /// </summary>
    /// <param name="clsFilter"></param>
    /// <param name="rptPR"></param>
    /// <history>
    ///   [aalcocer] 23/Mar/2016 Created
    ///   [aalcocer] 24/05/2016 Modified. Se agregó asincronía
    /// </history>
    private async void ShowPRReport(EnumRptPR rptPR, ClsFilter clsFilter)
    {
      FileInfo finfo = null;
      string dateRange = _blnOneDate ? DateHelper.DateRange(clsFilter.DtmInit, clsFilter.DtmInit.AddDays(6)) : DateHelper.DateRange(clsFilter.DtmStart, clsFilter.DtmEnd);
      string dateRangeFileNameRep = _blnOneDate ? DateHelper.DateRangeFileName(clsFilter.DtmInit, clsFilter.DtmInit) : DateHelper.DateRangeFileName(clsFilter.DtmStart, clsFilter.DtmEnd);

      string reportname = EnumToListHelper.GetEnumDescription(rptPR);
      var filters = new List<Tuple<string, string>> { new Tuple<string, string>("Date Range", dateRange) };
      var list = new List<dynamic>();
      string fileFullPath = ReportBuilder.CreateEmptyExcel(reportname, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, reportname);
      try
      {
        switch (rptPR)
        {
          #region Production by Couple Type

          case EnumRptPR.ProductionbyCoupleType:
            filters.Add(Tuple.Create(GetSettings.StrPR, clsFilter.BlnAllPersonnel ? GetSettings.StrAll : string.Join(",", clsFilter.LstPersonnel)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(
              await
                BRReportsByLeadSource.GetRptProductionByCoupleTypeInhouses(clsFilter.DtmStart, clsFilter.DtmEnd, new[] { GetSettings.StrAll },
                  clsFilter.LstPersonnel, _enumProgram, clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByCoupleTypeInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByCoupleTypeInhouse>().ToList());
            break;

          #endregion Production by Couple Type

          #region Production by Nationality

          case EnumRptPR.ProductionbyNationality:
            filters.Add(Tuple.Create(GetSettings.StrPR, clsFilter.BlnAllPersonnel ? GetSettings.StrAll : string.Join(",", clsFilter.LstPersonnel)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByNationalityInhouses(clsFilter.DtmStart, clsFilter.DtmEnd, new[] { GetSettings.StrAll },
              clsFilter.LstPersonnel, _enumProgram, clsFilter.EnumQuinellas, clsFilter.EnumSaveCourtesyTours, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = await clsReports.ExportRptProductionByNationalityInhouses(reportname, fileFullPath, filters, list.Cast<RptProductionByNationalityInhouse>().ToList());
            break;

          #endregion Production by Nationality

          #region Production by Nationality, Market & Originally Available

          case EnumRptPR.ProductionbyNationalityMarketOriginallyAvailable:
            filters.Add(Tuple.Create(GetSettings.StrPR, clsFilter.BlnAllPersonnel ? GetSettings.StrAll : string.Join(",", clsFilter.LstPersonnel)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRReportsByLeadSource.GetRptProductionByNationalityMarketOriginallyAvailableInhouses(clsFilter.DtmStart, clsFilter.DtmEnd,
              new[] { GetSettings.StrAll }, clsFilter.LstPersonnel, _enumProgram, clsFilter.EnumQuinellas, clsFilter.EnumSaveCourtesyTours, clsFilter.EnumBasedOnArrival));
            if (list.Any())
              finfo = await clsReports.ExportRptProductionByNationalityMarketOriginallyAvailableInhouses(reportname, fileFullPath, filters,
                list.Cast<RptProductionByNationalityMarketOriginallyAvailableInhouse>().ToList());
            break;

            #endregion Production by Nationality, Market & Originally Available
        }
        if (finfo == null)
        {
          finfo = ReportBuilder.CreateNoInfoRptExcel(filters, reportname, fileFullPath);
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

    #endregion ShowPRReport

    #region ShowGeneralReport

    /// <summary>
    ///   Muestra el reporte seleccionado
    /// </summary>
    /// <param name="rptGeneral"></param>
    /// <param name="clsFilter"></param>
    /// <history>
    ///   [aalcocer] 22/04/2016 Created
    ///   [aalcocer] 24/05/2016 Modified. Se agregó asincronía
    /// </history>
    private async void ShowGeneralReport(EnumRptGeneral rptGeneral, ClsFilter clsFilter)
    {
      FileInfo finfo = null;
      string dateRange = _blnOneDate ? DateHelper.DateRange(clsFilter.DtmInit, clsFilter.DtmInit.AddDays(6)) : DateHelper.DateRange(clsFilter.DtmStart, clsFilter.DtmEnd);
      string dateRangeFileNameRep = _blnOneDate ? DateHelper.DateRangeFileName(clsFilter.DtmInit, clsFilter.DtmInit) : DateHelper.DateRangeFileName(clsFilter.DtmStart, clsFilter.DtmEnd);

      string reportname = EnumToListHelper.GetEnumDescription(rptGeneral);
      var filters = new List<Tuple<string, string>> { new Tuple<string, string>("Date Range", dateRange) };
      var list = new List<dynamic>();
      string fileFullPath = ReportBuilder.CreateEmptyExcel(reportname, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, reportname);
      try
      {
        switch (rptGeneral)
        {
          #region Production by Agency (Monthly)

          case EnumRptGeneral.ProductionbyAgencyMonthly:
            filters.Add(Tuple.Create(GetSettings.StrAgencies, clsFilter.BlnAllAgencies ? GetSettings.StrAll : string.Join(",", clsFilter.LstAgencies)));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));

            list.AddRange(await BRGeneralReports.GetRptProductionByAgencyMonthly(clsFilter.DtmStart, clsFilter.DtmEnd, clsFilter.LstAgencies,
              clsFilter.EnumQuinellas, clsFilter.EnumBasedOnArrival));
            if (list.Any()) finfo = clsReports.ExportRptProductionByAgencyMonthly(reportname, fileFullPath, filters, list.Cast<RptProductionByAgencyMonthly>().ToList());

            break;

          #endregion Production by Agency (Monthly)

          #region Production by Member

          case EnumRptGeneral.ProductionbyMember:
            filters.Add(Tuple.Create("Membership", GetSettings.StrAll));
            if (Convert.ToBoolean(clsFilter.EnumQuinellas)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumQuinellas), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumBasedOnArrival)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumBasedOnArrival), string.Empty));
            if (Convert.ToBoolean(clsFilter.EnumOnlyWholesalers)) filters.Add(Tuple.Create(EnumToListHelper.GetEnumDescription(clsFilter.EnumOnlyWholesalers), string.Empty));

            list.AddRange(await BRGeneralReports.GetRptProductionByMembers(clsFilter.DtmStart, clsFilter.DtmEnd, new[] { GetSettings.StrAll },
              program: _enumProgram, company: clsFilter.IntCompany, club: clsFilter.Club, aplication: clsFilter.StrApplication,
              onlyWholesalers: clsFilter.EnumOnlyWholesalers, considerQuinellas: clsFilter.EnumQuinellas, basedOnArrival: clsFilter.EnumBasedOnArrival));

            if (list.Any()) finfo = clsReports.ExportRptProductionByMembers(reportname, fileFullPath, filters, list.Cast<RptProductionByMember>().ToList());
            break;

            #endregion Production by Member
        }
        if (finfo == null)
        {
          finfo = ReportBuilder.CreateNoInfoRptExcel(filters, reportname, fileFullPath);
        }
        frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(finfo, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly));
        frmDocumentViewver.Show();
        _frmReportQueue.SetExist(finfo.FullName, finfo);
        _frmReportQueue.Activate();
      }
      catch (Exception ex)
      {        
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion ShowGeneralReport
    
    #endregion Métodos Privados
  }
}