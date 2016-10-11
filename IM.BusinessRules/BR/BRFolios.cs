using IM.BusinessRules.Properties;
using IM.Model;
using IM.Model.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

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
    /// [erosado] 10/10/2016  Modified. Se agrego Async Await
    /// </history>
    public static async Task<bool> ValidateFolioReservation(string leadSource, string folio, int guestId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var res = dbContext.USP_OR_ValidateFolioReservation(leadSource, folio, guestId).SingleOrDefault();
          return string.IsNullOrEmpty(res);
        }
      });
    }

    #endregion ValidateFolioReservation

    #region ValidateFolioInvitationOutside

    /// <summary>
    /// Valida que el folio de invitacion outhouse exista en el catalogo y que no haya sido utilizado en otra invitacion
    /// </summary>
    /// <param name="guestId"></param>
    /// <param name="serie"></param>
    /// <param name="numero"></param>
    /// <returns>bool</returns>
    /// <history>
    /// [lchairez] 29/03/2016 Created
    /// [aalcocer] 12/08/2016. Modified. Se agrega  TimeOut
    /// [erosado] 31/08/2016  Modified. Ahora devuelve el mensaje de error.
    /// </history>
    public static string ValidateFolioInvitationOutside(int guestId, string serie, int numero)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.Database.CommandTimeout = Settings.Default.USP_OR_ValidateFolioInvitationOutside_Timeout;
        return dbContext.USP_OR_ValidateFolioInvitationOutside(serie, numero, guestId).FirstOrDefault();
      }
    }

    #endregion ValidateFolioInvitationOutside
  }
}