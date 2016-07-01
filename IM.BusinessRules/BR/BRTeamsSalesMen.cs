using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRTeamsSalesMen
  {
    #region GetTeamsSalesMen
    /// <summary>
    /// Obtiene registros del catalogo TeamsSalesmen
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="teamSalesMen">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo TeamSalesMen</returns>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    public static List<TeamSalesmen> GetTeamsSalesMen(int nStatus = -1, TeamSalesmen teamSalesMen = null)
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from ts in dbContext.TeamsSalesmen
                      select ts;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(ts => ts.tsA == blnStatus);
          }

          if (teamSalesMen != null)
          {
            if (!string.IsNullOrWhiteSpace(teamSalesMen.tssr))//Filtro por Sales Room
            {
              query = query.Where(ts => ts.tssr == teamSalesMen.tssr);
            }
            if (!string.IsNullOrWhiteSpace(teamSalesMen.tsID))//Filtro por ID
            {
              query = query.Where(ts => ts.tsID == teamSalesMen.tsID);
            }

            if (!string.IsNullOrWhiteSpace(teamSalesMen.tsN))//Filtro por descripción
            {
              query = query.Where(ts => ts.tsN.Contains(teamSalesMen.tsN));
            }

            if (!string.IsNullOrWhiteSpace(teamSalesMen.tsLeader))//Filtro por Lide
            {
              query = query.Where(ts => ts.tsLeader == teamSalesMen.tsLeader);
            }
          }
          return query.OrderBy(ts => ts.tsN).ToList();
        }
    } 
    #endregion
  }
}
