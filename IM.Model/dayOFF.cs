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
    
    public partial class DayOff
    {
        public string dope { get; set; }
        public bool doMonday { get; set; }
        public bool doTuesday { get; set; }
        public bool doWednesday { get; set; }
        public bool doThursday { get; set; }
        public bool doFriday { get; set; }
        public bool doSaturday { get; set; }
        public bool doSunday { get; set; }
        public string doList { get; set; }
    
        public virtual Personnel Personnel { internal get; set; }
    }
}
