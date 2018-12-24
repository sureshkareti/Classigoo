using Classigoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Classigoo.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
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
            return View(adminDashboard);

        }

        public ActionResult Dashboard1()
        {
            return View();
        }

        public ActionResult Dashboard2()
        {
            return View();
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
    }
}