using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRPeriods
  {
    #region GetPeriods
    /// <summary>
    /// Obtiene registros del catalogo Periods
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="period">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Periods</returns>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// [emoguel] modified 28/06/2016---> Se volvió async
    /// </history>
    public async static Task<List<Period>> GetPeriods(int nStatus = -1, Period period = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from pd in dbContext.Periods
                      select pd;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(pd => pd.pdA == blnStatus);
          }

          if (period != null)
          {
            if (!string.IsNullOrWhiteSpace(period.pdID))//Filtro por ID
            {
              query = query.Where(pd => pd.pdID == period.pdID);
            }

            if (!string.IsNullOrWhiteSpace(period.pdN))//Filtro por descripción
            {
              query = query.Where(pd => pd.pdN.Contains(period.pdN));
            }
          }

          return query.OrderBy(pd => pd.pdN).ToList();
        }
      });
    }
    #endregion
  }
}
