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
    
    public partial class ScoreRuleByLeadSourceDetail
    {
        public string sjls { get; set; }
        public byte sjsp { get; set; }
        public decimal sjScore { get; set; }
    
        public virtual LeadSource LeadSource { internal get; set; }
        public virtual ScoreRuleConcept ScoreRuleConcept { internal get; set; }
    }
}
