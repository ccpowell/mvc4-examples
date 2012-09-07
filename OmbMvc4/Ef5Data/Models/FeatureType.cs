using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class FeatureType
    {
        public FeatureType()
        {
            this.Features = new List<Feature>();
        }

        public int FeatureTypeID { get; set; }
        public string FeatureType1 { get; set; }
        public string ProjectType { get; set; }
        public string Selectability { get; set; }
        public virtual ICollection<Feature> Features { get; set; }
    }
}
