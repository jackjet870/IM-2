﻿using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    public void ConfigureForm(bool blnLeadSource = false, bool blnAllLeadSource = false,
      bool blnPaymentTypes = false, bool blnAllPaymentTypes = false, bool blnPRs = false, bool blnAllPRs = false,
      bool blnChargeTo = false, bool blnAllChargeTo = false, bool blnGifts = false, bool blnAllGifts = false, bool blnRangeFolios = false, bool blnAllRangeFolios = false,
      bool blnOneDate = false, bool blnOnlyOneRegister = false, bool blnChkUsedate = false, EnumPeriod enumPeriod = EnumPeriod.pdNone,
      EnumProgram enumPrograms = EnumProgram.All, bool blnOnePeriod = false, EnumBasedOnArrival enumBasedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival,
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
      LoadUserFilters();
    }
    #endregion

    #region ConfigureDates
    /// <summary>
    /// Configura los controles de fecha
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void ConfigureDates(bool blnOneDate, EnumPeriod enumPeriod)
    {
      ///Si es un rango de fechas.
      if (!blnOneDate)
      {
        cboDate.IsEnabled = true;
        pnlDtmEnd.IsEnabled = true;
        cboDate.Items.Add("Dates Specified");
        switch (enumPeriod)
        {
          ///Sin periodo
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
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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
        frmPO._lstLeadSources = grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => grdLeadSources.Items.IndexOf(c)).ToList();
      if (!chkAllPaymentTypes.IsChecked.Value)
        frmPO._lstPaymentTypes = grdPaymentTypes.SelectedItems.Cast<PaymentType>().Select(c => grdPaymentTypes.Items.IndexOf(c)).ToList();
      if (!chkAllPR.IsChecked.Value)
        frmPO._lstPRs = grdPR.SelectedItems.Cast<PersonnelShort>().Select(c => grdPR.Items.IndexOf(c)).ToList();
      if (!chkAllChargeTo.IsChecked.Value)
        frmPO._lstChargeTo = grdChargeTo.SelectedItems.Cast<ChargeTo>().Select(c => grdGifts.Items.IndexOf(c)).ToList();
      if (!chkAllGifts.IsChecked.Value)
        frmPO._lstGifts = grdGifts.SelectedItems.Cast<GiftShort>().Select(c => grdGifts.Items.IndexOf(c)).ToList();
      frmPO._cboDateSelected = cboDate.SelectedValue.ToString();
    //  frmPO._cboFolSeriesSelected = cboFolSeries.SelectedValue.ToString();
      frmPO._dtmStart = dtmStart.Value.Value;
      frmPO._dtmEnd = dtmEnd.Value.Value;
      frmPO._enumBasedOnArrival = (chkBasedOnArrival.IsChecked.Value) ? EnumBasedOnArrival.boaBasedOnArrival : EnumBasedOnArrival.boaNoBasedOnArrival;
      frmPO._enumBasedOnBooking = (chkBasedOnBooking.IsChecked.Value) ? EnumBasedOnBooking.bobBasedOnBooking : EnumBasedOnBooking.bobNoBasedOnBooking;
      frmPO._enumQuinellas = (chkQuinellas.IsChecked.Value) ? EnumQuinellas.quQuinellas : EnumQuinellas.quNoQuinellas;
      frmPO._enumDetailsGift = (chkDetailGifts.IsChecked.Value) ? EnumDetailGifts.dgDetailGifts : EnumDetailGifts.dgNoDetailGifts;
      frmPO._enumSalesByMemberShipType = (chkSalesByMembershipType.IsChecked.Value) ? EnumSalesByMemberShipType.sbmDetail : EnumSalesByMemberShipType.sbmNoDetail;
      frmPO._folFrom = txtFolFrom.Text;
      frmPO._folTo = txtFolTo.Text;
      frmPO._enumSaveCourtesyTours = ((KeyValuePair<EnumSaveCourtesyTours, string>)cboSaveCourtesyTours.SelectedItem).Key;
      frmPO._enumExternalInvitation = ((KeyValuePair<EnumExternalInvitation, string>)cboExternal.SelectedItem).Key;

    }
    #endregion

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
      if (!chkAllLeadSources.IsChecked.Value && chkAllLeadSources.IsEnabled)
      {
        frmPO._lstLeadSources.ForEach(c =>
        {
          grdLeadSources.SelectedItems.Add(grdLeadSources.Items.GetItemAt(c));
        });
      }
      if (!chkAllPaymentTypes.IsChecked.Value && chkAllPaymentTypes.IsEnabled)
      {
        frmPO._lstPaymentTypes.ForEach(c =>
        {
          grdPaymentTypes.SelectedItems.Add(grdPaymentTypes.Items.GetItemAt(c));
        });
      }
      if (!chkAllPR.IsChecked.Value && chkAllPR.IsEnabled)
      {
        frmPO._lstPRs.ForEach(c =>
        {
          grdPR.SelectedItems.Add(grdPR.Items.GetItemAt(c));
        });
      }
      if (!chkAllChargeTo.IsChecked.Value && chkAllChargeTo.IsEnabled)
      {
        frmPO._lstChargeTo.ForEach(c =>
        {
          grdChargeTo.SelectedItems.Add(grdChargeTo.Items.GetItemAt(c));
        });
      }
      if (!chkAllGifts.IsChecked.Value && chkAllGifts.IsEnabled)
      {
        frmPO._lstGifts.ForEach(c =>
        {
          grdGifts.SelectedItems.Add(grdGifts.Items.GetItemAt(c));
        });
      }

      cboDate.SelectedValue = (frmPO._cboDateSelected != null) ? frmPO._cboDateSelected : "Dates Specified";
      dtmStart.Value = frmPO._dtmStart;
      dtmEnd.Value = frmPO._dtmEnd;
      chkBasedOnArrival.IsChecked = (frmPO._enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival) ? true : false;
      chkBasedOnBooking.IsChecked = (frmPO._enumBasedOnBooking == EnumBasedOnBooking.bobBasedOnBooking) ? true : false;
      chkQuinellas.IsChecked = (frmPO._enumQuinellas == EnumQuinellas.quQuinellas) ? true : false;
      chkDetailGifts.IsChecked = (frmPO._enumDetailsGift == EnumDetailGifts.dgDetailGifts) ? true : false;
      chkSalesByMembershipType.IsChecked = (frmPO._enumSalesByMemberShipType == EnumSalesByMemberShipType.sbmDetail) ? true : false;

      txtFolFrom.Text = frmPO._folFrom;
      txtFolTo.Text = frmPO._folTo;

      cboFolSeries.SelectedValue = frmPO._cboFolSeriesSelected;
      cboSaveCourtesyTours.SelectedValue = frmPO._enumSaveCourtesyTours;
      cboExternal.SelectedValue = frmPO._enumExternalInvitation;
 
    }
    #endregion

    #region ValidateFields
    /// <summary>
    /// Valida si los grid tienen al menos un elemento seleccionado.
    /// </summary>
    /// <returns>Message/Empty</returns>
    /// <history>
    ///   [edgrodriguez] 05/Abr/2016 Created
    /// </history>
    public string ValidateFields()
    {
      if (pnlLeadSources.Visibility == Visibility.Visible && grdLeadSources.SelectedItems.Count == 0)
        return "No lead source is selected.";
      if (pnlPaymentTypes.Visibility == Visibility.Visible && grdPaymentTypes.SelectedItems.Count == 0)
        return "No payment Types is selected";
      if (pnlPR.Visibility == Visibility.Visible && grdPR.SelectedItems.Count == 0)
        return "No PR is selected.";
      if (pnlChargeTo.Visibility == Visibility.Visible && grdChargeTo.SelectedItems.Count == 0)
        return "No Charge To is selected.";
      if (pnlGifts.Visibility == Visibility.Visible && grdGifts.SelectedItems.Count == 0)
        return "No gift is selected.";
      else
        return "";
    }
    #endregion

    #endregion

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
      else {
        grdLeadSources.UnselectAll();
        grdLeadSources.IsEnabled = true;
      }
    }
    #endregion

    #region grdLeadSources_SelectionChanged
    /// <summary>
    ///  Barra de estado de leadsources al seleccionar
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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
    #endregion

    #region grdPaymentTypes_SelectionChanged
    /// <summary>
    ///  Barra de estado tipos de pago al seleccionar
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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
    #endregion

    #region grdPR_SelectionChanged
    /// <summary>
    ///  Barra de estado PR al seleccionar
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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
    #endregion

    #region grdChargeTo_SelectionChanged
    /// <summary>
    ///  Barra de estado cargar a al seleccionar
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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
    #endregion

    #region grdGifts_SelectionChanged
    /// <summary>
    ///  Barra de estado regalos al seleccionar
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
    private void grdGifts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumG.Content = string.Format("{0}/{1} Selected Gifts", grdGifts.SelectedItems.Count, grdGifts.Items.Count);
    }
    #endregion

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
    #endregion

    #region cboDate_SelectionChanged
    /// <summary>
    /// Modifica los rangos de fecha de los datepicker, según la opcion seleccionada.
    /// </summary>
    /// <history>
    ///   [vku] 22/Mar/2016 Created
    /// </history>
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
    #endregion

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
    #endregion

    #region Close_keyPreview
    private void Close_KeyPreviewESC(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        Close();
    }
    #endregion

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
    #endregion

    #endregion

  }
}
