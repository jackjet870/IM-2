using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRScoreRulesByLeadSource
  {
    #region GetScoreRuleByLeadSource
    /// <summary>
    /// Obtiene registros del catalogo ScoreRuleByLeadSource
    /// </summary>
    /// <returns>Lista de tipo ScoreRuleByLeadSource</returns>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    public async static Task<List<ScoreRuleByLeadSource>> GetScoreRuleByLeadSource(string idScoreRule="")
    {
      List<ScoreRuleByLeadSource> lstScoreRulesByLS = new List<ScoreRuleByLeadSource>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from sb in dbContext.ScoreRulesByLeadSources.Include("LeadSource")
                      select sb;

          if(!string.IsNullOrWhiteSpace(idScoreRule))
          {
            query = query.Where(sb => sb.sbls == idScoreRule);
          }
          lstScoreRulesByLS = query.OrderBy(sb => sb.sbls).ToList();            
        }
      });

      return lstScoreRulesByLS;
    }
    #endregion

    #region SaveScoreRuleByLeadSource
    /// <summary>
    /// Agrega un scoreRuleByLeadSource
    /// Agrega|Elimina details
    /// </summary>
    /// <param name="score">Objeto a guradar</param>
    /// <param name="lstAdd">Detalles a agregar</param>
    /// <param name="lstDel">Detalles a eliminar</param>
    /// <param name="lstUpd">Detalles a actualizar</param>
    /// <param name="blnUpdate">False. inserta un nuevo score Rule</param>
    /// <returns>-1. Existe un Rule con el mismo ID | 0. No se pudó guardar | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 30/05/2016
    /// </history>
    public static async Task<int> SaveScoreRuleByLeadSource(ScoreRuleByLeadSource score, List<ScoreRuleByLeadSourceDetail> lstAdd, List<ScoreRuleByLeadSourceDetail> lstDel,
      List<ScoreRuleByLeadSourceDetail> lstUpd, bool blnUpdate)
    {
      int nRes = 0;
     nRes= await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region Update
              if (!blnUpdate)
              {
                if (dbContext.ScoreRulesByLeadSources.Where(sb => sb.sbls == score.sbls).FirstOrDefault() != null)
                {
                  return -1;
                }
                else
                {
                  dbContext.ScoreRulesByLeadSources.Add(score);
                }
              }
              #endregion

              #region ScoreRulesDetail
              //Add
              lstAdd.ForEach(sj =>
              {
                sj.sjls = score.sbls;
                dbContext.ScoreRulesByLeadSourceDetails.Add(sj);
              });

              //Upd
              lstUpd.ForEach(sj =>
              {
                dbContext.Entry(sj).State = System.Data.Entity.EntityState.Modified;
              });

              //del
              lstDel.ForEach(sj => {
                dbContext.Entry(sj).State = System.Data.Entity.EntityState.Deleted;
              });
              #endregion

              int nSave = dbContext.SaveChanges();
              transacction.Commit();
              return nSave;
            }
            catch
            {
              transacction.Rollback();
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
