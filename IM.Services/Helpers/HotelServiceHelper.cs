using System;
using IM.Base.Helpers;
using IM.BusinessRules.BR;
using IM.Services.HotelService;
using PalaceResorts.Common.PalaceTools;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.Services.Helpers
{
  public class HotelServiceHelper
  {
    #region Atributos

    private static ServiceInterface _service;

    #endregion

    #region Propiedades
    /// <summary>
    /// Recupera una instacia del servicio web
    /// </summary>
    /// <history>
    /// [michan] 14/04/2016
    /// </history>
    public static ServiceInterface Current
    {
      get
      {
        
        if (_service == null)
        {
          //creamos una instancia del servicio
          _service = new ServiceInterface();
          _service.Url = ConfigHelper.GetString("Hotel.URL");
          Current.Timeout = int.Parse(ConfigHelper.GetString("TimeOutWebService"));
          // insertamos las cabeceras de autenticacion
          RequestHeader requestHander = new RequestHeader();
          requestHander.Headers = ServiceHelper.GetSecurityTokenHeaders();
          _service.RequestHeaderValue = requestHander;
        }
        
        return _service;

      }
    }
    #endregion

    #region Metodos

    #region GetReservationsByArrivalDate
      /// <summary>
      /// Obtiene las reservaciones por dia
      /// </summary>
      /// <param name="zone">Zona</param>
      /// <param name="dateFrom">Desde</param>
      /// <param name="dateTo">Hasta</param>
      /// <param name="hotels">Hoteles</param>
      /// <returns>List<ReservationOrigosTransfer></ReservationOrigosTransfer></returns>
      /// <history>
      /// [michan] 14/04/2016  Created
      /// </history>
    public async static Task<List<ReservationOrigosTransfer>> GetReservationsByArrivalDate(string zone, DateTime dateFrom, DateTime dateTo, string hotels, System.Threading.CancellationToken cancellationToken)
    {
      
      List<ReservationOrigosTransfer> lstOrigosTransfer = null;
      //using (var linkedTokenSource = System.Threading.CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
      //{
      
      await Task.Run(() =>
      {
        QueryRequest request = new QueryRequest();
        ReservationOrigosTransferResponse response = new ReservationOrigosTransferResponse();
        //System.Threading.CancellationToken disconnectedToken = Current.Abort();//..ClientDisconnectedToken;
        //var source = System.Threading.CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, disconnectedToken);
        //configuramos el Request
        request.Zona = zone;
        request.Desde = dateFrom;
        request.Hasta = dateTo;
        request.Hotel = hotels;
        //invocamos al servicio web
        response = Current.GetReservationsByArrivalDate(request);

        if (response.Data.Length > 0)
        {
          lstOrigosTransfer = response.Data?.Cast<ReservationOrigosTransfer>().ToList();
        }
      }, cancellationToken);
      
      //}
    return lstOrigosTransfer;
    }
    #endregion

    #region ObtenerFactoresConversion
    public async static Task<Rmmoney> ObtenerFactoresConversion(string strHotel, System.Threading.CancellationToken cToken = new System.Threading.CancellationToken())
    {
      Rmmoney canadianCurrency = null;
      await Task.Run(() => { 
        HotelStringRequest request = new HotelStringRequest();
        RmmoneyResponse response = new RmmoneyResponse();
        // configuramos el request
        request.Hotel = strHotel;
        request.FechaInicial = BRHelpers.GetServerDate();
        request.FechaFinal = BRHelpers.GetServerDate();
        //invocamos al servicio web
        response = Current.ObtenerFactoresConversion(request);// ObtenerFactoresConversion
        if (response.Data.Length > 0 && response.Data != null)
        {
          canadianCurrency = response.Data.SingleOrDefault(currency => currency.code == "DC");
        }
      }, cToken);
      return canadianCurrency;


    }
    #endregion

    #endregion

  }
}
