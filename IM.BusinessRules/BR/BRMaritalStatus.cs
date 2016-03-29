using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRMaritalStatus
  {
    #region GetMaritalStratus
    /// <summary>
    /// Obtiene la lista de estados civiles
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// </history>
    public static List<MaritalStatus> GetMaritalStatus(int status= 1)
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        bool statusMS = Convert.ToBoolean(status);
        return dbContext.MaritalStatusList.Where(ms => ms.msA == statusMS).ToList();
      }
    }
    #endregion
  }
}
