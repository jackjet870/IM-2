using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System;

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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetMailOutTextsByLeadSource(leadSourceID, status).ToList();
      }
    }

    #endregion

    #region GetMailOutsText
    /// <summary>
    /// Obtiene los MailOutsText
    /// </summary>
    /// <param name="leadSourceID">lsID</param>
    /// <param name="languageID">laID</param>
    /// <param name="status">-1 Todos, 0 Inactivos, 1 Activos</param>
    /// <returns>List<MailOutText></returns>
    public static List<MailOutText> GetMailOutTexts(string leadSourceID = null, string languageID = null, int status=-1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        var query = from mot in dbContext.MailOutTexts
                    select mot;

        if (leadSourceID != null)
        {
          query = query.Where(mot=> mot.mtls == leadSourceID);
        }
        if (languageID != null)
        {
          query = query.Where(mot => mot.mtla == languageID);
        }
        if (status != -1)
        {
          bool blstatus = Convert.ToBoolean(status);
          query = query.Where(mot => mot.mtA == blstatus);
        }
        return query.OrderBy(mot => mot.mtmoCode).ToList();
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.Entry(mot).State = System.Data.Entity.EntityState.Modified;
        int j = dbContext.SaveChanges();
      }
    }
    #endregion

  }
}