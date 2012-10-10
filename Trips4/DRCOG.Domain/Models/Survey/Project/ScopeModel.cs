using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DRCOG.Domain.Models.Survey
{

    public class ScopeModel : InstanceSecurity
    {
        public string ProjectDescription { get; set; }

        [DisplayName("Begin Construction Year:")]
        public int BeginConstructionYear { get; set; }
        [DisplayName("Open to Public Year:")]
        public int OpenToPublicYear { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectVersionId { get; set; }

        public IList<SegmentModel> ScopeSegments { get; set; }
    }

    
}
