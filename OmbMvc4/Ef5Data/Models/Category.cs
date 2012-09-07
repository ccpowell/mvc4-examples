using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Projects = new List<Project>();
            this.Projects1 = new List<Project>();
            this.Projects2 = new List<Project>();
            this.ProjectFinancialRecordDetailPhases = new List<ProjectFinancialRecordDetailPhase>();
            this.ProjectVersions = new List<ProjectVersion>();
            this.ProjectVersions1 = new List<ProjectVersion>();
            this.RTPProjectVersions = new List<RTPProjectVersion>();
            this.RTPProjectVersions1 = new List<RTPProjectVersion>();
            this.RTPProjectVersions2 = new List<RTPProjectVersion>();
            this.SponsorOrganizations = new List<SponsorOrganization>();
            this.Strikes = new List<Strike>();
            this.Organizations = new List<Organization>();
        }

        public int CategoryID { get; set; }
        public string Category1 { get; set; }
        public Nullable<int> CategoryTypeID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public virtual CategoryType CategoryType { get; set; }
        public virtual CDOTInvestmentTypeCategory CDOTInvestmentTypeCategory { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Project> Projects1 { get; set; }
        public virtual ICollection<Project> Projects2 { get; set; }
        public virtual ICollection<ProjectFinancialRecordDetailPhase> ProjectFinancialRecordDetailPhases { get; set; }
        public virtual ICollection<ProjectVersion> ProjectVersions { get; set; }
        public virtual ICollection<ProjectVersion> ProjectVersions1 { get; set; }
        public virtual ICollection<RTPProjectVersion> RTPProjectVersions { get; set; }
        public virtual ICollection<RTPProjectVersion> RTPProjectVersions1 { get; set; }
        public virtual ICollection<RTPProjectVersion> RTPProjectVersions2 { get; set; }
        public virtual RTPReportGroupingCategory RTPReportGroupingCategory { get; set; }
        public virtual ICollection<SponsorOrganization> SponsorOrganizations { get; set; }
        public virtual ICollection<Strike> Strikes { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
