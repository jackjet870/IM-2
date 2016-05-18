using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Classes
{
  public class ScoreByPRData
  {
    public List<RptScoreByPR> ScoreByPR { get; set; }

    public List<ScoreRuleConcept> ScoreRuleConcept { get; set; }
    public List<ScoreRule> ScoreRule { get; set; }

    public List<ScoreRuleDetail> ScoreRuleDetail { get; set; }
    public List<LeadSourceShort> LeadSourceShort { get; set; }

    public List<ScoreRuleByLeadSourceDetail> ScoreRuleByLeadSourceDetail { get; set; }

    public List<ScoreRuleType> ScoreRuleType { get; set; }
  }
}