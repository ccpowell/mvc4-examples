using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Status
    {
        public Status()
        {
            this.Cycles = new List<Cycle>();
            this.ProgramInstances = new List<ProgramInstance>();
            this.ProjectSegments = new List<ProjectSegment>();
            this.ProjectSegments1 = new List<ProjectSegment>();
            this.RTPProjectVersions = new List<RTPProjectVersion>();
            this.Strikes = new List<Strike>();
            this.SurveyProjectVersions = new List<SurveyProjectVersion>();
            this.SurveyProjectVersions1 = new List<SurveyProjectVersion>();
            this.SurveyProjectVersions2 = new List<SurveyProjectVersion>();
            this.TIPProjectVersions = new List<TIPProjectVersion>();
            this.TIPProjectVersions1 = new List<TIPProjectVersion>();
        }

        public int StatusID { get; set; }
        public Nullable<int> StatusTypeID { get; set; }
        public string Status1 { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Cycle> Cycles { get; set; }
        public virtual ICollection<ProgramInstance> ProgramInstances { get; set; }
        public virtual ICollection<ProjectSegment> ProjectSegments { get; set; }
        public virtual ICollection<ProjectSegment> ProjectSegments1 { get; set; }
        public virtual ICollection<RTPProjectVersion> RTPProjectVersions { get; set; }
        public virtual ICollection<Strike> Strikes { get; set; }
        public virtual ICollection<SurveyProjectVersion> SurveyProjectVersions { get; set; }
        public virtual ICollection<SurveyProjectVersion> SurveyProjectVersions1 { get; set; }
        public virtual ICollection<SurveyProjectVersion> SurveyProjectVersions2 { get; set; }
        public virtual ICollection<TIPProjectVersion> TIPProjectVersions { get; set; }
        public virtual ICollection<TIPProjectVersion> TIPProjectVersions1 { get; set; }
    }
}
