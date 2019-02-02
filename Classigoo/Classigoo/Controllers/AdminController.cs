using Classigoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using Classigoo.Models.Search;
using Newtonsoft.Json;
using Classigoo.Business;

namespace Classigoo.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorization(LoginPage = "~/Login/Index")]
        public ActionResult Dashboard()
        {
            var cookieRole = Request.Cookies["ClassigooLoginRole"];

            if (cookieRole != null && cookieRole.Value == "Employee")
            {
                return RedirectToAction("EmployeeDashboard", "Admin");
            }

            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            IEnumerable<CustomSurvey> surveyColl = new List<CustomSurvey>();
            AdminDashboard adminDashboard = new AdminDashboard();
            try
            {
                UserDBOperations db = new UserDBOperations();
                addColl = db.GetAdminAdds();
                surveyColl = db.GetSurvey();
                adminDashboard.SurveyColl = surveyColl;
                adminDashboard.AddsColl = addColl;
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Admin", ex);
            }

            ViewBag.role = "Admin";
            Session["LoginUserRole"] = "Admin";
            return View(adminDashboard);

        }

        [CustomAuthorization(LoginPage = "~/Login/Index")]
        public ActionResult EmployeeDashboard()
        {
            AdminService objAdmin = new AdminService();
            var cookieName = Request.Cookies["ClassigooLoginUser"];
            IEnumerable<AdminAdd> empAddColl = objAdmin.GetEmpAdds(cookieName.Value);

            ViewBag.role = "Employee";
            Session["LoginUserRole"] = "Employee";
            return View(empAddColl);
        }

        [CustomAuthorization(LoginPage = "~/Login/Index")]
        public ActionResult SendMessage()
        {
            //var cachedCategories = HttpContext.Cache.Get("Categories") as List<Category>;


            //if (cachedCategories == null)
            //{
            //    List<Category> categoriesList = AdminService.GetCatagories();
            //    HttpContext.Cache.Insert("Categories", categoriesList, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);

            //    ViewBag.TotalStudents = categoriesList;
            //}
            //else
            //{
            //    ViewBag.TotalStudents = cachedCategories;
            //}


            List<Category> categoriesList = AdminService.GetCatagories();

            List<SelectListItem> categorySelectList = new List<SelectListItem>();
            foreach (Category category in categoriesList)
            {
                categorySelectList.Add(new SelectListItem { Text = category.Name, Value = Convert.ToString(category.Id) });

            }

            ViewData["Category"] = categorySelectList;


            ViewBag.role = "Admin";
            Session["LoginUserRole"] = "Admin";
            return View();
        }

        [CustomAuthorization(LoginPage = "~/Login/Index")]
        public ActionResult CustomerInfo()
        {
            string custId = Request.QueryString["custId"];

            if (custId != null)
            {
                PostSurvey objPostSurvey = new PostSurvey();

                AdminService objAdmin = new AdminService();
                Survey survey = objAdmin.GetSurvey(custId);
                if (survey != null)
                {
                    PostSurvey postSurvey = new PostSurvey()
                    {
                        hdnCateFristLevel = survey.Category,
                        hdnCateSecondLevel = survey.SubCategory,
                        State = survey.State,
                        District = survey.District,
                        Mandal = survey.Mandal,

                        AddIdColl = survey.AddIdColl,
                        Name = survey.Name,
                        PhoneNumber = survey.PhoneNumber,
                        Remarks = survey.Remarks,
                        UserType = survey.UserType,
                        custId = survey.Id,
                        Status = survey.Status
                    };

                    ViewBag.role = "Admin";
                    Session["LoginUserRole"] = "Admin";
                    return View(postSurvey);
                }

            }


            ViewBag.role = "Admin";
            Session["LoginUserRole"] = "Admin";
            return View();
        }

        [HttpPost]
        public ActionResult CustomerInfo(PostSurvey postSurvey)
        {
            try
            {
                string queryStringForEdit = Request.QueryString["custId"];
                if (postSurvey.custId == 0)
                {
                    Survey survey = new Survey()
                    {
                        Category = postSurvey.hdnCateFristLevel,
                        SubCategory = postSurvey.hdnCateSecondLevel,
                        State = postSurvey.State.Trim(),
                        District = postSurvey.District.Trim(),
                        Mandal = postSurvey.Mandal.Trim(),

                        AddIdColl = postSurvey.AddIdColl.Trim(),
                        Name = postSurvey.Name.Trim(),
                        PhoneNumber = postSurvey.PhoneNumber.Trim(),
                        Remarks = postSurvey.Remarks.Trim(),
                        UserType = postSurvey.UserType.Trim(),
                        CreatedDate = CustomActions.GetCurrentISTTime(),
                        Status = postSurvey.Status
                    };


                    AdminService objAdmin = new AdminService();
                    ViewBag.Status = objAdmin.AddSurvey(survey);
                }
                else
                {
                    Survey survey = new Survey()
                    {
                        Category = postSurvey.hdnCateFristLevel,
                        SubCategory = postSurvey.hdnCateSecondLevel,
                        State = postSurvey.State.Trim(),
                        District = postSurvey.District.Trim(),
                        Mandal = postSurvey.Mandal.Trim(),

                        AddIdColl = postSurvey.AddIdColl.Trim(),
                        Name = postSurvey.Name.Trim(),
                        PhoneNumber = postSurvey.PhoneNumber.Trim(),
                        Remarks = postSurvey.Remarks.Trim(),
                        UserType = postSurvey.UserType.Trim(),
                        Status = postSurvey.Status,
                        Id = postSurvey.custId
                    };

                    AdminService objAdmin = new AdminService();
                    ViewBag.Status = objAdmin.UpdateSurvey(survey);
                }


            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpGet]
        public ActionResult PreviewCustomerInfo()
        {
            string custId = Request.QueryString["custId"];

            if (custId != null)
            {
                PostSurvey objPostSurvey = new PostSurvey();

                AdminService objAdmin = new AdminService();
                Survey survey = objAdmin.GetSurvey(custId);
                if (survey != null)
                {
                    PostSurvey postSurvey = new PostSurvey()
                    {
                        hdnCateFristLevel = survey.Category,
                        hdnCateSecondLevel = survey.SubCategory,
                        State = survey.State,
                        District = survey.District,
                        Mandal = survey.Mandal,

                        AddIdColl = survey.AddIdColl,
                        Name = survey.Name,
                        PhoneNumber = survey.PhoneNumber,
                        Remarks = survey.Remarks,
                        UserType = survey.UserType,
                        custId = survey.Id,
                        Status = survey.Status,
                        CreatedDate = survey.CreatedDate.ToShortTimeString()
                    };

                    return View(postSurvey);
                }

            }
            return View();
        }

        [CustomAuthorization(LoginPage = "~/Login/Index")]
        public ActionResult CustomerDashboard()
        {
            List<Survey> surveyColl = new List<Survey>();
            try
            {
                AdminService objAdmin = new AdminService();
                surveyColl = objAdmin.GetSurveys();

            }
            catch (Exception ex)
            {

            }

            ViewBag.role = "Admin";
            Session["LoginUserRole"] = "Admin";
            return View(surveyColl);
        }

        [CustomAuthorization(LoginPage = "~/Login/Index")]
        public ActionResult Delete()
        {
            string addId = Request.QueryString["custId"];

            if (addId != null)
            {
                AdminService objAdmin = new AdminService();
                bool isDeleted = objAdmin.DeleteSurvey(addId);
                if (isDeleted)
                {
                    return RedirectToAction("CustomerDashboard", "Admin");
                }
                else
                {
                    ViewBag.Message = "error";
                }
            }


            return View();
        }

        public JsonResult saveConsumerInfo(string name, string mobileNumber, string state, string dt, string mdl, string addId)
        {
            try
            {
                if (addId != string.Empty && addId != null)
                {

                    CommonDBOperations objCommonDBOperations = new CommonDBOperations();
                    Add addRecord = objCommonDBOperations.GetAdd(addId);

                    if (addRecord != null)
                    {
                        string userType = string.Empty;
                        if (addRecord.Type == "Rent")
                        {
                            userType = "Consumer";
                        }
                        else
                        {
                            userType = "Buyer";
                        }

                        Survey survey = new Survey()
                        {
                            Category = addRecord.Category,
                            SubCategory = addRecord.SubCategory,
                            State = state,
                            District = dt,
                            Mandal = mdl,

                            AddIdColl = addId,
                            Name = name,
                            PhoneNumber = mobileNumber,
                            Status = "Pending",
                            UserType = userType,
                            CreatedDate = CustomActions.GetCurrentISTTime(),

                        };


                        AdminService objAdmin = new AdminService();
                        bool isAdded = objAdmin.AddSurvey(survey);

                        if (isAdded)
                        {
                            return Json("sucess", JsonRequestBehavior.AllowGet);
                        }
                    }


                }

            }
            catch (Exception ex)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSubcategory(int id)
        {
            List<Category> categoriesList = AdminService.GetCatagories();

            var subcategories = categoriesList.FindAll(x => x.Id == id).Select(x => x.SubCategories).ToList();

            List<SelectListItem> subcategorySelectList = new List<SelectListItem>();
            if (subcategories.Count == 1)
            {
                foreach (SubCategory subCaretogry in subcategories[0])
                {
                    subcategorySelectList.Add(new SelectListItem { Text = subCaretogry.Name, Value = Convert.ToString(subCaretogry.Id) });

                }
            }
            return Json(new SelectList(subcategorySelectList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public ActionResult AddsInfo()
        {
            //if (isAdmin())
            //{
            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            // IEnumerable<CustomSurvey> surveyColl = new List<CustomSurvey>();
            // AdminDashboard adminDashboard = new AdminDashboard();
            try
            {
                UserDBOperations db = new UserDBOperations();
                addColl = db.GetAdminAdds();
                // surveyColl = db.GetSurvey();
                // adminDashboard.SurveyColl = surveyColl;
                // adminDashboard.AddsColl = addColl;
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Admin", ex);
            }
            return View(addColl);
            //}
            //else
            //{
            //    return RedirectToAction("Login", "User");
            //}

        }

        public ActionResult CustInfo()
        {
            //if (isAdmin())
            //{
            // IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            IEnumerable<CustomSurvey> surveyColl = new List<CustomSurvey>();
            // AdminDashboard adminDashboard = new AdminDashboard();
            try
            {
                UserDBOperations db = new UserDBOperations();
                //addColl = db.GetAdminAdds();
                surveyColl = db.GetSurvey();
                // adminDashboard.SurveyColl = surveyColl;
                // adminDashboard.AddsColl = addColl;
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Admin", ex);
            }
            return View(surveyColl);
            //}
            //else
            //{
            //    return RedirectToAction("Login", "User");
            //}
        }

        public ActionResult Test()
        {
            //if (isAdmin())
            //{
            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            // IEnumerable<CustomSurvey> surveyColl = new List<CustomSurvey>();
            // AdminDashboard adminDashboard = new AdminDashboard();
            try
            {
                UserDBOperations db = new UserDBOperations();
                addColl = db.GetAdminAdds();
                // surveyColl = db.GetSurvey();
                // adminDashboard.SurveyColl = surveyColl;
                // adminDashboard.AddsColl = addColl;
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Admin", ex);
            }
            return View(addColl);
            //}
            //else
            //{
            //    return RedirectToAction("Login", "User");
            //}

        }

        public ActionResult Info()
        {
            //if (isAdmin())
            //{
            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            // IEnumerable<CustomSurvey> surveyColl = new List<CustomSurvey>();
            // AdminDashboard adminDashboard = new AdminDashboard();
            try
            {
                UserDBOperations db = new UserDBOperations();
                addColl = db.GetAdminAdds();
                // surveyColl = db.GetSurvey();
                // adminDashboard.SurveyColl = surveyColl;
                // adminDashboard.AddsColl = addColl;
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Admin", ex);
            }
            return View(addColl);
            //}
            //else
            //{
            //    return RedirectToAction("Login", "User");
            //}
        }

        public JsonResult SearchOwenersData(string searchEntity)
        {
            //if(searchEntity !=null && searchEntity != string.Empty)
            //{
            //    var x = JsonConvert.DeserializeObject<SearchAddsEntity>(searchEntity);

            //    return Json("", JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            AjaxResponse<string> objAjaxResponse = new AjaxResponse<string>() { Status = ResponseStatus.failure, Data = "Invalid Input" };
            return Json(JsonConvert.SerializeObject(objAjaxResponse), JsonRequestBehavior.AllowGet);
            //}



        }

        public bool SendSms(string msg, string selectedOption, string searchQuery)
        {
            bool status = true;
            try
            {

                AdminService objAdmin = new AdminService();
                List<string> phoneNumColl = GetPhoneNumColl(selectedOption, searchQuery);

                Communication objComm = new Communication();
                foreach (string phoneNum in phoneNumColl)
                {
                    if (!string.IsNullOrEmpty(phoneNum))
                    {
                        // objComm.SendMessage(phoneNum, msg);
                    }
                }

            }
            catch (Exception ex)
            {
                Library.WriteLog("At sendmsg while sending msg from admin dashboard", ex);
                status = false;
            }

            return status;
        }

        public bool SendSmsFromgrid(string msg, string phoneNumColl)
        {
            bool status = true;
            try
            {
                AdminService objAdmin = new AdminService();
                // List<string> phoneNumColl = objAdmin.GetOwnersMobileNos();
                string[] mnColl = phoneNumColl.Split(',');
                Communication objComm = new Communication();
                foreach (string phoneNum in mnColl)
                {
                    if (!string.IsNullOrEmpty(phoneNum))
                    {
                        // objComm.SendMessage(phoneNum, msg);
                    }

                }
                // ViewBag.Status = "Messages have been sent successfully";

            }
            catch (Exception ex)
            {
                status = false;
                Library.WriteLog("At sendmsg while sending msg from admin dashboard", ex);
                // ViewBag.Status = "Error occured while sending Messages.";
            }

            return status;
        }

        public JsonResult GetOwnersData()
        {
            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            try
            {
                UserDBOperations db = new UserDBOperations();
                addColl = db.GetAdminAdds();
            }
            catch (Exception ex)
            {

            }

            return Json(addColl, JsonRequestBehavior.AllowGet);
        }

        public List<string> GetPhoneNumColl(string selectedOption, string searchQuery)
        {
            List<string> phoneNumColl = new List<string>();
            AdminService objAdminService = new AdminService();
            try
            {
                switch (selectedOption)
                {
                    case "allConsumersData":
                        phoneNumColl = objAdminService.GetConsumersMobileNos();
                        break;
                    case "allWonersData":
                        phoneNumColl = objAdminService.GetOwnersMobileNos();
                        break;
                    case "ownersData":
                        if (searchQuery != null && searchQuery != string.Empty)
                        {
                            SearchOwnerAddsEntity ownersSearchObj = JsonConvert.DeserializeObject<SearchOwnerAddsEntity>(searchQuery);

                            phoneNumColl = objAdminService.GetOwnersMobileNos(ownersSearchObj);
                        }
                        break;
                    case "consumersData":
                        if (searchQuery != null && searchQuery != string.Empty)
                        {
                            SearchOwnerAddsEntity consumerSearchObj = JsonConvert.DeserializeObject<SearchOwnerAddsEntity>(searchQuery);

                            phoneNumColl = objAdminService.GetConsumerMobileNos(consumerSearchObj);
                        }
                        break;
                    default:

                        break;

                }

            }

            catch (Exception ex)
            {

            }

            return phoneNumColl;
        }

        public JsonResult FillGrid(string selectedOption, string searchQuery)
        {
            bool status = true;
            UserDBOperations db = new UserDBOperations();
            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            try
            {
                List<string> phoneNumColl = new List<string>();
                AdminService objAdminService = new AdminService();

                switch (selectedOption)
                {
                    case "allConsumersData":

                        addColl = db.GetConsumerAdds();

                        break;
                    case "allWonersData":
                        addColl = db.GetAdminAdds();
                        break;
                    case "ownersData":
                        if (searchQuery != null && searchQuery != string.Empty)
                        {
                            SearchOwnerAddsEntity ownersSearchObj = JsonConvert.DeserializeObject<SearchOwnerAddsEntity>(searchQuery);

                            addColl = objAdminService.GetOwnersAdds(ownersSearchObj);
                        }
                        break;
                    case "consumersData":
                        if (searchQuery != null && searchQuery != string.Empty)
                        {
                            SearchOwnerAddsEntity ownersSearchObj = JsonConvert.DeserializeObject<SearchOwnerAddsEntity>(searchQuery);

                            addColl = objAdminService.GetConsumersAdds(ownersSearchObj);
                        }
                        break;
                    default:

                        break;

                }



            }
            catch (Exception ex)
            {
                Library.WriteLog("At sendmsg while sending msg from admin dashboard", ex);
                status = false;
            }

            return Json(addColl, JsonRequestBehavior.AllowGet);
        }


        public bool UpdateCustomerStatus(int cId, string status,string remarks,string reciptNumber)
        {
            bool isAddUpdated = false;
            try
            {
                AdminService db = new AdminService();
                isAddUpdated = db.UpdateCustomerStatus(cId, status , remarks, reciptNumber);

            }
            catch (Exception ex)
            {
                Library.WriteLog("At updating add status addId - ", ex);
            }

            return isAddUpdated;
        }
    }
}