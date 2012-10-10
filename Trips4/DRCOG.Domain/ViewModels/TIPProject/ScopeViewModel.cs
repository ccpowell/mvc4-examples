using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.ViewModels.TIPProject
{
    public class ScopeViewModel : ProjectBaseViewModel
    {
        public ScopeModel TipProjectScope { get; set; }
        public IList<SegmentModel> Segments { get; set; }
        public IList<PoolProject> PoolProjects { get; set; }
    }
}
