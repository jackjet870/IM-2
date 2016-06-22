using IM.BusinessRules.Properties;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public static class BRGeneralReports
  {
    #region Processor General

    #region GetRptAgencies

    /// <summary>
    /// Obtiene los datos para el reporte Agencies.
    /// </summary>
    /// <returns> List of RptAgencies </returns>
    /// <history>
    /// [edgrodriguez] 20/Abr/2016 Created
    /// </history>
    public static async Task<List<RptAgencies>> GetRptAgencies()
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_RptAgencies().ToList();
        }
      });
    }

    #endregion GetRptAgencies

    #region GetRptPersonnel

    /// <summary>
    /// Obtiene el reporte de personal.
    /// </summary>
    /// <returns> List of RptPersonnel </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static async Task<List<RptPersonnel>> GetRptPersonnel()
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_RptPersonnel().ToList();
        }
      });
    }

    #endregion GetRptPersonnel

    #region GetRptGifts

    /// <summary>
    /// Obtiene el reporte de regalos.
    /// </summary>
    /// <returns> List of RptGifts </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static async Task<List<RptGifts>> GetRptGifts()
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_RptGifts().ToList();
        }
      });
    }

    #endregion GetRptGifts

    #region GetRptGiftsKardex

    /// <summary>
    /// Obtiene el reporte de regalos.
    /// </summary>
    /// <returns> List of RptGiftsKardex </returns>
    /// <history>
    /// [edgrodriguez] 07/Jun/2016 Created
    /// </history>
    public static async Task<List<RptGiftsKardex>> GetRptGiftsKardex(DateTime? dtmStart, DateTime? dtmEnd, string salesRoom, string gifts = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_IM_RptGiftsKardex(dtmStart, dtmEnd, salesRoom, gifts).ToList();
        }
      });
    }

    #endregion GetRptGiftsKardex

    #region GetRptLoginsLog

    /// <summary>
    /// Obtiene el reporte Logins Log
    /// </summary>
    /// <returns> List of RptGifts </returns>
    /// <history>
    /// [edgrodriguez] 27/Abr/2016 Created
    /// </history>
    public static async Task<List<RptLoginLog>> GetRptLoginsLog(DateTime dtmStart, DateTime dtmEnd, string location = "ALL", string pcname = "ALL", string personnel = "ALL")
    {
      List<RptLoginLog> result = new List<RptLoginLog>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_IM_RptLoginLog(dtmStart, dtmEnd, location, pcname, personnel).ToList();
        }
      });
      return result;
    }

    #endregion GetRptLoginsLog

    #region GetRptProductionByLeadSourceMarketMonthly

    /// <summary>
    /// Obtiene el reporte Production By Lead Source & Market(Monthly).
    /// </summary>
    /// <returns>List of RptProductionByLeadSourceMarketMonthly</returns>
    /// <history>
    /// [edgrodriguez] 21/Abr/2016 Created
    /// </history>
    public static async Task<List<RptProductionByLeadSourceMarketMonthly>> GetRptProductionByLeadSourceMarketMonthly(DateTime? dtmStart, DateTime? dtmEnd, EnumQuinellas quinellas, EnumExternalInvitation external, EnumBasedOnArrival basedOnArrival, string leadSources = "ALL", EnumProgram program = EnumProgram.All)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_RptProductionByLeadSourceMarketMonthly(dtmStart, dtmEnd, leadSources, EnumToListHelper.GetEnumDescription(program), Convert.ToBoolean(quinellas), Convert.ToInt32(external), Convert.ToBoolean(basedOnArrival)).ToList();
        }
      });
    }

    #endregion GetRptProductionByLeadSourceMarketMonthly

    #region GetRptProductionReferral

    /// <summary>
    /// Obtiene el reporte Production Referral.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 22/Abr/2016 Created
    /// </history>
    public static async Task<List<RptProductionReferral>> GetRptProductionReferral(DateTime? dtmStart, DateTime? dtmEnd)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_RptProductionReferral(dtmStart, dtmEnd).ToList();
        }
      });
    }

    #endregion GetRptProductionReferral

    #region GetRptReps

    /// <summary>
    /// Obtiene el reporte Reps.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/Abr/2016 Created
    /// </history>
    public static async Task<List<Rep>> GetRptReps()
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.Reps.ToList();
        }
      });
    }

    #endregion GetRptReps

    #region GetRptSalesByProgramLeadSourceMarket

    /// <summary>
    /// Obtiene el reporte Sales By Program,LeadSource & Market.
    /// </summary>
    /// <history>
    /// [edgrodriguez] 23/Abr/2016 Created
    /// </history>
    public static async Task<List<RptSalesByProgramLeadSourceMarket>> GetRptSalesByProgramLeadSourceMarket(DateTime? dtmStart, DateTime? dtmEnd)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_RptSalesByProgramLeadSourceMarket(dtmStart, dtmEnd).ToList();
        }
      });
    }

    #endregion GetRptSalesByProgramLeadSourceMarket

    #region GetRptWarehouseMovements

    /// <summary>
    /// Obtiene el reporte de Warehouse Movements.
    /// </summary>
    /// <returns> List of RptWarehouseMovements</returns>
    /// <history>
    /// [edgrodriguez] 26/Abr/2016 Created
    /// </history>
    public static async Task<List<RptWarehouseMovements>> GetRptWarehouseMovements(DateTime? dtmStart, DateTime? dtmEnd, string whs)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.sprptWhsMovs(dtmStart, dtmEnd, whs).ToList();
        }
      });
    }

    #endregion GetRptWarehouseMovements

    #endregion Processor General

    #region Inhouse

    #region GetRptArrivals

    /// <summary>
    /// Obtiene el reporte RptArrivals
    /// </summary>/// <param name="date">Fecha </param>
    /// <param name="leadSource">LeadSource </param>
    /// <param name="markets">Mercado </param>
    /// <param name="available">Available </param>
    /// <param name="contacted">Contacted</param>
    /// <param name="invited">Invited</param>
    /// <param name="onGroup">OnGroup</param>
    /// <history>
    /// [ecanul] 18/04/2016 Created
    /// </history>
    public static List<RptArrivals> GetRptArrivals(DateTime date, string leadSource, string markets, int available, int contacted, int invited, int onGroup)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptArrivals(date, leadSource, markets, available, contacted, invited, onGroup).ToList();
      }
    }

    #endregion GetRptArrivals

    #region GetRptAviables

    /// <summary>
    /// Obtiene el reporte RptAviables
    /// </summary>
    /// <param name="date">Fecha </param>
    /// <param name="leadSource">LeadSource </param>
    /// <param name="markets">Mercado </param>
    /// <param name="contacted">Contacted</param>
    /// <param name="invited">Invited</param>
    /// <param name="onGroup">OnGroup</param>
    /// <history>
    /// [ecanul] 18/04/2016 Created
    /// </history>
    public static List<RptAvailables> GetRptAviables(DateTime date, string leadSource, string markets, int contacted, int invited, int onGroup)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptAvailables(date, leadSource, markets, contacted, invited, onGroup).ToList();
      }
    }

    #endregion GetRptAviables

    #region GetRptPremanifest

    /// <summary>
    /// Obtiene Un reporte RptPremanifest
    /// </summary>
    /// <param name="date">Fecha</param>
    /// <param name="placeId">ID de Palace</param>
    /// <param name="markets">Mercado</param>
    /// <param name="onGroup">OnGroup
    /// 0. No en Grupo
    /// 1. En Grupo
    /// 2. Sin Filtro</param>
    /// <param name="salesRoom">ID de SalesRoom</param>
    /// <history>
    /// [ecanul] 18/04/2016 Created
    /// [edgrodriguez] 21/06/2016 Modified. Se aplico asincronia. Y se cambiaron algunas variales a opcionales.
    /// </history>
    public static async Task<List<RptPremanifest>> GetRptPremanifest(DateTime date, string placeId = "ALL", string markets = "ALL", int onGroup = 2, string salesRoom = "ALL")
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_RptPremanifest(date, placeId, salesRoom, markets, onGroup, false, false, BRHelpers.GetServerDate()).ToList();
        }
      });
    }

    #endregion GetRptPremanifest

    #region GetRptPremanifestWithGifts

    /// <summary>
    /// Obtiene un reporte RptPremanifestWithGifts
    /// </summary>
    /// <param name="date">Fecha</param>
    /// <param name="placeId">ID de Palace</param>
    /// <param name="markets">Mercado</param>
    /// <param name="onGroup">OnGroup</param>
    /// <param name="salesRoom">ID de SalesRoom</param>
    /// <history>
    /// [ecanul] 18/04/2016 Created
    /// [edgrodriguez] 21/06/2016 Modified. Se aplico asincronia. Y se cambiaron algunas variales a opcionales.
    /// </history>
    public static async Task<List<RptPremanifestWithGifts>> GetRptPremanifestWithGifts(DateTime date, string placeId= "ALL",string salesRoom = "ALL", bool multiLeadSource = false, bool regen=false,DateTime? currentDateTime=null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_RptPremanifestWithGifts(date, placeId, salesRoom, multiLeadSource, regen, currentDateTime).ToList();
        }
      });
    }

    #endregion GetRptPremanifestWithGifts 
    
    #endregion

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
    /// [aalcocer] 24/05/2016 Modified. Se agregó asincronía
    /// </history>
    public static async Task<List<RptProductionByAgencyMonthly>> GetRptProductionByAgencyMonthly(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> agencies,
      EnumQuinellas considerQuinellas = EnumQuinellas.NoQuinellas, EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.NoBasedOnArrival)
    {
      var result = new List<RptProductionByAgencyMonthly>();

      await Task.Run(() =>
      {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByAgencyMonthly_Timeout;
          result = dbContext.USP_OR_RptProductionByAgencyMonthly(dtmStart, dtmEnd, string.Join(",", agencies), Convert.ToBoolean(considerQuinellas), Convert.ToBoolean(basedOnArrival)).ToList();
      }
      });
      return result;
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
    /// [aalcocer] 24/05/2016 Modified. Se agregó asincronía
    /// </history>
    public static async Task<List<RptProductionByMember>> GetRptProductionByMembers(DateTime dtmStart, DateTime dtmEnd, IEnumerable<string> leadSources,
      IEnumerable<string> pRs = null, EnumProgram program = EnumProgram.All, string aplication = "", int company = 0, Club club = null,
      EnumOnlyWholesalers onlyWholesalers = EnumOnlyWholesalers.NoOnlyWholesalers, EnumQuinellas considerQuinellas = EnumQuinellas.NoQuinellas, 
      EnumBasedOnArrival basedOnArrival = EnumBasedOnArrival.NoBasedOnArrival)
    {
      var result = new List<RptProductionByMember>();

      if (pRs == null || !pRs.Any())
        pRs = new[] { "ALL" };

      await Task.Run(() =>
      {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_RptProductionByMember_Timeout;
          result = dbContext.USP_OR_RptProductionByMember(dtmStart, dtmEnd, string.Join(",", leadSources), string.Join(",", pRs),
          EnumToListHelper.GetEnumDescription(program), string.IsNullOrWhiteSpace(aplication) ? "ALL" : aplication,
          company, club?.clID, Convert.ToBoolean(onlyWholesalers), Convert.ToBoolean(considerQuinellas),
          Convert.ToBoolean(basedOnArrival)).ToList();
      }
      });
      return result;
    }

    #endregion GetRptProductionByMembers

    #endregion Processor Inhouse

    #region Outhouse
    /// <summary>
    /// Regresa un lista de tipo RptPremanifestOuthouse
    /// </summary>
    /// <param name="date"></param>
    /// <param name="leadSource"></param>
    ///<history>
    ///[jorcanche] created 27/04/2016
    ///</history>
    public static List<RptPremanifestOuthouse> GetRptPremanifestOutSide(DateTime date, string leadSource)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptPremanifestOutside(date, leadSource).ToList();
      }
    } 
    #endregion
  }
}