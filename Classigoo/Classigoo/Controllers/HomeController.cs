using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
namespace Classigoo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            IEnumerable <tbl_Adds> Adds = null;

          
           using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51797/api/Adds");
                //HTTP GET
                var responseTask = client.GetAsync("GetAllAdds");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<tbl_Adds>>();
                    readTask.Wait();

                    Adds = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    Adds = Enumerable.Empty<tbl_Adds>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(Adds);
        }
           
        }
    }

