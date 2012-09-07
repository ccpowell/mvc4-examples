using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Role
    {
        public Role()
        {
            this.OrganizationPersonRoles = new List<OrganizationPersonRole>();
        }

        public int RoleID { get; set; }
        public string Role1 { get; set; }
        public int RoleTypeID { get; set; }
        public virtual ICollection<OrganizationPersonRole> OrganizationPersonRoles { get; set; }
        public virtual RoleType RoleType { get; set; }
    }
}
