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
    
    public partial class Contract
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contract()
        {
            this.cnA = true;
        }
    
        public string cnID { get; set; }
        public string cnN { get; set; }
        public bool cnA { get; set; }
        public int cnMinDaysTours { get; set; }
        public byte cnum { get; set; }
    
        public virtual UnavailableMotive UnavailableMotive { internal get; set; }
    }
}
