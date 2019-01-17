using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classigoo.Models.LoginUser
{
    public class LoginUser 
    {
        public string LoginId { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string EmailId { get; set; }

        public UserRole Role { get; set; }

        public List<string> Groups { get; set; }

        public DateTime LastLogin { get; set; }

        public bool IsActive { get; set; }

        public string Remarks { get; set; }

        public string ProfilePicture { get; set; }
    }

    public enum UserRole
    {
        Admin,
        Employee  
    }
}