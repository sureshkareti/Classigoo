using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Classigoo.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult GetAdds(string location)
        {
            List<Add> addColl = new List<Add>();
            using (var client = new HttpClient())
            {
                string url = "http://localhost:51797/api/SearchApi/GetAdds/?location="+location;
                client.BaseAddress = new Uri(url);
                //HTTP GET
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Add>>();
                    readTask.Wait();

                    addColl = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }





            return View("DisplayAdds", addColl);
        }
    }
}