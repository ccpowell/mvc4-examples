using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class MetroVisionMeasureSponsor
    {
        public int SpecificMetroVisionMeasureID { get; set; }
        public int MetroVisionMeasureID { get; set; }
        public Nullable<int> ProjectVersionID { get; set; }
        public int SponsorId { get; set; }
        public virtual MetroVisionMeasure MetroVisionMeasure { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
        public virtual SponsorOrganization SponsorOrganization { get; set; }
    }
}
