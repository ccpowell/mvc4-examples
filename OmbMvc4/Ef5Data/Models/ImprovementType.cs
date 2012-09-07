using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ImprovementType
    {
        public ImprovementType()
        {
            this.ProjectSegments = new List<ProjectSegment>();
            this.ProgramInstances = new List<ProgramInstance>();
        }

        public int ImprovementTypeID { get; set; }
        public string ImprovementType1 { get; set; }
        public Nullable<int> ProjectTypeID { get; set; }
        public string Details { get; set; }
        public string ShortName { get; set; }
        public Nullable<int> ModeID { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> Construction { get; set; }
        public Nullable<int> RSPCodeID { get; set; }
        public virtual ProjectType ProjectType { get; set; }
        public virtual ICollection<ProjectSegment> ProjectSegments { get; set; }
        public virtual RTPImprovementType RTPImprovementType { get; set; }
        public virtual ICollection<ProgramInstance> ProgramInstances { get; set; }
    }
}
