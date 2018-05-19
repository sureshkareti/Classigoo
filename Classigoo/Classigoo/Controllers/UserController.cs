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
                    string url = "http://localhost:51797/api/UserApi/IsValidUser/?userName=" + coll["email-phone"] + "&pwd=" + coll["pwd"] + "&logintype=" + coll["logintype"];
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

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (UserId != Guid.Empty)
            {
                Session["UserId"] = UserId;
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
            User user = new User();
            user.MobileNumber = coll["inputPhone"];
            user.Name = coll["inputName"];
            user.Password = coll["inputPassword"];
            Guid userId = IsUserExist(user.MobileNumber, "Custom");
             if (userId==Guid.Empty)
              {
                using (var client = new HttpClient())
                {

                    user.Type = "Custom";
                    string url = "http://localhost:51797/api/UserApi/AddUser/?user=" + user;
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
                        return RedirectToAction("Home", "User");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }

                }


            }
            else
            {
                @ViewBag.status = " Phone Number " + user.MobileNumber + " already Registered";
            }

            return View();

        }
        public Guid IsUserExist(string id, string type)
        {
           
            Guid userId = Guid.Empty;
            using (var client = new HttpClient())
            {
                string url = "http://localhost:51797/api/UserApi/CheckUser/?id=" + id + "&type=" + type;
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
                }
                else //web api sent error response 
                {
                    //log response status here..

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return userId;

        }
        public ActionResult UnableToLogin()
        {
            return View();
        }
        public ActionResult PostAdd()
        {
            return View();
        }
       

        public ActionResult Home()
        {
            Guid userId = (Guid)Session["UserId"];
            //Session["UserId"] = new Guid("280bf190-3fe3-4e1c-8f6e-e66edd7e272f");jdjfajdsf
            List<CustomAdd> addColl = GetMyAdds(userId);
            TempData["UserAddColl"] = addColl;
            return View(addColl);

        }
        [HttpPost]
        public ActionResult Home(FormCollection coll)
        {
            Guid userId = (Guid)Session["UserId"];
            User user = GetUserDetails(userId);
            List<CustomAdd> addColl = (List<CustomAdd>)TempData["UserAddColl"];
            Guid emailExist = IsUserExist(coll["txtEmail"], "Gmail");
            Guid phoneExist = IsUserExist(coll["txtPhone"], "Custom");
            TempData.Keep("UserAddColl");
            switch (coll["action"])
            {
                case "Change Password":
                    if (coll["txtOldPasscode"] == user.Password)
                    {
                        user.Password = coll["txtPasscode"];
                        UpdateUserDetails(user);
                        @ViewBag.status = "Password updated successfully";

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
                        UpdateUserDetails(user);
                        @ViewBag.status = "Email updated successfully";
                    }
                    else
                    {
                        @ViewBag.status = "Email already registered";
                    }
                    break;
                case "Change Phone":
                    if (phoneExist==Guid.Empty)
                    {
                        user.MobileNumber = coll["txtPhone"];
                        UpdateUserDetails(user);
                        @ViewBag.status = "Mobile Number updated successfully";
                    }
                    else
                    {
                        @ViewBag.status = "Mobile Number already registered";
                    }
                    break;
                default:
                    break;

            }
            return View(addColl);
        }
        public void UpdateUserDetails(User user)
        {
            using (var client = new HttpClient())
            {

                string url = "http://localhost:51797/api/UserApi/UpdateUserDetails/?user=" + user;
                client.BaseAddress = new Uri(url);
                var postTask = client.PutAsJsonAsync<User>(url, user);
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
                    //return RedirectToAction("Home", "User");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }

            }
        }
        public User GetUserDetails(Guid id)
        {
            User user = new User();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = "http://localhost:51797/api/UserApi/GetUser/?id=" + id;
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
                        //log response status here..

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {

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
                    string url = "http://localhost:51797/api/UserApi/GetMyAdds/?userId=" + id;
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
                        //log response status here..

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {

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
                    string url = "http://localhost:51797/api/UserApi/GetAddById/?addId=" + addId;
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
                        //log response status here..

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(customAdd);
        }
        public ActionResult ShowAdd()
        {

            return View();
        }

        public void AddLog(Exception Exception)
        {
            Log log = new Log();
            log.ExceptionMsg = Exception.Message.ToString();
            log.ExceptionSource = Exception.StackTrace.ToString();
            log.ExceptionType = Exception.GetType().ToString();
            log.ExceptionURL = Request.Url.ToString();
            log.UserId = "";
            log.CreatedDate = DateTime.Now;
            using (var client = new HttpClient())
            {

                string url = "http://localhost:51797/api/UserApi/AddLog/?log=" + log;
                client.BaseAddress = new Uri(url);
                var postTask = client.PostAsJsonAsync<Log>(url, log);
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

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }

            }
        }
        public ActionResult SignOut()
        {
            Session.Remove("UserId");
            return RedirectToAction("Login", "User");
        }
        public ActionResult Admin()
        {
            List<Add> addColl = new List<Add>();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = "http://localhost:51797/api/UserApi/Admin";
                    client.BaseAddress = new Uri(url);
                    //HTTP GET
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<Add>>();
                        readTask.Wait();

                        addColl = readTask.Result;

                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(addColl);
        }
        public ActionResult UpdateAddStatus(int addId, string status)
        {
            using (var client = new HttpClient())
            {

                string url = "http://localhost:51797/api/UserApi/UpdateAddStatus/?addId=" + addId + "&status=" + status;
                client.BaseAddress = new Uri(url);
                var postTask = client.PutAsJsonAsync(url, addId);
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
                    return RedirectToAction("Admin", "User");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    return RedirectToAction("", "");
                }

            }
        }
    }
}
