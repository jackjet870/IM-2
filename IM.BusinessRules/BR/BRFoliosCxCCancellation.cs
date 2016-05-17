using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRFoliosCxCCancellation
  {
    #region GetFoliossCxCCancellation
    /// <summary>
    /// Obtiene registros del catalogo FolioCxCCancellation
    /// </summary>
    /// <param name="prId">ID del PR asignado</param>
    /// <returns>Lista de tipo FolioCxCCancellation</returns>
    /// <history>
    /// [emoguel] created 05/05/2016
    /// </history>
    public static List<FolioCxCCancellation> GetFoliossCxCCancellation(string prId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.FoliosCxCCancellation.Where(fc => fc.fccpe == prId).ToList();
      }
    } 
    #endregion
  }
}
