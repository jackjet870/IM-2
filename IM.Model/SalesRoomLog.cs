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
    
    public partial class SalesRoomLog
    {
        public System.DateTime sqID { get; set; }
        public string sqChangedBy { get; set; }
        public string sqsr { get; set; }
        public System.DateTime sqGiftsRcptCloseD { get; set; }
        public System.DateTime sqCxCCloseD { get; set; }
        public Nullable<System.DateTime> sqShowsCloseD { get; set; }
        public Nullable<System.DateTime> sqMealTicketsCloseD { get; set; }
        public Nullable<System.DateTime> sqSalesCloseD { get; set; }
    
        public virtual Personnel Personnel { internal get; set; }
        public virtual SalesRoom SalesRoom { internal get; set; }
    }
}
