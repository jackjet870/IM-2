using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
    public class BRTransfer
    {
    #region Start
    /// <summary>
    /// Indica que la transferencia de reservaciones ha iniciado, toma las zonas activas
    /// </summary>
    /// <returns>List<TransferStartData></returns>
    /// <history>
    /// [michan] 13/Abril/2016 Created
    /// </history>
    public static List<TransferStartData> Start()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferStart().ToList();

        }
    }
    #endregion

    #region Stop
    /// <summary>
    /// Indica que la transferencia de reservaciones ha terminado
    /// </summary>
    /// <returns>List<TransferStartData></returns>
    /// <history>
    /// [michan] 13/Abril/2016 Created
    /// </history>
    public static void Stop()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            dbContext.USP_OR_TransferStop();

        }
    }
#endregion

    #region StopZone

    /// <summary>
    /// Indica que la transferencia de reservaciones de una zona ha terminado
    /// </summary>
    /// <param name="zone">Id de la zona</param>
    /// <history>
    ///   [michan] 24/02/2016 Created
    /// </history>
    /// 
    public static void StopZone(string zone)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_TransferStopZone(zone);
      }
    }

    #endregion

    #region AddCountries

    /// <summary>
    /// Agrega las paises en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int AddCountries()
        {
            using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
            {
                return dbContext.USP_OR_TransferAddCountries();
            }
        }
        #endregion

    #region AddCountriesHotel

    /// <summary>
    /// Agrega los paises del sistema Hotel en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int AddCountriesHotel()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferAddCountriesHotel();
        }
    }
    #endregion

    #region UpdateCountriesNames

    /// <summary>
    /// Actualiza las descripciones de los paises en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateCountriesNames()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateCountriesNames();
        }
    }
    #endregion

    #region UpdateCountriesHotelNames

    /// <summary>
    /// Actualiza las descripciones de los paises del sistema de Hotel en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateCountriesHotelNames()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateCountriesHotelNames();
        }
    }
    #endregion

    #region AddAgencies

    /// <summary>
    /// Agrega las agencias en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int AddAgencies()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferAddAgencies();
        }
    }
    #endregion

    #region AddAgenciesHotel

    /// <summary>
    /// Actualiza las descripciones de los paises del sistema de Hotel en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int AddAgenciesHotel()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferAddAgenciesHotel();
        }
    }
    #endregion

    #region UpdateAgenciesNames

    /// <summary>
    /// Actualiza las descripciones de las agencias en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateAgenciesNames()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateAgenciesNames();
        }
    }
    #endregion

    #region UpdateAgenciesHotelNames

    /// <summary>
    /// Actualiza las descripciones de las agencias del sistema de Hotel en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateAgenciesHotelNames()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateAgenciesHotelNames();
        }
    }
    #endregion

    #region AddRoomTypes

    /// <summary>
    /// Agrega los tipos de habitacion en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int AddRoomTypes()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferAddRoomTypes();
        }
    }
    #endregion

    #region UpdateRoomTypesNames

    /// <summary>
    /// Actualiza las descripciones de los tipos de habitacion en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateRoomTypesNames()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateRoomTypesNames();
        }
    }
    #endregion

    #region AddContracts

    /// <summary>
    /// Agrega los contratos en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int AddContracts()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferAddContracts();
        }
    }
    #endregion

    #region UpdateContractsNames

    /// <summary>
    /// Actualiza las descripciones de los tipos de habitacion en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateContractsNames()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateContractsNames();
        }
    }
    #endregion

    #region AddGroups

    /// <summary>
    /// Agrega los grupos en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int AddGroups()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferAddGroups();
        }
    }
    #endregion

    #region UpdateGroupsNames

    /// <summary>
    /// Actualiza las descripciones de los grupos en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateGroupsNames()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGroupsNames();
        }
    }
    #endregion

    #region UpdateTransferCountries

    /// <summary>
    /// Actualizar países de reservaciones migradas en el proceso de transferencia
    /// Actualiza las países de las reservaciones migradas
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferCountries()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferCountries();
        }
    }
    #endregion

    #region UpdateTransferAgencies

    /// <summary>
    /// Actualizar agencias de reservaciones migradas en el proceso de transferencia
    /// Actualiza las agencias de las reservaciones migradas
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferAgencies()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferAgencies();
        }
    }
    #endregion

    #region UpdateTransferLanguages

    /// <summary>
    /// Actualizar idiomas de reservaciones migradas en el proceso de transferencia
    /// Actualiza los idiomas de las reservaciones migradas
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferLanguages()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferLanguages();
        }
    }
    #endregion

    #region UpdateTransferMarkets

    /// <summary>
    /// Actualizar mercado de reservaciones migradas en el proceso de transferencia
    /// Actualiza el mercado de reservaciones migradas
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferMarkets()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferMarkets();
        }
    }
    #endregion

    #region UpdateTransferUnavailableMotivesByGroups

    /// <summary>
    /// Actualizar motivo de indisponibilidad de reservaciones migradas por grupos en el proceso de transferencia
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por grupos (2 = WITH GROUP)
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferUnavailableMotivesByGroups()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByGroups();
        }
    }
    #endregion

    #region UpdateTransferUnavailableMotivesByAgency

    /// <summary>
    /// Actualizar motivo de indisponibilidad de reservaciones migradas por agencia en el proceso de transferencia
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por agencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferUnavailableMotivesByAgency()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByAgency();
        }
    }
    #endregion

    #region UpdateTransferUnavailableMotivesByCountry

    /// <summary>
    /// Actualizar motivo de indisponibilidad de reservaciones migradas por país en el proceso de transferencia
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por país
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferUnavailableMotivesByCountry()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByCountry();
        }
    }
    #endregion

    #region UpdateTransferUnavailableMotivesByContract

    /// <summary>
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por contrato en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferUnavailableMotivesByContract()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByContract();
        }
    }
    #endregion

    #region UpdateTransferUnavailableMotivesBy1Night

    /// <summary>
    /// Actualizar motivo de indisponibilidad de reservaciones migradas por 1 noche en el proceso de transferencia
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por 1 noche (1 - JUST ONE NIGHT)
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferUnavailableMotivesBy1Night()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesBy1Night();
        }
    }
    #endregion

    #region UpdateTransferUnavailableMotivesBy2Nights

    /// <summary>
    /// Actualizar motivo de indisponibilidad de reservaciones migradas por 2 noches en el proceso de transferencia
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por 2 noches (13 - JUST TWO NIGHTS)
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferUnavailableMotivesBy2Nights()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesBy2Nights();
        }
    }
    #endregion

    #region UpdateTransferUnavailableMotivesByTransfer

    /// <summary>
    /// Actualizar motivo de indisponibilidad de reservaciones migradas por transferencia en el proceso de transferencia
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por transferencia (24 = TRANSFER)
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferUnavailableMotivesByTransfer()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByTransfer();
        }
    }
    #endregion

    #region UpdateTransferUnavailableMotivesByNewMember

    /// <summary>
    /// Actualizar motivo de indisponibilidad de reservaciones migradas por ser nuevo socio en el proceso de transferencia
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por ser nuevo socio (18 = NEW MEMBER)
    /// Un huésped es definido como nuevo socio si tiene una venta de cuando mucho una semana
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferUnavailableMotivesByNewMember()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByNewMember();
        }
    }
    #endregion

    #region UpdateTransferUnavailableMotivesByPax

    /// <summary>
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por pax (35 - PAX)
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferUnavailableMotivesByPax()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByPax();
        }
    }
    #endregion

    #region UpdateTransferAvailability

    /// <summary>
    /// Actualizar disponibilidad de las reservaciones migradas en el proceso de transferencia
    /// Actualiza la disponibilidad de las reservaciones migradas
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateTransferAvailability()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateTransferAvailability();
        }
    }
    #endregion

    #region UpdateGuestsRoomNumbers

    /// <summary>
    /// Actualiza los numeros de habitacion de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// Actualiza solo si aun no tiene show
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateGuestsRoomNumbers()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsRoomNumbers();
        }
    }
    #endregion

    #region UpdateGuestsRoomTypes

    /// <summary>
    /// Actualiza los tipos de habitacion de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateGuestsRoomTypes()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsRoomTypes();
        }
    }
    #endregion

    #region UpdateGuestsCreditCards

    /// <summary>
    /// Actualiza las tarjetas de credito de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int UpdateGuestsCreditCards()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsCreditCards();
        }
    }
    #endregion

    #region UpdateGuestsLastNames

    /// <summary>
    /// Actualiza los apellidos de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// Agrega el apellido de la reservacion
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsLastNames()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsLastNames();
        }
    }
    #endregion

    #region UpdateGuestsFirstNames

    /// <summary>
    /// Actualiza los nombres de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// Agrega el nombre y apellido de la reservacion
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsFirstNames()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsFirstNames();
        }
    }
    #endregion

    #region UpdateGuestsCheckInDates

    /// <summary>
    /// Actualiza las fechas de llegada de huespedes en el proceso de transferencia
    /// La restriccion para actualizar la fecha de llegada es que no haya sido invitado en lugar de que no haya hecho Check In
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsCheckInDates()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsCheckInDates();
        }
    }
    #endregion

    #region UpdateGuestsCheckIns

    /// <summary>
    /// Actualiza los check ins de huespedes en el proceso de transferencia
    /// Si  el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsCheckIns()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsCheckIns();
        }
    }
    #endregion

    #region UpdateGuestsCheckIns

    /// <summary>
    /// Actualiza las fechas de salida de huespedes en el proceso de transferencia
    /// Actualiza el nuevo campo que tiene la fecha de salida del sistema de Hotel (guCheckOutHotelD)
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsCheckOutDates()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsCheckOutDates();
        }
    }
    #endregion

    #region UpdateGuestsEmails

    /// <summary>
    /// Actualiza los correos electronicos de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsEmails()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsEmails();
        }
    }
    #endregion

    #region UpdateGuestsCities

    /// <summary>
    /// Actualiza las ciudades de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsCities()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsCities();
        }
    }
    #endregion

    #region UpdateGuestsStates

    /// <summary>
    /// Actualiza los estados de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsStates()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsStates();
        }
    }
    #endregion

    #region UpdateGuestsCountries

    /// <summary>
    /// Actualiza los paises de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsCountries()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsCountries();
        }
    }
    #endregion

    #region UpdateGuestsCheckOutsEarly

    /// <summary>
    /// Verifica las salidas anticipadas de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsCheckOutsEarly()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsCheckOutsEarly();
        }
    }
    #endregion

    #region UpdateGuestsGuestTypes

    /// <summary>
    /// Actualiza los tipos de huesped de los huespedes en el proceso de transferencia (se usa el campo opcional 2)
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsGuestTypes()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsGuestTypes();
        }
    }
    #endregion

    #region UpdateGuestsContracts

    /// <summary>
    /// Actualiza los contratos de huespedes en el proceso de transferencia (se usa el campo opcional 1)
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsContracts()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsContracts();
        }
    }
    #endregion

    #region UpdateGuestsBirthDates

    /// <summary>
    /// Actualiza las fechas de nacimiento de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsBirthDates()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsBirthDates();
        }
    }
    #endregion

    #region UpdateGuestsAges

    /// <summary>
    /// Actualiza las edades de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsAges()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsAges();
        }
    }
    #endregion

    #region UpdateGuestsPax

    /// <summary>
    /// Actualiza el pax de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsPax()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsPax();
        }
    }
    #endregion

    #region UpdateGuestsReservationTypes

    /// <summary>
    /// Actualiza el tipo de reservacion de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsReservationTypes()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsReservationTypes();
        }
    }
    #endregion

    #region UpdateGuestsIdProfileOpera

    /// <summary>
    /// Actualiza los id's de perfiles de Opera de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// 
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsIdProfileOpera()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsIdProfileOpera();
        }
    }
    #endregion

    #region UpdateGuestsAgencies

    /// <summary>
    /// Actualiza las agencias de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsAgencies()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsAgencies();
        }
    }
    #endregion

    #region UpdateGuestsMarkets

    /// <summary>
    /// Actualiza los mercados de huespedes en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsMarkets()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsMarkets();
        }
    }
    #endregion

    #region UpdateGuestsAvailabilityUnavailableMotives

    /// <summary>
    /// Actualiza la disponibilidad y los motivos de indisponibilidad de huespedes en el proceso de transferencia
    /// Actualiza el campo de disponible por sistema
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsAvailabilityUnavailableMotives()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsAvailabilityUnavailableMotives();
        }
    }
    #endregion

    #region UpdateGuestsUnavailableMotives1NightRevert

    /// <summary>
    /// Actualiza los motivos de indisponibilidad de huespedes (revirtiendo la disponibilidad por una noche) (1 - JUST ONE NIGHT)
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsUnavailableMotives1NightRevert()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsUnavailableMotives1NightRevert();
        }
    }
    #endregion

    #region UpdateGuestsAvailables

    /// <summary>
    /// Actualiza la disponibilidad de huespedes disponibles en el proceso de transferencia.
    /// Actualiza el campo de disponible por sistema.
    /// </summary>
    /// <param name="dateFrom">Fecha de inicio</param>
    /// <param name="dateTo">Fecha de fin</param>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static int UpdateGuestsAvailables(DateTime? dateFrom, DateTime? dateTo)
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferUpdateGuestsAvailables(dateFrom, dateTo);
        }
    }
    #endregion

    #region AddGuests

    /// <summary>
    /// Agrega las reservaciones en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>

    public static List<int> AddGuests()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferAddGuests().Where(x => x.HasValue).Select(x => (int)x).ToList();
            //.Select(s => (s)).ToList();//.ToList();
        }
    }
    #endregion

    #region DeleteReservationsCancelled

    /// <summary>
    /// Elimina las reservaciones canceladas en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int DeleteReservationsCancelled()
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            return dbContext.USP_OR_TransferDeleteReservationsCancelled();
        }
    }
    #endregion

    }
}