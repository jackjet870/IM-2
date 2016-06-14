using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRDepositsRefund
  {

    #region GetDepositsRefund
    /// <summary>
    /// Obtiene los deposits refund
    /// </summary>
    /// <param name="Guest"></param>
    /// <param name="RefundID"></param>
    /// <param name="Folio"></param>
    /// <param name="Name"></param>
    /// <param name="Reservation"></param>
    /// <param name="OutInv"></param>
    /// <param name="PR"></param>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    public async static Task<List<DepositsRefund>> GetDepositsRefund(int Guest = 0, int RefundID = 0, string Folio = "", string Name = "", string Reservation = "", string OutInv = "", string PR = "", DateTime? dateFrom = null, DateTime? dateTo = null)
    {
      List<DepositsRefund> lstDepositsRefund = new List<DepositsRefund>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          lstDepositsRefund = dbContext.USP_OR_GetDepositsRefund(Guest, RefundID, Folio, Name, Reservation, OutInv, PR, dateFrom, dateTo).ToList();
        }
      });

      return lstDepositsRefund;
    } 
    #endregion

  }
}
