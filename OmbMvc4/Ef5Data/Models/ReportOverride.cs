using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ReportOverride
    {
        public string TipId { get; set; }
        public int FundingTypeId { get; set; }
        public string ReportFY { get; set; }
        public Nullable<double> NetObligation { get; set; }
        public string FederalTotal { get; set; }
        public Nullable<bool> ShowNetObligation { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}
