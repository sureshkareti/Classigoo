using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classigoo
{
    public class AddsModel
    {
        public List<CustomAdd> Adds { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageCount { get; set; }
    }
    public class CustomAdd
    {
        public int AddId { get; set; }
        public string Title { get; set; }
        public string CreatedDate { set; get; }
        public string Description { set; get; }
        public string Location { set; get; }
        public string Price { set; get; }
        public string Category { set; get; }
        public RealEstate RealEstate { set; get; }
        public Car Cars { set; get; }
        public Electronic Electronics { set; get; }
    }
   
}