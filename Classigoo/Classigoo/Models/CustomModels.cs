using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classigoo
{
  

    public class ParentCategory
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

       // public int Id { get; set; }
        //public string Title { get; set; }
        //public string Price { get; set; }
        //public string Description { get; set; }
        //public string ImgUrlPrimary { get; set; }
        //public string ImgUrlSeconday { get; set; }
        //public string ImgUrlThird { get; set; }
        //public string ImgUrlFourth { get; set; }
        //public string Type { get; set; }
        //public string SubCategory { get; set; }
        //public string LocationId { get; set; }
        //public Nullable<System.DateTime> Created { get; set; }
        //public Nullable<int> AddId { get; set; }

       // public int Id { get; set; }
        //public string Title { get; set; }
        //public string Price { get; set; }
        public string Availability { get; set; }
        public string ListedBy { get; set; }
        public string Furnishing { get; set; }
        public string Bedrooms { get; set; }
        public string SquareFeets { get; set; }
        public string Squareyards { get; set; }
      //  public string Description { get; set; }
        public string TypeId { get; set; }
        public string SubCategoryId { get; set; }
        //public string LocationId { get; set; }
        //public Nullable<System.DateTime> Created { get; set; }
        //public Nullable<int> AddId { get; set; }
        //public string ImgUrlPrimary { get; set; }
        //public string ImgUrlSeconday { get; set; }
        //public string ImgUrlThird { get; set; }
        //public string ImgUrlFourth { get; set; }



    }

    public class AddsModel
    {
        ///<summary>
        /// Gets or sets Adds.
        ///</summary>
        public List<Add> Adds { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }

        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
}