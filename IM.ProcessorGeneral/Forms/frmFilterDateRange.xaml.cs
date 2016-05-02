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
using System.Windows.Input;

namespace IM.ProcessorGeneral.Forms
{
  /// <summary>
  /// Interaction logic for frmDateRangeSR.xaml
  /// </summary>
  public partial class frmFilterDateRange : Window
  {
    #region Atributos

    private List<GiftShort> _lstGifts;
    private List<GiftCategory> _lstGiftsCate;
    private List<SalesRoomByUser> _lstSalesRoom;
    private List<LeadSourceByUser> _lstLeadSources;
    private List<Program> _lstPrograms;
    private List<RateType> _lstRateTypes;
    private List<WarehouseByUser> _lstWarehouseByUsers;
    public bool BlnOk;
    public frmProcessorGeneral FrmProcGen = new frmProcessorGeneral();

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
      _lstLeadSources = BRLeadSources.GetLeadSourcesByUser(App.User.User.peID);
      _lstRateTypes = BRRateTypes.GetRateTypes();
      _lstWarehouseByUsers = BRWarehouses.GetWarehousesByUser(App.User.User.peID);

      cboStatus.ItemsSource = EnumToListHelper.GetList<EnumStatus>();
      cboSaveCourtesyTours.ItemsSource = EnumToListHelper.GetList<EnumSaveCourtesyTours>();
      cboExternal.ItemsSource = EnumToListHelper.GetList<EnumExternalInvitation>();
      cboGiftsReceiptType.ItemsSource = EnumToListHelper.GetList<EnumGiftsReceiptType>();
      cboGiftSale.ItemsSource = EnumToListHelper.GetList<EnumGiftSale>();
      PreviewKeyDown += Close_KeyPreviewESC;
    }

    #endregion Constructor

    #region Eventos del Formulario

    #region chkAllSalesRoom_Checked

