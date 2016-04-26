using System;
using System.Collections.Generic;
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
using IM.ProcessorOuthouse.Classes;
using System.Diagnostics;

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
      lblUserName.Content = App.User.User.peN;
    }
    #endregion

    #region Atributos
    private frmFilterDateRange _frmFilter;
    private bool _blnOneDate;
    private bool _blnOnlyOneRegister;

    public List<int> _lstLeadSources = new List<int>();
    public List<int> _lstLeadSourcesPaymentComm = new List<int>();
    public List<int> _lstPaymentTypes = new List<int>();
    public List<int> _lstPRs = new List<int>();
    public List<int> _lstChargeTo = new List<int>();
    public List<int> _lstGifts = new List<int>();
    public List<int> _lstGiftsProdGift = new List<int>();
    public string _cboDateSelected;
    public string _cboFolSeriesSelected;
    public DateTime _dtmStart = DateTime.Now.Date;
    public DateTime _dtmEnd = DateTime.Now.Date;
    public EnumBasedOnArrival _enumBasedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival;
    public EnumBasedOnBooking _enumBasedOnBooking = EnumBasedOnBooking.bobNoBasedOnBooking;
    public EnumQuinellas _enumQuinellas = EnumQuinellas.quNoQuinellas;
    public EnumDetailGifts _enumDetailsGift = EnumDetailGifts.dgNoDetailGifts;
    public EnumSalesByMemberShipType _enumSalesByMemberShipType = EnumSalesByMemberShipType.sbmNoDetail;
    public EnumStatus _enumStatus = EnumStatus.staActives;
    public EnumGiftSale _enumGiftSale = EnumGiftSale.gsAll;
    public EnumSaveCourtesyTours _enumSaveCourtesyTours = EnumSaveCourtesyTours.sctExcludeSaveCourtesyTours;
    public EnumExternalInvitation _enumExternalInvitation = EnumExternalInvitation.extExclude;
    public string _folFrom = "";
    public string _folTo = "";
    #endregion

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
        new {rptName="Unlinked Sales"}
      }.OrderBy(c => c.rptName).ToList());
      grdRptsByLeadSource.ItemsSource = lstRptsByLeadSource;
      #endregion

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
      #endregion

      #region Grid OtherRpts
      ListCollectionView lstOtherRpts = new ListCollectionView(new List<dynamic>{
        new {rptName="Folios Invitations Outhouse"},
        new {rptName="Folios Invitations Outhouse by PR"},
        new {rptName="Folios CxC by PR"},
        new {rptName="Folios CXC"}
      }.OrderBy(c => c.rptName).ToList());
      grdOtherRpts.ItemsSource = lstOtherRpts;
      #endregion

      StatusBarReg.Content = string.Format("{0} Reports", lstRptsByLeadSource.Count + lstRptsByPR.Count + lstOtherRpts.Count);
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
    #endregion

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
    #endregion

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
    #endregion

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
        case "Unlinked Sales":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true);
            break;
        case "PR Payment Commissions":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSourcesPaymentComm: true, blnAllLeadSourcePaymentComm: true, enumPeriod : EnumPeriod.pdWeekly );
          break;
        case "Production by Agency":
        case "Production by Agency & Sales Room":
            _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, enumSalesByMemberShipType: EnumSalesByMemberShipType.sbmDetail);
            break;
        case "Production by Gift (Invitation)":
        case "Production by Gift (Invitation) & Sales Room":
            _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, blnGiftProdGift: true, blnAllGiftProdGift: true);
            break;
        case "Production by Nationality":
        case "Production by Nationality & Sales Room":
            _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, enumSaveCourtesyTours: EnumSaveCourtesyTours.sctIncludeSaveCourtesyTours);
            break; 
        case "Production by PR":
        case "Production by PR & Sales Room":
        case "Production by PR & Sales Room (Deposits & Flyers Show)":
        case "Production by PR & Sales Room (Deposits)":
        case "Production by PR & Sales Room (Flyers)":
        case "Production by PR (Deposits & Flyers Show)":
        case "Production by PR (Deposits)":
        case "Production by PR (Flyers)":
            _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, enumBasedOnBooking: EnumBasedOnBooking.bobBasedOnBooking);
            break;
      }

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowLeadSourceReport(strReport);
        _frmFilter.Close();
      }
      else {
        _frmFilter.Close();
        _frmFilter = null;
      }
    }
    #endregion

    #region ShowLeadSourceReport
    /// <summary>
    ///  Muestra un reporte por lead source
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    public void ShowLeadSourceReport(string strReport)
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmStart.Value.Value) : DateHelper.DateRange(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmStart.Value.Value) : DateHelper.DateRangeFileName(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value);
      DataTable dtData = new DataTable();
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      WaitMessage(true, "Loading report...");

      switch (strReport)
      {
        #region Deposits Payment By PR
        case "Deposits Payment by PR":
          List<object> lstRptDepositsPaymentByPR = BRReportsByLeadSource.GetRptDepositsPaymentByPR(
            _frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            string.Join(",", _frmFilter.grdPaymentTypes.SelectedItems.Cast<PaymentType>().Select(c => c.ptID).ToList()),
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (_lstPaymentTypes.Any())
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));
            finfo = clsReports.ExportRptDepositsPaymentByPR(strReport, dateRangeFileNameRep, filters, lstRptDepositsPaymentByPR);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing Outhouse");
          }
          break;
        #endregion

        #region Gifts Received by Sales Room
        case "Gifts Received by Sales Room":
          List<object> lstRptGiftsReceivedBySR = BRReportsByLeadSource.GetRptGiftsReceivedBySR(
          _frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
          string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
          string.Join(",", _frmFilter.grdChargeTo.SelectedItems.Cast<ChargeTo>().Select(c => c.ctID).ToList()),
          string.Join(",", _frmFilter.grdGifts.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList()));
          if (lstRptGiftsReceivedBySR.Any())
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));
            filters.Add(new Tuple<string, string>("Charge To", _frmFilter.grdChargeTo.SelectedItems.Count == _frmFilter.grdChargeTo.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdChargeTo.SelectedItems.Cast<ChargeTo>().Select(c => c.ctID).ToList())));
            filters.Add(new Tuple<string, string>("Gifts", _frmFilter.grdGifts.SelectedItems.Count == _frmFilter.grdGifts.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdGifts.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList())));
            finfo = clsReports.ExportRptGiftsReceivedBySR(strReport, dateRangeFileNameRep, filters, lstRptGiftsReceivedBySR);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Guests Show No Presented Invitation
        case "Guests Show No Presented Invitation":
          List<GuestShowNoPresentedInvitation> lstRptGuestsShowNoPresentedInvitation = BRReportsByLeadSource.GetRptGuestsShowNoPresentedInvitation(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value, string.Join(",", _frmFilter.grdLeadSources.SelectedItems.OfType<LeadSourceByUser>().Select(c => c.lsID)));
          if (lstRptGuestsShowNoPresentedInvitation.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));
            finfo = clsReports.ExportRptGuestsShowNoPresentedInvitation(strReport, dateRangeFileNameRep, filters, lstRptGuestsShowNoPresentedInvitation);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region PR Payment Commissions
        case "PR Payment Commissions":
          List<RptProductionByPROuthouse> lstRptProductionByPROuthouseComm = BRReportsByLeadSource.GetRptProductionByPROuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSourcesPaymentComm.SelectedItems.Cast<LeadSource>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit,
            _enumBasedOnBooking);
          if (lstRptProductionByPROuthouseComm.Any())
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", string.Join(",", _frmFilter.grdLeadSourcesPaymentComm.SelectedItems.Cast<LeadSource>().Select(c => c.lsID).ToList())));
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPROuthouseComm);
          }
          else
          {
            UIHelper.ShowMessage("There is no data");
          }

          break;
        #endregion

        #region Production by Age 
        case "Production by Age":
          List<RptProductionByAgeOuthouse> lstRptProductionByAgeOuthouse = BRReportsByLeadSource.GetRptProductionByAge(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgeOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList())));
            finfo = clsReports.ExportRptProductionByAge(strReport, dateRangeFileNameRep, filters, lstRptProductionByAgeOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;

        #endregion

        #region Production by Age & Sales Room
        case "Production by Age & Sales Room":
          List<RptProductionByAgeSalesRoomOuthouse> lstRptProductionByAgeSalesRoomOuthouse = BRReportsByLeadSource.GetRptProductionByAgeSalesRoomOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgeSalesRoomOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByAgeSalesRoomOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByAgeSalesRoomOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by Agency
        case "Production by Agency":
          List<RptProductionByAgencyOuthouse> lstRptProductionByAgencyOuthouse = BRReportsByLeadSource.GetRptProductionByAgencyOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit,
            _enumSalesByMemberShipType);
          if (lstRptProductionByAgencyOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByAgencyOuhouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByAgencyOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by Agency & Sales Room
        case "Production by Agency & Sales Room":
          List<RptProductionByAgencySalesRoomOuthouse> lstRptProductionByAgencySalesRoomOuthouse = BRReportsByLeadSource.GetRptProductionByAgencySalesRoomOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgencySalesRoomOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByAgencySalesRoomOuhouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByAgencySalesRoomOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by Agency, Market & Hotel
        case "Production by Agency, Market & Hotel":
          List<RptProductionByAgencyMarketHotelOuthouse> lstRptProductionByAgencyMarketHotelOuthouse = BRReportsByLeadSource.GetRptProductionByAgencyMarketHotelOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgencyMarketHotelOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByAgencyMarketHotelOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByAgencyMarketHotelOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by Couple Type
        case "Production by Couple Type":
          List<RptProductionByCoupleTypeOuthouse> lstRptProductionByCoupleTypeOuthouse = BRReportsByLeadSource.GetRptProductionByCoupleTypeOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByCoupleTypeOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByCoupleTypeOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByCoupleTypeOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by Couple Type & Sales Room
        case "Production by Couple Type & Sales Room":
          List<RptProductionByCoupleTypeSalesRoomOuthouse> lstRptProductionByCoupleTypeSalesRoomOuthouse = BRReportsByLeadSource.GetRptProductionByCoupleTypeSalesRoomOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByCoupleTypeSalesRoomOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByCoupleTypeSalesRoomOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByCoupleTypeSalesRoomOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by Gift (Invitation)
        case "Production by Gift (Invitation)":
          List<RptProductionByGiftInvitation> lstRptProductionByGiftInvitation = BRReportsByLeadSource.GetRptProductionByGiftInvitation(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList()),
           EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByGiftInvitation.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            filters.Add(new Tuple<string, string>("Gifts", string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID))));
            finfo = clsReports.ExportRptProductionByGiftInvitation(strReport, dateRangeFileNameRep, filters, lstRptProductionByGiftInvitation);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthose");
          }
          break;
        #endregion

        #region Production by Gift (Invitation) & Sales Room
        case "Production by Gift (Invitation) & Sales Room":
          List<RptProductionByGiftInvitationSalesRoom> lstRptProductionByGiftInvitationSalesRoom = BRReportsByLeadSource.GetRptProductionByGiftInvitationSalesRoom(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList()),
           EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByGiftInvitationSalesRoom.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            filters.Add(new Tuple<string, string>("Gifts", string.Join(",", _frmFilter.grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => c.giID))));
            finfo = clsReports.ExportRptProductionByGiftInvitationSalesRoom(strReport, dateRangeFileNameRep, filters, lstRptProductionByGiftInvitationSalesRoom);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by Guest Status
        case "Production by Guest Status":
          List<RptProductionByGuestStatusOuthouse> lstRptProductionByGuestStatusOuthouse = BRReportsByLeadSource.GetRptProductionByGuestStatusOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdAll);
          if (lstRptProductionByGuestStatusOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByGuestStatusOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByGuestStatusOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by Nationality
        case "Production by Nationality":
          List<RptProductionByNationalityOuthouse> lstRptProductionByNationalityOuthouse = BRReportsByLeadSource.GetRptProductionByNationalityOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
              string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
            "ALL",
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit,
            _enumSaveCourtesyTours);
          if (lstRptProductionByNationalityOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByNationalityOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByNationalityOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by Nationality & Sales Room
        case "Production by Nationality & Sales Room":
          List<RptProductionByNationalitySalesRoomOuthouse> lstRptProductionByNationalitySalesRoomOuthouse = BRReportsByLeadSource.GetRptProductionByNationalitySalesRoomOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdDepositShowsNoDeposit,
             _enumSaveCourtesyTours);
          if (lstRptProductionByNationalitySalesRoomOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByNationalitySalesRoomOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByNationalitySalesRoomOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
          break;
        #endregion

        #region Production by PR
        case "Production by PR":
          List<RptProductionByPROuthouse> lstRptProductionByPROuthouse = BRReportsByLeadSource.GetRptProductionByPROuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdAll,
             _enumBasedOnBooking);
          if (lstRptProductionByPROuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPROuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing");
          }
          break;
        #endregion

        #region Production by PR & Sales Room
        case "Production by PR & Sales Room":
          List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomOuthouse = BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdAll,
             _enumBasedOnBooking);
          if (lstRptProductionByPRSalesRoomOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPRSalesRoomOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing");
          }
          break;
        #endregion

        #region Production by PR & Sales Room (Deposits & Flyers Show)
        case "Production by PR & Sales Room (Deposits & Flyers Show)":
          List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomDepositsFlyersShow = BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdDepositShowsNoDeposit,
             _enumBasedOnBooking);
          if (lstRptProductionByPRSalesRoomDepositsFlyersShow.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPRSalesRoomDepositsFlyersShow);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing");
          }
          break;
        #endregion

        #region Production by PR & Sales Room (Deposits)
        case "Production by PR Sales Room (Deposits)":
          List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomDeposits = BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdDeposit,
             _enumBasedOnBooking);
          if (lstRptProductionByPRSalesRoomDeposits.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPRSalesRoomDeposits);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing");
          }
          break;
        #endregion

        #region Production by PR & Sales Room (Flyers)
        case "Production by PR & Sales Room (Flyers)":
          List<RptProductionByPRSalesRoomOuthouse> lstRptProductionByPRSalesRoomFlyers = BRReportsByLeadSource.GetRptProductionByPRSalesRoomOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdNoDeposit,
             _enumBasedOnBooking);
          if (lstRptProductionByPRSalesRoomFlyers.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByPRSalesRoomOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPRSalesRoomFlyers);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing");
          }
          break;
        #endregion

        #region Production by PR (Deposits & Flyers Show)
        case "Production by PR (Deposits & Flyers Show)":
          List<RptProductionByPROuthouse> lstRptProductionByPRDepositsFlyersShowOuthouse = BRReportsByLeadSource.GetRptProductionByPROuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
             string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
             "ALL",
             EnumProgram.Outhouse,
             EnumFilterDeposit.fdDepositShowsNoDeposit,
             _enumBasedOnBooking);
          if (lstRptProductionByPRDepositsFlyersShowOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPRDepositsFlyersShowOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing");
          }
          break;
        #endregion

        #region Production by PR (Deposits)
        case "Production by PR (Deposits)":
          List<RptProductionByPROuthouse> lstRptProductionByPRDepositsOuthouse = BRReportsByLeadSource.GetRptProductionByPROuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
               string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
               "ALL",
               EnumProgram.Outhouse,
               EnumFilterDeposit.fdDeposit,
               _enumBasedOnBooking);
          if (lstRptProductionByPRDepositsOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPRDepositsOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing");
          }
          break;
        #endregion

        #region Production by PR (Flyers)
        case "Production by PR (Flyers)":
          List<RptProductionByPROuthouse> lstRptProductionByPRFlyersOuthouse = BRReportsByLeadSource.GetRptProductionByPROuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
               string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
               "ALL",
               EnumProgram.Outhouse,
               EnumFilterDeposit.fdNoDeposit,
               _enumBasedOnBooking);
          if (lstRptProductionByPRFlyersOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByPROuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPRFlyersOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing");
          }
          break;
        #endregion

        #region Production by PR Contact
        case "Production by PR Contact":
          List<RptProductionByPRContactOuthouse> lstRptProductionByPRContactOuthouse = BRReportsByLeadSource.GetProductionByPRContactOuthouse(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
               string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList()),
               "ALL",
               EnumProgram.Outhouse,
               EnumFilterDeposit.fdAll);
          if (lstRptProductionByPRContactOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("Lead Sources", _frmFilter.grdLeadSources.SelectedItems.Count == _frmFilter.grdLeadSources.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID))));
            finfo = clsReports.ExportRptProductionByPRContactOuthouse(strReport, dateRangeFileNameRep, filters, lstRptProductionByPRContactOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing");
          }
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
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true);
          break;
        case "Production by Gift (Invitation)":
        case "Production by Gift (Invitation) & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true, blnGiftProdGift: true, blnAllGiftProdGift: true);
          break;
        case "Production by Agency":
        case "Production by Agency & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true, enumSalesByMemberShipType: EnumSalesByMemberShipType.sbmDetail);
          break;
        case "Production by Nationality":
        case "Production by Nationality & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true, blnGifts: true, blnAllGifts: true, enumSaveCourtesyTours: EnumSaveCourtesyTours.sctExcludeSaveCourtesyTours);
          break;
        case "Production by PR & Sales Room (Deposits & Flyers Show)":
        case "Production by PR (Deposits & Flyers Show)":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true, blnGifts: true, blnAllGifts: true, enumBasedOnBooking: EnumBasedOnBooking.bobBasedOnBooking);
          break;
      }

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        ShowByPRReport(strReport);
        _frmFilter.Close();
      }
      else {
        _frmFilter.Close();
        _frmFilter = null;
      }
    }
    #endregion

    #region ShowByPRReport
    /// <summary>
    ///  Muestra un reporte por PR
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    public void ShowByPRReport(string strReport)
    {
      FileInfo finfo = null;
      string dateRange = (_blnOneDate) ? DateHelper.DateRange(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmStart.Value.Value) : DateHelper.DateRange(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value);
      string dateRangeFileNameRep = (_blnOneDate) ? DateHelper.DateRangeFileName(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmStart.Value.Value) : DateHelper.DateRangeFileName(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value);
      DataTable dtData = new DataTable();
      List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
      WaitMessage(true, "Loading report...");

      switch (strReport)
      {
        #region Production by Age
        case "Production by Age":
          List<RptProductionByAgeOuthouse> lstRptProductionByAgeOuthouse = BRReportsByLeadSource.GetRptProductionByAge(_frmFilter.dtmStart.Value.Value, _frmFilter.dtmEnd.Value.Value,
            "ALL",
            string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList()),
            EnumProgram.Outhouse,
            EnumFilterDeposit.fdDepositShowsNoDeposit);
          if (lstRptProductionByAgeOuthouse.Count > 0)
          {
            filters.Add(new Tuple<string, string>("Date Range", dateRange));
            filters.Add(new Tuple<string, string>("PR", _frmFilter.grdPR.SelectedItems.Count == _frmFilter.grdPR.Items.Count ? "ALL" : string.Join(",", _frmFilter.grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => c.peID).ToList())));
            finfo = clsReports.ExportRptProductionByAge(strReport, dateRangeFileNameRep, filters, lstRptProductionByAgeOuthouse);
          }
          else
          {
            UIHelper.ShowMessage("There is no data", MessageBoxImage.Warning, "Intelligence Marketing ProcessorOuthouse");
          }
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
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, blnPRs: true, blnUseDates: true, blnChkUsedate: true, blnFolSeries: true, blnFolFrom: true, blnFolTo: true, blnAllFolios: true);
          break;
        case "Folios CxC by PR":
        case "Folios CXC":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, blnPRs: true, blnUseDates: true, blnChkUsedate: true, blnFolFrom: true, blnFolTo: true, blnAllFolios: true);
          break;
      }

      WaitMessage(false);
      _frmFilter.ShowDialog();
      if (_frmFilter._blnOK)
      {
        _frmFilter.Close();
      }
    }
    #endregion

    #region ShowOtherReport
    /// <summary>
    ///  Muestra otros reportes
    /// </summary>
    /// <param name="strReport">Nombre del reporte</param>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    public void ShowOtherReport(string strReport)
    {

    }
    #endregion

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
    #endregion

    #endregion

    #region eventos

    #region btnPrintRptByLeadSource_Click
    /// <summary>
    /// Imprime un reporte por LeadSource
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void btnPrintRptByLeadSource_Click(object sender, RoutedEventArgs e)
    {
      PrepareReportByLeadSource();
    }
    #endregion

    #region btnPrintRptByPR_Click
    /// <summary>
    /// Imprime un reporte por PR
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void btnPrintRptByPR_Click(object sender, RoutedEventArgs e)
    {
      PrepareReportByPR();
    }
    #endregion

    #region btnPrintOtherRpts_Click
    /// <summary>
    /// Imprime un reporte de Others
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void btnPrintOtherRpts_Click(object sender, RoutedEventArgs e)
    {
      PrepareOtherReports();
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
    #endregion

    #region grdrptLeadSources_dblClick
    /// <summary>
    /// Se abre frmDateRange al hacer doble clic a un reporte por leadsource
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void grdrptLeadSources_dblClick(object sender, RoutedEventArgs e)
    {
      PrepareReportByLeadSource();
    }
    #endregion

    #region grdrptByPR_dblClick
    /// <summary>
    ///   Se abre frmDateRange al hacer doble clic a un reporte por PR
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void grdrptByPR_dblClick(object sender, RoutedEventArgs e)
    {
      PrepareReportByPR();
    }
    #endregion

    #region grdOtherRpt_dblClick
    /// <summary>
    ///   Se abre frmDateRange al hacer doble clic a un reporte de Others
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void grdOtherRpt_dblClick(object sender, RoutedEventArgs e)
    {
      PrepareOtherReports();
    }
    #endregion

    #endregion

  }
}
