using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.Survey;

namespace DRCOG.Domain.ViewModels.Survey
{
    public class FundingViewModel : ProjectBaseViewModel
    {
        public IList<Funding> ProjectFundingHistory { get; set; }
        public IDictionary<int, string> PlanTypes { get; set; }
        public IDictionary<int, string> AvailableFundingResources { get; set; }

        //public Funding ProjectFunding { get; set; }

        public IList<FundingSource> FundingSources { get; set; }
    }
}
