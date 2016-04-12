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

    #region GetMailOutTextsByLeadSourceAndLanguage
    /// <summary>
    /// Obtiene MailOutText por un leadSourceID y un LanguageID
    /// </summary>
    /// <param name="leadSourceID">lsID</param>
    /// <param name="languageID">laID</param>
    /// <returns>List<MailOutText></MailOutText></returns>
    /// <history>
    /// [erosado] Created 06/04/2016
    /// </history>
    public static List<MailOutText> GetMailOutTextsByLeadSourceAndLanguage(string leadSourceID, string languageID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from mot in dbContext.MailOutTexts where mot.mtls == leadSourceID && mot.mtla == languageID orderby mot.mtmoCode select mot).ToList();
      }
    }
    #endregion

    #region UpdateRTFMailOutTexts
    /// <summary>
    /// Actualiza el RTF de un MailOutsText
    /// </summary>
    /// <param name="mot">MailOutText</param>
    /// <history>
    /// [erosado] 08/04/2016  Created
    /// </history>
    public static void UpdateRTFMailOutTexts(MailOutText mot)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        MailOutText c = (from x in dbContext.MailOutTexts
                         where 
                         x.mtls == mot.mtls &&
                         x.mtla==mot.mtla &&
                         x.mtmoCode == mot.mtmoCode
                         select x).First();
        c.mtRTF = mot.mtRTF;
        dbContext.SaveChanges();
      }
    }
    #endregion
  }
}