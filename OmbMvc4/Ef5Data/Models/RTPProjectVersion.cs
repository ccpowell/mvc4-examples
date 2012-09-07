using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class RTPProjectVersion
    {
        public RTPProjectVersion()
        {
            this.CycleProjectVersions = new List<CycleProjectVersion>();
        }

        public int RTPProjectVersionID { get; set; }
        public Nullable<short> PlanID { get; set; }
        public Nullable<int> StageID { get; set; }
        public Nullable<int> CycleId { get; set; }
        public string ShortDescription { get; set; }
        public Nullable<decimal> ConstantCost { get; set; }
        public Nullable<decimal> VisionCost { get; set; }
        public Nullable<decimal> YOECost { get; set; }
        public Nullable<int> RTPCategoryID { get; set; }
        public string Temp_RTPSubcategory { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> VersionStatusID { get; set; }
        public Nullable<int> PlanTypeID { get; set; }
        public Nullable<decimal> RevisedConstantCost { get; set; }
        public Nullable<int> AmendmentStatusID { get; set; }
        public string AmendmentReason { get; set; }
        public virtual Category Category { get; set; }
        public virtual Category Category1 { get; set; }
        public virtual Category Category2 { get; set; }
        public virtual Cycle Cycle { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
        public virtual ICollection<CycleProjectVersion> CycleProjectVersions { get; set; }
        public virtual Status Status { get; set; }
        public virtual TimePeriod TimePeriod { get; set; }
    }
}
