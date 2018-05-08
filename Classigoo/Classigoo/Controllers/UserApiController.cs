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

        // POST: api/UsersApi

        [HttpPost]
        [ActionName("AddLog")]
        public IHttpActionResult AddLog(Log log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                db.Logs.Add(log);
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
               
            }

            return Ok(log);
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
        public IHttpActionResult CheckUser(string id,string type)
        {
            bool IsUserExist = false;
            if(type=="Gmail")
            {
                IsUserExist= db.Users.Count(e => e.Email == id) > 0;
            }
            else if(type=="Fb")
            {
                IsUserExist= db.Users.Count(e => e.FbId == id) > 0;
            }
            else if(type=="Custom")
            {
                IsUserExist= db.Users.Count(e => e.MobileNumber == id) > 0;
            }
            return Ok(IsUserExist);
        }
        [HttpGet]
        public IHttpActionResult IsValidUser(string userName, string pwd,string logintype)
        {
            List<User> user = new List<User>();
            if (logintype=="email")
            {
                 user = db.Users.Where(u => u.Email == userName).Where(u => u.Password == pwd).ToList();
            }
            else if(logintype=="phone")
            {
                user = db.Users.Where(u => u.MobileNumber == userName).Where(u => u.Password == pwd).ToList();
            }
            
            if(user.Count()>0)
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
            catch(Exception ex)
            {

            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        [HttpGet]
        [ActionName("GetMyAdds")]
        public IHttpActionResult GetMyAdds(Guid userId)
        {
            List<Add> adds = db.Adds.Where(a => a.UserId == userId).ToList();
            List<CustomAdd> addColl = new List<CustomAdd>();
            foreach(Add add in adds)
            {
                addColl.Add(CheckCategory(add));
            }
            if(addColl.Count>0)
            return Ok(addColl);
            else
                return NotFound();
        }
        [HttpGet]
        [ActionName("GetAddById")]
        public IHttpActionResult GetAddById(int addId)
        {
            Add add = db.Adds.Find(addId);
            if (add!=null)
                return Ok(add);
            else
                return NotFound();
        }
        public CustomAdd CheckCategory(Add add)
        {
            CustomAdd customAdd = new CustomAdd();
            customAdd.CreatedDate = add.Created.ToString();
            customAdd.AddId = add.AddId;
            switch (add.Category)
            {
                case "RealEstate":
                    {

                        foreach (var item in add.RealEstates)
                        {
                           // customAdd.Description = item.Description;
                           // customAdd.Title = item.Title;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }
                case "Cars":
                    {
                        //foreach (var item in add.Cars)
                        //{
                        //  //  customAdd.Description = item.Description;
                        //    customAdd.Title = item.Title;
                        //    customAdd.Price = item.Price;
                        //}
                        break;
                    }
                case "Electronics":
                    {
                        //foreach (var item in add.Electronics)
                        //{
                        //  //  customAdd.Description = item.Description;
                        //    customAdd.Title = item.Title;
                        //    customAdd.Price = item.Price;
                        //}
                        break;
                    }

            }
            return customAdd;
        }
        [HttpGet]
        [ActionName("Admin")]
        public  IHttpActionResult Admin()
        {
            List<Add> coll = new List<Add>();
            coll = db.Adds.ToList();
            if (coll.Count > 0)
            {
                return Ok(coll);
            }
            else
                return NotFound();
        }
        [HttpPut]
        [ActionName("UpdateAddStatus")]
        public IHttpActionResult UpdateAddStatus(int addId,string status)
        {
            try
            {
                Add add = db.Adds.Find(addId);
                add.Status = status;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                db.Entry(add).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
            }
            catch (Exception ex)
            {

            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}