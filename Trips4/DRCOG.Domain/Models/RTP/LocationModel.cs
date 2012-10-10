using System;
using DRCOG.Domain.Interfaces;
namespace DRCOG.Domain.Models.RTP
{
    public class LocationModel : RtpVersionModel
    {
        public string FacilityName { get; set; }
        public string Limits { get; set; }
        public int RouteId { get; set; }
    }
}
