using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System;

namespace IM.BusinessRules.BR
{
  public class BRGeneralReports
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
    #endregion

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
    #endregion

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

    #endregion

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

    #endregion

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

    #endregion

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

    #endregion


  }
}
