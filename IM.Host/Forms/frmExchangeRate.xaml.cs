using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using System.Collections.Generic;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Classes;
using IM.Base.Helpers;
using System.Windows.Media;

namespace IM.Host.Forms
{
  /// <summary>
  /// Interaction logic for frmExchangeRate.xaml
  /// </summary>
  /// <history>
  /// [vipacheco] 04/03/2016 Created
  /// </history>
  public partial class frmExchangeRate : Window
  {
    #region VARIABLES
    private int _monthCurrent;
    private int _dayCurrent;
    private string _currencyID;
    private int _dayLimit;
    private DateTime? _dtServer;
    public static decimal _exchangeRateMEX;
    List<Currency> _listCurrencies = new List<Currency>();
    List<ExchangeRateData> _listExchageRateData = new List<ExchangeRateData>();
    CollectionViewSource _dsExchangeRates;
    CollectionViewSource currencyViewSource;
    #endregion

    #region CONSTRUCTORES
    public frmExchangeRate(DateTime? dtServer)
    {
      _dtServer = dtServer;

      InitializeComponent();

      // Iniciamos las collectiones de los recursos.
      _dsExchangeRates = ((CollectionViewSource)(this.FindResource("cvsExchangeRates")));
      currencyViewSource = ((CollectionViewSource)(this.FindResource("currencyViewSource")));

      // Asignamos fecha del servidor
      calDate.SelectedDate = dtServer;

      // Asignamos el mes y dia inicial a las variables correspondientes.
      _monthCurrent = calDate.SelectedDate.Value.Month;
      _dayCurrent = calDate.SelectedDate.Value.Day;

      //Validamos permisos del usuario
      validateUserPermissions(App.User);
    }
    #endregion

    #region calDate_SelectedDatesChanged
    /// <summary>
    /// Función encargado de evaluar el cambio en el Calendar del formulario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 04/03/2016 Created
    /// </history>
    private void calDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
    {
      // Validamos el rango de dias!
      txtRange.Text = String.Format("Days Available : {0} to {1}", 1, validateRange(calDate.SelectedDate));

      //Obtenemos el Date
      txtDate.Text = String.Format("Date: {0} {1}", calDate.SelectedDate.Value.DayOfWeek, calDate.SelectedDate.Value.Day);

      //Cargamos la información con las nuevas fechas!
      _listExchageRateData = BRExchangeRate.GetGetExchangeRatesWithPesosByDate(calDate.SelectedDate.Value.Date);
      _exchangeRateMEX = _listExchageRateData.Where(x => x.excu == "MEX").Select(s => s.exExchRate).First();
      _dsExchangeRates.Source = _listExchageRateData;

    } 
    #endregion

    #region calDate_Loaded
    /// <summary>
    /// Función load del calendario que se utiliza para iniciar valores y rangos de fechas disponibles
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 05/03/2016 Created
    /// </history>
    private void calDate_Loaded(object sender, RoutedEventArgs e)
    {
      //Obtenemos el rango inicial de dias disponibles a evaluar segun el mes.
      txtRange.Text = String.Format("Days Available : {0} to {1}", 1, calDate.SelectedDate.Value.Day);

      //Obtenemos el Date
      txtDate.Text = String.Format("Date: {0} {1}", calDate.SelectedDate.Value.DayOfWeek, calDate.SelectedDate.Value.Day);

      //Agregamos el rango de dias disponibleas.
      calDate.DisplayDateStart = new DateTime(1990, calDate.SelectedDate.Value.Month, 1); //   Convert.ToDateTime(String.Format("{0}/{1}/{2}", calDate.SelectedDate.Value.Month, 01, 1990));
      calDate.DisplayDateEnd = new DateTime(calDate.SelectedDate.Value.Year, calDate.SelectedDate.Value.Month, calDate.SelectedDate.Value.Day); // Convert.ToDateTime(String.Format("{0}/{1}/{2}", calDate.SelectedDate.Value.Month, calDate.SelectedDate.Value.Day, calDate.SelectedDate.Value.Year));

      // Obtenemos los limites actuales
      _dayLimit = calDate.SelectedDate.Value.Day;
    } 
    #endregion

    #region validateRange
    /// <summary>
    /// Función encargada de evaluar el rango de dias por mes para mostrar en el textbox
    /// </summary>
    /// <param name="dateSelected"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 05/03/2016 Created
    /// </history>
    private int validateRange(DateTime? dateSelected)
    {
      if (dateSelected.Value.Month == _monthCurrent)
      {
        return _dayCurrent;
      }
      return DateTime.DaysInMonth(dateSelected.Value.Year, dateSelected.Value.Month);
    } 
    #endregion

    #region btnLog_Click
    /// <summary>
    /// Función evento click encargado llamar al formulario Log
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 05/03/2016 Created
    /// </history>
    private void btnLog_Click(object sender, RoutedEventArgs e)
    {
      frmExchangeRateLog _frmExchangeRateLog = new frmExchangeRateLog(_currencyID);
      _frmExchangeRateLog.Title += _currencyID;
      _frmExchangeRateLog.ShowInTaskbar = false;
      _frmExchangeRateLog.Owner = this;
      _frmExchangeRateLog.ShowDialog();
    } 
    #endregion

