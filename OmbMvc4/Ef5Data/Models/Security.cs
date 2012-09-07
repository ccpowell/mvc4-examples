using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Security
    {
        public int PersonID { get; set; }
        public int RandomNumber { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
    }
}
