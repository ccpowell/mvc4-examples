#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			        REMARKS
 * 01/26/2010	Danny Davidson     1. Initial Creation. 
 * 03/10/2010   DTucker            2. Added ProjectId
 * 
 * DESCRIPTION:
 * Holds the current (or default) search values which the View can use to fill in its values.
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Interfaces;

namespace DRCOG.Domain.Models
{
    public abstract class ProjectSearchModel
    {
        //Project Identifiers
        public string COGID { get; set; }
        public bool Exclude_COGID { get; set; }

        public string ProjectName { get; set; }
        public bool Exclude_ProjectName { get; set; }

        public Int32 ProjectId { get; set; }

        public string ProjectType { get; set; }
        public int? ProjectTypeID { get; set; }
        public bool Exclude_ProjectType { get; set; }

        public string FundingType { get; set; }
        public int? FundingTypeId { get; set; }
        public bool Exclude_FundingType { get; set; }

        public string ImprovementType { get; set; }
        public int? ImprovementTypeID { get; set; }
        public bool Exclude_ImprovementType { get; set; }

        //Sponsor / Geography
        public string SponsorAgency { get; set; }
        public int? SponsorAgencyID { get; set; }
        public bool Exclude_SponsorAgency { get; set; }

        public string GeographyName { get; set; }
        public int GeographyId { get; set; }

        public int VersionStatusId { get; set; }
        public string VersionStatus { get; set; }
        //public bool? ActiveVersion { get; set; }
        public bool Exclude_ActiveVersion { get; set; }

        public string AmendmentStatus { get; set; }
        public int? AmendmentStatusID { get; set; }
        public bool Exclude_AmendmentStatus { get; set; }

        public bool Exclude_ID { get; set; }
        public bool Exclude_Year { get; set; }

        public int AmendmentTypeId { get; set; }

        public int SponsorContactId { get; set; }

        public string ScopeTerm { get; set; }
        public string PoolTerm { get; set; }

    }
}

