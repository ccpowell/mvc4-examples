using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class validation_GetProject
    {
        public int ProjectID { get; set; }
        public string COGID { get; set; }
        public string ImprovementType { get; set; }
        public string Selector { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<bool> RegionalSignificance { get; set; }
        public string Route { get; set; }
        public string Administrative_Level { get; set; }
        public string Transportation_Type { get; set; }
    }
}
