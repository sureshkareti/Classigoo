using Classigoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Classigoo.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection coll)
        {
            try
            {
                UserDBOperations db = new UserDBOperations();

                Guid userId = db.IsValidUser(coll["email-phone"], coll["pwd"], coll["logintype"]);

                if (userId != Guid.Empty)//valid user
                {
                    Session["UserId"] = userId;
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
                        Session["UserId"] = userId;
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

        public ActionResult Home()
        {
            List<CustomAdd> addColl = new List<CustomAdd>();
            if (Session["UserId"] != null)//user logged in
            {
                Guid userId = (Guid)Session["UserId"];
                UserDBOperations db = new UserDBOperations();
                User user = db.GetUser(userId);
                if (user != null)
                {
                    Session["UserName"] = user.Name;
                }
                addColl = db.GetMyAdds(userId);
                TempData["UserAddColl"] = addColl;
                return View(addColl);
            }
            else//user not logged in
            {
                return RedirectToAction("Home", "List");
            }

        }

        [HttpPost]
        public ActionResult Home(FormCollection coll)
        {
            List<CustomAdd> addColl = new List<CustomAdd>();
            if (Session["UserId"] != null)//user logged in
            {
                try
                {
                    Guid userId = (Guid)Session["UserId"];
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
            }
            return View(addColl);
        }
        public ActionResult SignOut()
        {
            Session.Remove("UserId");
            return RedirectToAction("Home", "List");
        }
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

        public bool UpdateAddStatus(int addId, string status)
        {
            bool isAddUpdated = false;
            try
            {
                UserDBOperations db = new UserDBOperations();
                isAddUpdated = db.UpdateAddStatus(addId, status);
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
                Session["UserId"] = userId;
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
                    Session["UserId"] = userId;
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At AddUser", ex);
            }

            return userId;
        }
    }
}
