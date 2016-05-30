using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Classes;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRGuests
  {
    #region GetGuestsArrivals

    /// <summary>
    /// Obtiene las llegadas de huespedes
    /// </summary>
    /// <param name="Date">Fecha </param>
    /// <param name="LeadSource">LeadSource </param>
    /// <param name="Markets">Mercado </param>
    /// <param name="Available">Available </param>
    /// <param name="Contacted">Contacted</param>
    /// <param name="Invited">Invited</param>
    /// <param name="OnGroup">OnGroup</param>
    public static List<GuestArrival> GetGuestsArrivals(DateTime Date, string LeadSource, string Markets, int Available, int Contacted, int Invited, int OnGroup)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetArrivals_Timeout;
        return dbContext.USP_OR_GetArrivals(Date, LeadSource, Markets, Available, Contacted, Invited, OnGroup).ToList();
      }
    }

    #endregion

    #region GetGuestsAvailables

    /// <summary>
    /// Obtiene los huespedes disponibles
    /// </summary>
    /// <param name="Date">Fecha </param>
    /// <param name="LeadSource">LeadSource </param>
    /// <param name="Markets">Mercado </param>
    /// <param name="Contacted">Contacted</param>
    /// <param name="Invited">Invited</param>
    /// <param name="OnGroup">OnGroup</param>
    public static List<GuestAvailable> GetGuestsAvailables(DateTime Date, string LeadSource, string Markets, int Contacted, int Invited, int OnGroup)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetAvailables_Timeout;
        return dbContext.USP_OR_GetAvailables(Date, LeadSource, Markets, Contacted, Invited, OnGroup).ToList();
      }
    }

    #endregion

    #region GetGuestsPremanifest

    /// <summary>
    /// Obtiene los huespedes premanifestados
    /// </summary>
    /// <param name="Date">Fecha </param>
    /// <param name="LeadSource">LeadSource </param>
    /// <param name="Markets">Mercado </param>
    /// <param name="OnGroup">OnGroup</param>
    public static List<GuestPremanifest> GetGuestsPremanifest(DateTime Date, string LeadSource, string Markets, int OnGroup)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetPremanifest_Timeout;
        return dbContext.USP_OR_GetPremanifest(Date, LeadSource, Markets, OnGroup).ToList();
      }
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetPremanifestHost(Date, salesRoom).ToList();
      }
    }

    #endregion

    #region GetGuestByID

    /// <summary>
    /// Obtiene un invitado por su ID
    /// </summary>
    /// <param name="guestId">Identificador del invitado</param>
    /// <returns>Invitado</returns>
    public static Guest GetGuestById(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
    /// </history>
    public async static Task<List<GuestStatusType>> GetGuestStatusType(int status)
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          bool statusGuestStatus = Convert.ToBoolean(status);
          return await dbContext.GuestsStatusTypes.Where(gs => gs.gsA == statusGuestStatus).ToListAsync();
        }
    }
    #endregion

    #region GetGuest
    /// <summary>
    /// get a Guest
    /// </summary>
    /// <param name="guId">Id a Guest</param>
    /// <returns>Guest</returns>
    /// <history>
    /// [jorcanche] created 10/03/2016
    /// </history>
    public static Guest GetGuest(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from gu in dbContext.Guests where gu.guID == guestId select gu).Single();
      }
    }
    #endregion

    #region GetGuestShort
    /// <summary>
    /// Obtiene registros especificos de un guest Short con un guestID ingresado.
    /// </summary>
    /// <param name="guestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 11/Abril/2016 Created
    /// </history>
    public static GuestShort GetGuestShort(int guestID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGuestById(guestID).SingleOrDefault();
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
    public static void SaveGuestMovement(int guestId, EnumGuestsMovementsType guestMovementType, string changedBy, string computerName, string iPAddress)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_SaveGuestMovement(guestId, EnumToListHelper.GetEnumDescription(guestMovementType), changedBy, computerName, iPAddress);
      }
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
    public static int SaveGuest(Guest guest)
    {
      int nRes = 0;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Entry(guest).State = System.Data.Entity.EntityState.Modified;
        return nRes = dbContext.SaveChanges();

      }
    }
    #endregion

    #region GetGuestCreditCard
    /// <summary>
    /// Obtiene las tarjetas de crédito del invitado
    /// </summary>
    /// <param name="guestId">Identificador del invitado</param>
    /// <returns></returns>
    /// <history>
    /// [lchairez] 18/03/2016 Created.
    /// </history>
    public static List<GuestCreditCard> GetGuestCreditCard(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.GuestsCreditCards.Where(gc => gc.gdgu == guestId).ToList();
      }
    }
    #endregion

    #region GetExtraGuest
    /// <summary>
    /// Obtiene los datos de los invitados adicionales
    /// </summary>
    /// <param name="guestId">Identificador del invitado</param>
    /// <returns>List<Guest></returns>
    public static List<Guest> GetExtraGuest(int guestId)
    {
      return null;
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
    public static List<GuestSearched> GetGuests(DateTime dateFrom, DateTime dateTo, string leadSource, string name = "ALL",
                                                string roomNumber = "ALL", string reservation = "ALL", int guestID = 0)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Properties.Settings.Default.USP_OR_GetGuests_Timeout;
        return dbContext.USP_OR_GetGuests(dateFrom, dateTo, leadSource, name, roomNumber, reservation, guestID).ToList();
      }
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
    /// [erosado] 18/Mar/2016
    /// </history>
    public static List<GuestByPR> GetGuestsByPR(DateTime dateFrom, DateTime dateTo, string leadSources, string PR, List<bool> filtros)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGuestsByPR(dateFrom, dateTo, leadSources, PR, filtros[0], filtros[1], filtros[2], filtros[3], filtros[4], filtros[5], filtros[6]).ToList();
      }
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
    /// <param name="program"></param>
    /// <returns>Listado de Guest</returns>
    /// <history>
    /// [ECANUL] 01-04-2016 Created
    /// [jorcanche] 04/05/2016 Simplificado
    /// </history>
    public static List<Guest> GetSearchGuestByLS(string leadsource, string salesRoom, string name, string room, string reservation, int guid, DateTime from, DateTime to, EnumProgram program, string PR)
    {
      var pro = EnumToListHelper.GetEnumDescription(program);
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
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
    public static int SaveChangedOfGuest(Guest guest, short lsHoursDif, string changedBy)
    {
      int respuesta = 0;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {
            //Guardamos los cambios en el Guest
            dbContext.Entry(guest).State = System.Data.Entity.EntityState.Modified;

            //Guardamos el Log del guest
            dbContext.USP_OR_SaveGuestLog(guest.guID, lsHoursDif, changedBy);

            respuesta = dbContext.SaveChanges();
            transaction.Commit();
            return respuesta;
          }
          catch
          {
            transaction.Rollback();
            return respuesta = 0;
          }
        }
      }
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
    public static int SaveGuestContact(Guest guest, short lsHoursDif, string changedBy, EnumGuestsMovementsType guestMovementType, string computerName, string iPAddress)
    {
      int respuesta = 0;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {
            //guardamos la informacion de contacto
            dbContext.Entry(guest).State = System.Data.Entity.EntityState.Modified;

            //guardamos el Log del huesped
            dbContext.USP_OR_SaveGuestLog(guest.guID, lsHoursDif, changedBy);

            //guardamos el movimiento de contactacion del huesped
            dbContext.USP_OR_SaveGuestMovement(guest.guID, EnumToListHelper.GetEnumDescription(guestMovementType), changedBy, computerName, iPAddress);

            respuesta = dbContext.SaveChanges();
            transaction.Commit();
            return respuesta;
          }
          catch
          {
            transaction.Rollback();
            return respuesta = 0;
          }
        }
      }
    }
    #endregion

    #region GetSearchGuest

    public static List<Guest> GetSearchGuest(string leadSource, string name, string room, string reservacion, DateTime dtFrom, DateTime dtTo, string lsProgram)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var guests = (from g in dbContext.Guests
                      join ls in dbContext.LeadSources on g.guls equals ls.lsID
                      where ls.lspg == lsProgram
                           && (g.guCheckInD >= dtFrom && g.guCheckInD <= dtTo)
                      select g).ToList();
        if (!String.IsNullOrEmpty(leadSource))
        {
          guests = guests.Where(g => g.guls.Equals(leadSource)).ToList();
        }

        if (!String.IsNullOrEmpty(name))
        {
          guests = guests.Where(g => (g.guLastName1 != null && g.guLastName1.ToUpper().Contains(name))
                                || (g.guFirstName1 != null && g.guFirstName1.ToUpper().Contains(name))
                                || (g.guLastname2 != null && g.guLastname2.ToUpper().Contains(name))
                                || (g.guFirstName2 != null && g.guFirstName2.ToUpper().Contains(name))).ToList();

        }

        if (!String.IsNullOrEmpty(room))
        {
          guests = guests.Where(g => g.guRoomNum != null && g.guRoomNum.Contains(room)).ToList();
        }

        if (!String.IsNullOrEmpty(reservacion))
        {
          guests = guests.Where(g => g.guHReservID != null && g.guHReservID.Contains(reservacion)).ToList();
        }

        return guests;
      }
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
    /// <param name="guId">Id a Guest</param>
    /// <returns>Un listado de PRNotes</returns>
    /// <history>
    /// [jorcanche] created 22/03/2016
    /// </history>    
    public static List<GuestPremanifestOuthouse> GetGuestPremanifestOuthouse(bool bookinvit, DateTime Date, string LeadSource)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from G in dbContext.Guests
                join Co in dbContext.Countries on G.guco equals Co.coID
                join Ag in dbContext.Agencies on G.guag equals Ag.agID
                where (bookinvit ? G.guBookD : G.guInvitD) == Date
                where G.guls == LeadSource
                orderby G.guBookT, G.guLastName1
                select new GuestPremanifestOuthouse
                {
                  guStatus = G.guStatus,
                  guID = G.guID,
                  guCheckIn = G.guCheckIn,
                  guRoomNum = G.guRoomNum,
                  guLastName1 = G.guLastName1,
                  guFirstName1 = G.guFirstName1,
                  guCheckInD = G.guCheckInD,
                  guCheckOutD = G.guCheckOutD,
                  guco = G.guco,
                  coN = Co.coN,
                  guag = G.guag,
                  agN = Ag.agN,
                  guAvail = G.guAvail,
                  guInfo = G.guInfo,
                  guPRInfo = G.guPRInfo,
                  guInfoD = G.guInfoD,
                  guInvit = G.guInvit,
                  guInvitD = G.guInvitD,
                  guBookD = G.guBookD,
                  guBookT = G.guBookT,
                  guPRInvit1 = G.guPRInvit1,
                  guMembershipNum = G.guMembershipNum,
                  guBookCanc = G.guBookCanc,
                  guShow = G.guShow,
                  guSale = G.guSale,
                  guComments = G.guComments,
                  guPax = G.guPax
                }).ToList();
      }

    }

    #endregion

    /// <summary>
    /// Elimina un Guest
    /// </summary>
    /// <history>
    /// [jorcanche] 04/05/2016 created
    /// </history>
    /// <param name="guID"></param>
    public static void DeleteGuest(int guID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_DeleteGuest(guID);
      }
    }

    #region GetGuestValidForTransfer
    /// <summary>
    /// Valida si existe el guesta en los registros. Si existe retorna el registro.
    /// </summary>
    /// <param name="guHReservID">ID de la reservación</param>
    /// <param name="gulsOriginal">ID del guest</param>
    /// <returns>Resgistro del Guest</returns>
    /// <history>
    /// [michan]  20/04/2016  Created
    /// </history>
    public async static Task<Guest> GetGuestValidForTransfer(string guHReservID, string gulsOriginal)
    {
      Guest guest = null;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        //Revisamos si ya existe en Guest
        guest = await dbContext.Guests.SingleOrDefaultAsync(g => g.guHReservID == guHReservID && g.gulsOriginal == gulsOriginal);
      }
      return guest;
    }
    #endregion
  }

}