using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using IM.Model.Classes;

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
    public static List<GuestStatusType> GetGuestStatusType(int status)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        bool statusGuestStatus = Convert.ToBoolean(status);
        return dbContext.GuestsStatusTypes.Where(gs => gs.gsA == statusGuestStatus).ToList();
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
        dbContext.USP_OR_SaveGuestMovement(guestId, StrToEnums.EumGuestsMovementsTypeToString(guestMovementType), changedBy, computerName, iPAddress);
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
          if (invitation.NewGifts.Any())
            dbContext.InvitationsGifts.AddRange(invitation.NewGifts);

          //Actualizamos los regalos
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

          #endregion

          #region Depósitos
          //Agregamos los nuevos depósitos
          if (invitation.NewDeposits.Any())
            dbContext.BookingDeposits.AddRange(invitation.NewDeposits);

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
          #endregion

          #region Guest Status
          if (invitation.DeletedGuestStatus.Any())
          {

            foreach(var row in invitation.DeletedGuestStatus)
            {
              var gs = dbContext.GuestsStatus.SingleOrDefault(g => g.gtgu == row.gtgu && g.gtgs == row.gtgs);
              if(gs !=null)
                dbContext.GuestsStatus.Remove(gs);
            }
            
          }

          if (invitation.NewGuestStatus.Any())
            dbContext.GuestsStatus.AddRange(invitation.NewGuestStatus);

          #endregion

          #region Credit Cards
          if(invitation.DeletedCreditCards.Any())
          {
            var g = invitation.DeletedCreditCards.First().gdgu;
            var cc = dbContext.GuestsCreditCards.Where(c=> c.gdgu == g).ToList();
            dbContext.GuestsCreditCards.RemoveRange(cc);
          }
          
          if(invitation.NewCreditCards.Any())
            dbContext.GuestsCreditCards.AddRange(invitation.NewCreditCards);
          
          #endregion
          
          #region Additional Information
          //Borramos los invitados adicionales
          foreach(var row in invitation.DeletedAdditional)
          {
              var guestAddit = dbContext.Guests.SingleOrDefault(gg => gg.guID == row.guID);
              if (guestAddit != null)
                invitation.Guest.GuestsAdditional.Remove(guestAddit);
          }

          //Agregamos un nuevo invitado adicional
          foreach (var item in invitation.NewAdditional)
          {
            var guestAddit = dbContext.Guests.SingleOrDefault(gg => gg.guID == item.guID);
            if (guestAddit != null)
              invitation.Guest.GuestsAdditional.Add(guestAddit);
          }



          #endregion

          dbContext.SaveChanges();
        }
        catch(Exception ex)
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
        dbContext.Database.CommandTimeout = 120;        
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

    #region GetSearchGuestByLS
    /// <summary>
    /// Obtiene una lista de Guest resultado de una busqueda con parametros
    /// </summary>
    /// <param name="guest">Guest con informacion para buscar</param>
    /// <param name="leadSource">LS.lspg en el cual se hara la busqueda</param>
    /// <returns>Lista de Guests Encontrados</returns>
    /// <history>[ECANUL] 01-04-2016 Created</history>
    public static List<Guest> GetSearchGuestByLS(Guest guest, LeadSource leadSource)
    {
      List<Guest> lstGuest;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        lstGuest = new List<Guest>();
        if (guest.guID != 0) //Si tiene GUID, solo se busca por eso y por LS
        {
          lstGuest = (from gu in dbContext.Guests
                      join ls in dbContext.LeadSources
                      on gu.guls equals ls.lsID
                      where ls.lspg == leadSource.lspg && gu.guID == guest.guID
                      select gu).ToList();
        }
        else //Si no se tiene GUID se busca por los siguentes criterios
        { //Si envia El nombre o apellido del Huesped, Se busca por LS, Nombres o apellidos y por Fecha de llegada
          if (guest.guLastName1 != "" && guest.guLastName1 != null)
          {
            lstGuest = (from gu in dbContext.Guests
                        join ls in dbContext.LeadSources
                        on gu.guls equals ls.lsID
                        where ls.lspg == leadSource.lspg &&
                        (gu.guLastName1.Contains(guest.guLastName1) || gu.guFirstName1.Contains(guest.guFirstName1)
                        || gu.guLastname2.Contains(guest.guLastname2) || gu.guFirstName2.Contains(guest.guFirstName2))
                        && gu.guls == guest.guls
                        && (gu.guCheckInD >= guest.guCheckInD && gu.guCheckInD <= guest.guCheckOutD)
                        orderby gu.guls, gu.guLastName1
                        select gu).ToList();
          }  //Si tiene el numero de habitacion
          else if (guest.guRoomNum != "" && guest.guRoomNum != null)
          {
            lstGuest = (from gu in dbContext.Guests
                        join ls in dbContext.LeadSources
                        on gu.guls equals ls.lsID
                        where ls.lspg == leadSource.lspg && gu.guRoomNum == guest.guRoomNum
                        && (gu.guCheckInD >= guest.guCheckInD && gu.guCheckInD <= guest.guCheckOutD)
                        && gu.guls == guest.guls
                        select gu).ToList();
          } //Si tiene el numero de reservacion
          else if (guest.guHReservID != "" && guest.guHReservID != null)
          {
            lstGuest = (from gu in dbContext.Guests
                        join ls in dbContext.LeadSources
                        on gu.guls equals ls.lsID
                        where ls.lspg == leadSource.lspg && gu.guHReservID == guest.guHReservID
                        && (gu.guCheckInD >= guest.guCheckInD && gu.guCheckInD <= guest.guCheckOutD)
                        && gu.guls == guest.guls
                        select gu).ToList();
          }
          else //si no puso nada en los texbos solo se busca por LS y El rango de fechas
          {
            lstGuest = (from gu in dbContext.Guests
                        join ls in dbContext.LeadSources
                        on gu.guls equals ls.lsID
                        where ls.lspg == leadSource.lspg && gu.guCheckInD >= guest.guCheckInD && gu.guCheckInD <= guest.guCheckOutD
                        && gu.guls == guest.guls
                        select gu).ToList();
          }
        }
        return lstGuest;
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
            dbContext.USP_OR_SaveGuestMovement(guest.guID, StrToEnums.EumGuestsMovementsTypeToString(guestMovementType), changedBy, computerName, iPAddress);

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
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var guests = (from g in dbContext.Guests
                     join ls in dbContext.LeadSources on g.guls equals ls.lsID
                     where ls.lspg == lsProgram
                          && (g.guCheckInD >= dtFrom && g.guCheckInD <= dtTo)
                      select g).ToList();
        if(!String.IsNullOrEmpty(leadSource))
        {
          guests = guests.Where(g => g.guls.Equals(leadSource)).ToList();
        }

        if (!String.IsNullOrEmpty(name))
        {
          guests = guests.Where(g=> (g.guLastName1 != null && g.guLastName1.ToUpper().Contains(name))
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
  }
}