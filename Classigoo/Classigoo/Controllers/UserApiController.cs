using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web;
using Classigoo;

namespace Classigoo.Controllers
{
    public class UserApiController : ApiController
    {
        private ClassigooEntities db = new ClassigooEntities();

        // GET: api/UsersApi
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/UsersApi/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(Guid id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/UsersApi/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutUser(Guid id, User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != user.UserId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/UsersApi

        [HttpPost]
        [ActionName("AddUser")]
        public IHttpActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                user.Created = DateTime.Now;
                user.UserId = Guid.NewGuid();
            }

            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
        }

        // DELETE: api/UsersApi/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(Guid id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(Guid id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
        [HttpGet]
        public IHttpActionResult CheckUser(string id, string type)
        {
            bool IsUserExist = false;
            if (type == "Gmail")
            {
                IsUserExist = db.Users.Count(e => e.Email == id) > 0;
            }
            else if (type == "Fb")
            {
                IsUserExist = db.Users.Count(e => e.FbId == id) > 0;
            }
            else if (type == "Custom")
            {
                IsUserExist = db.Users.Count(e => e.MobileNumber == id) > 0;
            }
            return Ok(IsUserExist);
        }
        [HttpGet]
        public IHttpActionResult IsValidUser(string userName, string pwd, string logintype)
        {
            List<User> user = new List<User>();
            if (logintype == "email")
            {
                user = db.Users.Where(u => u.Email == userName).Where(u => u.Password == pwd).ToList();
            }
            else if (logintype == "phone")
            {
                user = db.Users.Where(u => u.MobileNumber == userName).Where(u => u.Password == pwd).ToList();
            }

            if (user.Count() > 0)
            {

                return Ok(user[0].UserId);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpPut]
        [ActionName("UpdateUserDetails")]
        public IHttpActionResult UpdateUserDetails(User user)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                db.Entry(user).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        [HttpGet]
        [ActionName("GetMyAdds")]
        public IHttpActionResult GetMyAdds(Guid userId)
        {
            var adds = db.Adds.Where(a => a.UserId == userId).ToList();
            if (adds.Count > 0)
                return Ok(adds);
            else
                return NotFound();
        }
        [HttpGet]
        [ActionName("GetAddById")]
        public IHttpActionResult GetAddById(int addId)
        {
            Add add = db.Adds.Find(addId);
            if (add != null)
                return Ok(add);
            else
                return NotFound();
        }


        [HttpPost]
        [ActionName("AddMessage")]
        public IHttpActionResult AddMessage(tbl_Messages tblMsg)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //else
            {
                tblMsg.CreatedOn = DateTime.Now;
            }

            db.tbl_Messages.Add(tblMsg);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                //if (UserExists(user.UserId))
                //{
                //    return Conflict();
                //}
                //else
                //{
                //    throw;
                //}
                throw;
            }

            return Ok(tblMsg);
        }


        [HttpGet]
        [ActionName("GetMessagesByFromToAd")]
        public IHttpActionResult GetMessagesByFromToAd(int AdId, string FromUserId, string ToUserId)
        {
            List<tbl_Messages> lst_tbl_Messages = db.tbl_Messages.ToList().Where(m => ((m.FromUserId == FromUserId && m.ToUserId == ToUserId) || (m.FromUserId == ToUserId && m.ToUserId == FromUserId)) && (m.AdId == AdId)).ToList();
            if (lst_tbl_Messages != null)
                return Ok(lst_tbl_Messages);
            else
                return NotFound();
            //return Ok(AdId);
        }

