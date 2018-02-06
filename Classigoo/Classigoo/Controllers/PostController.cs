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
                    var returnResult = result.Content.ReadAsAsync<int>();
                         
                    returnResult.Wait();

                    int value  = returnResult.Result;

                    RealEstate objRealEstate = new RealEstate()
                    {
                        Title = "Test title",
                        Price = "25000",
                        Availability = "Construnction going",
                        ListedBy = "Owner",
                        Furnishing = "Yes",
                        Bedrooms = "2",
                        SquareFeets = "260",
                        Description = "this is test description",
                        TypeId = "2",
                        SubCategoryId = "24",
                        LocationId = "23",
                        Created = DateTime.Now.Date,
                        AddId = value,
                        ImgUrlPrimary = "img1/testFolder/1.jpg",
                        ImgUrlSeconday = "img2/testFolder/2.jpg",
                        ImgUrlThird = "img3/testFolder/3.jpg",
                        ImgUrlFourth = "img4/testFolder/4.jpg"
                    };

                    string realEstatePostUrl = "http://localhost:51797/api/PostApi/P/?add=" + add;
                    client.BaseAddress = new Uri(realEstatePostUrl);

                    var postTask = client.PostAsJsonAsync<Add>(realEstatePostUrl, add);
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

                    }

                        return RedirectToAction("Home", "User");
                }

            }

            return View();
        }
    }
}