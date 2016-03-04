using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRLanguages
  {
    /// <summary>
    /// Obtiene el catalogo de lenguajes
    /// </summary
    /// <param name="status">0- Sin filtro, 1-Activos, 2. Inactivos</param>
    /// <returns>List<Model.GetLanguages></returns>
    /// <history>
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static List<Model.GetLanguages> GetLanguages(int status)
    {
      using (var cn = new Model.IMEntities())
      {
        return cn.USP_OR_GetLanguages(Convert.ToByte(status)).ToList();
      }
    }
  }
}