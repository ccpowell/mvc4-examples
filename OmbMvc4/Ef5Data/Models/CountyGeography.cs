using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class CountyGeography
    {
        public CountyGeography()
        {
            this.ProjectCountyGeographies = new List<ProjectCountyGeography>();
        }

        public int CountyGeographyID { get; set; }
        public string FIPSCode { get; set; }
        public string StateCode { get; set; }
        public virtual Geography Geography { get; set; }
        public virtual ICollection<ProjectCountyGeography> ProjectCountyGeographies { get; set; }
    }
}
