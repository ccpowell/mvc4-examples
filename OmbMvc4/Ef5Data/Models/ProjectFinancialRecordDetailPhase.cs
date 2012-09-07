using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectFinancialRecordDetailPhase
    {
        public int ProjectFinancialRecordID { get; set; }
        public int FundingIncrementID { get; set; }
        public int FundingResourceID { get; set; }
        public int PhaseID { get; set; }
        public bool IsInitiated { get; set; }
        public bool IsChecked { get; set; }
        public string MidYearStatus { get; set; }
        public string EndYearStatus { get; set; }
        public string ActionPlan { get; set; }
        public Nullable<System.DateTime> MeetingDate { get; set; }
        public string Notes { get; set; }
        public virtual Category Category { get; set; }
        public virtual FundingIncrement FundingIncrement { get; set; }
        public virtual FundingResource FundingResource { get; set; }
    }
}
