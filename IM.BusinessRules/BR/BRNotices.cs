using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRNotices
  {
    #region GetNotices
    /// <summary>
    /// Trae todas las noticias del día y del Hotel
    /// </summary>
    /// <param name="leadSource">Hotel en el que se cargo el  modulo</param>
    /// <param name="date">Fecha currente</param>
    /// <history>
    /// [jorcanche] created 18/04/2016 Created
    /// </history>
    public static List<NoticeShort> GetNotices(string leadSource, Nullable<System.DateTime> date)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetNotices(leadSource, date).ToList();        
      }
    }
    #endregion

  }
}