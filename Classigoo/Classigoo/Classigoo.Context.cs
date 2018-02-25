﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Classigoo
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ClassigooEntities : DbContext
    {
        public ClassigooEntities()
            : base("name=ClassigooEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_Adds> tbl_Adds { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Add> Adds { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Electronic> Electronics { get; set; }
        public virtual DbSet<RealEstate> RealEstates { get; set; }
    
        public virtual int FillAds(string categoryId, string locationId, Nullable<System.Guid> userId, ObjectParameter addId)
        {
            var categoryIdParameter = categoryId != null ?
                new ObjectParameter("CategoryId", categoryId) :
                new ObjectParameter("CategoryId", typeof(string));
    
            var locationIdParameter = locationId != null ?
                new ObjectParameter("LocationId", locationId) :
                new ObjectParameter("LocationId", typeof(string));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("FillAds", categoryIdParameter, locationIdParameter, userIdParameter, addId);
        }
    }
}
