using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.ProcessorInhouse.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IM.ProcessorInhouse.Forms
{
  /// <summary>
  /// Interaction logic for frmDateRangeSR.xaml
  /// </summary>
  public partial class frmFilterDateRange : Window
  {
    #region Atributos

    private List<PersonnelShort> _lstPersonnel = new List<PersonnelShort>();
    private List<LeadSourceByUser> _lstLeadSources = new List<LeadSourceByUser>();
    private List<ChargeTo> _lstCharteTo = new List<ChargeTo>();
    private List<GiftShort> _lstGifts = new List<GiftShort>();
    private List<GiftQuantity> _lstGiftsQuantity = new List<GiftQuantity>();

    private List<Agency> _lstAgencies = new List<Agency>();
    private List<MarketShort> _lstMarkets = new List<MarketShort>();
    private List<RateType> _lstCountries = new List<RateType>();

    public bool _blnOK;
    public frmProcessorInhouse frmIH = new frmProcessorInhouse();

    #endregion Atributos

    #region Properties

    public bool CloseAllowed { get; set; }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Carga los valores iniciales del formulario.
    /// </summary>
    /// <history>
    /// [aalcocer] 03/Mar/2016 Created
    /// </history>
    public frmFilterDateRange()
    {
      InitializeComponent();
      cboStatus.ItemsSource = EnumToListHelper.GetList<EnumStatus>();
      cboSaveCourtesyTours.ItemsSource = EnumToListHelper.GetList<EnumSaveCourtesyTours>();
      cboExternal.ItemsSource = EnumToListHelper.GetList<EnumExternalInvitation>();
      cboGiftsReceiptType.ItemsSource = EnumToListHelper.GetList<EnumGiftsReceiptType>();
      cboGiftSale.ItemsSource = EnumToListHelper.GetList<EnumGiftSale>();
      PreviewKeyDown += Close_KeyPreviewESC;
    }

    #endregion Constructor

    #region Eventos del Formulario

    #region btnOK_Click

    /// <summary>
    /// Devuelve un booleano para saber si se ha terminado de
    /// realizar su proceso de filtrado.
    /// </summary>
    /// <history>
    /// [aalcocer] 15/03/2016 Created
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
    /// [aalcocer] 15/03/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      _blnOK = false;
      SaveFrmFilterValues();
      Close();
    }

    #endregion btnCancel_Click

    #region chbx_Checked

    /// <summary>
    /// Selecciona/Deselecciona todos los elementos de las listas
    /// </summary>
    /// <history>
    /// [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void chbx_Checked(object sender, RoutedEventArgs e)
    {
      CheckBox _checkBox = (CheckBox)sender;
      bool _checked = _checkBox.IsChecked.HasValue && _checkBox.IsChecked.Value;
      DataGrid _dataGrid = null;

      if (_checkBox.Equals(chkAllLeadSources))
        _dataGrid = grdLeadSources;
      else if (_checkBox.Equals(chkAllPersonnel))
        _dataGrid = grdPersonnel;
      else if (_checkBox.Equals(chkAllPersonnel))
        _dataGrid = grdAgencies;
      else if (_checkBox.Equals(chkAllAgencies))
        _dataGrid = grdAgencies;
      else if (_checkBox.Equals(chkAllChargeTo))
        _dataGrid = grdChargeTo;
      else if (_checkBox.Equals(chkAllCountries))
        _dataGrid = grdCountries;
      else if (_checkBox.Equals(chkAllGifts))
        _dataGrid = grdGifts;
      else if (_checkBox.Equals(chkAllMarkets))
        _dataGrid = grdMarkets;
      else if (_checkBox.Equals(chkAllGiftsQuantity))
      {
        grdGiftsQuantity.Items.OfType<GiftQuantity>().ToList().ForEach(item => item.include = _checked);
        grdGiftsQuantity.Items.Refresh();
        _dataGrid = grdGiftsQuantity;
      }

      if (_dataGrid == null) return;
      if (_checked)
        _dataGrid.SelectAll();
      else
        _dataGrid.UnselectAll();

      _dataGrid.IsEnabled = !_checked;
    }

    #endregion chbx_Checked

    #region grd_SelectionChanged

    /// <summary>
    /// Actualiza el StatusBar mostrando la cantidad de items seleccionados.
    /// </summary>
    /// <history>
    /// [aalcocer] 15/03/2016 Created
    /// </history>
    private void grd_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DataGrid _dataGrid = (DataGrid)sender;
      if (_dataGrid.Equals(grdPersonnel))
        StatusBarNumPR.Content = $"{grdPersonnel.SelectedItems.Count}/{grdPersonnel.Items.Count} Selected SalesRooms";
      else if (_dataGrid.Equals(grdLeadSources))
        StatusBarNumLS.Content = $"{grdLeadSources.SelectedItems.Count}/{grdLeadSources.Items.Count} Selected LeadSources";
      else if (_dataGrid.Equals(grdChargeTo))
        StatusBarNumCha.Content = $"{grdChargeTo.SelectedItems.Count}/{grdChargeTo.Items.Count} Selected ChargeTo";
      else if (_dataGrid.Equals(grdGifts))
        StatusBarNumGifts.Content = $"{grdGifts.SelectedItems.Count}/{grdGifts.Items.Count} Selected Gifts";
      else if (_dataGrid.Equals(grdGiftsQuantity))
        StatusBarNumGiftsQuantity.Content = $"{grdGiftsQuantity.Items.Cast<GiftQuantity>().Count(x => x.include)}/{grdGiftsQuantity.Items.Count} Selected Gifts";
      else if (_dataGrid.Equals(grdMarkets))
      {
        var selectedMarkets = grdMarkets.SelectedItems.Cast<MarketShort>().Select(m => m.mkID);
        grdAgencies.ItemsSource = _lstAgencies.Where(c => selectedMarkets.Contains(c.agmk));
        if (chkAllAgencies.IsChecked.Value) grdAgencies.SelectAll();
        else StatusBarNumAgen.Content = $"{grdAgencies.SelectedItems.Count}/{grdAgencies.Items.Count} Selected Agencies";

        StatusBarNumMark.Content = $"{grdMarkets.SelectedItems.Count}/{grdMarkets.Items.Count} Selected Markets";
      }
      else if (_dataGrid.Equals(grdAgencies))
        StatusBarNumAgen.Content = $"{grdAgencies.SelectedItems.Count}/{grdAgencies.Items.Count} Selected Agencies";
      else if (_dataGrid.Equals(grdCountries))
        StatusBarNumCoun.Content = $"{grdCountries.SelectedItems.Count}/{grdCountries.Items.Count} Selected Countries";
    }

    #endregion grd_SelectionChanged

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

    #region Close_KeyPreviewESC

    /// <summary>
    ///
    /// </summary>
    /// <history>
    /// [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void Close_KeyPreviewESC(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        Close();
    }

    #endregion Close_KeyPreviewESC

    #region TextBoxNumeric_PreviewTextInput

    /// <summary>
    /// Valida que solo puedan insertar numeros enteros
    /// </summary>
    /// <history>
    /// [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void TextBoxNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      if (!char.IsDigit(e.Text, e.Text.Length - 1))
        e.Handled = true;
    }

    #endregion TextBoxNumeric_PreviewTextInput

    #region TextBoxNumeric_LostFocus

    /// <summary>
    /// Pone por Default el valor 0 en un TextBox al perder el foco y no tiene valor
    /// </summary>
    /// <history>
    /// [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void TextBoxNumeric_LostFocus(object sender, RoutedEventArgs e)
    {
      TextBox textBox = (TextBox)sender;
      if (textBox?.Text == string.Empty)
        textBox.Text = "0";
    }

    #endregion TextBoxNumeric_LostFocus

    private void grdGiftsQuantity_OnChecked(object sender, RoutedEventArgs e)
    {
      StatusBarNumGiftsQuantity.Content = $"{grdGiftsQuantity.Items.Cast<GiftQuantity>().Count(x => x.include)}/{_lstGiftsQuantity.Count} Selected Gifts";
    }

    #endregion Eventos del Formulario

    #region Métodos Publicos

    /// <summary>
    /// Configuracion de controles del formulario.
    /// </summary>
    /// <history>
    /// [aalcocer] 03/Mar/2016 Created
    /// </history>
    public void ConfigurarFomulario(bool blnOneDate, bool blnOnlyOneRegister, bool blnOnePeriod = false,
        bool blnPersonnel = false, bool blnAllPersonnel = false, bool blnLeadSources = false, bool blnAllLeadSources = false,
        bool blnChargeTo = false, bool blnAllChargeTo = false, bool blnGifts = false, bool blnAllGifts = false,
        bool blnMarkets = false, bool blnAllMarkets = false, bool blnAgencies = false, bool blnAllAgencies = false,
        bool blnCountries = false, bool blnAllCountries = false, bool blnGiftsQuantity = false, bool blnAllGiftsQuantity = false,
        EnumPeriod enumPeriod = EnumPeriod.pdNone, EnumProgram enumPrograms = EnumProgram.All, EnumBasedOnArrival? enumBasedOnArrival = null,
        EnumBasedOnBooking? enumBasedOnBooking = null, EnumQuinellas? enumQuinellas = null, EnumDetailGifts? enumDetailGifts = null,
        EnumSalesByMemberShipType? enumSalesByMemberShipType = null, EnumStatus? enumStatus = null, EnumGiftsReceiptType? enumGiftsReceiptType = null,
        string strGuestID = null, EnumGiftSale? enumGiftSale = null, EnumSaveCourtesyTours? enumSaveCourtesyTours = null,
        EnumExternalInvitation? enumExternalInvitation = null, bool blnClub = false, bool blnNight = false, bool blnLsHotelNotNull = false)
    {
      ConfigureDates(blnOneDate, enumPeriod);

      if (blnPersonnel)
      {
        var x = BRLeadSources.GetLeadSourcesByUser(App.userData.User.peID, EnumToListHelper.GetEnumDescription(enumPrograms)).Select(y => y.lsID);
        _lstPersonnel = BRPersonnel.GetPersonnel(string.Join(",", x), roles: "PR", status: 0);
      }
      if (blnLeadSources)
      {
        _lstLeadSources = BRLeadSources.GetLeadSourcesByUser(App.userData.User.peID, EnumToListHelper.GetEnumDescription(enumPrograms));
        if (blnLsHotelNotNull)
        {
          var lstLsIDHotelNotNull = BRLeadSources.GetLeadSources(1).
              Where(x => x.lsHotel != null).
              Select(x => x.lsID);
          _lstLeadSources = _lstLeadSources.Where(x => lstLsIDHotelNotNull.Contains(x.lsID)).ToList();
        }
      }
      if (blnGifts) _lstGifts = BRGifts.GetGifts();
      if (blnGiftsQuantity)
      {
        Dictionary<string, int> _productionByGiftQuantity = GetSettings.ProductionByGiftQuantity();
        List<GiftShort> _giftShorts = BRGifts.GetGiftsShortById(_productionByGiftQuantity.Keys);

        _lstGiftsQuantity = (from a in _giftShorts
                             join b in _productionByGiftQuantity on a.giID equals b.Key
                             select new GiftQuantity
                             {
                               giID = a.giID,
                               gigc = a.gigc,
                               giN = a.giN,
                               quantity = b.Value
                             }).ToList();
      }
      if (blnChargeTo) _lstCharteTo = BRChargeTos.GetChargeTos();

      if (blnAgencies) _lstAgencies = BRAgencies.GetAgencies();

      if (blnMarkets) _lstMarkets = BRMarkets.GetMarkets(1);

      //if (blnAgencies)
      //{
      //  List<string> _productionByAgencyMonthly = GetSettings.ProductionByAgencyMonthly();
      //  _lstAgencies = BRAgencies.GetAgenciesShortById(_productionByAgencyMonthly);
      //}

      if (blnClub) cboClub.ItemsSource = BRClubs.GetClubs(nStatus: 1);

      //Configuracion de Grids
      ConfigureGridPanels(blnPersonnel, blnLeadSources, blnChargeTo, blnGifts, blnGiftsQuantity, blnMarkets, blnAgencies, blnCountries);
      ConfigureSelection(blnOnlyOneRegister);
      ConfigureCheckboxSelectAll(blnOnlyOneRegister, blnAllPersonnel, blnAllLeadSources, blnAllChargeTo, blnAllGifts, blnAllGiftsQuantity, blnAllMarkets, blnAllAgencies, blnAllCountries);
      ConfigureFilters(enumBasedOnArrival, enumBasedOnBooking, enumQuinellas, enumDetailGifts, enumSalesByMemberShipType,
          enumStatus, enumGiftsReceiptType, strGuestID, enumGiftSale, enumSaveCourtesyTours, enumExternalInvitation, blnClub, blnNight);
      LoadUserFilters();
    }

    #endregion Métodos Publicos

    #region Métodos Privados

    #region ConfigureFilters

    /// <summary>
    /// Configura los controles que sirven para filtrar los reportes.
    /// </summary>
    /// <history>
    /// [aalcocer] 15/03/2016 Created
    /// </history>
    private void ConfigureFilters(EnumBasedOnArrival? enumBasedOnArrival, EnumBasedOnBooking? enumBasedOnBooking,
        EnumQuinellas? enumQuinellas, EnumDetailGifts? enumDetailGifts, EnumSalesByMemberShipType? enumSalesByMemberShipType,
        EnumStatus? enumStatus, EnumGiftsReceiptType? enumGiftsReceiptType, string strGuestID, EnumGiftSale? enumGiftSale,
        EnumSaveCourtesyTours? enumSaveCourtesyTours, EnumExternalInvitation? enumExternalInvitation, bool blnClub, bool blnNight)
    {
      if (enumBasedOnArrival != null)
        chkBasedOnArrival.IsChecked = enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival;
      else
        chkBasedOnArrival.Visibility = Visibility.Collapsed;

      if (enumBasedOnBooking != null)
        chkBasedOnBooking.IsChecked = enumBasedOnBooking == EnumBasedOnBooking.bobBasedOnBooking;
      else
        chkBasedOnBooking.Visibility = Visibility.Collapsed;

      if (enumQuinellas != null)
        chkQuinellas.IsChecked = enumQuinellas == EnumQuinellas.quQuinellas;
      else
        chkQuinellas.Visibility = Visibility.Collapsed;

      if (enumDetailGifts != null)
        chkDetailGifts.IsChecked = enumDetailGifts == EnumDetailGifts.dgDetailGifts;
      else
        chkDetailGifts.Visibility = Visibility.Collapsed;

      if (enumSalesByMemberShipType != null)
        chkSalesByMembershipType.IsChecked = enumSalesByMemberShipType == EnumSalesByMemberShipType.sbmDetail;
      else
        chkSalesByMembershipType.Visibility = Visibility.Collapsed;

      if (enumStatus != null)
        cboStatus.SelectedValue = enumStatus;
      else
        cboStatus.Visibility = lblStatus.Visibility = Visibility.Collapsed;

      if (enumGiftsReceiptType != null)
        cboGiftsReceiptType.SelectedValue = enumGiftsReceiptType;
      else
        cboGiftsReceiptType.Visibility = lblGiftReceiptType.Visibility = Visibility.Collapsed;

      if (strGuestID != null)
        txtGuestID.Text = strGuestID;
      else
        txtGuestID.Visibility = lblGuestID.Visibility = Visibility.Collapsed;

      if (enumGiftSale != null)
        cboGiftSale.SelectedValue = enumGiftSale;
      else
        cboGiftSale.Visibility = lblGiftSale.Visibility = Visibility.Collapsed;

      if (enumSaveCourtesyTours != null)
        cboSaveCourtesyTours.SelectedValue = enumSaveCourtesyTours;
      else
        cboSaveCourtesyTours.Visibility = Visibility.Collapsed;

      if (enumExternalInvitation != null)
        cboExternal.SelectedValue = enumExternalInvitation;
      else
        cboExternal.Visibility = Visibility.Collapsed;

      if (!blnClub)
        brdClub.Visibility = Visibility.Collapsed;

      txtStartN.Visibility = txtEndN.Visibility = lblNights.Visibility = lblNigths2.Visibility = blnNight ? Visibility.Visible : Visibility.Collapsed;
    }

    #endregion ConfigureFilters

    #region ConfigureGridPanels

    /// <summary>
    /// Configura los grids.
    /// </summary>
    /// <history>
    /// [aalcocer] 05/Mar/2016 Created
    /// </history>
    private void ConfigureGridPanels(bool blnPersonnel, bool blnLeadSources, bool blnChargeTo,
      bool blnGifts, bool blnGiftsQuantity, bool blnMarkets, bool blnAgencies, bool blnCountries)
    {
      pnlPersonnel.Visibility = blnPersonnel ? Visibility.Visible : Visibility.Collapsed;
      pnlLeadSource.Visibility = blnLeadSources ? Visibility.Visible : Visibility.Collapsed;
      pnlChargeTo.Visibility = blnChargeTo ? Visibility.Visible : Visibility.Collapsed;
      pnlGifts.Visibility = blnGifts ? Visibility.Visible : Visibility.Collapsed;
      pnlGiftsQuantity.Visibility = blnGiftsQuantity ? Visibility.Visible : Visibility.Collapsed;
      pnlMarkets.Visibility = blnMarkets ? Visibility.Visible : Visibility.Collapsed;
      pnlAgencies.Visibility = blnAgencies ? Visibility.Visible : Visibility.Collapsed;
      pnlCountries.Visibility = blnCountries ? Visibility.Visible : Visibility.Collapsed;

      grdPersonnel.ItemsSource = blnPersonnel ? _lstPersonnel : null;
      grdLeadSources.ItemsSource = blnLeadSources ? _lstLeadSources : null;
      grdChargeTo.ItemsSource = blnChargeTo ? _lstCharteTo : null;
      grdGifts.ItemsSource = blnGifts ? _lstGifts : null;
      grdGiftsQuantity.ItemsSource = blnGiftsQuantity ? _lstGiftsQuantity : null;
      grdMarkets.ItemsSource = blnMarkets ? _lstMarkets : null;
      grdAgencies.ItemsSource = blnAgencies ? _lstAgencies : null;
      grdCountries.ItemsSource = blnCountries ? _lstCountries : null;

      StatusBarNumPR.Content = blnPersonnel ? $"{0}/{_lstPersonnel.Count} Selected Personnel" : "";
      StatusBarNumLS.Content = blnLeadSources ? $"{0}/{_lstLeadSources.Count} Selected LeadSources" : "";
      StatusBarNumCha.Content = (blnChargeTo) ? $"{0}/{_lstCharteTo.Count} Selected ChargeTo" : "";
      StatusBarNumGifts.Content = blnGifts ? $"{0}/{_lstGifts.Count} Selected Gifts" : "";
      StatusBarNumGiftsQuantity.Content = blnGiftsQuantity ? $"{0}/{_lstGiftsQuantity.Count} Selected Gifts" : "";
      StatusBarNumMark.Content = (blnMarkets) ? $"{0}/{_lstMarkets.Count} Selected Markets" : "";
      StatusBarNumAgen.Content = (blnAgencies) ? $"{0}/{_lstAgencies.Count} Selected Agencies" : "";
      StatusBarNumCoun.Content = (blnCountries) ? $"{0}/{_lstCountries.Count} Selected Countries" : "";
    }

    #endregion ConfigureGridPanels

    #region ConfigureSelection

    /// <summary>
    /// Configura el modo de seleccion de los grids(Multiseleccion ó Solo un registro).
    /// Activa o desactiva los controles checkbox dependiendo el modo de seleccion configurado.
    /// </summary>
    /// <history>
    /// [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void ConfigureSelection(bool blnOnlyOneRegister)
    {
      grdPersonnel.SelectionMode = blnOnlyOneRegister ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdLeadSources.SelectionMode = blnOnlyOneRegister ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdChargeTo.SelectionMode = blnOnlyOneRegister ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdGifts.SelectionMode = blnOnlyOneRegister ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdGiftsQuantity.SelectionMode = blnOnlyOneRegister ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdMarkets.SelectionMode = blnOnlyOneRegister ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdAgencies.SelectionMode = blnOnlyOneRegister ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      grdCountries.SelectionMode = blnOnlyOneRegister ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
    }

    #endregion ConfigureSelection

    #region ConfigureDates

    /// <summary>
    /// Configura los controles de fecha.
    /// </summary>
    /// <history>
    /// [aalcocer] 16/Mar/2016 Created
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
    /// [aalcocer] 16/Mar/2016 Created
    /// </history>
    private void ConfigureCheckboxSelectAll(bool blnOnlyOneRegister, bool blnAllPersonnel,
      bool blnAllLeadSources, bool blnAllChargeTo, bool blnAllGifts, bool blnAllGiftsQuantity, bool blnAllMarkets,
      bool blnAllAgencies, bool blnAllCountries)
    {
      chkAllPersonnel.IsChecked = blnAllPersonnel;
      chkAllLeadSources.IsChecked = blnAllLeadSources;
      chkAllChargeTo.IsChecked = blnAllChargeTo;
      chkAllGifts.IsChecked = blnAllGifts;
      chkAllGiftsQuantity.IsChecked = blnAllGiftsQuantity;
      chkAllMarkets.IsChecked = blnAllMarkets;
      chkAllAgencies.IsChecked = blnAllAgencies;
      chkAllCountries.IsChecked = blnAllCountries;

      chkAllPersonnel.IsEnabled = !blnOnlyOneRegister;
      chkAllLeadSources.IsEnabled = !blnOnlyOneRegister;
      chkAllChargeTo.IsEnabled = !blnOnlyOneRegister;
      chkAllGifts.IsEnabled = !blnOnlyOneRegister;
      chkAllGiftsQuantity.IsEnabled = !blnOnlyOneRegister;
      chkAllMarkets.IsEnabled = !blnOnlyOneRegister;
      chkAllAgencies.IsEnabled = !blnOnlyOneRegister;
      chkAllCountries.IsEnabled = !blnOnlyOneRegister;
    }

    #endregion ConfigureCheckboxSelectAll

    #region saveFrmFilterValues

    /// <summary>
    /// Guarda los datos seleccionados por el usuario.
    /// </summary>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// </history>
    private void SaveFrmFilterValues()
    {
      if (pnlPersonnel.IsVisible)
        frmIH._lstPersonnel = grdPersonnel.SelectedItems.Cast<PersonnelShort>().Select(x => x.peID).ToList();
      if (pnlLeadSource.IsVisible)
        frmIH._lstLeadSources = grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(x => x.lsID).ToList();
      if (pnlMarkets.IsVisible)
        frmIH._lstMarkets = grdMarkets.SelectedItems.Cast<MarketShort>().Select(c => c.mkID).ToList();
      if (pnlAgencies.IsVisible)
        frmIH._lstAgencies = grdAgencies.SelectedItems.Cast<Agency>().Select(c => c.agID).ToList();

      frmIH._cboDateSelected = cboDate.SelectedValue.ToString();
      frmIH._dtmStart = dtmStart.Value.Value;
      frmIH._dtmEnd = dtmEnd.Value.Value;
      frmIH._enumBasedOnArrival = (chkBasedOnArrival.IsChecked.Value) ? EnumBasedOnArrival.boaBasedOnArrival : EnumBasedOnArrival.boaNoBasedOnArrival;
      //frmIH._enumBasedOnBooking = (chkBasedOnBooking.IsChecked.Value) ? EnumBasedOnBooking.bobBasedOnBooking : EnumBasedOnBooking.bobNoBasedOnBooking;
      frmIH._enumQuinellas = (chkQuinellas.IsChecked.Value) ? EnumQuinellas.quQuinellas : EnumQuinellas.quNoQuinellas;
      frmIH._enumDetailsGift = (chkDetailGifts.IsChecked.Value) ? EnumDetailGifts.dgDetailGifts : EnumDetailGifts.dgNoDetailGifts;
      frmIH._enumSalesByMemberShipType = (chkSalesByMembershipType.IsChecked.Value) ? EnumSalesByMemberShipType.sbmDetail : EnumSalesByMemberShipType.sbmNoDetail;
      frmIH._enumStatus = ((KeyValuePair<EnumStatus, string>)cboStatus.SelectedItem).Key;
      //frmIH._enumGiftsReceiptType = ((KeyValuePair<EnumGiftsReceiptType, string>)cboGiftsReceiptType.SelectedItem).Key;
      frmIH._guestID = txtGuestID.Text;
      //frmIH._enumGiftSale = ((KeyValuePair<EnumGiftSale, string>)cboGiftSale.SelectedItem).Key;
      frmIH._enumSaveCourtesyTours = ((KeyValuePair<EnumSaveCourtesyTours, string>)cboSaveCourtesyTours.SelectedItem).Key;
      frmIH._enumExternalInvitation = ((KeyValuePair<EnumExternalInvitation, string>)cboExternal.SelectedItem).Key;
    }

    #endregion saveFrmFilterValues

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
      if (pnlPersonnel.Visibility == Visibility.Visible && frmIH._lstPersonnel.Any())
      {
        chkAllPersonnel.IsChecked = false;
        frmIH._lstPersonnel.ForEach(c => grdPersonnel.SelectedItems.Add(_lstPersonnel.SingleOrDefault(x => x.peID == c)));
        if (grdPersonnel.SelectedItems.Count == grdPersonnel.Items.Count)
          chkAllPersonnel.IsChecked = true;
      }
      if (pnlLeadSource.Visibility == Visibility.Visible && frmIH._lstLeadSources.Any())
      {
        chkAllLeadSources.IsChecked = false;
        frmIH._lstLeadSources.ForEach(c => grdLeadSources.SelectedItems.Add(_lstLeadSources.SingleOrDefault(x => x.lsID == c)));
        if (grdLeadSources.SelectedItems.Count == grdLeadSources.Items.Count)
          chkAllLeadSources.IsChecked = true;
      }

      if (pnlMarkets.Visibility == Visibility.Visible && frmIH._lstMarkets.Any())
      {
        chkAllMarkets.IsChecked = false;
        frmIH._lstMarkets.ForEach(c => grdMarkets.SelectedItems.Add(_lstMarkets.SingleOrDefault(x => x.mkID == c)));
        if (grdMarkets.SelectedItems.Count == grdMarkets.Items.Count)
          chkAllMarkets.IsChecked = true;
      }
      if (pnlAgencies.Visibility == Visibility.Visible && frmIH._lstAgencies.Any())
      {
        chkAllAgencies.IsChecked = false;
        frmIH._lstAgencies.ForEach(c => grdAgencies.SelectedItems.Add(_lstAgencies.SingleOrDefault(x => x.agID == c)));
        if (grdAgencies.SelectedItems.Count == grdAgencies.Items.Count)
          chkAllAgencies.IsChecked = true;
      }

      cboDate.SelectedValue = frmIH._cboDateSelected ?? "Dates Specified";
      dtmStart.Value = frmIH._dtmStart;
      dtmEnd.Value = frmIH._dtmEnd;
      chkBasedOnArrival.IsChecked = (frmIH._enumBasedOnArrival == EnumBasedOnArrival.boaBasedOnArrival);
      //chkBasedOnBooking.IsChecked = (frmIH._enumBasedOnBooking == EnumBasedOnBooking.bobBasedOnBooking) ? true : false;
      chkQuinellas.IsChecked = (frmIH._enumQuinellas == EnumQuinellas.quQuinellas);
      chkDetailGifts.IsChecked = (frmIH._enumDetailsGift == EnumDetailGifts.dgDetailGifts);
      chkSalesByMembershipType.IsChecked = (frmIH._enumSalesByMemberShipType == EnumSalesByMemberShipType.sbmDetail);
      cboStatus.SelectedValue = frmIH._enumStatus;
      //cboGiftsReceiptType.SelectedValue = frmIH._enumGiftsReceiptType;
      txtGuestID.Text = frmIH._guestID;
      //cboGiftSale.SelectedValue = frmIH._enumGiftSale;
      cboSaveCourtesyTours.SelectedValue = frmIH._enumSaveCourtesyTours;
      cboExternal.SelectedValue = frmIH._enumExternalInvitation;
    }

    #endregion LoadUserFilters

    #region ValidateFields

    /// <summary>
    /// Valida si los grid tienen al menos un elemento seleccionado.
    /// </summary>
    /// <returns>Message/Empty</returns>
    /// <history>
    /// [aalcocer] 24/Mar/2016 Created
    /// </history>
    private string ValidateFields()
    {
      if (pnlLeadSource.Visibility == Visibility.Visible && grdLeadSources.SelectedItems.Count == 0)
        return "No Lead Source is selected";
      if (pnlMarkets.Visibility == Visibility.Visible && grdMarkets.SelectedItems.Count == 0)
        return "No Market is selected";
      if (pnlAgencies.Visibility == Visibility.Visible && grdAgencies.SelectedItems.Count == 0)
        return "No Agency is selected";
      //if (pnlRateTypes.Visibility == Visibility.Visible && grdRatetypes.SelectedItems.Count == 0)
      //  return "No Rate types is selected.";
      //if (pnlPrograms.Visibility == Visibility.Visible && grdPrograms.SelectedItems.Count == 0)
      //  return "No program is selected.";
      //if (pnlLeadSource.Visibility == Visibility.Visible && grdLeadSources.SelectedItems.Count == 0)
      //  return "No lead source is selected.";
      //if (pnlGifts.Visibility == Visibility.Visible && grdGifts.SelectedItems.Count == 0)
      //  return "No gift is selected.";
      //if (pnlCategories.Visibility == Visibility.Visible && grdCategories.SelectedItems.Count == 0)
      //  return "No category is selected.";
      if (pnlDtmEnd.IsEnabled && dtmEnd.Value.Value < dtmStart.Value.Value)
        return "End date must be greater than start date.";
      return string.Empty;
    }

    #endregion ValidateFields

    #endregion Métodos Privados
  }
}