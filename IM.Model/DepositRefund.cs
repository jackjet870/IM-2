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
    
    public partial class DepositRefund
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DepositRefund()
        {
            this.BookingDeposits = new HashSet<BookingDeposit>();
        }
    
        public int drID { get; set; }
        public int drFolio { get; set; }
        public System.DateTime drD { get; set; }
        public int drgu { get; set; }
        public string drrf { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingDeposit> BookingDeposits { internal get; set; }
        public virtual RefundType RefundType { internal get; set; }
        public virtual Guest Guest { internal get; set; }
    }
}
