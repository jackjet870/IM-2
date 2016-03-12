using IM.Model;
using System;
using System.Collections.Generic;
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
    /// </hystory>

    public static List<LeadSourceByUser> GetLeadSourcesByUser(string user, string programs, string regions)
    {
      using (var dbContext = new IMEntities())
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
      using (var dbContext = new IMEntities())
      {
        return dbContext.LeadSources.
          Where(ls => status.Equals(1) ? ls.lsA : status.Equals(2) ? !ls.lsA : true).
          OrderBy(ls => ls.lsN).ToList();
      }
    }

    #endregion GetLeadSources
  }
}