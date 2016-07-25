using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;
using IM.BusinessRules.Properties;
using System.Threading.Tasks;

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
    public static async Task<List<TransferStartData>> Start()
    {
      List < TransferStartData> listData = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          listData = dbContext.USP_OR_TransferStart().ToList();
        }
      });
      return listData;
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
    public async static Task<int> Stop()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferStop();
        }
      });
      return response;
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
    public async static Task<int> StopZone(string zone)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferStopZone(zone);
        }
      });
      return response;
    }

    #endregion

    #region AddCountries
    /// <summary>
    /// Agrega las paises en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> AddCountries()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferAddCountries();
        }
      });
      return response;
    }
    #endregion

    #region AddCountriesHotel
    /// <summary>
    /// Agrega los paises del sistema Hotel en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> AddCountriesHotel()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferAddCountriesHotel();
        }
      });
      return response;
    }
    #endregion

    #region UpdateCountriesNames
    /// <summary>
    /// Actualiza las descripciones de los paises en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> UpdateCountriesNames()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateCountriesNames();
        }
      });
      return response;
    }
    #endregion

    #region UpdateCountriesHotelNames
    /// <summary>
    /// Actualiza las descripciones de los paises del sistema de Hotel en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> UpdateCountriesHotelNames()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateCountriesHotelNames();
        }
      });
      return response;
    }
    #endregion

    #region AddAgencies
    /// <summary>
    /// Agrega las agencias en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> AddAgencies()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferAddAgencies();
        }
      });
      return response;
    }
    #endregion

    #region AddAgenciesHotel
    /// <summary>
    /// Actualiza las descripciones de los paises del sistema de Hotel en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> AddAgenciesHotel()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferAddAgenciesHotel();
        }
      }); 
      return response;
    }
    #endregion

    #region UpdateAgenciesNames
    /// <summary>
    /// Actualiza las descripciones de las agencias en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> UpdateAgenciesNames()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateAgenciesNames();
        }
      });
      return response;
    }
    #endregion

    #region UpdateAgenciesHotelNames
    /// <summary>
    /// Actualiza las descripciones de las agencias del sistema de Hotel en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> UpdateAgenciesHotelNames()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateAgenciesHotelNames();
        }
      });
      return response;
    }
    #endregion

    #region AddRoomTypes
    /// <summary>
    /// Agrega los tipos de habitacion en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> AddRoomTypes()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferAddRoomTypes();
        }
      });
      return response;
    }
    #endregion

    #region UpdateRoomTypesNames
    /// <summary>
    /// Actualiza las descripciones de los tipos de habitacion en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> UpdateRoomTypesNames()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateRoomTypesNames();
        }
      });
      return response;
    }
    #endregion

    #region AddContracts
    /// <summary>
    /// Agrega los contratos en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> AddContracts()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferAddContracts();
        }
      });
      return response;
    }
    #endregion

    #region UpdateContractsNames
    /// <summary>
    /// Actualiza las descripciones de los tipos de habitacion en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> UpdateContractsNames()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateContractsNames();
        }
      });
      return response;
    }
    #endregion

    #region AddGroups
    /// <summary>
    /// Agrega los grupos en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> AddGroups()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferAddGroups();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGroupsNames
    /// <summary>
    /// Actualiza las descripciones de los grupos en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> UpdateGroupsNames()
    {
      int response = 0;
      
        await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            response = dbContext.USP_OR_TransferUpdateGroupsNames();
          }
        });
      
      return response;
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
    public async static Task<int> UpdateTransferCountries()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferCountries();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferAgencies()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferAgencies();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferLanguages()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferLanguages();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferMarkets()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferMarkets();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferUnavailableMotivesByGroups()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByGroups();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferUnavailableMotivesByAgency()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByAgency();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferUnavailableMotivesByCountry()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByCountry();
        }
      });
      return response;
    }
    #endregion

    #region UpdateTransferUnavailableMotivesByContract
    /// <summary>
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por contrato en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> UpdateTransferUnavailableMotivesByContract()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByContract();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferUnavailableMotivesBy1Night()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesBy1Night();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferUnavailableMotivesBy2Nights()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesBy2Nights();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferUnavailableMotivesByTransfer()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByTransfer();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferUnavailableMotivesByNewMember()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByNewMember();
        }
      });
      return response;
    }
    #endregion

    #region UpdateTransferUnavailableMotivesByPax
    /// <summary>
    /// Actualiza el motivo de indisponibilidad de reservaciones migradas por pax (35 - PAX)
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int> UpdateTransferUnavailableMotivesByPax()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferUnavailableMotivesByPax();
        }
      });
      return response;
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
    public async static Task<int> UpdateTransferAvailability()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateTransferAvailability();
        }
      });
      return response;
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
    public async static Task<int> UpdateGuestsRoomNumbers()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsRoomNumbers();
        }
      });
      return response;
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
    public async static Task<int> UpdateGuestsRoomTypes()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsRoomTypes();
        }
      });
      return response;
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
    public async static Task<int> UpdateGuestsCreditCards()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsCreditCards();
        }
      });
      return response;
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
    public async static Task<int> UpdateGuestsLastNames()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsLastNames();
        }
      });
      return response;
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
    public async static Task<int> UpdateGuestsFirstNames()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsFirstNames();
        }
      });
      return response;
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
    public async static Task<int>  UpdateGuestsCheckInDates()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsCheckInDates();
        }
      });
      return response;
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
    public async static Task<int>  UpdateGuestsCheckIns()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsCheckIns();
        }
      });
      return response;
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
    public async static Task<int>  UpdateGuestsCheckOutDates()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsCheckOutDates();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsEmails
    /// <summary>
    /// Actualiza los correos electronicos de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsEmails()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsEmails();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsCities
    /// <summary>
    /// Actualiza las ciudades de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsCities()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsCities();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsStates
    /// <summary>
    /// Actualiza los estados de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsStates()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsStates();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsCountries
    /// <summary>
    /// Actualiza los paises de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsCountries()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsCountries();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsCheckOutsEarly
    /// <summary>
    /// Verifica las salidas anticipadas de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsCheckOutsEarly()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsCheckOutsEarly();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsGuestTypes
    /// <summary>
    /// Actualiza los tipos de huesped de los huespedes en el proceso de transferencia (se usa el campo opcional 2)
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsGuestTypes()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsGuestTypes();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsContracts
    /// <summary>
    /// Actualiza los contratos de huespedes en el proceso de transferencia (se usa el campo opcional 1)
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsContracts()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsContracts();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsBirthDates
    /// <summary>
    /// Actualiza las fechas de nacimiento de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsBirthDates()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsBirthDates();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsAges
    /// <summary>
    /// Actualiza las edades de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsAges()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsAges();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsPax
    /// <summary>
    /// Actualiza el pax de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsPax()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsPax();
        } 
      });
      return response;
    }
    #endregion

    #region UpdateGuestsReservationTypes
    /// <summary>
    /// Actualiza el tipo de reservacion de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsReservationTypes()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsReservationTypes();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsIdProfileOpera
    /// <summary>
    /// Actualiza los id's de perfiles de Opera de huespedes en el proceso de transferencia
    /// Si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsIdProfileOpera()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsIdProfileOpera();
        }
      });
      return response;
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
    public async static Task<int>  UpdateGuestsAgencies()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsAgencies();
        }
      });
      return response;
    }
    #endregion

    #region UpdateGuestsMarkets
    /// <summary>
    /// Actualiza los mercados de huespedes en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<int>  UpdateGuestsMarkets()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsMarkets();
        }
      });
      return response;
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
    public async static Task<int>  UpdateGuestsAvailabilityUnavailableMotives()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsAvailabilityUnavailableMotives();
        }
      });
      return response;
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
    public async static Task<int>  UpdateGuestsUnavailableMotives1NightRevert()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsUnavailableMotives1NightRevert();
        }
      });
      return response;
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
    public async static Task<int>  UpdateGuestsAvailables(DateTime? dateFrom, DateTime? dateTo)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferUpdateGuestsAvailables(dateFrom, dateTo);
        }
      });
      return response;
    }
    #endregion

    #region AddGuests
    /// <summary>
    /// Agrega las reservaciones en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 14/04/2016  created
    /// </hystory>
    public async static Task<List<int>> AddGuests()
    {
      List < int > response = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferAddGuests().Where(x => x.HasValue).Select(x => (int)x).ToList();
        }
      });
      return response;
    }
    #endregion

    #region DeleteReservationsCancelled
    /// <summary>
    /// Elimina las reservaciones canceladas en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>
    public async static Task<int>  DeleteReservationsCancelled()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Settings.Default.USP_OR_TransferDeleteReservationsCancelled_Timeout;
          response =  dbContext.USP_OR_TransferDeleteReservationsCancelled();
        }
      });
      return response;
    }
    #endregion

    #region DeleteTransfer
    /// <summary>
    /// Elimina los registros de la tabla de transferencias.
    /// </summary>
    /// <hystory>
    /// [michan] 16/04/2016  created
    /// </hystory>
    public async static Task<int>  DeleteTransfer()
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferDeleteTransfer();
        }
      });
      return response;
    }
    #endregion

    #region GetTransfer
    /// <summary>
    /// Trae rodos los registros de la tabla de transferencias.
    /// </summary>
    /// <hystory>
    /// [michan] 16/04/2016  created
    /// </hystory>
    public async static Task<List<Transfer>> GetTransfer()
    {
      List<Transfer> response = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_TransferGetTransfer().ToList();
        }
      });
      return response;
    }
    #endregion

    #region AddReservation
    /// <summary>
    /// Agrega una nueva reservacion en la tabla de osTrasnfer
    /// </summary>
    /// <param name="transfer">Entidad transferencia</param>
    /// <returns>Retorna un entero si se realizo el guardado</returns>
    public async static Task<int>  AddReservation(Transfer transfer)
    {
      int response = 0;
      
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Transfers.Add(transfer);
          response = await dbContext.SaveChangesAsync(); //SaveChanges()
        }
      
      return response;
    }
    #endregion

    #region ExistReservation
    /// <summary>
    /// Valida si existe la reservación en la tabla de trasnferencias
    /// </summary>
    /// <param name="leadSource">valor para tls</param>
    /// <param name="tHReservID">valor para tHReservID</param>
    /// <returns></returns>
    public async static Task<bool> ExistReservation(string leadSource, string tHReservID)
    {
      bool status = true;
      await Task.Run(() => { 
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var result = dbContext.Transfers.SingleOrDefault(transfer => transfer.tls == leadSource && transfer.tHReservID == tHReservID);
          status = (result != null) ?true : false;
        }
      });
      return  status;
    }
    #endregion

    #region Transfer
    /// <summary>
    ///   Transfiere los integrantes de un equipo a otro
    /// </summary>
    /// <param name="strUserID">Clave de usuario</param>
    /// <param name="strloID">Clave de locación</param>
    /// <param name="strTeamID">Clave de equipo</param>
    /// <param name="lstPersonnelTransfer">Lista de integrantes</param>
    /// <param name="_enumTeamType">Tipo de equipo TeamPRs | TeamSalesmen</param>
    /// <history>
    ///   [vku] 21/Jul/2016 Created
    /// </history>
    /// <returns></returns>
    public async static Task<int> TransferTeamMembers(string strUserID, string strPlaceID, string strTeamID, List<Personnel> lstPersonnelTransfer, EnumTeamType _enumTeamType)
    {
      int nRes = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {

            try
            {
              if (lstPersonnelTransfer.Count > 0)
              {
                dbContext.Personnels.AsEnumerable().Where(pe => lstPersonnelTransfer.Any(pee => pee.peID == pe.peID)).ToList().ForEach(pe =>
                {
                  pe.peTeamType = EnumToListHelper.GetEnumDescription(_enumTeamType);
                  pe.pePlaceID = strPlaceID;
                  pe.peTeam = strTeamID;

                  DateTime dtmServerDate = BRHelpers.GetServerDateTime();

                  TeamLog teamLog = new TeamLog();
                  teamLog.tlDT = dtmServerDate;
                  teamLog.tlChangedBy = strUserID;
                  teamLog.tlpe = pe.peID;
                  teamLog.tlTeamType = pe.peTeamType;
                  teamLog.tlPlaceID = pe.pePlaceID;
                  teamLog.tlTeam = pe.peTeam;
                  dbContext.TeamsLogs.Add(teamLog);
                });
              }
              int nSave = dbContext.SaveChanges();
              transaction.Commit();
              return nSave;
            }
            catch
            {
              transaction.Rollback();
              return 0;
            }
          }
        }
      });
      return nRes;
    }
    #endregion
  }
}