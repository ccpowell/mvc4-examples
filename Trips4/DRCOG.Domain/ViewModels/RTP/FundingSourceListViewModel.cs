//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/3/2009 3:21:19 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP
{
    public class FundingSourceListViewModel : RtpBaseViewModel
    {

        public IList<RTPFundingSourceModel> FundingSources { get; set; }
    }
}
