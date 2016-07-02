using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.BusinessRules.BR;
using IM.Base.Helpers;

namespace IM.Administrator.Forms
{
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
    private bool isEdit = false;
    private bool isCancel = false;
    private bool _blnEdit = false;//boleano para saber si se tiene minimo permiso para editar|agregar 
    #endregion

    public frmTourTimes()
    {
      InitializeComponent();
    }

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
          StatusBarReg.Content = dgrTourTimes.Items.Count + " Tour Times.";
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
          if (ttbylssr.ttT == null) { ShowMessage(1); blnValid = false; }
          else if(ExistTime(Convert.ToDateTime(ttbylssr.ttT))) { ShowMessage(4); blnValid = false; }
          else if (ttbylssr.ttPickUpT == null) { ShowMessage(1); blnValid = false; }
          else if (ttbylssr.ttMaxBooks.ToString() == "") { ShowMessage(3); blnValid = false; }
          break;
        case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
          TourTimeByDay ttbylssrwd = Entity as TourTimeByDay;
          if (ttbylssrwd.ttT == null) { ShowMessage(1); blnValid = false; }
          else if (ExistTime(Convert.ToDateTime(ttbylssrwd.ttT))) { ShowMessage(4); blnValid = false; }
          else if (ttbylssrwd.ttPickUpT == null) { ShowMessage(1); blnValid = false; }
          else if (ttbylssrwd.ttMaxBooks.ToString() == "") { ShowMessage(3); blnValid = false; }
          break;
        case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
          TourTimeBySalesRoomWeekDay ttbysrwd = Entity as TourTimeBySalesRoomWeekDay;
          if (ttbysrwd.ttT == null) { ShowMessage(1); blnValid = false; }
          else if (ExistTime(Convert.ToDateTime(ttbysrwd.ttT.ToString()))) { ShowMessage(4); blnValid = false; }
          else if (ttbysrwd.ttPickUpT == null) { ShowMessage(1); blnValid = false; }
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
      enumMode = ((_blnEdit == true) ? EnumMode.edit : EnumMode.preview);
      LoadCatalogs();
    }
    #endregion

    #region btnRef_Click
    /// <summary>
    ///   Refreca los 
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

    #region Cell_DoubleClick
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
     
    }
    #endregion

    #region Row_KeyDown
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void Row_KeyDown(object sender, KeyEventArgs e)
    {
      bool blnHandled = false;
      switch (e.Key)
      {
        case Key.Enter:
          {
            Cell_DoubleClick(null, null);
            blnHandled = true;
            break;
          }
      }
      e.Handled = blnHandled;
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {

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

    #region txtMaxBooks_PreviewTextInput
    /// <summary>
    ///   Valida que solo se puedan usar números
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    private void txtMaxBooks_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !ValidateHelper.OnlyNumbers(e.Text);
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
        dgr.CommitEdit();
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
                status.Visibility = Visibility.Visible;
                txtStatus.Text = "Saving Data...";
                ttbylssr.ttls = cboLeadSource.SelectedValue.ToString();
                ttbylssr.ttsr = cboSalesRoom.SelectedValue.ToString();
                nRes = await BREntities.OperationEntity(ttbylssr, EnumMode.add);
                dgr.Items.Refresh();
              }
              else
              {
                dgr.CancelEdit();
                dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
              }        
            }
            else {
              status.Visibility = Visibility.Visible;
              txtStatus.Text = "Saving Data...";
              nRes = await BREntities.OperationEntity(ttbylssr, EnumMode.edit); }
            break;
          #endregion

          #region TourTimesByDay
          case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
            TourTimeByDay ttbylssrwd = e.Row.DataContext as TourTimeByDay;
            if (isInsert)
            {
              if (Validate(ttbylssrwd))
              {
                status.Visibility = Visibility.Visible;
                txtStatus.Text = "Saving Data...";
                ttbylssrwd.ttls = cboLeadSource.SelectedValue.ToString();
                ttbylssrwd.ttsr = cboSalesRoom.SelectedValue.ToString();
                ttbylssrwd.ttDay = Convert.ToByte(cboWeekDay.SelectedValue.ToString());
                nRes = await BREntities.OperationEntity(ttbylssrwd, EnumMode.add);
                dgr.Items.Refresh();
              }
              else
              {
                dgr.CancelEdit();
                dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
              }
            }
            else {
              status.Visibility = Visibility.Visible;
              txtStatus.Text = "Saving Data...";
              nRes = await BREntities.OperationEntity(ttbylssrwd, EnumMode.edit); };
            break;
          #endregion

          #region TourTimeBySalesRoomWeekDay
          case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
            TourTimeBySalesRoomWeekDay ttbysrwd = e.Row.DataContext as TourTimeBySalesRoomWeekDay;
            if (isInsert)
            {
              if (Validate(ttbysrwd))
              { 
                status.Visibility = Visibility.Visible;
                txtStatus.Text = "Saving Data...";
                ttbysrwd.ttsr = cboSalesRoom.SelectedValue.ToString();
                ttbysrwd.ttDay = Convert.ToByte(cboWeekDay.SelectedValue.ToString());
                nRes = await BREntities.OperationEntity(ttbysrwd, EnumMode.add);
                dgr.Items.Refresh();
              }
              else
              {
                dgr.CancelEdit();
                dgr.RowEditEnding += dgrTourTimes_RowEditEnding;
              }
            }
            else {
              status.Visibility = Visibility.Visible;
              txtStatus.Text = "Saving Data...";
              nRes = await BREntities.OperationEntity(ttbysrwd, EnumMode.edit); };
            break;
            #endregion
        }
        status.Visibility = Visibility.Collapsed;
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
      if (_blnEdit)
      {
        if ((e.Column.Header.ToString() == "Tour Time" || e.Column.Header.ToString() == "Pickup Time") && !e.Row.IsNewItem)
        {
          e.Cancel = true;       
        }
        else
        {
          isEdit = true;
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
          case "Tour Time":
            string tt = changedTextBox.Text.ToString();
            if (!ValidateHelper.IsValidTimeFormat(tt))
            {
              isCancel = true;
              UIHelper.ShowMessage("Invalid time", MessageBoxImage.Warning, "IM.Administrator");
            }
            else
            {
              DateTime time = Convert.ToDateTime(tt);
              if (ExistTime(time)) { isCancel = true; UIHelper.ShowMessage(time.ToString("hh:mm tt") + " already exists", MessageBoxImage.Warning, "IM.Administrator"); }
            }
            break;
          case "Pickup Time":
            string pckUp = changedTextBox.Text.ToString();
            if (!ValidateHelper.IsValidTimeFormat(pckUp))
            {
              isCancel = true;
              UIHelper.ShowMessage("Invalid time", MessageBoxImage.Warning, "IM.Administrator");
            }
            break;
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
                UIHelper.ShowMessage("can not be greater than 255", MessageBoxImage.Warning, "IM.Administrator");
              else if (Convert.ToInt32(maxBooks) < 0)
                UIHelper.ShowMessage("can not be lower than 0", MessageBoxImage.Warning, "IM.Administrator");
            }
            break;
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
    private void btnCopyToLeadSource_Click(object sender, RoutedEventArgs e)
    {

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
    private void btnCopyToSalesRoom_Click(object sender, RoutedEventArgs e)
    {

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
    private void btnCopyToSalesRoomsWeekDaysOfLeadSource_Click(object sender, RoutedEventArgs e)
    {

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
    private void btnCopyToLeadSourcesOfProgram_Click(object sender, RoutedEventArgs e)
    {

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
    private void btnCopyToWeekDaysOfSalesRoom_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion

    #endregion

    #endregion

    private void dgrTourTimes_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      DataGrid grid = (DataGrid)sender;

      if (e.Key == Key.Enter || e.Key == Key.Return)
      {
        // get the selected row
        var selectedRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;

        // selectedRow can be null due to virtualization
        if (selectedRow != null)
        {
          // there should always be a selected cell
          if (grid.SelectedCells.Count != 0)
          {
            // get the cell info
            DataGridCellInfo currentCell = grid.SelectedCells[0];

            // get the display index of the cell's column + 1 (for next column)
            int columnDisplayIndex = currentCell.Column.DisplayIndex++;

            // if display index is valid
            if (columnDisplayIndex < grid.Columns.Count)
            {
              // get the DataGridColumn instance from the display index
              DataGridColumn nextColumn = grid.ColumnFromDisplayIndex(columnDisplayIndex);

              // now telling the grid, that we handled the key down event
              e.Handled = true;

              // setting the current cell (selected, focused)
              grid.CurrentCell = new DataGridCellInfo(grid.SelectedItem, nextColumn);

              // tell the grid to initialize edit mode for the current cell
              grid.BeginEdit();
            }
          }
        }
      }
    }
  }
}
