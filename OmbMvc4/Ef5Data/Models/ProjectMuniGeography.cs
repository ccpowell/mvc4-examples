using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectMuniGeography
    {
        public int MuniGeographyID { get; set; }
        public int ProjectID { get; set; }
        public Nullable<float> Share { get; set; }
        public Nullable<bool> Primary { get; set; }
        public virtual MuniGeography MuniGeography { get; set; }
        public virtual Project Project { get; set; }
    }
}
