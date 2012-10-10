using System;

namespace DRCOG.Domain.Models.TIPProject
{
    public abstract class VersionModel : ProjectModel
    {
        public int? ProjectVersionId { get; set; } // TipProjectVersion
        public string TipYear { get; set; } // TimePeriod via TipProjectVersion.TimePeriodId
    }
}
