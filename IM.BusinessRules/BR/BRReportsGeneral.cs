using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using System.Text;
using System.Threading.Tasks;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Clase para el manejo de los BR de los Reportes
  /// </summary>
  /// <history>
  /// [ecanul] 18/04/2016 Created
  /// </history>
  public class BRReportsGeneral
  {
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

  }
}
