using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRDisputeStatus
  {
    /// <summary>
    /// Obtiene la lista de estatus de disputas
    /// </summary>
    /// <param name="status">true = Activos | false = Inactivos</param>
    /// <returns>Task<List<DisputeStatus>></returns>
    /// <history>
    /// [erosado] 27/06/2016  Created
    /// </history>
    public async static Task<List<DisputeStatus>> GetDisputeStatus(bool status = true)
    {
      List<DisputeStatus> result = new List<DisputeStatus>();
      await Task.Run(() => {
        using (var db = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = (from x in db.DisputeStatusList where x.dsA == status select x).OrderBy(x => x.dsID);
          result = query.ToList();
        }
      });
      return result;
    }
  }
}
