using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Data;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Base.Classes;
using System.IO;

namespace IM.Base.Forms
{
  /// <summary>
  /// Interaction logic for frmAssistance.xaml
  /// </summary>
  public partial class frmAssistance : Window
  {
    #region Atributos

    private bool _isNew;
    private bool _isLoad = true;

    CollectionViewSource assistanceViewSource;
    CollectionViewSource assistanceDataViewSource;
    CollectionViewSource assistanceStatusViewSource;
    private DateTime _dtmServerDate = new DateTime();
    List<AssistanceStatus> lstAssistStatus;
    private List<AssistanceData> _listAssistData;
    EnumPlaceType enumPalaceType;
    string palaceId;
    UserData user;

    //Para el Excel
    private List<Tuple<string, string>> filters = new List<Tuple<string, string>>();
    private DataTable dt = new DataTable();
    private string rptName;
    #endregion Atributos

    #region Constructores y destructores
    public frmAssistance(EnumPlaceType pEnumPalaceType, UserData userdata)
    {
      InitializeComponent();
      enumPalaceType = pEnumPalaceType;
      if (enumPalaceType == EnumPlaceType.LeadSource)
        palaceId = userdata.LeadSource.lsID;
      else
        palaceId = userdata.SalesRoom.srID;
      cmbWeeks.SelectedIndex = 1;
      user = userdata;

    }

    #endregion Constructores y destructores 

    #region Metodos 

    #region EnableDateTimes
    /// <summary>
    /// Cambia de dispocicion los controles DateTimePicker 
    /// </summary>
    /// <param name="setMode">true para habilitar | false para inhabilitar</param>
    /// <history>[ECANUL] 15-03-2016 Created</history>
    void EnableDateTimes(bool setMode)
    {
      if (setMode)
      {
        dtpStartt.IsEnabled = true;
        dtpEndd.IsEnabled = true;
      }
      else
      {
        dtpStartt.IsEnabled = false;
        dtpEndd.IsEnabled = false;
      }
    }
    #endregion

    #region GetFirstDayOfWeek
    /// <summary>
    /// Selecciona Por defectlo la El primer dia de la semana de la fecha seleccionada
    /// </summary>
    /// <param name="currentDate">Fecha A calcular el primer dia</param>
    /// <returns>Fecha Coorespondiente al primer dia de la semana</returns>
    /// <history>[ECANUL] 16-03-2016 Created</history>
    DateTime GetFirstDayOfWeek(DateTime currentDate)
    {
      //Se optiene el dia de la semana de la fecha actual
      DayOfWeek dayOfWeek = currentDate.DayOfWeek;
      ///Como el primer día de la semana es el 0 (Lunes), construimos
      ///un array para conocer los días que hay que restar de la fecha.
      ///Así, si es Lunes (0) restaremos 6 días, si es Domingo(6)
      ///restaremos 5 días, y si es Martes (1) restaremos 0 días.
      int[] dias = { 6, 0, 1, 2, 3, 4, 5 };
      //De la fecha actual se restan los dias coorespondientes
      return currentDate.Subtract(new TimeSpan(dias[Convert.ToInt32(dayOfWeek)], 0, 0, 0));
    }
    #endregion

    #region ChangedtpDates
    /// <summary>
    /// Cambia las fechas seleccionadas en los DateTimePicker para que siempre se seleecione el lunes en dtpStart y domingo en dtpEnd
    /// </summary>
    /// <param name="type">1 si se toma fecha referenciada del </param>
    void ChangedtpDates(DateTime Date)
    {
      DateTime dt;
      dt = GetFirstDayOfWeek(Date);

      dtpStartt.Value = dt;
      dt = dt.Date.AddDays(6);
      dtpEndd.Value = dt;
    }
    #endregion

    #region ChangeWeek
    /// <summary>
    /// Cambia las fechas de los dtp retrocediendo en semanas, segun la semana especificada
    /// </summary>
    /// <param name="weeks">0 Esta semana | 1 Semana Pasada | 2 Hace dos semanas | 3 Hace tres Semanas</param>
    /// <history>[ECANUL] 16-03-2016 Created</history>
    void ChangeWeek(int weeks)
    {
      DateTime dt = new DateTime();
      dt = DateTime.Today;
      switch (weeks)
      {
        case 0://Esta semana
          break;
        case 1://La semana Pasada
          dt = dt.AddDays(-7);
          break;
        case 2://Hace 2 Semanas
          dt = dt.AddDays(-14);
          break;
        case 3: //Hace 3 Semanas
          dt = dt.AddDays(-21);
          break;
      }
      ChangedtpDates(dt);
    }
    #endregion

