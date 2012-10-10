//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/20/2009 3:24:12 PM
// Description:
//
//======================================================
using System;

namespace DRCOG.Domain.Models
{

    /// <summary>
    /// Details of the funding for a particular period
    /// </summary>
    public class FundingResource
    {

        /// <summary>
        /// ID of the record in the database
        /// </summary>
        public int? FundingResourceId { get; set; }

        public string FundingType { get; set; }

        /// <summary>
        /// Amount of money
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// Amount to be spent statewide
        /// </summary>
        public double? StateWideAmount { get; set; }

        /// <summary>
        /// The period (usually Year 1, Year 2 etc)
        /// that the funds are allocated for
        /// </summary>
        public FundingPeriod Period { get; set; }

    }
}
