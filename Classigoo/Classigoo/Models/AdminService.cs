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
    }
}