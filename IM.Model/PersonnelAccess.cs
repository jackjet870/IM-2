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
    
    public partial class PersonnelAccess
    {
        public string plpe { get; set; }
        public string plLSSR { get; set; }
        public string plLSSRID { get; set; }
    
        public virtual Personnel Personnel { get; set; }
        public virtual PlaceType PlaceType { get; set; }
    }
}
