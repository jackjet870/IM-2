using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRPaymentTypes
  {
    #region GetPaymentTypes

    /// <summary>
    /// devuelve datos del catalogo paymentTypes
    /// </summary>
    /// <param name="nStatus"> -1. Todos | 0. registros inactivos | 1. registros activos</param>
    /// <returns>lista de tipo payment type</returns>
    /// <history>
    /// [lchairez] created 10/03/2016
    /// [emoguel] modified 06/04/2016 Se agregaron mas filtros
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async static Task<List<PaymentType>> GetPaymentTypes(int status = -1, PaymentType paymentType = null)
    {
      List<PaymentType> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from pt in dbContext.PaymentTypes
                      select pt;
          if (status != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(status);
            query = query.Where(pt => pt.ptA == blnStatus);
          }

          #region Filtros adicionales
          if (paymentType != null)
          {
            if (!string.IsNullOrWhiteSpace(paymentType.ptID))//filtro por ID
            {
              query = query.Where(pt => pt.ptID == paymentType.ptID);
            }

            if (!string.IsNullOrWhiteSpace(paymentType.ptN))//Filtro por descripcion
            {
              query = query.Where(pt => pt.ptN.Contains(paymentType.ptN));
            }
          }
          #endregion

          result = query.OrderBy(pt => pt.ptN).ToList();
        }
      });
      return result;
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.PaymentTypes.Where(p => p.ptID == paymentTypeId).SingleOrDefault();
      }
    }
    #endregion
  }
}
