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
            int postId = 0;

            Guid userId = new Guid("19e2aca5-28a9-41ca-a641-e81c9139e34f");//19e2aca5-28a9-41ca-a641-e81c9139e34f 280BF190-3FE3-4E1C-8F6E-E66EDD7E272F
            Add add = new Add() { CategoryId = "1234", LocationId = "1234", UserId = userId };

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:51797/api/");


                string url = "http://localhost:51797/api/PostApi/PostAdd";
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

                    postId = returnResult.Result;
                }
            }

            using (var client = new HttpClient())
            {
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
                    Created = DateTime.Now.Date,
                    AddId = postId,
                    ImgUrlPrimary = "img1/testFolder/1.jpg",
                    ImgUrlSeconday = "img2/testFolder/2.jpg",
                    ImgUrlThird = "img3/testFolder/3.jpg",
                    ImgUrlFourth = "img4/testFolder/4.jpg"
                };

                string realEstatePostUrl = "http://localhost:51797/api/PostApi/RealEstate";
                client.BaseAddress = new Uri(realEstatePostUrl);
                var realEstatepostTask = client.PostAsJsonAsync<RealEstate>(realEstatePostUrl, objRealEstate);
                try
                {
                    realEstatepostTask.Wait();
                }
                catch (Exception ex)
                {

                }
                var realEstatePostTask = realEstatepostTask.Result;
                if (realEstatePostTask.IsSuccessStatusCode)
                {

                }

                return RedirectToAction("Home", "User");


            }

            return View();
        }

        public ActionResult Index1()
        {
            bool isEdit = true;
            if (isEdit)
            {
                PostAdd objPostAdd = new PostAdd();
                objPostAdd.txtTitle = "TestTitle";
                objPostAdd.ddlRentOrSale = "Sale";

                objPostAdd.hdnCateFristLevel = "Agricultural Vehicles";
                objPostAdd.hdnCateSecondLevel = "Tractors";

                objPostAdd.txtAV_Price = "123";
                objPostAdd.AVCompany_list = "Other";

                objPostAdd.txtAddDetails = "this is test description";
                return View(objPostAdd);
            }

            return View();
        }
        [HttpPost]
        public ActionResult Index1(PostAdd postAdd)
        {

            return View();
        }
    }
}