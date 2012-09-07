using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class FundingIncrement
    {
        public FundingIncrement()
        {
            this.FundingResourceAmounts = new List<FundingResourceAmount>();
            this.ProjectFinancialRecordDetails = new List<ProjectFinancialRecordDetail>();
            this.ProjectFinancialRecordDetailPhases = new List<ProjectFinancialRecordDetailPhase>();
            this.TimePeriods = new List<TimePeriod>();
        }

        public int FundingIncrementID { get; set; }
        public string FundingIncrement1 { get; set; }
        public byte BaseYearModifier { get; set; }
        public byte EndYearModifier { get; set; }
        public byte ListOrder { get; set; }
        public virtual ICollection<FundingResourceAmount> FundingResourceAmounts { get; set; }
        public virtual ICollection<ProjectFinancialRecordDetail> ProjectFinancialRecordDetails { get; set; }
        public virtual ICollection<ProjectFinancialRecordDetailPhase> ProjectFinancialRecordDetailPhases { get; set; }
        public virtual ICollection<TimePeriod> TimePeriods { get; set; }
    }
}
