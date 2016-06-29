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
    /// [emoguel] modified 09/06/2016-->Se volvió async
    /// </history>
    public async static Task<List<FolioCxCCancellation>> GetFoliossCxCCancellation(string prId)
    {
      List<FolioCxCCancellation> lstFolios = await Task.Run(() =>
         {
           using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
           {
             return dbContext.FoliosCxCCancellation.Where(fc => fc.fccpe == prId).ToList();
           }
         });
      return lstFolios;
    } 
    #endregion
  }
}
