using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRPersonnelLog
  {
    #region GetPersonnelLog
    /// <summary>
    /// Obtiene el log de un personal
    /// </summary>
    /// <param name="personnelID">Id del personal a buscar su log</param>
    /// <returns>Lista de tipo GetPersonnelLog</returns>
    /// <history>
    /// [emoguel] 17/10/2016 created
    /// </history>
    public async static Task<List<GetPersonnelLog>> GetPersonnelLog(string personnelID)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_IM_GetPersonnelLog(personnelID, null, null).ToList();
        }
      });
    } 
    #endregion
  }
}
