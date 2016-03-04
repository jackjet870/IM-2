using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRMailOutTexts
  {
    /// <summary>
    /// Obtiene el catalogo de MailOutText por la Clave del Lead Source
    /// </summary>
    /// <param name="leadSourceID">Clave del Lead Source</param>
    /// <param name="status">True-Activos, False-Inactivos</param>
    /// <returns></returns>
    /// <history>
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static List<Model.GetMailOutTextsByLeadSource> GetMailOutTextsByLeadSource(string leadSourceID, bool status)
    {
      using (var cn = new Model.IMEntities())
      {
        return cn.USP_OR_GetMailOutTextsByLeadSource(leadSourceID, status).ToList();
      }
    }
  }
}