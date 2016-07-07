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
using System.Windows.Input;

namespace IM.ProcessorGeneral.Forms
{
  /// <summary>
  /// Interaction logic for frmDateRangeSR.xaml
  /// </summary>
  public partial class frmFilterDateRange : Window
  {
    #region Atributos

    private List<GiftShort> _lstGifts = new List<GiftShort>();
    private List<GiftCategory> _lstGiftsCate = new List<GiftCategory>();
    private List<SalesRoomByUser> _lstSalesRoom = new List<SalesRoomByUser>();
    private List<LeadSourceByUser> _lstLeadSources = new List<LeadSourceByUser>();
    private List<Program> _lstPrograms = new List<Program>();
    private List<RateType> _lstRateTypes = new List<RateType>();
    private List<WarehouseByUser> _lstWarehouseByUsers = new List<WarehouseByUser>();
    public bool BlnOk;
    private EnumPeriod _enumPeriod;
    public frmProcessorGeneral FrmProcGen = new frmProcessorGeneral();

    #endregion Atributos

    #region Constructor

    #region frmFilterDateRange
    /// <summary>
    /// Carga los valores iniciales del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public frmFilterDateRange()
    {
      InitializeComponent();
      PreviewKeyDown += Close_KeyPreviewESC;
    }
    #endregion

    #endregion Constructor

    #region Eventos del Formulario

    #region ChkAllSalesRoom_Checked

