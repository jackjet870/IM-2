using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRLanguages
  {
    #region GetLanguages

    /// <summary>
    /// Obtiene el catalogo de idiomas
    /// </summary
    /// <param name="status">0- Sin filtro, 1-Activos, 2. Inactivos</param>
    /// <returns>List<Model.GetLanguages></returns>
    /// <history>
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static List<LanguageShort> GetLanguages(int status)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetLanguages(Convert.ToByte(status)).ToList();
      }
    }

    #endregion
  }
}