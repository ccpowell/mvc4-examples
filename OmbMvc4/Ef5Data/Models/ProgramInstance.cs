using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProgramInstance
    {
        public ProgramInstance()
        {
            this.FundingResources = new List<FundingResource>();
            this.ProgramInstanceProjectTypes = new List<ProgramInstanceProjectType>();
            this.ProgramInstanceSponsors = new List<ProgramInstanceSponsor>();
            this.ProjectPools = new List<ProjectPool>();
            this.ImprovementTypes = new List<ImprovementType>();
        }

        public int ProgramID { get; set; }
        public short TimePeriodID { get; set; }
        public Nullable<bool> Current { get; set; }
        public Nullable<bool> Pending { get; set; }
        public Nullable<bool> Previous { get; set; }
        public Nullable<System.DateTime> OpeningDate { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public string Notes { get; set; }
        public Nullable<int> StatusId { get; set; }
        public virtual ICollection<FundingResource> FundingResources { get; set; }
        public virtual Program Program { get; set; }
        public virtual Status Status { get; set; }
        public virtual TimePeriod TimePeriod { get; set; }
        public virtual ICollection<ProgramInstanceProjectType> ProgramInstanceProjectTypes { get; set; }
        public virtual ICollection<ProgramInstanceSponsor> ProgramInstanceSponsors { get; set; }
        public virtual ICollection<ProjectPool> ProjectPools { get; set; }
        public virtual RTPProgramInstance RTPProgramInstance { get; set; }
        public virtual SurveyProgramInstance SurveyProgramInstance { get; set; }
        public virtual TIPProgramInstance TIPProgramInstance { get; set; }
        public virtual ICollection<ImprovementType> ImprovementTypes { get; set; }
    }
}
