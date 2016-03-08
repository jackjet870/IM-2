using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRMarkets
  {
    #region GetMarkets

    /// <summary>
    /// Obtiene el catalogo de mercados
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    public static List<MarketShort> GetMarkets(int status)
    {
      using (var dbContext = new IM.Model.IMEntities())
      {
        return dbContext.USP_OR_GetMarkets(Convert.ToByte(status)).ToList();
      }
    }

    #endregion
  }
}
