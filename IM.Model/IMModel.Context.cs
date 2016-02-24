﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class IMEntities : DbContext
    {
        public IMEntities()
            : base("name=IMEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Guest> Guests { get; set; }
    
        public virtual ObjectResult<GetCountries> USP_OR_GetCountries(Nullable<byte> status)
        {
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(byte));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCountries>("USP_OR_GetCountries", statusParameter);
        }
    
        public virtual ObjectResult<GetAgencies> USP_OR_GetAgencies(Nullable<byte> status)
        {
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(byte));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAgencies>("USP_OR_GetAgencies", statusParameter);
        }
    
        public virtual ObjectResult<GetSalesRooms> USP_OR_GetSalesRooms(Nullable<byte> status)
        {
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(byte));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSalesRooms>("USP_OR_GetSalesRooms", statusParameter);
        }
    
        public virtual ObjectResult<GetMarkets> USP_OR_GetMarkets(Nullable<byte> status)
        {
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(byte));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetMarkets>("USP_OR_GetMarkets", statusParameter);
        }
    
        public virtual ObjectResult<GetLeadSourcesByUser> USP_OR_GetLeadSourcesByUser(string user, string programs, string regions)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var programsParameter = programs != null ?
                new ObjectParameter("Programs", programs) :
                new ObjectParameter("Programs", typeof(string));
    
            var regionsParameter = regions != null ?
                new ObjectParameter("Regions", regions) :
                new ObjectParameter("Regions", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetLeadSourcesByUser>("USP_OR_GetLeadSourcesByUser", userParameter, programsParameter, regionsParameter);
        }
    
        public virtual ObjectResult<GetSalesRoomsByUser> USP_OR_GetSalesRoomsByUser(string user, string regions)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var regionsParameter = regions != null ?
                new ObjectParameter("Regions", regions) :
                new ObjectParameter("Regions", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSalesRoomsByUser>("USP_OR_GetSalesRoomsByUser", userParameter, regionsParameter);
        }
    }
}
