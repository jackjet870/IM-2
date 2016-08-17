using IM.Model;
using IM.Model.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public static class BRShows
  {
    #region GetValidateShow

    ///  <summary>
    ///  Valida que los datos de un show existan
    ///  </summary>
    ///  <param name="changedBy">Clave del usuario que esta haciendo el cambio</param>
    ///  <param name="password">Contraseña del usuario que esta haciendo el cambio</param>
    /// <param name="guest">guest</param>
    /// <history>
    ///  [aalcocer]  11/08/2016 Created.
    /// </history>
    public static async Task<ValidationData> GetValidateShow(string changedBy, string password, Guest guest)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_ValidateShow(changedBy, password, guest.gusr, guest.guag, guest.guco, guest.guPRInvit1, guest.guPRInvit2,
            guest.guPRInvit3, guest.guLiner1, guest.guLiner2, guest.guCloser1, guest.guCloser2, guest.guCloser3, guest.guExit1,
            guest.guExit2, guest.guPodium, guest.guVLO, guest.guEntryHost, guest.guGiftsHost, guest.guExitHost).FirstOrDefault();
        }
      });
    }

    #endregion GetValidateShow
  }
}