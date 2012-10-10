#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		REMARKS
 * 06/29/2010	DTucker     1. Built working view model. Added default/current search parameters.
 * 
 * DESCRIPTION:
 * This VM is used in the Projects Search tab of any of the projects. Holds a ProjectSearchModel (current settings)
 * and all objects required to fill in the Search page drop downs.
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP
{
    public class ProjectSearchViewModel : RtpBaseViewModel
    {
        public RTPSearchModel ProjectSearchModel { get; set; }

        // Form specific collections
        
        public IDictionary<int, string> AvailablePlanYears { get; set; }
        public IDictionary<int, string> AvailableAmendmentStatuses { get; set; }
        public IDictionary<int, string> AvailableImprovementTypes { get; set; }
        public IDictionary<int, string> AvailableProjectTypes { get; set; }
        public IDictionary<int, string> AvailableNetworks { get; set; }
        public IDictionary<int, string> AvailablePlanTypes { get; set; }
    }
}
