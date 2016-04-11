using IM.Base.Helpers;
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
    public bool _blnOK = false;
    public frmProcessorGeneral frmPG = new frmProcessorGeneral();

    #endregion Atributos

    #region Constructor

    /// <summary>
    /// Carga los valores iniciales del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    public frmFilterDateRange()
    {
      InitializeComponent();
      _lstGifts = BRGifts.GetGifts();
      _lstGiftsCate = BRGifts.GetGiftsCategories();
      _lstSalesRoom = BRSalesRooms.GetSalesRoomsByUser(App.User.User.peID);
      _lstPrograms = BRPrograms.GetPrograms();
      _lstLeadSources = BRLeadSources.GetLeadSourcesByUser(App.User.User.peID, "ALL", "ALL");
      _lstRateTypes = BRRateTypes.GetRateType();

      cboStatus.ItemsSource = EnumToListHelper.GetList<EnumStatus>();
      cboSaveCourtesyTours.ItemsSource = EnumToListHelper.GetList<EnumSaveCourtesyTours>();
      cboExternal.ItemsSource = EnumToListHelper.GetList<EnumExternalInvitation>();
      cboGiftsReceiptType.ItemsSource = EnumToListHelper.GetList<EnumGiftsReceiptType>();
      cboGiftSale.ItemsSource = EnumToListHelper.GetList<EnumGiftSale>();
      this.PreviewKeyDown += new KeyEventHandler(Close_KeyPreviewESC);
    }

    #endregion Constructor

    #region Eventos del Formulario

    #region upd_Scroll

    /// <summary>
    /// Cambia la fecha, aumentando/disminuyendo los dias.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    private void upd_Scroll(object sender, ScrollEventArgs e)
    {
      //ScrollBar scBar = (ScrollBar)sender;
      //if (scBar.Name == "updStart")
      //{
      //  switch (e.ScrollEventType)
      //  {
      //    case ScrollEventType.SmallIncrement:
      //      dtmStart.vaSelectedDate = dtmStart.Value.Value.AddDays(-1);
      //      break;
      //    case ScrollEventType.SmallDecrement:
      //      dtmStart.Value = dtmStart.Value.Value.AddDays(1);
      //      break;
      //  }
      //}
      //else
      //{
      //  switch (e.ScrollEventType)
      //  {
      //    case ScrollEventType.SmallIncrement:
      //      dtmEnd.Value = dtmEnd.Value.Value.AddDays(-1);
      //      break;
      //    case ScrollEventType.SmallDecrement:
      //      dtmEnd.Value = dtmEnd.Value.Value.AddDays(1);
      //      break;
      //  }
      //}
    }

    #endregion upd_Scroll

    #region chkAllSalesRoom_Checked

    /// <summary>
    /// Valida si el control Checkbox fue activado/Desactivado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 04/Mar/2016 Created
    /// </history>
    private void chkAllSalesRoom_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllSalesRoom.IsChecked)
      {
        grdSalesRoom.SelectAll();
        grdSalesRoom.IsEnabled = false;
      }
      else {
        grdSalesRoom.UnselectAll();
        grdSalesRoom.IsEnabled = true;
      }
    }

    #endregion chkAllSalesRoom_Checked

    #region chkCategories_Cheked

    /// <summary>
    /// Obtiene el estatus del control checkbox de categoria.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void chkCategories_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllCategories.IsChecked)
      {
        grdCategories.SelectAll();
        grdCategories.IsEnabled = false;
      }
      else {
        grdCategories.UnselectAll();
        grdCategories.IsEnabled = true;
      }
    }

    #endregion chkCategories_Cheked

    #region chkGifts_Checked

    /// <summary>
    /// Obtiene el estatus del control checkbox de Gifts
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void chkGifts_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllGifts.IsChecked)
      {
        grdGifts.SelectAll();
        grdGifts.IsEnabled = false;
      }
      else {
        grdGifts.UnselectAll();
        grdGifts.IsEnabled = true;
      }
    }

    #endregion chkGifts_Checked

    #region grdCategories_SelectionChanged

    /// <summary>
    /// Filtra el grid de Gifts, al seleccionar una categoria.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void grdCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(bool)chkAllCategories.IsChecked)
      {
        List<string> lstcategories = grdCategories.SelectedItems.Cast<GiftCategory>().Select(C => C.gcID).ToList();
        grdGifts.ItemsSource = _lstGifts.Where(c => lstcategories.Contains(c.gigc)).ToList();
      }
      else
        grdGifts.ItemsSource = _lstGifts;

      chkGifts_Checked(null, null);
      StatusBarNumCat.Content = string.Format("{0}/{1} Selected Categories", grdCategories.SelectedItems.Count, grdCategories.Items.Count);
    }

    #endregion grdCategories_SelectionChanged

    #region chkPrograms_Checked

    /// <summary>
    ///  Obtiene el estatus del control checkbox de programas.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void chkPrograms_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllPrograms.IsChecked)
      {
        grdPrograms.SelectAll();
        grdPrograms.IsEnabled = false;
      }
      else {
        grdPrograms.UnselectAll();
        grdPrograms.IsEnabled = true;
      }
    }

    #endregion chkPrograms_Checked

    #region chkLeadSources_Checked

    /// <summary>
    ///  Obtiene el estatus del control checkbox de lead sources.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    private void chkLeadSources_Checked(object sender, RoutedEventArgs e)
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

    #endregion chkLeadSources_Checked

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
        if (!(bool)chkAllPrograms.IsChecked)
        {
          List<string> lstPrograms = grdPrograms.SelectedItems.Cast<Program>().Select(c => c.pgID).ToList();
          grdLeadSources.ItemsSource = _lstLeadSources.Where(c => lstPrograms.Contains(c.lspg)).ToList();
        }
        else
          grdLeadSources.ItemsSource = _lstLeadSources;

        chkLeadSources_Checked(null, null);
      }

      StatusBarNumProgs.Content = string.Format("{0}/{1} Selected Programs", grdPrograms.SelectedItems.Count, grdPrograms.Items.Count);
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
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      _blnOK = false;
      Close();
    }

    #endregion btnCancel_Click

    #region chkAllRateTypes_Checked

    /// <summary>
    ///  Obtiene el estatus del control checkbox de RateTypes.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void chkAllRatetypes_Checked(object sender, RoutedEventArgs e)
    {
      if ((bool)chkAllRatetypes.IsChecked)
      {
        grdRatetypes.SelectAll();
        grdRatetypes.IsEnabled = false;
      }
      else {
        grdRatetypes.UnselectAll();
        grdRatetypes.IsEnabled = true;
      }
    }

    #endregion chkAllRateTypes_Checked

    #region grdSalesRoom_SelectionChanged

    /// <summary>
    /// Actualiza el StatusBar mostrando la cantidad de items seleccionados.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void grdSalesRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumSR.Content = string.Format("{0}/{1} Selected Sales Room", grdSalesRoom.SelectedItems.Count, grdSalesRoom.Items.Count);
    }

    #endregion grdSalesRoom_SelectionChanged

    #region grdGifts_SelectionChanged

    /// <summary>
    /// Actualiza el StatusBar mostrando la cantidad de items seleccionados.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void grdGifts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumGifts.Content = string.Format("{0}/{1} Selected Gifts", grdGifts.SelectedItems.Count, grdGifts.Items.Count);
    }

    #endregion grdGifts_SelectionChanged

    #region grdRatetypes_SelectionChanged

    /// <summary>
    /// Actualiza el StatusBar mostrando la cantidad de items seleccionados.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void grdRatetypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumRateT.Content = string.Format("{0}/{1} Selected Ratetypes", grdRatetypes.SelectedItems.Count, grdRatetypes.Items.Count);
    }

    #endregion grdRatetypes_SelectionChanged

    #region grdLeadSources_SelectionChanged

    /// <summary>
    /// Actualiza el StatusBar mostrando la cantidad de items seleccionados.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void grdLeadSources_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumLS.Content = string.Format("{0}/{1} Selected LeadSources", grdLeadSources.SelectedItems.Count, grdLeadSources.Items.Count);
    }

    #endregion grdLeadSources_SelectionChanged

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

    #endregion Eventos del Formulario

    #region Métodos Publicos

    /// <summary>
    /// Configuracion de controles del formulario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 03/Mar/2016 Created
    /// </history>
    public void ConfigurarFomulario(bool blnSalesRoom = false, bool blnAllSalesRoom = false, bool blnGifts = false, bool blnAllGifts = false,
      bool blnCategories = false, bool blnAllCategories = false, bool blnPrograms = false, bool blnAllPrograms = false,
      bool blnRatetypes = false, bool blnAllRatetypes = false, bool blnLeadSources = false, bool blnAllLeadSources = false,
      bool blnOneDate = false, bool blnOnlyOneRegister = false, EnumPeriod enumPeriod = EnumPeriod.pdNone,
      EnumProgram enumPrograms = EnumProgram.All, bool blnOnePeriod = false, EnumBasedOnArrival enumBasedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival,
      EnumQuinellas enumQuinellas = EnumQuinellas.quNoQuinellas, EnumDetailGifts enumDetailGifts = EnumDetailGifts.dgNoDetailGifts, EnumSaveCourtesyTours? enumSaveCourtesyTours = null,
      EnumSalesByMemberShipType enumSalesByMemberShipType = EnumSalesByMemberShipType.sbmNoDetail, EnumBasedOnBooking enumBasedOnBooking = EnumBasedOnBooking.bobNoBasedOnBooking,
      EnumExternalInvitation? enumExternalInvitation = null, bool blncbStatus = false, bool blnGiftReceiptType = false, bool blnGuestID = false, bool blnGiftSale = false)
    {
      ConfigureDates(blnOneDate, enumPeriod);

      #region Configuracion de Grids.

      ConfigureGridPanels(blnSalesRoom, blnGifts, blnCategories, blnPrograms, blnRatetypes, blnLeadSources);
      ConfigureSelection(blnOnlyOneRegister);
      ConfigureCheckboxSelectAll(blnOnlyOneRegister, blnAllSalesRoom, blnAllCategories, blnAllGifts, blnAllLeadSources, blnAllPrograms, blnAllRatetypes);

      #endregion Configuracion de Grids.

      ConfigureFilters(enumBasedOnArrival, enumQuinellas, enumDetailGifts, enumSaveCourtesyTours,
        enumSalesByMemberShipType, enumBasedOnBooking, enumExternalInvitation, blncbStatus, blnGiftReceiptType,
        blnGuestID, blnGiftSale);
      LoadUserFilters();
    }

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
      EnumExternalInvitation? enumExternalInvitation, bool blncbStatus, bool blnGiftReceiptType, bool blnGuestID, bool blnGiftSale)
    {
      chkDetailGifts.Visibility = (enumDetailGifts == EnumDetailGifts.dgDetailGifts) ? Visibility.Visible : Visibility.Collapsed;
      chkBasedOnArrival.Visibility = (enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival) ? Visibility.Visible : Visibility.Collapsed;
      chkQuinellas.Visibility = (enumQuinellas == EnumQuinellas.quQuinellas) ? Visibility.Visible : Visibility.Collapsed;
      cboSaveCourtesyTours.Visibility = (enumSaveCourtesyTours != null) ? Visibility.Visible : Visibility.Collapsed;
      chkSalesByMembershipType.Visibility = (enumSalesMemberShipType == EnumSalesByMemberShipType.sbmDetail) ? Visibility.Visible : Visibility.Collapsed;
      chkBasedOnBooking.Visibility = (enumBasedOnBooking == EnumBasedOnBooking.bobBasedOnBooking) ? Visibility.Visible : Visibility.Collapsed;
      cboExternal.Visibility = (enumExternalInvitation != null) ? Visibility.Visible : Visibility.Collapsed;

      cboStatus.Visibility = lblStatus.Visibility = (blncbStatus) ? Visibility.Visible : Visibility.Collapsed;
      cboGiftsReceiptType.Visibility = lblGiftReceiptType.Visibility = (blnGiftReceiptType) ? Visibility.Visible : Visibility.Collapsed;
      txtGuestID.Visibility = lblGuestID.Visibility = (blnGuestID) ? Visibility.Visible : Visibility.Collapsed;
      cboGiftSale.Visibility = lblGiftSale.Visibility = (blnGiftSale) ? Visibility.Visible : Visibility.Collapsed;
      cboSaveCourtesyTours.Visibility = (enumSaveCourtesyTours != null) ? Visibility.Visible : Visibility.Collapsed;
      cboExternal.Visibility = (enumExternalInvitation != null) ? Visibility.Visible : Visibility.Collapsed;
    }

    #endregion ConfigureFilters

    #region ConfigureGridPanels

    /// <summary>
    /// Configura los grids.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 05/Mar/2016 Created
    /// </history>
    private void ConfigureGridPanels(bool blnSalesRoom, bool blnGifts, bool blnCategories,
      bool blnPrograms, bool blnRatetypes, bool blnLeadSources)
    {
      pnlCategories.Visibility = (blnCategories) ? Visibility.Visible : Visibility.Collapsed;
      pnlGifts.Visibility = (blnGifts) ? Visibility.Visible : Visibility.Collapsed;
      pnlLeadSource.Visibility = (blnLeadSources) ? Visibility.Visible : Visibility.Collapsed;
      pnlPrograms.Visibility = (blnPrograms) ? Visibility.Visible : Visibility.Collapsed;
      pnlRateTypes.Visibility = (blnRatetypes) ? Visibility.Visible : Visibility.Collapsed;
      pnlSalesRoom.Visibility = (blnSalesRoom) ? Visibility.Visible : Visibility.Collapsed;

      grdSalesRoom.ItemsSource = (blnSalesRoom) ? _lstSalesRoom : null;
      grdGifts.ItemsSource = (blnGifts) ? _lstGifts : null;
      grdCategories.ItemsSource = (blnCategories) ? _lstGiftsCate : null;
      grdPrograms.ItemsSource = (blnPrograms) ? _lstPrograms : null;
      grdLeadSources.ItemsSource = (blnLeadSources) ? _lstLeadSources : null;
      grdRatetypes.ItemsSource = (blnRatetypes) ? _lstRateTypes : null;

      StatusBarNumSR.Content = (blnSalesRoom) ? string.Format("{0}/{1} Selected SalesRooms", 0, _lstSalesRoom.Count) : "";
      StatusBarNumGifts.Content = (blnGifts) ? string.Format("{0}/{1} Selected Gifts", 0, _lstGifts.Count) : "";
      StatusBarNumCat.Content = (blnCategories) ? string.Format("{0}/{1} Selected Categories", 0, _lstGiftsCate.Count) : "";
      StatusBarNumProgs.Content = (blnPrograms) ? string.Format("{0}/{1} Selected Programs", 0, _lstPrograms.Count) : "";
      StatusBarNumLS.Content = (blnLeadSources) ? string.Format("{0}/{1} Selected LeadSources", 0, _lstLeadSources.Count) : "";
      StatusBarNumRateT.Content = (blnRatetypes) ? string.Format("{0}/{1} Selected Ratetypes", 0, _lstRateTypes.Count) : "";
    }

    #endregion ConfigureGridPanels

    #region ConfigureSelection

    /// <summary>
    /// Configura el modo de seleccion de los grids(Multiseleccion ó Solo un registro).
    /// Activa o desactiva los controles checkbox dependiendo el modo de seleccion configurado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 05/Mar/2016 Created
    /// </history>
    private void ConfigureSelection(bool blnOnlyOneRegister)
    {
      grdCategories.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdGifts.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdLeadSources.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdPrograms.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdRatetypes.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdSalesRoom.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
    }

    #endregion ConfigureSelection

    #region ConfigureDates

    /// <summary>
    /// Configura los controles de fecha.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 05/Mar/2016 Created
    /// </history>
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

    #endregion ConfigureDates

    #region ConfigureCheckboxSelectAll

    /// <summary>
    /// Valida los checkbox para seleccionar todos los registros de los grids.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 07/Mar/2016 Created
    /// </history>
    private void ConfigureCheckboxSelectAll(bool blnOnlyOneRegister, bool blnAllSalesRoom, bool blnAllCategories, bool blnAllGifts,
      bool blnAllLeadSources, bool blnAllPrograms, bool blnAllRatetypes)
    {
      chkAllSalesRoom.IsChecked = blnAllSalesRoom;
      chkAllCategories.IsChecked = blnAllCategories;
      chkAllGifts.IsChecked = blnAllGifts;
      chkAllLeadSources.IsChecked = blnAllLeadSources;
      chkAllPrograms.IsChecked = blnAllPrograms;
      chkAllRatetypes.IsChecked = blnAllRatetypes;

      chkAllSalesRoom.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkAllCategories.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkAllGifts.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkAllLeadSources.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkAllPrograms.IsEnabled = (blnOnlyOneRegister) ? false : true;
      chkAllRatetypes.IsEnabled = (blnOnlyOneRegister) ? false : true;
    }

    #endregion ConfigureCheckboxSelectAll

    #region SaveFrmFilterValues

    /// <summary>
    /// Guarda los datos seleccionados por el usuario.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 15/Mar/2016 Created
    /// </history>
    private void SaveFrmFilterValues()
    {
      if (!chkAllSalesRoom.IsChecked.Value)
        frmPG._lstSalesRoom = grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => grdSalesRoom.Items.IndexOf(c)).ToList();
      if (!chkAllCategories.IsChecked.Value)
        frmPG._lstGiftsCate = grdCategories.SelectedItems.Cast<GiftCategory>().Select(c => grdCategories.Items.IndexOf(c)).ToList();
      if (!chkAllGifts.IsChecked.Value)
        frmPG._lstGifts = grdGifts.SelectedItems.Cast<GiftShort>().Select(c => grdGifts.Items.IndexOf(c)).ToList();
      if (!chkAllRatetypes.IsChecked.Value)
        frmPG._lstRateTypes = grdRatetypes.SelectedItems.Cast<RateType>().Select(c => grdRatetypes.Items.IndexOf(c)).ToList();
      if (!chkAllPrograms.IsChecked.Value)
        frmPG._lstPrograms = grdPrograms.SelectedItems.Cast<Program>().Select(c => grdPrograms.Items.IndexOf(c)).ToList();
      if (!chkAllLeadSources.IsChecked.Value)
        frmPG._lstLeadSources = grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => grdLeadSources.Items.IndexOf(c)).ToList();
      frmPG._cboDateSelected = cboDate.SelectedValue.ToString();
      frmPG._dtmStart = dtmStart.Value.Value;
      frmPG._dtmEnd = dtmEnd.Value.Value;
      frmPG._enumBasedOnArrival = (chkBasedOnArrival.IsChecked.Value) ? EnumBasedOnArrival.boaBasedOnArrival : EnumBasedOnArrival.boaNoBasedOnArrival;
      frmPG._enumBasedOnBooking = (chkBasedOnBooking.IsChecked.Value) ? EnumBasedOnBooking.bobBasedOnBooking : EnumBasedOnBooking.bobNoBasedOnBooking;
      frmPG._enumQuinellas = (chkQuinellas.IsChecked.Value) ? EnumQuinellas.quQuinellas : EnumQuinellas.quNoQuinellas;
      frmPG._enumDetailsGift = (chkDetailGifts.IsChecked.Value) ? EnumDetailGifts.dgDetailGifts : EnumDetailGifts.dgNoDetailGifts;
      frmPG._enumSalesByMemberShipType = (chkSalesByMembershipType.IsChecked.Value) ? EnumSalesByMemberShipType.sbmDetail : EnumSalesByMemberShipType.sbmNoDetail;
      frmPG._enumStatus = ((KeyValuePair<EnumStatus, string>)cboStatus.SelectedItem).Key;
      frmPG._enumGiftsReceiptType = ((KeyValuePair<EnumGiftsReceiptType, string>)cboGiftsReceiptType.SelectedItem).Key;
      frmPG._GuestID = txtGuestID.Text;
      frmPG._enumGiftSale = ((KeyValuePair<EnumGiftSale, string>)cboGiftSale.SelectedItem).Key;
      frmPG._enumSaveCourtesyTours = ((KeyValuePair<EnumSaveCourtesyTours, string>)cboSaveCourtesyTours.SelectedItem).Key;
      frmPG._enumExternalInvitation = ((KeyValuePair<EnumExternalInvitation, string>)cboExternal.SelectedItem).Key;
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
    private void LoadUserFilters()
    {
      if (!chkAllSalesRoom.IsChecked.Value && chkAllSalesRoom.IsEnabled)
      {
        frmPG._lstSalesRoom.ForEach(c =>
        {
          grdSalesRoom.SelectedItems.Add(grdSalesRoom.Items.GetItemAt(c));
        });
      }
      if (!chkAllCategories.IsChecked.Value && chkAllCategories.IsEnabled)
      {
        frmPG._lstGiftsCate.ForEach(c =>
        {
          grdCategories.SelectedItems.Add(grdCategories.Items.GetItemAt(c));
        });
      }
      if (!chkAllGifts.IsChecked.Value && chkAllGifts.IsEnabled)
      {
        frmPG._lstGifts.ForEach(c =>
        {
          grdGifts.SelectedItems.Add(grdGifts.Items.GetItemAt(c));
        });
      }
      if (!chkAllRatetypes.IsChecked.Value && chkAllRatetypes.IsEnabled)
      {
        frmPG._lstRateTypes.ForEach(c =>
        {
          grdRatetypes.SelectedItems.Add(grdRatetypes.Items.GetItemAt(c));
        });
      }
      if (!chkAllPrograms.IsChecked.Value && chkAllPrograms.IsEnabled)
      {
        frmPG._lstPrograms.ForEach(c =>
        {
          grdPrograms.SelectedItems.Add(grdPrograms.Items.GetItemAt(c));
        });
      }
      if (!chkAllLeadSources.IsChecked.Value && chkAllLeadSources.IsEnabled)
      {
        frmPG._lstLeadSources.ForEach(c =>
        {
          grdLeadSources.SelectedItems.Add(grdLeadSources.Items.GetItemAt(c));
        });
      }

      cboDate.SelectedValue = (frmPG._cboDateSelected != null) ? frmPG._cboDateSelected : "Dates Specified";
      dtmStart.Value = frmPG._dtmStart;
      dtmEnd.Value = frmPG._dtmEnd;
      chkBasedOnArrival.IsChecked = (frmPG._enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival) ? true : false;
      chkBasedOnBooking.IsChecked = (frmPG._enumBasedOnBooking == EnumBasedOnBooking.bobBasedOnBooking) ? true : false;
      chkQuinellas.IsChecked = (frmPG._enumQuinellas == EnumQuinellas.quQuinellas) ? true : false;
      chkDetailGifts.IsChecked = (frmPG._enumDetailsGift == EnumDetailGifts.dgDetailGifts) ? true : false;
      chkSalesByMembershipType.IsChecked = (frmPG._enumSalesByMemberShipType == EnumSalesByMemberShipType.sbmDetail) ? true : false;
      cboStatus.SelectedValue = frmPG._enumStatus;
      cboGiftsReceiptType.SelectedValue = frmPG._enumGiftsReceiptType;
      txtGuestID.Text = frmPG._GuestID;
      cboGiftSale.SelectedValue = frmPG._enumGiftSale;
      cboSaveCourtesyTours.SelectedValue = frmPG._enumSaveCourtesyTours;
      cboExternal.SelectedValue = frmPG._enumExternalInvitation;
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
      if (pnlDtmEnd.IsEnabled == true && dtmEnd.Value.Value < dtmStart.Value.Value)
        return "End date must be greater than start date.";
      else
        return "";
    }

    #endregion ValidateFields

    #endregion Métodos Privados
  }
}