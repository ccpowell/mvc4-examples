using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectType
    {
        public ProjectType()
        {
            this.ImprovementTypes = new List<ImprovementType>();
            this.ProgramInstanceProjectTypes = new List<ProgramInstanceProjectType>();
            this.ProgramInstanceProjectTypes1 = new List<ProgramInstanceProjectType>();
            this.ProjectPools = new List<ProjectPool>();
            this.ProjectType11 = new List<ProjectType>();
            this.ProjectType12 = new List<ProjectType>();
        }

        public int ProjectTypeID { get; set; }
        public Nullable<int> ParentTypeID { get; set; }
        public Nullable<int> PolicyGroupID { get; set; }
        public string ProjectType1 { get; set; }
        public virtual ICollection<ImprovementType> ImprovementTypes { get; set; }
        public virtual ICollection<ProgramInstanceProjectType> ProgramInstanceProjectTypes { get; set; }
        public virtual ICollection<ProgramInstanceProjectType> ProgramInstanceProjectTypes1 { get; set; }
        public virtual ICollection<ProjectPool> ProjectPools { get; set; }
        public virtual ICollection<ProjectType> ProjectType11 { get; set; }
        public virtual ProjectType ProjectType2 { get; set; }
        public virtual ICollection<ProjectType> ProjectType12 { get; set; }
        public virtual ProjectType ProjectType3 { get; set; }
    }
}
