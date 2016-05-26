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
 public  class BRPayments
  {
    /// <summary>
    /// Obtiene la fecha de cierre de las ventas
    /// </summary>
    /// <param name="srId">ID Sale</param>
    /// <returns>Lista de Payment por Sale</returns>
    /// <hitory>
    /// [jorcanche] 20/05/2016
    /// </hitory>
    public static List<PaymentbySale> GetPaymentsbySale(int saID)
    {
      using (var dbCOntext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return (from gu in dbCOntext.Payments
                join pt in dbCOntext.PaymentTypes on gu.papt equals pt.ptID
                join cct in dbCOntext.CreditCardTypes on gu.pacc equals cct.ccID
                where gu.pasa == saID orderby gu.papt 
                select new PaymentbySale
                {
                  pasa = gu.pasa,
                  pacc = cct.ccN,
                  papt = pt.ptN
                }).ToList();
      }
    }
  }
}
