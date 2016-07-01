using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRScoreRulesConcepts
  {
    #region GetScoreRulesConcepts
    /// <summary>
    /// Obtiene registros del catalogo ScoresRulesConcepts
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="scoreRuleConcept">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo ScoreRuleConcept</returns>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// [emoguel] modified 26/05/2016 se volvió asincrono
    /// </history>
    public async static Task<List<ScoreRuleConcept>> GetScoreRulesConcepts(int nStatus=-1,ScoreRuleConcept scoreRuleConcept=null)
    {
      List<ScoreRuleConcept> lstScoreRuleConcept = new List<ScoreRuleConcept>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from sp in dbContext.ScoreRulesConcepts
                      select sp;
          if (nStatus != -1)//Filtro por Status
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(sp => sp.spA == blnStatus);
          }
          if (scoreRuleConcept != null)
          {
            if (scoreRuleConcept.spID > 0)//Filtro por ID
            {
              query = query.Where(sp => sp.spID == scoreRuleConcept.spID);
            }

            if (!string.IsNullOrWhiteSpace(scoreRuleConcept.spN))//Filtro por descripción
            {
              query = query.Where(sp => sp.spN.Contains(scoreRuleConcept.spN));
            }
          }
          lstScoreRuleConcept = query.OrderBy(sp => sp.spN).ToList();
        }
      });

      return lstScoreRuleConcept;
    }
    #endregion
  }
}
