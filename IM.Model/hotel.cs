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
    
    public partial class Hotel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hotel()
        {
            this.hoA = true;
            this.LeadSources = new HashSet<LeadSource>();
        }
    
        public string hoID { get; set; }
        public string hoGroup { get; set; }
        public string hoar { get; set; }
        public bool hoA { get; set; }
    
        public virtual Area Area { internal get; set; }
        public virtual HotelGroup HotelGroup { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeadSource> LeadSources { internal get; set; }
    }
}
