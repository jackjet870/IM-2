using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public static class BRGuests
  {
    #region GetGuestsArrivals

    /// <summary>
    /// Obtiene las llegadas de huespedes
    /// </summary>
    /// <param name="date">Fecha </param>
    /// <param name="leadSource">LeadSource </param>
    /// <param name="markets">Mercado </param>
    /// <param name="available">Available </param>
    /// <param name="contacted">Contacted</param>
    /// <param name="invited">Invited</param>
    /// <param name="onGroup">OnGroup</param>
    public static async Task<List<GuestArrival>> GetGuestsArrivals(DateTime date, string leadSource, string markets, int available, int contacted, int invited, int onGroup)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetArrivals_Timeout;
          return
            dbContext.USP_OR_GetArrivals(date, leadSource, markets, available, contacted, invited, onGroup).ToList();
        }
      });
    }

    #endregion GetGuestsArrivals

    #region GetGuestsAvailables

    /// <summary>
    /// Obtiene los huespedes disponibles
    /// </summary>
    /// <param name="date">Fecha </param>
    /// <param name="leadSource">LeadSource </param>
    /// <param name="markets">Mercado </param>
    /// <param name="contacted">Contacted</param>
    /// <param name="invited">Invited</param>
    /// <param name="onGroup">OnGroup</param>
    public static async Task<List<GuestAvailable>> GetGuestsAvailables(DateTime date, string leadSource, string markets, int contacted, int invited, int onGroup)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetAvailables_Timeout;
          return dbContext.USP_OR_GetAvailables(date, leadSource, markets, contacted, invited, onGroup).ToList();
        }
      });
    }

    #endregion GetGuestsAvailables

    #region GetGuestsPremanifest

    /// <summary>
    /// Obtiene los huespedes premanifestados
    /// </summary>
    /// <param name="date">Fecha </param>
    /// <param name="leadSource">LeadSource </param>
    /// <param name="markets">Mercado </param>
    /// <param name="onGroup">OnGroup</param>
    public static async Task<List<GuestPremanifest>> GetGuestsPremanifest(DateTime date, string leadSource, string markets, int onGroup)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetPremanifest_Timeout;
          return dbContext.USP_OR_GetPremanifest(date, leadSource, markets, onGroup).ToList();
        }
      });
    }

    #endregion GetGuestsPremanifest

    #region GetPremanifestHost

    /// <summary>
    /// Función para obtener el manifestHost
    /// </summary>
    /// <param name="currentDate"></param>
    /// <param name="salesRoomID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 22/02/2016 Created
    /// [wtorres]   23/03/2016 Modified. Movido desde BRSalesRooms
    /// [vipacheco] 10/Oct/2016 Modified -> Se agrego asincronia
    /// </history>
    public static async Task<List<GuestPremanifestHost>> GetPremanifestHost(DateTime? currentDate, string salesRoomID)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_GetPremanifestHost(currentDate, salesRoomID).ToList();
        }
      });
    }

    #endregion GetPremanifestHost

    #region GetGuestsMailOuts

    /// <summary>
    /// Obtiene los Mail Outs disponibles de un Lead Source
    /// </summary>
    /// <param name="leadSourceID">Clave del Lead Source</param>
    /// <param name="guCheckInD">Fecha de llegada</param>
    /// <param name="guCheckOutD">Fecha de salida</param>
    /// <param name="guBookD">Fecha de booking</param>
    ///  <history>
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static List<GuestMailOut> GetGuestsMailOuts(string leadSourceID, DateTime guCheckInD, DateTime guCheckOutD, DateTime guBookD)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetGuestsMailOuts(leadSourceID, guCheckInD, guCheckOutD, guBookD).ToList();
      }
    }

    #endregion GetGuestsMailOuts

    #region GetGuestsPremanifestHost

    /// <summary>
    /// Obtiene los huespedes premanifestados en el modulo Host
    /// </summary>
    /// <param name="Date">Fecha</param>
    /// <param name="salesRoom">Sala de ventas</param>
    /// <history>
    /// [wtorres]  07/Mar/2016 Created
    /// </history>
    public static List<GuestPremanifestHost> GetGuestsPremanifestHost(DateTime Date, string salesRoom)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetPremanifestHost(Date, salesRoom).ToList();
      }
    }

    #endregion GetGuestsPremanifestHost    

    #region GetGuestStatusType

    /// <summary>
    /// Obtiene la lista de estados de los invitados
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <history>
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// [erosado] 04/08/2016 Modified. Se estandarizó el valor que retorna.
    /// </history>
    public static async Task<List<GuestStatusType>> GetGuestStatusType(int status)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          bool statusGuestStatus = Convert.ToBoolean(status);
          return dbContext.GuestsStatusTypes.Where(gs => gs.gsA == statusGuestStatus).ToList();
        }
      });
    }

    #endregion GetGuestStatusType

    #region GetGuest

    /// <summary>
    /// get a Guest
    /// </summary>
    /// <param name="guestId">Id del huesped</param>
    /// <param name="withAditional">Indica si se quiere los Guest Adicionales</param>
    /// <returns>Guest</returns>
    /// <history>
    /// [jorcanche] created 10/03/2016
    /// [erosado] 04/08/2016  Modified. Se agregó la bandera withAditional, sirve para obtener guest Adicionales.
    /// </history>
    public static async Task<Guest> GetGuest(int guestId, bool withAditional = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          if (withAditional)
          {
            return dbContext.Guests.Include("GuestsAdditional").SingleOrDefault(g => g.guID == guestId);
          }
          else
          {
            return (from gu in dbContext.Guests where gu.guID == guestId select gu).FirstOrDefault();
          }
        }
      });
    }

    #endregion GetGuest

    #region GetAdditionalGuest

    /// <summary>
    /// Obtiene los invitados adicionales de una invitacion
    /// </summary>
    /// <param name="guestId">Guest ID</param>
    /// <returns>Lista de invitados adicionales</returns>
    /// <history>
    /// [erosado] 04/08/2016  Created.
    /// [edgrodriguez] 11/08/2016 Modified. Se realizó la consulta, ya que obtener los guest adicionales
    /// por medio de Entity no funciona muy bien.
    /// </history>
    public static async Task<List<Guest>> GetAdditionalGuest(int guestId, bool withAditional = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          #region Query

          string strQuery = $"Select * from dbo.Guests where guID in( SELECT gaAdditional from dbo.GuestsAdditional where gagu='{guestId}');";

          #endregion Query

          var dQuery = dbContext.Database.SqlQuery<Guest>(strQuery);
          return dQuery.ToList();
        }
      });
    }

    #endregion GetAdditionalGuest

    #region GetGuestShort

    /// <summary>
    /// Obtiene registros especificos de un guest Short con un guestID ingresado.
    /// </summary>
    /// <param name="guestId"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 11/Abril/2016 Created
    /// [vipacheco] 19/Sep/2016 Modified -> Se agrego asincronia
    /// </history>
    public async static Task<GuestShort> GetGuestShort(int guestId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_GetGuestById(guestId).SingleOrDefault();
        }
      });
    }

    #endregion GetGuestShort

    #region SaveGuestMovement

    /// <summary>
    /// Guarda los moviemientos del Guest
    /// </summary>
    /// <param name="guestId"></param>
    /// <param name="guestMovementType"></param>
    /// <param name="changedBy"></param>
    /// <param name="computerName"></param>
    /// <param name="iPAddress"></param>
    /// <history>
    /// [jorcanche] created 15/03/2016
    /// </history>
    public static async Task<int> SaveGuestMovement(int? guestId, EnumGuestsMovementsType guestMovementType, string changedBy, string computerName, string iPAddress)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_SaveGuestMovement(guestId, EnumToListHelper.GetEnumDescription(guestMovementType), changedBy, computerName, iPAddress);
        }
      });
    }

    #endregion SaveGuestMovement

    #region GetGuestMovement

    /// <summary>
    /// Devuelve los movimientos del Guest
    /// </summary>
    /// <param name="guestId">Id del Guest</param>
    /// <history>
    /// [jorcanche] created 20/06/2016
    /// </history>
    public static async Task<List<GuestMovements>> GetGuestMovement(int guestId)
    {
      return await Task.Run(() =>
     {
       using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
       {
         return dbContext.USP_OR_GetGuestMovements(guestId).ToList();
       }
     });
    }

    #endregion GetGuestMovement

    #region GetGuests

    /// <summary>
    /// Devuelve uno o varios Guest dependiento de los filtros de busqueda
    /// </summary>
    /// <returns>Guests</returns>
    /// <history>
    /// [jorcanche] created 16/03/2016
    /// </history>
    public static async Task<List<GuestSearched>> GetGuests(DateTime dateFrom, DateTime dateTo, string leadSource, string name = "ALL", string roomNumber = "ALL", string reservation = "ALL", int guestId = 0)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetGuests_Timeout;
          return dbContext.USP_OR_GetGuests(dateFrom, dateTo, leadSource, name, roomNumber, reservation, guestId).ToList();
        }
      });
    }

    #endregion GetGuests

    #region GetGuestByPR

    /// <summary>
    /// Obtiene Guests por PR
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <param name="leadSources"></param>
    /// <param name="PR"></param>
    /// <param name="filtros"></param>
    /// <returns>List<GuestByPR></returns>
    /// <history>
    /// [erosado] 18/03/2016 Created.
    /// [erosado] 06/07/2016 Modified. Se agregó Async.
    /// [edgrodriguez]  26/09/2016 Modified. Se agrega el parámetro Program.
    /// </history>
    public static async Task<List<GuestByPR>> GetGuestsByPR(DateTime dateFrom, DateTime dateTo, string leadSources, string program, string PR, List<bool> filtros)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetGuestsByPR;
          return dbContext.USP_OR_GetGuestsByPR(dateFrom, dateTo, leadSources, PR, filtros[0], filtros[1], filtros[2], filtros[3], filtros[4], filtros[5], filtros[6], program).ToList();
        }
      });
    }

    #endregion GetGuestByPR

    #region GetGuestsGroupIntegrants

    /// <summary>
    /// Obtiene el listado de Huespedes pertenecientes a un grupo especificado
    /// </summary>
    /// <param name="guestsGroup">Grupo al que pertenecen los Huespedes</param>
    /// <returns>Listado de Huespedes que integran el grupo</returns>
    /// <history>[ECANUL] 29-03-2016 Created</history>
    public static List<Guest> GetGuestsGroupsIntegrants(GuestsGroup guestsGroup)
    {
      List<Guest> lstGuests;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        lstGuests = (from gu in dbContext.Guests
                     from ggi in gu.GuestsGroups
                     join gg in dbContext.GuestsGroups
                     on ggi.gxID equals gg.gxID
                     where gg.gxID == guestsGroup.gxID
                     select gu).ToList();
      }
      return lstGuests;
    }

    #endregion GetGuestsGroupIntegrants

    #region GetSearchGuestByLS

    /// <summary>
    /// Trae los huespedes segun los parametros
    /// </summary>
    /// <param name="leadsource"> Clave del LeadSource</param>
    /// <param name="salesRoom"> Clave de la sala de venta</param>
    /// <param name="name"> Nombre del Guest </param>
    /// <param name="room"></param>
    /// <param name="reservation"> Numero de reservacion</param>
    /// <param name="guid">Clava del guest</param>
    /// <param name="from">Fecha inicial de busqueda</param>
    /// <param name="to">fecha final de busqueda</param>
    /// <param name="program"> Enumerado que indica el program de origen </param>
    /// <param name="PR"></param>
    /// <returns>Listado de Guest</returns>
    /// <history>
    /// [ECANUL] 01-04-2016 Created
    /// [jorcanche] 04/05/2016 Simplificado
    /// [vipacheco] 08/Agosto/2016 Modified -> Se le agrego timeout a la consulta.
    /// [vipacheco] 11/Agosto/2016 Modified -> Se agrego asincronia
    /// [vipacheco] 06/Sep/2016 Modified -> Se agrego dentro del where la busqueda por el rango de fechas para disminuir el tiempo de consulta.
    /// </history>
    public async static Task<List<Guest>> GetSearchGuestByLS(string leadsource, string salesRoom, string name,
      string room, string reservation, int guid, DateTime dateFrom, DateTime dateTo, EnumProgram program, string PR)
    {
      return await Task.Run(() =>
      {
        var pro = EnumToListHelper.GetEnumDescription(program);
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          // Agregamos el timeout de la consulta
          dbContext.Database.CommandTimeout = 180;         
          var query = from gu in dbContext.Guests
                      join ls in dbContext.LeadSources on gu.guls equals ls.lsID
                      where (ls.lspg == (program == EnumProgram.Outhouse ? ls.lspg : pro))
                      select gu; 
                    
          //Busqueda por clave de huesped
          if (guid != 0)
          {
            query = query.Where(gu => gu.guID == guid);
          }
          else
          {
            //Busqueda por nombre y aprellido
            if (!string.IsNullOrWhiteSpace(name))
            {
              query = query.Where(gu => gu.guLastName1.Contains(name) ||
                                        gu.guFirstName1.Contains(name) ||
                                        gu.guLastname2.Contains(name) ||
                                        gu.guLastname2.Contains(name));
            }
            //Busqueda por Lead Source
            if (!string.IsNullOrEmpty(leadsource))
            {
              query = query.Where(gu => gu.guls == leadsource);
            }
            //Busqueda por sala
            if (!string.IsNullOrEmpty(salesRoom))
            {
              query = query.Where(gu => gu.gusr == salesRoom);
            }
            //Busqueda por numero de habitacion
            if (!string.IsNullOrEmpty(room))
            {
              query = query.Where(gu => gu.guRoomNum.Contains(room));
            }
            //Busqueda por folio de reservacion
            if (!string.IsNullOrEmpty(reservation))
            {
              query = query.Where(gu => gu.guHReservID == reservation);
            }
            if (!string.IsNullOrEmpty(PR))
            {
              query = query.Where(gu => gu.guPRInvit1 == PR);
            }
            // Busqueda por rango de fechas 
            query = query.Where(gu => ((program == EnumProgram.Outhouse ? gu.guBookD.Value : gu.guCheckInD) >= dateFrom) &&
                                      ((program == EnumProgram.Outhouse ? gu.guBookD.Value : gu.guCheckInD) <= dateTo));

          }
          //Si se utiliza en el modulo Outhouse quiere decir que es una busqueda de un huesped con
          //invitacion para transferir y de ser así se utiliza esta condicion si se utiliza en Inhouse no se
          //utiliza esta condicion
          if (program == EnumProgram.Outhouse)
          {
            query = query.Where(gu => gu.guInvit == true && gu.guShow == false);
          }
          return query.OrderBy(gu => gu.gusr).ThenBy(gu => gu.guBookD).ThenBy(gu => gu.guLastName1).ToList();
        }
      });
    }

    #endregion GetSearchGuestByLS

    #region SaveChangedOfGuest

    /// <summary>
    /// Guarda los cambios en un guest y de igual forma el Log de este.
    /// </summary>
    /// <param name="guest">El Guest que se desea modificar</param>
    /// <param name="lsHoursDif">Hours dif para guardar el Log</param>
    /// <param name="changedBy">Usuario quien modifico el Guest y Log</param>
    /// <history>
    /// [jorcanche] 07/04/2016 Created
    /// </history>
    public static async Task<int> SaveChangedOfGuest(Guest guest, short lsHoursDif, string changedBy)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              //Guardamos los cambios en el Guest
              dbContext.Entry(guest).State = EntityState.Modified;
              var respuesta = dbContext.SaveChanges();

              //Guardamos el Log del guest
              dbContext.USP_OR_SaveGuestLog(guest.guID, lsHoursDif, changedBy);

              //Confirmammos la transaccion
              transaction.Commit();
              return respuesta;
            }
            catch
            {
              transaction.Rollback();
              throw;
            }
          }
        }
      });
    }

    #endregion SaveChangedOfGuest

    #region SaveGuestContact

    /// <summary>
    /// Guarda la Contactacion del guest, el Log y de igual forma los moviemientos de este (SaveGuestMovement).
    /// </summary>
    /// <param name="guest">El Guest que se desea guardar</param>
    /// <param name="lsHoursDif">Hours dif para guardar el Log</param>
    /// <param name="changedBy">Usuario quin modifico el registro</param>
    /// <param name="guestMovementType">Tipo de Moviemiento en este caso es "Contact"</param>
    /// <param name="computerName">Nombre de la computadora en donde se esta modificando el Guest</param>
    /// <param name="iPAddress">Dirrecion IP de la computadora en donde se esta modificando el Guest</param>
    /// <history>
    /// [jorcanche] 07/04/2016 Created
    /// </history>
    public static async Task<int> SaveGuestContact(Guest guest, short lsHoursDif, string changedBy, EnumGuestsMovementsType guestMovementType, string computerName, string iPAddress)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              //guardamos la informacion de contacto
              dbContext.Entry(guest).State = EntityState.Modified;
              var respuesta = dbContext.SaveChanges();

              //guardamos el Log del huesped
              dbContext.USP_OR_SaveGuestLog(guest.guID, lsHoursDif, changedBy);

              //guardamos el movimiento de contactacion del huesped
              dbContext.USP_OR_SaveGuestMovement(guest.guID, EnumToListHelper.GetEnumDescription(guestMovementType), changedBy, computerName, iPAddress);


              transaction.Commit();
              return respuesta;
            }
            catch
            {
              transaction.Rollback();
              throw;
            }
          }
        }
      });
    }

    #endregion SaveGuestContact

    #region ChangedByExist

    /// <summary>
    /// Valida si el usuario que realizará cambios existe
    /// </summary>
    /// <param name="changedBy">usuario</param>
    /// <param name="password">password</param>
    /// <param name="placeType">lugar</param>
    /// <param name="placesID">Id del lugar</param>
    /// <param name="userType">Tipo usuario</param>
    /// <param name="pr">Si se saliva el pr</param>
    /// <returns></returns>
    public static ValidationData ChangedByExist(string changedBy, string password, string placesID, string placeType = "LS", string userType = "Changed By", string pr = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_ValidateChangedBy(changedBy, password, placeType, placesID, userType, pr).SingleOrDefault();
      }
    }

    #endregion ChangedByExist

    #region UpdateFieldguMealTicket

    /// <summary>
    /// Actualiza el campo guMealTicket cuando sea requerido
    /// </summary>
    /// <param name="chkValue"></param>
    /// <param name="guestID"></param>
    /// <history>
    /// [vipacheco] 01/04/2016 Created
    /// </history>
    public static void UpdateFieldguMealTicket(bool chkValue, int guestID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        Guest _guest = dbContext.Guests.Where(x => x.guID == guestID).FirstOrDefault<Guest>();

        _guest.guMealTicket = chkValue;

        dbContext.Entry(_guest).State = System.Data.Entity.EntityState.Modified;
        dbContext.SaveChanges();
      }
    }

    #endregion UpdateFieldguMealTicket

    #region GetGuestPremanifestOuthouse

    /// <summary>
    /// Devuelve una Nota o varias dependiendo de cuantas tanga el Guest
    /// </summary>
    /// <param name="bookinvit"></param>
    /// <param name="date"></param>
    /// <param name="leadSource"></param>
    /// <returns>Un listado de PRNotes</returns>
    /// <history>
    /// [jorcanche] created 22/03/2016
    /// [erosado] 19/09/2016  Modified. Se agregó TimeOut
    /// </history>
    public static async Task<List<GuestPremanifestOuthouse>> GetGuestPremanifestOuthouse(bool bookinvit, DateTime date, string leadSource)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.GetGuestPremanifestOuthouse;
          try
          {
            return (from g in dbContext.Guests
                    join co in dbContext.Countries on g.guco equals co.coID
                    join ag in dbContext.Agencies on g.guag equals ag.agID
                    where (bookinvit ? g.guBookD : g.guInvitD) == date
                    where g.guls == leadSource
                    orderby g.guBookT, g.guLastName1
                    select new GuestPremanifestOuthouse
                    {
                      guStatus = g.guStatus,
                      guID = g.guID,
                      guCheckIn = g.guCheckIn,
                      guRoomNum = g.guRoomNum,
                      guLastName1 = g.guLastName1,
                      guFirstName1 = g.guFirstName1,
                      guCheckInD = g.guCheckInD,
                      guCheckOutD = g.guCheckOutD,
                      guco = g.guco,
                      coN = co.coN,
                      guag = g.guag,
                      agN = ag.agN,
                      guAvail = g.guAvail,
                      guInfo = g.guInfo,
                      guPRInfo = g.guPRInfo,
                      guInfoD = g.guInfoD,
                      guInvit = g.guInvit,
                      guInvitD = g.guInvitD,
                      guBookD = g.guBookD,
                      guBookT = g.guBookT,
                      guPRInvit1 = g.guPRInvit1,
                      guMembershipNum = g.guMembershipNum,
                      guBookCanc = g.guBookCanc,
                      guShow = g.guShow,
                      guSale = g.guSale,
                      guComments = g.guComments,
                      guPax = g.guPax
                    }).ToList();
          }
          catch (Exception)
          {           
            throw;
          }
        }
      });
    }
        
    #endregion GetGuestPremanifestOuthouse

    #region DeleteGuest

    /// <summary>
    /// Elimina un Guest
    /// </summary>
    /// <history>
    /// [jorcanche] 04/05/2016 created
    /// </history>
    /// <param name="guID">Id del Guest</param>
    public static async Task<int> DeleteGuest(int guID)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_DeleteGuest(guID);
        }
      });
    }

    #endregion DeleteGuest

    #region GetSearchGuestGeneral

    /// <summary>
    /// Busca los huespedes que cumplan los criterios de busqueda
    /// </summary>
    /// <param name="hostDateSelected">Fecha seleccionado en la ventana principal del modulo host</param>
    /// <param name="dtpStart">Fecha inicial seleccionada del formulario Search</param>
    /// <param name="dtpEnd">Fecha final seleccionada del formulario Search</param>
    /// <param name="guestID">Clave del huesped</param>
    /// <param name="guestName">Nombre del huesped</param>
    /// <param name="leadSource">Clave del Lead Source</param>
    /// <param name="salesRoom">Clave de la sala de venta</param>
    /// <param name="roomNum">Numero de habitacion</param>
    /// <param name="reservation">Numero de reservacion </param>
    /// <param name="PR">Clave del PR</param>
    /// <param name="module">Origen de invocacion del formulario Search</param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// [vipacheco] 09/Agosto/2016 Modified -> Se agrego el estandar del return, se agregaron nuevos parametros al metodo, se agrego un switch para validar tipos de busqueda, se agrego timeout.
    /// </history>
    public static async Task<List<Guest>> GetSearchGuestGeneral(DateTime hostDateSelected, DateTime dtpStart, DateTime dtpEnd, int guestID = 0, string guestName = "", string leadSource = "", string salesRoom = "", string roomNum = "",
                                                                string reservation = "", string PR = "", EnumSearchHostType module = EnumSearchHostType.General)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          // Agregamos el timeout de la consulta
          dbContext.Database.CommandTimeout = 180;

          IQueryable<Guest> query = Enumerable.Empty<Guest>().AsQueryable();

          if (module == EnumSearchHostType.General)
          {
            // Busqueda por Guest ID
            if (guestID > 0)
            {
              return dbContext.Guests.Where(x => x.guID == guestID).OrderBy(x => x.gusr).ToList();
            }
          }
          else
          {
            // Busqueda por Guest ID
            if (guestID > 0)
            {
              query = dbContext.Guests.Where(x => x.guID == guestID);
            }
          }
          // Busqueda por nombre o apellido
          if (guestName != "")
          {
            query = dbContext.Guests.Where(x => x.guLastName1.Contains(guestName) || x.guFirstName1.Contains(guestName) || x.guLastname2.Contains(guestName) || x.guFirstName2.Contains(guestName));
          }
          // Busqueda por Lead Source
          if (leadSource != "")
          {
            query = query.Any() ? query.Where(x => x.guls == leadSource) : dbContext.Guests.Where(x => x.guls == leadSource);
          }
          // Busqueda por sala
          if (salesRoom != "")
          {
            query = query.Any() ? query.Where(x => x.gusr == salesRoom) : dbContext.Guests.Where(x => x.gusr == salesRoom);
          }
          // Busqueda por numero de habitacion
          if (roomNum != "")
          {
            query = query.Any() ? query.Where(x => x.guRoomNum == roomNum) : dbContext.Guests.Where(x => x.guRoomNum == roomNum);
          }
          // Busqueda por folio de reservacion
          if (reservation != "")
          {
            query = query.Any() ? query.Where(x => x.guHReservID == reservation) : dbContext.Guests.Where(x => x.guHReservID == reservation);
          }
          //Busqueda por PR
          if (PR != "")
          {
            query = query.Any() ? query.Where(x => x.guPRInvit1 == PR) : dbContext.Guests.Where(x => x.guPRInvit1 == PR);
          }
          // Verificamos el tipo de busqueda a realizar
          switch (module)
          {
            // Si es de tipo transfer
            case EnumSearchHostType.Transfer:
              query = query.Any() ? query.Where(x => x.guBookD >= dtpStart && x.guBookD <= dtpEnd && x.guInvit == true && x.guShow == false) : dbContext.Guests.Where(x => x.guBookD >= dtpStart && x.guBookD <= dtpEnd && x.guInvit == true && x.guShow == false);
              break;
            // Si la busqueda es general 
            case EnumSearchHostType.General:
              query = query.Any() ? query.Where(x => x.guBookD >= dtpStart && x.guBookD <= dtpEnd && x.guInvit == true) : dbContext.Guests.Where(x => x.guBookD >= dtpStart && x.guBookD <= dtpEnd && x.guInvit == true);
              break;
            case EnumSearchHostType.Invit:
              query = query.Any() ? query.Where(x => x.guCheckOutD > hostDateSelected && x.guCheckIn == true) : dbContext.Guests.Where(x => x.guCheckOutD > hostDateSelected && x.guCheckIn == true);
              break;
          }
          //}
          return query.OrderBy(x => x.gusr).ToList();
        }
      });
    }

    #endregion GetSearchGuestGeneral

    #region GetGuestRegistration

    /// <summary>
    /// Retorna la informacion para el reporte Guest Registration
    /// </summary>
    /// <param name="GuestID"></param>
    /// <returns></returns>
    /// <history>
    /// [edgrodriguez] 26/Jul/2016 Created
    /// </history>
    public static async Task<List<IEnumerable>> GetGuestRegistration(int GuestID)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var lst = dbContext.USP_OR_RptGuestRegistration(GuestID)
          .MultipleResults()
          .With<RptGuestRegistration>()
          .With<RptGuestRegistration_Guests>()
          .With<RptGuestRegistration_Deposits>()
          .With<RptGuestRegistration_Gifts>()
          .With<RptGuestRegistration_Salesmen>()
          .With<RptGuestRegistration_CreditCards>()
          .With<RptGuestRegistration_Comments>()
          .GetValues();
          return lst.Any() ? lst : new List<IEnumerable>();
        }
      });
    }

    #endregion GetGuestRegistration

    #region GetAdditionalGuest

    /// <summary>
    /// Obtiene el huesped principal.
    /// </summary>
    /// <returns>  Huesped principal.</returns>
    /// <history>
    /// [edgrodriguez] 15/08/2016  Created.
    /// </history>
    public static async Task<Guest> GetMainGuest(int guestAdditionalId, int mainGuestId = 0)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          #region Query

          string strQuery = $"Select gu.* from GuestsAdditional guad inner join Guests gu on guad.gagu=gu.guID where guad.gaAdditional ='{guestAdditionalId}' and gu.guRef is NULL";
          if (mainGuestId > 0)
          {
            strQuery += $" and gagu <> {mainGuestId}";
          }

          #endregion Query

          var dQuery = dbContext.Database.SqlQuery<Guest>(strQuery);
          return dQuery.FirstOrDefault();
        }
      });
    }

    #endregion GetAdditionalGuest

    #region SaveGuestShow

    /// <summary>
    /// Proceso de guardado del Guest Show.
    /// </summary>
    /// <returns>  Huesped principal.</returns>
    /// <history>
    /// [edgrodriguez] 18/08/2016  Created.
    /// [edgrodriguez] 10/10/2016 Modified. Se agrega una validacion al proceso de Guest Additional
    /// </history>
    public static async Task<int> SaveGuestShow(GuestShow guestShow, UserData user, string changedBy, string machineName, string ipAddress)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          DateTime dtServerNow = BRHelpers.GetServerDateTime();
          //Establecemos el deposito
          SetDeposits(guestShow.BookingDepositList.ToList(), guestShow.Guest);

          // definimos al huesped interval
          guestShow.Guest.guInterval = true;
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              int result = 0;
              //Guardamos los cambios en el Guest
              dbContext.Entry(guestShow.Guest).State = EntityState.Modified;
              result = dbContext.SaveChanges();

              //Guardamos el Log del guest
              dbContext.USP_OR_SaveGuestLog(guestShow.Guest.guID, user.SalesRoom.srHoursDif, changedBy);

              #region Proceso Invitation Gifts
              //Invitation Gift a Eliminar
              #region Eliminar
              List<InvitationGift> lstGiftsDel = guestShow.CloneInvitationGiftList.Where(ig =>
                        !guestShow.InvitationGiftList.Any(igg => ig.iggu == igg.iggu && ig.iggi == igg.iggi && ig.igct == igg.igct)).ToList();

              lstGiftsDel.ForEach(ig => dbContext.Entry(ig).State = EntityState.Deleted);
              #endregion

              //Invitation Gift a Agregar
              #region Agregar
              List<InvitationGift> lstGiftsAdd = guestShow.InvitationGiftList.Where(ig =>
                        !guestShow.CloneInvitationGiftList.Any(igg => ig.iggu == igg.iggu && ig.iggi == igg.iggi && ig.igct == igg.igct)).ToList();

              lstGiftsAdd.ForEach(ig =>
              {
                ig.iggu = guestShow.Guest.guID;
                dbContext.Entry(ig).State = EntityState.Added;
              });
              #endregion

              //Invitation Gift a Actualizar
              //TODO: falta validar si cambió algun campo
              #region Actualizar
              List<InvitationGift> lstGiftsUpd = guestShow.InvitationGiftList.Where(ig =>
                        guestShow.CloneInvitationGiftList.Any(igg => ig.iggu == igg.iggu && ig.iggi == igg.iggi && ig.igct == igg.igct)).ToList();

              lstGiftsUpd.ForEach(ig => dbContext.Entry(ig).State = EntityState.Modified);
              #endregion
              #endregion

              #region Proceso Booking Deposits
              //Agregar
              #region Add
              List<BookingDeposit> lstDepositsAdd = guestShow.BookingDepositList.Where(bd => !guestShow.CloneBookingDepositList.Any(bdd => bdd.bdID == bd.bdID)).ToList();
              lstDepositsAdd.ForEach(bd =>
              {
                bd.bdgu = guestShow.Guest.guID;
                bd.bdD = dtServerNow;
                if (bd.bdFolioCXC != null)
                {
                  bd.bdUserCXC = user.User.peID;
                  bd.bdEntryDCXC = dtServerNow;
                }
                bd.bdds = (string.IsNullOrWhiteSpace(bd.bdds)) ? null : bd.bdds;

                dbContext.Entry(bd).State = EntityState.Added;
              });
              #endregion
              //Update
              #region Update
              List<BookingDeposit> lstDepositsUpd = guestShow.BookingDepositList.Where(bd => guestShow.CloneBookingDepositList.Any(bdd => bdd.bdID == bd.bdID)).ToList();
              lstDepositsUpd.ForEach(bd =>
              {
                bd.bdD = dtServerNow;
                if (bd.bdFolioCXC != null)
                {
                  bd.bdUserCXC = user.User.peID;
                  bd.bdEntryDCXC = dtServerNow;
                }
                else
                {
                  bd.bdUserCXC = null;
                  bd.bdEntryDCXC = null;
                }
                bd.bdds = (string.IsNullOrWhiteSpace(bd.bdds)) ? null : bd.bdds;

                dbContext.Entry(bd).State = EntityState.Modified;
              });
              #endregion

              //Eliminar
              #region Delete
              List<BookingDeposit> lstDepositsDel = guestShow.CloneBookingDepositList.Where(bd => !guestShow.BookingDepositList.Any(bdd => bdd.bdID == bd.bdID)).ToList();
              lstDepositsDel.ForEach(bd => dbContext.Entry(bd).State = EntityState.Deleted);
              #endregion
              #endregion

              #region Proceso Credit Cards
              var lstGuestCreditCards = dbContext.GuestsCreditCards.Where(cc => cc.gdgu == guestShow.Guest.guID);

              //Si se encontro alguno los eliminamos 
              if (lstGuestCreditCards.Any())
              {
                dbContext.GuestsCreditCards.RemoveRange(lstGuestCreditCards);
              }

              //Si se agregaron nuevos GuestCreditCards los guardamos.
              guestShow.GuestCreditCardList.ToList().ForEach(gcc =>
                dbContext.Entry(gcc).State = EntityState.Added
              );

              //Creamos la variable en donde se van a concatenar 
              //Recorremos el Listado y concatenamos con la condicion de que Quantity sea mayor que 1
              string strguCCType = string.Join(", ", guestShow.GuestCreditCardList.
                Select(x => (x.gdQuantity > 1 ? x.gdQuantity + " " : string.Empty) + x.gdcc));

              //Escojemos los primeros 30 caracteres porque es el límite del campo 
              guestShow.Guest.guCCType = strguCCType.Length > 30 ? strguCCType.Substring(0, 30) : strguCCType;
              #endregion

              #region Proceso GuestsStatus
              //Eliminamos los regsitros
              #region Delete
              var lstGuestStatusDel = dbContext.GuestsStatus.Where(gs => gs.gtgu == guestShow.Guest.guID);
              dbContext.GuestsStatus.RemoveRange(lstGuestStatusDel);
              #endregion
              #region Add
              if (guestShow.Guest.guStatus != null)
              {
                GuestStatus guestStatus = new GuestStatus();
                guestStatus.gtQuantity = 1;
                guestStatus.gtgu = guestShow.Guest.guID;
                guestStatus.gtgs = guestShow.Guest.guGStatus;
                dbContext.Entry(guestStatus).State = EntityState.Added;
              }
              #endregion
              #endregion

              #region Proceso Guest Additional
              var additionalGuests = guestShow.AdditionalGuestList.Cast<Guest>().ToList();
              additionalGuests.RemoveAll(c => c.guID == 0);
              if (additionalGuests.Any())
              {
                dbContext.USP_IM_SaveGuestAdditional(guestShow.Guest.guID, string.Join(",", additionalGuests.Select(c => c.guID).ToList()));
              }
              #endregion

              #region Proceso Guest Movements
              dbContext.USP_OR_SaveGuestMovement(guestShow.Guest.guID, EnumToListHelper.GetEnumDescription(EnumGuestsMovementsType.Show), changedBy, machineName, ipAddress);
              result += dbContext.SaveChanges();

              #endregion

              //Confirmammos la transaccion
              transaction.Commit();
              return result;
            }
            catch
            {
              transaction.Rollback();
              throw;
            }
          }
        }
      });
    }

    #endregion GetAdditionalGuest

    #region SaveGuestInvitation
    /// <summary>
    /// Guarda un GuestInvitation
    /// </summary>
    /// <param name="guestInvitation">Objeto con todos los datos del guest a guardar</param>
    /// <param name="enumProgram">program con el que se está guardando</param>
    /// <param name="enumModule">Desde dónde se está abriendo</param>
    /// <param name="user">Los datos del usuario que está guardando los datos</param>
    /// <param name="enumMode">Edit. Actualiza | Add. Agrega un nuevo guest</param>
    /// <param name="computerName">Nombre de la computadora desde donde se está guardando</param>
    /// <param name="iPAddress">Ip de la maquina desde donde se está guardando</param>
    /// <param name="guestMovementType">Tipo de movimiento del guest</param>
    /// <param name="hoursDiff"></param>
    /// <returns>0. No se guardó | >0. Los datos se guardaron correctamente</returns>
    /// <history>
    /// [emoguel]      18/08/2016 created
    /// [emoguel]      23/09/2016 Modified. Se cambio la validacion de GuestStatus
    /// [edgrodriguez] 10/10/2016 Modified. Se agrega una validacion al proceso de Guest Additional
    /// </history>
    public async static Task<int> SaveGuestInvitation(GuestInvitation guestInvitation, EnumProgram enumProgram, EnumModule enumModule, UserData user, EnumMode enumMode,
      string computerName, string iPAddress, EnumGuestsMovementsType guestMovementType, short hoursDiff)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          DateTime dtServerNow = BRHelpers.GetServerDateTime();
          bool blnInvited = false;
          //Establecemos el deposito
          SetDeposits(guestInvitation.BookingDepositList.ToList(), guestInvitation.Guest);

          //Asignamos valor default guCreationDate
          guestInvitation.Guest.guCreationD = dtServerNow;

          //Con Check In
          guestInvitation.Guest.guCheckIn = true;

          //Disponible
          if (string.IsNullOrWhiteSpace(guestInvitation.Guest.guPRAvail))
          {
            guestInvitation.Guest.guAvail = true;
          }
          //Si esta vacio le asignamos el guls
          if (string.IsNullOrWhiteSpace(guestInvitation.Guest.gulsOriginal))
          {
            guestInvitation.Guest.gulsOriginal = guestInvitation.Guest.guls;
          }
          //Si esta null o si tiene el valor Minimo
          if (guestInvitation.Guest.guCheckOutHotelD == null || guestInvitation.Guest.guCheckOutHotelD == DateTime.MinValue)
          {
            guestInvitation.Guest.guCheckOutHotelD = guestInvitation.Guest.guCheckOutD;
          }

          //Si No tiene Show el Numero de Shows es igual al Numero de habitaciones
          if (!guestInvitation.Guest.guShow)
          {
            guestInvitation.Guest.guShowsQty =(byte)guestInvitation.Guest.guRoomsQty;
          }

          #region Seguimiento
          if (enumProgram == EnumProgram.Inhouse)
          {
            //Si estaba contactado y no invitado
            if (guestInvitation.Guest.guInfo && !guestInvitation.Guest.guInvit)
            {
              //Con seguimiento
              guestInvitation.Guest.guFollow = true;
              //PR de seguimiento
              if (string.IsNullOrWhiteSpace(guestInvitation.Guest.guPRFollow))
              {
                guestInvitation.Guest.guPRFollow = guestInvitation.Guest.guPRInvit1;
              }

              //Fecha de seguimiento
              if (guestInvitation.Guest.guFollowD == null)
              {
                guestInvitation.Guest.guFollowD = guestInvitation.Guest.guInvitD;
              }
            }
          }
          #endregion

          #region Contactacion
          //contactado
          guestInvitation.Guest.guInfo = true;

          //PR de Contacto
          if (string.IsNullOrWhiteSpace(guestInvitation.Guest.guPRInfo))
          {
            guestInvitation.Guest.guPRInfo = guestInvitation.Guest.guPRInvit1;
          }

          //Fecha de contacto
          if (guestInvitation.Guest.guInfoD == null)
          {
            guestInvitation.Guest.guInfoD = guestInvitation.Guest.guInvitD;
          }

          //Locacion de contacto
          if (string.IsNullOrWhiteSpace(guestInvitation.Guest.guloInfo))
          {
            if (enumModule != EnumModule.Host)
            {
              guestInvitation.Guest.guloInfo = user.Location.loID;
            }
            else
            {
              guestInvitation.Guest.guloInfo = guestInvitation.Guest.guloInvit;
            }
          }
          #endregion

          #region Invitation
          //Invitado
          blnInvited = guestInvitation.Guest.guInvit;
          guestInvitation.Guest.guInvit = true;

          //invitation no cancelada
          guestInvitation.Guest.guBookCanc = false;

          //Captain de PR
          if (enumModule == EnumModule.Host || enumModule == EnumModule.OutHouse)
          {
            guestInvitation.Guest.guPRCaptain1 = dbContext.Personnels.Where(pe => pe.peID == guestInvitation.Guest.guPRInvit1).FirstOrDefault().peCaptain;
          }
          else if (!string.IsNullOrWhiteSpace(guestInvitation.Guest.guloInvit) && guestInvitation.Guest.guloInvit != guestInvitation.CloneGuest.guloInvit)
          {
            guestInvitation.Guest.guPRCaptain1 = dbContext.Locations.Where(lo => lo.loID == guestInvitation.Guest.guloInvit).FirstOrDefault().loPRCaptain;
          }

          //Fecha del primer booking
          if (enumMode == EnumMode.Add)
          {
            guestInvitation.Guest.guFirstBookD = guestInvitation.Guest.guBookD;
          }

          //Definimos si el huesped es interval
          guestInvitation.Guest.guInterval = true;

          #endregion

          #region Transaccion
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              int nSave = 0;
              
              //Guest
              if(guestInvitation.Guest.guID>0)
              {
                var guestSave = dbContext.Guests.Where(gu => gu.guID == guestInvitation.Guest.guID).FirstOrDefault();
                if (guestSave != null)
                {
                  ObjectHelper.CopyProperties(guestSave, guestInvitation.Guest);
                  guestInvitation.Guest = guestSave;
                }
                else
                {
                  dbContext.Guests.Add(guestInvitation.Guest);
                }
              }
              else
              {
                dbContext.Guests.Add(guestInvitation.Guest);
              }              
              nSave = dbContext.SaveChanges();//Para que se le agregué el Id al guest

              //Recargamos el codigo contable
              //TODO:Se comento esta linea por que actualmente se esta desarrollando el codigo contable
              //dbContext.USP_OR_SetAccountingCode(guestInvitation.Guest.guID, "MK");

              //Guardamos el historico del huesped
              dbContext.USP_OR_SaveGuestLog(guestInvitation.Guest.guID, hoursDiff, user.User.peID);

              #region Gifts
              //Invitation Gift a Eliminar
              #region Eliminar
              List<InvitationGift> lstGiftsDel = guestInvitation.CloneInvitationGiftList.Where(ig =>
                        !guestInvitation.InvitationGiftList.Any(igg => ig.iggu == igg.iggu && ig.iggi == igg.iggi && ig.igct == igg.igct)).ToList();

              lstGiftsDel.ForEach(ig => dbContext.Entry(ig).State = EntityState.Deleted);
              #endregion

              //Invitation Gift a Agregar
              #region Agregar
              List<InvitationGift> lstGiftsAdd = guestInvitation.InvitationGiftList.Where(ig =>
                        !guestInvitation.CloneInvitationGiftList.Any(igg => ig.iggu == igg.iggu && ig.iggi == igg.iggi && ig.igct == igg.igct)).ToList();

              lstGiftsAdd.ForEach(ig =>
              {
                ig.iggu = guestInvitation.Guest.guID;
                dbContext.Entry(ig).State = EntityState.Added;
              });
              #endregion

              //Invitation Gift a Actualizar
              //TODO: falta validar si cambió algun campo
              #region Actualizar
              List<InvitationGift> lstGiftsUpd = guestInvitation.InvitationGiftList.Where(ig =>
                        guestInvitation.CloneInvitationGiftList.Any(igg => ig.iggu == igg.iggu && ig.iggi == igg.iggi && ig.igct == igg.igct)).ToList();

              lstGiftsUpd.ForEach(ig => dbContext.Entry(ig).State = EntityState.Modified);
              #endregion




              #endregion

              #region Deposits              
              //Agregar
              #region Add
              List<BookingDeposit> lstDepositsAdd = guestInvitation.BookingDepositList.Where(bd => !guestInvitation.CloneBookingDepositList.Any(bdd => bdd.bdID == bd.bdID)).ToList();
              lstDepositsAdd.ForEach(bd =>
              {
                bd.bdgu = guestInvitation.Guest.guID;
                bd.bdD = dtServerNow;
                if (bd.bdFolioCXC != null)
                {
                  bd.bdUserCXC = user.User.peID;
                  bd.bdEntryDCXC = dtServerNow;
                }
                bd.bdds = (string.IsNullOrWhiteSpace(bd.bdds)) ? null : bd.bdds;

                dbContext.Entry(bd).State = EntityState.Added;
              });
              #endregion
              //Update
              #region Update
              List<BookingDeposit> lstDepositsUpd = guestInvitation.BookingDepositList.Where(bd => guestInvitation.CloneBookingDepositList.Any(bdd => bdd.bdID == bd.bdID)).ToList();
              lstDepositsUpd.ForEach(bd =>
              {
                bd.bdD = dtServerNow;
                if (bd.bdFolioCXC != null)
                {
                  bd.bdUserCXC = user.User.peID;
                  bd.bdEntryDCXC = dtServerNow;
                }
                else
                {
                  bd.bdUserCXC = null;
                  bd.bdEntryDCXC = null;
                }
                bd.bdds = (string.IsNullOrWhiteSpace(bd.bdds)) ? null : bd.bdds;

                dbContext.Entry(bd).State = EntityState.Modified;
              });
              #endregion

              //Eliminar
              #region Delete
              List<BookingDeposit> lstDepositsDel = guestInvitation.CloneBookingDepositList.Where(bd => !guestInvitation.BookingDepositList.Any(bdd => bdd.bdID == bd.bdID)).ToList();
              lstDepositsDel.ForEach(bd => dbContext.Entry(bd).State = EntityState.Deleted);
              #endregion
              #endregion

              #region CreditCard
              var lstGuestCreditCards = dbContext.GuestsCreditCards.Where(cc => cc.gdgu == guestInvitation.Guest.guID);

              //Si se encontro alguno los eliminamos 
              if (lstGuestCreditCards.Any())
              {
                dbContext.GuestsCreditCards.RemoveRange(lstGuestCreditCards);
              }

              //Si se agregaron nuevos GuestCreditCards los guardamos.
              guestInvitation.GuestCreditCardList.ToList().ForEach(gcc => {
                gcc.gdgu = guestInvitation.Guest.guID;
                dbContext.Entry(gcc).State = EntityState.Added;
              });

              //Creamos la variable en donde se van a concatenar 
              //Recorremos el Listado y concatenamos con la condicion de que Quantity sea mayor que 1
              string strguCCType = string.Join(", ", guestInvitation.GuestCreditCardList.
                Select(x => (x.gdQuantity > 1 ? x.gdQuantity + " " : string.Empty) + x.gdcc));
              
              //Escojemos los primeros 30 caracteres porque es el límite del campo 
              guestInvitation.Guest.guCCType = strguCCType.Length > 30 ? strguCCType.Substring(0, 30) : strguCCType;

              #endregion

              #region GuestStatus
              //Eliminamos los regsitros
              #region Delete
              var lstGuestStatusDel = dbContext.GuestsStatus.Where(gs => gs.gtgu == guestInvitation.Guest.guID);
              dbContext.GuestsStatus.RemoveRange(lstGuestStatusDel);
              #endregion

              #region Add
              if (guestInvitation.Guest.guGStatus != null)
              {
                GuestStatus guestStatus = new GuestStatus();
                guestStatus.gtQuantity = 1;
                guestStatus.gtgu = guestInvitation.Guest.guID;
                guestStatus.gtgs = guestInvitation.Guest.guGStatus;
                dbContext.Entry(guestStatus).State = EntityState.Added;
              }
              #endregion
              #endregion

              #region Proceso Guest Additional
              //Eliminamos los registros con guID=0
              var additionalGuests = guestInvitation.AdditionalGuestList.Cast<Guest>().ToList();
              additionalGuests.RemoveAll(c => c.guID == 0);
              if (additionalGuests.Any())
              {
                dbContext.USP_IM_SaveGuestAdditional(guestInvitation.Guest.guID, string.Join(",", additionalGuests.Select(c => c.guID).ToList()));
                if (enumProgram == EnumProgram.Inhouse)
                {
                  //Actualizamos los datos de los huespedes adicionales.
                  dbContext.USP_OR_UpdateGuestsAdditional(guestInvitation.Guest.guID, guestInvitation.Guest.guPRInvit1, dtServerNow, user.Location.loID, guestInvitation.Guest.guInvit);
                  guestInvitation.AdditionalGuestList.ToList().ForEach(c =>
                  {
                    //Guardamos el Log del guestAdditional
                    dbContext.USP_OR_SaveGuestMovement(guestInvitation.Guest.guID, EnumToListHelper.GetEnumDescription(EnumGuestsMovementsType.Contact), user.User.peID, computerName, iPAddress);
                  });
                }
              }
              #endregion

              #region GuestMovement
              dbContext.USP_OR_SaveGuestMovement(guestInvitation.Guest.guID, EnumToListHelper.GetEnumDescription(guestMovementType), user.User.peID, computerName, iPAddress);
              #endregion

              nSave += dbContext.SaveChanges();
              transacction.Commit();
              return nSave;

            }
            catch
            {
              transacction.Rollback();
              throw;
            }
          }
          #endregion
        }
      });
    }
    #endregion

    #region SetDeposits
    /// <summary>
    /// Establece el deposito
    /// </summary>
    /// <param name="lstDeposits">Lista de depositos</param>
    /// <param name="guest">guest para asiganr el deposito</param>
    /// <history>
    /// [emoguel] 17/08/2016 created
    /// </history>
    private static void SetDeposits(List<BookingDeposit> lstDeposits, Guest guest)
    {
      //Establecemos el primer deposito
      if (lstDeposits.Count > 0)
      {
        guest.guDeposit = lstDeposits.FirstOrDefault().bdAmount;
      }

      //Si no tuvo deposito, se establece el deposito quemado
      if (guest.guDeposit == 0)
      {
        guest.guDeposit = guest.guDepositTwisted;
      }
    }
    #endregion

    #region SaveGuest

    /// <summary>
    /// Proceso de guardado del Guest.
    /// </summary>
    /// <returns>  Huesped principal.</returns>
    /// <history>
    /// [edgrodriguez] 18/08/2016  Created.
    /// </history>
    public static async Task<int> SaveGuest(GuestInvitation guestInvitation)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          DateTime dtServerNow = BRHelpers.GetServerDateTime();
          #region Transaccion
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              int nSave = 0;
              guestInvitation.Guest.guCreationD = dtServerNow;
              //Guest
              dbContext.Entry(guestInvitation.Guest).State = (guestInvitation.Guest.guID > 0) ? EntityState.Modified : EntityState.Added;
              nSave = dbContext.SaveChanges();//Para que se le agregué el Id al guest

              #region Gifts
              //Invitation Gift a Eliminar
              #region Eliminar
              List<InvitationGift> lstGiftsDel = guestInvitation.CloneInvitationGiftList.Where(ig =>
                        !guestInvitation.InvitationGiftList.Any(igg => ig.iggu == igg.iggu && ig.iggi == igg.iggi && ig.igct == igg.igct)).ToList();

              lstGiftsDel.ForEach(ig => dbContext.Entry(ig).State = EntityState.Deleted);
              #endregion

              //Invitation Gift a Agregar
              #region Agregar
              List<InvitationGift> lstGiftsAdd = guestInvitation.InvitationGiftList.Where(ig =>
                        !guestInvitation.CloneInvitationGiftList.Any(igg => ig.iggu == igg.iggu && ig.iggi == igg.iggi && ig.igct == igg.igct)).ToList();

              lstGiftsAdd.ForEach(ig =>
              {
                ig.iggu = guestInvitation.Guest.guID;
                dbContext.Entry(ig).State = EntityState.Added;
              });
              #endregion

              //Invitation Gift a Actualizar
              //TODO: falta validar si cambió algun campo
              #region Actualizar
              List<InvitationGift> lstGiftsUpd = guestInvitation.InvitationGiftList.Where(ig =>
                        guestInvitation.CloneInvitationGiftList.Any(igg => ig.iggu == igg.iggu && ig.iggi == igg.iggi && ig.igct == igg.igct)).ToList();

              lstGiftsUpd.ForEach(ig => dbContext.Entry(ig).State = EntityState.Modified);
              #endregion




              #endregion

              #region GuestStatus
              //Eliminamos los registros
              #region Delete
              var lstGuestStatusDel = dbContext.GuestsStatus.Where(gs => gs.gtgu == guestInvitation.Guest.guID);
              dbContext.GuestsStatus.RemoveRange(lstGuestStatusDel);
              #endregion
              #region Add
              if (guestInvitation.Guest.guGStatus != null)
              {
                GuestStatus guestStatus = new GuestStatus();
                guestStatus.gtQuantity = 1;
                guestStatus.gtgu = guestInvitation.Guest.guID;
                guestStatus.gtgs = guestInvitation.Guest.guGStatus;
                dbContext.Entry(guestStatus).State = EntityState.Added;
              }
              #endregion
              #endregion

              nSave += dbContext.SaveChanges();
              transacction.Commit();

              return guestInvitation.Guest.guID;

            }
            catch
            {
              transacction.Rollback();
              throw;
            }
          }
          #endregion
        }
      });
    }

    #endregion GetAdditionalGuest

    #region ValidateInvitation
    /// <summary>
    /// Sirve para validar si los datos existen si no existen devuelven mensaje de error 
    /// </summary>
    /// <param name="changedBy"></param>
    /// <param name="pR"></param>
    /// <param name="location"></param>
    /// <param name="leadSource"></param>
    /// <param name="salesRoom"></param>
    /// <param name="agency"></param>
    /// <param name="country"></param>
    /// <returns>List<ValidationData></returns>
    /// <history>
    /// [erosado] 19/08/2016  Created.
    /// </history>
    public static async Task<List<ValidationData>> ValidateInvitation(string changedBy, string pR, string location, string leadSource, string salesRoom, string agency, string country)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_ValidateInvitation(changedBy, pR, location, leadSource, salesRoom, agency, country).ToList();
        }

      });
    }
    #endregion
  }
}