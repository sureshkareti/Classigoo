using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Classigoo.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string realEsateModel)
        {
            Guid userId = new Guid("280BF190-3FE3-4E1C-8F6E-E66EDD7E272F");
            Add add = new Add() { CategoryId = "1234", LocationId = "1234", UserId = userId };

            using(var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:51797/api/");


                string url = "http://localhost:51797/api/PostApi/PostAdd/?add=" + add;
                client.BaseAddress = new Uri(url);

                var postTask = client.PostAsJsonAsync<Add>(url, add);
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
                    return RedirectToAction("Home", "User");
                }

            }

            return View();
        }
    }
}