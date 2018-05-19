﻿using System;
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
using System.Globalization;

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
            customAdd.Location = add.Mandal + "," + add.State;

            DateTime dtTemp = add.Created.Value;

            customAdd.CreatedDate = dtTemp.ToString("MMMM") + ", " + dtTemp.Day + ", " + dtTemp.Year; // .mon.ToLongDateString();
            customAdd.AddId = add.AddId;
            customAdd.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(add.Title);
            customAdd.Category = add.Category;
            switch (add.Category)
            {
                case Constants.RealEstate:
                    {
                        foreach (var item in add.RealEstates)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }
                case Constants.TransportationVehicle:
                    {
                        foreach (var item in add.TransportationVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }
                case Constants.ConstructionVehicle:
                    {
                        foreach (var item in add.ConstructionVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }
                case Constants.AgriculturalVehicle:
                    {
                        foreach (var item in add.AgriculturalVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }
                case Constants.PassengerVehicle:
                    {
                        foreach (var item in add.PassengerVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.Price = item.Price;
                        }
                        break;
                    }

            }
            return customAdd;
        }
        [HttpGet]
        [ActionName("Admin")]
        public  IHttpActionResult Admin()
        {
            var addColl = (from add in db.Adds 
                            select new
                            {
                                AddId = add.AddId,
                                Created = add.Created,
                                Category = add.Category,
                                State = add.State,
                                District = add.District,
                                Mandal = add.Mandal,
                                Status = add.Status
                            }).ToList()
                            .Select(add=> new Add()
                            {
                           AddId = add.AddId,
                           Created = add.Created,
                           Category = add.Category,
                           State = add.State,
                           District = add.District,
                           Mandal = add.Mandal,
                          Status = add.Status
                          });
            if (addColl.Count() > 0)
            {
                return Ok(addColl);
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