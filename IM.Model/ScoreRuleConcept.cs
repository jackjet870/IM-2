//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IM.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ScoreRuleConcept
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ScoreRuleConcept()
        {
            this.ScoreRulesByLeadSourceDetails = new HashSet<ScoreRuleByLeadSourceDetail>();
            this.ScoreRulesDetails = new HashSet<ScoreRuleDetail>();
        }
    
        public byte spID { get; set; }
        public string spN { get; set; }
        public bool spA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScoreRuleByLeadSourceDetail> ScoreRulesByLeadSourceDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScoreRuleDetail> ScoreRulesDetails { get; set; }
    }
}