        [HttpGet]
        [ActionName("GetMessagesByUserId")]
        public IHttpActionResult GetMessagesByUserId(string UserId, string Mode)
        {
            List<MessageDetail> lst_tbl_Messages = new List<MessageDetail>();

            switch (Mode)
            {
                case "":
                case "Inbox":
                    //lst_tbl_Messages = db.tbl_Messages.ToList().Where(m => (m.ToUserId == UserId)).ToList();
                    lst_tbl_Messages = (from Msg in db.tbl_Messages.ToList()
                                        join FromUser in db.Users.ToList() on Msg.FromUserId equals FromUser.UserId.ToString()
                                        join ToUser in db.Users.ToList() on Msg.ToUserId equals ToUser.UserId.ToString()
                                        join tblAd in db.Adds.ToList() on Msg.AdId equals tblAd.AddId
                                        where (Msg.ToUserId == UserId)
                                        select new MessageDetail()
                                        {
                                            MsgId = Msg.MsgId,
                                            FromUserId = Msg.FromUserId,
                                            ToUserId = Msg.ToUserId,
                                            FromUser = FromUser.Name,
                                            ToUser = ToUser.Name,
                                            AdId = Msg.AdId,
                                            AdTitle = tblAd.Category,
                                            Message = Msg.Message,
                                            CreatedOn = Msg.CreatedOn,
                                            UpdatedOn = Msg.UpdatedOn,
                                            IsRed = Msg.IsRed,
                                        }).ToList();


                    break;
                case "Sent":
                    //lst_tbl_Messages = db.tbl_Messages.ToList().Where(m => (m.FromUserId == UserId)).ToList();
                    lst_tbl_Messages = (from Msg in db.tbl_Messages.ToList()
                                        join FromUser in db.Users.ToList() on Msg.FromUserId equals FromUser.UserId.ToString()
                                        join ToUser in db.Users.ToList() on Msg.ToUserId equals ToUser.UserId.ToString()
                                        join tblAd in db.Adds.ToList() on Msg.AdId equals tblAd.AddId
                                        where (Msg.FromUserId == UserId)
                                        select new MessageDetail()
                                        {
                                            MsgId = Msg.MsgId,
                                            FromUserId = Msg.FromUserId,
                                            ToUserId = Msg.ToUserId,
                                            FromUser = FromUser.Name,
                                            ToUser = ToUser.Name,
                                            AdId = Msg.AdId,
                                            AdTitle = tblAd.Category,
                                            Message = Msg.Message,
                                            CreatedOn = Msg.CreatedOn,
                                            UpdatedOn = Msg.UpdatedOn,
                                            IsRed = Msg.IsRed,
                                        }).ToList();
                    break;
                case "Archive":
                    //lst_tbl_Messages = db.tbl_Messages.ToList().Where(m => (m.ToUserId == UserId)).ToList();
                    lst_tbl_Messages = (from Msg in db.tbl_Messages.ToList()
                                        join FromUser in db.Users.ToList() on Msg.FromUserId equals FromUser.UserId.ToString()
                                        join ToUser in db.Users.ToList() on Msg.ToUserId equals ToUser.UserId.ToString()
                                        join tblAd in db.Adds.ToList() on Msg.AdId equals tblAd.AddId

                                        where (Msg.ToUserId == UserId && Msg.Archive == true)
                                        select new MessageDetail()
                                        {
                                            MsgId = Msg.MsgId,
                                            FromUserId = Msg.FromUserId,
                                            ToUserId = Msg.ToUserId,
                                            FromUser = FromUser.Name,
                                            ToUser = ToUser.Name,
                                            AdId = Msg.AdId,
                                            AdTitle = tblAd.Category,
                                            Message = Msg.Message,
                                            CreatedOn = Msg.CreatedOn,
                                            UpdatedOn = Msg.UpdatedOn,
                                            IsRed = Msg.IsRed,
                                        }).ToList();
                    break;
                default:
                    break;
            }

            if (lst_tbl_Messages != null)
                return Ok(lst_tbl_Messages);
            else
                return NotFound();
            //return Ok(AdId);
        }

        [HttpPost]
        [ActionName("MovetoArchiveFolder")]
        public IHttpActionResult MovetoArchiveFolder(tbl_Messages tbl_Messages)
        {

            try
            {

                foreach (var SelectedID in tbl_Messages.MovetoArchive)
                {
                    int selectedId = Convert.ToInt32(SelectedID);
                    tbl_Messages = db.tbl_Messages.Find(selectedId);

                    var query = (from q in db.tbl_Messages
                                 where q.MsgId == selectedId
                                 select q).First();
                    query.Archive = true;
                    db.Entry(tbl_Messages).State = EntityState.Modified;
                    db.SaveChanges();

                }


            }
            catch (DbUpdateException)
            {

                throw;
            }

            return Ok(tbl_Messages);
        }

        [HttpGet]
        [ActionName("ViewChat")]
        public IHttpActionResult ViewChat(int MsgId, int AdId, string FromUserId, string ToUserId, bool IsRed)
        {
            //List<tbl_Messages> lst_tbl_Messages = new List<tbl_Messages>();
            //lst_tbl_Messages = db.tbl_Messages.Where(d => d.AdId == AdId && d.FromUserId == FromUserId && d.ToUserId==ToUserId).ToList();
            //tbl_Messages tbl_Messages = new tbl_Messages();
            //foreach (var item in lst_tbl_Messages)
            //{
            //    tbl_Messages=item;
            //    tbl_Messages.IsRed = true;
            //    db.Entry(tbl_Messages).State = EntityState.Modified;
            //    db.SaveChanges();
            //}


            List<MessageDetail> lst_tbl_Messages = new List<MessageDetail>();

            lst_tbl_Messages = (from Msg in db.tbl_Messages.ToList()
                                join FromUser in db.Users.ToList() on Msg.FromUserId equals FromUser.UserId.ToString()
                                join ToUser in db.Users.ToList() on Msg.ToUserId equals ToUser.UserId.ToString()
                                join tblAd in db.Adds.ToList() on Msg.AdId equals tblAd.AddId
                                where (Msg.AdId == AdId && (Msg.FromUserId == FromUserId && Msg.ToUserId == ToUserId) || (Msg.FromUserId == ToUserId && Msg.ToUserId == FromUserId))
                                select new MessageDetail()
                                {
                                    MsgId = Msg.MsgId,
                                    FromUserId = Msg.FromUserId,
                                    ToUserId = Msg.ToUserId,
                                    FromUser = FromUser.Name,
                                    ToUser = ToUser.Name,
                                    AdId = Msg.AdId,
                                    AdTitle = tblAd.Category,
                                    Message = Msg.Message,
                                    CreatedOn = Msg.CreatedOn,
                                    UpdatedOn = Msg.UpdatedOn,
                                    IsRed = Msg.IsRed,
                                }).ToList();

            foreach (MessageDetail msgDtl in lst_tbl_Messages)
            {
                if (msgDtl.ToUserId == ToUserId)
                {
                    tbl_Messages tbl_Messages = db.tbl_Messages.Find(msgDtl.MsgId);
                    tbl_Messages.IsRed = true;
                    db.Entry(tbl_Messages).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Ok(lst_tbl_Messages);

        }
    }

}