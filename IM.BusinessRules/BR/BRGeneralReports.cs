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
    #region GetRptPersonnel

    /// <summary>
    /// Obtiene el reporte de personal.
    /// </summary>
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
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
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
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
  }
}