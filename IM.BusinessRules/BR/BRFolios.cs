using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRFolios
  {
    #region ValidateFolioReservation
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leadSource"></param>
    /// <param name="folio"></param>
    /// <param name="guestId"></param>
    /// <returns></returns>
    /// <history>
    /// [lchairez] 16/03/2016 Created
    /// </history>
    public static bool ValidateFolioReservation(string leadSource, string folio, int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var res = dbContext.USP_OR_ValidateFolioReservation(leadSource, folio, guestId).SingleOrDefault();
        return String.IsNullOrEmpty(res);
      }
    }
    #endregion

    #region ValidateFolioInvitationOutside

    public static bool ValidateFolioInvitationOutside(int guestId, string serie, int numero)
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var res = dbContext.USP_OR_ValidateFolioInvitationOutside(serie, numero, guestId).SingleOrDefault();
        return String.IsNullOrEmpty(res);
      }
    }
    #endregion
  }
}
