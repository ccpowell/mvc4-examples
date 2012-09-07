using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class MuniGeography
    {
        public MuniGeography()
        {
            this.ProjectMuniGeographies = new List<ProjectMuniGeography>();
        }

        public int MuniGeographyID { get; set; }
        public string PlaceCode { get; set; }
        public string FIPS { get; set; }
        public Nullable<int> MuniTypeID { get; set; }
        public virtual Geography Geography { get; set; }
        public virtual GeographyType GeographyType { get; set; }
        public virtual ICollection<ProjectMuniGeography> ProjectMuniGeographies { get; set; }
    }
}
