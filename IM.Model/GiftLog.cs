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
    
    public partial class GiftLog
    {
        public System.DateTime ggID { get; set; }
        public string ggChangedBy { get; set; }
        public string gggi { get; set; }
        public decimal ggPrice1 { get; set; }
        public decimal ggPrice2 { get; set; }
        public decimal ggPrice3 { get; set; }
        public decimal ggPrice4 { get; set; }
        public bool ggPack { get; set; }
        public string gggc { get; set; }
        public bool ggInven { get; set; }
        public bool ggA { get; set; }
        public bool ggWFolio { get; set; }
        public bool ggWPax { get; set; }
        public int ggO { get; set; }
        public decimal ggPriceAdults { get; set; }
        public decimal ggPriceMinors { get; set; }
        public decimal ggPriceExtraAdults { get; set; }
    
        public virtual Gift Gift { internal get; set; }
        public virtual Personnel Personnel { internal get; set; }
    }
}
