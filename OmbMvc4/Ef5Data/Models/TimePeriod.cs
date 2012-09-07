using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class TimePeriod
    {
        public TimePeriod()
        {
            this.ProgramInstances = new List<ProgramInstance>();
            this.Report1 = new List<Report1>();
            this.RTPProgramInstances = new List<RTPProgramInstance>();
            this.RTPProgramInstances1 = new List<RTPProgramInstance>();
            this.RTPProgramInstances2 = new List<RTPProgramInstance>();
            this.RTPProjectVersions = new List<RTPProjectVersion>();
            this.RTPReportGroupingCategories = new List<RTPReportGroupingCategory>();
            this.SurveyProjectVersions = new List<SurveyProjectVersion>();
            this.TimePeriodCycles = new List<TimePeriodCycle>();
            this.TIPProjectVersions = new List<TIPProjectVersion>();
            this.FundingIncrements = new List<FundingIncrement>();
        }

        public short TimePeriodID { get; set; }
        public string TimePeriod1 { get; set; }
        public int TimePeriodTypeID { get; set; }
        public string Comments { get; set; }
        public Nullable<int> ListOrder { get; set; }
        public virtual ICollection<ProgramInstance> ProgramInstances { get; set; }
        public virtual ICollection<Report1> Report1 { get; set; }
        public virtual ICollection<RTPProgramInstance> RTPProgramInstances { get; set; }
        public virtual ICollection<RTPProgramInstance> RTPProgramInstances1 { get; set; }
        public virtual ICollection<RTPProgramInstance> RTPProgramInstances2 { get; set; }
        public virtual ICollection<RTPProjectVersion> RTPProjectVersions { get; set; }
        public virtual ICollection<RTPReportGroupingCategory> RTPReportGroupingCategories { get; set; }
        public virtual ICollection<SurveyProjectVersion> SurveyProjectVersions { get; set; }
        public virtual TimePeriodType TimePeriodType { get; set; }
        public virtual ICollection<TimePeriodCycle> TimePeriodCycles { get; set; }
        public virtual ICollection<TIPProjectVersion> TIPProjectVersions { get; set; }
        public virtual ICollection<FundingIncrement> FundingIncrements { get; set; }
    }
}
