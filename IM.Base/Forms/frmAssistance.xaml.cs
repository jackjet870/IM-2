﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Linq;
using System.Data;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using OfficeOpenXml.Style;
using IM.Base.Classes;

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

    /// <summary>
    /// Suma el numero de asistencias para actualizar el grid
    /// </summary>
    /// <returns>total de asistencias</returns>
    /// <history>[ECANUL] 22-03-2016 CREATED</history>
    int ChangeCountAssistanceDays()
    {
      int numAssist = 0;
      List<AssistanceData> assist = assistanceDataDataGrid.SelectedItems.OfType<AssistanceData>().ToList();
      if (assist[0].asMonday == "A" || assist[0].asMonday == "L")
        numAssist = numAssist + 1;
      if (assist[0].asTuesday == "A" || assist[0].asTuesday == "L")
        numAssist = numAssist + 1;
      if (assist[0].asWednesday == "A" || assist[0].asWednesday == "L")
        numAssist = numAssist + 1;
      if (assist[0].asThursday == "A" || assist[0].asThursday == "L")
        numAssist = numAssist + 1;
      if (assist[0].asFriday == "A" || assist[0].asFriday == "L")
        numAssist = numAssist + 1;
      if (assist[0].asSaturday == "A" || assist[0].asSaturday == "L")
        numAssist = numAssist + 1;
      if (assist[0].asSunday == "A" || assist[0].asSunday == "L")
        numAssist = numAssist + 1;
      //assistanceDataDataGrid.SelectedItems.Add(numAssist);
      return numAssist;
    }

    /// <summary>
    /// Guarda las Asistencias de un periodo de fechas del personal del lugar
    /// </summary>
    /// <history>
    /// [ecanul] 22/03/2016 Created
    /// [ecanul] 04/05/2016 Modificated Ahora indica si es Nuevo o Modificacion antes de guardar, Usa el BR BREntities
    /// [jorcanche] 26062016 se agrego asincronia
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

    #endregion Metodos

    #region Eventos del Formulario
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

    private void dateTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (!_isLoad)
        ChangedtpDates(dtpStartt.Value.Value);
    }

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

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      SaveAssistances();
    }

    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      ChangeUseMode(true);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      ChangeUseMode(false);
      LoadGrid();
    }

    private void btnShow_Click(object sender, RoutedEventArgs e)
    {
      LoadGrid();
    }

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
        dt = TableHelper.GetDataTableFromList(_listAssistData, true);
        rptName = "Assistance " + palaceId;

        string dateRange = DateHelper.DateRange(dtpStartt.Value.Value, dtpEndd.Value.Value);
        string dateRangeFileName = DateHelper.DateRangeFileName(dtpStartt.Value.Value, dtpEndd.Value.Value);

        ExcelFormatItemsList format = new ExcelFormatItemsList();
        format.Add("Place Type", "asPlaceType");
        format.Add("Place ID", "asPlaceID");
        format.Add("Date Start", "asStartD", format: EnumFormatTypeExcel.Date);
        format.Add("Date End", "asEndD", format: EnumFormatTypeExcel.Date);
        format.Add("ID", "aspe");
        format.Add("Name", "peN");
        format.Add("Monday", "asMonday");
        format.Add("Tuesday", "asTuesday");
        format.Add("Wednesday", "asWednesday");
        format.Add("Thursday", "asThursday");
        format.Add("Friday", "asFriday");
        format.Add("Saturday", "asSaturday");
        format.Add("Sunday", "asSunday");
        format.Add("#Assistence", "asNum");
        //TODO: EZE por favor comenta el codigo, abre el reporte, y cambia MessageBox icono Informacion.
        await EpplusHelper.CreateCustomExcel(dt, filters, rptName, dateRangeFileName, format);
        UIHelper.ShowMessage("Generated Report", MessageBoxImage.Exclamation, "Generated");
      }
      else
      {
        UIHelper.ShowMessage("There is no Information to generate the report", MessageBoxImage.Exclamation, "Save the data");
      }
      StaEnd();
      btnToExcel.IsEnabled = true;
    }

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

    private void Window_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
      KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
      KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
    }

    #endregion Eventos del Formulario


  }
}
