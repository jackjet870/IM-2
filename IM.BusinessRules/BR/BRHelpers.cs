using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRHelpers
  {
    /// <summary>
    /// Obtiene la fecha y hora del servidor.
    /// </summary>
    /// <returns>DateTime</returns>
    /// <history>
    /// [edgrodriguez] 29/Feb/2016 Created
    /// </history>
    public static DateTime GetServerDate()
    {
      using (var dbContext = new Model.IMEntities())
      {
        var dQuery = dbContext.Database.SqlQuery<DateTime>("SELECT GETDATE()");
        return dQuery.AsEnumerable().First();
      }
    }
  }
}
