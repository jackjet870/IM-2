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
    
    public partial class Language
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Language()
        {
            this.laMrMrs = "";
            this.laRoom = "";
            this.laMember = "";
            this.laDate = "";
            this.laTime = "";
            this.laName = "";
            this.laMaritalSt = "";
            this.laAge = "";
            this.laOccupation = "";
            this.laCountry = "";
            this.laAgency = "";
            this.laHotel = "";
            this.laPax = "";
            this.laDesposit = "";
            this.laGift = "";
            this.laLocation = "";
            this.laPR = "";
            this.laExtraInfo = "";
            this.Countries = new HashSet<Country>();
            this.InvitationsTexts = new HashSet<InvitationText>();
            this.MailOutTexts = new HashSet<MailOutText>();
            this.ProductsLegends = new HashSet<ProductLegend>();
            this.ReportsTexts = new HashSet<ReportText>();
        }
    
        public string laID { get; set; }
        public string laN { get; set; }
        public bool laA { get; set; }
        public string laMrMrs { get; set; }
        public string laRoom { get; set; }
        public string laMember { get; set; }
        public string laDate { get; set; }
        public string laTime { get; set; }
        public string laName { get; set; }
        public string laMaritalSt { get; set; }
        public string laAge { get; set; }
        public string laOccupation { get; set; }
        public string laCountry { get; set; }
        public string laAgency { get; set; }
        public string laHotel { get; set; }
        public string laPax { get; set; }
        public string laDesposit { get; set; }
        public string laGift { get; set; }
        public string laLocation { get; set; }
        public string laPR { get; set; }
        public string laExtraInfo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Country> Countries { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvitationText> InvitationsTexts { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailOutText> MailOutTexts { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductLegend> ProductsLegends { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportText> ReportsTexts { internal get; set; }
    }
}
