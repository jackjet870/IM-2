using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

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
    public static List<LeadSourceByUser> GetLeadSourcesByUser(string user, string programs, string regions)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetLeadSourcesByUser(user, programs, regions).ToList();
      }
    }

    #endregion
  }
}
