using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRTeamsGuestServices
  {
    #region GetTeamsGuestServices
    /// <summary>
    /// Obtiene registros del catalogo TeamGuestServices
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="teamGuestServices">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo teamGuestServices</returns>
    /// <history>
    /// [emoguel] created 27/04/2016
    /// </history>
    public static List<TeamGuestServices> GetTeamsGuestServices(int nStatus = -1, TeamGuestServices teamGuestServices = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        var query = from tg in dbContext.TeamsGuestServices
                    select tg;
        
        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(tg => tg.tgA == blnStatus);
        }

        if (teamGuestServices != null)
        {
          if (!string.IsNullOrWhiteSpace(teamGuestServices.tglo))//Filtro por location
          {
            query = query.Where(tg => tg.tglo == teamGuestServices.tglo);
          }

          if(!string.IsNullOrWhiteSpace(teamGuestServices.tgID))//Filtro por ID
          {
            query = query.Where(tg => tg.tgID == teamGuestServices.tgID);
          }

          if(!string.IsNullOrWhiteSpace(teamGuestServices.tgN))//Filtro por descripción
          {
            query = query.Where(tg => tg.tgN.Contains(teamGuestServices.tgN));
          }

          if(!string.IsNullOrWhiteSpace(teamGuestServices.tgLeader))//Filtro por leader
          {
            query = query.Where(tg => tg.tgLeader == teamGuestServices.tgLeader);
          }
        }

        return query.OrderBy(tg => tg.tgN).ToList();
      }
    } 
    #endregion
  }
}