    /// <summary>
    /// Valida si el control Checkbox fue activado/Desactivado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 04/Mar/2016 Created
    /// </history>
    private void ChkAllSalesRoom_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllSalesRoom.IsChecked != null && (bool)chkAllSalesRoom.IsChecked)
      {
        grdSalesRoom.SelectedItem = null;
        grdSalesRoom.SelectAll();
        grdSalesRoom.IsEnabled = false;
      }
      else {
        grdSalesRoom.UnselectAll();
        grdSalesRoom.IsEnabled = true;
      }
    }

    #endregion ChkAllSalesRoom_Checked

    #region ChkAllCategories_Checked

    /// <summary>
    /// Obtiene el estatus del control checkbox de categoria.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void ChkAllCategories_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllCategories.IsChecked != null && (bool)chkAllCategories.IsChecked)
      {
        grdCategories.SelectedItem = null;
        grdCategories.SelectAll();
        grdCategories.IsEnabled = false;
      }
      else {
        grdCategories.UnselectAll();
        grdCategories.IsEnabled = true;
      }
    }

    #endregion ChkAllCategories_Checked

    #region ChkAllGifts_Checked

    /// <summary>
    /// Obtiene el estatus del control checkbox de Gifts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void ChkAllGifts_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllGifts.IsChecked != null && (bool)chkAllGifts.IsChecked)
      {
        grdGifts.SelectedItem = null;
        grdGifts.SelectAll();
        grdGifts.IsEnabled = false;
      }
      else {
        grdGifts.UnselectAll();
        grdGifts.IsEnabled = true;
      }
    }

    #endregion ChkAllGifts_Checked

    #region grdCategories_SelectionChanged

    /// <summary>
    /// Filtra el grid de Gifts, al seleccionar una categoria.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void grdCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (chkAllCategories.IsChecked != null && !(bool)chkAllCategories.IsChecked)
      {
        List<string> lstcategories = grdCategories.SelectedItems.Cast<GiftCategory>().Select(c => c.gcID).ToList();
        grdGifts.ItemsSource = _lstGifts.Where(c => lstcategories.Contains(c.gigc)).ToList();
      }
      else
        grdGifts.ItemsSource = _lstGifts;

      ChkAllGifts_Checked(null, null);
    }

    #endregion grdCategories_SelectionChanged

    #region ChkAllPrograms_Checked

    /// <summary>
    ///  Obtiene el estatus del control checkbox de programas.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void ChkAllPrograms_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllPrograms.IsChecked != null && (bool)chkAllPrograms.IsChecked)
      {
        grdPrograms.SelectedItem = null;
        grdPrograms.SelectAll();
        grdPrograms.IsEnabled = false;
      }
      else {
        grdPrograms.UnselectAll();
        grdPrograms.IsEnabled = true;
      }
    }

    #endregion ChkAllPrograms_Checked

    #region ChkAllLeadSources_Checked

    /// <summary>
    ///  Obtiene el estatus del control checkbox de lead sources.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void ChkAllLeadSources_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllLeadSources.IsChecked != null && (bool)chkAllLeadSources.IsChecked)
      {
        grdLeadSources.SelectedItem = null;
        grdLeadSources.SelectAll();
        grdLeadSources.IsEnabled = false;
      }
      else {
        grdLeadSources.UnselectAll();
        grdLeadSources.IsEnabled = true;
      }
    }

    #endregion ChkAllLeadSources_Checked

    #region grdPrograms_SelectionChanged

    /// <summary>
    /// Filtra el grid de LeadSources, al seleccionar un programa.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void grdPrograms_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (grdLeadSources.Visibility == Visibility.Visible)
      {
        if (chkAllPrograms.IsChecked != null && !(bool)chkAllPrograms.IsChecked)
        {
          List<string> lstPrograms = grdPrograms.SelectedItems.Cast<Program>().Select(c => c.pgID).ToList();
          grdLeadSources.ItemsSource = _lstLeadSources.Where(c => lstPrograms.Contains(c.lspg)).ToList();
        }
        else
          grdLeadSources.ItemsSource = _lstLeadSources;

        ChkAllLeadSources_Checked(null, null);
      }
    }

    #endregion grdPrograms_SelectionChanged

    #region btnOK_Click

    /// <summary>
    /// Devuelve un booleano para saber si se ha terminado de
    /// realizar su proceso de filtrado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      string message = ValidateFields();
      if (message == "")
      {
        BlnOk = true;
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
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      BlnOk = false;
      SaveFrmFilterValues();
      Close();
    }

    #endregion btnCancel_Click

    #region ChkAllRatetypes_Checked

    /// <summary>
    ///  Obtiene el estatus del control checkbox de RateTypes.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void ChkAllRatetypes_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllRatetypes.IsChecked != null && (bool)chkAllRatetypes.IsChecked)
      {
        grdRatetypes.SelectedItem = null;
        grdRatetypes.SelectAll();
        grdRatetypes.IsEnabled = false;
      }
      else {
        grdRatetypes.UnselectAll();
        grdRatetypes.IsEnabled = true;
      }
    }

    #endregion ChkAllRatetypes_Checked

    #region Close_KeyPreviewESC

    /// <summary>
    ///
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/Mar/2016 Created
    /// </history>
    private void Close_KeyPreviewESC(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        Close();
    }

    #endregion Close_KeyPreviewESC

    #region cboDate_SelectionChanged

    /// <summary>
    /// Modifica los rangos de fecha de los datepicker, según la opcion seleccionada.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 11/03/2016 Created
    /// </history>
    private void cboDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count <= 0) return;
      var selected = e.AddedItems.Cast<KeyValuePair<EnumPredefinedDate, string>>().FirstOrDefault().Key;
      var dateRange = DateHelper.GetDateRange(selected);

      dtmStart.Value = dateRange.Item1;
      dtmEnd.Value = dateRange.Item2;
      if (selected == EnumPredefinedDate.DatesSpecified)
      {
        pnlDtmStart.IsEnabled = pnlDtmEnd.IsEnabled = true;
        dtmStart.LostFocus += date_LostFocus;
        dtmEnd.LostFocus += date_LostFocus;
      }
      else
      {
        pnlDtmStart.IsEnabled = pnlDtmEnd.IsEnabled = false;
        dtmStart.LostFocus -= date_LostFocus;
        dtmEnd.LostFocus -= date_LostFocus;
      }
    }

    #endregion cboDate_SelectionChanged

    #region Window_Closing

    /// <summary>
    /// Guarda los valores de cada control, antes de cerrar el formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 31/03/2016 Created
    /// </history>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      SaveFrmFilterValues();
    }

    #endregion Window_Closing

    #region ChkAllWarehouses_Checked

    /// <summary>
    ///  Obtiene el estatus del control checkbox de RateTypes.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 26/04/2016 Created
    /// </history>
    private void ChkAllWarehouses_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllWarehouse.IsChecked != null && (bool)chkAllWarehouse.IsChecked)
      {
        grdWarehouse.SelectedItem = null;
        grdWarehouse.SelectAll();
        grdWarehouse.IsEnabled = false;
      }
      else
      {
        grdWarehouse.UnselectAll();
        grdWarehouse.IsEnabled = true;
      }
    }

    #endregion ChkAllWarehouses_Checked

    #region date_LostFocus
    /// <summary>
    /// Realiza la seleccion de las fechas segun el Periodo enviado. Monthly o Weekly
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void date_LostFocus(object sender, RoutedEventArgs e)
    {
      DateTime dtStart = dtmStart.Value.Value;
      DateTime dtEnd = dtmEnd.Value.Value;

      switch (_enumPeriod)
      {
        case EnumPeriod.Weekly:
          //dtmStart.Value = dtStart.AddDays(DayOfWeek.Monday - dtStart.DayOfWeek);
          //dtmEnd.Value = dtEnd.AddDays((DayOfWeek.Monday - dtEnd.DayOfWeek) + 6);
          break;

        case EnumPeriod.Monthly:
          dtmStart.Value = new DateTime(dtStart.Year, dtStart.Month, 1);
          dtmEnd.Value = new DateTime(dtEnd.Year, dtEnd.Month, DateTime.DaysInMonth(dtEnd.Year, dtEnd.Month));
          break;
      }
    }
    #endregion

    #endregion Eventos del Formulario

    #region Métodos Publicos

    #region ConfigurarFormulario
    /// <summary>
    /// Configuracion de controles del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    public void ConfigurarFomulario(bool blnSalesRoom = false, bool blnAllSalesRoom = false, bool blnGifts = false, bool blnAllGifts = false,
      bool blnCategories = false, bool blnAllCategories = false, bool blnPrograms = false, bool blnAllPrograms = false,
      bool blnRatetypes = false, bool blnAllRatetypes = false, bool blnLeadSources = false, bool blnAllLeadSources = false,
      bool blnWarehouse = false, bool blnAllWarehouse = false, bool blnOneDate = false, bool blnOnlyOneRegister = false, EnumPeriod enumPeriod = EnumPeriod.None,
      EnumProgram enumPrograms = EnumProgram.All, bool blnOnePeriod = false, EnumBasedOnArrival enumBasedOnArrival = EnumBasedOnArrival.NoBasedOnArrival,
      EnumQuinellas enumQuinellas = EnumQuinellas.NoQuinellas, EnumDetailGifts enumDetailGifts = EnumDetailGifts.NoDetailGifts, EnumSaveCourtesyTours? enumSaveCourtesyTours = null,
      EnumSalesByMemberShipType enumSalesByMemberShipType = EnumSalesByMemberShipType.NoDetail, EnumBasedOnBooking enumBasedOnBooking = EnumBasedOnBooking.NoBasedOnBooking,
      EnumExternalInvitation? enumExternalInvitation = null, bool blncbStatus = false, bool blnGiftReceiptType = false, bool blnGuestId = false, bool blnGiftSale = false)
    {
      ConfigureDates(blnOneDate, enumPeriod);
      ConfigureFilters(enumBasedOnArrival, enumQuinellas, enumDetailGifts, enumSaveCourtesyTours,
        enumSalesByMemberShipType, enumBasedOnBooking, enumExternalInvitation, blncbStatus, blnGiftReceiptType,
        blnGuestId, blnGiftSale);

      LoadCombos();

      #region Configuracion de Grids.
      LoadSalesRooms(blnOnlyOneRegister, blnSalesRoom, blnAllSalesRoom);
      LoadCategories(blnOnlyOneRegister, blnCategories, blnAllCategories);
      LoadGifts(blnOnlyOneRegister, blnGifts, blnAllGifts);
      LoadLeadSources(blnOnlyOneRegister, blnLeadSources, blnAllLeadSources);
      LoadPrograms(blnOnlyOneRegister, blnPrograms, blnAllPrograms);
      LoadRateTypes(blnOnlyOneRegister, blnRatetypes, blnAllRatetypes);
      LoadWarehouses(blnOnlyOneRegister, blnWarehouse, blnAllWarehouse);
      #endregion Configuracion de Grids.

      LoadUserFilters(blnOneDate);
    } 
    #endregion

    #endregion Métodos Publicos

    #region Métodos Privados

    #region ConfigureFilters

    /// <summary>
    /// Configura los controles que sirven para filtrar los reportes.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void ConfigureFilters(EnumBasedOnArrival enumBasedOnArrival, EnumQuinellas enumQuinellas, EnumDetailGifts enumDetailGifts,
      EnumSaveCourtesyTours? enumSaveCourtesyTours, EnumSalesByMemberShipType? enumSalesMemberShipType, EnumBasedOnBooking enumBasedOnBooking,
      EnumExternalInvitation? enumExternalInvitation, bool blncbStatus, bool blnGiftReceiptType, bool blnGuestId, bool blnGiftSale)
    {
      chkDetailGifts.Visibility = (enumDetailGifts == EnumDetailGifts.DetailGifts) ? Visibility.Visible : Visibility.Collapsed;
      chkBasedOnArrival.Visibility = (enumBasedOnArrival == EnumBasedOnArrival.BasedOnArrival) ? Visibility.Visible : Visibility.Collapsed;
      chkQuinellas.Visibility = (enumQuinellas == EnumQuinellas.Quinellas) ? Visibility.Visible : Visibility.Collapsed;
      cboSaveCourtesyTours.Visibility = (enumSaveCourtesyTours != null) ? Visibility.Visible : Visibility.Collapsed;
      chkSalesByMembershipType.Visibility = (enumSalesMemberShipType == EnumSalesByMemberShipType.Detail) ? Visibility.Visible : Visibility.Collapsed;
      chkBasedOnBooking.Visibility = (enumBasedOnBooking == EnumBasedOnBooking.BasedOnBooking) ? Visibility.Visible : Visibility.Collapsed;
      cboExternal.Visibility = (enumExternalInvitation != null) ? Visibility.Visible : Visibility.Collapsed;

      cboStatus.Visibility = lblStatus.Visibility = (blncbStatus) ? Visibility.Visible : Visibility.Collapsed;
      cboGiftsReceiptType.Visibility = lblGiftReceiptType.Visibility = (blnGiftReceiptType) ? Visibility.Visible : Visibility.Collapsed;
      txtGuestID.Visibility = lblGuestID.Visibility = (blnGuestId) ? Visibility.Visible : Visibility.Collapsed;
      cboGiftSale.Visibility = lblGiftSale.Visibility = (blnGiftSale) ? Visibility.Visible : Visibility.Collapsed;
      cboSaveCourtesyTours.Visibility = (enumSaveCourtesyTours != null) ? Visibility.Visible : Visibility.Collapsed;
      cboExternal.Visibility = (enumExternalInvitation != null) ? Visibility.Visible : Visibility.Collapsed;
    }

    #endregion ConfigureFilters

    #region ConfigureDates

    /// <summary>
    /// Configura los controles de fecha.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 05/Mar/2016 Created
    /// </history>
    private void ConfigureDates(bool blnOneDate, EnumPeriod enumPeriod)
    {
      Dictionary<EnumPredefinedDate, string> dictionaryPredefinedDate = EnumToListHelper.GetList<EnumPredefinedDate>();

      cboDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.DatesSpecified));
      _enumPeriod = enumPeriod;//Asignamos el enumPeriod a la variable privada del formulario.
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

    #region SaveFrmFilterValues

    /// <summary>
    /// Guarda los datos seleccionados por el usuario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Mar/2016 Created
    /// </history>
    private void SaveFrmFilterValues()
    {
      //if (chkAllSalesRoom.IsChecked != null && !chkAllSalesRoom.IsChecked.Value)
        FrmProcGen.ClsFilter.LstSalesRooms = grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => c.srID).ToList();
      //if (chkAllCategories.IsChecked != null && !chkAllCategories.IsChecked.Value)
        FrmProcGen.ClsFilter.LstGiftsCate = grdCategories.SelectedItems.Cast<GiftCategory>().Select(c =>c.gcID).ToList();
      //if (chkAllGifts.IsChecked != null && !chkAllGifts.IsChecked.Value)
        FrmProcGen.ClsFilter.LstGifts = grdGifts.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList();
      //if (chkAllRatetypes.IsChecked != null && !chkAllRatetypes.IsChecked.Value)
        FrmProcGen.ClsFilter.LstRateTypes = grdRatetypes.SelectedItems.Cast<RateType>().Select(c => c.raID).ToList();
      //if (chkAllPrograms.IsChecked != null && !chkAllPrograms.IsChecked.Value)
        FrmProcGen.ClsFilter.LstPrograms = grdPrograms.SelectedItems.Cast<Program>().Select(c => c.pgID).ToList();
      //if (chkAllLeadSources.IsChecked != null && !chkAllLeadSources.IsChecked.Value)
        FrmProcGen.ClsFilter.LstLeadSources = grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => c.lsID).ToList();
      //if (chkAllWarehouse.IsChecked != null && !chkAllWarehouse.IsChecked.Value)
        FrmProcGen.ClsFilter.LstWarehouses = grdWarehouse.SelectedItems.Cast<WarehouseByUser>().Select(c => c.whID).ToList();

      FrmProcGen.ClsFilter.SelectedDate = (EnumPredefinedDate)cboDate.SelectedValue;
      FrmProcGen.ClsFilter.StartDate = dtmStart.Value.Value;
      FrmProcGen.ClsFilter.EndDate = dtmEnd.Value.Value;
      FrmProcGen.ClsFilter.BasedOnArrival = (chkBasedOnArrival.IsChecked.Value) ? EnumBasedOnArrival.BasedOnArrival : EnumBasedOnArrival.NoBasedOnArrival;
      FrmProcGen.ClsFilter.BasedOnBooking = (chkBasedOnBooking.IsChecked.Value) ? EnumBasedOnBooking.BasedOnBooking : EnumBasedOnBooking.NoBasedOnBooking;
      FrmProcGen.ClsFilter.Quinellas = (chkQuinellas.IsChecked.Value) ? EnumQuinellas.Quinellas : EnumQuinellas.NoQuinellas;
      FrmProcGen.ClsFilter.DetailGift = (chkDetailGifts.IsChecked.Value) ? EnumDetailGifts.DetailGifts : EnumDetailGifts.NoDetailGifts;
      FrmProcGen.ClsFilter.SalesByMemberShipType = (chkSalesByMembershipType.IsChecked.Value) ? EnumSalesByMemberShipType.Detail : EnumSalesByMemberShipType.NoDetail;
      FrmProcGen.ClsFilter.Status = ((KeyValuePair<EnumStatus, string>)cboStatus.SelectedItem).Key;
      FrmProcGen.ClsFilter.GiftsReceiptType = ((KeyValuePair<EnumGiftsReceiptType, string>)cboGiftsReceiptType.SelectedItem).Key;
      FrmProcGen.ClsFilter.GuestId = txtGuestID.Text;
      FrmProcGen.ClsFilter.GiftSale = ((KeyValuePair<EnumGiftSale, string>)cboGiftSale.SelectedItem).Key;
      FrmProcGen.ClsFilter.SaveCourtesyTours = ((KeyValuePair<EnumSaveCourtesyTours, string>)cboSaveCourtesyTours.SelectedItem).Key;
      FrmProcGen.ClsFilter.ExternalInvitation = ((KeyValuePair<EnumExternalInvitation, string>)cboExternal.SelectedItem).Key;

      FrmProcGen.ClsFilter.AllGifts = chkAllGifts.IsChecked.Value;
      FrmProcGen.ClsFilter.AllGiftsCate = chkAllCategories.IsChecked.Value;
      FrmProcGen.ClsFilter.AllLeadSources = chkAllLeadSources.IsChecked.Value;
      FrmProcGen.ClsFilter.AllPrograms = chkAllPrograms.IsChecked.Value;
      FrmProcGen.ClsFilter.AllRateTypes = chkAllRatetypes.IsChecked.Value;
      FrmProcGen.ClsFilter.AllSalesRooms = chkAllSalesRoom.IsChecked.Value;
      FrmProcGen.ClsFilter.AllWarehouses = chkAllWarehouse.IsChecked.Value;

    }

    #endregion SaveFrmFilterValues

    #region LoadUserFilters

    /// <summary>
    /// Obtiene los filtros que el usuario habiá seleccionado
    /// y los aplica al formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    private void LoadUserFilters(bool onlyOneDate)
    {

      cboDate.SelectedValue = (!onlyOneDate && cboDate.Items.Cast<KeyValuePair<EnumPredefinedDate, string>>().Any(c => c.Key == FrmProcGen.ClsFilter.SelectedDate)) ? FrmProcGen.ClsFilter.SelectedDate : EnumPredefinedDate.DatesSpecified;
      dtmStart.Value = FrmProcGen.ClsFilter.StartDate;
      dtmEnd.Value = FrmProcGen.ClsFilter.EndDate;
      chkBasedOnArrival.IsChecked = (FrmProcGen.ClsFilter.BasedOnArrival == EnumBasedOnArrival.BasedOnArrival);
      chkBasedOnBooking.IsChecked = (FrmProcGen.ClsFilter.BasedOnBooking == EnumBasedOnBooking.BasedOnBooking);
      chkQuinellas.IsChecked = (FrmProcGen.ClsFilter.Quinellas == EnumQuinellas.Quinellas);
      chkDetailGifts.IsChecked = (FrmProcGen.ClsFilter.DetailGift == EnumDetailGifts.DetailGifts);
      chkSalesByMembershipType.IsChecked = (FrmProcGen.ClsFilter.SalesByMemberShipType == EnumSalesByMemberShipType.Detail);
      cboStatus.SelectedValue = FrmProcGen.ClsFilter.Status;
      cboGiftsReceiptType.SelectedValue = FrmProcGen.ClsFilter.GiftsReceiptType;
      txtGuestID.Text = FrmProcGen.ClsFilter.GuestId;
      cboGiftSale.SelectedValue = FrmProcGen.ClsFilter.GiftSale;
      cboSaveCourtesyTours.SelectedValue = FrmProcGen.ClsFilter.SaveCourtesyTours;
      cboExternal.SelectedValue = FrmProcGen.ClsFilter.ExternalInvitation;
    }

    #endregion LoadUserFilters

    #region ValidateFields

    /// <summary>
    /// Valida si los grid tienen al menos un elemento seleccionado.
    /// </summary>
    /// <returns>Message/Empty</returns>
    /// <history>
    /// [edgrodriguez] 24/Mar/2016 Created
    /// </history>
    private string ValidateFields()
    {
      if (pnlSalesRoom.Visibility == Visibility.Visible && grdSalesRoom.SelectedItems.Count == 0)
        return "No sales room is selected";
      if (pnlRateTypes.Visibility == Visibility.Visible && grdRatetypes.SelectedItems.Count == 0)
        return "No Rate types is selected.";
      if (pnlPrograms.Visibility == Visibility.Visible && grdPrograms.SelectedItems.Count == 0)
        return "No program is selected.";
      if (pnlLeadSource.Visibility == Visibility.Visible && grdLeadSources.SelectedItems.Count == 0)
        return "No lead source is selected.";
      if (pnlGifts.Visibility == Visibility.Visible && grdGifts.SelectedItems.Count == 0)
        return "No gift is selected.";
      if (pnlCategories.Visibility == Visibility.Visible && grdCategories.SelectedItems.Count == 0)
        return "No category is selected.";
      if (pnlDtmEnd.IsEnabled && dtmEnd.Value.Value < dtmStart.Value.Value)
        return "End date must be greater than start date.";
      else
        return "";
    }

    #endregion ValidateFields

    #region LoadCombos
    /// <summary>
    /// Carga los combobox.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 21/May/2016 Created
    /// </history>
    private void LoadCombos()
    {

      cboStatus.ItemsSource = EnumToListHelper.GetList<EnumStatus>();
      cboSaveCourtesyTours.ItemsSource = EnumToListHelper.GetList<EnumSaveCourtesyTours>();
      cboExternal.ItemsSource = EnumToListHelper.GetList<EnumExternalInvitation>();
      cboGiftsReceiptType.ItemsSource = EnumToListHelper.GetList<EnumGiftsReceiptType>();
      cboGiftSale.ItemsSource = EnumToListHelper.GetList<EnumGiftSale>();
    }
    #endregion

    #region LoadSalesRooms
    /// <summary>
    /// Carga y configuracion del grid Salesrooms
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnSalesRoom"></param>
    /// <param name="blnAllSalesRoom"></param>
    /// <history>
    /// [edgrodriguez] 21/May/2016 Created
    /// </history>
    private async void LoadSalesRooms(bool blnOnlyOneRegister, bool blnSalesRoom, bool blnAllSalesRoom)
    {
      if (!blnSalesRoom) return;

      grdSalesRoom.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlSalesRoom.Visibility = Visibility.Visible;
      _lstSalesRoom = await BRSalesRooms.GetSalesRoomsByUser(App.User.User.peID);
      grdSalesRoom.ItemsSource = _lstSalesRoom;

      chkAllSalesRoom.IsChecked = blnAllSalesRoom;
      chkAllSalesRoom.IsEnabled = !blnOnlyOneRegister;

      if (!FrmProcGen.ClsFilter.LstSalesRooms.Any()) return;

      chkAllSalesRoom.IsChecked = (grdSalesRoom.SelectionMode == DataGridSelectionMode.Extended) && FrmProcGen.ClsFilter.AllSalesRooms;
      if (grdSalesRoom.ItemsSource != null && !FrmProcGen.ClsFilter.AllSalesRooms && !blnOnlyOneRegister)
      {
        grdSalesRoom.SelectedItem = null;
        FrmProcGen.ClsFilter.LstSalesRooms.ForEach(c =>
        {
          grdSalesRoom.SelectedItems.Add(_lstSalesRoom.FirstOrDefault(s => s.srID == c));
        });
      }
      else
        grdSalesRoom.SelectedItem = _lstSalesRoom.FirstOrDefault(c => c.srID == FrmProcGen.ClsFilter.LstSalesRooms[0]);
    }
    #endregion

    #region LoadLeadSources
    /// <summary>
    /// Carga y configuracion del grid de LeadSources 
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnLeadSources"></param>
    /// <param name="blnAllLeadSources"></param>
    /// <history>
    /// [edgrodriguez] 21/May/2016 Created
    /// </history>
    private async void LoadLeadSources(bool blnOnlyOneRegister, bool blnLeadSources, bool blnAllLeadSources)
    {
      if (!blnLeadSources) return;

      grdLeadSources.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlLeadSource.Visibility = Visibility.Visible;
      _lstLeadSources = await BRLeadSources.GetLeadSourcesByUser(App.User.User.peID);
      grdLeadSources.ItemsSource = _lstLeadSources;

      chkAllLeadSources.IsChecked = blnAllLeadSources;
      chkAllLeadSources.IsEnabled = !blnOnlyOneRegister;

      if (!FrmProcGen.ClsFilter.LstLeadSources.Any()) return;

      chkAllLeadSources.IsChecked = (grdLeadSources.SelectionMode == DataGridSelectionMode.Extended) && FrmProcGen.ClsFilter.AllLeadSources;
      if (grdLeadSources.ItemsSource != null && !FrmProcGen.ClsFilter.AllLeadSources && !blnOnlyOneRegister)
      {
        grdLeadSources.SelectedItem = null;
        FrmProcGen.ClsFilter.LstLeadSources.ForEach(c =>
        {
          grdLeadSources.SelectedItems.Add(_lstLeadSources.FirstOrDefault(l => l.lsID == c));
        });
      }
      else
        grdLeadSources.SelectedItem = _lstLeadSources.FirstOrDefault(c => c.lsID == FrmProcGen.ClsFilter.LstLeadSources[0]);
    }
    #endregion

    #region LoadCategories
    /// <summary>
    /// Carga y configuracion del grid Categories.
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnCategories"></param>
    /// <param name="blnAllCategories"></param>
    /// <history>
    /// [edgrodriguez] 21/May/2016 Created
    /// </history>
    private async void LoadCategories(bool blnOnlyOneRegister, bool blnCategories, bool blnAllCategories)
    {
      if (!blnCategories) return;

      grdCategories.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlCategories.Visibility = Visibility.Visible;
      _lstGiftsCate = await BRGiftsCategories.GetGiftsCategories();
      grdCategories.ItemsSource = _lstGiftsCate;

      chkAllCategories.IsChecked = blnAllCategories;
      chkAllCategories.IsEnabled = !blnOnlyOneRegister;

      if (!FrmProcGen.ClsFilter.LstGiftsCate.Any()) return;

      chkAllCategories.IsChecked = (grdCategories.SelectionMode == DataGridSelectionMode.Extended) && FrmProcGen.ClsFilter.AllGiftsCate;
      if (grdCategories.ItemsSource != null && !FrmProcGen.ClsFilter.AllGiftsCate && !blnOnlyOneRegister)
      {
        grdCategories.SelectedItem = null;
        FrmProcGen.ClsFilter.LstGiftsCate.ForEach(c =>
        {
          grdCategories.SelectedItems.Add(_lstGiftsCate.FirstOrDefault(g => g.gcID == c));
        });
      }
      else
        grdCategories.SelectedItem = _lstGiftsCate.FirstOrDefault(c => c.gcID == FrmProcGen.ClsFilter.LstGiftsCate[0]);
    }
    #endregion

    #region LoadGifts
    /// <summary>
    /// Carga y configuracion del grid Gifts.
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnGifts"></param>
    /// <param name="blnAllGifts"></param>
    /// <history>
    /// [edgrodriguez] 21/May/2016 Created
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

      if (!FrmProcGen.ClsFilter.LstGifts.Any()) return;

      chkAllGifts.IsChecked = (grdGifts.SelectionMode == DataGridSelectionMode.Extended) && FrmProcGen.ClsFilter.AllGifts;
      if (grdGifts.ItemsSource != null && !FrmProcGen.ClsFilter.AllGifts && !blnOnlyOneRegister)
      {
        grdGifts.SelectedItem = null;
        FrmProcGen.ClsFilter.LstGifts.ForEach(c =>
        {
          grdGifts.SelectedItems.Add(_lstGifts.FirstOrDefault(g => g.giID == c));
        });
      }
      else
        grdGifts.SelectedItem = _lstGifts.FirstOrDefault(c=>c.giID == FrmProcGen.ClsFilter.LstGifts[0]);
    }
    #endregion

    #region LoadRateTypes
    /// <summary>
    /// Carga y configuracion del grid RateTypes
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnRateTypes"></param>
    /// <param name="blnAllRateTypes"></param>
    /// <history>
    /// [edgrodriguez] 21/May/2016 Created
    /// </history>
    private async void LoadRateTypes(bool blnOnlyOneRegister, bool blnRateTypes, bool blnAllRateTypes)
    {
      if (!blnRateTypes) return;

      grdRatetypes.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlRateTypes.Visibility = Visibility.Visible;
      _lstRateTypes = await BRRateTypes.GetRateTypes();
      grdRatetypes.ItemsSource = _lstRateTypes;

      chkAllRatetypes.IsChecked = blnAllRateTypes;
      chkAllRatetypes.IsEnabled = !blnOnlyOneRegister;

      if (!FrmProcGen.ClsFilter.LstRateTypes.Any()) return;

      chkAllRatetypes.IsChecked = (grdRatetypes.SelectionMode == DataGridSelectionMode.Extended) && FrmProcGen.ClsFilter.AllRateTypes;
      if (grdRatetypes.ItemsSource != null && !FrmProcGen.ClsFilter.AllRateTypes && !blnOnlyOneRegister)
      {
        grdRatetypes.SelectedItem = null;
        FrmProcGen.ClsFilter.LstRateTypes.ForEach(c =>
        {
          grdRatetypes.SelectedItems.Add(_lstRateTypes.FirstOrDefault(r => r.raID == c));
        });
      }
      else
        grdRatetypes.SelectedItem = _lstRateTypes.FirstOrDefault(c => c.raID == FrmProcGen.ClsFilter.LstRateTypes[0]);
    }
    #endregion

    #region LoadPrograms
    /// <summary>
    /// Carga y configuracion del grid Programs
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnPrograms"></param>
    /// <param name="blnAllPrograms"></param>
    /// <history>
    /// [edgrodriguez] 21/May/2016 Created
    /// </history>
    private async void LoadPrograms(bool blnOnlyOneRegister, bool blnPrograms, bool blnAllPrograms)
    {
      if (!blnPrograms) return;

      grdPrograms.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlPrograms.Visibility = Visibility.Visible;
      _lstPrograms = await BRPrograms.GetPrograms();
      grdPrograms.ItemsSource = _lstPrograms;

      chkAllPrograms.IsChecked = blnAllPrograms;
      chkAllPrograms.IsEnabled = !blnOnlyOneRegister;

      if (!FrmProcGen.ClsFilter.LstPrograms.Any()) return;

      chkAllPrograms.IsChecked = (grdPrograms.SelectionMode == DataGridSelectionMode.Extended) && FrmProcGen.ClsFilter.AllPrograms;
      if (grdPrograms.ItemsSource != null && !FrmProcGen.ClsFilter.AllPrograms && !blnOnlyOneRegister)
      {
        grdPrograms.SelectedItem = null;
        FrmProcGen.ClsFilter.LstPrograms.ForEach(c =>
        {
          grdPrograms.SelectedItems.Add(_lstPrograms.FirstOrDefault(p => p.pgID == c));
        });
      }
      else
        grdPrograms.SelectedItem = _lstPrograms.FirstOrDefault(c => c.pgID == FrmProcGen.ClsFilter.LstPrograms[0]);
    }
    #endregion

    #region LoadWarehouses
    /// <summary>
    /// Carga y configuracion del grid Warehouses
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnWarehouses"></param>
    /// <param name="blnAllWarehouses"></param>
    /// <history>
    /// [edgrodriguez] 21/May/2016 Created
    /// </history>
    private async void LoadWarehouses(bool blnOnlyOneRegister, bool blnWarehouses, bool blnAllWarehouses)
    {
      if (!blnWarehouses) return;

      grdWarehouse.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      pnlWarehouse.Visibility = Visibility.Visible;
      _lstWarehouseByUsers = await BRWarehouses.GetWarehousesByUser(App.User.User.peID);
      grdWarehouse.ItemsSource = _lstWarehouseByUsers;

      chkAllWarehouse.IsChecked = blnAllWarehouses;
      chkAllWarehouse.IsEnabled = !blnOnlyOneRegister;

      if (!FrmProcGen.ClsFilter.LstWarehouses.Any()) return;

      chkAllWarehouse.IsChecked = (grdWarehouse.SelectionMode == DataGridSelectionMode.Extended) && FrmProcGen.ClsFilter.AllWarehouses;
      if (grdWarehouse.ItemsSource != null && !FrmProcGen.ClsFilter.AllWarehouses && !blnOnlyOneRegister)
      {
        grdWarehouse.SelectedItem = null;
        FrmProcGen.ClsFilter.LstWarehouses.ForEach(c =>
        {
          grdWarehouse.SelectedItems.Add(_lstWarehouseByUsers.FirstOrDefault(w => w.whID == c));
        });
      }
      else
        grdWarehouse.SelectedItem = _lstWarehouseByUsers.FirstOrDefault(c=>c.whID == FrmProcGen.ClsFilter.LstWarehouses[0]);
    } 
    #endregion

    #endregion Métodos Privados
  }
}