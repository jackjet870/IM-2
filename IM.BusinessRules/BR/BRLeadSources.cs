using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRLeadSources
  {
    #region GetLeadSourcesByUser

    /// <summary>
    /// Obtiene los Lead Sources de un usuario
    /// </summary>
    /// <param name="user">Usuario </param>
    /// <param name="programs"> Programa o default('ALL') </param>
    /// <param name="regions">Region o default('ALL') </param>
    /// <returns>List<LeadSourceByUser></returns>
    /// <hystory>
    /// [erosado] 08/03/2016  created
    /// [aalcocer] 17/03/2016 Modified. Agregado parametros por default
    /// </hystory>

    public static List<LeadSourceByUser> GetLeadSourcesByUser(string user, string programs = "ALL", string regions = "ALL")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetLeadSourcesByUser(user, programs, regions).ToList();
      }
    }

    #endregion GetLeadSourcesByUser

    #region GetLeadSources

    /// <summary>
    /// Obtiene catalogo de Lead Sources .
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    /// <returns>List<LeadSource></returns>
    /// <history>
    /// [aalcocer] 09/03/2016 Created
    /// </history>
    public static List<LeadSource> GetLeadSources(int status = 0)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.LeadSources.
          Where(ls => status.Equals(1) ? ls.lsA : status.Equals(2) ? !ls.lsA : true).
          OrderBy(ls => ls.lsN).ToList();
      }
    }

    #endregion GetLeadSources


    #region GetOccupationLeadSources
    /// <summary>
    /// Obtiene el porcentaje de ocupacion de un Lead Source en una fecha determinada
    /// </summary>
    /// <param name="date">La fecha del cual se motrara el porcentaje de ese día</param>
    /// <param name="lS">EL hotel del cual mostrara su porcentaje</param>
    /// <returns>string</returns>
    /// <history>
    /// [jorcanche] 11/04/2016 created
    /// </history>
    public static string GetOccupationLeadSources(DateTime date, string lS)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return "Occupancy " + dbContext.USP_OR_Occupation(date, lS).Single();
      }
    } 
    #endregion

  }
}