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
    
    public partial class GiftsReceiptLog
    {
        public System.DateTime goID { get; set; }
        public int gogr { get; set; }
        public System.DateTime goD { get; set; }
        public string goHost { get; set; }
        public decimal goDeposit { get; set; }
        public decimal goBurned { get; set; }
        public string gocu { get; set; }
        public decimal goCXCPRDeposit { get; set; }
        public decimal goTaxiOut { get; set; }
        public decimal goTotalGifts { get; set; }
        public string goct { get; set; }
        public string gope { get; set; }
        public decimal goCXCGifts { get; set; }
        public decimal goCXCAdj { get; set; }
        public decimal goTaxiOutDiff { get; set; }
        public string goChangedBy { get; set; }
        public string gopt { get; set; }
        public byte goReimpresion { get; set; }
        public Nullable<byte> gorm { get; set; }
        public string goAuthorizedBy { get; set; }
        public Nullable<decimal> goAmountPaid { get; set; }
        public Nullable<int> goup { get; set; }
        public Nullable<System.DateTime> goCancelD { get; set; }
    
        public virtual ChargeTo ChargeTo { internal get; set; }
        public virtual Currency Currency { internal get; set; }
        public virtual GiftsReceipt GiftsReceipt { internal get; set; }
        public virtual PaymentType PaymentType { internal get; set; }
        public virtual Personnel Personnel_AuthorizedBy { internal get; set; }
        public virtual Personnel Personnel_ChangedBy { internal get; set; }
        public virtual Personnel Personnel_Host { internal get; set; }
        public virtual Personnel Personnel_PR { internal get; set; }
        public virtual ReimpresionMotive ReimpresionMotive { internal get; set; }
        public virtual UnderPaymentMotive UnderPaymentMotive { internal get; set; }
    }
}
