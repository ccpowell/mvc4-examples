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
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP
{
    /// <summary>
    /// ViewModel for /Funding/Detail and /Funding/Create
    /// </summary>
    public class FundingSourceViewModel : RtpBaseViewModel
    {
        //public string TipYear { get; set; }        
        public RTPFundingSourceModel FundingSource { get; set; }

        //Lookup Tables for the view
        public IDictionary<int, string> AvailableFundingGroups { get; set; }
        public IDictionary<int, string> AvailableFundingLevels { get; set; }
        public IDictionary<int, string> AvailableOrganizations { get; set; }

    }
}
