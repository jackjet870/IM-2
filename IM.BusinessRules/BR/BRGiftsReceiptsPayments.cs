using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGiftsReceiptsPayments
  {

    #region GetGiftsReceiptPayments
    /// <summary>
    /// Consulta los pagos de un recibo de regalos
    /// </summary>
    /// <param name="receipt"> Clave del recibo de regalos </param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 08/04/2016 Created
    /// </history>
    public static List<GiftsReceiptPaymentShort> GetGiftsReceiptPayments(int receipt)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGiftsReceiptPayments(receipt).ToList();
      }
    } 
    #endregion

  }
}
