using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRAccountingCodes
  {
    #region GetAccountingCode

    /// <summary>
    /// Obtiene el código contable de un guest.
    /// </summary>
    /// <param name="guestId">Guest ID</param>
    /// <param name="activity">Actividad</param>
    /// <history>
    /// [edgrodriguez] 15/10/2016  Created.
    /// </history>
    public async static Task<string> GetAccountingCode(int guestId,string activity)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.UFN_OR_GetAccountingCode(guestId,activity);
        }
      });
    }

    #endregion

    #region GetMissingAccountInfo

    /// <summary>
    /// Obtiene la razon de falta de codigo contable
    /// </summary>
    /// <param name="guestId">Guest ID</param>
    /// <param name="activity">Actividad</param>
    /// <history>
    /// [edgrodriguez] 15/10/2016  Created.
    /// </history>
    public async static Task<string> GetMissingAccountInfo(int guestId, string activity)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.UFN_OR_GetMissingAccountInfo(guestId, activity);
        }
      });
    }

    #endregion   
  }
}
