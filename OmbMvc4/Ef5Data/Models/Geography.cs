using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Geography
    {
        public int GeographyID { get; set; }
        public string Geography1 { get; set; }
        public int GeographyTypeID { get; set; }
        public virtual CountyGeography CountyGeography { get; set; }
        public virtual GeographyType GeographyType { get; set; }
        public virtual MuniGeography MuniGeography { get; set; }
    }
}
