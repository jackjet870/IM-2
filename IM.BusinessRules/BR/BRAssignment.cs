using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
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
    public static List<GuestUnassigned> GetGuestUnassigned(DateTime dateFrom, DateTime dateTo, String leadSource, String markets,  Boolean onlyAvail)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetGuestsUnassigned(dateFrom, dateTo, leadSource, markets, onlyAvail).OrderBy(o => o.guCheckInD).ToList();
      }
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
    public static List<RptAssignmentByPR> RptAssignmentByPR(DateTime dateFrom, DateTime dateTo, String leadSource, String markets, String PR)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_RptAssignmentByPR(dateFrom, dateTo, leadSource, markets, PR).ToList();
      }
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
    public static List<RptAssignment> RptAssignment(DateTime dateFrom, DateTime dateTo, String leadSource, String markets)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_RptAssignment(dateFrom, dateTo, leadSource, markets).ToList();
      }
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
    public static List<RptAssignmentArrivals> RptAssignmetArrivals(DateTime dateFrom, DateTime dateTo, String leadSource, String markets)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_RptAssignmentArrivals(dateFrom, dateTo, leadSource, markets).ToList();
      }
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
    public static List<PRAssigned> GetPRsAssigned(DateTime dateFrom, DateTime dateTo, String leadSource, String markets, Boolean guPRs, Boolean mbrPRs)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetPRsAssigned(dateFrom, dateTo, leadSource, markets, guPRs, mbrPRs).OrderByDescending(o => o.Assigned).ToList();

        ;
      }
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
    public static List<GuestAssigned> GetGuestAssigned(DateTime dateFrom, DateTime dateTo, String leadSource, String PRs, String markets)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetGuestsAssigned(dateFrom, dateTo, leadSource, PRs, markets).ToList();
      }
    }
    #endregion

    #region SaveGuestsPRAssign
    public static int SaveGuetsPRAssign(List<int> listguID, String idPR)
    {
      int res = 0;
      using (var dbContext = new IMEntities())
      {
        var guests = dbContext.Guests.Where(Guest => listguID.Contains(Guest.guID)).ToList();
        guests.ForEach(g => g.guPRAssign = idPR);
        res = dbContext.SaveChanges();
      }
      return res;
    }
    #endregion

    #region SaveGuestsUnassign
    public static int SaveGuestUnassign(List<int> listguID, String idPR)
    {
      int res = 0;
      using (var dbContext = new IMEntities())
      {
        var guests = dbContext.Guests.Where(Guest => listguID.Contains(Guest.guID)).ToList();
        guests.ForEach(g => g.guPRAssign = null);
        res = dbContext.SaveChanges();
      }
      return res;
    }
    #endregion
  }
}
