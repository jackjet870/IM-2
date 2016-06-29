using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRSourcePayments
  {
    #region GetSourcePayments
    /// <summary>
    /// Obtiene registro del catalogo SourcePayments
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="sourcePayment">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Source Payments</returns>
    /// <history>
    /// [emoguel] created 26/04/2016
    /// [emoguel] modified 28/06/2016 --> Se volvió async
    /// </history>
    public async static Task<List<SourcePayment>> GetSourcePayments(int nStatus = -1, SourcePayment sourcePayment = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from sb in dbContext.SourcePayments
                      select sb;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(sb => sb.sbA == blnStatus);
          }

          if (sourcePayment != null)
          {
            if (!string.IsNullOrWhiteSpace(sourcePayment.sbID))//Filtro por ID
            {
              query = query.Where(sb => sb.sbID == sourcePayment.sbID);
            }

            if (!string.IsNullOrWhiteSpace(sourcePayment.sbN))//Filtro por descripción
            {
              query = query.Where(sb => sb.sbN.Contains(sourcePayment.sbN));
            }
          }
          return query.OrderBy(sb => sb.sbN).ToList();
        }
      });
    }

    #endregion
  }
}
