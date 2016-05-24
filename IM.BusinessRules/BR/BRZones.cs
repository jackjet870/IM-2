using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
    public class BRZones
  {
    #region GetZonesTransfer
    /// <summary>
    /// Obtiene las zonas de transferencia
    /// </summary>
    /// <returns></returns>
    /// <history>
    ///   [michan] 24/02/2016 Created
    /// </history>
    public async static Task<List<ZoneTransfer>> GetZonesTransfer()
    {
      List < ZoneTransfer > zoneTransfer  = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          zoneTransfer = dbContext.USP_OR_GetZonesTransfer().ToList();
        }
      });
      return zoneTransfer;
    }
    #endregion
  }
}
