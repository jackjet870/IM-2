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
    
    public partial class LoginLog
    {
        public System.DateTime llID { get; set; }
        public string lllo { get; set; }
        public string llpe { get; set; }
        public string llPCName { get; set; }
    
        public virtual Location Location { internal get; set; }
        public virtual Personnel Personnel { internal get; set; }
    }
}
