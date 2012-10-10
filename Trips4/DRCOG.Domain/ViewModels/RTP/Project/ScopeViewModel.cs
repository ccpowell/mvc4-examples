using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP.Project
{
    public class ScopeViewModel : ProjectBaseViewModel
    {
        public IDictionary<int, string> AvailableImprovementTypes { get; set; }
        public IDictionary<int, string> AvailableNetworks { get; set; }
        public IDictionary<int, string> AvailableFacilityTypes { get; set; }

        public ScopeModel RtpProjectScope { get; set; }
        public IList<SegmentModel> Segments { get; set; }
        public IList<PoolProject> PoolProjects { get; set; }
    }
}
