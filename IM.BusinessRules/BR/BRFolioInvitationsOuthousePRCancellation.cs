using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRFolioInvitationsOuthousePRCancellation
  {
    #region GetFoliosCancellation
    /// <summary>
    /// Obtiene registros del catalog FolioInvitationOuthousePRCancellation
    /// con un Personnel especiífico
    /// </summary>
    /// <param name="idPr">id del pr a buscar</param>
    /// <returns>Lusta de tipo FolioInvitationOuthousePRCancellation</returns>
    /// <history>
    /// [emoguel] created 09/05/2016
    /// </history>
    public static List<FolioInvitationOuthousePRCancellation> GetFoliosCancellation(string idPr)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.FoliosInvitationsOuthousePRCancellation.Where(fic => fic.ficpe == idPr).OrderBy(fic => fic.ficID).ToList();
      }
    } 
    #endregion
  }
}
