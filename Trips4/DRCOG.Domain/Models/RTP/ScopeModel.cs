using System;
using System.Collections.Generic;

namespace DRCOG.Domain.Models.RTP
{
    public class ScopeModel : RtpVersionModel
    {
        public string ProjectDescription { get; set; }
        public string ShortDescription { get; set; }
        
        public int? BeginConstructionYear { get; set; }
        public int? OpenToPublicYear { get; set; }


        public IList<SegmentModel> ScopeSegments { get; set; }
    }

    
}
