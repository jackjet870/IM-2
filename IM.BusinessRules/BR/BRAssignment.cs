using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRAssignment
  {
    #region GetGuestUnassigned
    /// <summary>
    /// Obtiene los huespedes no asignados
    /// </summary>
    /// <param name="dateFrom">Fecha desde</param>
    /// <param name="dateTo">Fecha hasta</param>
    /// <param name="leadSource">Clave del LeadSource</param>
    /// <param name="markets">Claves de Mercados</param>
    /// <param name="onlyAvail">Indica si solo se desean los huespedes disponibles</param>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Se agregá asincronía.
    /// </history>
    public async static Task<List<GuestUnassigned>> GetGuestUnassigned(DateTime dateFrom, DateTime dateTo, String leadSource, String markets,  Boolean onlyAvail)
    {
      List<GuestUnassigned> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_GetGuestsUnassigned(dateFrom, dateTo, leadSource, markets, onlyAvail).OrderBy(o => o.guCheckInD).ToList();
        }
      });
      return result; 
    }
    #endregion

    #region RptAssignmentByPR
    /// <summary>
    /// Obtiene los datos para el reporte de huespedes asignados por PR
    /// </summary>
    /// <param name="dateFrom">Fecha desde</param>
    /// <param name="dateTo">Fecha hasta</param>
    /// <param name="leadSource">Clave del LeadSource</param>
    /// <param name="markets">Claves de Mercados</param>
    /// <param name="PR">Clave del PR</param>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Se agrago asincronía.
    /// </history>
    public async static Task<List<RptAssignmentByPR>> RptAssignmentByPR(DateTime dateFrom, DateTime dateTo, String leadSource, String markets, String PR)
    {
      List<RptAssignmentByPR> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_RptAssignmentByPR(dateFrom, dateTo, leadSource, markets, PR).ToList();
        }
      });
      return result;
    }
    #endregion

    #region RptAssignment
    /// <summary>
    /// Obtiene los datos para el reporte de huespedes asignados
    /// </summary>
    /// <param name="dateFrom">Fecha desde</param>
    /// <param name="dateTo">Fecha hasta</param>
    /// <param name="leadSource">Clave del LeadSource</param>
    /// <param name="markets">Claves de Mercados</param>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Se agrego asincronía
    /// </history>
    public async static Task<List<RptAssignment>> RptAssignment(DateTime dateFrom, DateTime dateTo, String leadSource, String markets)
    {
      List<RptAssignment> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_RptAssignment(dateFrom, dateTo, leadSource, markets).ToList();
        }
      });
      return result;
    }
    #endregion

    #region RtpAssignmentArrivals
    /// <summary>
    /// Obtiene los datos para el reporte de llegadas y su asignacion
    /// </summary>
    /// <param name="dateFrom">Fecha desde</param>
    /// <param name="dateTo">Fecha hasta</param>
    /// <param name="leadSource">Clave del LeadSource</param>
    /// <param name="markets">Claves de Mercados</param>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Se agrego asincronia.
    /// </history>
    public async static Task<List<RptAssignmentArrivals>> RptAssignmetArrivals(DateTime dateFrom, DateTime dateTo, String leadSource, String markets)
    {
      List<RptAssignmentArrivals> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_RptAssignmentArrivals(dateFrom, dateTo, leadSource, markets).ToList();
        }
      });
      return result;
    }
    #endregion

    #region GetPRsAssigned
    /// <summary>
    /// Obtiene los PRs que tienen asignado al menos a un huesped
    /// </summary>
    /// <param name="dateFrom">Fecha desde</param>
    /// <param name="dateTo">Fecha hasta</param>
    /// <param name="leadSource">Clave del LeadSource</param>
    /// <param name="markets">Claves de Mercados</param>
    /// <param name="guPRs">Indica si se desean los PRs de huespedes</param>
    /// <param name="mbrPRs">Indica si se desean los PRs de socios</param>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Se agrego asincronía
    /// </history>
    public async static Task<List<PRAssigned>> GetPRsAssigned(DateTime dateFrom, DateTime dateTo, String leadSource, String markets, Boolean guPRs, Boolean mbrPRs)
    {
      List<PRAssigned> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_GetPRsAssigned(dateFrom, dateTo, leadSource, markets, guPRs, mbrPRs).OrderByDescending(c => c.peN).OrderByDescending(c => c.Assigned).ToList();
        }
      });
      return result;
    }

    #endregion

    #region GetGuestAssigned
    /// <summary>
    /// Obtiene los huespedes asignados
    /// </summary>
    /// <param name="dateFrom">Fecha desde</param>
    /// <param name="dateTo">Fecha hasta</param>
    /// <param name="leadSource">Clave del LeadSource</param>
    /// <param name="PRs">Claves de PRs</param>
    /// <param name="markets">Claves de Mercados</param>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 27/May/2016 Modified. Se agrego asincronía.
    /// </history>
    public async static Task<List<GuestAssigned>> GetGuestAssigned(DateTime dateFrom, DateTime dateTo, String leadSource, String PRs, String markets)
    {
      List<GuestAssigned> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_GetGuestsAssigned(dateFrom, dateTo, leadSource, PRs, markets).ToList();
        }
      });
      return result;
    }
    #endregion

    #region SaveGuestsPRAssign
    /// <summary>
    ///   Asigna huespedes a PR
    /// </summary>
    /// <param name="listguID">Claves de huespedes</param>
    /// <param name="idPR">Clave de PR</param>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 31/May/2016 Modified. Se agregó asincronía
    /// </history>
    public async static Task<int> SaveGuetsPRAssign(List<int> listguID, String idPR)
    {
      int res = 0;
      res = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var guests = dbContext.Guests.Where(Guest => listguID.Contains(Guest.guID)).ToList();
          guests.ForEach(g => g.guPRAssign = idPR);
          return dbContext.SaveChanges();
        }
      });
      return res;
    }
    #endregion

    #region SaveGuestsUnassign
    /// <summary>
    ///   Remueve asignaciones de huespedes
    /// </summary>
    /// <param name="listguID">Claves de huespedes</param>
    /// <param name="idPR">Clave de PR</param>
    /// <history>
    ///   [vku] 08/Mar/2016 Created
    ///   [vku] 31/May/2016 Modified. Se agregó asincronía
    /// </history>
    public async static Task<int> SaveGuestUnassign(List<int> listguID, String idPR)
    {
      int res = 0;
      res = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var guests = dbContext.Guests.Where(Guest => listguID.Contains(Guest.guID)).ToList();
          guests.ForEach(g => g.guPRAssign = null);
          return dbContext.SaveChanges();
        }
      });
      return res;
    }
    #endregion
  }
}
