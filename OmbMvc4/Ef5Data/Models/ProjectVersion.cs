using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectVersion
    {
        public ProjectVersion()
        {
            this.MetroVisionMeasureSponsors = new List<MetroVisionMeasureSponsor>();
            this.PoolProjects = new List<PoolProject>();
            this.ProjectFinancialRecords = new List<ProjectFinancialRecord>();
            this.ProjectModelCodings = new List<ProjectModelCoding>();
            this.ProjectSegments = new List<ProjectSegment>();
            this.ProjectVersion1 = new List<ProjectVersion>();
            this.ProjectVersion11 = new List<ProjectVersion>();
            this.ReportProjectVersionSortings = new List<ReportProjectVersionSorting>();
        }

        public int ProjectVersionID { get; set; }
        public int ProjectID { get; set; }
        public Nullable<int> PoolID { get; set; }
        public Nullable<int> PoolMasterVersionID { get; set; }
        public string Limits { get; set; }
        public string BeginAt { get; set; }
        public string EndAt { get; set; }
        public string FacilityName { get; set; }
        public Nullable<int> AmendmentTypeID { get; set; }
        public Nullable<int> AmendmentReasonID { get; set; }
        public Nullable<int> Temp_PreviousScopeID { get; set; }
        public Nullable<int> PreviousProjectVersionID { get; set; }
        public Nullable<int> AmendmentStatusID { get; set; }
        public Nullable<System.DateTime> AmendmentDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> AmendmentFundingTypeID { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string ProjectName { get; set; }
        public string Scope { get; set; }
        public Nullable<int> SponsorContactID { get; set; }
        public string SponsorNotes { get; set; }
        public string DRCOGNotes { get; set; }
        public Nullable<int> LocationMapID { get; set; }
        public Nullable<int> CrossSectionMapID___deprecating { get; set; }
        public string AmendmentCharacter { get; set; }
        public string AmendmentReason { get; set; }
        public Nullable<int> ModelingStatusID { get; set; }
        public Nullable<int> ShapeFileID { get; set; }
        public Nullable<short> BeginConstructionYear { get; set; }
        public string ProjectType { get; set; }
        public Nullable<int> CDOTRegionId { get; set; }
        public string STIPId { get; set; }
        public Nullable<int> AffectedProjectDelaysLocationId { get; set; }
        public virtual Category Category { get; set; }
        public virtual Category Category1 { get; set; }
        public virtual ICollection<MetroVisionMeasureSponsor> MetroVisionMeasureSponsors { get; set; }
        public virtual ICollection<PoolProject> PoolProjects { get; set; }
        public virtual ProjectCDOTData ProjectCDOTData { get; set; }
        public virtual ICollection<ProjectFinancialRecord> ProjectFinancialRecords { get; set; }
        public virtual ICollection<ProjectModelCoding> ProjectModelCodings { get; set; }
        public virtual ProjectPool ProjectPool { get; set; }
        public virtual ICollection<ProjectSegment> ProjectSegments { get; set; }
        public virtual ICollection<ProjectVersion> ProjectVersion1 { get; set; }
        public virtual ProjectVersion ProjectVersion2 { get; set; }
        public virtual ICollection<ProjectVersion> ProjectVersion11 { get; set; }
        public virtual ProjectVersion ProjectVersion3 { get; set; }
        public virtual ICollection<ReportProjectVersionSorting> ReportProjectVersionSortings { get; set; }
        public virtual RTPProjectVersion RTPProjectVersion { get; set; }
        public virtual SurveyProjectVersion SurveyProjectVersion { get; set; }
        public virtual TIPProjectVersion TIPProjectVersion { get; set; }
    }
}
