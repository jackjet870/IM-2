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
    
    public partial class RptSalesByLocationMonthly
    {
        public string Location { get; set; }
        public Nullable<bool> LocationTotal { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public string MonthN { get; set; }
        public Nullable<int> Shows { get; set; }
        public Nullable<int> Sales { get; set; }
        public Nullable<decimal> SalesAmountTotal { get; set; }
        public Nullable<decimal> SalesAmountCancel { get; set; }
        public Nullable<decimal> SalesAmount { get; set; }
        public Nullable<decimal> ClosingFactor { get; set; }
        public Nullable<decimal> Efficiency { get; set; }
        public Nullable<decimal> AverageSale { get; set; }
    }
}
