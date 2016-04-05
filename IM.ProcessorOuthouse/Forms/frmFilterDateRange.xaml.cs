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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using IM.Model.Enums;
using IM.Model;
using IM.Model.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.ProcessorOuthouse.Forms
{
  /// <summary>
  /// Formulario con el filtro de fechas, LeadSources, Formas de pago, PRs, Cargar a,
  /// Regalos
  /// </summary>
  /// <history>
  /// [vku] 22/03/2016 Created
  /// </history>
  public partial class frmFilterDateRange : Window
  {

    #region Contructor
    public frmFilterDateRange()
    {
      InitializeComponent();

      _lstGifts = BRGifts.GetGifts();
      _lstLeadSources = BRLeadSources.GetLeadSourcesByUser(App.User.User.peID, "OUT", "ALL");
      _lstChargeTo = BRChargeTos.GetChargeTos(_chargeToFilter,-1);
      _lstPaymentType = BRPaymentTypes.GetPaymentTypes(1);
      _lstPRs = BRPersonnel.GetPersonnel("MPS", "ALL" ,Model.Classes.StrToEnums.EnumRoleToString(Model.Enums.EnumRole.PR),1);
      _lstFoliosInvitationOuthouse = BRFoliosInvitationsOuthouse.GetFoliosInvittionsOutside(nStatus: 1);
      //_lstPRs = BRPersonnel.GetPersonnel("App.userData.LeadSource.lsID","ALL","PR",1,"ALL");


      cboSaveCourtesyTours.ItemsSource = EnumToListHelper.GetList<EnumSaveCourtesyTours>();
      cboExternal.ItemsSource = EnumToListHelper.GetList<EnumExternalInvitation>();
      cboFolSeries.ItemsSource = _lstFoliosInvitationOuthouse;
      this.PreviewKeyDown += new KeyEventHandler(Close_KeyPreviewESC);
    }
    #endregion

    #region Atributos
    private List<LeadSourceByUser> _lstLeadSources = new List<LeadSourceByUser>();
    private List<PaymentType> _lstPaymentType = new List<PaymentType>();
    private List<PersonnelShort> _lstPRs = new List<PersonnelShort>();
    private List<ChargeTo> _lstChargeTo = new List<ChargeTo>();
    private List<GiftShort> _lstGifts = new List<GiftShort>();
    private List<FolioInvitationOuthouse> _lstFoliosInvitationOuthouse = new List<FolioInvitationOuthouse>();
    private List<FolioCxCPR> _lstRangeFolios= new List<FolioCxCPR>();
    private FolioInvitationOuthouse _folioInvOutFilter = new FolioInvitationOuthouse();
    private ChargeTo _chargeToFilter = new ChargeTo();
    public bool _blnOK = false;

    public frmProcessorOuthouse frmPO = new frmProcessorOuthouse();
    #endregion

    #region Metodos

    #region ConfigureGridsPanels
    /// <summary>
    /// Configura los Grids LeadSources, Formas de pago, cargar a y regalos
    /// </summary>
    private void ConfigureGridsPanels(bool blnLeadSources, bool blnPaymentTypes, bool blnPR,
      bool blnChargeTo, bool blnGifts, bool blnRangeFolios)
    {
      pnlLeadSources.Visibility = (blnLeadSources) ? Visibility.Visible : Visibility.Collapsed;
      pnlPaymentTypes.Visibility = (blnPaymentTypes) ? Visibility.Visible : Visibility.Collapsed;
      pnlPR.Visibility = (blnPR) ? Visibility.Visible : Visibility.Collapsed;
      pnlChargeTo.Visibility = (blnChargeTo) ? Visibility.Visible : Visibility.Collapsed;
      pnlGifts.Visibility = (blnGifts) ? Visibility.Visible : Visibility.Collapsed;

      grdLeadSources.ItemsSource = (blnLeadSources) ? _lstLeadSources : null;
      grdPaymentTypes.ItemsSource = (blnPaymentTypes) ? _lstPaymentType : null;
      grdPR.ItemsSource = (blnPR) ? _lstPRs : null;
      grdChargeTo.ItemsSource = (blnChargeTo) ? _lstChargeTo : null;
      grdGifts.ItemsSource = (blnGifts) ? _lstGifts : null;
      
      StatusBarNumPT.Content = (blnPaymentTypes) ? string.Format("{0}/{1} Selected PaymentTypes", 0, _lstPaymentType.Count) : "";
      StatusBarNumPR.Content = (blnPR) ? string.Format("{0}/{1} Selected PRs", 0, _lstPRs.Count) : "";
      StatusBarNumCT.Content = (blnChargeTo) ? string.Format("{0}/{1} Selected ChargeTo", 0, _lstChargeTo.Count) : "";
      StatusBarNumG.Content = (blnGifts) ? string.Format("{0}/{1} Selected Gifts", 0, _lstGifts.Count) : "";
      StatusBarNumLS.Content = (blnLeadSources) ? string.Format("{0}/{1} Selected LeadSources", 0, _lstLeadSources.Count) : "";
    }
    #endregion

    #region ConfigureForm
    /// <summary>
    /// Configura los controles del formulario
    /// </summary>
    public void ConfigureForm(bool blnLeadSource = false, bool blnAllLeadSource = false,
      bool blnPaymentTypes = false, bool blnAllPaymentTypes = false, bool blnPRs = false, bool blnAllPRs = false,
      bool blnChargeTo = false, bool blnAllChargeTo = false, bool blnGifts = false, bool blnAllGifts = false, bool blnRangeFolios = false, bool blnAllRangeFolios = false,
      bool blnOneDate = false, bool blnOnlyOneRegister = false, bool blnChkUsedate = false, EnumPeriod enumPeriod = EnumPeriod.pdNone,
      EnumPrograms enumPrograms = EnumPrograms.pgAll, bool blnOnePeriod = false, EnumBasedOnArrival enumBasedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival,
      EnumQuinellas enumQuinellas = EnumQuinellas.quNoQuinellas, EnumDetailGifts enumDetailGifts = EnumDetailGifts.dgNoDetailGifts, EnumSaveCourtesyTours? enumSaveCourtesyTours = null,
      EnumSalesByMemberShipType enumSalesByMemberShipType = EnumSalesByMemberShipType.sbmNoDetail, EnumBasedOnBooking enumBasedOnBooking = EnumBasedOnBooking.bobNoBasedOnBooking,
      EnumExternalInvitation? enumExternalInvitation = null, bool blnFolSeries = false, bool blnFolFrom = false, bool blnFolTo = false, bool blnUseDates = false, bool blnAllFolios = false)
    {
     ConfigureDates(blnOneDate, enumPeriod);
      ConfigureGridsPanels(blnLeadSource, blnPaymentTypes, blnPRs, blnChargeTo, blnGifts, blnRangeFolios);
      ConfigureSelection(blnOnlyOneRegister);
      ConfigureCheckboxSelectAll(blnOnlyOneRegister, blnAllLeadSource, blnAllPaymentTypes, blnAllPRs, blnAllChargeTo, blnAllGifts, blnAllRangeFolios, blnChkUsedate);
      ConfigureFilters(enumBasedOnArrival, enumQuinellas, enumDetailGifts, enumSaveCourtesyTours,
        enumSalesByMemberShipType, enumBasedOnBooking, enumExternalInvitation, blnFolSeries, blnFolFrom, blnFolTo, blnUseDates, blnAllFolios);
    }
    #endregion

    #region ConfigureDates
    /// <summary>
    /// Configura los controles de fecha
    /// </summary>
    private void ConfigureDates(bool blnOneDate, EnumPeriod enumPeriod)
    {
      //Si es un rango de fechas.
      if (!blnOneDate)
      {
        cboDate.IsEnabled = true;
        pnlDtmEnd.IsEnabled = true;
        cboDate.Items.Add("Dates Specified");
        switch (enumPeriod)
        {
          //Sin periodo
          case EnumPeriod.pdNone:
            cboDate.Items.Add("Today");
            cboDate.Items.Add("Yesterday");
            cboDate.Items.Add("This week");
            cboDate.Items.Add("Previous week");
            cboDate.Items.Add("This half");
            cboDate.Items.Add("Previous half");
            cboDate.Items.Add("This month");
            cboDate.Items.Add("Previous month");
            cboDate.Items.Add("This year");
            cboDate.Items.Add("Previous year");
            break;

          //Semanal
          case EnumPeriod.pdWeekly:
            cboDate.Items.Add("This week");
            cboDate.Items.Add("Previous week");
            cboDate.Items.Add("Two weeks ago");
            cboDate.Items.Add("Three weeks ago");
            Title += " (Weekly)";
            break;

          //Mensual
          case EnumPeriod.pdMonthly:
            cboDate.Items.Add("This month");
            cboDate.Items.Add("Previous month");
            cboDate.Items.Add("Two months ago");
            cboDate.Items.Add("Three months ago");
            Title += " (Monthly)";
            break;
        }
        cboDate.SelectedIndex = 0;
      }
      else
      {
        cboDate.Items.Add("Dates Specified");
        cboDate.SelectedIndex = 0;
        cboDate.IsEnabled = false;
        pnlDtmEnd.IsEnabled = false;
      }
    }
    #endregion

    #region ConfigureSelection
    /// <summary>
    /// Configura el modo de seleccion de los grids(Multiseleccion ó Solo un registro).
    /// Activa o desactiva los controles checkbox dependiendo el modo de seleccion configurado.
    /// </summary>
    private void ConfigureSelection(bool blnOnlyOneRegister)
    {
      grdLeadSources.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdPaymentTypes.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdPR.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdChargeTo.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdGifts.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
    }
    #endregion

    #region ConfigureCheckboxSelectAll
    /// <summary>
    /// Valida los checkbox para seleccionar todos los registros de los Grids
    /// </summary>
    private void ConfigureCheckboxSelectAll(bool blnOnlyOneRegister, bool blnAllLeadSources, bool blnAllPaymentTypes, bool blnAllPRs, bool blnAllChargeTo, bool blnAllGifts, bool blnAllRangeFolios, bool blnChkUseDate)
    {
      chkAllLeadSources.IsChecked = blnAllLeadSources;
      chkAllPaymentTypes.IsChecked = blnAllPaymentTypes;
      chkAllPR.IsChecked = blnAllPRs;
      chkAllChargeTo.IsChecked = blnAllChargeTo;
      chkAllGifts.IsChecked = blnAllGifts;
      chkUseDates.IsChecked = blnChkUseDate;

      chkAllLeadSources.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkAllPaymentTypes.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkAllPR.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkAllChargeTo.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkAllGifts.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkUseDates.IsEnabled = (blnOnlyOneRegister) ? false : true;
    }
    #endregion

    #region ConfigureFilters
    /// <summary>
    /// Configura los controles para los filtros de los reportes
    /// </summary>
    private void ConfigureFilters(EnumBasedOnArrival enumBasedOnArrival, EnumQuinellas enumQuinellas, EnumDetailGifts enumDetailGifts,
      EnumSaveCourtesyTours? enumSaveCourtesyTours, EnumSalesByMemberShipType? enumSalesMemberShipType, EnumBasedOnBooking enumBasedOnBooking,
      EnumExternalInvitation? enumExternalInvitation, bool blnFolSeries, bool blnFolFrom, bool blnFolTo, bool blnUseDates, bool blnAllFolios)
    {
      chkDetailGifts.Visibility = (enumDetailGifts == EnumDetailGifts.dgDetailGifts) ? Visibility.Visible : Visibility.Collapsed;
      chkBasedOnArrival.Visibility = (enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival) ? Visibility.Visible : Visibility.Collapsed;
      chkQuinellas.Visibility = (enumQuinellas == EnumQuinellas.quQuinellas) ? Visibility.Visible : Visibility.Collapsed;
      cboSaveCourtesyTours.Visibility = (enumSaveCourtesyTours != null) ? Visibility.Visible : Visibility.Collapsed;
      chkSalesByMembershipType.Visibility = (enumSalesMemberShipType == EnumSalesByMemberShipType.sbmDetail) ? Visibility.Visible : Visibility.Collapsed;
      chkBasedOnBooking.Visibility = (enumBasedOnBooking == EnumBasedOnBooking.bobBasedOnBooking) ? Visibility.Visible : Visibility.Collapsed;
      cboExternal.Visibility = (enumExternalInvitation != null) ? Visibility.Visible : Visibility.Collapsed;

      
      txtFolFrom.Visibility = lblFolFrom.Visibility = (blnFolFrom) ? Visibility.Visible : Visibility.Collapsed;
      txtFolTo.Visibility = lblFolTo.Visibility = (blnFolTo) ? Visibility.Visible : Visibility.Collapsed;
      chkUseDates.Visibility =  (blnUseDates) ? Visibility.Visible : Visibility.Collapsed;
      chkAllFolios.Visibility = (blnAllFolios) ? Visibility.Visible : Visibility.Collapsed;
     
      cboSaveCourtesyTours.Visibility = (enumSaveCourtesyTours != null) ? Visibility.Visible : Visibility.Collapsed;
      cboExternal.Visibility = (enumExternalInvitation != null) ? Visibility.Visible : Visibility.Collapsed;
      cboFolSeries.Visibility = lblFolSeries.Visibility = (blnFolSeries) ? Visibility.Visible : Visibility.Collapsed;
    }
    #endregion

    #endregion

    #region Eventos

    #region chkAllLeadSources_Checked
    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista leadsources
    /// </summary>
    private void chkAllLeadSources_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllLeadSources.IsChecked)
      {
        grdLeadSources.SelectAll();
        grdLeadSources.IsEnabled = false;
      }
      else {
        grdLeadSources.UnselectAll();
        grdLeadSources.IsEnabled = true;
      }
    }
    #endregion

    #region grdLeadSources_SelectionChanged
    private void grdLeadSources_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumLS.Content = string.Format("{0}/{1} Selected LeadSources", grdLeadSources.SelectedItems.Count, grdLeadSources.Items.Count);
    }
    #endregion

    #region chkAllPaymentTypes_Checked
    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista formas de pago
    /// </summary>
    private void chkAllPaymentTypes_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllPaymentTypes.IsChecked)
      {
        grdPaymentTypes.SelectAll();
        grdPaymentTypes.IsEnabled = false;
      }
      else
      {
        grdPaymentTypes.UnselectAll();
        grdPaymentTypes.IsEnabled = true;
      }
    }
    #endregion

    #region grdPaymentTypes_SelectionChanged
    private void grdPaymentTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumPT.Content = string.Format("{0}/{1} Selected PaymentTypes", grdPaymentTypes.SelectedItems.Count, grdPaymentTypes.Items.Count);
    }
    #endregion

    #region chkAllPR_Checked
    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista PRs
    /// </summary>
    private void chkAllPR_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllPR.IsChecked)
      {
        grdPR.SelectAll();
        grdPR.IsEnabled = false;
      }
      else
      {
        grdPR.UnselectAll();
        grdPR.IsEnabled = true;
      }
    }
    #endregion

    #region grdPR_SelectionChanged
    private void grdPR_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumPR.Content = string.Format("{0}/{1} Selected PRs", grdPR.SelectedItems.Count, grdPR.Items.Count);
    }
    #endregion

    #region chkAllChargeTo_Checked
    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista Cargar a
    /// </summary>
    private void chkAllChargeTo_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllChargeTo.IsChecked)
      {
        grdChargeTo.SelectAll();
        grdChargeTo.IsEnabled = false;
      }
      else
      {
        grdChargeTo.UnselectAll();
        grdChargeTo.IsEnabled = true;
      }
    }
    #endregion

    #region grdChargeTo_SelectionChanged
    private void grdChargeTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumCT.Content = string.Format("{0}/{1} Selected Charge To", grdChargeTo.SelectedItems.Count, grdChargeTo.Items.Count);
    }
    #endregion

    #region chkAllGifts_Checked
    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista regalos
    /// </summary>
    private void chkAllGifts_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllGifts.IsChecked)
      {
        grdGifts.SelectAll();
        grdGifts.IsEnabled = false;
      }
      else
      {
        grdGifts.UnselectAll();
        grdGifts.IsEnabled = true;
      }
    }
    #endregion

    #region grdGifts_SelectionChanged
    private void grdGifts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumG.Content = string.Format("{0}/{1} Selected Gifts", grdGifts.SelectedItems.Count, grdGifts.Items.Count);
    }
    #endregion

    #region chkUseDates_Checked
    /// <summary>
    ///  Valida si se habilitan el filtro de fechas
    /// </summary>
    private void chkUseDates_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkUseDates.IsChecked)
      {
        cboDate.IsEnabled = true;
        dtmStart.IsEnabled = true;
        dtmEnd.IsEnabled = true;
      }
      else
      {
        cboDate.IsEnabled = false;
        dtmStart.IsEnabled = false;
        dtmEnd.IsEnabled = false;
      }
    }
    #endregion

    #region cboDate_SelectionChanged
    /// <summary>
    /// Modifica los rangos de fecha de los datepicker, según la opcion seleccionada.
    /// </summary>
    private void cboDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      EnumPredefinedDate selected = (EnumPredefinedDate)((ComboBox)e.OriginalSource).SelectedIndex;
      DateTime today = BRHelpers.GetServerDate();
      pnlDtmStart.IsEnabled = false;
      pnlDtmEnd.IsEnabled = false;
      switch (selected)
      {
        case EnumPredefinedDate.daToday:
          dtmStart.Value = dtmEnd.Value = today;
          break;
        case EnumPredefinedDate.daYesterday:
          dtmStart.Value = dtmEnd.Value = today.Date.AddDays(-1);
          break;
        case EnumPredefinedDate.daThisWeek:
          dtmStart.Value = today.AddDays((DayOfWeek.Monday - today.DayOfWeek));
          dtmEnd.Value = today.AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;
        case EnumPredefinedDate.daPreviousWeek:
          dtmStart.Value = today.AddDays(-7).AddDays(DayOfWeek.Monday - today.DayOfWeek);
          dtmEnd.Value = today.AddDays(-7).AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;
        case EnumPredefinedDate.daThisHalf:
          if (today.Day <= 15)
          {
            dtmStart.Value = new DateTime(today.Year, today.Month, 1);
            dtmEnd.Value = new DateTime(today.Year, today.Month, 15);
          }
          else {
            dtmStart.Value = new DateTime(today.Year, today.Month, 16);
            dtmEnd.Value = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
          }
          break;
        case EnumPredefinedDate.daPreviousHalf:

          if (today.Day <= 15)
          {
            if (today.Month > 1)
            {
              dtmStart.Value = new DateTime(today.Year, today.Month - 1, 16);
              dtmEnd.Value = new DateTime(today.Year, today.Month - 1, DateTime.DaysInMonth(today.Year, today.Month - 1));
            }
            else
            {
              dtmStart.Value = new DateTime(today.Year - 1, 12, 16);
              dtmEnd.Value = new DateTime(today.Year - 1, 12, 31);
            }
          }
          else
          {
            dtmStart.Value = new DateTime(today.Year, today.Month, 1);
            dtmEnd.Value = new DateTime(today.Year, today.Month, 15);
          }
          break;
        case EnumPredefinedDate.daThisMonth:
          dtmStart.Value = new DateTime(today.Year, today.Month, 1);
          dtmEnd.Value = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
          break;
        case EnumPredefinedDate.daPreviousMonth:
          dtmStart.Value = new DateTime(today.Year, today.Month - 1, 1);
          dtmEnd.Value = new DateTime(today.Year, today.Month - 1, DateTime.DaysInMonth(today.Year, today.Month - 1));
          break;
        case EnumPredefinedDate.daThisYear:
          dtmStart.Value = new DateTime(today.Year, 1, 1);
          dtmEnd.Value = new DateTime(today.Year, 12, 31);
          break;
        case EnumPredefinedDate.daPreviousYear:
          dtmStart.Value = new DateTime(today.Year - 1, 1, 1);
          dtmEnd.Value = new DateTime(today.Year - 1, 12, 31);
          break;
        default:
          pnlDtmStart.IsEnabled = pnlDtmEnd.IsEnabled = true;
          break;
      }
    }
    #endregion

    #region btnOK_Click
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #region Close_keyPreview
    private void Close_KeyPreviewESC(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        Close();
    }
    #endregion

    #endregion

  }
}
