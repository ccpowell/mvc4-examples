using System;
using System.Collections.Generic;

namespace DRCOG.Domain.Models.TIPProject
{
    public class ScopeModel : TipVersionModel
    {
        public string ProjectDescription { get; set; }
        public int? BeginConstructionYear { get; set; }
        public int? OpenToPublicYear { get; set; }

        public IList<SegmentModel> ScopeSegments { get; set; }
        
    }
}