    #region ChangeUseMode
    /// <summary>
    /// Habilita o Inabilita los controles del formulario segun sea el caso
    /// </summary>
    /// <param name="mode">true Habilita | false Inabilita</param>
    void ChangeUseMode(bool mode)
    {//true Edit, False View
      if (mode)
      {
        ///Inhabilita
        btnShow.IsEnabled = false;
        btnEdit.IsEnabled = false;
        cmbWeeks.IsEnabled = false;
        dtpStartt.IsEnabled = false;
        dtpEndd.IsEnabled = false;
        ///Habilita
        assistanceDataDataGrid.IsReadOnly = false;
        btnSave.IsEnabled = true;
        btnCancel.IsEnabled = true;
      }
      else
      {
        //Habilita
        btnShow.IsEnabled = true;
        btnEdit.IsEnabled = true;
        cmbWeeks.IsEnabled = true;
        if (cmbWeeks.SelectedIndex == 0)
        {
          dtpStartt.IsEnabled = true;
          dtpEndd.IsEnabled = true;
        }
        //Inhabilita
        assistanceDataDataGrid.IsReadOnly = true;
        btnSave.IsEnabled = false;
        btnCancel.IsEnabled = false;
      }
    }
    #endregion

    #region LoadGrid
    /// <summary>
    /// Carga El grid Con la lista de asistencia con las fechas seleccionadas
    /// </summary>
    /// <history>[ECANUL] 19-03-2016 CREATED</history>
    async void LoadGrid()
    {
      StaStart("Loading Assistance List...");
      _listAssistData = BRAssistance.GetAssistance(enumPalaceType, palaceId, dtpStartt.Value.Value, dtpEndd.Value.Value);
      assistanceDataViewSource = ((CollectionViewSource)(this.FindResource("assistanceDataViewSource")));
      assistanceStatusViewSource = ((CollectionViewSource)(this.FindResource("assistanceStatusViewSource")));
      assistanceViewSource = ((CollectionViewSource)(this.FindResource("assistanceViewSource")));
      AssistanceStatus ast = new AssistanceStatus();
      lstAssistStatus = await BRAssistancesStatus.GetAssitanceStatus(ast, 1);
      assistanceStatusViewSource.Source = lstAssistStatus;
      if (_listAssistData.Count != 0)
      {
        assistanceDataDataGrid.ItemsSource = _listAssistData;
        _isNew = false;
      }
      else
      {
        if (UIHelper.ShowMessage("There is no assistance for this week.\nWould you like to generate?", MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
          List<PersonnelAssistance> lstPersonAssist = BRAssistance.GetPersonnelAssistance(enumPalaceType, palaceId, dtpStartt.Value.Value, dtpEndd.Value.Value);
          lstPersonAssist.ForEach(c =>
          {
            var assistance = AssistanceToAssistance.ConvertPersonnelAssistanceToAssistanceData(c);
            _listAssistData.Add(assistance);
          });
          _isNew = true;
          btnEdit.IsEnabled = true;
        }
        else
          btnEdit.IsEnabled = false;
        assistanceDataDataGrid.ItemsSource = _listAssistData;
      }
      StaEnd();
    }
    #endregion

    #region SaveAssistances
    /// <summary>
    /// Guarda las Asistencias de un periodo de fechas del personal del lugar
    /// </summary>
    /// <history>
    /// [ecanul] 22/03/2016 Created
    /// [ecanul] 04/05/2016 Modificated Ahora indica si es Nuevo o Modificacion antes de guardar, Usa el BR BREntities
    /// [jorcanche] 26/06/2016 se agrego asincronia
    /// </history>
    async void SaveAssistances()
    {
      StaStart("Saving Data...");
      int nres = 0;
      List<Assistance> lstAssistances = new List<Assistance>();

      _listAssistData.ForEach(c =>
      {
        lstAssistances.Add(AssistanceToAssistance.ConvertAssistanceDataToAssistance(c));
      });

      nres = await BREntities.OperationEntities(lstAssistances, _isNew ? EnumMode.Add : EnumMode.Edit);
      ChangeUseMode(false);
      UIHelper.ShowMessage("Saved Assistance", MessageBoxImage.Information, "Saved");
      LoadGrid();
      StaEnd();
    }
    #endregion

    #region StaStart
    /// <summary>
    /// Indica en la barra de estado que se inicio un proceso
    /// </summary>
    /// <param name="message">mensaje a mostrar</param>
    /// <history>[ECANUL] 18-03-2016 Created </history>
    private void StaStart(String message)
    {
      lblStatusBarMessage.Content = message;
      imgStatusBarMessage.Visibility = Visibility.Visible;
      this.Cursor = Cursors.Wait;
    }
    #endregion

    #region StaEnd
    /// <summary>
    /// Indica en la barra de estado que se finalizo un proceso
    /// </summary>
    /// <history>[ECANUL] 18-03-2016 Created </history>
    private void StaEnd()
    {
      lblStatusBarMessage.Content = null;
      imgStatusBarMessage.Visibility = Visibility.Hidden;
      this.Cursor = null;
    }
    #endregion

    #endregion Metodos

    #region Eventos del Formulario

    #region cmbWeeks_SelectionChanged
    /// <summary>
    /// Cambia las fechas para el listado de asistencias
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [ecanul] 15/03/2016 Created
    /// </history>
    private void cmbWeeks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      int cmbSelected = cmbWeeks.SelectedIndex;
      switch (cmbSelected)
      {
        case 0: //El usuario escoge
          EnableDateTimes(true);
          break;
        case 1: //Esta semana
          EnableDateTimes(false);
          ChangeWeek(0);
          LoadGrid();
          break;
        case 2: //la semana pasada
          EnableDateTimes(false);
          ChangeWeek(1);
          LoadGrid();
          break;
        case 3: //Hace 2 semanas
          EnableDateTimes(false);
          ChangeWeek(2);
          LoadGrid();
          break;
        case 4: //Hace 3 semanas
          EnableDateTimes(false);
          ChangeWeek(3);
          LoadGrid();
          break;
      }
    }
    #endregion

