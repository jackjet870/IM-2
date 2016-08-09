using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Classes;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections;

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

    #endregion

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

    #endregion

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

    #endregion

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
    /// </history>
    public static List<GuestPremanifestHost> GetPremanifestHost(DateTime? currentDate, string salesRoomID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetPremanifestHost(currentDate, salesRoomID).ToList();
      }
    }
    #endregion

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

    #endregion

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

    #endregion

    #region GetGuestByID

    /// <summary>
    /// Obtiene un invitado por su ID e Include("GuestsAdditional")
    /// </summary>
    /// <param name="guestId">Identificador del invitado</param>
    /// <returns>Invitado</returns>
    public static Guest GetGuestById(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.Guests.Include("GuestsAdditional").SingleOrDefault(g => g.guID == guestId);
      }
    }

    #endregion

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
    public async static Task<List<GuestStatusType>> GetGuestStatusType(int status)
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
    #endregion

    #region GetGuest

    /// <summary>
    /// get a Guest
    /// </summary>
    /// <param name="guestId">Id del huesped</param>
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
    #endregion

    #region GetAdditionalGuest

    /// <summary>
    /// Obtiene los invitados adicionales de una invitacion
    /// </summary>
    /// <param name="guestId">Guest ID</param>
    /// <returns>Lista de invitados adicionales</returns>
    /// <history>
    /// [erosado] 04/08/2016  Created. 
    /// </history>
    public static async Task<List<Guest>> GetAdditionalGuest(int guestId, bool withAditional = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return (from g in dbContext.Guests from a in g.GuestsAdditional where g.guID == guestId select a).ToList();
        }
      });
    }
    #endregion

    #region GetGuestShort
    /// <summary>
    /// Obtiene registros especificos de un guest Short con un guestID ingresado.
    /// </summary>
    /// <param name="guestId"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 11/Abril/2016 Created
    /// </history>
    public static GuestShort GetGuestShort(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetGuestById(guestId).SingleOrDefault();
      }
    }
    #endregion

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

    #endregion

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
    #endregion

    #region SaveGuest
    /// <summary>
    /// Guarda los datos del Guest
    /// </summary>
    /// <returns>Guest</returns>
    /// <history>
    /// [jorcanche] created 14/03/2016
    /// </history>
    public static async Task<int> SaveGuest(Guest guest)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Entry(guest).State = EntityState.Modified;
          return dbContext.SaveChanges();
        }
      });
    }
    #endregion

    #region SaveGuestInvitation

    /// <summary>
    /// Guarda la información de unn invitado de la forma invitación.
    /// </summary>
    /// <param name="guest">Invitado a guardar</param>
    /// <history>
    /// [lchairez] 18/03/2016 Created.
    /// </history>
    public static void SaveGuestInvitation(Invitation invitation)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        try
        {
          dbContext.Guests.Attach(invitation.Guest);
          dbContext.Entry(invitation.Guest).State = System.Data.Entity.EntityState.Modified;

          #region Regalos

          //Agregamos los regalos nuevos
          if (invitation.NewGifts != null && invitation.NewGifts.Any())
            dbContext.InvitationsGifts.AddRange(invitation.NewGifts);

          //Actualizamos los regalos
          if (invitation.UpdatedGifts != null)
          {
            foreach (var item in invitation.UpdatedGifts)
            {
              var gi = dbContext.InvitationsGifts.SingleOrDefault(g => g.iggu == item.iggu && g.iggi == item.iggi);
              if (gi != null)
              {
                gi.igAdults = item.igAdults;
                gi.igct = item.igct;
                gi.igExtraAdults = item.igExtraAdults;
                gi.iggi = item.iggi;
                gi.iggu = item.iggu;
                gi.igMinors = item.igMinors;
                gi.igPriceA = item.igPriceA;
                gi.igPriceAdult = item.igPriceAdult;
                gi.igPriceExtraAdult = item.igPriceExtraAdult;
                gi.igPriceM = item.igPriceM;
                gi.igPriceMinor = item.igPriceMinor;
                gi.igQty = item.igQty;
              }
            }
          }


          #endregion

          #region Depósitos
          //Agregamos los nuevos depósitos
          if (invitation.NewDeposits != null && invitation.NewDeposits.Any())
            dbContext.BookingDeposits.AddRange(invitation.NewDeposits);

          if (invitation.UpdatedDeposits != null)
          {
            foreach (var item in invitation.UpdatedDeposits)
            {
              var dep = dbContext.BookingDeposits.SingleOrDefault(d => d.bdID == item.bdID);
              dep.bdAmount = item.bdAmount;
              dep.bdAuth = item.bdAuth;
              dep.bdCardNum = item.bdCardNum;
              dep.bdcc = item.bdcc;
              dep.bdcu = item.bdcu;
              dep.bdEntryDCXC = item.bdEntryDCXC;
              dep.bdExpD = item.bdExpD;
              dep.bdFolioCXC = item.bdFolioCXC;
              dep.bdgu = item.bdgu;
              dep.bdpc = item.bdpc;
              dep.bdpt = item.bdpt;
              dep.bdReceived = item.bdReceived;
              dep.bdUserCXC = item.bdUserCXC;
            }
          }

          #endregion

          #region Guest Status
          if (invitation.DeletedGuestStatus != null && invitation.DeletedGuestStatus.Any())
          {

            foreach (var row in invitation.DeletedGuestStatus)
            {
              var gs = dbContext.GuestsStatus.SingleOrDefault(g => g.gtgu == row.gtgu && g.gtgs == row.gtgs);
              if (gs != null)
                dbContext.GuestsStatus.Remove(gs);
            }

          }

          if (invitation.NewGuestStatus != null && invitation.NewGuestStatus.Any())
            dbContext.GuestsStatus.AddRange(invitation.NewGuestStatus);

          #endregion

          #region Credit Cards
          if (invitation.DeletedCreditCards != null && invitation.DeletedCreditCards.Any())
          {
            var g = invitation.DeletedCreditCards.First().gdgu;
            var cc = dbContext.GuestsCreditCards.Where(c => c.gdgu == g).ToList();
            dbContext.GuestsCreditCards.RemoveRange(cc);
          }

          if (invitation.NewCreditCards != null && invitation.NewCreditCards.Any())
            dbContext.GuestsCreditCards.AddRange(invitation.NewCreditCards);

          #endregion

          #region Additional Information
          if (invitation.DeletedAdditional != null)
          {
            //Borramos los invitados adicionales
            foreach (var row in invitation.DeletedAdditional)
            {
              var guestAddit = dbContext.Guests.SingleOrDefault(gg => gg.guID == row.guID);
              if (guestAddit != null)
                invitation.Guest.GuestsAdditional.Remove(guestAddit);
            }
          }

          if (invitation.NewAdditional != null)
          {
            //Agregamos un nuevo invitado adicional
            foreach (var item in invitation.NewAdditional)
            {
              var guestAddit = dbContext.Guests.SingleOrDefault(gg => gg.guID == item.guID);
              if (guestAddit != null)
                invitation.Guest.GuestsAdditional.Add(guestAddit);
            }
          }

          #endregion

          dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
    }

    #endregion

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
    #endregion

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
    /// </history>
    public async static Task<List<GuestByPR>> GetGuestsByPR(DateTime dateFrom, DateTime dateTo, string leadSources, string PR, List<bool> filtros)
    {
      List<GuestByPR> result = new List<GuestByPR>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetGuestsByPR;
          result = dbContext.USP_OR_GetGuestsByPR(dateFrom, dateTo, leadSources, PR, filtros[0], filtros[1], filtros[2], filtros[3], filtros[4], filtros[5], filtros[6]).ToList();
        }
      });
      return result;
    }

    #endregion

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
    #endregion

    #region GetSearchGuestByLS
    /// <summary>
    /// Trae los huespedes segun los parametros
    /// </summary>
    /// <param name="leadsource"></param>
    /// <param name="salesRoom"></param>
    /// <param name="name"></param>
    /// <param name="room"></param>
    /// <param name="reservation"></param>
    /// <param name="guid"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="program"> Enumerado que indica el program de origen </param>
    /// <returns>Listado de Guest</returns>
    /// <history>
    /// [ECANUL] 01-04-2016 Created
    /// [jorcanche] 04/05/2016 Simplificado
    /// </history>
    public static List<Guest> GetSearchGuestByLS(string leadsource, string salesRoom, string name, string room, string reservation, int guid, DateTime from, DateTime to, EnumProgram program, string PR)
    {
      var pro = EnumToListHelper.GetEnumDescription(program);
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        // Agregamos el timeout de la consulta
        dbContext.Database.CommandTimeout = 180;

        var query = from gu in dbContext.Guests
                    join ls in dbContext.LeadSources
                    on gu.guls equals ls.lsID
                    //busqueda por program
                    where ls.lspg == (program == EnumProgram.Outhouse ? ls.lspg : pro)
                    select gu;
        //Busqueda por clave de huesped
        if (guid != 0)
        {
          query = query.Where(gu => gu.guID == guid);
        }
        else
        { //Busqueda por nombre y aprellido
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
          //Busqueda por fecha de llegada

          query = query.Where(gu =>
                          (program == EnumProgram.Outhouse ? gu.guBookD : gu.guCheckInD) >= from &&
                          (program == EnumProgram.Outhouse ? gu.guBookD : gu.guCheckInD) <= to);
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
    }
    #endregion

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

              //Guardamos el Log del guest
              dbContext.USP_OR_SaveGuestLog(guest.guID, lsHoursDif, changedBy);

              var respuesta = dbContext.SaveChanges();
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
    #endregion

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

              //guardamos el Log del huesped
              dbContext.USP_OR_SaveGuestLog(guest.guID, lsHoursDif, changedBy);

              //guardamos el movimiento de contactacion del huesped
              dbContext.USP_OR_SaveGuestMovement(guest.guID, EnumToListHelper.GetEnumDescription(guestMovementType), changedBy, computerName, iPAddress);

              var respuesta = dbContext.SaveChanges();
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

    #endregion

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
    #endregion

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
    #endregion

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
    /// </history>    
    public static async Task<List<GuestPremanifestOuthouse>> GetGuestPremanifestOuthouse(bool bookinvit, DateTime date, string leadSource)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
      });
    }

    #endregion

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
    #endregion

    #region GetSearchGuestGeneral
    /// <summary>
    /// Busca los huespedes que cumplan los criterios de busqueda
    /// </summary>
    /// <param name="dtpStart"></param>
    /// <param name="dtpEnd"></param>
    /// <param name="GuestID"></param>
    /// <param name="GuestName"></param>
    /// <param name="LeadSource"></param>
    /// <param name="salesRoom"></param>
    /// <param name="RoomNum"></param>
    /// <param name="Reservation"></param>
    /// <param name="PR"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 06/Junio/2016 Created
    /// </history>
    public async static Task<List<Guest>> GetSearchGuestGeneral(DateTime dtpStart, DateTime dtpEnd, int GuestID = 0, string GuestName = "", string LeadSource = "", string salesRoom = "", string RoomNum = "", string Reservation = "", string PR = "")
    {
      List<Guest> lstGuests = new List<Guest>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          // Agregamos el timeout de la consulta
          dbContext.Database.CommandTimeout = 180;

          var query = from _Guests in dbContext.Guests
                      where _Guests.guInvit == true && (_Guests.guBookD >= dtpStart && _Guests.guBookD <= dtpEnd)
                      select _Guests;

          // Busqueda por Guest ID
          if (GuestID > 0)
            query = query.Where(x => x.guID == GuestID);
          else
          {
            // Busqueda por nombre o apellido
            if (GuestName != "")
              query = query.Where(x => x.guLastName1.Contains(GuestName) || x.guFirstName1.Contains(GuestName) || x.guLastname2.Contains(GuestName) || x.guFirstName2.Contains(GuestName));

            // Busqueda por Lead Source
            if (LeadSource != "")
              query = query.Where(x => x.guls == LeadSource);

            // Busqueda por sala
            if (salesRoom != "")
              query = query.Where(x => x.gusr == salesRoom);

            // Busqueda por numero de habitacion
            if (RoomNum != "")
              query = query.Where(x => x.guRoomNum == RoomNum);

            // Busqueda por folio de reservacion
            if (Reservation != "")
              query = query.Where(x => x.guHReservID == Reservation);

            //Busqueda por PR
            if (PR != "")
              query = query.Where(x => x.guPRInvit1 == PR);

          }
          lstGuests = query.ToList();
        }
      });

      return lstGuests;
    }
    #endregion

    #region GetGuestRegistration
    /// <summary>
    /// Retorna la informacion para el reporte Guest Registration
    /// </summary>
    /// <param name="GuestID"></param>
    /// <returns></returns>
    /// <history>
    /// [edgrodriguez] 26/Jul/2016 Created
    /// </history>
    public async static Task<List<IEnumerable>> GetGuestRegistration(int GuestID)
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
    #endregion
  }

}