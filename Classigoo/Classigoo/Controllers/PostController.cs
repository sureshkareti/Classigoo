using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Classigoo.Models;
using System.Text;
using System.Reflection;

namespace Classigoo.Controllers
{
    public class PostController : Controller
    {
        // GET: Post


        [HttpGet]
        public ActionResult Index()
        {

            //var convertedValue = Convert.ToDouble("0.0");

            //Library.SendEmailFromGodaddy();
            //Library.EmailErrors("Test Message");


            PostAdd objPost = new PostAdd();
            Guid userId = Guid.Empty;
            UserDBOperations userObj = new UserDBOperations();
            UserController objUserCont = new UserController();
            objUserCont.ControllerContext = new ControllerContext(this.Request.RequestContext, objUserCont);

            //if (Session["UserId"] != null)
            //{
            //    userId = (Guid)Session["UserId"];
            //}

            string addId = Request.QueryString["addId"];


            userId = objUserCont.GetUserId();
            if (userId != Guid.Empty)
            {
                User user = userObj.GetUser(userId);
                objPost.PhoneNumber = user.MobileNumber;
                objPost.Name = user.Name;

                ViewBag.Name = user.Name;
                ViewBag.Number = user.MobileNumber;
            }


            if (addId != null)
            {
                if (!User.Identity.IsAuthenticated)
                {

                    return RedirectToAction("Index", "Post");
                }
                else
                {




                    PostAdd objPostAdd = new PostAdd();

                    //PostDBOperations objPostDbOpareations = new PostDBOperations();
                    CommonDBOperations objCommonDBOperations = new CommonDBOperations();
                    Add addRecord = objCommonDBOperations.GetAdd(addId);
                    User user = userObj.GetUser((Guid)addRecord.UserId);

                    Guid adminUserId = userObj.UserExist("1111111111", "Custom");

                    var cookieName = Request.Cookies["ClassigooLoginUser"];
                    var cookieRole = Request.Cookies["ClassigooLoginRole"];


                    bool isAuthorizedUser = false;


                    if (cookieRole != null && cookieRole.Value == "Admin")
                    {
                        isAuthorizedUser = true;
                    }
                    if (cookieRole != null && cookieRole.Value == "Employee")
                    {
                        isAuthorizedUser = true;
                    }
                    else if (userId == addRecord.UserId)//he is owner
                    {
                        isAuthorizedUser = true;
                    }
                    else if (userId == adminUserId)//he is admin
                    {
                        isAuthorizedUser = true;
                    }
                    if (!isAuthorizedUser)
                    {
                        return RedirectToAction("Index", "Post");
                    }
                    else
                    {
                        ViewBag.addId = addId;

                        objPostAdd.AddId = Convert.ToString(addRecord.AddId);
                        objPostAdd.txtTitle = addRecord.Title;
                        objPostAdd.hdnCateFristLevel = addRecord.Category;
                        objPostAdd.hdnCateSecondLevel = addRecord.SubCategory;
                        objPostAdd.State = addRecord.State;
                        objPostAdd.District = addRecord.District;
                        objPostAdd.Mandal = addRecord.Mandal;
                        objPostAdd.LocalArea = addRecord.NearestArea;
                        objPostAdd.ddlRentOrSale = addRecord.Type;

                        ViewBag.Name = user.Name;
                        ViewBag.Number = user.MobileNumber;



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

                            ViewBag.img1 = realEstateRecord.ImgUrlPrimary.Replace("\\", "\\\\"); ;
                            ViewBag.img2 = realEstateRecord.ImgUrlSeconday.Replace("\\", "\\\\"); ;
                            ViewBag.img3 = realEstateRecord.ImgUrlThird.Replace("\\", "\\\\"); ;
                            ViewBag.img4 = realEstateRecord.ImgUrlFourth.Replace("\\", "\\\\"); ;

                            return View(objPostAdd);

                            #endregion
                        }
                        else if (addRecord.Category == Constants.ConstructionVehicle)
                        {
                            #region CV
                            ConstructionVehicle constructionVehicleRecord = objCommonDBOperations.GetCV(addId);

                            objPostAdd.CVCompany_list = constructionVehicleRecord.Company;
                            objPostAdd.CVOtherCompany = constructionVehicleRecord.OtherCompany;
                            objPostAdd.txtCV_Model = constructionVehicleRecord.Model;
                            objPostAdd.txtCV_MYear = constructionVehicleRecord.ManufacturingYear;

                            objPostAdd.hdnCateSecondLevel = constructionVehicleRecord.SubCategory;

                            objPostAdd.txtCV_Price = Convert.ToString(constructionVehicleRecord.Price);
                            objPostAdd.txtAddDetails = constructionVehicleRecord.Description;

                            ViewBag.img1 = constructionVehicleRecord.ImgUrlPrimary.Replace("\\", "\\\\");
                            ViewBag.img2 = constructionVehicleRecord.ImgUrlSeconday.Replace("\\", "\\\\"); ;
                            ViewBag.img3 = constructionVehicleRecord.ImgUrlThird.Replace("\\", "\\\\"); ;
                            ViewBag.img4 = constructionVehicleRecord.ImgUrlFourth.Replace("\\", "\\\\"); ;
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

                            objPostAdd.txtTV_Model = transportationVehicleRecord.Model;
                            objPostAdd.txtTV_MYear = transportationVehicleRecord.ManufacturingYear;

                            objPostAdd.txtTV_Price = Convert.ToString(transportationVehicleRecord.Price);
                            objPostAdd.txtAddDetails = transportationVehicleRecord.Description;

                            ViewBag.img1 = transportationVehicleRecord.ImgUrlPrimary.Replace("\\", "\\\\"); ;
                            ViewBag.img2 = transportationVehicleRecord.ImgUrlSeconday.Replace("\\", "\\\\"); ;
                            ViewBag.img3 = transportationVehicleRecord.ImgUrlThird.Replace("\\", "\\\\"); ;
                            ViewBag.img4 = transportationVehicleRecord.ImgUrlFourth.Replace("\\", "\\\\"); ;
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

                            objPostAdd.txtAV_Model = agriculturalVehicleRecord.Model;
                            objPostAdd.txtAV_MYear = agriculturalVehicleRecord.ManufacturingYear;

                            objPostAdd.txtAV_Price = Convert.ToString(agriculturalVehicleRecord.Price);
                            objPostAdd.txtAddDetails = agriculturalVehicleRecord.Description;

                            ViewBag.img1 = agriculturalVehicleRecord.ImgUrlPrimary.Replace("\\", "\\\\"); ;
                            ViewBag.img2 = agriculturalVehicleRecord.ImgUrlSeconday.Replace("\\", "\\\\"); ;
                            ViewBag.img3 = agriculturalVehicleRecord.ImgUrlThird.Replace("\\", "\\\\"); ;
                            ViewBag.img4 = agriculturalVehicleRecord.ImgUrlFourth.Replace("\\", "\\\\"); ;
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

                            objPostAdd.txtPV_Model = Convert.ToString(passengerVehicleRecord.Model);

                            objPostAdd.txtPV_Year = Convert.ToString(passengerVehicleRecord.Year);
                            objPostAdd.PVfueltype_list = Convert.ToString(passengerVehicleRecord.FuelType);
                            objPostAdd.txtPV_kmdriven = Convert.ToString(passengerVehicleRecord.KMDriven);
                            objPostAdd.txtAddDetails = passengerVehicleRecord.Description;

                            ViewBag.img1 = passengerVehicleRecord.ImgUrlPrimary.Replace("\\", "\\\\"); ;
                            ViewBag.img2 = passengerVehicleRecord.ImgUrlSeconday.Replace("\\", "\\\\"); ;
                            ViewBag.img3 = passengerVehicleRecord.ImgUrlThird.Replace("\\", "\\\\"); ;
                            ViewBag.img4 = passengerVehicleRecord.ImgUrlFourth.Replace("\\", "\\\\"); ;
                            return View(objPostAdd);

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
            var cookieName = Request.Cookies["ClassigooLoginUser"];
            var cookieRole = Request.Cookies["ClassigooLoginRole"];


            PostDBOperations objPostDbOpareations = new PostDBOperations();
            Communication objComm = new Communication();
            UserDBOperations userObj = new UserDBOperations();
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
                if (userId == Guid.Empty)//User not logged in 
                {


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

                            if (cookieRole == null && (cookieRole.Value == "Admin" || cookieRole.Value == "Employee"))
                            {
                                objUserCont.SetUserId(userId, false);
                            } 
                            
                            //Session["UserId"] = userId;
                        }
                        else//unbale to add register user
                        {

                        }
                    }
                    else//Phone Num Eixst already
                    {
                        userId = userExist;                       
                        if (cookieRole == null && (cookieRole.Value == "Admin" || cookieRole.Value == "Employee"))
                        {
                            objUserCont.SetUserId(userId, false);
                        }

                        // Session["UserId"] = userId;
                    }
                }



                #endregion


                //  Guid userId = new Guid("280bf190-3fe3-4e1c-8f6e-e66edd7e272f");
                string postedBy = string.Empty;
                if (cookieRole != null && cookieRole.Value == "Admin")
                {
                    postedBy = cookieName.Value;
                }
                else if (cookieRole != null && cookieRole.Value == "Employee")
                {
                    postedBy = cookieName.Value;
                }

                Add add = new Add()
                {
                    Category = postAdd.hdnCateFristLevel,
                    SubCategory = postAdd.hdnCateSecondLevel,
                    State = postAdd.State.Trim(),
                    District = postAdd.District.Trim(),
                    Mandal = postAdd.Mandal.Trim(),
                    NearestArea = postAdd.LocalArea,
                    Title = postAdd.txtTitle,
                    Type = postAdd.ddlRentOrSale,
                    Status = Constants.PendingSatus,
                    UserId = userId,
                    Created = CustomActions.GetCurrentISTTime(),
                    PostedBy= postedBy
                };


                postId = objPostDbOpareations.PostAdd(add);



                string img1 = string.Empty;
                string img2 = string.Empty;
                string img3 = string.Empty;
                string img4 = string.Empty;
                try
                {


                    #region Images Saving Subdomain

                    string[] pathSplits = Server.MapPath("~").Split('\\');
                    string combinedPath = pathSplits[0] + "\\" + pathSplits[1] + "\\" + pathSplits[2] + "\\img.classigoo.com";
                    CreateFolderSubdomain(combinedPath + "\\Img\\" + postAdd.State.Trim());
                    CreateFolderSubdomain(combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim());
                    CreateFolderSubdomain(combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim());


                    if (Image1 != null)
                    {
                        string img1SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 1 + Image1.FileName);
                        Image1.SaveAs(img1SubdomainPath);

                        img1 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 1 + Image1.FileName);

                    }

                    if (Image2 != null)
                    {
                        string img2SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 2 + Image2.FileName);
                        Image2.SaveAs(img2SubdomainPath);

                        img2 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 2 + Image2.FileName);

                    }
                    if (Image3 != null)
                    {
                        string img3SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 3 + Image3.FileName);
                        Image3.SaveAs(img3SubdomainPath);

