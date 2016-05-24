using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRPrograms
  {
    #region GetPrograms
    /// <summary>
    /// Obtiene una lista de Programas
    /// </summary>
    /// <returns>List<Program></returns>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// [edgrodriguez] 21/05/2016 Modified. El metodo se volvió asincrónico.
    /// </history>
    public async static Task<List<Program>> GetPrograms()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return await dbContext.Programs.ToListAsync();
      }
    } 
    #endregion
  }
}
