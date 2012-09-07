using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class OrganizationType
    {
        public OrganizationType()
        {
            this.Organizations = new List<Organization>();
        }

        public int OrganizationTypeID { get; set; }
        public string OrganizationType1 { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
