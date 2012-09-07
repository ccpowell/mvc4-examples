using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectModelCoding
    {
        public int ProjectModelCodingID { get; set; }
        public Nullable<int> ProjectSegmentID { get; set; }
        public Nullable<int> ProjectVersionID { get; set; }
        public string ScenarioNameID { get; set; }
        public Nullable<int> CodingStatusID { get; set; }
        public string Notes { get; set; }
        public string Temp_OldEndConstr { get; set; }
        public string Temp_RegionRank { get; set; }
        public string Temp_TIPSelectNum { get; set; }
        public string Temp_UniqueID { get; set; }
        public virtual ProjectSegment ProjectSegment { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
    }
}
