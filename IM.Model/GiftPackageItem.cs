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
    
    public partial class GiftPackageItem
    {
        public string gpPack { get; set; }
        public string gpgi { get; set; }
        public int gpQty { get; set; }
    
        public virtual Gift GiftItem { get; set; }
        public virtual Gift GiftPackage { internal get; set; }
    }
}
