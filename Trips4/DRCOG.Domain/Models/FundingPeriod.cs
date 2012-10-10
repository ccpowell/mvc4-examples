//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/20/2009 3:26:25 PM
// Description:
//
//======================================================
using System;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// The period (usually Year 1, Year 2 etc)
    /// to which funding is assigned.
    /// </summary>
    /// <remarks>The table in the database is called FundingIncrement.</remarks>
    public class FundingPeriod
    {
        /// <summary>
        /// The Database ID of the period.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The period (Year 1, Year 2 etc)
        /// </summary>
        public string Name { get; set; }
    }
}
