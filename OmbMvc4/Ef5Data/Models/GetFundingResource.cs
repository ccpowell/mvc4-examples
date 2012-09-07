using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class GetFundingResource
    {
        public string FundingType { get; set; }
        public string Code { get; set; }
        public string SourceAgency { get; set; }
        public string RecipientAgency { get; set; }
        public Nullable<bool> Discretion { get; set; }
        public string TimePeriod { get; set; }
        public string Program { get; set; }
        public string FundingGroup { get; set; }
    }
}
