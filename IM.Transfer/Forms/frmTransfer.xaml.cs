using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Services.Helpers;
using IM.Services.HotelService;
using IM.Services.IntranetService;
using PalaceResorts.Common.PalaceTools;
using System.Threading;
using System.ComponentModel;
using IM.Model;

namespace IM.Transfer.Forms

{
    
    /// <summary>
    /// Interaction logic for frmTransfer.xaml
    /// </summary>
    ///
    /// 
    public partial class frmTransferLauncher : Window
    {
        #region Atributos
        
        private static TimeSpan _stratTimeT;//Hora inicial del proceso de transferencia de reservaciones
        private static TimeSpan _endTimeT;//Hora final del proceso de transferencia de reservaciones
        private static TimeSpan _intervalTimeT;//Intervalo de tiempo del proceso de transferencia de reservaciones
        private static int _daysBeforeDAY;//Numero de dias anteriores al dia de hoy para obtener reservaciones
        private static int _daysAfterDAY;//Numero de dias posteriores al dia de hoy para obtener reservaciones
        private static int _retrys;//Numero de reintentos
        private static string _timeOutT;//Tiempo de espera del proceso 7,200,000 = 2 horas
        private static string _timeOutWebServiceT;//Tiempo de espera del servicio web 300,000 = 5 minutos
        private static TimeSpan _standbyIntervalTime; //Tiempo de espera para verificar el tiempo de ejecucion de la transferencia = 5 segundos
        public static DateTime _dtpServerDate = new DateTime();
        public static string _currencyId;
        public static DateTime _dateToday;
        public static decimal _exchangeRateType;
        private static TimeSpan _tranferExchangeRatesStartTime;//Hora inicial del proceso de actualización de tipos de cambio
        private static TimeSpan _tranferExchangeRatesEndTime;//Hora final del proceso de actualización de tipos de cambio
        private static TimeSpan _tranferExchangeRatesIntervalTime;//Intervalo de tiempo del proceso de actualización de tipos de cambio
        private static DateTime _lastExchangeRate;//Ultima hora que se ejecuto el proceso de actualización de tipos de cambio
        public static List<LogHelper.Transaction> listTransactions = new List<LogHelper.Transaction>();
        public readonly BackgroundWorker WorkerExchangeRate;
        public static string lblTextLastExchangeRage = "Last Transfer Exchange Rate ";
        public static string lblTextLastReservations = "Last Transfer Reservations ";


    #endregion

        #region Contructores y Destructores
        public frmTransferLauncher()
            {
                InitializeComponent();
                InitializeValuesParameters();
                WorkerExchangeRate = new BackgroundWorker();
                WorkerExchangeRate.WorkerReportsProgress = true;
                WorkerExchangeRate.WorkerSupportsCancellation = true;
                WorkerExchangeRate.DoWork += WorkerUpdateExchangeRateDoWork;
                WorkerExchangeRate.RunWorkerCompleted += WorkerExchangeRateRunWorkerCompleted;
                WorkerExchangeRate.ProgressChanged += new ProgressChangedEventHandler(progresBarTransfer_Changed);

            }

        #endregion

    #region Metodos para transferencias

    #region InitializeValuesParameters
    ///<summary>Metodo que inicializa los parametros y configuracion de las transferencias</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private void InitializeValuesParameters() {
              //Se inicializan los valores de los parametros para la transferencia de reservaciones.
              _stratTimeT = TimeSpan.Parse(ConfigHelper.GetString("StartTime"));
              _endTimeT = TimeSpan.Parse(ConfigHelper.GetString("EndTime"));
              _intervalTimeT = TimeSpan.Parse(ConfigHelper.GetString("IntervalTime"));
              _daysBeforeDAY = Convert.ToInt32(ConfigHelper.GetString("DaysBefore"));
              _daysAfterDAY = Convert.ToInt32(ConfigHelper.GetString("DaysAfter"));
              _retrys = Convert.ToInt32(ConfigHelper.GetString("Retrys"));
              _timeOutT = ConfigHelper.GetString("TimeOut");
              _timeOutWebServiceT = ConfigHelper.GetString("TimeOutWebService");
              _standbyIntervalTime = TimeSpan.Parse(ConfigHelper.GetString("StandbyIntervalTime"));

