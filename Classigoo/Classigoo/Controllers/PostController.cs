using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Classigoo.Models;

namespace Classigoo.Controllers
{
    public class PostController : Controller
    {
        // GET: Post


        [HttpGet]
        public ActionResult Index()
        {
            //DeleteAdd("Real Estate", "56");


            PostAdd objPost = new PostAdd();
            Guid userId = Guid.Empty;


            //ViewBag.scripCall = "LoaderLoad();";

            if (Session["UserId"] != null)
            {
                userId = (Guid)Session["UserId"];
            }
            if (userId != Guid.Empty)
            {
                UserController userContObj = new UserController();
                User user = userContObj.GetUserDetails(userId);
                objPost.PhoneNumber = user.MobileNumber;
                objPost.Name = user.Name;

                ViewBag.Name = user.Name;
                ViewBag.Number = user.MobileNumber;
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

            //return View(objPost);




            return View();
        }

        [HttpPost]
        public ActionResult Index(PostAdd postAdd, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4)
        {




            int postId = 0;

            #region Login


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
                        string url = Constants.DomainName + "/ api/UserApi/AddUser/?user=" + user;
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

                    userId = userExist;
                    Session["UserId"] = userId;
                }
            }



            #endregion


            //  Guid userId = new Guid("280bf190-3fe3-4e1c-8f6e-e66edd7e272f");

            Add add = new Add()
            {
                Category = postAdd.hdnCateFristLevel,
                SubCategory = postAdd.hdnCateSecondLevel,
                State = postAdd.State,
                District = postAdd.District,
                Mandal = postAdd.Mandal,
                NearestArea = postAdd.LocalArea,
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
                    Library.WriteLog("At controller Executing Add addtable record", ex);

                    ViewBag.Message = "error";
                }
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var returnResult = result.Content.ReadAsAsync<int>();

                    returnResult.Wait();

                    postId = returnResult.Result;

                    string img1 = string.Empty;
                    string img2 = string.Empty;
                    string img3 = string.Empty;
                    string img4 = string.Empty;
                    try
                    {
                        CreateFolder("/ImgColl/" + postAdd.State);
                        CreateFolder("/ImgColl/" + postAdd.State + "/" + postAdd.District);
                        CreateFolder("/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal);

                        string currentDomain = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                        //for production
                        //img1 = currentDomain + "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image1.FileName + 1 + "-" + postId);
                        //img2 = currentDomain + "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image2.FileName + 2 + "-" + postId);
                        //img3 = currentDomain + "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image3.FileName + 3 + "-" + postId);
                        //img4 = currentDomain + "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image4.FileName + 4 + "-" + postId);

                        //for test
                        if (Image1 != null)
                        {
                            img1 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 1 + Image1.FileName);
                            Image1.SaveAs(Server.MapPath(img1));
                        }

                        if (Image2 != null)
                        {
                            img2 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 2 + Image2.FileName);

                            Image2.SaveAs(Server.MapPath(img2));
                        }
                        if (Image3 != null)
                        {
                            img3 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 3 + Image3.FileName);


                            Image3.SaveAs(Server.MapPath(img3));
                        }
                        if (Image4 != null)
                        {
                            img4 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 4 + Image4.FileName);


