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
    
    public partial class MarketSegment
    {
        public string mksID { get; set; }
        public string mksN { get; set; }
        public string mksCode { get; set; }
        public string mkspg { get; set; }
    
        public virtual Program Program { internal get; set; }
    }
}
