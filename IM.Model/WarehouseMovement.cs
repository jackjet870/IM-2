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
    
    public partial class WarehouseMovement
    {
        public int wmID { get; set; }
        public System.DateTime wmD { get; set; }
        public string wmwh { get; set; }
        public int wmQty { get; set; }
        public string wmgi { get; set; }
        public string wmComments { get; set; }
        public string wmpe { get; set; }
    
        public virtual Gift Gift { internal get; set; }
        public virtual Personnel Personnel { internal get; set; }
        public virtual Warehouse Warehouse { internal get; set; }
    }
}
