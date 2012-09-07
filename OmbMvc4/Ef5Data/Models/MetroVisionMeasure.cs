using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class MetroVisionMeasure
    {
        public MetroVisionMeasure()
        {
            this.MetroVisionMeasureSponsors = new List<MetroVisionMeasureSponsor>();
        }

        public int MetroVisionMeasureID { get; set; }
        public string MetroVisionMeasure1 { get; set; }
        public Nullable<bool> ProjectLevel { get; set; }
        public string TIPYear { get; set; }
        public string Description { get; set; }
        public virtual ICollection<MetroVisionMeasureSponsor> MetroVisionMeasureSponsors { get; set; }
    }
}
