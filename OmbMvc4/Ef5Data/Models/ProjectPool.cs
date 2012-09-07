using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectPool
    {
        public ProjectPool()
        {
            this.ProjectVersions = new List<ProjectVersion>();
        }

        public int ProjectPoolID { get; set; }
        public Nullable<int> ProjectTypeID { get; set; }
        public int ProgramID { get; set; }
        public short TimePeriodID { get; set; }
        public string Description { get; set; }
        public Nullable<int> FundingResourceID { get; set; }
        public string PoolName { get; set; }
        public Nullable<bool> BasicListVersion { get; set; }
        public Nullable<int> ReportInsetTableTypeID { get; set; }
        public virtual FundingResource FundingResource { get; set; }
        public virtual ProgramInstance ProgramInstance { get; set; }
        public virtual ProjectType ProjectType { get; set; }
        public virtual ICollection<ProjectVersion> ProjectVersions { get; set; }
    }
}
