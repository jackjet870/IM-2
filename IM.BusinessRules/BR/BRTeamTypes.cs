using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRTeamTypes
  {
    #region GetTeamTypes
    /// <summary>
    /// Obtiene registros del catalogo TeamTypes
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="teamType">Objeto con filtros adicionales</param>
    /// <returns>Lista tipo TeamType</returns>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// [emoguel] modified 28/06/2016---> Se volvió async
    /// </history>
    public async static Task<List<TeamType>> GetTeamTypes(int nStatus = -1, TeamType teamType = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from tt in dbContext.TeamsTypes
                      select tt;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(tt => tt.ttA == blnStatus);
          }

          if (teamType != null)
          {
            if (!string.IsNullOrWhiteSpace(teamType.ttID))
            {
              query = query.Where(tt => tt.ttID == teamType.ttID);
            }
            if (!string.IsNullOrWhiteSpace(teamType.ttN))
            {
              query = query.Where(tt => tt.ttN.Contains(teamType.ttN));
            }
          }
          return query.OrderBy(tt => tt.ttN).ToList();
        }
      });
    } 
    #endregion
  }
}
