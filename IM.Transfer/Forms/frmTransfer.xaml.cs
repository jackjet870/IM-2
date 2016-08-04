using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Forms;

using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Services.Helpers;
using IM.Services.HotelService;
using IM.Services.IntranetService;
using PalaceResorts.Common.PalaceTools;
using System.ComponentModel;
using System.Linq;
using IM.Model;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Media;
using System.Windows.Data;

namespace IM.Transfer.Forms

{
  /// <summary>
  /// Interaction logic for frmTransfer.xaml
  /// </summary>
  public partial class frmTransferLauncher : Window
  { 
    #region Atributos

    #region Parametros para trasnferencia de Reservaciones
    private static TimeSpan _startTimeReservations;//Hora inicial del proceso de transferencia de reservaciones
    private static TimeSpan _endTimeReservations;//Hora final del proceso de transferencia de reservaciones
    private static TimeSpan _intervalTimeReservations;//Intervalo de tiempo del proceso de transferencia de reservaciones
    private static int _daysBeforeDAY;//Numero de dias anteriores al dia de hoy para obtener reservaciones
    private static int _daysAfterDAY;//Numero de dias posteriores al dia de hoy para obtener reservaciones
    private static int _retrys;//Numero de reintentos
    private static string _timeOutT;//Tiempo de espera del proceso 7,200,000 = 2 horas
    private static string _timeOutWebServiceT;//Tiempo de espera del servicio web 300,000 = 5 minutos
    private static TimeSpan _standbyIntervalTimeReservations; //Tiempo de espera para verificar el tiempo de ejecucion de la transferencia de reservaciones = 5 segundos
    private DateTime _lastReservations;//Fecha de ultima transferencia de reservaciones
    private DateTime _nextReservations; // Fecha proxima que se ejecutara la transferencia de reservaciones
    private static DispatcherTimer dispatcherBlinkLabelReservations = null; //Dispacher para dar efecto blink a label de status
    #endregion

    #region Parametros para trasnferencias de Exchange Rate
    public static decimal _exchangeRateType; 
    private static TimeSpan _tranferExchangeRatesStartTime;//Hora inicial del proceso de actualización de tipos de cambio
    private static TimeSpan _tranferExchangeRatesEndTime;//Hora final del proceso de actualización de tipos de cambio
    private static TimeSpan _tranferExchangeRatesIntervalTime;//Intervalo de tiempo del proceso de actualización de tipos de cambio
    private DateTime _lastExchangeRate;//Ultima hora que se ejecuto el proceso de actualización de tipos de cambio
    private DateTime _nextExchangeRate;//Hora que se volvera a ejecutar Exchange rate
    private static TimeSpan _standbyIntervalTimeExchangeRate; //Tiempo de espera para verificar el tiempo de ejecucion de la actualizacion de exchange rate = 5 segundos
    private static DispatcherTimer dispatcherBlinkLabelExchangeRate = null; //Dispacher para dar efecto blink a label de status
    #endregion

    #region _dtmServerDate
    //Datetime utilizado para la fecha del servidor.
    public static DateTime _dtmServerDate = new DateTime();
    public static string _currencyId;
    //Datetime para la fecha actual.
    public static DateTime _dateToday;
    #endregion

    #region CancellationTokenSource
    // instacia de cancelacion de actualización de reservaciones
    CancellationTokenSource cancelTokenReservations;
    // instacia de cancelacion de actualización de exchange rate
    CancellationTokenSource cancelTokenExchangeRate;
    #endregion

    #region Lista de registros
    // lista de transacciones de reservaciones
    public static List<LogHelper.Transaction> listTransactionsExchangeReservations = new List<LogHelper.Transaction>();
    CollectionViewSource reservationsCollectionViewSource;
    // lista de transacciones de exchange rate
    public static List<LogHelper.Transaction> listTransactionsReservations = new List<LogHelper.Transaction>();
    CollectionViewSource exchangeRateCollectionViewSource;
    #endregion

    #region timeWatch
    // Utilizado para calcular el tiempo que se tarda en llevar el proceso de obtencion de reservaciones
    public static Stopwatch timeWatchReservations = new Stopwatch(); 
    public static TimeSpan timeGetReservations = TimeSpan.MinValue;
    // Utilizado para calcular el tiempo que se tarda en llevar el proceso de obtencion de exchange rate
    public static Stopwatch timeWatchExchangeRate = new Stopwatch(); 
    public static TimeSpan timeGetExchangeRate = TimeSpan.MinValue;
    #endregion

    #region lblTextLast and lblTextLast
    public static string lblTextLast = "Last ";
    public static string lblTextNext = "Next ";
    #endregion

    #region Booleans
    // indica si desea transferir reservaciones para una noche
    public static bool blnOneNight = false;
    // inidica si desea transferir reservaciones para dos noches
    public static bool blnTwoNight = false;
    // si desea utilizar bandas.
    public static bool blnuseBand = false;
    //Indica si se desea iniciar el proceso de transferencia de reservaciones
    public static bool blnCallTransfer;
    //Indica si se desea transferir las reservaciones a la tabla de transferencia 
    public static bool blnTransfer;
    //Indica si se desea transferir las reservaciones a la tabla de huespedes
    public static bool blnTransferToGuests;

    #endregion

    #region Status Transfers
    // Parametros para validar el estatus de las trasnferencias
    // valida el estatus de la transferencia de reservaciones
    public bool blnRunOrCancelReservations = false;
    // valida el estatus de la transferencia de exchange rate 
    public bool blnRunOrCancelExchangeRate = false;
    #endregion

    #endregion

