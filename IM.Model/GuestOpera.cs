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
    
    public partial class GuestOpera
    {
        public int gogu { get; set; }
        public Nullable<int> goSourceID { get; set; }
        public Nullable<int> goTravelAgentID { get; set; }
        public Nullable<int> goGroupID { get; set; }
        public string goBlockCode { get; set; }
        public string goMarketCode { get; set; }
        public string goMarketGroup { get; set; }
        public string goSourceGroup { get; set; }
        public string goRegion { get; set; }
        public string goCountry { get; set; }
        public string goTerritory { get; set; }
    
        public virtual Guest Guest { get; set; }
    }
}