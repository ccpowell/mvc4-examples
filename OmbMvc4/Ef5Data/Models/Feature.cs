using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Feature
    {
        public Feature()
        {
            this.TIPProjectEvaluation2010 = new List<TIPProjectEvaluation2010>();
        }

        public int FeatureID { get; set; }
        public string Feature1 { get; set; }
        public Nullable<int> FeatureTypeID { get; set; }
        public Nullable<int> PedPoints { get; set; }
        public Nullable<int> BikePoints { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public Nullable<int> Points { get; set; }
        public string Selectability { get; set; }
        public virtual FeatureType FeatureType { get; set; }
        public virtual ICollection<TIPProjectEvaluation2010> TIPProjectEvaluation2010 { get; set; }
    }
}
