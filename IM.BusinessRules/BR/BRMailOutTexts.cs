using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public static class BRMailOutTexts
  {
   
    #region GetMailOutsText
    /// <summary>
    /// Obtiene los MailOutsText
    /// </summary>
    /// <param name="leadSourceID">lsID</param>
    /// <param name="languageID">laID</param>
    /// <param name="status">-1 Todos, 0 Inactivos, 1 Activos</param>
    /// <returns>List<MailOutText></returns>
    /// <history>
    /// [erosado] 14/04/2016  Created.
    /// [erosado] 05/07/2016  Modified. Se agregó async.
    /// </history>
    public async static Task<List<MailOutText>> GetMailOutTexts(string leadSourceID = null, string languageID = null, int status = -1)
    {
      List<MailOutText> result = new List<MailOutText>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from mot in dbContext.MailOutTexts
                      select mot;

          if (leadSourceID != null)
          {
            query = query.Where(mot => mot.mtls == leadSourceID);
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
          result = query.OrderBy(mot => mot.mtmoCode).ToList();
        }
      });

      return result;
    }
    #endregion

    #region UpdateRTFMailOutTexts
    /// <summary>
    /// Actualiza el RTF de un MailOutsText
    /// </summary>
    /// <param name="mot">MailOutText</param>
    /// <history>
    /// [erosado] 08/04/2016  Created
    /// [erosado] 05/07/2016  Modified. Se agregó Async.
    /// </history>
    public async static Task UpdateRTFMailOutTexts(MailOutText mot)
    {
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.Entry(mot).State = System.Data.Entity.EntityState.Modified;
          dbContext.SaveChanges();
        }
      });
    }
    #endregion

  }
}