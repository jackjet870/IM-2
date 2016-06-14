using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IM.ProcessorSales.Classes;
using Xceed.Wpf.Toolkit;
using System.Windows.Input;

namespace IM.ProcessorSales.Forms
{
  /// <summary>
  /// Interaction logic for frmFilterDateRange.xaml
  /// </summary>
  public partial class frmFilterDateRange
  {
    #region Atributos
    //BANDERA PARA EVITAR CONFLICTO ENTRE LOS EVENTOS txtSalesman_TextChanged Y cmbSalesman_SelectionChanged 
    private bool _changecmb = true;
    private DateTime _dtStart;
    private DateTime _dtEnd;
    private string _salesRoom = string.Empty;
    private EnumPeriod _period;
    private bool _onePeriod;
    private List<SalesRoomByUser> _lstSalesRoomByUsers;
    private List<Program> _lstPrograms;
    private List<SegmentByAgency> _lstSegmentByAgencies;
    private List<Efficiency> _lstWeeks;
    private List<WeeksHelpper> _lstWeeksHelpp;
    private List<GoalsHelpper> _lstGoals;
    private List<MultiDateHelpper> _lstMultiDate;
    private List<PersonnelShort> _lstPersonnels;

    public frmProcessorSales frmPrs = new frmProcessorSales();
    public bool ok;
    #endregion

    #region Metodos

    #region GetFirstDayOfWeek

    /// <summary>
    /// Obtiene el primer dia de la semana
    /// </summary>
    /// <param name="currentDate">fecha a obtener el primer dia</param>
    /// [Ecanul] 03/05/2016 Created
    private DateTime GetFirstDayOfWeek(DateTime currentDate)
    {
      //Se optiene el dia de la semana de la fecha actual
      DayOfWeek dayOfWeek = currentDate.DayOfWeek;
      //Como el primer día de la semana es el 0 (Lunes), construimos
      //un array para conocer los días que hay que restar de la fecha.
      //Así, si es Lunes (0) restaremos 6 días, si es Domingo(6)
      //restaremos 5 días, y si es Martes (1) restaremos 0 días.
      int[] dias = { 6, 0, 1, 2, 3, 4, 5 };
      //De la fecha actual se restan los dias coorespondientes
      return currentDate.Subtract(new TimeSpan(dias[Convert.ToInt32(dayOfWeek)], 0, 0, 0));
    }

    #endregion

    #region ChangeDates

    /// <summary>
    /// Cambia los DateTimePikers segun el periodo
    /// </summary>
    /// <param name="dt">Fecha a cambiar</param>
    /// [ecanul] 03/05/2016 Created
    private void ChangeDates(DateTime dt)
    {
      if (_onePeriod)
      {
        switch (_period)
        {
          case EnumPeriod.Weekly:
            dt = GetFirstDayOfWeek(dt);
            dtmStart.Value = dt;
            dtmEnd.Value = dt.AddDays(6);
            break;
          case EnumPeriod.Monthly:
            var firstDay = new DateTime(dt.Year, dt.Month, 1);
            dtmStart.Value = firstDay;
            dtmEnd.Value = firstDay.AddMonths(1).AddDays(-1);
            break;
        }
      }
    }

    #endregion

    #region ConfigureDates

    /// <summary>
    /// Configura los controles de fecha
    /// </summary>
    /// <param name="oneDate">Si requiere o no de fechas</param>
    /// <param name="period">Periodo en el que se va a filtrar las fechas</param>
    /// <history>
    /// [ecanul] 26/04/2016 Created
    /// </history>
    private void ConfigureDates(bool oneDate, EnumPeriod period = EnumPeriod.None)
    {
      if (!oneDate) //si no es solo una fecha 
      {
        Dictionary<EnumPredefinedDate, string> dictionaryPredefinedDate = EnumToListHelper.GetList<EnumPredefinedDate>();

        cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.DatesSpecified));

