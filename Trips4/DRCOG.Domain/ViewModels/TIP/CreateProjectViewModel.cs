//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/25/2009 10:15:40 AM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;

namespace DRCOG.Domain.ViewModels.TIP
{
    /// <summary>
    /// Contains the drop down lists for the General Info
    /// components of the Create Project View
    /// </summary>
    public class CreateProjectViewModel:TipBaseViewModel
    {
        public string TipYear { get; set; }
        // form specific props
        //public IDictionary<int, string> AvailableSponsors { get; set; }
        public IDictionary<int, string> AvailableRoadOrTransitTypes { get; set; }
        public IDictionary<int, string> AvailableSponsorContacts { get; set; }
        public IDictionary<int, string> AvailableAdminLevels { get; set; }
        public IDictionary<int, string> AvailableImprovementTypes { get; set; }
        public IDictionary<int, string> AvailableProjectTypes { get; set; }
        public IDictionary<int, string> AvailablePools { get; set; }
        public IDictionary<int, string> AvailablePoolMasters { get; set; }
        public IDictionary<int, string> AvailableSelectionAgencies { get; set; }

    }
}
