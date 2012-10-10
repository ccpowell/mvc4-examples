//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/25/2009 4:05:35 PM
// Description:
//
//======================================================
using System;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// Summary of the project version information for
    /// display in the "Project Info" box on the 
    /// Project pages. This is a READ-ONLY in that 
    /// there are no Repository methods that 
    /// will do an update with a TipProjectVersionSummaryModel
    /// </summary>
    [Obsolete]
    public class TipProjectVersionSummaryModel
    {
        public int? ProjectVersionId { get; set; }
        public string ProjectName { get; set; }
        public string TipId { get; set; }

        public string TipYear { get; set; }
        public string COGID { get; set; }
        public string SponsorAgency { get; set; }

        /// <summary>
        /// Is this version editable
        /// Rules: ProjectVersion is in the CurrentTIP and it is the CurrentScenario
        /// </summary>
        public bool IsEditable { get; set; }

        public string ProjectType { get; set; }
        public string NextVersionTipYear { get; set; }
        public int? NextVersionId { get; set; }
        public string PreviousVersionTipYear { get; set; }
        public int? PreviousVersionId { get; set; }
    }
}
