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
        }
    
        public int AddId { get; set; }
        public string CategoryId { get; set; }
        public string LocationId { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Electronic> Electronics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RealEstate> RealEstates { get; set; }
    }
}
