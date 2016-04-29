using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRShowProgramsCategories
  {
    #region GetShowProgramsCategories
    /// <summary>
    /// Obtiene registros del catalogo ShowProgramsCategories
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0.Inactivos | 1. todos</param>
    /// <param name="showProgramCategory">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo ShowProgramCategory</returns>
    /// <history>
    /// [emoguel] created 25/04/2016
    /// </history>
    public static List<ShowProgramCategory> GetShowProgramsCategories(int nStatus = -1, ShowProgramCategory showProgramCategory = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from sg in dbContext.ShowProgramsCategories
                    select sg;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(sk => sk.sgA == blnStatus);
        }

        if (showProgramCategory != null)//Filtro por ID
        {
          if (!string.IsNullOrWhiteSpace(showProgramCategory.sgID))
          {
            query = query.Where(sk => sk.sgID == showProgramCategory.sgID);
          }

          if (!string.IsNullOrWhiteSpace(showProgramCategory.sgN))//Filtro por descripción
          {
            query = query.Where(sk => sk.sgN.Contains(showProgramCategory.sgN));
          }
        }

        return query.OrderBy(sg => sg.sgN).ToList();
      }
    } 
    #endregion
  }
}
