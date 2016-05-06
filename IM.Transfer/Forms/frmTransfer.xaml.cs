using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Forms;
using IM.Transfer.Clases;
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

    public static DateTime _dtpServerDate = new DateTime();
    public static string _currencyId;
    public static DateTime _dateToday;

    #region Lista de registros de transacciones para datagrid
    public static List<LogHelper.Transaction> listTransactionsExchangeReservations = new List<LogHelper.Transaction>();
    public static List<LogHelper.Transaction> listTransactionsReservations = new List<LogHelper.Transaction>();
    #endregion

    #region Instancias de BackgroundWorker 
    public readonly BackgroundWorker WorkerExchangeRate = null;
    public readonly BackgroundWorker WorkerReservations = null;
    #endregion

    #region Instancias de delegados
    public UpdateDelegateGridExchangeRate updateGridExchangeRate = null;
    public UpdateDelegateGridReservations updateGridReservations = null;
    public UpdateDelegateButton updateContentButton = null;
    UpdateDelegateLabel updateDelegateLabel = null;
    #endregion


    #region timeWatch
    public static Stopwatch timeWatch = new Stopwatch(); // Utilizado para calcular el tiempo que se tarda en llevar algun proceso
    public static TimeSpan timeGetReservations = TimeSpan.MinValue;
    #endregion

    #region lblTextLast and lblTextLast
    public static string lblTextLast = "Last Transfer ";
    public static string lblTextNext = "Next Transfer ";
    #endregion

    #region blnuseBand, blnTwoNight, blnOneNight
    public static bool blnOneNight = false;
    public static bool blnTwoNight = false;
    public static bool blnuseBand = false;
    #endregion

    #region Status Transfers
    // Parametros para validar el estatus de las trasnferencias
    public bool blnRunTransfer = false;
    public bool blnRunOrCancellReservations = false; // valida el estatus de la transferencia de reservaciones
    public bool blnRunOrCancellExchangeRate = false; // valida el estatus de la transferencia de exchange rate
    #endregion

    #region Manejo de la ventana
    //parametros para el manejo de la ventana
    private bool pushCtrl = false;
    private bool pushCtrlE = false;
    #endregion

    #endregion
    
    #region Contructores y Destructores
    public frmTransferLauncher()
    {
      InitializeComponent();

      InitializeValuesParameters();

      #region WorkerExchangeRate
      WorkerExchangeRate = new BackgroundWorker();
      WorkerExchangeRate.WorkerReportsProgress = true;
      WorkerExchangeRate.WorkerSupportsCancellation = true;
      WorkerExchangeRate.DoWork += WorkerUpdateExchangeRateDoWork;
      WorkerExchangeRate.RunWorkerCompleted += WorkerRunWorkerExchangeRateCompleted;
      WorkerExchangeRate.ProgressChanged += new ProgressChangedEventHandler(progresBarTransfer_Changed);
      #endregion

      #region WorkerReservations
      WorkerReservations = new BackgroundWorker();
      WorkerReservations.WorkerReportsProgress = true;
      WorkerReservations.WorkerSupportsCancellation = true;
      WorkerReservations.DoWork += DoWorkerTransferReservations;
      WorkerReservations.RunWorkerCompleted += WorkerRunWorkerReservationsCompleted;
      WorkerReservations.ProgressChanged += new ProgressChangedEventHandler(progresBarReservations_Changed);
      #endregion

      
    }

    

    #endregion

    #region InitializeValuesParameters
    ///<summary>Metodo que inicializa los parametros y configuracion de las transferencias</summary>
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
        if (!DateHelper.isDateEquals(_currentTime, _nextExchangeRate))
        {
          _nextExchangeRate = _currentTime;
        }
        if (!DateHelper.isDateEquals(_currentTime, _nextReservations))
        {
          _nextReservations = _currentTime;
        }
        

        if (DateHelper.IsRangeHours(_currentTime.TimeOfDay, _tranferExchangeRatesStartTime, _tranferExchangeRatesEndTime))
        {
          if ((DateHelper.IsRangeTime(_currentTime, _nextExchangeRate) || DateHelper.IsRangeHours(_nextExchangeRate.TimeOfDay, _tranferExchangeRatesStartTime, _currentTime.TimeOfDay)) && (!blnRunOrCancellReservations))
          {
            StartExchangeRate(); 
          }
        }

        if (DateHelper.IsRangeHours(_currentTime.TimeOfDay, _startTimeReservations, _endTimeReservations))
        {
          if ((DateHelper.IsRangeTime(_currentTime, _nextReservations) || DateHelper.IsRangeHours(_nextReservations.TimeOfDay, _startTimeReservations, _currentTime.TimeOfDay)) && (!blnRunOrCancellExchangeRate))
          {
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
    public void StartReservations()
    {
      if(!WorkerReservations.IsBusy)
      {
        OnOffBlinkReservations();
        WorkerReservations.RunWorkerAsync();
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
    public void StartExchangeRate()
    {
      if (!WorkerExchangeRate.IsBusy)
      {
        OnOffBlinkExchangeRate();
        WorkerExchangeRate.RunWorkerAsync();
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
    private void LoadDataGRidExchangeRate()
    {
      DateTime dateTo = DateHelper.DaysBeforeOrAfter(_daysBeforeDAY);
      Task.Factory.StartNew(() =>
        {
          listTransactionsExchangeReservations = LogHelper.LoadHistoryLog("ExchangeRate", DateTime.Now, dateTo);
        }
      ).ContinueWith(
        (task1) =>
          {
            if (task1.IsCompleted)
            {
              task1.Wait(1000);

              if (listTransactionsExchangeReservations.Count > 0)
              {
                var last = listTransactionsExchangeReservations.Select(ob => ob).Where(ob => ob.LogLevel == "Finish" || ob.LogLevel == "Start" || ob.LogLevel == "Success").LastOrDefault();
                if (last != null)
                {
                  _lastExchangeRate = last.Date;
                  _nextExchangeRate = last.Date;
                }
                UpdatingGrid(grdLogExchangeRate, listTransactionsExchangeReservations);
              }
            }
          },
        TaskScheduler.FromCurrentSynchronizationContext()
      );
    }
    #endregion

    #region LoadDataGRidReservations
    ///<summary>Metodo que carga e inicializa el grid del log de Reservations</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private void LoadDataGRidReservations()
    {
      DateTime dateTo = DateHelper.DaysBeforeOrAfter(_daysBeforeDAY);
      Task.Factory.StartNew(() =>
      {
        listTransactionsReservations = LogHelper.LoadHistoryLog("Reservations", DateTime.Now, dateTo);
      }
      ).ContinueWith(
        (task1) =>
        {
          if (task1.IsCompleted)
          {
            task1.Wait(1000);

            if (listTransactionsExchangeReservations.Count > 0)
            {
              var last = listTransactionsReservations.Select(ob => ob).Where(ob => ob.LogLevel == "Finish" || ob.LogLevel == "Start" || ob.LogLevel == "Success").LastOrDefault();
              if (last != null)
              {
                _lastReservations = last.Date;
                _nextReservations = last.Date;
              }
              UpdatingGrid(grdLogReservations, listTransactionsReservations);
            }
          }
        },
        TaskScheduler.FromCurrentSynchronizationContext()
      );
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
    public void DoWorkerTransferReservations(object Sender, DoWorkEventArgs e)
    {
      // Se valida el estatus del formulario
      StatusForm(Sender, e);
      blnRunOrCancellReservations = true; // ponemos en verdadero la ejecucion de transferencia de reservaciones
      UpdateButton(btnReservations, "Cancell Reservations"); // cambiamos el testo del boton para cancelar la transferemcia
      
      // Transferencia iniciada
      UpdateLabelStatusReservations("PROCESSING");
      AddLogGridReservations("Start", "Transfer Started.");

      //indicamos que inicio la transferencia de reservaciones
      List<TransferStartData> options = BRTransfer.Start();
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
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        // obtenemos las zonas de transferencia
        List<ZoneTransfer> zoneTransfer = BRZones.GetZonesTransfer();
        // si hay al menos una zona
        if (zoneTransfer.Count > 0 && zoneTransfer != null)
        {
          // recorremos las zonas
          foreach (ZoneTransfer zone in zoneTransfer)
          {
            string zoneID = zone.znID.ToString();
            string zoneName = zone.znN.ToString();
            string zoneHotel = zone.znZoneHotel.ToString();
            // si se cancelo
            if (!blnRunOrCancellReservations && WorkerReservations.CancellationPending) break;

            // validamos que hayan hoteles para la zona     
            if (!(String.IsNullOrEmpty(zoneID) && !String.IsNullOrWhiteSpace(zoneID)) && (!String.IsNullOrEmpty(zoneName) && !String.IsNullOrWhiteSpace(zoneName)) && (!String.IsNullOrEmpty(zoneHotel) && !String.IsNullOrWhiteSpace(zoneHotel)))
            {

              // si se cancelo
              if (!blnRunOrCancellReservations && WorkerReservations.CancellationPending) break;
              //exportamos las reservaciones a la tabla de transferencia
              ExportToTransfer(zoneID, zoneName, zoneHotel);

              // si se cancelo
              if (!blnRunOrCancellReservations && WorkerReservations.CancellationPending) break;
              //exportamos los registros de la tabla de transferencia a la tabla de huespedes
              ExportToGuests();

              if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
              {
                AddLogGridReservations("Info", "Transfer Completed.");
              }
              else
              {
                AddLogGridReservations("Info", "Transfer Canceled.");
              }

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
          CancellWorkerReservations();
        }
      }
      // indicamos que ya no se esta transfiriendo
      BRTransfer.Stop();
      ReloadReservations();
      AddLogGridReservations("Finish", "Process Finished.");
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
    public void ExportToTransfer(string zoneID, string zoneName, string zoneHotel)
    {
      // obtenemos las reservaciones
      List<ReservationOrigosTransfer> reservationOrigosTransfer = GetReservations(zoneID, zoneName, zoneHotel);
      string strTransferingReservation = "";
      if ((reservationOrigosTransfer != null && reservationOrigosTransfer.Count > 0))
      {
        if(blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
        {
          // transfiriendo reservaciones a la base de datos.
          AddLogGridReservations("Insert", "Transfering Reservations to Database.");
          //limpiamos la tabla de transferencia
          BRTransfer.DeleteTransfer();
          BRTransfer.GetTransfer();

          // transferimos las reservaciones a la base de datos
          int cont = 0;
          double porcent = 0;
          foreach (ReservationOrigosTransfer reservationOrigos in reservationOrigosTransfer)
          {
            if (!blnRunOrCancellReservations || WorkerReservations.CancellationPending) break;
            //exportamos las reservaciones a la tabla de transferencia
            cont = cont + 1;
            porcent = Porcent(cont, reservationOrigosTransfer.Count);
            AddValueProgressBarReservations(porcentBase: porcent);
            // desplegamos la reservacion que se esta transfiriendo
            strTransferingReservation = "Transfering Reservation " + reservationOrigos.Hotel + " No. " + reservationOrigos.Folio + " " + "\r\n" + "Check In Date: " + DateHelper.StringDate(Convert.ToDateTime(reservationOrigos.Arrival));//System.Environment.NewLine                                                                                                                                                                                                                       // desplegamos la reservacion que se esta transfiriendo
            UpdateLabelTrasnferReservations(strTransferingReservation);
            //agregamos la reservacion transferida
            AddTransfer(reservationOrigos);
          }
          if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
          {
            //Total de registros agregados de la zona (X)
            AddLogGridReservations("Info", reservationOrigosTransfer.Count.ToString() + " Records Added from(" + zoneName + ").");
          }
          //indicamos que la zona ya no esta transfiriendo
          TransferStopZone(zoneID);
        }
        
      }
      else
      {
        AddLogGridReservations("Warning", "No records were found to process.");
      }
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
    public void AddTransfer(ReservationOrigosTransfer reservationOrigos)
    {
      // localizamos la reservacion
      if (!BRTransfer.ExistReservation(reservationOrigos.Hotel, reservationOrigos.Folio.ToString()))
      {
        if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
        {
          // agregamos la reservacion
          //si la reservacion no ha sido agregada todavia
          Model.Transfer transfer = Transfer(reservationOrigos);
          BRTransfer.AddReservation(transfer);

          // Agregamos la informacion de Origen a GuestOpera
          AddTransferToOperaGuest(reservationOrigos);
        }
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
    public void AddTransferToOperaGuest(ReservationOrigosTransfer reservationOrigos)
    {
      //Si hay GuestID, se asigna, de lo contrario es OutHouse y no se agrega
      Model.Guest guest = BRGuests.GetGuestValidForTransfer(reservationOrigos.Folio.ToString(), reservationOrigos.Hotel);
      if ((guest != null))
      {
        // Revisamos si ya existe en Guest
        GuestOpera guestOpera = BRGuestOpera.GetGuestOpera(guest.guID);
        GuestOpera guOpera = null;
        // Si hay GuestID, se asigna, de lo contrario es OutHouse y no se agrega
        if (guestOpera != null)
        {
          // Si ya existe, se actualiza el valor
          guOpera = UpdateGuestOpera(reservationOrigosTransfer: reservationOrigos, gOpera: guestOpera);
          BRGuestOpera.SaveGuestOpera(guestOpera, false);
        }
        else
        {
          // Si no existe, se agrega
          guOpera = UpdateGuestOpera(reservationOrigosTransfer: reservationOrigos, gogu: guest.guID);
          BRGuestOpera.SaveGuestOpera(guOpera, true);
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
    public List<ReservationOrigosTransfer> GetReservations(string zoneID, string zoneName, string zoneHotel)
    {
      // obtenemos los hoteles de la zona
      List<LeadSourceShort> hotels = BRLeadSources.GetLeadSourcesByZoneBoss(zoneID);
      DateTime dateFrom = DateHelper.DaysBeforeOrAfter(_daysBeforeDAY);
      DateTime dateTo = DateHelper.DaysBeforeOrAfter(_daysAfterDAY);
      List<ReservationOrigosTransfer> reservations = null;

      string strArchivo = AppContext.BaseDirectory + "\\Configuration.ini";
      if (LogHelper.ExistFile(strArchivo))
      {
        IniFileHelper _iniFileHelper = new IniFileHelper(strArchivo);
        dateFrom = _iniFileHelper.readDate("TransferReservations", "DateFrom", dateFrom);
        dateTo = _iniFileHelper.readDate("TransferReservations", "DateTo", dateTo);
      }
      string leadSourceID = StringIDHoteles(hotels);
      //si no hay hoteles en la zona
      if ((hotels.Count > 0) && (hotels != null) && (dateFrom.CompareTo(dateTo) < 0))
      {
        if(blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
        {
          AddLogGridReservations("Info", "Getting Reservations from " + zoneName + " (Check In Date: " + DateHelper.StringDate(dateFrom, dateTo) + ", Hotels: " + leadSourceID + ").");
          timeWatch.Start();
          reservations = HotelService.GetReservationsByArrivalDate(zoneHotel, dateFrom, dateTo, leadSourceID);
          timeWatch.Stop();
        }

        if (reservations != null && reservations.Count > 0)
        {
          // Calculamos el tiempo de obtencion de las reservaciones
          TimeSpan tSpan = timeWatch.Elapsed;
          // Convertimos el tiempo de obtencion en string para pintar en el grid
          string elapsedTime = StringTimeDifference(tSpan);
          if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)AddLogGridReservations("Info", reservations.Count.ToString() + " Reservations were Obtained in " + elapsedTime);
        }
        else
        {
          // si no se pudo obtener las reservaciones, detenemos la transferencia
          AddLogGridReservations("Error", "Could not get reservations from Zone " + zoneName + " (Check In Date: " + DateHelper.StringDate(dateFrom, dateTo) + ").");
        }
        timeWatch.Reset();
      }
      else
      {
        AddLogGridReservations("Error", "There is not hotels for this zone (" + zoneName + ")");
      }
      return reservations;

    }
    #endregion

    #region StringTimeDifference
    public string StringTimeDifference(TimeSpan timeDiference)
    {
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
      if (ValidateHelper.IsValidEmail(reservationOrigosTransfer.Email))
      {
        transfer.tEmail = reservationOrigosTransfer.Email;
      }
      else
      {
        transfer.tEmail = "";
      }

      //Ciudad
      transfer.tCity = Trim(reservationOrigosTransfer.City);
      //Estado
      transfer.tState = Trim(reservationOrigosTransfer.State);
      //Pais
      transfer.tcoID = SetIDName(reservationOrigosTransfer.Country);
      transfer.tcoN = SetIDName(reservationOrigosTransfer.CountryN);
      //Agencia
      transfer.tagID = SetIDName(reservationOrigosTransfer.Agency);
      transfer.tagN = SetIDName(reservationOrigosTransfer.AgencyN);
      //Pais de la agencia
      transfer.tcoAID = SetIDName(reservationOrigosTransfer.CountryAgency);
      transfer.tcoAN = SetIDName(reservationOrigosTransfer.CountryAgencyN);
      //Club de la membresia
      transfer.tcl = ConvertHelper.StringToIntOrNull(reservationOrigosTransfer.Club.ToString());
      //Compañia de la membresia
      transfer.tCompany = reservationOrigosTransfer.Company;
      //Numero de membresia
      transfer.tMember = ConvertHelper.StringToBool(reservationOrigosTransfer.Membership.Trim());
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
      transfer.tFolioPrevious = reservationOrigosTransfer.FolioPrevious;
      //Motivo de indisponibilidad (0 - AVAILABLE)
      transfer.tum = 0;
      //Disponibilidad (No disponible)
      transfer.tmk = "?";
      transfer.tAvail = false;
      //Idioma
      transfer.tla = "EN";
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
    public GuestOpera UpdateGuestOpera(ReservationOrigosTransfer reservationOrigosTransfer, int? gogu = null,GuestOpera gOpera = null)
    {
      GuestOpera guestOpera = null;
      if (gOpera != null)
      {
        guestOpera = gOpera;
      }
      else
      {
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
    public void ExportToGuests()
    {
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        //procesando registros transferidos
        AddLogGridReservations("Info", "Processing Transferred Records.");
        //actualizamos el catalogo de paises
        UpdateCountries();
        //actualizamos el catalogo de agencias
        UpdateAgencies();
        //actualizamos el catalogo de tipos de habitacion
        UpdateRoomTypes();
        //actualizamos el catalogo de contratos
        UpdateContracts();
        //actualizamos el catalogo de grupos
        UpdateGroups();
        //actualizamos la tabla de transferencia
        UpdateTransfer();
        //actualizamos los huespedes
        UpdateGuests();
      }
    }
    #endregion

    #region UpdateCountries
    /// <summary>
    /// actualizamos el catalogo de paises
    /// </summary>
    /// <history>
    /// [michan]  21/04/2016  Created
    /// </history>
    public void UpdateCountries()
    {
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        // agregando paises de Hotel
        AddLogGridReservations("Insert", "Adding Hotel Countries.");
        BRTransfer.AddCountriesHotel();
        // agregando paises de Origos
        AddLogGridReservations("Insert", "Adding Origos Countries.");
        BRTransfer.AddCountries();
        // actualizando las descripciones de los paises de Hotel
        AddLogGridReservations("Update", "Updating Hotel Countries Names.");
        BRTransfer.UpdateCountriesHotelNames();
        // actualizando las descripciones de los paises de Origos
        AddLogGridReservations("Update", "Updating Origos Countries Names.");
        BRTransfer.UpdateCountriesNames();
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
    public void UpdateAgencies()
    {
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        //agregando agencias de Hotel

        AddLogGridReservations("Insert", "Adding Hotel Agencies.");
        BRTransfer.AddAgenciesHotel();
        //agregando agencias de Origos

        AddLogGridReservations("Insert", "Adding Origos Agencies.");
        BRTransfer.AddAgencies();
        //actualizando las descripciones de las agencias de Hotel

        AddLogGridReservations("Update", "Updating Hotel Agencies Names.");
        BRTransfer.UpdateAgenciesHotelNames();
        //actualizando las descripciones de las agencias de Origos

        AddLogGridReservations("Update", "Updating Origos Agencies Names.");
        BRTransfer.UpdateAgenciesNames();
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
    public void UpdateRoomTypes()
    {
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        //agregando tipos de habitacion
        AddLogGridReservations("Insert", "Adding Room Types.");
        BRTransfer.AddRoomTypes();

        //actualizando las descripciones de los tipos de habitacion
        AddLogGridReservations("Update", "Updating Room Types Names.");
        BRTransfer.UpdateRoomTypesNames();
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
    public void UpdateContracts()
    {
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        //agregando contratos
        AddLogGridReservations("Insert", "Adding Contracts.");
        BRTransfer.AddContracts();
        //actualizando las descripciones de los contratos
        //
        AddLogGridReservations("Update", "Updating Contracts Names.");
        BRTransfer.UpdateContractsNames();
      }
    }
    #endregion

    #region UpdateGroups
    /// <summary>
    /// Actualiza el catalogo de grupos
    /// </summary>
    public void UpdateGroups()
    {
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        // agregando contratos
        AddLogGridReservations("Insert", "Adding Groups.");
        BRTransfer.AddGroups();

        //actualizando las descripciones de los contratos
        AddLogGridReservations("Update", "Updating Groups Names");
        BRTransfer.UpdateGroupsNames();
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
    public void UpdateTransfer()
    {
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        //actualizando paises de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Countries");
        BRTransfer.UpdateTransferCountries();

        //actualizando agencias de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Agencies");
        BRTransfer.UpdateTransferAgencies();

        //actualizando idiomas de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Languages");
        BRTransfer.UpdateTransferLanguages();

        //actualizando mercados de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Markets");
        BRTransfer.UpdateTransferMarkets();

        //actualizando disponibilidad por grupos
        AddLogGridReservations("Update", "Updating Unavailable Motives by Groups");
        BRTransfer.UpdateTransferUnavailableMotivesByGroups();

        //actualizando motivos de indisponibilidad de la tabla de transferencia por agencia
        AddLogGridReservations("Update", "Updating Unavailable Motives by Agency");
        BRTransfer.UpdateTransferUnavailableMotivesByAgency();

        //actualizando motivos de indisponibilidad de la tabla de transferencia por pais
        AddLogGridReservations("Update", "Updating Unavailable Motives by Country");
        BRTransfer.UpdateTransferUnavailableMotivesByCountry();

        //actualizando motivos de indisponibilidad de la tabla de transferencia por contrato
        AddLogGridReservations("Update", "Updating Unavailable Motives by Contract");
        BRTransfer.UpdateTransferUnavailableMotivesByContract();

        if (blnOneNight)
        {
          //actualizando motivos de indisponibilidad de la tabla de transferencia por 1 noche
          AddLogGridReservations("Update", "Updating Unavailable Motives by Just One Night");
          BRTransfer.UpdateTransferUnavailableMotivesBy1Night();
        }
        if (blnTwoNight)
        {
          //actualizando motivos de indisponibilidad de la tabla de transferencia por 2 noches
          AddLogGridReservations("Update", "Updating Unavailable Motives by Just Two Nights");
          BRTransfer.UpdateTransferUnavailableMotivesBy2Nights();
        }

        //actualizando motivos de indisponibilidad de la tabla de transferencia por transferencia
        AddLogGridReservations("Update", "Updating Unavailable Motives by Transfer");
        BRTransfer.UpdateTransferUnavailableMotivesByTransfer();

        //actualizando motivos de indisponibilidad de la tabla de transferencia por ser nuevo socio
        AddLogGridReservations("Update", "Updating Unavailable Motives by New Member");
        BRTransfer.UpdateTransferUnavailableMotivesByNewMember();

        //actualizando motivos de indisponibilidad de la tabla de transferencia por tener pax 1
        AddLogGridReservations("Update", "Updating Unavailable Motives by Pax");
        BRTransfer.UpdateTransferUnavailableMotivesByPax();

        //actualizando disponibilidad de la tabla de transferencia
        AddLogGridReservations("Update", "Updating Availability");
        BRTransfer.UpdateTransferAvailability();
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
    public void UpdateGuests()
    {
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        //eliminando reservaciones canceladas
        AddLogGridReservations("Delete", "Deleting Cancelled Reservations.");
        BRTransfer.DeleteReservationsCancelled();

        //actualizando numeros de habitacion de huespedes
        AddLogGridReservations("Update", "Updating Room Numbers");
        BRTransfer.UpdateGuestsRoomNumbers();

        //actualizando tipos de habitacion de huespedes
        AddLogGridReservations("Update", "Updating Room Types");
        BRTransfer.UpdateGuestsRoomTypes();

        //actualizando tarjetas de credito de huespedes
        AddLogGridReservations("Update", "Updating Credit Cards");
        BRTransfer.UpdateGuestsCreditCards();

        //actualizando apellidos de huespedes
        AddLogGridReservations("Update", "Updating Last Names");
        BRTransfer.UpdateGuestsLastNames();

        //actualizando nombres de huespedes
        AddLogGridReservations("Update", "Updating First Names");
        BRTransfer.UpdateGuestsFirstNames();

        //actualizando fechas de llegada de huespedes
        AddLogGridReservations("Update", "Updating Check-In Dates");
        BRTransfer.UpdateGuestsCheckInDates();

        //actualizando Check Ins de huespedes
        AddLogGridReservations("Update", "Updating Check-Ins");
        BRTransfer.UpdateGuestsCheckIns();

        //actualizando fechas de salida de huespedes
        AddLogGridReservations("Update", "Updating Check-Out Dates");
        BRTransfer.UpdateGuestsCheckOutDates();

        //actualizando correos electronicos de huespedes
        AddLogGridReservations("Update", "Updating E-mails");
        BRTransfer.UpdateGuestsEmails();

        //actualizando ciudades de huespedes
        AddLogGridReservations("Update", "Updating Cities");
        BRTransfer.UpdateGuestsCities();

        //actualizando estados de huespedes
        AddLogGridReservations("Update", "Updating States");
        BRTransfer.UpdateGuestsStates();

        //actualizando paises de huespedes
        AddLogGridReservations("Update", "Updating Countries");
        BRTransfer.UpdateGuestsCountries();

        //actualizando salidas anticipadas de huespedes
        AddLogGridReservations("Update", "Updating Early Check-Outs");
        BRTransfer.UpdateGuestsCheckOutsEarly();

        //actualizando tipos de huespedes
        AddLogGridReservations("Update", "Updating Guest Types");
        BRTransfer.UpdateGuestsContracts();

        //actualizando contratos de huespedes
        AddLogGridReservations("Update", "Updating Contracts");
        BRTransfer.UpdateGuestsContracts();

        //actualizando fechas de nacimiento de huespedes
        AddLogGridReservations("Update", "Updating Birth Dates");
        BRTransfer.UpdateGuestsBirthDates();

        //actualizando edades de huespedes
        AddLogGridReservations("Update", "Updating Ages");
        BRTransfer.UpdateGuestsAges();

        //actualizando pax de huespedes
        AddLogGridReservations("Update", "Updating Pax");
        BRTransfer.UpdateGuestsPax();

        //actualizando tipos de reservacion de huespedes
        AddLogGridReservations("Update", "Updating Reservation Types");
        BRTransfer.UpdateGuestsReservationTypes();

        //actualizando id del perfil de Opera de huespedes
        AddLogGridReservations("Update", "Updating Id Profile Opera");
        BRTransfer.UpdateGuestsIdProfileOpera();

        //actualizando agencias de huespedes
        AddLogGridReservations("Update", "Updating Agencies");
        BRTransfer.UpdateGuestsAgencies();

        //actualizando mercados de huespedes
        AddLogGridReservations("Update", "Updating Markets");
        BRTransfer.UpdateGuestsMarkets();

        //actualizando disponibilidad y motivos de indisponibilidad de huespedes
        AddLogGridReservations("Update", "Updating Availability & Unavailable Motives");
        BRTransfer.UpdateGuestsAvailabilityUnavailableMotives();

        //actualizando motivos de indisponibilidad de huespedes (Revirtiendo proceso de disponibilidad por una noche)
        AddLogGridReservations("Update", "Updating Unavailable Motives (Reverting One Night Availability)");
        BRTransfer.UpdateGuestsUnavailableMotives1NightRevert();


        //actualizando huespedes disponibles
        AddLogGridReservations("Update", "Updating Guests Availables.");
        DateTime dateFrom = DateHelper.DaysBeforeOrAfter(-60);
        DateTime dateTo = DateHelper.DaysBeforeOrAfter(60);
        BRTransfer.UpdateGuestsAvailables(dateFrom, dateTo);


        //agregando nuevos huespedes
        AddLogGridReservations("Insert", "Adding New Guests.");
        BRTransfer.AddGuests();
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
    public void TransferStopZone(string strZone)
    {
      if (blnRunOrCancellReservations && !WorkerReservations.CancellationPending)
      {
        AddLogGridReservations("Stop", "Stop Transafer " + strZone);
        BRTransfer.StopZone(strZone);
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
      foreach (LeadSourceShort hotel in hotels)
      {
        leadSourceId += hotel.lsID + ",";
      }
      
      leadSourceId = leadSourceId.Remove(leadSourceId.Length - 1);
      return leadSourceId;
    }
    #endregion

    #region DeleteInvalidCharacters
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
    public string SetIDName(string strString)
    {
      string str = "?"; 
      if (!String.IsNullOrEmpty(strString) || !String.IsNullOrWhiteSpace(strString))
      {
        str = strString;
      }
      return str;
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

    #region Metodos para actualizar botones

    #region UpdateDelegateButton
    ///<summary>Delegado utilizado en la actualizacion el content de los botones</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public delegate void UpdateDelegateButton(System.Windows.Controls.Button btnName, string strContent);
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
    public void ContentButton(System.Windows.Controls.Button btnName, string strContetn)
    {
      btnName.Content = strContetn;
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
    public void UpdateButton(System.Windows.Controls.Button btnName, string strContent)
    {
      if (updateContentButton == null)
      {
        updateContentButton = new UpdateDelegateButton(ContentButton);
      }
      btnName.Dispatcher.BeginInvoke(DispatcherPriority.Render, updateContentButton, btnName, strContent);
    }
    #endregion

    #endregion

    #region Actualizacion de Grids

    #region UpdateDelegateGridReservations
    ///<summary>Metodo para crear delegado utilizado en la actualizacion el grid del log de reservations</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public delegate void UpdateDelegateGridReservations(DateTime date, string logLevel, string message);
    #endregion

    #region UpdateDelegateGridExchangeRate
    ///<summary>Metodo para crear delegado utilizado en la actualizacion el grid del log de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public delegate void UpdateDelegateGridExchangeRate(DateTime date, string logLevel, string message);
    #endregion

    #region UpdatingGrid
    /// <summary>
    /// Metodo para actualizar datagrid de los log
    /// </summary>
    /// <param name="dataGrid">Datagrid a actualizar</param>
    /// <param name="listTransations">Lista de transacciones a enviar al datagrid</param>
    public void UpdatingGrid(System.Windows.Controls.DataGrid dataGrid, List<LogHelper.Transaction> listTransations)
    {
      dataGrid.ItemsSource = null;
      dataGrid.ItemsSource = listTransations.Skip(Math.Max(0, listTransations.Count() - 499)).Take(499).ToList();
      dataGrid.Items.Refresh();
      /*dataGrid.SelectedIndex = lastItemIndex;
      dataGrid.ScrollIntoView(dataGrid.Items[lastItemIndex]);
      dataGrid.UpdateLayout();
      dataGrid.ScrollIntoView(dataGrid.SelectedItem);
      dataGrid.CurrentCell = new DataGridCellInfo(
      dataGrid.Items[0], dataGrid.Columns[3]);
      dataGrid.BeginEdit();*/
      int index = dataGrid.Items.Count - 1;
      GridHelper.SelectRow(dataGrid, index);

    }
    #endregion

    #region UpdateDelegateDatagridReservations
    ///<summary>Metodo que agrega nuevo registro en el log de reservations y actualiza el grid de log de reservations</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void UpdateDelegateDatagridReservations(DateTime date, string logLevel = "", string message = "")
    {
      listTransactionsReservations.Add(LogHelper.AddTransaction("Reservations", date, logLevel, message));
      UpdatingGrid(grdLogReservations, listTransactionsReservations);
    }
    #endregion

    #region UpdateDelegateDatagridExchangeRate
    ///<summary>Metodo que agrega nuevo registro en el log de reservations y actualiza el grid de log de exchange rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void UpdateDatagridExchangeRate(DateTime date, string logLevel = "", string message = "")
    {
      listTransactionsExchangeReservations.Add(LogHelper.AddTransaction("ExchangeRate", date, logLevel, message));
      UpdatingGrid(grdLogExchangeRate, listTransactionsExchangeReservations);
    }
    #endregion

    #region AddLogGridReservations
    public void AddLogGridReservations(string strLogLevel, string strLogMessage)
    {
      if (updateGridReservations == null)
      {
        updateGridReservations = new UpdateDelegateGridReservations(UpdateDelegateDatagridReservations);
      }
      grdLogReservations.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateGridReservations, DateTime.Now, strLogLevel, strLogMessage);
    }
    #endregion

    #region AddLogGridExchangeRate
    public void AddLogGridExchangeRate(string strLogLevel, string strLogMessage)
    {
      if (updateGridExchangeRate == null)
      {
        updateGridExchangeRate = new UpdateDelegateGridExchangeRate(UpdateDatagridExchangeRate);
      }
      grdLogExchangeRate.Dispatcher.BeginInvoke(DispatcherPriority.Normal, updateGridExchangeRate, DateTime.Now, strLogLevel, strLogMessage);
    }
    #endregion

    #endregion

    #region Metodos de Cancelacion de trasnferencias

    #region CancellWorkerReservations
    ///<summary>Metodo que cancela y finaliza la tarea de transferencia de Reservations</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void CancellWorkerReservations()
    {
      if (WorkerReservations.WorkerSupportsCancellation)
      {
        if (WorkerReservations.IsBusy)
        {
          AddLogGridReservations("Info", "Canceling Transfer Reservations");
          ReloadReservations();
          WorkerReservations.CancelAsync();
        }  
      }
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
        if (WorkerExchangeRate.IsBusy)
        {
          AddLogGridReservations("Info", "Canceling Exchange Rate");
          WorkerExchangeRate.CancelAsync();

        }
      }
    }
    #endregion

    #endregion

    #region Metodos para reinicar valores de Exchange Rate y Reservations

    #region ReloadReservations
    /// <summary>
    /// Metotdo que reinicia los valores para la transferencia de reservaciones
    /// </summary>
    /// <history>
    /// [michan]  28/04/2016/ Created
    /// </history>
    public void ReloadReservations()
    {
      blnRunTransfer = false;
      AddValueProgressBarReservations(porcentBase: 0);
      blnRunOrCancellReservations = false;
      UpdateButton(btnReservations, "Run Reservations");
      UpdateLabel(lblPorcentProgresBar, "");
      UpdateLabel(lblTransferReservations, "");
    }
    #endregion

    #region ReloadExchangeRate
    /// <summary>
    /// Metotdo que reinicia los valores para la transferencia de Exchange Rate
    /// </summary>
    /// <history>
    /// [michan]  28/04/2016/ Created
    /// </history>
    public void ReloadExchangeRate()
    {
      AddLogGridExchangeRate("Finished", "Finish Exchange Rate");
      AddValueProgressBarExchangeRate(value: 0);
      blnRunOrCancellExchangeRate = false;
      blnRunTransfer = false;
      UpdateButton(btnExchangeRate, "Run Exchange Rate");
      UpdateLabel(lblPorcentProgresBar, "");
    }
    #endregion
    
    #endregion

    #region Actualiza Labels

    #region UpdateDelegateLabelText
    /// <summary>
    /// Delegado para actualizar los labels del formulario en interacción.
    /// </summary>
    /// <param name="lblUpdate">label que se desea actualizar</param>
    /// <param name="stringContent">Cadena que se desea pintar en el label</param>
    private delegate void UpdateDelegateLabel(System.Windows.Controls.Label lblUpdate, string stringContent);
    #endregion

    #region UpdateLabelDelegate
    /// <summary>
    /// Metodo que recibe los valores para el label aactualizar
    /// </summary>
    /// <param name="lblUpdate">Labeel a aactualizar</param>
    /// <param name="stringContent">Contenido del label</param>
    private void UpdateLabelDelegate(System.Windows.Controls.Label lblUpdate, string stringContent)
    {
      lblUpdate.Content = stringContent;
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
      if (updateDelegateLabel == null)
      {
        updateDelegateLabel = new UpdateDelegateLabel(UpdateLabelDelegate);
      }
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
      UpdateLabel(lblTransferReservations, stringContent);
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
      //Configuración del ProgressBar
      
      progresBarExchangeRate.Minimum = 0;//valor mínimo (inicio de la barra de carga)
      progresBarExchangeRate.Maximum = 100;//valor máximo(hasta donde se carga, como ejemplo 100)
      progresBarExchangeRate.Value = 0;//valor de inicio
      

      //new SolidColorBrush(Color.FromArgb(128, 255, 0, 0)) brdName.BorderBrush = (blnOnOff) ? System.Windows.Media.Brushes.Black : System.Windows.Media.Brushes.White;
      lblPorcentProgresBar.Content = "";//String.Format("{0:0%}", 0);

      //Configuración del ProgressBar
      
      progresBarReservations.Minimum = 0;//valor mínimo (inicio de la barra de carga)
      progresBarReservations.Maximum = 100;//valor máximo(hasta donde se carga, como ejemplo 100)
      progresBarReservations.Value = 0;//valor de inicio
      
      lblPorcentProgresBarReservations.Content = "";//String.Format("{0:0%}", 0);


    }
    #endregion

    #region ChangedProgresBarTransfer
    /// <summary>
    /// Metodo que se encarga de actualizar el progresbar para las transsferencias
    /// </summary>
    ///<history>
    ///[michan] 25/04/2016  Created
    ///</history>
    private void progresBarTransfer_Changed(object sender, ProgressChangedEventArgs e)
    {
      progresBarExchangeRate.Value = e.ProgressPercentage;
      lblPorcentProgresBar.Content = String.Format("{0:0} %", e.UserState);
    }
    #endregion

    #region ChangedProgresBarReservations
    /// <summary>
    /// Metodo que se encarga de actualizar el progresbar para las transferencias de reservaciones
    /// </summary>
    ///<history>
    ///[michan] 4/05/2016  Created
    ///</history>
    private void progresBarReservations_Changed(object sender, ProgressChangedEventArgs e)
    {
      progresBarReservations.Value = e.ProgressPercentage;
      lblPorcentProgresBarReservations.Content = String.Format("{0:0} %", e.UserState);
    }
    #endregion

    #region AddValueProgressBar
    ///<summary>Metodo que actualiza la barra de progreso</summary>
    ///<param name="value">recibe el valor de la carra de progreso del 1 al  10</param>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    public void AddValueProgressBar(BackgroundWorker worker, double? value = null, double? porcentBase = null)
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
      if ((valueIncrement >=0) && (porcentValue >=0) && (!valueIncrement.Equals(null)) && (!porcentValue.Equals(null)) && worker != null)
      {
        worker.ReportProgress(valueIncrement, porcentValue);
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
      AddValueProgressBar(worker: WorkerReservations, porcentBase:porcentBase);
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
      AddValueProgressBar(worker: WorkerExchangeRate, value: value);
    }
    #endregion

    #endregion

    #region WorkerRunWorkerExchangeRateCompleted
    ///<summary>Metodo que se ejecuta cuando se finaliza alguna tarea</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private void WorkerRunWorkerExchangeRateCompleted(object Sender, RunWorkerCompletedEventArgs e)
    {
      OnOffBlinkExchangeRate(false);
      UpdateLabel(lblPorcentProgresBar, "");
      blnRunOrCancellExchangeRate = false;
      _lastExchangeRate = DateTime.Now;
      UpdateLabel(lblLastTransferExchangeRate, lblTextLast + _lastExchangeRate.ToString());
      _nextExchangeRate = DateHelper.AddTimeDate(_tranferExchangeRatesIntervalTime);
      UpdateLabel(lblNextTransferExchangeRate, lblTextNext + _nextExchangeRate.ToString());
      if (!e.Cancelled)
      {
        UpdateLabelStatusExchangeRate("FINISHED");
      }
      else if (e.Cancelled)
      {
        UpdateLabelStatusExchangeRate("CANCELLED");
      }
      else if (!(e.Error == null))
      {
        UpdateLabelStatusExchangeRate("ERROR");
      }
      else
      {
        UpdateLabelStatusExchangeRate("STOP");
      }
      AddLogGridExchangeRate("Finished", "Process finished");
      UpdateLabelStatusExchangeRate("STAND BY");
    }
    #endregion

    #region WorkerRunWorkerReservationsCompleted
    ///<summary>Metodo que se ejecuta cuando se finaliza alguna tarea</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private void WorkerRunWorkerReservationsCompleted(object Sender, RunWorkerCompletedEventArgs e)
    {
      OnOffBlinkReservations(false);
      UpdateLabel(lblPorcentProgresBarReservations, "");
      blnRunOrCancellReservations = false;
      _lastReservations = DateTime.Now;
      _nextReservations = DateHelper.AddTimeDate(_intervalTimeReservations);
      UpdateLabel(lblLastTransferReservations, lblTextLast + _lastReservations.ToString());
      UpdateLabel(lblNextTransferReservations, lblTextNext + _nextReservations.ToString());

      if (!e.Cancelled)
      {
        UpdateLabelStatusReservations("FINISHED");
      }
      else if (e.Cancelled)
      {
        UpdateLabelStatusReservations("CANCELLED");
      }
      else if (!(e.Error == null))
      {
        UpdateLabelStatusReservations("ERROR"); 
      }
      else
      {
        UpdateLabelStatusReservations("STOP");
      }    
      UpdateLabelStatusReservations("STAND BY");
    }
    #endregion

    #region Metodos de transferencia de Exchange Rate

    #region WorkerUpdateExchangeRateDoWork
    ///<summary>Metodo que contiene la tarea que realiza todo el proceso de actualizacion de Exchange Rate</summary>
    ///<history>
    ///[michan] 15/04/2016 Created
    ///</history>
    private void WorkerUpdateExchangeRateDoWork(object Sender, DoWorkEventArgs e)
    {
      // Se valida el estatus del formulario
      StatusForm(Sender, e);
      //Se inicia el proceso para Exchange Rate
      blnRunOrCancellExchangeRate = true;
      UpdateButton(btnExchangeRate, "Cancell Exchange Rate");
      OnOffBlinkExchangeRate();
      DateTime startExchangeRate = DateTime.Now;
      DateTime endExchangeRate;
        
      //Se obtiene la fecha del servidor
      _dtpServerDate = BRHelpers.GetServerDate();
        
            
      AddLogGridExchangeRate("Start", "Start Updating Exchange Rates");
      AddValueProgressBarExchangeRate(value: 1);
      Thread.Sleep(100);
      //agregamos los tipos de cambio faltantes hasta la fecha actual para que no existan huecos
      BRExchangeRate.InsertExchangeRate(_dtpServerDate);

        
      AddValueProgressBarExchangeRate(value: 2);
      
      //obtenemos el tipo de cambio de la Intranet
      AddLogGridExchangeRate("Info", "Getting Exchange Rate from Intranet Service");
      TipoCambioTesoreria exchangeRate = IntranetHelper.TipoCambioTesoreria(Convert.ToDateTime("01/08/2008"), "USD");//Convert.ToDateTime("01/08/2014")
      if (exchangeRate != null)
      {
        _exchangeRateType = Convert.ToDecimal(exchangeRate.TipoCambio);
        //comparamos si el cambio es positivo y mayor a cero
        if (_exchangeRateType > 0)
        {
          _currencyId = "MX";
          AddValueProgressBarExchangeRate(value: 3);
          //Se realiza la actualizacion y se registra en el log la inicialización

          AddLogGridExchangeRate("Info", "Updating Exchange Rate " + _currencyId + " (" + _exchangeRateType.ToString() + ")");
          AddValueProgressBarExchangeRate(value: 4);
          Thread.Sleep(100);
          BRExchangeRate.UpdateExchangeRate(_dtpServerDate, _currencyId, _exchangeRateType);
          endExchangeRate = DateTime.Now;

          AddLogGridExchangeRate("Info", "Updating Exchange Rate in " + DateHelper.timeDuration(startExchangeRate, endExchangeRate).ToString(@"hh\ \h\ \:mm\ \m\ \:ss\ \s\ \:fff\ \m\s"));
          AddValueProgressBarExchangeRate(value: 6);
          Thread.Sleep(100);
          AddLogGridExchangeRate("Success", "Exchange Rate finished");
          AddValueProgressBarExchangeRate(value: 10);
          Thread.Sleep(100);
        }
        else
        {
          // Registramos en el log el tipo de error que se encontro, en el tipo de cambio
          AddLogGridExchangeRate("Error", "Exchange rate must be positive " + _currencyId + " (" + _exchangeRateType.ToString() + ")");
          //CancellWorkerExchangeRate();
        }
      }
      else
      {
        // Registramos en el log que no existen cambios en la fecha especificada y el tipo de moneda
        AddLogGridExchangeRate("Warning", "Exchange rate does not exists for day");
        //CancellWorkerExchangeRate();
      }
      ReloadExchangeRate();
    }
    #endregion

    #endregion

    #region Metodos de Ventana

    #region Window_Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      
      LoadDataGRidExchangeRate();
      LoadDataGRidReservations();
      InitializerProgressBar();

      var thisApp = Assembly.GetExecutingAssembly();
      AssemblyName name = new AssemblyName(thisApp.FullName);
      this.Title = " Inteligence Marketing Transfer v"+name.Version;
      
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

    #region Window_StateChanged
    /// <summary>
    /// Metodo de la ventana para determinar el status de la ventana
    /// </summary>
    ///<history>
    ///[michan] 29/04/2016  Created
    ///</history>
    public void Window_StateChanged(object sender, EventArgs e)
    {
      /// cada vez que cambie el status de la ventana independientemente si es por el notifyicon o del boton minimizar de la ventana
      /// se verificara el state y se procede a ocultar y a mostrar la nube del notifyicon.
      NotifyIcon notifyIcon = NotifyIconHelper.Notify(this);
      if (this.WindowState == WindowState.Minimized)
      {
        this.Hide();
        //notifyIcon = NotifyIconHelper.Notify(this);// Creamos el objeto notifyicon
        if (notifyIcon != null)
        {
          notifyIcon.ShowBalloonTip(500);
        }
      }
      else if (this.WindowState == WindowState.Normal)
      {
        notifyIcon.Visible = false;
        this.ShowInTaskbar = true;
        this.Show();
        this.Activate();
      }
    }
    #endregion

    #region Window_KeyDown
    /// <summary>
    /// Ejecuta tareas con algunas teclas de configuración se realizan tareas.
    /// </summary>
    /// <history>
    /// [michan] 28/04/2016 Created
    /// </history>
    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
      {
        pushCtrl = true;
      }
      
      if ((e.Key == Key.F4) && (e.Key == Key.LeftAlt || e.Key == Key.RightAlt))
      {
        this.WindowState = WindowState.Minimized;
      }

      if (e.Key == Key.E && pushCtrl)
      {
        pushCtrlE = true;
        this.Close();
      }
      else if (e.Key == Key.P && pushCtrl)
      {
        ShowInfoUser();
      }
      else if (e.Key == Key.S && pushCtrl)
      {
        UIHelper.ShowMessage("Setup");
      }
    }
    #endregion

    #region ShowInfoUser
    /// <summary>
    /// Muestra información sobre el usuario
    /// </summary>
    /// <history>
    /// [michan] 25/04/2016 Created
    /// </history>
    private void ShowInfoUser()
    {

      string userName = Environment.UserName;
      string machineName = Environment.MachineName;
      string ipAddress = "Local IP Address Not Found!"; ;

      var host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (var ip in host.AddressList)
      {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
          ipAddress = ip.ToString();
          break;
        }
      }

      string message = String.Format("User Name: {0}\nComputer Name Local: {1}\nComputer IP Address Local: {2}\nComputer Name Remote: {3}\nComputer IP Address Remote: {4}", userName, machineName, ipAddress, String.Empty, String.Empty);

      UIHelper.ShowMessage(message);
    }
    #endregion

    #region StatusForm
    /// <summary>
    /// Metodo que valida el estatus de formulario
    /// </summary>
    ///<history>
    ///[michan] 28/04/2016 Created
    ///</history>
    public void StatusForm(object sender, DoWorkEventArgs e)
    {
      Dispatcher.Invoke
      (
        new Action
        (() =>
        {
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

    #region Window_Closing
    /// <summary>
    /// Evita que el formulario se cierre al oprimir el botón de cerrar
    /// </summary>
    /// <history>
    /// [michan] 25/04/2016 Created
    /// </history>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      e.Cancel = !pushCtrlE;
      this.WindowState = WindowState.Minimized;
    }


    #endregion
    
    #endregion

    #region Metodos de acciones de botones

    #region Run or Cancell Reservations
    /// <summary>
    /// Acción para cancelar o inicializar la trasnferencia de reservaciones
    /// </summary>
    /// <history>
    /// [michan] 28/04/2016 Created
    /// </history>
    private void btnReservations_Click(object sender, RoutedEventArgs e)
    {
      if (blnRunOrCancellReservations)
      {
        CancellWorkerReservations();
      }
      else
      {
        if (!blnRunOrCancellExchangeRate)
        {
          StartReservations();
        }
        else
        {
          Task.Factory.StartNew(() =>
          {
            UIHelper.ShowMessage("An update is running , please wait...", MessageBoxImage.Error, "RunReservations");
          });
        }

      }
    }
    #endregion

    #region Run or Cancell Exchange Rate
    /// <summary>
    /// Acción para cancelar o inicializar la trasnferencia de Exchange Rate
    /// </summary>
    /// <history>
    /// [michan] 28/04/2016 Created
    /// </history>
    private void btnExchangeRate_Click(object sender, RoutedEventArgs e)
    {
      if (blnRunOrCancellExchangeRate)
      {
        CancellWorkerExchangeRate();
      }
      else
      {
        if (!blnRunOrCancellReservations)
        {
          StartExchangeRate();
        }
        else
        {
          Task.Factory.StartNew(() =>
          {
            UIHelper.ShowMessage("An update is running , please wait...", MessageBoxImage.Error, "RunExchangeRate");
          });
          
        }

      }
      
    }
    #endregion}

    #endregion

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

    public void Blink(DispatcherTimer dispatcherTime, System.Windows.Controls.Label lblName, System.Windows.Controls.Border brdName, bool? blnStatus = true)
    {
      if (dispatcherTime != null && !blnStatus.Value)
      {
        dispatcherTime.Stop();
        if (!dispatcherTime.IsEnabled)
        {
          if(lblName != null)
          {
            lblName.Foreground = Brushes.Black;
            lblName.Background = Brushes.Transparent;
          }
          if(brdName != null)
          {
            brdName.Background = Brushes.Transparent;
            brdName.BorderBrush = Brushes.Black;
          }
        }
      }
      else if (dispatcherTime == null && blnStatus.Value)
      {
        dispatcherTime = BlinkLabel(lblName: lblName, brdName: brdName);
        dispatcherTime.Start();
      }
      else if (dispatcherTime != null && blnStatus.Value)
      {
        dispatcherTime.Start();
      }
      
    }

    #region BlinkLabel 

    /// <history>
    /// [michan]  02/05/2016  Created
    /// </history>   
    public DispatcherTimer BlinkLabel(System.Windows.Controls.Label lblName = null, System.Windows.Controls.Border brdName = null, bool? blnStatus = true) //where T : Window //, 
    {
      bool blnOnOff = false;
      
      DispatcherTimer dispatcher = new DispatcherTimer();
      dispatcher.Interval = new TimeSpan(0, 0, 0, 0, 500);
      dispatcher.Tick += (s, a) =>
      {
        if (lblName != null)
        {
          lblName.Foreground = (blnOnOff) ? Brushes.Black :Brushes.Transparent;
          lblName.Background = Brushes.Transparent;
          //lblName.BorderBrush = (blnOnOff) ? System.Windows.Media.Brushes.BlueViolet : System.Windows.Media.Brushes.Black;
        }
        
        if (brdName != null)
        {
          brdName.Background = (blnOnOff) ? Brushes.DarkRed : Brushes.Transparent;
          brdName.Opacity = (blnOnOff) ? .6 : 0;
          brdName.BorderBrush = Brushes.Black;
          //new SolidColorBrush(Color.FromArgb(128, 255, 0, 0)) brdName.BorderBrush = (blnOnOff) ? System.Windows.Media.Brushes.Black : System.Windows.Media.Brushes.White;
        }

        blnOnOff = !blnOnOff;
      };
      return dispatcher;
    }

    #endregion
  }
}