    /// <summary>
    /// Valida si el control Checkbox fue activado/Desactivado.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 04/Mar/2016 Created
    /// </history>
    private void chkAllSalesRoom_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllSalesRoom.IsChecked != null && (bool)chkAllSalesRoom.IsChecked)
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
      if (chkAllCategories.IsChecked != null && (bool)chkAllCategories.IsChecked)
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
      if (chkAllGifts.IsChecked != null && (bool)chkAllGifts.IsChecked)
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
      if (chkAllCategories.IsChecked != null && !(bool)chkAllCategories.IsChecked)
      {
        List<string> lstcategories = grdCategories.SelectedItems.Cast<GiftCategory>().Select(c => c.gcID).ToList();
        grdGifts.ItemsSource = _lstGifts.Where(c => lstcategories.Contains(c.gigc)).ToList();
      }
      else
        grdGifts.ItemsSource = _lstGifts;

      chkGifts_Checked(null, null);
      StatusBarNumCat.Content = $"{grdCategories.SelectedItems.Count}/{grdCategories.Items.Count} Selected Categories";
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
      if (chkAllPrograms.IsChecked != null && (bool)chkAllPrograms.IsChecked)
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
      if (chkAllLeadSources.IsChecked != null && (bool)chkAllLeadSources.IsChecked)
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
        if (chkAllPrograms.IsChecked != null && !(bool)chkAllPrograms.IsChecked)
        {
          List<string> lstPrograms = grdPrograms.SelectedItems.Cast<Program>().Select(c => c.pgID).ToList();
          grdLeadSources.ItemsSource = _lstLeadSources.Where(c => lstPrograms.Contains(c.lspg)).ToList();
        }
        else
          grdLeadSources.ItemsSource = _lstLeadSources;

        chkLeadSources_Checked(null, null);
      }

      StatusBarNumProgs.Content = $"{grdPrograms.SelectedItems.Count}/{grdPrograms.Items.Count} Selected Programs";
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

    #region chkAllRateTypes_Checked

    /// <summary>
    ///  Obtiene el estatus del control checkbox de RateTypes.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 08/03/2016 Created
    /// </history>
    private void chkAllRatetypes_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllRatetypes.IsChecked != null && (bool)chkAllRatetypes.IsChecked)
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
      StatusBarNumSR.Content = $"{grdSalesRoom.SelectedItems.Count}/{grdSalesRoom.Items.Count} Selected Sales Room";
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
      StatusBarNumGifts.Content = $"{grdGifts.SelectedItems.Count}/{grdGifts.Items.Count} Selected Gifts";
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
      StatusBarNumRateT.Content = $"{grdRatetypes.SelectedItems.Count}/{grdRatetypes.Items.Count} Selected Ratetypes";
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
      StatusBarNumLS.Content = $"{grdLeadSources.SelectedItems.Count}/{grdLeadSources.Items.Count} Selected LeadSources";
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
      EnumPredefinedDate selected = ((EnumPredefinedDate)((ComboBox)e.OriginalSource).SelectedValue);
      DateTime today = BRHelpers.GetServerDate();
      pnlDtmStart.IsEnabled = false;
      pnlDtmEnd.IsEnabled = false;
      switch (selected)
      {
        case EnumPredefinedDate.Today:
          dtmStart.Value = dtmEnd.Value = today;
          break;

        case EnumPredefinedDate.Yesterday:
          dtmStart.Value = dtmEnd.Value = today.Date.AddDays(-1);
          break;

        case EnumPredefinedDate.ThisWeek:
          dtmStart.Value = today.AddDays((DayOfWeek.Monday - today.DayOfWeek));
          dtmEnd.Value = today.AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;

        case EnumPredefinedDate.PreviousWeek:
          dtmStart.Value = today.AddDays(-7).AddDays(DayOfWeek.Monday - today.DayOfWeek);
          dtmEnd.Value = today.AddDays(-7).AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;

        case EnumPredefinedDate.TwoWeeksAgo:
          dtmStart.Value = today.AddDays(-14).AddDays(DayOfWeek.Monday - today.DayOfWeek);
          dtmEnd.Value = today.AddDays(-14).AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;

        case EnumPredefinedDate.ThreeWeeksAgo:
          dtmStart.Value = today.AddDays(-21).AddDays(DayOfWeek.Monday - today.DayOfWeek);
          dtmEnd.Value = today.AddDays(-21).AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;

        case EnumPredefinedDate.ThisHalf:
          if (today.Day <= 15)
          {
            dtmStart.Value = new DateTime(today.Year, today.Month, 1);
            dtmEnd.Value = new DateTime(today.Year, today.Month, 15);
          }
          else
          {
            dtmStart.Value = new DateTime(today.Year, today.Month, 16);
            dtmEnd.Value = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
          }
          break;

        case EnumPredefinedDate.PreviousHalf:

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

        case EnumPredefinedDate.ThisMonth:
          dtmStart.Value = new DateTime(today.Year, today.Month, 1);
          dtmEnd.Value = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
          break;

        case EnumPredefinedDate.PreviousMonth:
          dtmStart.Value = new DateTime(today.Year, today.Month - 1, 1);
          dtmEnd.Value = new DateTime(today.Year, today.Month - 1, DateTime.DaysInMonth(today.Year, today.Month - 1));
          break;

        case EnumPredefinedDate.TwoMonthsAgo:
          dtmStart.Value = new DateTime(today.Year, today.Month - 2, 1);
          dtmEnd.Value = new DateTime(today.Year, today.Month - 2, DateTime.DaysInMonth(today.Year, today.Month - 2));
          break;

        case EnumPredefinedDate.ThreeMonthsAgo:
          dtmStart.Value = new DateTime(today.Year, today.Month - 3, 1);
          dtmEnd.Value = new DateTime(today.Year, today.Month - 3, DateTime.DaysInMonth(today.Year, today.Month - 3));
          break;

        case EnumPredefinedDate.ThisYear:
          dtmStart.Value = new DateTime(today.Year, 1, 1);
          dtmEnd.Value = new DateTime(today.Year, 12, 31);
          break;

        case EnumPredefinedDate.PreviousYear:
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

    #region chkAllWarehouses_Checked

    /// <summary>
    ///  Obtiene el estatus del control checkbox de RateTypes.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 26/04/2016 Created
    /// </history>
    private void chkAllWarehouses_Checked(object sender, RoutedEventArgs e)
    {
      if (chkAllWarehouse.IsChecked != null && (bool)chkAllWarehouse.IsChecked)
      {
        grdWarehouse.SelectAll();
        grdWarehouse.IsEnabled = false;
      }
      else
      {
        grdWarehouse.UnselectAll();
        grdWarehouse.IsEnabled = true;
      }
    }

    #endregion chkAllRateTypes_Checked

    #region grdWareHouse_SelectionChanged

    /// <summary>
    /// Actualiza el StatusBar mostrando la cantidad de items seleccionados.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 26/04/2016 Created
    /// </history>
    private void grdWareHouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      StatusBarNumWh.Content = $"{grdWarehouse.SelectedItems.Count}/{grdWarehouse.Items.Count} Selected Warehouses";
    }

    #endregion grdLeadSources_SelectionChanged

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
      bool blnWarehouse = false, bool blnAllWarehouse = false, bool blnOneDate = false, bool blnOnlyOneRegister = false, EnumPeriod enumPeriod = EnumPeriod.None,
      EnumProgram enumPrograms = EnumProgram.All, bool blnOnePeriod = false, EnumBasedOnArrival enumBasedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival,
      EnumQuinellas enumQuinellas = EnumQuinellas.quNoQuinellas, EnumDetailGifts enumDetailGifts = EnumDetailGifts.dgNoDetailGifts, EnumSaveCourtesyTours? enumSaveCourtesyTours = null,
      EnumSalesByMemberShipType enumSalesByMemberShipType = EnumSalesByMemberShipType.sbmNoDetail, EnumBasedOnBooking enumBasedOnBooking = EnumBasedOnBooking.bobNoBasedOnBooking,
      EnumExternalInvitation? enumExternalInvitation = null, bool blncbStatus = false, bool blnGiftReceiptType = false, bool blnGuestId = false, bool blnGiftSale = false)
    {
      ConfigureDates(blnOneDate, enumPeriod);

      #region Configuracion de Grids.

      ConfigureGridPanels(blnSalesRoom, blnGifts, blnCategories, blnPrograms, blnRatetypes, blnLeadSources, blnWarehouse);
      ConfigureSelection(blnOnlyOneRegister);
      ConfigureCheckboxSelectAll(blnOnlyOneRegister, blnAllSalesRoom, blnAllCategories, blnAllGifts, blnAllLeadSources, blnAllPrograms, blnAllRatetypes, blnAllWarehouse);

      #endregion Configuracion de Grids.

      ConfigureFilters(enumBasedOnArrival, enumQuinellas, enumDetailGifts, enumSaveCourtesyTours,
        enumSalesByMemberShipType, enumBasedOnBooking, enumExternalInvitation, blncbStatus, blnGiftReceiptType,
        blnGuestId, blnGiftSale);
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
      EnumExternalInvitation? enumExternalInvitation, bool blncbStatus, bool blnGiftReceiptType, bool blnGuestId, bool blnGiftSale)
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
      txtGuestID.Visibility = lblGuestID.Visibility = (blnGuestId) ? Visibility.Visible : Visibility.Collapsed;
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
      bool blnPrograms, bool blnRatetypes, bool blnLeadSources, bool blnWarehouse)
    {
      pnlCategories.Visibility = (blnCategories) ? Visibility.Visible : Visibility.Collapsed;
      pnlGifts.Visibility = (blnGifts) ? Visibility.Visible : Visibility.Collapsed;
      pnlLeadSource.Visibility = (blnLeadSources) ? Visibility.Visible : Visibility.Collapsed;
      pnlPrograms.Visibility = (blnPrograms) ? Visibility.Visible : Visibility.Collapsed;
      pnlRateTypes.Visibility = (blnRatetypes) ? Visibility.Visible : Visibility.Collapsed;
      pnlSalesRoom.Visibility = (blnSalesRoom) ? Visibility.Visible : Visibility.Collapsed;
      pnlWarehouse.Visibility = (blnWarehouse) ? Visibility.Visible : Visibility.Collapsed;

      grdSalesRoom.ItemsSource = (blnSalesRoom) ? _lstSalesRoom : null;
      grdGifts.ItemsSource = (blnGifts) ? _lstGifts : null;
      grdCategories.ItemsSource = (blnCategories) ? _lstGiftsCate : null;
      grdPrograms.ItemsSource = (blnPrograms) ? _lstPrograms : null;
      grdLeadSources.ItemsSource = (blnLeadSources) ? _lstLeadSources : null;
      grdRatetypes.ItemsSource = (blnRatetypes) ? _lstRateTypes : null;
      grdWarehouse.ItemsSource = (blnWarehouse) ? _lstWarehouseByUsers : null;

      StatusBarNumSR.Content = (blnSalesRoom) ? $"{0}/{_lstSalesRoom.Count} Selected SalesRooms" : "";
      StatusBarNumGifts.Content = (blnGifts) ? $"{0}/{_lstGifts.Count} Selected Gifts" : "";
      StatusBarNumCat.Content = (blnCategories) ? $"{0}/{_lstGiftsCate.Count} Selected Categories" : "";
      StatusBarNumProgs.Content = (blnPrograms) ? $"{0}/{_lstPrograms.Count} Selected Programs" : "";
      StatusBarNumLS.Content = (blnLeadSources) ? $"{0}/{_lstLeadSources.Count} Selected LeadSources" : "";
      StatusBarNumRateT.Content = (blnRatetypes) ? $"{0}/{_lstRateTypes.Count} Selected Ratetypes" : "";
      StatusBarNumWh.Content = (blnWarehouse) ? $"{0}/{_lstWarehouseByUsers.Count} Selected Warehouses" : "";
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
      grdWarehouse.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
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
      cboDate.IsEnabled = !blnOneDate;
      pnlDtmEnd.IsEnabled = !blnOneDate;
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
      bool blnAllLeadSources, bool blnAllPrograms, bool blnAllRatetypes, bool blnAllWarehouses)
    {
      chkAllSalesRoom.IsChecked = blnAllSalesRoom;
      chkAllCategories.IsChecked = blnAllCategories;
      chkAllGifts.IsChecked = blnAllGifts;
      chkAllLeadSources.IsChecked = blnAllLeadSources;
      chkAllPrograms.IsChecked = blnAllPrograms;
      chkAllRatetypes.IsChecked = blnAllRatetypes;
      chkAllWarehouse.IsChecked = blnAllWarehouses;

      chkAllSalesRoom.IsEnabled = !blnOnlyOneRegister;
      chkAllCategories.IsEnabled = !blnOnlyOneRegister;
      chkAllGifts.IsEnabled = !blnOnlyOneRegister;
      chkAllLeadSources.IsEnabled = !blnOnlyOneRegister;
      chkAllPrograms.IsEnabled = !blnOnlyOneRegister;
      chkAllRatetypes.IsEnabled = !blnOnlyOneRegister;
      chkAllWarehouse.IsEnabled = !blnOnlyOneRegister;
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
      if (chkAllSalesRoom.IsChecked != null && !chkAllSalesRoom.IsChecked.Value)
        FrmProcGen._lstSalesRoom = grdSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(c => grdSalesRoom.Items.IndexOf(c)).ToList();
      if (chkAllCategories.IsChecked != null && !chkAllCategories.IsChecked.Value)
        FrmProcGen._lstGiftsCate = grdCategories.SelectedItems.Cast<GiftCategory>().Select(c => grdCategories.Items.IndexOf(c)).ToList();
      if (chkAllGifts.IsChecked != null && !chkAllGifts.IsChecked.Value)
        FrmProcGen._lstGifts = grdGifts.SelectedItems.Cast<GiftShort>().Select(c => grdGifts.Items.IndexOf(c)).ToList();
      if (chkAllRatetypes.IsChecked != null && !chkAllRatetypes.IsChecked.Value)
        FrmProcGen._lstRateTypes = grdRatetypes.SelectedItems.Cast<RateType>().Select(c => grdRatetypes.Items.IndexOf(c)).ToList();
      if (chkAllPrograms.IsChecked != null && !chkAllPrograms.IsChecked.Value)
        FrmProcGen._lstPrograms = grdPrograms.SelectedItems.Cast<Program>().Select(c => grdPrograms.Items.IndexOf(c)).ToList();
      if (chkAllLeadSources.IsChecked != null && !chkAllLeadSources.IsChecked.Value)
        FrmProcGen._lstLeadSources = grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(c => grdLeadSources.Items.IndexOf(c)).ToList();
      FrmProcGen._cboDateSelected = (EnumPredefinedDate)cboDate.SelectedValue;
      FrmProcGen._dtmStart = dtmStart.Value.Value;
      FrmProcGen._dtmEnd = dtmEnd.Value.Value;
      FrmProcGen._enumBasedOnArrival = (chkBasedOnArrival.IsChecked.Value) ? EnumBasedOnArrival.boaBasedOnArrival : EnumBasedOnArrival.boaNoBasedOnArrival;
      FrmProcGen._enumBasedOnBooking = (chkBasedOnBooking.IsChecked.Value) ? EnumBasedOnBooking.bobBasedOnBooking : EnumBasedOnBooking.bobNoBasedOnBooking;
      FrmProcGen._enumQuinellas = (chkQuinellas.IsChecked.Value) ? EnumQuinellas.quQuinellas : EnumQuinellas.quNoQuinellas;
      FrmProcGen._enumDetailsGift = (chkDetailGifts.IsChecked.Value) ? EnumDetailGifts.dgDetailGifts : EnumDetailGifts.dgNoDetailGifts;
      FrmProcGen._enumSalesByMemberShipType = (chkSalesByMembershipType.IsChecked.Value) ? EnumSalesByMemberShipType.sbmDetail : EnumSalesByMemberShipType.sbmNoDetail;
      FrmProcGen._enumStatus = ((KeyValuePair<EnumStatus, string>)cboStatus.SelectedItem).Key;
      FrmProcGen._enumGiftsReceiptType = ((KeyValuePair<EnumGiftsReceiptType, string>)cboGiftsReceiptType.SelectedItem).Key;
      FrmProcGen._GuestID = txtGuestID.Text;
      FrmProcGen._enumGiftSale = ((KeyValuePair<EnumGiftSale, string>)cboGiftSale.SelectedItem).Key;
      FrmProcGen._enumSaveCourtesyTours = ((KeyValuePair<EnumSaveCourtesyTours, string>)cboSaveCourtesyTours.SelectedItem).Key;
      FrmProcGen._enumExternalInvitation = ((KeyValuePair<EnumExternalInvitation, string>)cboExternal.SelectedItem).Key;
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
      if (chkAllSalesRoom.IsChecked != null && (!chkAllSalesRoom.IsChecked.Value && chkAllSalesRoom.IsEnabled))
      {
        FrmProcGen._lstSalesRoom.ForEach(c =>
        {
          grdSalesRoom.SelectedItems.Add(grdSalesRoom.Items.GetItemAt(c));
        });
      }
      if (chkAllCategories.IsChecked != null && (!chkAllCategories.IsChecked.Value && chkAllCategories.IsEnabled))
      {
        FrmProcGen._lstGiftsCate.ForEach(c =>
        {
          grdCategories.SelectedItems.Add(grdCategories.Items.GetItemAt(c));
        });
      }
      if (chkAllGifts.IsChecked != null && (!chkAllGifts.IsChecked.Value && chkAllGifts.IsEnabled))
      {
        FrmProcGen._lstGifts.ForEach(c =>
        {
          grdGifts.SelectedItems.Add(grdGifts.Items.GetItemAt(c));
        });
      }
      if (chkAllRatetypes.IsChecked != null && (!chkAllRatetypes.IsChecked.Value && chkAllRatetypes.IsEnabled))
      {
        FrmProcGen._lstRateTypes.ForEach(c =>
        {
          grdRatetypes.SelectedItems.Add(grdRatetypes.Items.GetItemAt(c));
        });
      }
      if (chkAllPrograms.IsChecked != null && (!chkAllPrograms.IsChecked.Value && chkAllPrograms.IsEnabled))
      {
        FrmProcGen._lstPrograms.ForEach(c =>
        {
          grdPrograms.SelectedItems.Add(grdPrograms.Items.GetItemAt(c));
        });
      }
      if (chkAllLeadSources.IsChecked != null && (!chkAllLeadSources.IsChecked.Value && chkAllLeadSources.IsEnabled))
      {
        FrmProcGen._lstLeadSources.ForEach(c =>
        {
          grdLeadSources.SelectedItems.Add(grdLeadSources.Items.GetItemAt(c));
        });
      }

      cboDate.SelectedValue = FrmProcGen._cboDateSelected ?? EnumPredefinedDate.DatesSpecified;
      dtmStart.Value = FrmProcGen._dtmStart;
      dtmEnd.Value = FrmProcGen._dtmEnd;
      chkBasedOnArrival.IsChecked = (FrmProcGen._enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival);
      chkBasedOnBooking.IsChecked = (FrmProcGen._enumBasedOnBooking == EnumBasedOnBooking.bobBasedOnBooking);
      chkQuinellas.IsChecked = (FrmProcGen._enumQuinellas == EnumQuinellas.quQuinellas);
      chkDetailGifts.IsChecked = (FrmProcGen._enumDetailsGift == EnumDetailGifts.dgDetailGifts);
      chkSalesByMembershipType.IsChecked = (FrmProcGen._enumSalesByMemberShipType == EnumSalesByMemberShipType.sbmDetail);
      cboStatus.SelectedValue = FrmProcGen._enumStatus;
      cboGiftsReceiptType.SelectedValue = FrmProcGen._enumGiftsReceiptType;
      txtGuestID.Text = FrmProcGen._GuestID;
      cboGiftSale.SelectedValue = FrmProcGen._enumGiftSale;
      cboSaveCourtesyTours.SelectedValue = FrmProcGen._enumSaveCourtesyTours;
      cboExternal.SelectedValue = FrmProcGen._enumExternalInvitation;
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

    #endregion Métodos Privados
  }
}