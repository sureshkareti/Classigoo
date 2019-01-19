using System;
using System.Collections.Generic;
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
                                   PostedBy=add.PostedBy
                               }).OrderByDescending(add => add.Created).Where(add=>add.PostedBy== empId).ToList()
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
                                         PostedBy=add.PostedBy
                                     });
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At empadds db", ex);
            }
            return addColl;
        }
    }
}