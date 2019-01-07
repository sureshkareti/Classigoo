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
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<AgriculturalVehicle> AgriculturalVehicles { get; set; }
        public virtual DbSet<ConstructionVehicle> ConstructionVehicles { get; set; }
        public virtual DbSet<PassengerVehicle> PassengerVehicles { get; set; }
        public virtual DbSet<TransportationVehicle> TransportationVehicles { get; set; }
        public virtual DbSet<RealEstate> RealEstates { get; set; }
        public virtual DbSet<Add> Adds { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
    
        public virtual int FillAds(string category, string subCategory, string state, string district, string mandal, string nearestArea, string title, string type, string status, Nullable<System.Guid> userId, Nullable<System.DateTime> createdDate, ObjectParameter addId)
        {
            var categoryParameter = category != null ?
                new ObjectParameter("Category", category) :
                new ObjectParameter("Category", typeof(string));
    
            var subCategoryParameter = subCategory != null ?
                new ObjectParameter("SubCategory", subCategory) :
                new ObjectParameter("SubCategory", typeof(string));
    
            var stateParameter = state != null ?
                new ObjectParameter("State", state) :
                new ObjectParameter("State", typeof(string));
    
            var districtParameter = district != null ?
                new ObjectParameter("District", district) :
                new ObjectParameter("District", typeof(string));
    
            var mandalParameter = mandal != null ?
                new ObjectParameter("Mandal", mandal) :
                new ObjectParameter("Mandal", typeof(string));
    
            var nearestAreaParameter = nearestArea != null ?
                new ObjectParameter("NearestArea", nearestArea) :
                new ObjectParameter("NearestArea", typeof(string));
    
            var titleParameter = title != null ?
                new ObjectParameter("Title", title) :
                new ObjectParameter("Title", typeof(string));
    
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var statusParameter = status != null ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(string));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(System.Guid));
    
            var createdDateParameter = createdDate.HasValue ?
                new ObjectParameter("CreatedDate", createdDate) :
                new ObjectParameter("CreatedDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("FillAds", categoryParameter, subCategoryParameter, stateParameter, districtParameter, mandalParameter, nearestAreaParameter, titleParameter, typeParameter, statusParameter, userIdParameter, createdDateParameter, addId);
        }
    }
}
