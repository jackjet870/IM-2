using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRRefundTypes
  {
    #region GetRefundTypes
    /// <summary>
    /// Obtiene registros del catalogo refundTypes
    /// </summary>
    /// <param name="nStatus">-1 Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="refundType">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo refund Type</returns>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// [emoguel] modified 28/06/2016 --> Se volvió async
    /// </history>
    public async static Task<List<RefundType>> GetRefundTypes(int nStatus = -1, RefundType refundType = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from rf in dbContext.RefundTypes
                      select rf;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(rf => rf.rfA == blnStatus);
          }

          if (refundType != null)//Validamos si tiene un objeto
          {
            if (!string.IsNullOrWhiteSpace(refundType.rfID))//Filtro por ID
            {
              query = query.Where(rf => rf.rfID == refundType.rfID);
            }

            if (!string.IsNullOrWhiteSpace(refundType.rfN))//Filtro por estatus
            {
              query = query.Where(rf => rf.rfN.Contains(refundType.rfN));
            }
          }

          return query.OrderBy(rf => rf.rfN).ToList();
        }
      });
    }
    #endregion
  }
}
