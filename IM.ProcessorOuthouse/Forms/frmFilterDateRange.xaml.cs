using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.ProcessorOuthouse.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.ProcessorOuthouse.Forms
{
  /// <summary>
  /// Formulario con el filtro de fechas, LeadSources, Formas de pago, PRs, Cargar a,
  /// Regalos
  /// </summary>
  /// <history>
  /// [vku] 22/Mar/2016 Created
  /// </history>
  public partial class frmFilterDateRange : Window
  {
    #region Contructor

    public frmFilterDateRange()
    {
      InitializeComponent();
      txtFolFrom.PreviewTextInput += TextBoxHelper.IntTextInput;
      txtFolTo.PreviewTextInput += TextBoxHelper.IntTextInput;
      this.PreviewKeyDown += new KeyEventHandler(Close_KeyPreviewESC);
    }

    #endregion Contructor

    #region Atributos

    private List<LeadSourceByUser> _lstLeadSources = new List<LeadSourceByUser>();
    private List<LeadSource> _lstLeadSourcesPaymentComm = new List<LeadSource>();
    private List<PaymentType> _lstPaymentType = new List<PaymentType>();
    private List<PersonnelShort> _lstPRs = new List<PersonnelShort>();
    private List<ChargeTo> _lstChargeTo = new List<ChargeTo>();
    private List<GiftShort> _lstGifts, _lstGiftsProdGift = new List<GiftShort>();
    private List<FolioInvitationOuthouse> _lstFoliosInvitationOuthouse = new List<FolioInvitationOuthouse>();
    private List<FolioCxCPR> _lstRangeFolios = new List<FolioCxCPR>();
    private FolioInvitationOuthouse _folioInvOutFilter = new FolioInvitationOuthouse();
    private ChargeTo _chargeToFilter = new ChargeTo();
    public bool _blnOK = false;
    public frmProcessorOuthouse frmPO = new frmProcessorOuthouse();

    #endregion Atributos

    #region Metodos

    #region ConfigureForm

    /// <summary>
    /// Configura los controles del formulario
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    public void ConfigureForm(bool blnLeadSource = false, bool blnLeadSourcesPaymentComm = false, bool blnAllLeadSource = false, bool blnAllLeadSourcePaymentComm = false,
      bool blnPaymentTypes = false, bool blnAllPaymentTypes = false, bool blnPRs = false, bool blnAllPRs = false,
      bool blnChargeTo = false, bool blnAllChargeTo = false, bool blnGifts = false, bool blnAllGifts = false, bool blnGiftProdGift = false, bool blnAllGiftProdGift = false, bool blnRangeFolios = false, bool blnAllRangeFolios = false,
      bool blnOneDate = false, bool blnOnlyOneRegister = false, bool blnChkUsedate = false, EnumPeriod enumPeriod = EnumPeriod.None,
      EnumProgram enumPrograms = EnumProgram.Outhouse, bool blnOnePeriod = false, EnumBasedOnArrival enumBasedOnArrival = EnumBasedOnArrival.NoBasedOnArrival,
      EnumQuinellas enumQuinellas = EnumQuinellas.NoQuinellas, EnumDetailGifts enumDetailGifts = EnumDetailGifts.NoDetailGifts, EnumSaveCourtesyTours? enumSaveCourtesyTours = null,
      EnumSalesByMemberShipType enumSalesByMemberShipType = EnumSalesByMemberShipType.NoDetail, EnumBasedOnBooking enumBasedOnBooking = EnumBasedOnBooking.NoBasedOnBooking,
      EnumExternalInvitation? enumExternalInvitation = null, bool blnFolSeries = false, bool blnFolFrom = false, bool blnFolTo = false, bool blnUseDates = false, bool blnAllFolios = false)
    {
      ConfigureDates(blnOneDate, enumPeriod);
      ConfigureFilters(enumBasedOnArrival, enumQuinellas, enumDetailGifts, enumSaveCourtesyTours,
      enumSalesByMemberShipType, enumBasedOnBooking, enumExternalInvitation, blnFolSeries, blnFolFrom, blnFolTo, blnUseDates, blnAllFolios);

      LoadCombos();

      #region Configuracion de grids
      LoadLeadSources(blnOnlyOneRegister, blnLeadSource, blnAllLeadSource, enumPrograms);
      LoadLeadSourcesPaymentCoom(blnOnlyOneRegister, blnLeadSourcesPaymentComm, blnAllLeadSourcePaymentComm);
      LoadPaymentTypes(blnOnlyOneRegister, blnPaymentTypes, blnAllPaymentTypes);
      LoadPRs(blnOnlyOneRegister, blnPRs, blnAllPRs);
      LoadChargeTo(blnOnlyOneRegister, blnChargeTo, blnAllChargeTo);
      LoadGifts(blnOnlyOneRegister, blnGifts, blnAllGifts);
      LoadGiftsProdGift(blnOnlyOneRegister, blnGiftProdGift, blnAllGiftProdGift);
      #endregion

      LoadUserFilters();
    }

    #endregion ConfigureForm

    #region ConfigureDates

    /// <summary>
    /// Configura los controles de fecha
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void ConfigureDates(bool blnOneDate, EnumPeriod enumPeriod)
    {
      Dictionary<EnumPredefinedDate, string> dictionaryPredefinedDate = EnumToListHelper.GetList<EnumPredefinedDate>();

      cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.DatesSpecified));

      switch (enumPeriod)
      {
        //Sin periodo
        case EnumPeriod.None:

          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.Today));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.Yesterday));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisWeek));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousWeek));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisHalf));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousHalf));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisMonth));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousMonth));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisYear));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousYear));
          break;

        //Semanal
        case EnumPeriod.Weekly:
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisWeek));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousWeek));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.TwoWeeksAgo));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThreeWeeksAgo));
          Title += $" ({EnumToListHelper.GetEnumDescription(enumPeriod)})";
          break;

        //Mensual
        case EnumPeriod.Monthly:
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisMonth));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousMonth));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.TwoMonthsAgo));
          cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThreeMonthsAgo));
          Title += $" ({EnumToListHelper.GetEnumDescription(enumPeriod)})";
          break;
      }
      cboDate.SelectedIndex = 0;
      //Si es un rango de fechas.
      cboDate.IsEnabled = pnlDtmEnd.IsEnabled = !blnOneDate;
    }

    #endregion ConfigureDates

    #region ConfigureFilters

    /// <summary>
    /// Configura los controles para los filtros de los reportes
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void ConfigureFilters(EnumBasedOnArrival enumBasedOnArrival, EnumQuinellas enumQuinellas, EnumDetailGifts enumDetailGifts,
      EnumSaveCourtesyTours? enumSaveCourtesyTours, EnumSalesByMemberShipType? enumSalesMemberShipType, EnumBasedOnBooking enumBasedOnBooking,
      EnumExternalInvitation? enumExternalInvitation, bool blnFolSeries, bool blnFolFrom, bool blnFolTo, bool blnUseDates, bool blnAllFolios)
    {
      chkDetailGifts.Visibility = (enumDetailGifts == EnumDetailGifts.DetailGifts) ? Visibility.Visible : Visibility.Collapsed;
      chkBasedOnArrival.Visibility = (enumBasedOnArrival == EnumBasedOnArrival.BasedOnArrival) ? Visibility.Visible : Visibility.Collapsed;
      chkQuinellas.Visibility = (enumQuinellas == EnumQuinellas.Quinellas) ? Visibility.Visible : Visibility.Collapsed;
      cboSaveCourtesyTours.Visibility = (enumSaveCourtesyTours != null) ? Visibility.Visible : Visibility.Collapsed;
      chkSalesByMembershipType.Visibility = (enumSalesMemberShipType == EnumSalesByMemberShipType.Detail) ? Visibility.Visible : Visibility.Collapsed;
      chkBasedOnBooking.Visibility = (enumBasedOnBooking == EnumBasedOnBooking.BasedOnBooking) ? Visibility.Visible : Visibility.Collapsed;
      cboExternal.Visibility = (enumExternalInvitation != null) ? Visibility.Visible : Visibility.Collapsed;

      
      txtFolFrom.Visibility = lblFolFrom.Visibility = (blnFolFrom) ? Visibility.Visible : Visibility.Collapsed;
      txtFolTo.Visibility = lblFolTo.Visibility = (blnFolTo) ? Visibility.Visible : Visibility.Collapsed;
      chkUseDates.Visibility = (blnUseDates) ? Visibility.Visible : Visibility.Collapsed;
      chkUseDates.IsChecked = chkUseDates.IsChecked = (blnUseDates) ? true : false;
      chkAllFolios.Visibility = (blnAllFolios) ? Visibility.Visible : Visibility.Collapsed;

      cboSaveCourtesyTours.Visibility = (enumSaveCourtesyTours != null) ? Visibility.Visible : Visibility.Collapsed;
      cboExternal.Visibility = (enumExternalInvitation != null) ? Visibility.Visible : Visibility.Collapsed;
      cboFolSeries.Visibility = lblFolSeries.Visibility = (blnFolSeries) ? Visibility.Visible : Visibility.Collapsed;
    }

    #endregion ConfigureFilters

    #region SaveFrmFilterValues

    /// <summary>
    ///  Guarda los datos seleccionados por el usuario
    /// </summary>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    public void SaveFrmFilterValues()
    {
      if (!chkAllLeadSources.IsChecked.Value)
        frmPO._lstLeadSources = grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList();
      if (!chkAllLeadSourcesPaymentComm.IsChecked.Value)
        frmPO._lstLeadSourcesPaymentComm = grdLeadSourcesPaymentComm.SelectedItems.Cast<LeadSource>().Select(c => grdLeadSourcesPaymentComm.Items.IndexOf(c)).ToList();
      if (!chkAllPaymentTypes.IsChecked.Value)
        frmPO._lstPaymentTypes = grdPaymentTypes.SelectedItems.Cast<PaymentType>().Select(c => grdPaymentTypes.Items.IndexOf(c)).ToList();
      if (!chkAllPR.IsChecked.Value)
        frmPO._lstPRs = grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => grdPR.Items.IndexOf(c)).ToList();
      if (!chkAllChargeTo.IsChecked.Value)
        frmPO._lstChargeTo = grdChargeTo.SelectedItems.Cast<ChargeTo>().Select(c => grdGifts.Items.IndexOf(c)).ToList();
      if (!chkAllGifts.IsChecked.Value)
        frmPO._lstGifts = grdGifts.SelectedItems.Cast<GiftShort>().Select(c => grdGifts.Items.IndexOf(c)).ToList();
      if (!chkAllGiftsProdGift.IsChecked.Value)
        frmPO._lstGiftsProdGift = grdGiftsProdGift.SelectedItems.Cast<GiftShort>().Select(c => grdGiftsProdGift.Items.IndexOf(c)).ToList();
      frmPO._cboDateSelected = (EnumPredefinedDate)cboDate.SelectedValue;
      frmPO._cboFolSeriesSelected = cboFolSeries.SelectedValue.ToString();
      frmPO._dtmStart = dtmStart.Value.Value;
      frmPO._dtmEnd = dtmEnd.Value.Value;
      frmPO._enumBasedOnArrival = (chkBasedOnArrival.IsChecked.Value) ? EnumBasedOnArrival.BasedOnArrival : EnumBasedOnArrival.NoBasedOnArrival;
      frmPO._enumBasedOnBooking = (chkBasedOnBooking.IsChecked.Value) ? EnumBasedOnBooking.BasedOnBooking : EnumBasedOnBooking.NoBasedOnBooking;
      frmPO._enumQuinellas = (chkQuinellas.IsChecked.Value) ? EnumQuinellas.Quinellas : EnumQuinellas.NoQuinellas;
      frmPO._enumDetailsGift = (chkDetailGifts.IsChecked.Value) ? EnumDetailGifts.DetailGifts : EnumDetailGifts.NoDetailGifts;
      frmPO._enumSalesByMemberShipType = (chkSalesByMembershipType.IsChecked.Value) ? EnumSalesByMemberShipType.Detail : EnumSalesByMemberShipType.NoDetail;
      frmPO._folFrom = (txtFolFrom.Text!=null) ? txtFolFrom.Text : "0" ;
      frmPO._folTo = (txtFolTo.Text!=null) ? txtFolTo.Text : "0";
      frmPO._enumSaveCourtesyTours = ((KeyValuePair<EnumSaveCourtesyTours, string>)cboSaveCourtesyTours.SelectedItem).Key;
      frmPO._enumExternalInvitation = ((KeyValuePair<EnumExternalInvitation, string>)cboExternal.SelectedItem).Key;
    }

    #endregion SaveFrmFilterValues

    #region LoadUserFilters

    /// <summary>
    /// Obtiene los filtros que el usuario habiá seleccionado
    /// y los aplica al formulario.
    /// </summary>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    public void LoadUserFilters()
    {
      cboDate.SelectedValue = frmPO._cboDateSelected ?? EnumPredefinedDate.DatesSpecified;
      cboFolSeries.SelectedValue = (frmPO._cboFolSeriesSelected != null) ? frmPO._cboFolSeriesSelected : "ALL";
      dtmStart.Value = frmPO._dtmStart;
      dtmEnd.Value = frmPO._dtmEnd;
      chkBasedOnArrival.IsChecked = (frmPO._enumBasedOnArrival == EnumBasedOnArrival.BasedOnArrival) ? true : false;
      chkBasedOnBooking.IsChecked = (frmPO._enumBasedOnBooking == EnumBasedOnBooking.BasedOnBooking) ? true : false;
      chkQuinellas.IsChecked = (frmPO._enumQuinellas == EnumQuinellas.Quinellas) ? true : false;
      chkDetailGifts.IsChecked = (frmPO._enumDetailsGift == EnumDetailGifts.DetailGifts) ? true : false;
      chkSalesByMembershipType.IsChecked = (frmPO._enumSalesByMemberShipType == EnumSalesByMemberShipType.Detail) ? true : false;

      txtFolFrom.Text = (frmPO._folFrom != null) ? frmPO._folFrom : "0";
      txtFolTo.Text = (frmPO._folTo != null) ? frmPO._folTo : "0";

      cboSaveCourtesyTours.SelectedValue = frmPO._enumSaveCourtesyTours;
      cboExternal.SelectedValue = frmPO._enumExternalInvitation;
    }

    #endregion LoadUserFilters

    #region ValidateFields

    /// <summary>
    /// Valida si los grid tienen al menos un elemento seleccionado.
    /// </summary>
    /// <returns>Message/Empty</returns>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    public string ValidateFields()
    {
      if (pnlLeadSources.Visibility == Visibility.Visible && grdLeadSources.SelectedItems.Count == 0)
        return "No lead source is selected.";
      if (pnlLeadSourcesPaymentComm.Visibility == Visibility.Visible && grdLeadSourcesPaymentComm.SelectedItems.Count == 0)
        return "No lead source is selected.";
      if (pnlPaymentTypes.Visibility == Visibility.Visible && grdPaymentTypes.SelectedItems.Count == 0)
        return "No payment Types is selected";
      if (pnlPR.Visibility == Visibility.Visible && grdPR.SelectedItems.Count == 0)
        return "No PR is selected.";
      if (pnlChargeTo.Visibility == Visibility.Visible && grdChargeTo.SelectedItems.Count == 0)
        return "No Charge To is selected.";
      if (pnlGifts.Visibility == Visibility.Visible && grdGifts.SelectedItems.Count == 0)
        return "No gift is selected.";
      if (pnlGiftsProdGift.Visibility == Visibility.Visible && grdGiftsProdGift.SelectedItems.Count == 0)
        return "No gift is selected.";
      else
        return "";
    }

    #endregion ValidateFields

    #region LoadCombos
    /// <summary>
    ///   Carga los Combos
    /// </summary>
    /// <history>
    ///   [vku] 25/May/2016 Created
    /// </history>
    private void LoadCombos()
    {
      _lstFoliosInvitationOuthouse = BRFoliosInvitationsOuthouse.GetFoliosInvittionsOutside(nStatus: 1).ToList();
      _lstFoliosInvitationOuthouse.Insert(0, new FolioInvitationOuthouse { fiID = 0, fiSerie = "ALL", fiFrom = 0, fiTo = 0, fiA = Convert.ToBoolean(1) });
      cboFolSeries.ItemsSource = _lstFoliosInvitationOuthouse.Select(c => c.fiSerie).Distinct();
      cboSaveCourtesyTours.ItemsSource = EnumToListHelper.GetList<EnumSaveCourtesyTours>();
      cboExternal.ItemsSource = EnumToListHelper.GetList<EnumExternalInvitation>();
    }
    #endregion

    #region LoadLeadSources
    /// <summary>
    ///  Carga y configuración del grid LeadSources
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnLeadSources"></param>
    /// <param name="blnAllLeadSources"></param>
    /// <param name="program"></param>
    /// <history>
    ///   [vku] 25/May/2016 Created
    /// </history>
    private async void LoadLeadSources(bool blnOnlyOneRegister, bool blnLeadSource, bool blnAllLeadSources, EnumProgram program = EnumProgram.Outhouse)
    {
      if (!blnLeadSource) return;

      grdLeadSources.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlLeadSources.Visibility = Visibility.Visible;
      _lstLeadSources = await BRLeadSources.GetLeadSourcesByUser(App.User.User.peID, program, "ALL");
      grdLeadSources.ItemsSource = _lstLeadSources;

      chkAllLeadSources.IsChecked = blnAllLeadSources;
      chkAllLeadSources.IsEnabled = !blnOnlyOneRegister;

      if (grdLeadSources.ItemsSource != null && chkAllLeadSources.IsChecked != null && (!chkAllLeadSources.IsChecked.Value && chkAllLeadSources.IsEnabled))
      {
        frmPO._lstLeadSources.ForEach(c =>
        {
          grdLeadSources.SelectedItems.Add(_lstLeadSources.SingleOrDefault(x => x.lsID == c));
        });
      }
    }
    #endregion

    #region LoadLeadSourcesPaymentComm
    /// <summary>
    ///  Carga y configuración del grid LeadSourcesPaymentCoom
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnLeadSourcesPaymentComm"></param>
    /// <param name="blnAllLeadSourcesPaymentComm"></param>
    /// <history>
    ///   [vku] 25/May/2016 Created 
    /// </history>
    private void LoadLeadSourcesPaymentCoom(bool blnOnlyOneRegister, bool blnLeadSourcesPaymentComm, bool blnAllLeadSourcesPaymentComm)
    {
      if (!blnLeadSourcesPaymentComm) return;

      grdLeadSourcesPaymentComm.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlLeadSourcesPaymentComm.Visibility = Visibility.Visible;
      List<string> _paymentComm = GetSettings.PRPaymentCommissions();
      _lstLeadSourcesPaymentComm = BRLeadSources.GetLeadSourceById(_paymentComm);
      grdLeadSourcesPaymentComm.ItemsSource = _lstLeadSourcesPaymentComm;

      chkAllLeadSourcesPaymentComm.IsChecked = blnAllLeadSourcesPaymentComm;
      chkAllLeadSourcesPaymentComm.IsEnabled = !blnOnlyOneRegister;

      if (grdLeadSourcesPaymentComm.ItemsSource != null && chkAllLeadSourcesPaymentComm.IsChecked != null && (!chkAllLeadSourcesPaymentComm.IsChecked.Value && chkAllLeadSourcesPaymentComm.IsEnabled))
      {
        frmPO._lstLeadSourcesPaymentComm.ForEach(c =>
        {
          grdLeadSourcesPaymentComm.SelectedItems.Add(grdLeadSourcesPaymentComm.Items.GetItemAt(c));
        });
      }
    }
    #endregion

    #region LoadPaymentTypes
    /// <summary>
    ///  Carga y configuración del grid PaymentTipes
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnPaymentTypes"></param>
    /// <param name="blnAllPaymentTypes"></param>
    /// <history>
    ///   [vku] 25/May/2016 Created
    /// </history>
    private async void LoadPaymentTypes(bool blnOnlyOneRegister, bool blnPaymentTypes, bool blnAllPaymentTypes)
    {
      if (!blnPaymentTypes) return;

      grdPaymentTypes.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlPaymentTypes.Visibility = Visibility.Visible;
      _lstPaymentType = await BRPaymentTypes.GetPaymentTypes(-1);
      grdPaymentTypes.ItemsSource = _lstPaymentType;

      chkAllPaymentTypes.IsChecked = blnAllPaymentTypes;
      chkAllPaymentTypes.IsEnabled = !blnOnlyOneRegister;

      if (grdPaymentTypes.ItemsSource != null && chkAllPaymentTypes.IsChecked != null && (!chkAllPaymentTypes.IsChecked.Value && chkAllPaymentTypes.IsEnabled))
      {
        frmPO._lstPaymentTypes.ForEach(c =>
        {
          grdPaymentTypes.SelectedItems.Add(grdPaymentTypes.Items.GetItemAt(c));
        });
      }
    }
    #endregion

    #region LoadPRs
    /// <summary>
    ///  Carga y configuracion del grid PRs
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnPRs"></param>
    /// <param name="blnAllPRs"></param>
    /// <history>
    ///   [vku] 25/May/2016 Created
    /// </history>
    private async void LoadPRs(bool blnOnlyOneRegister, bool blnPRs, bool blnAllPRs)
    {
      if (!blnPRs) return;

      grdPR.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlPR.Visibility = Visibility.Visible;
      var x = (await BRLeadSources.GetLeadSourcesByUser(App.User.User.peID, EnumProgram.Outhouse)).Select(y => y.lsID).ToList();
      _lstPRs = await BRPersonnel.GetPersonnel(string.Join(",", x), roles: EnumToListHelper.GetEnumDescription(EnumRole.PR), status: 0);
      grdPR.ItemsSource = _lstPRs;

      chkAllPR.IsChecked = blnAllPRs;
      chkAllPR.IsEnabled = !blnOnlyOneRegister;

      if (grdPR.ItemsSource != null && chkAllPR.IsChecked != null && (!chkAllPR.IsChecked.Value && chkAllPR.IsEnabled))
      {
        frmPO._lstPRs.ForEach(c =>
        {
          grdPR.SelectedItems.Add(grdPR.Items.GetItemAt(c));
        });
      }
    }
    #endregion

    #region LoadChargeTo
    /// <summary>
    ///  Carga y configuracion del grid ChargeTo
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnChargeTo"></param>
    /// <param name="blnAllChargeTo"></param>
    /// <history>
    ///   [vku] 25/May/2016 Created
    /// </history>
    private async void LoadChargeTo(bool blnOnlyOneRegister, bool blnChargeTo, bool blnAllChargeTo)
    {
      if (!blnAllChargeTo) return;

      grdChargeTo.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlChargeTo.Visibility = Visibility.Visible;
      _lstChargeTo = await BRChargeTos.GetChargeTos(_chargeToFilter, -1);
      grdChargeTo.ItemsSource = _lstChargeTo;

      chkAllChargeTo.IsChecked = blnAllChargeTo;
      chkAllChargeTo.IsEnabled = !blnOnlyOneRegister;

      if (grdChargeTo.ItemsSource != null && chkAllChargeTo.IsChecked != null && (!chkAllChargeTo.IsChecked.Value && chkAllChargeTo.IsEnabled))
      {
        frmPO._lstChargeTo.ForEach(c =>
        {
          grdChargeTo.SelectedItems.Add(grdChargeTo.Items.GetItemAt(c));
        });
      }
    }
    #endregion

    #region LoadGifts
    /// <summary>
    ///  Carga y configuración del grid Gifts
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnGifts"></param>
    /// <param name="blnAllGifts"></param>
    /// <history>
    ///   [vku] 25/May/2016 Created
    /// </history>
    private async void LoadGifts(bool blnOnlyOneRegister, bool blnGifts, bool blnAllGifts)
    {
      if (!blnGifts) return;

      grdGifts.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlGifts.Visibility = Visibility.Visible;
      _lstGifts = await BRGifts.GetGiftsShort();
      grdGifts.ItemsSource = _lstGifts;

      chkAllGifts.IsChecked = blnAllGifts;
      chkAllGifts.IsEnabled = !blnOnlyOneRegister;

      if (grdGifts.ItemsSource != null && chkAllGifts.IsChecked != null && (!chkAllGifts.IsChecked.Value && chkAllGifts.IsEnabled))
      {
        frmPO._lstGifts.ForEach(c =>
        {
          grdGifts.SelectedItems.Add(grdGifts.Items.GetItemAt(c));
        });
      }
    }
    #endregion

    #region LoadGiftsProdGift
    /// <summary>
    ///  Carga y configuración del grid GiftsProdGifts
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnGiftsProdGift"></param>
    /// <param name="blnAllGiftsProdGift"></param>
    /// <history>
    ///   [vku] 25/May/2016 Created
    /// </history>
    private async void LoadGiftsProdGift(bool blnOnlyOneRegister, bool blnGiftProdGift, bool blnAllGiftsProdGift)
    {
      if (!blnGiftProdGift) return;

      grdGiftsProdGift.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlGiftsProdGift.Visibility = Visibility.Visible;
      List<string> _prodByGift = GetSettings.ProductionByGift();
      _lstGiftsProdGift = await BRGifts.GetGiftsShortById(_prodByGift);
      grdGiftsProdGift.ItemsSource = _lstGiftsProdGift;

      chkAllGiftsProdGift.IsChecked = blnAllGiftsProdGift;
      chkAllGiftsProdGift.IsEnabled = !blnOnlyOneRegister;

      if (grdGiftsProdGift.ItemsSource != null && chkAllGiftsProdGift.IsChecked != null && (!chkAllGiftsProdGift.IsChecked.Value && chkAllGiftsProdGift.IsEnabled))
      {
        frmPO._lstGiftsProdGift.ForEach(c =>
        {
          grdGiftsProdGift.SelectedItems.Add(grdGiftsProdGift.Items.GetItemAt(c));
        });
      }
    }
    #endregion

    #endregion Metodos

    #region Eventos

    #region chkAllLeadSources_Checked

    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista leadsources
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void chkAllLeadSources_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllLeadSources.IsChecked)
      {
        grdLeadSources.SelectAll();
        grdLeadSources.IsEnabled = false;
      }
      else
      {
        grdLeadSources.UnselectAll();
        grdLeadSources.IsEnabled = true;
      }
    }

    #endregion chkAllLeadSources_Checked

    #region chkAllLeadSourcesPaymentComm_Checked

    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista leadsources
    /// </summary>
    /// <history>
    ///   [vku] 20/Abr/2016 Created
    /// </history>
    private void chkAllLeadSourcesPaymentComm_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllLeadSourcesPaymentComm.IsChecked)
      {
        grdLeadSourcesPaymentComm.SelectAll();
        grdLeadSourcesPaymentComm.IsEnabled = false;
      }
      else
      {
        grdLeadSourcesPaymentComm.UnselectAll();
        grdLeadSourcesPaymentComm.IsEnabled = true;
      }
    }

    #endregion chkAllLeadSourcesPaymentComm_Checked

    #region chkAllPaymentTypes_Checked

    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista formas de pago
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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

    #endregion chkAllPaymentTypes_Checked

    #region chkAllPR_Checked

    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista PRs
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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

    #endregion chkAllPR_Checked
   
    #region chkAllChargeTo_Checked

    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista Cargar a
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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

    #endregion chkAllChargeTo_Checked

    #region chkAllGifts_Checked

    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista regalos
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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

    #endregion chkAllGifts_Checked

    #region chkAllGiftsProdGift_Checked

    /// <summary>
    ///  Valida si el checkbox ALL fua activo o desactivado para seleccionar o
    ///  deselecionar la lista regalos
    /// </summary>
    /// <history>
    ///   [vku] 21/Abr/2016 Created
    /// </history>
    private void chkAllGiftsProdGift_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllGiftsProdGift.IsChecked)
      {
        grdGiftsProdGift.SelectAll();
        grdGiftsProdGift.IsEnabled = false;
      }
      else
      {
        grdGiftsProdGift.UnselectAll();
        grdGiftsProdGift.IsEnabled = true;
      }
    }

    #endregion chkAllGiftsProdGift_Checked

    #region chkUseDates_Checked

    /// <summary>
    ///  Valida si se habilitan el filtro de fechas
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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

    #endregion chkUseDates_Checked

    #region cboDate_SelectionChanged

    /// <summary>
    /// Modifica los rangos de fecha de los datepicker, según la opcion seleccionada.
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void cboDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count <= 0) return;
      EnumPredefinedDate selected = (EnumPredefinedDate)((ComboBox)e.OriginalSource).SelectedValue;
      var dateRange = DateHelper.GetDateRange(selected);
      pnlDtmStart.IsEnabled = pnlDtmEnd.IsEnabled = (selected == EnumPredefinedDate.DatesSpecified);
      dtmStart.Value = dateRange.Item1;
      dtmEnd.Value = dateRange.Item2;
    }

    #endregion cboDate_SelectionChanged

    #region btnOK_Click

    /// <summary>
    /// Devuelve un booleano para saber si se ha terminado de
    /// realizar su proceso de filtrado.
    /// </summary>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      string message = ValidateFields();
      if (message == "")
      {
        _blnOK = true;
        SaveFrmFilterValues();
        Close();
      }
      else
        UIHelper.ShowMessage(message);
    }

    #endregion btnOK_Click

    #region btnCancel_Click

    /// <summary>
    /// Devuelve un booleano para saber si se ha terminado de
    /// realizar su proceso de filtrado.
    /// </summary>
    /// <history>
    ///   [vku] 08/Abr/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      _blnOK = false;
      Close();
    }

    #endregion btnCancel_Click

    #region Close_keyPreview

    private void Close_KeyPreviewESC(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        Close();
    }

    #endregion Close_keyPreview

    #region Window_Closing

    /// <summary>
    /// Guarda los valores de cada control, antes de cerrar el formulario.
    /// </summary>
    /// <history>
    ///   [vku] 05/Abr/2016 Created
    /// </history>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      SaveFrmFilterValues();
    }

    #endregion Window_Closing

    #endregion Eventos
  }
}