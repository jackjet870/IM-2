using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRScoreRulesTypes
  {
    
    #region GetScoreRulesTypes
    /// <summary>
    /// Obtiene registros del catalogo ScoreRulesTypes
    /// </summary>
    /// <param name="nStatus">-1. todos | 0. Inactivos | 1. Activos</param>
    /// <param name="scoreRuleType">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo ScoreRuleType</returns>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    public static List<ScoreRuleType> GetScoreRulesTypes(int nStatus = -1, ScoreRuleType scoreRuleType = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from sy in dbContext.ScoreRulesTypes
                    select sy;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(sy => sy.syA == blnStatus);
        }

        if (scoreRuleType != null)
        {
          if (!string.IsNullOrWhiteSpace(scoreRuleType.syID))//Filtro por ID
          {
            query = query.Where(sy => sy.syID == scoreRuleType.syID);
          }

          if (!string.IsNullOrWhiteSpace(scoreRuleType.syN))//Filtro por descripción
          {
            query = query.Where(sy => sy.syN.Contains(scoreRuleType.syN));
          }
        }

        return query.OrderBy(sy => sy.syN).ToList();
      }
    }
    #endregion
  }
}
