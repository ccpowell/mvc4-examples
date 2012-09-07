using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class validation_GetProjectSponsors
    {
        public int ProjectID { get; set; }
        public string COGID { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<bool> Primary { get; set; }
    }
}
