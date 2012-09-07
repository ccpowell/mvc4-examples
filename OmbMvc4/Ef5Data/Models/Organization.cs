using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Organization
    {
        public Organization()
        {
            this.Addresses = new List<Address>();
            this.FundingTypes = new List<FundingType>();
            this.OrganizationPersonRoles = new List<OrganizationPersonRole>();
            this.OrganizationTypes = new List<OrganizationType>();
        }

        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<int> Temp_OldAgencyID { get; set; }
        public string LegalName { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<FundingType> FundingTypes { get; set; }
        public virtual ICollection<OrganizationPersonRole> OrganizationPersonRoles { get; set; }
        public virtual SponsorOrganization SponsorOrganization { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<OrganizationType> OrganizationTypes { get; set; }
    }
}
