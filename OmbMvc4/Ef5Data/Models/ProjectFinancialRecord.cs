using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectFinancialRecord
    {
        public ProjectFinancialRecord()
        {
            this.ProjectFinancialRecordDetails = new List<ProjectFinancialRecordDetail>();
        }

        public int ProjectFinancialRecordID { get; set; }
        public Nullable<int> FundPeriodID { get; set; }
        public int ProjectVersionID { get; set; }
        public Nullable<decimal> Previous { get; set; }
        public Nullable<decimal> Future { get; set; }
        public Nullable<decimal> TIPFunding { get; set; }
        public Nullable<decimal> FederalTotal { get; set; }
        public Nullable<decimal> StateTotal { get; set; }
        public Nullable<decimal> LocalTotal { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<int> Temp_PreviousAmendID { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
        public virtual ICollection<ProjectFinancialRecordDetail> ProjectFinancialRecordDetails { get; set; }
    }
}
