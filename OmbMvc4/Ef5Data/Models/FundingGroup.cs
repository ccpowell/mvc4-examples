using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class FundingGroup
    {
        public FundingGroup()
        {
            this.FundingTypes = new List<FundingType>();
        }

        public int FundingGroupID { get; set; }
        public string FundingGroup1 { get; set; }
        public virtual ICollection<FundingType> FundingTypes { get; set; }
    }
}
