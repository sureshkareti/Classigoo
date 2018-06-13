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

        // POST: User
        [HttpPost]
        public ActionResult Login(FormCollection coll)
        {
            Guid UserId = Guid.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                   
                    string url = Constants.DomainName+"/api/UserApi/IsValidUser/?userName=" + coll["email-phone"] + "&pwd=" + coll["pwd"] + "&logintype=" + coll["logintype"];
                    client.BaseAddress = new Uri(url);
                    //HTTP GET
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Guid>();
                        readTask.Wait();

                        UserId = readTask.Result;

                    }
                    else //web api sent error response 
                    {
                        //log response status here..
                        Library.WriteLog("At Login post webapi sent error");
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Login post",ex);
            }
            if (UserId != Guid.Empty)
            {
                System.Web.HttpContext.Current.Session["UserId"] = UserId;
                if (coll["email-phone"] =="1111111111" && coll["pwd"] == "admin")
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

            return View();
        }

        public ActionResult Index()
        {
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
                Guid userId = IsUserExist(user.MobileNumber, "Custom");
                if (userId == Guid.Empty)
                {
                    if (AddUser(user))
                        return RedirectToAction("Home", "User");
                }
                else
                {
                    @ViewBag.status = " Phone Number " + user.MobileNumber + " already Registered";
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Registering user custom method",ex);
            }

            return View();

        }
        public Guid IsUserExist(string id, string type)
        {
            Guid userId = Guid.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = Constants.DomainName + "/api/UserApi/CheckUser/?id=" + id + "&type=" + type;
                    client.BaseAddress = new Uri(url);
                    //HTTP GET
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Guid>();
                        readTask.Wait();

                        userId = readTask.Result;
                        if (userId != Guid.Empty)
                            System.Web.HttpContext.Current.Session["UserId"] = userId;
                    }
                    else //web api sent error response 
                    {
                        Library.WriteLog("At Isuserexist webapi sent error type- " + type);
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Isuserexist type- "+type,ex);
            }

            return userId;

        }
        public ActionResult UnableToLogin()
        {
            return View();
        }
        public ActionResult Home()
        {
             List<CustomAdd> addColl = new List<CustomAdd>();
            if (Session["UserId"] != null)
            {
                Guid userId = (Guid)Session["UserId"];
                Session["UserName"] = GetUserDetails(userId).Name;
                addColl = GetMyAdds(userId);
                TempData["UserAddColl"] = addColl;
                return View(addColl);
            }
            else
            {
                return RedirectToAction("Home", "List");
            }

        }
        [HttpPost]
        public ActionResult Home(FormCollection coll)
        {
            List<CustomAdd> addColl = new List<CustomAdd>();
            if (Session["UserId"] != null)
            {
                try { 
                Guid userId = (Guid)Session["UserId"];
                User user = GetUserDetails(userId);
                addColl = (List<CustomAdd>)TempData["UserAddColl"];
                Guid emailExist = IsUserExist(coll["txtEmail"], "Gmail");
                Guid phoneExist = IsUserExist(coll["txtPhone"], "Custom");
                TempData.Keep("UserAddColl");
                    switch (coll["action"])
                    {
                        case "Change Password":
                            if (coll["txtOldPasscode"] == user.Password)
                            {
                                user.Password = coll["txtPasscode"];
                                if (UpdateUserDetails(user))
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

                            break;
                        case "Change Email":
                            if (emailExist == Guid.Empty)
                            {
                                user.Email = coll["txtEmail"];
                                if (UpdateUserDetails(user))
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
                            break;
                        case "Change Phone":
                            if (phoneExist == Guid.Empty)
                            {
                                user.MobileNumber = coll["txtPhone"];
                                if (UpdateUserDetails(user))
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
                            break;
                        default:
                            break;
                    }
                  
                }
                catch(Exception ex)
                {
                    Library.WriteLog("At Updating user details",ex);
                }
            }
            return View(addColl);
        }
        public bool UpdateUserDetails(User user)
        {
            bool isSuccess = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = Constants.DomainName + "/api/UserApi/UpdateUserDetails/?user=" + user;
                    client.BaseAddress = new Uri(url);
                    var postTask = client.PutAsJsonAsync<User>(url, user);

                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        Library.WriteLog("At updateuserdetails webapi sent error");
                        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }

                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At updateuserdetails",ex);
            }
            return isSuccess;
        }
        public User GetUserDetails(Guid id)
        {
            User user = new User();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = Constants.DomainName+"/api/UserApi/GetUser/?id=" + id;
                    client.BaseAddress = new Uri(url);
                    //HTTP GET
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<User>();
                        readTask.Wait();

                        user = readTask.Result;

                    }
                    else //web api sent error response 
                    {
                        Library.WriteLog("At Getuserdetails webapi sent error userid - " + id);
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Getuserdetails userid - " + id, ex);
            }
            return user;
        }
        public List<CustomAdd> GetMyAdds(Guid id)
        {
            List<CustomAdd> addColl = new List<CustomAdd>();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = Constants.DomainName+"/api/UserApi/GetMyAdds/?userId=" + id;
                    client.BaseAddress = new Uri(url);
                    //HTTP GET
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<CustomAdd>>();
                        readTask.Wait();

                        addColl = readTask.Result;

                    }
                    else //web api sent error response 
                    {
                        Library.WriteLog("At Getmyadds webapisent error userid- " + id);

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Getmyadds userid- "+id,ex);
            }
            return addColl;
        }

        public ActionResult PreviewAdd(int addId)
        {
            Add add = new Add();
            CustomAdd customAdd = new CustomAdd();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = Constants.DomainName+"/api/UserApi/GetAddById/?addId=" + addId;
                    client.BaseAddress = new Uri(url);
                    //HTTP GET
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Add>();
                        readTask.Wait();

                        add = readTask.Result;
                        CustomActions obj = new CustomActions();
                        customAdd = obj.CheckCategory(add);
                    }
                    else //web api sent error response 
                    {
                        Library.WriteLog("At Preview add webapi sent error addid- " + addId);
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Preview add addid- "+addId,ex);
            }
            return View(customAdd);
        }

        //public void AddLog(Exception Exception)
        //{
        //    Log log = new Log();
        //    log.ExceptionMsg = Exception.Message.ToString();
        //    log.ExceptionSource = Exception.StackTrace.ToString();
        //    log.ExceptionType = Exception.GetType().ToString();
        //    log.ExceptionURL = Request.Url.ToString();
        //    log.UserId = "";
        //    log.CreatedDate = DateTime.Now;
        //    using (var client = new HttpClient())
        //    {

        //        string url = Constants.DomainName+"/api/UserApi/AddLog/?log=" + log;
        //        client.BaseAddress = new Uri(url);
        //        var postTask = client.PostAsJsonAsync<Log>(url, log);
        //        try
        //        {
        //            postTask.Wait();
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        var result = postTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {

        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
        //        }

        //    }
        //}
        public ActionResult SignOut()
        {
            Session.Remove("UserId");
            return RedirectToAction("Home", "List");
        }
        public ActionResult Admin()
        {
            List<AdminAdd> addColl = new List<AdminAdd>();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = Constants.DomainName + "/api/UserApi/Admin";
                    client.BaseAddress = new Uri(url);
                    //HTTP GET
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<AdminAdd>>();
                        readTask.Wait();

                        addColl = readTask.Result;

                    }
                    else //web api sent error response 
                    {
                        Library.WriteLog("At Admin webapi sent error");

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Admin",ex);
            }
            return View(addColl);
        }

        public bool UpdateAddStatus(int addId, string status)
        {
            bool isSuccess = false;
            try
            {
                using (var client = new HttpClient())
                {

                    string url = Constants.DomainName + "/api/UserApi/UpdateAddStatus/?addId=" + addId + "&status=" + status;
                    client.BaseAddress = new Uri(url);
                    var postTask = client.PutAsJsonAsync(url, addId);

                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        isSuccess = true;

                    }
                    else
                    {
                        Library.EmailErrors("At updating add status webapisenterror addid- " + addId);
                        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

                    }
                }
            }
            catch(Exception ex)
            {
                Library.EmailErrors("At updating add status - "+ex.Message);
            }
               
                return isSuccess;
            }
        

        public bool AddUser(User user)
        {
            bool isSuccess = true;
            using (var client = new HttpClient())
            {
                string url = Constants.DomainName + "/api/UserApi/AddUser/?user=" + user;
                client.BaseAddress = new Uri(url);
                var postTask = client.PostAsJsonAsync<User>(url, user);
                try
                {
                    postTask.Wait();
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    Library.WriteLog("At Adduser",ex);
                }
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<User>();
                    readTask.Wait();
                    Session["UserId"] = readTask.Result.UserId;
                   // return RedirectToAction("Home", "User");
                }
                else
                {
                    isSuccess = false;
                    Library.WriteLog("At Adduser webapisent error");
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }
                return isSuccess;
            }
        }

      
    }
}
