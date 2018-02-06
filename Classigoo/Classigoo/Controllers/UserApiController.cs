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
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
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

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
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
        public IHttpActionResult IsValidUser(string userName, string pwd)
        {
        
            var user = db.Users.Where(u => u.MobileNumber == userName).Where(u => u.Password == pwd).ToList();
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
        
    }
}