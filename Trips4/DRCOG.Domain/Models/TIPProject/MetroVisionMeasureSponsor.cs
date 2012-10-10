using System;

namespace DRCOG.Domain.Models.TIPProject
{
    public class MetroVisionMeasureSponsor : VersionModel
    {
        public Int32 SpecificMetroVisionMeasureId { get; set; }
        public Int32 MetroVisionMeasureId { get; set; }
        public Int32 SponsorId { get; set; }
    }
}
