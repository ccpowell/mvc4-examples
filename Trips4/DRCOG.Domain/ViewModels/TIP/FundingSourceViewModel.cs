//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/20/2009 12:05:11 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ViewModels.TIP
{
    /// <summary>
    /// ViewModel for /Funding/Detail and /Funding/Create
    /// </summary>
    public class FundingSourceViewModel:TipBaseViewModel
    {
        public string TipYear { get; set; }        
        public FundingSourceModel FundingSource {get;set;}

        //Lookup Tables for the view
        public IDictionary<int, string> AvailableFundingGroups { get; set; }
        public IDictionary<int, string> AvailableFundingLevels { get; set; }
        public IDictionary<int, string> AvailableOrganizations { get; set; }

    }
}
