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
        public DateTime Createddate { set; get; }
        public string Description { set; get; }
        public string Location { set; get; }
    }
    public class JQueryDataTableParams
    {     
        public string sEcho { get; set; }

        public string sSearch { get; set; }

        
        public int iDisplayLength { get; set; }

        public int iDisplayStart { get; set; }

        
        public int iColumns { get; set; }

        
        public int iSortingCols { get; set; }

        
        public string sColumns { get; set; }

        
        public int iSortCol_0 { get; set; }

        
        public string sSortDir_0 { get; set; }
    }
}