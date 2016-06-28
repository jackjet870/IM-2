using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Classes;

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
    public static List<Payment> GetPaymentsbySale(int saID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from gu in dbContext.Payments
                where gu.pasa == saID
                orderby gu.papt
                select gu).ToList();
      }
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
    public static int DeletePaymentsbySale(int saleID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var lstPayments = dbContext.Payments.Where(p => p.pasa == saleID);
        dbContext.Payments.RemoveRange(lstPayments);
        return dbContext.SaveChanges();
      }
    } 
    #endregion
  }
}
