using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.Models.TIPProject
{
    public class FundingModel : VersionModel
    {
        public int? ProjectFinancialRecordID {get; set;}
        public int? TimePeriodID { get; set; }
        public string TimePeriod { get; set; }
        public double? Previous {get; set;}
	    public double? Future {get; set;}
	    public double? TIPFunding {get; set;}
	    public double? FederalTotal {get; set;}
	    public double? StateTotal {get; set;}
	    public double? LocalTotal{get; set;}
	    public double? TotalCost {get; set;}

    }
}
