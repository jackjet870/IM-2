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
    
    public partial class PersonnelPermission
    {
        public string pppe { get; set; }
        public string pppm { get; set; }
        public int pppl { get; set; }
    
        public virtual Permission Permission { internal get; set; }
        public virtual PermissionLevel PermissionLevel { internal get; set; }
        public virtual Personnel Personnel { internal get; set; }
    }
}
