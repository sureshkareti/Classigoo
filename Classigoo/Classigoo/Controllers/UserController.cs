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

        #region "Login"
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
                return RedirectToAction("UserDashboard", "User");
            }
            else
            {
                @ViewBag.status = " Invalid Email/Phone Number or Password";
            }

            return View();



        }
        #endregion

        #region "Home"
        public ActionResult Home()
        {
            Messages AllMessages = new Messages();
            try
            {
                AllMessages = GetAllMessages();
            }
            catch (Exception ex)
            {

            }
            return View(AllMessages);
        }

        [HttpPost]
        public ActionResult Home(FormCollection coll, Messages AllMessages)
        {
            //Messages AllMessages = new Messages();
            try
            {
                AllMessages = GetAllMessages();
            }
            catch (Exception ex)
            {

            }
            return View(AllMessages);
        }

        #endregion

        #region "GetAllMessages"
        public Messages GetAllMessages()
        {
            Messages AllMessages = new Messages();
            try
            {
                List<GroupedMessageDetail> tbl_MessagesInbox = new List<GroupedMessageDetail>();
                List<GroupedMessageDetail> tbl_MessagesSent = new List<GroupedMessageDetail>();
                List<GroupedMessageDetail> tbl_MessagesArchive = new List<GroupedMessageDetail>();

                #region "Inbox"
                using (var clientMsg = new HttpClient())
                {
                    string urlMsg = "http://localhost:51797/api/UserApi/GetMessagesByUserId?UserId=" + Session["UserId"].ToString() + "&Mode=Inbox";
                    clientMsg.BaseAddress = new Uri(urlMsg);
                    var postTaskMsg = clientMsg.GetAsync(urlMsg);
                    try
                    {
                        postTaskMsg.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                    var resultMsg = postTaskMsg.Result;
                    if (resultMsg.IsSuccessStatusCode)
                    {
                        var readTaskMsg = resultMsg.Content.ReadAsAsync<List<MessageDetail>>();
                        readTaskMsg.Wait();
                        List<MessageDetail> tbl_MessagesInboxDetails = readTaskMsg.Result;


                        tbl_MessagesInbox = tbl_MessagesInboxDetails.OrderByDescending(a => a.CreatedOn).GroupBy(p => p.FromUser,
                       (k, c) => new GroupedMessageDetail()
                       {
                           User = k,
                           GroupedMessages = c.ToList(),
                            //Notification = c.Select(m=>m.ToUserId== Session["UserId"].ToString()).Count
                       }
                      ).ToList();


                        //foreach (GroupedMessageDetail item in GMD)
                        //{
                        //    Console.WriteLine(item.UserId);
                        //    foreach (string car in item.Cars)
                        //    {
                        //        Console.WriteLine(car);
                        //    }
                        //}


                        //            List<Result> results2 = persons
                        //.GroupBy(p => p.PersonId,
                        //         (k, c) => new Result()
                        //         {
                        //             PersonId = k,
                        //             Cars = c.Select(cs => cs.car).ToList()
                        //         }
                        //        ).ToList();

                        //            foreach (Result item in results2)
                        //            {
                        //                Console.WriteLine(item.PersonId);
                        //                foreach (string car in item.Cars)
                        //                {
                        //                    Console.WriteLine(car);
                        //                }
                        //            }


                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }
                }
                #endregion

                #region "Sent"
                using (var clientMsg = new HttpClient())
                {
                    string urlMsg = "http://localhost:51797/api/UserApi/GetMessagesByUserId?UserId=" + Session["UserId"].ToString() + "&Mode=Sent";
                    clientMsg.BaseAddress = new Uri(urlMsg);
                    var postTaskMsg = clientMsg.GetAsync(urlMsg);
                    try
                    {
                        postTaskMsg.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                    var resultMsg = postTaskMsg.Result;
                    if (resultMsg.IsSuccessStatusCode)
                    {
                        var readTaskMsg = resultMsg.Content.ReadAsAsync<List<MessageDetail>>();
                        readTaskMsg.Wait();

                        List<MessageDetail> tbl_MessagesSentDetails = readTaskMsg.Result;

                        tbl_MessagesSent = tbl_MessagesSentDetails.OrderByDescending(a => a.CreatedOn).GroupBy(p => p.ToUser,
                       (k, c) => new GroupedMessageDetail()
                       {
                           User = k,
                           GroupedMessages = c.ToList()

                       }
                      ).ToList();


                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }
                }
                #endregion

                #region "Archive"
                using (var clientMsg = new HttpClient())
                {
                    string urlMsg = "http://localhost:51797/api/UserApi/GetMessagesByUserId?UserId=" + Session["UserId"].ToString() + "&Mode=Archive";
                    clientMsg.BaseAddress = new Uri(urlMsg);
                    var postTaskMsg = clientMsg.GetAsync(urlMsg);
                    try
                    {
                        postTaskMsg.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                    var resultMsg = postTaskMsg.Result;
                    if (resultMsg.IsSuccessStatusCode)
                    {
                        var readTaskMsg = resultMsg.Content.ReadAsAsync<List<MessageDetail>>();
                        readTaskMsg.Wait();


                        List<MessageDetail> tbl_MessagesArchiveDetails = readTaskMsg.Result;


                        tbl_MessagesArchive = tbl_MessagesArchiveDetails.OrderByDescending(a => a.CreatedOn).GroupBy(p => p.FromUser,
                       (k, c) => new GroupedMessageDetail()
                       {
                           User = k,
                           GroupedMessages = c.ToList()
                        
                       }
                      ).ToList();
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }
                }
                #endregion

                AllMessages.Inbox = tbl_MessagesInbox;
                AllMessages.Sent = tbl_MessagesSent;
                AllMessages.Archive = tbl_MessagesArchive;
                AllMessages.InboxCount = tbl_MessagesInbox.Count;
                AllMessages.SentCount = tbl_MessagesSent.Count;
                AllMessages.ArchiveCount = tbl_MessagesArchive.Count;

            }
            catch (Exception ex)
            {
            }
            return AllMessages;
        }
        #endregion

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
            if (!IsUserExist(user.MobileNumber, "Custom"))
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
        public bool IsUserExist(string id, string type)
        {
            bool IsUserExist = false;
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
            List<Add> addColl = GetMyAdds(new Guid("19e2aca5-28a9-41ca-a641-e81c9139e34f"));
            return View(addColl);
        }
        [HttpPost]
        public ActionResult UserDashboard(FormCollection coll)
        {
            // List<Add> addColl=  GetMyAdds((Guid)Session["UserId"]);

            // User user = GetUserDetails((Guid)Session["UserId"]);
            //if(!IsUserExist(coll["txtEmail"],"Gmail"))
            //{
            //    user.Email = coll["txtEmail"];
            //    UpdateUserDetails(user);
            //}
            //else
            //{
            //    @ViewBag.status = "Email already registered";
            //}
            //if (!IsUserExist(coll["txtPhone"], "Custom"))
            //{
            //    user.MobileNumber = coll["txtPhone"];
            //    UpdateUserDetails(user);
            //}
            //else
            //{
            //    @ViewBag.status = "Mobile Number already registered";
            //}
            //if (coll["txtOldPasscode"] == user.Password)
            //{
            //    user.Password = coll["txtPasscode"];
            //    UpdateUserDetails(user);
            //}
            //else
            //{
            //    @ViewBag.status = "Old Password is incorrect";
            //}

            return View();
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
        public List<Add> GetMyAdds(Guid id)
        {
            List<Add> addColl = new List<Add>();
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

        [HttpPost]
        public ActionResult PreviewAdd(FormCollection coll, CustomAdd customAdd)
        {

            Add add = new Add();
            //CustomAdd customAdd = new CustomAdd();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = "http://localhost:51797/api/UserApi/GetAddById/?addId=" + customAdd.AddId;
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

                        using (var clientMsg = new HttpClient())
                        {
                            tbl_Messages tblMsg = new tbl_Messages();
                            tblMsg.AdId = customAdd.AddId;
                            tblMsg.FromUserId = Session["UserId"].ToString();
                            tblMsg.ToUserId = add.UserId.ToString();
                            tblMsg.Message = coll["user-message"].ToString();
                            string urlMsg = "http://localhost:51797/api/UserApi/AddMessage/?tblMsg=" + tblMsg;
                            clientMsg.BaseAddress = new Uri(urlMsg);
                            var postTaskMsg = clientMsg.PostAsJsonAsync<tbl_Messages>(urlMsg, tblMsg);
                            try
                            {
                                postTaskMsg.Wait();
                            }
                            catch (Exception ex)
                            {

                            }
                            var resultMsg = postTaskMsg.Result;
                            if (resultMsg.IsSuccessStatusCode)
                            {
                                //return RedirectToAction("Home", "User");
                            }
                            else
                            {
                                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                            }

                        }


                        CustomActions obj = new CustomActions();
                        customAdd = obj.CheckCategory(add);


                        List<tbl_Messages> tbl_Messages = new List<tbl_Messages>();
                        using (var clientMsg = new HttpClient())
                        {
                            tbl_Messages tblMsg = new tbl_Messages();
                            tblMsg.AdId = customAdd.AddId;
                            tblMsg.FromUserId = Session["UserId"].ToString();
                            tblMsg.ToUserId = add.UserId.ToString();
                            string urlMsg = String.Format("http://localhost:51797/api/UserApi/GetMessagesByFromToAd?tbl_Messages.AdId={0}&tblMsg.FromUserId={1}&tblMsg.ToUserId={2}", tblMsg.AdId, tblMsg.FromUserId, tblMsg.ToUserId);
                            string querystring = "http://localhost:51797/api/UserApi/GetMessagesByFromToAd?AdId=" + tblMsg.AdId + "&FromUserId=" + tblMsg.FromUserId + "&ToUserId=" + tblMsg.ToUserId;

                            clientMsg.BaseAddress = new Uri(urlMsg);
                            var postTaskMsg = clientMsg.GetAsync(querystring);
                            try
                            {
                                postTaskMsg.Wait();
                            }
                            catch (Exception ex)
                            {

                            }
                            var resultMsg = postTaskMsg.Result;
                            if (resultMsg.IsSuccessStatusCode)
                            {
                                var readTaskMsg = resultMsg.Content.ReadAsAsync<List<tbl_Messages>>();
                                readTaskMsg.Wait();

                                tbl_Messages = readTaskMsg.Result;
                                customAdd.tbl_Messages = tbl_Messages;
                                //return RedirectToAction("Home", "User");
                            }
                            else
                            {
                                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                            }
                        }
                        customAdd.tbl_Messages = tbl_Messages;

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
            //return RedirectToAction("MessageGrid", customAdd);
            //TempData["loginModel"] = customAdd;

        }

        public ActionResult ShowAdd()
        {

            return View();
        }


        public ActionResult ArchiveChat(string[] selectedIDs)
        {


            tbl_Messages tbl_Messages = new tbl_Messages();
            tbl_Messages.MovetoArchive = selectedIDs;

            using (var clientMsg = new HttpClient())
            {

                string urlMsg = "http://localhost:51797/api/UserApi/MovetoArchiveFolder/?MsgId=" + tbl_Messages;
                clientMsg.BaseAddress = new Uri(urlMsg);
                var postTaskMsg = clientMsg.PostAsJsonAsync<tbl_Messages>(urlMsg, tbl_Messages);
                try
                {
                    postTaskMsg.Wait();
                }
                catch (Exception ex)
                {

                }

                var result = postTaskMsg.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Home");
                }

            }
            return RedirectToAction("Home");


        }

        //public JsonResult ViewChat(string ValuesArray)
        //{
        //    string[] ValuesarrayItems = ValuesArray.Split('_');
        //    tbl_Messages tbl_Messages = new tbl_Messages();
        //    List<tbl_Messages> lst_tbl_Messages = new List<tbl_Messages>();


        //    tbl_Messages.MsgId = Convert.ToInt32(ValuesarrayItems[0]);
        //    tbl_Messages.AdId = Convert.ToInt32(ValuesarrayItems[1]);
        //    tbl_Messages.FromUserId = ValuesarrayItems[2];
        //    using (var clientMsg = new HttpClient())
        //    {
        //        string querystring = "http://localhost:51797/api/UserApi/ViewChat?MsgId=" + tbl_Messages.MsgId + "&AdId=" + tbl_Messages.AdId + "&FromUserId=" + tbl_Messages.FromUserId;
        //        //string urlMsg = "http://localhost:51797/api/UserApi/MovetoArchiveFolder/?MsgId=" + tbl_Messages;
        //        clientMsg.BaseAddress = new Uri(querystring);
        //        var postTaskMsg = clientMsg.GetAsync(querystring);
        //        try
        //        {
        //            postTaskMsg.Wait();
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //        var resultMsg = postTaskMsg.Result;
        //        if (resultMsg.IsSuccessStatusCode)
        //        {
        //            var readTaskMsg = resultMsg.Content.ReadAsAsync<List<tbl_Messages>>();
        //            readTaskMsg.Wait();
        //            lst_tbl_Messages = readTaskMsg.Result;
        //            //return RedirectToAction("Home", "User");
        //        }
        //        else
        //        {
        //            //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
        //        }

        //    }
        //    return Json(lst_tbl_Messages,JsonRequestBehavior.AllowGet);
        //}


        public List<MessageDetail> ViewhatMethod(int MsgId, int AdId, string FromUserId, string ToUserId, bool IsRed)
        {
            List<MessageDetail> lst_tbl_Messages = new List<MessageDetail>();
            try
            {
                using (var clientMsg = new HttpClient())
                {
                    string querystring = "http://localhost:51797/api/UserApi/ViewChat?MsgId=" + MsgId + "&AdId=" + AdId + "&FromUserId=" + FromUserId + "&ToUserId=" + ToUserId + "&IsRed=" + IsRed;
                    clientMsg.BaseAddress = new Uri(querystring);
                    var postTaskMsg = clientMsg.GetAsync(querystring);
                    try
                    {
                        postTaskMsg.Wait();
                    }
                    catch (Exception ex)
                    {

                    }

                    var resultMsg = postTaskMsg.Result;
                    if (resultMsg.IsSuccessStatusCode)
                    {
                        var readTaskMsg = resultMsg.Content.ReadAsAsync<List<MessageDetail>>();
                        readTaskMsg.Wait();
                        lst_tbl_Messages = readTaskMsg.Result;
                        //return RedirectToAction("Home", "User");
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return lst_tbl_Messages;
        }


        [HttpGet]
        public ActionResult ViewChatModal(string ValuesArray)
        {
            List<MessageDetail> lst_tbl_Messages = new List<MessageDetail>();
            if (ValuesArray != "")
            {
                string[] ValuesarrayItems = ValuesArray.Split('_');
                if (ValuesarrayItems != null && ValuesarrayItems.Length > 0)
                    lst_tbl_Messages = ViewhatMethod(Convert.ToInt32(ValuesarrayItems[0]), Convert.ToInt32(ValuesarrayItems[1]), ValuesarrayItems[2], ValuesarrayItems[3], Convert.ToBoolean(ValuesarrayItems[4]));
                TempData["MsgCollection"] = lst_tbl_Messages;
            }
            return PartialView(lst_tbl_Messages);
        }

        [HttpPost]
        public ActionResult ViewChatModal(FormCollection coll)
        {
            List<MessageDetail> UserPostChat = TempData["MsgCollection"] as List<MessageDetail>;
            using (var clientMsg = new HttpClient())
            {
                tbl_Messages tblMsg = new tbl_Messages();
                tblMsg.AdId = UserPostChat[0].AdId;
                tblMsg.FromUserId = UserPostChat[0].ToUserId;
                tblMsg.ToUserId = UserPostChat[0].FromUserId;
                tblMsg.Message = coll["name"].ToString();
                string urlMsg = "http://localhost:51797/api/UserApi/AddMessage/?tblMsg=" + tblMsg;
                clientMsg.BaseAddress = new Uri(urlMsg);
                var postTaskMsg = clientMsg.PostAsJsonAsync<tbl_Messages>(urlMsg, tblMsg);
                try
                {
                    postTaskMsg.Wait();
                }
                catch (Exception ex)
                {

                }
                var resultMsg = postTaskMsg.Result;
                if (resultMsg.IsSuccessStatusCode)
                {
                    //return RedirectToAction("Home", "User");
                }
                else
                {
                    //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }

            }

            //List<tbl_Messages> lst_tbl_Messages = new List<tbl_Messages>();
            //lst_tbl_Messages = ViewhatMethod(Convert.ToInt32(UserPostChat[0].MsgId), Convert.ToInt32(UserPostChat[0].AdId), UserPostChat[0].FromUserId, UserPostChat[0].ToUserId, Convert.ToBoolean(UserPostChat[0].IsRed));
            //TempData["MsgCollection"] = lst_tbl_Messages;
            //return PartialView(lst_tbl_Messages);

            return RedirectToAction("Home");
        }
    }
}
