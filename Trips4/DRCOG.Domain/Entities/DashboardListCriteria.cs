//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/23/2009 11:09:30 AM
// Description:
//
//======================================================
using System;

namespace DRCOG.Entities
{
    /// <summary>
    /// </summary>
    public class DashboardListCriteria
    {

        /// <summary>
        /// Page of the resultset
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Type of the list to return
        /// </summary>
        /// <remarks>Sponsor, Project Type, Amendment Status</remarks>
        public string ListType { get; set; }

    }
}
