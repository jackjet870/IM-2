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

    public bool _blnOK;
    public frmProcessorInhouse _frmIh = new frmProcessorInhouse();

    #endregion Atributos

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
      bool _checked = Convert.ToBoolean(_checkBox.IsChecked);
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

      if (_dataGrid.Equals(grdGiftsQuantity))
        StatusBarNumGiftsQuantity.Content = $"{grdGiftsQuantity.Items.Cast<GiftQuantity>().Count(x => x.include)}/{grdGiftsQuantity.Items.Count} Selected Gifts";
      else if (_dataGrid.Equals(grdMarkets))
      {
        List<string> selectedMarkets = grdMarkets.SelectedItems.Cast<MarketShort>().Select(m => m.mkID).ToList();
        grdAgencies.ItemsSource = _lstAgencies.Where(c => selectedMarkets.Contains(c.agmk));
        if (Convert.ToBoolean(chkAllAgencies.IsChecked))
          grdAgencies.SelectAll();
      }
    }

    #endregion grd_SelectionChanged

    #region cboDate_SelectionChanged

    /// <summary>
    /// Modifica los rangos de fecha de los datepicker, según la opcion seleccionada.
    /// </summary>
    /// <history>
    /// [aalcocer] 11/03/2016 Created
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

    #region grdGiftsQuantity_OnChecked

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void grdGiftsQuantity_OnChecked(object sender, RoutedEventArgs e)
    {
      StatusBarNumGiftsQuantity.Content = $"{grdGiftsQuantity.Items.Cast<GiftQuantity>().Count(x => x.include)}/{_lstGiftsQuantity.Count} Selected Gifts";
    }

    #endregion grdGiftsQuantity_OnChecked

    #endregion Eventos del Formulario

    #region Métodos Publicos

    /// <summary>
    /// Configuracion de controles del formulario.
    /// </summary>
    /// <history>
    /// [aalcocer] 03/Mar/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// [edgrodriguez] 21/May/2016 Modified El método GetLeadSourcesByUser se volvió asincrónico.
    /// </history>
    public void ConfigurarFomulario(bool blnOneDate, bool blnOnlyOneRegister,
        bool blnPersonnel = false, bool blnAllPersonnel = false, bool blnLeadSources = false, bool blnAllLeadSources = false,
        bool blnChargeTo = false, bool blnAllChargeTo = false, bool blnGifts = false, bool blnAllGifts = false,
        bool blnMarkets = false, bool blnAllMarkets = false, bool blnAgencies = false, bool blnAllAgencies = false,
        bool blnGiftsQuantity = false, bool blnAllGiftsQuantity = false,
        EnumPeriod enumPeriod = EnumPeriod.None, EnumProgram program = EnumProgram.All, EnumBasedOnArrival? enumBasedOnArrival = null,
        EnumBasedOnPRLocation? enumBasedOnPRLocation = null, EnumQuinellas? enumQuinellas = null, EnumDetailGifts? enumDetailGifts = null,
        EnumSalesByMemberShipType? enumSalesByMemberShipType = null,
        EnumSaveCourtesyTours? enumSaveCourtesyTours = null,
        EnumExternalInvitation? enumExternalInvitation = null, bool blnClub = false, bool blnNight = false, bool blnLsHotelNotNull = false, bool blnAgencyMonthly = false,
        EnumOnlyWholesalers?  enumOnlyWholesalers = null)
    {
      ConfigureDates(blnOneDate, enumPeriod);
      ConfigureFilters(enumBasedOnArrival, enumBasedOnPRLocation, enumQuinellas, enumDetailGifts, enumSalesByMemberShipType, enumSaveCourtesyTours, enumExternalInvitation, blnClub, blnNight, enumOnlyWholesalers);

      LoadCombos(blnClub);

      #region Configuracion de Grids.

      LoadPersonnel(blnOnlyOneRegister, blnPersonnel, blnAllPersonnel, program);
      LoadLeadSources(blnOnlyOneRegister, blnLeadSources, blnAllLeadSources, program, blnLsHotelNotNull);
      LoadGifts(blnOnlyOneRegister, blnGifts, blnAllGifts);
      LoadGiftsQuantity(blnOnlyOneRegister, blnGiftsQuantity, blnAllGiftsQuantity);
      LoadChargeTo(blnOnlyOneRegister, blnChargeTo, blnAllChargeTo);
      LoadAgencies(blnOnlyOneRegister, blnAgencies, blnAllAgencies, blnAgencyMonthly);
      LoadMarkets(blnOnlyOneRegister, blnMarkets, blnAllMarkets);

      #endregion Configuracion de Grids.
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
    private void ConfigureFilters(EnumBasedOnArrival? enumBasedOnArrival, EnumBasedOnPRLocation? enumBasedOnPRLocation,
        EnumQuinellas? enumQuinellas, EnumDetailGifts? enumDetailGifts, EnumSalesByMemberShipType? enumSalesByMemberShipType,
       EnumSaveCourtesyTours? enumSaveCourtesyTours, EnumExternalInvitation? enumExternalInvitation, bool blnClub, bool blnNight, EnumOnlyWholesalers? enumOnlyWholesalers)
    {
      if (enumBasedOnArrival != null)
        chkBasedOnArrival.IsChecked = Convert.ToBoolean(enumBasedOnArrival);
      else
        chkBasedOnArrival.Visibility = Visibility.Collapsed;

      if (enumBasedOnPRLocation != null)
        chkBasedOnPRLocation.IsChecked = Convert.ToBoolean(enumBasedOnPRLocation);
      else
        chkBasedOnPRLocation.Visibility = Visibility.Collapsed;
      if (enumQuinellas != null)
        chkQuinellas.IsChecked = Convert.ToBoolean(enumQuinellas);
      else
        chkQuinellas.Visibility = Visibility.Collapsed;

      if (enumDetailGifts != null)
        chkDetailGifts.IsChecked = Convert.ToBoolean(enumDetailGifts);
      else
        chkDetailGifts.Visibility = Visibility.Collapsed;

      if (enumSalesByMemberShipType != null)
        chkSalesByMembershipType.IsChecked = Convert.ToBoolean(enumSalesByMemberShipType);
      else
        chkSalesByMembershipType.Visibility = Visibility.Collapsed;

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


      if (enumOnlyWholesalers != null)
        chkOnlyWholesalers.IsChecked = Convert.ToBoolean(enumOnlyWholesalers);
      else
        chkOnlyWholesalers.Visibility = Visibility.Collapsed;

      txtStartN.Visibility = txtEndN.Visibility = lblNights.Visibility = lblNigths2.Visibility = blnNight ? Visibility.Visible : Visibility.Collapsed;
    }

    #endregion ConfigureFilters

    #region ConfigureDates

    /// <summary>
    /// Configura los controles de fecha.
    /// </summary>
    /// <history>
    /// [aalcocer] 16/Mar/2016 Created
    /// [aalcocer
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
      {
        _frmIh._clsFilter.LstPersonnel = grdPersonnel.SelectedItems.Cast<PersonnelShort>().Select(x => x.peID).ToList();
        _frmIh._clsFilter.BlnAllPersonnel = grdPersonnel.Items.Count == grdPersonnel.SelectedItems.Count;
      }
      if (pnlLeadSource.IsVisible)
      {
        _frmIh._clsFilter.LstLeadSources = grdLeadSources.SelectedItems.Cast<LeadSourceByUser>().Select(x => x.lsID).ToList();
        _frmIh._clsFilter.BlnAllLeadSources = grdLeadSources.Items.Count == grdLeadSources.SelectedItems.Count;
      }
      if (pnlMarkets.IsVisible)
      {
        _frmIh._clsFilter.LstMarkets = grdMarkets.SelectedItems.Cast<MarketShort>().Select(c => c.mkID).ToList();
        _frmIh._clsFilter.BlnAllMarkets = grdMarkets.Items.Count == grdMarkets.SelectedItems.Count;
      }
      if (pnlAgencies.IsVisible)
      {
        _frmIh._clsFilter.LstAgencies = grdAgencies.SelectedItems.Cast<Agency>().Select(c => c.agID).ToList();
        _frmIh._clsFilter.BlnAllAgencies = grdAgencies.Items.Count == grdAgencies.SelectedItems.Count;
      }
      if (pnlChargeTo.IsVisible)
      {
        _frmIh._clsFilter.LstChargeTo = grdChargeTo.SelectedItems.Cast<ChargeTo>().Select(c => c.ctID).ToList();
        _frmIh._clsFilter.BlnAllChargeTo = grdChargeTo.Items.Count == grdChargeTo.SelectedItems.Count;
      }
      if (pnlGifts.IsVisible)
      {
        _frmIh._clsFilter.LstGifts = grdGifts.SelectedItems.Cast<GiftShort>().Select(c => c.giID).ToList();
        _frmIh._clsFilter.BlnAllGifts = grdGifts.Items.Count == grdGifts.SelectedItems.Count;
      }
      if (pnlGiftsQuantity.IsVisible)
      {
        _frmIh._clsFilter.LstGiftsQuantity = grdGiftsQuantity.Items.Cast<GiftQuantity>().Where(x => x.include).ToDictionary(x => x.giID, x => x.quantity);        
      }

      if (cboDate.IsEnabled)
      {
        _frmIh._clsFilter.DtmStart = Convert.ToDateTime(dtmStart.Value);
        _frmIh._clsFilter.CboDateSelected = ((KeyValuePair<EnumPredefinedDate, string>)cboDate.SelectedItem).Key;
      }
      else
        _frmIh._clsFilter.DtmInit = Convert.ToDateTime(dtmStart.Value);
      _frmIh._clsFilter.DtmEnd = Convert.ToDateTime(dtmEnd.Value);
      _frmIh._clsFilter.EnumBasedOnArrival = Convert.ToBoolean(chkBasedOnArrival.IsChecked) ? EnumBasedOnArrival.BasedOnArrival : EnumBasedOnArrival.NoBasedOnArrival;
      _frmIh._clsFilter.EnumQuinellas = Convert.ToBoolean(chkQuinellas.IsChecked) ? EnumQuinellas.Quinellas : EnumQuinellas.NoQuinellas;
      _frmIh._clsFilter.EnumDetailGifts = Convert.ToBoolean(chkDetailGifts.IsChecked) ? EnumDetailGifts.DetailGifts : EnumDetailGifts.NoDetailGifts;
      _frmIh._clsFilter.EnumSalesByMemberShipType = Convert.ToBoolean(chkSalesByMembershipType.IsChecked) ? EnumSalesByMemberShipType.Detail : EnumSalesByMemberShipType.NoDetail;
      _frmIh._clsFilter.EnumOnlyWholesalers = Convert.ToBoolean(chkOnlyWholesalers.IsChecked) ? EnumOnlyWholesalers.OnlyWholesalers : EnumOnlyWholesalers.NoOnlyWholesalers;
      _frmIh._clsFilter.StrApplication = txtApplication.Text;
      _frmIh._clsFilter.IntCompany = Convert.ToInt32(txtCompany.Text);
      _frmIh._clsFilter.Club = (Club)cboClub.SelectedItem;
      if(cboSaveCourtesyTours.SelectedItem!=null)
        _frmIh._clsFilter.EnumSaveCourtesyTours = ((KeyValuePair<EnumSaveCourtesyTours, string>)cboSaveCourtesyTours.SelectedItem).Key;
      if (cboExternal.SelectedItem != null)
        _frmIh._clsFilter.EnumExternalInvitation = ((KeyValuePair<EnumExternalInvitation, string>)cboExternal.SelectedItem).Key;
    }

    #endregion saveFrmFilterValues

    #region LoadUserFilters

    /// <summary>
    /// Obtiene los filtros que el usuario habiá seleccionado
    /// y los aplica al formulario.
    /// </summary>
    /// <history>
    /// [aalcocer] 23/Mar/2016 Created
    /// </history>
    private void LoadUserFilters()
    {
      if(cboDate.IsEnabled)
        cboDate.SelectedValue = cboDate.Items.Cast<KeyValuePair<EnumPredefinedDate, string>>().Any(c => c.Key == _frmIh._clsFilter.CboDateSelected) ? _frmIh._clsFilter.CboDateSelected : EnumPredefinedDate.DatesSpecified;

      dtmStart.Value = cboDate.IsEnabled ? _frmIh._clsFilter.DtmStart : _frmIh._clsFilter.DtmInit;
      dtmEnd.Value = _frmIh._clsFilter.DtmEnd;
      chkBasedOnArrival.IsChecked = Convert.ToBoolean(_frmIh._clsFilter.EnumBasedOnArrival);
      chkQuinellas.IsChecked = Convert.ToBoolean(_frmIh._clsFilter.EnumQuinellas);
      chkDetailGifts.IsChecked = Convert.ToBoolean(_frmIh._clsFilter.EnumDetailGifts);
      chkSalesByMembershipType.IsChecked = Convert.ToBoolean(_frmIh._clsFilter.EnumSalesByMemberShipType);
      chkOnlyWholesalers.IsChecked = Convert.ToBoolean(_frmIh._clsFilter.EnumOnlyWholesalers); 
      txtApplication.Text = _frmIh._clsFilter.StrApplication;
      txtCompany.Text = $"{_frmIh._clsFilter.IntCompany}";
      if (_frmIh._clsFilter.Club != null)
        cboClub.SelectedValue = _frmIh._clsFilter.Club.clID;
      cboSaveCourtesyTours.SelectedValue = _frmIh._clsFilter.EnumSaveCourtesyTours;
      cboExternal.SelectedValue = _frmIh._clsFilter.EnumExternalInvitation;
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
      if (pnlPersonnel.Visibility == Visibility.Visible && grdPersonnel.SelectedItems.Count == 0)
        return "No PR is selected.";
      if (pnlChargeTo.Visibility == Visibility.Visible && grdChargeTo.SelectedItems.Count == 0)
        return "No Charge To is selected.";
      if (pnlGifts.Visibility == Visibility.Visible && grdGifts.SelectedItems.Count == 0)
        return "No Gift is selected.";
      if (pnlGiftsQuantity.Visibility == Visibility.Visible && !grdGiftsQuantity.Items.Cast<GiftQuantity>().Any(x => x.include))
        return "No Gift is selected.";
      if (pnlDtmEnd.IsEnabled && Convert.ToDateTime(dtmEnd.Value) < Convert.ToDateTime(dtmStart.Value))
        return "End date must be greater than start date.";
      return string.Empty;
    }

    #endregion ValidateFields

    #region LoadCombos

    /// <summary>
    /// Carga los combobox.
    /// </summary>
    /// <param name="blnClub"></param>
    /// <history>
    /// [aalcocer] 24/May/2016 Created
    /// </history>
    private async void LoadCombos(bool blnClub)
    {
      cboSaveCourtesyTours.ItemsSource = EnumToListHelper.GetList<EnumSaveCourtesyTours>();
      cboExternal.ItemsSource = EnumToListHelper.GetList<EnumExternalInvitation>();

      if (blnClub)
        cboClub.ItemsSource = await BRClubs.GetClubs(nStatus: 1);

      LoadUserFilters();
    }

    #endregion LoadCombos

    #region LoadPersonnel

    /// <summary>
    /// Carga y configuracion del grid de Personnel
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnPersonnel"></param>
    /// <param name="blnAllPersonnel"></param>
    /// <param name="program"></param>
    private async void LoadPersonnel(bool blnOnlyOneRegister, bool blnPersonnel, bool blnAllPersonnel, EnumProgram program)
    {
      if (!blnPersonnel)
      {
        pnlPersonnel.Visibility = Visibility.Collapsed;
        return;
      }
      grdPersonnel.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      List<LeadSourceByUser> listLeadSourceByUsers = await BRLeadSources.GetLeadSourcesByUser(App.User.User.peID, program);
      _lstPersonnel = await BRPersonnel.GetPersonnel(string.Join(",", listLeadSourceByUsers.Select(y => y.lsID)), roles: "PR", status: 0);
      grdPersonnel.ItemsSource = _lstPersonnel;
      chkAllPersonnel.IsChecked = blnAllPersonnel;
      chkAllPersonnel.IsEnabled = !blnOnlyOneRegister;

      if (!_frmIh._clsFilter.LstPersonnel.Any()) return;

      chkAllPersonnel.IsChecked = false;
      _frmIh._clsFilter.LstPersonnel.ForEach(c => grdPersonnel.SelectedItems.Add(_lstPersonnel.SingleOrDefault(x => x.peID == c)));
      if (grdPersonnel.SelectedItems.Count == grdPersonnel.Items.Count)
        chkAllPersonnel.IsChecked = true;
    }

    #endregion LoadPersonnel

    #region LoadLeadSources

    /// <summary>
    /// Carga y configuracion del grid de LeadSources
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnLeadSources"></param>
    /// <param name="blnAllLeadSources"></param>
    /// <param name="program"></param>
    /// <param name="blnLsHotelNotNull"></param>
    /// <history>
    /// [aalcocer] 26/May/2016 Created
    /// </history>
    private async void LoadLeadSources(bool blnOnlyOneRegister, bool blnLeadSources, bool blnAllLeadSources, EnumProgram program, bool blnLsHotelNotNull)
    {
      if (!blnLeadSources)
      {
        pnlLeadSource.Visibility = Visibility.Collapsed;
        return;
      }
      grdLeadSources.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      _lstLeadSources = await BRLeadSources.GetLeadSourcesByUser(App.User.User.peID, program);
      if (blnLsHotelNotNull)
      {
        List<string> lstLsIDHotelNotNull = (await BRLeadSources.GetLeadSources(1, EnumProgram.All)).Where(x => x.lsHotel != null).Select(x => x.lsID).ToList();
        _lstLeadSources = _lstLeadSources.Where(x => lstLsIDHotelNotNull.Contains(x.lsID)).ToList();
      }
      grdLeadSources.ItemsSource = _lstLeadSources;
      chkAllLeadSources.IsChecked = blnAllLeadSources;
      chkAllLeadSources.IsEnabled = !blnOnlyOneRegister;

      if (!_frmIh._clsFilter.LstLeadSources.Any()) return;

      chkAllLeadSources.IsChecked = false;
      _frmIh._clsFilter.LstLeadSources.ForEach(c => grdLeadSources.SelectedItems.Add(_lstLeadSources.SingleOrDefault(x => x.lsID == c)));
      if (grdLeadSources.SelectedItems.Count == grdLeadSources.Items.Count)
        chkAllLeadSources.IsChecked = true;
    }

    #endregion LoadLeadSources

    #region LoadGifts

    /// <summary>
    /// Carga y configuracion del grid de Gifts
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnGifts"></param>
    /// <param name="blnAllGifts"></param>
    /// <history>
    /// [aalcocer] 26/May/2016 Created
    /// </history>
    private async void LoadGifts(bool blnOnlyOneRegister, bool blnGifts, bool blnAllGifts)
    {
      if (!blnGifts)
      {
        pnlGifts.Visibility = Visibility.Collapsed;
        return;
      }
      grdGifts.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      _lstGifts = await BRGifts.GetGiftsShort();
      grdGifts.ItemsSource = _lstGifts;
      chkAllGifts.IsChecked = blnAllGifts;
      chkAllGifts.IsEnabled = !blnOnlyOneRegister;

      if (!_frmIh._clsFilter.LstGifts.Any()) return;

      chkAllGifts.IsChecked = false;
      _frmIh._clsFilter.LstGifts.ForEach(c => grdGifts.SelectedItems.Add(_lstGifts.SingleOrDefault(x => x.giID == c)));
      if (grdGifts.SelectedItems.Count == grdGifts.Items.Count)
        chkAllGifts.IsChecked = true;
    }

    #endregion LoadGifts

    #region LoadGiftsQuantity

    /// <summary>
    /// Carga y configuracion del grid de Gifts Quantity
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnGiftsQuantity"></param>
    /// <param name="blnAllGiftsQuantity"></param>
    /// <history>
    /// [aalcocer] 26/May/2016 Created
    /// </history>
    private async void LoadGiftsQuantity(bool blnOnlyOneRegister, bool blnGiftsQuantity, bool blnAllGiftsQuantity)
    {
      if (!blnGiftsQuantity)
      {
        pnlGiftsQuantity.Visibility = Visibility.Collapsed;
        return;
      }
      grdGiftsQuantity.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      Dictionary<string, int> _productionByGiftQuantity = GetSettings.ProductionByGiftQuantity;
      List<string> listKeys = new List<string>();
      listKeys.AddRange(_productionByGiftQuantity.Keys);
      List<GiftShort> _giftShorts = await BRGifts.GetGiftsShortById(listKeys);
      _lstGiftsQuantity = (from a in _giftShorts
                           join b in _productionByGiftQuantity on a.giID equals b.Key
                           select new GiftQuantity
                           {
                             giID = a.giID,
                             gigc = a.gigc,
                             giN = a.giN,
                             quantity = b.Value
                           }).ToList();
      grdGiftsQuantity.ItemsSource = _lstGiftsQuantity;

      chkAllGiftsQuantity.IsChecked = blnAllGiftsQuantity;
      chkAllGiftsQuantity.IsEnabled = !blnOnlyOneRegister;
      StatusBarNumGiftsQuantity.Content = $"{grdGiftsQuantity.Items.Cast<GiftQuantity>().Count(x => x.include)} / {_lstGiftsQuantity.Count} Selected Gifts";

      if (!_frmIh._clsFilter.LstGiftsQuantity.Any()) return;

      chkAllGiftsQuantity.IsChecked = false;
      _frmIh._clsFilter.LstGiftsQuantity.ToList().ForEach(x =>
      {
        GiftQuantity giftQuantity = grdGiftsQuantity.Items.Cast<GiftQuantity>().SingleOrDefault(q => q.giID == x.Key);
        if (giftQuantity != null)
          giftQuantity.include = true;
      });

      if (grdGiftsQuantity.Items.Cast<GiftQuantity>().Count(c => c.include) == grdGiftsQuantity.Items.Count)
        chkAllGiftsQuantity.IsChecked = true;
    }

    #endregion LoadGiftsQuantity

    #region LoadChargeTo

    /// <summary>
    /// Carga y configuracion del grid de Charge To
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnChargeTo"></param>
    /// <param name="blnAllChargeTo"></param>
    /// <history>
    /// [aalcocer] 26/May/2016 Created
    /// </history>
    private async void LoadChargeTo(bool blnOnlyOneRegister, bool blnChargeTo, bool blnAllChargeTo)
    {
      if (!blnChargeTo)
      {
        pnlChargeTo.Visibility = Visibility.Collapsed;
        return;
      }
      grdChargeTo.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      _lstCharteTo = await BRChargeTos.GetChargeTos();
      grdChargeTo.ItemsSource = _lstCharteTo;
      chkAllChargeTo.IsChecked = blnAllChargeTo;
      chkAllChargeTo.IsEnabled = !blnOnlyOneRegister;

      if (!_frmIh._clsFilter.LstChargeTo.Any()) return;

      chkAllChargeTo.IsChecked = false;
      _frmIh._clsFilter.LstChargeTo.ForEach(c => grdChargeTo.SelectedItems.Add(_lstCharteTo.SingleOrDefault(x => x.ctID == c)));
      if (grdChargeTo.SelectedItems.Count == grdChargeTo.Items.Count)
        chkAllChargeTo.IsChecked = true;
    }

    #endregion LoadChargeTo

    #region LoadAgencies

    /// <summary>
    /// Carga y configuracion del grid de Agencies
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnAgencies"></param>
    /// <param name="blnAllAgencies"></param>
    /// <param name="blnAgencyMonthly"></param>
    /// <history>
    /// [aalcocer] 26/May/2016 Created
    /// </history>
    private async void LoadAgencies(bool blnOnlyOneRegister, bool blnAgencies, bool blnAllAgencies, bool blnAgencyMonthly)
    {
      if (!blnAgencies)
      {
        pnlAgencies.Visibility = Visibility.Collapsed;
        return;
      }

      grdAgencies.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      if (blnAgencyMonthly)
        _lstAgencies = await BRAgencies.GetAgenciesByIds(GetSettings.ProductionByAgencyMonthly);
      else
        _lstAgencies = await BRAgencies.GetAgencies();
      grdAgencies.ItemsSource = _lstAgencies;
      chkAllAgencies.IsChecked = blnAllAgencies;
      chkAllAgencies.IsEnabled = !blnOnlyOneRegister;

      if (!_frmIh._clsFilter.LstAgencies.Any()) return;

      chkAllAgencies.IsChecked = false;
      _frmIh._clsFilter.LstAgencies.ForEach(c => grdAgencies.SelectedItems.Add(_lstAgencies.SingleOrDefault(x => x.agID == c)));
      if (grdAgencies.SelectedItems.Count == grdAgencies.Items.Count)
        chkAllAgencies.IsChecked = true;
    }

    #endregion LoadAgencies

    #region LoadMarkets

    /// <summary>
    /// Carga y configuracion del grid de Markets
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnMarkets"></param>
    /// <param name="blnAllMarkets"></param>
    /// <history>
    /// [aalcocer] 26/May/2016 Created
    /// </history>
    private async void LoadMarkets(bool blnOnlyOneRegister, bool blnMarkets, bool blnAllMarkets)
    {
      if (!blnMarkets)
      {
        pnlMarkets.Visibility = Visibility.Collapsed;
        return;
      }

      grdMarkets.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      _lstMarkets = await BRMarkets.GetMarkets(1);
      grdMarkets.ItemsSource = _lstMarkets;
      chkAllMarkets.IsChecked = blnAllMarkets;
      chkAllMarkets.IsEnabled = !blnOnlyOneRegister;

      if (!_frmIh._clsFilter.LstMarkets.Any()) return;

      chkAllMarkets.IsChecked = false;
      _frmIh._clsFilter.LstMarkets.ForEach(c => grdMarkets.SelectedItems.Add(_lstMarkets.SingleOrDefault(x => x.mkID == c)));
      if (grdMarkets.SelectedItems.Count == grdMarkets.Items.Count)
        chkAllMarkets.IsChecked = true;
    }

    #endregion LoadMarkets

    #endregion Métodos Privados
  }
}