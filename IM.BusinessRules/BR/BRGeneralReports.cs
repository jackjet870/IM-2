using IM.BusinessRules.Properties;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public static class BRGeneralReports
  {
    #region Processor General

    #region GetRptAgencies

    /// <summary>
    /// Obtiene los datos para el reporte Agencies.
    /// </summary>
    /// <returns> List<RptAgencies> </returns>
    /// <history>
    /// [edgrodriguez] 20/Abr/2016 Created
    /// </history>
    public static List<RptAgencies> GetRptAgencies()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptAgencies().ToList();
      }
    }

    #endregion GetRptAgencies

    #region GetRptPersonnel

    /// <summary>
    /// Obtiene el reporte de personal.
    /// </summary>
    /// <returns> List<RptPersonnel> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptPersonnel> GetRptPersonnel()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptPersonnel().ToList();
      }
    }

    #endregion GetRptPersonnel

    #region GetRptGifts

    /// <summary>
    /// Obtiene el reporte de regalos.
    /// </summary>
    /// <returns> List<RptGifts> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptGifts> GetRptGifts()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptGifts().ToList();
      }
    }

    #endregion GetRptGifts

    #region GetRptLoginsLog

    /// <summary>
    /// Obtiene el reporte Logins Log
    /// </summary>
    /// <returns> List<RptGifts> </returns>
    /// <history>
    /// [edgrodriguez] 27/Abr/2016 Created
    /// </history>
    public static List<RptLoginLog> GetRptLoginsLog(DateTime dtmStart, DateTime dtmEnd, string location = "ALL", string pcname = "ALL", string personnel = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_IM_RptLoginLog(dtmStart, dtmEnd, location, pcname, personnel).ToList();
      }
    }

    #endregion GetRptLoginsLog

    #region GetRptProductionByLeadSourceMarketMonthly

    /// <summary>
    /// Obtiene el reporte Production By Lead Source & Market(Monthly).
    /// </summary>
    /// <returns>List<RptProductionByLeadSourceMarketMonthly></returns>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static List<RptProductionByLeadSourceMarketMonthly> GetRptProductionByLeadSourceMarketMonthly(DateTime? dtmStart, DateTime? dtmEnd, EnumQuinellas quinellas, EnumExternalInvitation external, EnumBasedOnArrival basedOnArrival, string leadSources = "ALL", string Program = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionByLeadSourceMarketMonthly(dtmStart, dtmEnd, leadSources, Program, Convert.ToBoolean(quinellas), Convert.ToInt32(external), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByLeadSourceMarketMonthly

    #region GetRptProductionReferral

    /// <summary>
    /// Obtiene el reporte Production Referral.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/Abr/2016 Created
    /// </history>
    public static List<RptProductionReferral> GetRptProductionReferral(DateTime? dtmStart, DateTime? dtmEnd)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptProductionReferral(dtmStart, dtmEnd).ToList();
      }
    }

    #endregion GetRptProductionReferral

    #region GetRptReps

    /// <summary>
    /// Obtiene el reporte Reps.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/Abr/2016 Created
    /// </history>
    public static List<Rep> GetRptReps()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.Reps.ToList();
      }
    }

    #endregion GetRptReps

    #region GetRptSalesByProgramLeadSourceMarket

    /// <summary>
    /// Obtiene el reporte Sales By Program,LeadSource & Market.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/Abr/2016 Created
    /// </history>
    public static List<RptSalesByProgramLeadSourceMarket> GetRptSalesByProgramLeadSourceMarket(DateTime? dtmStart, DateTime? dtmEnd)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptSalesByProgramLeadSourceMarket(dtmStart, dtmEnd).ToList();
      }
    }

    #endregion GetRptSalesByProgramLeadSourceMarket

    #region GetRptWarehouseMovements

    /// <summary>
    /// Obtiene el reporte de Warehouse Movements.
    /// </summary>
    /// <returns> List<RptWarehouseMovements> </returns>
    /// <history>
    /// [edgrodriguez] 26/Abr/2016 Created
    /// </history>
    public static List<RptWarehouseMovements> GetRptWarehouseMovements(DateTime? dtmStart, DateTime? dtmEnd, string whs)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.sprptWhsMovs(dtmStart, dtmEnd, whs).ToList();
      }
    }

    #endregion GetRptWarehouseMovements

    #endregion Processor General

    #region GetRptArrivals

    /// <summary>
    /// Obtiene el reporte RptArrivals
    /// </summary>/// <param name="Date">Fecha </param>
    /// <param name="LeadSource">LeadSource </param>
    /// <param name="Markets">Mercado </param>
    /// <param name="Available">Available </param>
    /// <param name="Contacted">Contacted</param>
    /// <param name="Invited">Invited</param>
    /// <param name="OnGroup">OnGroup</param>
    /// <history>
    /// [ecanul] 18/04/2016 Created
    /// </history>
    public static List<RptArrivals> GetRptArrivals(DateTime Date, string LeadSource, string Markets, int Available, int Contacted, int Invited, int OnGroup)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptArrivals(Date, LeadSource, Markets, Available, Contacted, Invited, OnGroup).ToList();
      }
    }

    #endregion GetRptArrivals

    #region GetRptAviables

    /// <summary>
    /// Obtiene el reporte RptAviables
    /// </summary>
    /// <param name="Date">Fecha </param>
    /// <param name="LeadSource">LeadSource </param>
    /// <param name="Markets">Mercado </param>
    /// <param name="Contacted">Contacted</param>
    /// <param name="Invited">Invited</param>
    /// <param name="OnGroup">OnGroup</param>
    /// <history>
    /// [ecanul] 18/04/2016 Created
    /// </history>
    public static List<RptAvailables> GetRptAviables(DateTime Date, string LeadSource, string Markets, int Contacted, int Invited, int OnGroup)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptAvailables(Date, LeadSource, Markets, Contacted, Invited, OnGroup).ToList();
      }
    }

    #endregion GetRptAviables

    #region GetRptPremanifest

    /// <summary>
    /// Obtiene Un reporte RptPremanifest
    /// </summary>
    /// <param name="Date">Fecha</param>
    /// <param name="PalaceID">ID de Palace</param>
    /// <param name="Markets">Mercado</param>
    /// <param name="OnGroup">OnGroup</param>
    /// <param name="SalesRoom">ID de SalesRoom</param>
    /// <history>
    /// [ecanul] 18/04/2016 Created
    /// </history>
    public static List<RptPremanifest> GetRptPremanifest(DateTime Date, string PalaceID, string Markets, int OnGroup, string SalesRoom = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptPremanifest(Date, PalaceID, SalesRoom, Markets, OnGroup, false, false, BRHelpers.GetServerDate()).ToList();
      }
    }

    #endregion GetRptPremanifest

    #region GetRptPremanifestWithGifts

    /// <summary>
    /// Obtiene un reporte RptPremanifestWithGifts
    /// </summary>
    /// <param name="Date">Fecha</param>
    /// <param name="PalaceID">ID de Palace</param>
    /// <param name="Markets">Mercado</param>
    /// <param name="OnGroup">OnGroup</param>
    /// <param name="SalesRoom">ID de SalesRoom</param>
    /// <history>
    /// [ecanul] 18/04/2016 Created
    /// </history>
    public static List<RptPremanifestWithGifts> GetRptPremanifestWithGifts(DateTime Date, string PalaceID, string Markets, int OnGroup, string SalesRoom = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptPremanifestWithGifts(Date, PalaceID, SalesRoom, false, false, BRHelpers.GetServerDate()).ToList();
      }
    }

    #endregion GetRptPremanifestWithGifts

    #region Processor Inhouse

    #region GetRptProductionByAgencyMonthly

    /// <summary>
    /// Devuelve los datos para el reporte de porcentaje de show por fecha de booking
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="agencies">Claves de agencias</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByAgencyMonthly"></list></returns>
    /// <history>
    /// [aalcocer] 21/04/2016 Created
    /// </history>
    public static List<RptProductionByAgencyMonthly> GetRptProductionByAgencyMonthly(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> agencies,
      EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgencyMonthly_Timeout;
        return dbContext.USP_OR_RptProductionByAgencyMonthly(dtmStart, dtmEnd, string.Join(",", agencies), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByAgencyMonthly

    #region GetRptProductionByMembers

    /// <summary>
    /// Devuelve los datos para el reporte de produccion por membresia
    /// </summary>
    /// <param name="dtmStart">Fecha desde</param>
    /// <param name="dtmEnd">Fecha hasta</param>
    /// <param name="leadSources">Claves de Lead Sources</param>
    /// <param name="pRs">Claves de PRs</param>
    /// <param name="program">Clave de programa</param>
    /// <param name="aplication">Clave de membresia</param>
    /// <param name="company">Clave de compania</param>
    /// <param name="club">Clave de club</param>
    /// <param name="onlyWholesalers">Indica si se desean solo mayoristas</param>
    /// <param name="considerQuinellas">Indica si se debe considerar quinielas</param>
    /// <param name="basedOnArrival">Indica si se debe basar en la fecha de llegada</param>
    /// <returns><list type="RptProductionByMember"></list></returns>
    /// <history>
    /// [aalcocer] 03/05/2016 Created
    /// </history>
    public static List<RptProductionByMember> GetRptProductionByMembers(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      IEnumerable<string> pRs = null, EnumProgram program = EnumProgram.All, string aplication = "", int company = 0, Club club = null,
      bool onlyWholesalers = false, EnumQuinellas considerQuinellas = EnumQuinellas.quNoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.boaNoBasedOnArrival)
    {
      if (pRs == null || !pRs.Any()) pRs = new[] { "ALL" };
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByMember_Timeout;
        return dbContext.USP_OR_RptProductionByMember(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", pRs),
          EnumToListHelper.GetEnumDescription(program), string.IsNullOrWhiteSpace(aplication) ? "ALL" : aplication,
          company, club?.clID, onlyWholesalers, Convert.ToBoolean(considerQuinellas),
          Convert.ToBoolean(basedOnArrival)).ToList();
      }
    }

    #endregion GetRptProductionByMembers

    #endregion Processor Inhouse

    /// <summary>
    /// Regresa un lista de tipo RptPremanifestOuthouse
    /// </summary>
    /// <param name="Date"></param>
    /// <param name="LeadSource"></param>
    ///<history>
    ///[jorcanche] created 27/04/2016
    ///</history>
    public static List<RptPremanifestOuthouse> GetRptPremanifestOutSide(DateTime Date, string LeadSource)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptPremanifestOutside(Date, LeadSource).ToList();
      }
    }
  }
}