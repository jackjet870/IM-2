using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.BusinessRules.Classes;
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

    #region GetBookingDepositsByInvitation
    /// <summary>
    /// Obtiene los datos que se mostrarán en el grid de depositos de las invitaciones
    /// </summary>
    /// <param name="guestId">Identificador del invitado</param>
    /// <returns>Lista de la clase BookingDepositInvitation</returns>
    /// <history>
    /// [lchairez] 23/Mar/2016 Created
    /// </history>
    public static List<BookingDepositInvitation> GetBookingDepositsByInvitation(int guestId)
    {
      var dep = GetBookingDeposits(guestId);
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var depInivt = (from d in dep
                        join cu in dbContext.Currencies on d.bdcu equals cu.cuID
                        join cc in dbContext.CreditCardTypes on d.bdcc equals cc.ccID
                        join pt in dbContext.PaymentTypes on d.bdpt equals pt.ptID
                        select new BookingDepositInvitation
                        {
                          bdID = d.bdID,
                          bdgu = d.bdgu,
                          bdAmount = d.bdAmount,
                          bdAuth = d.bdAuth,
                          bdFolioCXC = d.bdFolioCXC,
                          bdExpD = d.bdExpD,
                          bdEntryDCXC = d.bdEntryDCXC,
                          bdcu = d.bdcu,
                          Currency = cu.cuN,
                          bdCardNum = d.bdCardNum,
                          bdcc = d.bdcc,
                          CreditCard = cc.ccN,
                          bdpc = d.bdpc,
                          bdpt = d.bdpt,
                          PaymentType = pt.ptN,
                          bdReceived = d.bdReceived,
                          bdUserCXC = d.bdUserCXC
                        }).ToList();

        return depInivt;
      }
    }
    #endregion
  }
}
