using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRMailOutTexts
  {
    #region GetMailOutTextsByLeadSource

    /// <summary>
    /// Obtiene el catalogo de MailOutText por la Clave del Lead Source
    /// </summary>
    /// <param name="leadSourceID">Clave del Lead Source</param>
    /// <param name="status">True-Activos, False-Inactivos</param>
    /// <returns></returns>
    /// <history>
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static List<MailOutTextByLeadSource> GetMailOutTextsByLeadSource(string leadSourceID, bool status)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetMailOutTextsByLeadSource(leadSourceID, status).ToList();
      }
    }

    #endregion
  }
}