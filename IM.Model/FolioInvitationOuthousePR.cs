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
    
    public partial class FolioInvitationOuthousePR
    {
        public int fipID { get; set; }
        public string fippe { get; set; }
        public string fipSerie { get; set; }
        public int fipFrom { get; set; }
        public int fipTo { get; set; }
    
        public virtual Personnel Personnel { internal get; set; }
    }
}
