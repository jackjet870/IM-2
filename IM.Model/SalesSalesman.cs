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
    
    public partial class SalesSalesman
    {
        public int smsa { get; set; }
        public string smpe { get; set; }
        public decimal smSaleAmountOwn { get; set; }
        public decimal smSaleAmountWith { get; set; }
        public bool smSale { get; set; }
    
        public virtual Personnel Personnel { internal get; set; }
        public virtual Sale Sale { internal get; set; }
    }
}
