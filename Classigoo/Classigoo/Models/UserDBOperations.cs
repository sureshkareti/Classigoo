using System;
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
                user.Created = CustomActions.GetCurrentISTTime();
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
                            CommonDBOperations objCommonDbOperations = new CommonDBOperations();
                            #region RealEstates
                            if (add.Category==Constants.RealEstate)
                            {
                                RealEstate re = objCommonDbOperations.GetRealEstate(add.AddId.ToString());
                                if(re!=null)
                                {
                                    customAdd.ImgUrlPrimary = re.ImgUrlPrimary;
                                    customAdd.Price = re.Price;
                                }
                            }
                            #endregion
                            #region TransportationVehicles
                            else if (add.Category==Constants.TransportationVehicle)
                            {
                                TransportationVehicle tv = objCommonDbOperations.GetTV(add.AddId.ToString());
                                if(tv!=null)
                                {
                                    customAdd.ImgUrlPrimary = tv.ImgUrlPrimary;
                                    customAdd.Price = tv.Price;
                                }
                            }
                            #endregion
                            #region ConstructionVehicles
                            else if (add.Category==Constants.ConstructionVehicle)
                            {
                                ConstructionVehicle cv = objCommonDbOperations.GetCV(add.AddId.ToString());
                                if(cv!=null)
                                {
                                    customAdd.ImgUrlPrimary = cv.ImgUrlPrimary;
                                    customAdd.Price = cv.Price;
                                }
                            }
                            #endregion
                            #region AgriculturalVehicles
                            else if (add.Category == Constants.AgriculturalVehicle)
                            {
                                AgriculturalVehicle av = objCommonDbOperations.GetAV(add.AddId.ToString());
                                if(av!=null)
                                {
                                    customAdd.ImgUrlPrimary = av.ImgUrlPrimary;
                                    customAdd.Price = av.Price;
                                }
                            }
                            #endregion
                            #region PassengerVehicles
                            else if (add.Category == Constants.PassengerVehicle)
                            {
                                PassengerVehicle pv = objCommonDbOperations.GetPV(add.AddId.ToString());
                                if(pv!=null)
                                {
                                    customAdd.ImgUrlPrimary = pv.ImgUrlPrimary;
                                    customAdd.Price = pv.Price;
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
                                   PhoneNum = add.User.MobileNumber,
                                   Remarks=add.Remarks
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
                                         PhoneNum = add.PhoneNum,
                                         Remarks = add.Remarks
                                     });
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Admin db", ex);
            }
            return addColl;
        }
        
        public bool UpdateAddStatus(int addId, string status,string remarks)
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
                        add.Remarks = remarks;
                        response = db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Updating Add Status db addId- "+addId, ex);
            }
            if (response == 1)
            {
                return true;
            }
            else
            { 
                return false;
            }
        }

        public void IsPwdEmpty(Guid userId)
        {
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    db.Users.Find(userId);
                }
            }

            catch(Exception ex)
            {

            }
        }
        
    }
}