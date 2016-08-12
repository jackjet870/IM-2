using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Globalization;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
  #region DateTimeConvert
  /// <summary>
  ///   Formatea los horarios de tour
  /// </summary>
  /// <history>
  ///   [vku] 05/Jul/2016 Created
  /// </history>
  public class DateTimeConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is DateTime)
      {
        var test = (DateTime)value;
        if (test == DateTime.MinValue)
        {
          return "";
        }
        var date = test.ToString("hh:mm tt");
        return (date);
      }

      return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (ValidateHelper.IsValidTimeFormat(value.ToString()))
      {
        var test = DateTime.Parse(value.ToString());
        if (test == DateTime.MinValue)
        {
          return "";
        }
        return (test);
      }
      return string.Empty;
    }
  }
  #endregion

  /// <summary>
  /// Interaction logic for frmTourTimes.xaml
  /// Catalogo de horarios de tour por dia
  /// </summary>
  public partial class frmTourTimes : Window
  {
    #region Atributos
    List<TourTime> _tourTimes = new List<TourTime>();
    List<TourTimeByDay> _tourTimeByDay = new List<TourTimeByDay>();
    List<TourTimeBySalesRoomWeekDay> _tourTimeBySalesRoomWeekDay = new List<TourTimeBySalesRoomWeekDay>();
    private EnumMode enumMode;
    private EnumTourTimesSchema _enumTourTimes;
    private bool isInsert = false;
    private bool isCancel = false;
    private bool _blnEdit = false;//boleano para saber si se tiene minimo permiso para editar|agregar 
    #endregion

    #region Constructor
    public frmTourTimes()
    {
      InitializeComponent();
    }
    #endregion

    #region Metodos
    #region LoadCatalogs
    /// <summary>
    ///   Carga los catalogos de los combos
    /// </summary>
    /// <history>
    ///   [vku] 22/Jun/2016 Created
    /// </history>
    protected async void LoadCatalogs()
    {
      try
      {
        List<TourTimesSchema> lstTourTimesSchemas = await BRTourTimesSchemas.GetTourTimesSchemas(nStatus: 1);
        List<LeadSourceByUser> lstLeadSourceByUser = await BRLeadSources.GetLeadSourcesByUser((App.User.User.peID));
        List<SalesRoomByUser> lstSalesRoomByUser = await BRSalesRooms.GetSalesRoomsByUser((App.User.User.peID));
        List<WeekDay> lstWeekDays = await BRWeekDays.GetWeekDays("EN");
        int tourTimesSchema = await BRConfiguration.GetTourTimesSchema();
        cboSchema.SelectedValue = tourTimesSchema;
        cboSchema.ItemsSource = lstTourTimesSchemas;
        cboLeadSource.ItemsSource = lstLeadSourceByUser;
        cboSalesRoom.ItemsSource = lstSalesRoomByUser;
        cboWeekDay.ItemsSource = lstWeekDays;
        cboLeadSourceTo.ItemsSource = lstLeadSourceByUser;
        cboSalesRoomTo.ItemsSource = lstSalesRoomByUser;
        EnableCopy();
        LoadListTourTimes();
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Sales Room");
      }
    }
    #endregion

    #region LoadListTourTimes
    /// <summary>
    ///   Carga los horarios de Tours
    /// </summary>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    protected async void LoadListTourTimes()
    {
      string ttls = string.Empty, ttsr = string.Empty;
      int ttday = 0;
      try
      {
        status.Visibility = Visibility.Visible;
        string sMsj = ValidateHelper.ValidateForm(this, "Tour Times");
        if (sMsj == "")
        {
          ttls = (string)cboLeadSource.SelectedValue;
          ttday = Convert.ToInt32(cboWeekDay.SelectedValue);
          ttsr = (string)cboSalesRoom.SelectedValue;
          TourTimes _lstTourTimes = await BRTourTimes.GetTourTimes(_enumTourTimes, ttls, ttsr, ttday);
          TourTimes lstTourTimes = await BRTourTimes.GetTourTimes(_enumTourTimes, ttls, ttsr, ttday);
          switch (_enumTourTimes)
          {
            case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
              _tourTimes = _lstTourTimes.TourTimeByLeadSourceSalesRoom;
              dgrTourTimes.DataContext = lstTourTimes.TourTimeByLeadSourceSalesRoom;
              dgrTourTimes.ItemsSource = lstTourTimes.TourTimeByLeadSourceSalesRoom;
              break;
            case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
              _tourTimeByDay = _lstTourTimes.TourTimeByLeadSourceSalesRoomWeekDay;
              dgrTourTimes.DataContext = lstTourTimes.TourTimeByLeadSourceSalesRoomWeekDay;
              dgrTourTimes.ItemsSource = lstTourTimes.TourTimeByLeadSourceSalesRoomWeekDay;
              break;
            case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
              _tourTimeBySalesRoomWeekDay = _lstTourTimes.TourTimeBySalesRoomWeekDay;
              dgrTourTimes.DataContext = lstTourTimes.TourTimeBySalesRoomWeekDay;
              dgrTourTimes.ItemsSource = lstTourTimes.TourTimeBySalesRoomWeekDay;
              break;
          }
          StatusBarReg.Content = dgrTourTimes.Items.Count - 1 + " Tour Times.";
          status.Visibility = Visibility.Collapsed;
        }
      }
      catch (Exception ex)
      {
        UIHelper.ShowMessage(ex.Message, MessageBoxImage.Error, "Tour Times");
      }
    }
    #endregion

    #region ShowCombos
    /// <summary>
    ///   Muestra u oculta los combos de Lead Source y de dia de la semana
    /// </summary>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    protected void ShowCombos()
    {
      cboLeadSource.Visibility = (_enumTourTimes != EnumTourTimesSchema.ttsBySalesRoomWeekDay) ? Visibility.Visible : Visibility.Collapsed;
      lblLeadSource.Visibility = (_enumTourTimes != EnumTourTimesSchema.ttsBySalesRoomWeekDay) ? Visibility.Visible : Visibility.Collapsed;

      cboWeekDay.Visibility = (_enumTourTimes != EnumTourTimesSchema.ttsByLeadSourceSalesRoom) ? Visibility.Visible : Visibility.Collapsed;
      lblWeekDay.Visibility = (_enumTourTimes != EnumTourTimesSchema.ttsByLeadSourceSalesRoom) ? Visibility.Visible : Visibility.Collapsed;
    }
    #endregion

    #region ShowMessage
    /// <summary>
    ///   Muestra los mensajes de validación
    /// </summary>
    /// <param name="msj"></param>
    /// <history>
    ///   [vku] 28/Jun/2016 Created
    /// </history>
    public void ShowMessage(int msj)
    {
      switch (msj)
      {
        case 1:
          UIHelper.ShowMessage("Specify time", MessageBoxImage.Warning, "IM.Administrator");
          break;
        case 2:
          UIHelper.ShowMessage("Specify pick up time", MessageBoxImage.Warning, "IM.Administrator");
          break;
        case 3:
          UIHelper.ShowMessage("Specify max books", MessageBoxImage.Warning, "IM.Administrator");
          break;
        case 4:
          UIHelper.ShowMessage("Tour Time already exists", MessageBoxImage.Warning, "IM.Administrator");
          break;
      }
    }
    #endregion

    #region Validate
    /// <summary>
    ///   Valida los datos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Entity"></param>
    /// <returns></returns>
    /// <history>
    ///   [vku] 28/Jun/2016 Created
    /// </history>
    public bool Validate<T>(T Entity)
    {
      bool blnValid = true;
      switch (_enumTourTimes)
      {
        case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
          TourTime ttbylssr = Entity as TourTime;
          if (ttbylssr.ttT == DateTime.MinValue) { ShowMessage(1); blnValid = false; }
          else if (ttbylssr.ttPickUpT == DateTime.MinValue) { ShowMessage(2); blnValid = false; }
          else if (ttbylssr.ttMaxBooks.ToString() == "") { ShowMessage(3); blnValid = false; }
          break;
        case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
          TourTimeByDay ttbylssrwd = Entity as TourTimeByDay;
          if (ttbylssrwd.ttT == DateTime.MinValue) { ShowMessage(1); blnValid = false; }
          else if (ttbylssrwd.ttPickUpT == DateTime.MinValue) { ShowMessage(2); blnValid = false; }
          else if (ttbylssrwd.ttMaxBooks.ToString() == "") { ShowMessage(3); blnValid = false; }
          break;
        case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
          TourTimeBySalesRoomWeekDay ttbysrwd = Entity as TourTimeBySalesRoomWeekDay;
          if (ttbysrwd.ttT == DateTime.MinValue) { ShowMessage(1); blnValid = false; }
          else if (ttbysrwd.ttPickUpT == DateTime.MinValue) { ShowMessage(2); blnValid = false; }
          else if (ttbysrwd.ttMaxBooks.ToString() == "") { ShowMessage(3); blnValid = false; }
          break;
      }
      return blnValid;
    }
    #endregion

    #region ExistTime
    /// <summary>
    ///   Valida que el horario no este repetido (Si existe)
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    /// <history>
    ///   [vku] 29/Jun/2016 Created
    /// </history>
    public bool ExistTime(DateTime time)
    {
      bool blnExist = false;
      switch (_enumTourTimes)
      {
        case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
          foreach(TourTime tt in _tourTimes) { if(tt.ttT.ToString("hh:mm tt") == time.ToString("hh: mm tt")) blnExist = true; }
          break;
        case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
          foreach (TourTimeByDay tt in _tourTimeByDay) { if (tt.ttT.ToString("hh:mm tt") == time.ToString("hh:mm tt")) blnExist = true; }
          break;
        case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
          foreach (TourTimeBySalesRoomWeekDay tt in _tourTimeBySalesRoomWeekDay) { if (tt.ttT.ToString("hh:mm tt") == time.ToString("hh:mm tt")) blnExist = true; }
          break;
      }
      return blnExist;
    }
    #endregion

    #region EnableCopy
    /// <summary>
    ///   Habilita / deshabilita los controles de copia de horarios de tour
    /// </summary>
    /// <history>
    ///   [vku] 01/Jul/2016 Created
    /// </history>
    public void EnableCopy()
    {
      stkCopyToLeadSource.IsEnabled = _enumTourTimes != EnumTourTimesSchema.ttsBySalesRoomWeekDay;
      btnCopyToSalesRoomsWeekDaysOfLeadSource.IsEnabled = _enumTourTimes == EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay;
      btnCopyToLeadSourcesOfProgram.IsEnabled = _enumTourTimes != EnumTourTimesSchema.ttsBySalesRoomWeekDay;
      btnCopyToWeekDaysOfSalesRoom.IsEnabled = _enumTourTimes == EnumTourTimesSchema.ttsBySalesRoomWeekDay;
    }
    #endregion

    #endregion

    #region Eventos
    #region Window_Loaded
    /// <summary>
    ///   Carga la ventana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _blnEdit = App.User.HasPermission(EnumPermission.TourTimes, EnumPermisionLevel.Special);
      if (!_blnEdit)
      {
        grpCopy.Visibility = Visibility.Collapsed;
      }
      enumMode = ((_blnEdit == true) ? EnumMode.Edit : EnumMode.ReadOnly);
      LoadCatalogs();
    }
    #endregion

    #region btnRef_Click
    /// <summary>
    ///   Refreca la lista de tour times
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void btnRef_Click(object sender, RoutedEventArgs e)
    {
      LoadListTourTimes();
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    ///   Valida las teclas INS|MAYSU|LOCKNUM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Capital:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            break;
          }
        case Key.Insert:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            break;
          }
        case Key.NumLock:
          {
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            break;
          }
      }
    }
    #endregion

    #region cboSchema_SelectionChanged
    /// <summary>
    ///   Despliega los horarios de tour al cambiar de esquema
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void cboSchema_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _enumTourTimes = (EnumTourTimesSchema)cboSchema.SelectedValue;
      ShowCombos();
      EnableCopy();
      LoadListTourTimes();
    }
    #endregion

    #region cboLeadSource_SelectionChanged
    /// <summary>
    ///   Despliega los horarios de tour al cambiar de Lead Source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void cboLeadSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LoadListTourTimes();
    }
    #endregion

    #region cboSalesRoom_SelectionChanged
    /// <summary>
    ///   Despliega los horarios de tour al cambiar de sala de venta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void cboSalesRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LoadListTourTimes();
    }
    #endregion

    #region cboWeekDay_selectionChanged
    /// <summary>
    ///   Depliega los horarios de tour al cambiar el dia de la semana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void cboWeekDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LoadListTourTimes();
    }
    #endregion

    #region Eventos DataGrid TourTimes
    #region dgrTourTimes_RowEditEnding
    /// <summary>
    ///   Guarda o modifica los horarios de tour despues de editar el datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 27/Jun/2016 Created
    /// </history>
    private async void dgrTourTimes_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      DataGrid dgr = sender as DataGrid;
      dgr.RowEditEnding -= dgrTourTimes_RowEditEnding;
      if (isCancel)
      {
        dgr.CancelEdit();
        dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
      }
      else
      {
        int nRes = 0;
        switch (_enumTourTimes)
        {
          #region TourTime
          case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
            TourTime ttbylssr = e.Row.DataContext as TourTime;
            if (isInsert)
            {
              if (Validate(ttbylssr))
              {
                if (MessageBox.Show("Are you sure you want add new Tour Time?", "IM.Administrator",
                 MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                  dgr.CommitEdit();
                  status.Visibility = Visibility.Visible;
                  txtStatus.Text = "Saving Data...";
                  ttbylssr.ttls = cboLeadSource.SelectedValue.ToString();
                  ttbylssr.ttsr = cboSalesRoom.SelectedValue.ToString();
                  nRes = await BREntities.OperationEntity(ttbylssr, EnumMode.Add);
                  dgr.Items.Refresh();
                  dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
                }
                else
                {
                  dgr.CancelEdit();
                  dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
                }
              }
              else
              {
                e.Cancel = true;
                dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
              }           
            }
            else {
              status.Visibility = Visibility.Visible;
              txtStatus.Text = "Saving Data...";
              nRes = await BREntities.OperationEntity(ttbylssr, EnumMode.Edit);
              dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
            }
            break;
          #endregion

          #region TourTimesByDay
          case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
            TourTimeByDay ttbylssrwd = e.Row.DataContext as TourTimeByDay;
            if (isInsert)
            {
              if (Validate(ttbylssrwd))
              {
                if (MessageBox.Show("Are you sure you want add new Tour Time?", "IM.Administrator",
                 MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                  dgr.CommitEdit();
                  status.Visibility = Visibility.Visible;
                  txtStatus.Text = "Saving Data...";
                  ttbylssrwd.ttls = cboLeadSource.SelectedValue.ToString();
                  ttbylssrwd.ttsr = cboSalesRoom.SelectedValue.ToString();
                  ttbylssrwd.ttDay = Convert.ToByte(cboWeekDay.SelectedValue.ToString());
                  nRes = await BREntities.OperationEntity(ttbylssrwd, EnumMode.Add);
                  dgr.Items.Refresh();
                  dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
                }
                else
                {
                  dgr.CancelEdit();
                  dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
                }
              }
              else
              {
                e.Cancel = true;
                dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
              }  
            }
            else {
              status.Visibility = Visibility.Visible;
              txtStatus.Text = "Saving Data...";
              nRes = await BREntities.OperationEntity(ttbylssrwd, EnumMode.Edit);
              dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
            }
            break;
          #endregion

          #region TourTimeBySalesRoomWeekDay
          case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
            TourTimeBySalesRoomWeekDay ttbysrwd = e.Row.DataContext as TourTimeBySalesRoomWeekDay;
            if (isInsert)
            {
              if(Validate(ttbysrwd))
              {
                if (MessageBox.Show("Are you sure you want add new Tour Time?", "IM.Administrator",
                 MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                  dgr.CommitEdit();
                  status.Visibility = Visibility.Visible;
                  txtStatus.Text = "Saving Data...";
                  ttbysrwd.ttsr = cboSalesRoom.SelectedValue.ToString();
                  ttbysrwd.ttDay = Convert.ToByte(cboWeekDay.SelectedValue.ToString());
                  nRes = await BREntities.OperationEntity(ttbysrwd, EnumMode.Add);
                  dgr.Items.Refresh();
                  dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
                }
                else
                {
                  dgr.CancelEdit();
                  dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
                }
              }
              else
              {
                e.Cancel = true;
                dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
              }
            }
            else {
              status.Visibility = Visibility.Visible;
              txtStatus.Text = "Saving Data...";
              nRes = await BREntities.OperationEntity(ttbysrwd, EnumMode.Edit);
              dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
            }
            break;
            #endregion
        }
        status.Visibility = Visibility.Collapsed;
        StatusBarReg.Content = dgrTourTimes.Items.Count - 1 + " Tour Times.";
      }
    }
    #endregion

    #region dgrTourTimes_AddingNewItem
    /// <summary>
    ///   Evento que se dispara al insertar un registro en el datagrid TourTime
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 24/Jun/2016 Created
    /// </history>
    private void dgrTourTimes_AddingNewItem(object sender, AddingNewItemEventArgs e)
    {
      isInsert = true;
    }
    #endregion

    #region dgrTourTimes_BeginningEdit
    /// <summary>
    ///   Evento que se dispara cuando se empieza a editar un registro del datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 24/Jun/2016 Created
    /// </history>
    private void dgrTourTimes_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      DataGrid dg = sender as DataGrid; 
      if (_blnEdit)
      {
        if ((e.Column.Header.ToString() == "Tour Time" || e.Column.Header.ToString() == "Pickup Time") && !e.Row.IsNewItem)
        {
          e.Cancel = true;       
        }
        else
        {
         if(e.Column.Header.ToString() == "Pickup Time" && e.Row.IsNewItem)
          {
            switch (_enumTourTimes)
            {
              case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
                TourTime rowlssr = (TourTime)dgrTourTimes.SelectedItem;
                if (rowlssr.ttT != DateTime.MinValue)
                  rowlssr.ttPickUpT = rowlssr.ttT;
                else
                  e.Cancel = true;
                break;
              case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
                TourTimeByDay rowlssrwd = (TourTimeByDay)dgrTourTimes.SelectedItem;
                if (rowlssrwd.ttT != DateTime.MinValue)
                  rowlssrwd.ttPickUpT = rowlssrwd.ttT;
                else
                  e.Cancel = true;
                break;
              case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
                TourTimeBySalesRoomWeekDay rowttbysrwd = (TourTimeBySalesRoomWeekDay)dgrTourTimes.SelectedItem;
                if (rowttbysrwd.ttT != DateTime.MinValue)
                  rowttbysrwd.ttPickUpT = rowttbysrwd.ttT;
                else
                  e.Cancel = true;
                break;
            }
          }
          else
          {
            if (e.Column.Header.ToString() == "Max Books" && e.Row.IsNewItem)
            {
              switch (_enumTourTimes)
              {
                case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
                  TourTime rowlssr = (TourTime)dgrTourTimes.SelectedItem;
                  if (rowlssr.ttT != DateTime.MinValue && rowlssr.ttPickUpT != DateTime.MinValue)
                    rowlssr.ttMaxBooks = 1;
                  else
                    e.Cancel = true;
                  break;
                case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
                  TourTimeByDay rowlssrwd = (TourTimeByDay)dgrTourTimes.SelectedItem;
                  if (rowlssrwd.ttT != DateTime.MinValue && rowlssrwd.ttPickUpT != DateTime.MinValue)
                    rowlssrwd.ttMaxBooks = 1;
                  else
                    e.Cancel = true;
                  break;
                case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
                  TourTimeBySalesRoomWeekDay rowsrwd = (TourTimeBySalesRoomWeekDay)dgrTourTimes.SelectedItem;
                  if (rowsrwd.ttT != DateTime.MinValue && rowsrwd.ttPickUpT != DateTime.MinValue)
                    rowsrwd.ttMaxBooks = 1;
                  else
                    e.Cancel = true;
                  break;
              }
            }
          }    
        }
      }
      else
      {
        e.Cancel = true;
      }
    }
    #endregion

    #region dgrTourTimes_CellEditEnding
    /// <summary>
    ///   Valida las celdas al terminar de ser editadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 27/Jun/2016 Created
    /// </history>
    private void dgrTourTimes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      object item = dgrTourTimes.SelectedItem;
      if (e.EditAction == DataGridEditAction.Cancel)
      {
        isCancel = true;
      }
      else
      {
        isCancel = false;
        TextBox changedTextBox = e.EditingElement as TextBox;
        switch (e.Column.Header.ToString())
        {
          #region Tour Time
          case "Tour Time":
            if (changedTextBox.Text.ToString() == "")
              isCancel = true;
            else
            {
              if (ValidateHelper.IsValidTimeFormat(changedTextBox.Text.ToString()) || ValidateHelper.IsDate(changedTextBox.Text.ToString()))
              {
                string tt = changedTextBox.Text.ToString();
                DateTime time = Convert.ToDateTime(tt);
                if (ExistTime(time))
                {
                  isCancel = true;
                  UIHelper.ShowMessage(time.ToString("hh:mm tt") + " already exists", MessageBoxImage.Warning, "IM.Administrator");
                }
                else
                {
                  switch (_enumTourTimes)
                  {
                    case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
                      ((TourTime)dgrTourTimes.SelectedItem).ttT =  time;
                      break;
                    case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
                      ((TourTimeByDay)dgrTourTimes.SelectedItem).ttT = time; 
                      break;
                    case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
                      ((TourTimeBySalesRoomWeekDay)dgrTourTimes.SelectedItem).ttT = time;
                      break;
                  }
                }
              }else
              {
                UIHelper.ShowMessage("Invalid Time", MessageBoxImage.Error, "IM.Administrator");
              }
            }
            break;
          #endregion
          #region Pickup Time
          case "Pickup Time":
            if (changedTextBox.Text.ToString() == "")
              isCancel = true;
            else
            {
              if (ValidateHelper.IsValidTimeFormat(changedTextBox.Text.ToString()) || ValidateHelper.IsDate(changedTextBox.Text.ToString()))
              {
                string pt = changedTextBox.Text.ToString();
                DateTime time = Convert.ToDateTime(pt);
                switch (_enumTourTimes)
                {
                  case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
                    ((TourTime)dgrTourTimes.SelectedItem).ttPickUpT = time;
                    break;
                  case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
                    ((TourTimeByDay)dgrTourTimes.SelectedItem).ttPickUpT = time;
                    break;
                  case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
                    ((TourTimeBySalesRoomWeekDay)dgrTourTimes.SelectedItem).ttPickUpT = time;
                    break;
                }
              }
              else
              {
                UIHelper.ShowMessage("Invalid Time", MessageBoxImage.Error, "IM.Administrator");
              }
            }
            break;
          #endregion
          #region Max Books
          case "Max Books":
            string maxBooks = changedTextBox.Text.ToString();
            if (!ValidateHelper.OnlyNumbers(maxBooks))
            {
              isCancel = true;
              UIHelper.ShowMessage("Invalid number", MessageBoxImage.Warning, "IM.Administrator");
            }
            else
            {
              if (Convert.ToInt32(maxBooks) > 255)
              {
                isCancel = true;
                UIHelper.ShowMessage("can not be greater than 255", MessageBoxImage.Warning, "IM.Administrator");
              }
              else if (Convert.ToInt32(maxBooks) < 0)
              {
                isCancel = true;
                UIHelper.ShowMessage("can not be lower than 0", MessageBoxImage.Warning, "IM.Administrator");
              }
              else
              {
                switch (_enumTourTimes)
                {
                  case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
                    if (!e.Row.IsNewItem && Convert.ToByte(maxBooks) == ((TourTime)dgrTourTimes.SelectedItem).ttMaxBooks)
                      isCancel = true;
                    else
                      ((TourTime)dgrTourTimes.SelectedItem).ttMaxBooks = Convert.ToByte(maxBooks);
                    break;
                  case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
                    if (!e.Row.IsNewItem && Convert.ToByte(maxBooks) == ((TourTimeByDay)dgrTourTimes.SelectedItem).ttMaxBooks)
                      isCancel = true;
                    else
                      ((TourTimeByDay)dgrTourTimes.SelectedItem).ttMaxBooks = Convert.ToByte(maxBooks);
                    break;
                  case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
                    if (!e.Row.IsNewItem && Convert.ToByte(maxBooks) == ((TourTimeBySalesRoomWeekDay)dgrTourTimes.SelectedItem).ttMaxBooks)
                      isCancel = true;
                    else
                      ((TourTimeBySalesRoomWeekDay)dgrTourTimes.SelectedItem).ttMaxBooks = Convert.ToByte(maxBooks);
                    break;
                }
              }
            }
            break;
            #endregion
        }
      }
    }
    #endregion

    #endregion

    #region Eventos btnsCopysTourTimes

    #region btnCopyToLeadSource_Click
    /// <summary>
    ///   Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de un Lead Source a otro
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 29/Jun/2016 Created
    /// </history>
    private async void btnCopyToLeadSource_Click(object sender, RoutedEventArgs e)
    {
      if(!ValidateHelper.ValidateRequired(cboLeadSource, "lead source (FROM)"))
        return;
      else
      {
        if (!ValidateHelper.ValidateRequired(cboLeadSourceTo, "lead source (TO)"))
          return;
        else
        {
          if (MessageBox.Show("Are you sure you want to copy the tour times of the Lead Source " + "\""+cboLeadSource.Text+"\"" +
             " to the Lead Source " + "\"" +cboLeadSourceTo.Text+"\"" + "?", "IM.Administrator", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
          {
            if(_enumTourTimes == EnumTourTimesSchema.ttsByLeadSourceSalesRoom)    
              await BRTourTimes.CopyTourTimesByLeadSourceSalesRoomToLeadSource(cboLeadSource.SelectedValue.ToString(), cboLeadSourceTo.SelectedValue.ToString());
            else
              await BRTourTimes.CopyTourTimesByLeadSourceSalesRoomWeekDayToLeadSource(cboLeadSource.SelectedValue.ToString(), cboLeadSourceTo.SelectedValue.ToString());  
          }
        }
      }
    }
    #endregion

    #region btnCopyToSalesRoom_Click
    /// <summary>
    ///   Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de una sala de ventas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 29/Jun/2016 Created
    /// </history>
    private async void btnCopyToSalesRoom_Click(object sender, RoutedEventArgs e)
    {
      if (!ValidateHelper.ValidateRequired(cboSalesRoom, "sales room (FROM)"))
        return;
      else
      {
        if (!ValidateHelper.ValidateRequired(cboSalesRoomTo, "sales room (TO)"))
          return;
        else
        {
          if (MessageBox.Show("Are you sure you want to copy the tour times of the Sales Room " + "\"" +cboSalesRoom.Text+"\"" +
             " to the Sales Room " + "\"" +cboSalesRoomTo.Text+"\"" + "?", "IM.Administrator", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
          {
            switch (_enumTourTimes)
            {
              case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
                await BRTourTimes.CopyTourTimesByLeadSourceSalesRoomToSalesRoom(cboSalesRoom.SelectedValue.ToString(), cboSalesRoomTo.SelectedValue.ToString());
                break;
              case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
                await BRTourTimes.CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoom(cboSalesRoom.SelectedValue.ToString(), cboSalesRoomTo.SelectedValue.ToString());
                break;
              case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
                await BRTourTimes.CopyTourTimesBySalesRoomWeekDayToSalesRoom(cboSalesRoom.SelectedValue.ToString(), cboSalesRoomTo.SelectedValue.ToString());
                break;
            }
          }
        }
      }
    }
    #endregion

    #region btnCopyToSalesRoomsWeekDaysOfLeadSource_Click
    /// <summary>
    ///   Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de un Lead Source, 
    ///   sala de ventas y dia de la semana a todas las salas de ventas y dias de la semana del mismo Lead Source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 29/Jun/2016 Created
    /// </history>
    private async void btnCopyToSalesRoomsWeekDaysOfLeadSource_Click(object sender, RoutedEventArgs e)
    {
      if (!ValidateHelper.ValidateRequired(cboLeadSource, "lead source"))
        return;
      else
      {
        if (!ValidateHelper.ValidateRequired(cboSalesRoom, "sales room"))
          return;
        else
        {
          if(!ValidateHelper.ValidateRequired(cboWeekDay, "week day")) 
            return;
          else
          {
            if (MessageBox.Show("Are you sure you want to copy the tour times of the Lead Source "  + "\""+cboLeadSource.Text+"\"" +
             ", Sales Room " + "\""+cboSalesRoom.Text+"\"" + ", Week Day " + "\""+cboWeekDay.Text+"\"" + " to all others Sales Rooms & Week Days of Lead Source " + "\""+cboLeadSource.Text+"\"" +  "?", "Tour Times", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
              await BRTourTimes.CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoomsWeekDaysOfLeadSource(cboLeadSource.SelectedValue.ToString(), cboSalesRoom.SelectedValue.ToString(), cboWeekDay.SelectedValue.ToString());
          }
        }
      }
    }
    #endregion

    #region btnCopyToLeadSourcesOfProgram_Click
    /// <summary>
    ///   Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de un Lead Source a 
    ///   todos los Lead Sources del mismo programa
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 29/Jun/2016 Created
    /// </history>
    private async void btnCopyToLeadSourcesOfProgram_Click(object sender, RoutedEventArgs e)
    {
      if(!ValidateHelper.ValidateRequired(cboLeadSource, "lead source"))
        return;
      else
      {
        string strProgram;
        strProgram = await BRPrograms.GetProgram(cboLeadSource.SelectedValue.ToString());
        if(MessageBox.Show("Are you sure you want to copy the tour times of the Lead Source " + "\""+cboLeadSource.Text+"\"" +
             " to all others Lead Sources of Program " + "\""+strProgram+"\"" + "?", "IM.Administrator", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
        {
          if(_enumTourTimes == EnumTourTimesSchema.ttsByLeadSourceSalesRoom)
            await BRTourTimes.CopyTourTimesByLeadSourceSalesRoomToLeadSourcesOfProgram(cboLeadSource.SelectedValue.ToString());
          else
            await BRTourTimes.CopyTourTimesByLeadSourceSalesRoomWeekDayToLeadSourcesOfProgram(cboLeadSource.SelectedValue.ToString());
        }
      }
    }
    #endregion

    #region btnCopyToWeekDaysOfSalesRoom_Click
    /// <summary>
    ///   Copia los horarios de tour por sala de ventas y dia de la semana de una sala de ventas y dia de la 
    ///   semana a todos los dias de la semana de la misma sala de ventas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 29/Jun/2016 Created
    /// </history>
    private async void btnCopyToWeekDaysOfSalesRoom_Click(object sender, RoutedEventArgs e)
    {
      if(!ValidateHelper.ValidateRequired(cboSalesRoom, "sales room"))
        return;
      else
      {
        if(!ValidateHelper.ValidateRequired(cboWeekDay, "week day"))
          return;
        else
        {
          if(MessageBox.Show("Are you sure you want to copy the tour times of the Sales Room " + "\""+cboSalesRoom.Text+"\"" +
             ", Week Day " + "\""+cboWeekDay.Text+"\"" + " to all others Week Days of Sales Room " + "\""+cboSalesRoom.Text+"\"" + "?", "IM.Administrator", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            await BRTourTimes.CopyTourTimesBySalesRoomWeekDayToWeekDaysOfSalesRoom(cboSalesRoom.SelectedValue.ToString(), cboWeekDay.SelectedValue.ToString());
        }
      }
    }
    #endregion

    #endregion

    #endregion
  }
}
