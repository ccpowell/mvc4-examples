using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class GeographyType
    {
        public GeographyType()
        {
            this.Geographies = new List<Geography>();
            this.MuniGeographies = new List<MuniGeography>();
        }

        public int GeographyTypeID { get; set; }
        public string GeographyType1 { get; set; }
        public virtual ICollection<Geography> Geographies { get; set; }
        public virtual ICollection<MuniGeography> MuniGeographies { get; set; }
    }
}
