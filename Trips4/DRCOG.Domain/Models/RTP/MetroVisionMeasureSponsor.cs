using System;

namespace DRCOG.Domain.Models.RTP
{
    public class MetroVisionMeasureSponsor : RtpVersionModel
    {
        public Int32 SpecificMetroVisionMeasureId { get; set; }
        public Int32 MetroVisionMeasureId { get; set; }
        public Int32 SponsorId { get; set; }
    }
}
