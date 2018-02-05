﻿using System;
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
                    string url = "http://localhost:51797/api/UserApi/IsValidUser/?userName=" + coll["email-phone"] + "&pwd=" + coll["pwd"] + "";
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
            catch(Exception ex)
            {

            }
            if(UserId!=Guid.Empty)
            {
                Session["UserId"] = UserId;
                return   RedirectToAction("UserDashboard", "User");
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
        public ActionResult Register(User User)
        {
            if (!IsUserExist(User.MobileNumber))
            {
                using (var client = new HttpClient())
                {

                    User.Type = "Custom";

                    client.BaseAddress = new Uri("http://localhost:51797/api/");
                    var postTask = client.PostAsJsonAsync<User>("UserApi", User);
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
                        return RedirectToAction("Home","User");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }

                }

           
            }
            else
            {
                @ViewBag.status = " Phone Number "+User.MobileNumber+ " already Registered";
            }

            return View();
           
        }
        public bool IsUserExist(string id)
       {
        bool IsUserExist = false;
        using (var client = new HttpClient())
        {
            string url = "http://localhost:51797/api/UserApi/CheckUser/?id=" + id + "&type=Custom";
            client.BaseAddress = new Uri(url);
            //HTTP GET
            var responseTask = client.GetAsync(url);
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<bool>();
                readTask.Wait();

                IsUserExist = readTask.Result;
            }
            else //web api sent error response 
            {
                //log response status here..

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
        }
        
        return IsUserExist;

    }



        public ActionResult UnableToLogin()
        {
            return View();
        }
        public ActionResult Home()
        {
           
            return View();
           
        }
        [HttpPost]
        public ActionResult Home(FormCollection coll)
        {
            
            return View();
        }
        public ActionResult PostAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostAdd(tbl_Adds Add)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51797/api/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<tbl_Adds>("Adds", Add);
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
                    return RedirectToAction("Home");
                }

            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(Add);
        }

        public ActionResult UserDashboard()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserDashboard(FormCollection coll)
        {
            ChangePassword(coll["txtPasscode"]);
            return View();
        }
        public void ChangePassword(string password)
        {
            using (var client = new HttpClient())
            {
                User user = GetUserDetails((Guid)Session["UserId"]);
                user.Password = password;
                string url = "http://localhost:51797/api/UserApi/ChangePassword/?user="+user;
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

                        user= readTask.Result;

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
    }
    }
