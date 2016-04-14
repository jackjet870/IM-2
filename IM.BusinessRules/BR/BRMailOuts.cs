using System;
using IM.Model;
using IM.Model.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRMailOuts
  {
    #region ProcessMailOuts

    /// <summary>
    /// Procesa los MailOuts por la  Clave del Lead Source
    /// </summary>
    /// <param name="leadSourceID">Clave del Lead Source</param>
    /// <param name="date"> Opcional:Fecha </param>
    /// <history>
    ///   [aalcocer] 24/02/2016 Created
    /// </history>
    public static void ProcessMailOuts(string leadSourceID, DateTime? date = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.spProcessMailOuts(leadSourceID, date);
      }
    }

    #endregion

    #region GetMailOuts
    /// <summary>
    /// Obtiene los Mail Outs
    /// </summary>
    /// <param name="leadSourceId">lsID o null si queremos todos</param>
    /// <param name="status">-1 Todos - 1 Activos - 0 Inactivos</param>
    /// <returns>List<MailOuts></MailOuts></returns>
    /// <history>
    /// [erosado] 14/04/2016  Created
    /// </history>
    public static List<MailOut> GetMailOuts(string leadSourceId = null, int status = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from mo in dbContext.MailOuts
                    select mo;
        if (leadSourceId != null)
        {
          query = query.Where(mo => mo.mols == leadSourceId);
        }
        if (status != -1)
        {
          bool blStatus = Convert.ToBoolean(status);
          query = query.Where(mo => mo.moA == blStatus);
        }
        return query.ToList();
      }
    }

    #endregion

  }
}