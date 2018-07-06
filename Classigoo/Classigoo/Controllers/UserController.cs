using Classigoo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (User.Identity.IsAuthenticated)
            {
                if (isAdmin())
                {
                    return RedirectToAction("Admin", "User");
                }
                else
                {
                    return RedirectToAction("Home", "User");
                }

            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection coll, string ReturnUrl = null)
        {
            try
            {
                UserDBOperations db = new UserDBOperations();

                Guid userId = db.IsValidUser(coll["email-phone"], coll["pwd"], coll["logintype"]);
                //bool isLoggedIn = Membership.ValidateUser("fdsa","asfds");
                if (userId != Guid.Empty)//valid user
                {
                    SetUserId(userId, false);
                    // Session["UserId"] = userId;
                    if (coll["email-phone"] == "1111111111" && coll["pwd"] == "admin")//admin login
                    {
                        SetUserRole(false);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        return RedirectToAction("Admin", "User");
                    }

                    else
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
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
                if (userId == Guid.Empty)//User doesnot exist lets add user
                {
                    Communication objCommunication = new Communication();
                    bool status = objCommunication.SendOTP(user.MobileNumber);
                    if (status)
                    {
                        @ViewBag.Status = "Please enter the verification code sent to " + user.MobileNumber + " to Login";
                        LoginWithOTP objLoginWithOtp = new LoginWithOTP();
                        objLoginWithOtp.PhoneNumber = user.MobileNumber;
                        objLoginWithOtp.VerifyType = Constants.VerifyOTPFrmRegistration;
                        TempData["UserToAdd"] = user;
                        return View("VerifyOTP", objLoginWithOtp);
                    }
                    else
                    {
                        @ViewBag.status = "Error occured while sending OTP please try again later";
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
            CustomHomeModel homeModel = new CustomHomeModel();


            if (isAdmin())
            {
                return RedirectToAction("Admin", "User");
            }
            List<CustomAdd> addColl = new List<CustomAdd>();
            List<CustomMessage> chatColl = new List<CustomMessage>();
            //   List<CustomMessage> inboxChatColl = new List<CustomMessage>();
            //  List<CustomMessage> sentChatColl = new List<CustomMessage>();

            try
            {
                Guid userId = GetUserId();
                UserDBOperations db = new UserDBOperations();
                addColl = db.GetMyAdds(userId);
                TempData["UserAddColl"] = addColl;
                MessageDBOperations msgDb = new MessageDBOperations();
                chatColl = msgDb.GetMyChats(userId);
                TempData["UserChatColl"] = chatColl;
                ViewBag.IsPwdEmpty = db.IsPwdEmpty(userId);
                if (TempData["status"] != null)//mobile number update status
                {
                    ViewBag.status = TempData["status"].ToString();
                }
                // inboxChatColl = chatColl.Where(chat => chat.Msg.ToUserId == userId).OrderBy(chat=>chat.Msg.CreatedOn).ToList();
                // sentChatColl = chatColl.Where(chat => chat.Msg.FromUserId == userId).OrderBy(chat=>chat.Msg.CreatedOn).ToList();

                homeModel.AddColl = addColl;
                //  homeModel.InboxChatColl =inboxChatColl;
                //  homeModel.SentChatColl = sentChatColl;
                homeModel.ChatColl = chatColl;
            }
            catch (Exception ex)
            {
                Library.WriteLog("At User Home", ex);
            }

            return View(homeModel);

        }

        [Authorize]
        [HttpPost]
        public ActionResult Home(FormCollection coll)
        {
            List<CustomAdd> addColl = new List<CustomAdd>();
            List<CustomMessage> chatColl = new List<CustomMessage>();
            CustomHomeModel homeModel = new CustomHomeModel();
            // List<CustomMessage> inboxChatColl = new List<CustomMessage>();
            // List<CustomMessage> sentChatColl = new List<CustomMessage>();
            try
            {
                Guid userId = GetUserId();
                UserDBOperations db = new UserDBOperations();
                User user = db.GetUser(userId);
                addColl = (List<CustomAdd>)TempData["UserAddColl"];
                chatColl = (List<CustomMessage>)TempData["UserChatColl"];
                Guid emailExist = db.UserExist(coll["txtEmail"], "Gmail");
                Guid phoneExist = db.UserExist(coll["txtPhone"], "Custom");
                ViewBag.IsPwdEmpty = Convert.ToBoolean(coll["IsPwdEmpty"]);
                TempData.Keep("UserAddColl");
                TempData.Keep("UserChatColl");

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
                            Communication objCommunication = new Communication();
                            bool status = objCommunication.SendOTP(user.MobileNumber);
                            if (status)
                            {
                                @ViewBag.Status = "Please enter the verification code sent to " + user.MobileNumber + " to Login";
                                LoginWithOTP objLoginWithOtp = new LoginWithOTP();
                                objLoginWithOtp.PhoneNumber = user.MobileNumber;
                                objLoginWithOtp.VerifyType = Constants.VerifyOTPFrmChangePhoneNum;
                                TempData["UserToModify"] = user;
                                return View("VerifyOTP", objLoginWithOtp);
                            }
                            else
                            {
                                @ViewBag.status = "Error occured while sending OTP please try again later";
                            }

                        }
                        else
                        {
                            @ViewBag.status = "Mobile Number already registered";
                        }
                        #endregion
                        break;
                    case "Create Password":
                        #region CreatePassword
                        user.Password = coll["txtPasscode"];
                        if (db.UpdateUserDetails(user))
                        {
                            @ViewBag.status = "Password updated successfully";
                            ViewBag.IsPwdEmpty = false;
                        }
                        else
                        {
                            @ViewBag.status = "Eror occured while updating Password";
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                // inboxChatColl = chatColl.Where(chat => chat.Msg.ToUserId == userId).OrderBy(chat => chat.Msg.CreatedOn).ToList();
                // sentChatColl = chatColl.Where(chat => chat.Msg.FromUserId == userId).OrderBy(chat => chat.Msg.CreatedOn).ToList();

                homeModel.AddColl = addColl;
                //homeModel.InboxChatColl = inboxChatColl;
                //homeModel.SentChatColl = sentChatColl;
                
                homeModel.ChatColl = chatColl;
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Updating user details", ex);
            }

            return View(homeModel);
        }
        [Authorize]
        public ActionResult SignOut()
        {
            Session.Remove("UserId");
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Classigoo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["Classigoo"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }

                HttpCookie signinUserCookie = this.ControllerContext.HttpContext.Request.Cookies["ClassigooUserRole"];
                if (signinUserCookie != null)
                {
                    signinUserCookie.Expires = DateTime.Now.AddDays(-1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(signinUserCookie);
                }

            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Home", "List");
        }
        [Authorize]
        public ActionResult Admin()
        {
            if (isAdmin())
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
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public bool UpdateAddStatus(int addId, string status, string remarks)
        {
            bool isAddUpdated = false;
            try
            {
                UserDBOperations db = new UserDBOperations();
                isAddUpdated = db.UpdateAddStatus(addId, status, remarks);
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

        public void SetUserId(Guid userId, bool rememberMe)
        {
            try
            {
                HttpCookie signinCookie = new HttpCookie("Classigoo");
                signinCookie.Value = userId.ToString();

                if (rememberMe)
                {
                    signinCookie.Expires = DateTime.Now.AddDays(5);
                }
                else
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
            catch (Exception ex)
            {
                Library.WriteLog("At setuserid saving userid to cookie", ex);
            }
        }

        public void SetUserRole(bool rememberMe)
        {
            try
            {
                HttpCookie signinUserCookie = new HttpCookie("ClassigooUserRole");
                signinUserCookie.Value = "admin";

                if (rememberMe)
                {
                    signinUserCookie.Expires = DateTime.Now.AddDays(5);
                }
                else
                {
                    signinUserCookie.Expires = DateTime.Now.AddDays(2);
                }

                this.ControllerContext.HttpContext.Response.Cookies.Add(signinUserCookie);
            }
            catch (Exception ex)
            {
                Library.WriteLog("At set userrole cookies ", ex);
            }
        }

        public Guid GetUserId()
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                Library.WriteLog("Getting username from session/db", ex);
            }

            return userName;
        }

        public bool isAdmin()
        {
            if (Request.Cookies["ClassigooUserRole"] != null)
            {
                var value = Request.Cookies["ClassigooUserRole"].Value;
                if (value == "admin")
                {
                    return true;
                }
            }

            return false;
        }

        public ActionResult LoginWithOtp()
        {

            return View();
        }
        [HttpPost]
        public ActionResult LoginWithOtp(LoginWithOTP loginWithOtp)
        {
            try
            {
                UserDBOperations db = new UserDBOperations();
                Guid userId = db.UserExist(loginWithOtp.PhoneNumber, "Custom");
                if (userId != Guid.Empty)
                {
                    Communication objCommunication = new Communication();
                    bool status = objCommunication.SendOTP(loginWithOtp.PhoneNumber);
                    if (status)
                    {
                        if (TempData["VerifyType"] != null)
                        {
                            if (TempData["VerifyType"].ToString() == Constants.VerifyOTPFrmForgotPwd)
                                loginWithOtp.VerifyType = Constants.VerifyOTPFrmForgotPwd;
                        }
                        else
                        {
                            loginWithOtp.VerifyType = Constants.VerifyOTPFrmLoginWIthOTP;
                        }

                        ViewBag.Status = "Please enter the verification code sent to " + loginWithOtp.PhoneNumber + " to Login";
                        return View("VerifyOTP", loginWithOtp);
                    }
                    else
                    {
                        ViewBag.Status = "Error occured while sending OTP please try again later";
                    }
                }
                else
                {
                    ViewBag.Status = "Account does not exist with this phone please register before using this feature";
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Send OTP", ex);
            }
            return View();
        }

        public ActionResult VerifyOTP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyOTP(LoginWithOTP loginWithOtp)
        {
            try
            {
                Communication objCommunication = new Communication();
                bool isVerified = objCommunication.VerifyOTP(loginWithOtp.PhoneNumber, loginWithOtp.OTP);
                if (isVerified)
                {
                    UserDBOperations db = new UserDBOperations();
                    if (loginWithOtp.VerifyType == Constants.VerifyOTPFrmRegistration)// verifying otp from registeration page so lets add user
                    {
                        User user = (User)TempData["UserToAdd"];
                        Guid userId = db.AddUser(user);
                        if (userId != Guid.Empty)//User Added successfully
                        {
                            SetUserId(userId, false);
                            return RedirectToAction("Home", "User");
                        }
                        else//Error occured while adding user
                        {
                            @ViewBag.Status = "Error Occured while Registering User";
                        }
                    }
                    else if (loginWithOtp.VerifyType == Constants.VerifyOTPFrmLoginWIthOTP)
                    {
                        Guid userId = db.UserExist(loginWithOtp.PhoneNumber, "Custom");
                        SetUserId(userId, false);
                        return RedirectToAction("Home", "User");
                    }
                    else if (loginWithOtp.VerifyType == Constants.VerifyOTPFrmChangePhoneNum)
                    {
                        User user = (User)TempData["UserToModify"];
                        if (db.UpdateUserDetails(user))
                        {
                            TempData["status"] = "Mobile Number updated successfully";
                        }
                        else
                        {
                            TempData["status"] = "Error occured while updating Mobile Number ";
                        }
                        return RedirectToAction("Home", "User");
                    }
                    else if (loginWithOtp.VerifyType == Constants.VerifyOTPFrmForgotPwd)
                    {
                        ForgotPwd objForgotPwd = new ForgotPwd();
                        objForgotPwd.PhoneNumber = loginWithOtp.PhoneNumber;

                        return View("ForgotPwd", objForgotPwd);
                    }
                }
                else
                {
                    @ViewBag.Status = "Wrong or Expired OTP! Use resend to send OTP on " + loginWithOtp.PhoneNumber;
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At verifyOTP", ex);
            }
            return View(loginWithOtp);
        }

        public ActionResult ResendOtp(LoginWithOTP loginWithOtp)
        {
            Communication objCommunication = new Communication();
            bool isOTPSent = objCommunication.ResendOTP(loginWithOtp.PhoneNumber);
            if (isOTPSent)
            {
                ViewBag.Status = "Please enter the verification code sent to " + loginWithOtp.PhoneNumber + " to Login";
            }
            else
            {
                ViewBag.Status = "Error occured while sending OTP please try again later";
            }
            return View("VerifyOTP", loginWithOtp);

        }

        //public ActionResult VerifyOTP()
        //{

        //    return View();
        //}

        public PartialViewResult LoadChat(int addid,Guid requestorUserId)
        {
            MessageDBOperations objMsgDbOperations = new MessageDBOperations();
            Guid userId = GetUserId();
            List<CustomMessage> chatColl = objMsgDbOperations.LoadChat(userId, addid, requestorUserId);
            IndividualChat individualChatColl = new IndividualChat();
            individualChatColl.CustomMsgColl = chatColl;
            individualChatColl.FromUserId = userId;
            individualChatColl.RequestorUserId = requestorUserId;
            individualChatColl.AddId = addid;
            individualChatColl.AddTitle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(chatColl[0].AddTitle);
            if (requestorUserId==userId)//you are not add owner
            {
                foreach(CustomMessage msg in chatColl)
                {
                    if(msg.Status=="send")
                    {
                        individualChatColl.ToUserId = msg.ToUserId;
                        break;
                    }
                }
                
            }
            else//your are not a requestor so ur add owner 
            {
                foreach (CustomMessage msg in chatColl)
                {
                    if (msg.Status == "receive")
                    {
                        individualChatColl.ToUserId = msg.FromUserId;
                        break;
                    }
                }
            }

            return PartialView("_LoadChat", individualChatColl);
        }

        public ActionResult ForgotPwd()
        {
            TempData["VerifyType"] = Constants.VerifyOTPFrmForgotPwd;
            return View("LoginWithOtp");

            //return View();
        }
        [HttpPost]
        public ActionResult ForgotPwd(ForgotPwd forgotPwd)
        {
            try
            {
                UserDBOperations db = new UserDBOperations();
                Guid userId = db.UserExist(forgotPwd.PhoneNumber, "Custom");
                User user = db.GetUser(userId);
                user.Password = forgotPwd.txtPasscode;
                if (db.UpdateUserDetails(user))
                {
                    TempData["status"] = "Password changed successfully";
                    SetUserId(userId, false);
                }
                else
                {
                    @ViewBag.Status = "Error occured while changed Password ";
                }
                return RedirectToAction("Home", "User");
            }
            catch (Exception ex)
            {
                Library.WriteLog("At forgot password", ex);
                @ViewBag.Status = "Error occured while changing Password ";
            }
            return View();
        }

        public PartialViewResult AddChat(int addId,Guid frmUserId, Guid toUserId,Guid requestorUserId, string userMessage)
        {
            Message msg = new Message();
            bool status = false;
            try
            {
                MessageDBOperations msgDbObj = new MessageDBOperations();
                msg.AdId = addId;
                msg.CreatedOn = CustomActions.GetCurrentISTTime();
                msg.FromUserId = frmUserId;
                msg.ToUserId = toUserId;
                msg.RequestorUserId = requestorUserId;
                msg.Message1 = userMessage;
                status = msgDbObj.AddChat(msg);
                
            }
            catch(Exception ex)
            {
                Library.WriteLog("At add chat from dashboard", ex);
            }
            //if(status)
            return PartialView("_SentChat", msg);
           // else
          //  {
               // return RedirectToAction("Home","User");
            //}
        }
    }
}
