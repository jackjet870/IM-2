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
    
    public partial class TeamGuestServices
    {
        public string tglo { get; set; }
        public string tgID { get; set; }
        public string tgN { get; set; }
        public string tgLeader { get; set; }
        public bool tgA { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual Personnel Personnel { get; set; }
    }
}