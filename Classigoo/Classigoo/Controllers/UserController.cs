using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Classigoo.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult UnableToLogin()
        {
            return View();
        }
        public ActionResult Home()
        {
            IEnumerable<tbl_Adds> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51797/api/");
                //HTTP GET
                var responseTask = client.GetAsync("Adds");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<tbl_Adds>>();
                    readTask.Wait();

                    students = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    students = Enumerable.Empty<tbl_Adds>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(students);
           
        }

        public ActionResult PostAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostAdd(tbl_Adds Add)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51797/api/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<tbl_Adds>("Adds", Add);
                try
                {

                    postTask.Wait();
                }
                catch (Exception ex)
                {

                }

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Home");
                }

            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(Add);
        }
    }
    }
