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
    
    public partial class AgriculturalVehicle
    {
        public int Id { get; set; }
        public string SubCategory { get; set; }
        public string Company { get; set; }
        public string OtherCompany { get; set; }
        public Nullable<int> Price { get; set; }
        public string Description { get; set; }
        public string ImgUrlPrimary { get; set; }
        public string ImgUrlSeconday { get; set; }
        public string ImgUrlThird { get; set; }
        public string ImgUrlFourth { get; set; }
        public Nullable<int> AddId { get; set; }
    
        public virtual Add Add { get; set; }
    }
}
