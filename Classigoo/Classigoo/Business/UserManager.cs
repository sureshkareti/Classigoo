using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Classigoo.Models;

namespace Classigoo.Business
{
    public class UserManager
    {

        public LoginUser GetLoginUserByLoginIdPassword(string loginId, string password)
        {
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    LoginUser user = db.LoginUsers.Include("Role").ToList().Find(x => (x.UserId == loginId) && (x.Password == password));
                    if (user != null)
                        return user;
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Get Login User", ex);

            }

            return null;
        }

    }
}