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
    
    public partial class RptProductionByHotel
    {
        public string HotelID { get; set; }
        public Nullable<int> Books { get; set; }
        public Nullable<int> InOuts { get; set; }
        public Nullable<int> GrossBooks { get; set; }
        public Nullable<int> Shows { get; set; }
        public Nullable<decimal> ShowsFactor { get; set; }
        public Nullable<int> Sales_TOTAL { get; set; }
        public Nullable<decimal> SalesAmount_TOTAL { get; set; }
        public Nullable<int> Sales_PROC { get; set; }
        public Nullable<decimal> SalesAmount_PROC { get; set; }
        public Nullable<int> Sales_OOP { get; set; }
        public Nullable<decimal> SalesAmount_OOP { get; set; }
        public Nullable<int> Sales_CANCEL { get; set; }
        public Nullable<decimal> SalesAmount_CANCEL { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<decimal> CancelFactor { get; set; }
        public Nullable<decimal> ClosingFactor { get; set; }
        public Nullable<decimal> Efficiency { get; set; }
        public Nullable<decimal> AverageSale { get; set; }
    }
}
