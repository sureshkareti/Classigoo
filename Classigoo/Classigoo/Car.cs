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
    
    public partial class Car
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Fuel { get; set; }
        public string KMDriven { get; set; }
        public string Description { get; set; }
        public string ImgUrlPrimary { get; set; }
        public string ImgUrlSeconday { get; set; }
        public string ImgUrlThird { get; set; }
        public string ImgUrlFourth { get; set; }
        public string Type { get; set; }
        public string SubCategory { get; set; }
        public string LocationId { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> AddId { get; set; }
        public string Location { get; set; }
    
        public virtual Add Add { get; set; }
    }
}
