using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classigoo.Models.Search
{
    public class SearchAddsEntity
    {
        public string Type { set; get; }
        public string Status { set; get; }
        public string MobileNumber { set; get; }
        public string State { set; get; }
        public string District { set; get; }
        public string Mandal { set; get; }
        public string Category { set; get; }
        public string SubCaregory { set; get; }
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