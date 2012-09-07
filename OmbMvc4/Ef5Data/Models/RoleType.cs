using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class RoleType
    {
        public RoleType()
        {
            this.Roles = new List<Role>();
        }

        public int RoleTypeID { get; set; }
        public string RoleType1 { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
