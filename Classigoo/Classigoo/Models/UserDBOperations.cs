﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace Classigoo.Models
{
    public class UserDBOperations
    {

        public User GetUser(Guid id)
        {
            User user = new User();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                 user = db.Users.Find(id);
                 return user;
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Get User", ex);
                return user;
            }
               
        }

        public Guid AddUser(User user)
        {
            try
            {
                user.Created = DateTime.Now;
                user.UserId = Guid.NewGuid();
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At AddUser db", ex);
                return Guid.Empty;
            }

            return user.UserId;
        }
      
        public Guid UserExist(string id, string type)
        {
            User user = new User();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
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
            }
            catch (Exception ex)
            {
                Library.WriteLog("At UserExist db UserName- " + id, ex);
                return Guid.Empty;
            }
            if (user!= null)
            {
                return user.UserId;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public Guid IsValidUser(string userName, string pwd, string logintype)
        {
            User user = new User();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
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
            }
            catch (Exception ex)
            {
                Library.WriteLog("At IsValidUser UserName- " + userName, ex);
                return Guid.Empty;
            }

            if (user != null)
            {
               return user.UserId;
            }
            else
            {
                return Guid.Empty;
            }

        }

        public bool UpdateUserDetails(User user)
        {
            int response = 0;
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    db.Entry(user).State = EntityState.Modified;
                    response = db.SaveChanges();  
                }
                
            }
            catch (Exception ex)
            {
                Library.WriteLog("At UpdatingUserDetails UserId - "+user.UserId, ex);
            }
            if (response == 1)
                return true;
            else
            {
                return false;
            }
        }
       
        public List<CustomAdd> GetMyAdds(Guid userId)
        {
            List<CustomAdd> addColl = new List<CustomAdd>();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    List<Add> adds = db.Adds.Where(a => a.UserId == userId).
                        OrderByDescending(add => add.Created).ToList();

                    foreach (Add add in adds)
                    {
                        CustomAdd customAdd = new CustomAdd();
                        try
                        {
                            customAdd.AddId = add.AddId;
                            customAdd.CreatedDate = add.Created.ToString();
                            customAdd.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(add.Title);
                            customAdd.Status = add.Status;
                            #region RealEstates
                            if (add.RealEstates.Count()==1)
                            {
                                foreach (var item in add.RealEstates)
                                {
                                    customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                                    customAdd.Price = item.Price;
                                }
                            }
                            #endregion
                            #region TransportationVehicles
                            else if (add.TransportationVehicles.Count==1)
                            {
                                foreach (var item in add.TransportationVehicles)
                                {
                                    customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                                    customAdd.Price = item.Price;
                                }
                            }
                            #endregion
                            #region ConstructionVehicles
                            else if (add.ConstructionVehicles.Count==1)
                            {
                                foreach (var item in add.ConstructionVehicles)
                                {
                                    customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                                    customAdd.Price = item.Price;
                                }
                            }
                            #endregion
                            #region AgriculturalVehicles
                            else if (add.AgriculturalVehicles.Count == 1)
                            {
                                foreach (var item in add.AgriculturalVehicles)
                                {
                                    customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                                    customAdd.Price = item.Price;
                                }
                            }
                            #endregion
                            #region PassengerVehicles
                            else if (add.PassengerVehicles.Count == 1)
                            {
                                foreach (var item in add.PassengerVehicles)
                                {
                                    customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                                    customAdd.Price = item.Price;
                                }
                            }
                            #endregion

                            addColl.Add(customAdd);
                        }
                        catch (Exception ex)
                        {
                            Library.WriteLog("Checking Category for getmyadds addid- "+add.AddId, ex);
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At GetMyadds  UserId - " + userId, ex);
            }
            return addColl;

        }
       
        public Add GetAddById(int addId)
        {
            Add add = new Add();
            try
            {
                ClassigooEntities db = new ClassigooEntities();
                //  using (ClassigooEntities db = new ClassigooEntities())
                //{
                add = db.Adds.Find(addId);
                //}
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Getting add by id- " + addId, ex);
            }

            return add;
        }
     
        public IEnumerable<AdminAdd>  GetAdminAdds()
        {
            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    addColl = (from add in db.Adds
                               select new
                               {
                                   AddId = add.AddId,
                                   Created = add.Created,
                                   Category = add.Category,
                                   State = add.State,
                                   District = add.District,
                                   Mandal = add.Mandal,
                                   Status = add.Status,
                                   Type = add.Type,
                                   UserName = add.User.Name,
                                   PhoneNum = add.User.MobileNumber
                               }).OrderByDescending(add => add.Created).ToList()
                                     .Select(add => new AdminAdd()
                                     {
                                         AddId = add.AddId,
                                         Created = add.Created,
                                         Category = add.Category,
                                         State = add.State,
                                         District = add.District,
                                         Mandal = add.Mandal,
                                         Status = add.Status,
                                         Type = add.Type,
                                         UserName = add.UserName,
                                         PhoneNum = add.PhoneNum
                                     });
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Admin db", ex);
            }
            return addColl;
        }
        
        public bool UpdateAddStatus(int addId, string status)
        {
            int response = 0;
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    Add add = db.Adds.SingleOrDefault(a => a.AddId == addId);
                    if (add != null)
                    {
                        add.Status = status;
                        response = db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Updating Add Status db addId- "+addId, ex);
            }
            Library.WriteLog("At updating add status response code- " + response);
            if (response == 1)
            {
                return true;
            }
            else
            { 
                return false;
            }
        }
        
    }
}