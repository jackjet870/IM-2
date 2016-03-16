using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRPrograms
  {
    #region getPrograms
    /// <summary>
    /// Obtiene una lista de Programas
    /// </summary>
    /// <returns>List<Program></returns>
    /// <history>
    /// [edgrodriguez] 07/03/2016 Created
    /// </history>
    public static List<Program> getPrograms()
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.Programs.ToList();
      }
    } 
    #endregion
  }
}
