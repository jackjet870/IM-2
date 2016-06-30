using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
 public class BRPayments
  {
    #region GetPaymentsbySale
    /// <summary>
    /// Obtiene los pagos por venta
    /// </summary>
    /// <param name="srId">ID Sale</param>
    /// <returns>Lista de Payment por Sale</returns>
    /// <hitory>
    /// [jorcanche] 20/05/2016 created
    /// </hitory>
    public async static Task<List<Payment>> GetPaymentsbySale(int saID)
    {
      var res = new List<Payment>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          res = (from gu in dbContext.Payments
                 where gu.pasa == saID
                 orderby gu.papt
                 select gu).ToList();
        }
      });
      return res;
    }
    #endregion

    #region DeletePaymentsbySale
    /// <summary>
    /// Elimina uno o mas registros que contengan el Id Sale en la tabla Payments
    /// </summary>
    /// <param name="saleID">Idetificador de Sale</param>
    /// <history>
    /// [jorcanche]  created 24062016
    /// </history>
    public async static Task<int> DeletePaymentsbySale(int saleID)
    {
      int res = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var lstPayments = dbContext.Payments.Where(p => p.pasa == saleID);
          dbContext.Payments.RemoveRange(lstPayments);
          res =  dbContext.SaveChanges();
        }
      });
      return res;
    } 
    #endregion
  }
}
