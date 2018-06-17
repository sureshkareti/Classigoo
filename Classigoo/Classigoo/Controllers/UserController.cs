﻿using Classigoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Classigoo.Controllers
{
    public class UserController : Controller
    {

        public ActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "User");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection coll)
        {
            try
            {
                UserDBOperations db = new UserDBOperations();

                Guid userId = db.IsValidUser(coll["email-phone"], coll["pwd"], coll["logintype"]);
                //bool isLoggedIn = Membership.ValidateUser("fdsa","asfds");
                if (userId != Guid.Empty)//valid user
                {
                    SetUserId(userId,false);
                   // Session["UserId"] = userId;
                    if (coll["email-phone"] == "1111111111" && coll["pwd"] == "admin")//admin login
                    {
                        return RedirectToAction("Admin", "User");
                    }

                    else
                    {
                        return RedirectToAction("Home", "User");
                    }
                }
                else
                {
                    @ViewBag.status = " Invalid Email/Phone Number or Password";
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Login UserName - " + coll["email-phone"], ex);
            }

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection coll)
        {
            try
            {
                User user = new User();
                user.MobileNumber = coll["inputPhone"];
                user.Name = coll["inputName"];
                user.Password = coll["inputPassword"];
                user.Type = "Custom";
                UserDBOperations db = new UserDBOperations();
                Guid userId = db.UserExist(user.MobileNumber, "Custom");
                if (userId == Guid.Empty)//User doesnot exist
                {
                    userId = db.AddUser(user);
                    if (userId != Guid.Empty)//User Added successfully
                    {
                        // Session["UserId"] = userId;
                        SetUserId(userId, false);
                        return RedirectToAction("Home", "User");
                    }
                    else//Error occured while adding user
                    {
                        @ViewBag.status = "Error Occured while Registering User";
                    }
                }
                else
                {
                    @ViewBag.status = " Phone Number " + user.MobileNumber + " already Registered";
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Registering user custom method", ex);
            }

            return View();

        }

        [Authorize]
        public ActionResult Home()
        {
            List<CustomAdd> addColl = new List<CustomAdd>();
            try
            {
                Guid userId = GetUserId();
                UserDBOperations db = new UserDBOperations();
                addColl = db.GetMyAdds(userId);
                TempData["UserAddColl"] = addColl;
                //if (TempData["UserAddColl"]!=null)
                //{
                //    addColl= addColl = (List<CustomAdd>)TempData["UserAddColl"];
                //}
                //else
                //{
                //    addColl = db.GetMyAdds(userId);
                //    TempData["UserAddColl"] = addColl;
                //}
                
            }
            catch(Exception ex)
            {
                Library.WriteLog("At User Home",ex);
            }
            return View(addColl);

        }
        [Authorize]
        [HttpPost]
        public ActionResult Home(FormCollection coll)
        {
            List<CustomAdd> addColl = new List<CustomAdd>();
           //if (User.Identity.IsAuthenticated)//user logged in
         // {
                try
                {
                    Guid userId = GetUserId();
                    UserDBOperations db = new UserDBOperations();
                    User user = db.GetUser(userId);
                    addColl = (List<CustomAdd>)TempData["UserAddColl"];
                    Guid emailExist = db.UserExist(coll["txtEmail"], "Gmail");
                    Guid phoneExist = db.UserExist(coll["txtPhone"], "Custom");
                    TempData.Keep("UserAddColl");
                    switch (coll["action"])
                    {
                        case "Change Password":
                            #region ChangePassword
                            if (coll["txtOldPasscode"] == user.Password)
                            {
                                user.Password = coll["txtPasscode"];
                                if (db.UpdateUserDetails(user))
                                {
                                    @ViewBag.status = "Password updated successfully";
                                }
                                else
                                {
                                    @ViewBag.status = "Eror occured while updating Password";
                                }

                            }
                            else
                            {
                                @ViewBag.status = "Old Password is incorrect";
                            }
                            #endregion
                            break;
                        case "Change Email":
                            #region ChangeEmail
                            if (emailExist == Guid.Empty)//Email doesnot exist
                            {
                                user.Email = coll["txtEmail"];
                                if (db.UpdateUserDetails(user))
                                {
                                    @ViewBag.status = "Email updated successfully";
                                }
                                else
                                {
                                    @ViewBag.status = "Eror occured while updating Email";
                                }
                            }
                            else
                            {
                                @ViewBag.status = "Email already registered";
                            }
                            #endregion
                            break;
                        case "Change Phone":
                            #region ChangePhone
                            if (phoneExist == Guid.Empty)//Phone Num doesnt exist
                            {
                                user.MobileNumber = coll["txtPhone"];
                                if (db.UpdateUserDetails(user))
                                {
                                    @ViewBag.status = "Mobile Number updated successfully";
                                }
                                else
                                {
                                    @ViewBag.status = "Error occured while updating Mobile Number ";
                                }
                            }
                            else
                            {
                                @ViewBag.status = "Mobile Number already registered";
                            }
                            #endregion
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Library.WriteLog("At Updating user details", ex);
                }
            //}
            return View(addColl);
        }
        [Authorize]
        public ActionResult SignOut()
        {
            Session.Remove("UserId");
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Classigoo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["Classigoo"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Home", "List");
        }
        [Authorize]
        public ActionResult Admin()
        {
            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            try
            {
                UserDBOperations db = new UserDBOperations();
                addColl = db.GetAdminAdds();
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Admin", ex);
            }
            return View(addColl);
        }

        public bool UpdateAddStatus(int addId, string status,string remarks)
        {
            bool isAddUpdated = false;
            try
            {
                UserDBOperations db = new UserDBOperations();
                isAddUpdated = db.UpdateAddStatus(addId, status,remarks);
            }
            catch (Exception ex)
            {
                Library.WriteLog("At updating add status addId - " + addId, ex);
            }

            return isAddUpdated;
        }

        public ActionResult UnableToLogin()
        {
            return View();
        }

        public Guid AddUser(User user)
        {
            Guid userId = Guid.Empty;
            try
            {
                UserDBOperations db = new UserDBOperations();
                userId = db.AddUser(user);
                //Session["UserId"] = userId;
                SetUserId(userId, false);
            }
            catch (Exception ex)
            {
                Library.WriteLog("At AddUser", ex);
            }

            return userId;
        }

        public Guid UserExist(string id, string type)
        {
            Guid userId = Guid.Empty;
            try
            {
                UserDBOperations db = new UserDBOperations();
                userId = db.UserExist(id, type);
                if (userId != Guid.Empty)
                {
                    // Session["UserId"] = userId;
                    SetUserId(userId, false);
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At AddUser", ex);
            }

            return userId;
        }

        public void SetUserId(Guid userId,bool rememberMe)
        {
            try
            {
                HttpCookie signinCookie = new HttpCookie("Classigoo");
                signinCookie.Value = userId.ToString();
    
                if (rememberMe)
                {
                    signinCookie.Expires = DateTime.Now.AddDays(2);
                }
                UserDBOperations db = new UserDBOperations();
                User user = db.GetUser(userId);
                if (user != null)
                {
                    //Session["UserName"] = user.Name;
                    FormsAuthentication.SetAuthCookie(user.Name, rememberMe);
                }
                this.ControllerContext.HttpContext.Response.Cookies.Add(signinCookie);
            }
            catch(Exception ex)
            {
                Library.WriteLog("At setuserid saving userid to cookie", ex);
            }
        }

        public  Guid  GetUserId()
        {
            Guid userId = Guid.Empty;
            try
            {
                if (Request.Cookies["Classigoo"] != null)
                {
                    var value = Request.Cookies["Classigoo"].Value;
                    userId = Guid.Parse(value);
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At GetUserId getting userid from cookie", ex);
            }
            return userId;
        }
        public string GetUserName()
        {
            string userName = string.Empty;
            try
            {
                if (Session["UserName"] != null)
                {
                    userName = Session["UserName"].ToString();
                }
                else
                {
                    if (Request.Cookies["Classigoo"] != null)
                    {
                        var value = Request.Cookies["Classigoo"].Value;
                        UserDBOperations db = new UserDBOperations();
                        User user = db.GetUser(Guid.Parse(value));
                        if (user != null)
                        {
                            Session["UserName"] = user.Name;
                            userName = user.Name;
                        }
                       
                    }
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("Getting username from session/db", ex);
            }

            return userName;
        }
    }
}
