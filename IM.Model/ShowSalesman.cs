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
    
    public partial class ShowSalesman
    {
        public int shgu { get; set; }
        public string shpe { get; set; }
        public bool shUp { get; set; }
    
        public virtual Guest Guest { internal get; set; }
        public virtual Personnel Personnel { get; set; }
    }
}
