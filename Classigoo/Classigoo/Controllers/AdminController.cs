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

            return View(adminDashboard);

        }

        [CustomAuthorization(LoginPage = "~/Login/Index")]
        public ActionResult EmployeeDashboard()
        {
            AdminService objAdmin = new AdminService();
            var cookieName = Request.Cookies["ClassigooLoginUser"];
            IEnumerable<AdminAdd> empAddColl=  objAdmin.GetEmpAdds(cookieName.Value);

            ViewBag.role = "Employee";
            return View(empAddColl);
        }

      //  [CustomAuthorization(LoginPage = "~/Login/Index")]
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

            return View();
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

        public ActionResult SendSms(string msg)
        {
            try
            {
                AdminService objAdmin = new AdminService();
                List<string> phoneNumColl = objAdmin.GetOwnersMobileNos();

                Communication objComm = new Communication();
                foreach (string phoneNum in phoneNumColl)
                {
                    if (!string.IsNullOrEmpty(phoneNum))
                        objComm.SendMessage(phoneNum, msg);

                }
                ViewBag.Status = "Messages have been sent successfully";

            }
            catch (Exception ex)
            {
                Library.WriteLog("At sendmsg while sending msg from admin dashboard", ex);
                ViewBag.Status = "Error occured while sending Messages.";
            }

            return PartialView();
        }
        public ActionResult SendSms(string msg,string phoneNumColl)
        {
            try
            {
                AdminService objAdmin = new AdminService();
                // List<string> phoneNumColl = objAdmin.GetOwnersMobileNos();
                string[] mnColl = phoneNumColl.Split(',');
                Communication objComm = new Communication();
                foreach (string phoneNum in mnColl)
                {
                    if (!string.IsNullOrEmpty(phoneNum))
                        objComm.SendMessage(phoneNum, msg);

                }
                ViewBag.Status = "Messages have been sent successfully";

            }
            catch (Exception ex)
            {
                Library.WriteLog("At sendmsg while sending msg from admin dashboard", ex);
                ViewBag.Status = "Error occured while sending Messages.";
            }

            return PartialView();
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
    }
}