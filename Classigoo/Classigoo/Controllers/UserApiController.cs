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
using System.Globalization;
using Classigoo.Models;

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
            catch (DbUpdateException ex)
            {
                if (UserExists(user.UserId))
                {
                    Library.WriteLog("At adduser user already exist", ex);
                   // return Conflict();
                }
                else
                {
                    Library.WriteLog("At adduser", ex);
                    // throw;
                }
            }

            return Ok(user);
        }
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
            User user = new User();
            try
            {
                if (type == "Gmail")
                {
                    user = db.Users.Where(e => e.Email == id).FirstOrDefault();
                }
                else if (type == "Fb")
                {
                    user = db.Users.Where(e => e.FbId == id).FirstOrDefault();
                }
                else if (type == "Custom")
                {
                    user = db.Users.Where(e => e.MobileNumber == id).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At checking user username- "+ id, ex);
            }
            if(user!=null)
            {
                return Ok(user.UserId);
            }
            else
            {
                return Ok(Guid.Empty);
            }
            
        }
        [HttpGet]
        public IHttpActionResult IsValidUser(string userName, string pwd,string logintype)
        {
            User user = new User();
            try
            {
                if (logintype == "email")
                {
                    user = db.Users.Where(u => u.Email == userName).Where(u => u.Password == pwd).FirstOrDefault();
                }
                else if (logintype == "phone")
                {
                    user = db.Users.Where(u => u.MobileNumber == userName).Where(u => u.Password == pwd).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At isvalid user username- "+userName, ex);
            }
            
            if(user!=null)
            {
                return Ok(user.UserId);
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
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                db.Entry(user).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Library.WriteLog("At updating user details- dbupdate", ex);
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At updating user details",ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        [HttpGet]
        [ActionName("GetMyAdds")]
        public IHttpActionResult GetMyAdds(Guid userId)
        {
            List<CustomAdd> addColl = new List<CustomAdd>();
            try
            {
                List<Add> adds = db.Adds.Where(a => a.UserId == userId).OrderByDescending(add => add.Created).ToList();
               
                foreach (Add add in adds)
                {
                    addColl.Add(CheckCategory(add));
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At GetMyadds for userid- " + userId, ex);
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
            Add add = new Add();
            try
            {
                 add = db.Adds.Find(addId);
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Getting add by id- "+addId, ex);
            }
            if (add!=null)
                return Ok(add);
            else
                return NotFound();
        }
        public CustomAdd CheckCategory(Add add)
        {
            CustomAdd customAdd = new CustomAdd();
            try
            {
                customAdd.Location = add.Mandal + "," + add.State;

                DateTime dtTemp = add.Created.Value;

                customAdd.CreatedDate = dtTemp.ToString("MMMM") + ", " + dtTemp.Day + ", " + dtTemp.Year; // .mon.ToLongDateString();
                customAdd.AddId = add.AddId;
                customAdd.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(add.Title);
                customAdd.Category = add.Category;
                customAdd.Status = add.Status;
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
            }
            catch(Exception ex)
            {
                Library.WriteLog("Checking Category for getmyadds", ex);
            }
            return customAdd;
        }
        [HttpGet]
        [ActionName("Admin")]
        public  IHttpActionResult Admin()
        {
            
              var   addColl = (from add in db.Adds
                               select new
                               {
                                   AddId = add.AddId,
                                   Created = add.Created,
                                   Category = add.Category,
                                   State = add.State,
                                   District = add.District,
                                   Mandal = add.Mandal,
                                   Status = add.Status
                               }).OrderByDescending(add => add.Created).ToList()
                                .Select(add => new Add()
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
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                db.Entry(add).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Library.WriteLog("At Updating Add Status- db update", ex);
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Updating Add Status", ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}