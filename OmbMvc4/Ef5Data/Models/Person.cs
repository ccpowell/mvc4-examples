using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Person
    {
        public Person()
        {
            this.Addresses = new List<Address>();
            this.OrganizationPersonRoles = new List<OrganizationPersonRole>();
            this.Projects = new List<Project>();
            this.SponsorOrganizations = new List<SponsorOrganization>();
        }

        public int PersonID { get; set; }
        public Nullable<System.Guid> PersonGUID { get; set; }
        public string Firstname { get; set; }
        public string MiddleInitial { get; set; }
        public string Lastname { get; set; }
        public string Division { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Temp_PreviousContactID { get; set; }
        public string SponsorKey { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<OrganizationPersonRole> OrganizationPersonRoles { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<SponsorOrganization> SponsorOrganizations { get; set; }
    }
}
