//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Add
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Add()
        {
            this.Cars = new HashSet<Car>();
            this.Electronics = new HashSet<Electronic>();
            this.RealEstates = new HashSet<RealEstate>();
            this.AgriculturalVehicles = new HashSet<AgriculturalVehicle>();
            this.ConstructionVehicles = new HashSet<ConstructionVehicle>();
            this.PassengerVehicles = new HashSet<PassengerVehicle>();
            this.TransportationVehicles = new HashSet<TransportationVehicle>();
        }
    
        public int AddId { get; set; }
        public string CategoryId { get; set; }
        public string LocationId { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Mandal { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string SubCategory { get; set; }
        public string NearestArea { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Electronic> Electronics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RealEstate> RealEstates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgriculturalVehicle> AgriculturalVehicles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConstructionVehicle> ConstructionVehicles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PassengerVehicle> PassengerVehicles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransportationVehicle> TransportationVehicles { get; set; }
    }
}