        switch (period)
        {
          //Sin periodo
          case EnumPeriod.None:

            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.Today));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.Yesterday));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisWeek));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousWeek));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisHalf));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousHalf));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisMonth));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousMonth));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisYear));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousYear));
            break;

          //Semanal
          case EnumPeriod.Weekly:
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisWeek));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousWeek));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.TwoWeeksAgo));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThreeWeeksAgo));
            //Title += $" ({EnumToListHelper.GetEnumDescription(enumPeriod)})";
            break;

          //Mensual
          case EnumPeriod.Monthly:
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThisMonth));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.PreviousMonth));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.TwoMonthsAgo));
            cmbDate.Items.Add(dictionaryPredefinedDate.Single(c => c.Key == EnumPredefinedDate.ThreeMonthsAgo));
            //Title += $" ({EnumToListHelper.GetEnumDescription(enumPeriod)})";
            break;
        }
        cmbDate.SelectedIndex = 0;
      }
      else // el formulario no necesita de filtro de fechas
      {
        grdDates.Visibility = Visibility.Collapsed;
        bdrDates.Visibility=Visibility.Collapsed;
      }
    }

    #endregion

    #region LoadEfficiencyWeek

    /// <summary>
    /// Carga el grid de Eficiencias semanales
    /// </summary>
    /// <history>
    /// [ecanul] 02/05/2016 Created
    /// </history>
    private async void LoadEfficiencyWeek()
    {
      _lstWeeks = await BREfficiency.GetEffificencyBySr(_salesRoom, _dtStart, _dtEnd);
      _lstWeeksHelpp = new List<WeeksHelpper>();
      foreach (var week in _lstWeeks)
      {
        WeeksHelpper w = new WeeksHelpper
        {
          dtStart = week.efDateFrom,
          dtEnd = week.efDateTo,
          include = true
        };
        _lstWeeksHelpp.Add(w);
      }
      dtgWeeks.ItemsSource = _lstWeeksHelpp;
    }

    #endregion

    #region ConfigureGrids

    /// <summary>
    /// Configura y llena los grids Segun sea el caso
    /// </summary>
    /// <param name="sr">Sales Rooms</param>
    /// <param name="programs">Programs</param>
    /// <param name="segments">Segments</param>
    /// <param name="multidate">Multidate</param>
    /// <param name="concentrate">Concentrate</param>
    /// <param name="weeks">Weeks</param>
    /// <history>
    /// [ecanul] 26/04/2016 Created
    /// [edgrodriguez] 21/05/2016 Modified. El metodo GetPrograms se volvió asincrónico.
    /// </history>
    private async void ConfigureGrids(bool sr, bool programs, bool segments, bool multidate, bool concentrate, bool weeks,
      bool onlyOneRegister, bool allPrograms = false, bool allSegments = false)
    {
      #region Visibilidad de los grids
      //Visibilidad de los grids
      pnlSalesRoom.Visibility = (sr) ? Visibility.Visible : Visibility.Collapsed;
      pnlProggrams.Visibility = (programs) ? Visibility.Visible : Visibility.Collapsed;
      pnlSegments.Visibility = (segments) ? Visibility.Visible : Visibility.Collapsed;
      pnlSalesRoomMultiDateRange.Visibility = (multidate) ? Visibility.Visible : Visibility.Collapsed;
      pnlSalesRoomConcentrate.Visibility = (concentrate) ? Visibility.Visible : Visibility.Collapsed;
      pnlWeeks.Visibility = (weeks) ? Visibility.Visible : Visibility.Collapsed;

      #endregion
      
      //Carga salesRoom
      _lstSalesRoomByUsers = await BRSalesRooms.GetSalesRoomsByUser(App.User.User.peID);
     
      //Si tiene SR
      dtgSalesRoom.ItemsSource = (sr) ? _lstSalesRoomByUsers : null;

      #region Programs
      //Si tiene Programs
      if (programs)
      {
        _lstPrograms = await BRPrograms.GetPrograms();
        dtgPrograms.ItemsSource = _lstPrograms;
      }

      #endregion

      #region Segments
      //Si tiene Segments
      if (segments)
      {
        _lstSegmentByAgencies = await BRSegmentsByAgency.GetSegMentsByAgency();
        dtgSegments.ItemsSource = _lstSegmentByAgencies;
      }
      #endregion
      
      #region concentrate
      //Si es concentrate
      if (concentrate)
      {
        _lstGoals = new List<GoalsHelpper>();
        foreach (var item in _lstSalesRoomByUsers)
        {
          GoalsHelpper goal = new GoalsHelpper
          {
            isCheck = false,
            salesRoom = item,
            goal = 0
          };
          _lstGoals.Add(goal);
        }

        dtgSalesRoomConcentrate.ItemsSource = _lstGoals;
      }

      #endregion

      #region multidate
      //si es multidate
      if (multidate)
      {
        _lstMultiDate = new List<MultiDateHelpper>();
        MultiDateHelpper mdh = new MultiDateHelpper
        {
          dtStart = _dtStart,
          dtEnd = _dtEnd
        };
        _lstMultiDate.Add(mdh);
        cmbSalesRoom.ItemsSource = _lstSalesRoomByUsers;
        dtgSalesRoomMultiDateRange.ItemsSource = _lstMultiDate;
        chkAllSalesRoomMultiDateRange.Visibility = Visibility.Collapsed;
      }

      #endregion

      #region weeks
      //Si tiene weeks
      if (weeks)
        LoadEfficiencyWeek();
      #endregion

      //Status Bar
      statusBarNumSalesRoom.Content = (sr) ? $"{0}/{_lstSalesRoomByUsers.Count} Sales Rooms selected" : string.Empty;
      statusBarNumPg.Content = (programs) ? $"{0}/{_lstPrograms.Count} Programs selected" : string.Empty;
      statusBarNumSegments.Content = (segments) ? $"{0}/{_lstSegmentByAgencies.Count} Segments selected" : string.Empty;
      //Toma la configuracion del padre
      LoadUserFilters();
      //configura el check all de los grids
      ConfigureSelection(onlyOneRegister, allPrograms, allSegments);
    }

    #endregion

    #region ConfigureSelection

    /// <summary>
    /// Configura el modo de seleccion de los grids(Multiseleccion ó Solo un registro). SOLO FUNCIONA PARA SALESROOM
    /// Activa o desactiva los controles checkbox dependiendo el modo de seleccion configurado
    /// </summary>
    /// <param name="onlyOneRegister">solo un registro</param>
    /// <param name="allPrograms">si lleva todos los programas </param>
    /// <param name="allSegments">si leva todos los segmentos</param>
    /// <history>
    /// [ecanul] 27/04/2016 Created
    /// </history>
    private void ConfigureSelection(bool onlyOneRegister, bool allPrograms = false, bool allSegments = false)
    {
      dtgSalesRoom.SelectionMode = onlyOneRegister ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;
      chkAllSalesRoom.IsEnabled = !onlyOneRegister;

      if (allPrograms)
        chkAllPrograms.IsChecked = true;
      if (allSegments)
        chkAllSegments.IsChecked = true;
    }

    #endregion

    private async void LoadSalesman()
    {
      _lstPersonnels = await BRPersonnel.GetPersonnel("All", "All", "PR,LINER,CLOSER,EXIT", 1, "All", "=",
         EnumPermisionLevel.None, "All");
      cmbSalesman.ItemsSource = _lstPersonnels;
    }

    #region ConfigureFilters

    /// <summary>
    /// Configura la visibilidad de los filtros no relacionados con las fechas
    /// </summary>
    /// <param name="basedOnArrival">check arrival</param>
    /// <param name="quinellas">check quiniellas</param>
    /// <param name="shGroup">mostrar groups</param>
    /// <param name="group">check groups</param>
    /// <param name="shAllSalesmen">mostrar all salesman</param>
    /// <param name="allSalesmen">check all salesman</param>
    /// <param name="isGoal">si es goal</param>
    /// <param name="goal">goal</param>
    /// <param name="isSalesman">si el filtro es para un reporte de salesman</param>
    /// <param name="shRoles">si se quiere mostrar los roles</param>
    /// <param name="pr">chkpr</param>
    /// <param name="liner">chkliner</param>
    /// <param name="closer">chkcloser</param>
    /// <param name="exit">chkLiner</param>
    /// <history>
    /// [ecanul] 27/04/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private void ConfigureFilters(EnumBasedOnArrival? basedOnArrival, EnumQuinellas? quinellas, bool shGroup, bool group, bool shAllSalesmen, bool allSalesmen, bool isGoal, decimal? goal,
      //seccion salesman
      bool isSalesman, bool shRoles, bool pr, bool liner, bool closer, bool exit)
    {
      //check based on arrival date
      if (basedOnArrival != null)
        chkBasedOnArrival.IsChecked = Convert.ToBoolean(basedOnArrival);
      else
        chkBasedOnArrival.Visibility = Visibility.Collapsed;
      //check consider quiniellas
      if (quinellas != null)
        chkQuinellas.IsChecked = Convert.ToBoolean(quinellas);
      else
        chkQuinellas.Visibility = Visibility.Collapsed;
      //check group by teams
      if (shGroup)
        chkGroupedByTeams.IsChecked = group;
      else
        chkGroupedByTeams.Visibility = Visibility.Collapsed;

      if (isGoal)
        txtGoal.Text = goal.ToString();
      else
        grdGoal.Visibility = Visibility.Collapsed;
      //check include all salesman
      if (shAllSalesmen)
        chkIncludeAllSalesmen.IsChecked = allSalesmen;
      else
        chkIncludeAllSalesmen.Visibility = Visibility.Collapsed;
      //salesman filter section
      if (isSalesman)
      {
        //cmb
        LoadSalesman();
        //roles
        if (shRoles)
        {
          chkPr.IsChecked = pr;
          chkLiner.IsChecked = liner;
          chkCloser.IsChecked = closer;
          chkExit.IsChecked = exit;
        }
        else
          gpbRoles.Visibility = Visibility.Collapsed;
      }
      else
        grdSalesman.Visibility = Visibility.Collapsed;

    }

    #endregion

    #region LoadUserFilters

    /// <summary>
    /// Obtiene los filtros que el usuario habiá seleccionado y los aplica al formulario
    /// </summary>
    /// <history>
    /// [ecanul] 27/04/2016 Created
    /// </history>
    private void LoadUserFilters()
    {
      if (pnlSalesRoom.Visibility == Visibility.Visible && !string.IsNullOrEmpty(_salesRoom))
      {
        dtgSalesRoom.SelectedItem = (from sr in _lstSalesRoomByUsers where sr.srID == _salesRoom select sr).FirstOrDefault();
      }

      if (grdGoal.Visibility == Visibility.Visible && frmPrs.goal != 0)
        txtGoal.Text = frmPrs.goal.ToString();

      cmbDate.SelectedValue =
        cmbDate.Items.Cast<KeyValuePair<EnumPredefinedDate, string>>().Any(c => c.Key == frmPrs.predefinedDate)
          ? frmPrs.predefinedDate
          : EnumPredefinedDate.DatesSpecified;
      if (grdDates.Visibility == Visibility.Visible)
      {
        dtmStart.Value = frmPrs.dtmStart;
        dtmEnd.Value = frmPrs.dtmEnd;
      }
      chkBasedOnArrival.IsChecked = Convert.ToBoolean(frmPrs.basedOnArrival);
      chkQuinellas.IsChecked = Convert.ToBoolean(frmPrs.quinellas);

    }

    #endregion

    #region ValidateFields

    /// <summary>
    /// Valida que el grid Tenga al menos un elemento seleccionado
    /// </summary>
    /// <history>
    /// [ecanul] 28/04/2016 Created
    /// </history>
    private string ValidateFields()
    {
      if (pnlSalesRoom.Visibility == Visibility.Visible && dtgSalesRoom.SelectedItems.Count == 0)
        return "No Sales Room is selected";
      if (pnlProggrams.Visibility == Visibility.Visible && dtgPrograms.SelectedItems.Count == 0)
        return "No programs is selected";
      if (pnlSegments.Visibility == Visibility.Visible && dtgSegments.SelectedItems.Count == 0)
        return "No Segments is selected";
      if (pnlSalesRoomConcentrate.Visibility == Visibility.Visible && _lstGoals.Count(x => x.isCheck) == 0)
        return "No sales room is selected";
      if (pnlSalesRoomMultiDateRange.Visibility == Visibility.Visible && _lstMultiDate.Count(x => x.salesRoom != "") == 0)
        return "Please specify one sales room";
      if (pnlSalesRoomMultiDateRange.Visibility == Visibility.Visible && _lstMultiDate.Count(x => x.isMain) == 0)
        return "Please specify the main sales room";
      //validar salesman
      

      return string.Empty;
    }

    #endregion

    #region SaveFilterValues

    /// <summary>
    /// Guarda los datos seleccionados por el usuario
    /// </summary>
    /// <history>
    /// [ecanul] 28/04/2016 Created
    /// </history>
    private void SaveFilterValues()
    {
      if (pnlSalesRoom.IsVisible)
        if (chkAllSalesRoom.IsChecked.Value)
        {
          frmPrs.lstSalesRoom.Clear();
          frmPrs.lstSalesRoom.Add("All");
        }
        else
          frmPrs.lstSalesRoom = dtgSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(x => x.srID).ToList();

      if (pnlProggrams.IsVisible)
        frmPrs.lstPrograms = dtgPrograms.SelectedItems.Cast<Program>().Select(x => x.pgID).ToList();

      if (pnlSegments.IsVisible)
        frmPrs.lstSegments = dtgSegments.SelectedItems.Cast<SegmentByAgency>().Select(x => x.seID).ToList();
      
      if (pnlSalesRoomConcentrate.IsVisible)
        frmPrs.lstGoals = _lstGoals.Where(g => g.isCheck).ToList();
      if (pnlSalesRoomMultiDateRange.Visibility == Visibility.Visible)
        frmPrs.lstMultiDate = _lstMultiDate;
      
      //Mandar los datos de fechas
      if (grdDates.IsVisible)
      {
        frmPrs.predefinedDate = ((KeyValuePair<EnumPredefinedDate, string>)cmbDate.SelectedItem).Key;
        frmPrs.dtmStart = (DateTime)dtmStart.Value;
        frmPrs.dtmEnd = (DateTime)dtmEnd.Value;
      }
      
      //si goal esta visible
      if (grdGoal.Visibility == Visibility.Visible)
        frmPrs.goal = Convert.ToDecimal(txtGoal.Text.Trim());

      //Region de checks
      frmPrs.basedOnArrival = (chkBasedOnArrival.IsChecked.Value) ? EnumBasedOnArrival.BasedOnArrival : EnumBasedOnArrival.NoBasedOnArrival;
      frmPrs.quinellas = (chkQuinellas.IsChecked.Value) ? EnumQuinellas.Quinellas : EnumQuinellas.NoQuinellas;
      frmPrs.groupedByTeams = chkGroupedByTeams.IsChecked.Value;
      frmPrs.includeAllSalesmen = chkIncludeAllSalesmen.IsChecked.Value;

      //si es de salesman 
      if(grdSalesman.Visibility==Visibility.Visible)
      {
        PersonnelShort item =(PersonnelShort) cmbSalesman.SelectedItem;
        frmPrs.lstPersonnel.Add(item);
        //si tiene los roles visibles
        if (gpbRoles.IsVisible)
        {
          frmPrs.pr = chkPr.IsChecked.Value;
          frmPrs.liner = chkLiner.IsChecked.Value;
          frmPrs.closer = chkCloser.IsChecked.Value;
          frmPrs.exit = chkExit.IsChecked.Value;
        }
      }
    }

    #endregion

    #region Metodos Publicos

    #region ConfigForm

    public void ConfigForm(DateTime dtStart, DateTime dtEnd, //Necesarios siempre
      string salesRoom = null, string segments = null, string salesman = null, decimal? goal = null, //Necesarios segun el tipo de reporte
      //Checks arriba de los grids y configuracion del friltro
      bool allSalesRoom = false, bool allProgams = false, bool allSegments = false, bool oneDate = false, bool onlyOneRegister = false, bool groupByTeams = false,
      bool allSalesmen = false, bool shRoles = false, bool pr = false, bool liner = false, bool closer = false, bool exit = false, bool onePeriod = false,
      //Enumerados, 
      EnumPeriod period = EnumPeriod.None, EnumBasedOnArrival? basedOnArrival = null, EnumQuinellas? quinellas = null,
      //para saber que mostrar Grids 
      bool shSr = true, bool shPrograms = false, bool shSegments = false, bool shMultiDateRanges = false, bool shConcentrate = false, bool shWeeks = false,
      //para saber que mostrar Filtros
      bool shGroupsByTeams = false, bool shAllSalesmen = false, bool isBySalesman = false, bool isGoal = false)
    {
      //Datos siempre usados
      _dtStart = dtStart;
      _dtEnd = dtEnd;
      _salesRoom = salesRoom;
      _period = period;
      _onePeriod = onePeriod;
      //Configura Fechas
      ConfigureDates(oneDate, period);
      //configura la visibilidad de los filtros 
      ConfigureFilters(basedOnArrival, quinellas, shGroupsByTeams, groupByTeams, shAllSalesmen, allSalesmen, isGoal,
        goal, isBySalesman, shRoles, pr, liner, closer, exit);
      //configura Los grids y los carga
      ConfigureGrids(shSr, shPrograms, shSegments, shMultiDateRanges, shConcentrate, shWeeks, onlyOneRegister, allProgams, allSegments);
      
    }

    #endregion ConfigForm

    #endregion Metodos Publicos

    #endregion Metodos

    #region Constructores y destructores

    public frmFilterDateRange()
    {
      InitializeComponent();
      PreviewKeyDown += Close_KeyPreviewESC;
    }
    #endregion

    #region Eventos del Formulario

    #region Botones
    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      ok = false;
      Close();
    }
    #endregion

    #region btnOk_Click
    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      string message = ValidateFields();
      if (message == "" || message == string.Empty)
      {
        ok = true;
        SaveFilterValues();
        Close();
      }
      else
        UIHelper.ShowMessage(message);
    }
    #endregion

    #endregion

    #region Cheks
    #region chbx_Checked

    /// <summary>
    /// Funcion Check Uncheck de todos los checkAll
    /// </summary>
    /// <history>
    /// [ecanul] 29/04/2016 Created
    /// </history>
    private void chbx_Checked(object sender, RoutedEventArgs e)
    {
      CheckBox chk = (CheckBox)sender;
      bool isCheck = chk.IsChecked.HasValue && chk.IsChecked.Value;
      DataGrid dtg = null;
      if (chk.Equals(chkAllSalesRoom))
        dtg = dtgSalesRoom;
      else if (chk.Equals(chkAllPrograms))
        dtg = dtgPrograms;
      else if (chk.Equals(chkAllSegments))
        dtg = dtgSegments;
      else if (chk.Equals(chkAllSalesRoomConcentrate))
      {
        dtg = dtgSalesRoomConcentrate;
        foreach (GoalsHelpper item in dtg.ItemsSource)
        {
          item.isCheck = chkAllSalesRoomConcentrate.IsChecked.Value;
        }
      }
      if (dtg == null) return;
      if (isCheck)
        dtg.SelectAll();
      else
        dtg.UnselectAll();
      dtg.IsEnabled = !isCheck;
    }

    #endregion

    #region chkSalesRoom_Checked
    private void chkSalesRoom_Checked(object sender, RoutedEventArgs e)
    {
      int count = _lstGoals.Count(item => item.isCheck);
      statusBarNumSalesRoomConcentrate.Content = $"{count}/{dtgSalesRoomConcentrate.Items.Count} Sales Rooms selected";
    }
    #endregion

    #endregion

    #region Grids

    #region dtgSalesRoom_SelectionChanged
    private void dtgSalesRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      statusBarNumSalesRoom.Content = $"{dtgSalesRoom.SelectedItems.Count}/{dtgSalesRoom.Items.Count} Sales Rooms selected";
    }
    #endregion

    #region dtgPrograms_SelectionChanged
    private void dtgPrograms_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      statusBarNumPg.Content = $"{dtgPrograms.SelectedItems.Count}/{dtgPrograms.Items.Count} Programs selected";
    }
    #endregion

    #region dtgSegments_SelectionChanged
    private void dtgSegments_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      statusBarNumSegments.Content = $"{dtgSegments.SelectedItems.Count}/{dtgSegments.Items.Count} Segments selected";
    }
    #endregion

    #endregion

    #region TextBox
    #region txtSalesman_TextChanged
    private void txtSalesman_TextChanged(object sender, TextChangedEventArgs e)
    {
      cmbSalesman.SelectedValue = txtSalesman.Text;
      _changecmb = false;
    }
    #endregion

    #region txtGoal_PreviewTextInput
    private void txtGoal_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      if (!char.IsDigit(e.Text, e.Text.Length - 1))
        e.Handled = true;
    }
    #endregion 

    #endregion

    #region cmbSalesman_SelectionChanged
    private void cmbSalesman_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (_changecmb)
        txtSalesman.Text = cmbSalesman.SelectedValue.ToString();
      else
        _changecmb = true;
    }
    #endregion
    
    #region cmbSalesman_PreviewMouseDown

    /// <summary>
    /// Al dar click al combo la bandera siempre sera positiva
    /// </summary>
    /// <history>
    /// [ecanul] 03/05/2016 Created
    /// </history>
    private void cmbSalesman_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      _changecmb = true;
    }


    #endregion

    #region cmbDate_SelectionChanged
    private void cmbDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      EnumPredefinedDate select = (EnumPredefinedDate)((ComboBox)e.OriginalSource).SelectedValue;
      DateTime today = BRHelpers.GetServerDate();
      grdDtmStart.IsEnabled = false;
      grdDtgEnd.IsEnabled = false;

      switch (select)
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
          grdDtmStart.IsEnabled = grdDtgEnd.IsEnabled = true;
          break;
      }
    }
    #endregion

    #region dateTime_ValueChanged
    private void dateTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var picker = sender as DateTimePicker;
      if (picker == null) return;
      DateTime date = (DateTime)picker.Value;
      ChangeDates(date);
    }
    #endregion

    #region Close_KeyPreviewESC
    private void Close_KeyPreviewESC(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        Close();
    }
    #endregion

    #endregion
  }
}
