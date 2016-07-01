using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRPersonnelStatus
  {
    #region getPersonnelStatus
    /// <summary>
    /// obtiene la lista de status de personnel
    /// </summary>
    /// <returns>
    /// Devuelve una lista de tipo PersonnelStatus
    /// </returns>
    /// <history>
    /// [emoguel] created 14/06/2016
    /// </history>
    public static async Task<List<PersonnelStatus>> getPersonnelStatus()
    {
      List<PersonnelStatus> lstPersonnelsStatus = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.PersonnelStatusList.OrderBy(ps=>ps.psN).ToList();
        }
      });

      return lstPersonnelsStatus;
    } 
    #endregion
  }
}
