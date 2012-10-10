#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			        REMARKS
 * 01/25/2010	Danny Davidson	1. Built working view model. Added default/current search parameters.
 * 
 * DESCRIPTION:
 * This VM is used in the Projects Search tab of any of the projects. Holds a ProjectSearchModel (current settings)
 * and all objects required to fill in the Search page drop downs.
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using System.Web.Mvc;

namespace DRCOG.Domain.ViewModels.TIP
{
    public class ProjectSearchViewModel : TipBaseViewModel
    {
        public TIPSearchModel ProjectSearchModel { get; set; }

        // Form specific collections
        
        public IDictionary<int, string> AvailableTipYears { get; set; }
        public IDictionary<int, string> AvailableAmendmentStatuses { get; set; }
        public IDictionary<int, string> AvailableVersionStatuses { get; set; }
        public IDictionary<int, string> AvailableImprovementTypes { get; set; }
        public IDictionary<int, string> AvailableProjectTypes { get; set; }
        public IDictionary<int, string> AvailableFundingTypes { get; set; }
        public IEnumerable<string> AvailableGeographies { get; set; }
        public IEnumerable<SelectListItem> CdotRegions { get; set; }
    }
}
