using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectCountyGeography
    {
        public int CountyGeographyID { get; set; }
        public int ProjectID { get; set; }
        public Nullable<float> Share { get; set; }
        public Nullable<bool> Primary { get; set; }
        public virtual CountyGeography CountyGeography { get; set; }
    }
}
