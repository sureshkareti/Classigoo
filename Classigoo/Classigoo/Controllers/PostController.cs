using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            PostAdd objPost = new PostAdd();
            Guid userId = Guid.Empty;
            if (Session["UserId"] != null)
            {
                userId = (Guid)Session["UserId"];
            }
            if(userId!=Guid.Empty)
            {
                UserController userContObj = new UserController();
                User user = userContObj.GetUserDetails(userId);
                objPost.PhoneNumber = user.MobileNumber;
                objPost.Name = user.Name;

            }
            //bool isEdit = true;
            //if (isEdit)
            //{
            //    PostAdd objPostAdd = new PostAdd();
            //    objPostAdd.txtTitle = "TestTitle";
            //    objPostAdd.ddlRentOrSale = "Sale";

            //    objPostAdd.hdnCateFristLevel = "Agricultural Vehicles";
            //    objPostAdd.hdnCateSecondLevel = "Tractors";

            //    objPostAdd.txtAV_Price = "123";
            //    objPostAdd.AVCompany_list = "Other";

            //    objPostAdd.txtAddDetails = "this is test description";
            //    return View(objPostAdd);
            //}

            return View(objPost);
        }
        [HttpPost]
        public ActionResult Index(PostAdd postAdd, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4)
        {
           
            CreateFolder("/ImgColl/" + postAdd.State);
            CreateFolder("/ImgColl/" + postAdd.State +"/"+ postAdd.District);
            CreateFolder("/ImgColl/" + postAdd.State + "/" + postAdd.District+"/" + postAdd.Mandal);
            string img1 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image1.FileName);
            string img2 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image2.FileName);
            string img3 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image3.FileName);
            string img4 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image4.FileName);
            Image1.SaveAs(Server.MapPath(img1));
            Image2.SaveAs(Server.MapPath(img2));
            Image3.SaveAs(Server.MapPath(img3));
            Image4.SaveAs(Server.MapPath(img4));

            int postId = 0;
            Guid userId = Guid.Empty;
            if (Session["UserId"] != null)
            {
                userId = (Guid)Session["UserId"];
            }
            else
            {
                UserController userContr = new UserController();

               Guid userExist = userContr.IsUserExist(postAdd.PhoneNumber, "Custom");
                if (userExist == Guid.Empty)
                {
                    using (var client = new HttpClient())
                {
                    User user = new User();
                    user.MobileNumber = postAdd.PhoneNumber;
                    user.Name = postAdd.Name;
                    //  user.Password = coll["inputPassword"];
                    user.Type = "Custom";
                    string url = "http://localhost:51797/api/UserApi/AddUser/?user=" + user;
                    client.BaseAddress = new Uri(url);
                    var postTask = client.PostAsJsonAsync<User>(url, user);
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
                          var readTask = result.Content.ReadAsAsync<User>();
                            readTask.Wait();

                            userId = readTask.Result.UserId;
                        
                            Session["UserId"] = userId;
                        }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }

                }
                    
            }
                else
                {

                    userId=userContr.IsUserExist(postAdd.PhoneNumber, "Custom");
                    Session["UserId"] = userId;
                }
            }
          //  Guid userId = new Guid("280bf190-3fe3-4e1c-8f6e-e66edd7e272f");

            Add add = new Add()
            {
                Category = postAdd.hdnCateFristLevel,
                SubCategory = postAdd.hdnCateSecondLevel,
                State = postAdd.State,
                District = postAdd.District,
                Mandal = postAdd.Mandal,
                NearestArea=postAdd.LocalArea,
                Title = postAdd.txtTitle,
                Type = postAdd.ddlRentOrSale,
                Status = Constants.PendingSatus,
                UserId = userId
            };

            using (var client = new HttpClient())
            {

                string url = Constants.PostAddUrl; //"http://localhost:51797/api/PostApi/PostAdd";
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


            if (postAdd.hdnCateFristLevel == "Real Estate")
            {
                using (var client = new HttpClient())
                {
                    RealEstate objRealEstate = new RealEstate()
                    {
                        SubCategory = postAdd.hdnCateSecondLevel,

                        Price =postAdd.txtPro_Price,
                        Availability = postAdd.ddlAvailability,
                        ListedBy = postAdd.ddlPostedBy,
                        Furnishing = postAdd.ddlFurnishing,
                        Bedrooms = postAdd.ddlBedrooms,
                        SquareFeets = postAdd.txtSquareFeet,
                        Squareyards = postAdd.txtSquareYards,
                        
                        Description = postAdd.txtAddDetails,
                        
                        AddId = postId,
                        ImgUrlPrimary = img1,
                        ImgUrlSeconday = img2,
                        ImgUrlThird = img3,
                        ImgUrlFourth = img4
                    };

                    string realEstatePostUrl = Constants.PostRealEstateUrl; //"http://localhost:51797/api/PostApi/RealEstate";
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
            }
            else if (postAdd.hdnCateFristLevel == "Construction Vehicles")
            {
                using (var client = new HttpClient())
                {
                    ConstructionVehicle objConstructionVehicle = new ConstructionVehicle()
                    {
                        Company = postAdd.CVCompany_list,
                        OtherCompany = postAdd.CVOtherCompany,
                        SubCategory = postAdd.hdnCateSecondLevel,

                        Price = postAdd.txtCV_Price,
                        Description = postAdd.txtAddDetails,
                        AddId = postId,
                        ImgUrlPrimary = img1,
                        ImgUrlSeconday = img2,
                        ImgUrlThird = img3,
                        ImgUrlFourth = img4
                    };

                    string ConstructionVPostUrl = Constants.PostConstructionVehicleUrl; 
                    client.BaseAddress = new Uri(ConstructionVPostUrl);
                    var constructionVPostTask = client.PostAsJsonAsync<ConstructionVehicle>(ConstructionVPostUrl, objConstructionVehicle);
                    try
                    {
                        constructionVPostTask.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                    var constructionVResponse = constructionVPostTask.Result;
                    if (constructionVResponse.IsSuccessStatusCode)
                    {

                    }

                    return RedirectToAction("Home", "User");


                }
            }
            else if (postAdd.hdnCateFristLevel == "Transportation Vehicles")
            {
                using (var client = new HttpClient())
                {
                    TransportationVehicle objTransportationVehicle = new TransportationVehicle()
                    {
                        Company = postAdd.TVCompany_list,
                        OtherCompany = postAdd.TVOtherCompany,
                        SubCategory = postAdd.hdnCateSecondLevel,

                        Price = postAdd.txtTV_Price,
                        Description = postAdd.txtAddDetails,
                        AddId = postId,
                        ImgUrlPrimary = img1,
                        ImgUrlSeconday = img2,
                        ImgUrlThird = img3,
                        ImgUrlFourth = img4
                    };

                    string TransportationVPostUrl = Constants.PostTransportationVehicleUrl;
                    client.BaseAddress = new Uri(TransportationVPostUrl);
                    var transportationVPostTask = client.PostAsJsonAsync<TransportationVehicle>(TransportationVPostUrl, objTransportationVehicle);
                    try
                    {
                        transportationVPostTask.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                    var transportationVResponse = transportationVPostTask.Result;
                    if (transportationVResponse.IsSuccessStatusCode)
                    {

                    }

                    return RedirectToAction("Home", "User");


                }
            }
            else if (postAdd.hdnCateFristLevel == "Agricultural Vehicles")
            {

                using (var client = new HttpClient())
                {
                    AgriculturalVehicle objAgriculturalVehicle = new AgriculturalVehicle()
                    {
                        Company = postAdd.AVCompany_list,
                        OtherCompany=postAdd.AVOtherCompany,
                        SubCategory=postAdd.hdnCateSecondLevel,

                        Price = postAdd.txtAV_Price,                      
                        Description = postAdd.txtAddDetails,                     
                        AddId = postId,
                        ImgUrlPrimary = img1,
                        ImgUrlSeconday = img2,
                        ImgUrlThird = img3,
                        ImgUrlFourth = img4
                    };

                    string agriculturalVPostUrl = Constants.PostAgricutureVehicleUrl; // "http://localhost:51797/api/PostApi/AgriculturalVehicle";
                    client.BaseAddress = new Uri(agriculturalVPostUrl);
                    var agriculturalVPostTask = client.PostAsJsonAsync<AgriculturalVehicle>(agriculturalVPostUrl, objAgriculturalVehicle);
                    try
                    {
                        agriculturalVPostTask.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                    var agriculturalVResponse = agriculturalVPostTask.Result;
                    if (agriculturalVResponse.IsSuccessStatusCode)
                    {

                    }

                    return RedirectToAction("Home", "User");


                }
            }
            else if (postAdd.hdnCateFristLevel == "Passenger Vehicles")
            {
                using (var client = new HttpClient())
                {
                    PassengerVehicle objPassengerVehicle = new PassengerVehicle()
                    {
                        Company = postAdd.PVCompany_list,
                        OtherCompany = postAdd.PVOtherCompany,
                        SubCategory = postAdd.hdnCateSecondLevel,

                        Price = postAdd.txtPV_price,
                        Model=postAdd.PVModel_list,
                        Year = postAdd.txtPV_Year,
                        FuelType=postAdd.PVfueltype_list,
                        KMDriven=postAdd.txtPV_kmdriven,
                        Description = postAdd.txtAddDetails,
                        AddId = postId,
                        ImgUrlPrimary = img1,
                        ImgUrlSeconday = img2,
                        ImgUrlThird = img3,
                        ImgUrlFourth = img4
                    };

                    string passengerVPostUrl = Constants.PostPassengerVehicleUrl;
                    client.BaseAddress = new Uri(passengerVPostUrl);
                    var passengerVPostTask = client.PostAsJsonAsync<PassengerVehicle>(passengerVPostUrl, objPassengerVehicle);
                    try
                    {
                        passengerVPostTask.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                    var passengerVResponse = passengerVPostTask.Result;

                    if(passengerVResponse.StatusCode== HttpStatusCode.BadRequest)
                    {

                    }

                    if (passengerVResponse.IsSuccessStatusCode)
                    {

                    }

                    return RedirectToAction("Home", "User");


                }
            }

            return View();
        }
        
        public void CreateFolder(string path)
        {
            string dirPath = Server.MapPath(path);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}