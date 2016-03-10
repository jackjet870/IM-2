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
    
    public partial class Agency
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agency()
        {
            this.Guests = new HashSet<Guest>();
            this.MealTickets = new HashSet<MealTicket>();
            this.Gifts = new HashSet<Gift>();
            this.LeadSources = new HashSet<LeadSource>();
        }
    
        public string agID { get; set; }
        public byte agum { get; set; }
        public string agmk { get; set; }
        public decimal agShowPay { get; set; }
        public decimal agSalePay { get; set; }
        public string agrp { get; set; }
        public bool agA { get; set; }
        public string agco { get; set; }
        public bool agList { get; set; }
        public bool agVerified { get; set; }
        public string agse { get; set; }
        public bool agIncludedTour { get; set; }
        public string agN { get; set; }
        public Nullable<int> agcl { get; set; }
    
        public virtual Club Club { get; set; }
        public virtual Country Country { get; set; }
        public virtual Market Market { get; set; }
        public virtual Rep Rep { get; set; }
        public virtual SegmentByAgency SegmentsByAgency { get; set; }
        public virtual UnavailableMotive UnavailMot { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealTicket> MealTickets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gift> Gifts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeadSource> LeadSources { get; set; }
    }
}