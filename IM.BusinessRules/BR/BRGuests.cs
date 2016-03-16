using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Classes;
using IM.Model.Enums;

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
      using (var dbContext = new IMEntities())
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
      using (var dbContext = new IMEntities())
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
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetPremanifest(Date, LeadSource, Markets, OnGroup).ToList();
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
      using (var dbContext = new IMEntities())
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
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetPremanifestHost(Date, salesRoom).ToList();
      }
    }

    #endregion

    #region GetGuestLog

    /// <summary>
    /// Obtiene log del Guest Ingresado
    /// </summary>
    /// <param name="IdGuest">Id del Guest</param>
    /// <history>[jorcanche] 09/03/2016</history>
    public static List<GuestLogData> GetGuestLog(int IdGuest)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetGuestLog(IdGuest).ToList();
      }
    }
    #endregion

    #region SaveGuestLog

    /// <summary>
    /// Guarda los cambios del log del Guest
    /// </summary>
    ///<param name="IdGuest"></param>
    /// <param name="changedBy"></param>
    /// <param name="changedBy"></param>
    /// <history>[jorcanche] 11/03/2016</history>
    public static void SaveGuestLog(int IdGuest, short lsHoursDif, string changedBy)
    {
      using (var dbContext = new IMEntities())
      {
        dbContext.USP_OR_SaveGuestLog(IdGuest, lsHoursDif, changedBy);
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
      using (var dbContext = new IMEntities())
      {
        return (from gu in dbContext.Guests where gu.guID == guestId select gu).Single();
      }
    }

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
      using (var dbContext = new IMEntities())
      {
        dbContext.Entry(guest).State = System.Data.Entity.EntityState.Modified;
       return nRes = dbContext.SaveChanges();
       
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
       using (var dbContext = new IMEntities())
      {
        dbContext.USP_OR_SaveGuestMovement(guestId, StrToEnums.EumGuestsMovementsTypeToString(guestMovementType), changedBy, computerName, iPAddress);
      }
    }
    #endregion
    #endregion
  }
}



