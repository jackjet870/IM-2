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
    /// [erosado] 04/08/2016 Modified. Se estandarizó el valor que retorna, y se agregó la bandera para que incluya CreditCardType y PaymentType
    /// </history>
    public async static Task<List<BookingDeposit>> GetBookingDeposits(int guestId, bool withCreditCardTypeAndPaymentType = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          if (withCreditCardTypeAndPaymentType)
          {
            return dbContext.BookingDeposits.Include("CreditCardType").Include("PaymentType").Where(b => b.bdgu == guestId).ToList();
          }
          else
          {
            return dbContext.BookingDeposits.Where(b => b.bdgu == guestId).ToList();
          }
        }
      });
    }
    #endregion

    #region GetBookingDepositsByGuest
    /// <summary>
    /// Obtiene los datos para la pantalla de deposits refund
    /// </summary>
    /// <param name="GuestID"></param>
    /// <param name="RefundID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    public async static Task<List<DepositToRefund>> GetDepositsToRefund(int GuestID, int RefundID = 0)
    {
      List<DepositToRefund> lstBookingDeposits = new List<DepositToRefund>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          lstBookingDeposits = dbContext.USP_OR_GetBookingDepositsByGuest(GuestID, RefundID).ToList();
        }
      });

      return lstBookingDeposits;
    } 
    #endregion

  }
}
