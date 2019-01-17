using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Classigoo.Business;
using Microsoft.Owin.Security.Cookies;
using System.Web.Security;
using Classigoo.Models;

namespace Classigoo.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (isAdmin())
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {                  
                    return RedirectToAction("EmployeeDashboard", "Admin");
                }

            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName, string pwd)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pwd) && pwd.Trim() != string.Empty)
            {

                var user = new UserManager().GetLoginUserByLoginIdPassword(userName, pwd);

                if (user == null) return View();

                var isAuthorized = true; 

                if (isAuthorized)
                {
                    SetUserId(user, false);

                    if(user.Role.RoleName == "Admin")
                    {
                       
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                       
                        return RedirectToAction("EmployeeDashboard", "Admin");
                    }
                                     
                }
            }

            return View();
        }

        [Authorize]
        public ActionResult SignOut()
        {
       
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("ClassigooLoginUser"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["ClassigooLoginUser"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }

                HttpCookie signinUserCookie = this.ControllerContext.HttpContext.Request.Cookies["ClassigooLoginRole"];
                if (signinUserCookie != null)
                {
                    signinUserCookie.Expires = DateTime.Now.AddDays(-1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(signinUserCookie);
                }
            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

        public void SetUserId(LoginUser userName, bool rememberMe)
        {
            try
            {
                HttpCookie signinCookie = new HttpCookie("ClassigooLoginUser");
                signinCookie.Value = userName.UserId.ToString();

                HttpCookie signinUserCookie = new HttpCookie("ClassigooLoginRole");
                signinUserCookie.Value = userName.Role.RoleName;

              
                if (rememberMe)
                {
                    signinCookie.Expires = DateTime.Now.AddDays(5);
                    signinUserCookie.Expires = DateTime.Now.AddDays(5);
                }
                else
                {
                    signinCookie.Expires = DateTime.Now.AddDays(2);
                    signinUserCookie.Expires = DateTime.Now.AddDays(2);
                }

                //Session["UserName"] = user.Name;
                FormsAuthentication.SetAuthCookie(userName.UserId, true);
                Session["LoginUserRole"] = userName.Role.RoleName;

                

                this.ControllerContext.HttpContext.Response.Cookies.Add(signinCookie);
                this.ControllerContext.HttpContext.Response.Cookies.Add(signinUserCookie);
            }
            catch (Exception ex)
            {
                Library.WriteLog("At set user role saving userid to cookie", ex);
            }
        }

        public bool isAdmin()
        {
            if (Request.Cookies["ClassigooLoginUser"] != null)
            {
                var value = Request.Cookies["ClassigooLoginRole"].Value;
                if (value == "Admin")
                {
                    return true;
                }
            }

            return false;
        }
    }
}