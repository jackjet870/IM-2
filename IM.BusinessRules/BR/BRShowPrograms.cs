using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRShowPrograms
  {
    #region GetShowPrograms
    /// <summary>
    /// Obtiene registros del catalogo ShowPrograms
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1.Activos</param>
    /// <param name="showProgram">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo ShowProgram</returns>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    public static List<ShowProgram> GetShowPrograms(int nStatus, ShowProgram showProgram = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from sk in dbContext.ShowPrograms
                    select sk;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(sk => sk.skA == blnStatus);
        }

        if (showProgram != null)
        {
          if (!string.IsNullOrWhiteSpace(showProgram.skID))//Filtro por ID
          {
            query = query.Where(sk => sk.skID == showProgram.skID);
          }

          if (!string.IsNullOrWhiteSpace(showProgram.skN))//Filtro por descripción
          {
            query = query.Where(sk => sk.skN.Contains(showProgram.skN));
          }

          if (!string.IsNullOrWhiteSpace(showProgram.sksg))//Filtro por category
          {
            query = query.Where(sk => sk.sksg == showProgram.sksg);
          }
        }

        return query.OrderBy(sk => sk.skN).ToList();
      }
    } 
    #endregion
  }
}
