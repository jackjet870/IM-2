using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRBookingDeposits
  {
    #region GetBookingDeposits
    /// <summary>
    /// Obtiene los depósitos por invitado
    /// </summary>
    /// <param name="guestId">Identificador del invitado</param>
    /// <returns>lista de BookingDeposit</returns>
    /// <history>
    /// [lchairez] 13/03/2016 Created.
    /// </history>
    public static List<BookingDeposit> GetBookingDeposits(int guestId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.BookingDeposits.Include("CreditCardType").Include("PaymentType").Where(b => b.bdgu == guestId).ToList();
      }
    }

    #endregion

  }
}
