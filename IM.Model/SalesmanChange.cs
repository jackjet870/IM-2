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
    
    public partial class SalesmanChange
    {
        public System.DateTime schDT { get; set; }
        public int schsa { get; set; }
        public string schAuthorizedBy { get; set; }
        public string schMadeBy { get; set; }
        public string schro { get; set; }
        public byte schPosition { get; set; }
        public string schOldSalesman { get; set; }
        public string schNewSalesman { get; set; }
    
        public virtual Personnel Personnel_AuthorizedBy { internal get; set; }
        public virtual Personnel Personnel_MadeBy { internal get; set; }
        public virtual Personnel Personnel_NewSalesman { internal get; set; }
        public virtual Personnel Personnel_OldSalesman { internal get; set; }
        public virtual Role Role { internal get; set; }
        public virtual Sale Sale { internal get; set; }
    }
}
