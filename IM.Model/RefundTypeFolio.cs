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
    
    public partial class RefundTypeFolio
    {
        public string rtfrf { get; set; }
        public int rtfFolio { get; set; }
    
        public virtual RefundType RefundType { get; set; }
    }
}
