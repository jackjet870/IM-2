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
    
    public partial class RptGiftsSale
    {
        public string Program { get; set; }
        public int Receipt { get; set; }
        public System.DateTime Date { get; set; }
        public bool Cancel { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public string SalesRoom { get; set; }
        public string LeadSource { get; set; }
        public string PR { get; set; }
        public string PRN { get; set; }
        public string OutInvit { get; set; }
        public Nullable<int> GuestID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gift { get; set; }
        public string GiftN { get; set; }
        public string GiftSale { get; set; }
        public Nullable<int> Adults { get; set; }
        public Nullable<int> Minors { get; set; }
        public Nullable<int> ExtraAdults { get; set; }
        public Nullable<decimal> PriceUS { get; set; }
        public Nullable<decimal> PriceMX { get; set; }
        public Nullable<decimal> PriceCAN { get; set; }
        public Nullable<decimal> TotalToPay { get; set; }
        public Nullable<decimal> PaymentTotal { get; set; }
        public Nullable<decimal> Difference { get; set; }
        public string User { get; set; }
        public string UserN { get; set; }
        public string Comments { get; set; }
        public string ExchangeRate { get; set; }
    }
}
