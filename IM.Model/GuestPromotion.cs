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
    
    public partial class GuestPromotion
    {
        public int gpgu { get; set; }
        public int gpgr { get; set; }
        public string gpgi { get; set; }
        public string gpPromotion { get; set; }
        public string gpPromotionOpera { get; set; }
        public int gpQty { get; set; }
        public int gpBalance { get; set; }
        public System.DateTime gpD { get; set; }
        public string gpHotel { get; set; }
        public string gpFolio { get; set; }
        public bool gpNotified { get; set; }
    
        public virtual Gift Gift { internal get; set; }
        public virtual GiftsReceipt GiftsReceipt { internal get; set; }
        public virtual Guest Guest { internal get; set; }
    }
}
