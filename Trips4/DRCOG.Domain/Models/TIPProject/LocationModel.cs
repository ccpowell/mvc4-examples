using System;
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Common.Services;
namespace DRCOG.Domain.Models.TIPProject
{
    public class LocationModel : TipVersionModel
    {
        public string FacilityName { get; set; }
        public string Limits { get; set; }

        public int LocationMapId { get; set; }
        public Image Image { get; set; }
        public int CdotRegionId { get; set; }
        public int AffectedProjectDelaysLocationId { get; set; }
    }
}
