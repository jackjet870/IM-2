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
    
    public partial class TourTimeBySalesRoomWeekDay
    {
        public string ttsr { get; set; }
        public byte ttDay { get; set; }
        public System.DateTime ttT { get; set; }
        public System.DateTime ttPickUpT { get; set; }
        public byte ttMaxBooks { get; set; }
    
        public virtual SalesRoom SalesRoom { internal get; set; }
    }
}
