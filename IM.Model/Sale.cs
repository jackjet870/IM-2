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
    
    public partial class Sale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sale()
        {
            this.Sales1 = new HashSet<Sale>();
            this.SalesLogs = new HashSet<SaleLog>();
            this.SalesLogs_Reference = new HashSet<SaleLog>();
            this.SalesmenChanges = new HashSet<SalesmanChange>();
            this.SalesSalesmen = new HashSet<SalesSalesman>();
        }
    
        public int saID { get; set; }
        public string saMembershipNum { get; set; }
        public Nullable<int> sagu { get; set; }
        public System.DateTime saD { get; set; }
        public string sast { get; set; }
        public Nullable<int> saReference { get; set; }
        public string saRefMember { get; set; }
        public bool saUpdated { get; set; }
        public string samt { get; set; }
        public string saLastName1 { get; set; }
        public string saFirstName1 { get; set; }
        public string saLastName2 { get; set; }
        public string saFirstName2 { get; set; }
        public decimal saOriginalAmount { get; set; }
        public Nullable<decimal> saNewAmount { get; set; }
        public decimal saGrossAmount { get; set; }
        public bool saProc { get; set; }
        public Nullable<System.DateTime> saProcD { get; set; }
        public bool saCancel { get; set; }
        public Nullable<System.DateTime> saCancelD { get; set; }
        public string salo { get; set; }
        public string sals { get; set; }
        public string sasr { get; set; }
        public string saPR1 { get; set; }
        public bool saSelfGen { get; set; }
        public string saPR2 { get; set; }
        public string saPR3 { get; set; }
        public string saPRCaptain1 { get; set; }
        public string saPRCaptain2 { get; set; }
        public string saPRCaptain3 { get; set; }
        public byte saLiner1Type { get; set; }
        public string saLiner1 { get; set; }
        public string saLiner2 { get; set; }
        public string saLinerCaptain1 { get; set; }
        public string saCloser1 { get; set; }
        public string saCloser2 { get; set; }
        public string saCloser3 { get; set; }
        public string saExit1 { get; set; }
        public string saExit2 { get; set; }
        public string saCloserCaptain1 { get; set; }
        public string saPodium { get; set; }
        public string saVLO { get; set; }
        public byte saLiner1P { get; set; }
        public byte saCloser1P { get; set; }
        public byte saCloser2P { get; set; }
        public byte saCloser3P { get; set; }
        public byte saExit1P { get; set; }
        public byte saExit2P { get; set; }
        public decimal saClosingCost { get; set; }
        public decimal saOverPack { get; set; }
        public string saComments { get; set; }
        public bool saDeposit { get; set; }
        public Nullable<System.DateTime> saProcRD { get; set; }
        public bool saByPhone { get; set; }
        public string samtGlobal { get; set; }
        public int saCompany { get; set; }
        public decimal saDownPayment { get; set; }
        public decimal saDownPaymentPercentage { get; set; }
        public decimal saDownPaymentPaid { get; set; }
        public decimal saDownPaymentPaidPercentage { get; set; }
        public decimal saOriginalAmountWithVAT { get; set; }
        public decimal saNewAmountWithVAT { get; set; }
        public decimal saGrossAmountWithVAT { get; set; }
        public Nullable<decimal> saVATRate { get; set; }
        public string saFTB1 { get; set; }
        public string saFTB2 { get; set; }
        public string saFTB3 { get; set; }
    
        public virtual Guest Guest { internal get; set; }
        public virtual LeadSource LeadSource { internal get; set; }
        public virtual Location Location { internal get; set; }
        public virtual MembershipType MembershipType { internal get; set; }
        public virtual MembershipType MembershipType_Global { internal get; set; }
        public virtual Personnel Personnel_Closer1 { internal get; set; }
        public virtual Personnel Personnel_Closer2 { internal get; set; }
        public virtual Personnel Personnel_Closer3 { internal get; set; }
        public virtual Personnel Personnel_CloserCaptain { internal get; set; }
        public virtual Personnel Personnel_Exit1 { internal get; set; }
        public virtual Personnel Personnel_Exit2 { internal get; set; }
        public virtual Personnel Personnel_Liner1 { internal get; set; }
        public virtual Personnel Personnel_Liner2 { internal get; set; }
        public virtual Personnel Personnel_LinerCaptain1 { internal get; set; }
        public virtual Personnel Personnel_Podium { get; set; }
        public virtual Personnel Personnel_PR1 { internal get; set; }
        public virtual Personnel Personnel_PR2 { internal get; set; }
        public virtual Personnel Personnel_PR3 { internal get; set; }
        public virtual Personnel Personnel_PRCaptain1 { internal get; set; }
        public virtual Personnel Personnel_PRCaptain2 { internal get; set; }
        public virtual Personnel Personnel_PRCaptain3 { internal get; set; }
        public virtual Personnel Personnel_VLO { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales1 { internal get; set; }
        public virtual Sale Sale1 { internal get; set; }
        public virtual SalesRoom SalesRoom { internal get; set; }
        public virtual SaleType SaleType { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs_Reference { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesmanChange> SalesmenChanges { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesSalesman> SalesSalesmen { internal get; set; }
        public virtual Personnel Personnel_FTB1 { get; set; }
        public virtual Personnel Personnel_FTB2 { get; set; }
        public virtual Personnel Personnel_FTB3 { get; set; }
    }
}
