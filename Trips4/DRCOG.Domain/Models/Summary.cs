#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		REMARKS
 * 07/13/2009	DBouwman    1. Initial Creation (DTS).
 * 02/09/2010	DDavidson	2. Reformatted.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// Light-weight summary object for displaying lists of projects
    /// Summary of the project version information for
    /// display in the "Project Info" box on the 
    /// Project pages. This is a READ-ONLY in that 
    /// there are no Repository methods that 
    /// will do an update with a ProjectSummary
    /// </summary>
    public abstract class Summary : ISummary
    {
        //public string RTPId { get; set; }
        public string COGID { get; set; }
        public string Title { get; set; }
        public string SponsorAgency { get; set; }
        public string ProjectType { get; set; }
        public string AmendmentStatus { get; set; }
        public string ImprovementType { get; set; }
        public string VersionStatus { get; set; }

        /// <summary>
        /// Is this version editable
        /// Rules: ProjectVersion is in the CurrentTIP and it is the CurrentScenario
        /// </summary>
        //public bool IsEditable { get; set; }

        public string NextVersionYear { get; set; }
        public int? NextVersionId { get; set; }
        public string PreviousVersionYear { get; set; }
        public int PreviousVersionId { get; set; }
        //public short RTPYearTimePeriodID { get; set; }

    }
}
