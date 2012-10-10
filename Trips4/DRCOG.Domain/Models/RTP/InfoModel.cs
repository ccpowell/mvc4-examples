#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		REMARKS
 * 08/06/2009	Unknown     1. Initial Creation (DTS).
 * 02/03/2010	DDavidson	2. Reformatted. Several improvements.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;

namespace DRCOG.Domain.Models.RTP
{
    /// <summary>
    /// Class that contains the General Information about a RTP Project Version
    /// </summary>
    public class InfoModel : RtpVersionModel
    {
        //public int? ProjectId { get; set; } // TipProject
        //public int? ProjectVersionId { get; set; } // TipProjectVersion
        //public string ProjectName { get; set; } // ProjectVersion and Project
        

        public int? SponsorId { get; set; } // (Organization) ProjectVersion via SponsorContactId
        public int? SponsorContactId { get; set; } // ProjectVersion 
        public int? AdministrativeLevelId { get; set; } // Project
        public int? ProjectTypeId { get; set; } // ImprovementType
        public string ProjectType { get; set; }
        public int? ImprovementTypeId { get; set; } // Project
        public int? ProjectPoolId { get; set; } // ProjectVersion - not implemented
        public bool? IsPoolMaster { get; set; } // ProjectVersion  // No longer used? -DBD 02/03/2010
        public int? PoolMasterVersionID { get; set; } 
        public int? SelectionAgencyId { get; set; } // Project.SelectorID
        public int? TransportationTypeId { get; set; } // Project.TransportationTypeId via Category (via Category Type of 'Transportation Type') via
        public string SponsorNotes { get; set; } // ProjectVersion
        public string DRCOGNotes { get; set; } // ProjectVersion
        public bool? IsRegionallySignificant { get; set; } // Project
    }
}