    #region dateTime_ValueChanged
    /// <summary>
    /// Cambia el listado al cambiar la fecha
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    ///  [ecanul] 15/03/2016 Created
    private void dateTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (!_isLoad)
        ChangedtpDates(dtpStartt.Value.Value);
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Carga los datos iniciales
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    ///  [ecanul] 15/03/2016 Created
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _isLoad = false;
      StaStart("Loading Data...");
      _dtmServerDate = BRHelpers.GetServerDate();
      dtpStartt.Value = _dtmServerDate;
      dtpEndd.Value = _dtmServerDate.AddDays(7);
      ChangeUseMode(false);
      //_isLoad = false;
      StaEnd();
    }
    #endregion

    #region btnSave_Click
    /// <summary>
    /// Invoca el guardado de la asistencia
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [ecanul] 15/03/2016 Created
    /// </history> 
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      SaveAssistances();
    }
    #endregion

    #region btnEdit_Clic
    /// <summary>
    /// Cambia el modo de Edicion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [ecanul] 15/03/2016 Created
    /// </history> 
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      ChangeUseMode(true);
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cancela la Edicion 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [ecanul] 15/03/2016 Created
    /// </history> 
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      ChangeUseMode(false);
      LoadGrid();
    }
    #endregion

    #region btnShow_Click
    /// <summary>
    /// Muestra el listado de asistencia
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [ecanul] 15/03/2016 Created
    /// </history> 
    private void btnShow_Click(object sender, RoutedEventArgs e)
    {
      LoadGrid();
    } 
    #endregion

    #region btnToExcel_Click
    /// <summary>
    /// Genera el excel y lo guarda
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [ecanul] 15/03/2016 Created
    /// </history> 
    private async void btnToExcel_Click(object sender, RoutedEventArgs e)
    {
      StaStart("Loading assistances Excel...");
      btnToExcel.IsEnabled = false;
      //Si tiene LeadSource se toma este de preferencia
      if (user.LeadSource != null)
        filters.Add(Tuple.Create("Lead Sourse", user.LeadSource.lsID));
      else
        filters.Add(Tuple.Create("Sales Room", user.SalesRoom.srID));

      _listAssistData = BRAssistance.GetAssistance(enumPalaceType, palaceId, dtpStartt.Value.Value, dtpEndd.Value.Value);
      if (_listAssistData.Count > 0)
      {
        FileInfo file = await ExportReports.RptAssitance(_listAssistData, dtpStartt.Value.Value, dtpEndd.Value.Value, filters);
        if (file != null)
        {
          frmDocumentViewer documentViewver = new frmDocumentViewer(file, user.HasPermission(EnumPermission.RptExcel, EnumPermisionLevel.ReadOnly),false);
          documentViewver.ShowDialog();
        }          
      }
      else
      {
        UIHelper.ShowMessage("There is no Information to generate the report", MessageBoxImage.Exclamation, "Save the data");
      }
      StaEnd();
      btnToExcel.IsEnabled = true;
    }
    #endregion

    #region Windows_KeyDown
    /// <summary>
    /// Al oprimir una tecla
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [ecanul] 15/03/2016 Created
    /// </history> 
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
          break;
        case Key.Insert:
          KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
          break;
        case Key.NumLock:
          KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
          break;
      }
    }
    #endregion

    #region Window_IsKeyboardFocusedChanged
    /// <summary>
    /// KeyboardFocusedChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [ecanul] 15/03/2016 Created
    /// </history> 
    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    } 
    #endregion

    #endregion Eventos del Formulario
  }
}
