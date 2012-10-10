using System;
namespace DRCOG.Domain.Models
{
    interface ISummary
    {
        //string ActiveVersion { get; set; }
        string VersionStatus { get; set; }
        string AmendmentStatus { get; set; }
        string COGID { get; set; }
        string ImprovementType { get; set; }
        int? NextVersionId { get; set; }
        string NextVersionYear { get; set; }
        int PreviousVersionId { get; set; }
        string PreviousVersionYear { get; set; }
        string ProjectType { get; set; }
        string SponsorAgency { get; set; }
        string Title { get; set; }
    }
}
