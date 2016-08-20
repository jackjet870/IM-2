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
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Windows.Input;

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
    private int _dayLimit;
    public static decimal _exchangeRateMEX;
    #endregion

    #region CONSTRUCTORES
    /// <summary>
    /// 
    /// </summary>
    /// <history>
    /// [vipacheco] 10/Agosto/2016 Modified -> Se eliminaron parametros y se optimizo las consultas.
    /// </history>
    public frmExchangeRate()
    {
      InitializeComponent();

      // Asignamos fecha del servidor
      calDate.SelectedDate = frmHost.dtpServerDate;

      // Asignamos el mes y dia inicial a las variables correspondientes.
      _monthCurrent = calDate.SelectedDate.Value.Month;
      _dayCurrent = calDate.SelectedDate.Value.Day;

      //Validamos permisos del usuario
      if (!App.User.HasPermission(EnumPermission.ExchangeRates, EnumPermisionLevel.Standard))
      {
        // Ocultamos los botones necesarios.
        btnAdd.IsEnabled = false;
        btnEdit.IsEnabled = false;
      }
    }
    #endregion

    #region calDate_SelectedDatesChanged
    /// <summary>
    /// Función encargado de evaluar el cambio en el Calendar del formulario
    /// </summary>
    /// <history>
    /// [vipacheco] 04/03/2016 Created
    /// </history>
    private void calDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
    {
      // Validamos el rango de dias!
      txtRange.Text = string.Format("Days Available : {0} to {1}", 1, validateRange(calDate.SelectedDate));

      //Obtenemos el Date
      txtDate.Text = string.Format("Date: {0} {1}", calDate.SelectedDate.Value.DayOfWeek, calDate.SelectedDate.Value.Day);

      //Cargamos la información con las nuevas fechas!
      List<ExchangeRateData> lstExchangeRates = BRExchangeRate.GetGetExchangeRatesWithPesosByDate(calDate.SelectedDate.Value.Date);
      _exchangeRateMEX = lstExchangeRates.Where(x => x.excu == "MEX").Select(s => s.exExchRate).First();

      // Iniciamos las collectiones de los recursos.
      CollectionViewSource dsExchangeRates = ((CollectionViewSource)(FindResource("dsExchangeRates")));
      dsExchangeRates.Source = lstExchangeRates;

    }
    #endregion

    #region calDate_Loaded
    /// <summary>
    /// Función load del calendario que se utiliza para iniciar valores y rangos de fechas disponibles
    /// </summary>
    /// <history>
    /// [vipacheco] 05/03/2016 Created
    /// </history>
    private void calDate_Loaded(object sender, RoutedEventArgs e)
    {
      //Obtenemos el rango inicial de dias disponibles a evaluar segun el mes.
      txtRange.Text = string.Format("Days Available : {0} to {1}", 1, calDate.SelectedDate.Value.Day);

      //Obtenemos el Date
      txtDate.Text = string.Format("Date: {0} {1}", calDate.SelectedDate.Value.DayOfWeek, calDate.SelectedDate.Value.Day);

      //Agregamos el rango de dias disponibleas.
      calDate.DisplayDateStart = new DateTime(1990, calDate.SelectedDate.Value.Month, 1);
      calDate.DisplayDateEnd = new DateTime(calDate.SelectedDate.Value.Year, calDate.SelectedDate.Value.Month, calDate.SelectedDate.Value.Day);

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
    /// <history>
    /// [vipacheco] 05/03/2016 Created
    /// </history>
    private void btnLog_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      ExchangeRateData itemRow = grdExchangeRate.SelectedItem as ExchangeRateData;

      if (itemRow != null)
      {
        frmExchangeRateLog _frmExchangeRateLog = new frmExchangeRateLog(itemRow.excu) { Owner = this };
        _frmExchangeRateLog.Title += frmHost._lstCurrencies.Where(x => x.cuID == itemRow.excu).Select(s => s.cuN).Single();
        _frmExchangeRateLog.ShowDialog();
      }
      else
      {
        UIHelper.ShowMessage("Select an exchange rate", MessageBoxImage.Information);
      }
    }
    #endregion

    #region btnEdit_Click
    /// <summary>
    /// Función que inhabilita el row MEXICAN
    /// </summary>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      // Habilitamos los botones correspondientes.
      btnCancel.IsEnabled = true;
      btnEdit.IsEnabled = false;
    }
    #endregion

    #region btnCancel_Click
    /// <summary>
    /// Cancela una edicion
    /// </summary>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      // Deshabilitamos los botones correspondientes.
      btnEdit.IsEnabled = true;
      btnCancel.IsEnabled = false;
    }
    #endregion

    #region btnAdd_Click
    /// <summary>
    /// Funcion que muestra el dialogo para agregar un nuevo currency a la lista de Exchange Rate
    /// </summary>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void btnAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      // Se verifica si esta en modo edicion
      if (!btnEdit.IsEnabled)
      {
        UIHelper.ShowMessage("This form is currently in edit mode. Please save or undo your changes before closing it.", MessageBoxImage.Information);
        return;
      }

      List<Currency> lstDistict = frmHost._lstCurrencies.Where(x => !grdExchangeRate.ItemsSource.OfType<ExchangeRateData>().Select(s => s.excu).ToList().Contains(x.cuID) && x.cuID != "US").ToList();
      if (lstDistict.Any())
      {
        // Mandamos ejecutar el formulario para agregar nuevo Exchange Rate
        ExchangeRateData exchangeCurrent = grdExchangeRate.SelectedItem as ExchangeRateData;
        frmExchangeRateDetail frmExchangeEdit = new frmExchangeRateDetail(EnumMode.Add) { Owner = this };
        frmExchangeEdit.lstCurrencies = lstDistict;

        // Si se agregó un nuevo item
        if (frmExchangeEdit.ShowDialog().Value)
        {
          //Actualizamos el Data Source.
          calDate_SelectedDatesChanged(null, null);
        }
      }
      // Todos los currency estan agregados
      else { UIHelper.ShowMessage("All currencies already added", MessageBoxImage.Information); }
    }
    #endregion

    #region Window_Loaded
    /// <summary>
    /// Función Load del Window
    /// </summary>
    /// <history>
    /// [vipacheco] 10/03/2016 Created
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// [vipacheco] 10/Agosto/2016 Modified -> Se optimizo la consulta y se elimino la asincronia porque ya no se utilizaba.
    /// </history>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      CollectionViewSource dsCurrencies = ((CollectionViewSource)(FindResource("dsCurrencies")));
      //Obtenemos la lista de Currencies, excluyendo los US
      dsCurrencies.Source = frmHost._lstCurrencies.Where(x => x.cuID != "US").ToList();
    }
    #endregion

    #region Cell_DoubleClick    
    /// <summary>
    /// Despliega el formulario de detalles
    /// </summary>
    /// <history>
    /// [vipacheco] 03/14/2016 Created
    /// </history>
    private void Cell_DoubleClick(object sender, RoutedEventArgs e)
    {
      //Verificamos que este en modo Edición!
      if (!btnEdit.IsEnabled)
      {
        // Construimos el formulario a mostrar
        ExchangeRateData exchangeCurrent = grdExchangeRate.SelectedItem as ExchangeRateData;

        // Verificamos que no sea currency MEX
        if (!exchangeCurrent.excu.Equals("MEX"))
        {
          frmExchangeRateDetail frmExchangeEdit = new frmExchangeRateDetail(EnumMode.Edit) { Owner = this };
          // clonamos las propiedades del ExchangeRateData a editar
          frmExchangeEdit.exchangeDate = ObjectHelper.CopyProperties(exchangeCurrent);

          if (frmExchangeEdit.ShowDialog().Value)
          {
            // Si son diferentes
            if (!ObjectHelper.IsEquals(frmExchangeEdit.exchangeDate, exchangeCurrent))
            {
              //Recargamos el datagrid!
              calDate_SelectedDatesChanged(null, null);
            }
          }
        }
      }
    }
    #endregion


    #region grdReceipts_KeyDown
    /// <summary>
    /// abre la ventana detalle con el boton enter
    /// cambia de fila con el boton tab
    /// </summary>
    /// <history>
    /// [vipacheco] 16/Agosto/2016 Created
    /// </history>
    private void grdExchangeRate_KeyDown(object sender, KeyEventArgs e)
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

    #region calDate_DisplayDateChanged
    /// <summary>
    /// Actualiza la informacion cuando se cambia la fecha por los botones del Calendar
    /// </summary>
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
