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
    
    public partial class MembershipType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MembershipType()
        {
            this.Sales = new HashSet<Sale>();
            this.Sales1 = new HashSet<Sale>();
            this.SalesLogs = new HashSet<SaleLog>();
        }
    
        public string mtID { get; set; }
        public string mtN { get; set; }
        public string mtGroup { get; set; }
        public bool mtA { get; set; }
        public Nullable<byte> mtLevel { get; set; }
        public decimal mtFrom { get; set; }
        public decimal mtTo { get; set; }
    
        public virtual MembershipGroup MembershipGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleLog> SalesLogs { get; set; }
    }
}
