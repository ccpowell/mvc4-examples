using System;
using DRCOG.Domain.Interfaces;
namespace DRCOG.Domain.Models.Survey
{
    public class LocationModel : InstanceSecurity
    {
        public string FacilityName { get; set; }
        public string Limits { get; set; }
        public int RouteId { get; set; }
    }
}
