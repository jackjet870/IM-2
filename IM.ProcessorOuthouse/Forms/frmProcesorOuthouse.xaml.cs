using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Base.Helpers;
using Xceed.Wpf.Toolkit;

namespace IM.ProcessorOuthouse.Forms
{
  /// <summary>
  /// Formulario para generar los reportes Outside
  /// Interaction logic for frmProcessorOuthouse.xaml
  /// </summary>
  /// <history>
  ///   [vku] 22/03/2016 Created
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
    #endregion

    #region Metodos

    #region ConfigGrds
    /// <summary>
    /// Se configuran los treeview, agregando 
    /// los reportes.
    /// </summary>
    /// <history>
    /// [vku] 22/03/2016 Created
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
        new {rptName="Production by PR (Contac)"},
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
    private void PrepareReportByLeadSource()
    {
      String strReport = "";
      // Se Valida que haya un reporte seleccionado
      if (grdRptsByLeadSource.SelectedItems.Count < 0)
        return;

        WaitMessage(true, "Loading Date Range Window...");
    
      //Se obtiene el nombre del reporte
      strReport = ((dynamic)grdRptsByLeadSource.SelectedItem).rptName;
 
      _blnOneDate = false;
      _blnOnlyOneRegister = false;

      switch (strReport)
      {
        case "PR Payment Commissions":
          _blnOnlyOneRegister = true;
        break;
      }

      OpenFilterDateRangeLS(strReport);
    }
    #endregion

    #region PrepareReportByPR
    /// <summary>
    /// Prepara un reporte por PR
    /// </summary>
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
        case "Production by PR (Contac)":
        case "Production by Wave":
        case "Production by Wave & Sales Room":
        case "Unlinked Sales":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true);
            break;
        case "PR Payment Commissions":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true);
            break;
        case "Production by Agency":
        case "Production by Agency & Sales Room":
            _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, enumSalesByMemberShipType: EnumSalesByMemberShipType.sbmDetail);
            break;
        case "Production by Gift (Invitation)":
        case "Production by Gift (Invitation) & Sales Room":
            _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnLeadSource: true, blnGifts: true, blnAllGifts: true);
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
        _frmFilter.Close();
      }
    }
    #endregion

    #region OpenFilterDateRangePR
    /// <summary>
    /// Abre la ventan frmFilterDateRange con los controles configurados
    /// segun el reporte seleccionado por PR
    /// </summary>
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
        case "Production by Gift (Invitation)":
        case "Production by Gift (Invitation) & Sales Room":
        case "Production by Guest Status":
        case "Production by Hotel":
        case "Production by Hotel & Sales Room":
        case "Production by Hotel Group":
        case "Production by Hotel Group & Sales Room":
        case "Production by Wave":
        case "Production by Wave & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true, blnGifts: true, blnAllGifts: true);
          break;
        case "Production by Agency":
        case "Production by Agency & Sales Room":
          _frmFilter.ConfigureForm(blnOneDate: _blnOneDate, blnOnlyOneRegister: _blnOnlyOneRegister, blnPRs: true, blnGifts: true, blnAllGifts: true, enumSalesByMemberShipType: EnumSalesByMemberShipType.sbmDetail);
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
        _frmFilter.Close();
      }
    }
    #endregion

    #region OpenFIlterDateRangeOtherReport
    /// <summary>
    /// Abre la ventan frmFilterDateRange con los controles configurados
    /// segun el reporte seleccionado por LeadSource
    /// </summary>
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

    #region WaitMessage
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
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
    private void btnPrintRptByLeadSource_Click(object sender, RoutedEventArgs e)
    {
      PrepareReportByLeadSource();
    }
    #endregion

    #region btnPrintRptByPR_Click
    /// <summary>
    /// Imprime un reporte por PR
    /// </summary>
    private void btnPrintRptByPR_Click(object sender, RoutedEventArgs e)
    {
      PrepareReportByPR();
    }
    #endregion

    #region btnPrintOtherRpts_Click
    /// <summary>
    /// Imprime un reporte de Others
    /// </summary>
    private void btnPrintOtherRpts_Click(object sender, RoutedEventArgs e)
    {
      PrepareOtherReports();
    }
    #endregion

    #region btnExit_Click
    /// <summary>
    /// Cierra la aplicacion Processor Outhouse
    /// </summary>
    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      App.Current.Shutdown();
    }
    #endregion

    #region grdrptLeadSources_dblClick
    /// <summary>
    /// Se abre frmDateRange al hacer doble clic a un reporte por leadsource
    /// </summary>
    private void grdrptLeadSources_dblClick(object sender, RoutedEventArgs e)
    {
      PrepareReportByLeadSource();
    }
    #endregion

    #region grdrptByPR_dblClick
    //Se abre frmDateRange al hacer doble clic a un reporte por PR
    private void grdrptByPR_dblClick(object sender, RoutedEventArgs e)
    {
      PrepareReportByPR();
    }
    #endregion

    #region grdOtherRpt_dblClick
    //Se abre frmDateRange al hacer doble clic a un reporte de Others
    private void grdOtherRpt_dblClick(object sender, RoutedEventArgs e)
    {
      PrepareOtherReports();
    }
    #endregion

    #endregion

  }
}
