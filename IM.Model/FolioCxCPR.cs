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
    
    public partial class FolioCxCPR
    {
        public int fcpID { get; set; }
        public string fcppe { get; set; }
        public int fcpFrom { get; set; }
        public int fcpTo { get; set; }
    
        public virtual Personnel Personnel { get; set; }
    }
}