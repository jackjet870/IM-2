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
    
    public partial class FolioCxCCancellation
    {
        public int fccID { get; set; }
        public string fccpe { get; set; }
        public int fccFrom { get; set; }
        public int fccTo { get; set; }
        public string fccrcf { get; set; }
    
        public virtual Personnel Personnel { get; set; }
        public virtual ReasonCancellationFolio ReasonCancellationFolio { get; set; }
    }
}
