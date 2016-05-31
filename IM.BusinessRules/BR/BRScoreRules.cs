using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRScoreRules
  {
    #region GetScoreRules
    /// <summary>
    /// Obtiene registros de Score
    /// </summary>
    /// <param name="nStatus">Estaus de los registros</param>
    /// <param name="scoreRule">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Score RUle</returns>
    /// <history>
    /// [emoguel] created 26/05/2016
    /// </history>
    public static async Task<List<ScoreRule>> GetScoreRules(int nStatus = -1, ScoreRule scoreRule = null)
    {
      List<ScoreRule> lstScoreRule = new List<ScoreRule>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from su in dbContext.ScoreRules
                      select su;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(su => su.suA == blnStatus);
          }

          if (scoreRule != null)
          {
            if (scoreRule.suID > 0)//Filtro por ID
            {
              query = query.Where(su => su.suID == scoreRule.suID);
            }

            if (!string.IsNullOrWhiteSpace(scoreRule.suN))//Filtro por descripción
            {
              query = query.Where(su => su.suN.Contains(scoreRule.suN));
            }
          }

          lstScoreRule = query.OrderBy(su => su.suN).ToList();
        }
      });

      return lstScoreRule;
    }
    #endregion

    #region SaveScore
    /// <summary>
    /// Agrega|Actualiza un registro de ScoreRule
    /// </summary>
    /// <param name="score">Objeto a guardar</param>
    /// <param name="lstAdd">ScoreRuleDetail a agregar</param>
    /// <param name="lstDel">ScoreRuleDetail a eliminar</param>
    /// <param name="lstUpd">ScoreRuleDetail a actualizar</param>
    /// <param name="blnUpdate">True. Agrega | False. Actualiza</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se guardó | >0 Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    public static async Task<int> SaveScore(ScoreRule score,List<ScoreRuleDetail> lstAdd, List<ScoreRuleDetail> lstDel, List<ScoreRuleDetail> lstUpd, bool blnUpdate)
    {
      int nRes = 0;

      nRes=await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {          
          using (var transaccion = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
          try
          {
            #region Actualizar
            if (blnUpdate)
            {
              dbContext.Entry(score).State = System.Data.Entity.EntityState.Modified;
            }
            #endregion
            #region Insertar
            else
            {
              if (dbContext.ScoreRules.Where(su => su.suID == score.suID).FirstOrDefault() != null)
              {
                return -1;
              }
              else
              {
                dbContext.ScoreRules.Add(score);
              }
            }
            #endregion

            #region ScoreRulesDetail
            //Add
            lstAdd.ForEach(si =>
            {
              si.sisu = score.suID;
              dbContext.ScoreRulesDetails.Add(si);
            });

            //Upd
            lstUpd.ForEach(si =>
            {
              dbContext.Entry(si).State = System.Data.Entity.EntityState.Modified;
            });

            //del
            lstDel.ForEach(si => {
              dbContext.Entry(si).State = System.Data.Entity.EntityState.Deleted;
            });
            #endregion

              int nSave = dbContext.SaveChanges();
              transaccion.Commit();
              return nSave;
            }
            catch
            {
              transaccion.Rollback();
              return 0;
            }
          }
        }
      });

      return nRes;
    }
    #endregion
  }
}
