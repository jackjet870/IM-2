using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRScoreRulesDetails
  {
    #region GetScoreRulesDetails
    /// <summary>
    /// Devuelve registros del catalogo ScoreRuleDetail
    /// </summary>
    /// <param name="idScoreRule">id del score RUle</param>
    /// <returns>Devuelve una lista de ScoreRuleDetail</returns>
    /// <history>
    /// [emoguel] created 27/05/2016
    /// </history>
    public async static Task<List<ScoreRuleDetail>> GetScoreRulesDetails(int idScoreRule)
    {
      List<ScoreRuleDetail> lstScoreRuleDetail = new List<ScoreRuleDetail>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          lstScoreRuleDetail = dbContext.ScoreRulesDetails.Where(si => si.sisu == idScoreRule).ToList();
        }

      });

      return lstScoreRuleDetail;
    } 
    #endregion
  }
}
