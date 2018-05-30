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



            using (ClassigooEntities classigooEntities = new ClassigooEntities())
            {

               Add obj =   classigooEntities.Adds.Find(56);

              TransportationVehicle objTV =  classigooEntities.TransportationVehicles.First(x=>x.AddId == obj.AddId);


                //classigooEntities.RealEstates.Add(realEstate);
                //classigooEntities.SaveChanges();
            }


            PostAdd objPost = new PostAdd();
            Guid userId = Guid.Empty;
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

                ViewBag.Name = "Suresh";
                ViewBag.Number = "9014454730";
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

                        img1 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image1.FileName + 1 + "-" + postId);
                        img2 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image2.FileName + 2 + "-" + postId);
                        img3 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image3.FileName + 3 + "-" + postId);
                        img4 = "/ImgColl/" + postAdd.State + "/" + postAdd.District + "/" + postAdd.Mandal + "/" + Path.GetFileName(Image4.FileName + 4 + "-" + postId);

                        Image1.SaveAs(Server.MapPath(img1));
                        Image2.SaveAs(Server.MapPath(img2));
                        Image3.SaveAs(Server.MapPath(img3));
                        Image4.SaveAs(Server.MapPath(img4));
                    }
                    catch (Exception ex)
                    {

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
                            }
                            catch (Exception ex)
                            {

                            }
                            var realEstatePostTask = realEstatepostTask.Result;
                            if (realEstatePostTask.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Home", "User");
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
                            }
                            catch (Exception ex)
                            {

                            }
                            var passengerVResponse = passengerVPostTask.Result;

                            if (passengerVResponse.StatusCode == HttpStatusCode.BadRequest)
                            {

                            }

                            if (passengerVResponse.IsSuccessStatusCode)
                            {

                            }

                            return RedirectToAction("Home", "User");


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



    }
}