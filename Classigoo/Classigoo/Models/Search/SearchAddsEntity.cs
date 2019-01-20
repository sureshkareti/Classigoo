using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classigoo.Models.Search
{
    public class SearchOwnerAddsEntity
    {
        public string Type { set; get; }
        public string Status { set; get; }
        public string MobileNumber { set; get; }
        public string State { set; get; }
        public string District { set; get; }
        public string Mandal { set; get; }
        public string Category { set; get; }
        public string SubCategory { set; get; }
    }
    public class SearchConsumerAddsEntity
    {
        public string Type { set; get; }
        public string Status { set; get; }
        public string MobileNumber { set; get; }
        public string Category { set; get; }
        public string SubCategory { set; get; }
        public string Company { set; get; }
        public string State { set; get; }
        public string District { set; get; }
        public string Mandal { set; get; }
        public string From { set; get; }
        public string To { set; get; }
       
    }
    public class AjaxResponse<T>
    {
        public ResponseStatus Status { set; get; }
        public T Data { set; get; }
    }

    public enum ResponseStatus{
        success,
        failure
    }
}