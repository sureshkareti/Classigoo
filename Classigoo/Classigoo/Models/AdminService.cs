using Classigoo.Models.Search;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Classigoo.Models
{
    public class AdminService
    {
        public static List<Category> GetCatagories()
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    List<Category> objRealestae = classigooEntities.Categories.Include("SubCategories").ToList<Category>();

                    if (objRealestae != null)
                    {
                        return objRealestae;
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get GetCatagories()", ex);

            }

            return null;
        }
     

        public List<string> GetOwnersMobileNos()
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    List<string> mnColl = classigooEntities.Adds.Include("User").ToList().Select(add => add.User.MobileNumber).ToList();
                    // .//Include("User").Select(user=>user.User.MobileNumber)

                    return mnColl.Distinct<string>().ToList();
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get GetOwnersMobileNos()", ex);

            }

            return null;
        }

        public List<string> GetConsumersMobileNos()
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    List<string> mnColl = classigooEntities.Surveys.Select(consumer => consumer.PhoneNumber).ToList();
                    // .//Include("User").Select(user=>user.User.MobileNumber)

                    return mnColl.Distinct<string>().ToList();
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get GetConsumersMobileNos()", ex);

            }

            return null;
        }

        public IEnumerable<AdminAdd> GetEmpAdds(string empId)
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
                                   Remarks = add.Remarks,
                                   SubCategory = add.SubCategory,
                                   PostedBy = add.PostedBy
                               }).OrderByDescending(add => add.Created).Where(add => add.PostedBy == empId).ToList()
                                     .Select(add => new AdminAdd()
                                     {
                                         AddId = add.AddId.ToString(),
                                         Created = add.Created,
                                         Category = add.Category,
                                         State = add.State,
                                         District = add.District,
                                         Mandal = add.Mandal,
                                         Status = add.Status,
                                         Type = add.Type,
                                         UserName = add.UserName,
                                         PhoneNum = add.PhoneNum,
                                         Remarks = add.Remarks,
                                         SubCategory = add.SubCategory,
                                         PostedBy = add.PostedBy
                                     });
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At empadds db", ex);
            }
            return addColl;
        }

        public List<string> GetOwnersMobileNos(SearchOwnerAddsEntity searchQuery)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {

                    List<string> mnColl = (from add in classigooEntities.Adds
                                           join user in classigooEntities.Users on add.UserId equals user.UserId
                                           where
                          (add.Type == searchQuery.Type) &&
                         (searchQuery.Status != "" ? add.Status == searchQuery.Status : true) &&
                           (searchQuery.MobileNumber != "" ? user.MobileNumber == searchQuery.MobileNumber : true) &&
                                         (searchQuery.State != "" ? add.State == searchQuery.State : true) &&
                                          (searchQuery.District != "" ? add.District == searchQuery.District : true) &&
                                          (searchQuery.Mandal != "" ? add.Mandal == searchQuery.Mandal : true) &&
                                        (searchQuery.Category != "select" ? add.Category == searchQuery.Category : true) &&
                                         (searchQuery.SubCategory != "select" ? add.SubCategory == searchQuery.SubCategory : true)

                                           select user.MobileNumber).ToList();


                    return mnColl.Distinct<string>().ToList();
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get GetOwnersMobileNos()", ex);

            }

            return null;
        }

        public List<string> GetConsumerMobileNos(SearchOwnerAddsEntity searchQuery)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {

                    List<string> mnColl = (from survey in classigooEntities.Surveys
                                               //join user in classigooEntities.Users on survey.UserId equals user.UserId
                                           where
                          (survey.UserType == searchQuery.Type) &&
                         (searchQuery.Status != "" ? survey.Status == searchQuery.Status : true) &&
                           (searchQuery.MobileNumber != "" ? survey.PhoneNumber == searchQuery.MobileNumber : true) &&
                                         (searchQuery.State != "" ? survey.State == searchQuery.State : true) &&
                                          (searchQuery.District != "" ? survey.District == searchQuery.District : true) &&
                                          (searchQuery.Mandal != "" ? survey.Mandal == searchQuery.Mandal : true) &&
                                        (searchQuery.Category != "select" ? survey.Category == searchQuery.Category : true) &&
                                         (searchQuery.SubCategory != "select" ? survey.SubCategory == searchQuery.SubCategory : true)

                                           select survey.PhoneNumber).ToList();


                    return mnColl.Distinct<string>().ToList();
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get GetOwnersMobileNos()", ex);

            }

            return null;
        }

        public IEnumerable<AdminAdd> GetOwnersAdds(SearchOwnerAddsEntity searchQuery)
        {
            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {

                    addColl = (from add in classigooEntities.Adds
                               join user in classigooEntities.Users on add.UserId equals user.UserId
                               where
              (add.Type == searchQuery.Type) &&
             (searchQuery.Status != "" ? add.Status == searchQuery.Status : true) &&
               (searchQuery.MobileNumber != "" ? user.MobileNumber == searchQuery.MobileNumber : true) &&
                             (searchQuery.State != "" ? add.State == searchQuery.State : true) &&
                              (searchQuery.District != "" ? add.District == searchQuery.District : true) &&
                              (searchQuery.Mandal != "" ? add.Mandal == searchQuery.Mandal : true) &&
                            (searchQuery.Category != "select" ? add.Category == searchQuery.Category : true) &&
                             (searchQuery.SubCategory != "select" ? add.SubCategory == searchQuery.SubCategory : true)

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
                                   Remarks = add.Remarks,
                                   SubCategory = add.SubCategory,
                                   AddStatus = add.AddStatus,
                                   ReceiptNumber = add.ReceiptNumber

                               }).OrderByDescending(add => add.Created).ToList()
                                           .Select(add => new AdminAdd()
                                           {
                                               AddId = add.AddId.ToString(),
                                               Created = add.Created,
                                               Category = add.Category,
                                               State = add.State,
                                               District = add.District,
                                               Mandal = add.Mandal,
                                               Status = add.Status,
                                               Type = add.Type,
                                               UserName = add.UserName,
                                               PhoneNum = add.PhoneNum,
                                               Remarks = add.Remarks,
                                               SubCategory = add.SubCategory,
                                               AddStatus = add.AddStatus,
                                               ReceiptNumber = add.ReceiptNumber
                                           });


                    return addColl;
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get GetOwnersMobileNos()", ex);

            }

            return null;
        }
        public IEnumerable<AdminAdd> GetConsumersAdds(SearchOwnerAddsEntity searchQuery)
        {
            IEnumerable<AdminAdd> addColl = new List<AdminAdd>();
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {

                    addColl = (from survey in classigooEntities.Surveys
                                   //join user in classigooEntities.Users on add.UserId equals user.UserId
                               where
              (survey.UserType == searchQuery.Type) &&
             (searchQuery.Status != "" ? survey.Status == searchQuery.Status : true) &&
               (searchQuery.MobileNumber != "" ? survey.PhoneNumber == searchQuery.MobileNumber : true) &&
                             (searchQuery.State != "" ? survey.State == searchQuery.State : true) &&
                              (searchQuery.District != "" ? survey.District == searchQuery.District : true) &&
                              (searchQuery.Mandal != "" ? survey.Mandal == searchQuery.Mandal : true) &&
                            (searchQuery.Category != "select" ? survey.Category == searchQuery.Category : true) &&
                             (searchQuery.SubCategory != "select" ? survey.SubCategory == searchQuery.SubCategory : true)

                               select new
                               {
                                   AddId = survey.AddIdColl,
                                   Created = survey.CreatedDate,
                                   Category = survey.Category,
                                   State = survey.State,
                                   District = survey.District,
                                   Mandal = survey.Mandal,
                                   Status = survey.Status,
                                   Type = survey.UserType,
                                   UserName = survey.Name,
                                   PhoneNum = survey.PhoneNumber,
                                   Remarks = survey.Remarks,
                                   SubCategory = survey.SubCategory,
                                   AddStatus = survey.Status,
                                   // ReceiptNumber = survey.re

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
                                               Remarks = add.Remarks,
                                               SubCategory = add.SubCategory,
                                               AddStatus = add.AddStatus,
                                               // ReceiptNumber = add.ReceiptNumber
                                           });


                    return addColl;
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get GetOwnersMobileNos()", ex);

            }

            return null;
        }

        public bool AddSurvey(Survey survey)
        {
            bool isSuccess = true;
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {

                    db.Surveys.Add(survey);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At AddSurvey db", ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        public Survey GetSurvey(string custId)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(custId);
                    Survey objAdd = classigooEntities.Surveys.SingleOrDefault(a => a.Id == id);

                    if (objAdd != null)
                    {
                        return objAdd;
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get add post survey operations", ex);
            }

            return null;

        }

        public List<Survey> GetSurveys()
        {
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {

                    db.Surveys.ToList();

                    return db.Surveys.ToList();
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At GetSurvey db", ex);
                return null;
            }
        }

        public bool UpdateSurvey(Survey survey)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {

                    var objAddTemp = classigooEntities.Surveys.ToList().FindAll(x => x.Id == survey.Id);

                    if (objAddTemp.Count > 0)
                    {
                        var objSurvey = objAddTemp[0];
                        if (objSurvey != null)
                        {
                            objSurvey.Category = survey.Category;
                            objSurvey.SubCategory = survey.SubCategory;
                            objSurvey.State = survey.State;
                            objSurvey.District = survey.District;
                            objSurvey.Mandal = survey.Mandal;

                            objSurvey.Name = survey.Name;
                            objSurvey.PhoneNumber = survey.PhoneNumber;

                            objSurvey.UserType = survey.UserType;
                            objSurvey.Status = survey.Status;
                            objSurvey.AddIdColl = survey.AddIdColl;
                            objSurvey.Remarks = survey.Remarks;

                            int response = classigooEntities.SaveChanges();
                            if (response == 1)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at update add post survey operations", ex);
                return false;
            }

            return true;
        }

        public bool UpdateCustomerStatus(int cId, string status, string ramarks, string reciptNumber)
        {
            int response = 0;
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    Survey survey = db.Surveys.Find(cId);
                    if (survey != null)
                    {
                        survey.Status = status;
                        survey.Remarks = ramarks;
                        survey.ReceiptNumber = reciptNumber;
                        db.Entry(survey).State = EntityState.Modified;
                        response = db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                Library.WriteLog("At UpdatingUserDetails UserId - ", ex);
            }
            if (response == 1)
                return true;
            else
            {
                return false;
            }
        }

        public bool DeleteSurvey(string custId)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(custId);

                    var objAddTemp = classigooEntities.Surveys.ToList().FindAll(x => x.Id == id);
                    if (objAddTemp.Count > 0)
                    {
                        var objAdd = objAddTemp[0];
                        if (objAdd != null)
                        {
                            classigooEntities.Surveys.Remove(objAdd);
                            classigooEntities.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get add delete survey operations", ex);
                return false;
            }

            return true;
        }


        public List<Classigoo.LoginUser> GetEmployees()
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    
                    return classigooEntities.LoginUsers.ToList().FindAll(x=>x.RoleId==2);

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get get employee to update", ex);
            }

            return null;

        }
        public Classigoo.LoginUser GetEmployee(string empId)
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(empId);
                    var objEmp = classigooEntities.LoginUsers.SingleOrDefault(a => a.Id == id);

                    if (objEmp != null)
                    {
                        return objEmp;
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get get employee to update", ex);
            }

            return null;

        }

        public Classigoo.LoginUser GetEmployeeLastId()
        {
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    //int id = Convert.ToInt32(empId);
                    //var objEmp = classigooEntities.LoginUsers.Where(x => x.RoleId == 2).ToList();


                    var orderByDescendingResult = from s in classigooEntities.LoginUsers
                                                  orderby s.Id descending
                                                  select s;

                    List<Classigoo.LoginUser> allresults = orderByDescendingResult.ToList();

                    if (allresults.Count > 0)
                    {
                        return allresults[0];
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get last employee id", ex);
            }

            return null;

        }

        public string UpdateEmployee(Classigoo.LoginUser loginUser)
        {
            string returnString = string.Empty;
            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    var objEmpTemp = classigooEntities.LoginUsers.ToList().FindAll(x => x.Id == loginUser.Id);

                    if (objEmpTemp.Count > 0)
                    {
                        var objEmp = objEmpTemp[0];
                        if (objEmp != null)
                        {
                            objEmp.UserId = loginUser.UserId;
                            objEmp.FirstName = loginUser.FirstName;
                            objEmp.LastName = loginUser.LastName;                           
                            objEmp.Password = loginUser.Password;
                            objEmp.RoleId = 2;

                            int response = classigooEntities.SaveChanges();
                            if (response == 1)
                            {
                              
                            }
                        }
                    }
                    else
                    {
                        returnString =   AddEmployee(loginUser);
                        return returnString;
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at update add post survey operations", ex);
                return "error";
            }

            return "updated";
        }

        public string AddEmployee(Classigoo.LoginUser loginUser)
        {
         
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {

                    var objEmp = db.LoginUsers.SingleOrDefault(a => a.UserId == loginUser.UserId);

                    if(objEmp == null)
                    {
                        loginUser.RoleId = 2;
                        db.LoginUsers.Add(loginUser);
                        db.SaveChanges();
                    }
                    else
                    {
                        return "existed";
                    }
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At emp db", ex);
               
            }

            return "done";
        }

        public bool DeleteEmployee(string empId)
        {

            try
            {
                using (ClassigooEntities classigooEntities = new ClassigooEntities())
                {
                    int id = Convert.ToInt32(empId);

                    var objAddTemp = classigooEntities.LoginUsers.ToList().FindAll(x => x.Id == id);
                    if (objAddTemp.Count > 0)
                    {
                        var objAdd = objAddTemp[0];
                        if (objAdd != null)
                        {
                            classigooEntities.LoginUsers.Remove(objAdd);
                            classigooEntities.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("exception at get add delete survey operations", ex);
                return false;
            }

            return true;
        }
    }
}