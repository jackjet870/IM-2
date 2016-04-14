using System;
using IM.Base.Helpers;
using IM.Services.HotelService;
using PalaceResorts.Common.PalaceTools;
using System.Windows;

namespace IM.Services.Helpers
{
    class HotelService
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
                    _service.Timeout = int.Parse(ConfigHelper.GetString("TimeOutWebService"));
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
        /// Obtiene el cambio de tipo de tesoreria
        /// </summary>
        /// <param name="zone">Zona</param>
        /// <param name="dateFrom">Desde</param>
        /// <param name="dateTo">Hasta</param>
        /// <param name="hotels">Hoteles</param>
        /// <history>
        /// [michan] 14/04/2016  Created
        /// </history>
        public static ReservationOrigosTransfer GetReservationsByArrivalDate(string zone, DateTime dateFrom, DateTime dateTo, string hotels)
        {
            
            QueryRequest request = new QueryRequest();
            ReservationOrigosTransferResponse response = new ReservationOrigosTransferResponse();
            ReservationOrigosTransfer data = null;
           
            //configuramos el Request
            request.Zona = zone;
            request.Desde = dateFrom;
            request.Hasta = dateTo;
            request.Hotel = hotels;

            //invocamos al servicio web
            
            response = Current.GetReservationsByArrivalDate(request);

            //Si ocurrio un error 
            if (response.HasErrors)
            {
                UIHelper.ShowMessage(response.ExceptionInfo.Message, MessageBoxImage.Error, "GetReservationsByArrivalDate");
            }

            if (response.Data.Length > 0)
            {
                data = response.Data[0];
            }

            return data;
        }
        #endregion

        #endregion
    }
}
