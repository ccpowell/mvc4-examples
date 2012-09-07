using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectSponsor
    {
        public int ProjectID { get; set; }
        public int SponsorID { get; set; }
        public Nullable<bool> Primary { get; set; }
        public virtual Project Project { get; set; }
        public virtual SponsorOrganization SponsorOrganization { get; set; }
    }
}
