using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRWarehouses
  {
    #region GetWarehousesByUser

    /// <summary>
    /// Obtiene los almacenes de un usuario
    /// </summary>
    /// <param name="user">Usuario </param>
    /// <param name="regions">Region o default('ALL') </param>
    /// <history>
    /// [wtorres]  07/Mar/2016 Created
    /// </history>
    public static List<WarehouseByUser> GetWarehousesByUser(string user, string regions)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetWarehousesByUser(user, regions).ToList();
      }
    }

    #endregion
  }
}
