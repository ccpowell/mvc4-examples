using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectSegment
    {
        public ProjectSegment()
        {
            this.LRS = new List<LR>();
            this.ProjectModelCodings = new List<ProjectModelCoding>();
        }

        public int ProjectSegmentID { get; set; }
        public int ProjectVersionID { get; set; }
        public Nullable<int> ImprovementTypeID { get; set; }
        public Nullable<int> ModelingFacilityTypeID { get; set; }
        public Nullable<int> PlanFacilityTypeID { get; set; }
        public Nullable<int> NetworkID { get; set; }
        public Nullable<short> OpenYear { get; set; }
        public string FacilityName { get; set; }
        public string StartAt { get; set; }
        public string EndAt { get; set; }
        public Nullable<float> Length { get; set; }
        public Nullable<short> LanesBase { get; set; }
        public Nullable<short> LanesFuture { get; set; }
        public Nullable<short> SpacesFuture { get; set; }
        public Nullable<short> VehiclesFuture { get; set; }
        public Nullable<int> LRSObjectID { get; set; }
        public Nullable<int> AssignmentStatusID { get; set; }
        public string LRSLinkage { get; set; }
        public Nullable<int> LRSLinkageStatusID { get; set; }
        public Nullable<bool> NeedLocationMap { get; set; }
        public Nullable<int> Temp_PreviousImproveID { get; set; }
        public Nullable<bool> ModelingCheck { get; set; }
        public Nullable<float> Cost { get; set; }
        public Nullable<short> SpacesBase { get; set; }
        public virtual GISCategory GISCategory { get; set; }
        public virtual GISCategory GISCategory1 { get; set; }
        public virtual ImprovementType ImprovementType { get; set; }
        public virtual ICollection<LR> LRS { get; set; }
        public virtual Network Network { get; set; }
        public virtual ICollection<ProjectModelCoding> ProjectModelCodings { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
        public virtual Status Status { get; set; }
        public virtual Status Status1 { get; set; }
    }
}
