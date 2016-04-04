using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.BusinessRules.Classes;
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

    #region GetGuestStatusTypeInvit
    /// <summary>
    /// Obtiene una lista específica de elementos para el grid de guest estatus
    /// </summary>
    /// <param name="guestId">Identificador del invitado</param>
    /// <returns>Lista de GuestStatusTypeInvit</returns>
    /// <history>
    /// [lchairez] 16/03/2016 Created.
    /// </history>
    public static List<GuestStatusInvitation> GetGuestStatusTypeInvit(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var gs = (from gss in dbContext.GuestsStatus
                  join gst in dbContext.GuestsStatusTypes on gss.gtgs equals gst.gsID
                  where gss.gtgu == guestId
                  select new GuestStatusInvitation
                  {
                    gsgu = gss.gtgu,
                    gsID = gss.gtgs,
                    GuestStatus = gst.gsN,
                    gsQty = gss.gtQuantity
                  }).ToList();
        return gs;
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
    public static void SaveGuestInvitation(Guest guest
                                          , List<InvitationGift> lstNewGifts
                                          , List<GiftInvitation> lstUpdatedGifts
                                          , List<BookingDeposit> lstNewDeposits
                                          , List<BookingDepositInvitation> lstUpdatedDeposits
                                          , List<GuestStatus> lstNewGuestStatus
                                          , List<GuestStatusInvitation> lstUpdatedGuestStatus
                                          , List<GuestCreditCard> lstNewCreditCard
                                          , List<GuestCreditCardInvitation> lstUpdatedCreditCard
                                          , List<Guest> lstNewGuestAdditional
                                          , List<GuestAdditionalInvitation> lstUpdatedGuestAdditional)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        try
        {
          dbContext.Guests.Attach(guest);
          dbContext.Entry(guest).State = System.Data.Entity.EntityState.Modified;

          #region Regalos

          //Agregamos los regalos nuevos
          if (lstNewGifts.Any())
            dbContext.InvitationsGifts.AddRange(lstNewGifts);

          //Actualizamos los regalos
          foreach (var item in lstUpdatedGifts)
          {
            var gi = dbContext.InvitationsGifts.SingleOrDefault(g => g.iggu == item.iggu && g.iggi == item.iggiPrevious);
            if (gi != null)
            {
              if (item.iggi != item.iggiPrevious)
              {
                dbContext.InvitationsGifts.Remove(gi);

                var newGift = new InvitationGift();
                newGift.igAdults = item.igAdults;
                newGift.igct = item.igct;
                newGift.igExtraAdults = item.igExtraAdults;
                newGift.iggi = item.iggi;
                newGift.iggu = item.iggu;
                newGift.igMinors = item.igMinors;
                newGift.igPriceA = item.igPriceA;
                newGift.igPriceAdult = item.igPriceAdult;
                newGift.igPriceExtraAdult = item.igPriceExtraAdult;
                newGift.igPriceM = item.igPriceM;
                newGift.igPriceMinor = item.igPriceMinor;
                newGift.igQty = item.igQty;

                dbContext.InvitationsGifts.Add(newGift);
              }
              else
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
          if (lstNewDeposits.Any())
            dbContext.BookingDeposits.AddRange(lstNewDeposits);

          foreach (var item in lstUpdatedDeposits)
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
          if (lstNewGuestStatus.Any())
            dbContext.GuestsStatus.AddRange(lstNewGuestStatus);

          foreach (var item in lstUpdatedGuestStatus)
          {
            var gs = dbContext.GuestsStatus.SingleOrDefault(g => g.gtgu == item.gsgu && g.gtgs == item.gsIDPrevious);
            if (gs != null)
            {
              if (item.gsID != item.gsIDPrevious) //si el ID de los estatus es diferente se elimina y se inserta un nuevo registro nuevamente
              {
                dbContext.GuestsStatus.Remove(gs);

                var newGS = new GuestStatus();
                newGS.gtgs = item.gsID;
                newGS.gtgu = item.gsgu;
                newGS.gtQuantity = Convert.ToByte(item.gsQty);
                dbContext.GuestsStatus.Add(newGS);
              }
              else //sino solo se modifica el registro existente
              {
                gs.gtQuantity = Convert.ToByte(item.gsQty);
              }
            }

          }
          #endregion

          #region Credit Cards
          if (lstNewCreditCard.Any())
            dbContext.GuestsCreditCards.AddRange(lstNewCreditCard);

          foreach (var item in lstUpdatedCreditCard)
          {
            var cc = dbContext.GuestsCreditCards.SingleOrDefault(c => c.gdcc == item.ccIDPrevious && c.gdgu == item.ccgu);
            if (cc != null)
            {
              if (item.ccID != item.ccIDPrevious) //si el ID de las tarejtas son diferente se elimina y se inserta un nuevo registro nuevamente
              {
                dbContext.GuestsCreditCards.Remove(cc);
                var newCC = new GuestCreditCard();
                newCC.gdcc = item.ccID;
                newCC.gdgu = item.ccgu;
                newCC.gdQuantity = Convert.ToByte(item.ccQty);

                dbContext.GuestsCreditCards.Add(newCC);
              }
              else //sino solo se actualiza el registro
              {
                cc.gdQuantity = Convert.ToByte(item.ccQty);
              }
            }
          }
          #endregion
          
          #region Additional Information
          //Agregamos un nuevo invitado adicional
          foreach (var item in lstNewGuestAdditional)
          {
            var guestAddit = dbContext.Guests.SingleOrDefault(gg => gg.guID == item.guID);
            if (guestAddit != null)
              guest.GuestsAdditional.Add(guestAddit);
          }

          //Actualizamos los adicionales
          foreach (var item in lstUpdatedGuestAdditional)
          {
            var updatedGuestAddit = guest.GuestsAdditional.SingleOrDefault(gg => gg.guID == item.guIDPrevious);
            if (updatedGuestAddit != null)
            {
              guest.GuestsAdditional.Remove(updatedGuestAddit);
              var guestAddit = dbContext.Guests.SingleOrDefault(gg => gg.guID == item.guID);
              if (guestAddit != null)
                guest.GuestsAdditional.Add(guestAddit);
            }

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

    #region GuestStatustypeId
    /// <summary>
    /// Obtiene el tipo de  estatus del invitado por su ID
    /// </summary>
    /// <param name="gstId">Identificador del tipo de estado del invitado</param>
    /// <returns>GuestStatusType</returns>
    /// <history>
    /// [lchairez] 24/03/2016 Created.
    /// </history>
    public static GuestStatusType GetGuestStatusTypeId(string gstId)
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.GuestsStatusTypes.Where(g => g.gsID == gstId).SingleOrDefault();
      }
    }
    #endregion

    #region GetGuestCreditCardInvitation
    /// <summary>
    /// Obtiene una lista específica para llenar el grid de tarjetas de credito para las invitaciones
    /// </summary>
    /// <param name="guestId">Identificador del invitado</param>
    /// <returns>Lista de GuestCreditCardInvitation</returns>
    /// <history>
    /// [lchairez] 24/03/2016 Created.
    /// </history>
    public static List<GuestCreditCardInvitation> GetGuestCreditCardInvitation(int guestId)
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var creditCards = GetGuestCreditCard(guestId);

        var cc = (from gc in creditCards
                  join c in dbContext.CreditCardTypes on gc.gdcc equals c.ccID
                  select new GuestCreditCardInvitation
                  {
                    ccgu = gc.gdgu,
                    ccID = gc.gdcc,
                    ccQty = gc.gdQuantity,
                    CreditCard = c.ccN
                  }).ToList();

        return cc;
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
                        gu.guLastName1.Contains(guest.guLastName1) || gu.guFirstName1.Contains(guest.guFirstName1)
                        || gu.guLastname2.Contains(guest.guLastname2) || gu.guFirstName2.Contains(guest.guFirstName2)
                        && (gu.guCheckInD >= guest.guCheckInD && gu.guCheckInD <= guest.guCheckOutD)
                        && gu.guls == guest.guls
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

  }
}