    #region dgExchangeRates_SelectionChanged
    /// <summary>
    /// Función para actualizar el currency utilizado al invocar el formulario log.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 05/03/2016 Created
    /// </history>
    private void dgExchangeRates_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // Obtenemos y casteamos el row seleccionado.
      ExchangeRateData itemRow = (ExchangeRateData)dgExchangeRates.SelectedItem;

      // Se asigna el tipo de currency segun el row seleccionado.
      _currencyID = itemRow.excu;
    } 
    #endregion

    #region validateUserPermissions
    /// <summary>
    /// Función para validar los permisos y opciones a mostrar.
    /// </summary>
    /// <param name="userData"></param>
    /// <history>
    /// [vipacheco] 07/03/2016 Created
    /// </history>
    private void validateUserPermissions(UserData userData)
    {
      if (userData.Permissions.Exists(c => c.pppm == "EXCHRATES" && c.pppl == 1))
      {
        // Ocultamos los botones necesarios.
        btnAdd.Visibility = Visibility.Collapsed;
        btnEdit.Visibility = Visibility.Collapsed;
        return;
      }
    }
    #endregion

    #region btnEdit_Click
    /// <summary>
    /// Función que inhabilita el row MEXICAN
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
      // Habilitamos los botones correspondientes.
      btnCancel.IsEnabled = true;
      btnEdit.IsEnabled = false;

      string valor = "";

      // Deshabilitamos la opcion MEXICAN PESOS
      foreach (var item in dgExchangeRates.Items)
      {
        DataGridRow row = (DataGridRow)dgExchangeRates.ItemContainerGenerator.ContainerFromItem(item);

        if (dgExchangeRates.Columns[0].GetCellContent(row) is ComboBox)
        {
          valor = ((ComboBox)dgExchangeRates.Columns[0].GetCellContent(row)).SelectedValue.ToString();

          if (valor.Equals("MEX"))
          {
            row.IsEnabled = false;
            ((ComboBox)dgExchangeRates.Columns[0].GetCellContent(row)).Foreground = new SolidColorBrush(Colors.Black);
            ((TextBlock)dgExchangeRates.Columns[2].GetCellContent(row)).Foreground = new SolidColorBrush(Colors.Black);
          }
          
        }
      }
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cancela una edicion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      // Deshabilitamos los botones correspondientes.
      btnEdit.IsEnabled = true;
      btnCancel.IsEnabled = false;
      exExchRateColumn.Foreground = new SolidColorBrush(Colors.Black);
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    /// Funcion que muestra el dialogo para agregar un nuevo currency a la lista de Exchange Rate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      // Se verifica si esta en modo edicion
      if (!btnEdit.IsEnabled)
      {
        UIHelper.ShowMessage("This form is currently in edit mode. Please save or undo your changes before closing it.", MessageBoxImage.Information);
        return;
      }

      // verificamos que contengan Currencies por agregar
      if (_listExchageRateData.Count == _listCurrencies.Count)
      {
        UIHelper.ShowMessage("Empty list of currencies", MessageBoxImage.Information);
        return;
      }

      // Mandamos ejecutar el formulario para agregar nuevo Exchange Rate
      frmAddExchangeRate _frmNewExchangeRate = new frmAddExchangeRate(_listExchageRateData.Select(x => x.excu).ToList(), _exchangeRateMEX);
      _frmNewExchangeRate.ShowInTaskbar = false;
      _frmNewExchangeRate.Owner = this;
      _frmNewExchangeRate.ShowDialog();

      //Actializamos el Data Source.
      calDate_SelectedDatesChanged(null, null);
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Función Load del Window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 10/03/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      List<string> _exceptCurency = new List<string> { "US" };
      //Obtenemos la lista de Currencies
      _listCurrencies = await BRCurrencies.GetCurrencies(null, 1, _exceptCurency);
      currencyViewSource.Source = _listCurrencies;
    }
    #endregion

    #region Cell_DoubleClick    
    /// <summary>
    /// Despliega el formulario de detalles
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      //Verificamos que este en modo Edición!
      if (!btnEdit.IsEnabled)
      {
        // Obtenemos el row seleccionado!
        DataGridRow row = sender as DataGridRow;

        // Construimos el formulario a mostrar
        ExchangeRateData _exchangeRateRow = (ExchangeRateData)row.DataContext;
        frmExchangeRateEdit frmExchangeEdit = new frmExchangeRateEdit(_exchangeRateRow);
        frmExchangeEdit.ShowInTaskbar = false;
        frmExchangeEdit.Owner = this;
        frmExchangeEdit.ShowDialog();

        //Recargamos el datagrid!
        calDate_SelectedDatesChanged(null, null);
      }
    }
    #endregion

    #region calDate_DisplayDateChanged
    /// <summary>
    /// Actualiza la informacion cuando se cambia la fecha por los botones del Calendar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [vipacheco] 14/Julio/2016 Created
    /// </history>
    private void calDate_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
    {
      int day = calDate.SelectedDate.Value.Day;

      if (day > _dayLimit)
        calDate.SelectedDate = e.AddedDate.Value.AddDays(_dayLimit - 1);
      else
        calDate.SelectedDate = e.AddedDate.Value.AddDays(day - 1);
    } 
    #endregion

  }
}
