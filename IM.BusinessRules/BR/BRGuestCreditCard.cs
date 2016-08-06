using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGuestCreditCard
  {
    /// <summary>
    /// Obtiene las tarjetas de crédito del invitado
    /// </summary>
    /// <param name="guestId">Guest ID</param>
    /// <returns>Lista de Tarjetas de credito del invitado</returns>
    /// <history>
    /// [erosado] 04/08/2016  Created.
    /// </history>
    public static async Task<List<GuestCreditCard>> GetGuestCreditCard(int guestId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.GuestsCreditCards.Where(gc => gc.gdgu == guestId).ToList();
        }
      });
    }
  }
}