              _dateToday = DateTime.Now;

              //se inicializan los parametros para ejecutar ExchangeRate
              _tranferExchangeRatesStartTime = TimeSpan.Parse(ConfigHelper.GetString("TranferExchangeRatesStartTime"));
              _tranferExchangeRatesEndTime = TimeSpan.Parse(ConfigHelper.GetString("TranferExchangeRatesEndTime"));
              _tranferExchangeRatesIntervalTime = TimeSpan.Parse(ConfigHelper.GetString("TranferExchangeRatesIntervalTime"));
              _lastExchangeRate = DateTime.Now;
            
          }
    #endregion

    #region InitializeExchangeRate
    ///<summary>Metodo que inicializa y ejecuta cuando se cumple la validación para la actualización del Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void InitializeExchangeRate() {
              //INSTANCIANDO EL TIMER CON LA CLASE DISPATCHERTIMER 
              DispatcherTimer dispathcerT = new DispatcherTimer();
              //EL INTERVALO DEL TIMER ES DE HORAS, MINUTOS Y SEGUNDOS QUE SE PASAN COMO PARAMETRO 
              dispathcerT.Interval = _standbyIntervalTime;//new TimeSpan(0, 0, Convert.ToInt32(_standbyIntervalTime));
              //EL EVENTO TICK SE SUBSCRIBE A UN CONTROLADOR DE EVENTOS UTILIZANDO LAMBDA 
              dispathcerT.Tick += (s, a) =>
              {
                  //ACCION QUE SE DETONA CUANDO YA TRANSCURRIERON LOS SEGUNDOS ESTABLECIDOS
                  dispathcerT.Stop();
                  DateTime _currentTime = DateTime.Now;
              
                  if (!DateHelper.isDateEquals(_currentTime, _lastExchangeRate))
                  {
                      _lastExchangeRate = DateTime.Now;
                  }                
                
                  if (DateHelper.IsRangeTime(_currentTime, _lastExchangeRate) && DateHelper.IsRangeHours(_currentTime.TimeOfDay, _tranferExchangeRatesStartTime, _tranferExchangeRatesEndTime))
                  {  
                      WorkerExchangeRate.RunWorkerAsync();
                      _lastExchangeRate = DateHelper.AddTimeDate(_tranferExchangeRatesIntervalTime);
                  }
                  dispathcerT.Start();

              };
              dispathcerT.Start();
            
          }
    #endregion


    private static void InitializerReservations()
        {
            //INSTANCIANDO EL TIMER CON LA CLASE DISPATCHERTIMER 
            DispatcherTimer dispathcerT = new DispatcherTimer();
            //EL INTERVALO DEL TIMER ES DE HORAS, MINUTOS Y SEGUNDOS QUE SE PASAN COMO PARAMETRO 
            dispathcerT.Interval = _standbyIntervalTime;//new TimeSpan(0, 0, Convert.ToInt32(_standbyIntervalTime));
            //EL EVENTO TICK SE SUBSCRIBE A UN CONTROLADOR DE EVENTOS UTILIZANDO LAMBDA 
            dispathcerT.Tick += (s, a) =>
            {
                //ACCION QUE SE DETONA CUANDO YA TRANSCURRIERON LOS SEGUNDOS ESTABLECIDOS

                DateTime _currentTime = DateTime.Now;

                
                //DateTime.Today.AddDays(-20);
                /*if (IsRangeTime(_currentTime, _intervalTimeT) && IsRangeHours(_currentTime.TimeOfDay, _stratTimeT, _endTimeT))
                {
                    MessageBox.Show(_standbyIntervalTime.ToString(), "Minutis");
                    dispathcerT.Stop();
                }*/
            };
            dispathcerT.Start();

        }

    #region InitializerProgressBar
    ///<summary>Metodo que inicializa y reestablece los valores de la barra de progreso</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void InitializerProgressBar()
        {
            //Configuración del ProgressBar
            progresBarTransfer.Minimum = 0;//valor mínimo (inicio de la barra de carga)
            progresBarTransfer.Maximum = 100;//valor máximo(hasta donde se carga, como ejemplo 100)
            progresBarTransfer.Value = 0;//valor de inicio
            textStatusProgresBar.Text = String.Format("{0:0%}", 0);


        }
    #endregion

    #region LoadDataGRidXML
    ///<summary>Metodo que carga e inicializa el grid del log de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private void LoadDataGRidXML()
        { 
            DateTime dateTo = DateHelper.DaysBeforeOrAfter(_daysBeforeDAY);

            /*Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                    new Action(
                        () =>
                        {*/
                            Task.Factory.StartNew(() => 
                            listTransactions = LogHelper.LoadHistoryLog("ExchangeRate", DateTime.Now, dateTo))
                                .ContinueWith(
                                    (task1) =>
                                        {
                                            if (task1.IsFaulted)
                                            {
                                                UIHelper.ShowMessage(task1.Exception.InnerException.Message, MessageBoxImage.Error);
                                                return false;
                                            }
                                            else
                                            {
                                                if (task1.IsCompleted)
                                                {
                                                    task1.Wait(1000);
                                   
                                                    if (listTransactions.Count > 0)
                                                    {
                                                        _lastExchangeRate = listTransactions[listTransactions.Count - 1].Date;
                                                        grdLogTransfer.ItemsSource = listTransactions;
                                                        grdLogTransfer.Items.Refresh();//Refrescamos el grid
                                                        //GridHelper.SelectRow(grdLogTransfer, grdLogTransfer.Items.Count);


                                                    }
                                                }
                            
                                                return false;
                                            }
                                        },
                                      TaskScheduler.FromCurrentSynchronizationContext()
                                    );
                       /* }
                    )
                 );*/
        }
    #endregion

    public void DoReservations()
    {
      List<ZoneTransfer> zoneTransfer = BRZones.GetZonesTransfer();
      string zoneID = "";
      string zoneName = "";
      string zoneHotel = "";
      if (zoneTransfer.Count <= 0)
      {
        
      }
      
      foreach (ZoneTransfer zone in zoneTransfer)
      {
        zoneID = zone.znID.ToString();
        zoneName = zone.znN.ToString();
        zoneHotel = zone.znZoneHotel.ToString();
        ExportToTransfer(zoneID, zoneName, zoneHotel);


        Console.WriteLine("ID: " + zoneID + "\t " + "Zone Name: " + zoneName + "\t" + "Zone Hotel: " + zoneHotel + "\t" + "\t");
      }

    }

    public void ExportToTransfer(string zoneID, string zoneName, string zoneHotel)
    {
      GetReservations(zoneID, zoneName, zoneHotel);
    }

    public void GetReservations(string zoneID, string zoneName, string zoneHotel)
    {
      List<LeadSourceShort> hotels =  BRLeadSources.GetLeadSourcesByZoneBoss(zoneID);
      DateTime dateFrom =  DateHelper.DaysBeforeOrAfter(_daysBeforeDAY);
      DateTime dateTo = DateHelper.DaysBeforeOrAfter(_daysAfterDAY);
      string leadSourceID = StringIDHoteles(hotels);


      if (hotels.Count <= 0 )
      {}
      ReservationOrigosTransfer reservations = HotelService.GetReservationsByArrivalDate(zoneHotel, dateFrom, dateTo, leadSourceID);
      if( reservations != null )
      {
       
        
        
      }

    }

    public string StringIDHoteles(List<LeadSourceShort> hotels)
    {
      string leadSourceId = string.Empty;
      foreach (LeadSourceShort hotel in hotels)
      {
        Console.WriteLine("ID: " + hotel.lsID + "\t " + "Zone Name: " + hotel.lsN + "\t" + "\t" + "\t");
        leadSourceId += hotel.lsID + ",";
      }
      leadSourceId = leadSourceId.Remove(leadSourceId.Length - 1);
      return leadSourceId;
    }

    private void UpdateExchangeRates()
        {
            DateTime startExchangeRate = DateTime.Now;
            DateTime endExchangeRate;            
            //Se obtiene la fecha del servidor
            _dtpServerDate = BRHelpers.GetServerDate();
            AddItemDataGridExchangeRate(DateTime.Now, "Start", "Updating Exchange Rates");

            //agregamos los tipos de cambio faltantes hasta la fecha actual para que no existan huecos
            BRExchangeRate.InsertExchangeRate(_dtpServerDate);
               
            AddItemDataGridExchangeRate(DateTime.Now, "Info", "Getting Exchange Rate from Intranet Service");

            //obtenemos el tipo de cambio de la Intranet
            TipoCambioTesoreria exchangeRate = IntranetHelper.TipoCambioTesoreria(Convert.ToDateTime("01/08/2008"), "USD");
            if (exchangeRate != null)
            {
                _exchangeRateType = Convert.ToDecimal(exchangeRate.TipoCambio);
                //comparamos si el cambio es positivo y mayor a cero
                if (_exchangeRateType > 0)
                {
                    _currencyId = "MX";
                    //Se realiza la actualizacion y se registra en el log la inicialización
                    try
                    {
                        AddItemDataGridExchangeRate(DateTime.Now, "Info", "Updating Exchange Rate " + _currencyId + " (" + _exchangeRateType.ToString() + ")");
                        BRExchangeRate.UpdateExchangeRate(_dtpServerDate, _currencyId, _exchangeRateType);
                        endExchangeRate = DateTime.Now;

                        AddItemDataGridExchangeRate(DateTime.Now, "Info", "Updating Exchange Rate in " + DateHelper.timeDuration(startExchangeRate, endExchangeRate).ToString(@"hh\ \h\ \:mm\ \m\ \:ss\ \s\ \:fff\ \m\s"));

                        AddItemDataGridExchangeRate(DateTime.Now, "Success", "Process finished");
                    }
                    catch (Exception error)
                    {
                        // Registramos en el log el tipo de error que se encontro
                        AddItemDataGridExchangeRate(DateTime.Now, "Error", error.ToString());
                    }
                }
                else
                {
                    // Registramos en el log el tipo de error que se encontro, en el tipo de cambio
                    AddItemDataGridExchangeRate(DateTime.Now, "Error", "Exchange rate must be positive " + _currencyId + " (" + _exchangeRateType.ToString() + ")");
                }
            }
            else
            {
                // Registramos en el log que no existen cambios en la fecha especificada y el tipo de moneda
                AddItemDataGridExchangeRate(DateTime.Now, "Warning", "Exchange rate does not exists for day");
            }
                        

        }


        public void AddItemDataGridExchangeRate(DateTime date, string logLevel, string message)
        {
            /*Application.Current.Dispatcher.Invoke(
               DispatcherPriority.Background,
                   new Action(
                       () =>
                       {*/
            
                    listTransactions.Add(LogHelper.AddTransaction("ExchangeRate", date, logLevel, message));
                    grdLogTransfer.ItemsSource = null;
                    grdLogTransfer.ItemsSource = listTransactions;
                    //grdLogTransfer.Items.Add(Log.AddTransaction("ExchangeRate", date, logLevel, message));
                    grdLogTransfer.Items.Refresh();//Refrescamos el grid
                    GridHelper.SelectRow(grdLogTransfer, grdLogTransfer.Items.Count);


            

            /*}
         )
     );*/




        }

    #region AddValueProgressBar
    ///<summary>Metodo que actualiza la barra de progreso</summary>
    ///<param name="value">recibe el valor de la carra de progreso del 1 al  10</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void AddValueProgressBar(int value)
        {
            int valueIncrement = (value * 10);
            double porcentValue = 0.00;
            if ( valueIncrement > 0)
            {
                porcentValue = (valueIncrement / 100) + 0.00;
               
                Console.WriteLine("Porcent: " + porcentValue.ToString() + "\t");
            }
            
            WorkerExchangeRate.ReportProgress(80, 0.80);

        }
    #endregion

    #region CancellWorkerExchangeRate
    ///<summary>Metodo que cancela y finaliza la tarea de transferencia de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void CancellWorkerExchangeRate()
        {
            if (WorkerExchangeRate.WorkerSupportsCancellation == true)
            {
                WorkerExchangeRate.CancelAsync();
            }
        }
    #endregion

    #region UpdateDelegateExchangeRate
    ///<summary>Metodo para crear delegado utilizado en la actualizacion el grid del log</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public delegate void UpdateDelegateExchangeRate(DateTime date, string logLevel, string message);
    #endregion

    #region UpdateDelegateDatagridExchangeRate
    ///<summary>Metodo que agrega nuevo registro en el log de exchange rate y actualiza el grid de log de exchange rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void UpdateDelegateDatagridExchangeRate(DateTime date, string logLevel = "", string message = "")
        {
            //lblLastTransferReservations.Content = " Time :" + " = > "+ date.ToString();
            listTransactions.Add(LogHelper.AddTransaction("ExchangeRate", date, logLevel, message));
            grdLogTransfer.ItemsSource = null;
            grdLogTransfer.ItemsSource = listTransactions;
            grdLogTransfer.Items.Refresh();//Refrescamos el grid
            GridHelper.SelectRow(grdLogTransfer, grdLogTransfer.Items.Count);


      //grdLogTransfer.Items.Add(Log.AddTransaction("ExchangeRate", date, logLevel, message));

    }
    #endregion

    #region WorkerExchangeRateRunWorkerCompleted
    ///<summary>Metodo que se ejecuta cuando se finaliza alguna tarea</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private void WorkerExchangeRateRunWorkerCompleted(object Sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                lblLastTransferReservations.Content = "Completed!";
                //AddValueProgressBar(0);

            }
            else if (e.Cancelled == true)
            {
                lblLastTransferReservations.Content = "Canceled!";
                //AddValueProgressBar(0);
            }
            else if (!(e.Error == null))
            {
                lblLastTransferReservations.Content = "Error: " + e.Error.Message.ToString();
                //AddValueProgressBar(0);
            }
            else
            {
                lblLastTransferReservations.Content = "Sorry it's Fail";
                AddValueProgressBar(0);
            }
        }
    #endregion

    #region WorkerUpdateExchangeRateDoWork
    ///<summary>Metodo que contiene la tarea que realiza todo el proceso de actualizacion de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private void WorkerUpdateExchangeRateDoWork(object Sender, DoWorkEventArgs e)
        {
            /*for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(1000);
                if (WorkerExchangeRate.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                UpdateDelegateExchangeRate updateDelegate = new UpdateDelegateExchangeRate(UpdateDelegateDatagridExchangeRate);
                lblLastTransferReservations.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegate, i + 1, DateTime.Now);

            }*/
            DateTime startExchangeRate = DateTime.Now;
            DateTime endExchangeRate;
            UpdateDelegateExchangeRate updateDelegate = new UpdateDelegateExchangeRate(UpdateDelegateDatagridExchangeRate);
            
            //Se obtiene la fecha del servidor
            _dtpServerDate = BRHelpers.GetServerDate();
            //AddItemDataGridExchangeRate(DateTime.Now, "Start", "Updating Exchange Rates");
            
            grdLogTransfer.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegate, DateTime.Now, "Start", "Updating Exchange Rates");
            AddValueProgressBar(1);

            //agregamos los tipos de cambio faltantes hasta la fecha actual para que no existan huecos
            BRExchangeRate.InsertExchangeRate(_dtpServerDate);

            //AddItemDataGridExchangeRate(DateTime.Now, "Info", "Getting Exchange Rate from Intranet Service");
            
            grdLogTransfer.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegate, DateTime.Now, "Start", "Updating Exchange Rates");
            AddValueProgressBar(2);
            //obtenemos el tipo de cambio de la Intranet
            TipoCambioTesoreria exchangeRate = IntranetHelper.TipoCambioTesoreria(Convert.ToDateTime("01/08/2008"), "USD");
            if (exchangeRate != null)
            {
                _exchangeRateType = Convert.ToDecimal(exchangeRate.TipoCambio);
                //comparamos si el cambio es positivo y mayor a cero
                if (_exchangeRateType > 0)
                {
                    _currencyId = "MX";
                    AddValueProgressBar(3);
                    //Se realiza la actualizacion y se registra en el log la inicialización
                    try
                    {
                        //AddItemDataGridExchangeRate(DateTime.Now, "Info", "Updating Exchange Rate " + _currencyId + " (" + _exchangeRateType.ToString() + ")");
                        grdLogTransfer.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegate, DateTime.Now, "Info", "Updating Exchange Rate " + _currencyId + " (" + _exchangeRateType.ToString() + ")");
                        AddValueProgressBar(4);
                        BRExchangeRate.UpdateExchangeRate(_dtpServerDate, _currencyId, _exchangeRateType);
                        
                        endExchangeRate = DateTime.Now;

                        //AddItemDataGridExchangeRate(DateTime.Now, "Info", "Updating Exchange Rate in " + timeDuration(startExchangeRate, endExchangeRate).ToString(@"hh\ \h\ \:mm\ \m\ \:ss\ \s\ \:fff\ \m\s"));
                        grdLogTransfer.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegate, DateTime.Now, "Info", "Updating Exchange Rate in " + DateHelper.timeDuration(startExchangeRate, endExchangeRate).ToString(@"hh\ \h\ \:mm\ \m\ \:ss\ \s\ \:fff\ \m\s"));
                        AddValueProgressBar(6);
                        //AddItemDataGridExchangeRate(DateTime.Now, "Success", "Process finished");
                        grdLogTransfer.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegate, DateTime.Now, "Success", "Process finished");
                        AddValueProgressBar(10);
                        Thread.Sleep(100);
                        AddValueProgressBar(0);
                        CancellWorkerExchangeRate();


                    }
                    catch (Exception error)
                    {
                        // Registramos en el log el tipo de error que se encontro
                        AddItemDataGridExchangeRate(DateTime.Now, "Error", error.ToString());
                        grdLogTransfer.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegate, DateTime.Now, "Error", error.ToString());
                    }
                }
                else
                {
                    // Registramos en el log el tipo de error que se encontro, en el tipo de cambio
                    //AddItemDataGridExchangeRate(DateTime.Now, "Error", "Exchange rate must be positive " + _currencyId + " (" + _exchangeRateType.ToString() + ")");
                    grdLogTransfer.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegate, DateTime.Now, "Error", "Exchange rate must be positive " + _currencyId + " (" + _exchangeRateType.ToString() + ")");
                }
            }
            else
            {
                // Registramos en el log que no existen cambios en la fecha especificada y el tipo de moneda
                //AddItemDataGridExchangeRate(DateTime.Now, "Warning", "Exchange rate does not exists for day");
                grdLogTransfer.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegate, DateTime.Now, "Warning", "Exchange rate does not exists for day");
            }
        }
    #endregion

    #endregion

    #region Metodos de Ventana
    private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblDate.Content = DateTime.Now.ToString("dd/MM/yyyy");
            lblStatus.Content = "STAND BY";
            LoadDataGRidXML();
            InitializeExchangeRate();
            lblLastTransferExchangeRate.Content = lblTextLastExchangeRage + _lastExchangeRate.ToString();
            InitializerProgressBar();
            DoReservations();
        }
    #endregion

    #region Metodos de acciones de botones
    private void btnTrasnferReservations_Click(object sender, RoutedEventArgs e)
        {
            
            //WorkerExchangeRate.RunWorkerAsync();

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (WorkerExchangeRate.WorkerSupportsCancellation == true)
            {
                WorkerExchangeRate.CancelAsync();
            }

        }
    #endregion

    private void progresBarTransfer_Changed(object sender, ProgressChangedEventArgs e)
        {
            progresBarTransfer.Value = e.ProgressPercentage;
            textStatusProgresBar.Text = String.Format("{0:p0}", e.UserState);
            
        }


}
}
