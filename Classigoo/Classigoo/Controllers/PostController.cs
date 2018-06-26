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


            PostAdd objPost = new PostAdd();
            Guid userId = Guid.Empty;
            UserDBOperations userObj = new UserDBOperations();
            UserController objUserCont = new UserController();
            objUserCont.ControllerContext = new ControllerContext(this.Request.RequestContext, objUserCont);

            //if (Session["UserId"] != null)
            //{
            //    userId = (Guid)Session["UserId"];
            //}
            userId = objUserCont.GetUserId();
            if (userId != Guid.Empty)
            {
                User user = userObj.GetUser(userId);
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

                //PostDBOperations objPostDbOpareations = new PostDBOperations();
                CommonDBOperations objCommonDBOperations = new CommonDBOperations();
                Add addRecord = objCommonDBOperations.GetAdd(addId);
                objPostAdd.AddId = Convert.ToString(addRecord.AddId);
                objPostAdd.txtTitle = addRecord.Title;
                objPostAdd.hdnCateFristLevel = addRecord.Category;
                objPostAdd.hdnCateSecondLevel = addRecord.SubCategory;
                objPostAdd.State = addRecord.State;
                objPostAdd.District = addRecord.District;
                objPostAdd.Mandal = addRecord.Mandal;
                objPostAdd.LocalArea = addRecord.NearestArea;
                objPostAdd.ddlRentOrSale = addRecord.Type;


                if (addRecord.Category == Constants.RealEstate)
                {
                    #region RealEstate



                    RealEstate realEstateRecord = objCommonDBOperations.GetRealEstate(addId);

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

                    #endregion
                }
                else if (addRecord.Category == Constants.ConstructionVehicle)
                {
                    #region CV
                    ConstructionVehicle constructionVehicleRecord = objCommonDBOperations.GetCV(addId);

                    objPostAdd.CVCompany_list = constructionVehicleRecord.Company;
                    objPostAdd.CVOtherCompany = constructionVehicleRecord.OtherCompany;
                    objPostAdd.hdnCateSecondLevel = constructionVehicleRecord.SubCategory;

                    objPostAdd.txtCV_Price = Convert.ToString(constructionVehicleRecord.Price);
                    objPostAdd.txtAddDetails = constructionVehicleRecord.Description;

                    ViewBag.img1 = constructionVehicleRecord.ImgUrlPrimary;
                    ViewBag.img2 = constructionVehicleRecord.ImgUrlSeconday;
                    ViewBag.img3 = constructionVehicleRecord.ImgUrlThird;
                    ViewBag.img4 = constructionVehicleRecord.ImgUrlFourth;
                    return View(objPostAdd);

                    #endregion
                }
                else if (addRecord.Category == Constants.TransportationVehicle)
                {
                    #region TV

                    TransportationVehicle transportationVehicleRecord = objCommonDBOperations.GetTV(addId);

                    objPostAdd.TVCompany_list = transportationVehicleRecord.Company;
                    objPostAdd.TVOtherCompany = transportationVehicleRecord.OtherCompany;
                    objPostAdd.hdnCateSecondLevel = transportationVehicleRecord.SubCategory;

                    objPostAdd.txtTV_Price = Convert.ToString(transportationVehicleRecord.Price);
                    objPostAdd.txtAddDetails = transportationVehicleRecord.Description;

                    ViewBag.img1 = transportationVehicleRecord.ImgUrlPrimary;
                    ViewBag.img2 = transportationVehicleRecord.ImgUrlSeconday;
                    ViewBag.img3 = transportationVehicleRecord.ImgUrlThird;
                    ViewBag.img4 = transportationVehicleRecord.ImgUrlFourth;
                    return View(objPostAdd);


                    #endregion
                }
                else if (addRecord.Category == Constants.AgriculturalVehicle)
                {

                    #region AV
                    AgriculturalVehicle agriculturalVehicleRecord = objCommonDBOperations.GetAV(addId);

                    objPostAdd.AVCompany_list = agriculturalVehicleRecord.Company;
                    objPostAdd.AVOtherCompany = agriculturalVehicleRecord.OtherCompany;
                    objPostAdd.hdnCateSecondLevel = agriculturalVehicleRecord.SubCategory;

                    objPostAdd.txtAV_Price = Convert.ToString(agriculturalVehicleRecord.Price);
                    objPostAdd.txtAddDetails = agriculturalVehicleRecord.Description;

                    ViewBag.img1 = agriculturalVehicleRecord.ImgUrlPrimary;
                    ViewBag.img2 = agriculturalVehicleRecord.ImgUrlSeconday;
                    ViewBag.img3 = agriculturalVehicleRecord.ImgUrlThird;
                    ViewBag.img4 = agriculturalVehicleRecord.ImgUrlFourth;
                    return View(objPostAdd);



                    #endregion
                }
                else if (addRecord.Category == Constants.PassengerVehicle)
                {
                    #region PV
                    PassengerVehicle passengerVehicleRecord = objCommonDBOperations.GetPV(addId);

                    objPostAdd.PVCompany_list = passengerVehicleRecord.Company;
                    objPostAdd.PVOtherCompany = passengerVehicleRecord.OtherCompany;
                    objPostAdd.hdnCateSecondLevel = passengerVehicleRecord.SubCategory;

                    objPostAdd.txtPV_price = Convert.ToString(passengerVehicleRecord.Price);
                    objPostAdd.PVModel_list = Convert.ToString(passengerVehicleRecord.Model);
                    objPostAdd.txtPV_Year = Convert.ToString(passengerVehicleRecord.Year);
                    objPostAdd.PVfueltype_list = Convert.ToString(passengerVehicleRecord.FuelType);
                    objPostAdd.txtPV_kmdriven = Convert.ToString(passengerVehicleRecord.KMDriven);
                    objPostAdd.txtAddDetails = passengerVehicleRecord.Description;

                    ViewBag.img1 = passengerVehicleRecord.ImgUrlPrimary;
                    ViewBag.img2 = passengerVehicleRecord.ImgUrlSeconday;
                    ViewBag.img3 = passengerVehicleRecord.ImgUrlThird;
                    ViewBag.img4 = passengerVehicleRecord.ImgUrlFourth;
                    return View(objPostAdd);

                    #endregion
                }



            }


            return View();
        }

        [HttpPost]
        public ActionResult Index(PostAdd postAdd, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4, string addId)
        {
           

            PostDBOperations objPostDbOpareations = new PostDBOperations();
            string queryStringForEdit = Request.QueryString["addId"];
            if (postAdd.AddId == null)
            {
                #region AddPost

                int postId = 0;



                #region Login


                Guid userId = Guid.Empty;
                UserController objUserCont = new UserController();
                objUserCont.ControllerContext = new ControllerContext(this.Request.RequestContext, objUserCont);
                userId = objUserCont.GetUserId();
                //if (Session["UserId"] != null)
                //{
                //    userId = (Guid)Session["UserId"];
                //}
                if(userId==Guid.Empty)//User not logged in 
                {
                    UserDBOperations userObj = new UserDBOperations();

                    Guid userExist = userObj.UserExist(postAdd.PhoneNumber, "Custom");
                    if (userExist == Guid.Empty)//user(PhoneNum) doesnot exist so add user
                    {
                        User user = new User();
                        user.MobileNumber = postAdd.PhoneNumber;
                        user.Name = postAdd.Name;
                        user.Type = "Custom";
                        Guid newUserId = userObj.AddUser(user);
                        if (newUserId != null)//user added successfully
                        {
                            userId = newUserId;
                            objUserCont.SetUserId(userId, false);
                            //Session["UserId"] = userId;
                        }
                        else//unbale to add register user
                        {

                        }
                    }
                    else//Phone Num Eixst already
                    {
                        userId = userExist;
                        objUserCont.SetUserId(userId, false);
                        // Session["UserId"] = userId;
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
                    UserId = userId,
                    Created=DateTime.Now
                };


                postId = objPostDbOpareations.PostAdd(add);



                string img1 = string.Empty;
                string img2 = string.Empty;
                string img3 = string.Empty;
                string img4 = string.Empty;
                try
                {
                    Library.WriteLog(Server.MapPath("."));
                    Library.WriteLog(Server.MapPath("/"));
                    Library.WriteLog(Server.MapPath("~"));
                    Library.WriteLog(Server.MapPath(".."));
                    


                    //string pathOfSubdomain = Server.MapPath("~").Split('\\')[]

                    string psth = @"../../img.classigoo.com/img/" + Image1.FileName;

                    Library.WriteLog(Server.MapPath(psth));

                    Image1.SaveAs(Server.MapPath(psth));

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

                    try
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

                        bool isRealestateadedd = objPostDbOpareations.RealEstate(objRealEstate);

                        if (isRealestateadedd)
                        {
                            return RedirectToAction("Home", "User");
                        }
                        else
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



                    #endregion
                }
                else if (postAdd.hdnCateFristLevel == "Construction Vehicles")
                {
                    #region CV
                    try
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


                        bool isCVadedd = objPostDbOpareations.ConstructionVehicle(objConstructionVehicle);

                        if (isCVadedd)
                        {
                            return RedirectToAction("Home", "User");
                        }
                        else
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

                    #endregion
                }
                else if (postAdd.hdnCateFristLevel == "Transportation Vehicles")
                {
                    #region TV
                    try
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

                        bool isTVadedd = objPostDbOpareations.TransportationVehicle(objTransportationVehicle);


                        if (isTVadedd)
                        {
                            return RedirectToAction("Home", "User");
                        }
                        else
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


                    #endregion
                }
                else if (postAdd.hdnCateFristLevel == "Agricultural Vehicles")
                {

                    #region AV
                    try
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

                        bool isAVadedd = objPostDbOpareations.AgriculturalVehicle(objAgriculturalVehicle);

                        if (isAVadedd)
                        {
                            return RedirectToAction("Home", "User");
                        }
                        else
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


                    #endregion
                }
                else if (postAdd.hdnCateFristLevel == "Passenger Vehicles")
                {
                    #region PV
                    try
                    {
                        PassengerVehicle objPassengerVehicle = new PassengerVehicle()
                        {
                            Company = postAdd.PVCompany_list,
                            OtherCompany = postAdd.PVOtherCompany,
                            SubCategory = postAdd.hdnCateSecondLevel,

                            Price = Convert.ToInt32(postAdd.txtPV_price),
                            Model = postAdd.PVModel_list,
                            Year = Convert.ToInt32(postAdd.txtPV_Year),
                            FuelType = postAdd.PVfueltype_list,
                            KMDriven = Convert.ToInt32(postAdd.txtPV_kmdriven),
                            Description = postAdd.txtAddDetails,
                            AddId = postId,
                            ImgUrlPrimary = img1,
                            ImgUrlSeconday = img2,
                            ImgUrlThird = img3,
                            ImgUrlFourth = img4
                        };

                        bool isPVadedd = objPostDbOpareations.PassengerVehicle(objPassengerVehicle);


                        if (isPVadedd)
                        {
                            return RedirectToAction("Home", "User");
                        }
                        else
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
                    #endregion
                }

                #endregion
            }
            else
            {
                #region UpdatePost
                int postId = Convert.ToInt32(postAdd.AddId);

                Guid userId = Guid.Empty;
                UserController objUserCont = new UserController();
                objUserCont.ControllerContext = new ControllerContext(this.Request.RequestContext, objUserCont);
                userId = objUserCont.GetUserId();
                //if (Session["UserId"] != null)
                //{
                //    userId = (Guid)Session["UserId"];
                //}
                if(userId==Guid.Empty)
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


                try
                {
                    bool isAddUpdated = objPostDbOpareations.UpdateAdd(add);

                    if (isAddUpdated)
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


                            //for test
                            if (Image1 != null)
                            {
                                img1 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 1 + Image1.FileName);
                                if (!System.IO.File.Exists(Server.MapPath(img1)))
                                {
                                    Image1.SaveAs(Server.MapPath(img1));
                                }
                                else
                                {
                                    img1 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-copy" + 1 + Image1.FileName);

                                }

                            }

                            if (Image2 != null)
                            {
                                img2 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 2 + Image2.FileName);

                                if (!System.IO.File.Exists(Server.MapPath(img2)))
                                {
                                    Image2.SaveAs(Server.MapPath(img2));
                                }
                                else
                                {
                                    img2 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-copy" + 2 + Image2.FileName);

                                }
                            }
                            if (Image3 != null)
                            {
                                img3 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 3 + Image3.FileName);

                                if (!System.IO.File.Exists(Server.MapPath(img3)))
                                {
                                    Image3.SaveAs(Server.MapPath(img3));
                                }
                                else
                                {
                                    img3 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-copy" + 3 + Image3.FileName);

                                }
                            }
                            if (Image4 != null)
                            {
                                img4 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 4 + Image4.FileName);

                                if (!System.IO.File.Exists(Server.MapPath(img4)))
                                {
                                    Image4.SaveAs(Server.MapPath(img4));
                                }
                                else
                                {
                                    img4 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-copy" + 4 + Image4.FileName);
                                }
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
                            try
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

                                bool isRealEstaeUpdated = objPostDbOpareations.UpdateRealEstate(objRealEstate);

                                if (isRealEstaeUpdated)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else
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


                            #endregion
                        }
                        else if (postAdd.hdnCateFristLevel == "Construction Vehicles")
                        {
                            #region CV
                            try
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

                                bool isConstructionVehicleUpdated = objPostDbOpareations.UpdateCV(objConstructionVehicle);

                                if (isConstructionVehicleUpdated)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else
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



                            #endregion
                        }
                        else if (postAdd.hdnCateFristLevel == "Transportation Vehicles")
                        {
                            #region TV
                            try
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


                                bool isTransportationVehicleUpdated = objPostDbOpareations.UpdateTV(objTransportationVehicle);


                                if (isTransportationVehicleUpdated)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else
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


                            #endregion
                        }
                        else if (postAdd.hdnCateFristLevel == "Agricultural Vehicles")
                        {

                            #region AV
                            try
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


                                bool isAgriculturalVehicleUpdated = objPostDbOpareations.UpdateAV(objAgriculturalVehicle);


                                if (isAgriculturalVehicleUpdated)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else
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


                            #endregion
                        }
                        else if (postAdd.hdnCateFristLevel == "Passenger Vehicles")
                        {
                            #region PV
                            try
                            {
                                PassengerVehicle objPassengerVehicle = new PassengerVehicle()
                                {
                                    Company = postAdd.PVCompany_list,
                                    OtherCompany = postAdd.PVOtherCompany,
                                    SubCategory = postAdd.hdnCateSecondLevel,

                                    Price = Convert.ToInt32(postAdd.txtPV_price),
                                    Model = postAdd.PVModel_list,
                                    Year = Convert.ToInt32(postAdd.txtPV_Year),
                                    FuelType = postAdd.PVfueltype_list,
                                    KMDriven = Convert.ToInt32(postAdd.txtPV_kmdriven),
                                    Description = postAdd.txtAddDetails,
                                    AddId = postId,
                                    ImgUrlPrimary = img1,
                                    ImgUrlSeconday = img2,
                                    ImgUrlThird = img3,
                                    ImgUrlFourth = img4
                                };

                                bool isPassengerVehicleUpdated = objPostDbOpareations.UpdatePV(objPassengerVehicle);

                                if (isPassengerVehicleUpdated)
                                {
                                    return RedirectToAction("Home", "User");
                                }
                                else
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


                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    Library.WriteLog("At controller Executing update add", ex);
                    ViewBag.Message = "error";
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

            bool isDeleted = new PostDBOperations().DeleteAdd(new string[] { type, id });
            if (isDeleted)
            {
                return true;
            }

            return false;
        }

        public bool DeleteImage(List<string> urls)
        {
            string domain = string.Empty;
            foreach (string url in urls)
            {
                try
                {
                    domain = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + url;

                    FileInfo file = new FileInfo(domain);
                    if (file.Exists)//check file exsit or not
                    {
                        file.Delete();

                        return true;
                    }
                }
                catch(Exception ex)
                {
                    Library.WriteLog("At Deleting physical image ImgName - "+domain, ex);
                   
                }
            }

            return false;
        }

        public JsonResult DeleteImageEdit(string imgUrl, string category, string position, string id)
        {


            string[] allImages = new PostDBOperations().DeleteImage(new string[] { category, id, position });

            bool isImgDeleted = DeleteImage(new List<string>() { imgUrl });
            if (!isImgDeleted)
            {
                Library.WriteLog("At controller Executing deletig physical image");
            }

            return Json(allImages, JsonRequestBehavior.AllowGet);

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