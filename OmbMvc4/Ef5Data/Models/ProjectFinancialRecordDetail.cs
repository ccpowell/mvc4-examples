using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectFinancialRecordDetail
    {
        public int ProjectFinancialRecordID { get; set; }
        public int FundingIncrementID { get; set; }
        public int FundingResourceID { get; set; }
        public Nullable<decimal> FederalAmount { get; set; }
        public Nullable<decimal> StateAmount { get; set; }
        public Nullable<decimal> LocalAmount { get; set; }
        public virtual FundingIncrement FundingIncrement { get; set; }
        public virtual ProjectFinancialRecord ProjectFinancialRecord { get; set; }
    }
}
