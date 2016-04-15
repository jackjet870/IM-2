﻿using System;
using IM.Base.Helpers;
using IM.Services.IntranetService;
using PalaceResorts.Common.PalaceTools;
using System.Windows;


namespace IM.Services.Helpers
{
    public class IntranetHelper
    {
        #region Atributos

        private static ServiceInterface _service;

        #endregion

        #region Propiedades
        /// <summary>
        /// Recupera una instacia del servicio web
        /// </summary>
        /// <history>
        /// [michan] 07/04/2016
        /// </history>
        public static ServiceInterface Current
        {
            get
            {
                if (_service == null)
                {
                    //creamos una instancia del servicio
                    _service = new ServiceInterface();
                    _service.Url = ConfigHelper.GetString("Intranet.URL");
                    
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





        #region TipoCambioTesoreria
        /// <summary>
        /// Obtiene el cambio de tipo de tesoreria
        /// </summary>
        /// <param name="date">Fecha</param>
        /// <param name="currencyId">Tipo de moneda</param>
        /// <history>
        /// [michan] 07/04/2016  Created
        /// </history>
        public static TipoCambioTesoreria TipoCambioTesoreria(DateTime date, string currencyId)
        {
            TipoCambioTesoreriaRequest request = new TipoCambioTesoreriaRequest();                   
            TipoCambioTesoreriaResponse response = new TipoCambioTesoreriaResponse();
            TipoCambioTesoreria exchangeRate = null;
            RequestHeader requestHander = new RequestHeader();

            //configuramos el Request
            request.TipoCambioTesoreria = new TipoCambioTesoreria();
            
            request.TipoCambioTesoreria.Fecha = date;
            request.TipoCambioTesoreria.IdMoneda = currencyId;
            
            

            //invocamos al servicio web
            
            response = Current.ObtenerTipoCambioTesoreria(request);

            //Si ocurrio un error 
            if (response.HasErrors)
            {
                UIHelper.ShowMessage(response.ExceptionInfo.Message, MessageBoxImage.Error, "TipoCambioTesoreria");
            }
            var Data = response.Data;
            if (Data.Length > 0)
            {
                exchangeRate = Data[0];
                
            }
            
            return exchangeRate;
        }
        #endregion

        #endregion
    }
}