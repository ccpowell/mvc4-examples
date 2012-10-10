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
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ViewModels.TIP
{
    public class FundingSourceListViewModel : TipBaseViewModel
    {
        public FundingSourceModel FundingSource { get; set; }
        public IList<FundingSourceModel> FundingSources { get; set; }
        public IDictionary<int,string> SourceAgencies { get; set; }
        public IDictionary<int, string> RecipientAgencies { get; set; }
        public IDictionary<int, string> FundingGroups { get; set; }


    }
}
