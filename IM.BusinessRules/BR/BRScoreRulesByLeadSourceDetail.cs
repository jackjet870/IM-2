using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRScoreRulesByLeadSourceDetail
  {
    #region GetScoreRulesByLeadSourceDetail
    /// <summary>
    /// Obtiene registros del catalogo ScoreRulesByLeadSources
    /// </summary>
    /// <param name="idLS">Id del leadSource</param>
    /// <returns>Lista de tipo ScoreRuleByLeadSourceDetail</returns>
    /// <history>
    /// [emoguel] created 28/05/2016
    /// </history>
    public async static Task<List<ScoreRuleByLeadSourceDetail>> GetScoreRulesByLeadSourceDetail(string idLS)
    {
      List<ScoreRuleByLeadSourceDetail> lstScoreRules = new List<ScoreRuleByLeadSourceDetail>();

      await Task.Run(() =>
      {

        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          lstScoreRules = dbContext.ScoreRulesByLeadSourceDetails.Where(sj => sj.sjls == idLS).ToList();
        }
      });

      return lstScoreRules;
    } 
    #endregion
  }
}
