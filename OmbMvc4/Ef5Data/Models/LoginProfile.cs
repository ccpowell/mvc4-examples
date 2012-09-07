using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class LoginProfile
    {
        public int LoginProfileID { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> ChangedPasswordDate { get; set; }
    }
}
