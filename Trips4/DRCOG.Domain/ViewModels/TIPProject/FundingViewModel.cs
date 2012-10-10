using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.ViewModels.TIPProject
{
    public class FundingViewModel : ProjectBaseViewModel
    {
        public IDictionary<int, string> FundingTypes { get; set; }
        public IDictionary<int, string> FundingLevels { get; set; }
        public IDictionary<int, string> ProjectFundingResources { get; set; }
        public IList<FundingModel> TipProjectFunding { get; set; }
        public IList<FundingModel> ProjectFundingHistory { get; set; }
        //public IList<FundingDetailModel> TipProjectFundingDetail { get; set; }
        public IList<FundingIncrement> FundingIncrements { get; set; }
        public FundingDetailPivotModel FundingDetailPivotModel { get; set; }
        public IList<FundingPhase> FundingPhases { get; set; }
        public IDictionary<int, string> FundingYearsAvailable { get; set; }
        public IDictionary<int, string> FundingPhasesAvailable { get; set; }
    }
}
