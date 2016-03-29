using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRPaymentTypes
  {
    #region GetPaymentTypes

    /// <summary>
    /// devuelve datos del catalogo paymentTypes
    /// </summary>
    /// <param name="nStatus"> 0. registros inactivos | 1. registros activos</param>
    /// <returns>lista de tipo payment type</returns>
    /// <history>
    /// [lchairez] created 10/03/2016
    /// </history>
    public static List<PaymentType> GetPaymentTypes(int status)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        bool statusPaymentTypes = Convert.ToBoolean(status);
        return dbContext.PaymentTypes.Where(p => p.ptA == statusPaymentTypes).ToList();
      }
    }
    #endregion

    #region GetPaymentTypeId
    /// <summary>
    /// Obtiene un tipo de pago en específico
    /// </summary>
    /// <param name="paymentTypeId">Identificador del pago</param>
    /// <returns>PaymentType</returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// </history>
    public static PaymentType GetPaymentTypeId(string paymentTypeId)
    {
      using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.PaymentTypes.Where(p => p.ptID == paymentTypeId).SingleOrDefault();
      }
    }
    #endregion
  }
}
