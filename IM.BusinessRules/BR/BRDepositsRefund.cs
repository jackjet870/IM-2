using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections;
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
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          lstDepositsRefund = dbContext.USP_OR_GetDepositsRefund(Guest, RefundID, Folio, Name, Reservation, OutInv, PR, dateFrom, dateTo).ToList();
        }
      });

      return lstDepositsRefund;
    }
    #endregion

    #region GetDepositsRefund
    /// <summary>
    /// Obtiene el reporte Refund Letter
    /// </summary>
    /// <returns> List of IEnumerable. </returns>
    /// <history>
    /// [edgrodriguez] 14/Jul/2016 Created
    /// </history>
    public async static Task<List<IEnumerable>> GetRptRefundLetter(int RefundID)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
           var lstRptDepositLetter=dbContext.USP_OR_RptRefundLetter(RefundID)
          .MultipleResults()
          .With<RptRefundLetter>()
          .With<RptRefundLetter_BookingDeposit>()
          .GetValues();
          return ((lstRptDepositLetter[0] as List<RptRefundLetter>).Any()) ? lstRptDepositLetter : new List<IEnumerable>();
        }
      });
    }
    #endregion

    #region SaveDepositsRefund
    /// <summary>
    /// Genera una devolucion de depositos y marca los depositos como devueltos
    /// </summary>
    /// <param name="pGuestID"></param>
    /// <param name="pFolio"></param>
    /// <param name="pRefundType"></param>
    /// <param name="pDeposits"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 21/Julio/2016 Created
    /// </history>
    public async static Task SaveDepositsRefund(int pGuestID, int pFolio, string pRefundType, string pDeposits)
    {
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.USP_OR_SaveDepositsRefund(pGuestID, pFolio, pRefundType, pDeposits);
        }
      });
    } 
    #endregion

  }
}
