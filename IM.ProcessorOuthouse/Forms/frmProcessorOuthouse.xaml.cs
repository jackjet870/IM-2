using IM.Base.Helpers;
using IM.Base.Forms;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.ProcessorOuthouse.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IM.Model.Classes;

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
      GetFirstDayValue();
      lblUserName.Content = App.User.User.peN;
      _frmReportQueue = new frmReportQueue();
    }
    #endregion Constructor

    #region Atributos

    private frmFilterDateRange _frmFilter;
    private SystemCfg _SystemConfig;
    private frmReportQueue _frmReportQueue;
    private bool _blnOneDate;
    private bool _blnOnlyOneRegister;

    public List<string> _lstLeadSources = new List<string>();
    public List<int> _lstLeadSourcesPaymentComm = new List<int>();
    public List<int> _lstPaymentTypes = new List<int>();
    public List<int> _lstPRs = new List<int>();
    public List<int> _lstChargeTo = new List<int>();
    public List<int> _lstGifts = new List<int>();
    public List<int> _lstGiftsProdGift = new List<int>();
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
        new {rptName="Folios Invitations Outhouse"},
        new {rptName="Folios Invitations Outhouse by PR"},
        new {rptName="Folios CxC by PR"},
        new {rptName="Folios CXC"}
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

      switch (strReport)
      {
        case "Folios Invitations Outhouse":
        case "Folios Invitations Outhouse by PR":
        case "Folios CXC by PR":
        case "Folios CXC":
          break;
      }
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
      _frmFilter = new frmFilterDateRange();
      _frmFilter.frmPO = this;
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
        ShowLeadSourceReport(strReport);
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
    public async void ShowLeadSourceReport(string strReport)
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_dtmStart, _dtmEnd) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);
      DataTable dtData = new DataTable();
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      string fileFullPath = EpplusHelper.CreateEmptyExcel(strReport, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, strReport);

      switch (strReport)
      {
        #region Deposits Payment By PR

        case "Deposits Payment by PR":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));

          DepositsPaymentByPRData lstRptDepositsPaymentByPR = await BRReportsByLeadSource.GetRptDepositsPaymentByPRData(
          _dtmStart, _dtmEnd,
          string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
          "ALL",
          EnumProgram.Outhouse,
          string.Join(",", _frmFilter.grdPaymentTypes.SelectedItems.Cast<PaymentType>().Select(c => c.ptID).ToList()),
          EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptDepositsPaymentByPR != null) 
            finfo = clsReports.ExportRptDepositsPaymentByPR(strReport, fileFullPath, filters, lstRptDepositsPaymentByPR);
          break;

        #endregion Deposits Payment By PR

        #region Gifts Received by Sales Room

        case "Gifts Received by Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));
          filters.Add(new Tuple<string, string>("Charge To", _frmFilter.grdChargeTo.SelectedItems.Count == _frmFilter.grdChargeTo.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdChargeTo.SelectedItems.Cast<ChargeTo>().Select(c => c.ctID).ToList())));
          filters.Add(new Tuple<string, string>("Gifts", _frmFilter.grdGifts.SelectedItems.Count == _frmFilter.grdGifts.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdGifts.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList())));

          GiftsReceivedBySRData lstRptGiftsReceivedBySR = await BRReportsByLeadSource.GetRptGiftsReceivedBySRData(
          _dtmStart, _dtmEnd,
          string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
          _frmFilter.chkAllChargeTo.IsChecked.Value ? "ALL" : string.Join(",", _frmFilter.grdChargeTo.SelectedItems.Cast<ChargeTo>().Select(c => c.ctID).ToList()),
          _frmFilter.chkAllGifts.IsChecked.Value ? "ALL" : string.Join(",", _frmFilter.grdGifts.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList()));
          if (lstRptGiftsReceivedBySR != null)
            finfo = clsReports.ExportRptGiftsReceivedBySR(strReport, fileFullPath, filters, lstRptGiftsReceivedBySR);
          break;

        #endregion Gifts Received by Sales Room

        #region Guests Show No Presented Invitation

        case "Guests Show No Presented Invitation":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));

          List<GuestShowNoPresentedInvitation> lstRptGuestsShowNoPresentedInvitation = await BRReportsByLeadSource.GetRptGuestsShowNoPresentedInvitation(
            _dtmStart, _dtmEnd, 
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID)));
          if (lstRptGuestsShowNoPresentedInvitation.Count > 0)
            finfo = clsReports.ExportRptGuestsShowNoPresentedInvitation(strReport, fileFullPath, filters, lstRptGuestsShowNoPresentedInvitation);
          break;

        #endregion Guests Show No Presented Invitation

        #region PR Payment Commissions

        case "PR Payment Commissions":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", string.Join(",", _frmFilter.grdLeadSourcesPaymentComm.SelectedItems.Cast<LeadSource>().Select(c => c.lsID).ToList())));

          List<RptProductionByPROuthouse> lstRptProductionByPROuthouseComm = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSourcesPaymentComm.SelectedItems.Cast<LeadSource>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit,
            _enumBasedOnBooking);
          if (lstRptProductionByPROuthouseComm.Any())
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPROuthouseComm);
          break;

        #endregion PR Payment Commissions

        #region Production by Age

        case "Production by Age":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));

          List<RptProductionByAgeOuthouse> lstRptProductionByAgeOuthouse = await BRReportsByLeadSource.GetRptProductionByAge(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgeOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByAgeOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgeOuthouse);
          break;

        #endregion Production by Age

        #region Production by Age & Sales Room

        case "Production by Age & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByAgeSalesRoomOuthouse> lstRptProductionByAgeSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByAgeSalesRoomOuthouse(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgeSalesRoomOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByAgeSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgeSalesRoomOuthouse);
          break;

        #endregion Production by Age & Sales Room

        #region Production by Agency

        case "Production by Agency":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          ProductionByAgencyOuthouseData lstRptProductionByAgencyOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencyOuthouseData(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit,
            _enumSalesByMemberShipType);
          if (lstRptProductionByAgencyOuthouse != null)
            finfo = clsReports.ExportRptProductionByAgencyOuhouse(strReport, fileFullPath, filters, lstRptProductionByAgencyOuthouse, _enumSalesByMemberShipType);
          break;

        #endregion Production by Agency

        #region Production by Agency & Sales Room

        case "Production by Agency & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          ProductionByAgencySalesRoomOuthouseData lstRptProductionByAgencySalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencySalesRoomOuthouseData(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgencySalesRoomOuthouse != null)
            finfo = clsReports.ExportRptProductionByAgencySalesRoomOuhouse(strReport, fileFullPath, filters, lstRptProductionByAgencySalesRoomOuthouse);
          break;

        #endregion Production by Agency & Sales Room

        #region Production by Agency, Market & Hotel

        case "Production by Agency, Market & Hotel":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByAgencyMarketHotelOuthouse> lstRptProductionByAgencyMarketHotelOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencyMarketHotelOuthouse(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgencyMarketHotelOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByAgencyMarketHotelOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgencyMarketHotelOuthouse);
          break;

        #endregion Production by Agency, Market & Hotel

        #region Production by Couple Type

        case "Production by Couple Type":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByCoupleTypeOuthouse> lstRptProductionByCoupleTypeOuthouse = await BRReportsByLeadSource.GetRptProductionByCoupleTypeOuthouse(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByCoupleTypeOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByCoupleTypeOuthouse(strReport, fileFullPath, filters, lstRptProductionByCoupleTypeOuthouse);
          break;

        #endregion Production by Couple Type

        #region Production by Couple Type & Sales Room

        case "Production by Couple Type & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByCoupleTypeSalesRoomOuthouse> lstRptProductionByCoupleTypeSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByCoupleTypeSalesRoomOuthouse(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByCoupleTypeSalesRoomOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByCoupleTypeSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByCoupleTypeSalesRoomOuthouse);
          break;

        #endregion Production by Couple Type & Sales Room

        #region Production by Flight & Sales Room
        case "Production by Flight & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByFlightSalesRoom> lstRptProductionByFlightSalesRoom = await BRReportsByLeadSource.GetRptProductionByFlightSalesRoom(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByFlightSalesRoom.Count > 0)
            finfo = clsReports.ExportRptProductionByFlightSalesRoom(strReport, fileFullPath, filters, lstRptProductionByFlightSalesRoom);
          break;
        #endregion

        #region Production by Gift (Invitation)

        case "Production by Gift (Invitation)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
          filters.Add(new Tuple<string, string>("Gifts", string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID))));

          List<RptProductionByGiftInvitation> lstRptProductionByGiftInvitation = await BRReportsByLeadSource.GetRptProductionByGiftInvitation(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList()),
           EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByGiftInvitation.Count > 0)
            finfo = clsReports.ExportRptProductionByGiftInvitation(strReport, fileFullPath, filters, lstRptProductionByGiftInvitation);
          break;

        #endregion Production by Gift (Invitation)

        #region Production by Gift (Invitation) & Sales Room

        case "Production by Gift (Invitation) & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
          filters.Add(new Tuple<string, string>("Gifts", string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID))));

          List<RptProductionByGiftInvitationSalesRoom> lstRptProductionByGiftInvitationSalesRoom = await BRReportsByLeadSource.GetRptProductionByGiftInvitationSalesRoom(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList()),
           EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByGiftInvitationSalesRoom.Count > 0)
            finfo = clsReports.ExportRptProductionByGiftInvitationSalesRoom(strReport, fileFullPath, filters, lstRptProductionByGiftInvitationSalesRoom);
          break;

        #endregion Production by Gift (Invitation) & Sales Room

        #region Production by Guest Status

        case "Production by Guest Status":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByGuestStatusOuthouse> lstRptProductionByGuestStatusOuthouse = await BRReportsByLeadSource.GetRptProductionByGuestStatusOuthouse(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdAll);
          if (lstRptProductionByGuestStatusOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByGuestStatusOuthouse(strReport, fileFullPath, filters, lstRptProductionByGuestStatusOuthouse);
          break;

        #endregion Production by Guest Status

        #region Production by Hotel
        case "Production by Hotel":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByHotel> lstRptProductionByHotel = await BRReportsByLeadSource.GetRptProductionByHotel(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByHotel.Count > 0)
            finfo = clsReports.ExportRptProductionByHotel(strReport, fileFullPath, filters, lstRptProductionByHotel);
          break;
        #endregion

        #region Production by Hotel & Sales Room
        case "Production by Hotel & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByHotelSalesRoom> lstRptProductionByHotelSalesRoom = await BRReportsByLeadSource.GetRptProductionByHotelSalesRoom(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByHotelSalesRoom.Count > 0)
            finfo = clsReports.ExportRptProductionByHotelSalesRoom(strReport, fileFullPath, filters, lstRptProductionByHotelSalesRoom);
          break;
        #endregion

        #region Production by Hotel Group
        case "Production by Hotel Group":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByHotelGroup> lstRptProductionByHotelGroup = await BRReportsByLeadSource.GetRptProductionByHotelGroup(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByHotelGroup.Count > 0)
            finfo = clsReports.ExportRptProductionByHotelGroup(strReport, fileFullPath, filters, lstRptProductionByHotelGroup);
          break;
        #endregion

        #region Production by Hotel Group & Sales Room
        case "Production by Hotel Group & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByHotelGroupSalesRoom> lstRptProductionByHotelGroupSalesRoom = await BRReportsByLeadSource.GetRptProductionByHotelGroupSalesRoom(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if(lstRptProductionByHotelGroupSalesRoom.Count > 0)
            finfo = clsReports.ExportRptProductionByHotelGroupSalesRoom(strReport, fileFullPath, filters, lstRptProductionByHotelGroupSalesRoom);
          break;
        #endregion

        #region Production by Nationality

        case "Production by Nationality":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByNationalityOuthouse> lstRptProductionByNationalityOuthouse = await BRReportsByLeadSource.GetRptProductionByNationalityOuthouse(_dtmStart, _dtmEnd,
              string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit,
            _enumSaveCourtesyTours);
          if (lstRptProductionByNationalityOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByNationalityOuthouse(strReport, fileFullPath, filters, lstRptProductionByNationalityOuthouse);
          break;

        #endregion Production by Nationality

        #region Production by Nationality & Sales Room

        case "Production by Nationality & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByNationalitySalesRoomOuthouse> lstRptProductionByNationalitySalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByNationalitySalesRoomOuthouse(_dtmStart, _dtmEnd,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdDepositShowsNoDeposit,
             _enumSaveCourtesyTours);
          if (lstRptProductionByNationalitySalesRoomOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByNationalitySalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByNationalitySalesRoomOuthouse);
          break;

        #endregion Production by Nationality & Sales Room

        #region Production by PR

        case "Production by PR":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByPROuthouse> lstRptProductionByPROuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdAll,
             _enumBasedOnBooking);
          if (lstRptProductionByPROuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPROuthouse);
          break;

        #endregion Production by PR

        #region Production by PR & Sales Room

        case "Production by PR & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdAll,
             _enumBasedOnBooking);
          if (lstRptProductionByPRSalesRoomOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomOuthouse);
          break;

        #endregion Production by PR & Sales Room

        #region Production by PR & Sales Room (Deposits & Flyers Show)

        case "Production by PR & Sales Room (Deposits & Flyers Show)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomDepositsFlyersShow = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdDepositShowsNoDeposit,
             _enumBasedOnBooking);
          if (lstRptProductionByPRSalesRoomDepositsFlyersShow.Count > 0)
            finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomDepositsFlyersShow);
          break;

        #endregion Production by PR & Sales Room (Deposits & Flyers Show)

        #region Production by PR & Sales Room (Deposits)

        case "Production by PR Sales Room (Deposits)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomDeposits = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdDeposit,
             _enumBasedOnBooking);
          if (lstRptProductionByPRSalesRoomDeposits.Count > 0)
            finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomDeposits);
          break;

        #endregion Production by PR & Sales Room (Deposits)

        #region Production by PR & Sales Room (Flyers)

        case "Production by PR & Sales Room (Flyers)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomFlyers = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdNoDeposit,
             _enumBasedOnBooking);
          if (lstRptProductionByPRSalesRoomFlyers.Count > 0)
            finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomFlyers);
          break;

        #endregion Production by PR & Sales Room (Flyers)

        #region Production by PR (Deposits & Flyers Show)

        case "Production by PR (Deposits & Flyers Show)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByPROuthouse> lstRptProductionByPRDepositsFlyersShowOuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdDepositShowsNoDeposit,
             _enumBasedOnBooking);
          if (lstRptProductionByPRDepositsFlyersShowOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPRDepositsFlyersShowOuthouse);
          break;

        #endregion Production by PR (Deposits & Flyers Show)

        #region Production by PR (Deposits)

        case "Production by PR (Deposits)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByPROuthouse> lstRptProductionByPRDepositsOuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
               string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
               "ALL",
               EnumProgram.Outhouse,
               EnumFilterDeposit.fdDeposit,
               _enumBasedOnBooking);
          if (lstRptProductionByPRDepositsOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPRDepositsOuthouse);
          break;

        #endregion Production by PR (Deposits)

        #region Production by PR (Flyers)

        case "Production by PR (Flyers)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByPROuthouse> lstRptProductionByPRFlyersOuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
               string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
               "ALL",
               EnumProgram.Outhouse,
               EnumFilterDeposit.fdNoDeposit,
               _enumBasedOnBooking);
          if (lstRptProductionByPRFlyersOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPRFlyersOuthouse);
          break;

        #endregion Production by PR (Flyers)

        #region Production by PR Contact

        case "Production by PR Contact":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByPRContactOuthouse> lstRptProductionByPRContactOuthouse = await BRReportsByLeadSource.GetProductionByPRContactOuthouse(_dtmStart, _dtmEnd,
               string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
               "ALL",
               EnumProgram.Outhouse,
               EnumFilterDeposit.fdAll);
          if (lstRptProductionByPRContactOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByPRContactOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRContactOuthouse);
          break;

        #endregion Production by PR Contact

        #region Production by Wave
        case "Production by Wave":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByWave> lstRptProductionByWave = await BRReportsByLeadSource.GetRptProductionByWave(_dtmStart, _dtmEnd,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByWave.Count > 0)
            finfo = clsReports.ExportRptProductionByWave(strReport, fileFullPath, filters, lstRptProductionByWave);
          break;
        #endregion

        #region Production by Wave & Sales Room
        case "Production by Wave & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));

          List<RptProductionByWaveSalesRoom> lstRptProductionByWaveSalesRoom = await BRReportsByLeadSource.GetRptProductionByWaveSalesRoom(_dtmStart, _dtmEnd,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByWaveSalesRoom.Count > 0)
            finfo = clsReports.ExportRptProductionByWaveSalesRoom(strReport, fileFullPath, filters, lstRptProductionByWaveSalesRoom);
          break;
          #endregion
      }

      if (finfo == null)
      {
        finfo = EpplusHelper.CreateNoInfoRptExcel(filters, strReport, fileFullPath);
      }
      _frmReportQueue.SetFileInfo(fileFullPath, finfo);
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
      _frmFilter = new frmFilterDateRange();
      _frmFilter.frmPO = this;
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
        ShowByPRReport(strReport);
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
    public async void ShowByPRReport(string strReport)
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_dtmStart, _dtmStart) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);
      DataTable dtData = new DataTable();
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      string fileFullPath = EpplusHelper.CreateEmptyExcel(strReport, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, strReport);

      switch (strReport)
      {
        #region Production by Age

        case "Production by Age":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList())));

          List<RptProductionByAgeOuthouse> lstRptProductionByAgeOuthouse = await BRReportsByLeadSource.GetRptProductionByAge(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgeOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByAgeOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgeOuthouse);
          break;

        #endregion Production by Age

        #region Production by Age & Sales Room
        case "Production by Age & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList())));

          List<RptProductionByAgeSalesRoomOuthouse> lstRptProductionByAgeSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByAgeSalesRoomOuthouse(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgeSalesRoomOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByAgeSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgeSalesRoomOuthouse);
          break;
        #endregion

        #region Production by Agency, Market & Hotel
        case "Production by Agency, Market & Hotel":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList())));

          List<RptProductionByAgencyMarketHotelOuthouse> lstRptProductionByAgencyMarketHotelOuthouse = await BRReportsByLeadSource.GetRptProductionByAgencyMarketHotelOuthouse(_dtmStart, _dtmEnd,
            "ALL",
             string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgencyMarketHotelOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByAgencyMarketHotelOuthouse(strReport, fileFullPath, filters, lstRptProductionByAgencyMarketHotelOuthouse);
          break;
        #endregion

        #region Production by Couple Type
        case "Production by Couple Type":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByCoupleTypeOuthouse> lstRptProductionByCoupleTypeOuthouse = await BRReportsByLeadSource.GetRptProductionByCoupleTypeOuthouse(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByCoupleTypeOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByCoupleTypeOuthouse(strReport, fileFullPath, filters, lstRptProductionByCoupleTypeOuthouse);
          break;
        #endregion

        #region Production by Couple Type & Sales Room
        case "Production by Couple Type & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByCoupleTypeSalesRoomOuthouse> lstRptProductionByCoupleTypeSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByCoupleTypeSalesRoomOuthouse(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByCoupleTypeSalesRoomOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByCoupleTypeSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByCoupleTypeSalesRoomOuthouse);
          break;
        #endregion

        #region Production by Gift (Invitation)
        case "Production by Gift (Invitation)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));
          filters.Add(new Tuple<string, string>("Gifts", string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID))));

          List<RptProductionByGiftInvitation> lstRptProductionByGiftInvitation = await BRReportsByLeadSource.GetRptProductionByGiftInvitation(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList()),
           EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByGiftInvitation.Count > 0)
            finfo = clsReports.ExportRptProductionByGiftInvitation(strReport, fileFullPath, filters, lstRptProductionByGiftInvitation);
          break;
        #endregion

        #region Production by Gift (Invitation) & Sales Room
        case "Production by Gift (Invitation) & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));
          filters.Add(new Tuple<string, string>("Gifts", string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID))));

          List<RptProductionByGiftInvitationSalesRoom> lstRptProductionByGiftInvitationSalesRoom = await BRReportsByLeadSource.GetRptProductionByGiftInvitationSalesRoom(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList()),
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByGiftInvitationSalesRoom.Count > 0)
            finfo = clsReports.ExportRptProductionByGiftInvitationSalesRoom(strReport, fileFullPath, filters, lstRptProductionByGiftInvitationSalesRoom);
          break;
        #endregion

        #region Production by Guest Status
        case "Production by Guest Status":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByGuestStatusOuthouse> lstRptProductionByGuestStatusOuthouse = await BRReportsByLeadSource.GetRptProductionByGuestStatusOuthouse(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByGuestStatusOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByGuestStatusOuthouse(strReport, fileFullPath, filters, lstRptProductionByGuestStatusOuthouse);
          break;
        #endregion

        #region Production by Hotel
        case "Production by Hotel":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByHotel> lstRptProductionByHotel = await BRReportsByLeadSource.GetRptProductionByHotel(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if(lstRptProductionByHotel.Count > 0)
            finfo = clsReports.ExportRptProductionByHotel(strReport, fileFullPath, filters, lstRptProductionByHotel);
          break;
        #endregion

        #region Production by Hotel & Sales Room
        case "Production by Hotel & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByHotelSalesRoom> lstRptProductionByHotelSalesRoom = await BRReportsByLeadSource.GetRptProductionByHotelSalesRoom(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByHotelSalesRoom.Count > 0)
            finfo = clsReports.ExportRptProductionByHotelSalesRoom(strReport, fileFullPath, filters, lstRptProductionByHotelSalesRoom);
          break;
        #endregion

        #region Production by Hotel Group
        case "Production by Hotel Group":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByHotelGroup> lstRptProductionByHotelGroup = await BRReportsByLeadSource.GetRptProductionByHotelGroup(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if(lstRptProductionByHotelGroup.Count > 0)
            finfo = clsReports.ExportRptProductionByHotelGroup(strReport, fileFullPath, filters, lstRptProductionByHotelGroup);
          break;
        #endregion

        #region Production by Hotel Group & Sales Room
        case "Production by Hotel Group & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByHotelGroupSalesRoom> lstRptProductionByHotelGroupSalesRoom = await BRReportsByLeadSource.GetRptProductionByHotelGroupSalesRoom(_dtmStart, _dtmEnd,
             "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if(lstRptProductionByHotelGroupSalesRoom.Count > 0)
            finfo = clsReports.ExportRptProductionByHotelGroupSalesRoom(strReport, fileFullPath, filters, lstRptProductionByHotelGroupSalesRoom);
          break;
        #endregion

        #region Production by Nationality
        case "Production by Nationality":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByNationalityOuthouse> lstRptProductionByNationalityOuthouse = await BRReportsByLeadSource.GetRptProductionByNationalityOuthouse(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByNationalityOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByNationalityOuthouse(strReport, fileFullPath, filters, lstRptProductionByNationalityOuthouse);
          break;
        #endregion

        #region Production by Nationality & Sales Room
        case "Production by Nationality & Sales Room":

          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByNationalitySalesRoomOuthouse> lstRptProductionByNationalitySalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByNationalitySalesRoomOuthouse(_dtmEnd, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByNationalitySalesRoomOuthouse.Count > 0) 
            finfo = clsReports.ExportRptProductionByNationalitySalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByNationalitySalesRoomOuthouse);  
          break;
        #endregion

        #region Production by PR & Sales Room (Deposits & Flyers Show)
        case "Production by PR & Sales Room (Deposits & Flyers Show)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomOuthouse = await BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByPRSalesRoomOuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, fileFullPath, filters, lstRptProductionByPRSalesRoomOuthouse);
          break;
        #endregion

        #region Production by PR (Deposits & Flyers Show)
        case "Production by PR (Deposits & Flyers Show)":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByPROuthouse> lstRptProductionByPROuthouse = await BRReportsByLeadSource.GetRptProductionByPROuthouse(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByPROuthouse.Count > 0)
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, fileFullPath, filters, lstRptProductionByPROuthouse);
          break;
        #endregion

        #region Production by Wave
        case "Production by Wave":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByWave> lstRptProductionByWave = await BRReportsByLeadSource.GetRptProductionByWave(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if(lstRptProductionByWave.Count > 0)
            finfo = clsReports.ExportRptProductionByWave(strReport, fileFullPath, filters, lstRptProductionByWave);
          break;
        #endregion

        #region Production by Wave & Sales Room
        case "Production by Wave & Sales Room":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID))));

          List<RptProductionByWaveSalesRoom> lstRptProductionByWaveSalesRoom = await BRReportsByLeadSource.GetRptProductionByWaveSalesRoom(_dtmStart, _dtmEnd,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByWaveSalesRoom.Count > 0)
            finfo = clsReports.ExportRptProductionByWaveSalesRoom(strReport, fileFullPath, filters, lstRptProductionByWaveSalesRoom);
          break;
          #endregion
      }

      if (finfo == null)
      {
        finfo = EpplusHelper.CreateNoInfoRptExcel(filters, strReport, fileFullPath);
      }
      _frmReportQueue.SetFileInfo(fileFullPath, finfo);
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
      _frmFilter = new frmFilterDateRange();
      _frmFilter.frmPO = this;

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
        ShowOtherReport(strReport);
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
    public async void ShowOtherReport(string strReport)
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_dtmStart, _dtmStart) : DateHelper.DateRange(_dtmStart, _dtmEnd);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_dtmStart, _dtmStart) : DateHelper.DateRangeFileName(_dtmStart, _dtmEnd);
      DataTable dtData = new DataTable();
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      string fileFullPath = EpplusHelper.CreateEmptyExcel(strReport, dateRangeFileNameRep);
      _frmReportQueue.AddReport(fileFullPath, strReport);

      switch (strReport)
      {
        #region Folios Invitations Outhouse
        case "Folios Invitations Outhouse":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));

          List<RptFoliosInvitationByDateFolio> lstRptFoliosInvitationByDateFolio = await BRReportsByLeadSource.GetRptFoliosInvitationByDateFolio(_frmFilter.chkUseDates.IsChecked.Value ? _dtmStart : (DateTime?)null, _frmFilter.chkUseDates.IsChecked.Value ? _dtmEnd : (DateTime?)null, 
            _cboFolSeriesSelected,
            _folFrom,
            _folTo,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList())
            );
          if (lstRptFoliosInvitationByDateFolio.Count > 0)
            finfo = clsReports.ExportRptFoliosInvitationByDateFolio(strReport, fileFullPath, filters, lstRptFoliosInvitationByDateFolio);
          break;
        #endregion

        #region Folios Invitations Outhouse by PR
        case "Folios Invitations Outhouse by PR":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));

          List<RptFoliosInvitationsOuthouseByPR> lstRptFoliosInvitationsOuthouseByPR = await BRReportsByLeadSource.GetRptFoliosInvitationsOuthouseByPR(_frmFilter.chkUseDates.IsChecked.Value ? _dtmStart : (DateTime?)null, _frmFilter.chkUseDates.IsChecked.Value ? _dtmEnd : (DateTime?)null,
            _cboFolSeriesSelected,
            _folFrom,
            _folTo,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList())
            );
          if (lstRptFoliosInvitationsOuthouseByPR.Count > 0)
            finfo = clsReports.ExportRptFoliosInvitationOuthouseByPR(strReport, fileFullPath, filters, lstRptFoliosInvitationsOuthouseByPR);
          break;
        #endregion

        #region Folios CxC by PR
        case "Folios CxC by PR":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));

          List<RptFoliosCxCByPR> lstRptFoliosCxCByPR = await BRReportsByLeadSource.GetRptFoliosCxCByPR(_frmFilter.chkUseDates.IsChecked.Value ? _dtmStart : (DateTime?)null, _frmFilter.chkUseDates.IsChecked.Value ? _dtmEnd : (DateTime?)null,
            _frmFilter.chkAllFolios.IsChecked.Value,
            _folFrom,
            _folTo,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            _frmFilter.chkAllPR.IsChecked.Value ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()));
          if (lstRptFoliosCxCByPR.Count > 0)
            finfo = clsReports.ExportRptFoliosCxCByPR(strReport, fileFullPath, filters, lstRptFoliosCxCByPR);
          break;
        #endregion

        #region Folios CXC
        case "Folios CXC":
          filters.Add(new Tuple<string, string>("Date Range", dateRange));
          filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));

          List<RptFoliosCXC> lstRptFoliosCXC = await BRReportsByLeadSource.GetRptFoliosCXC(_frmFilter.chkUseDates.IsChecked.Value ? _dtmStart : (DateTime?)null, _frmFilter.chkUseDates.IsChecked.Value ? _dtmEnd : (DateTime?)null,
            _frmFilter.chkAllFolios.IsChecked.Value,
            _folFrom,
            _folTo,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()));
          if(lstRptFoliosCXC.Count > 0)
            finfo = clsReports.ExportRptFoliosCXC(strReport, fileFullPath, filters, lstRptFoliosCXC);
          break;
          #endregion
      }

      if (finfo == null)
      {
        finfo = EpplusHelper.CreateNoInfoRptExcel(filters, strReport, fileFullPath);
      }
      _frmReportQueue.SetFileInfo(fileFullPath, finfo);
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

    #region btnPrintRptByLeadSource_Click

    /// <summary>
    /// Imprime un reporte por LeadSource
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    ///   [vku] 10/Jun/2016 Modified. Ahora verifica que este configurado la ruta para guardar el reporte
    /// </history>
    private void btnPrintRptByLeadSource_Click(object sender, RoutedEventArgs e)
    {
      if (ConfigRegistry.ExistReportsPath())
      {
        PrepareReportByLeadSource();
      }
      else
      {
        MessageBoxResult result = UIHelper.ShowMessage("It is not configured path yet. Do you want to configure path now?", MessageBoxImage.Question, "Proccesor Outhouse");
        if (result == MessageBoxResult.Yes)
        {
          _SystemConfig = new SystemCfg(EnumConfiguration.ReportsPath);
          _SystemConfig.Show();
        }
      }   
    }

    #endregion btnPrintRptByLeadSource_Click

    #region btnPrintRptByPR_Click

    /// <summary>
    /// Imprime un reporte por PR
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void btnPrintRptByPR_Click(object sender, RoutedEventArgs e)
    {
      if (ConfigRegistry.ExistReportsPath())
      {
        PrepareReportByPR();
      }
      else
      {
        MessageBoxResult result = UIHelper.ShowMessage("It is not configured path yet. Do you want to configure path now?", MessageBoxImage.Question, "Proccesor Outhouse");
        if (result == MessageBoxResult.Yes)
        {
          _SystemConfig = new SystemCfg(EnumConfiguration.ReportsPath);
          _SystemConfig.Show();
        }
      }
    }

    #endregion btnPrintRptByPR_Click

    #region btnPrintOtherRpts_Click

    /// <summary>
    /// Imprime un reporte de Others
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void btnPrintOtherRpts_Click(object sender, RoutedEventArgs e)
    {
      if (ConfigRegistry.ExistReportsPath())
      {
        PrepareOtherReports();
      }
      else
      {
        MessageBoxResult result = UIHelper.ShowMessage("It is not configured path yet. Do you want to configure path now?", MessageBoxImage.Question, "Proccesor Outhouse");
        if (result == MessageBoxResult.Yes)
        {
          _SystemConfig = new SystemCfg(EnumConfiguration.ReportsPath);
          _SystemConfig.Show();
        }
      }
    }

    #endregion btnPrintOtherRpts_Click

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
    /// </history>
    private void grdrpt_MouseDoubleClick(object sender, RoutedEventArgs e)
    {
      var _dataGridRow = (DataGridRow)sender;
      if (ConfigRegistry.ExistReportsPath())
      {
        if (_dataGridRow.Item.Equals(grdRptsByLeadSource.CurrentItem)) PrepareReportByLeadSource();
        else if (_dataGridRow.Item.Equals(grdRptsByPR.CurrentItem)) PrepareReportByPR();
        else if (_dataGridRow.Item.Equals(grdOtherRpts.CurrentItem)) PrepareOtherReports();
      }
      else
      {
        MessageBoxResult result = UIHelper.ShowMessage("It is not configured path yet. Do you want to configure path now?", MessageBoxImage.Question, "Proccesor Outhouse");
        if (result == MessageBoxResult.Yes)
        {
          _SystemConfig = new SystemCfg(EnumConfiguration.ReportsPath);
          _SystemConfig.Show();
        }
      }
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