    #region Contructores y Destructores
    public frmTransferLauncher()
    {
      InitializeComponent();
      //Inicializa los parametros de la aplicación.
      InitializeValuesParameters();
      // inicializamos los viewsource
      exchangeRateCollectionViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("exchangeRateCollectionViewSource")));
      reservationsCollectionViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("reservationsCollectionViewSource")));
     
    }
    #endregion

    #region InitializeValuesParameters
    ///<summary>
    ///Metodo que inicializa los parametros y configuracion de las transferencias
    ///</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private void InitializeValuesParameters() {
      //Se inicializan los valores de los parametros para la transferencia de reservaciones.
      _startTimeReservations = TimeSpan.Parse(ConfigHelper.GetString("StartTime"));
      _endTimeReservations = TimeSpan.Parse(ConfigHelper.GetString("EndTime"));
      _intervalTimeReservations = TimeSpan.Parse(ConfigHelper.GetString("IntervalTime"));
      _daysBeforeDAY = Convert.ToInt32(ConfigHelper.GetString("DaysBefore"));
      _daysAfterDAY = Convert.ToInt32(ConfigHelper.GetString("DaysAfter"));
      _retrys = Convert.ToInt32(ConfigHelper.GetString("Retrys"));
      _timeOutT = ConfigHelper.GetString("TimeOut");
      _timeOutWebServiceT = ConfigHelper.GetString("TimeOutWebService");
      _standbyIntervalTimeReservations = TimeSpan.Parse(ConfigHelper.GetString("StandbyIntervalTimeReservations"));

      // fecha actual
      _dateToday = DateTime.Now;

      //se inicializan los parametros para ejecutar ExchangeRate
      _tranferExchangeRatesStartTime = TimeSpan.Parse(ConfigHelper.GetString("TranferExchangeRatesStartTime"));
      _tranferExchangeRatesEndTime = TimeSpan.Parse(ConfigHelper.GetString("TranferExchangeRatesEndTime"));
      _tranferExchangeRatesIntervalTime = TimeSpan.Parse(ConfigHelper.GetString("TranferExchangeRatesIntervalTime"));
      _standbyIntervalTimeExchangeRate = TimeSpan.Parse(ConfigHelper.GetString("StandbyIntervalTimeExchangeRate"));
      _lastExchangeRate = DateTime.Now;
      _lastReservations = DateTime.Now;
      //_notifyIconFormTransfers = NotifyIconHelper.Notify(form: this);

      // archivo para inicializar los valores booleanos de trasnferencia de reservaciones.
      string strArchivo = Path.Combine(AppContext.BaseDirectory, "Configuration.ini");
      if (LogHelper.ExistFile(strArchivo))
      {
        IniFileHelper _iniFileHelper = new IniFileHelper(strArchivo);
        blnCallTransfer = _iniFileHelper.ReadBoolean("TransferReservations", "CallTransfer", true);
        blnTransfer = _iniFileHelper.ReadBoolean("TransferReservations", "Transfer", true);
        blnTransferToGuests = _iniFileHelper.ReadBoolean("TransferReservations", "TransferToGuests", true);
      }

    }
    #endregion

    #region InitializeTransfers
    ///<summary>Metodo que inicializa y ejecuta las actualizaciones</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void InitializeTransfers() {
      //INSTANCIANDO EL TIMER CON LA CLASE DISPATCHERTIMER 
      DispatcherTimer dispathcerT = new DispatcherTimer();
      //EL INTERVALO DEL TIMER ES DE HORAS, MINUTOS Y SEGUNDOS QUE SE PASAN COMO PARAMETRO 
      dispathcerT.Interval = _standbyIntervalTimeExchangeRate;
      //EL EVENTO TICK SE SUBSCRIBE A UN CONTROLADOR DE EVENTOS UTILIZANDO LAMBDA 
      dispathcerT.Tick += (s, a) =>
      {
        //ACCION QUE SE DETONA CUANDO YA TRANSCURRIERON LOS SEGUNDOS ESTABLECIDOS
        
        DateTime _currentTime = DateTime.Now;
        // validamos si la ultima fecha de la actualizacion es del dia de hoy, si no lo es agrega la fecha actual.
        if (!DateHelper.isDateEquals(_currentTime, _nextExchangeRate))
          _nextExchangeRate = _currentTime;
        
        if (!DateHelper.isDateEquals(_currentTime, _nextReservations))
          _nextReservations = _currentTime;
        
        // validamos que la hora actual este en el rango de fechas para realizar la actualizacion
        if (DateHelper.IsRangeHours(_currentTime.TimeOfDay, _tranferExchangeRatesStartTime, _tranferExchangeRatesEndTime))
        {
          // validamos que ha transcurrido el tiempo para ejecutar la actualizacion
          if ((DateHelper.IsRangeTime(_currentTime, _nextExchangeRate) || DateHelper.IsRangeHours(_nextExchangeRate.TimeOfDay, _tranferExchangeRatesStartTime, _currentTime.TimeOfDay)) && (!blnRunOrCancelExchangeRate))
          {
            // se ejecuta el proceso de actualizacion de exchange rate.
            StartExchangeRate(); 
          }
        }
        // validamos que la hora actual este en el rango de fechas para realizar la actualizacion
        if (DateHelper.IsRangeHours(_currentTime.TimeOfDay, _startTimeReservations, _endTimeReservations))
        {
          // validamos que ha transcurrido el tiempo para ejecutar la actualizacion
          if ((DateHelper.IsRangeTime(_currentTime, _nextReservations) || DateHelper.IsRangeHours(_nextReservations.TimeOfDay, _startTimeReservations, _currentTime.TimeOfDay)) && (!blnRunOrCancelReservations))
          {
            // se ejecuta el proceso de actualizacion de reservaciones
            StartReservations();
          }
        }
        
      };
      dispathcerT.Start();

    }
    #endregion

    #region Metodo para inicializar las Transferencias

    #region StartReservations
    /// <summary>
    /// Inicializa las transferencia de Reservaciones
    /// </summary>
    /// <history>
    /// [michan] 28/04/2016
    /// </history>
    public async Task StartReservations()
    {
      // Se valida el estatus del formulario
      StatusForm();
      cancelTokenReservations = new CancellationTokenSource();
      // ponemos en verdadero la ejecucion de transferencia de reservaciones
      blnRunOrCancelReservations = true;
      // cambiamos el testo del boton para cancelar la transferemcia
      btnReservations.Content = "Cancel updating of Reservations";
      //UpdateButton(btnReservations, "Cancel updating of Reservations");
      // iniciamos el efecto blink en el label de status 
      OnOffBlinkReservations();

      try
      {
        await DoTransfer();
      }
      catch (OperationCanceledException)
      {
        AddLogGridReservations("Info", "Transfer Canceled.");
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Info", UIHelper.GetMessageError(exception));
      }
      finally
      {
        // indicamos que ya no se esta transfiriendo
        await BRTransfer.Stop();
        ResetParametersReservations();
        AddLogGridReservations("Finish", "Process Finished.");
        cancelTokenReservations.Dispose();
        cancelTokenReservations = null;
      }
      
      
    }
    #endregion

    #region StartExchangeRate
    /// <summary>
    /// Inicializa las transferencia de Exchange Rate
    /// </summary>
    /// <history>
    /// [michan] 28/04/2016
    /// </history>
    public async Task StartExchangeRate()
    {
      // Se valida el estatus del formulario
      StatusForm();

      //Se inicia el proceso para Exchange Rate
      blnRunOrCancelExchangeRate = true;
      UpdateButton(btnExchangeRate, "Cancel updating of Exchange Rate");
      OnOffBlinkExchangeRate();
      cancelTokenExchangeRate = new CancellationTokenSource();
      
      try
      {
        await TransferExchangeRates();
      }
      catch (OperationCanceledException)
      {
        AddLogGridExchangeRate("Info", "Update Canceled.");
      }
      catch (Exception exception)
      {
        AddLogGridExchangeRate("Info", UIHelper.GetMessageError(exception));
      }
      finally
      {
        // indicamos que el proceso de actualización ha terminado
        AddLogGridExchangeRate("Finish", "Process Finished.");
        // reseteamos los valores de configuración para la actaulización del exchange rate
        ResetParametersExchangeRate();
        cancelTokenExchangeRate.Dispose();
        cancelTokenExchangeRate = null;
      }
      
    }
    #endregion

    #endregion

    #region inicializa grids

    #region LoadDataGRidExchangeRate
    ///<summary>Metodo que carga e inicializa el grid del log de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private async Task LoadDataGRidExchangeRate()
    {
      
        DateTime dateTo = DateHelper.DaysBeforeOrAfter(_daysBeforeDAY);
        listTransactionsExchangeReservations = await LogHelper.LoadHistoryLog("ExchangeRate", DateTime.Now, dateTo);
        if (listTransactionsExchangeReservations.Count > 0 && listTransactionsExchangeReservations != null)
        {
          var last = listTransactionsExchangeReservations.LastOrDefault(level => level.LogLevel.ToUpper() == "FINISH");
          if (last != null)
          {
            _lastExchangeRate = last.Date;
            _nextExchangeRate = last.Date;
          }
          UpdatingGrid(grdLogExchangeRate, listTransactionsExchangeReservations, exchangeRateCollectionViewSource);
        }
      
    }
    #endregion

    #region LoadDataGRidReservations
    ///<summary>Metodo que carga e inicializa el grid del log de Reservations</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private async Task LoadDataGRidReservations()
    {
      
        DateTime dateTo = DateHelper.DaysBeforeOrAfter(_daysBeforeDAY);
        listTransactionsReservations = await LogHelper.LoadHistoryLog("Reservations", DateTime.Now, dateTo);
        if (listTransactionsExchangeReservations.Count > 0 && listTransactionsExchangeReservations != null)
        {
          var last = listTransactionsReservations.LastOrDefault(level => level.LogLevel.ToUpper() == "FINISH");
          if (last != null)
          {
            _lastReservations = last.Date;
            _nextReservations = last.Date;
          }
          UpdatingGrid(grdLogReservations, listTransactionsReservations, reservationsCollectionViewSource);
        }
      
      

    }
    #endregion

    #endregion

    #region Transferencia de reservaciones

    #region DoTransfer
    /// <summary>
    /// Inicia la transferencia de reservaciones
    /// </summary>
    /// <history>
    /// [michan] 23/04/2016 Created
    /// </history>
    public async Task DoTransfer()//object Sender, DoWorkEventArgs e
    {
      
      // Transferencia iniciada
      UpdateLabelStatusReservations("PROCESSING");
      AddLogGridReservations("Start", "Transfer Started.");

      //indicamos que inicio la transferencia de reservaciones
      List<TransferStartData> options = await BRTransfer.Start();
      if ((options != null) && (options.Count > 0))
      {
        //obtenemos los parametros de configuracion
        var transferStartData  = options.FirstOrDefault();
        if (transferStartData != null)
        {
          blnOneNight = transferStartData.ocOneNightV;
          blnTwoNight = transferStartData.ocTwoNightV;
        }
      }
      
      UpdateLabelStatusReservations("STARTED");
      
      // obtenemos las zonas de transferencia
      List<ZoneTransfer> zoneTransfer = await BRZones.GetZonesTransfer();
      // si hay al menos una zona
      if (zoneTransfer.Count > 0 && zoneTransfer != null)
      { 
        // recorremos las zonas
        foreach (ZoneTransfer zone in zoneTransfer)
        {
          cancelTokenReservations.Token.ThrowIfCancellationRequested();
          // validamos que hayan hoteles para la zona     
          if (!(String.IsNullOrEmpty(zone.znID) && !String.IsNullOrWhiteSpace(zone.znID)) && (!String.IsNullOrEmpty(zone.znN) && !String.IsNullOrWhiteSpace(zone.znN)) && (!String.IsNullOrEmpty(zone.znZoneHotel) && !String.IsNullOrWhiteSpace(zone.znZoneHotel)))
          {
            //exportamos las reservaciones a la tabla de transferencia
            int itotalReservations = await ExportToTransfer(zone.znID, zone.znN, zone.znZoneHotel);

            // valida si hay registro para transferir si hay 
            // exportamos los registros de la tabla de transferencia a la tabla de huespedes
            if (itotalReservations > 0) await ExportToGuests();

            //Total de registros agregados
            AddLogGridReservations("Info", "Transfer Completed.");

            // si la actualización no fue cancelada mostramos el total de reservaciones actualizadas
            AddLogGridReservations("Info", $"{ itotalReservations } Records Added.");
          }
          else
          {
            // si no existe o no hay la zona para la trasnferencia
            AddLogGridReservations("Warning", "No exist the zone");
          }
        }
      }
      else
      {
        // si no hay zonas para la transferencia
        AddLogGridReservations("Warning", "No zones transfer to process.");
      }
    }
    #endregion

    #region ExportToTransfer
    /// <summary>
    /// Exporta las reservaciones transferidas a la tabla de transferencia
    /// </summary>
    /// <param name="zoneID">ID de la zona que se exportara la información</param>
    /// <param name="zoneName">Nombre de la zona que se exportara la zona</param>
    /// <param name="zoneHotel">ID de los hoteles de la zona</param>
    /// <history>
    /// [michan]  23/04/2016 Created
    /// </history>
    public async Task<int> ExportToTransfer(string zoneID, string zoneName, string zoneHotel)
    {
      #region  obtenemos las reservaciones
      // obtenemos las reservaciones
      List<ReservationOrigosTransfer> reservationOrigosTransfer = null;
      reservationOrigosTransfer = await GetReservations(zoneID, zoneName, zoneHotel);
      int iCountReservations = 0;
      string strTransferingReservation = "";
      #endregion

      #region validamos que hayan registros por actualizar.
      // validamos que hayan registros por actualizar.
      if ((reservationOrigosTransfer != null && reservationOrigosTransfer.Count > 0))
      {
        iCountReservations = reservationOrigosTransfer.Count;

        // transfiriendo reservaciones a la base de datos.
        AddLogGridReservations("Insert", "Transfering Reservations to Database.");
        // limpiamos la tabla de transferencia
        await BRTransfer.DeleteTransfer();
        await BRTransfer.GetTransfer();

        // transferimos las reservaciones a la base de datos
        int cont = 0;
        double dPorcent = 0;

        #region Guarda las transferencias en osTransfer
        foreach (ReservationOrigosTransfer reservationOrigos in reservationOrigosTransfer)
        {
          // valida si se ha cancelado la operación.
          if (cancelTokenReservations.Token.IsCancellationRequested)
          { iCountReservations = 0; break; }
           
          //exportamos las reservaciones a la tabla de transferencia
          //cont = cont + 1;
          dPorcent = Porcent(cont++, iCountReservations);
          //progressReservations.Report(dPorcent);
          AddValueProgressBarReservations(porcentBase: dPorcent);
          // desplegamos la reservacion que se esta transfiriendo
          strTransferingReservation = $"Transfering Reservation {reservationOrigos.Hotel} No. {reservationOrigos.Folio} \r\nCheck In Date: {reservationOrigos.Arrival:dd/MMM/yyyy}";
          UpdateLabelTrasnferReservations(strTransferingReservation);
          //agregamos la reservacion transferida
          await AddTransfer(reservationOrigos);
        }
        #endregion

        //Total de registros agregados de la zona (X)
        AddLogGridReservations("Info", $"{iCountReservations}  Records Added from {zoneName}.");

        //indicamos que la zona ya no esta transfiriendo
        await TransferStopZone(zoneID);
      }
      else
      {
        // si no existen registros por actualizar no se realiza nada.
        AddLogGridReservations("Warning", "No records were found to process.");
      }
      #endregion

      // retornamos la cnatidad de reservaciones actualizadas
      return iCountReservations;
    }
    #endregion

    #region AddTransfer
    /// <summary>
    /// Agrega la reservacion a la tabla temporal de transferencia
    /// </summary>
    /// <param name="reservationOrigos">Objecto con informacion para insertar a la tabla de trasnferencias</param>
    /// <history>
    /// [michan]  23/04/2016  Created
    /// </history>
    public async Task AddTransfer(ReservationOrigosTransfer reservationOrigos)
    {
      
      // localizamos la reservacion
      bool existReservation = await BRTransfer.ExistReservation(reservationOrigos.Hotel, reservationOrigos.Folio.ToString());
      if (!existReservation)
      {
        // agregamos la reservacion
        //si la reservacion no ha sido agregada todavia
        Model.Transfer transfer = await Transfer(reservationOrigos);
        await BRTransfer.AddReservation(transfer);
      }
      else
      {
        AddLogGridReservations("Warning", "Reservation duplicated. Hotel: " + reservationOrigos.Hotel.ToString() + ", Folio: " + reservationOrigos.Folio.ToString());
      }
    }
    #endregion

    #region GetReservations
    /// <summary>
    /// Obtiene las reservaciones
    /// </summary>
    /// <param name="zoneID">Zona de transfeencia</param>
    /// <param name="zoneName">Nombre de la zona</param>
    /// <param name="zoneHotel">Hoteles que pertenecen a la zona</param>
    /// <returns>Lista de Reservaciones</returns>
    /// <history>
    /// [michan]  23/04/2016  Created
    /// </history>
    public async Task<List<ReservationOrigosTransfer>> GetReservations(string zoneID, string zoneName, string zoneHotel)
    {

      List<ReservationOrigosTransfer> reservations = null;
      // obtenemos los hoteles de la zona
      List<LeadSourceShort> hotels = await BRLeadSources.GetLeadSourcesByZoneBoss(zoneID);

      // inicializamos el rango de fecha para obtener las reservaciones
      DateTime dateFrom = DateHelper.DaysBeforeOrAfter(_daysBeforeDAY);
      DateTime dateTo = DateHelper.DaysBeforeOrAfter(_daysAfterDAY);
      

      // validamos si existe un archivo inicial
      string strArchivo = Path.Combine(AppContext.BaseDirectory, "Configuration.ini");
      if (LogHelper.ExistFile(strArchivo))
      {
        IniFileHelper _iniFileHelper = new IniFileHelper(strArchivo);
        // asignamos la fecha inicial
        dateFrom = _iniFileHelper.readDate("TransferReservations", "DateFrom", dateFrom);
        // asignamos la fecha final
        dateTo = _iniFileHelper.readDate("TransferReservations", "DateTo", dateTo);
      }
      string leadSourceID = StringIDHoteles(hotels);
      //si no hay hoteles en la zona
      if ((hotels.Count > 0) && (hotels != null) && dateFrom <= dateTo)
      {
        // obtenemos las reservaciones e la zona
        AddLogGridReservations("Info", $"Getting Reservations from {zoneName} (Check In Date: {DateHelper.DateRange(dateFrom, dateTo)} , Hotels: {leadSourceID} ).");
        timeWatchReservations.Start();
        reservations = await HotelServiceHelper.GetReservationsByArrivalDate(zoneHotel, dateFrom, dateTo, leadSourceID, cancelTokenReservations.Token);
        timeWatchReservations.Stop();
        

        // valida si se obtubieron reservaciones
        if (reservations != null && reservations.Count > 0)
        {
          // Calculamos el tiempo de obtencion de las reservaciones
          TimeSpan tSpan = timeWatchReservations.Elapsed;
          // Convertimos el tiempo de obtencion en string para pintar en el grid
          string elapsedTime = StringTimeDifference(tSpan);
          AddLogGridReservations("Info", $"{reservations.Count } Reservations were Obtained in { elapsedTime}");
        }
        else
        {
          // si no se pudo obtener las reservaciones, detenemos la transferencia
          AddLogGridReservations("Error", $"Could not get reservations from Zone {zoneName} (Check In Date: {DateHelper.DateRange(dateFrom, dateTo)} ).");
        }
        timeWatchReservations.Reset();
      }
      else
      {
        // si no se obtuvieron reservaciones
        AddLogGridReservations("Error", "There is not hotels for this zone (" + zoneName + ")");
      }

      // si se encontraron reservaciones las retornamos
      return reservations;

    }
    #endregion

    #region StringTimeDifference
    /// <summary>
    /// Metodo para dar formato de tiempo a un Timespean en string
    /// </summary>
    /// <param name="timeDiference"></param>
    /// <returns></returns>
    /// <history>
    /// [michan]  23/04/2016  Created
    /// </history>
    public string StringTimeDifference(TimeSpan timeDiference)
    {
      // Formateamos el timespan a string
      return String.Format("{0:00} h {1:00} m {2:00} s {3:00} ms", timeDiference.Hours, timeDiference.Minutes, timeDiference.Seconds, timeDiference.Milliseconds / 10);
    }
    #endregion

    #region Transfer
    /// <summary>
    /// Transfiere los registros obtenido del web service a un objeto Transfer
    /// </summary>
    /// <param name="reservationOrigosTransfer"></param>
    /// <returns></returns>
    public async Task<Model.Transfer> Transfer(ReservationOrigosTransfer reservationOrigosTransfer)
    {
      Model.Transfer transfer = new Model.Transfer();
      await Task.Run(() => {   
      #region Transfer

      //Hotel
      transfer.tls = reservationOrigosTransfer.Hotel;
      
      //Folio
      transfer.tHReservID = Convert.ToString(reservationOrigosTransfer.Folio);
      
      //Nombre
      transfer.tFirstName = DeleteInvalidCharacters(reservationOrigosTransfer.FirstName);
      
      //Apellido
      transfer.tLastName = DeleteInvalidCharacters(reservationOrigosTransfer.LastName);
      
      //Habitacion
      transfer.tRoomNum = reservationOrigosTransfer.Room;
      
      //Tipo de habitacion
      transfer.trt = ConvertHelper.StringEmptyToNull(reservationOrigosTransfer.RoomType);
      transfer.trtN = ConvertHelper.StringEmptyToNull(reservationOrigosTransfer.RoomTypeN);
      
      //Pax
      transfer.tPax = reservationOrigosTransfer.Pax;
      
      //Fecha de llegada
      transfer.tCheckInD = reservationOrigosTransfer.Arrival;
      
      //Fecha de salida
      transfer.tCheckOutD = reservationOrigosTransfer.Departure;
      
      //Correo electronico
      transfer.tEmail = (ValidateHelper.IsValidEmail(reservationOrigosTransfer.Email)) ? reservationOrigosTransfer.Email : "";
     
      //Ciudad
      transfer.tCity = Trim(reservationOrigosTransfer.City);
      //Estado
      transfer.tState = Trim(reservationOrigosTransfer.State);
      //Pais
      transfer.tcoID = SetIDName(reservationOrigosTransfer.Country);
      transfer.tcoN = SetIDName(reservationOrigosTransfer.Country, reservationOrigosTransfer.CountryN);
      
      //Agencia
      transfer.tagID = SetIDName(reservationOrigosTransfer.Agency);
      transfer.tagN = SetIDName(reservationOrigosTransfer.Agency, reservationOrigosTransfer.AgencyN);
      
      //Pais de la agencia
      transfer.tcoAID = SetIDName(reservationOrigosTransfer.CountryAgency);
      transfer.tcoAN = SetIDName(reservationOrigosTransfer.CountryAgency, reservationOrigosTransfer.CountryAgencyN);

      //Club de la membresia
      transfer.tcl = ConvertHelper.StringToIntOrNull(reservationOrigosTransfer.Club.ToString());
      
      //Compañia de la membresia
      transfer.tCompany = reservationOrigosTransfer.Company;

      //Numero de membresia
      transfer.tMembershipNum = reservationOrigosTransfer.Membership;
      //transfer.tMember = ConvertHelper.StringToBool(reservationOrigosTransfer.Membership.Trim());

      //Estatus
      transfer.tGuestStatus = Trim(reservationOrigosTransfer.Status);
      
      //En grupo
      transfer.tOnGroup = reservationOrigosTransfer.Group;
      
      //Cortesia
      transfer.tComplim = reservationOrigosTransfer.Courtesy;
      
      //Contrato(Se usa el campo opcional 1)
      transfer.tO1 = Trim(reservationOrigosTransfer.Contract);
      transfer.tcnN = Trim(reservationOrigosTransfer.ContractN);
     
      //Tipo de huesped (Se usa el campo opcional 2)
      transfer.tO2 = Trim(reservationOrigosTransfer.GuestType);
      
      //Tarjeta de credito
      transfer.tCCType = Trim(reservationOrigosTransfer.CreditCard);
      
      //Reservaciones enlazadas - Consecutivo (0 no enlazada, 1 primera, 2 segunda, etc.)
      transfer.tDivResConsec = Convert.ToByte(reservationOrigosTransfer.LinkedConsecutive);
      
      //Reservaciones enlazadas - Hotel anterior
      transfer.tDivResLeadSource = ConvertHelper.StringEmptyToNull(reservationOrigosTransfer.LinkedHotelPrevious);
      
      //Reservaciones enlazadas - Folio de reservacion anterior
      transfer.tDivResResNum = ConvertHelper.StringEmptyToNull(reservationOrigosTransfer.LinkedHotelPrevious);
      //      Tipo de socio
      // - G - Guest (Invitado)
      // - M - Member (Socio))        
      transfer.tGuestRef = ConvertHelper.StringEmptyToNull(reservationOrigosTransfer.MemberType);
      
      //Fechas de cumpleaños
      transfer.tBirthDate1 = DateHelper.IsDefaultDate(reservationOrigosTransfer.BirthDate);
      transfer.tBirthDate2 = DateHelper.IsDefaultDate(reservationOrigosTransfer.BirthDate2);
      transfer.tBirthDate3 = DateHelper.IsDefaultDate(reservationOrigosTransfer.BirthDate3);
      transfer.tBirthDate4 = DateHelper.IsDefaultDate(reservationOrigosTransfer.BirthDate4);
      
      //Edades
      transfer.tAge1 = DateHelper.GetAge(reservationOrigosTransfer.BirthDate);
      transfer.tAge2 = DateHelper.GetAge(reservationOrigosTransfer.BirthDate2);
      
      //Tipo de reservacion
      transfer.tReservationType = reservationOrigosTransfer.Type;
      //Hotel anterior
      transfer.tHotelPrevious = reservationOrigosTransfer.HotelPrevious;
      // Folio anterior
      transfer.tFolioPrevious = reservationOrigosTransfer.FolioPrevious;
      //Motivo de indisponibilidad (0 - AVAILABLE)
      transfer.tum = 0;
      //Disponibilidad (No disponible)
      
      transfer.tAvail = false;
      //Idioma
      //transfer.tla = ;
      //Id del perfil de Opera
      transfer.tIdProfileOpera = reservationOrigosTransfer.IdProfileOpera;
      //Fecha y hora de modificacion
      transfer.tEditDT = DateTime.Now;
        #endregion
      });
      return transfer;
    }
    #endregion

    #region ExportToGuests
    /// <summary>
    /// Exporta los registros de la tabla de transferencia a la tabla de huespedes
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task ExportToGuests()
    {
      cancelTokenReservations.Token.ThrowIfCancellationRequested();
      //procesando registros transferidos
      AddLogGridReservations("Info", "Processing Transferred Records.");
      //actualizamos el catalogo de paises
      await UpdateCountries();
      cancelTokenReservations.Token.ThrowIfCancellationRequested();
      //actualizamos el catalogo de agencias
      await UpdateAgencies();
      cancelTokenReservations.Token.ThrowIfCancellationRequested();
      //actualizamos el catalogo de tipos de habitacion
      await UpdateRoomTypes();
      cancelTokenReservations.Token.ThrowIfCancellationRequested();
      //actualizamos el catalogo de contratos
      await UpdateContracts();
      cancelTokenReservations.Token.ThrowIfCancellationRequested();
      //actualizamos el catalogo de grupos
      await UpdateGroups();
      cancelTokenReservations.Token.ThrowIfCancellationRequested();
      //actualizamos la tabla de transferencia
      await UpdateTransfer();
      cancelTokenReservations.Token.ThrowIfCancellationRequested();
      //actualizamos los huespedes
      await UpdateGuests();
      cancelTokenReservations.Token.ThrowIfCancellationRequested();

    }
    #endregion

    #region UpdateCountries
    /// <summary>
    /// actualizamos el catalogo de paises
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateCountries()
    {
      
      #region AddCountriesHotel
      try
      {
        // agregando paises de Hotel
        AddLogGridReservations("Insert", "Adding Hotel Countries.");
        await BRTransfer.AddCountriesHotel();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region AddCountries
      try
      {
        // agregando paises de Origos
        AddLogGridReservations("Insert", "Adding Origos Countries.");
        await BRTransfer.AddCountries();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateCountriesHotelNames
      try
      {
        // actualizando las descripciones de los paises de Hotel
        AddLogGridReservations("Update", "Updating Hotel Countries Names.");
        await BRTransfer.UpdateCountriesHotelNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateCountriesNames
      try
      {
        // actualizando las descripciones de los paises de Origos
        AddLogGridReservations("Update", "Updating Origos Countries Names.");
        await BRTransfer.UpdateCountriesNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

    }
    #endregion

    #region UpdateAgencies
    /// <summary>
    /// Actualiza el catalogo de agencias
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateAgencies()
    {
      #region AddAgenciesHotel
      try
      {
        //agregando agencias de Hotel
        AddLogGridReservations("Insert", "Adding Hotel Agencies.");
        await BRTransfer.AddAgenciesHotel();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region AddAgencies
      try
      {
        //agregando agencias de Origos
        AddLogGridReservations("Insert", "Adding Origos Agencies.");
        await BRTransfer.AddAgencies();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateAgenciesHotelNames
      try
      {
        //actualizando las descripciones de las agencias de Hotel
        AddLogGridReservations("Update", "Updating Hotel Agencies Names.");
        await BRTransfer.UpdateAgenciesHotelNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateAgenciesNames
      try
      {
        //actualizando las descripciones de las agencias de Origos
        AddLogGridReservations("Update", "Updating Origos Agencies Names.");
        await BRTransfer.UpdateAgenciesNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion
    }
    #endregion

    #region UpdateRoomTypes
    /// <summary>
    /// Actualiza el catalogo de tipos de habitacion
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateRoomTypes()
    {
      #region AddRoomTypes
      try
      {
        //agregando tipos de habitacion
        AddLogGridReservations("Insert", "Adding Room Types.");
        await BRTransfer.AddRoomTypes();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateRoomTypesNames
      try
      {
        //actualizando las descripciones de los tipos de habitacion
        AddLogGridReservations("Update", "Updating Room Types Names.");
        await BRTransfer.UpdateRoomTypesNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion
    }
    #endregion

    #region UpdateContracts
    /// <summary>
    /// Actualiza el catalogo de contratos
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateContracts()
    {
      #region AddContracts
      try
      {
        //agregando contratos
        AddLogGridReservations("Insert", "Adding Contracts.");
        await BRTransfer.AddContracts();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateContractsNames
      try
      {
        //actualizando las descripciones de los contratos
        AddLogGridReservations("Update", "Updating Contracts Names.");
        await BRTransfer.UpdateContractsNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion
    }
    #endregion

    #region UpdateGroups
    /// <summary>
    /// Actualiza el catalogo de grupos
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateGroups()
    {
      try
      {
        // agregando contratos
        AddLogGridReservations("Insert", "Adding Groups.");
        await BRTransfer.AddGroups();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }

      try
      {
        //actualizando las descripciones de los contratos
        AddLogGridReservations("Update", "Updating Groups Names");
        await BRTransfer.UpdateGroupsNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      
    }
    #endregion

    #region UpdateTransfer
    /// <summary>
    /// Actualiza la tabla de transferencia
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateTransfer()
    {
      #region UpdateTransferCountries
      try
      {
        //actualizando paises de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Countries");
        await BRTransfer.UpdateTransferCountries();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferAgencies
      try
      {

        //actualizando agencias de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Agencies");
        await BRTransfer.UpdateTransferAgencies();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferLanguages
      try
      {
        //actualizando idiomas de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Languages");
        await BRTransfer.UpdateTransferLanguages();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferMarkets
      try
      {
        //actualizando mercados de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Markets");
        await BRTransfer.UpdateTransferMarkets();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferUnavailableMotivesByGroups
      try
      {
        //actualizando disponibilidad por grupos
        AddLogGridReservations("Update", "Updating Unavailable Motives by Groups");
        await BRTransfer.UpdateTransferUnavailableMotivesByGroups();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferUnavailableMotivesByAgency
      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por agencia
        AddLogGridReservations("Update", "Updating Unavailable Motives by Agency");
        await BRTransfer.UpdateTransferUnavailableMotivesByAgency();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferUnavailableMotivesByCountry
      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por pais
        AddLogGridReservations("Update", "Updating Unavailable Motives by Country");
        await BRTransfer.UpdateTransferUnavailableMotivesByCountry();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferUnavailableMotivesByContract
      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por contrato
        AddLogGridReservations("Update", "Updating Unavailable Motives by Contract");
        await BRTransfer.UpdateTransferUnavailableMotivesByContract();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferUnavailableMotivesBy1Night and  UpdateTransferUnavailableMotivesBy2Nights
      if (blnOneNight)
      {
        try
        {
          //actualizando motivos de indisponibilidad de la tabla de transferencia por 1 noche
          AddLogGridReservations("Update", "Updating Unavailable Motives by Just One Night");
          await BRTransfer.UpdateTransferUnavailableMotivesBy1Night();
        }
        catch (Exception exception)
        {
          AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
        }
      }
      if (blnTwoNight)
      {
        try
        {
          //actualizando motivos de indisponibilidad de la tabla de transferencia por 2 noches
          AddLogGridReservations("Update", "Updating Unavailable Motives by Just Two Nights");
          await BRTransfer.UpdateTransferUnavailableMotivesBy2Nights();
        }
        catch (Exception exception)
        {
          AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
        }
      }
      #endregion

      #region UpdateTransferUnavailableMotivesByTransfer
      try
      {

        //actualizando motivos de indisponibilidad de la tabla de transferencia por transferencia
        AddLogGridReservations("Update", "Updating Unavailable Motives by Transfer");
        await BRTransfer.UpdateTransferUnavailableMotivesByTransfer();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferUnavailableMotivesByNewMember
      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por ser nuevo socio
        AddLogGridReservations("Update", "Updating Unavailable Motives by New Member");
        await BRTransfer.UpdateTransferUnavailableMotivesByNewMember();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferUnavailableMotivesByPax
      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por tener pax 1
        AddLogGridReservations("Update", "Updating Unavailable Motives by Pax");
        await BRTransfer.UpdateTransferUnavailableMotivesByPax();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateTransferAvailability
      try
      {
        //actualizando disponibilidad de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Availability");
        await BRTransfer.UpdateTransferAvailability();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

    }
    #endregion

    #region UpdateGuests
    /// <summary>
    /// Actualiza los huespedes
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateGuests()
    {
      #region DeleteReservationsCancelled
      try
      {
        //eliminando reservaciones canceladas
        await BRTransfer.DeleteReservationsCancelled();
        AddLogGridReservations("Delete", "Deleting Canceled Reservations.");
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsRoomNumbers
      try
      {
        //actualizando numeros de habitacion de huespedes
        AddLogGridReservations("Update", "Updating Room Numbers");
        await BRTransfer.UpdateGuestsRoomNumbers();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsRoomTypes
      try
      {
        //actualizando tipos de habitacion de huespedes
        AddLogGridReservations("Update", "Updating Room Types");
        await BRTransfer.UpdateGuestsRoomTypes();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsCreditCards
      try
      {
        //actualizando tarjetas de credito de huespedes
        AddLogGridReservations("Update", "Updating Credit Cards");
        await BRTransfer.UpdateGuestsCreditCards();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsLastNames
      try
      {
        //actualizando apellidos de huespedes
        AddLogGridReservations("Update", "Updating Last Names");
        await BRTransfer.UpdateGuestsLastNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsFirstNames
      try
      {
        //actualizando nombres de huespedes
        AddLogGridReservations("Update", "Updating First Names");
        await BRTransfer.UpdateGuestsFirstNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsCheckInDates
      try
      {
        //actualizando fechas de llegada de huespedes
        AddLogGridReservations("Update", "Updating Check-In Dates");
        await BRTransfer.UpdateGuestsCheckInDates();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsCheckIns
      try
      {
        //actualizando Check Ins de huespedes
        AddLogGridReservations("Update", "Updating Check-Ins");
        await BRTransfer.UpdateGuestsCheckIns();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsCheckOutDates
      try
      {
        //actualizando fechas de salida de huespedes
        AddLogGridReservations("Update", "Updating Check-Out Dates");
        await BRTransfer.UpdateGuestsCheckOutDates();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsEmails
      try
      {
        //actualizando correos electronicos de huespedes
        AddLogGridReservations("Update", "Updating E-mails");
        await BRTransfer.UpdateGuestsEmails();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsCities
      try
      {
        //actualizando ciudades de huespedes
        AddLogGridReservations("Update", "Updating Cities");
        await BRTransfer.UpdateGuestsCities();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsStates
      try
      {
        //actualizando estados de huespedes
        AddLogGridReservations("Update", "Updating States");
        await BRTransfer.UpdateGuestsStates();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsCountries
      try
      {
        //actualizando paises de huespedes
        AddLogGridReservations("Update", "Updating Countries");
        await BRTransfer.UpdateGuestsCountries();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsCheckOutsEarly
      try
      {
        //actualizando salidas anticipadas de huespedes
        AddLogGridReservations("Update", "Updating Early Check-Outs");
        await BRTransfer.UpdateGuestsCheckOutsEarly();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region  UpdateGuestsGuestTypes
      try
      {
        //actualizando tipos de huespedes
        AddLogGridReservations("Update", "Updating Guest Types");
        await BRTransfer.UpdateGuestsGuestTypes();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsContracts
      try
      {
        //actualizando contratos de huespedes
        AddLogGridReservations("Update", "Updating Contracts");
        await BRTransfer.UpdateGuestsContracts();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsBirthDates
      try
      {
        //actualizando fechas de nacimiento de huespedes
        AddLogGridReservations("Update", "Updating Birth Dates");
        await BRTransfer.UpdateGuestsBirthDates();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsAges
      try
      {

        //actualizando edades de huespedes
        AddLogGridReservations("Update", "Updating Ages");
        await BRTransfer.UpdateGuestsAges();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsPax
      try
      {
        //actualizando pax de huespedes
        AddLogGridReservations("Update", "Updating Pax");
        await BRTransfer.UpdateGuestsPax();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsReservationTypes
      try
      {
        //actualizando tipos de reservacion de huespedes
        AddLogGridReservations("Update", "Updating Reservation Types");
        await BRTransfer.UpdateGuestsReservationTypes();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsIdProfileOpera
      try
      {
        //actualizando id del perfil de Opera de huespedes
        AddLogGridReservations("Update", "Updating Id Profile Opera");
        await BRTransfer.UpdateGuestsIdProfileOpera();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsAgencies
      try
      {
        //actualizando agencias de huespedes
        AddLogGridReservations("Update", "Updating Agencies");
        await BRTransfer.UpdateGuestsAgencies();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsMarkets
      try
      {
        //actualizando mercados de huespedes
        AddLogGridReservations("Update", "Updating Markets");
        await BRTransfer.UpdateGuestsMarkets();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsAvailabilityUnavailableMotives
      try
      {
        //actualizando disponibilidad y motivos de indisponibilidad de huespedes
        AddLogGridReservations("Update", "Updating Availability & Unavailable Motives");
        await BRTransfer.UpdateGuestsAvailabilityUnavailableMotives();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsUnavailableMotives1NightRevert
      try
      {
        //actualizando motivos de indisponibilidad de huespedes (Revirtiendo proceso de disponibilidad por una noche)
        AddLogGridReservations("Update", "Updating Unavailable Motives (Reverting One Night Availability)");
        await BRTransfer.UpdateGuestsUnavailableMotives1NightRevert();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region UpdateGuestsAvailables
      try
      {
        //actualizando huespedes disponibles
        AddLogGridReservations("Update", "Updating Guests Availables.");
        DateTime dateFrom = DateHelper.DaysBeforeOrAfter(-60);
        DateTime dateTo = DateHelper.DaysBeforeOrAfter(60);
        await BRTransfer.UpdateGuestsAvailables(dateFrom, dateTo);
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

      #region AddGuests
      try
      {
        //agregando nuevos huespedes
        AddLogGridReservations("Insert", "Adding New Guests.");
        await BRTransfer.AddGuests();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      #endregion

    }
    #endregion

    #region TransferStopZone
    /// <summary>
    /// Indica que una zona ya no esta transfiriendo
    /// </summary>
    /// <param name="strZone">ID de la zona que se indicara el stop de transferencia</param>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task TransferStopZone(string strZone)
    {

      AddLogGridReservations("Stop", "Stop Transafer " + strZone);
      try
      {
        await BRTransfer.StopZone(strZone);
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", UIHelper.GetMessageError(exception));
      }
      
    }
    #endregion

    #region StringIDHoteles
    /// <summary>
    /// Metodo que retorna en string los hoteles de una zona
    /// </summary>
    /// <param name="hotels">Hoteles</param>
    /// <returns>String de hoteles</returns>
    /// <history>
    /// [michan]  16/04/2016  Created
    /// </history>
    public string StringIDHoteles(List<LeadSourceShort> hotels)
    {
      string leadSourceId = string.Empty;
      // recorremos la lista
      foreach (LeadSourceShort hotel in hotels)
      {
        // concatenamos la lista de hoteles
        leadSourceId += hotel.lsID + ",";
      }
      //eliminamos la ultima coma.
      leadSourceId = leadSourceId.Remove(leadSourceId.Length - 1);
      // retornamos el string.
      return leadSourceId;
    }
    #endregion

    #region DeleteInvalidCharacters
    /// <summary>
    /// Elimina los caracteres invalidos de una cadena
    /// </summary>
    /// <param name="strString"></param>
    /// <returns></returns>
    public string DeleteInvalidCharacters(string strString)
    {
      string str = strString;
      char caracter = Convert.ToChar(146);
      if(!String.IsNullOrEmpty(strString))
      {
        str.Replace("-", " ");
        str.Replace("'", " ");
        str.Replace(""+caracter, " ");
        str.Trim();
        str.TrimEnd();
        str.TrimStart();
      }
      return str;

    }
    #endregion

    #region SetIDName
    /// <summary>
    /// Establece el ID y el nombre de un elemento
    /// </summary>
    /// <param name="strString">ID del elemento</param>
    /// <param name="strStringN">Nombre del elemento</param>
    /// <returns></returns>
    public string SetIDName(string strString, string strStringN = null)
    {
      string strTemp = "?";
      // valida si que el strStringN no sea nullo o vacio
      if (!String.IsNullOrEmpty(strStringN) && !String.IsNullOrWhiteSpace(strStringN))
      {
        // asigna el valor de strStringN a strTemp
        strTemp = strStringN;
      }
      // valida si la strString no sea nullo o vacio
      // si el strStringN es nullo o vacio retorna el valor de strString
      // de lo contrario asigna el valor de strTemp (strStringN)
      strTemp = ((!String.IsNullOrEmpty(strString) && !String.IsNullOrWhiteSpace(strString)) && (String.IsNullOrEmpty(strStringN) && String.IsNullOrWhiteSpace(strStringN))) ? strString : strTemp;
      
      return strTemp;
    }
    #endregion

    #region Trim
    public string Trim(string strString)
    {
      string str = strString;
      if (!String.IsNullOrEmpty(strString) || !String.IsNullOrWhiteSpace(strString))
      {
        str = strString.Trim();
      }
      return str;
    }
    #endregion

    #endregion
    
    #region efecto blink

    #region OnOffBlinkReservations
    /// <summary>
    /// Metodo para aplicar efecto blink a label de status de actualizacion de reservaciones
    /// </summary>
    /// <param name="blnStatus">True para iniciar y False paraterminar</param>
    /// <history>
    /// [michan]  28/04/2016  Created
    /// </history>
    public void OnOffBlinkReservations(bool? blnStatus = true)
    {
      lblStatusReservations.IsEnabled = blnStatus.Value;
    }
    #endregion

    #region OnOffBlinkExchangeRate
    /// <summary>
    /// Metodo para aplicar efecto blink a label de status de actualizacion de exchange rate
    /// </summary>
    /// <param name="blnStatus">True para iniciar y False paraterminar</param>
    /// <history>
    /// [michan]  28/04/2016  Created
    /// </history>
    public void OnOffBlinkExchangeRate(bool? blnStatus = true)
    {
      lblStatusExchangeRate.IsEnabled = blnStatus.Value;
    }
    #endregion
    #endregion

    #region Metodos para actualizar botones

    #region UpdateButton
    /// <summary>
    /// Metodo que recibe la instancia del boton y contenido a actualizar
    /// </summary>
    /// <param name="btnName">Boton a actualizar o modificar su contenido</param>
    /// <param name="strContetn">contenido del boton</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void UpdateButton(System.Windows.Controls.Button btnName, string strContent, bool? blnIsEnable = true)
    {
      btnName.Content = strContent;
      btnName.IsEnabled = blnIsEnable.Value;
      /*
      // si no se ha instanciado el delegado
      if (updateContentButton == null)
      {
        // se crea instancia de delegado.
        updateContentButton = new UpdateDelegateButton(ContentButton);
      }
      // actualizamos el buttom con el delegado
      btnName.Dispatcher.BeginInvoke(DispatcherPriority.Render, updateContentButton, btnName, strContent, blnIsEnable.Value);*/
    }
    #endregion

    #endregion

    #region Actualizacion de Grids

    #region UpdatingGrid
    /// <summary>
    /// Metodo para actualizar datagrid de los log
    /// </summary>
    /// <param name="dataGrid">Datagrid a actualizar</param>
    /// <param name="listTransations">Lista de transacciones a enviar al datagrid</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void UpdatingGrid(System.Windows.Controls.DataGrid grdDataGrid, List<LogHelper.Transaction> listTransations, CollectionViewSource collectionView)
    {
      
        // valida si la lista tiene elementos
        // validamos que ayan pasado un objeto datagrid
        if (listTransations != null && grdDataGrid != null && collectionView != null && listTransations.Count > 0)
        {
          // se obtienen los primeros 500 registros.
          var itemsSource = listTransations.Skip(Math.Max(0, listTransations.Count() - 499)).Take(499).ToList();
          collectionView.Source = null;
          // asignamos los registros al datagrid
          collectionView.Source = itemsSource;
          // refrescamos el datagrid con los nuevos elementos.
          grdDataGrid.Items.Refresh();
          // obtenemos el total de registros en el datagrid
          int index = (grdDataGrid.Items.Count > 2) ? grdDataGrid.Items.Count - 1 : 0;
          // seleccionamos el ultimo elementos en el datagrid.
          GridHelper.SelectRow(grdDataGrid, index);
        }
      
    }
    #endregion

    #region AddLogGridReservations
    /// <summary>
    /// Metodo para actualizar el grid de log de la actualizacion de reservaciones
    /// </summary>
    /// <param name="strLogLevel">Tipo de mensaje en el log</param>
    /// <param name="strLogMessage">Mensaje en el log</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void AddLogGridReservations(string strLogLevel, string strLogMessage)
    {
      // validamos que el strLogLevel y el strMessage no sean vacios o nullos
      if ((!String.IsNullOrEmpty(strLogLevel) && !String.IsNullOrWhiteSpace(strLogLevel)) && (!String.IsNullOrEmpty(strLogMessage) && !String.IsNullOrWhiteSpace(strLogMessage)))
      {
        // agregamos al log el registro
        var logTransfer = LogHelper.AddTransaction("Reservations", DateTime.Now, strLogLevel, strLogMessage);
        if (logTransfer != null)
        {
          // agregamos el log a la lista de logs
          listTransactionsReservations.Add(logTransfer);
          // se actualiza el grid
          UpdatingGrid(grdLogReservations, listTransactionsReservations, reservationsCollectionViewSource);
        }
      }
    }
    #endregion

    #region AddLogGridExchangeRate
    /// <summary>
    /// Metodo para actualizar el grid de la actualizan de exchange rate.
    /// </summary>
    /// <param name="strLogLevel">Tipo de mensaje en el log</param>
    /// <param name="strLogMessage">Mesanje del log</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void AddLogGridExchangeRate(string strLogLevel, string strLogMessage)
    {
      
        // validamos que el tipo de log y el mensaje no esten vacios.
        if ((!String.IsNullOrEmpty(strLogLevel) && !String.IsNullOrWhiteSpace(strLogLevel)) && (!String.IsNullOrEmpty(strLogMessage) && !String.IsNullOrWhiteSpace(strLogMessage)))
        {
          var logTransfer = LogHelper.AddTransaction("ExchangeRate", DateTime.Now, strLogLevel, strLogMessage);
          if (logTransfer != null)
          {
            listTransactionsExchangeReservations.Add(logTransfer);
            UpdatingGrid(grdLogExchangeRate, listTransactionsExchangeReservations, exchangeRateCollectionViewSource);
          }
        }
      
    }
    #endregion

    #endregion

    #region Metodos de Cancelacion de trasnferencias

    #region CancelWorkerReservations
    ///<summary>Metodo que cancela y finaliza la tarea de transferencia de Reservations</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void CancelWorkerReservations()
    {
      // se cancela la tarea y se reinicia los parametros de la tarea.
      AddLogGridReservations("Info", "Canceling Transfer Reservations");
      UpdateLabelStatusReservations("CANCELING");
      UpdateButton(btnReservations, "Cancel updating of Reservations", false); // cambiamos el testo del boton para cancelar la transferemcia
      if(cancelTokenReservations != null) cancelTokenReservations.Cancel();
    }
    #endregion

    #region CancelWorkerExchangeRate
    ///<summary>Metodo que cancela y finaliza la tarea de transferencia de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void CancelWorkerExchangeRate()
    {
      // se cancela la tarea y se reinicia los parametros de la tarea.
      AddLogGridExchangeRate("Info", "Cancelling Exchange Rate");
      UpdateLabelStatusExchangeRate("CANCELING");
      UpdateButton(btnExchangeRate, "Cancel updating of Exchange Rate", false);
      if(cancelTokenExchangeRate != null) cancelTokenExchangeRate.Cancel();
    }
    #endregion

    #endregion

    #region Metodos para reinicar valores de Exchange Rate y Reservations

    #region ResetParametersReservations
    /// <summary>
    /// Metotdo que reinicia los valores para la transferencia de reservaciones
    /// </summary>
    /// <history>
    /// [michan]  28/04/2016/ Created
    /// </history>
    public void ResetParametersReservations()
    {
      // cambiamos la bandera para indicar que no se esta actualizando
      blnRunOrCancelReservations = false;
      // reiniciamos a cero los valores del progresbar
      AddValueProgressBarReservations(porcentBase: 0);
      lblPorcentProgresBarReservations.Content = string.Empty;
      
      // cambiamos el label del boton de reservaciones
      UpdateButton(btnReservations, "Update Reservations");
      // borramos los registros del label de visor de reservaciones
      lblTransferReservations.Content = string.Empty;
      
      // actualizamos la hora y fecha de la ultima y proxima actualización.
      _lastReservations = DateTime.Now;
      _nextReservations = DateHelper.AddTimeDate(_intervalTimeReservations);
      lblLastTransferReservations.Content = lblTextLast + _lastReservations.ToString();
      lblNextTransferReservations.Content = lblTextNext + _nextReservations.ToString();
     
      // Detenemos el efecto blink
      OnOffBlinkReservations(false);
      // ponemos en stand by el label de estatus
      UpdateLabelStatusReservations("STAND BY");
      btnReservations.IsEnabled = true;

    }
    #endregion

    #region ResetParametersExchangeRate
    /// <summary>
    /// Metotdo que reinicia los valores para la transferencia de Exchange Rate
    /// </summary>
    /// <history>
    /// [michan]  28/04/2016/ Created
    /// </history>
    public void ResetParametersExchangeRate()
    {
      // actualizamos la fecha y hora de la ultima altualización
      _lastExchangeRate = DateTime.Now;
      lblLastTransferExchangeRate.Content = lblTextLast + _lastExchangeRate.ToString();
      
      // actualizamos la fecha y hora de la siguiente actualización
      _nextExchangeRate = DateHelper.AddTimeDate(_tranferExchangeRatesIntervalTime);
      // actualizamos el label de estatus de la actualización
      lblNextTransferExchangeRate.Content = lblTextNext + _nextExchangeRate.ToString();
     
      // regresamos a cero el progressbar
      AddValueProgressBarExchangeRate(value: 0);
      lblPorcentProgressExchangeRate.Content = string.Empty;
     
      // cambiamos el estatus de la bandera de actualización
      blnRunOrCancelExchangeRate = false;
      // actualizamos el label del button
      UpdateButton(btnExchangeRate, "Update Exchange Rate");
      btnExchangeRate.IsEnabled = true;
      // detiene el efecto blink
      OnOffBlinkExchangeRate(false);
      // Pone en stand by el label de estatus.
      UpdateLabelStatusExchangeRate("STAND BY");

    }
    #endregion
    
    #endregion

    #region Actualiza Labels

    #region UpdateLabelTrasnferReservations
    /// <summary>
    /// Metodo para actualizar las transferencias realizadas en las reservaciones
    /// </summary>
    /// <param name="stringContent">cadena que contiene la trasnferencia realizada</param>
    public void UpdateLabelTrasnferReservations(string stringContent)
    {
      // validamos que el conetenido no sea nullo o vacio.
      lblTransferReservations.Content = (!String.IsNullOrEmpty(stringContent) && !String.IsNullOrWhiteSpace(stringContent)) ? stringContent : ""; ;
    }
    #endregion

    #region UpdateLabelStatusExchangeRate
    /// <summary>
    /// Metodo que actualiza el label de esatus de las transferencias de exchange rate
    /// </summary>
    /// <param name="stringContent">contenido del label status</param>
    public void UpdateLabelStatusExchangeRate(string stringContent)
    {
      lblStatusExchangeRate.Content = stringContent;
    }
    #endregion

    #region UpdateLabelStatusReservations
    /// <summary>
    /// Metodo que actualiza el label de esatus de las transferencias de reservaciones
    /// </summary>
    /// <param name="stringContent">contenido del label status</param>
    public void UpdateLabelStatusReservations(string stringContent)
    {
      lblStatusReservations.Content = stringContent;
    }
    #endregion

    #endregion

    #region Actualiza ProgresBar

    #region InitializerProgressBar
    ///<summary>Metodo que inicializa y reestablece los valores de la barra de progreso</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void InitializerProgressBar()
    {
      //Configuración del ProgressBar de exchange rate
      
      progresBarExchangeRate.Minimum = 0;//valor mínimo (inicio de la barra de carga)
      progresBarExchangeRate.Maximum = 100;//valor máximo(hasta donde se carga, como ejemplo 100)
      progresBarExchangeRate.Value = 0;//valor de inicio
      lblPorcentProgressExchangeRate.Content = "";//String.Format("{0:0%}", 0);

      //Configuración del ProgressBar de reservaciones
      progresBarReservations.Minimum = 0;//valor mínimo (inicio de la barra de carga)
      progresBarReservations.Maximum = 100;//valor máximo(hasta donde se carga, como ejemplo 100)
      progresBarReservations.Value = 0;//valor de inicio
      
      lblPorcentProgresBarReservations.Content = "";//String.Format("{0:0%}", 0);


    }
    #endregion

    #region AddValueProgressBar
    ///<summary>Metodo que actualiza la barra de progreso</summary>
    ///<param name="value">recibe el valor de la carra de progreso del 1 al  10</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void AddValueProgressBar(System.Windows.Controls.ProgressBar progressBar, System.Windows.Controls.Label lblProcent,  double? value = null, double? porcentBase = null)
    {
      int valueIncrement = 0;
      double porcentValue = 0.00;
      if (porcentBase != null)
      {
        porcentValue = (double)porcentBase.Value;
        // pasamos el valor del prociento a escala de 100
        // obtenemos la equivalencia en la escala de 100
        double toDouble = ((porcentValue / 100) * 100);
        valueIncrement = (int)toDouble;
      }
      else
      {
        if (value != null)
        {
          valueIncrement = (int)(value.Value * 10);
          if (valueIncrement > 0)
          {
            //porcentValue = (valueIncrement / 100) + 0.00;
            porcentValue = Porcent(valueIncrement);
          }
        }
      }
      if ((valueIncrement >=0) && (porcentValue >=0) && (!valueIncrement.Equals(null)) && (!porcentValue.Equals(null)))
      {
        progressBar.Value = valueIncrement;
        lblProcent.Content = String.Format("{0:0} %", porcentValue);
      }
      

    }
    #endregion

    #region Porcent
    /// <summary>
    /// Metodo para calcular el porcentaje
    /// </summary>
    /// <param name="partial">Valor a calcular su porcentaje</param>
    /// <param name="total">Valor maximo sobre el que se saca el procentaje</param>
    /// <returns></returns>
    public double Porcent(int partial, int? total = -1)
    {
      float dividendoF = (float)partial;
      float divisorF = 100;
      if (total.Value > 0)
      {
        divisorF = (float)total.Value;
      }
      float division = (dividendoF * 100) / divisorF;
      double mult = Math.Pow(10.0, 2);
      double result = Math.Truncate(mult * division) / mult;
      return result;
    }
    #endregion

    #region AddValueProgressBarReservations
    ///<summary>Metodo que actualiza la barra de progreso</summary>
    ///<param name="porcentBase">recibe el valor en porcentaje para la barra de progreso</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void AddValueProgressBarReservations(double porcentBase)
    {
      AddValueProgressBar(progressBar: progresBarReservations, lblProcent: lblPorcentProgresBarReservations, porcentBase:porcentBase);
    }
    #endregion

    #region AddValueProgressBarExchangeRate
    ///<summary>Metodo que actualiza la barra de progreso</summary>
    ///<param name="value">recibe el valor de la barra de progreso del 1 al  10</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void AddValueProgressBarExchangeRate(double value)
    {
      AddValueProgressBar(progressBar: progresBarExchangeRate, lblProcent: lblPorcentProgressExchangeRate, value:value);
    }
    #endregion

    #endregion

    #region Transferencia de Exchange Rate

    #region TransferExchangeRates
    ///<summary>Metodo que contiene la tarea que realiza todo el proceso de actualizacion de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public async Task TransferExchangeRates()
    {
      UpdateLabelStatusExchangeRate("STARTED");
      //Se obtiene la fecha del servidor
      _dtmServerDate = BRHelpers.GetServerDate();
      AddLogGridExchangeRate("Start", "Start Updating Exchange Rates");
      AddValueProgressBarExchangeRate(value: 1);
      Thread.Sleep(100);

      //agregamos los tipos de cambio faltantes hasta la fecha actual para que no existan huecos
      UpdateLabelStatusExchangeRate("PROCESSING");
      await BRExchangeRate.InsertExchangeRate(_dtmServerDate);
      AddValueProgressBarExchangeRate(value: 2);
      // actualizamos los tipos de cambio
      await UpdateExchangeRatesFromIntranet();
      // actualizamos el tipo de cambio Canadience
      await UpdateExchangeRatesFromHotel();

    }
    #endregion

    #region UpdateExchangeRatesFromIntranet
    public async Task UpdateExchangeRatesFromIntranet()
    {
      //Se obtiene la fecha del servidor
      _dtmServerDate = BRHelpers.GetServerDateTime();
      //obtenemos el tipo de cambio de la Intranet
      AddValueProgressBarExchangeRate(value: 2);
      AddLogGridExchangeRate("Info", "Getting Exchange Rate from Intranet Service");
      TipoCambioTesoreria exchangeRate = await IntranetHelper.TipoCambioTesoreria(_dtmServerDate, "USD", cancelTokenExchangeRate.Token);
      if (exchangeRate != null)
      {
        timeWatchExchangeRate.Start();
        _exchangeRateType = Convert.ToDecimal(exchangeRate.TipoCambio);

        AddValueProgressBarExchangeRate(value: 3);
        //comparamos si el cambio es positivo y mayor a cero
        if (_exchangeRateType > 0)
        {
          cancelTokenExchangeRate.Token.ThrowIfCancellationRequested();
          AddValueProgressBarExchangeRate(value: 4);
          _currencyId = "MX";
          //Se realiza la actualizacion y se registra en el log la inicialización
          AddLogGridExchangeRate("Info", $"Updating Exchange Rate { _currencyId } ({_exchangeRateType})");
          // actualizamos el exchange rate
          await BRExchangeRate.UpdateExchangeRate(_dtmServerDate, _currencyId, _exchangeRateType);
          timeWatchExchangeRate.Stop();

          TimeSpan tSpan = timeWatchReservations.Elapsed;
          // Convertimos el tiempo de obtencion en string para pintar en el grid
          string elapsedTime = StringTimeDifference(tSpan);

          AddLogGridExchangeRate("Info", $"Updating Exchange Rate in {elapsedTime}");
          AddLogGridExchangeRate("Success", "Exchange Rate finished");
        }
        else
        {
          // Registramos en el log el tipo de error que se encontro, en el tipo de cambio
          AddLogGridExchangeRate("Error", $"Exchange rate must be positive { _currencyId } ({ _exchangeRateType})");
        }
      }
      else
      {
        // Registramos en el log que no existen cambios en la fecha especificada y el tipo de moneda
        AddLogGridExchangeRate("Warning", "Exchange rate does not exists for day");
      }
      AddValueProgressBarExchangeRate(value: 5);
    }
    #endregion

    #region UpdateExchangeRatesFromHotel
    /// <summary>
    /// Actualizamos el tipo de cambio de divisa canadiense
    /// </summary>
    /// <returns></returns>
    public async Task UpdateExchangeRatesFromHotel()
    {
      cancelTokenExchangeRate.Token.ThrowIfCancellationRequested();
      AddValueProgressBarExchangeRate(value: 6);
      AddLogGridExchangeRate("Info", "Updating CurrencyChange...");
      //Se obtiene la fecha del servidor
      _dtmServerDate = BRHelpers.GetServerDateTime();
      //agregamos los tipos de cambio faltantes hasta la fecha actual para que no existan huecos
      await BRExchangeRate.InsertExchangeRate(_dtmServerDate);
      AddValueProgressBarExchangeRate(value: 7);
      // obtenemos el tipo de cambio de la Intranet
      AddLogGridExchangeRate("Info", "Getting Currency Change from Intranet Service...");
      Rmmoney currencyChange = await HotelServiceHelper.ObtenerFactoresConversion("CP", cancelTokenExchangeRate.Token);

      // validamos que exista el tipo de cambio para el dia actual
      if (currencyChange != null)
      {
        // validamos que el tipo de cambio sea positivo
        if (currencyChange.factor > 0)
        {
          // actualizamos el tipo de cambio
          cancelTokenExchangeRate.Token.ThrowIfCancellationRequested();
          AddValueProgressBarExchangeRate(value: 8);
          _currencyId = "CAN";
          AddLogGridExchangeRate("Info", "Updating currencyChange...");
          decimal factor = (1 / currencyChange.factor);
          await BRExchangeRate.UpdateExchangeRate(_dtmServerDate, _currencyId, factor);
          AddLogGridExchangeRate("Info", "Update completed...");
        }
        else
        {
          AddLogGridExchangeRate("Info", $"Canadian currency must be positive ({currencyChange.factor}).");
        }
      }
      else
      {
        AddLogGridExchangeRate("Info", $"Canadian Currency Change does not exists for day { _dtmServerDate:dd/MMM/yyyy}");
      }
      AddValueProgressBarExchangeRate(value: 10);
    }
    #endregion

    #endregion

    #region Metodos de Ventana

    #region Window_Loaded
    /// <summary>
    /// Metodo para cargar componentes iniciales del formulario.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <history>
    /// [michan]  03/May/2016 Created
    /// </history>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      WindowsHelper.EventsKeys(window:this, blnNotifycon:false);
      // cargamos el contenido de los grid.
      await LoadDataGRidExchangeRate();
      await LoadDataGRidReservations();
      // inicializamos los valores default de los progres bar.
      InitializerProgressBar();
        
      // ontenemos el titulo y version del transfer
      var thisApp = Assembly.GetExecutingAssembly();
      AssemblyName name = new AssemblyName(thisApp.FullName);
      this.Title = " Inteligence Marketing Transfer v" + name.Version;

      // inicializamos la fecha actual en el form.
      lblDate.Content = DateTime.Now.ToString("dd/MM/yyyy");
      lblStatusExchangeRate.Content = "STAND BY";
      lblStatusReservations.Content = "STAND BY";
      lblTransferReservations.Content = "";
      lblLastTransferReservations.Content = lblTextLast + _lastReservations.ToString();
      lblNextTransferExchangeRate.Content = "";
      lblLastTransferExchangeRate.Content = lblTextLast + _lastExchangeRate.ToString();
      lblNextTransferReservations.Content = "";
      
      InitializeTransfers();
      
    }
    #endregion

    #region StatusForm
    /// <summary>
    /// Metodo que valida el estatus de formulario
    /// </summary>
    ///<history>
    ///[michan] 28/04/2016 Created
    ///</history>
    public void StatusForm()
    {
      // se verifica si el formulario esta maximizado.
      Dispatcher.Invoke
      (
        new Action
        (() =>
        {
          // si esta oculto se maximiza
          if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Minimized)
          {
            System.Windows.Application.Current.MainWindow.WindowState = WindowState.Normal;
          }
      
      }
      ),
      DispatcherPriority.ContextIdle
    );

    }
    #endregion

    #endregion

    #region Metodos de acciones de botones

    #region Run or Cancel Reservations
    /// <summary>
    /// Acción para cancelar o inicializar la trasnferencia de reservaciones
    /// </summary>
    /// <history>
    /// [michan] 28/04/2016 Created
    /// </history>
    private void btnReservations_Click(object sender, RoutedEventArgs e)
    {
      // valida si se esta realizando la actualización de reservaciones.
      if (blnRunOrCancelReservations)
      {
        // pregunta, si desea cancelar la actualización de reservaciones
        if (UIHelper.ShowMessage("You want to cancel the update of reservciones?", MessageBoxImage.Question, "Cancel Reservations") == MessageBoxResult.Yes)
        {
          // en caso de aceptar se invoca el metodo para cancelar las actualizaiones.
          CancelWorkerReservations();
        } 
      }
      else
      {
        // si no se estas transferiendo
        if (!blnRunOrCancelReservations)
        {
          // se ejecuta metodo para iniciar las actualizaciones.
          StartReservations();
        }
        else
        {
          // en caso de que se este actualizando se muestra mensaje de espera.
          Task.Factory.StartNew(() =>
          {
            UIHelper.ShowMessage("An update is running , please wait...", MessageBoxImage.Error, "RunReservations");
          });
        }

      }
    }
    #endregion

    #region Run or Cancel Exchange Rate
    /// <summary>
    /// Acción para cancelar o inicializar la trasnferencia de Exchange Rate
    /// </summary>
    /// <history>
    /// [michan] 28/04/2016 Created
    /// </history>
    private void btnExchangeRate_Click(object sender, RoutedEventArgs e)
    {
      // valida si se esta realizando la actualización
      if (blnRunOrCancelExchangeRate)
      {
        // pregunta si se desea detener la actualizacion de exchange rate
        if (UIHelper.ShowMessage("You want to cancel the update of Exchange Rate?", MessageBoxImage.Question, "Update Cancel Exchange Rate") == MessageBoxResult.Yes)
        {
          // si se desea detener, se invoca el metodo para cancelar las actualizaciones.
          CancelWorkerExchangeRate();
        }
      }
      else
      {
        // valida si no se esta realizando la actualizacion de exchange rate.
        if (!blnRunOrCancelExchangeRate)
        {
          // si no se esta ejecutnado la actualizacion, se invoca el metodo para ejecutar las aactualizaciones.
          StartExchangeRate();
        }
        else
        {
          // en caso de que se este ejecutando se muestra el mensaje de que se esta actualizando.l
          Task.Factory.StartNew(() =>
          {
            UIHelper.ShowMessage("An update is running , please wait...", MessageBoxImage.Error, "RunExchangeRate");
          });
          
        }

      }
      
    }
    #endregion}

    #endregion

    

  }

  
}
