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

            Library.WriteLog("hi");

            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    Add add = db.Adds.SingleOrDefault(x => x.AddId == 20);
                    add.Status = "Deactive";


                    int response = db.SaveChanges();
                    if (response == 1)
                    {
                        Library.WriteLog("ok changed");

                    }
                    else
                    {
                        Library.WriteLog("got exception");
                    }
                }

            }
            catch (Exception ex)
            {
                Library.WriteLog("got exception",ex);
            }


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

            string addId = Request.QueryString["addId"];

            if (addId != null)
            {
                ViewBag.addId = addId;

                PostAdd objPostAdd = new PostAdd();

                using (var clientGetAdd = new HttpClient())
                {

                    string url = Constants.DomainName + "/api/UserApi/GetAddById/?addId=" + addId;
                    clientGetAdd.BaseAddress = new Uri(url);
                    //HTTP GET
                    var getAddTask = clientGetAdd.GetAsync(url);


                    try
                    {
                        getAddTask.Wait();

                    }
                    catch (Exception ex)
                    {
                        Library.WriteLog("At getting add record", ex);
                        ViewBag.Message = "error";
                    }

                    if (getAddTask.Result.StatusCode == HttpStatusCode.NotFound)
                    {
                        ViewBag.status = "NotFound";
                        return View();
                    }
                    else if (getAddTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                    {
                        ViewBag.Message = "error";
                        return View();
                    }
                    else if (getAddTask.Result.StatusCode == HttpStatusCode.OK)
                    {

                        var readTask = getAddTask.Result.Content.ReadAsAsync<Add>();
                        readTask.Wait();


                        Add addRecord = readTask.Result;

                        objPostAdd.txtTitle = addRecord.Title;
                        objPostAdd.hdnCateFristLevel = addRecord.Category;
                        objPostAdd.hdnCateSecondLevel = addRecord.SubCategory;
                        objPostAdd.State = addRecord.State;
                        objPostAdd.District = addRecord.District;
                        objPostAdd.Mandal = addRecord.Mandal;
                        objPostAdd.LocalArea = addRecord.NearestArea;
                        objPostAdd.ddlRentOrSale = addRecord.Type;


                        if (addRecord.RealEstates.Count == 1)
                        {
                            #region RealEstate



                            RealEstate realEstateRecord = addRecord.RealEstates.ToList()[0];

                            objPostAdd.txtPro_Price = Convert.ToString(realEstateRecord.Price);//  realEstateRecord.Price == null ? 0 : Convert.ToInt32(realEstateRecord.Price);
                            objPostAdd.ddlAvailability = realEstateRecord.Availability;
                            objPostAdd.ddlFurnishing = realEstateRecord.Furnishing;

                            objPostAdd.txtAcres = realEstateRecord.Acres == null ? "" : Convert.ToString(realEstateRecord.Acres);
                            objPostAdd.ddlPostedBy = realEstateRecord.ListedBy;
                            objPostAdd.ddlBedrooms = realEstateRecord.Bedrooms;
                            objPostAdd.txtSquareFeet = Convert.ToString(realEstateRecord.SquareFeets);  //realEstateRecord.SquareFeets == null ? default(int) : Convert.ToInt32(realEstateRecord.SquareFeets);
                            objPostAdd.txtSquareYards = Convert.ToString(realEstateRecord.Squareyards); // realEstateRecord.Squareyards == null ? default(int) : Convert.ToInt32(realEstateRecord.Squareyards);
                            objPostAdd.txtAddDetails = realEstateRecord.Description;

                            ViewBag.img1 = realEstateRecord.ImgUrlPrimary;
                            ViewBag.img2 = realEstateRecord.ImgUrlSeconday;
                            ViewBag.img3 = realEstateRecord.ImgUrlThird;
                            ViewBag.img4 = realEstateRecord.ImgUrlFourth;

                            return View(objPostAdd);


                            //using (var clientRealEstate = new HttpClient())
                            //{
                            //    clientRealEstate.BaseAddress = new Uri(Constants.GetRealestate);
                            //    var realEstategetTask = clientRealEstate.GetAsync(Constants.GetRealestate + "?addId=" + addId);
                            //    try
                            //    {
                            //        realEstategetTask.Wait();

                            //        if (realEstategetTask.Result.StatusCode == HttpStatusCode.NotFound)
                            //        {
                            //            ViewBag.status = "NotFound";
                            //            return View();
                            //        }
                            //        else if (realEstategetTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                            //        {
                            //            ViewBag.Message = "error";
                            //            return View();
                            //        }
                            //        else if (realEstategetTask.Result.StatusCode == HttpStatusCode.OK)
                            //        {
                            //            var readRealestateTask = realEstategetTask.Result.Content.ReadAsAsync<RealEstate>();
                            //            readTask.Wait();

                            //            RealEstate realEstateRecord = readRealestateTask.Result;

                            //            objPostAdd.txtPro_Price = realEstateRecord.Price == null ? 0 : Convert.ToInt32(realEstateRecord.Price);
                            //            objPostAdd.ddlAvailability = realEstateRecord.Availability;
                            //            objPostAdd.ddlFurnishing = realEstateRecord.Furnishing;
                            //            objPostAdd.ddlPostedBy = realEstateRecord.ListedBy;
                            //            objPostAdd.ddlBedrooms = realEstateRecord.Bedrooms;
                            //            objPostAdd.txtSquareFeet = realEstateRecord.SquareFeets == null ? default(int) : Convert.ToInt32(realEstateRecord.SquareFeets);
                            //            objPostAdd.txtSquareYards = realEstateRecord.Squareyards == null ? default(int) : Convert.ToInt32(realEstateRecord.Squareyards);
                            //            objPostAdd.txtAddDetails = realEstateRecord.Description;

                            //            ViewBag.img1 = realEstateRecord.ImgUrlPrimary;
                            //            ViewBag.img1 = realEstateRecord.ImgUrlSeconday;
                            //            ViewBag.img1 = realEstateRecord.ImgUrlThird;
                            //            ViewBag.img1 = realEstateRecord.ImgUrlFourth;

                            //            return View(objPostAdd);

                            //        }

                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Library.WriteLog("At getting real estate record", ex);
                            //        ViewBag.Message = "error";
                            //        return View();
                            //    }

                            //}
                            #endregion
                        }
                        else if (addRecord.Category == Constants.ConstructionVehicle)
                        {
                            #region CV
                            //using (var clientCV = new HttpClient())
                            //{
                            //    ConstructionVehicle objConstructionVehicle = new ConstructionVehicle()
                            //    {
                            //        Company = postAdd.CVCompany_list,
                            //        OtherCompany = postAdd.CVOtherCompany,
                            //        SubCategory = postAdd.hdnCateSecondLevel,

                            //        Price = postAdd.txtCV_Price,
                            //        Description = postAdd.txtAddDetails,
                            //        AddId = postId,
                            //        ImgUrlPrimary = img1,
                            //        ImgUrlSeconday = img2,
                            //        ImgUrlThird = img3,
                            //        ImgUrlFourth = img4
                            //    };

                            //    string ConstructionVPostUrl = Constants.PostConstructionVehicleUrl;
                            //    clientCV.BaseAddress = new Uri(ConstructionVPostUrl);
                            //    var constructionVPostTask = clientCV.PostAsJsonAsync<ConstructionVehicle>(ConstructionVPostUrl, objConstructionVehicle);
                            //    try
                            //    {
                            //        constructionVPostTask.Wait();


                            //        if (constructionVPostTask.Result.StatusCode == HttpStatusCode.Created)
                            //        {
                            //            return RedirectToAction("Home", "User");
                            //        }
                            //        else if (constructionVPostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                            //        {
                            //            DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                            //            ViewBag.Message = "error";
                            //            return View();
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Library.WriteLog("At create creating construction vehicles", ex);
                            //        DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                            //        ViewBag.Message = "error";
                            //        return View();
                            //    }


                            //}
                            #endregion
                        }
                        else if (addRecord.Category == Constants.TransportationVehicle)
                        {
                            #region TV
                            //using (var clientTV = new HttpClient())
                            //{
                            //    TransportationVehicle objTransportationVehicle = new TransportationVehicle()
                            //    {
                            //        Company = postAdd.TVCompany_list,
                            //        OtherCompany = postAdd.TVOtherCompany,
                            //        SubCategory = postAdd.hdnCateSecondLevel,

                            //        Price = postAdd.txtTV_Price,
                            //        Description = postAdd.txtAddDetails,
                            //        AddId = postId,
                            //        ImgUrlPrimary = img1,
                            //        ImgUrlSeconday = img2,
                            //        ImgUrlThird = img3,
                            //        ImgUrlFourth = img4
                            //    };

                            //    string TransportationVPostUrl = Constants.PostTransportationVehicleUrl;
                            //    clientTV.BaseAddress = new Uri(TransportationVPostUrl);
                            //    var transportationVPostTask = clientTV.PostAsJsonAsync<TransportationVehicle>(TransportationVPostUrl, objTransportationVehicle);
                            //    try
                            //    {
                            //        transportationVPostTask.Wait();

                            //        if (transportationVPostTask.Result.StatusCode == HttpStatusCode.Created)
                            //        {
                            //            return RedirectToAction("Home", "User");
                            //        }
                            //        else if (transportationVPostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                            //        {
                            //            DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                            //            ViewBag.Message = "error";
                            //            return View();
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Library.WriteLog("At create creating transportation vehicles", ex);

                            //        DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                            //        ViewBag.Message = "error";
                            //        return View();
                            //    }

                            //}
                            #endregion
                        }
                        else if (addRecord.Category == Constants.AgriculturalVehicle)
                        {

                            #region AV
                            //using (var clientAV = new HttpClient())
                            //{
                            //    AgriculturalVehicle objAgriculturalVehicle = new AgriculturalVehicle()
                            //    {
                            //        Company = postAdd.AVCompany_list,
                            //        OtherCompany = postAdd.AVOtherCompany,
                            //        SubCategory = postAdd.hdnCateSecondLevel,

                            //        Price = postAdd.txtAV_Price,
                            //        Description = postAdd.txtAddDetails,
                            //        AddId = postId,
                            //        ImgUrlPrimary = img1,
                            //        ImgUrlSeconday = img2,
                            //        ImgUrlThird = img3,
                            //        ImgUrlFourth = img4
                            //    };

                            //    string agriculturalVPostUrl = Constants.PostAgricutureVehicleUrl; // "http://localhost:51797/api/PostApi/AgriculturalVehicle";
                            //    clientAV.BaseAddress = new Uri(agriculturalVPostUrl);
                            //    var agriculturalVPostTask = clientAV.PostAsJsonAsync<AgriculturalVehicle>(agriculturalVPostUrl, objAgriculturalVehicle);
                            //    try
                            //    {
                            //        agriculturalVPostTask.Wait();


                            //        if (agriculturalVPostTask.Result.StatusCode == HttpStatusCode.Created)
                            //        {
                            //            return RedirectToAction("Home", "User");
                            //        }
                            //        else if (agriculturalVPostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                            //        {
                            //            DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                            //            ViewBag.Message = "error";
                            //            return View();
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Library.WriteLog("At create creating agricultural vehicles", ex);

                            //        DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                            //        ViewBag.Message = "error";
                            //        return View();
                            //    }

                            //}
                            #endregion
                        }
                        else if (addRecord.Category == Constants.PassengerVehicle)
                        {
                            #region PV
                            //using (var clientPV = new HttpClient())
                            //{
                            //    PassengerVehicle objPassengerVehicle = new PassengerVehicle()
                            //    {
                            //        Company = postAdd.PVCompany_list,
                            //        OtherCompany = postAdd.PVOtherCompany,
                            //        SubCategory = postAdd.hdnCateSecondLevel,

                            //        Price = postAdd.txtPV_price,
                            //        Model = postAdd.PVModel_list,
                            //        Year = postAdd.txtPV_Year,
                            //        FuelType = postAdd.PVfueltype_list,
                            //        KMDriven = postAdd.txtPV_kmdriven,
                            //        Description = postAdd.txtAddDetails,
                            //        AddId = postId,
                            //        ImgUrlPrimary = img1,
                            //        ImgUrlSeconday = img2,
                            //        ImgUrlThird = img3,
                            //        ImgUrlFourth = img4
                            //    };

                            //    string passengerVPostUrl = Constants.PostPassengerVehicleUrl;
                            //    clientPV.BaseAddress = new Uri(passengerVPostUrl);
                            //    var passengerVPostTask = clientPV.PostAsJsonAsync<PassengerVehicle>(passengerVPostUrl, objPassengerVehicle);
                            //    try
                            //    {
                            //        passengerVPostTask.Wait();

                            //        if (passengerVPostTask.Result.StatusCode == HttpStatusCode.Created)
                            //        {
                            //            return RedirectToAction("Home", "User");
                            //        }
                            //        else if (passengerVPostTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                            //        {
                            //            DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                            //            ViewBag.Message = "error";
                            //            return View();
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Library.WriteLog("At create creating passenger vehicles", ex);

                            //        DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                            //        ViewBag.Message = "error";
                            //        return View();
                            //    }

                            //}
                            #endregion
                        }

                    }
                }


            }


            return View();
        }

        [HttpPost]
        public ActionResult Index(PostAdd postAdd, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4, string addId)
        {

            string queryStringForEdit = Request.QueryString["addId"];
            if (queryStringForEdit == null)
            {


                #region AddPost

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

                                    Price = Convert.ToInt32(postAdd.txtPro_Price),
                                    Availability = postAdd.ddlAvailability,
                                    ListedBy = postAdd.ddlPostedBy,
                                    Furnishing = postAdd.ddlFurnishing,
                                    Bedrooms = postAdd.ddlBedrooms,
                                    SquareFeets = Convert.ToInt32(postAdd.txtSquareFeet),
                                    Squareyards = Convert.ToInt32(postAdd.txtSquareYards),
                                    Acres = Convert.ToDecimal(postAdd.txtAcres),
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

                                    Price = Convert.ToInt32(postAdd.txtCV_Price),
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

                                    Price = Convert.ToInt32(postAdd.txtTV_Price),
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

                                    Price = Convert.ToInt32(postAdd.txtAV_Price),
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

                                    Price = Convert.ToInt32(postAdd.txtPV_price),
                                    Model = postAdd.PVModel_list,
                                    Year = postAdd.txtPV_Year,
                                    FuelType = postAdd.PVfueltype_list,
                                    KMDriven = Convert.ToInt32(postAdd.txtPV_kmdriven),
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
                #endregion

            }
            else
            {
                #region UpdatePost
                int postId = Convert.ToInt32(queryStringForEdit);

                Guid userId = Guid.Empty;
                if (Session["UserId"] != null)
                {
                    userId = (Guid)Session["UserId"];
                }
                else
                {
                    ViewBag.Message = "nologin";
                    return View();
                }

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
                    UserId = userId,
                    AddId = postId
                };

                using (var client = new HttpClient())
                {

                    string url = Constants.UpdateAddUrl; //"http://localhost:51797/api/PostApi/PostAdd";
                    client.BaseAddress = new Uri(url);
                    var postTask = client.PostAsJsonAsync<Add>(url, add);
                    try
                    {
                        postTask.Wait();

                        var result = postTask.Result;
                        if (postTask.Result.StatusCode == HttpStatusCode.OK)
                        {
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

                                        Price = Convert.ToInt32(postAdd.txtPro_Price),
                                        Availability = postAdd.ddlAvailability,
                                        ListedBy = postAdd.ddlPostedBy,
                                        Furnishing = postAdd.ddlFurnishing,
                                        Bedrooms = postAdd.ddlBedrooms,
                                        SquareFeets = Convert.ToInt32(postAdd.txtSquareFeet),
                                        Squareyards = Convert.ToInt32(postAdd.txtSquareYards),
                                        Acres = Convert.ToDecimal(postAdd.txtAcres),
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

                                        Price = Convert.ToInt32(postAdd.txtCV_Price),
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

                                        Price = Convert.ToInt32(postAdd.txtTV_Price),
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

                                        Price = Convert.ToInt32(postAdd.txtAV_Price),
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

                                        Price = Convert.ToInt32(postAdd.txtPV_price),
                                        Model = postAdd.PVModel_list,
                                        Year = postAdd.txtPV_Year,
                                        FuelType = postAdd.PVfueltype_list,
                                        KMDriven = Convert.ToInt32(postAdd.txtPV_kmdriven),
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
                    catch (Exception ex)
                    {
                        Library.WriteLog("At controller Executing update add", ex);
                        ViewBag.Message = "error";
                    }

                }

                #endregion
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
                string domain = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + url;

                FileInfo file = new FileInfo(domain);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();

                    return true;
                }
            }

            return false;
        }

        public JsonResult DeleteImageEdit(string imgUrl, string category, string position, string id)
        {
            using (var clientDeleteImg = new HttpClient())
            {

                clientDeleteImg.BaseAddress = new Uri(Constants.PostDeleteImage);
                var deleteimgTask = clientDeleteImg.PostAsJsonAsync<String[]>(Constants.PostDeleteImage, new string[] { category, id, position });
                try
                {
                    deleteimgTask.Wait();
                }
                catch (Exception ex)
                {
                    Library.WriteLog("At controller Executing Delete Image", ex);

                    return Json("error", JsonRequestBehavior.AllowGet);
                }

                if (deleteimgTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else if (deleteimgTask.Result.StatusCode == HttpStatusCode.OK)
                {
                    bool isImgDeleted = DeleteImage(new List<string>() { imgUrl });
                    if (!isImgDeleted)
                    {
                        Library.WriteLog("At controller Executing deletig physical image");
                    }

                    var readTask = deleteimgTask.Result.Content.ReadAsAsync<string[]>();
                    readTask.Wait();


                    string[] allImages = readTask.Result;
                    return Json(allImages, JsonRequestBehavior.AllowGet);

                    //return Json("sucess", JsonRequestBehavior.AllowGet);
                }
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeDefaultImage(string category, string position, string id)
        {
            using (var clientchangeDefaultImg = new HttpClient())
            {

                clientchangeDefaultImg.BaseAddress = new Uri(Constants.ChangeDefaultImage);
                var changedefaltimgTask = clientchangeDefaultImg.PostAsJsonAsync<String[]>(Constants.ChangeDefaultImage, new string[] { category, id, position });
                try
                {
                    changedefaltimgTask.Wait();

                    if (changedefaltimgTask.Result.StatusCode == HttpStatusCode.ExpectationFailed)
                    {
                        return Json("error", JsonRequestBehavior.AllowGet);
                    }
                    else if (changedefaltimgTask.Result.StatusCode == HttpStatusCode.OK)
                    {

                        var readTask = changedefaltimgTask.Result.Content.ReadAsAsync<string[]>();
                        readTask.Wait();


                        string[] allImages = readTask.Result;
                        return Json(allImages, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    Library.WriteLog("At controller Executing Delete Image", ex);
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Index1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index1(PostAdd postAdd)
        {
            return View();
        }

    }
}