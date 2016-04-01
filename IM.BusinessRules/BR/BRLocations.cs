using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRLocations
  {
    #region GetLocationsByUser

    /// <summary>
    /// Obtiene las locaciones de un usuario
    /// </summary>
    /// <param name="user">Usuario </param>
    /// <param name="programs"> Programa o default('ALL') </param>
    /// <history>
    /// [wtorres]  07/Mar/2016 Created
    /// </history>
    public static List<LocationByUser> GetLocationsByUser(string user, string programs)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetLocationsByUser(user, programs).ToList();
      }
    }

    #endregion
  }
}
