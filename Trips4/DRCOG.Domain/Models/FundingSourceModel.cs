//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/20/2009 12:03:21 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Domain.Entities;
using System.ComponentModel;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// Model for the Funding Source details view
    /// </summary>
    public class FundingSourceModel
    {
        /// <summary>
        /// Constructor just makes sure the FundingResources list
        /// is initialized.
        /// </summary>
        public FundingSourceModel()
        {
            this.Resources = new List<FundingResource>();
            this.RecipentOrganization = new Organization();
            this.SourceOrganization = new Organization();
            this.FundingGroup = new FundingGroup();
        }

        /// <summary>
        /// Database Id
        /// </summary>
        public int? FundingTypeId { get; set; }

        /// <summary>
        /// Description of the funding type
        /// </summary>
        [DisplayName("Funding Type:")]
        public string FundingType { get; set; }

        /// <summary>
        /// Short code for the funding type
        /// </summary>
        [DisplayName("Code:")]
        public string Code { get; set; }

        /// <summary>
        /// Level that the funding comes from (Local/State/Federal)
        /// </summary>
        public FundingLevel FundingLevel { get; set; }

        /// <summary>
        /// Organization recieveing the funding
        /// </summary>
        public Organization RecipentOrganization { get; set; }

        /// <summary>
        /// Organization supplying the funding
        /// </summary>
        public Organization SourceOrganization { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public FundingGroup FundingGroup { get; set; }
        
        /// <summary>
        /// Does the recieving organization have discretionay
        /// spending authority
        /// </summary>
        public bool IsDiscretionary { get; set; }

        public bool IsConformityImpact { get; set; }

        public bool IsState { get; set; }
        public bool IsFederal { get; set; }
        public bool IsLocal { get; set; }

        public int TimePeriodId { get; set; }

        /// <summary>
        /// TipYear that the funding is associated with
        /// </summary>
        public string TimePeriod { get; set; }

        public int ProgramId { get; set; }


        /// <summary>        
        /// Not in the database, but in previous UI's
        /// </summary>
        [Obsolete("Not in database, was in old UIs")]
        public string Selector { get; set; }

        /// <summary>
        /// Flag indicating if the funding source can be 
        /// deleted. Based on presence or absence of 
        /// projects
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// List of the resources
        /// </summary>
        /// <remarks>Listing of the allocated funds by
        /// year. <see cref="FundingResource"/></remarks>
        public IList<FundingResource> Resources { get; set; }

    }

}
