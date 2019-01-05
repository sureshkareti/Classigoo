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
    }
}