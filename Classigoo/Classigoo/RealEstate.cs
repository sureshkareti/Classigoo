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
    
    public partial class RealEstate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Availability { get; set; }
        public string ListedBy { get; set; }
        public string Furnishing { get; set; }
        public string Bedrooms { get; set; }
        public string SquareFeets { get; set; }
        public string Squareyards { get; set; }
        public string Description { get; set; }
        public string TypeId { get; set; }
        public string SubCategoryId { get; set; }
        public string LocationId { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> AddId { get; set; }
        public string ImgUrlPrimary { get; set; }
        public string ImgUrlSeconday { get; set; }
        public string ImgUrlThird { get; set; }
        public string ImgUrlFourth { get; set; }
    
        public virtual Add Add { get; set; }
    }
}
