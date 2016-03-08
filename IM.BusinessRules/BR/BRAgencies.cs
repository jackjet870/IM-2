using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRAgencies
  {
    #region GetAgencies

    /// <summary>
    /// Obtiene el catalogo de agencias
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    public static List<AgencyShort> GetAgencies(int status)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetAgencies(Convert.ToByte(status)).ToList();
      }
    }

    #endregion
  }
}
