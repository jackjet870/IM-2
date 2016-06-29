using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRReasonCancellationFolios
  {
    #region GetReasonCancellationFolios
    /// <summary>
    /// Obtiene registros del Catalogo ReasonCancellationFolios
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos  | 1.Activos</param>
    /// <param name="reasonCancellationFolio">objeto con filtros especiales</param>
    /// <returns>Lista de tipo ReasonCancellationFolio</returns>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    public async static Task<List<ReasonCancellationFolio>> GetReasonCancellationFolios(int nStatus = -1, ReasonCancellationFolio reasonCancellationFolio = null)
    {
      List<ReasonCancellationFolio> lstReasonCancellation = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
          {
            var query = from rcf in dbContext.ReasonsCancellationFolios
                        select rcf;

            if (nStatus != -1)//Filtro por estatus
          {
              bool blnStatus = Convert.ToBoolean(nStatus);
              query = query.Where(rcf => rcf.rcfA == blnStatus);
            }

            if (reasonCancellationFolio != null)//Verificamos que se tenga un objeto
          {
              if (!string.IsNullOrWhiteSpace(reasonCancellationFolio.rcfID))//Filtro por ID
            {
                query = query.Where(rcf => rcf.rcfID == reasonCancellationFolio.rcfID);
              }

              if (!string.IsNullOrWhiteSpace(reasonCancellationFolio.rcfN))//Filtro po descripción
            {
                query = query.Where(rcf => rcf.rcfN.Contains(reasonCancellationFolio.rcfN));
              }
            }
            return query.OrderBy(rcf => rcf.rcfN).ToList();
          }
        });
      return lstReasonCancellation;
    }

    #endregion
  }
}
