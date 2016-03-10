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
    
    public partial class Configuration
    {
        public decimal ocDBVersion { get; set; }
        public string ocDBRevision { get; set; }
        public bool ocQs { get; set; }
        public bool ocRunTrans { get; set; }
        public System.DateTime ocTransDT { get; set; }
        public bool ocOneNightV { get; set; }
        public bool ocUseAgencyIDs { get; set; }
        public decimal ocEfficiencyTh { get; set; }
        public decimal ocShowFactorTh { get; set; }
        public decimal ocBookFactorTh { get; set; }
        public int ocShowsTh { get; set; }
        public bool ocDBMaint { get; set; }
        public byte ocWeekDayStart { get; set; }
        public System.DateTime ocStartD { get; set; }
        public string ocAdminUser { get; set; }
        public bool ocTwoNightV { get; set; }
        public byte ocWelcomeCopies { get; set; }
        public int ocTourTimesSchema { get; set; }
        public string ocBoss { get; set; }
        public bool ocRunTransferMemberships { get; set; }
        public Nullable<System.DateTime> ocTransferMembershipsStartD { get; set; }
        public Nullable<System.DateTime> ocTransferMembershipsEndD { get; set; }
        public decimal ocVATRate { get; set; }
        public Nullable<System.DateTime> ocInvitationsCloseD { get; set; }
    
        public virtual Personnel Personnel { get; set; }
        public virtual Personnel Personnel1 { get; set; }
        public virtual TourTimesSchema TourTimesSchema { get; set; }
    }
}