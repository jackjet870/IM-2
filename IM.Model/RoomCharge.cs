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
    
    public partial class RoomCharge
    {
        public string rhls { get; set; }
        public string rhFolio { get; set; }
        public int rhConsecutive { get; set; }
    
        public virtual LeadSource LeadSource { internal get; set; }
    }
}
