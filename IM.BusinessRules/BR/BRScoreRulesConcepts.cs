using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    /// </history>
    public static List<ScoreRuleConcept> GetScoreRulesConcepts(int nStatus=-1,ScoreRuleConcept scoreRuleConcept=null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from sp in dbContext.ScoreRulesConcepts
                    select sp;
        if(nStatus!=-1)//Filtro por Status
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(sp => sp.spA == blnStatus);
        }
        if(scoreRuleConcept!=null)
        {
          if(scoreRuleConcept.spID>0)//Filtro por ID
          {
            query = query.Where(sp => sp.spID == scoreRuleConcept.spID);
          }

          if(!string.IsNullOrWhiteSpace(scoreRuleConcept.spN))//Filtro por descripción
          {
            query = query.Where(sp => sp.spN.Contains(scoreRuleConcept.spN));
          }
        }
        return query.OrderBy(sp => sp.spN).ToList();
      }
    }
    #endregion

    #region SaveScoreRuleConcept
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo ScoreRulesConcepts
    /// </summary>
    /// <param name="scoreRuleConcept">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Agrega | False. Actualiza</param>
    /// <returns>0. No se guardó | 1. Se guardó  | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 23/04/2016
    /// </history>
    public static int SaveScoreRuleConcept(ScoreRuleConcept scoreRuleConcept, bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region
        if(blnUpdate)
        {
          dbContext.Entry(scoreRuleConcept).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Add
        else
        {
          ScoreRuleConcept scoreRuleConceptVal = dbContext.ScoreRulesConcepts.Where(sp => sp.spID == scoreRuleConcept.spID).FirstOrDefault();
          if(scoreRuleConceptVal!=null)//Verificamos si existe un registro con el mismo ID
          {
            return -1;
          }
          else//Agregamos
          {
            dbContext.ScoreRulesConcepts.Add(scoreRuleConcept);
          }
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
