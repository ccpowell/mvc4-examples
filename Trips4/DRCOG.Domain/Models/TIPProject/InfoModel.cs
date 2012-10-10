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
using DRCOG.Common.Services;
using DRCOG.Common.DesignByContract;

namespace DRCOG.Domain.Models.TIPProject
{
    /// <summary>
    /// Class that contains the General Information about a TIP Project Version
    /// </summary>
    public class InfoModel : VersionModel
    {
        public InfoModel()
        {
            Image = new Image();
        }

        private String _STIPID;

        //public int? ProjectId { get; set; } // TipProject
        //public int? ProjectVersionId { get; set; } // TipProjectVersion
        //public string ProjectName { get; set; } // ProjectVersion and Project
        public string TipId { get; set; } // TipProject
        public string TipYear { get; set; } // TimePeriod via TipProjectVersion.TimePeriodId

        public int? SponsorId { get; set; } // (Organization) ProjectVersion via SponsorContactId
        public int? SponsorContactId { get; set; } // ProjectVersion 
        public int? AdministrativeLevelId { get; set; } // Project
        public int? ProjectTypeId { get; set; } // ImprovementType
        public int? ImprovementTypeId { get; set; } // Project
        public int? ProjectPoolId { get; set; } // ProjectVersion - not implemented
        public bool? IsPoolMaster { get; set; } // ProjectVersion  // No longer used? -DBD 02/03/2010
        public int? PoolMasterVersionID { get; set; } 
        public int? SelectionAgencyId { get; set; } // Project.SelectorID
        public int? TransportationTypeId { get; set; } // Project.TransportationTypeId via Category (via Category Type of 'Transportation Type') via
        public string SponsorNotes { get; set; } // ProjectVersion
        public string DRCOGNotes { get; set; } // ProjectVersion
        public bool? IsRegionallySignificant { get; set; } // Project
        public int LocationMapId { get; set; }
        public Image Image { get; set; }

        public String STIPID
        {
            get { return _STIPID; }
            set
            {
                if(!String.IsNullOrEmpty(value))
                    Check.Assert(value.Length <= 20, "STIPID must be less than or equal to 20 characters");
                _STIPID = value;
            }
        }
    }
}
