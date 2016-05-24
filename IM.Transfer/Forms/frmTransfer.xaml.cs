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

    #region Lista de registros de transacciones para datagrid
    public static List<LogHelper.Transaction> listTransactionsExchangeReservations = new List<LogHelper.Transaction>();
    public static List<LogHelper.Transaction> listTransactionsReservations = new List<LogHelper.Transaction>();
    #endregion

    #region Instancias de delegados
    //Instancia de delegado para la actualizacion del grid de Exchange Rate
    public UpdateDelegateGridExchangeRate updateGridExchangeRate = null;
    //Instancia de delegado para la actualizacion del grid de las reservaciones.
    public UpdateDelegateGridReservations updateGridReservations = null;
    //Instancia de delegado para la actualizcion de label de los botones.
    public UpdateDelegateButton updateContentButton = null;
    // Instancia de delegado para la actualizacion del texto de labels.
    UpdateDelegateLabel updateDelegateLabel = null;
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
    public static bool blnOneNight = false;
    public static bool blnTwoNight = false;
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
    public bool blnRunTransfer = false;
    public bool blnRunOrCancelReservations = false; // valida el estatus de la transferencia de reservaciones
    public bool blnRunOrCancelExchangeRate = false; // valida el estatus de la transferencia de exchange rate
    #endregion

    #endregion
    
    #region Contructores y Destructores
    public frmTransferLauncher()
    {
      InitializeComponent();

      //Inicializa los parametros de la aplicación.
      InitializeValuesParameters();
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
        blnCallTransfer = _iniFileHelper.readBool("TransferReservations", "CallTransfer", true);
        blnTransfer = _iniFileHelper.readBool("TransferReservations", "Transfer", true);
        blnTransferToGuests = _iniFileHelper.readBool("TransferReservations", "TransferToGuests", true);
      }

    }
    #endregion

    #region InitializeTransfers
    ///<summary>Metodo que inicializa y ejecuta cuando se cumple la validación para la actualización del Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void InitializeTransfers() {
      //INSTANCIANDO EL TIMER CON LA CLASE DISPATCHERTIMER 
      DispatcherTimer dispathcerT = new DispatcherTimer();
      //EL INTERVALO DEL TIMER ES DE HORAS, MINUTOS Y SEGUNDOS QUE SE PASAN COMO PARAMETRO 
      dispathcerT.Interval = _standbyIntervalTimeExchangeRate;//new TimeSpan(0, 0, Convert.ToInt32(_standbyIntervalTime));
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
    public async void StartReservations()
    {
      // Se valida el estatus del formulario
      StatusForm();
      cancelTokenReservations = new CancellationTokenSource();
      // ponemos en verdadero la ejecucion de transferencia de reservaciones
      blnRunOrCancelReservations = true;
      // cambiamos el testo del boton para cancelar la transferemcia
      UpdateButton(btnReservations, "Cancel updating of Reservations");
      // iniciamos el efecto blink en el label de status 
      OnOffBlinkReservations();
      try { 
        await DoWorkerTransferReservations(cancelTokenReservations.Token);
      }
      catch (OperationCanceledException)
      {
        AddLogGridReservations("Info", "Transfer Canceled.");
      }
      catch (Exception)
      {
        AddLogGridReservations("Info", "Transfer Canceled.");
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
    public async void StartExchangeRate()
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
        await WorkerUpdateExchangeRateDoWork(cancelTokenExchangeRate.Token);
      }
      catch (OperationCanceledException)
      {
        AddLogGridExchangeRate("Info", "Update Canceled.");
      }
      catch (Exception)
      {
        AddLogGridExchangeRate("Info", "Update Canceled.");
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
    private async void LoadDataGRidExchangeRate()
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
          UpdatingGrid(grdLogExchangeRate, listTransactionsExchangeReservations);
        }
      
    }
    #endregion

    #region LoadDataGRidReservations
    ///<summary>Metodo que carga e inicializa el grid del log de Reservations</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private async void LoadDataGRidReservations()
    {
      //await Task.Run(() => {
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
          UpdatingGrid(grdLogReservations, listTransactionsReservations);
        }
      //});
      

    }
    #endregion

    #endregion

    #region Metodos para transferencia de reservaciones

    #region DoWorkerTransferReservations
    /// <summary>
    /// Inicia la transferencia de reservaciones
    /// </summary>
    /// <history>
    /// [michan] 23/04/2016 Created
    /// </history>
    public async Task DoWorkerTransferReservations(CancellationToken cancelToken)//object Sender, DoWorkEventArgs e
    {
      cancelToken.ThrowIfCancellationRequested();
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
        // si se cancelo
        cancelToken.ThrowIfCancellationRequested();
        // validamos que hayan hoteles para la zona     
        if (!(String.IsNullOrEmpty(zone.znID) && !String.IsNullOrWhiteSpace(zone.znID)) && (!String.IsNullOrEmpty(zone.znN) && !String.IsNullOrWhiteSpace(zone.znN)) && (!String.IsNullOrEmpty(zone.znZoneHotel) && !String.IsNullOrWhiteSpace(zone.znZoneHotel)))
          {
            //exportamos las reservaciones a la tabla de transferencia
            int itotalReservations = await ExportToTransfer(cancelToken, zone.znID, zone.znN, zone.znZoneHotel);

            // si se cancelo
            cancelToken.ThrowIfCancellationRequested();

            // valida si hay registro para transferir si hay 
            // exportamos los registros de la tabla de transferencia a la tabla de huespedes
            if (itotalReservations>0) await ExportToGuests(cancelToken);

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
    public async Task<int> ExportToTransfer(CancellationToken cancelToken, string zoneID, string zoneName, string zoneHotel)
    {
      // validamos si se cancelo la actualizacion de registros
      cancelToken.ThrowIfCancellationRequested();
      // obtenemos las reservaciones
      List<ReservationOrigosTransfer> reservationOrigosTransfer = await GetReservations(cancelToken, zoneID, zoneName, zoneHotel);
      int iCountReservations = 0;
      string strTransferingReservation = "";
      // validamos que hayan registros por actualizar.
      if ((reservationOrigosTransfer != null && reservationOrigosTransfer.Count > 0))
      {
        // validamos si se cancelo la actualizacion de registros
        cancelToken.ThrowIfCancellationRequested();

        iCountReservations = reservationOrigosTransfer.Count;
       
        // transfiriendo reservaciones a la base de datos.
        AddLogGridReservations("Insert", "Transfering Reservations to Database.");
        //limpiamos la tabla de transferencia
        await BRTransfer.DeleteTransfer();
        await BRTransfer.GetTransfer();

        // transferimos las reservaciones a la base de datos
        int cont = 0;
        double dPorcent = 0;
          
        foreach (ReservationOrigosTransfer reservationOrigos in reservationOrigosTransfer)
        {
          // validamos si se cancelo la actualizacion de registros
          cancelToken.ThrowIfCancellationRequested();

          //exportamos las reservaciones a la tabla de transferencia
          //cont = cont + 1;
          dPorcent = Porcent(cont++, iCountReservations);
          AddValueProgressBarReservations(porcentBase: dPorcent);
          // desplegamos la reservacion que se esta transfiriendo
          strTransferingReservation = $"Transfering Reservation {reservationOrigos.Hotel} No. {reservationOrigos.Folio} \r\nCheck In Date: {reservationOrigos.Arrival:dd/MMM/yyyy}";
          UpdateLabelTrasnferReservations(strTransferingReservation);
          //agregamos la reservacion transferida
          await AddTransfer(cancelToken, reservationOrigos);
        }
          
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
    public async Task AddTransfer(CancellationToken cancelToken, ReservationOrigosTransfer reservationOrigos)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      // localizamos la reservacion
      bool existReservation = await BRTransfer.ExistReservation(reservationOrigos.Hotel, reservationOrigos.Folio.ToString());
      if (!existReservation)
      {
        // agregamos la reservacion
        //si la reservacion no ha sido agregada todavia
        Model.Transfer transfer = Transfer(reservationOrigos);
        await BRTransfer.AddReservation(transfer);

        // Agregamos la informacion de Origen a GuestOpera
        await AddTransferToOperaGuest(cancelToken, reservationOrigos);
      }
      else
      {
        AddLogGridReservations("Warning", "Reservation duplicated. Hotel: " + reservationOrigos.Hotel.ToString() + ", Folio: " + reservationOrigos.Folio.ToString());
      }
    }
    #endregion

    #region AddTransferToOperaGuest
    /// <summary>
    /// Agrega la informaicon de la reservacion de Opera  a la tabla GuestOpera
    /// </summary>
    /// <param name="reservationOrigos">Reservacion obtenida en web service</param>
    /// <history>
    /// [michan]  23/04/2016  Created
    /// </history>
    public async Task AddTransferToOperaGuest(CancellationToken cancelToken, ReservationOrigosTransfer reservationOrigos)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      //Si hay GuestID, se asigna, de lo contrario es OutHouse y no se agrega
      Model.Guest guest = await BRGuests.GetGuestValidForTransfer(reservationOrigos.Folio.ToString(), reservationOrigos.Hotel);
      if ((guest != null))
      {
        // Revisamos si ya existe en Guest
        GuestOpera guestOpera = await BRGuestOpera.GetGuestOpera(guest.guID);
        GuestOpera guOpera = null;
        // Si hay GuestID, se asigna, de lo contrario es OutHouse y no se agrega
        if (guestOpera != null)
        {
          // Si ya existe, se actualiza el valor
          guOpera = await UpdateGuestOpera(reservationOrigosTransfer: reservationOrigos, gOpera: guestOpera);
          await BRGuestOpera.SaveGuestOpera(guestOpera, false);
        }
        else
        {
          // Si no existe, se agrega
          guOpera = await UpdateGuestOpera(reservationOrigosTransfer: reservationOrigos, gogu: guest.guID);
          await BRGuestOpera.SaveGuestOpera(guOpera, true);
        }
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
    public async Task<List<ReservationOrigosTransfer>> GetReservations(CancellationToken cancelToken, string zoneID, string zoneName, string zoneHotel)
    {
      // validamos si se cancelo la actualizacion de registros
      cancelToken.ThrowIfCancellationRequested();

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
      string leadSourceID = await StringIDHoteles(hotels);
      //si no hay hoteles en la zona
      if ((hotels.Count > 0) && (hotels != null) && dateFrom <= dateTo)
      {
        // validamos si se cancelo la actualizacion de registros
        cancelToken.ThrowIfCancellationRequested();
        // obtenemos las reservaciones e la zona
        AddLogGridReservations("Info", $"Getting Reservations from {zoneName} (Check In Date: {DateHelper.DateRange(dateFrom, dateTo)} , Hotels: {leadSourceID} ).");
        timeWatchReservations.Start();
        reservations = await HotelServiceHelper.GetReservationsByArrivalDate(zoneHotel, dateFrom, dateTo, leadSourceID, cancelToken);
        timeWatchReservations.Stop();
        
        // valida si se obtubieron reservaciones
        if (reservations != null && reservations.Count > 0)
        {
          // validamos si se cancelo la actualizacion de registros
          cancelToken.ThrowIfCancellationRequested();
          // Calculamos el tiempo de obtencion de las reservaciones
          TimeSpan tSpan = timeWatchReservations.Elapsed;
          // Convertimos el tiempo de obtencion en string para pintar en el grid
          string elapsedTime = StringTimeDifference(tSpan);
          AddLogGridReservations("Info", reservations.Count.ToString() + " Reservations were Obtained in " + elapsedTime);
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
    public Model.Transfer Transfer(ReservationOrigosTransfer reservationOrigosTransfer)
    {
      Model.Transfer transfer = new Model.Transfer();

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

      return transfer;
    }
    #endregion

    #region UpdateGuestOpera
    /// <summary>
    /// Metodo para crear un nuevo registro para la tabla Trasnfer
    /// </summary>
    /// <param name="reservationOrigosTransfer">Datos obtenidos del web service</param>
    /// <param name="gogu">ID del registro que proviene de la tabla Guest</param>
    /// <param name="gOpera">Registro del guestOpera ha actualizar</param>
    /// <returns></returns>
    /// <history>
    /// [michan]  25/04/2016  Created
    /// </history>
    public async Task<GuestOpera> UpdateGuestOpera(ReservationOrigosTransfer reservationOrigosTransfer, int? gogu = null,GuestOpera gOpera = null)
    {
      // Creamos un objecto GuestOpera
      GuestOpera guestOpera = null;
      await Task.Run(() =>
      {
        // si recibimos un objecto GuestOpera 
        if (gOpera != null)
        {
          // asignamos el objeto gOpera al objeto guestOpera creado
          guestOpera = gOpera;
        }
        else
        {
          // instnaceamos un objecto de tipo de tipo GuestOpera
          guestOpera = new GuestOpera();
          guestOpera.gogu = gogu.Value;
        }
        guestOpera.goSourceID = ConvertHelper.StringToInt(reservationOrigosTransfer.Source_id);
        guestOpera.goTravelAgentID = ConvertHelper.StringToInt(reservationOrigosTransfer.Travel_Agent_Id);
        guestOpera.goGroupID = ConvertHelper.StringToInt(reservationOrigosTransfer.Group_id);
        guestOpera.goBlockCode = reservationOrigosTransfer.Block_code;
        guestOpera.goMarketCode = reservationOrigosTransfer.Market_code;
        guestOpera.goMarketGroup = reservationOrigosTransfer.Market_group;
        guestOpera.goSourceGroup = reservationOrigosTransfer.Source_group;
        guestOpera.goRegion = reservationOrigosTransfer.Source_Region;
        guestOpera.goCountry = reservationOrigosTransfer.Source_Country;
        guestOpera.goTerritory = reservationOrigosTransfer.Source_Territory;
      });
      return guestOpera;
    }
    #endregion

    #region ExportToGuests
    /// <summary>
    /// Exporta los registros de la tabla de transferencia a la tabla de huespedes
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task ExportToGuests(CancellationToken cancelToken)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      //procesando registros transferidos
      AddLogGridReservations("Info", "Processing Transferred Records.");
      //actualizamos el catalogo de paises
      await UpdateCountries(cancelToken);
      //actualizamos el catalogo de agencias
      await UpdateAgencies(cancelToken);
      //actualizamos el catalogo de tipos de habitacion
      await UpdateRoomTypes(cancelToken);
      //actualizamos el catalogo de contratos
      await UpdateContracts(cancelToken);
      //actualizamos el catalogo de grupos
      await UpdateGroups(cancelToken);
      //actualizamos la tabla de transferencia
      await UpdateTransfer(cancelToken);
      //actualizamos los huespedes
      await UpdateGuests(cancelToken);
    }
    #endregion

    #region UpdateCountries
    /// <summary>
    /// actualizamos el catalogo de paises
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateCountries(CancellationToken cancelToken)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      try
      { 
        // agregando paises de Hotel
        AddLogGridReservations("Insert", "Adding Hotel Countries.");
        await BRTransfer.AddCountriesHotel();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      { 
        // agregando paises de Origos
        AddLogGridReservations("Insert", "Adding Origos Countries.");
        await BRTransfer.AddCountries();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      { 
        // actualizando las descripciones de los paises de Hotel
        AddLogGridReservations("Update", "Updating Hotel Countries Names.");
        await BRTransfer.UpdateCountriesHotelNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      { 
        // actualizando las descripciones de los paises de Origos
        AddLogGridReservations("Update", "Updating Origos Countries Names.");
        await BRTransfer.UpdateCountriesNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }
      
    }
    #endregion

    #region UpdateAgencies
    /// <summary>
    /// Actualiza el catalogo de agencias
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateAgencies(CancellationToken cancelToken)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      try
      { 
        //agregando agencias de Hotel
        AddLogGridReservations("Insert", "Adding Hotel Agencies.");
        await BRTransfer.AddAgenciesHotel();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      { 
        //agregando agencias de Origos
        AddLogGridReservations("Insert", "Adding Origos Agencies.");
        await BRTransfer.AddAgencies();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      { 
        //actualizando las descripciones de las agencias de Hotel
        AddLogGridReservations("Update", "Updating Hotel Agencies Names.");
        await BRTransfer.UpdateAgenciesHotelNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      { 
        //actualizando las descripciones de las agencias de Origos
        AddLogGridReservations("Update", "Updating Origos Agencies Names.");
        await BRTransfer.UpdateAgenciesNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }
    
    }
    #endregion

    #region UpdateRoomTypes
    /// <summary>
    /// Actualiza el catalogo de tipos de habitacion
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateRoomTypes(CancellationToken cancelToken)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      try
      { 
        //agregando tipos de habitacion
        AddLogGridReservations("Insert", "Adding Room Types.");
        await BRTransfer.AddRoomTypes();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      { 

        //actualizando las descripciones de los tipos de habitacion
        AddLogGridReservations("Update", "Updating Room Types Names.");
        await BRTransfer.UpdateRoomTypesNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }
      
    }
    #endregion

    #region UpdateContracts
    /// <summary>
    /// Actualiza el catalogo de contratos
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateContracts(CancellationToken cancelToken)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      try
      { 
        //agregando contratos
        AddLogGridReservations("Insert", "Adding Contracts.");
        await BRTransfer.AddContracts();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }
      try
      {
        //actualizando las descripciones de los contratos
        AddLogGridReservations("Update", "Updating Contracts Names.");
        await BRTransfer.UpdateContractsNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }
      
    }
    #endregion

    #region UpdateGroups
    /// <summary>
    /// Actualiza el catalogo de grupos
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateGroups(CancellationToken cancelToken)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      try
      {
        // agregando contratos
        AddLogGridReservations("Insert", "Adding Groups.");
        await BRTransfer.AddGroups();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      { 
        //actualizando las descripciones de los contratos
        AddLogGridReservations("Update", "Updating Groups Names");
        await BRTransfer.UpdateGroupsNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
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
    public async Task UpdateTransfer(CancellationToken cancelToken)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      try
      { 
        //actualizando paises de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Countries");
        await BRTransfer.UpdateTransferCountries();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {

        //actualizando agencias de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Agencies");
        await BRTransfer.UpdateTransferAgencies();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando idiomas de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Languages");
        await BRTransfer.UpdateTransferLanguages();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando mercados de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Markets");
        await BRTransfer.UpdateTransferMarkets();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando disponibilidad por grupos
        AddLogGridReservations("Update", "Updating Unavailable Motives by Groups");
        await BRTransfer.UpdateTransferUnavailableMotivesByGroups();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por agencia
        AddLogGridReservations("Update", "Updating Unavailable Motives by Agency");
        await BRTransfer.UpdateTransferUnavailableMotivesByAgency();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por pais
        AddLogGridReservations("Update", "Updating Unavailable Motives by Country");
        await BRTransfer.UpdateTransferUnavailableMotivesByCountry();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por contrato
        AddLogGridReservations("Update", "Updating Unavailable Motives by Contract");
        await BRTransfer.UpdateTransferUnavailableMotivesByContract();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

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
          AddLogGridReservations("Error", exception.InnerException.Message);
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
          AddLogGridReservations("Error", exception.InnerException.Message);
        }
      }

      try
      {

        //actualizando motivos de indisponibilidad de la tabla de transferencia por transferencia
        AddLogGridReservations("Update", "Updating Unavailable Motives by Transfer");
        await BRTransfer.UpdateTransferUnavailableMotivesByTransfer();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por ser nuevo socio
        AddLogGridReservations("Update", "Updating Unavailable Motives by New Member");
        await BRTransfer.UpdateTransferUnavailableMotivesByNewMember();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando motivos de indisponibilidad de la tabla de transferencia por tener pax 1
        AddLogGridReservations("Update", "Updating Unavailable Motives by Pax");
        await BRTransfer.UpdateTransferUnavailableMotivesByPax();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando disponibilidad de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Availability");
        await BRTransfer.UpdateTransferAvailability();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }
      
    }
    #endregion

    #region UpdateGuests
    /// <summary>
    /// Actualiza los huespedes
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public async Task UpdateGuests(CancellationToken cancelToken)
    {
      // validamos si se cancelo la actualizacion
      cancelToken.ThrowIfCancellationRequested();

      try
      {
        //eliminando reservaciones canceladas
        AddLogGridReservations("Delete", "Deleting Canceled Reservations.");
        await BRTransfer.DeleteReservationsCancelled();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando numeros de habitacion de huespedes
        AddLogGridReservations("Update", "Updating Room Numbers");
        await BRTransfer.UpdateGuestsRoomNumbers();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando tipos de habitacion de huespedes
        AddLogGridReservations("Update", "Updating Room Types");
        await BRTransfer.UpdateGuestsRoomTypes();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando tarjetas de credito de huespedes
        AddLogGridReservations("Update", "Updating Credit Cards");
        await BRTransfer.UpdateGuestsCreditCards();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando apellidos de huespedes
        AddLogGridReservations("Update", "Updating Last Names");
        await BRTransfer.UpdateGuestsLastNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando nombres de huespedes
        AddLogGridReservations("Update", "Updating First Names");
        await BRTransfer.UpdateGuestsFirstNames();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando fechas de llegada de huespedes
        AddLogGridReservations("Update", "Updating Check-In Dates");
        await BRTransfer.UpdateGuestsCheckInDates();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando Check Ins de huespedes
        AddLogGridReservations("Update", "Updating Check-Ins");
        await BRTransfer.UpdateGuestsCheckIns();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando fechas de salida de huespedes
        AddLogGridReservations("Update", "Updating Check-Out Dates");
        await BRTransfer.UpdateGuestsCheckOutDates();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando correos electronicos de huespedes
        AddLogGridReservations("Update", "Updating E-mails");
        await BRTransfer.UpdateGuestsEmails();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando ciudades de huespedes
        AddLogGridReservations("Update", "Updating Cities");
        await BRTransfer.UpdateGuestsCities();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando estados de huespedes
        AddLogGridReservations("Update", "Updating States");
        await BRTransfer.UpdateGuestsStates();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando paises de huespedes
        AddLogGridReservations("Update", "Updating Countries");
        await BRTransfer.UpdateGuestsCountries();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando salidas anticipadas de huespedes
        AddLogGridReservations("Update", "Updating Early Check-Outs");
        await BRTransfer.UpdateGuestsCheckOutsEarly();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando tipos de huespedes
        AddLogGridReservations("Update", "Updating Guest Types");
        await BRTransfer.UpdateGuestsGuestTypes();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando contratos de huespedes
        AddLogGridReservations("Update", "Updating Contracts");
        await BRTransfer.UpdateGuestsContracts();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando fechas de nacimiento de huespedes
        AddLogGridReservations("Update", "Updating Birth Dates");
        await BRTransfer.UpdateGuestsBirthDates();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {

        //actualizando edades de huespedes
        AddLogGridReservations("Update", "Updating Ages");
        await BRTransfer.UpdateGuestsAges();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando pax de huespedes
        AddLogGridReservations("Update", "Updating Pax");
        await BRTransfer.UpdateGuestsPax();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando tipos de reservacion de huespedes
        AddLogGridReservations("Update", "Updating Reservation Types");
        await BRTransfer.UpdateGuestsReservationTypes();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando id del perfil de Opera de huespedes
        AddLogGridReservations("Update", "Updating Id Profile Opera");
        await BRTransfer.UpdateGuestsIdProfileOpera();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando agencias de huespedes
        AddLogGridReservations("Update", "Updating Agencies");
        await BRTransfer.UpdateGuestsAgencies();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando mercados de huespedes
        AddLogGridReservations("Update", "Updating Markets");
        await BRTransfer.UpdateGuestsMarkets();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando disponibilidad y motivos de indisponibilidad de huespedes
        AddLogGridReservations("Update", "Updating Availability & Unavailable Motives");
        await BRTransfer.UpdateGuestsAvailabilityUnavailableMotives();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //actualizando motivos de indisponibilidad de huespedes (Revirtiendo proceso de disponibilidad por una noche)
        AddLogGridReservations("Update", "Updating Unavailable Motives (Reverting One Night Availability)");
        await BRTransfer.UpdateGuestsUnavailableMotives1NightRevert();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

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
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

      try
      {
        //agregando nuevos huespedes
        AddLogGridReservations("Insert", "Adding New Guests.");
        await BRTransfer.AddGuests();
      }
      catch (Exception exception)
      {
        AddLogGridReservations("Error", exception.InnerException.Message);
      }

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
          AddLogGridReservations("Error", exception.InnerException.Message);
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
    public async Task<string> StringIDHoteles(List<LeadSourceShort> hotels)
    {
      string leadSourceId = string.Empty;
      await Task.Run(() =>
      {
        // recorremos la lista
        foreach (LeadSourceShort hotel in hotels)
        {
          // concatenamos la lista de hoteles
          leadSourceId += hotel.lsID + ",";
        }
        //eliminamos la ultima coma.
        leadSourceId = leadSourceId.Remove(leadSourceId.Length - 1);
      });
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
      if (dispatcherBlinkLabelReservations != null && !blnStatus.Value)
      {
        dispatcherBlinkLabelReservations.Stop();
        if (!dispatcherBlinkLabelReservations.IsEnabled)
        {
          if (lblStatusReservations != null)
          {
            lblStatusReservations.Foreground = Brushes.Black;
            lblStatusReservations.Background = Brushes.Transparent;
            lblStatusReservations.BorderBrush = Brushes.Transparent;
          }
          if (brdStatusReservations != null)
          {
            brdStatusReservations.Background = Brushes.Transparent;
            brdStatusReservations.BorderBrush = Brushes.Black;
          }
        }
      }
      else if (dispatcherBlinkLabelReservations == null && blnStatus.Value)
      {
        dispatcherBlinkLabelReservations = BlinkLabel(lblName: lblStatusReservations, brdName: brdStatusReservations);
        dispatcherBlinkLabelReservations.Start();
      }
      else if (dispatcherBlinkLabelReservations != null && blnStatus.Value)
      {
        dispatcherBlinkLabelReservations.Start();
      }
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
      if (dispatcherBlinkLabelExchangeRate != null && !blnStatus.Value)
      {
        dispatcherBlinkLabelExchangeRate.Stop();
        if (!dispatcherBlinkLabelExchangeRate.IsEnabled)
        {
          if (lblStatusExchangeRate != null)
          {
            lblStatusExchangeRate.Foreground = Brushes.Black;
            lblStatusExchangeRate.Background = Brushes.Transparent;
            lblStatusExchangeRate.BorderBrush = Brushes.Transparent;
          }
          if (brdStatusExchangeRate != null)
          {
            brdStatusExchangeRate.Background = Brushes.Transparent;
            brdStatusExchangeRate.BorderBrush = Brushes.Black;
          }
        }
      }
      else if (dispatcherBlinkLabelExchangeRate == null && blnStatus.Value)
      {
        dispatcherBlinkLabelExchangeRate = BlinkLabel(lblName: lblStatusExchangeRate, brdName: brdStatusExchangeRate);
        dispatcherBlinkLabelExchangeRate.Start();
      }
      else if (dispatcherBlinkLabelExchangeRate != null && blnStatus.Value)
      {
        dispatcherBlinkLabelExchangeRate.Start();
      }
    }
    #endregion

    #region BlinkLabel
    /// <summary>
    /// Metodo para aplicar efecto blink a label
    /// </summary>
    /// <param name="lblName">Label a que se aplicara el efecto</param>
    /// <param name="brdName">Si tiene borde el label se envia para aplicar efecto igual</param>
    /// <param name="blnStatus">True para iniciar y False paraterminar</param>
    /// <history>
    /// [michan]  02/05/2016  Created
    /// </history>   
    public DispatcherTimer BlinkLabel(System.Windows.Controls.Label lblName = null, System.Windows.Controls.Border brdName = null, bool? blnStatus = true) //where T : Window //, 
    {
      //Booleano para control del efecto blink.
      bool blnOnOff = false;
      // Distpacher que ejecutara determinado tiempo el efecto blink
      DispatcherTimer dispatcher = new DispatcherTimer();
      // agregamos el tiempo de efecto blink
      dispatcher.Interval = new TimeSpan(0, 0, 0, 0, 500);
      // tarea para aplciar el efecto blink
      dispatcher.Tick += (s, a) =>
      {
        // valida si hay label para aplicar efecto
        if (lblName != null)
        {
          // aplica color de letra a label
          lblName.Foreground = (blnOnOff) ? Brushes.Black : Brushes.Transparent;
          // se aplica fondo trasnparente al label.
          lblName.Background = Brushes.Transparent;
          //lblName.BorderBrush = (blnOnOff) ? System.Windows.Media.Brushes.BlueViolet : System.Windows.Media.Brushes.Black;
        }
        // valida si tiene bordes el label
        if (brdName != null)
        {
          // cambia de color de fondo al borde
          brdName.Background = (blnOnOff) ? Brushes.DarkRed : Brushes.Transparent;
          // aplica transparencia para no perder el label.
          brdName.Opacity = (blnOnOff) ? .6 : 0;
          brdName.BorderBrush = Brushes.Black;
          //new SolidColorBrush(Color.FromArgb(128, 255, 0, 0)) brdName.BorderBrush = (blnOnOff) ? System.Windows.Media.Brushes.Black : System.Windows.Media.Brushes.White;
        }
        //cambiamos el control del efecto.
        blnOnOff = !blnOnOff;
      };
      return dispatcher;
    }

    #endregion

    #endregion

    #region Metodos para actualizar botones

    #region UpdateDelegateButton
    ///<summary>Delegado utilizado en la actualizacion el content de los botones</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public delegate void UpdateDelegateButton(System.Windows.Controls.Button btnName, string strContent, bool? blnIsEnable = true);
    #endregion

    #region ContentButton
    /// <summary>
    /// Metodo para actualizar el contenido de los botones
    /// </summary>
    /// <param name="btnName">Boton a actualizar o modificar su contenido</param>
    /// <param name="strContetn">contenido del boton</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void ContentButton(System.Windows.Controls.Button btnName, string strContetn, bool? blnIsEnable = true)
    {
      btnName.Content = strContetn;
      btnName.IsEnabled = blnIsEnable.Value;
    }
    #endregion

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
      // si no se ha instanciado el delegado
      if (updateContentButton == null)
      {
        // se crea instancia de delegado.
        updateContentButton = new UpdateDelegateButton(ContentButton);
      }
      // actualizamos el buttom con el delegado
      btnName.Dispatcher.BeginInvoke(DispatcherPriority.Render, updateContentButton, btnName, strContent, blnIsEnable.Value);
    }
    #endregion

    #endregion

    #region Actualizacion de Grids

    #region UpdateDelegateGridReservations
    ///<summary>Metodo para crear delegado utilizado en la actualizacion el grid del log de reservations</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public delegate void UpdateDelegateGridReservations(DateTime dtmDate, string strLogLevel, string strMessage);
    #endregion

    #region UpdateDelegateGridExchangeRate
    ///<summary>Metodo para crear delegado utilizado en la actualizacion el grid del log de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public delegate void UpdateDelegateGridExchangeRate(DateTime dtmDate, string strLogLevel, string strMessage);
    #endregion

    #region UpdatingGrid
    /// <summary>
    /// Metodo para actualizar datagrid de los log
    /// </summary>
    /// <param name="dataGrid">Datagrid a actualizar</param>
    /// <param name="listTransations">Lista de transacciones a enviar al datagrid</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void UpdatingGrid(System.Windows.Controls.DataGrid grdDataGrid, List<LogHelper.Transaction> listTransations)
    {
      // validamos que ayan pasado un objeto datagrid
      if (grdDataGrid != null)
      {
        // valida si el datagrid si tiene registros
        if (grdDataGrid.ItemsSource != null)
          // borra los registros del datagrid
          grdDataGrid.ItemsSource = null;

        // valida si la lista tiene elementos
        if (listTransations != null)
        {
          // se obtienen los primeros 500 registros.
          var itemsSource = listTransations.Skip(Math.Max(0, listTransations.Count() - 499)).Take(499).ToList();
          if (itemsSource != null)
            // asignamos los registros al datagrid
            grdDataGrid.ItemsSource = itemsSource;

          // refrescamos el datagrid con los nuevos elementos.
          grdDataGrid.Items.Refresh();
          
          // obtenemos el total de registros en el datagrid
          int index = (grdDataGrid.Items.Count > 0) ? grdDataGrid.Items.Count - 1 : 0;
          // seleccionamos el ultimo elementos en el datagrid.
          GridHelper.SelectRow(grdDataGrid, index);
        }
      }
      
    }
    #endregion

    #region UpdateDelegateDatagridReservations
    ///<summary>Metodo que agrega nuevo registro en el log de reservations y actualiza el grid de log de reservations</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void UpdateDelegateDatagridReservations(DateTime dtmDate, string strLogLevel, string strMessage)
    {
      // agregamos al log el registro
      var logTransfer = LogHelper.AddTransaction("Reservations", dtmDate, strLogLevel, strMessage);
      if (logTransfer != null)
      {
        // agregamos el log a la lista de logs
        listTransactionsReservations.Add(logTransfer);
        // se actualiza el grid
        UpdatingGrid(grdLogReservations, listTransactionsReservations);
      }
      
      
    }
    #endregion

    #region UpdateDelegateDatagridExchangeRate
    ///<summary>
    ///Metodo que agrega nuevo registro en el log de reservations y actualiza el grid de log de exchange rate
    ///</summary>
    ///<param name="dtmDate">Fecha de registro</param>
    ///<param name="strLogLevel">Tipo de mensaje en el log</param>
    ///<param name="strMessage">Mensaje del log</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void UpdateDatagridExchangeRate(DateTime dtmDate, string strLogLevel = "", string strMessage = "")
    {
      var logTransfer = LogHelper.AddTransaction("ExchangeRate", dtmDate, strLogLevel, strMessage);
      if(logTransfer != null)
      {
        listTransactionsExchangeReservations.Add(logTransfer);
        UpdatingGrid(grdLogExchangeRate, listTransactionsExchangeReservations);
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
        // validamos que exista la instancia del delegado para actualizar el grid de reservaciones
        if (updateGridReservations == null)
          // si es nullo se crea la instancia
          updateGridReservations = new UpdateDelegateGridReservations(UpdateDelegateDatagridReservations);
        // actualizamos el grid de reservaciones.
        grdLogReservations.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateGridReservations, DateTime.Now, strLogLevel, strLogMessage);
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
        // validamos la instancia del delegado para actualizar el grid
        if (updateGridExchangeRate == null)
          // creamos la instancia si no existe.
          updateGridExchangeRate = new UpdateDelegateGridExchangeRate(UpdateDatagridExchangeRate);
        // actualizamos el grid.
        grdLogExchangeRate.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateGridExchangeRate, DateTime.Now, strLogLevel, strLogMessage);
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
      cancelTokenReservations.Cancel();
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
      cancelTokenExchangeRate.Cancel();
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
      blnRunTransfer = false;
      blnRunOrCancelReservations = false;
      // reiniciamos a cero los valores del progresbar
      AddValueProgressBarReservations(porcentBase: 0);
      UpdateLabel(lblPorcentProgresBarReservations, "");
      // cambiamos el label del boton de reservaciones
      UpdateButton(btnReservations, "Update Reservations");
      // borramos los registros del label de visor de reservaciones
      UpdateLabel(lblTransferReservations, "");

      // actualizamos la hora y fecha de la ultima y proxima actualización.
      _lastReservations = DateTime.Now;
      _nextReservations = DateHelper.AddTimeDate(_intervalTimeReservations);
      UpdateLabel(lblLastTransferReservations, lblTextLast + _lastReservations.ToString());
      UpdateLabel(lblNextTransferReservations, lblTextNext + _nextReservations.ToString());

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
      UpdateLabel(lblLastTransferExchangeRate, lblTextLast + _lastExchangeRate.ToString());
      // actualizamos la fecha y hora de la siguiente actualización
      _nextExchangeRate = DateHelper.AddTimeDate(_tranferExchangeRatesIntervalTime);
      // actualizamos el label de estatus de la actualización
      UpdateLabel(lblNextTransferExchangeRate, lblTextNext + _nextExchangeRate.ToString());

      // regresamos a cero el progressbar
      AddValueProgressBarExchangeRate(value: 0);
      UpdateLabel(lblPorcentProgressExchangeRate, "");
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

    #region UpdateDelegateLabelText
    /// <summary>
    /// Delegado para actualizar los labels del formulario en interacción.
    /// </summary>
    /// <param name="lblUpdate">label que se desea actualizar</param>
    /// <param name="strContent">Cadena que se desea pintar en el label</param>
    private delegate void UpdateDelegateLabel(System.Windows.Controls.Label lblUpdate, string strContent);
    #endregion

    #region UpdateLabelDelegate
    /// <summary>
    /// Metodo que recibe los valores para el label aactualizar
    /// </summary>
    /// <param name="lblUpdate">Labeel a aactualizar</param>
    /// <param name="stringContent">Contenido del label</param>
    private void UpdateLabelDelegate(System.Windows.Controls.Label lblUpdate, string strContent)
    {
      // agregamos el contenido (string) al label
      lblUpdate.Content = strContent;
    }
    #endregion

    #region UpdateLabel
    /// <summary>
    /// Metodo que invoca al delegado para la actualizacion del contenido del label
    /// </summary>
    /// <param name="lblUpdate">Label a actualizar</param>
    /// <param name="stringContent">contenido del label</param>
    public void UpdateLabel(System.Windows.Controls.Label lblUpdate, string stringContent)
    {
      // validamos si el delegado para la actualizacion del label se encuentra inicalizado
      if (updateDelegateLabel == null)
        // si no se encuentra inicializado se inicializa.
        updateDelegateLabel = new UpdateDelegateLabel(UpdateLabelDelegate);
      // se actualiza el label con la informacion recbida
      lblUpdate.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateDelegateLabel, lblUpdate, stringContent);
    }
    #endregion

    #region UpdateLabelTrasnferReservations
    /// <summary>
    /// Metodo para actualizar las transferencias realizadas en las reservaciones
    /// </summary>
    /// <param name="stringContent">cadena que contiene la trasnferencia realizada</param>
    public void UpdateLabelTrasnferReservations(string stringContent)
    {
      // validamos que el conetenido no sea nullo o vacio.
      string stringLabel = (!String.IsNullOrEmpty(stringContent) && !String.IsNullOrWhiteSpace(stringContent)) ? stringContent : "";
      UpdateLabel(lblTransferReservations, stringLabel);

    }
    #endregion

    #region UpdateLabelStatusExchangeRate
    /// <summary>
    /// Metodo que actualiza el label de esatus de las transferencias de exchange rate
    /// </summary>
    /// <param name="stringContent">contenido del label status</param>
    public void UpdateLabelStatusExchangeRate(string stringContent)
    {
      UpdateLabel(lblStatusExchangeRate, stringContent);
    }
    #endregion

    #region UpdateLabelStatusReservations
    /// <summary>
    /// Metodo que actualiza el label de esatus de las transferencias de reservaciones
    /// </summary>
    /// <param name="stringContent">contenido del label status</param>
    public void UpdateLabelStatusReservations(string stringContent)
    {
      UpdateLabel(lblStatusReservations, stringContent);
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
        //((IProgress<double>)progressBar).Report(valueIncrement);
      
        progressBar.Value = valueIncrement;
        lblProcent.Content = String.Format("{0:0} %", porcentValue);
        //progressBar.ReportProgress(valueIncrement, porcentValue);
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

    #region Metodos de transferencia de Exchange Rate

    #region WorkerUpdateExchangeRateDoWork
    ///<summary>Metodo que contiene la tarea que realiza todo el proceso de actualizacion de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private async Task WorkerUpdateExchangeRateDoWork(CancellationToken cancelationtoken)
    {
      // valida si el proceso de actualización no ha sido cancelado
      cancelationtoken.ThrowIfCancellationRequested();
      
        
      //Se obtiene la fecha del servidor
      _dtmServerDate = BRHelpers.GetServerDate();    
      AddLogGridExchangeRate("Start", "Start Updating Exchange Rates");
      AddValueProgressBarExchangeRate(value: 1);
      Thread.Sleep(100);

      //agregamos los tipos de cambio faltantes hasta la fecha actual para que no existan huecos
      BRExchangeRate.InsertExchangeRate(_dtmServerDate);
      AddValueProgressBarExchangeRate(value: 2);

      // valida si el proceso de actualización no ha sido cancelado
      cancelationtoken.ThrowIfCancellationRequested();

      //obtenemos el tipo de cambio de la Intranet
      AddLogGridExchangeRate("Info", "Getting Exchange Rate from Intranet Service");
      TipoCambioTesoreria exchangeRate = await IntranetHelper.TipoCambioTesoreria(_dtmServerDate, "USD");
      if (exchangeRate != null)
      {
        timeWatchExchangeRate.Start();
        _exchangeRateType = Convert.ToDecimal(exchangeRate.TipoCambio);
        //comparamos si el cambio es positivo y mayor a cero
        if (_exchangeRateType > 0)
        {
          _currencyId = "MX";
          AddValueProgressBarExchangeRate(value: 3);
          //Se realiza la actualizacion y se registra en el log la inicialización

          AddLogGridExchangeRate("Info", $"Updating Exchange Rate { _currencyId } ({_exchangeRateType})");
          AddValueProgressBarExchangeRate(value: 4);

          // valida si el proceso de actualización no ha sido cancelado
          cancelationtoken.ThrowIfCancellationRequested();

          // actualizamos el exchange rate
          await BRExchangeRate.UpdateExchangeRate(_dtmServerDate, _currencyId, _exchangeRateType);
          timeWatchExchangeRate.Stop();

          TimeSpan tSpan = timeWatchReservations.Elapsed;
          // Convertimos el tiempo de obtencion en string para pintar en el grid
          string elapsedTime = StringTimeDifference(tSpan);

          AddLogGridExchangeRate("Info", $"Updating Exchange Rate in {elapsedTime }");
          AddValueProgressBarExchangeRate(value: 6);
          Thread.Sleep(1000);
          AddLogGridExchangeRate("Success", "Exchange Rate finished");
          AddValueProgressBarExchangeRate(value: 10);
          Thread.Sleep(1000);
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
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      WindowsHelper.EventsKeys(window:this, blnNotifycon:true);
      // cargamos el contenido de los grid.
      LoadDataGRidExchangeRate();
      LoadDataGRidReservations();
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
