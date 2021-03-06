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
    
    public partial class BookingDeposit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BookingDeposit()
        {
            this.bdAmount = 0m;
            this.bdReceived = 0m;
            this.bdpt = "CS";
            this.bdpc = "NR";
            this.bdExpD = "";
        }
    
        public int bdID { get; set; }
        public int bdgu { get; set; }
        public string bdcu { get; set; }
        public decimal bdAmount { get; set; }
        public decimal bdReceived { get; set; }
        public string bdpt { get; set; }
        public string bdpc { get; set; }
        public string bdcc { get; set; }
        public string bdCardNum { get; set; }
        public string bdAuth { get; set; }
        public Nullable<bool> bdRefund { get; set; }
        public Nullable<int> bddr { get; set; }
        public Nullable<int> bdFolioCXC { get; set; }
        public string bdUserCXC { get; set; }
        public Nullable<System.DateTime> bdEntryDCXC { get; set; }
        public string bdExpD { get; set; }
        public string bdds { get; set; }
        public Nullable<System.DateTime> bdD { get; set; }
    
        public virtual CreditCardType CreditCardType { internal get; set; }
        public virtual PaymentPlace PaymentPlace { internal get; set; }
        public virtual PaymentType PaymentType { internal get; set; }
        public virtual DepositRefund DepositsRefund { internal get; set; }
        public virtual Personnel Personnel { internal get; set; }
        public virtual DisputeStatus DisputeStatus { get; set; }
    }
}
