using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class FundingType
    {
        public FundingType()
        {
            this.FundingResources = new List<FundingResource>();
        }

        public int FundingTypeID { get; set; }
        public string FundingType1 { get; set; }
        public string Code { get; set; }
        public Nullable<int> FundingGroupID { get; set; }
        public Nullable<int> SourceAgencyID { get; set; }
        public Nullable<int> RecipientAgencyID { get; set; }
        public Nullable<bool> Discretion { get; set; }
        public Nullable<bool> ConformityImpact { get; set; }
        public Nullable<short> RankOrder { get; set; }
        public virtual FundingGroup FundingGroup { get; set; }
        public virtual ICollection<FundingResource> FundingResources { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
