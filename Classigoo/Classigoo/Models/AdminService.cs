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
                                         ((searchQuery.State != "" ? add.State == searchQuery.State : true) ||
                                          (searchQuery.District != "" ? add.District == searchQuery.District : true) ||
                                          (searchQuery.Mandal != "" ? add.Mandal == searchQuery.Mandal : true)) &&
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

        public bool UpdateCustomerStatus(int cId, string status)
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

    }
}