                        img3 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 3 + Image3.FileName);

                    }
                    if (Image4 != null)
                    {
                        string img4SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 4 + Image4.FileName);
                        Image4.SaveAs(img4SubdomainPath);

                        img4 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 4 + Image4.FileName);

                    }

                    #endregion

                    #region Images Saving Local

                    //CreateFolder("/ImgColl/" + postAdd.State);
                    //CreateFolder("/ImgColl/" + postAdd.State + "/" + postAdd.District);
                    //CreateFolder("/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal);


                    //if (Image1 != null)
                    //{
                    //    img1 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 1 + Image1.FileName);
                    //    Image1.SaveAs(Server.MapPath(img1));
                    //}

                    //if (Image2 != null)
                    //{
                    //    img2 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 2 + Image2.FileName);
                    //    Image2.SaveAs(Server.MapPath(img2));
                    //}
                    //if (Image3 != null)
                    //{

                    //    img3 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 3 + Image3.FileName);
                    //    Image3.SaveAs(Server.MapPath(img3));
                    //}
                    //if (Image4 != null)
                    //{

                    //    img4 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 4 + Image4.FileName);
                    //    Image4.SaveAs(Server.MapPath(img4));
                    //}
                    #endregion


                    //if (img2 == string.Empty)
                    //{
                    //    img2 = "http://www.classigoo.com/images/upimglogo1.png";
                    //}
                    //if (img3 == string.Empty)
                    //{
                    //    img3 = "http://www.classigoo.com/images/upimglogo1.png";
                    //}
                    //if (img4 == string.Empty)
                    //{
                    //    img4 = "http://www.classigoo.com/images/upimglogo1.png";
                    //}

                }
                catch (Exception ex)
                {
                    Library.WriteLog("At saving images", ex);
                    new PostDBOperations().DeleteImageLocal(new List<string>() { img1, img2, img3, img4 });
                    DeleteAdd("", Convert.ToString(postId));

                    ViewBag.Message = "error";
                }
                #region successmsg
                string homePageUrl = Constants.DomainName + "/User/Home";
                string addUrl = Constants.DomainName + "/List/PreviewAdd?addId=" + postId + "";
                var message = new StringBuilder();
                message.AppendLine("Congracts," + postAdd.Name + "!");
                message.AppendLine("Your Ad published successfully. ");
                message.AppendLine("View and manage adds here: ");
                message.AppendLine(homePageUrl);
                var body = new StringBuilder();
                body.AppendLine("Hello Admin,");
                body.AppendLine();
                body.AppendLine("New Ad was published.");
                body.AppendLine();
                body.AppendLine("AdId: " + postId + "");
                body.AppendLine();
                body.AppendLine("Preview Add: <a href=\"" + addUrl + "\">" + addUrl + "</a>");
                body.AppendLine();
                #endregion
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
                            ViewBag.addId = postId;
                            ViewBag.Message = "sucess";
                            objComm.SendMessage(postAdd.PhoneNumber, message.ToString());
                            //Library.SendEmail(postId.ToString());
                            Library.SendEmailFromGodaddy("New Ad Published", body.ToString());
                            //return RedirectToAction("Home", "User");
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

                            ManufacturingYear = postAdd.txtCV_MYear,
                            Model = postAdd.txtCV_Model,

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
                            ViewBag.addId = postId;
                            ViewBag.Message = "sucess";
                            objComm.SendMessage(postAdd.PhoneNumber, message.ToString());
                            //Library.SendEmail(postId.ToString());
                            Library.SendEmailFromGodaddy("New Ad Published", body.ToString());
                            //return RedirectToAction("Home", "User");
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

                            ManufacturingYear = postAdd.txtTV_MYear,
                            Model = postAdd.txtTV_Model,

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
                            ViewBag.addId = postId;
                            ViewBag.Message = "sucess";
                            objComm.SendMessage(postAdd.PhoneNumber, message.ToString());
                            // Library.SendEmail(postId.ToString());
                            Library.SendEmailFromGodaddy("New Ad Published", body.ToString());
                            //return RedirectToAction("Home", "User");
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

                            ManufacturingYear = postAdd.txtAV_MYear,
                            Model = postAdd.txtAV_Model,

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
                            ViewBag.addId = postId;
                            ViewBag.Message = "sucess";
                            objComm.SendMessage(postAdd.PhoneNumber, message.ToString());
                            // Library.SendEmail(postId.ToString());
                            Library.SendEmailFromGodaddy("New Ad Published", body.ToString());
                            //return RedirectToAction("Home", "User");
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

                        string model = string.Empty;

                        if (postAdd.hdnCateSecondLevel.Trim() == "Cars" || postAdd.hdnCateSecondLevel.Trim() == "Bikes")
                        {
                            model = postAdd.PVModel_list;
                        }
                        else
                        {
                            model = postAdd.txtPV_Model;
                        }




                        PassengerVehicle objPassengerVehicle = new PassengerVehicle()
                        {
                            Company = postAdd.PVCompany_list,
                            OtherCompany = postAdd.PVOtherCompany,
                            SubCategory = postAdd.hdnCateSecondLevel,

                            Price = Convert.ToInt32(postAdd.txtPV_price),
                            Model = model, //postAdd.PVModel_list,
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
                            ViewBag.addId = postId;
                            ViewBag.Message = "sucess";
                            objComm.SendMessage(postAdd.PhoneNumber, message.ToString());
                            // Library.SendEmail(postId.ToString());
                            Library.SendEmailFromGodaddy("New Ad Published", body.ToString());
                            //return RedirectToAction("Home", "User");
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

              

                bool isAuthorizedUser = false;


                if (cookieRole != null && cookieRole.Value == "Admin")
                {
                    isAuthorizedUser = true;
                }
                else if (cookieRole != null && cookieRole.Value == "Employee")
                {
                    isAuthorizedUser = true;
                }
                else if (userId == Guid.Empty)
                {
                    ViewBag.Message = "nologin";
                    return View();
                }
                else if ((Session["LoginUserRole"] != null) && ((string)Session["LoginUserRole"] != "Admin"))
                {
                     isAuthorizedUser = true;
                }
                else
                {
                    isAuthorizedUser = false;
                    MessageDBOperations objMsgDbOper = new MessageDBOperations();
                    Guid addOwnerUserId = (Guid)objMsgDbOper.GetAddOwnerUserId(postId);
                    Guid adminUserId = userObj.UserExist("1111111111", "Custom");
                    if (userId == addOwnerUserId)//he is owner
                    {
                        isAuthorizedUser = true;
                    }
                    else if (userId == adminUserId)//he is admin
                    {
                        isAuthorizedUser = true;
                    }
                    if (!isAuthorizedUser)
                    {
                        ViewBag.Message = "UnAuthorizedToUpdate";
                        return View();
                    }
                }

                Add add = new Add()
                {
                    Category = postAdd.hdnCateFristLevel,
                    SubCategory = postAdd.hdnCateSecondLevel,
                    State = postAdd.State.Trim(),
                    District = postAdd.District.Trim(),
                    Mandal = postAdd.Mandal.Trim(),
                    NearestArea = postAdd.LocalArea,
                    Title = postAdd.txtTitle,
                    Type = postAdd.ddlRentOrSale,
                    UserId = userId,
                    AddId = postId
                };


                try
                {

                    //to find isCategoryChange
                    bool isCategoryCnaged = false;
                    bool isSubCategoryCnaged = false;
                    CommonDBOperations objCommonDBOperations = new CommonDBOperations();
                    Add addRecord = objCommonDBOperations.GetAdd(postId.ToString());

                    if (addRecord != null)
                    {
                        if (addRecord.Category != postAdd.hdnCateFristLevel)
                        {
                            isCategoryCnaged = true;

                        }

                        string img1 = string.Empty;
                        string img2 = string.Empty;
                        string img3 = string.Empty;
                        string img4 = string.Empty;

                        bool isChilUpdated = false;

                        try
                        {

                            #region Images SavingSubdomain

                            string[] pathSplits = Server.MapPath("~").Split('\\');

                            string combinedPath = pathSplits[0] + "\\" + pathSplits[1] + "\\" + pathSplits[2] + "\\img.classigoo.com";

                            CreateFolderSubdomain(combinedPath + "\\Img\\" + postAdd.State.Trim());
                            CreateFolderSubdomain(combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim());
                            CreateFolderSubdomain(combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim());


                            if (Image1 != null)
                            {
                                string img1SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 1 + Image1.FileName);

                                if (!System.IO.File.Exists(img1SubdomainPath))
                                {
                                    Image1.SaveAs(img1SubdomainPath);
                                    img1 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 1 + Image1.FileName);
                                }
                                else
                                {
                                    //System.IO.FileInfo obj = new FileInfo(img1SubdomainPath);
                                    //string previousname = "copy-"+ obj.Name;

                                    img1SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-copy" + 1 + Image1.FileName);
                                    Image1.SaveAs(img1SubdomainPath);
                                    img1 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-copy" + 1 + Image1.FileName);

                                }
                            }

                            if (Image2 != null)
                            {
                                string img2SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 2 + Image2.FileName);

                                if (!System.IO.File.Exists(img2SubdomainPath))
                                {
                                    Image2.SaveAs(img2SubdomainPath);
                                    img2 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 2 + Image2.FileName);
                                }
                                else
                                {
                                    img2SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-copy" + 2 + Image2.FileName);
                                    Image2.SaveAs(img2SubdomainPath);
                                    img2 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-copy" + 2 + Image2.FileName);

                                }


                            }
                            if (Image3 != null)
                            {
                                string img3SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 3 + Image3.FileName);

                                if (!System.IO.File.Exists(img3SubdomainPath))
                                {
                                    Image3.SaveAs(img3SubdomainPath);

                                    img3 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 3 + Image3.FileName);
                                }
                                else
                                {
                                    img3SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-copy" + 3 + Image3.FileName);
                                    Image3.SaveAs(img3SubdomainPath);

                                    img3 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-copy" + 3 + Image3.FileName);

                                }

                            }
                            if (Image4 != null)
                            {
                                string img4SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 4 + Image4.FileName);
                                Image4.SaveAs(img4SubdomainPath);

                                if (!System.IO.File.Exists(img4SubdomainPath))
                                {
                                    Image4.SaveAs(img4SubdomainPath);
                                    img4 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-" + 4 + Image4.FileName);
                                }
                                else
                                {
                                    img4SubdomainPath = combinedPath + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-copy" + 4 + Image4.FileName);
                                    Image4.SaveAs(img4SubdomainPath);
                                    img4 = "http:\\\\img.classigoo.com" + "\\Img\\" + postAdd.State.Trim() + "\\" + postAdd.District.Trim() + "\\" + postAdd.Mandal.Trim() + "\\" + Path.GetFileName(postId + "-copy" + 4 + Image4.FileName);

                                }


                            }

                            #endregion

                            #region Updating Images Local

                            //CreateFolder("/ImgColl/" + postAdd.State);
                            //CreateFolder("/ImgColl/" + postAdd.State + "/" + postAdd.District);
                            //CreateFolder("/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal);


                            ////for test
                            //if (Image1 != null)
                            //{
                            //    img1 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 1 + Image1.FileName);
                            //    if (!System.IO.File.Exists(Server.MapPath(img1)))
                            //    {
                            //        Image1.SaveAs(Server.MapPath(img1));
                            //    }
                            //    else
                            //    {
                            //        img1 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-copy" + 1 + Image1.FileName);
                            //        Image1.SaveAs(Server.MapPath(img1));
                            //    }

                            //}

                            //if (Image2 != null)
                            //{
                            //    img2 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 2 + Image2.FileName);

                            //    if (!System.IO.File.Exists(Server.MapPath(img2)))
                            //    {
                            //        Image2.SaveAs(Server.MapPath(img2));
                            //    }
                            //    else
                            //    {
                            //        img2 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-copy" + 2 + Image2.FileName);
                            //        Image2.SaveAs(Server.MapPath(img2));
                            //    }
                            //}

                            //if (Image3 != null)
                            //{
                            //    img3 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 3 + Image3.FileName);

                            //    if (!System.IO.File.Exists(Server.MapPath(img3)))
                            //    {
                            //        Image3.SaveAs(Server.MapPath(img3));
                            //    }
                            //    else
                            //    {
                            //        img3 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-copy" + 3 + Image3.FileName);
                            //        Image3.SaveAs(Server.MapPath(img3));
                            //    }
                            //}

                            //if (Image4 != null)
                            //{
                            //    img4 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-" + 4 + Image4.FileName);

                            //    if (!System.IO.File.Exists(Server.MapPath(img4)))
                            //    {
                            //        Image4.SaveAs(Server.MapPath(img4));
                            //    }
                            //    else
                            //    {
                            //        img4 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(postId + "-copy" + 4 + Image4.FileName);
                            //        Image4.SaveAs(Server.MapPath(img4));
                            //    }
                            //}

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            Library.WriteLog("At saving images", ex);
                            new PostDBOperations().DeleteImageLocal(new List<string>() { img1, img2, img3, img4 });
                            DeleteAdd("", Convert.ToString(postId));

                            ViewBag.Message = "error";
                        }

                        string[] currentIamgeUrl = new string[4] { img1, img2, img3, img4 };

                        string[] updatedImagesUrls = GetOldImages(addRecord.Category, currentIamgeUrl, postAdd.AddId);


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
                                    ImgUrlPrimary = updatedImagesUrls[0],
                                    ImgUrlSeconday = updatedImagesUrls[1],
                                    ImgUrlThird = updatedImagesUrls[2],
                                    ImgUrlFourth = updatedImagesUrls[3]
                                };

                                if (isCategoryCnaged)
                                {

                                    bool isRealestateadedd = objPostDbOpareations.RealEstate(objRealEstate);

                                    if (isRealestateadedd)
                                    {
                                        objPostDbOpareations.DeleteAddWhenCategoryChange(Convert.ToString(postId));

                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        //return RedirectToAction("Home", "User");

                                        isChilUpdated = true;
                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }
                                else
                                {
                                    bool isRealEstaeUpdated = objPostDbOpareations.UpdateRealEstate(objRealEstate);

                                    if (isRealEstaeUpdated)
                                    {
                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        //return RedirectToAction("Home", "User");
                                        isChilUpdated = true;
                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At update  realestate", ex);
                                //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                ViewBag.Message = "error";

                                isChilUpdated = false;
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

                                    ManufacturingYear = postAdd.txtCV_MYear,
                                    Model = postAdd.txtCV_Model,

                                    Price = Convert.ToInt32(postAdd.txtCV_Price),
                                    Description = postAdd.txtAddDetails,
                                    AddId = postId,
                                    ImgUrlPrimary = updatedImagesUrls[0],
                                    ImgUrlSeconday = updatedImagesUrls[1],
                                    ImgUrlThird = updatedImagesUrls[2],
                                    ImgUrlFourth = updatedImagesUrls[3]
                                };

                                if (isCategoryCnaged)
                                {
                                    bool isConstructionAdded = objPostDbOpareations.ConstructionVehicle(objConstructionVehicle);

                                    if (isConstructionAdded)
                                    {
                                        objPostDbOpareations.DeleteAddWhenCategoryChange(Convert.ToString(postId));

                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        isChilUpdated = true;
                                        //return RedirectToAction("Home", "User");
                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }
                                else
                                {
                                    bool isConstructionVehicleUpdated = objPostDbOpareations.UpdateCV(objConstructionVehicle);

                                    if (isConstructionVehicleUpdated)
                                    {
                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        isChilUpdated = true;
                                        //return RedirectToAction("Home", "User");
                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At create updationg construction vehicles", ex);
                                //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                ViewBag.Message = "error";
                                isChilUpdated = false;
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

                                    ManufacturingYear = postAdd.txtTV_MYear,
                                    Model = postAdd.txtTV_Model,

                                    Price = Convert.ToInt32(postAdd.txtTV_Price),
                                    Description = postAdd.txtAddDetails,
                                    AddId = postId,
                                    ImgUrlPrimary = updatedImagesUrls[0],
                                    ImgUrlSeconday = updatedImagesUrls[1],
                                    ImgUrlThird = updatedImagesUrls[2],
                                    ImgUrlFourth = updatedImagesUrls[3]
                                };

                                if (isCategoryCnaged)
                                {
                                    bool isTVadedd = objPostDbOpareations.TransportationVehicle(objTransportationVehicle);


                                    if (isTVadedd)
                                    {
                                        objPostDbOpareations.DeleteAddWhenCategoryChange(Convert.ToString(postId));
                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        isChilUpdated = true;
                                        //return RedirectToAction("Home", "User");
                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }
                                else
                                {
                                    bool isTransportationVehicleUpdated = objPostDbOpareations.UpdateTV(objTransportationVehicle);


                                    if (isTransportationVehicleUpdated)
                                    {
                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        //return RedirectToAction("Home", "User");
                                        isChilUpdated = true;
                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At create updating transportation vehicles", ex);

                                //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
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

                                    ManufacturingYear = postAdd.txtAV_MYear,
                                    Model = postAdd.txtAV_Model,

                                    Price = Convert.ToInt32(postAdd.txtAV_Price),
                                    Description = postAdd.txtAddDetails,
                                    AddId = postId,
                                    ImgUrlPrimary = updatedImagesUrls[0],
                                    ImgUrlSeconday = updatedImagesUrls[1],
                                    ImgUrlThird = updatedImagesUrls[2],
                                    ImgUrlFourth = updatedImagesUrls[3]
                                };

                                if (isCategoryCnaged)
                                {
                                    bool isAVadedd = objPostDbOpareations.AgriculturalVehicle(objAgriculturalVehicle);

                                    if (isAVadedd)
                                    {
                                        objPostDbOpareations.DeleteAddWhenCategoryChange(Convert.ToString(postId));
                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        //return RedirectToAction("Home", "User");
                                        isChilUpdated = true;

                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }
                                else
                                {
                                    bool isAgriculturalVehicleUpdated = objPostDbOpareations.UpdateAV(objAgriculturalVehicle);


                                    if (isAgriculturalVehicleUpdated)
                                    {
                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        isChilUpdated = true;
                                        //return RedirectToAction("Home", "User");
                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At create updating agricultural vehicles", ex);

                                //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                ViewBag.Message = "error";
                                isChilUpdated = false;
                                return View();
                            }


                            #endregion
                        }
                        else if (postAdd.hdnCateFristLevel == "Passenger Vehicles")
                        {
                            #region PV
                            try
                            {
                                string modelUpdate = string.Empty;

                                if (postAdd.hdnCateSecondLevel.Trim() == "Cars" || postAdd.hdnCateSecondLevel.Trim() == "Bikes")
                                {
                                    modelUpdate = postAdd.PVModel_list;
                                }
                                else
                                {
                                    modelUpdate = postAdd.txtPV_Model;
                                }

                                PassengerVehicle objPassengerVehicle = new PassengerVehicle()
                                {
                                    Company = postAdd.PVCompany_list,
                                    OtherCompany = postAdd.PVOtherCompany,
                                    SubCategory = postAdd.hdnCateSecondLevel,

                                    Price = Convert.ToInt32(postAdd.txtPV_price),
                                    Model = modelUpdate,//postAdd.PVModel_list,
                                    Year = Convert.ToInt32(postAdd.txtPV_Year),
                                    FuelType = postAdd.PVfueltype_list,
                                    KMDriven = Convert.ToInt32(postAdd.txtPV_kmdriven),
                                    Description = postAdd.txtAddDetails,
                                    AddId = postId,
                                    ImgUrlPrimary = updatedImagesUrls[0],
                                    ImgUrlSeconday = updatedImagesUrls[1],
                                    ImgUrlThird = updatedImagesUrls[2],
                                    ImgUrlFourth = updatedImagesUrls[3]
                                };

                                if (isCategoryCnaged)
                                {
                                    bool isPVadedd = objPostDbOpareations.PassengerVehicle(objPassengerVehicle);


                                    if (isPVadedd)
                                    {
                                        objPostDbOpareations.DeleteAddWhenCategoryChange(Convert.ToString(postId));
                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        //return RedirectToAction("Home", "User");
                                        isChilUpdated = true;
                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }
                                else
                                {
                                    bool isPassengerVehicleUpdated = objPostDbOpareations.UpdatePV(objPassengerVehicle);

                                    if (isPassengerVehicleUpdated)
                                    {
                                        ViewBag.addId = postId;
                                        ViewBag.Message = "updated";
                                        isChilUpdated = true;
                                        //return RedirectToAction("Home", "User");
                                    }
                                    else
                                    {
                                        //DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                        ViewBag.Message = "error";
                                        isChilUpdated = false;
                                        return View();
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                Library.WriteLog("At create updating passenger vehicles", ex);

                                DeleteAdd(postAdd.hdnCateFristLevel, Convert.ToString(postId));
                                ViewBag.Message = "error";
                                return View();
                            }


                            #endregion
                        }

                        if (isChilUpdated)
                        {
                            bool isAddUpdated = objPostDbOpareations.UpdateAdd(add);

                            if (cookieRole != null && cookieRole.Value == "Admin" )
                            {
                                return RedirectToAction("Dashboard", "Admin");
                            }
                            else if (cookieRole != null &&  cookieRole.Value == "Employee")
                            {
                                return RedirectToAction("EmployeeDashboard", "Admin");
                            }                          
                        }


                    }
                    else
                    {
                        Library.WriteLog("No Records were found !!! with id " + postId);
                    }
                }
                catch (Exception ex)
                {
                    Library.WriteLog("At controller Executing update add", ex);
                    ViewBag.Message = "error";
                }

                #endregion
            }

            if (cookieRole != null && cookieRole.Value == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else if (cookieRole != null && cookieRole.Value == "Employee")
            {
                return RedirectToAction("EmployeeDashboard", "Admin");
            }

            return View();
        }

        public ActionResult Delete()
        {
            string addId = Request.QueryString["addId"];
            var cookieRole = Request.Cookies["ClassigooLoginRole"];
            if (addId != null)
            {
                bool isDeleted = new PostDBOperations().DeleteAdd(addId);
                if (isDeleted)
                {
                    if (cookieRole != null && cookieRole.Value == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (cookieRole != null && cookieRole.Value == "Employee")
                    {
                        return RedirectToAction("EmployeeDashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Home", "User");
                    }
                   
                }
                else
                {
                    ViewBag.Message = "error";
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

        public void CreateFolderSubdomain(string path)
        {
            //  string dirPath = Server.MapPath(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
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

        public JsonResult DeleteImageEdit(string imgUrl, string category, string position, string id)
        {
            Library.WriteLog(imgUrl);

            string[] allImages = new PostDBOperations().DeleteImage(new string[] { category, id, position });

            bool isImgDeleted = new PostDBOperations().DeleteImageLocal(new List<string>() { imgUrl });
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

        public string[] GetOldImages(string category, string[] Oldimages, string addId)
        {

            CommonDBOperations objCommonDBOperation = new CommonDBOperations();
            string[] imges = new string[4];

            #region switch 
            switch (category)
            {
                case Constants.AgriculturalVehicle:
                    var av = objCommonDBOperation.GetAV(addId);

                    if (av != null)
                    {

                        if (Oldimages[0] == string.Empty)
                        {
                            imges[0] = av.ImgUrlPrimary;
                        }
                        else
                        {
                            imges[0] = Oldimages[0];
                        }

                        if (Oldimages[1] == string.Empty)
                        {
                            imges[1] = av.ImgUrlSeconday;
                        }
                        else
                        {
                            imges[1] = Oldimages[1];
                        }

                        if (Oldimages[2] == string.Empty)
                        {
                            imges[2] = av.ImgUrlThird;
                        }
                        else
                        {
                            imges[2] = Oldimages[2];
                        }

                        if (Oldimages[3] == string.Empty)
                        {
                            imges[3] = av.ImgUrlFourth;
                        }
                        else
                        {
                            imges[3] = Oldimages[3];
                        }
                    }
                    break;
                case Constants.TransportationVehicle:
                    var tv = objCommonDBOperation.GetTV(addId);

                    if (tv != null)
                    {

                        if (Oldimages[0] == string.Empty)
                        {
                            imges[0] = tv.ImgUrlPrimary;
                        }
                        else
                        {
                            imges[0] = Oldimages[0];
                        }

                        if (Oldimages[1] == string.Empty)
                        {
                            imges[1] = tv.ImgUrlSeconday;
                        }
                        else
                        {
                            imges[1] = Oldimages[1];
                        }

                        if (Oldimages[2] == string.Empty)
                        {
                            imges[2] = tv.ImgUrlThird;
                        }
                        else
                        {
                            imges[2] = Oldimages[2];
                        }

                        if (Oldimages[3] == string.Empty)
                        {
                            imges[3] = tv.ImgUrlFourth;
                        }
                        else
                        {
                            imges[3] = Oldimages[3];
                        }
                    }
                    break;
                case Constants.PassengerVehicle:
                    var pv = objCommonDBOperation.GetPV(addId);

                    if (pv != null)
                    {

                        if (Oldimages[0] == string.Empty)
                        {
                            imges[0] = pv.ImgUrlPrimary;
                        }
                        else
                        {
                            imges[0] = Oldimages[0];
                        }

                        if (Oldimages[1] == string.Empty)
                        {
                            imges[1] = pv.ImgUrlSeconday;
                        }
                        else
                        {
                            imges[1] = Oldimages[1];
                        }

                        if (Oldimages[2] == string.Empty)
                        {
                            imges[2] = pv.ImgUrlThird;
                        }
                        else
                        {
                            imges[2] = Oldimages[2];
                        }

                        if (Oldimages[3] == string.Empty)
                        {
                            imges[3] = pv.ImgUrlFourth;
                        }
                        else
                        {
                            imges[3] = Oldimages[3];
                        }
                    }
                    break;
                case Constants.RealEstate:
                    var re = objCommonDBOperation.GetRealEstate(addId);

                    if (re != null)
                    {

                        if (Oldimages[0] == string.Empty)
                        {
                            imges[0] = re.ImgUrlPrimary;
                        }
                        else
                        {
                            imges[0] = Oldimages[0];
                        }

                        if (Oldimages[1] == string.Empty)
                        {
                            imges[1] = re.ImgUrlSeconday;
                        }
                        else
                        {
                            imges[1] = Oldimages[1];
                        }

                        if (Oldimages[2] == string.Empty)
                        {
                            imges[2] = re.ImgUrlThird;
                        }
                        else
                        {
                            imges[2] = Oldimages[2];
                        }

                        if (Oldimages[3] == string.Empty)
                        {
                            imges[3] = re.ImgUrlFourth;
                        }
                        else
                        {
                            imges[3] = Oldimages[3];
                        }
                    }
                    break;
                case Constants.ConstructionVehicle:
                    var cv = objCommonDBOperation.GetCV(addId);

                    if (cv != null)
                    {

                        if (Oldimages[0] == string.Empty)
                        {
                            imges[0] = cv.ImgUrlPrimary;
                        }
                        else
                        {
                            imges[0] = Oldimages[0];
                        }

                        if (Oldimages[1] == string.Empty)
                        {
                            imges[1] = cv.ImgUrlSeconday;
                        }
                        else
                        {
                            imges[1] = Oldimages[1];
                        }

                        if (Oldimages[2] == string.Empty)
                        {
                            imges[2] = cv.ImgUrlThird;
                        }
                        else
                        {
                            imges[2] = Oldimages[2];
                        }

                        if (Oldimages[3] == string.Empty)
                        {
                            imges[3] = cv.ImgUrlFourth;
                        }
                        else
                        {
                            imges[3] = Oldimages[3];
                        }
                    }
                    break;


            }
            #endregion

            return imges;
        }

    }
}