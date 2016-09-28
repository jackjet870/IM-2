using IM.Base.Classes;
using IM.Base.Forms;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.ProcessorOuthouse.Classes;
using PalaceResorts.Common.PalaceTools.Epplus.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace IM.ProcessorOuthouse.Forms
{
  /// <summary>
  /// Formulario para generar los reportes Outside
  /// Interaction logic for frmProcessorOuthouse.xaml
  /// </summary>
  /// <history>
  ///   [vku] 22/Mar/2016 Created
  /// </history>
  public partial class frmProcessorOuthouse : Window
  {
    #region Constructor
    public frmProcessorOuthouse()
    {
      InitializeComponent();
      ConfigGrds();
      _clsFilter = new clsFilter();
      GetFirstDayValue();
      lblUserName.Content = Context.User.User.peN;
      _frmReportQueue = new frmReportQueue(Context.User.HasPermission(EnumPermission.RptExcel,EnumPermisionLevel.ReadOnly));
    }
    #endregion Constructor

    #region Atributos

    private frmFilterDateRange _frmFilter;
    private frmReportQueue _frmReportQueue;
    private bool _blnOneDate;
    private bool _blnOnlyOneRegister;
    public clsFilter _clsFilter;

    public List<string> _lstLeadSources = new List<string>();
    public EnumPredefinedDate? _cboDateSelected;
    public string _cboFolSeriesSelected;
    public DateTime _dtmInit;
    public DateTime _dtmStart = DateTime.Now.Date;
    public DateTime _dtmEnd = DateTime.Now.Date;
    public EnumConfiguration _enumConfiguration = EnumConfiguration.ReportsPath;
    public EnumBasedOnArrival _enumBasedOnArrival = EnumBasedOnArrival.NoBasedOnArrival;
    public EnumBasedOnBooking _enumBasedOnBooking = EnumBasedOnBooking.NoBasedOnBooking;
    public EnumQuinellas _enumQuinellas = EnumQuinellas.NoQuinellas;
    public EnumDetailGifts _enumDetailsGift = EnumDetailGifts.NoDetailGifts;
    public EnumSalesByMemberShipType _enumSalesByMemberShipType = EnumSalesByMemberShipType.NoDetail;
    public EnumStatus _enumStatus = EnumStatus.staActives;
    public EnumGiftSale _enumGiftSale = EnumGiftSale.gsAll;
    public EnumSaveCourtesyTours _enumSaveCourtesyTours = EnumSaveCourtesyTours.ExcludeSaveCourtesyTours;
    public EnumExternalInvitation _enumExternalInvitation = EnumExternalInvitation.Exclude;
    public string _folFrom;
    public string _folTo;

    #endregion Atributos

    #region Metodos

    #region ConfigGrds

    /// <summary>
    /// Se configuran los treeview, agregando
    /// los reportes.
    /// </summary>
    /// <history>
    /// [vku] 22/Mar/2016 Created
    /// </history>
    private void ConfigGrds()
    {
      #region Grid RptsByLeadSource

      ListCollectionView lstRptsByLeadSource = new ListCollectionView(new List<dynamic>{
        new {rptName="Deposits Payment by PR" },
        new {rptName="Gifts Received by Sales Room" },
        new {rptName="Guests Show No Presented Invitation"},
        new {rptName="PR Payment Commissions"},
        new {rptName="Production by Age"},
        new {rptName="Production by Age & Sales Room"},
        new {rptName="Production by Agency"},
        new {rptName="Production by Agency & Sales Room"},
        new {rptName="Production by Agency, Market & Hotel"},
        new {rptName="Production by Couple Type" },
        new {rptName="Production by Couple Type & Sales Room" },
        new {rptName="Production by Flight & Sales Room" },
        new {rptName="Production by Gift (Invitation)"},
        new {rptName="Production by Gift (Invitation) & Sales Room"},
        new {rptName="Production by Guest Status"},
        new {rptName="Production by Hotel"},
        new {rptName="Production by Hotel & Sales Room"},
        new {rptName="Production by Hotel Group"},
        new {rptName="Production by Hotel Group & Sales Room"},
        new {rptName="Production by Nationality" },
        new {rptName="Production by Nationality & Sales Room" },
        new {rptName="Production by PR"},
        new {rptName="Production by PR & Sales Room"},
        new {rptName="Production by PR & Sales Room (Deposits & Flyers Show)"},
        new {rptName="Production by PR & Sales Room (Deposits)"},
        new {rptName="Production by PR & Sales Room (Flyers)"},
        new {rptName="Production by PR (Deposits & Flyers Show)" },
        new {rptName="Production by PR (Deposits)" },
        new {rptName="Production by PR (Flyers)" },
        new {rptName="Production by PR Contact"},
        new {rptName="Production by Wave"},
        new {rptName="Production by Wave & Sales Room"},
      }.OrderBy(c => c.rptName).ToList());
      grdRptsByLeadSource.ItemsSource = lstRptsByLeadSource;

      #endregion Grid RptsByLeadSource

      #region Grid RptsByPR

      ListCollectionView lstRptsByPR = new ListCollectionView(new List<dynamic>{
        new {rptName="Production by Age"},
        new {rptName="Production by Age & Sales Room"},
        new {rptName="Production by Agency"},
        new {rptName="Production by Agency & Sales Room"},
        new {rptName="Production by Agency, Market & Hotel"},
        new {rptName="Production by Couple Type" },
        new {rptName="Production by Couple Type & Sales Room" },
        new {rptName="Production by Gift (Invitation)"},
        new {rptName="Production by Gift (Invitation) & Sales Room"},
        new {rptName="Production by Guest Status"},
        new {rptName="Production by Hotel"},
        new {rptName="Production by Hotel & Sales Room"},
        new {rptName="Production by Hotel Group"},
        new {rptName="Production by Hotel Group & Sales Room"},
        new {rptName="Production by Nationality" },
        new {rptName="Production by Nationality & Sales Room" },
        new {rptName="Production by PR & Sales Room (Deposits & Flyers Show)"},
        new {rptName="Production by PR (Deposits & Flyers Show)" },
        new {rptName="Production by Wave"},
        new {rptName="Production by Wave & Sales Room"}
      }.OrderBy(c => c.rptName).ToList());
      grdRptsByPR.ItemsSource = lstRptsByPR;

      #endregion Grid RptsByPR

      #region Grid OtherRpts

      ListCollectionView lstOtherRpts = new ListCollectionView(new List<dynamic>{
        new {rptName="Folios Invitations Outhouse"}
      }.OrderBy(c => c.rptName).ToList());
      grdOtherRpts.ItemsSource = lstOtherRpts;

      #endregion Grid OtherRpts

      StatusBarReg.Content = string.Format("{0} Reports", lstRptsByLeadSource.Count + lstRptsByPR.Count + lstOtherRpts.Count);
    }

    #endregion ConfigGrds

    #region GetFirtsDayValue
    /// <summary>
    ///   Obtiene las fechas iniciales y finales de los reportes
    /// </summary>
    /// <history>
    ///   [vku] 03/Jun/2016 Created
    /// </history>
    private void GetFirstDayValue()
    {
      DateTime serverDate = BRHelpers.GetServerDate();
      // Fecha inicial
      _dtmStart = new DateTime(serverDate.Year, serverDate.Month, 1);

      // obtenemos la fecha de inicio de la semana
      _dtmInit = DateHelper.GetStartWeek(serverDate.AddDays(-7)).Date;

      //Fecha final
      _dtmEnd = serverDate;
      string strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (!File.Exists(strArchivo)) return;
      var _iniFileHelper = new IniFileHelper(strArchivo);
      _dtmStart = _iniFileHelper.readDate("FilterDate", "DateStart", _dtmStart);
      _dtmEnd = _iniFileHelper.readDate("FilterDate", "DateEnd", _dtmEnd);
      //string strLeadSource = _iniFileHelper.ReadText("FilterDate", "LeadSource", string.Empty);
      //if (!string.IsNullOrEmpty(strLeadSource)) _clsFilter.Add(strLeadSource);
    }
    #endregion

    #region PrepareReportByLeadSource

    /// <summary>
    /// Prepara un reporte por LeadSource
    /// </summary>
    /// <history>
    ///   [vku] 28/Mar/2016 Created
    /// </history>
    private void PrepareReportByLeadSource()
    {
      String strReport = "";
      ///Se Valida que haya un reporte seleccionado
      if (grdRptsByLeadSource.SelectedItems.Count < 0)
        return;

      WaitMessage(true, "Loading Date Range Window...");

      ///Se obtiene el nombre del reporte
      strReport = ((dynamic)grdRptsByLeadSource.SelectedItem).rptName;

      _blnOneDate = false;
      _blnOnlyOneRegister = false;

      switch (strReport)
      {
        case "PR Payment Commissions":
          // _blnOnlyOneRegister = true;
          break;
      }

      OpenFilterDateRangeLS(strReport);
    }

    #endregion PrepareReportByLeadSource

    #region PrepareReportByPR

    /// <summary>
    /// Prepara un reporte por PR
    /// </summary>
    /// <history>
    ///   [vku] 28/Mar/2016 Created
    /// </history>
    public void PrepareReportByPR()
    {
      String strReport = "";
      if (grdRptsByPR.SelectedItems.Count < 0)
        return;

      WaitMessage(true, "Loading Date Range Window...");

      strReport = ((dynamic)grdRptsByPR.SelectedItem).rptName;
      _blnOneDate = false;
      _blnOnlyOneRegister = false;

      switch (strReport)
      {
        case "Production by Age":
        case "Production by Age y Sales Room":
        case "Production by Agency":
        case "Production by Agency & Sales Room":
        case "Production by Agency, Market & Hotel":
        case "Production by Couple Type":
        case "Production by Couple Type & Sales Room":
        case "Production by Gift (Invitation)":
        case "Production by Gift (Invitation) & Sales Room":
        case "Production by Guest Status":
        case "Production by Hotel":
        case "Production by Hotel & Sales Room":
        case "Production by Hotel Group":
        case "Production by Hotel Group & Sales Room":
        case "Production by Nationality":
        case "Production by Nationality & Sales Room":
        case "Production by PR & Sales Room (Deposits & Flayers Show)":
        case "Production by PR (Deposits & Flyers Show)":
        case "Production by Wave":
        case "Production by Wave & Sales Room":
          break;
      }
      OpenFilterDateRangePR(strReport);
    }

    #endregion PrepareReportByPR

    #region PrepareOtherReports

    /// <summary>
    /// Prepara un reporte de Others
    /// </summary>
    /// <history>
    ///   [vku] 28/Mar/2016 Created
    /// </history>
    public void PrepareOtherReports()
    {
      String strReport = "";
      if (grdOtherRpts.SelectedItems.Count < 0)
        return;

      WaitMessage(true, "Loading Date Range Window...");

      strReport = ((dynamic)grdOtherRpts.SelectedItem).rptName;
      _blnOneDate = false;
      _blnOnlyOneRegister = false;
      OpenFilterDateRangeOtherReport(strReport);
    }

    #endregion PrepareOtherReports

    #region OpenFilterDateRangeLS

    /// <summary>
    /// Abre la ventan frmFilterDateRange con los controles configurados
    /// segun el reporte seleccionado por LeadSource
    /// </summary>
    /// <history>
    ///   [vku] 28/Mar/2016 Created
    /// </history>
    private void OpenFilterDateRangeLS(string strReport)
    {
      _frmFilter = new frmFilterDateRange { frmPO = this, Owner = this };
      switch (strReport)
      {
        case "Deposits Payment by PR":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, blnPaymentTypes: true, blnAllPaymentTypes: true);
          break;

        case "Gifts Received by Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, blnChargeTo: true, blnGifts: true, blnAllChargeTo: true, blnAllGifts: true);
          break;

        case "Guests Show No Presented Invitation":
        case "Production by Age":
        case "Production by Age & Sales Room":
        case "Production by Agency, Market & Hotel":
        case "Production by Couple Type":
        case "Production by Couple Type & Sales Room":
        case "Production by Flight & Sales Room":
        case "Production by Guest Status":
        case "Production by Hotel":
        case "Production by Hotel & Sales Room":
        case "Production by Hotel Group":
        case "Production by Hotel Group & Sales Room":
        case "Production by PR Contact":
        case "Production by Wave":
        case "Production by Wave & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true);
          break;

        case "PR Payment Commissions":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSourcesPaymentComm: true, blnAllLeadSourcePaymentComm: true, enumPeriod: EnumPeriod.Weekly);
          break;

        case "Production by Agency":
        case "Production by Agency & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, enumSalesByMemberShipType: EnumSalesByMemberShipType.Detail);
          break;

        case "Production by Gift (Invitation)":
        case "Production by Gift (Invitation) & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, blnGiftProdGift: true, blnAllGiftProdGift: true);
          break;

        case "Production by Nationality":
        case "Production by Nationality & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, enumSaveCourtesyTours: EnumSaveCourtesyTours.IncludeSaveCourtesyTours);
          break;

        case "Production by PR":
        case "Production by PR & Sales Room":
        case "Production by PR & Sales Room (Deposits & Flyers Show)":
        case "Production by PR & Sales Room (Deposits)":
        case "Production by PR & Sales Room (Flyers)":
        case "Production by PR (Deposits & Flyers Show)":
        case "Production by PR (Deposits)":
        case "Production by PR (Flyers)":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, enumBasedOnBooking: EnumBasedOnBooking.BasedOnBooking);
          break;
      }

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowLeadSourceReport(strReport, _clsFilter);
        _frmFilter.Close();
      }
      else
      {
        _frmFilter.Close();
        _frmFilter = null;
      }
    }

    #endregion OpenFilterDateRangeLS

    #region ShowLeadSourceReport

    /// <summary>
    ///  Muestra un reporte por lead source
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    ///   
    /// </history>
    public async void ShowLeadSourceReport(string strReport, clsFilter filter)
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_dtmStart, _dtmEnd) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);
      DataTable dtData = new DataTable();
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      string fileFullPath = ReportBuilder.CreateEmptyExcel(strReport, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, strReport);
      try
      {
        switch (strReport)
        {
          #region Deposits Payment By PR

          case "Deposits Payment by PR":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            DepositsPaymentByPRData lstRptDepositsPaymentByPR = await BRReportsByLeadSource.GetRptDepositsPaymentByPRData(
            _dtmStart, _dtmEnd,
            string.Join(",", filter._lstLeadSources),
            "ALL",
            EnumProgram.Outhouse,
            string.Join(",", filter._lstPaymentTypes),
            EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptDepositsPaymentByPR.DepositsPaymentByPR.Any())
              finfo = clsReports.ExportRptDepositsPaymentByPR(strReport, fileFullPath, filters, lstRptDepositsPaymentByPR);
            break;

          #endregion Deposits Payment By PR

          #region Gifts Received by Sales Room

          case "Gifts Received by Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources.ToList())));
            filters.Add(new Tuple<string, string>("Charge To", filter.AllChargeTo ? "ALL" : string.Join(",", filter._lstChargeTo)));
            filters.Add(new Tuple<string, string>("Gifts", filter.AllGift ? "ALL" : string.Join(",", filter._lstGifts)));

            GiftsReceivedBySRData lstRptGiftsReceivedBySR = await BRReportsByLeadSource.GetRptGiftsReceivedBySRData(
            _dtmStart, _dtmEnd,
            string.Join(",", filter._lstLeadSources),
            filter.AllChargeTo ? "ALL" : string.Join(",", filter._lstChargeTo),
            filter.AllGift ? "ALL" : string.Join(",", filter._lstGifts));
            if (lstRptGiftsReceivedBySR.GiftsReceivedBySR.Any())
              finfo = await clsReports.ExportRptGiftsReceivedBySR(strReport, fileFullPath, filters, lstRptGiftsReceivedBySR);
            break;

          #endregion Gifts Received by Sales Room

          #region Guests Show No Presented Invitation

          case "Guests Show No Presented Invitation":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<GuestShowNoPresentedInvitation> lstRptGuestsShowNoPresentedInvitation = await BRReportsByLeadSource.GetRptGuestsShowNoPresentedInvitation(
              _dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources));
            if (lstRptGuestsShowNoPresentedInvitation.Any())
              finfo = await clsReports.ExportRptGuestsShowNoPresentedInvitation(strReport, fileFullPath, filters, lstRptGuestsShowNoPresentedInvitation);
            break;

          #endregion Guests Show No Presented Invitation

          #region PR Payment Commissions

          case "PR Payment Commissions":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", string.Join(",", filter._lstLeadSourcesPaymentComm)));

            List<RptProductionByPROuthouse> lstRptProductionByPROuthouseComm = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSourcesPaymentComm),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit,
              _enumBasedOnBooking);
            if (lstRptProductionByPROuthouseComm.Any())
              finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPROuthouseComm);
            break;

          #endregion PR Payment Commissions

          #region Production by Age

          case "Production by Age":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByAgeOuthouse> lstRptProductionByAgeOuthouse = await BRReportsByLeadSource.GetRptProductionByAge(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByAgeOuthouse.Any())
              finfo = clsReports.ExportRptProductionByAgeOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgeOuthouse);
            break;

          #endregion Production by Age

          #region Production by Age & Sales Room

          case "Production by Age & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByAgeSalesRoomOuthouse> lstRptProductionByAgeSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByAgeSalesRoomOuthouse(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByAgeSalesRoomOuthouse.Any())
              finfo = clsReports.ExportRptProductionByAgeSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgeSalesRoomOuthouse);
            break;

          #endregion Production by Age & Sales Room

          #region Production by Agency

          case "Production by Agency":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            ProductionByAgencyOuthouseData lstRptProductionByAgencyOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencyOuthouseData(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit,
              _enumSalesByMemberShipType);
            if (lstRptProductionByAgencyOuthouse.ProductionByAgencyOuthouse.Any())
              finfo = await clsReports.ExportRptProductionByAgencyOuhouse(strReport, fileFullPath, filters, lstRptProductionByAgencyOuthouse, _enumSalesByMemberShipType);
            break;

          #endregion Production by Agency

          #region Production by Agency & Sales Room

          case "Production by Agency & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            ProductionByAgencySalesRoomOuthouseData lstRptProductionByAgencySalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencySalesRoomOuthouseData(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit,
              _enumSalesByMemberShipType);
            if (lstRptProductionByAgencySalesRoomOuthouse.ProductionByAgencySalesRoomOuthouse.Any())
              finfo = await clsReports.ExportRptProductionByAgencySalesRoomOuhouse(strReport, fileFullPath, filters, lstRptProductionByAgencySalesRoomOuthouse, _enumSalesByMemberShipType);
            break;

          #endregion Production by Agency & Sales Room

          #region Production by Agency, Market & Hotel

          case "Production by Agency, Market & Hotel":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByAgencyMarketHotelOuthouse> lstRptProductionByAgencyMarketHotelOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencyMarketHotelOuthouse(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByAgencyMarketHotelOuthouse.Any())
              finfo = clsReports.ExportRptProductionByAgencyMarketHotelOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgencyMarketHotelOuthouse);
            break;

          #endregion Production by Agency, Market & Hotel

          #region Production by Couple Type

          case "Production by Couple Type":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByCoupleTypeOuthouse> lstRptProductionByCoupleTypeOuthouse = await BRReportsByLeadSource.GetRptProductionByCoupleTypeOuthouse(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByCoupleTypeOuthouse.Any())
              finfo = clsReports.ExportRptProductionByCoupleTypeOuthouse(strReport, fileFullPath, filters, lstRptProductionByCoupleTypeOuthouse);
            break;

          #endregion Production by Couple Type

          #region Production by Couple Type & Sales Room

          case "Production by Couple Type & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByCoupleTypeSalesRoomOuthouse> lstRptProductionByCoupleTypeSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByCoupleTypeSalesRoomOuthouse(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByCoupleTypeSalesRoomOuthouse.Any())
              finfo = clsReports.ExportRptProductionByCoupleTypeSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByCoupleTypeSalesRoomOuthouse);
            break;

          #endregion Production by Couple Type & Sales Room

          #region Production by Flight & Sales Room
          case "Production by Flight & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByFlightSalesRoom> lstRptProductionByFlightSalesRoom = await BRReportsByLeadSource.GetRptProductionByFlightSalesRoom(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByFlightSalesRoom.Any())
              finfo = clsReports.ExportRptProductionByFlightSalesRoom(strReport, fileFullPath, filters, lstRptProductionByFlightSalesRoom);
            break;
          #endregion

          #region Production by Gift (Invitation)

          case "Production by Gift (Invitation)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));
            filters.Add(new Tuple<string, string>("Gifts", string.Join(",", filter._lstGiftsProdGift)));

            List<RptProductionByGiftInvitation> lstRptProductionByGiftInvitation = await BRReportsByLeadSource.GetRptProductionByGiftInvitation(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              string.Join(",", filter._lstGiftsProdGift),
             EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByGiftInvitation.Any())
              finfo = clsReports.ExportRptProductionByGiftInvitation(strReport, fileFullPath, filters, lstRptProductionByGiftInvitation);
            break;

          #endregion Production by Gift (Invitation)

          #region Production by Gift (Invitation) & Sales Room

          case "Production by Gift (Invitation) & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));
            filters.Add(new Tuple<string, string>("Gifts", string.Join(",", filter._lstGiftsProdGift)));

            List<RptProductionByGiftInvitationSalesRoom> lstRptProductionByGiftInvitationSalesRoom = await BRReportsByLeadSource.GetRptProductionByGiftInvitationSalesRoom(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              string.Join(",", filter._lstGiftsProdGift),
             EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByGiftInvitationSalesRoom.Any())
              finfo = clsReports.ExportRptProductionByGiftInvitationSalesRoom(strReport, fileFullPath, filters, lstRptProductionByGiftInvitationSalesRoom);
            break;

          #endregion Production by Gift (Invitation) & Sales Room

          #region Production by Guest Status

          case "Production by Guest Status":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByGuestStatusOuthouse> lstRptProductionByGuestStatusOuthouse = await BRReportsByLeadSource.GetRptProductionByGuestStatusOuthouse(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdAll);
            if (lstRptProductionByGuestStatusOuthouse.Any())
              finfo = clsReports.ExportRptProductionByGuestStatusOuthouse(strReport, fileFullPath, filters, lstRptProductionByGuestStatusOuthouse);
            break;

          #endregion Production by Guest Status

          #region Production by Hotel
          case "Production by Hotel":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByHotel> lstRptProductionByHotel = await BRReportsByLeadSource.GetRptProductionByHotel(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByHotel.Any())
              finfo = clsReports.ExportRptProductionByHotel(strReport, fileFullPath, filters, lstRptProductionByHotel);
            break;
          #endregion

          #region Production by Hotel & Sales Room
          case "Production by Hotel & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByHotelSalesRoom> lstRptProductionByHotelSalesRoom = await BRReportsByLeadSource.GetRptProductionByHotelSalesRoom(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByHotelSalesRoom.Any())
              finfo = clsReports.ExportRptProductionByHotelSalesRoom(strReport, fileFullPath, filters, lstRptProductionByHotelSalesRoom);
            break;
          #endregion

          #region Production by Hotel Group
          case "Production by Hotel Group":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByHotelGroup> lstRptProductionByHotelGroup = await BRReportsByLeadSource.GetRptProductionByHotelGroup(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByHotelGroup.Any())
              finfo = clsReports.ExportRptProductionByHotelGroup(strReport, fileFullPath, filters, lstRptProductionByHotelGroup);
            break;
          #endregion

          #region Production by Hotel Group & Sales Room
          case "Production by Hotel Group & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByHotelGroupSalesRoom> lstRptProductionByHotelGroupSalesRoom = await BRReportsByLeadSource.GetRptProductionByHotelGroupSalesRoom(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByHotelGroupSalesRoom.Any())
              finfo = clsReports.ExportRptProductionByHotelGroupSalesRoom(strReport, fileFullPath, filters, lstRptProductionByHotelGroupSalesRoom);
            break;
          #endregion

          #region Production by Nationality

          case "Production by Nationality":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByNationalityOuthouse> lstRptProductionByNationalityOuthouse = await BRReportsByLeadSource.GetRptProductionByNationalityOuthouse(_dtmStart, _dtmEnd,
                string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit,
              _enumSaveCourtesyTours);
            if (lstRptProductionByNationalityOuthouse.Any())
              finfo = clsReports.ExportRptProductionByNationalityOuthouse(strReport, fileFullPath, filters, lstRptProductionByNationalityOuthouse);
            break;

          #endregion Production by Nationality

          #region Production by Nationality & Sales Room

          case "Production by Nationality & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByNationalitySalesRoomOuthouse> lstRptProductionByNationalitySalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByNationalitySalesRoomOuthouse(_dtmStart, _dtmEnd,
               string.Join(",", filter._lstLeadSources),
               "ALL",
               EnumProgram.All,
               EnumFilterDeposit.fdDepositShowsNoDeposit,
               _enumSaveCourtesyTours);
            if (lstRptProductionByNationalitySalesRoomOuthouse.Any())
              finfo = clsReports.ExportRptProductionByNationalitySalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByNationalitySalesRoomOuthouse);
            break;

          #endregion Production by Nationality & Sales Room

          #region Production by PR

          case "Production by PR":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByPROuthouse> lstRptProductionByPROuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
               string.Join(",", filter._lstLeadSources),
               "ALL",
               EnumProgram.All,
               EnumFilterDeposit.fdAll,
               _enumBasedOnBooking);
            if (lstRptProductionByPROuthouse.Any())
              finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPROuthouse);
            break;

          #endregion Production by PR

          #region Production by PR & Sales Room

          case "Production by PR & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
               string.Join(",", filter._lstLeadSources),
               "ALL",
               EnumProgram.All,
               EnumFilterDeposit.fdAll,
               _enumBasedOnBooking);
            if (lstRptProductionByPRSalesRoomOuthouse.Any())
              finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomOuthouse);
            break;

          #endregion Production by PR & Sales Room

          #region Production by PR & Sales Room (Deposits & Flyers Show)

          case "Production by PR & Sales Room (Deposits & Flyers Show)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomDepositsFlyersShow = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
               string.Join(",", filter._lstLeadSources),
               "ALL",
               EnumProgram.All,
               EnumFilterDeposit.fdDepositShowsNoDeposit,
               _enumBasedOnBooking);
            if (lstRptProductionByPRSalesRoomDepositsFlyersShow.Any())
              finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomDepositsFlyersShow);
            break;

          #endregion Production by PR & Sales Room (Deposits & Flyers Show)

          #region Production by PR & Sales Room (Deposits)

          case "Production by PR & Sales Room (Deposits)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomDeposits = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
               string.Join(",", filter._lstLeadSources),
               "ALL",
               EnumProgram.All,
               EnumFilterDeposit.fdDeposit,
               _enumBasedOnBooking);
            if (lstRptProductionByPRSalesRoomDeposits.Any())
              finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomDeposits);
            break;

          #endregion Production by PR & Sales Room (Deposits)

          #region Production by PR & Sales Room (Flyers)

          case "Production by PR & Sales Room (Flyers)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomFlyers = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
               string.Join(",", filter._lstLeadSources),
               "ALL",
               EnumProgram.All,
               EnumFilterDeposit.fdNoDeposit,
               _enumBasedOnBooking);
            if (lstRptProductionByPRSalesRoomFlyers.Any())
              finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomFlyers);
            break;

          #endregion Production by PR & Sales Room (Flyers)

          #region Production by PR (Deposits & Flyers Show)

          case "Production by PR (Deposits & Flyers Show)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByPROuthouse> lstRptProductionByPRDepositsFlyersShowOuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
               string.Join(",", filter._lstLeadSources),
               "ALL",
               EnumProgram.All,
               EnumFilterDeposit.fdDepositShowsNoDeposit,
               _enumBasedOnBooking);
            if (lstRptProductionByPRDepositsFlyersShowOuthouse.Any())
              finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPRDepositsFlyersShowOuthouse);
            break;

          #endregion Production by PR (Deposits & Flyers Show)

          #region Production by PR (Deposits)

          case "Production by PR (Deposits)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByPROuthouse> lstRptProductionByPRDepositsOuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
                 string.Join(",", filter._lstLeadSources),
                 "ALL",
                 EnumProgram.All,
                 EnumFilterDeposit.fdDeposit,
                 _enumBasedOnBooking);
            if (lstRptProductionByPRDepositsOuthouse.Any())
              finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPRDepositsOuthouse);
            break;

          #endregion Production by PR (Deposits)

          #region Production by PR (Flyers)

          case "Production by PR (Flyers)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByPROuthouse> lstRptProductionByPRFlyersOuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
                 string.Join(",", filter._lstLeadSources),
                 "ALL",
                 EnumProgram.All,
                 EnumFilterDeposit.fdNoDeposit,
                 _enumBasedOnBooking);
            if (lstRptProductionByPRFlyersOuthouse.Any())
              finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPRFlyersOuthouse);
            break;

          #endregion Production by PR (Flyers)

          #region Production by PR Contact

          case "Production by PR Contact":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByPRContactOuthouse> lstRptProductionByPRContactOuthouse = await BRReportsByLeadSource.GetProductionByPRContactOuthouse(_dtmStart, _dtmEnd,
                 string.Join(",", filter._lstLeadSources),
                 "ALL",
                 EnumProgram.All,
                 EnumFilterDeposit.fdAll);
            if (lstRptProductionByPRContactOuthouse.Any())
              finfo = clsReports.ExportRptProductionByPRContactOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRContactOuthouse);
            break;

          #endregion Production by PR Contact

          #region Production by Wave
          case "Production by Wave":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByWave> lstRptProductionByWave = await BRReportsByLeadSource.GetRptProductionByWave(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByWave.Any())
              finfo = clsReports.ExportRptProductionByWave(strReport, fileFullPath, filters, lstRptProductionByWave);
            break;
          #endregion

          #region Production by Wave & Sales Room
          case "Production by Wave & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptProductionByWaveSalesRoom> lstRptProductionByWaveSalesRoom = await BRReportsByLeadSource.GetRptProductionByWaveSalesRoom(_dtmStart, _dtmEnd,
              string.Join(",", filter._lstLeadSources),
              "ALL",
              EnumProgram.All,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByWaveSalesRoom.Any())
              finfo = clsReports.ExportRptProductionByWaveSalesRoom(strReport, fileFullPath, filters, lstRptProductionByWaveSalesRoom);
            break;
            #endregion
        }

        if (finfo == null)
        {
          finfo = ReportBuilder.CreateNoInfoRptExcel(filters, strReport, fileFullPath);          
        }
        frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(finfo, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly));
        frmDocumentViewver.Show();
        _frmReportQueue.SetExist(finfo.FullName, finfo);
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion ShowLeadSourceReport

    #region OpenFilterDateRangePR

    /// <summary>
    /// Abre la ventan frmFilterDateRange con los controles configurados
    /// segun el reporte seleccionado por PR
    /// </summary>
    /// <history>
    ///   [vku] 28/Mar/2016 Created
    /// </history>
    public void OpenFilterDateRangePR(string strReport)
    {
      _frmFilter = new frmFilterDateRange { frmPO = this, Owner = this };
      switch (strReport)
      {
        case "Production by Age":
        case "Production by Age & Sales Room":
        case "Production by Agency, Market & Hotel":
        case "Production by Couple Type":
        case "Production by Couple Type & Sales Room":
        case "Production by Guest Status":
        case "Production by Hotel":
        case "Production by Hotel & Sales Room":
        case "Production by Hotel Group":
        case "Production by Hotel Group & Sales Room":
        case "Production by Wave":
        case "Production by Wave & Sales Room":
        case "Production by Nationality":
        case "Production by Nationality & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true);
          break;

        case "Production by Gift (Invitation)":
        case "Production by Gift (Invitation) & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true, blnGiftProdGift: true, blnAllGiftProdGift: true);
          break;

        case "Production by Agency":
        case "Production by Agency & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true, enumSalesByMemberShipType: EnumSalesByMemberShipType.Detail);
          break;


        case "Production by PR & Sales Room (Deposits & Flyers Show)":
        case "Production by PR (Deposits & Flyers Show)":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true, enumBasedOnBooking: EnumBasedOnBooking.BasedOnBooking);
          break;
      }

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowByPRReport(strReport, _clsFilter);
        _frmFilter.Close();
      }
      else
      {
        _frmFilter.Close();
        _frmFilter = null;
      }
    }

    #endregion OpenFilterDateRangePR

    #region ShowByPRReport

    /// <summary>
    ///  Muestra un reporte por PR
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    ///   [vku] 30/May/2016 Modified. Ahora el metodo es asincrono
    /// </history>
    public async void ShowByPRReport(string strReport, clsFilter filter)
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_dtmStart, _dtmStart) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);
      DataTable dtData = new DataTable();
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      string fileFullPath = ReportBuilder.CreateEmptyExcel(strReport, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, strReport);
      try
      {
        switch (strReport)
        {
          #region Production by Age

          case "Production by Age":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByAgeOuthouse> lstRptProductionByAgeOuthouse = await BRReportsByLeadSource.GetRptProductionByAge(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByAgeOuthouse.Any())
              finfo = clsReports.ExportRptProductionByAgeOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgeOuthouse);
            break;

          #endregion Production by Age

          #region Production by Age & Sales Room
          case "Production by Age & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByAgeSalesRoomOuthouse> lstRptProductionByAgeSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByAgeSalesRoomOuthouse(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByAgeSalesRoomOuthouse.Any())
              finfo = clsReports.ExportRptProductionByAgeSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgeSalesRoomOuthouse);
            break;
          #endregion

          #region Production by Agency

          case "Production by Agency":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PRs", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            ProductionByAgencyOuthouseData lstRptProductionByAgencyOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencyOuthouseData(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit,
              _enumSalesByMemberShipType);
            if (lstRptProductionByAgencyOuthouse.ProductionByAgencyOuthouse.Any())
              finfo = await clsReports.ExportRptProductionByAgencyOuhouse(strReport, fileFullPath, filters, lstRptProductionByAgencyOuthouse, _enumSalesByMemberShipType);
            break;

          #endregion Production by Agency

          #region Production by Agency & Sales Room

          case "Production by Agency & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PRs", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            ProductionByAgencySalesRoomOuthouseData lstRptProductionByAgencySalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencySalesRoomOuthouseData(_dtmStart, _dtmEnd,
              "ALL",
             string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit,
              _enumSalesByMemberShipType);
            if (lstRptProductionByAgencySalesRoomOuthouse.ProductionByAgencySalesRoomOuthouse.Any())
              finfo = await clsReports.ExportRptProductionByAgencySalesRoomOuhouse(strReport, fileFullPath, filters, lstRptProductionByAgencySalesRoomOuthouse, _enumSalesByMemberShipType);
            break;

          #endregion Production by Agency & Sales Room

          #region Production by Agency, Market & Hotel
          case "Production by Agency, Market & Hotel":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByAgencyMarketHotelOuthouse> lstRptProductionByAgencyMarketHotelOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencyMarketHotelOuthouse(_dtmStart, _dtmEnd,
              "ALL",
               string.Join(",", filter._lstPRs),
               EnumProgram.Outhouse,
               EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByAgencyMarketHotelOuthouse.Any())
              finfo = clsReports.ExportRptProductionByAgencyMarketHotelOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgencyMarketHotelOuthouse);
            break;
          #endregion

          #region Production by Couple Type
          case "Production by Couple Type":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByCoupleTypeOuthouse> lstRptProductionByCoupleTypeOuthouse = await BRReportsByLeadSource.GetRptProductionByCoupleTypeOuthouse(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByCoupleTypeOuthouse.Any())
              finfo = clsReports.ExportRptProductionByCoupleTypeOuthouse(strReport, fileFullPath, filters, lstRptProductionByCoupleTypeOuthouse);
            break;
          #endregion

          #region Production by Couple Type & Sales Room
          case "Production by Couple Type & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByCoupleTypeSalesRoomOuthouse> lstRptProductionByCoupleTypeSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByCoupleTypeSalesRoomOuthouse(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByCoupleTypeSalesRoomOuthouse.Any())
              finfo = clsReports.ExportRptProductionByCoupleTypeSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByCoupleTypeSalesRoomOuthouse);
            break;
          #endregion

          #region Production by Gift (Invitation)
          case "Production by Gift (Invitation)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));
            filters.Add(new Tuple<string, string>("Gifts", string.Join(",", filter._lstGiftsProdGift)));

            List<RptProductionByGiftInvitation> lstRptProductionByGiftInvitation = await BRReportsByLeadSource.GetRptProductionByGiftInvitation(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              string.Join(",", filter._lstGiftsProdGift),
             EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByGiftInvitation.Any())
              finfo = clsReports.ExportRptProductionByGiftInvitation(strReport, fileFullPath, filters, lstRptProductionByGiftInvitation);
            break;
          #endregion

          #region Production by Gift (Invitation) & Sales Room
          case "Production by Gift (Invitation) & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));
            filters.Add(new Tuple<string, string>("Gifts", string.Join(",", filter._lstGiftsProdGift)));

            List<RptProductionByGiftInvitationSalesRoom> lstRptProductionByGiftInvitationSalesRoom = await BRReportsByLeadSource.GetRptProductionByGiftInvitationSalesRoom(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              string.Join(",", filter._lstGiftsProdGift),
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByGiftInvitationSalesRoom.Any())
              finfo = clsReports.ExportRptProductionByGiftInvitationSalesRoom(strReport, fileFullPath, filters, lstRptProductionByGiftInvitationSalesRoom);
            break;
          #endregion

          #region Production by Guest Status
          case "Production by Guest Status":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByGuestStatusOuthouse> lstRptProductionByGuestStatusOuthouse = await BRReportsByLeadSource.GetRptProductionByGuestStatusOuthouse(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdAll);
            if (lstRptProductionByGuestStatusOuthouse.Any())
              finfo = clsReports.ExportRptProductionByGuestStatusOuthouse(strReport, fileFullPath, filters, lstRptProductionByGuestStatusOuthouse);
            break;
          #endregion

          #region Production by Hotel
          case "Production by Hotel":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByHotel> lstRptProductionByHotel = await BRReportsByLeadSource.GetRptProductionByHotel(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByHotel.Any())
              finfo = clsReports.ExportRptProductionByHotel(strReport, fileFullPath, filters, lstRptProductionByHotel);
            break;
          #endregion

          #region Production by Hotel & Sales Room
          case "Production by Hotel & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByHotelSalesRoom> lstRptProductionByHotelSalesRoom = await BRReportsByLeadSource.GetRptProductionByHotelSalesRoom(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByHotelSalesRoom.Any())
              finfo = clsReports.ExportRptProductionByHotelSalesRoom(strReport, fileFullPath, filters, lstRptProductionByHotelSalesRoom);
            break;
          #endregion

          #region Production by Hotel Group
          case "Production by Hotel Group":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByHotelGroup> lstRptProductionByHotelGroup = await BRReportsByLeadSource.GetRptProductionByHotelGroup(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByHotelGroup.Any())
              finfo = clsReports.ExportRptProductionByHotelGroup(strReport, fileFullPath, filters, lstRptProductionByHotelGroup);
            break;
          #endregion

          #region Production by Hotel Group & Sales Room
          case "Production by Hotel Group & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByHotelGroupSalesRoom> lstRptProductionByHotelGroupSalesRoom = await BRReportsByLeadSource.GetRptProductionByHotelGroupSalesRoom(_dtmStart, _dtmEnd,
               "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByHotelGroupSalesRoom.Any())
              finfo = clsReports.ExportRptProductionByHotelGroupSalesRoom(strReport, fileFullPath, filters, lstRptProductionByHotelGroupSalesRoom);
            break;
          #endregion

          #region Production by Nationality
          case "Production by Nationality":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByNationalityOuthouse> lstRptProductionByNationalityOuthouse = await BRReportsByLeadSource.GetRptProductionByNationalityOuthouse(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByNationalityOuthouse.Any())
              finfo = clsReports.ExportRptProductionByNationalityOuthouse(strReport, fileFullPath, filters, lstRptProductionByNationalityOuthouse);
            break;
          #endregion

          #region Production by Nationality & Sales Room
          case "Production by Nationality & Sales Room":

            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByNationalitySalesRoomOuthouse> lstRptProductionByNationalitySalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByNationalitySalesRoomOuthouse(_dtmEnd, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByNationalitySalesRoomOuthouse.Any())
              finfo = clsReports.ExportRptProductionByNationalitySalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByNationalitySalesRoomOuthouse);
            break;
          #endregion

          #region Production by PR & Sales Room (Deposits & Flyers Show)
          case "Production by PR & Sales Room (Deposits & Flyers Show)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByPRSalesRoomOuthouse.Any())
              finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomOuthouse);
            break;
          #endregion

          #region Production by PR (Deposits & Flyers Show)
          case "Production by PR (Deposits & Flyers Show)":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByPROuthouse> lstRptProductionByPROuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
              "ALL",
              string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByPROuthouse.Any())
              finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPROuthouse);
            break;
          #endregion

          #region Production by Wave
          case "Production by Wave":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByWave> lstRptProductionByWave = await BRReportsByLeadSource.GetRptProductionByWave(_dtmStart, _dtmEnd,
              "ALL",
              filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByWave.Any())
              finfo = clsReports.ExportRptProductionByWave(strReport, fileFullPath, filters, lstRptProductionByWave);
            break;
          #endregion

          #region Production by Wave & Sales Room
          case "Production by Wave & Sales Room":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs)));

            List<RptProductionByWaveSalesRoom> lstRptProductionByWaveSalesRoom = await BRReportsByLeadSource.GetRptProductionByWaveSalesRoom(_dtmStart, _dtmEnd,
              "ALL",
             filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs),
              EnumProgram.Outhouse,
              EnumFilterDeposit.fdDepositShowsNoDeposit);
            if (lstRptProductionByWaveSalesRoom.Any())
              finfo = clsReports.ExportRptProductionByWaveSalesRoom(strReport, fileFullPath, filters, lstRptProductionByWaveSalesRoom);
            break;
            #endregion
        }
        if (finfo == null)
        {
          finfo = ReportBuilder.CreateNoInfoRptExcel(filters, strReport, fileFullPath);
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

    #endregion ShowByPRReport

    #region OpenFIlterDateRangeOtherReport

    /// <summary>
    /// Abre la ventan frmFilterDateRange con los controles configurados
    /// segun el reporte seleccionado por LeadSource
    /// </summary>
    /// <history>
    ///   [vku] 28/Mar/2016 Created
    /// </history>
    public void OpenFilterDateRangeOtherReport(string strReport)
    {
      _frmFilter = _frmFilter = new frmFilterDateRange { frmPO = this, Owner = this };

      switch (strReport)
      {
        case "Folios Invitations Outhouse":
        case "Folios Invitations Outhouse by PR":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, blnPRs: true, blnUseDates: true, blnChkUsedate: true, blnFolSeries: true, blnFolFrom: true, blnFolTo: true, enumPrograms: EnumProgram.All);
          break;

        case "Folios CxC by PR":
        case "Folios CXC":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, blnPRs: true, blnUseDates: true, blnChkUsedate: true, blnFolFrom: true, blnFolTo: true, blnAllFolios: true, enumPrograms: EnumProgram.All);
          break;
      }

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowOtherReport(strReport, _clsFilter);
        _frmFilter.Close();
      }
      else
      {
        _frmFilter.Close();
        _frmFilter = null;
      }
    }

    #endregion OpenFIlterDateRangeOtherReport

    #region ShowOtherReport

    /// <summary>
    ///  Muestra otros reportes
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    ///   [vku] 30/May/2016 Modified. Ahora el metodo es asincrono
    /// </history>
    public async void ShowOtherReport(string strReport, clsFilter filter)
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_dtmStart, _dtmStart) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);
      DataTable dtData = new DataTable();
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      string fileFullPath = ReportBuilder.CreateEmptyExcel(strReport, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, strReport);
      try
      {
        switch (strReport)
        {
          #region Folios Invitations Outhouse
          case "Folios Invitations Outhouse":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptFoliosInvitationByDateFolio> lstRptFoliosInvitationByDateFolio = await BRReportsByLeadSource.GetRptFoliosInvitationByDateFolio(_frmFilter.chkUseDates.IsChecked.Value ? _dtmStart : (DateTime?)null, _frmFilter.chkUseDates.IsChecked.Value ? _dtmEnd : (DateTime?)null,
              _cboFolSeriesSelected,
              _folFrom,
              _folTo,
              string.Join(",", filter._lstLeadSources),
              string.Join(",", filter._lstPRs)
              );
            if (lstRptFoliosInvitationByDateFolio.Any())
              finfo = await clsReports.ExportRptFoliosInvitationByDateFolio(strReport, fileFullPath, filters, lstRptFoliosInvitationByDateFolio);
            break;
          #endregion

          #region Folios Invitations Outhouse by PR
          case "Folios Invitations Outhouse by PR":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptFoliosInvitationsOuthouseByPR> lstRptFoliosInvitationsOuthouseByPR = await BRReportsByLeadSource.GetRptFoliosInvitationsOuthouseByPR(_frmFilter.chkUseDates.IsChecked.Value ? _dtmStart : (DateTime?)null, _frmFilter.chkUseDates.IsChecked.Value ? _dtmEnd : (DateTime?)null,
              _cboFolSeriesSelected,
              _folFrom,
              _folTo,
              string.Join(",", filter._lstLeadSources),
              string.Join(",", filter._lstPRs)
              );
            if (lstRptFoliosInvitationsOuthouseByPR.Any())
              finfo = clsReports.ExportRptFoliosInvitationOuthouseByPR(strReport, fileFullPath, filters, lstRptFoliosInvitationsOuthouseByPR);
            break;
          #endregion

          #region Folios CxC by PR
          case "Folios CxC by PR":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptFoliosCxCByPR> lstRptFoliosCxCByPR = await BRReportsByLeadSource.GetRptFoliosCxCByPR(_frmFilter.chkUseDates.IsChecked.Value ? _dtmStart : (DateTime?)null, _frmFilter.chkUseDates.IsChecked.Value ? _dtmEnd : (DateTime?)null,
              _frmFilter.chkAllFolios.IsChecked.Value,
              _folFrom,
              _folTo,
              string.Join(",", filter._lstLeadSources),
              filter.AllPRs ? "ALL" : string.Join(",", filter._lstPRs));
            if (lstRptFoliosCxCByPR.Any())
              finfo = clsReports.ExportRptFoliosCxCByPR(strReport, fileFullPath, filters, lstRptFoliosCxCByPR);
            break;
          #endregion

          #region Folios CXC
          case "Folios CXC":
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", filter.AllLeadSources ? "ALL" : string.Join(",", filter._lstLeadSources)));

            List<RptFoliosCXC> lstRptFoliosCXC = await BRReportsByLeadSource.GetRptFoliosCXC(_frmFilter.chkUseDates.IsChecked.Value ? _dtmStart : (DateTime?)null, _frmFilter.chkUseDates.IsChecked.Value ? _dtmEnd : (DateTime?)null,
              _frmFilter.chkAllFolios.IsChecked.Value,
              _folFrom,
              _folTo,
              string.Join(",", filter._lstLeadSources),
              string.Join(",", filter._lstPRs));
            if (lstRptFoliosCXC.Any())
              finfo = await clsReports.ExportRptFoliosCXC(strReport, fileFullPath, filters, lstRptFoliosCXC);
            break;
            #endregion
        }
        if (finfo == null)
        {
          finfo = ReportBuilder.CreateNoInfoRptExcel(filters, strReport, fileFullPath);
          frmDocumentViewer frmDocumentViewver = new frmDocumentViewer(finfo, Context.User.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly));
          frmDocumentViewver.Show();
        }        
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex);
      }
    }

    #endregion ShowOtherReport

    #region WaitMessage

    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <history>
    ///   [vku] 28/Mar/2016 Created
    /// </history>
    private void WaitMessage(bool show, String message = "")
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = (show) ? Visibility.Visible : Visibility.Hidden;
      this.Cursor = (show) ? Cursors.Wait : null;
      UIHelper.ForceUIToUpdate();
    }

    #endregion WaitMessage

    #endregion Metodos

    #region eventos

    #region Window_keyDown
    /// <summary>
    ///   Verifica si los botones estan activos
    /// </summary>
    /// <history>
    ///   [vku] 10/May/2016 Created
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
    ///  Verifica si los botones estan activos
    /// </summary>
    /// <history>
    ///   [vku] 10/May/2016 Created
    /// </history>
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }
    #endregion

    #region Window_Closing
    /// <summary>
    ///   Cambia la propiedad CloseAllowed de la ventana frmFilterDateRange
    ///   para permitir el cierre de la misma.
    /// </summary>
    /// <history>
    ///   [vku] 10/May/2016 Created
    /// </history>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      _frmFilter?.Close();
      Application.Current.Shutdown();
    }
    #endregion

    #region btnPrint_Click
    /// <summary>
    ///   Abre la ventana de filtros
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 16/Jun/2016 Created
    ///   [vku] 17/Jun/2016 Modified. Ahora abre la ventana de filtros si se configura la ruta
    /// </history>
    private void btnPrint_Click(object sender, RoutedEventArgs e)
    {
      if (sender.Equals(btnPrintRptByLeadSource)) PrepareReportByLeadSource();
      else if (sender.Equals(btnPrintRptByPR)) PrepareReportByPR();
      else if (sender.Equals(btnPrintOtherRpts)) PrepareOtherReports();
    }
    #endregion

    #region btnExit_Click

    /// <summary>
    /// Cierra la aplicacion Processor Outhouse
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      App.Current.Shutdown();
    }

    #endregion btnExit_Click

    #region btnReportQueue_Click
    /// <summary>
    ///   Metodo para abrir la cola de reportes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 07/Jun/2016 Created
    /// </history>
    private void btnReportQueue_Click(object sender, RoutedEventArgs e)
    {
      _frmReportQueue.Show();
      if (_frmReportQueue.WindowState == WindowState.Minimized)
        _frmReportQueue.WindowState = WindowState.Normal;
      _frmReportQueue.Activate();
    }
    #endregion

    #region grdrpt_MouseDoubleClick
    /// <summary>
    ///    Método para abrir la ventana de filtros  al hacer doble clic sobre alguno registro de un Grid
    /// </summary>
    /// <history>
    ///   [vku] 10/May/2016 Created
    ///   [vku] 10/Jun/2016 Modified. Ahora verifica que este configurado la ruta para guardar el reporte
    ///   [vku] 17/Jun/2016 Modified. Ahora abre la ventana de filtros si se configura la ruta
    /// </history>
    private void grdrpt_MouseDoubleClick(object sender, RoutedEventArgs e)
    {
      var _dataGridRow = (DataGridRow)sender;
      if (_dataGridRow.Item.Equals(grdRptsByLeadSource.CurrentItem)) PrepareReportByLeadSource();
      else if (_dataGridRow.Item.Equals(grdRptsByPR.CurrentItem)) PrepareReportByPR();
      else if (_dataGridRow.Item.Equals(grdOtherRpts.CurrentItem)) PrepareOtherReports();
    }
    #endregion

    #region grdrp_PreviewKeyDown
    /// <summary>
    ///   Método para abrir la ventana de filtros  al hacer Enter sobre alguno registro de un Grid
    /// </summary>
    /// <history>
    ///   [vku] 10/May/2016 Created
    /// </history>
    private void grdrp_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter) return;
      var _dataGridRow = (DataGridRow)sender;
      if (_dataGridRow.Item.Equals(grdRptsByLeadSource.CurrentItem)) PrepareReportByLeadSource();
      else if (_dataGridRow.Item.Equals(grdRptsByPR.CurrentItem)) PrepareReportByPR();
      else if (_dataGridRow.Item.Equals(grdOtherRpts.CurrentItem)) PrepareOtherReports();
    }
    #endregion

    #endregion eventos

  }
}