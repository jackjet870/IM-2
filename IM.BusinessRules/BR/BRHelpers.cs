using System;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRHelpers
  {
    #region GetServerDate

    /// <summary>
    /// Obtiene la fecha y hora del servidor.
    /// </summary>
    /// <returns>DateTime</returns>
    /// <history>
    /// [edgrodriguez] 29/Feb/2016 Created
    /// </history>
    public static DateTime GetServerDate()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var dQuery = dbContext.Database.SqlQuery<DateTime>("SELECT GETDATE()");
        return dQuery.AsEnumerable().First();
      }
    }

    #endregion
  }
}
