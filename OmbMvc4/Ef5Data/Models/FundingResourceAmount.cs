using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class FundingResourceAmount
    {
        public int FundingResourceAmountID { get; set; }
        public int FundingResourceID { get; set; }
        public int FundingIncrementID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> StateWideAmount { get; set; }
        public virtual FundingIncrement FundingIncrement { get; set; }
    }
}
