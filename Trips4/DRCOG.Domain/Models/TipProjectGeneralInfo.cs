using System;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.Models
{
    public class TipProjectGeneralInfo : VersionModel
    {
        public int? SponsorId { get; set; } // (Organization) ProjectVersion via SponsorContactId
        public int? SponsorContactId { get; set; } // ProjectVersion 
        public int? AdministrativeLevelId { get; set; } // Project
        public int? ProjectTypeId { get; set; } // ImprovementType
        public int? ImprovementTypeId { get; set; } // Project
        public int? ProjectPoolId { get; set; } // ProjectVersion - not implemented
        public bool? IsPoolMaster { get; set; } // ProjectVersion
        public int? SelectionAgencyId { get; set; } // not implemented
        public int? TransportationTypeId { get; set; } // Project.TransportationTypeId via Category (via Category Type of 'Transportation Type') via
        public string SponsorNotes { get; set; } // ProjectVersion
        public string DRCOGNotes { get; set; } // ProjectVersion

    }
}
