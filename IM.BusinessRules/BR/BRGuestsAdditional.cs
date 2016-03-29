using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Reglas de negocios de huespedes adicionales
  /// </summary>
  /// <history>
  /// [wtorres]  17/Mar/2016 Created
  /// </history>
  public class BRGuestsAdditional
  {
    #region GetGuestsAdditional

    /// <summary>
    /// Obtiene los huespedes adicionales de un huesped
    /// </summary>
    /// <param name="guestId">Clave del huesped</param>
    /// <history>
    /// [wtorres]  17/Mar/2016 Created
    /// </history>
    public static List<Guest> GetGuestsAdditional(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from g in dbContext.Guests
                from a in g.GuestsAdditional
                where g.guID == guestId
                select a).ToList();
      }
    }
    #endregion
  }
}