                            Image4.SaveAs(Server.MapPath(img4));
                        }

                    }
                    catch (Exception ex)
                    {
                        Library.WriteLog("At saving images", ex);
                        DeleteImage(new List<string>() { img1, img2, img3, img4 });
                        DeleteAdd("", Convert.ToString(postId));

                        ViewBag.Message = "error";
                    }

                    if (postAdd.hdnCateFristLevel == "Real Estate")
                    {
                        #region RealEstate

                        using (var clientRealEstate = new HttpClient())
                        {
                            RealEstate objRealEstate = new RealEstate()
                            {
                                SubCategory = postAdd.hdnCateSecondLevel,

                                Price = postAdd.txtPro_Price,
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
                            clientRealEstate.BaseAddress = new Uri(realEstatePostUrl);
                            var realEstatepostTask = clientRealEstate.PostAsJsonAsync<RealEstate>(realEstatePostUrl, objRealEstate);
                            try
                            {
                                realEstatepostTask.Wait();

                                if (realEstatepostTask.Result.StatusCode == HttpStatusCode.Created)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else if (realEstatepostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                                {
                                    DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                    ViewBag.Message = "error";
                                    return View();
                                }
                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At create realestate", ex);
                                DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                ViewBag.Message = "error";
                                return View();
                            }

                        }
                        #endregion
                    }
                    else if (postAdd.hdnCateFristLevel == "Construction Vehicles")
                    {
                        #region CV
                        using (var clientCV = new HttpClient())
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
                            clientCV.BaseAddress = new Uri(ConstructionVPostUrl);
                            var constructionVPostTask = clientCV.PostAsJsonAsync<ConstructionVehicle>(ConstructionVPostUrl, objConstructionVehicle);
                            try
                            {
                                constructionVPostTask.Wait();


                                if (constructionVPostTask.Result.StatusCode == HttpStatusCode.Created)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else if (constructionVPostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                                {
                                    DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                    ViewBag.Message = "error";
                                    return View();
                                }
                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At create creating construction vehicles", ex);
                                DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                ViewBag.Message = "error";
                                return View();
                            }


                        }
                        #endregion
                    }
                    else if (postAdd.hdnCateFristLevel == "Transportation Vehicles")
                    {
                        #region TV
                        using (var clientTV = new HttpClient())
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
                            clientTV.BaseAddress = new Uri(TransportationVPostUrl);
                            var transportationVPostTask = clientTV.PostAsJsonAsync<TransportationVehicle>(TransportationVPostUrl, objTransportationVehicle);
                            try
                            {
                                transportationVPostTask.Wait();

                                if (transportationVPostTask.Result.StatusCode == HttpStatusCode.Created)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else if (transportationVPostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                                {
                                    DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                    ViewBag.Message = "error";
                                    return View();
                                }
                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At create creating transportation vehicles", ex);

                                DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                ViewBag.Message = "error";
                                return View();
                            }

                        }
                        #endregion
                    }
                    else if (postAdd.hdnCateFristLevel == "Agricultural Vehicles")
                    {

                        #region AV
                        using (var clientAV = new HttpClient())
                        {
                            AgriculturalVehicle objAgriculturalVehicle = new AgriculturalVehicle()
                            {
                                Company = postAdd.AVCompany_list,
                                OtherCompany = postAdd.AVOtherCompany,
                                SubCategory = postAdd.hdnCateSecondLevel,

                                Price = postAdd.txtAV_Price,
                                Description = postAdd.txtAddDetails,
                                AddId = postId,
                                ImgUrlPrimary = img1,
                                ImgUrlSeconday = img2,
                                ImgUrlThird = img3,
                                ImgUrlFourth = img4
                            };

                            string agriculturalVPostUrl = Constants.PostAgricutureVehicleUrl; // "http://localhost:51797/api/PostApi/AgriculturalVehicle";
                            clientAV.BaseAddress = new Uri(agriculturalVPostUrl);
                            var agriculturalVPostTask = clientAV.PostAsJsonAsync<AgriculturalVehicle>(agriculturalVPostUrl, objAgriculturalVehicle);
                            try
                            {
                                agriculturalVPostTask.Wait();


                                if (agriculturalVPostTask.Result.StatusCode == HttpStatusCode.Created)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else if (agriculturalVPostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                                {
                                    DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                    ViewBag.Message = "error";
                                    return View();
                                }
                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At create creating agricultural vehicles", ex);

                                DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                ViewBag.Message = "error";
                                return View();
                            }

                        }
                        #endregion
                    }
                    else if (postAdd.hdnCateFristLevel == "Passenger Vehicles")
                    {
                        #region PV
                        using (var clientPV = new HttpClient())
                        {
                            PassengerVehicle objPassengerVehicle = new PassengerVehicle()
                            {
                                Company = postAdd.PVCompany_list,
                                OtherCompany = postAdd.PVOtherCompany,
                                SubCategory = postAdd.hdnCateSecondLevel,

                                Price = postAdd.txtPV_price,
                                Model = postAdd.PVModel_list,
                                Year = postAdd.txtPV_Year,
                                FuelType = postAdd.PVfueltype_list,
                                KMDriven = postAdd.txtPV_kmdriven,
                                Description = postAdd.txtAddDetails,
                                AddId = postId,
                                ImgUrlPrimary = img1,
                                ImgUrlSeconday = img2,
                                ImgUrlThird = img3,
                                ImgUrlFourth = img4
                            };

                            string passengerVPostUrl = Constants.PostPassengerVehicleUrl;
                            clientPV.BaseAddress = new Uri(passengerVPostUrl);
                            var passengerVPostTask = clientPV.PostAsJsonAsync<PassengerVehicle>(passengerVPostUrl, objPassengerVehicle);
                            try
                            {
                                passengerVPostTask.Wait();

                                if (passengerVPostTask.Result.StatusCode == HttpStatusCode.Created)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else if (passengerVPostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                                {
                                    DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                    ViewBag.Message = "error";
                                    return View();
                                }
                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At create creating passenger vehicles", ex);

                                DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                ViewBag.Message = "error";
                                return View();
                            }

                        }
                        #endregion
                    }
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


        public bool DeleteAdd(string type, string id)
        {
            using (var clientDeleteAdd = new HttpClient())
            {

                clientDeleteAdd.BaseAddress = new Uri(Constants.PostDeleteUrl);
                var deletepostTask = clientDeleteAdd.PostAsJsonAsync<String[]>(Constants.PostDeleteUrl, new string[] { type, id });
                try
                {
                    deletepostTask.Wait();
                }
                catch (Exception ex)
                {

                }

                if (deletepostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                {
                    return false;
                }
                else if (deletepostTask.Result.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }

            return false;
        }

        public bool DeleteImage(List<string> urls)
        {
            foreach (string url in urls)
            {
                FileInfo file = new FileInfo(url);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();

                    return true;
                }
            }

            return false;
        }

    }
}