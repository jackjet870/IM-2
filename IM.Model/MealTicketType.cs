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
    
    public partial class MealTicketType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MealTicketType()
        {
            this.MealTickets = new HashSet<MealTicket>();
        }
    
        public string myID { get; set; }
        public string myN { get; set; }
        public bool myWPax { get; set; }
        public decimal myPriceA { get; set; }
        public decimal myPriceM { get; set; }
        public decimal myCollaboratorWithCost { get; set; }
        public decimal myCollaboratorWithoutCost { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealTicket> MealTickets { internal get; set; }
    }
}
