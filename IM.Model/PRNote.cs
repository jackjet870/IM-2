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
    
    public partial class PRNote
    {
        public int pngu { get; set; }
        public System.DateTime pnDT { get; set; }
        public string pnPR { get; set; }
        public string pnText { get; set; }
    
        public virtual Guest Guest { internal get; set; }
        public virtual Personnel Personnel { internal get; set; }
    }
}
