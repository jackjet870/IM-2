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
    
    public partial class SaleTypeCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaleTypeCategory()
        {
            this.stcA = true;
            this.SaleTypes = new HashSet<SaleType>();
        }
    
        public string stcID { get; set; }
        public string stcN { get; set; }
        public bool stcA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleType> SaleTypes { internal get; set; }
    }
}
