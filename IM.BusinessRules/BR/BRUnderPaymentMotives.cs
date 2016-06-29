using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRUnderPaymentMotives
  {
    #region getUnderPaymentMotives
    /// <summary>
    /// Obtiene registros del catalogo UnderPaymentMotives -1-Todos | 0-Inactivos | 1-Activos
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="underPaymentMotive">Objeto con filtros actuales</param>
    /// <returns>Lista de tipo UnderPaymentMotive</returns>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// [emoguel] modified 28/06/2016 ---> Se volvió async
    /// </history>
    public async static Task<List<UnderPaymentMotive>> getUnderPaymentMotives(int nStatus = -1, UnderPaymentMotive underPaymentMotive = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from up in dbContext.UnderPaymentMotives
                      select up;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(up => up.upA == blnStatus);
          }

          if (underPaymentMotive != null)
          {
            if (underPaymentMotive.upID > 0)//Filtro por ID
            {
              query = query.Where(up => up.upID == underPaymentMotive.upID);
            }

            if (!string.IsNullOrWhiteSpace(underPaymentMotive.upN))//Filtro por descripción
            {
              query = query.Where(up => up.upN.Contains(underPaymentMotive.upN));
            }
          }
          return query.OrderBy(up => up.upN).ToList();
        }
      });
    } 
    #endregion
  }
}
