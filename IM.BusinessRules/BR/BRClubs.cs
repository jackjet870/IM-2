using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRClubs
  {
    #region GetClubs
    /// <summary>
    /// Devuelve registros del catalogo Clubs
    /// </summary>
    /// <param name="club">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. registros inactivos | 1.Registros Activos</param>
    /// <returns>Devuelve Lista de Clubs</returns>
    /// <history>
    /// [emoguel] created 03/11/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// </history>
    public static List<Club> GetClubs(Club club=null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from cb in dbContext.Clubs
                    select cb;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(cb => cb.clA == blnEstatus);
        }

        if (club != null)//Si se tiene un objeto
        {
          if (club.clID > 0)//Filtro por ID
          {
            query = query.Where(cb => cb.clID == club.clID);
          }

          if (!string.IsNullOrWhiteSpace(club.clN))//Filtro por nombre
          {
            query = query.Where(cb => cb.clN == club.clN);
          }
        }

        return query.OrderBy(cb=>cb.clN).ToList();
      }
    } 
    #endregion
  }
}
