using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
    private string _salesRoom = string.Empty;
    private EnumPeriod _period;
    private bool _onePeriod, _shWeeks;
    private readonly List<SalesRoomByUser> _lstSalesRoomByUsers = new List<SalesRoomByUser>();
    private List<Program> _lstPrograms;
    private List<SegmentByAgency> _lstSegmentByAgencies;
    private List<EfficiencyData> _lstEfficiencyWeeks;
    private readonly ObservableCollection<GoalsHelpper> _lstGoals = new ObservableCollection<GoalsHelpper>();
    private readonly ObservableCollection<MultiDateHelpper> _lstMultiDate = new ObservableCollection<MultiDateHelpper>();
    private readonly List<PersonnelShort> _lstPersonnels = new List<PersonnelShort>();

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
    /// <param name="multiDate">Si requiere o no de fechas</param>
    /// <param name="period">Periodo en el que se va a filtrar las fechas</param>
    /// <history>
    /// [ecanul] 26/04/2016 Created
    /// </history>
    private void ConfigureDates(bool multiDate, EnumPeriod period = EnumPeriod.None)
    {
      if (!multiDate) //si no es solo una fecha 
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
        bdrDates.Visibility = Visibility.Collapsed;
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
      if (!_shWeeks)
      {
        pnlWeeks.Visibility = Visibility.Collapsed;
        return;
      }
      _lstEfficiencyWeeks = await BREfficiency.GetEfficiencyByWeeks(_salesRoom, dtmStart.Value.Value.Date, dtmEnd.Value.Value.Date);
      
      dtgWeeks.ItemsSource = _lstEfficiencyWeeks;

      lblDate.Content = $"Weeks of {dtmStart.Value.Value.ToString("MMMM")} of {dtmStart.Value.Value.Year}";
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

    #region LoadSalesman
    /// <summary>
    ///Carga y configuracion del combo de Salesman
    /// </summary>
    /// <history>
    /// [ecanul] 14/06/2016 Created
    /// </history>
    private async void LoadSalesman()
    {
      _lstPersonnels.AddRange(await BRPersonnel.GetPersonnel("All", "All", new List<EnumRole> {
        EnumRole.PR,
        EnumRole.Liner,
        EnumRole.Closer,
        EnumRole.ExitCloser
      }.EnumListToString(), 1, "All", "=", EnumPermisionLevel.None, "All"));
      int index = _lstPersonnels.FindIndex(x => x.peID.Equals(frmPrs._clsFilter.Salesman?.peID));
      if (index != -1)
      {
        cmbSalesman.SelectedIndex = index;
      }
    }
    #endregion LoadSalesman

    #region ConfigureFilters

    /// <summary>
    /// Configura la visibilidad de los filtros no relacionados con las fechas
    /// </summary>
    /// <param name="shGroup">mostrar groups</param>
    /// <param name="group">check groups</param>
    /// <param name="shAllSalesmen">mostrar all salesman</param>
    /// <param name="allSalesmen">check all salesman</param>
    /// <param name="isGoal">si es goal</param>
    /// <param name="isSalesman">si el filtro es para un reporte de salesman</param>
    /// <param name="shRoles">si se quiere mostrar los roles</param>
    /// <history>
    /// [ecanul] 27/04/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private void ConfigureFilters(EnumBasedOnArrival? basedOnArrival, EnumQuinellas? quinellas, bool shGroup, bool group, bool shAllSalesmen, bool allSalesmen, bool isGoal,
      //seccion salesman
      bool isSalesman, bool shRoles)
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

      if (!isGoal)
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
        if (!shRoles)
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
      if (grdGoal.Visibility == Visibility.Visible && frmPrs._clsFilter.Goal != 0)
        txtGoal.Text = $"{frmPrs._clsFilter.Goal}";

      cmbDate.SelectedValue =
        cmbDate.Items.Cast<KeyValuePair<EnumPredefinedDate, string>>().Any(c => c.Key == frmPrs._clsFilter.CboDateSelected)
          ? frmPrs._clsFilter.CboDateSelected
          : EnumPredefinedDate.DatesSpecified;
      if (grdDates.Visibility == Visibility.Visible)
      {
        dtmStart.Value = frmPrs._clsFilter.DtmStart;
        dtmEnd.Value = frmPrs._clsFilter.DtmEnd;
      }
      chkBasedOnArrival.IsChecked = Convert.ToBoolean(frmPrs.basedOnArrival);
      chkQuinellas.IsChecked = Convert.ToBoolean(frmPrs.quinellas);

      if (gpbRoles.Visibility == Visibility.Visible)
      {
        chkPr.IsChecked = frmPrs._clsFilter.LstEnumRole.Contains(EnumRole.PR);
        chkLiner.IsChecked = frmPrs._clsFilter.LstEnumRole.Contains(EnumRole.Liner);
        chkCloser.IsChecked = frmPrs._clsFilter.LstEnumRole.Contains(EnumRole.Closer);
        chkExit.IsChecked = frmPrs._clsFilter.LstEnumRole.Contains(EnumRole.ExitCloser);
      }

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
      if (pnlPrograms.Visibility == Visibility.Visible && dtgPrograms.SelectedItems.Count == 0)
        return "No programs is selected";
      if (pnlSegments.Visibility == Visibility.Visible && dtgSegments.SelectedItems.Count == 0)
        return "No Segments is selected";
      if (pnlSalesRoomConcentrate.Visibility == Visibility.Visible && !_lstGoals.Any(x => x.IsCheck))
        return "No sales room is selected";
      if (pnlSalesRoomMultiDateRange.Visibility == Visibility.Visible && _lstMultiDate.All(x => string.IsNullOrWhiteSpace(x.SalesRoom)))
        return "Please specify one sales room";
      if (pnlSalesRoomMultiDateRange.Visibility == Visibility.Visible && !_lstMultiDate.Any(x => x.IsMain))
        return "Please specify the main sales room";
      if (pnlSalesRoomMultiDateRange.Visibility == Visibility.Visible && _lstMultiDate.Count(x => x.IsMain) > 1)
        return "Please specify only one main sales room";
      if (grdSalesman.Visibility == Visibility.Visible && cmbSalesman.SelectedItem == null)
        return "Please specify the salesman";


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
      #region SalesRooms
      if (pnlSalesRoom.IsVisible)
        if (chkAllSalesRoom.IsChecked.Value)
        {
          frmPrs._clsFilter.LstSalesRooms.Clear();
          frmPrs._clsFilter.LstSalesRooms.Add("All");
        }
        else
          frmPrs._clsFilter.LstSalesRooms = dtgSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(x => x.srID).ToList();

      #endregion

      #region programs
      if (pnlPrograms.IsVisible)
        frmPrs._clsFilter.EnumProgram = (dtgPrograms.Items.Count == dtgPrograms.SelectedItems.Count || dtgPrograms.SelectedItems.Count == 0)
          ? EnumProgram.All : EnumToListHelper.GetList<EnumProgram>().SingleOrDefault(x => x.Value == ((Program)dtgPrograms.SelectedItem).pgID).Key;
      #endregion

      #region segments
      if (pnlSegments.IsVisible)
      {
        frmPrs._clsFilter.LstSegments = dtgSegments.SelectedItems.Cast<SegmentByAgency>().Select(c => c.seID).ToList();
        frmPrs._clsFilter.BlnAllSegments = dtgSegments.Items.Count == dtgSegments.SelectedItems.Count;
      }
      #endregion

      #region Concentrate
      if (pnlSalesRoomConcentrate.IsVisible)
        frmPrs._clsFilter.LstGoals = _lstGoals.Where(g => g.IsCheck).ToList();
      #endregion
      #region Multidate
      if (pnlSalesRoomMultiDateRange.Visibility == Visibility.Visible)
        frmPrs._clsFilter.LstMultiDate = _lstMultiDate.Where(x => !string.IsNullOrWhiteSpace(x.SalesRoom)).OrderByDescending(x => x.IsMain).ToList();
      #endregion

      #region Dates
      //Mandar los datos de fechas
      if (grdDates.IsVisible)
      {
        frmPrs._clsFilter.CboDateSelected = ((KeyValuePair<EnumPredefinedDate, string>)cmbDate.SelectedItem).Key;
        frmPrs._clsFilter.DtmStart = Convert.ToDateTime(dtmStart.Value);
        frmPrs._clsFilter.DtmEnd = Convert.ToDateTime(dtmEnd.Value);
      }
      #endregion

      #region Goal
      //si goal esta visible
      if (grdGoal.Visibility == Visibility.Visible)
        frmPrs._clsFilter.Goal = Convert.ToDecimal(txtGoal.Text.Trim());
      #endregion

      #region Checks
      //Region de checks
      frmPrs._clsFilter.BlnGroupedByTeams = Convert.ToBoolean(chkGroupedByTeams.IsChecked);
      frmPrs._clsFilter.BlnIncludeAllSalesmen = Convert.ToBoolean(chkIncludeAllSalesmen.IsChecked);
      #endregion

      #region Salesman Filters
      //si es de salesman 
      if (grdSalesman.Visibility == Visibility.Visible)
      {
        frmPrs._clsFilter.Salesman = (PersonnelShort)cmbSalesman.SelectedItem;
        //si tiene los roles visibles
        if (gpbRoles.IsVisible)
        {
          frmPrs._clsFilter.LstEnumRole.Clear();
          if (Convert.ToBoolean(chkPr.IsChecked) && Convert.ToBoolean(chkLiner.IsChecked) && Convert.ToBoolean(chkCloser.IsChecked) && Convert.ToBoolean(chkExit.IsChecked))
            frmPrs._clsFilter.LstEnumRole.Add(EnumRole.All);
          else
          {
            if (Convert.ToBoolean(chkPr.IsChecked))
              frmPrs._clsFilter.LstEnumRole.Add(EnumRole.PR);
            if (Convert.ToBoolean(chkLiner.IsChecked))
              frmPrs._clsFilter.LstEnumRole.Add(EnumRole.Liner);
            if (Convert.ToBoolean(chkCloser.IsChecked))
              frmPrs._clsFilter.LstEnumRole.Add(EnumRole.Closer);
            if (Convert.ToBoolean(chkExit.IsChecked))
              frmPrs._clsFilter.LstEnumRole.Add(EnumRole.ExitCloser);
          }
        }
      }
      #endregion

      #region EfficiencyWeekly
      if (pnlWeeks.IsVisible)
      {
        frmPrs._clsFilter.LstSalesRooms = dtgSalesRoom.SelectedItems.Cast<SalesRoomByUser>().Select(x => x.srID).ToList();
        var effDates = new EfficiencyData()
        {
          efDateFrom = dtmStart.Value.Value,
          efDateTo = dtmEnd.Value.Value,
          Include = true
          
        };
        frmPrs._clsFilter.lstEfficiency.Add(effDates);
        frmPrs._clsFilter.lstEfficiency.AddRange(_lstEfficiencyWeeks.Where(g => g.Include == true).ToList());
      }
      #endregion
    }

    #endregion

    #region LoadSalesRooms

    /// <summary>
    /// Carga y configuracion del grid Salesrooms
    /// </summary>
    /// <param name="blnOnlyOneRegister"></param>
    /// <param name="blnSalesRoom"></param>
    /// <param name="blnAllSalesRoom"></param>
    /// <param name="multiDate"></param>
    /// <param name="concentrate"></param>
    /// <history>
    /// [aalcocer] 09/07/2016 Created
    /// </history>
    private async void LoadSalesRooms(bool blnOnlyOneRegister, bool blnSalesRoom, bool blnAllSalesRoom, bool multiDate, bool concentrate)
    {
      pnlSalesRoom.Visibility = blnSalesRoom ? Visibility.Visible : Visibility.Collapsed;
      pnlSalesRoomMultiDateRange.Visibility = multiDate ? Visibility.Visible : Visibility.Collapsed;
      pnlSalesRoomConcentrate.Visibility = concentrate ? Visibility.Visible : Visibility.Collapsed;

      if (!blnSalesRoom && !multiDate && !concentrate)
        return;

      _lstSalesRoomByUsers.AddRange(await BRSalesRooms.GetSalesRoomsByUser(App.User.User.peID));

      #region multidate
      //si es multidate
      if (multiDate)
      {

        MultiDateHelpper mdh = new MultiDateHelpper
        {
          SalesRoom = frmPrs._clsFilter.LstSalesRooms.FirstOrDefault(),
          DtStart = frmPrs._clsFilter.DtmStart,
          DtEnd = frmPrs._clsFilter.DtmEnd,
          IsMain = true
        };
        _lstMultiDate.Add(mdh);
        dtgSalesRoomMultiDateRange.Items.Refresh();
        chkAllSalesRoomMultiDateRange.Visibility = Visibility.Collapsed;
        return;
      }

      #endregion

      #region concentrate
      //Si es concentrate
      if (concentrate)
      {
        foreach (var item in _lstSalesRoomByUsers)
        {
          GoalsHelpper goal = new GoalsHelpper
          {
            IsCheck = false,
            SalesRoomByUser = item,
            Goals = 0
          };
          _lstGoals.Add(goal);
        }
        return;
      }

      #endregion

      dtgSalesRoom.SelectionMode = (blnOnlyOneRegister) ? DataGridSelectionMode.Single : DataGridSelectionMode.Extended;

      dtgSalesRoom.ItemsSource = _lstSalesRoomByUsers;

      chkAllSalesRoom.IsChecked = blnAllSalesRoom;
      chkAllSalesRoom.IsEnabled = !blnOnlyOneRegister;

      if (!frmPrs._clsFilter.LstSalesRooms.Any()) return;

      chkAllSalesRoom.IsChecked = (dtgSalesRoom.SelectionMode == DataGridSelectionMode.Extended) && frmPrs._clsFilter.BlnAllSalesRooms;
      if (dtgSalesRoom.ItemsSource != null && !frmPrs._clsFilter.BlnAllSalesRooms && !blnOnlyOneRegister)
      {
        dtgSalesRoom.SelectedItem = null;
        frmPrs._clsFilter.LstSalesRooms.ForEach(c =>
        {
          dtgSalesRoom.SelectedItems.Add(_lstSalesRoomByUsers.FirstOrDefault(s => s.srID == c));
        });
      }
      else
        dtgSalesRoom.SelectedItem = _lstSalesRoomByUsers.FirstOrDefault(c => c.srID == frmPrs._clsFilter.LstSalesRooms.First());
    }
    #endregion

    #region LoadPrograms
    /// <summary>
    /// Carga y configuracion del grid Programs
    /// </summary>    
    /// <param name="blnPrograms"></param>
    /// <history>
    /// [aalcocer] 09/07/2016 Created
    /// </history>
    private async void LoadPrograms(bool blnPrograms)
    {
      if (!blnPrograms)
      {
        pnlPrograms.Visibility = Visibility.Collapsed;
        return;
      }

      _lstPrograms = await BRPrograms.GetPrograms();
      dtgPrograms.ItemsSource = _lstPrograms;

      if (frmPrs._clsFilter.EnumProgram == EnumProgram.All)
        chkAllPrograms.IsChecked = true;
      else
        dtgPrograms.SelectedItem = _lstPrograms.FirstOrDefault(c => c.pgID == EnumToListHelper.GetEnumDescription(frmPrs._clsFilter.EnumProgram));
    }
    #endregion

    #region LoadSegments
    /// <summary>
    /// Carga y configuracion del grid Segments
    /// </summary>    
    /// <param name="blnSegments"></param>
    /// <param name="blnAllSegments"></param>
    /// <history>
    /// [aalcocer] 09/07/2016 Created
    /// </history>
    private async void LoadSegments(bool blnSegments, bool blnAllSegments)
    {
      if (!blnSegments)
      {
        pnlSegments.Visibility = Visibility.Collapsed;
        return;
      }

      _lstSegmentByAgencies = await BRSegmentsByAgency.GetSegMentsByAgency();
      dtgSegments.ItemsSource = _lstSegmentByAgencies;

      chkAllSegments.IsChecked = blnAllSegments;

      if (!frmPrs._clsFilter.LstSegments.Any()) return;

      chkAllSegments.IsChecked = frmPrs._clsFilter.BlnAllSegments;
      if (dtgSegments.ItemsSource != null && !frmPrs._clsFilter.BlnAllSegments)
      {
        dtgSegments.SelectedItem = null;
        frmPrs._clsFilter.LstSegments.ForEach(c =>
        {
          dtgSegments.SelectedItems.Add(_lstSegmentByAgencies.FirstOrDefault(p => p.seID == c));
        });
      }
      else
        dtgSegments.SelectedItem = _lstSegmentByAgencies.FirstOrDefault(c => c.seID == frmPrs._clsFilter.LstSegments.First());
    }
    #endregion

    #region Metodos Publicos

    #region ConfigForm

    public void ConfigurarFomulario(
       string salesRoom = null,  //Necesarios segun el tipo de reporte
                                 //Checks arriba de los grids y configuracion del friltro
      bool blnAllSalesRoom = false, bool blnAllSegments = false, bool multiDate = false, bool blnOnlyOneRegister = false, bool groupByTeams = false,
      bool allSalesmen = false, bool shRoles = false, bool onePeriod = false,
      //Enumerados, 
      EnumPeriod period = EnumPeriod.None, EnumBasedOnArrival? basedOnArrival = null, EnumQuinellas? quinellas = null,
      //para saber que mostrar Grids 
      bool blnSalesRoom = true, bool blnPrograms = false, bool blnSegments = false, bool shMultiDateRanges = false, bool shConcentrate = false, bool shWeeks = false,
      //para saber que mostrar Filtros
      bool shGroupsByTeams = false, bool shAllSalesmen = false, bool isBySalesman = false, bool isGoal = false)
    {
      //Datos siempre usados
      _salesRoom = salesRoom;
      _period = period;
      _onePeriod = onePeriod;
      _shWeeks = shWeeks;
      //Configura Fechas
      ConfigureDates(multiDate, period);
      //configura la visibilidad de los filtros 
      ConfigureFilters(basedOnArrival, quinellas, shGroupsByTeams, groupByTeams, shAllSalesmen, allSalesmen, isGoal, isBySalesman, shRoles);

      #region Configuracion de Grids.
      LoadSalesRooms(blnOnlyOneRegister, blnSalesRoom, blnAllSalesRoom, shMultiDateRanges, shConcentrate);
      LoadPrograms(blnPrograms);
      LoadSegments(blnSegments, blnAllSegments);
      LoadEfficiencyWeek();
      #endregion Configuracion de Grids.

      //Toma la configuracion del padre
      LoadUserFilters();
      //configura el check all de los grids
      // ConfigureSelection(onlyOneRegister, allPrograms, allSegments);

    }

    #endregion ConfigForm

    #endregion Metodos Publicos

    #endregion Metodos

    #region Constructores y destructores

    public frmFilterDateRange()
    {
      InitializeComponent();
      var objMultiDateHelpper = (CollectionViewSource)FindResource("ObjMultiDateHelpper");
      objMultiDateHelpper.Source = _lstMultiDate;
      var objSalesRoomByUser = (CollectionViewSource)FindResource("ObjSalesRoomByUser");
      objSalesRoomByUser.Source = _lstSalesRoomByUsers;
      var objGoalsHelpper = (CollectionViewSource)FindResource("ObjGoalsHelpper");
      objGoalsHelpper.Source = _lstGoals;
      var objPersonnelShort = (CollectionViewSource)FindResource("ObjPersonnelShort");
      objPersonnelShort.Source = _lstPersonnels;

      KeyDown += Close_KeyPreviewESC;
    }
    #endregion

    #region Eventos del Formulario

    #region Botones
    #region btnCancel_Click
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      ok = false;
      SaveFilterValues();
      Close();
    }
    #endregion

    #region btnOk_Click
    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      if (DateHelper.ValidateValueDate(dtmStart, dtmEnd))
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
          item.IsCheck = chkAllSalesRoomConcentrate.IsChecked.Value;
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
      int count = _lstGoals.Count(item => item.IsCheck);
      statusBarNumSalesRoomConcentrate.Content = $"{count}/{dtgSalesRoomConcentrate.Items.Count} Sales Rooms selected";
    }
    #endregion

    #endregion

    #region Grids

    #endregion

    #region cmbDate_SelectionChanged
    private void cmbDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count <= 0) return;
      EnumPredefinedDate selected = (EnumPredefinedDate)((ComboBox)e.OriginalSource).SelectedValue;
      var dateRange = DateHelper.GetDateRange(selected);
      grdDtmStart.IsEnabled = grdDtgEnd.IsEnabled = (selected == EnumPredefinedDate.DatesSpecified);
      dtmStart.Value = dateRange.Item1;
      dtmEnd.Value = dateRange.Item2;
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

    private void dateTimeEnd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var picker = sender as DateTimePicker;
      if (picker == null) return;
      DateTime date = (DateTime)picker.Value;
      ChangeDates(date);
      if (_shWeeks)
        LoadEfficiencyWeek();
    }
    #endregion

    #region Close_KeyPreviewESC
    private void Close_KeyPreviewESC(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        btnCancel_Click(null, null);
    }
    #endregion

    #region dtgSalesRoomMultiDateRange_BeginningEdit
    /// <summary>
    /// Valida que se pueda editar la fila del grid
    /// </summary>
    /// <history>
    /// [aalcocer] 05/07/2016 Created
    /// </history>
    private void dtgSalesRoomMultiDateRange_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      var multiDateHelpper = e.Row.Item as MultiDateHelpper;
      if (multiDateHelpper != null && (string.IsNullOrEmpty(multiDateHelpper.SalesRoom) && !e.Column.Equals(cmbSalesRoom)))
      {
        UIHelper.ShowMessage("Enter the sales room first.", MessageBoxImage.Exclamation, "Intelligence Marketing");
        int rowIndex = dtgSalesRoomMultiDateRange.SelectedIndex != -1 ? dtgSalesRoomMultiDateRange.SelectedIndex : 0;
        GridHelper.SelectRow(dtgSalesRoomMultiDateRange, rowIndex, 0, true);
        e.Cancel = true;
      }
    }
    #endregion

    #region TextBoxNumeric_PreviewTextInput

    /// <summary>
    /// Valida que solo puedan insertar numeros enteros
    /// </summary>
    /// <history>
    /// [aalcocer] 07/07/2016 Created
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
    /// [aalcocer] 07/07/2016 Created
    /// </history>
    private void TextBoxNumeric_LostFocus(object sender, RoutedEventArgs e)
    {
      TextBox textBox = (TextBox)sender;
      if (textBox?.Text == string.Empty)
        textBox.Text = "0";
    }

    #endregion TextBoxNumeric_LostFocus

    #endregion


  }
}
