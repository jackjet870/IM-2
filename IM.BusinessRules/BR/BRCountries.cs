using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRCountries
  {
    #region GetCountries

    /// <summary>
    /// Obtiene el catalogo de paises 
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    public static List<CountryShort> GetCountries(int status)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetCountries(Convert.ToByte(status)).ToList();
      }
    }

    #endregion
  }
}
