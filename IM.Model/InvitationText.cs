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
    
    public partial class InvitationText
    {
        public string itls { get; set; }
        public string itla { get; set; }
        public string itRTF { get; set; }
        public string itRTFHeader { get; set; }
        public string itRTFFooter { get; set; }
    
        public virtual Language Language { internal get; set; }
        public virtual LeadSource LeadSource { internal get; set; }
    }
}
