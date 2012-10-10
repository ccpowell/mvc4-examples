using System;

namespace DRCOG.Domain.Models
{
    public abstract class TipProjectBase
    {
        public int? ProjectId { get; set; } // TipProject
        public string ProjectName { get; set; } // Project
        public string TipId { get; set; } // TipProject
    }

    public abstract class TipProjectVersionBase : TipProjectBase
    {
        public int? ProjectVersionId { get; set; } // TipProjectVersion
        public string TipYear { get; set; } // TimePeriod via TipProjectVersion.TimePeriodId
    }
}
