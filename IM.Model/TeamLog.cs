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
    
    public partial class TeamLog
    {
        public int tlID { get; set; }
        public System.DateTime tlDT { get; set; }
        public string tlChangedBy { get; set; }
        public string tlpe { get; set; }
        public string tlTeamType { get; set; }
        public string tlPlaceID { get; set; }
        public string tlTeam { get; set; }
    
        public virtual Personnel Personnel { internal get; set; }
        public virtual Personnel Personnel_ChangedBy { internal get; set; }
        public virtual TeamType TeamsType { internal get; set; }
    }
}
