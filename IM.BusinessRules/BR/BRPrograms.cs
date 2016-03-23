using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    /// </history>
    public static List<Program> GetPrograms()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.Programs.ToList();
      }
    } 
    #endregion
  }
}
