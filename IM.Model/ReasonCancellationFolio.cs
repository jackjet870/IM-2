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
    
    public partial class ReasonCancellationFolio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReasonCancellationFolio()
        {
            this.FoliosCxCCancellations = new HashSet<FolioCxCCancellation>();
            this.FoliosInvitationsOuthouseByPRCancellations = new HashSet<FolioInvitationOuthousePRCancellation>();
        }
    
        public string rcfID { get; set; }
        public string rcfN { get; set; }
        public bool rcfA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FolioCxCCancellation> FoliosCxCCancellations { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FolioInvitationOuthousePRCancellation> FoliosInvitationsOuthouseByPRCancellations { internal get; set; }
    }
}
