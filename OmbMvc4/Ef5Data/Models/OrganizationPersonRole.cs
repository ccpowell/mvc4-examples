using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class OrganizationPersonRole
    {
        public int OrganizationID { get; set; }
        public int PersonID { get; set; }
        public int RoleID { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Person Person { get; set; }
        public virtual Role Role { get; set; }
    }
}
