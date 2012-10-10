using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.Survey;
using DRCOG.Domain.Interfaces;

namespace DRCOG.Domain.ViewModels.Survey
{
    

    public class ScopeViewModel : ProjectBaseViewModel
    {
        public IDictionary<int, string> AvailableImprovementTypes { get; set; }
        public IDictionary<int, string> AvailableNetworks { get; set; }
        public IDictionary<int, string> AvailableFacilityTypes { get; set; }

        public ScopeModel Scope { get; set; }
        public IList<SegmentModel> Segments { get; set; }
    }
}
